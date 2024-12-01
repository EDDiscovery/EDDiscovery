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
using EMK.LightGeometry;
using ExtendedControls;
using System;
using System.Collections.Generic;
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

        private int computing = 0;      // 0 = none, 1 = internal, 2 = web
        private double jumprangelastfound;

        private const string dbPermit = "PermitAllowed";

        #region  Init

        public UserControlRoute()
        {
            InitializeComponent();

            // do at init, seems to cause problems on some PCs doing after

            var enumlist = new Enum[] { EDTx.UserControlRoute_SystemCol, EDTx.UserControlRoute_NoteCol, EDTx.UserControlRoute_DistanceCol, EDTx.UserControlRoute_StarClassCol, EDTx.UserControlRoute_WayPointDistCol,
                                        EDTx.UserControlRoute_DeviationCol,
                                        EDTx.UserControlRoute_checkBox_FsdBoost, EDTx.UserControlRoute_buttonExtTravelTo, EDTx.UserControlRoute_buttonExtTravelFrom,
                                        EDTx.UserControlRoute_buttonExtTargetTo,  EDTx.UserControlRoute_buttonTargetFrom, EDTx.UserControlRoute_labelEDSMBut,
                                        EDTx.UserControlRoute_labelLy2, EDTx.UserControlRoute_labelLy1, EDTx.UserControlRoute_labelTo,
                                        EDTx.UserControlRoute_labelMaxJump, EDTx.UserControlRoute_labelDistance, EDTx.UserControlRoute_labelMetric,
                                        EDTx.UserControlRoute_extButtonRoute, EDTx.UserControlRoute_labelFrom,
                                        EDTx.UserControlRoute_groupBoxSpansh, EDTx.UserControlRoute_extButtonSpanshRoadToRiches, EDTx.UserControlRoute_extButtonNeutronRouter,
                                        EDTx.UserControlRoute_extButtonFleetCarrier,EDTx.UserControlRoute_extButtonSpanshGalaxyPlotter,EDTx.UserControlRoute_extButtonExoMastery,
                                        EDTx.UserControlRoute_extButtonSpanshAmmoniaWorlds,EDTx.UserControlRoute_extButtonSpanshEarthLikes,EDTx.UserControlRoute_extButtonSpanshTradeRouter,
                                        EDTx.UserControlRoute_groupBoxInternal,EDTx.UserControlRoute_groupBoxPara,
                                        EDTx.UserControlRoute_extCheckBoxPermitSystems, EDTx.UserControlRoute_labelCargo };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);

            var enumlistcms = new Enum[] { EDTx.UserControlRoute_showInEDSMToolStripMenuItem, EDTx.UserControlRoute_copyToolStripMenuItem, EDTx.UserControlRoute_showScanToolStripMenuItem,
                                            EDTx.UserControlRoute_viewOnSpanshToolStripMenuItem};
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);

            var enumlisttt = new Enum[] { EDTx.UserControlRoute_checkBox_FsdBoost_ToolTip, EDTx.UserControlRoute_buttonExtExcel_ToolTip, EDTx.UserControlRoute_textBox_ToName_ToolTip,
                                        EDTx.UserControlRoute_textBox_FromName_ToolTip, EDTx.UserControlRoute_comboBoxRoutingMetric_ToolTip, EDTx.UserControlRoute_buttonExtTravelTo_ToolTip,
                                        EDTx.UserControlRoute_buttonExtTravelFrom_ToolTip, EDTx.UserControlRoute_buttonExtTargetTo_ToolTip, EDTx.UserControlRoute_buttonToEDSM_ToolTip,
                                        EDTx.UserControlRoute_buttonFromEDSM_ToolTip, EDTx.UserControlRoute_buttonTargetFrom_ToolTip,
                                        EDTx.UserControlRoute_cmd3DMap_ToolTip, EDTx.UserControlRoute_textBox_From_ToolTip, EDTx.UserControlRoute_textBox_Range_ToolTip,
                                        EDTx.UserControlRoute_textBox_To_ToolTip, EDTx.UserControlRoute_textBox_Distance_ToolTip, EDTx.UserControlRoute_textBox_ToZ_ToolTip,
                                        EDTx.UserControlRoute_textBox_ToY_ToolTip, EDTx.UserControlRoute_textBox_ToX_ToolTip, EDTx.UserControlRoute_textBox_FromZ_ToolTip,
                                        EDTx.UserControlRoute_extButtonRoute_ToolTip, EDTx.UserControlRoute_textBox_FromY_ToolTip, EDTx.UserControlRoute_textBox_FromX_ToolTip,
                                        EDTx.UserControlRoute_extButtonExpeditionSave_ToolTip,EDTx.UserControlRoute_extButtonExpeditionPush_ToolTip,
                                        };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);
        }

        public override void Init()
        {
            DBBaseName = "UCRoute";

            EnableOutputButtons();

            fromupdatetimer = new System.Windows.Forms.Timer();
            toupdatetimer = new System.Windows.Forms.Timer();

            fromupdatetimer.Interval = 1000;
            fromupdatetimer.Tick += FromUpdateTick;
            toupdatetimer.Interval = 1000;
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
            textBox_To.SetAutoCompletor(SystemCache.ReturnSystemAdditionalListForAutoComplete, true);
            textBox_To.AutoCompleteTimeout = 500;

            textBox_From.Text = GetSetting("_RouteFrom", "");
            textBox_To.Text = GetSetting("_RouteTo", "");
            textBox_FromX.ValueNoChange = GetSetting("_RouteFromX", 0.0);
            textBox_FromY.ValueNoChange = GetSetting("_RouteFromY", 0.0);
            textBox_FromZ.ValueNoChange = GetSetting("_RouteFromZ", 0.0);
            textBox_ToX.ValueNoChange = GetSetting("_RouteToX", 0.0);
            textBox_ToY.ValueNoChange = GetSetting("_RouteToY", 0.0);
            textBox_ToZ.ValueNoChange = GetSetting("_RouteToZ", 0.0);
            numberBoxIntCargo.ValueNoChange = GetSetting("_Cargo", 0);
            jumprangelastfound = textBox_Range.Value = DiscoveryForm.History.GetLast?.ShipInformation?.GetJumpRange(numberBoxIntCargo.Value) ?? 25;

            int metricvalue = GetSetting("RouteMetric", 0);
            comboBoxRoutingMetric.SelectedIndex = Enum.IsDefined(typeof(SystemCache.SystemsNearestMetric), metricvalue)
                ? metricvalue
                : (int)SystemCache.SystemsNearestMetric.IterativeNearestWaypoint;

            UpdateDistance();
            EnableRouteButtonsIfValid();

            changesilence = false;

            edsmSpanshButton.Init(this, "EDSMSpansh", "");

      
            waitforspanshresulttimer.Interval = 1000;
            waitforspanshresulttimer.Tick += Waitforspanshresulttimer_Tick;

            NoteCol.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            textBox_FromX.ValidityChanged += ValidityChanges;
            textBox_FromY.ValidityChanged += ValidityChanges;
            textBox_FromZ.ValidityChanged += ValidityChanges;
            textBox_ToX.ValidityChanged += ValidityChanges;
            textBox_ToY.ValidityChanged += ValidityChanges;
            textBox_ToZ.ValidityChanged += ValidityChanges;
            textBox_Range.ValidityChanged += ValidityChanges;
            numberBoxIntCargo.ValueChanged += NumberBoxIntCargo_ValueChanged;

            labelRouteName.Text = "";

            DiscoveryForm.OnHistoryChange += DiscoveryForm_OnHistoryChange;
            DiscoveryForm.OnNewEntry += DiscoveryForm_OnNewEntry;
            DiscoveryForm.OnSyncComplete += DiscoveryForm_OnSyncComplete;
            extCheckBoxPermitSystems.Click += ExtCheckBoxPermitSystems_Click; ;
        }

        private void NumberBoxIntCargo_ValueChanged(object sender, EventArgs e)
        {
            double? range = DiscoveryForm.History.GetLast?.ShipInformation?.GetJumpRange(numberBoxIntCargo.Value);
            if (range != null)
                textBox_Range.Value = range.Value;
        }

        private void DiscoveryForm_OnNewEntry(HistoryEntry he)
        {
            double? range = he.ShipInformation?.GetJumpRange(numberBoxIntCargo.Value);      // using cargo, what is the range?

            if (range.HasValue && range != jumprangelastfound)      // if we selected a different ship or changed modules, detected by jump range changing, update
            {
                System.Diagnostics.Debug.WriteLine($"Router ship range has changed, updating");
                jumprangelastfound = textBox_Range.Value = range.Value;
            }
        }

        private void DiscoveryForm_OnHistoryChange()
        {
            jumprangelastfound = textBox_Range.Value = DiscoveryForm.History.GetLast?.ShipInformation?.GetJumpRange(numberBoxIntCargo.Value) ?? 25;
        }

        private void DiscoveryForm_OnSyncComplete(long arg1, long arg2)
        {
            SetPermit();
        }
        private void SetPermit()
        {
            bool permitsystems = SystemsDatabase.Instance.PermitSystems.Count > 0;
            extCheckBoxPermitSystems.Enabled = permitsystems;
            extCheckBoxPermitSystems.Checked = permitsystems ? GetSetting(dbPermit, false) : false;
        }
        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewRoute);
            SetPermit();
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

            PutSetting("_RouteFrom", textBox_From.Text);
            PutSetting("_RouteTo", textBox_To.Text);
            PutSetting("_RouteFromX", textBox_FromX.Value);
            PutSetting("_RouteFromY", textBox_FromY.Value);
            PutSetting("_RouteFromZ", textBox_FromZ.Value);
            PutSetting("_RouteToX", textBox_ToX.Value);
            PutSetting("_RouteToY", textBox_ToY.Value);
            PutSetting("_RouteToZ", textBox_ToZ.Value);
            PutSetting("_RouteMetric", comboBoxRoutingMetric.SelectedIndex);
            PutSetting("_Cargo", numberBoxIntCargo.Value);

            DiscoveryForm.OnHistoryChange -= DiscoveryForm_OnHistoryChange;
            DiscoveryForm.OnSyncComplete -= DiscoveryForm_OnSyncComplete;
            DiscoveryForm.OnNewEntry -= DiscoveryForm_OnNewEntry;
        }

        #endregion

        #region From

        private bool GetCoordsFrom(out Point3D pos)
        {
            if (textBox_FromX.IsValid && textBox_FromY.IsValid && textBox_FromZ.IsValid)
            {
                pos = new Point3D(textBox_FromX.Value, textBox_FromY.Value, textBox_FromZ.Value);
                return true;
            }
            else
            {
                pos = new Point3D(0, 0, 0);
                return false;
            }
        }

        // give box updating, and optional new From name

        private void UpdateFrom(object sender, string optupdatefrom = null)
        {
            changesilence = true;

            if (optupdatefrom != null)
                textBox_From.Text = optupdatefrom;

            if (sender == textBox_From)
            {
                ISystem ds1 = SystemCache.FindSystem(textBox_From.Text, DiscoveryForm.GalacticMapping, edsmSpanshButton.WebLookup);     // if we have a name, find it

                if (ds1 != null)
                {
                    textBox_FromName.Text = ds1.Name;
                    textBox_FromX.ValueNoChange = ds1.X;
                    textBox_FromY.ValueNoChange = ds1.Y;
                    textBox_FromZ.ValueNoChange = ds1.Z;
                }
                else
                {
                    textBox_FromX.SetBlank();
                    textBox_FromY.SetBlank();
                    textBox_FromZ.SetBlank();
                }
            }
            else
            {
                string res = "", resname = "";
                if (GetCoordsFrom(out Point3D curpos))          // else if we have co-ords, find nearest
                {
                    Cursor = Cursors.WaitCursor;

                    ISystem nearest = SystemCache.FindNearestSystemTo(curpos.X, curpos.Y, curpos.Z, 40, edsmSpanshButton.WebLookup, DiscoveryForm.GalacticMapping);

                    if (nearest != null)
                    {
                        res = resname = nearest.Name;

                        double distance = Point3D.DistanceBetween(curpos, new Point3D(nearest.X, nearest.Y, nearest.Z));
                        if (distance > 0.1)
                            resname = nearest.Name + " @ " + distance.ToString("0.00") + "ly";
                    }
                    Cursor = Cursors.Default;
                }

                textBox_From.Text = res;
                textBox_FromName.Text = resname;
            }

            UpdateDistance();
            EnableRouteButtonsIfValid();
            changesilence = false;
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
                fromupdatetimer.Tag = sender;
                fromupdatetimer.Start();
            }
        }

        private void valueBox_From_ValueChanged(object sender, EventArgs e)
        {
            fromupdatetimer.Stop();
            fromupdatetimer.Tag = sender;
            fromupdatetimer.Start();
        }

        private void buttonFromHistory_Click(object sender, EventArgs e)
        {
            var last_history_he = DiscoveryForm.History.GetLast;
            if (last_history_he != null)
                UpdateFrom(textBox_From, last_history_he.System.Name);
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
            EDSMClass edsm = new EDSMClass();
            if (!edsm.ShowSystemInEDSM(textBox_From.Text))
                MessageBoxTheme.Show(FindForm(), "System unknown to EDSM");
        }

        private void extButtonFromSpansh_Click(object sender, EventArgs e)
        {
            if ( !EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForSystem(textBox_From.Text))
                MessageBoxTheme.Show(FindForm(), "System unknown to Spansh");
        }

        #endregion

        #region To

        public bool GetCoordsTo(out Point3D pos)
        {
            if (textBox_ToX.IsValid && textBox_ToY.IsValid && textBox_ToZ.IsValid)
            {
                pos = new Point3D(textBox_ToX.Value, textBox_ToY.Value, textBox_ToZ.Value);
                return true;
            }
            else
            {
                pos = new Point3D(0, 0, 0);
                return false;
            }
        }

        private void UpdateTo(object sender, string optupdateto = null)
        {
            changesilence = true;

            if (optupdateto != null)
                textBox_To.Text = optupdateto;

            if (sender == textBox_To)
            {
                ISystem ds1 = SystemCache.FindSystem(textBox_To.Text, DiscoveryForm.GalacticMapping, edsmSpanshButton.WebLookup);
                if (ds1 != null)
                {
                    textBox_ToName.Text = ds1.Name;
                    textBox_ToX.ValueNoChange = ds1.X;
                    textBox_ToY.ValueNoChange = ds1.Y;
                    textBox_ToZ.ValueNoChange = ds1.Z;
                }
                else
                {
                    textBox_ToX.SetBlank();
                    textBox_ToY.SetBlank();
                    textBox_ToZ.SetBlank();
                }
            }
            else
            {
                string res = "", resname = "";

                if (GetCoordsTo(out Point3D curpos))
                {
                    Cursor = Cursors.WaitCursor;

                    ISystem nearest = SystemCache.FindNearestSystemTo(curpos.X, curpos.Y, curpos.Z, 40, edsmSpanshButton.WebLookup, DiscoveryForm.GalacticMapping);

                    if (nearest != null)
                    {
                        res = resname = nearest.Name;

                        double distance = Point3D.DistanceBetween(curpos, new Point3D(nearest.X, nearest.Y, nearest.Z));
                        if (distance > 0.1)
                            resname = nearest.Name + " @ " + distance.ToString("0.00") + "ly";
                    }
                    Cursor = Cursors.Default;
                }

                textBox_To.Text = res;
                textBox_ToName.Text = resname;
            }

            UpdateDistance();
            EnableRouteButtonsIfValid();
            changesilence = false;
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
                toupdatetimer.Tag = sender;
                toupdatetimer.Start();
            }
        }

        private void valueBox_To_ValueChanged(object sender, EventArgs e)
        {
            toupdatetimer.Stop();
            toupdatetimer.Tag = sender;
            toupdatetimer.Start();
        }

        private void buttonToHistory_Click(object sender, EventArgs e)
        {
            var last_history_he = DiscoveryForm.History.GetLast;
            if (last_history_he != null)
                UpdateTo(textBox_To, last_history_he.System.Name);
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
            EDSMClass edsm = new EDSMClass();
            if (!edsm.ShowSystemInEDSM(textBox_To.Text))
                MessageBoxTheme.Show(FindForm(), "System unknown to EDSM");
        }

        private void extButtonToSpansh_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForSystem(textBox_To.Text);
        }

        #endregion

        #region Internal Route Plotter

        private RoutePlotter plotter = null;

        private void button_Route_Click(object sender, EventArgs e)
        {
            if (routingthread == null || !routingthread.IsAlive)
            {
                plotter = new RoutePlotter();
                plotter.MaxRange = (float)textBox_Range.Value;
                GetCoordsFrom(out plotter.Coordsfrom);                      // will be valid for a system or a co-ords box
                GetCoordsTo(out plotter.Coordsto);
                plotter.FromSystem = !textBox_FromName.Text.Contains("@") && textBox_From.Text.HasChars() ? textBox_From.Text : "START POINT";
                plotter.ToSystem = !textBox_ToName.Text.Contains("@") && textBox_To.Text.HasChars() ? textBox_To.Text : "END POINT";
                plotter.RouteMethod = (SystemCache.SystemsNearestMetric)comboBoxRoutingMetric.SelectedIndex;
                plotter.UseFsdBoost = checkBox_FsdBoost.Checked;
                plotter.WebLookup = edsmSpanshButton.WebLookup;
                plotter.DiscardList = extCheckBoxPermitSystems.Enabled ? (extCheckBoxPermitSystems.Checked ? null : SystemsDatabase.Instance.PermitSystems) : null;

                int PossibleJumps = (int)(Point3D.DistanceBetween(plotter.Coordsfrom, plotter.Coordsto) / plotter.MaxRange);

                if (PossibleJumps > 100)
                {
                    DialogResult res = MessageBoxTheme.Show(FindForm(),
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

                extButtonRoute.Text = "Cancel".T(EDTx.Cancel);

                EnableOutputButtons();
                computing = 1;
                EnableRouteButtonsIfValid();

                labelRouteName.Text = $"{textBox_From.Text}-{textBox_To.Text} (DB)";
                routingthread.Start(plotter);
            }
            else
            {
                plotter.StopPlotter = true;
            }
        }

        private Thread routingthread;

        private void EDDRoutingThread(object plotter)
        {
            RoutePlotter p = (RoutePlotter)plotter;

            routeSystems = null;    // so its null until route interative finishes

            routeSystems = p.RouteIterative(EDDAppendData);

            this.BeginInvoke(new Action(() =>
                {
                    RequestPanelOperation(this, new PushRouteList() { Systems = routeSystems });
                    EnableOutputButtons(true);
                    extButtonRoute.Text = "Find Route".TxID(EDTx.UserControlRoute_extButtonRoute);
                    computing = 0;
                    EnableRouteButtonsIfValid();
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
                        info.pos == null ? "" : info.pos.X.ToString("0.####"),
                        info.pos == null ? "" : info.pos.Y.ToString("0.####"),
                        info.pos == null ? "" : info.pos.Z.ToString("0.####"),
                        info.pos == null ? "" : info.system == null ? Stars.StarName(EDStar.Unknown) : Stars.StarName(info.system.MainStarType),
                        double.IsNaN(info.waypointdist) ? "" : info.waypointdist.ToString("0.0"),
                        double.IsNaN(info.deviation) ? "" : info.deviation.ToString("0.0")
                        );

                rw.Tag = info.system;       // may be null if waypoint or not a system
                rw.Cells[0].Tag = info.system?.Name;    // write the name of the system into the cells'tag for copying
                rw.HeaderCell.Value = info.pos != null ? (dataGridViewRoute.Rows.Count + 1).ToStringInvariant() : "-";
                dataGridViewRoute.Rows.Add(rw);
                if (!rw.Displayed)
                {
                    dataGridViewRoute.SafeFirstDisplayedScrollingRowIndex(dataGridViewRoute.SafeFirstDisplayedScrollingRowIndex() + 1);
                }
            });

            WaitHandle.WaitAny(new WaitHandle[] { CloseRequested, ar.AsyncWaitHandle });
        }

        #endregion

    }
}
