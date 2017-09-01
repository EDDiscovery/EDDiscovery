/*
 * Copyright © 2016 - 2017 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using EDDiscovery.DB;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class SavedRouteExpeditionControl : UserControl
    {
        public static bool DeleteIsPermanent = true;

        private List<SavedRouteClass> _savedRoutes;
        private SavedRouteClass _currentRoute;
        private EDDiscoveryForm _discoveryForm;
        private EDSMClass edsm;
        private Rectangle _dragBox;
        private int _dragRowIndex;
        private bool _suppressCombo = false;

        public SavedRouteExpeditionControl()
        {
            InitializeComponent();
            SystemName.AutoCompleteGenerator = SystemClassDB.ReturnOnlySystemsListForAutoComplete;
            _currentRoute = new SavedRouteClass("");
            _savedRoutes = new List<SavedRouteClass>();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            edsm = new EDSMClass();
        }

        public void LoadControl()
        {
            _savedRoutes = SavedRouteClass.GetAllSavedRoutes();

            foreach (var initroute in EDSMClass.Expeditions)
            {
                if (!_savedRoutes.Any(r => r.Name == initroute.Name || r.Name == "\x7F" + initroute.Name))
                {
                    initroute.Add();
                    _savedRoutes.Add(initroute);
                }
            }

            _savedRoutes = _savedRoutes.Where(r => !r.Name.StartsWith("\x7F")).OrderBy(r => r.Name).ToList();

            UpdateComboBox();
        }

        private void UpdateComboBox()
        {
            _suppressCombo = true;
            toolStripComboBoxRouteSelection.Items.Clear();

            foreach (var route in _savedRoutes)
            {
                toolStripComboBoxRouteSelection.Items.Add(route.Name);
            }

            toolStripComboBoxRouteSelection.Text = _currentRoute.Name;
            _suppressCombo = false;
        }

        private void UpdateUndeleteMenu()
        {
            var delrts = SavedRouteClass.GetAllSavedRoutes().Where(r => r.Name.StartsWith("\x7F")).OrderBy(r => r.Name);

            if (ctxMenuItemUndelete.HasDropDownItems)
            {
                for (int i = ctxMenuItemUndelete.DropDownItems.Count - 1; i >= 0; i--)
                    ctxMenuItemUndelete.DropDownItems[i].Dispose();
                ctxMenuItemUndelete.DropDownItems.Clear();
            }

            foreach (var drt in delrts)
            {
                var menuitem = new ToolStripMenuItem(drt.Name.TrimStart("\x7F".ToCharArray()));
                menuitem.Tag = drt;
                menuitem.Click += UndeleteRouteSubMenuItem_Click;
                ctxMenuItemUndelete.DropDownItems.Add(menuitem);
            }
        }

        private void UndeleteRouteSubMenuItem_Click(object sender, EventArgs e)
        {
            var tmi = sender as ToolStripMenuItem;
            var rte = tmi.Tag as SavedRouteClass;

            rte.Name = tmi.Text;    // .Text already had the DEL prefix removed, so let's use that.
            rte.Update();

            _savedRoutes = SavedRouteClass.GetAllSavedRoutes().Where(r => !r.Name.StartsWith("\x7F")).OrderBy(r => r.Name).ToList();
            UpdateComboBox();

            tmi.Dispose();  // Gets removed automatically.
        }

        public void InsertRows(int insertIndex, params string[] sysnames)
        {
            foreach (var row in sysnames)
            {
                dataGridViewRouteSystems.Rows.Insert(insertIndex, row, "", "");
                insertIndex++;
            }
            UpdateSystemRows();
        }

        public void AppendRows(params string[] sysnames)
        {
            foreach (var row in sysnames)
            {
                dataGridViewRouteSystems.Rows.Add(row, "", "");
            }
            UpdateSystemRows();
        }

        private float CalculateRouteMaxDistFromOrigin()
        {
            if (dataGridViewRouteSystems.Rows.Count < 2)
                return 100;

            double maxdist = 25;
            var systems = dataGridViewRouteSystems.Rows.OfType<DataGridViewRow>()
                .Where(r => r.Index < dataGridViewRouteSystems.NewRowIndex && r.Cells[0].Tag != null)
                .Select(r => r.Cells[0].Tag as ISystemBase)
                .Where(s => s.HasCoordinate);
            var sys0 = systems.FirstOrDefault();

            if (sys0 != null)
            {
                foreach (var sys in systems)
                {
                    double dist = SystemClassDB.Distance(sys0, sys);

                    if (dist > maxdist)
                    {
                        maxdist = dist;
                    }
                }
            }

            return (float)maxdist;
        }

        private ISystemBase GetSystem(string sysname)
        {
            ISystemBase sys;
            if (!SystemClassDB.TryGetSystem(sysname, out sys, true) && edsm.IsKnownSystem(sysname))
                sys = new SystemClassDB(sysname);

            return sys;
        }

        // If the route has any unsaved changes, prompt the user to save them.
        // Returns true if the caller is allowed to continue; false if caller should abort.
        private bool PromptAndSaveIfNeeded()
        {
            var rt = new SavedRouteClass();
            UpdateRouteInfo(rt);

            if (rt.Equals(_currentRoute))   // No changes have been made.
                return true;
            else
            {
                var result = ExtendedControls.MessageBoxTheme.Show(ParentForm, "There are unsaved changes to the current route." + Environment.NewLine
                    + "Would you like to save the current route before proceeding?", "Unsaved route", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                switch (result)
                {
                    case DialogResult.Yes:
                        return SaveCurrentRoute();

                    case DialogResult.No:
                        return true;

                    case DialogResult.Cancel:
                    default:
                        return false;
                }
            }
        }

        private bool SaveCurrentRoute()
        {
            bool ret = false;
            string newrtname = textBoxRouteName.Text?.Trim();
            string oldrtname = _currentRoute.Name;
            var newrt = new SavedRouteClass();
            var oldrt = _currentRoute;

            UpdateRouteInfo(newrt);

            if (string.IsNullOrEmpty(newrtname))
            {
                ExtendedControls.MessageBoxTheme.Show(ParentForm, "Please specify a name for the route.", "Unsaved Route", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBoxRouteName.Select();
            }
            else if (EDSMClass.Expeditions.Any(r => r.Name.Equals(newrtname)))
            {
                ExtendedControls.MessageBoxTheme.Show(ParentForm, "The current route name conflicts with a well-known expedition." + Environment.NewLine
                    + "Please specify a new name to save your changes.", "Unsaved Route", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBoxRouteName.Select();
                textBoxRouteName.SelectAll();
            }
            else
            {
                var overwriteroute = _savedRoutes.Where(r => r.Name.Equals(newrtname) && r.Id != _currentRoute.Id).FirstOrDefault();
                if (overwriteroute != null)
                {
                    if (MessageBoxTheme.Show(ParentForm, "Warning: route already exists. Would you like to overwrite it?", "Route Exists", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return false;

                    overwriteroute.Delete();
                    _savedRoutes.Remove(overwriteroute);
                }
                if (_currentRoute.Id < 0)
                {
                    ret = newrt.Add();
                }
                else
                {
                    newrt.Id = _currentRoute.Id;
                    ret = newrt.Update();
                    _savedRoutes.Remove(oldrt);
                }
                _currentRoute = newrt;
                _savedRoutes.Add(newrt);
                _savedRoutes = _savedRoutes.Where(r => !r.Name.StartsWith("\x7F")).OrderBy(r => r.Name).ToList();
                UpdateComboBox();
            }

            return ret;
        }

        private void UpdateSystemRow(int rowindex)
        {
            if (rowindex < dataGridViewRouteSystems.Rows.Count &&
                dataGridViewRouteSystems[0, rowindex].Value != null)
            {
                string sysname = dataGridViewRouteSystems[0, rowindex].Value.ToString();
                var sys = GetSystem(sysname);
                dataGridViewRouteSystems[1, rowindex].Value = "";

                if (rowindex > 0 && rowindex < dataGridViewRouteSystems.Rows.Count &&
                    dataGridViewRouteSystems[0, rowindex - 1].Value != null &&
                    dataGridViewRouteSystems[0, rowindex].Value != null)
                {
                    string prevsysname = dataGridViewRouteSystems[0, rowindex - 1].Value.ToString();
                    var prevsys = GetSystem(prevsysname);

                    if (sys != null && prevsys != null)
                    {
                        double dist = SystemClassDB.Distance(sys, prevsys);
                        string strdist = dist >= 0 ? ((double)dist).ToString("0.00") : "";
                        dataGridViewRouteSystems[1, rowindex].Value = strdist;
                    }
                }

                dataGridViewRouteSystems[0, rowindex].Tag = sys;
                dataGridViewRouteSystems.Rows[rowindex].DefaultCellStyle.ForeColor = (sys != null && sys.HasCoordinate) ? _discoveryForm.theme.VisitedSystemColor : _discoveryForm.theme.NonVisitedSystemColor;

                if (sys != null)
                {
                    string note = "";
                    SystemNoteClass sn = SystemNoteClass.GetNoteOnSystem(sys.name, sys.id_edsm);
                    if (sn != null && !string.IsNullOrWhiteSpace(sn.Note))
                    {
                        note = sn.Note;
                    }
                    else
                    {
                        BookmarkClass bkmark =BookmarkClass.bookmarks.Find(x => x.StarName != null && x.StarName.Equals(sys.name));
                        if (bkmark != null && !string.IsNullOrWhiteSpace(bkmark.Note))
                            note = bkmark.Note;
                        else
                        {
                            var gmo = _discoveryForm.galacticMapping.Find(sys.name);
                            if (gmo != null && !string.IsNullOrWhiteSpace(gmo.description))
                                note = gmo.description;
                        }
                    }
                   dataGridViewRouteSystems[2, rowindex].Value = note.WordWrap(60);
                }

                if (sys == null && sysname != "")
                {
                    dataGridViewRouteSystems.Rows[rowindex].ErrorText = "System not known to EDSM";
                }
                else
                {
                    dataGridViewRouteSystems.Rows[rowindex].ErrorText = "";
                }
            }
        }


        private void UpdateSystemRows()
        {
            for (int i = 0; i < dataGridViewRouteSystems.Rows.Count; i++)
            {
                dataGridViewRouteSystems[1, i].ReadOnly = true;
                dataGridViewRouteSystems[2, i].ReadOnly = true;
                UpdateSystemRow(i);
            }
            UpdateTotalDistances();
        }
        private void UpdateTotalDistances()
        {
            double distance = 0;
            txtCmlDistance.Text = distance.ToString("0.00") + "LY";
            txtP2PDIstance.Text = distance.ToString("0.00") + "LY";
            if (dataGridViewRouteSystems.Rows.Count > 1)
            {
                SystemClassDB firstSC = null;
                SystemClassDB lastSC = null;
                for (int i = 0; i < dataGridViewRouteSystems.Rows.Count; i++)
                {
                    if (firstSC == null && dataGridViewRouteSystems[0, i].Tag != null)
                        firstSC = (SystemClassDB)dataGridViewRouteSystems[0, i].Tag;
                    if (dataGridViewRouteSystems[0, i].Tag != null)
                        lastSC = (SystemClassDB)dataGridViewRouteSystems[0, i].Tag;
                    String value = dataGridViewRouteSystems[1, i].Value as string;
                    if (!String.IsNullOrWhiteSpace(value))
                        distance += Double.Parse(value);
                }
                txtCmlDistance.Text = distance.ToString("0.00") + "LY";
                distance = 0;
                if (firstSC != null && lastSC != null)
                {
                    distance = SystemClassDB.Distance(firstSC, lastSC);
                    txtP2PDIstance.Text = distance.ToString("0.00") + "LY";
                }
            }
        }

        private void UpdateRouteInfo(SavedRouteClass route)
        {
            route.Name = textBoxRouteName.Text.Trim();
            route.Systems.Clear();
            route.Systems.AddRange(dataGridViewRouteSystems.Rows.OfType<DataGridViewRow>()
                .Where(r => r.Index < dataGridViewRouteSystems.NewRowIndex && r.Cells[0].Value != null)
                .Select(r => r.Cells[0].Value.ToString()));

            if (dateTimePickerStartDate.Checked)
            {
                route.StartDate = dateTimePickerStartDate.Value.Date;

                if (dateTimePickerStartTime.Checked)
                {
                    route.StartDate += dateTimePickerStartTime.Value.TimeOfDay;
                }
            }
            else
            {
                route.StartDate = null;
            }

            if (dateTimePickerEndDate.Checked)
            {
                route.EndDate = dateTimePickerEndDate.Value.Date;

                if (dateTimePickerEndTime.Checked)
                {
                    route.EndDate += dateTimePickerEndTime.Value.TimeOfDay;
                }
                else
                {
                    route.EndDate += new TimeSpan(23, 59, 59);
                }
            }
            else
            {
                route.EndDate = null;
            }
        }

        private void toolStripComboBoxRouteSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressCombo)
                return;

            if (!PromptAndSaveIfNeeded())
            {
                _suppressCombo = true;
                toolStripComboBoxRouteSelection.Text = _currentRoute.Name;
                _suppressCombo = false;
                return;
            }

            if (toolStripComboBoxRouteSelection.SelectedIndex == -1)
                return;


            _currentRoute = _savedRoutes[toolStripComboBoxRouteSelection.SelectedIndex];

            dataGridViewRouteSystems.Rows.Clear();

            textBoxRouteName.Text = _currentRoute.Name;
            if (_currentRoute.StartDate == null)
            {
                dateTimePickerStartDate.Value = DateTime.Now;
                dateTimePickerStartDate.Checked = false;
                dateTimePickerStartTime.Value = DateTime.Now;
                dateTimePickerStartTime.Checked = false;
            }
            else
            {
                dateTimePickerStartDate.Checked = true;
                dateTimePickerStartDate.Value = (DateTime)_currentRoute.StartDate;
                dateTimePickerStartTime.Value = (DateTime)_currentRoute.StartDate;

                if (((DateTime)_currentRoute.StartDate).TimeOfDay == new TimeSpan(0, 0, 0))
                {
                    dateTimePickerStartTime.Checked = false;
                }
                else
                {
                    dateTimePickerStartTime.Checked = true;
                }
            }

            if (_currentRoute.EndDate == null)
            {
                dateTimePickerEndDate.Value = DateTime.Now;
                dateTimePickerEndDate.Checked = false;
                dateTimePickerEndTime.Value = DateTime.Now;
                dateTimePickerEndTime.Checked = false;
            }
            else
            {
                dateTimePickerEndDate.Checked = true;
                dateTimePickerEndDate.Value = (DateTime)_currentRoute.EndDate;
                dateTimePickerEndTime.Value = (DateTime)_currentRoute.EndDate;

                if (((DateTime)_currentRoute.EndDate).TimeOfDay == new TimeSpan(23, 59, 59))
                {
                    dateTimePickerEndTime.Checked = false;
                }
                else
                {
                    dateTimePickerEndTime.Checked = true;
                }
            }

            foreach (string sysname in _currentRoute.Systems)
            {
                var rowobj = new object[] { sysname, "", "" };
                dataGridViewRouteSystems.Rows.Add(rowobj);
            }

            UpdateSystemRows();
        }

        private void toolStripComboBoxRouteSelection_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                UpdateUndeleteMenu();
                if (ctxMenuItemUndelete.DropDownItems.Count > 0)
                    ctxMenuCombo.Show(toolStripComboBoxRouteSelection.Control.PointToScreen(e.Location));
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveCurrentRoute();
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            if (!PromptAndSaveIfNeeded())
                return;

            ClearRoute();
            toolStripComboBoxRouteSelection.SelectedItem = null;
        }

        private void ClearRoute()
        {
            toolStripComboBoxRouteSelection.Text = "";
            _currentRoute = new SavedRouteClass { Name = "" };
            dataGridViewRouteSystems.Rows.Clear();
            dateTimePickerStartDate.Value = DateTime.Now;
            dateTimePickerStartDate.Checked = false;
            dateTimePickerEndDate.Value = DateTime.Now;
            dateTimePickerEndDate.Checked = false;
            dateTimePickerStartTime.Value = DateTime.Now;
            dateTimePickerStartTime.Checked = false;
            dateTimePickerEndTime.Value = DateTime.Now;
            dateTimePickerEndTime.Checked = false;
            textBoxRouteName.Text = "";
            txtCmlDistance.Text = "";
            txtP2PDIstance.Text = "";
        }

        private void toolStripButtonShowOn3DMap_Click(object sender, EventArgs e)
        {
            var map = _discoveryForm.Map;
            var route = dataGridViewRouteSystems.Rows.OfType<DataGridViewRow>()
                .Where(r => r.Index < dataGridViewRouteSystems.NewRowIndex && r.Cells[0].Tag != null)
                .Select(s => s.Cells[0].Tag as SystemClassDB)
                .Where(s => s.HasCoordinate).ToList();

            if (route.Count >= 2)
            {
                _discoveryForm.history.FillInPositionsFSDJumps();
                map.Prepare(route[0], _discoveryForm.GetHomeSystem(), route[0], 400 / CalculateRouteMaxDistFromOrigin(), _discoveryForm.history.FilterByTravel);
                map.SetPlanned(route);
                map.Show();
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show(ParentForm, "No route set up. Please add at least two systems.", "No Route", MessageBoxButtons.OK);
                return;
            }
        }

        private void dataGridViewRouteSystems_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string sysname = e.FormattedValue.ToString();
                var row = dataGridViewRouteSystems.Rows[e.RowIndex];
                var cell = dataGridViewRouteSystems[e.ColumnIndex, e.RowIndex];

                SystemClass sys = SystemClassDB.GetSystem(sysname);

                if (sysname != "" && sys == null && !edsm.IsKnownSystem(sysname))
                {
                    row.ErrorText = "System not known to EDSM";
                }
                else
                {
                    row.ErrorText = "";
                }
            }
        }

        private void dataGridViewRouteSystems_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                //UpdateSystemRow(e.RowIndex);
                //UpdateSystemRow(e.RowIndex + 1);
                //Force the totals to update
                UpdateSystemRows();
            }
        }

        private void dataGridViewRouteSystems_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                var hit = dataGridViewRouteSystems.HitTest(e.X, e.Y);
                if (hit.Type == DataGridViewHitTestType.RowHeader && hit.RowIndex != -1)
                {
                    _dragRowIndex = hit.RowIndex;
                    Size dragsize = SystemInformation.DragSize;
                    _dragBox = new Rectangle(e.X - dragsize.Width / 2, e.Y - dragsize.Height / 2, dragsize.Width, dragsize.Height);
                }
                else
                {
                    _dragBox = Rectangle.Empty;
                }
            }
        }

        private void dataGridViewRouteSystems_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                if (_dragBox != Rectangle.Empty && !_dragBox.Contains(e.Location))
                {
                    dataGridViewRouteSystems.DoDragDrop(dataGridViewRouteSystems.Rows[_dragRowIndex], DragDropEffects.Move);
                }
            }
        }

        private void dataGridViewRouteSystems_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
            {
                var data = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                if (data.DataGridView == dataGridViewRouteSystems)
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
            else if (e.Data.GetDataPresent(typeof(string)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void dataGridViewRouteSystems_DragDrop(object sender, DragEventArgs e)
        {
            Point p = dataGridViewRouteSystems.PointToClient(new Point(e.X, e.Y));
            var insertIndex = dataGridViewRouteSystems.HitTest(p.X, p.Y).RowIndex;
            if (insertIndex >= dataGridViewRouteSystems.Rows.Count)
            {
                insertIndex = dataGridViewRouteSystems.Rows.Count - 1;
            }

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
            {
                if (e.Effect == DragDropEffects.Move)
                {
                    var row = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                    if (row.DataGridView == dataGridViewRouteSystems)
                    {
                        dataGridViewRouteSystems.Rows.Remove(row);
                        dataGridViewRouteSystems.Rows.Insert(insertIndex, row);
                        UpdateSystemRows();
                    }
                }
            }
            else if (e.Data.GetDataPresent(typeof(string)) && e.Effect == DragDropEffects.Copy)
            {
                var data = e.Data.GetData(typeof(string)) as string;
                var rows = data.Replace("\r", "").Split('\n').Where(r => r != "").ToArray();
                InsertRows(insertIndex, rows);
            }
        }

        private void contextMenuCopyPaste_Opening(object sender, CancelEventArgs e)
        {
            bool hastext = false;

            try
            {
                hastext = Clipboard.ContainsText();
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Unable to access clipboard");
            }

            if (hastext)
            {
                pasteToolStripMenuItem.Enabled = true;
                insertCopiedToolStripMenuItem.Enabled = true;
            }
            else
            {
                pasteToolStripMenuItem.Enabled = false;
                insertCopiedToolStripMenuItem.Enabled = false;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataObject obj = dataGridViewRouteSystems.GetClipboardContent();

            try
            {
                Clipboard.SetDataObject(obj);
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Unable to access clipboard");
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string data = null;

            try
            {
                data = Clipboard.GetText();
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Unable to access clipboard");
            }

            if (data != null)
            {
                var rows = data.Replace("\r", "").Split('\n').Where(r => r != "").ToArray();
                int[] selectedRows = dataGridViewRouteSystems.SelectedCells.OfType<DataGridViewCell>().Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
                int insertRow = selectedRows.FirstOrDefault();
                foreach (int index in selectedRows.Reverse())
                {
                    if (index != dataGridViewRouteSystems.NewRowIndex)
                    {
                        dataGridViewRouteSystems.Rows.RemoveAt(index);
                    }
                }
                InsertRows(insertRow, rows);
            }
        }

        private void insertCopiedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string data = null;

            try
            {
                data = Clipboard.GetText();
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Unable to access clipboard");
            }

            if (data != null)
            {
                var rows = data.Replace("\r", "").Split('\n').Where(r => r != "").ToArray();
                int[] selectedRows = dataGridViewRouteSystems.SelectedCells.OfType<DataGridViewCell>().Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
                int insertRow = selectedRows.FirstOrDefault();
                InsertRows(insertRow, rows);
            }
        }

        private void dataGridViewRouteSystems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {           // autopaint the row number..
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            if (!e.IsLastVisibleRow)
            {
                var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);

                // right alignment might actually make more sense for numbers
                using (var centerFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (Brush br = new SolidBrush(grid.RowHeadersDefaultCellStyle.ForeColor))
                    e.Graphics.DrawString(rowIdx, grid.RowHeadersDefaultCellStyle.Font, br, headerBounds, centerFormat);
            }
        }

        private void deleteRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridViewRouteSystems.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridViewRouteSystems.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
            foreach (int index in selectedRows.Reverse())
            {
                dataGridViewRouteSystems.Rows.RemoveAt(index);
            }
            UpdateSystemRows();
        }

        private void buttonReverseRoute_Click(object sender, EventArgs e)
        {
            var route = new SavedRouteClass();
            UpdateRouteInfo(route);
            dataGridViewRouteSystems.Rows.Clear();
            InsertRows(0, route.Systems.Reverse<string>().ToArray());
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show(ParentForm, "Are you sure you want to delete this route?", "Delete Route", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (_currentRoute.Id >= 0)
                {
                    if (DeleteIsPermanent && !EDSMClass.Expeditions.Any(r => r.Name.Equals(_currentRoute.Name)))
                    {
                        _currentRoute.Delete();
                    }
                    else
                    {   // Expeditions shouldn't use .Delete(), as LoadControl will ignorantly re-create them at next startup.
                        _currentRoute.Name = "\x7F" + _currentRoute.Name;
                        _currentRoute.Update();
                    }

                    _savedRoutes.Remove(_currentRoute);
                    UpdateComboBox();
                }

                ClearRoute();
            }
        }
        
        private void toolStripButtonImportFile_Click(object sender, EventArgs e)
        {
            if (!PromptAndSaveIfNeeded())
                return;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files|*.txt";
            ofd.Title = "Select a route file";

            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            string[] sysnames;

            try
            {
                sysnames = System.IO.File.ReadAllLines(ofd.FileName);
            }
            catch (IOException)
            {
                ExtendedControls.MessageBoxTheme.Show(ParentForm, $"There was an error reading {ofd.FileName}",
                    "Import route", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<String> systems = new List<String>();
            foreach (String name in sysnames)
            {
                String sysname = name;
                if (sysname.Contains(","))
                {
                    String[] values = sysname.Split(',');
                    sysname = values[0];
                }
                if (!String.IsNullOrWhiteSpace(sysname))
                    systems.Add(sysname.Trim());
            }
            if (systems.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(ParentForm, "The imported file contains no known system names",
                    "Import route", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ClearRoute();
            toolStripComboBoxRouteSelection.SelectedItem = null;
            foreach (var sysname in systems)
            {
                dataGridViewRouteSystems.Rows.Add(sysname, "", "");
            }
            UpdateSystemRows();
        }

        private void toolStripButtonImportRoute_Click(object sender, EventArgs e)
        {
            if (_discoveryForm.RouteControl.RouteSystems == null
                || _discoveryForm.RouteControl.RouteSystems.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(ParentForm, "Please create a route on the route tab", "Import from route tab");
                return;
            }
            else if (!PromptAndSaveIfNeeded())
                return;

            ClearRoute();
            toolStripComboBoxRouteSelection.SelectedItem = null;

            foreach (EliteDangerousCore.DB.SystemClassDB s in _discoveryForm.RouteControl.RouteSystems)
            {
                dataGridViewRouteSystems.Rows.Add(s.name, "", "");
            }
            UpdateSystemRows();
        }

        private void toolStripButtonExport_Click(object sender, EventArgs e)
        {
            string filename = "";
            var rt = new SavedRouteClass();
            UpdateRouteInfo(rt);

            try
            {
                if (rt.Systems.Count < 1)
                {
                    ExtendedControls.MessageBoxTheme.Show(ParentForm, "There is no route to export ",
                        "Export route", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Route export| *.txt";
                dlg.Title = "Export route";
                if (_currentRoute != null && !String.IsNullOrWhiteSpace(_currentRoute.Name))
                    dlg.FileName = _currentRoute.Name + ".txt";
                else
                    dlg.FileName = "route.txt";

                string fileName = dlg.FileName;
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }
                dlg.FileName = fileName;

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                filename = dlg.FileName;
                using (StreamWriter writer = new StreamWriter(filename, false))
                {
                    foreach (var sysname in rt.Systems)
                    {
                        if (!string.IsNullOrWhiteSpace(sysname))
                            writer.WriteLine(sysname);
                    }
                }
                ExtendedControls.MessageBoxTheme.Show(ParentForm, $"Export completed to {filename}", "Export route");
            }
            catch (IOException)
            {
                ExtendedControls.MessageBoxTheme.Show(ParentForm, $"Problem exporting route. Is file {filename} already open?",
                    "Export route", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void setTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridViewRouteSystems.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridViewRouteSystems.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();

            if (selectedRows.Length == 0)
                return;
            var obj = dataGridViewRouteSystems[0, selectedRows[0]].Value;

            if (obj == null)
                return;
            TargetHelpers.setTargetSystem(this,_discoveryForm, (string)obj);
        }

        private void editBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridViewRouteSystems.SelectedCells.OfType<DataGridViewCell>()
                .Where(c => c.RowIndex != dataGridViewRouteSystems.NewRowIndex)
                .Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();

            if (selectedRows.Length == 0)
                return;
            var obj = dataGridViewRouteSystems[0, selectedRows[0]].Value;

            if (obj == null)
                return;
            SystemClass sc = SystemClassDB.GetSystem((string)obj);
            if (sc == null)
            {
                ExtendedControls.MessageBoxTheme.Show(ParentForm, "Unknown system, system is without co-ordinates", "Edit bookmark", MessageBoxButtons.OK);
            }
            else
                TargetHelpers.showBookmarkForm(this,_discoveryForm, sc, null, false);
        }
    }
}
