using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using System.Threading;
using EMK.LightGeometry;
using EliteDangerousCore.EDSM;
using System.Diagnostics;

namespace EDDiscovery.UserControls
{
    public partial class UserControlRoute : UserControlCommonBase
    {
        private string DbSave(string s) { return DBName("UCRoute" ,  "_" + s); }

        private List<ISystem> routeSystems; // only valid systems get passed back
        private bool changesilence;

        private System.Windows.Forms.Timer fromupdatetimer;
        private System.Windows.Forms.Timer toupdatetimer;

        public UserControlRoute()
        {
            InitializeComponent();
            var corner = dataGridViewRoute.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            button_Route.Enabled = false;
            cmd3DMap.Enabled = false;

            fromupdatetimer = new System.Windows.Forms.Timer();
            toupdatetimer = new System.Windows.Forms.Timer();

            fromupdatetimer.Interval = 500;
            fromupdatetimer.Tick += FromUpdateTick;
            toupdatetimer.Interval = 500;
            toupdatetimer.Tick += ToUpdateTick;

            for (int i = 0; i < RoutePlotter.metric_options.Length; i++)
                comboBoxRoutingMetric.Items.Add(RoutePlotter.metric_options[i]);

            textBox_From.SetAutoCompletor(SystemClassDB.ReturnSystemListForAutoComplete);
            textBox_To.SetAutoCompletor(SystemClassDB.ReturnSystemListForAutoComplete);

            textBox_From.Text = SQLiteDBClass.GetSettingString(DbSave("RouteFrom"), "");
            textBox_To.Text = SQLiteDBClass.GetSettingString(DbSave("RouteTo"), "");
            //Console.WriteLine("Load {0} {1}", textBox_From.Text, textBox_To.Text);
            textBox_Range.Text = SQLiteDBClass.GetSettingString(DbSave("RouteRange"), "30");
            if (textBox_Range.Text == "")
                textBox_Range.Text = "30";
            textBox_FromX.Text = SQLiteDBClass.GetSettingString(DbSave("RouteFromX"), "");
            textBox_FromY.Text = SQLiteDBClass.GetSettingString(DbSave("RouteFromY"), "");
            textBox_FromZ.Text = SQLiteDBClass.GetSettingString(DbSave("RouteFromZ"), "");
            textBox_ToX.Text = SQLiteDBClass.GetSettingString(DbSave("RouteToX"), "");
            textBox_ToY.Text = SQLiteDBClass.GetSettingString(DbSave("RouteToY"), "");
            textBox_ToZ.Text = SQLiteDBClass.GetSettingString(DbSave("RouteToZ"), "");
            bool fromstate = SQLiteDBClass.GetSettingBool(DbSave("RouteFromState"), false);
            bool tostate = SQLiteDBClass.GetSettingBool(DbSave("RouteToState"), false);

            int metricvalue = SQLiteDBClass.GetSettingInt(DbSave("RouteMetric"), 0);
            comboBoxRoutingMetric.SelectedIndex = (metricvalue >= 0 && metricvalue < comboBoxRoutingMetric.Items.Count) ? metricvalue : SystemClassDB.metric_waypointdev2;

            SelectToMaster(tostate);
            UpdateTo(true);
            SelectFromMaster(fromstate);
            UpdateFrom(true);
            textBox_Range.ReadOnly = false;
            comboBoxRoutingMetric.Enabled = true;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg = thc;
        }

