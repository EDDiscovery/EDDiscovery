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

namespace EDDiscovery.UserControls
{
    public partial class UserControlExpedition : UserControlCommonBase
    {
        private List<SavedRouteClass> savedroute;
        private SavedRouteClass currentroute;
        private Rectangle dragBox;
        private int dragRowIndex;
        private bool suppressCombo = false;

        private List<ISystem> latestplottedroute;

        private string DbColumnSave { get { return DBName("UserControlExpedition","DGVCol"); } }

        #region Standard UC Interfaces

        public UserControlExpedition()
        {
            InitializeComponent();
            var corner = dataGridViewRouteSystems.TopLeftHeaderCell; // work around #1487
            SystemName.AutoCompleteGenerator = SystemCache.ReturnSystemAutoCompleteList;
            currentroute = new SavedRouteClass("");
            savedroute = new List<SavedRouteClass>();
        }

        public override void Init()
        {
            dateTimePickerEndDate.Value = dateTimePickerEndTime.Value = dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);

            discoveryform.OnNewCalculatedRoute += discoveryForm_OnNewCalculatedRoute;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolStrip, this);
            BaseUtils.Translator.Instance.Translate(contextMenuCopyPaste, this);
            BaseUtils.Translator.Instance.Translate(ctxMenuCombo, this);
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            DisplayRoute();
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewRouteSystems, DbColumnSave);

            UpdateRoutes();
            discoveryform.OnExpeditionsDownloaded += Discoveryform_OnExpeditionsDownloaded; // only from now on are we interested in a change

            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList += OnNewStars;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList -= OnNewStars;
            uctg = thc;
            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList += OnNewStars;
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewRouteSystems, DbColumnSave);

            discoveryform.OnNewCalculatedRoute -= discoveryForm_OnNewCalculatedRoute;
            discoveryform.OnExpeditionsDownloaded -= Discoveryform_OnExpeditionsDownloaded;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;

            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList -= OnNewStars;
        }

        private void Discoveryform_OnExpeditionsDownloaded(bool changed)        // because this is done async, pick up so we can refresh
        {
            System.Diagnostics.Debug.WriteLine("Expeditions downloaded, changed " + changed);
            if (changed)
                UpdateRoutes();
        }

        private void UpdateRoutes()
        {
            savedroute = SavedRouteClass.GetAllSavedRoutes();   // all including deleted
            savedroute = savedroute.Where(r => !r.Deleted).OrderBy(r => r.Name).ToList();   // don't list deleted
            UpdateUndeleteMenu();
            UpdateComboBox();
        }

        private void DisplayRoute()
        {
            dataGridViewRouteSystems.Rows.Clear();

            textBoxRouteName.Text = currentroute.Name;
            if (currentroute.StartDateUTC == null)
            {
                dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
                dateTimePickerStartTime.Checked = dateTimePickerStartDate.Checked = false;
            }
            else
            {
                dateTimePickerStartTime.Checked = dateTimePickerStartDate.Checked = true;
                dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(currentroute.StartDateUTC.Value);
            }

            if (currentroute.EndDateUTC == null)
            {
                dateTimePickerEndTime.Value = dateTimePickerEndDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
                dateTimePickerEndTime.Checked = dateTimePickerEndDate.Checked = false;
            }
            else
            {
                dateTimePickerEndTime.Checked = dateTimePickerEndDate.Checked = true;
                dateTimePickerEndTime.Value = dateTimePickerEndDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(currentroute.EndDateUTC.Value);
            }

            foreach (string sysname in currentroute.Systems)
            {
                var rowobj = new object[] { sysname, "", "" };
                dataGridViewRouteSystems.Rows.Add(rowobj);
            }

            UpdateSystemRows();
        }

        #endregion

        #region Interaction with other parts of the system

        private void discoveryForm_OnNewCalculatedRoute(List<ISystem> obj) // called when a new route is calculated
        {
            latestplottedroute = obj;
        }

        private void OnNewStars(List<string> obj, OnNewStarsPushType command)    // and if a user asked for stars to be added
        {
            if (command == OnNewStarsPushType.Expedition)
                AppendRows(obj.ToArray());
        }

        #endregion

        #region UC call backs

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

        #endregion

        #region Set up UI

        private void UpdateComboBox()
        {
            suppressCombo = true;
            toolStripComboBoxRouteSelection.Items.Clear();

            foreach (var route in savedroute)
            {
                toolStripComboBoxRouteSelection.Items.Add(route.Name);
            }

            toolStripComboBoxRouteSelection.Text = currentroute.Name;
            suppressCombo = false;
        }

        private void UpdateUndeleteMenu()
        {
            var delrts = SavedRouteClass.GetAllSavedRoutes().Where(r => r.Deleted).OrderBy(r => r.Name);

            if (ctxMenuItemUndelete.HasDropDownItems)
            {
                for (int i = ctxMenuItemUndelete.DropDownItems.Count - 1; i >= 0; i--)
                    ctxMenuItemUndelete.DropDownItems[i].Dispose();
                ctxMenuItemUndelete.DropDownItems.Clear();
            }

            foreach (var drt in delrts)
            {
                var menuitem = new ToolStripMenuItem(drt.Name)
                {
                    Name = "UndeleteRouteSubMenuItem_" + drt.Name.Replace(" ", string.Empty),
                    Tag = drt
                };
                menuitem.Click += UndeleteRouteSubMenuItem_Click;
                ctxMenuItemUndelete.DropDownItems.Add(menuitem);
            }
        }

        #endregion

        #region UI

        private void UndeleteRouteSubMenuItem_Click(object sender, EventArgs e)
        {
            var tmi = sender as ToolStripMenuItem;
            var rte = tmi.Tag as SavedRouteClass;

            rte.Deleted = false;
            rte.Update();
            savedroute.Add(rte);
            savedroute = savedroute.OrderBy(r => r.Name).ToList();
            UpdateComboBox();

            tmi.Dispose();          // Gets removed automatically.
        }


        private void toolStripComboBoxRouteSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressCombo)
                return;

            if (!PromptAndSaveIfNeeded())
            {
                suppressCombo = true;
                toolStripComboBoxRouteSelection.Text = currentroute.Name;
                suppressCombo = false;
                return;
            }

            if (toolStripComboBoxRouteSelection.SelectedIndex == -1)
                return;

            currentroute = savedroute[toolStripComboBoxRouteSelection.SelectedIndex];
            DisplayRoute();
        }


        private void toolStripComboBoxRouteSelection_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && ctxMenuItemUndelete.DropDownItems.Count > 0)
            {
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

        private void toolStripButtonShowOn3DMap_Click(object sender, EventArgs e)
        {
            var map = discoveryform.Map;
            var route = dataGridViewRouteSystems.Rows.OfType<DataGridViewRow>()
                .Where(r => r.Index < dataGridViewRouteSystems.NewRowIndex && r.Cells[0].Tag != null)
                .Select(s => s.Cells[0].Tag as ISystem)
                .Where(s => s.HasCoordinate).ToList();

            if (route.Count >= 2)
            {
                this.Cursor = Cursors.WaitCursor;
                discoveryform.history.FillInPositionsFSDJumps();
                map.Prepare(route[0], EDCommander.Current.HomeSystemTextOrSol, route[0], 400 / CalculateRouteMaxDistFromOrigin(route), discoveryform.history.FilterByTravel);
                map.SetPlanned(route);
                map.MoveToSystem(route[0]);
                map.Show();
                this.Cursor = Cursors.Default;
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No route set up. Please add at least two systems.".T(EDTx.UserControlExpedition_NoRoute), "Warning".T(EDTx.Warning), MessageBoxButtons.OK);
                return;
            }
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
            if (ExtendedControls.MessageBoxTheme.Show(FindForm(), "Are you sure you want to delete this route?".T(EDTx.UserControlExpedition_Delete), "Warning".T(EDTx.Warning), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (currentroute.Id >= 0)
                {
                    if (currentroute.EDSM)
                    {
                        currentroute.Deleted = true;
                        currentroute.Update();
                        UpdateUndeleteMenu();
                    }
                    else
                    {
                        currentroute.Delete();
                    }

                    savedroute.Remove(currentroute);
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
            ofd.Title = "Select a route file".T(EDTx.UserControlExpedition_SelRoute);

            if (ofd.ShowDialog(FindForm()) != System.Windows.Forms.DialogResult.OK)
                return;
            string[] sysnames;

            try
            {
                sysnames = System.IO.File.ReadAllLines(ofd.FileName);
            }
            catch (IOException)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "There was an error reading file".T(EDTx.UserControlExpedition_FileE),
                    "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "The imported file contains no known system names".T(EDTx.UserControlExpedition_Nonames),
                    "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            if (latestplottedroute == null || latestplottedroute.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Please create a route on a route panel".T(EDTx.UserControlExpedition_Createroute), "Warning".T(EDTx.Warning));
                return;
            }
            else if (!PromptAndSaveIfNeeded())
                return;

            ClearRoute();
            toolStripComboBoxRouteSelection.SelectedItem = null;

            foreach (ISystem s in latestplottedroute)
            {
                dataGridViewRouteSystems.Rows.Add(s.Name, "", "");
            }
            UpdateSystemRows();
        }

        private void toolStripButtonExport_Click(object sender, EventArgs e)
        {
            var rt = new SavedRouteClass();
            UpdateRouteInfo(rt);

            if (rt.Systems.Count < 1)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "There is no route to export ".T(EDTx.UserControlExpedition_NoRouteExport),
                    "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(new string[] { "Route", "Grid" }, disablestartendtime: true, outputext: new string[] { "Text File|*.txt", "CSV export| *.csv"});

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (frm.SelectedIndex == 0)     // old route export
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(frm.Path, false))
                        {
                            foreach (var sysname in rt.Systems)
                            {
                                if (!string.IsNullOrWhiteSpace(sysname))
                                    writer.WriteLine(sysname);
                            }
                        }

                        if (frm.AutoOpen)
                        {
                            try
                            {
                                System.Diagnostics.Process.Start(frm.Path);
                            }
                            catch
                            {
                                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to open " + frm.Path, "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }
                    catch 
                    {
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), $"Problem exporting route. Is file {frm.Path} already open?",
                            "Export route", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                    grd.SetCSVDelimiter(frm.Comma);
                    grd.GetLineStatus += delegate (int r)
                    {
                        if (r < dataGridViewRouteSystems.Rows.Count)
                        {
                            return BaseUtils.CSVWriteGrid.LineStatus.OK;
                        }
                        else
                            return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        DataGridViewRow rw = dataGridViewRouteSystems.Rows[r];
                        return new Object[] { rw.Cells[0].Value, rw.Cells[1].Value, rw.Cells[2].Value };
                    };

                    grd.GetHeader += delegate (int c)
                    {
                        return (c < 3 && frm.IncludeHeader) ? dataGridViewRouteSystems.Columns[c].HeaderText : null;
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }
        }

        #endregion

        #region Mouse UI

        private void dataGridViewRouteSystems_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                var hit = dataGridViewRouteSystems.HitTest(e.X, e.Y);
                if (hit.Type == DataGridViewHitTestType.RowHeader && hit.RowIndex != -1)
                {
                    dragRowIndex = hit.RowIndex;
                    Size dragsize = SystemInformation.DragSize;
                    dragBox = new Rectangle(e.X - dragsize.Width / 2, e.Y - dragsize.Height / 2, dragsize.Width, dragsize.Height);
                }
                else
                {
                    dragBox = Rectangle.Empty;
                }
            }
        }

        private void dataGridViewRouteSystems_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                if (dragBox != Rectangle.Empty && !dragBox.Contains(e.Location))
                {
                    dataGridViewRouteSystems.DoDragDrop(dataGridViewRouteSystems.Rows[dragRowIndex], DragDropEffects.Move);
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

        #endregion


        #region Right Click Context

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

        private void deleteRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridViewRouteSystems.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridViewRouteSystems.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
            foreach (int index in selectedRows.Reverse())
            {
                dataGridViewRouteSystems.Rows.RemoveAt(index);
            }
            UpdateSystemRows();
        }

        private void setTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridViewRouteSystems.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridViewRouteSystems.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();

            if (selectedRows.Length == 0)
                return;
            var obj = dataGridViewRouteSystems[0, selectedRows[0]].Value;

            if (obj == null)
                return;
            TargetHelpers.SetTargetSystem(this, discoveryform, (string)obj);
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
            ISystem sc = SystemCache.FindSystem((string)obj);
            if (sc == null)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Unknown system, system is without co-ordinates".T(EDTx.UserControlExpedition_UnknownS), "Warning".T(EDTx.Warning), MessageBoxButtons.OK);
            }
            else
                TargetHelpers.ShowBookmarkForm(this, discoveryform, sc, null, false);
        }


        #endregion


        #region Route Helpers

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

        private void ClearRoute()
        {
            toolStripComboBoxRouteSelection.Text = "";
            currentroute = new SavedRouteClass { Name = "" };
            dataGridViewRouteSystems.Rows.Clear();
            dateTimePickerEndDate.Value = dateTimePickerEndTime.Value = dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
            dateTimePickerEndTime.Checked = dateTimePickerEndDate.Checked = dateTimePickerStartTime.Checked = dateTimePickerStartDate.Checked = false;
            textBoxRouteName.Text = "";
            txtCmlDistance.Text = "";
            txtP2PDIstance.Text = "";
        }

        private static float CalculateRouteMaxDistFromOrigin(List<ISystem> systems)
        {
            var locSystems = systems?.Where(s => s != null && s.HasCoordinate).Distinct().ToList();
            if (locSystems == null || locSystems.Count < 2)
                return 100;

            return (float)locSystems
                .Except(new[] { locSystems[0] })
                .Select(s => locSystems[0].Distance(s))
                .Max();
        }

        private ISystem GetSystem(string sysname)
        {
            return SystemCache.FindSystem(sysname);
        }

        // If the route has any unsaved changes, prompt the user to save them.
        // Returns true if the caller is allowed to continue; false if caller should abort.
        private bool PromptAndSaveIfNeeded()
        {
            var rt = new SavedRouteClass();
            UpdateRouteInfo(rt);

            if (rt.Equals(currentroute))   // No changes have been made.
                return true;
            else
            {

             //   here get scanner to complain

                var result = ExtendedControls.MessageBoxTheme.Show(FindForm(), ("There are unsaved changes to the current route." + Environment.NewLine
                    + "Would you like to save the current route before proceeding?").T(EDTx.UserControlExpedition_Unsaved), "Warning".T(EDTx.Warning), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
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
            string newrtname = textBoxRouteName.Text?.Trim();
            string oldrtname = currentroute.Name;
            var newrt = new SavedRouteClass();
            var oldrt = currentroute;
            bool ret = false;

            UpdateRouteInfo(newrt);

            SavedRouteClass foundedsm = savedroute.Find(x => x.Name.Equals(newrtname, StringComparison.InvariantCultureIgnoreCase) && x.EDSM);

            if (string.IsNullOrEmpty(newrtname))
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Please specify a name for the route.".T(EDTx.UserControlExpedition_Specify), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBoxRouteName.Select();
            }
            else if ( foundedsm != null )
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), ("The current route name conflicts with a well-known expedition." + Environment.NewLine
                    + "Please specify a new name to save your changes.").T(EDTx.UserControlExpedition_Conflict), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBoxRouteName.Select();
                textBoxRouteName.SelectAll();
            }
            else
            {
                var overwriteroute = savedroute.Where(r => r.Name.Equals(newrtname) && r.Id != currentroute.Id).FirstOrDefault();

                if (overwriteroute != null)
                {
                    if (MessageBoxTheme.Show(FindForm(), "Warning: route already exists. Would you like to overwrite it?".T(EDTx.UserControlExpedition_Overwrite), "Warning".T(EDTx.Warning), MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return false;

                    overwriteroute.Delete();
                    savedroute.Remove(overwriteroute);
                }

                if (currentroute.Id < 0)
                {
                    ret = newrt.Add();
                }
                else
                {
                    newrt.Id = currentroute.Id;
                    ret = newrt.Update();
                    savedroute.Remove(oldrt);
                }
                currentroute = newrt;
                savedroute.Add(newrt);
                savedroute = savedroute.Where(r => !r.Deleted).OrderBy(r => r.Name).ToList();
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
                        double dist = sys.Distance(prevsys);
                        string strdist = dist >= 0 ? ((double)dist).ToString("0.00") : "";
                        dataGridViewRouteSystems[1, rowindex].Value = strdist;
                    }
                }

                dataGridViewRouteSystems[0, rowindex].Tag = sys;
                dataGridViewRouteSystems.Rows[rowindex].DefaultCellStyle.ForeColor = (sys != null && sys.HasCoordinate) ? discoveryform.theme.VisitedSystemColor : discoveryform.theme.NonVisitedSystemColor;

                if (sys != null)
                {
                    string note = "";
                    SystemNoteClass sn = SystemNoteClass.GetNoteOnSystem(sys.Name, sys.EDSMID);
                    if (sn != null && !string.IsNullOrWhiteSpace(sn.Note))
                    {
                        note = sn.Note;
                    }
                    else
                    {
                        BookmarkClass bkmark = GlobalBookMarkList.Instance.FindBookmarkOnSystem(sys.Name);
                        if (bkmark != null && !string.IsNullOrWhiteSpace(bkmark.Note))
                            note = bkmark.Note;
                        else
                        {
                            var gmo = discoveryform.galacticMapping.Find(sys.Name);
                            if (gmo != null && !string.IsNullOrWhiteSpace(gmo.description))
                                note = gmo.description;
                        }
                    }

                    dataGridViewRouteSystems[2, rowindex].Value = note.WordWrap(60);
                    dataGridViewRouteSystems.Rows[rowindex].Cells[0].ToolTipText = string.Format("{0:0.#},{1:0.#},{2:0.#}", sys.X, sys.Y, sys.Z);
                }

                if (sys == null && sysname != "")
                {
                    dataGridViewRouteSystems.Rows[rowindex].ErrorText = "System not known to EDSM".T(EDTx.UserControlExpedition_EDSMUnk);
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
                ISystem firstSC = null;
                ISystem lastSC = null;
                for (int i = 0; i < dataGridViewRouteSystems.Rows.Count; i++)
                {
                    if (firstSC == null && dataGridViewRouteSystems[0, i].Tag != null)
                        firstSC = (ISystem)dataGridViewRouteSystems[0, i].Tag;
                    if (dataGridViewRouteSystems[0, i].Tag != null)
                        lastSC = (ISystem)dataGridViewRouteSystems[0, i].Tag;
                    String value = dataGridViewRouteSystems[1, i].Value as string;
                    if (!String.IsNullOrWhiteSpace(value))
                        distance += Double.Parse(value);
                }
                txtCmlDistance.Text = distance.ToString("0.00") + "LY";
                distance = 0;
                if (firstSC != null && lastSC != null)
                {
                    distance = firstSC.Distance(lastSC);
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
                route.StartDateUTC = EDDConfig.Instance.ConvertTimeToUTCFromSelected(dateTimePickerStartDate.Value.Date);
                if (dateTimePickerStartTime.Checked)
                    route.StartDateUTC += dateTimePickerStartTime.Value.TimeOfDay;
            }
            else
            {
                route.StartDateUTC = null;
            }

            if (dateTimePickerEndDate.Checked)
            {
                route.EndDateUTC = EDDConfig.Instance.ConvertTimeToUTCFromSelected(dateTimePickerEndDate.Value.Date);
                route.EndDateUTC += dateTimePickerEndTime.Checked ? dateTimePickerEndTime.Value.TimeOfDay : new TimeSpan(23, 59, 59);
            }
            else
            {
                route.EndDateUTC = null;
            }
        }

        #endregion

        #region Validation

        private void dataGridViewRouteSystems_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string sysname = e.FormattedValue.ToString();
                var row = dataGridViewRouteSystems.Rows[e.RowIndex];

                if (sysname != "" && GetSystem(sysname) == null)
                {
                    row.ErrorText = "System not known to EDSM".T(EDTx.UserControlExpedition_EDSMUnk);
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
                UpdateSystemRows();
            }
        }

        #endregion
    }
}
