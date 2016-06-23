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

namespace EDDiscovery
{
    public partial class SavedRouteExpeditionControl : UserControl
    {
        public static SavedRouteClass[] InitialRoutes =
        {
            new SavedRouteClass(
                "Distant Worlds (3302)",
                "Pallaeni",
                "Fine Ring Sector JH-V c2-4",
                "NGC 6530 WFI 16706",
                "Omega Sector EL-Y d60",
                "Eagle Sector EL-Y d203",
                "NGC 6357 Sector DL-Y e22",
                "Blaa Hypai UC-G c12-6",
                "Greae Phio LS-L c23-221",
                "Speamoea WU-E d12-543",
                "Athaip CR-C b55-4",
                "Myriesly EC-B c27-381",
                "Stuemeae KM-W c1-342",
                "Nyuena JS-B d342",
                "Phipoea DD-F c26-1311",
                "Dryao Chrea VU-P e5-7481",
                "Eorld Byoe YA-W e2-4084",
                "Eok Gree TO-Q e5-3167",
                "Pheia Briae DK-A e303",
                "Greeroi MD-Q d6-5",
                "Eactainds GN-W c1-6",
                "Oupailks BB-M c8-5",
                "Qautheia BA-A e0",
                "Cheae Eurl AA-A e0",
                "Ceeckia ZQ-L c24-0"
            )
            {
                StartDate = new DateTime(2016, 1, 14, 20, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                EndDate = new DateTime(2016, 6, 1, 23, 59, 59, DateTimeKind.Utc).ToLocalTime(),
            },
            new SavedRouteClass(
                "Sagittarius-Carina Mission",
                "Sol",
                "NGC 6530 WFI 16706",
                "Syralia JT-V b7-0",
                "CSI-61-15434",
                "CPD-65 2513",
                "Thaile HW-V e2-7",
                "GCRV 6807",
                "Prue Hypa CL-Y g2",
                "Pueliae IT-H d10-1",
                "Grie Hypai DL-Y g2",
                "Eock Prau WD-T d3-6",
                "Mycapp TX-U d2-4",
                "Eembaitl DL-Y d13",
                "Hypaa Byio ZE-A g1",
                "Gooroa PT-Q e5-5",
                "Braitu EG-Y g1",
                "Suvaa NL-P d5-29",
                "Truechea SD-T d3-14",
                "Hyphielie GR-N d6-9",
                "Cho Eur QY-S e3-2",
                "Fleckia FI-Z d1-6",
                "Ceeckia ZQ-L c24-0",
                "Myeia Thaa ZE-R d4-0",
                "Syriae Thaa PJ-I d9-1",
                "Pyrie Eurk QX-U e2-0",
                "Cho Thua SF-W b29-0",
                "Pyriveae FK-C d14-72",
                "Tyroerts RX-U d2-0",
                "Eactaify GD-A d14-18",
                "Preia Flyuae XY-A e1865",
                "13 MU SAGITTARII",
                "Chraisa AY-F d12-133",
                "Dryio Bloo PZ-W d2-1161",
                "Stuemiae BB-O e6-61",
                "Hypua Flyoae WU-X e1-4448",
                "Oephaif RJ-G d11-408",
                "Froaln II-W b7-1",
                "Eodgorph IN-Z c14-32",
                "CSI-06-19031",
                "Omega Sector EL-Y d60",
                "HIP 72043"
            )
            {
                StartDate = new DateTime(2015, 8, 1, 19, 0, 0, DateTimeKind.Utc).ToLocalTime(),
                EndDate = new DateTime(2018, 4, 9, 23, 59, 59, DateTimeKind.Utc).ToLocalTime()
            }
        };

        private List<SavedRouteClass> _savedRoutes;
        private SavedRouteClass _currentRoute;
        private EDDiscoveryForm _discoveryForm;
        private EDSMClass edsm;
        private int _currentRouteIndex;
        private Rectangle _dragBox;
        private int _dragRowIndex;

        public SavedRouteExpeditionControl()
        {
            InitializeComponent();
            _currentRoute = new SavedRouteClass("");
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            var db = new SQLiteDBClass();
            _savedRoutes = db.GetAllSavedRoutes();

            foreach (var initroute in InitialRoutes)
            {
                if (!_savedRoutes.Any(r => r.Name == initroute.Name))
                {
                    initroute.Add();
                    _savedRoutes.Add(initroute);
                }
            }

            UpdateComboBox();
            _currentRoute = new SavedRouteClass("");
            edsm = new EDSMClass();
        }

        private void UpdateComboBox()
        {
            toolStripComboBoxRouteSelection.Items.Clear();

            foreach (var route in _savedRoutes)
            {
                toolStripComboBoxRouteSelection.Items.Add(route.Name);
            }
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

        private float CalculateRouteMaxDistFromOrigin()
        {
            if (dataGridViewRouteSystems.Rows.Count < 2)
                return 100;

            double maxdist = 25;
            var rows = dataGridViewRouteSystems.Rows.OfType<DataGridViewRow>().Where(r => r.Cells[0].Value != null).ToList();
            List<SystemClass> systems = rows.Select(r => r.Cells[0].Tag as SystemClass).ToList();
            SystemClass sys0 = systems.FirstOrDefault(s => s != null && s.HasCoordinate);

            if (sys0 != null)
            {
                foreach (var sys in systems.Where(s => s != null))
                {
                    double dist;
                    if (sys.HasCoordinate)
                    {
                        dist = SystemData.Distance(sys0, sys);
                    }
                    else
                    {
                        dist = DistanceClass.Distance(sys0, sys);
                    }

                    if (dist > maxdist)
                    {
                        maxdist = dist;
                    }
                }
            }

            return (float)maxdist;
        }

        private SystemClass GetSystem(string sysname)
        {
            SystemClass sys = SystemData.GetSystem(sysname);

            if (sys == null)
            {
                if (edsm.IsKnownSystem(sysname))
                {
                    sys = new SystemClass(sysname);
                }
            }

            return sys;
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
                        double dist;
                        if (sys.HasCoordinate && prevsys.HasCoordinate)
                        {
                            dist = SystemData.Distance(sys, prevsys);
                        }
                        else
                        {
                            dist = DistanceClass.Distance(sys, prevsys);
                        }

                        string strdist = dist >= 0 ? ((double)dist).ToString("0.00") : "";
                        dataGridViewRouteSystems[1, rowindex].Value = strdist;
                    }
                }

                dataGridViewRouteSystems[0, rowindex].Tag = sys;
                dataGridViewRouteSystems.Rows[rowindex].DefaultCellStyle.ForeColor = (sys != null && sys.HasCoordinate) ? _discoveryForm.theme.VisitedSystemColor : _discoveryForm.theme.NonVisitedSystemColor;

                if (sys != null)
                {
                    string sysnote = sys.Note == null ? "" : sys.Note;
                    dataGridViewRouteSystems[2, rowindex].Value = sysnote;
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
                UpdateSystemRow(i);
            }
        }