        public override void Closing()
        {
            SQLiteDBClass.PutSettingString(DbSave("RouteFrom"), textBox_From.Text);
            SQLiteDBClass.PutSettingString(DbSave("RouteTo"), textBox_To.Text);
            SQLiteDBClass.PutSettingString(DbSave("RouteRange"), textBox_Range.Text);
            SQLiteDBClass.PutSettingString(DbSave("RouteFromX"), textBox_FromX.Text);
            SQLiteDBClass.PutSettingString(DbSave("RouteFromY"), textBox_FromY.Text);
            SQLiteDBClass.PutSettingString(DbSave("RouteFromZ"), textBox_FromZ.Text);
            SQLiteDBClass.PutSettingString(DbSave("RouteToX"), textBox_ToX.Text);
            SQLiteDBClass.PutSettingString(DbSave("RouteToY"), textBox_ToY.Text);
            SQLiteDBClass.PutSettingString(DbSave("RouteToZ"), textBox_ToZ.Text);
            SQLiteDBClass.PutSettingBool(DbSave("RouteFromState"), textBox_From.ReadOnly);
            SQLiteDBClass.PutSettingBool(DbSave("RouteToState"), textBox_To.ReadOnly);
            SQLiteDBClass.PutSettingInt(DbSave("RouteMetric"), comboBoxRoutingMetric.SelectedIndex);
        }

        private void ToggleButtons(bool state)
        {
            button_Route.Enabled = state;
            cmd3DMap.Enabled = state;
        }

        string SystemNameOnly(string s)             // removes @ at end.
        {
            int atpos = s?.IndexOf('@') ?? -1;
            if (s != null && atpos != -1)
                s = s.Substring(0, atpos);
            s = s?.Trim();
            return s;
        }

        private void textBox_Clicked(object sender, EventArgs e)
        {
            ((ExtendedControls.TextBoxBorder)sender).SelectAll(); // clicking highlights everything
        }

        #region router
        private Thread ThreadRoute;

        private RoutePlotter CreateRoutePlotter()
        {
            RoutePlotter p = new RoutePlotter();
            string maxrangetext = textBox_Range.Text;
            if (!float.TryParse(maxrangetext, out p.maxrange)) p.maxrange = 30;
            p.usingcoordsfrom = textBox_From.ReadOnly == true;
            p.usingcoordsto = textBox_To.ReadOnly == true;
            GetCoordsFrom(out p.coordsfrom);                      // will be valid for a system or a co-ords box
            GetCoordsTo(out p.coordsto);
            p.fromsys = textBox_From.Text;
            p.tosys = textBox_To.Text;
            p.routemethod = comboBoxRoutingMetric.SelectedIndex;

            if (p.usingcoordsfrom)
                p.fromsys = "START POINT";
            if (p.usingcoordsto)
                p.tosys = "END POINT";

            p.possiblejumps = (int)(Point3D.DistanceBetween(p.coordsfrom, p.coordsto) / p.maxrange);

            return p;
        }

        private void RouteMain(object _plotter)
        {
            RoutePlotter p = (RoutePlotter)_plotter;

            routeSystems = null;    // so its null until route interative finishes

            routeSystems = p.RouteIterative(AppendData);

            this.BeginInvoke(new Action(() => { discoveryform.NewCalculatedRoute(routeSystems); ToggleButtons(true); }));
        }

        private void AppendData(RoutePlotter.ReturnInfo info)
        {
            BeginInvoke((MethodInvoker)delegate
            {
                DataGridViewRow rw = dataGridViewRoute.RowTemplate.Clone() as DataGridViewRow;
                rw.CreateCells(dataGridViewRoute, 
                        info.name, 
                        double.IsNaN(info.dist) ? "" : info.dist.ToString("N2"),
                        info.pos == null ? "" : info.pos.X.ToString("0.00"),
                        info.pos == null ? "" : info.pos.Y.ToString("0.00"),
                        info.pos == null ? "" : info.pos.Z.ToString("0.00"),
                        double.IsNaN(info.waypointdist) ? "" : info.waypointdist.ToString("0.0"),
                        double.IsNaN(info.deviation) ? "" : info.deviation.ToString("0.0")
                        );

                rw.Tag = info.system;       // may be null if waypoint or not a system
                rw.HeaderCell.Value = info.pos != null ? (dataGridViewRoute.Rows.Count + 1).ToStringInvariant() : "-";
                dataGridViewRoute.Rows.Add(rw);
            });
        }

        #endregion

        #region Helpers

