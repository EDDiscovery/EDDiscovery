/*
 * Copyright © 2016 - 2020 EDDiscovery development team
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

// turn on for play testing of all your scans
//#define PLAYTHRU

using BaseUtils;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScan : UserControlCommonBase
    {
        HistoryEntry last_he = null;

        bool override_system = false;

        ISystem showing_system;                         // set from last_he or manually..
        List<MaterialCommodityMicroResource> showing_matcomds;

        bool closing = false;           // set when closing, to prevent a resize, which you can get, causing a big redraw

        string[] bodyfilters;           // body filters
        string[] displayfilters;        // display filters

        #region Init
        public UserControlScan()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;            // we are dealing with graphics.. lets turn off dialog scaling.
        }

        public override void Init()
        {
            DBBaseName = "ScanPanel";

            panelStars.SystemDisplay.ShowEDSMBodies = checkBoxEDSM.Checked = GetSetting("EDSM", false);
            this.checkBoxEDSM.CheckedChanged += new System.EventHandler(this.checkBoxEDSM_CheckedChanged);

            bodyfilters = GetSetting("BodyFilters", "All").Split(';');

            displayfilters = GetSetting("DisplayFilters", "moons;icons;mats;allg;habzone;starclass;planetclass;dist;").Split(';');
            ApplyDisplayFilters();

            panelStars.SystemDisplay.ValueLimit = GetSetting("ValueLimit", 50000);

            rollUpPanelTop.PinState = GetSetting("PinState", true);

            int size = GetSetting("Size", 64);
            SetSizeImage(size);

            discoveryform.OnNewEntry += NewEntry;

            var enumlisttt = new Enum[] { EDTx.UserControlScan_extCheckBoxStar_ToolTip, EDTx.UserControlScan_extButtonFilter_ToolTip, EDTx.UserControlScan_extButtonDisplayFilters_ToolTip, EDTx.UserControlScan_buttonSize_ToolTip, EDTx.UserControlScan_checkBoxEDSM_ToolTip, EDTx.UserControlScan_extButtonHighValue_ToolTip, EDTx.UserControlScan_buttonExtExcel_ToolTip };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            rollUpPanelTop.SetToolTip(toolTip);     // set after translater

#if PLAYTHRU
            t = new Timer();      // debug, keep for now
            t.Interval = 100;
            t.Tick += T_Tick;
#endif
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void InitialDisplay()
        {
            last_he = uctg.GetCurrentHistoryEntry;
            DrawSystem(last_he);    // may be null
        }

        public override void Closing()
        {
            PutSetting("PinState", rollUpPanelTop.PinState );

            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            discoveryform.OnNewEntry -= NewEntry;
            closing = true;
        }

        #endregion

        #region Transparency
        public override bool SupportTransparency { get { return true; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            panelStars.SetBackground(curcol);
            this.BackColor = panelControls.BackColor = curcol;
            rollUpPanelTop.BackColor = curcol;
			rollUpPanelTop.ShowHiddenMarker = !on;
            DrawSystem();
        }

        private void UserControlScan_Resize(object sender, EventArgs e)
        {
            if (!closing && showing_system != null)
            {
                int newspace = panelStars.WidthAvailable;

                if (newspace < panelStars.SystemDisplay.DisplayAreaUsed.X || newspace > panelStars.SystemDisplay.DisplayAreaUsed.X +  panelStars.SystemDisplay.StarSize.Width)
                {
                    DrawSystem();
                }
            }
        }

#endregion

#region Display

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made.. check to see if its a scan update
        {
            if (he != null)
            {
                // Star scan type, or material entry type, or a bodyname/id entry, or not set, or not same system
                if ( he.journalEntry is IStarScan || he.journalEntry is IMaterialJournalEntry || he.journalEntry is IBodyNameAndID || last_he == null || last_he.System != he.System) 
                {
                    last_he = he;
                    DrawSystem(last_he);
                }
            }
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
#if PLAYTHRU
            t.Start();    // debug, for playing all scans thru
#endif

            if (he != null)
            {
                if (last_he == null || last_he.System != he.System) // if we changed system, we need to represent
                {
                    last_he = he;
                    DrawSystem(last_he);
                }
            }
        }

        void DrawSystem(HistoryEntry he)
        {
            if (override_system)        // no change, last_he continues to track cursor for restore..
                return;

            showing_matcomds = he != null ? discoveryform.history.MaterialCommoditiesMicroResources.Get(he.MaterialCommodity) : null;
            showing_system = he?.System;
            DrawSystem();
        }

        async void DrawSystem()   // draw showing_system (may be null), showing_matcomds (may be null)
        {
            panelStars.HideInfo();

            // showing_system = new SystemClass("Qi Lieh");
            //showing_system = new SystemClass("Pallaeni"); - problem with shrinking lines
            //   showing_system = new SystemClass("Borann");
            //showing_system = new SystemClass("Skaudai AM-B d14-138");
            //showing_system = new SystemClass("Eorgh Prou JH-V e2-1979");
           //  showing_system = new SystemClass("HYPAA FLYIAE CB-O D6-8");

#if PLAYTHRU
            StarScan.SystemNode data = showing_system != null ? await discoveryform.history.starscan.FindSystemAsync(showing_system, false, byname: true) : null;
#else
            StarScan.SystemNode data = showing_system != null ? await discoveryform.history.StarScan.FindSystemAsync(showing_system, checkBoxEDSM.Checked) : null;
#endif
            string control_text = "No System";

            if (showing_system != null)
            {
                control_text = showing_system.Name;

                if (data != null)
                {
                    long value = data.ScanValue(checkBoxEDSM.Checked);
                    control_text += " ~ " + value.ToString("N0") + " cr";

                    int scanned = data.StarPlanetsScanned();

                    if (scanned > 0)
                    {
                        control_text += " " + "Scan".T(EDTx.UserControlSurveyor_Scan) + " " + scanned.ToString() + (data.FSSTotalBodies != null ? (" / " + data.FSSTotalBodies.Value.ToString()) : "");
                    }

                    int fsssignals = data.FSSSignalList.Count;
                    if (fsssignals>0)
                    {
                        control_text += " " + " Signals " + fsssignals.ToString();
                    }
                }
                else
                    control_text += " " + "No Scan".T(EDTx.NoScan);
            }

            var curmats = discoveryform.history.MaterialCommoditiesMicroResources.GetLast();

            panelStars.DrawSystem(data, showing_matcomds, curmats, (HasControlTextArea() && !displayfilters.Contains("sys")) ? null : control_text, bodyfilters);

            SetControlText(control_text);
        }

#endregion

#region User interaction

        private void extCheckBoxStar_Click(object sender, EventArgs e)
        {
            if ( extCheckBoxStar.Checked == true )
            {
                ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
                int width = 500;
                f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "System:".T(EDTx.UserControlScan_System), new Point(10, 40), new Size(110, 24), null));
                f.Add(new ExtendedControls.ConfigurableForm.Entry("Sys", typeof(ExtendedControls.ExtTextBoxAutoComplete), "", new Point(120, 40), new Size(width - 120 - 20, 24), null));

                f.AddOK(new Point(width - 20 - 80, 80));
                f.AddCancel(new Point(width - 200, 80));

                f.Trigger += (dialogname, controlname, tag) =>
                {
                    if (controlname == "OK" || controlname == "Cancel" || controlname == "Close" )
                    {
                        f.ReturnResult(controlname == "OK" ? DialogResult.OK : DialogResult.Cancel);
                    }
                    else if ( controlname == "Sys:Return")
                    {
                        if (f.Get("Sys").HasChars())
                        {
                            f.ReturnResult(DialogResult.OK);
                        }

                        f.SwallowReturn = true;
                    }
                };

                f.InitCentred(this.FindForm(), this.FindForm().Icon, "Show System".T(EDTx.UserControlScan_EnterSys), null, null, closeicon:true);
                f.GetControl<ExtendedControls.ExtTextBoxAutoComplete>("Sys").SetAutoCompletor(SystemCache.ReturnSystemAutoCompleteList, true);
                DialogResult res = f.ShowDialog(this.FindForm());

                if (res == DialogResult.OK)
                {
                    string sname = f.Get("Sys");
                    if (sname.HasChars())
                    {
                        showing_matcomds = null;
                        showing_system = new EliteDangerousCore.SystemClass(sname);
                        override_system = true;
                        DrawSystem();
                        extCheckBoxStar.Checked = true;
                    }
                    else
                        extCheckBoxStar.Checked = false;
                }
                else
                    extCheckBoxStar.Checked = false;
            }
            else
            {
                override_system = false;
                DrawSystem(last_he);
                extCheckBoxStar.Checked = false;
            }

        }

        private void checkBoxEDSM_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting("EDSM", checkBoxEDSM.Checked);
            panelStars.SystemDisplay.ShowEDSMBodies = checkBoxEDSM.Checked;
            DrawSystem();
        }

        private void extButtonHighValue_Click(object sender, EventArgs e)
        {
            ExtendedControls.ConfigurableForm cf = new ConfigurableForm();
            int width = 300;
            int height = 100;

            cf.Add(new ExtendedControls.ConfigurableForm.Entry("UC", typeof(ExtendedControls.NumberBoxLong), panelStars.SystemDisplay.ValueLimit.ToStringInvariant(),
                                        new Point(5, 30), new Size(width - 5 - 20, 24), null)
            { numberboxlongminimum = 1, numberboxlongmaximum = 2000000000 });

            cf.Add(new ExtendedControls.ConfigurableForm.Entry("OK", typeof(ExtendedControls.ExtButton), "OK".T(EDTx.OK),
                        new Point(width - 20 - 80, height - 40), new Size(80, 24), ""));

            cf.Trigger += (dialogname, controlname, tag) =>
            {
                System.Diagnostics.Debug.WriteLine("control" + controlname);

                if (controlname.Contains("Validity:False"))
                    cf.GetControl("OK").Enabled = false;
                else if (controlname.Contains("Validity:True"))
                    cf.GetControl("OK").Enabled = true;
                else if (controlname == "OK")
                {
                    cf.ReturnResult(DialogResult.OK);
                }
                else if (controlname == "Cancel")
                {
                    cf.ReturnResult(DialogResult.Cancel);
                }
            };

            if (cf.ShowDialogCentred(this.FindForm(), this.FindForm().Icon,  "Set Valuable Minimum".T(EDTx.UserControlScan_VLMT)) == DialogResult.OK)
            {
                long? value = cf.GetLong("UC");
                panelStars.SystemDisplay.ValueLimit = (int)value.Value;
                PutSetting("ValueLimit", panelStars.SystemDisplay.ValueLimit);
                DrawSystem();
            }
        }

        ExtListBoxForm dropdown;

        private void buttonSize_Click(object sender, EventArgs e)
        {
            dropdown = new ExtListBoxForm("", true);

            Image[] imagelist = new Image[] {
                global::EDDiscovery.Icons.Controls.Scan_SizeHuge ,
                global::EDDiscovery.Icons.Controls.Scan_SizeVeryLarge ,
                global::EDDiscovery.Icons.Controls.Scan_SizeLarge ,
                global::EDDiscovery.Icons.Controls.Scan_SizeMedium ,
                global::EDDiscovery.Icons.Controls.Scan_SizeSmall ,
                global::EDDiscovery.Icons.Controls.Scan_SizeTiny ,
                global::EDDiscovery.Icons.Controls.Scan_SizeTinyTiny ,
                global::EDDiscovery.Icons.Controls.Scan_SizeMinuscule ,
            };

            string[] textlist = new string[] { "256", "192", "128", "96", "64", "48", "32", "16" };

            dropdown.Items = textlist.ToList();
            dropdown.ImageItems = imagelist.ToList();
            dropdown.FlatStyle = FlatStyle.Popup;
            dropdown.PositionBelow(buttonSize);
            dropdown.SelectedIndexChanged += (s, ea) =>
            {
                int size = textlist[dropdown.SelectedIndex].InvariantParseInt(64);
                SetSizeImage(size);
                DrawSystem();
            };

            ExtendedControls.Theme.Current.ApplyStd(dropdown,true);

            dropdown.Show(this.FindForm());
        }

        private void SetSizeImage(int size)
        {
            if (size == 256)
                buttonSize.Image = global::EDDiscovery.Icons.Controls.Scan_SizeHuge;
            else if (size == 192)
                buttonSize.Image = global::EDDiscovery.Icons.Controls.Scan_SizeVeryLarge;
            else if (size == 128)
                buttonSize.Image = global::EDDiscovery.Icons.Controls.Scan_SizeLarge;
            else if (size == 96)
                buttonSize.Image = global::EDDiscovery.Icons.Controls.Scan_SizeMedium;
            else if (size == 64)
                buttonSize.Image = global::EDDiscovery.Icons.Controls.Scan_SizeSmall;
            else if (size == 48)
                buttonSize.Image = global::EDDiscovery.Icons.Controls.Scan_SizeTiny;
            else if (size == 32)
                buttonSize.Image = global::EDDiscovery.Icons.Controls.Scan_SizeTinyTiny;
            else
                buttonSize.Image = global::EDDiscovery.Icons.Controls.Scan_SizeMinuscule;

            panelStars.SystemDisplay.SetSize(size);
            PutSetting("Size", size);
        }

#endregion

        private void extButtonFilter_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup bodyfilter = new CheckedIconListBoxFormGroup();
            bodyfilter.AddAllNone();
            foreach (var x in Enum.GetValues(typeof(EDPlanet)))
                bodyfilter.AddStandardOption(x.ToString(), Bodies.PlanetTypeName((EDPlanet)x) );
            foreach (var x in Enum.GetNames(typeof(EDStar)))
                bodyfilter.AddStandardOption(x.ToString(), Bodies.StarName(x.ParseEnum<EDStar>()));

            // these are filter types for items which are either do not have scandata or are not stars/bodies.  Only Belts/Barycentre are displayed.. scans of rings/beltculsters are not displayed
            bodyfilter.AddStandardOption("star", "Star".T(EDTx.UserControlScan_Star));
            bodyfilter.AddStandardOption("body", "Body".T(EDTx.UserControlScan_Body));
            bodyfilter.AddStandardOption("barycentre", "Barycentre".T(EDTx.UserControlScan_Barycentre));
            bodyfilter.AddStandardOption("belt", "Belt".T(EDTx.UserControlScan_Belt));

            bodyfilter.SaveSettings = (s, o) => 
            {
                bodyfilters = s.Split(';');
                PutSetting("BodyFilters", string.Join(";", bodyfilters));
                DrawSystem();
            };

            bodyfilter.CloseBoundaryRegion = new Size(32, extButtonFilter.Height);

            bodyfilter.Show(string.Join(";",bodyfilters), extButtonFilter, this.FindForm());
        }

        private void extButtonDisplayFilters_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            displayfilter.AddAllNone();
            displayfilter.AddStandardOption("icons", "Show body status icons".T(EDTx.UserControlScan_StatusIcons), global::EDDiscovery.Icons.Controls.Scan_ShowOverlays);
            displayfilter.AddStandardOption("mats", "Show Materials".T(EDTx.UserControlScan_ShowMaterials), global::EDDiscovery.Icons.Controls.Scan_ShowAllMaterials);
            displayfilter.AddStandardOption("rares", "Show rare materials only".T(EDTx.UserControlScan_ShowRaresOnly), global::EDDiscovery.Icons.Controls.Scan_ShowRareMaterials);
            displayfilter.AddStandardOption("matfull", "Hide materials which have reached their storage limit".T(EDTx.UserControlScan_MatFull), global::EDDiscovery.Icons.Controls.Scan_HideFullMaterials);
            displayfilter.AddStandardOption("moons", "Show Moons".T(EDTx.UserControlScan_ShowMoons), global::EDDiscovery.Icons.Controls.Scan_ShowMoons);
            displayfilter.AddStandardOption("allg", "Show G on all planets".T(EDTx.UserControlScan_AllG), global::EDDiscovery.Icons.Controls.ShowAllG);
            displayfilter.AddStandardOption("habzone", "Show Habitation Zone".T(EDTx.UserControlScan_HabZone), global::EDDiscovery.Icons.Controls.ShowHabZone);
            displayfilter.AddStandardOption("starclass", "Show Classes of Stars".T(EDTx.UserControlScan_StarClass), global::EDDiscovery.Icons.Controls.ShowStarClasses);
            displayfilter.AddStandardOption("planetclass", "Show Classes of Planets".T(EDTx.UserControlScan_PlanetClass), global::EDDiscovery.Icons.Controls.ShowPlanetClasses);
            displayfilter.AddStandardOption("dist", "Show distance of bodies".T(EDTx.UserControlScan_Distance), global::EDDiscovery.Icons.Controls.ShowDistances);
            displayfilter.AddStandardOption("sys", "Show system and value in main display".T(EDTx.UserControlScan_SystemValue), global::EDDiscovery.Icons.Controls.Scan_DisplaySystemAlways);

            displayfilter.SaveSettings = (s, o) =>
            {
                displayfilters = s.Split(';');
                PutSetting("DisplayFilters", string.Join(";", displayfilters));
                ApplyDisplayFilters();
                DrawSystem();
            };

            displayfilter.CloseBoundaryRegion = new Size(32, extButtonDisplayFilters.Height);

            displayfilter.Show(string.Join(";", displayfilters), extButtonDisplayFilters, this.FindForm());
        }

        private void ApplyDisplayFilters()
        {
            bool all = displayfilters.Contains("All");
            panelStars.SystemDisplay.ShowMoons = displayfilters.Contains("moons") || all;
            panelStars.SystemDisplay.ShowOverlays = displayfilters.Contains("icons") || all;
            panelStars.SystemDisplay.ShowMaterials = displayfilters.Contains("mats") || all;
            panelStars.SystemDisplay.ShowOnlyMaterialsRare = displayfilters.Contains("rares") || all;
            panelStars.SystemDisplay.HideFullMaterials = displayfilters.Contains("matfull") || all;
            panelStars.SystemDisplay.ShowAllG = displayfilters.Contains("allg") || all;
            panelStars.SystemDisplay.ShowHabZone = displayfilters.Contains("habzone") || all;
            panelStars.SystemDisplay.ShowStarClasses = displayfilters.Contains("starclass") || all;
            panelStars.SystemDisplay.ShowPlanetClasses = displayfilters.Contains("planetclass") || all;
            panelStars.SystemDisplay.ShowDist = displayfilters.Contains("dist") || all;
        }

#region Export

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            if (showing_system == null)
                return;
            var sysnode = discoveryform.history.StarScan.FindSystemSynchronous(showing_system, false);

            if ( sysnode != null )
            {
                foreach (StarScan.ScanNode starnode in sysnode.StarNodes.Values)
                    StarScan.ScanNode.DumpTree(starnode, starnode.FullName, 0);

                var nodes = sysnode?.OrderedSystemTree();

                if (nodes != null)
                {

                    StarScan.ScanNode.DumpTree(nodes, "Top", 0);

                    Forms.ExportForm frm = new Forms.ExportForm();
                    frm.Init(false, new string[] {
                                        "JSON Orbital Parameters",
                                        },
                        outputext: new string[] { "JSON|*.json|All|*.*" },
                        showflags: new Forms.ExportForm.ShowFlags[] { Forms.ExportForm.ShowFlags.DTCVSOI },
                        suggestedfilenames: new string[] { last_he?.System.Name ?? "" }
                    );

                    if (frm.ShowDialog(FindForm()) == DialogResult.OK)
                    {
                        try
                        {
                            QuickJSON.JObject jobj = StarScan.ScanNode.DumpOrbitElements(nodes);
                            File.WriteAllText(frm.Path, jobj.ToString(true)); // failure will be picked up below
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Scan json " + ex);
                            ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to write to " + frm.Path, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }
        }

        #endregion

#if PLAYTHRU
        int hec = 20000;        // debug code to check all my scans don't crash - start point - 20000 is robs best position
        Timer t;
        private void T_Tick(object sender, EventArgs e)
        {
            while (hec < discoveryform.history.EntryOrder.Count)
            {
                HistoryEntry he = discoveryform.history.EntryOrder[hec++];
                if (he.System.Name != last_he.System.Name)
                {
                    last_he = he;
                    DrawSystem(he);
                    break;
                }
            }
        }

#endif

    }
}
