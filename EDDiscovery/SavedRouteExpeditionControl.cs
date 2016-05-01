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
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            var db = new SQLiteDBClass();
            _savedRoutes = db.GetAllSavedRoutes();
            UpdateComboBox();
            _currentRoute = new SavedRouteClass();
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

                        string strdist = ((double)dist).ToString("0.00");
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
                if (_currentRoute.Name != null)
                {
                    var result = MessageBox.Show(_discoveryForm, "There are unsaved changes to the current route.\r\nAre you sure you want to select another route without saving?", "Unsaved route", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.No)
                    {
                        toolStripComboBoxRouteSelection.SelectedIndex = _currentRouteIndex;
                    }
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
            _currentRoute = new SavedRouteClass();
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

            map.Instance.SystemNames = _discoveryForm.SystemNames;
            map.Instance.VisitedSystems = _discoveryForm.VisitedSystems;

            var route = dataGridViewRouteSystems.Rows.OfType<DataGridViewRow>().Select(s => s.Cells[0].Tag as SystemClass).Where(s => s != null && s.HasCoordinate).ToList();

            if (route.Count >= 2)
            {
                float zoom = 400 / CalculateRouteMaxDistFromOrigin();
                if (zoom < 0.01) zoom = 0.01f;
                if (zoom > 50) zoom = 50f;
                map.ShowPlanned(route[0].name, _discoveryForm.settings.MapHomeSystem, route[0].name, zoom, route);
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
                _dragRowIndex = dataGridViewRouteSystems.HitTest(e.X, e.Y).RowIndex;

                if (_dragRowIndex != -1)
                {
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
        }

        private void dataGridViewRouteSystems_DragDrop(object sender, DragEventArgs e)
        {
            Point p = dataGridViewRouteSystems.PointToClient(new Point(e.X, e.Y));
            var insertIndex = dataGridViewRouteSystems.HitTest(p.X, p.Y).RowIndex;
            if (insertIndex >= dataGridViewRouteSystems.Rows.Count)
            {
                insertIndex = dataGridViewRouteSystems.Rows.Count - 1;
            }

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
    }
}