        private bool IsValid()                          // have we star names or co-ords ready to go
        {
            bool readytocalc = true;
            Point3D pos;

            if (textBox_From.ReadOnly == false)          // if enabled, we are doing star names
            {
                if (discoveryform.history.FindSystem(SystemNameOnly(textBox_From.Text), discoveryform.galacticMapping) == null)
                    readytocalc = false;
            }
            else // check co-ords
            {
                if (!GetCoordsFrom(out pos))
                    readytocalc = false;
            }
            if (textBox_To.ReadOnly == false)          // if enabled, we are doing star names
            {
                if (discoveryform.history.FindSystem(SystemNameOnly(textBox_To.Text), discoveryform.galacticMapping) == null)
                    readytocalc = false;
            }
            else // check co-ords
            {
                if (!GetCoordsTo(out pos))
                    readytocalc = false;
            }

            if (comboBoxRoutingMetric.SelectedIndex < 0)
                readytocalc = false;

            return readytocalc;
        }

        private void UpdateDistance()
        {
            Point3D from, to;
            string dist = "";
            if (GetCoordsFrom(out from) && GetCoordsTo(out to))
            {
                dist = Point3D.DistanceBetween(from, to).ToString("0.00");
            }

            textBox_Distance.Text = dist;
        }


        #endregion

        #region UI

