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

using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BaseUtils;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScan : UserControlCommonBase
    {
        private string DbSave { get { return DBName("ScanPanel"); } }

        HistoryEntry last_he = null;

        bool override_system = false;

        ISystem showing_system;                         // set from last_he or manually..
        MaterialCommoditiesList showing_matcomds;

        bool closing = false;           // set when closing, to prevent a resize, which you can get, causing a big redraw

        string[] bodyfilters;           // body filters
        string[] displayfilters;        // display filters

        #region Init
        public UserControlScan()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;            // we are dealing with graphics.. lets turn off dialog scaling.
            toolTip.ShowAlways = true;
        }

        public override void Init()
        {
            panelStars.CheckEDSM = checkBoxEDSM.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "EDSM", false);
            this.checkBoxEDSM.CheckedChanged += new System.EventHandler(this.checkBoxEDSM_CheckedChanged);

            bodyfilters = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbSave + "BodyFilters", "All").Split(';');

            displayfilters = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbSave + "DisplayFilters", "moons;icons;mats;allg;habzone;starclass;planetclass;dist;").Split(';');
            ApplyDisplayFilters();

            panelStars.ValueLimit = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbSave + "ValueLimit", 50000);

            rollUpPanelTop.PinState = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "PinState", true);

            int size = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbSave + "Size", 64);
            SetSizeImage(size);

            discoveryform.OnNewEntry += NewEntry;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

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
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "PinState", rollUpPanelTop.PinState );

            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            discoveryform.OnNewEntry -= NewEntry;
            closing = true;
        }

#endregion

#region Transparency
        Color transparencycolor = Color.Green;
        public override Color ColorTransparency { get { return transparencycolor; } }
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

                if (newspace < panelStars.DisplayAreaUsed.X || newspace > panelStars.DisplayAreaUsed.X +  panelStars.StarSize.Width)
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
                // new scan, new materials (for the count display), new SAA Signals all can cause display to change.
                if (he.EntryType == JournalTypeEnum.Scan || he.journalEntry is IMaterialJournalEntry || he.journalEntry is JournalSAASignalsFound ||
                    he.journalEntry is JournalFSSDiscoveryScan || 
                                last_he == null || last_he.System != he.System) //  or not presenting or diff sys
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

            showing_matcomds = he?.MaterialCommodity;
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
            StarScan.SystemNode data = showing_system != null ? await discoveryform.history.starscan.FindSystemAsync(showing_system, panelStars.CheckEDSM) : null;
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
                }
                else
                    control_text += " " + "No Scan".T(EDTx.NoScan);


            }

            panelStars.DrawSystem(data, showing_matcomds, discoveryform.history, (HasControlTextArea() && !displayfilters.Contains("sys")) ? null : control_text, bodyfilters);
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
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "EDSM", checkBoxEDSM.Checked);
            panelStars.CheckEDSM = checkBoxEDSM.Checked;
            DrawSystem();
        }

        private void extButtonHighValue_Click(object sender, EventArgs e)
        {
            ExtendedControls.ConfigurableForm cf = new ConfigurableForm();
            int width = 300;
            int height = 100;

            cf.Add(new ExtendedControls.ConfigurableForm.Entry("UC", typeof(ExtendedControls.NumberBoxLong), panelStars.ValueLimit.ToStringInvariant(),
                                        new Point(5, 30), new Size(width - 5 - 20, 24), null)
            { numberboxlongminimum = 1, numberboxlongmaximum = 2000000000 });

            cf.Add(new ExtendedControls.ConfigurableForm.Entry("OK", typeof(ExtendedControls.ExtButton), "OK".T(EDTx.OK),
                        new Point(width - 20 - 80, height - 40), new Size(80, 24), ""));

            cf.Trigger += (dialogname, controlname, tag) =>
            {
                System.Diagnostics.Debug.WriteLine("control" + controlname);

                if (controlname.Contains("Validity:False"))
                    cf.SetEnabled("OK", false);
                else if (controlname.Contains("Validity:True"))
                    cf.SetEnabled("OK", true);
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
                panelStars.ValueLimit = (int)value.Value;
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DbSave + "ValueLimit", panelStars.ValueLimit);
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

            EDDTheme.Instance.ApplyStd(dropdown,true);

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

            panelStars.SetSize(size);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DbSave + "Size", size);
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
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbSave + "BodyFilters", string.Join(";", bodyfilters));
                DrawSystem();
            };

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
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbSave + "DisplayFilters", string.Join(";", displayfilters));
                ApplyDisplayFilters();
                DrawSystem();
            };

            displayfilter.Show(string.Join(";", displayfilters), extButtonDisplayFilters, this.FindForm());
        }

        private void ApplyDisplayFilters()
        {
            bool all = displayfilters.Contains("All");
            panelStars.ShowMoons = displayfilters.Contains("moons") || all;
            panelStars.ShowOverlays = displayfilters.Contains("icons") || all;
            panelStars.ShowMaterials = displayfilters.Contains("mats") || all;
            panelStars.ShowOnlyMaterialsRare = displayfilters.Contains("rares") || all;
            panelStars.HideFullMaterials = displayfilters.Contains("matfull") || all;
            panelStars.ShowAllG = displayfilters.Contains("allg") || all;
            panelStars.ShowHabZone = displayfilters.Contains("habzone") || all;
            panelStars.ShowStarClasses = displayfilters.Contains("starclass") || all;
            panelStars.ShowPlanetClasses = displayfilters.Contains("planetclass") || all;
            panelStars.ShowDist = displayfilters.Contains("dist") || all;
        }

