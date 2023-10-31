/*
 * Copyright © 2019-2023 EDDiscovery development team
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
 */

using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.GMO;
using EMK.LightGeometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlRoute : UserControlCommonBase
    {
        private List<ISystem> routeSystems; // only valid systems get passed back
        private bool changesilence;

        private System.Windows.Forms.Timer fromupdatetimer;
        private System.Windows.Forms.Timer toupdatetimer;
        private ManualResetEvent CloseRequested = new ManualResetEvent(false);

        private HistoryEntry last_history_he = null;

        #region  Init

        public UserControlRoute()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "UCRoute";

            extButtonRoute.Enabled = false;
            buttonExtExcel.Enabled = extButtonExpeditionPush.Enabled = cmd3DMap.Enabled = false;

            fromupdatetimer = new System.Windows.Forms.Timer();
            toupdatetimer = new System.Windows.Forms.Timer();

            fromupdatetimer.Interval = 500;
            fromupdatetimer.Tick += FromUpdateTick;
            toupdatetimer.Interval = 500;
            toupdatetimer.Tick += ToUpdateTick;

            string[] MetricNames = {        // synchronise with SystemCache.SystemsNearestMetric, really should be translated, but there you go.
                "Nearest to Waypoint".T(EDTx.UserControlRoute_M1),
                "Minimum Deviation from Path".T(EDTx.UserControlRoute_M2),
                "Nearest to Waypoint with dev<=100ly".T(EDTx.UserControlRoute_M3),
                "Nearest to Waypoint with dev<=250ly".T(EDTx.UserControlRoute_M4),
                "Nearest to Waypoint with dev<=500ly".T(EDTx.UserControlRoute_M5),
                "Nearest to Waypoint + Deviation / 2".T(EDTx.UserControlRoute_M6),
                };

            foreach (SystemCache.SystemsNearestMetric values in Enum.GetValues(typeof(SystemCache.SystemsNearestMetric)))
                comboBoxRoutingMetric.Items.Insert((int)values, MetricNames[(int)values]);

            changesilence = true;

            textBox_From.SetAutoCompletor(SystemCache.ReturnSystemAdditionalListForAutoComplete, true);
            textBox_From.AutoCompleteTimeout = 500;
            textBox_To.SetAutoCompletor(SystemCache.ReturnSystemAdditionalListForAutoComplete , true);
            textBox_To.AutoCompleteTimeout = 500;

            textBox_From.Text = GetSetting("_RouteFrom", "");
            textBox_To.Text = GetSetting("_RouteTo", "");
            textBox_Range.Value = GetSetting("_RouteRange", 30);
            textBox_FromX.Text = GetSetting("_RouteFromX", "");
            textBox_FromY.Text = GetSetting("_RouteFromY", "");
            textBox_FromZ.Text = GetSetting("_RouteFromZ", "");
            textBox_ToX.Text = GetSetting("_RouteToX", "");
            textBox_ToY.Text = GetSetting("_RouteToY", "");
            textBox_ToZ.Text = GetSetting("_RouteToZ", "");

            int metricvalue = GetSetting("RouteMetric", 0);
            comboBoxRoutingMetric.SelectedIndex = Enum.IsDefined(typeof(SystemCache.SystemsNearestMetric), metricvalue)
                ? metricvalue
                : (int) SystemCache.SystemsNearestMetric.IterativeNearestWaypoint;

            UpdateDistance();
            extButtonRoute.Enabled = IsValid();

            changesilence = false;

            comboBoxRoutingMetric.Enabled = true;

            edsmSpanshButton.Init(this, "EDSMSpansh", "");

            var enumlist = new Enum[] { EDTx.UserControlRoute_SystemCol, EDTx.UserControlRoute_NoteCol, EDTx.UserControlRoute_DistanceCol, EDTx.UserControlRoute_StarClassCol, EDTx.UserControlRoute_WayPointDistCol,
                                        EDTx.UserControlRoute_DeviationCol,
                                        EDTx.UserControlRoute_checkBox_FsdBoost, EDTx.UserControlRoute_buttonExtTravelTo, EDTx.UserControlRoute_buttonExtTravelFrom,
                                        EDTx.UserControlRoute_buttonExtTargetTo,  EDTx.UserControlRoute_buttonTargetFrom,
                                        EDTx.UserControlRoute_cmd3DMap, EDTx.UserControlRoute_labelLy2, EDTx.UserControlRoute_labelLy1, EDTx.UserControlRoute_labelTo,
                                        EDTx.UserControlRoute_labelMaxJump, EDTx.UserControlRoute_labelDistance, EDTx.UserControlRoute_labelMetric,
                                        EDTx.UserControlRoute_extButtonRoute, EDTx.UserControlRoute_labelFrom,
                                        EDTx.UserControlRoute_groupBoxSpansh, EDTx.UserControlRoute_extButtonSpanshRoadToRiches, EDTx.UserControlRoute_groupBoxInternal,EDTx.UserControlRoute_groupBoxPara};
                                        
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);

            var enumlistcms = new Enum[] { EDTx.UserControlRoute_showInEDSMToolStripMenuItem, EDTx.UserControlRoute_copyToolStripMenuItem, EDTx.UserControlRoute_showScanToolStripMenuItem };
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);

            var enumlisttt = new Enum[] { EDTx.UserControlRoute_checkBox_FsdBoost_ToolTip, EDTx.UserControlRoute_buttonExtExcel_ToolTip, EDTx.UserControlRoute_textBox_ToName_ToolTip, 
                                        EDTx.UserControlRoute_textBox_FromName_ToolTip, EDTx.UserControlRoute_comboBoxRoutingMetric_ToolTip, EDTx.UserControlRoute_buttonExtTravelTo_ToolTip, 
                                        EDTx.UserControlRoute_buttonExtTravelFrom_ToolTip, EDTx.UserControlRoute_buttonExtTargetTo_ToolTip, EDTx.UserControlRoute_buttonToEDSM_ToolTip, 
                                        EDTx.UserControlRoute_buttonFromEDSM_ToolTip, EDTx.UserControlRoute_buttonTargetFrom_ToolTip, EDTx.UserControlRoute_checkBoxEDSM_ToolTip, 
                                        EDTx.UserControlRoute_cmd3DMap_ToolTip, EDTx.UserControlRoute_textBox_From_ToolTip, EDTx.UserControlRoute_textBox_Range_ToolTip, 
                                        EDTx.UserControlRoute_textBox_To_ToolTip, EDTx.UserControlRoute_textBox_Distance_ToolTip, EDTx.UserControlRoute_textBox_ToZ_ToolTip, 
                                        EDTx.UserControlRoute_textBox_ToY_ToolTip, EDTx.UserControlRoute_textBox_ToX_ToolTip, EDTx.UserControlRoute_textBox_FromZ_ToolTip, 
                                        EDTx.UserControlRoute_extButtonRoute_ToolTip, EDTx.UserControlRoute_textBox_FromY_ToolTip, EDTx.UserControlRoute_textBox_FromX_ToolTip };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            DiscoveryForm.OnHistoryChange += HistoryChanged;
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewRoute);
        }

        public override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewRoute);

            if (routingthread != null && routingthread.IsAlive && plotter != null)
            {
                plotter.StopPlotter = true;
                CloseRequested.Set();
                routingthread.Join();
            }

            DiscoveryForm.OnHistoryChange -= HistoryChanged;

            PutSetting("_RouteFrom", textBox_From.Text);
            PutSetting("_RouteTo", textBox_To.Text);
            PutSetting("_RouteRange", (int)textBox_Range.Value);
            PutSetting("_RouteFromX", textBox_FromX.Text);
            PutSetting("_RouteFromY", textBox_FromY.Text);
            PutSetting("_RouteFromZ", textBox_FromZ.Text);
            PutSetting("_RouteToX", textBox_ToX.Text);
            PutSetting("_RouteToY", textBox_ToY.Text);
            PutSetting("_RouteToZ", textBox_ToZ.Text);
            PutSetting("_RouteMetric", comboBoxRoutingMetric.SelectedIndex);
        }

        public void HistoryChanged()           // on History change, we now have history systems to look up, so make sure the To/From get a chance to update
        {
            if (DiscoveryForm.History.Count > 0)
            {
                UpdateTo(null);
                UpdateFrom(null);
                last_history_he = DiscoveryForm.History.GetLast;
            }
        }

        #endregion
        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            last_history_he = he;       // keep track
        }

        #region From

        private bool GetCoordsFrom(out Point3D pos)
        {
            double x = 0, y = 0, z = 0;

            bool worked = System.Double.TryParse(textBox_FromX.Text, out x) &&
                            System.Double.TryParse(textBox_FromY.Text, out y) &&
                            System.Double.TryParse(textBox_FromZ.Text, out z);
            pos = new Point3D(x, y, z);
            return worked;
        }

        // give box updating, and optional new From name

        private void UpdateFrom(object sender, string optupdatefrom = null)
        {
            changesilence = true;

            if (optupdatefrom != null)
                textBox_From.Text = optupdatefrom;

            if (sender == textBox_From)
            {
               ISystem ds1 = SystemCache.FindSystem(SystemNameOnly(textBox_From.Text), DiscoveryForm.GalacticMapping, EliteDangerousCore.WebExternalDataLookup.All);     // if we have a name, find it
                if (ds1 != null)
                {
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
                string res = "",resname="";
                if (GetCoordsFrom(out Point3D curpos))          // else if we have co-ords, find nearest
                {
                    ISystem nearest = SystemCache.FindNearestSystemTo(curpos.X, curpos.Y, curpos.Z, 100);
                    GalacticMapObject nearestgmo = DiscoveryForm.GalacticMapping.FindNearest(curpos.X, curpos.Y, curpos.Z);

                    if (nearest != null)
                    {
                        if (nearestgmo != null && nearest.Distance(curpos.X, curpos.Y, curpos.Z) > nearestgmo.GetSystem().Distance(curpos.X, curpos.Y, curpos.Z))
                            nearest = nearestgmo.GetSystem();
                    }
                    else
                        nearest = nearestgmo?.GetSystem();

                    if (nearest != null)
                    {
                        res = resname = nearest.Name;

                        double distance = Point3D.DistanceBetween(curpos, new Point3D(nearest.X, nearest.Y, nearest.Z));
                        if (distance > 0.1)
                            resname = nearest.Name + " @ " + distance.ToString("0.00") + "ly";
                    }
                }

                textBox_From.Text = res;
                textBox_FromName.Text = resname;
            }

            UpdateDistance();
            extButtonRoute.Enabled = IsValid();
            changesilence = false;
        }
        
        private void textBox_From_Enter(object sender, EventArgs e)
        {
            fromupdatetimer.Stop();
            fromupdatetimer.Start();
            fromupdatetimer.Tag = sender;
            if ( sender != textBox_From)
                ((ExtendedControls.ExtTextBox)sender).Select(0, 1000); // entering selects everything
        }

        void FromUpdateTick(object sender, EventArgs e)                 // timer timed out, 
        {
            fromupdatetimer.Stop();
            UpdateFrom(fromupdatetimer.Tag);
        }

        private void textBox_From_TextChanged(object sender, EventArgs e)
        {
            if (!changesilence)
            {
                fromupdatetimer.Stop();
                fromupdatetimer.Start();
                fromupdatetimer.Tag = sender;
            }
        }

        private void buttonFromHistory_Click(object sender, EventArgs e)
        {
            if (last_history_he != null)
            {
                UpdateFrom(textBox_From, last_history_he.System.Name);
            }
        }

        private void buttonFromTarget_Click(object sender, EventArgs e)
        {
            if (TargetClass.GetTargetPosition(out string name, out double x, out double y, out double z))
            {
                UpdateFrom(textBox_From, name);
            }
        }

        private void buttonFromEDSM_Click(object sender, EventArgs e)
        {
            string sysname = SystemNameOnly(textBox_From.Text);
            EDSMClass edsm = new EDSMClass();
            if (!edsm.ShowSystemInEDSM(sysname))
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System unknown to EDSM");
        }

        private void extButtonFromSpansh_Click(object sender, EventArgs e)
        {
            string sysname = SystemNameOnly(textBox_From.Text);
            EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForSystem(sysname);
        }

        #endregion

        #region To

        public bool GetCoordsTo(out Point3D pos)
        {
            double x = 0, y = 0, z = 0;

            bool worked = System.Double.TryParse(textBox_ToX.Text, out x) &&
                            System.Double.TryParse(textBox_ToY.Text, out y) &&
                            System.Double.TryParse(textBox_ToZ.Text, out z);
            pos = new Point3D(x, y, z);
            return worked;
        }

        private void UpdateTo(object sender, string optupdateto = null)
        {
            changesilence = true;

            if (optupdateto!= null)
                textBox_To.Text = optupdateto;

            if (sender == textBox_To)
            {
                ISystem ds1 = SystemCache.FindSystem(SystemNameOnly(textBox_To.Text), DiscoveryForm.GalacticMapping, EliteDangerousCore.WebExternalDataLookup.All);
                if (ds1 != null)
                {
                    textBox_ToName.Text = ds1.Name;
                    textBox_ToX.Text = ds1.X.ToString("0.00");
                    textBox_ToY.Text = ds1.Y.ToString("0.00");
                    textBox_ToZ.Text = ds1.Z.ToString("0.00");
                }
                else
                    textBox_ToX.Text = textBox_ToY.Text = textBox_ToZ.Text = "";
            }
            else
            {
                string res = "", resname = "";

                if (GetCoordsTo(out Point3D curpos))
                {
                    ISystem nearest = SystemCache.FindNearestSystemTo(curpos.X, curpos.Y, curpos.Z, 100);
                    GalacticMapObject nearestgmo = DiscoveryForm.GalacticMapping.FindNearest(curpos.X, curpos.Y, curpos.Z);

                    if (nearest != null)
                    {
                        if (nearestgmo != null && nearest.Distance(curpos.X, curpos.Y, curpos.Z) > nearestgmo.GetSystem().Distance(curpos.X, curpos.Y, curpos.Z))
                            nearest = nearestgmo.GetSystem();
                    }
                    else
                        nearest = nearestgmo?.GetSystem();

                    if (nearest != null)
                    {
                        res = resname = nearest.Name;

                        double distance = Point3D.DistanceBetween(curpos, new Point3D(nearest.X, nearest.Y, nearest.Z));
                        if (distance > 0.1)
                            resname = nearest.Name + " @ " + distance.ToString("0.00") + "ly";
                    }
                }

                textBox_To.Text = res;
                textBox_ToName.Text = resname;
            }

            UpdateDistance();
            extButtonRoute.Enabled = IsValid();
            changesilence = false;
        }


        private void textBox_To_Enter(object sender, EventArgs e)   // To has been tabbed/clicked..
        {
            toupdatetimer.Stop();
            toupdatetimer.Start();
            toupdatetimer.Tag = sender;
            if (sender != textBox_To)
                ((ExtendedControls.ExtTextBox)sender).Select(0, 1000); // entering selects everything
        }

        void ToUpdateTick(object sender, EventArgs e)
        {
            toupdatetimer.Stop();
            UpdateTo(toupdatetimer.Tag);
        }

        private void textBox_To_TextChanged(object sender, EventArgs e)
        {
            if (!changesilence)
            {
                toupdatetimer.Stop();
                toupdatetimer.Start();
                toupdatetimer.Tag = sender;
            }
        }

        private void buttonToHistory_Click(object sender, EventArgs e)
        {
            if (last_history_he != null)
            {
                UpdateTo(textBox_From, last_history_he.System.Name);
            }
        }

        private void buttonToTarget_Click(object sender, EventArgs e)
        {
            if (TargetClass.GetTargetPosition(out string name, out double x, out double y, out double z))
            {
                UpdateTo(textBox_To, name);
            }
        }

        private void buttonToEDSM_Click(object sender, EventArgs e)
        {
            string sysname = SystemNameOnly(textBox_To.Text);
            EDSMClass edsm = new EDSMClass();
            if (!edsm.ShowSystemInEDSM(sysname))
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System unknown to EDSM");
        }


        private void extButtonToSpansh_Click(object sender, EventArgs e)
        {
            string sysname = SystemNameOnly(textBox_To.Text);
            EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForSystem(sysname);
        }

        #endregion

        #region Internal Route Plotter

        private RoutePlotter plotter = null;

        private void button_Route_Click(object sender, EventArgs e)
        {
            if (routingthread == null  || !routingthread.IsAlive)
            {
                plotter = new RoutePlotter();
                plotter.MaxRange = textBox_Range.Value;
                GetCoordsFrom(out plotter.Coordsfrom);                      // will be valid for a system or a co-ords box
                GetCoordsTo(out plotter.Coordsto);
                plotter.FromSystem = !textBox_FromName.Text.Contains("@") && textBox_From.Text.HasChars() ? textBox_From.Text : "START POINT";
                plotter.ToSystem = !textBox_ToName.Text.Contains("@") && textBox_To.Text.HasChars() ? textBox_To.Text : "END POINT";
                plotter.RouteMethod = (SystemCache.SystemsNearestMetric) comboBoxRoutingMetric.SelectedIndex;
                plotter.UseFsdBoost = checkBox_FsdBoost.Checked;
                plotter.WebLookup = edsmSpanshButton.WebLookup;

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
                routingthread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(EDDRoutingThread));
                routingthread.Name = "Thread Route";

                cmd3DMap.Enabled = false;
                extButtonRoute.Text = "Cancel".T(EDTx.Cancel);
                routingthread.Start(plotter);
            }
            else
            {
                plotter.StopPlotter = true;
                extButtonRoute.Enabled = false;
            }
        }

        private Thread routingthread;

        private void EDDRoutingThread(object _plotter)
        {
            RoutePlotter p = (RoutePlotter)_plotter;

            routeSystems = null;    // so its null until route interative finishes

            routeSystems = p.RouteIterative(EDDAppendData);

            this.BeginInvoke(new Action(() =>
                {
                    RequestPanelOperation(this, new PushRouteList() { Systems = routeSystems });
                    buttonExtExcel.Enabled = extButtonExpeditionPush.Enabled = cmd3DMap.Enabled = true;
                    extButtonRoute.Text = "Find Route".TxID(EDTx.UserControlRoute_extButtonRoute);
                    extButtonRoute.Enabled = true;
                }));
        }

        private void EDDAppendData(RoutePlotter.ReturnInfo info)   // IN thread context, need to invoke
        {
            var ar = BeginInvoke((MethodInvoker)delegate      // using Invoke blocks the thread until the UI finishes.  Using BeginInvoke async causes it to overload the UI
            {
                DataGridViewRow rw = dataGridViewRoute.RowTemplate.Clone() as DataGridViewRow;
                rw.CreateCells(dataGridViewRoute,
                        info.name,
                        info?.system?.Tag as string ?? "",
                        double.IsNaN(info.dist) ? "" : info.dist.ToString("N2"),
                        info.pos == null ? "" : info.pos.X.ToString("0.00"),
                        info.pos == null ? "" : info.pos.Y.ToString("0.00"),
                        info.pos == null ? "" : info.pos.Z.ToString("0.00"),
                        info.pos == null ? "" : info.system == null ? Bodies.StarName(EDStar.Unknown) : Bodies.StarName(info.system.MainStarType),
                        double.IsNaN(info.waypointdist) ? "" : info.waypointdist.ToString("0.0"),
                        double.IsNaN(info.deviation) ? "" : info.deviation.ToString("0.0")
                        );

                rw.Tag = info.system;       // may be null if waypoint or not a system
                rw.Cells[0].Tag = info.system?.Name;    // write the name of the system into the cells'tag for copying
                rw.HeaderCell.Value = info.pos != null ? (dataGridViewRoute.Rows.Count + 1).ToStringInvariant() : "-";
                dataGridViewRoute.Rows.Add(rw);
                if (!rw.Displayed)
                {
                    dataGridViewRoute.SafeFirstDisplayedScrollingRowIndex(dataGridViewRoute.SafeFirstDisplayedScrollingRowIndex()+1);
                }
            });

            WaitHandle.WaitAny(new WaitHandle[] { CloseRequested, ar.AsyncWaitHandle });
        }

        #endregion

    }
}
