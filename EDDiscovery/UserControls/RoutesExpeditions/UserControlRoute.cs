/*
 * Copyright © 2019 EDDiscovery development team
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
using EMK.LightGeometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlRoute : UserControlCommonBase
    {
        private string DbSave(string s) { return DBName("UCRoute" ,  "_" + s); }

        private List<ISystem> routeSystems; // only valid systems get passed back
        private bool changesilence;

        private System.Windows.Forms.Timer fromupdatetimer;
        private System.Windows.Forms.Timer toupdatetimer;
        private ManualResetEvent CloseRequested = new ManualResetEvent(false);

        private static readonly string[] MetricNames = {        // synchronise with SystemsDB.SystemsNearestMetric, really should be translated, but there you go.
            "Nearest to Waypoint",
            "Minimum Deviation from Path",
            "Nearest to Waypoint with dev<=100ly",
            "Nearest to Waypoint with dev<=250ly",
            "Nearest to Waypoint with dev<=500ly",
            "Nearest to Waypoint + Deviation / 2",
        };

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

            foreach (SystemsDB.SystemsNearestMetric values in Enum.GetValues(typeof(SystemsDB.SystemsNearestMetric)))
                comboBoxRoutingMetric.Items.Insert((int)values, MetricNames[(int)values]);

            textBox_From.SetAutoCompletor(SystemCache.ReturnSystemAdditionalListForAutoComplete, true);
            textBox_To.SetAutoCompletor(SystemCache.ReturnSystemAdditionalListForAutoComplete , true);

            textBox_From.Text = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbSave("RouteFrom"), "");
            textBox_To.Text = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbSave("RouteTo"), "");
            textBox_Range.Value = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbSave("RouteRange"), 30);
            textBox_FromX.Text = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbSave("RouteFromX"), "");
            textBox_FromY.Text = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbSave("RouteFromY"), "");
            textBox_FromZ.Text = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbSave("RouteFromZ"), "");
            textBox_ToX.Text = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbSave("RouteToX"), "");
            textBox_ToY.Text = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbSave("RouteToY"), "");
            textBox_ToZ.Text = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbSave("RouteToZ"), "");
            bool fromstate = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave("RouteFromState"), false);
            bool tostate = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave("RouteToState"), false);

            int metricvalue = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbSave("RouteMetric"), 0);
            comboBoxRoutingMetric.SelectedIndex = Enum.IsDefined(typeof(SystemsDB.SystemsNearestMetric), metricvalue)
                ? metricvalue
                : (int) SystemsDB.SystemsNearestMetric.IterativeNearestWaypoint;

            SeleteToCoords(tostate);
            UpdateTo(true);
            SelectFromCoords(fromstate);
            UpdateFrom(true);
            comboBoxRoutingMetric.Enabled = true;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            discoveryform.OnHistoryChange += HistoryChanged;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg = thc;
        }

        public override void Closing()
        {
            if (routingthread != null && routingthread.IsAlive && plotter != null)
            {
                plotter.StopPlotter = true;
                CloseRequested.Set();
                routingthread.Join();
            }

            discoveryform.OnHistoryChange -= HistoryChanged;

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbSave("RouteFrom"), textBox_From.Text);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbSave("RouteTo"), textBox_To.Text);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DbSave("RouteRange"), (int)textBox_Range.Value);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbSave("RouteFromX"), textBox_FromX.Text);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbSave("RouteFromY"), textBox_FromY.Text);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbSave("RouteFromZ"), textBox_FromZ.Text);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbSave("RouteToX"), textBox_ToX.Text);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbSave("RouteToY"), textBox_ToY.Text);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbSave("RouteToZ"), textBox_ToZ.Text);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave("RouteFromState"), textBox_From.ReadOnly);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave("RouteToState"), textBox_To.ReadOnly);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DbSave("RouteMetric"), comboBoxRoutingMetric.SelectedIndex);
        }

        public void HistoryChanged(HistoryList hl)           // on History change, we now have history systems to look up, so make sure the To/From get a chance to update
        {
            if (hl != null && hl.Count > 0)
            {
                UpdateTo(true);
                UpdateFrom(true);
            }
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
            ((ExtendedControls.ExtTextBox)sender).SelectAll(); // clicking highlights everything
        }


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

        RoutePlotter plotter = null;

        private void button_Route_Click(object sender, EventArgs e)
        {
            if (routingthread == null  || !routingthread.IsAlive)
            {
                plotter = new RoutePlotter();
                plotter.MaxRange = textBox_Range.Value;
                GetCoordsFrom(out plotter.Coordsfrom);                      // will be valid for a system or a co-ords box
                GetCoordsTo(out plotter.Coordsto);
                plotter.FromSystem = textBox_From.Text;
                plotter.ToSystem = textBox_To.Text;
                plotter.RouteMethod = (SystemsDB.SystemsNearestMetric) comboBoxRoutingMetric.SelectedIndex;
                plotter.UseFsdBoost = checkBox_FsdBoost.Checked;

                if (textBox_From.ReadOnly == true)
                    plotter.FromSystem = "START POINT";
                if (textBox_To.ReadOnly == true)
                    plotter.ToSystem = "END POINT";

                int PossibleJumps = (int)(Point3D.DistanceBetween(plotter.Coordsfrom, plotter.Coordsto) / plotter.MaxRange);

                if (PossibleJumps > 100)
                {
                    DialogResult res = ExtendedControls.MessageBoxTheme.Show(FindForm(),
                        string.Format(("This will result in a large number ({0}) of jumps" + Environment.NewLine + "Confirm please").T(EDTx.UserControlRoute_Confirm),
                        PossibleJumps), "Warning".T(EDTx.Warning), MessageBoxButtons.YesNo);
                    if (res != System.Windows.Forms.DialogResult.Yes)
                    {
                        return;
                    }
                }

                dataGridViewRoute.Rows.Clear();
                routingthread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RoutingThread));
                routingthread.Name = "Thread Route";

                cmd3DMap.Enabled = false;
                button_Route.Text = "Cancel".T(EDTx.Cancel);
                routingthread.Start(plotter);
            }
            else
            {
                plotter.StopPlotter = true;
                button_Route.Enabled = false;
            }
        }

        private Thread routingthread;

        private void RoutingThread(object _plotter)
        {
            RoutePlotter p = (RoutePlotter)_plotter;

            routeSystems = null;    // so its null until route interative finishes

            routeSystems = p.RouteIterative(AppendData);

            this.BeginInvoke(new Action(() => 
                {
                    discoveryform.NewCalculatedRoute(routeSystems);
                    cmd3DMap.Enabled = true;
                    button_Route.Text = "Find Route".Tx(this, "button_Route");
                    button_Route.Enabled = true;
                }));
        }

        private void AppendData(RoutePlotter.ReturnInfo info)   // IN thread context, need to invoke
        {
            var ar = BeginInvoke((MethodInvoker)delegate      // using Invoke blocks the thread until the UI finishes.  Using BeginInvoke async causes it to overload the UI
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
                rw.Cells[0].Tag = info.system?.Name;    // write the name of the system into the cells'tag for copying
                rw.HeaderCell.Value = info.pos != null ? (dataGridViewRoute.Rows.Count + 1).ToStringInvariant() : "-";
                dataGridViewRoute.Rows.Add(rw);
                if (!rw.Displayed)
                {
                    dataGridViewRoute.SafeFirstDisplayedScrollingRowIndex(dataGridViewRoute.FirstDisplayedScrollingRowIndex+1);
                }
            });

            WaitHandle.WaitAny(new WaitHandle[] { CloseRequested, ar.AsyncWaitHandle });
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
                map.Prepare(routeSystems.First(), EDCommander.Current.HomeSystemTextOrSol, routeSystems.First(), 400 / dist, discoveryform.history.FilterByTravel);
                map.SetPlanned(routeSystems);
                map.Show();
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No route set up, retry".T(EDTx.UserControlRoute_NoRoute), "Warning".T(EDTx.Warning), MessageBoxButtons.OK);
                return;
            }
        }

        private void comboBoxRoutingMetric_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_Route.Enabled = IsValid();
        }

        #endregion

        #region From

        private void SelectFromCoords(bool coords)
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
                    ISystem nearest = SystemCache.FindNearestSystemTo(curpos.X, curpos.Y, curpos.Z,100);

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
            SelectFromCoords(false);                              // enable system box
            if (fromupdatetimer != null)  // for the designer
            {
                fromupdatetimer.Stop();
                fromupdatetimer.Start();
            }
        }

        private void textBox_FromXYZ_Enter(object sender, EventArgs e)
        {
            SelectFromCoords(true);                       // coords master
            fromupdatetimer.Stop();
            fromupdatetimer.Start();
            ((ExtendedControls.ExtTextBox)sender).Select(0, 1000); // entering selects everything
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
                SelectFromCoords(false);                              // enable system box
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
                SelectFromCoords(false);                              // enable system box
                textBox_From.Text = TargetClass.GetNameWithoutPrefix(name);
                UpdateFrom(false);
            }
        }

        private void buttonFromEDSM_Click(object sender, EventArgs e)
        {
            ISystem ds1 = discoveryform.history.FindSystem(SystemNameOnly(textBox_From.Text), discoveryform.galacticMapping);
            string sysname = ds1?.Name ?? SystemNameOnly(textBox_From.Text);
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

        private void SeleteToCoords(bool coords)
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
                    ISystem nearest = SystemCache.FindNearestSystemTo(curpos.X, curpos.Y, curpos.Z,100);

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
            SeleteToCoords(false);                              // enable system box

            if (toupdatetimer != null)  // for the designer
            {
                toupdatetimer.Stop();
                toupdatetimer.Start();
            }
        }

        private void textBox_ToXYZ_Enter(object sender, EventArgs e)
        {
            SeleteToCoords(true);                       // coords master
            toupdatetimer.Stop();
            toupdatetimer.Start();
            ((ExtendedControls.ExtTextBox)sender).Select(0, 1000); // clicking highlights everything
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
                SeleteToCoords(false);                              // enable system box
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
                SeleteToCoords(false);                              // enable system box
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

                grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
            }
        }

        private void dataGridViewRoute_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridViewRoute.CurrentCell;
            if (cell != null)
            {
                // If a cell contains a tag (i.e. a system name), copy the string of the tag
                // else, copy whatever text is inside
                string s = "";
                if (cell.Tag != null)
                    s = cell.Tag.ToString();
                else
                    s = (string)cell.Value;
                SetClipboardText(s);
            }
        }

        
        private void dataGridViewRoute_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dataGridViewRoute.CurrentCell?.RowIndex ?? -1;
            if ( row >= 0 )
            {
                ISystem sys = dataGridViewRoute.Rows[row].Tag as ISystem;
                ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), sys, true, discoveryform.history);
            }
        }

        int rightclickrow = -1;

        private void dataGridViewRoute_MouseDown(object sender, MouseEventArgs e)
        {
            dataGridViewRoute.HandleClickOnDataGrid(e, out int unusedleftclickrow, out rightclickrow);

            showInEDSMToolStripMenuItem.Enabled = rightclickrow != -1 && dataGridViewRoute.Rows[rightclickrow].Tag != null;
            showScanToolStripMenuItem.Enabled = rightclickrow != -1 && dataGridViewRoute.Rows[rightclickrow].Tag != null;
            copyToolStripMenuItem.Enabled = dataGridViewRoute.GetCellCount(DataGridViewElementStates.Selected) > 0;
        }

        private void showInEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickrow >= 0)
            {
                ISystem sys = dataGridViewRoute.Rows[rightclickrow].Tag as ISystem;

                if (sys != null) // paranoia because it should not be enabled otherwise
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

        private void showScanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickrow >= 0)
            {
                ISystem sys = dataGridViewRoute.Rows[rightclickrow].Tag as ISystem;
                ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), sys, true, discoveryform.history);    // protected against sys = null
            }
        }


        #endregion

    }
}
