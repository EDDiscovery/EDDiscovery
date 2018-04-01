/*
 * Copyright © 2017 EDDiscovery development team
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EliteDangerousCore.EDSM;
using System.IO;
using EDDiscovery.UserControls;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using EDDiscovery.Forms;
using EliteDangerousCore.DB;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

namespace EDDiscovery.UserControls
{
    public partial class UserControlExploration :  UserControlCommonBase
    {
        private string DbColumnSave { get { return ("ModulesGrid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }

        public static bool DeleteIsPermanent = true;

        private List<ExplorationSetClass> _savedExplorationSets;
        private ExplorationSetClass _currentExplorationSet;
        private EDSMClass edsm;
        private Rectangle _dragBox;
        private int _dragRowIndex;
        HistoryList hl = null;
        //HistoryEntry last_he = null;

        public int JounalScan { get; private set; }

        public UserControlExploration()
        {
            InitializeComponent();
            var corner = dataGridViewExplore.TopLeftHeaderCell; // work around #1487
            ColumnSystemName.AutoCompleteGenerator = SystemClassDB.ReturnOnlySystemsListForAutoComplete;
            _currentExplorationSet = new ExplorationSetClass();
        }

        public override void Init()
        {
            edsm = new EDSMClass();
            _currentExplorationSet = new ExplorationSetClass();
            _savedExplorationSets = new List<ExplorationSetClass>();
            discoveryform.OnNewEntry += NewEntry;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }


        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewExplore, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewExplore, DbColumnSave);
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= NewEntry;
        }

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made.. check to see if its a scan update
        {
            if (he.EntryType == JournalTypeEnum.Scan || he.EntryType == JournalTypeEnum.FSDJump)
                UpdateSystemRows();
        }

        public override void InitialDisplay()
        {
            UpdateSystemRows();
        }

        private void Display(HistoryEntry he, HistoryList hl)            // when user clicks around..
        {
            UpdateSystemRows();
        }

        public void InsertRows(int insertIndex, params string[] sysnames)
        {
            foreach (var row in sysnames)
            {
                dataGridViewExplore.Rows.Insert(insertIndex, row, "", "", "", "", "", "", "", "");
                insertIndex++;
            }
            UpdateSystemRows();
        }

        public void AppendRows(params string[] sysnames)
        {
            foreach (var row in sysnames)
            {
                dataGridViewExplore.Rows.Add(row, "", "", "", "", "", "", "", "");
            }
            UpdateSystemRows();
        }

   

        private ISystem GetSystem(string sysname)
        {
            ISystem sys = SystemClassDB.GetSystem(sysname);

            if (sys == null)
            {
                //if (edsm.IsKnownSystem(sysname))
                //{
                //    sys = new SystemClass(sysname);
                //}
            }

            return sys;
        }

        private void UpdateSystemRow(int rowindex)
        {
            const int idxVisits = 5;
            const int idxScans = 6;
            const int idxPriStar = 7;
            const int idxInfo = 8;
            const int idxNote = 9;

            if (hl == null)
                hl = discoveryform.history;

            HistoryEntry last = hl.GetLast;

            if ( last == null )
                return;

            ISystem currentSystem = last.System;

            if (rowindex < dataGridViewExplore.Rows.Count && dataGridViewExplore[0, rowindex].Value != null)
            {
                string sysname = dataGridViewExplore[0, rowindex].Value.ToString();
                ISystem sys = (ISystem)dataGridViewExplore[0, rowindex].Tag;

                if (sys == null)
                {
                    
                    sys = GetSystem(sysname);
                }

                if (sys != null && currentSystem != null)
                {
                    double dist = sys.Distance(currentSystem);
                    string strdist = dist >= 0 ? ((double)dist).ToString("0.00") : "";
                    dataGridViewExplore[1, rowindex].Value = strdist;
                }

                dataGridViewExplore[0, rowindex].Tag = sys;
                dataGridViewExplore.Rows[rowindex].DefaultCellStyle.ForeColor = (sys != null && sys.HasCoordinate) ? discoveryform.theme.VisitedSystemColor : discoveryform.theme.NonVisitedSystemColor;


                if (sys != null)
                {
                    if (sys.HasCoordinate)
                    {
                        dataGridViewExplore[2, rowindex].Value = sys.X.ToString("0.00");
                        dataGridViewExplore[3, rowindex].Value = sys.Y.ToString("0.00");
                        dataGridViewExplore[4, rowindex].Value = sys.Z.ToString("0.00");
                    }


                    dataGridViewExplore[idxVisits, rowindex].Value = hl.GetVisitsCount(sysname).ToString();

                    List<JournalScan> scans = hl.GetScans(sysname);
                    dataGridViewExplore[idxScans, rowindex].Value = scans.Count.ToString();

                    string pristar = "";
                    // Search for primary star
                    foreach (var scan in scans)
                    {
                        if (scan.IsStar && scan.DistanceFromArrivalLS==0.0)
                        {
                            pristar = scan.StarType;
                            break;
                        }
                    }
                    dataGridViewExplore[idxPriStar, rowindex].Value = pristar;


                    string info = "";

                    foreach (var scan in scans)
                    {
                        if (scan.IsStar)
                        {
                            if (scan.StarTypeID == EDStar.AeBe)
                                info = info + " " + "AeBe";
                            if (scan.StarTypeID == EDStar.N)
                                info = info + " " + "NS";
                            if (scan.StarTypeID == EDStar.H)
                                info = info + " " + "BH";
                        }
                        else
                        {
                            if (scan.PlanetTypeID == EDPlanet.Earthlike_body)
                                info = info + " " + "ELW";
                            if (scan.PlanetTypeID == EDPlanet.Water_world)
                                info = info + " " + "WW";
                        }
                    }

                    dataGridViewExplore[idxInfo, rowindex].Value = info.Trim();


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
                    dataGridViewExplore[idxNote, rowindex].Value = note.WordWrap(60);
                }

                if (sys == null && sysname != "")
                {
                    dataGridViewExplore.Rows[rowindex].ErrorText = "System not known";
                }
                else
                {
                    dataGridViewExplore.Rows[rowindex].ErrorText = "";
                }
            }
        }


        private void UpdateSystemRows()
        {
            for (int i = 0; i < dataGridViewExplore.Rows.Count; i++)
            {
                UpdateSystemRow(i);
            }
        }

        private void UpdateExplorationInfo(ExplorationSetClass route)
        {
            route.Name = textBoxRouteName.Text.Trim();
            route.Systems.Clear();
            route.Systems.AddRange(dataGridViewExplore.Rows.OfType<DataGridViewRow>().Where(r => r.Cells[0].Value != null).Select(r => r.Cells[0].Value.ToString()));

        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            string explorepath = Path.Combine(EDDOptions.Instance.AppDataDirectory, "Exploration");
            if (!Directory.Exists(explorepath))
                Directory.CreateDirectory(explorepath);

            UpdateExplorationInfo(_currentExplorationSet);

            if (String.IsNullOrEmpty(textBoxFileName.Text))
            {
                SaveFileDialog dlg = new SaveFileDialog();

                dlg.InitialDirectory = explorepath;
                dlg.OverwritePrompt = true;
                dlg.DefaultExt = "json";
                dlg.AddExtension = true;
                dlg.Filter = "Explore file| *.json";

                if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    textBoxFileName.Text = dlg.FileName;
                }
                else
                    return;

     
            }
            UpdateExplorationInfo(_currentExplorationSet);
            _currentExplorationSet.Save(textBoxFileName.Text);
        }

        private void toolStripButtonLoad_Click(object sender, EventArgs e)
        {
            string explorepath = Path.Combine(EDDOptions.Instance.AppDataDirectory, "Exploration");
            if (!Directory.Exists(explorepath))
                Directory.CreateDirectory(explorepath);


            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = explorepath;
            dlg.DefaultExt = "json";
            dlg.AddExtension = true;
            dlg.Filter = "Explore file| *.json";

            if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
            {
                textBoxFileName.Text = dlg.FileName;
                _currentExplorationSet.Clear();
                _currentExplorationSet.Load(dlg.FileName);

                textBoxRouteName.Text = _currentExplorationSet.Name;
                dataGridViewExplore.Rows.Clear();
                foreach (var sysname in _currentExplorationSet.Systems)
                {
                    dataGridViewExplore.Rows.Add(sysname, "", "");
                }
                UpdateSystemRows();

            }
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            string explorepath = Path.Combine(EDDOptions.Instance.AppDataDirectory, "Exploration");
            if (!Directory.Exists(explorepath))
                Directory.CreateDirectory(explorepath);

            dlg.InitialDirectory = explorepath;
            dlg.OverwritePrompt = true;
            dlg.DefaultExt = "json";
            dlg.AddExtension = true;
            dlg.Filter = "Explore file| *.json";


            if (dlg.ShowDialog(FindForm()) == DialogResult.OK)
            {
                textBoxFileName.Text = dlg.FileName;
                UpdateExplorationInfo(_currentExplorationSet);
                ClearExplorationSet();
            }
        }

        private void ClearExplorationSet()
        {
            _currentExplorationSet = new ExplorationSetClass { Name = "" };
            dataGridViewExplore.Rows.Clear();
        
            textBoxRouteName.Text = "";
        
        }

     
        private void dataGridViewRouteSystems_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string sysname = e.FormattedValue.ToString();
                var row = dataGridViewExplore.Rows[e.RowIndex];
                var cell = dataGridViewExplore[e.ColumnIndex, e.RowIndex];

                ISystem sys = SystemClassDB.GetSystem(sysname);

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

        private void dataGridViewExplore_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                UpdateSystemRow(e.RowIndex);
            }
        }

        private void dataGridViewRouteSystems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var textbox = (ExtendedControls.TextBoxBorder)e.Control;
            if (dataGridViewExplore.CurrentCell.ColumnIndex != 0)
            {
                textbox.AutoCompleteMode = AutoCompleteMode.None;
                return;
            }

            //TBD this used to have an autocomplete, but now we don't have systemnames we are lacking it
        }

        private void dataGridViewRouteSystems_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                var hit = dataGridViewExplore.HitTest(e.X, e.Y);
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
                    dataGridViewExplore.DoDragDrop(dataGridViewExplore.Rows[_dragRowIndex], DragDropEffects.Move);
                }
            }
        }

        private void dataGridViewRouteSystems_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
            {
                var data = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                if (data.DataGridView == dataGridViewExplore)
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
            Point p = dataGridViewExplore.PointToClient(new Point(e.X, e.Y));
            var insertIndex = dataGridViewExplore.HitTest(p.X, p.Y).RowIndex;
            if (insertIndex >= dataGridViewExplore.Rows.Count)
            {
                insertIndex = dataGridViewExplore.Rows.Count - 1;
            }

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
            {
                if (e.Effect == DragDropEffects.Move)
                {
                    var row = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                    if (row.DataGridView == dataGridViewExplore)
                    {
                        dataGridViewExplore.Rows.Remove(row);
                        dataGridViewExplore.Rows.Insert(insertIndex, row);
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
            DataObject obj = dataGridViewExplore.GetClipboardContent();

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
                int[] selectedRows = dataGridViewExplore.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridViewExplore.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
                int insertRow = selectedRows.FirstOrDefault();
                foreach (int index in selectedRows.Reverse())
                {
                    dataGridViewExplore.Rows.RemoveAt(index);
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
                int[] selectedRows = dataGridViewExplore.SelectedCells.OfType<DataGridViewCell>().Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
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
            int[] selectedRows = dataGridViewExplore.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridViewExplore.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
            foreach (int index in selectedRows.Reverse())
            {
                dataGridViewExplore.Rows.RemoveAt(index);
            }
            UpdateSystemRows();
        }


        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show(FindForm(), "Are you sure you want to delete this route?", "Delete Route", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (DeleteIsPermanent)
                {
                    _currentExplorationSet.Delete();
                }
                else
                {
                    _currentExplorationSet.Name = "\x7F" + _currentExplorationSet.Name;

                }

                _savedExplorationSets.Remove(_currentExplorationSet);

                ClearExplorationSet();
            }
        }
        
        private void toolStripButtonImportFile_Click(object sender, EventArgs e)
        {


            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files|*.txt";
            ofd.Title = "Select a exploration set file";

            if (ofd.ShowDialog(FindForm()) != System.Windows.Forms.DialogResult.OK)
                return;

            ClearExplorationSet();

            string[] sysnames;



            try
            {
                sysnames = System.IO.File.ReadAllLines(ofd.FileName);
            }
            catch (IOException)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), $"There was a problem opening file {ofd.FileName}", "Import file",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<String> systems = new List<String>();
            int countunknown = 0;
            foreach (String name in sysnames)
            {
                String sysname = name;
                if (sysname.Contains(","))
                {
                    String[] values = sysname.Split(',');
                    sysname = values[0];
                }
                if (String.IsNullOrWhiteSpace(sysname))
                    continue;
                ISystem sc = GetSystem(sysname.Trim());
                if (sc == null)
                {
                    sc = new SystemClass(sysname.Trim());
                    countunknown++;
                }
                systems.Add(sc.Name);

            }
            if (systems.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(),
                    "The imported file contains no known system names",
                    "Unsaved", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

      
            foreach (var sysname in systems)
            {
                dataGridViewExplore.Rows.Add(sysname, "", "");
            }
            UpdateSystemRows();
        }

 

        private void toolStripButtonExport_Click(object sender, EventArgs e)
        {
            string filename = "";
            try
            {

                if (dataGridViewExplore.Rows.Count == 0
                    || (dataGridViewExplore.Rows.Count == 1 && dataGridViewExplore[0, 0].Value == null))
                {
                    ExtendedControls.MessageBoxTheme.Show(FindForm(),
                    "There is no route to export ", "Export route", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Route export| *.txt";
                dlg.Title = "Export route";
                if (_currentExplorationSet != null && !String.IsNullOrWhiteSpace(_currentExplorationSet.Name))
                    dlg.FileName = _currentExplorationSet.Name + ".txt";
                else
                    dlg.FileName = "route.txt";

                string fileName = dlg.FileName;
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }
                dlg.FileName = fileName;

                if (dlg.ShowDialog(FindForm()) != DialogResult.OK)
                    return;
                filename = dlg.FileName;
                using (StreamWriter writer = new StreamWriter(filename, false))
                {
                    for (int i = 0; i < dataGridViewExplore.Rows.Count; i++)
                    {
                        String sysname = (String)dataGridViewExplore[0, i].Value;
                        if (!String.IsNullOrWhiteSpace(sysname))
                            writer.WriteLine(sysname);
                    }
                }
                ExtendedControls.MessageBoxTheme.Show(FindForm(), $"Export complete to {filename}", "Export route");
            }
            catch (IOException)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), $"Error exporting route. Is file {filename} open?", "Export route",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void setTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridViewExplore.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridViewExplore.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();

            if (selectedRows.Length == 0)
                return;
            var obj = dataGridViewExplore[0, selectedRows[0]].Value;

            if (obj == null)
                return;
            TargetHelpers.setTargetSystem(this,discoveryform, (string)obj);
        }

        private void editBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridViewExplore.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridViewExplore.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();

            if (selectedRows.Length == 0)
                return;
            var obj = dataGridViewExplore[0, selectedRows[0]].Value;

            if (obj == null)
                return;
            ISystem sc = SystemClassDB.GetSystem((string)obj);
            if (sc == null)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Unknown system, system is without co-ordinates", "Edit bookmark", MessageBoxButtons.OK);
            }
            else
                TargetHelpers.showBookmarkForm(this,discoveryform, sc, null, false);
        }


        private void tsbImportSphere_Click(object sender, EventArgs e)
        {
            string systemName;
            double radius;

            if (!ImportSphere.showDialog(discoveryform, out systemName, out radius, FindForm()))
                return;

            if (String.IsNullOrWhiteSpace(systemName))
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System name not set");
                return;
            }

            if (radius < 0 || radius > 1000.0) { 
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Radius should be a number 0.0 and 1000.0");
                return;
            }

            ClearExplorationSet();
            EDSMClass edsm = new EDSMClass();
            Cursor.Current = Cursors.WaitCursor;
            Task<List<Tuple<ISystem, double>>> taskEDSM = Task<List<Tuple<ISystem, double>>>.Factory.StartNew(() =>
            {
                return edsm.GetSphereSystems(systemName, radius);
            });
            Task.WaitAll();
            LoadSphereData(taskEDSM);
            Cursor.Current = Cursors.Default;

        }

        private void LoadSphereData(Task<List<Tuple<ISystem, double>>> task)
        {
            List<String> systems = new List<String>();
            int countunknown = 0;
            foreach (Tuple<ISystem,double> ret in task.Result)
            {
                string name = ret.Item1.Name;

                ISystem sc = GetSystem(name.Trim());
                if (sc == null)
                {
                    sc = new SystemClass(name.Trim());
                    countunknown++;
                }
                systems.Add(sc.Name);
            }
            if (systems.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "The imported file contains no known system names",
                    "Unsaved", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (var sysname in systems)
            {
                dataGridViewExplore.Rows.Add(sysname, "", "");
            }
            UpdateSystemRows();
        }

        private void dataGridViewExplore_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 1 && e.Column.Index <= 6)
                e.SortDataGridViewColumnDate();

        }
    }

    public class ExplorationSetClass
    {
        public string Name { get; set; }
        public List<string> Systems { get; private set; }

        public ExplorationSetClass()
        {
            this.Systems = new List<string>();
        }

        public void Delete()
        { }


        public void Save(string fileName)
        {
            JObject jo = new JObject();

            if (string.IsNullOrEmpty(fileName))
                return;

            jo["Name"] = Name;

            jo["Systems"] = new JArray(Systems);

            File.WriteAllText(fileName, jo.ToString());
        }


        public void Load(string fileName)
        {
            JObject jo = JObject.Parse(File.ReadAllText(fileName));


            Name = jo["Name"].ToString();

            Systems.Clear();

            JArray ja = (JArray)jo["Systems"];

            foreach (var jsys in ja)
            {
                string sysname = jsys.Value<String>();
                if (!Systems.Contains(sysname))
                    Systems.Add(sysname);
            }

        }

        internal void Clear()
        {
            Name = "";
            Systems.Clear();
        }
    }
}