        private void UpdateRouteInfo(SavedRouteClass route)
        {
            route.Name = textBoxRouteName.Text.Trim();
            route.Systems.Clear();
            route.Systems.AddRange(dataGridViewRouteSystems.Rows.OfType<DataGridViewRow>().Where(r => r.Cells[0].Value != null).Select(r => r.Cells[0].Value.ToString()));

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
            SavedRouteClass newroute = new SavedRouteClass();
            UpdateRouteInfo(newroute);

            if (!newroute.Equals(_currentRoute))
            {
                var result = MessageBox.Show(_discoveryForm, "There are unsaved changes to the current route.\r\nAre you sure you want to select another route without saving?", "Unsaved route", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.No)
                {
                    toolStripComboBoxRouteSelection.SelectedIndex = _currentRouteIndex;
                    return;
                }
            }

            _currentRouteIndex = toolStripComboBoxRouteSelection.SelectedIndex;
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

            for (int i = 0; i < dataGridViewRouteSystems.Rows.Count; i++)
            {
                dataGridViewRouteSystems[1, i].ReadOnly = true;
                dataGridViewRouteSystems[2, i].ReadOnly = true;
                UpdateSystemRow(i);
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            UpdateRouteInfo(_currentRoute);

            if (_currentRoute.Id == -1)
            {
                _currentRoute.Add();
                _savedRoutes.Add(_currentRoute);
                UpdateComboBox();
            }
            else
            {
                _currentRoute.Update();
            }
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            SavedRouteClass newroute = new SavedRouteClass();
            UpdateRouteInfo(newroute);

            if (!newroute.Equals(_currentRoute))
            {
                var result = MessageBox.Show(_discoveryForm, "There are unsaved changes to the current route.\r\nAre you sure you want to select another route without saving?", "Unsaved route", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.No)
                {
                    toolStripComboBoxRouteSelection.SelectedIndex = _currentRouteIndex;
                    return;
                }
            }

            _currentRoute = new SavedRouteClass { Name = "" };
            dataGridViewRouteSystems.Rows.Clear();
        }

        private void toolStripButtonShowOn3DMap_Click(object sender, EventArgs e)
        {
            if (_discoveryForm.SystemNames.Count == 0)
            {
                MessageBox.Show("Systems have not been loaded yet, please wait", "No Systems Available", MessageBoxButtons.OK);
                return;
            }

            var map = _discoveryForm.Map;

            var route = dataGridViewRouteSystems.Rows.OfType<DataGridViewRow>().Select(s => s.Cells[0].Tag as SystemClass).Where(s => s != null && s.HasCoordinate).ToList();

            if (route.Count >= 2)
            {
                float zoom = 400 / CalculateRouteMaxDistFromOrigin();
                if (zoom < 0.01) zoom = 0.01f;
                if (zoom > 50) zoom = 50f;

                map.Prepare(route[0].name, _discoveryForm.settings.MapHomeSystem, route[0].name, zoom, _discoveryForm.SystemNames, _discoveryForm.VisitedSystems);
                map.SetPlanned(route);
                map.Show();
            }
            else
            {
                MessageBox.Show("No route set up, retry", "No Route", MessageBoxButtons.OK);
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

                SystemClass sys = SystemData.GetSystem(sysname);

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
                UpdateSystemRow(e.RowIndex);
                UpdateSystemRow(e.RowIndex + 1);
            }
        }

        private void dataGridViewRouteSystems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var textbox = (TextBox)e.Control;
            if (dataGridViewRouteSystems.CurrentCell.ColumnIndex != 0)
            {
                textbox.AutoCompleteMode = AutoCompleteMode.None;
                return;
            }

            textbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textbox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            var items = new AutoCompleteStringCollection();

            items.AddRange(SystemData.SystemList.OrderBy(s => s.name).Select(s => s.name).ToArray());

            textbox.AutoCompleteCustomSource = items;
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
                    dataGridViewRouteSystems.Rows.RemoveAt(index);
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
            int[] selectedRows = dataGridViewRouteSystems.SelectedCells.OfType<DataGridViewCell>().Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
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
    }
}
