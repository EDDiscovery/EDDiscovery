using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery.DB;
using EDDiscovery2.DB;
using EDDiscovery2.EDSM;
using System.IO;
using EMK.LightGeometry;
using EDDiscovery.EDSM;
using EDDiscovery2;
using EDDiscovery.UserControls;
using EDDiscovery.EliteDangerous.JournalEvents;

namespace EDDiscovery
{
    public partial class UserControlExploration :  UserControlCommonBase
    {
        private int displaynumber = 0;

        public static bool DeleteIsPermanent = true;

        private List<ExplorationSetClass> _savedExplorationSets;
        private ExplorationSetClass _currentExplorationSet;
        private EDDiscoveryForm _discoveryForm;
        private TravelHistoryControl travelhistorycontrol;
        private EDSMClass edsm;
        private int _currentExploreIndex;
        private Rectangle _dragBox;
        private int _dragRowIndex;
        HistoryList hl = null;
        HistoryEntry last_he = null;

        public int JounalScan { get; private set; }

        public UserControlExploration()
        {
            InitializeComponent();
            _currentExplorationSet = new ExplorationSetClass();
        }

        public override void Init(EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            _discoveryForm = ed;
            travelhistorycontrol = ed.TravelControl;
            displaynumber = vn;
            edsm = new EDSMClass();
            _currentExplorationSet = new ExplorationSetClass();
            _savedExplorationSets = new List<ExplorationSetClass>();
            _discoveryForm.OnNewEntry += NewEntry;
            travelhistorycontrol.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            travelhistorycontrol.OnTravelSelectionChanged -= Display;
            _discoveryForm.OnNewEntry -= NewEntry;
        }

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made.. check to see if its a scan update
        {
            if (he.EntryType == EliteDangerous.JournalTypeEnum.Scan || he.EntryType == EliteDangerous.JournalTypeEnum.FSDJump)
                UpdateSystemRows();
        }

        public override void Display(HistoryEntry he, HistoryList hl)            // when user clicks around..
        {
            UpdateSystemRows();
        }

