/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.Threading.Tasks;
using System.Windows.Forms;
using ExtendedControls;
using System.Drawing.Drawing2D;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.DB;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScan : UserControlCommonBase
    {
        private string DbSave { get { return DBName("ScanPanel" ); } }

        HistoryEntry last_he = null;

        bool override_system = false;

        ISystem showing_system;                         // set from last_he or manually..
        MaterialCommoditiesList showing_matcomds;

        bool closing = false;           // set when closing, to prevent a resize, which you can get, causing a big redraw

        #region Init
        public UserControlScan()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;            // we are dealing with graphics.. lets turn off dialog scaling.
            toolTip.ShowAlways = true;
        }

        public override void Init()
        {
            progchange = true;
            panelStars.ShowMaterials = checkBoxMaterials.Checked = SQLiteDBClass.GetSettingBool(DbSave + "Materials", true);
            panelStars.ShowMaterialsRare = checkBoxMaterialsRare.Checked = SQLiteDBClass.GetSettingBool(DbSave + "MaterialsRare", false);
            panelStars.ShowMoons = checkBoxMoons.Checked = SQLiteDBClass.GetSettingBool(DbSave + "Moons", true);
            panelStars.CheckEDSM = checkBoxEDSM.Checked = SQLiteDBClass.GetSettingBool(DbSave + "EDSM", false);
            panelStars.HideFullMaterials = checkBoxCustomHideFullMats.Checked = SQLiteDBClass.GetSettingBool(DbSave + "MaterialsFull", false);
            panelStars.ShowOverlays = chkShowOverlays.Checked = SQLiteDBClass.GetSettingBool(DbSave + "BodyOverlays", false);
            panelStars.ValueLimit = SQLiteDBClass.GetSettingInt(DbSave + "ValueLimit", 50000);
            progchange = false;

            rollUpPanelTop.PinState = SQLiteConnectionUser.GetSettingBool(DbSave + "PinState", true);

            int size = SQLiteDBClass.GetSettingInt(DbSave + "Size", 64);
            SetSizeImage(size);

            discoveryform.OnNewEntry += NewEntry;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            SQLiteConnectionUser.PutSettingBool(DbSave + "PinState", rollUpPanelTop.PinState );
            uctg.OnTravelSelectionChanged -= Display;
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
        }

        private void UserControlScan_Resize(object sender, EventArgs e)
        {
            //PositionInfo();
            //System.Diagnostics.Debug.WriteLine("Resize panel stars {0} {1}", DisplayRectangle, panelStars.Size);

            if (!closing && last_he != null)
            {
                int newspace = panelStars.WidthAvailable;

                if (newspace < panelStars.DisplayAreaUsed.X || newspace > panelStars.DisplayAreaUsed.X +  panelStars.StarSize.Width * 2)
                {
                    DrawSystem(last_he);
                }
            }
        }

        #endregion

        #region Display

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made.. check to see if its a scan update
        {
            // if he valid, and last is null, or not he, or we have a new scan
            if (he != null && (last_he == null || he != last_he || he.EntryType == JournalTypeEnum.Scan))
            {
                last_he = he;
                DrawSystem(last_he);
            }
        }

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        private void Display(HistoryEntry he, HistoryList hl) =>
            Display(he, hl, true);

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)            // when user clicks around..
        {
            if (he != null && (last_he == null || he.System != last_he.System))
            {
                last_he = he;
                DrawSystem(last_he);
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

        void DrawSystem()   // draw showing_system (may be null), showing_matcomds (may be null)
        {
            panelStars.HideInfo();

            StarScan.SystemNode data = panelStars.DrawSystem(showing_system, showing_matcomds, discoveryform.history);

            if (showing_system == null)
            {
                SetControlText("No System");
            }
            else
            {
                if (data != null)
                {
                    long value = data.ScanValue(checkBoxEDSM.Checked);
                    SetControlText(data.system.Name + " (~" + value.ToString() + " cr)");
                }
                else
                    SetControlText(data == null ? "No Scan".Tx() : data.system.Name);
            }
        }

        #endregion

        #region User interaction

        private void extCheckBoxStar_Click(object sender, EventArgs e)
        {
            if ( extCheckBoxStar.Checked == true )
            {
                ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
                int width = 700;
                f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "System:".Tx(this), new Point(10, 40), new Size(160, 24), null));
                f.Add(new ExtendedControls.ConfigurableForm.Entry("Sys", typeof(ExtendedControls.ExtTextBoxAutoComplete), "", new Point(180, 40), new Size(width - 180 - 20, 24), null));

                f.Add(new ExtendedControls.ConfigurableForm.Entry("OK", typeof(ExtendedControls.ExtButton), "OK".Tx(), new Point(width - 20 - 80, 80), new Size(80, 24), ""));
                f.Add(new ExtendedControls.ConfigurableForm.Entry("Cancel", typeof(ExtendedControls.ExtButton), "Cancel".Tx(), new Point(width - 200, 80), new Size(80, 24), ""));

                f.Trigger += (dialogname, controlname, tag) =>
                {
                    if (controlname == "OK" || controlname == "Cancel")
                    {
                        f.DialogResult = controlname == "OK" ? DialogResult.OK : DialogResult.Cancel;
                        f.Close();
                    }
                };

                f.Init(this.FindForm().Icon, new Size(width, 120), new Point(-999, -999), "Show System".Tx(this, "EnterSys"), null, null);
                f.GetControl<ExtendedControls.ExtTextBoxAutoComplete>("Sys").SetAutoCompletor(SystemClassDB.ReturnOnlySystemsListForAutoComplete, true);
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


        bool progchange = false;

        private void checkBoxMaterials_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
            {
                SQLiteDBClass.PutSettingBool(DbSave + "Materials", checkBoxMaterials.Checked);
                panelStars.ShowMaterials = checkBoxMaterials.Checked;
                DrawSystem();
            }
        }

        private void checkBoxMaterialsRare_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
            {
                SQLiteDBClass.PutSettingBool(DbSave + "MaterialsRare", checkBoxMaterialsRare.Checked);

                progchange = true;
                checkBoxMaterials.Checked = true;
                panelStars.ShowMaterials = checkBoxMaterials.Checked;
                panelStars.ShowMaterialsRare = checkBoxMaterialsRare.Checked;
                progchange = false;

                DrawSystem();
            }
        }

        private void checkBoxCustomHideFullMats_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
            {
                SQLiteDBClass.PutSettingBool(DbSave + "MaterialsFull", checkBoxCustomHideFullMats.Checked);
                panelStars.HideFullMaterials = checkBoxCustomHideFullMats.Checked;
                DrawSystem();
            }

        }

        private void checkBoxMoons_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
            {
                SQLiteDBClass.PutSettingBool(DbSave + "Moons", checkBoxMoons.Checked);
                panelStars.ShowMoons = checkBoxMoons.Checked;
                DrawSystem();
            }
        }

        private void checkBoxEDSM_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
            {
                SQLiteDBClass.PutSettingBool(DbSave + "EDSM", checkBoxEDSM.Checked);
                panelStars.CheckEDSM = checkBoxEDSM.Checked;
                DrawSystem();
            }
        }

        private void chkShowOverlays_CheckedChanged(object sender, EventArgs e)
        {
            if (!progchange)
            {
                SQLiteDBClass.PutSettingBool(DbSave + "BodyOverlays", chkShowOverlays.Checked);
                panelStars.ShowOverlays = chkShowOverlays.Checked;
                DrawSystem();
            }
        }

        private void extButtonHighValue_Click(object sender, EventArgs e)
        {
            ExtendedControls.ConfigurableForm cf = new ConfigurableForm();
            int width = 300;
            int height = 100;

            cf.Add(new ExtendedControls.ConfigurableForm.Entry("UC", typeof(ExtendedControls.NumberBoxLong), panelStars.ValueLimit.ToStringInvariant(),
                                        new Point(5, 30), new Size(width - 5 - 20, 24), null)
            { numberboxlongminimum = 1, numberboxlongmaximum = 2000000000 });

            cf.Add(new ExtendedControls.ConfigurableForm.Entry("OK", typeof(ExtendedControls.ExtButton), "OK".Tx(),
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
                    cf.DialogResult = DialogResult.OK;
                    cf.Close();
                }
            };

            if (cf.ShowDialog(this.FindForm(), this.FindForm().Icon, new Size(width, height), new Point(-999, -999), "Set Valuable Minimum".Tx(this, "VLMT")) == DialogResult.OK)
            {
                long? value = cf.GetLong("UC");
                panelStars.ValueLimit = (int)value.Value;
                SQLiteDBClass.PutSettingInt(DbSave + "ValueLimit", panelStars.ValueLimit);
                DrawSystem();
            }
        }

        ExtListBoxForm dropdown;

        private void buttonSize_Click(object sender, EventArgs e)
        {
            // 128,96,64,48
            dropdown = new ExtListBoxForm("", true);

            Image[] imagelist = new Image[] { global::EDDiscovery.Icons.Controls.Scan_SizeLarge ,
                global::EDDiscovery.Icons.Controls.Scan_SizeMedium ,
                global::EDDiscovery.Icons.Controls.Scan_SizeSmall ,
                global::EDDiscovery.Icons.Controls.Scan_SizeTiny ,
                global::EDDiscovery.Icons.Controls.Scan_SizeTinyTiny ,
                global::EDDiscovery.Icons.Controls.Scan_SizeMinuscule ,
            };

            string[] textlist = new string[] { "128", "96", "64", "48", "32", "16" };

            dropdown.ItemHeight = imagelist[0].Size.Height + 2;
            dropdown.Items = textlist.ToList();
            dropdown.ImageItems = imagelist.ToList();
            dropdown.FlatStyle = FlatStyle.Popup;
            dropdown.Activated += (s, ea) =>
            {
                Point location = buttonSize.PointToScreen(new Point(0, 0));
                dropdown.Location = dropdown.PositionWithinScreen(location.X, location.Y);
                this.Invalidate(true);
            };
            dropdown.SelectedIndexChanged += (s, ea) =>
            {
                int size = textlist[dropdown.SelectedIndex].InvariantParseInt(64);
                SetSizeImage(size);
                DrawSystem();
            };

            dropdown.Size = new Size(64, dropdown.ItemHeight * textlist.Length + 8);

            EDDTheme.Instance.ApplyToControls(dropdown);

            dropdown.Show(this.FindForm());
        }

        private void SetSizeImage(int size)
        {
            if (size == 128)
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
            SQLiteDBClass.PutSettingInt(DbSave + "Size", size);
        }

        #endregion

        #region Export

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(new string[] { "All", "Stars only",
                                    "Planets only", //2
                                    "Exploration List Stars", //3
                                    "Exploration List Planets", //4
                                    "Sold Exploration Data", // 5
                                        });

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                BaseUtils.CSVWrite csv = new BaseUtils.CSVWrite();
                csv.SetCSVDelimiter(frm.Comma);

                try
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(frm.Path))
                    {
                        if (frm.SelectedIndex == 5)
                        {
                            int count;
                            List<HistoryEntry> data = HistoryList.FilterByJournalEvent(discoveryform.history.ToList(), "Sell Exploration Data", out count);
                            data = (from he in data where he.EventTimeLocal >= frm.StartTime && he.EventTimeLocal <= frm.EndTime orderby he.EventTimeUTC descending select he).ToList();

                            List<HistoryEntry> scans = HistoryList.FilterByJournalEvent(discoveryform.history.ToList(), "Scan", out count);

                            if (frm.IncludeHeader)
                            {
                                writer.Write(csv.Format("Time"));
                                writer.Write(csv.Format("System"));
                                writer.Write(csv.Format("Star type"));
                                writer.Write(csv.Format("Planet type", false));
                                writer.WriteLine();
                            }

                            foreach (HistoryEntry he in data)
                            {
                                JournalSellExplorationData jsed = he.journalEntry as JournalSellExplorationData;
                                if (jsed == null || jsed.Discovered == null)
                                    continue;
                                foreach (String system in jsed.Discovered)
                                {
                                    writer.Write(csv.Format(jsed.EventTimeLocal));
                                    writer.Write(csv.Format(system));

                                    EDStar star = EDStar.Unknown;
                                    EDPlanet planet = EDPlanet.Unknown;

                                    foreach (HistoryEntry scanhe in scans)
                                    {
                                        JournalScan scan = scanhe.journalEntry as JournalScan;
                                        if (scan.BodyName.Equals(system, StringComparison.OrdinalIgnoreCase))
                                        {
                                            star = scan.StarTypeID;
                                            planet = scan.PlanetTypeID;
                                            break;
                                        }
                                    }
                                    writer.Write(csv.Format((star != EDStar.Unknown) ? Enum.GetName(typeof(EDStar), star) : ""));
                                    writer.Write(csv.Format((planet != EDPlanet.Unknown) ? Enum.GetName(typeof(EDPlanet), planet) : "", false));
                                    writer.WriteLine();
                                }
                            }
                        }
                        else
                        {
                            List<JournalScan> scans = null;
                            
                            if (frm.SelectedIndex < 3)
                            {
                                var entries = JournalEntry.GetByEventType(JournalTypeEnum.Scan, EDCommander.CurrentCmdrID, frm.StartTime, frm.EndTime);
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
                                        List<long> edsmidlist = SystemClassDB.GetEdsmIdsFromName(system);

                                        if (edsmidlist.Count > 0)
                                        {
                                            for (int ii = 0; ii < edsmidlist.Count; ii++)
                                            {
                                                List<JournalScan> sysscans = EDSMClass.GetBodiesList((int)edsmidlist[ii]);
                                                if (sysscans != null)
                                                    scans.AddRange(sysscans);
                                            }
                                        }
                                    }
                                }
                                else
                                    return;
                            }

                            bool ShowStars = frm.SelectedIndex < 2 || frm.SelectedIndex == 3;
                            bool ShowPlanets = frm.SelectedIndex == 0 || frm.SelectedIndex == 2 || frm.SelectedIndex == 4;

                            List<JournalSAAScanComplete> mappings = ShowPlanets ? 
                                JournalEntry.GetByEventType(JournalTypeEnum.SAAScanComplete, EDCommander.CurrentCmdrID, frm.StartTime, frm.EndTime)
                                .ConvertAll(x => (JournalSAAScanComplete)x)
                                : null;

                            if (frm.IncludeHeader)
                            {
                                // Write header

                                writer.Write(csv.Format("Time"));
                                writer.Write(csv.Format("BodyName"));
                                writer.Write(csv.Format("Estimated Value"));
                                writer.Write(csv.Format("DistanceFromArrivalLS"));
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

                                if (ShowPlanets == false)  // Then only show stars.
                                    if (String.IsNullOrEmpty(scan.StarType))
                                        continue;

                                if (ShowStars == false)   // Then only show planets
                                    if (String.IsNullOrEmpty(scan.PlanetClass))
                                        continue;

                                writer.Write(csv.Format(scan.EventTimeUTC));
                                writer.Write(csv.Format(scan.BodyName));
                                writer.Write(csv.Format(scan.EstimatedValue));
                                writer.Write(csv.Format(scan.DistanceFromArrivalLS));

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

    }
}