        private void button_Route_Click(object sender, EventArgs e)
        {
            ToggleButtons(false);           // beware the tab order, this moves the focus onto the next control, which in this dialog can be not what we want.
            RoutePlotter plotter = CreateRoutePlotter();

            if (plotter.possiblejumps > 100)
            {
                DialogResult res = ExtendedControls.MessageBoxTheme.Show(FindForm(), 
                    string.Format(("This will result in a large number ({0})) of jumps" + Environment.NewLine + "Confirm please").Tx(this,"Confirm"), 
                    plotter.possiblejumps), "Warning".Tx(), MessageBoxButtons.YesNo);
                if (res != System.Windows.Forms.DialogResult.Yes)
                {
                    ToggleButtons(true);
                    return;
                }
            }

            dataGridViewRoute.Rows.Clear();
            ThreadRoute = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RouteMain));
            ThreadRoute.Name = "Thread Route";
            ThreadRoute.Start(plotter);
        }

        private void cmd3DMap_Click(object sender, EventArgs e)
        {
            var map = discoveryform.Map;

            if (routeSystems != null && routeSystems.Any())
            {
                float dist;
                if (!float.TryParse(textBox_Distance.Text, out dist))       // in case text is crap
                    dist = 30;

                discoveryform.history.FillInPositionsFSDJumps();
                map.Prepare(routeSystems.First(), EDDConfig.Instance.HomeSystem, routeSystems.First(), 400 / dist, discoveryform.history.FilterByTravel);
                map.SetPlanned(routeSystems);
                map.Show();
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No route set up, retry".Tx(this,"NoRoute"), "Warning".Tx(), MessageBoxButtons.OK);
                return;
            }
        }

        private void comboBoxRoutingMetric_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_Route.Enabled = IsValid();
        }

        private void textBox_Range_KeyPress(object sender, KeyPressEventArgs e)
        {
            ExtendedControls.TextBoxBorder tbb = sender as ExtendedControls.TextBoxBorder;
            tbb.NumericKeyPressHandler(e);
        }

        #endregion

        #region From

        private void SelectFromMaster(bool coords)
        {
            textBox_From.ReadOnly = coords;
            textBox_FromX.ReadOnly = !coords;
            textBox_FromY.ReadOnly = !coords;
            textBox_FromZ.ReadOnly = !coords;
        }

        private bool GetCoordsFrom(out Point3D pos)
        {
            double x = 0, y = 0, z = 0;

            bool worked = System.Double.TryParse(textBox_FromX.Text, out x) &&
                            System.Double.TryParse(textBox_FromY.Text, out y) &&
                            System.Double.TryParse(textBox_FromZ.Text, out z);
            pos = new Point3D(x, y, z);
            return worked;
        }

        private void UpdateFrom(bool updatename)
        {
            changesilence = true;

            if (textBox_From.ReadOnly == false)                // if entering system name..
            {
                ISystem ds1 = discoveryform.history.FindSystem(SystemNameOnly(textBox_From.Text), discoveryform.galacticMapping);

                if (ds1 != null)
                {
                    if (updatename)                          // can't fix it as you type.. so leave alone
                    {
                        if (ds1 is GalacticMapSystem)
                        {
                            GalacticMapSystem gms = (GalacticMapSystem)ds1;
                            textBox_From.Text = gms.GalMapObject.name;
                        }
                        else
                        {
                            textBox_From.Text = ds1.Name;
                        }
                    }

                    textBox_FromName.Text = ds1.Name;
                    textBox_FromX.Text = ds1.X.ToString("0.00");
                    textBox_FromY.Text = ds1.Y.ToString("0.00");
                    textBox_FromZ.Text = ds1.Z.ToString("0.00");
                }
                else
                    textBox_FromX.Text = textBox_FromY.Text = textBox_FromZ.Text = "";
            }
            else
            {
                string res = "";
                Point3D curpos;
                if (GetCoordsFrom(out curpos))
                {
                    ISystem nearest = SystemClassDB.FindNearestSystemTo(curpos.X, curpos.Y, curpos.Z);

                    if (nearest != null)
                    {
                        double distance = Point3D.DistanceBetween(curpos, new Point3D(nearest.X, nearest.Y, nearest.Z));
                        if (distance < 0.1)
                            res = nearest.Name;
                        else
                            res = nearest.Name + " @ " + distance.ToString("0.00") + "ly";
                    }
                }

                textBox_From.Text = res;
                textBox_FromName.Text = res;
            }

            UpdateDistance();

            changesilence = false;
            button_Route.Enabled = IsValid();
        }

        void FromUpdateTick(object sender, EventArgs e)
        {
            fromupdatetimer.Stop();
            UpdateFrom(false);
        }


        private void textBox_From_Enter(object sender, EventArgs e)
        {
            SelectFromMaster(false);                              // enable system box
            if (fromupdatetimer != null)  // for the designer
            {
                fromupdatetimer.Stop();
                fromupdatetimer.Start();
            }
        }

        private void textBox_FromXYZ_Enter(object sender, EventArgs e)
        {
            SelectFromMaster(true);                       // coords master
            fromupdatetimer.Stop();
            fromupdatetimer.Start();
            ((ExtendedControls.TextBoxBorder)sender).Select(0, 1000); // entering selects everything
        }

        private void textBox_From_TextChanged(object sender, EventArgs e)
        {
            if (!changesilence)
            {
                fromupdatetimer.Stop();
                fromupdatetimer.Start();
            }
        }

        private void textBox_FromXYZ_TextChanged(object sender, EventArgs e)
        {
            if (!changesilence)
            {
                fromupdatetimer.Stop();
                fromupdatetimer.Start();
            }
        }

        private void buttonExtTravelFrom_Click(object sender, EventArgs e)
        {
            if (uctg.GetCurrentHistoryEntry != null)
            {
                SelectFromMaster(false);                              // enable system box
                textBox_From.Text = uctg.GetCurrentHistoryEntry.System.Name;
                UpdateFrom(false);
            }
        }

        private void buttonTargetFrom_Click(object sender, EventArgs e)
        {
            string name;
            double x, y, z;

            if (TargetClass.GetTargetPosition(out name, out x, out y, out z))
            {
                SelectFromMaster(false);                              // enable system box
                textBox_From.Text = TargetClass.GetNameWithoutPrefix(name);
                UpdateFrom(false);
            }
        }

        private void buttonFromEDSM_Click(object sender, EventArgs e)
        {
            ISystem ds1 = discoveryform.history.FindSystem(SystemNameOnly(textBox_To.Text), discoveryform.galacticMapping);
            string sysname = ds1?.Name ?? SystemNameOnly(textBox_To.Text);
            long? edsmid = ds1?.EDSMID;

            EDSMClass edsm = new EDSMClass();
            string url = edsm.GetUrlToEDSMSystem(sysname, edsmid);

            if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                Process.Start(url);
            else
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System unknown to EDSM");
        }


        #endregion

        #region To

        private void SelectToMaster(bool coords)
        {
            textBox_To.ReadOnly = coords;
            textBox_ToX.ReadOnly = !coords;
            textBox_ToY.ReadOnly = !coords;
            textBox_ToZ.ReadOnly = !coords;
        }

        public bool GetCoordsTo(out Point3D pos)
        {
            double x = 0, y = 0, z = 0;

            bool worked = System.Double.TryParse(textBox_ToX.Text, out x) &&
                            System.Double.TryParse(textBox_ToY.Text, out y) &&
                            System.Double.TryParse(textBox_ToZ.Text, out z);
            pos = new Point3D(x, y, z);
            return worked;
        }


        private void UpdateTo(bool updatename)
        {
            changesilence = true;

            if (textBox_To.ReadOnly == false)                // if entering system name..
            {
                ISystem ds1 = discoveryform.history.FindSystem(SystemNameOnly(textBox_To.Text), discoveryform.galacticMapping);

                if (ds1 != null)
                {
                    if (updatename)                          // can't fix it as you type.. so leave alone
                    {
                        if (ds1 is GalacticMapSystem)
                        {
                            GalacticMapSystem gms = (GalacticMapSystem)ds1;
                            textBox_To.Text = gms.GalMapObject.name;
                        }
                        else
                        {
                            textBox_To.Text = ds1.Name;
                        }
                    }

                    textBox_ToName.Text = ds1.Name;
                    textBox_ToX.Text = ds1.X.ToString("0.00");
                    textBox_ToY.Text = ds1.Y.ToString("0.00");
                    textBox_ToZ.Text = ds1.Z.ToString("0.00");
                }
                else
                    textBox_ToX.Text = textBox_ToY.Text = textBox_ToZ.Text = "";
            }
            else // Co-ords..
            {
                string res = "";
                Point3D curpos;
                if (GetCoordsTo(out curpos))
                {
                    ISystem nearest = SystemClassDB.FindNearestSystemTo(curpos.X, curpos.Y, curpos.Z);

                    if (nearest != null)
                    {
                        double distance = Point3D.DistanceBetween(curpos, new Point3D(nearest.X, nearest.Y, nearest.Z));
                        if (distance < 0.1)
                            res = nearest.Name;
                        else
                            res = nearest.Name + " @ " + distance.ToString("0.00") + "ly";
                    }
                }

                textBox_To.Text = res;
                textBox_ToName.Text = res;
            }

            UpdateDistance();

            changesilence = false;
            button_Route.Enabled = IsValid();
        }

        void ToUpdateTick(object sender, EventArgs e)
        {
            toupdatetimer.Stop();
            UpdateTo(false);
        }

        private void textBox_To_Enter(object sender, EventArgs e)   // To has been tabbed/clicked..
        {
            SelectToMaster(false);                              // enable system box

            if (toupdatetimer != null)  // for the designer
            {
                toupdatetimer.Stop();
                toupdatetimer.Start();
            }
        }

        private void textBox_ToXYZ_Enter(object sender, EventArgs e)
        {
            SelectToMaster(true);                       // coords master
            toupdatetimer.Stop();
            toupdatetimer.Start();
            ((ExtendedControls.TextBoxBorder)sender).Select(0, 1000); // clicking highlights everything
        }

        private void textBox_To_TextChanged(object sender, EventArgs e)
        {
            if (!changesilence)
            {
                toupdatetimer.Stop();
                toupdatetimer.Start();
            }
        }

        private void textBox_ToXYZ_TextChanged(object sender, EventArgs e)
        {
            if (!changesilence)
            {
                toupdatetimer.Stop();
                toupdatetimer.Start();
            }
        }

        private void buttonExtTravelTo_Click(object sender, EventArgs e)
        {
            if (uctg.GetCurrentHistoryEntry != null)
            {
                SelectToMaster(false);                              // enable system box
                textBox_To.Text = uctg.GetCurrentHistoryEntry.System.Name;
                UpdateTo(false);
            }
        }

        private void buttonTargetTo_Click(object sender, EventArgs e)
        {
            string name;
            double x, y, z;

            if (TargetClass.GetTargetPosition(out name, out x, out y, out z))
            {
                SelectToMaster(false);                              // enable system box
                textBox_To.Text = TargetClass.GetNameWithoutPrefix(name);
                UpdateTo(false);
            }
        }

        private void buttonToEDSM_Click(object sender, EventArgs e)
        {
            ISystem ds1 = discoveryform.history.FindSystem(SystemNameOnly(textBox_To.Text), discoveryform.galacticMapping);
            string sysname = ds1?.Name ?? SystemNameOnly(textBox_To.Text);
            long? edsmid = ds1?.EDSMID;

            EDSMClass edsm = new EDSMClass();
            string url = edsm.GetUrlToEDSMSystem(sysname, edsmid);

            if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                Process.Start(url);
            else
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System unknown to EDSM");

        }

        #endregion

        #region Excel

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            if ( dataGridViewRoute.Rows.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No Route Plotted", "Route", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(new string[] { "All" }, disablestartendtime: true);

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                grd.SetCSVDelimiter(frm.Comma);

                grd.GetLineStatus += delegate (int r)
                {
                    if (r < dataGridViewRoute.Rows.Count)
                        return BaseUtils.CSVWriteGrid.LineStatus.OK;
                    else
                        return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                };

                grd.GetLine += delegate (int r)
                {
                    DataGridViewRow rw = dataGridViewRoute.Rows[r];

                    return new Object[] { rw.Cells[0].Value,rw.Cells[1].Value,
                                          rw.Cells[2].Value,rw.Cells[3].Value,rw.Cells[4].Value,
                                          rw.Cells[5].Value,rw.Cells[6].Value };
                };

                grd.GetHeader += delegate (int c)
                {
                    return (c < dataGridViewRoute.Columns.Count) ? dataGridViewRoute.Columns[c].HeaderText : null;
                };


                if (grd.WriteCSV(frm.Path))
                {
                    if (frm.AutoOpen)
                        System.Diagnostics.Process.Start(frm.Path);
                }
                else
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to write to " + frm.Path, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataGridViewRoute_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridViewRoute.CurrentCell;
            if (cell != null)
            {
                string s = (string)cell.Value;
                SetClipboardText(s);
            }
        }

        int rightclickrow = -1;

        private void dataGridViewRoute_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                rightclickrow = -1;
            }

            if (dataGridViewRoute.SelectedCells.Count < 2 || dataGridViewRoute.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
            {
                DataGridView.HitTestInfo hti = dataGridViewRoute.HitTest(e.X, e.Y);
                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    dataGridViewRoute.ClearSelection();                // select row under cursor.
                    dataGridViewRoute.Rows[hti.RowIndex].Selected = true;

                    if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
                    {
                        rightclickrow = hti.RowIndex;
                    }
                }
            }

            showInEDSMToolStripMenuItem.Enabled = rightclickrow != -1 && dataGridViewRoute.Rows[rightclickrow].Tag != null;
        }
                                                                    
        private void showInEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISystem sys = dataGridViewRoute.Rows[rightclickrow].Tag as ISystem;

            if (sys!=null) // paranoia because it should not be enabled otherwise
            {
                this.Cursor = Cursors.WaitCursor;

                EliteDangerousCore.EDSM.EDSMClass edsm = new EDSMClass();
                long? id_edsm = sys.EDSMID;

                if (id_edsm <= 0)
                {
                    id_edsm = null;
                }

                if (!edsm.ShowSystemInEDSM(sys.Name, id_edsm))
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable");

                this.Cursor = Cursors.Default;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewRoute.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {
                    Clipboard.SetDataObject(dataGridViewRoute.GetClipboardContent());
                }
                catch { }
            }
        }

        #endregion
    }
}
