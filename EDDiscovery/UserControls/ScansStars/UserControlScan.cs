/*
 * Copyright 2016 - 2025 EDDiscovery development team
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

// turn on for play testing of all your scans
//#define PLAYTHRU

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
        const string dbValueLimit = "ValueLimit";

        #region Init
        public UserControlScan()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;            // we are dealing with graphics.. lets turn off dialog scaling.
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip, this);

            DBBaseName = "ScanPanel";
        }

        protected override void Init()
        {
            edsmSpanshButton.Init(this, "EDSMSpansh", "");
            edsmSpanshButton.ValueChanged += (s,ch) =>
            {
                panelStars.SystemDisplay.ShowWebBodies = edsmSpanshButton.IsAnySet;
                DrawSystem();
            };

            UserDatabaseSettingsSaver dbsaver = new UserDatabaseSettingsSaver(this, "");

            scanDisplayConfigureButton.Init(dbsaver, "DisplayFilters");
            scanDisplayConfigureButton.ValueChanged += (s, ch) =>
            {
                scanDisplayConfigureButton.ApplyDisplayFilters(panelStars);
                DrawSystem();
            };

            scanDisplayBodyFiltersButton.Init(dbsaver, "BodyFilters");
            scanDisplayBodyFiltersButton.ValueChanged += (s, ch) =>
            {
                DrawSystem();
            };

            panelStars.SystemDisplay.ShowWebBodies = edsmSpanshButton.IsAnySet;

            scanDisplayConfigureButton.ApplyDisplayFilters(panelStars);

            panelStars.SystemDisplay.ValueLimit = GetSetting(dbValueLimit, 50000);

            rollUpPanelTop.PinState = GetSetting("PinState", true);

            int size = GetSetting("Size", 64);
            SetSizeImage(size);

            DiscoveryForm.OnNewEntry += NewEntry;

            rollUpPanelTop.SetToolTip(toolTip);     // set after translater

#if PLAYTHRU
            t = new Timer();      // debug, keep for now
            t.Interval = 100;
            t.Tick += T_Tick;
#endif
        }

        protected override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestHistoryGridPos());     //request an update 
        }

        protected override void Closing()
        {
            PutSetting("PinState", rollUpPanelTop.PinState );
            DiscoveryForm.OnNewEntry -= NewEntry;
            closing = true;
        }

        #endregion

        #region Transparency
        public override bool SupportTransparency { get { return true; } }
        protected override void SetTransparency(bool on, Color curcol)
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

        public void NewEntry(HistoryEntry he)               // called when a new entry is made.. check to see if its a scan update
        {
            if (he != null)
            {
                // Star scan type, or material entry type, or a bodyname/id entry, or not set, or not same system
                if ( he.journalEntry is IStarScan || he.journalEntry is IMaterialJournalEntry || he.journalEntry is IBodyFeature || last_he == null || last_he.System != he.System) 
                {
                    last_he = he;
                    DrawSystem(last_he);
                }
            }
        }
        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
#if PLAYTHRU
            t.Start();    // debug, for playing all scans thru
#endif
            if (last_he == null || last_he.System != he.System) // if we changed system, we need to represent
            {
                last_he = he;
                DrawSystem(last_he);
            }
        }

        void DrawSystem(HistoryEntry he)
        {
            if (override_system)        // no change, last_he continues to track cursor for restore..
                return;

            showing_matcomds = he != null ? DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(he.MaterialCommodity) : null;
            showing_system = he?.System;
            DrawSystem();
        }

        async void DrawSystem()   // draw showing_system (may be null), showing_matcomds (may be null)
        {
            panelStars.HideInfo();

            // showing_system = new SystemClass("Qi Lieh");
           // showing_system = new SystemClass("Pallaeni"); 
            //   showing_system = new SystemClass("Borann");
            //showing_system = new SystemClass("Skaudai AM-B d14-138");
            //showing_system = new SystemClass("Eorgh Prou JH-V e2-1979");
            // showing_system = new SystemClass("HYPAA FLYIAE CB-O D6-8");
            //showing_system = new SystemClass("Scheau Prao ME-M c22-21");
          // showing_system = new SystemClass("Eorm Chruia OJ-Q e5-265");

            string control_text = "No System";

#if PLAYTHRU
                EliteDangerousCore.StarScan2.SystemNode data = showing_system != null ? await DiscoveryForm.History.StarScan2.FindSystemAsync(showing_system,WebExternalDataLookup.None) : null;
#else
            EliteDangerousCore.StarScan2.SystemNode data = showing_system != null ? await DiscoveryForm.History.StarScan2.FindSystemAsync(showing_system, edsmSpanshButton.WebLookup) : null;
#endif

            if (showing_system != null)
            {
                control_text = showing_system.Name;

                if (data != null)
                {
                    long value = data.ScanValue(edsmSpanshButton.IsAnySet);
                    control_text += " ~ " + value.ToString("N0") + " cr";

                    int scanned = data.StarPlanetsScanned(edsmSpanshButton.IsAnySet);

                    if (scanned > 0)
                    {
                        control_text += " " + "Scan".Tx()+ " " + scanned.ToString() + (data.FSSTotalBodies != null ? (" / " + data.FSSTotalBodies.Value.ToString()) : "");
                    }

                    int fsssignals = data.FSSSignals?.Count ?? 0;
                    if (fsssignals>0)
                    {
                        control_text += " " + " Signals " + fsssignals.ToString();
                    }
                }
                else
                    control_text += " " + "No Scan".Tx();
            }

            var curmats = DiscoveryForm.History.MaterialCommoditiesMicroResources.GetLast();

            panelStars.DrawSystem(data, showing_matcomds, curmats, (HasControlTextArea() && !scanDisplayConfigureButton.DisplayFilters.Contains("sys")) ? null : control_text, scanDisplayBodyFiltersButton.BodyFilters);

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
                f.Add(new ExtendedControls.ConfigurableEntryList.Entry("L", typeof(Label), "System".Tx()+": ", new Point(10, 40), new Size(110, 24), null));
                f.Add(new ExtendedControls.ConfigurableEntryList.Entry("Sys", typeof(ExtendedControls.ExtTextBoxAutoComplete), "", new Point(120, 40), new Size(width - 120 - 20, 24), null));

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

                f.InitCentred(this.FindForm(), this.FindForm().Icon, "Show System".Tx(), null, null, closeicon:true);
                f.GetControl<ExtendedControls.ExtTextBoxAutoComplete>("Sys").SetAutoCompletor(SystemCache.ReturnSystemAutoCompleteList, true);
                DialogResult res = f.ShowDialog(this.FindForm());

                if (res == DialogResult.OK)
                {
                    string sname = f.Get("Sys");
                    if ( sname.HasChars() && DiscoveryForm.History.StarScan2.GetISystem(sname)!=null )
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

        private void extButtonHighValue_Click(object sender, EventArgs e)
        {
            int v = ScanDisplayUserControl.HighValueForm(this.FindForm(), panelStars.SystemDisplay.ValueLimit);
            if ( v >= 0)
            {
                panelStars.SystemDisplay.ValueLimit = v;
                PutSetting(dbValueLimit, panelStars.SystemDisplay.ValueLimit);
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
            dropdown.SelectedIndexChanged += (s, ea, key) =>
            {
                int size = textlist[dropdown.SelectedIndex].InvariantParseInt(64);
                SetSizeImage(size);
                DrawSystem();
            };

            ExtendedControls.Theme.Current.ApplyStd(dropdown,true);

            dropdown.Show(this.FindForm());
        }

        private void extButtonNewBookmark_Click(object sender, EventArgs e)
        {
            BookmarkHelpers.SendToBookmarkForm(this.FindForm(), DiscoveryForm, last_he?.System);

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

#region Export

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            if (showing_system == null)
                return;

            var sysnode = DiscoveryForm.History.StarScan2.FindSystemSynchronous(showing_system);

            if ( sysnode != null )
            {
                sysnode.DumpTree();

                Forms.ImportExportForm frm = new Forms.ImportExportForm();
                frm.Export(new string[] {
                                        "JSON Orbital Parameters",
                                        },
                    new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.None },
                    new string[] { "JSON|*.json|All|*.*" },
                    new string[] { last_he?.System.Name ?? "" }
                );

                if (frm.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    try
                    {
                        var json = sysnode.ToJson();
                        File.WriteAllText(frm.Path, json.ToString(true)); // failure will be picked up below
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Scan json " + ex);
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to write to " + frm.Path, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            while (hec < DiscoveryForm.History.EntryOrder().Count)
            {
                HistoryEntry he = DiscoveryForm.History.EntryOrder()[hec++];
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
