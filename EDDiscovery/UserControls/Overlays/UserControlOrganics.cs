/*
 * Copyright © 2021 - 2021 EDDiscovery development team
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
using EDDiscovery.Controls;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlOrganics : UserControlCommonBase
    {
        HistoryEntry lasthe = null;

        EliteDangerousCore.UIEvents.UIGUIFocus.Focus uistate = EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus;

        private Font displayfont;
        
        private string dbStartDate = "StartDate";
        private string dbStartDateOn = "StartDateChecked";
        private string dbEndDate = "EndDate";
        private string dbEndDateOn = "EndDateChecked";

        #region Initialisation

        public UserControlOrganics()
        {
            InitializeComponent();
            DBBaseName = "Organics";
        }

        public override void Init()
        {
            ctrlset = GetSettingAsCtrlSet<CtrlList>(DefaultSetting);

            extCheckBoxWordWrap.Checked = GetSetting("wordwrap", false);
            extCheckBoxWordWrap.Click += wordWrapToolStripMenuItem_Click;

            dataGridView.MakeDoubleBuffered();
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            dataGridView.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            dataGridView.DefaultCellStyle.WrapMode = extCheckBoxWordWrap.Checked ? DataGridViewTriState.True : DataGridViewTriState.False;

            discoveryform.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;

            BaseUtils.Translator.Instance.Translate(toolTip, this);

            displayfont = FontHelpers.GetFont(GetSetting("font", ""), discoveryform.theme.GetFont);
            System.Diagnostics.Debug.WriteLine($"Surveyor font {FontHelpers.GetFontSettingString(displayfont)}");

            extDateTimePickerStartDate.Value = GetSetting(dbStartDate, new DateTime(2014, 12, 14));
            var startchecked = extDateTimePickerStartDate.Checked = GetSetting(dbStartDateOn, false);
            extDateTimePickerEndDate.Value = GetSetting(dbEndDate, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
            var endchecked = extDateTimePickerEndDate.Checked = GetSetting(dbEndDateOn, false);

            extDateTimePickerStartDate.ValueChanged += DateTimePicker_ValueChangedStart;
            extDateTimePickerEndDate.ValueChanged += DateTimePicker_ValueChangedEnd;

            rollUpPanelTop.PinState = GetSetting("PinState", true);
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
            lasthe = uctg.GetCurrentHistoryEntry;
            DrawSystem(lasthe,true);    // may be null
        }

        public override void Closing()
        {
            PutSetting("PinState", rollUpPanelTop.PinState);
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            discoveryform.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }

        public override bool SupportTransparency { get { return true; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            extPictureBoxScroll.ScrollBarEnabled = !on;     // turn off the scroll bar if its transparent
            extPictureBoxScroll.BackColor = pictureBox.BackColor = this.BackColor = curcol;
            rollUpPanelTop.Visible = !on;
        }

        private void Discoveryform_OnHistoryChange(HistoryList hl)
        {
            lasthe = hl.GetLast;      // may be null
            DrawSystem(lasthe, true);    // may be null
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            // received a new navroute, and we have navroute selected, reload
            if (he.EntryType == JournalTypeEnum.ScanOrganic)
            {
                lasthe = hl.GetLast;      // may be null
                DrawSystem(lasthe,true);      // need to recreate the grid
            }
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (he != null)
            {
                lasthe = he;
                DrawSystem(lasthe,false);
            }
        }

        private void Discoveryform_OnNewUIEvent(UIEvent uievent)
        {
            EliteDangerousCore.UIEvents.UIGUIFocus gui = uievent as EliteDangerousCore.UIEvents.UIGUIFocus;

            if (gui != null)
            {
                bool refresh = gui.GUIFocus != uistate;
                uistate = gui.GUIFocus;

                if (refresh)
                    DrawSystem(lasthe,false);
            }
        }

        #endregion

        #region Main
        private void DrawSystem(HistoryEntry he, bool redogrid)
        {
            System.Diagnostics.Debug.WriteLine($"Organics {displaynumber} Draw {he?.System.Name} {he?.System.HasCoordinate}");

            var picelements = new List<ExtPictureBox.ImageElement>();       // accumulate picture elements in here and render under lock due to async below.

            // if system, and we are in no focus or don't care
            if ((uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus || !IsSet(CtrlList.autohide)
                                || (uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.FSSMode && IsSet(CtrlList.donthidefssmode))))
            {
                if (he != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Organics {displaynumber} Go for draw on {he.WhereAmI} {he.BodyType}");

                    int vpos = 30;
                    StringFormat frmt = new StringFormat(extCheckBoxWordWrap.Checked ? 0 : StringFormatFlags.NoWrap);
                    frmt.Alignment = StringAlignment.Near;
                    var textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                    var backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                    var i = new ExtPictureBox.ImageElement();
                    i.TextAutoSize(new Point(3, vpos),
                                                    new Size(Math.Max(pictureBox.Width - 6, 24), 10000),
                                                    $"Holds list of current results for current body `{he.System.Name}` bt `{he.BodyType}` where `{he.WhereAmI}`- autosized",
                                                    displayfont,
                                                    textcolour,
                                                    backcolour,
                                                    1.0F,
                                                    frmt: frmt);
                    picelements.Add(i);

                    frmt.Dispose();
                }

                panelGrid.Visible = !IsTransparent;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Organics ${displaynumber} display disabled");
                panelGrid.Visible = false;
            }

            if ( redogrid && discoveryform.history != null )
            {
                DateTime? start = extDateTimePickerStartDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(extDateTimePickerStartDate.Value) : default(DateTime?);
                DateTime? end = extDateTimePickerEndDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(extDateTimePickerEndDate.Value.EndOfDay()) : default(DateTime?);

                dataGridView.Rows.Clear();
                foreach (var syskvp in discoveryform.history.StarScan.ScanDataByName)
                {
                    foreach( var starkvp in syskvp.Value.StarNodes)
                    {
                        foreach( var body in starkvp.Value.Descendants)
                        {
                            if (body.Organics != null)
                            {
                                var orglist = JournalScanOrganic.SortList(body.Organics);
                                
                                if ( start != null || end != null)      // if sorting by date, knock out ones outside range
                                {
                                    orglist = orglist.Where(x => (start == null || x.EventTimeUTC >= start) && (end == null || x.EventTimeUTC <= end)).ToList();
                                }

                                string last_key = null;

                                foreach (var os in orglist)
                                {
                                    string key = os.Genus + ":" + os.Species;     // don't repeat genus/species
                                    if (key != last_key)
                                    {
                                        last_key = key;
                                        DateTime time = EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(os.EventTimeUTC);

                                        object[] data = new object[]
                                        {
                                            time.ToStringYearFirst(),
                                            syskvp.Key.ToString() + ": " + starkvp.Key.ToString(),
                                            body.FullName,
                                            body.ScanData?.PlanetTypeText ?? "",
                                            os.Genus_Localised,
                                            os.Species_Localised,
                                            os.ScanType,
                                        };

                                        dataGridView.Rows.Add(data);
                                    }
                                }
                            }

                        }

                    }
                }
            }

            lock (extPictureBoxScroll)      // because of the async call above, we may be running two of these at the same time. So, we lock and then add/update/render
            {
                pictureBox.ClearImageList();
                pictureBox.AddRange(picelements);
                extPictureBoxScroll.Render();
                Refresh();
            }
        }

        private void UserControl_Resize(object sender, EventArgs e)
        {
        }

        #endregion

        #region UI

        protected enum CtrlList
        {
            autohide, donthidefssmode
        };

        private bool[] ctrlset; // holds current state of each control above

        private bool IsSet(CtrlList v)
        {
            return ctrlset[(int)v];
        }

        protected virtual bool DefaultSetting(CtrlList e)
        {
            bool def = true;
            return def;
        }

        private void extButtonPlanets_Click(object sender, EventArgs e)
        {
        }

        private void extButtonStars_Click(object sender, EventArgs e)
        {
        }

        private void extButtonShowControl_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(CtrlList.autohide.ToString(), "Auto Hide".TxID("UserControlSurveyor.autoHideToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.donthidefssmode.ToString(), "Don't hide in FSS Mode".TxID("UserControlSurveyor.dontHideInFSSModeToolStripMenuItem"));

            CommonCtrl(displayfilter, extButtonShowControl);
        }

        #endregion

        private void CommonCtrl(ExtendedControls.CheckedIconListBoxFormGroup displayfilter, Control under)
        {
            displayfilter.AllOrNoneBack = false;
            displayfilter.ImageSize = new Size(24, 24);
            displayfilter.ScreenMargin = new Size(0, 0);

            displayfilter.SaveSettings = (s, o) =>
            {
                PutBoolSettingsFromString(s, displayfilter.SettingsTagList());
                ctrlset = GetSettingAsCtrlSet<CtrlList>(DefaultSetting);
                DrawSystem(lasthe,false);
            };

            displayfilter.Show(typeof(CtrlList), ctrlset, under, this.FindForm());
        }

        private void extButtonFont_Click(object sender, EventArgs e)
        {
            Font f = FontHelpers.FontSelection(this.FindForm(), displayfont);
            string setting = FontHelpers.GetFontSettingString(f);
            System.Diagnostics.Debug.WriteLine($"Organics Font selected {setting}");
            PutSetting("font", setting);
            displayfont = f != null ? f : discoveryform.theme.GetFont;
            DrawSystem(lasthe,false);
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("wordwrap", extCheckBoxWordWrap.Checked);
            dataGridView.DefaultCellStyle.WrapMode = extCheckBoxWordWrap.Checked ? DataGridViewTriState.True : DataGridViewTriState.False;
            DrawSystem(lasthe, false);
        }

        private void DateTimePicker_ValueChangedStart(object sender, EventArgs e)
        {
            PutSetting(dbStartDate, extDateTimePickerStartDate.Value);
            PutSetting(dbStartDateOn, extDateTimePickerStartDate.Checked);
            DrawSystem(lasthe, true);
        }

        private void DateTimePicker_ValueChangedEnd(object sender, EventArgs e)
        {
            PutSetting(dbEndDate, extDateTimePickerEndDate.Value);
            PutSetting(dbEndDateOn, extDateTimePickerEndDate.Checked);
            DrawSystem(lasthe, true);
       }

    }
}