        public void LoadControl()
        {
            //_savedRoutes = ExplorationSetClass.GetAllSavedRoutes();

            //foreach (var initroute in InitialRoutes)
            //{
            //    if (!_savedRoutes.Any(r => r.Name == initroute.Name || r.Name == "\x7F" + initroute.Name))
            //    {
            //        initroute.Add();
            //        _savedRoutes.Add(initroute);
            //    }
            //}

            //_savedRoutes = _savedRoutes.Where(r => !r.Name.StartsWith("\x7F")).ToList();


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

   

        private SystemClass GetSystem(string sysname)
        {
            SystemClass sys = SystemClass.GetSystem(sysname);

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
            const int idxNote = 8;
            const int idxScans = 6;
            const int idxVisits = 5;

            if (hl == null)
                hl = _discoveryForm.history;

            if (hl.GetLast == null)
                return;
            ISystem currentSystem = hl.GetLast.System;

            

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
                    double dist = SystemClass.Distance(sys, currentSystem);
                    string strdist = dist >= 0 ? ((double)dist).ToString("0.00") : "";
                    dataGridViewExplore[1, rowindex].Value = strdist;
                }

                dataGridViewExplore[0, rowindex].Tag = sys;
                dataGridViewExplore.Rows[rowindex].DefaultCellStyle.ForeColor = (sys != null && sys.HasCoordinate) ? _discoveryForm.theme.VisitedSystemColor : _discoveryForm.theme.NonVisitedSystemColor;


                if (sys != null)
                {
                    if (sys.HasCoordinate)
                    {
                        dataGridViewExplore[2, rowindex].Value = sys.x.ToString("0.00");
                        dataGridViewExplore[3, rowindex].Value = sys.y.ToString("0.00");
                        dataGridViewExplore[4, rowindex].Value = sys.z.ToString("0.00");
                    }


                    dataGridViewExplore[idxVisits, rowindex].Value = hl.GetVisitsCount(sysname).ToString();

                    List<JournalScan> scans = hl.GetScans(sysname);
                    dataGridViewExplore[idxScans, rowindex].Value = scans.Count.ToString();


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
                    dataGridViewExplore[idxNote, rowindex].Value = Tools.WordWrap(note, 60);
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
            //UpdateRouteInfo(_currentRoute);

            //if (_currentRoute.Id == -1)
            //{
            //    _currentRoute.Add();
            //    _savedRoutes.Add(_currentRoute);
            //    UpdateComboBox();
            //}
            //else
            //{
            //    _currentRoute.Update();
            //}
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            ExplorationSetClass newroute = new ExplorationSetClass();
            UpdateExplorationInfo(newroute);

            //if (!newroute.Equals(_currentRoute))
            //{
            //    var result = MessageBox.Show(_discoveryForm, "There are unsaved changes to the current route.\r\nAre you sure you want to select another route without saving?", "Unsaved route", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            //    if (result == DialogResult.No)
            //    {
            //        toolStripComboBoxRouteSelection.SelectedIndex = _currentRouteIndex;
            //        return;
            //    }
            //}

            ClearExplorationSet();
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

                SystemClass sys = SystemClass.GetSystem(sysname);

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

        private void dataGridViewRouteSystems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var textbox = (TextBox)e.Control;
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
                var centerFormat = new StringFormat()
                {
                    // right alignment might actually make more sense for numbers
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);

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
            if (MessageBox.Show(_discoveryForm, "Are you sure you want to delete this route?", "Delete Route", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (DeleteIsPermanent)
                {
                    _currentExplorationSet.Delete();
                }
                else
                {
                    _currentExplorationSet.Name = "\x7F" + _currentExplorationSet.Name;
                    _currentExplorationSet.Update();
                }

                _savedExplorationSets.Remove(_currentExplorationSet);

                ClearExplorationSet();
            }
        }
        
        private void toolStripButtonImportFile_Click(object sender, EventArgs e)
        {
            ExplorationSetClass newroute = new ExplorationSetClass();
            UpdateExplorationInfo(newroute);
            if (!newroute.Equals(_currentExplorationSet))
            {
                var result = MessageBox.Show(_discoveryForm, "There are unsaved changes to the current route.\r\n"
                    + "Are you sure you want to import a route without saving?", "Unsaved route", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.No)
                    return;
            }

            ClearExplorationSet();


            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files|*.txt";
            ofd.Title = "Select a exploration set file";

            if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            string[] sysnames;

            try
            {
                sysnames = System.IO.File.ReadAllLines(ofd.FileName);
            }
            catch (IOException)
            {
                MessageBox.Show(String.Format("There has been an error openning file {0}", ofd.FileName), "Import file",
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
                SystemClass sc = GetSystem(sysname.Trim());
                if (sc == null)
                {
                    sc = new SystemClass(sysname.Trim());
                    countunknown++;
                }
                systems.Add(sc.name);

            }
            if (systems.Count == 0)
            {
                MessageBox.Show(_discoveryForm,
                String.Format("There are no known system names in the file import", countunknown),
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
                    MessageBox.Show(_discoveryForm,
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

                if (dlg.ShowDialog() != DialogResult.OK)
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
                MessageBox.Show(String.Format("Export complete {0}", filename), "Export route");
            }
            catch (IOException)
            {
                MessageBox.Show(String.Format("Is file {0} open?", filename), "Export route",
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
            RoutingUtils.setTargetSystem(_discoveryForm, (string)obj);
        }

        private void editBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridViewExplore.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridViewExplore.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();

            if (selectedRows.Length == 0)
                return;
            var obj = dataGridViewExplore[0, selectedRows[0]].Value;

            if (obj == null)
                return;
            SystemClass sc = SystemClass.GetSystem((string)obj);
            if (sc == null) {
                MessageBox.Show("Unknown system, system is without co-ordinates", "Edit bookmark", MessageBoxButtons.OK);
                return;
            }
            RoutingUtils.showBookmarkForm(_discoveryForm, sc, null, false);
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
        public void Update()
        {

        }

    }
}