#region Export

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(new string[] { "All",
                                    "Stars only",
                                    "Planets only", //2
                                    "Exploration List Stars", //3
                                    "Exploration List Planets", //4
                                    "Ring Scans", //5
                                        });

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                BaseUtils.CSVWrite csv = new BaseUtils.CSVWrite();
                csv.SetCSVDelimiter(frm.Comma);

                try
                {
                    if (frm.SelectedIndex == 5)
                    {
                        var saaentries = JournalEntry.GetByEventType(JournalTypeEnum.SAASignalsFound, EDCommander.CurrentCmdrID, frm.StartTimeUTC, frm.EndTimeUTC).ConvertAll(x => (JournalSAASignalsFound)x);
                        var scanentries = JournalEntry.GetByEventType(JournalTypeEnum.Scan, EDCommander.CurrentCmdrID, frm.StartTimeUTC, frm.EndTimeUTC).ConvertAll(x => (JournalScan)x);

                        BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                        grd.SetCSVDelimiter(frm.Comma);

                        string[] headers1 = { "", "", "", "", "","",
                            "Icy Ring" , "","","","","","","","","",
                            "Rocky","","","","","","","",
                            "Metal Rich","","","","",
                            "Metalic","","","","",
                            "Other"
                        };

                        string[] headers2 = { "Time", "BodyName", "Ring Types", "Mass MT", "Inner Rad (ls)","Outer Rad (ls)",

                            // icy ring
                            "Water", "Liquid Oxygen", "Methanol Mono", "Methane","Bromellite", "Grandidierite", "Low Temp Diamonds", "Void Opals","Alexandrite" , "Tritium",
                            // rocky
                            "Bauxite","Indite","Alexandrite","Monazite","Musgravite","Benitoite","Serendibite","Rhodplumsite",
                            // metal rich
                            "Rhodplumsite","Serendibite","Platinum","Monazite","Painite",
                            // metalic
                            "Rhodplumsite","Serendibite","Platinum","Monazite","Painite",
                            // others
                            "Geological","Biological","Thargoid","Human","Guardian",
                        };

                        string[] fdname = { "Water", "LiquidOxygen", "methanolmonohydratecrystals", "MethaneClathrate",     // icy
                                            "Bromellite", "Grandidierite", "lowtemperaturediamond", "Opal",
                                            "Alexandrite", "Tritium",
                                            "Bauxite","Indite","Alexandrite","Monazite","Musgravite","Benitoite","Serendibite","Rhodplumsite",          // rocky
                                            "Rhodplumsite","Serendibite","Platinum","Monazite","Painite",   // metal rich  
                                            "Rhodplumsite","Serendibite","Platinum","Monazite","Painite", // metalic
                                            "$SAA_SignalType_Geological;","$SAA_SignalType_Biological;","$SAA_SignalType_Thargoid;","$SAA_SignalType_Human;","$SAA_SignalType_Guardian;"
                                          };

                        grd.GetLineHeader = (row) => { return row == 1 ? headers2 : row == 0 ? headers1 : null; };
                        grd.GetLineStatus = (row) =>
                        {
                            if (row < saaentries.Count)
                            {
                                for (int rp = row + 1; rp < saaentries.Count; rp++)
                                {
                                    if (saaentries[rp].BodyName == saaentries[row].BodyName && (saaentries[rp].EventTimeUTC - saaentries[row].EventTimeUTC) < new TimeSpan(30, 0, 0, 0))
                                        return CSVWriteGrid.LineStatus.Skip; // if matches one in front, and its less than 30 days from it, ignore
                                }

                                return CSVWriteGrid.LineStatus.OK;
                            }
                            else
                                return CSVWriteGrid.LineStatus.EOF;
                        };

                        grd.GetLine = (r) =>
                        {
                            var entry = saaentries[r];
                            string signals = string.Join(",", entry.Signals.Select(x => x.Type_Localised));

                            JournalScan scanof = scanentries.Find(x => x.FindRing(entry.BodyName) != null);

                            bool showrocky = true, showmr = true, showmetalic = true, showicy = true;       // only used if item appears more than once below

                            string ringtype = "", mass = "", innerrad = "",outerrad = "";
                            if (scanof != null)
                            {
                                var ri = scanof.FindRing(entry.BodyName);
                                ringtype = ri.RingClassNormalised();
                                if (ringtype.Contains("metalic", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    showicy = showrocky = showmr = false;
                                }
                                else if (ringtype.Contains("Metal", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    showicy = showrocky = showmetalic = false;
                                }
                                else if (ringtype.Contains("rocky", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    showicy = showmetalic = showmr = false;
                                }
                                else if (ringtype.Contains("icy", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    showrocky= showmetalic = showmr = false;
                                }

                                mass = ri.MassMT.ToStringInvariant();
                                innerrad = (ri.InnerRad / JournalScan.oneLS_m).ToStringInvariant("N3");
                                outerrad = (ri.OuterRad / JournalScan.oneLS_m).ToStringInvariant("N3");
                            }

                            //string sig = string.Join(",", entry.Signals.Select(x=>x.Type)); // debug

                            return new object[] { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(entry.EventTimeUTC), entry.BodyName, ringtype,
                                mass,innerrad,outerrad ,
                                entry.ContainsStr(fdname[0]),entry.ContainsStr(fdname[1]),entry.ContainsStr(fdname[2]),entry.ContainsStr(fdname[3]),
                                entry.ContainsStr(fdname[4]),entry.ContainsStr(fdname[5]),entry.ContainsStr(fdname[6]),entry.ContainsStr(fdname[7]),
                                entry.ContainsStr(fdname[8], showicy), entry.ContainsStr(fdname[9]),

                                // "Bauxite","Indite","Alexandrite","Monazite", // rocky
                                entry.ContainsStr(fdname[10]),entry.ContainsStr(fdname[11]),entry.ContainsStr(fdname[12],showrocky),entry.ContainsStr(fdname[13], showrocky),
                                // "Musgravite","Benitoite","Serendibite","Rhodplumsite",          // rocky
                                entry.ContainsStr(fdname[14]),entry.ContainsStr(fdname[15]),entry.ContainsStr(fdname[16],showrocky),entry.ContainsStr(fdname[17],showrocky),

                                // "Rhodplumsite","Serendibite","Platinum","Monazite","Painite",   // metal rich  
                                entry.ContainsStr(fdname[18],showmr),entry.ContainsStr(fdname[19],showmr),entry.ContainsStr(fdname[20],showmr),entry.ContainsStr(fdname[21],showmr),entry.ContainsStr(fdname[22],showmr),

                                // "Rhodplumsite","Serendibite","Platinum","Monazite","Painite", // metalic
                                entry.ContainsStr(fdname[23],showmetalic),entry.ContainsStr(fdname[24],showmetalic),entry.ContainsStr(fdname[25],showmetalic),entry.ContainsStr(fdname[26],showmetalic),entry.ContainsStr(fdname[27],showmetalic),

                                entry.ContainsStr(fdname[28]),entry.ContainsStr(fdname[29]),entry.ContainsStr(fdname[30]),entry.ContainsStr(fdname[31]),entry.ContainsStr(fdname[32]),
                            };
                        };

                        grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                    }
                    else
                    { 
                        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(frm.Path))
                        {
                            List<JournalScan> scans = null;
                            
                            if (frm.SelectedIndex < 3)
                            {
                                var entries = JournalEntry.GetByEventType(JournalTypeEnum.Scan, EDCommander.CurrentCmdrID, frm.StartTimeUTC, frm.EndTimeUTC);
                                scans = entries.ConvertAll(x => (JournalScan)x);
                            }
                            else
                            {
                                ExplorationSetClass currentExplorationSet = new ExplorationSetClass();

                                string file = currentExplorationSet.DialogLoad(FindForm());

                                if (file != null)
                                {
                                    scans = new List<JournalScan>();

                                    foreach (string system in currentExplorationSet.Systems)
                                    {
                                        ISystem sys = SystemCache.FindSystem(system);
                                        if (sys != null)
                                        {
                                            var jl = EDSMClass.GetBodiesList(sys);
                                            List<JournalScan> sysscans = jl.Item1;
                                            if (sysscans != null)
                                                scans.AddRange(sysscans);
                                        }
                                    }
                                }
                                else
                                    return;
                            }

                            bool ShowStars = frm.SelectedIndex < 2 || frm.SelectedIndex == 3;
                            bool ShowPlanets = frm.SelectedIndex == 0 || frm.SelectedIndex == 2 || frm.SelectedIndex == 4;
                            bool ShowBeltClusters = frm.SelectedIndex == 0;

                            List<JournalSAAScanComplete> mappings = ShowPlanets ?
                                                        JournalEntry.GetByEventType(JournalTypeEnum.SAAScanComplete, EDCommander.CurrentCmdrID, frm.StartTimeUTC, frm.EndTimeUTC).ConvertAll(x => (JournalSAAScanComplete)x)
                                                        : null;

                            if (frm.IncludeHeader)
                            {
                                // Write header

                                writer.Write(csv.Format("Time"));
                                writer.Write(csv.Format("BodyName"));
                                writer.Write(csv.Format("Estimated Value"));
                                writer.Write(csv.Format("DistanceFromArrivalLS"));
                                writer.Write(csv.Format("WasMapped"));
                                writer.Write(csv.Format("WasDiscovered"));
                                if (ShowStars)
                                {
                                    writer.Write(csv.Format("StarType"));
                                    writer.Write(csv.Format("StellarMass"));
                                    writer.Write(csv.Format("AbsoluteMagnitude"));
                                    writer.Write(csv.Format("Age MY"));
                                    writer.Write(csv.Format("Luminosity"));
                                }
                                writer.Write(csv.Format("Radius"));
                                writer.Write(csv.Format("RotationPeriod"));
                                writer.Write(csv.Format("SurfaceTemperature"));

                                if (ShowPlanets)
                                {
                                    writer.Write(csv.Format("TidalLock"));
                                    writer.Write(csv.Format("TerraformState"));
                                    writer.Write(csv.Format("PlanetClass"));
                                    writer.Write(csv.Format("Atmosphere"));
                                    writer.Write(csv.Format("Iron"));
                                    writer.Write(csv.Format("Silicates"));
                                    writer.Write(csv.Format("SulphurDioxide"));
                                    writer.Write(csv.Format("CarbonDioxide"));
                                    writer.Write(csv.Format("Nitrogen"));
                                    writer.Write(csv.Format("Oxygen"));
                                    writer.Write(csv.Format("Water"));
                                    writer.Write(csv.Format("Argon"));
                                    writer.Write(csv.Format("Ammonia"));
                                    writer.Write(csv.Format("Methane"));
                                    writer.Write(csv.Format("Hydrogen"));
                                    writer.Write(csv.Format("Helium"));
                                    writer.Write(csv.Format("Volcanism"));
                                    writer.Write(csv.Format("SurfaceGravity"));
                                    writer.Write(csv.Format("SurfacePressure"));
                                    writer.Write(csv.Format("Landable"));
                                    writer.Write(csv.Format("EarthMasses"));
                                    writer.Write(csv.Format("IcePercent"));
                                    writer.Write(csv.Format("RockPercent"));
                                    writer.Write(csv.Format("MetalPercent"));
                                }
                                // Common orbital param
                                writer.Write(csv.Format("SemiMajorAxis"));
                                writer.Write(csv.Format("Eccentricity"));
                                writer.Write(csv.Format("OrbitalInclination"));
                                writer.Write(csv.Format("Periapsis"));
                                writer.Write(csv.Format("OrbitalPeriod"));
                                writer.Write(csv.Format("AxialTilt"));


                                if (ShowPlanets)
                                {
                                    writer.Write(csv.Format("Carbon"));
                                    writer.Write(csv.Format("Iron"));
                                    writer.Write(csv.Format("Nickel"));
                                    writer.Write(csv.Format("Phosphorus"));
                                    writer.Write(csv.Format("Sulphur"));
                                    writer.Write(csv.Format("Arsenic"));
                                    writer.Write(csv.Format("Chromium"));
                                    writer.Write(csv.Format("Germanium"));
                                    writer.Write(csv.Format("Manganese"));
                                    writer.Write(csv.Format("Selenium"));
                                    writer.Write(csv.Format("Vanadium"));
                                    writer.Write(csv.Format("Zinc"));
                                    writer.Write(csv.Format("Zirconium"));
                                    writer.Write(csv.Format("Cadmium"));
                                    writer.Write(csv.Format("Mercury"));
                                    writer.Write(csv.Format("Molybdenum"));
                                    writer.Write(csv.Format("Niobium"));
                                    writer.Write(csv.Format("Tin"));
                                    writer.Write(csv.Format("Tungsten"));
                                    writer.Write(csv.Format("Antimony"));
                                    writer.Write(csv.Format("Polonium"));
                                    writer.Write(csv.Format("Ruthenium"));
                                    writer.Write(csv.Format("Technetium"));
                                    writer.Write(csv.Format("Tellurium"));
                                    writer.Write(csv.Format("Yttrium"));
                                }

                                writer.WriteLine();
                            }

                            foreach (JournalScan je in scans)
                            {
                                JournalScan scan = je as JournalScan;

                                if ((je.IsPlanet && ShowPlanets) || (je.IsStar && ShowStars) || (je.IsBeltCluster && ShowBeltClusters))
                                {
                                    if (je.IsPlanet)
                                    {
                                        var mapping = mappings?.FirstOrDefault(m => m.BodyID == scan.BodyID && m.BodyName == scan.BodyName);

                                        if (mapping != null)
                                        {
                                            scan.SetMapped(true, mapping.ProbesUsed <= mapping.EfficiencyTarget);
                                        }
                                    }

                                    writer.Write(csv.Format(EDDConfig.Instance.ConvertTimeToSelectedFromUTC(scan.EventTimeUTC)));
                                    writer.Write(csv.Format(scan.BodyName));
                                    writer.Write(csv.Format(scan.EstimatedValue));
                                    writer.Write(csv.Format(scan.DistanceFromArrivalLS));
                                    writer.Write(csv.Format(scan.WasMapped));
                                    writer.Write(csv.Format(scan.WasDiscovered));

                                    if (ShowStars)
                                    {
                                        writer.Write(csv.Format(scan.StarType));
                                        writer.Write(csv.Format((scan.nStellarMass.HasValue) ? scan.nStellarMass.Value : 0));
                                        writer.Write(csv.Format((scan.nAbsoluteMagnitude.HasValue) ? scan.nAbsoluteMagnitude.Value : 0));
                                        writer.Write(csv.Format((scan.nAge.HasValue) ? scan.nAge.Value : 0));
                                        writer.Write(csv.Format(scan.Luminosity));
                                    }


                                    writer.Write(csv.Format(scan.nRadius.HasValue ? scan.nRadius.Value : 0));
                                    writer.Write(csv.Format(scan.nRotationPeriod.HasValue ? scan.nRotationPeriod.Value : 0));
                                    writer.Write(csv.Format(scan.nSurfaceTemperature.HasValue ? scan.nSurfaceTemperature.Value : 0));

                                    if (ShowPlanets)
                                    {
                                        writer.Write(csv.Format(scan.nTidalLock.HasValue ? scan.nTidalLock.Value : false));
                                        writer.Write(csv.Format((scan.TerraformState != null) ? scan.TerraformState : ""));
                                        writer.Write(csv.Format((scan.PlanetClass != null) ? scan.PlanetClass : ""));
                                        writer.Write(csv.Format((scan.Atmosphere != null) ? scan.Atmosphere : ""));
                                        writer.Write(csv.Format(scan.GetAtmosphereComponent("Iron")));
                                        writer.Write(csv.Format(scan.GetAtmosphereComponent("Silicates")));
                                        writer.Write(csv.Format(scan.GetAtmosphereComponent("SulphurDioxide")));
                                        writer.Write(csv.Format(scan.GetAtmosphereComponent("CarbonDioxide")));
                                        writer.Write(csv.Format(scan.GetAtmosphereComponent("Nitrogen")));
                                        writer.Write(csv.Format(scan.GetAtmosphereComponent("Oxygen")));
                                        writer.Write(csv.Format(scan.GetAtmosphereComponent("Water")));
                                        writer.Write(csv.Format(scan.GetAtmosphereComponent("Argon")));
                                        writer.Write(csv.Format(scan.GetAtmosphereComponent("Ammonia")));
                                        writer.Write(csv.Format(scan.GetAtmosphereComponent("Methane")));
                                        writer.Write(csv.Format(scan.GetAtmosphereComponent("Hydrogen")));
                                        writer.Write(csv.Format(scan.GetAtmosphereComponent("Helium")));
                                        writer.Write(csv.Format((scan.Volcanism != null) ? scan.Volcanism : ""));
                                        writer.Write(csv.Format(scan.nSurfaceGravity.HasValue ? scan.nSurfaceGravity.Value : 0));
                                        writer.Write(csv.Format(scan.nSurfacePressure.HasValue ? scan.nSurfacePressure.Value : 0));
                                        writer.Write(csv.Format(scan.nLandable.HasValue ? scan.nLandable.Value : false));
                                        writer.Write(csv.Format((scan.nMassEM.HasValue) ? scan.nMassEM.Value : 0));
                                        writer.Write(csv.Format(scan.GetCompositionPercent("Ice")));
                                        writer.Write(csv.Format(scan.GetCompositionPercent("Rock")));
                                        writer.Write(csv.Format(scan.GetCompositionPercent("Metal")));
                                    }
                                    // Common orbital param
                                    writer.Write(csv.Format(scan.nSemiMajorAxis.HasValue ? scan.nSemiMajorAxis.Value : 0));
                                    writer.Write(csv.Format(scan.nEccentricity.HasValue ? scan.nEccentricity.Value : 0));
                                    writer.Write(csv.Format(scan.nOrbitalInclination.HasValue ? scan.nOrbitalInclination.Value : 0));
                                    writer.Write(csv.Format(scan.nPeriapsis.HasValue ? scan.nPeriapsis.Value : 0));
                                    writer.Write(csv.Format(scan.nOrbitalPeriod.HasValue ? scan.nOrbitalPeriod.Value : 0));
                                    writer.Write(csv.Format(scan.nAxialTilt.HasValue ? scan.nAxialTilt : null));

                                    if (ShowPlanets)
                                    {
                                        writer.Write(csv.Format(scan.GetMaterial("Carbon")));
                                        writer.Write(csv.Format(scan.GetMaterial("Iron")));
                                        writer.Write(csv.Format(scan.GetMaterial("Nickel")));
                                        writer.Write(csv.Format(scan.GetMaterial("Phosphorus")));
                                        writer.Write(csv.Format(scan.GetMaterial("Sulphur")));
                                        writer.Write(csv.Format(scan.GetMaterial("Arsenic")));
                                        writer.Write(csv.Format(scan.GetMaterial("Chromium")));
                                        writer.Write(csv.Format(scan.GetMaterial("Germanium")));
                                        writer.Write(csv.Format(scan.GetMaterial("Manganese")));
                                        writer.Write(csv.Format(scan.GetMaterial("Selenium")));
                                        writer.Write(csv.Format(scan.GetMaterial("Vanadium")));
                                        writer.Write(csv.Format(scan.GetMaterial("Zinc")));
                                        writer.Write(csv.Format(scan.GetMaterial("Zirconium")));
                                        writer.Write(csv.Format(scan.GetMaterial("Cadmium")));
                                        writer.Write(csv.Format(scan.GetMaterial("Mercury")));
                                        writer.Write(csv.Format(scan.GetMaterial("Molybdenum")));
                                        writer.Write(csv.Format(scan.GetMaterial("Niobium")));
                                        writer.Write(csv.Format(scan.GetMaterial("Tin")));
                                        writer.Write(csv.Format(scan.GetMaterial("Tungsten")));
                                        writer.Write(csv.Format(scan.GetMaterial("Antimony")));
                                        writer.Write(csv.Format(scan.GetMaterial("Polonium")));
                                        writer.Write(csv.Format(scan.GetMaterial("Ruthenium")));
                                        writer.Write(csv.Format(scan.GetMaterial("Technetium")));
                                        writer.Write(csv.Format(scan.GetMaterial("Tellurium")));
                                        writer.Write(csv.Format(scan.GetMaterial("Yttrium")));
                                    }
                                    writer.WriteLine();
                                }
                            }

                            writer.Close();
                        }

                        if (frm.AutoOpen)
                            System.Diagnostics.Process.Start(frm.Path);
                    }
                }
                catch
                {
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to write to " + frm.Path, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
