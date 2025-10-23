/*
 * Copyright © 2021-2023 EDDiscovery development team
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
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private StringAlignment alignment = StringAlignment.Near;

        private string dbStartDate = "StartDate";
        private string dbStartDateOn = "StartDateChecked";
        private string dbEndDate = "EndDate";
        private string dbEndDateOn = "EndDateChecked";

        private bool intransparent = false;

        #region Initialisation

        public UserControlOrganics()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip, this);

            DBBaseName = "Organics";
        }

        protected override void Init()
        {
            PopulateCtrlList();

            dataGridView.MakeDoubleBuffered();
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            dataGridView.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);

            extCheckBoxWordWrap.Checked = GetSetting("wordwrap", false);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += wordWrapToolStripMenuItem_Click;

            DiscoveryForm.OnNewUIEvent += Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
            rollUpPanelTop.SetToolTip(toolTip);

            displayfont = FontHelpers.GetFont(GetSetting("font", ""), null);

            // pickers we don't worry about the Kind, we use the picker convert functions later 
            extDateTimePickerStartDate.Value = GetSetting(dbStartDate, EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow));
            extDateTimePickerStartDate.Checked = GetSetting(dbStartDateOn, false);
            extDateTimePickerEndDate.Value = GetSetting(dbEndDate, EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow));
            extDateTimePickerEndDate.Checked = GetSetting(dbEndDateOn, false);
            VerifyDates();
            extDateTimePickerStartDate.ValueChanged += DateTimePicker_ValueChangedStart;
            extDateTimePickerEndDate.ValueChanged += DateTimePicker_ValueChangedEnd;

            rollUpPanelTop.PinState = GetSetting("PinState", true);
            extCheckBoxShowIncomplete.Checked = GetSetting("ShowIncomplete", true);
            extCheckBoxShowIncomplete.Click += ExtCheckBoxShowIncomplete_Click;

            TravelHistoryFilter.InitialiseComboBox(comboBoxTime, "", true, false, false);
            comboBoxTime.Text = "";
            this.comboBoxTime.SelectedIndexChanged += new System.EventHandler(this.comboBoxTime_SelectedIndexChanged);

            labelValue.Text = "";       // as its set to <code>
        }


        protected override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView);
        }


        protected override void InitialDisplay()
        {
            DrawGrid();
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
        }

        protected override void Closing()
        {
            PutSetting("PinState", rollUpPanelTop.PinState);
            DiscoveryForm.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DGVSaveColumnLayout(dataGridView);
        }

        public override bool SupportTransparency { get { return true; } }
        protected override void SetTransparency(bool on, Color curcol)
        {
            intransparent = on;
            extPictureBoxScroll.BackColor = pictureBox.BackColor = this.BackColor = curcol;
            ControlVisibility();
        }
        protected override void TransparencyModeChanged(bool on)
        {
            DrawAll();
        }

        private void Discoveryform_OnHistoryChange()
        {
            VerifyDates();      // date range may have changed
            DrawGrid();             // don't do the Body info, its tied to the UCTG
            ControlVisibility();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he)
        {
            // received a new navroute, and we have navroute selected, reload
            if (he.EntryType == JournalTypeEnum.ScanOrganic)
            {
                lasthe = DiscoveryForm.History.GetLast;      // may be null
                DrawAll();
            }
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)
        { 
            // TBD actions on every HE when on planet..
            lasthe = he;
            DrawBodyInfo();
            ControlVisibility();
        }

        private void Discoveryform_OnNewUIEvent(UIEvent uievent)
        {
            EliteDangerousCore.UIEvents.UIGUIFocus gui = uievent as EliteDangerousCore.UIEvents.UIGUIFocus;

            if (gui != null)
            {
                bool refresh = gui.GUIFocus != uistate;
                uistate = gui.GUIFocus;

                if (refresh)
                    ControlVisibility();
            }
        }

        #endregion

        #region Main

        private void DrawAll()
        {
            DrawBodyInfo();
            DrawGrid();
            ControlVisibility();
        }

        private void ControlVisibility()
        {
            // if not in transparent mode, OR in No focus, or not autohiding, or FSSMode and don't hide in FSS mode
            if ( !IsTransparentModeOn ||  uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus || !IsSet(CtrlList.autohide))
            {
                // these are controlled by tranparency
                flowLayoutPanelGridControl.Visible = dataViewScrollerPanel.Visible = extPictureBoxScroll.ScrollBarEnabled =
                rollUpPanelTop.Visible = !intransparent;
                extPictureBoxScroll.Visible = pictureBox.Count > 0;
            }
            else
            {
                flowLayoutPanelGridControl.Visible = dataViewScrollerPanel.Visible = extPictureBoxScroll.Visible =
                rollUpPanelTop.Visible = false;
            }
        }

        private void DrawBodyInfo()
        {
            //System.Diagnostics.Debug.WriteLine($"Organics {displaynumber} Draw {lasthe?.System.Name} {lasthe?.System.HasCoordinate}");

            pictureBox.ClearImageList();

            if (lasthe != null && lasthe.Status.HasBodyID && lasthe.Status.BodyType == BodyDefinitions.BodyType.Planet)
            {
                StarScan.SystemNode data = DiscoveryForm.History.StarScan.FindSystemSynchronous(lasthe.System);

                if (data != null && data.NodesByID.TryGetValue(lasthe.Status.BodyID.Value, out StarScan.ScanNode node))
                {
                    var picelements = new List<ExtPictureBox.ImageElement>();       // accumulate picture elements in here

                    Font dfont = displayfont ?? this.Font;

                   // System.Diagnostics.Debug.WriteLine($"Organics {displaynumber} Go for draw on {lasthe.WhereAmI} {lasthe.BodyType}");

                    int vpos = 0;
                    StringFormat frmt = new StringFormat(extCheckBoxWordWrap.Checked ? 0 : StringFormatFlags.NoWrap);
                    frmt.Alignment = alignment;
                    var textcolour = IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
                    var backcolour = IsTransparentModeOn ? Color.Transparent : this.BackColor;

                    string l = string.Format("At {0}".Tx(), node.BodyDesignator);
                    if (node.ScanData != null)
                    {
                        l += string.Format(", {0}, Radius {1}, {2}, {3}, Bio Signals: {4}{5}".Tx(), node.ScanData.PlanetTypeText, node.ScanData.RadiusText,
                                                (Math.Round(node.ScanData.nSurfaceGravityG.Value, 2, MidpointRounding.AwayFromZero).ToString() ?? "?") + " g", node.ScanData.AtmosphereTranslated, node.CountBioSignals.ToString(), ((node.Genuses != null && node.CountBioSignals > 0) ? ": " + String.Join(", ", node.Genuses?.Select(x => x.Genus_Localised).ToArray()) : ""));
                    }

                    string s = node.Organics != null ? JournalScanOrganic.OrganicListString(node.Organics, 0, false, Environment.NewLine) : "No organic scanned";

                    var i = new ExtPictureBox.ImageElement();
                    i.TextAutoSize(new Point(3, vpos),
                                                    new Size(Math.Max(pictureBox.Width - 6, 24), 10000),
                                                    l.AppendPrePad(s,Environment.NewLine),
                                                    dfont,
                                                    textcolour,
                                                    backcolour,
                                                    1.0F,
                                                    frmt: frmt); ;

                    extPictureBoxScroll.Height = i.Location.Bottom + 8;
                    picelements.Add(i);
                    frmt.Dispose();

                    pictureBox.AddRange(picelements);
                    extPictureBoxScroll.Render();
                    Refresh();
                }
            }
        }

        void DrawGrid()
        {
            if (DiscoveryForm.History != null)        //??its never null?
            {
                // change display time to utc
                DateTime? startutc = extDateTimePickerStartDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(extDateTimePickerStartDate.Value) : default(DateTime?);
                DateTime? endutc = extDateTimePickerEndDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(extDateTimePickerEndDate.Value) : default(DateTime?);

                DataGridViewColumn sortcolprev = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
                SortOrder sortorderprev = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Descending;
                object curtag = dataGridView.CurrentCell != null ? dataGridView.Rows[dataGridView.CurrentCell.RowIndex].Tag : null;

                dataGridView.Rows.Clear();
                long totalvalue = 0;

                foreach (var syskvp in DiscoveryForm.History.StarScan.ScanDataByName)
                {
                    foreach (var starkvp in syskvp.Value.StarNodes)
                    {
                        foreach (var body in starkvp.Value.Bodies())
                        {
                            if (body.Organics != null)
                            {
                                var orglist = JournalScanOrganic.SortList(body.Organics);

                                if (startutc != null || endutc != null)      // if sorting by date, knock out ones outside range
                                {
                                    orglist = orglist.Where(x => (startutc == null || x.Item2.EventTimeUTC >= startutc) && (endutc == null || x.Item2.EventTimeUTC <= endutc)).ToList();
                                }

                                foreach (var os in orglist)
                                {
                                    if (os.Item2.ScanType == JournalScanOrganic.ScanTypeEnum.Analyse || extCheckBoxShowIncomplete.Checked)
                                    {
                                        DateTime time = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(os.Item2.EventTimeUTC);

                                        string valuestr = os.Item2.EstimatedValue.HasValue ? os.Item2.EstimatedValue.Value.ToString("N0") :
                                                            os.Item2.PotentialEstimatedValue.HasValue ? "(" + os.Item2.PotentialEstimatedValue.Value.ToString("N0") + ")" : "";

                                        object[] data = new object[]
                                        {
                                                time.ToStringYearFirst(),
                                                syskvp.Key.ToString() + ": " + starkvp.Key.ToString(),
                                                body.BodyDesignator,
                                                body.ScanData?.PlanetTypeText ?? "",
                                                os.Item2.Genus_Localised??"",
                                                os.Item2.Species_Localised_Short,
                                                os.Item2.Variant_Localised_Short,
                                                os.Item2.ScanType,
                                                valuestr,
                                        };

                                        dataGridView.Rows.Add(data);

                                        totalvalue += os.Item2.EstimatedValue != null ? os.Item2.EstimatedValue.Value : 0;
                                        dataGridView.Rows[dataGridView.RowCount - 1].Tag = os.Item2;
                                    }
                                }
                            }
                        }
                    }

                }

                dataGridView.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridView.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;

                int rowwithtag;
                if (curtag != null && (rowwithtag = dataGridView.FindRowWithTag(curtag)) >= 0)
                {
                    dataGridView.SetCurrentSelOnRow(rowwithtag, 0);
                }
                else if (dataGridView.RowCount > 0)
                {
                    dataGridView.SetCurrentSelOnRow(0, 0);
                }

                labelValue.Text = totalvalue>0 ? (totalvalue.ToString("N0") + " cr") : "";
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            DrawBodyInfo();
        }

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == ColDate)
                e.SortDataGridViewColumnDate();
            else if ( e.Column == ColValue )
                e.SortDataGridViewColumnNumeric();
        }

        #endregion

        #region UI

        protected enum CtrlList
        {
            autohide,
            alignleft, aligncenter, alignright
        };

        private bool[] ctrlset; // holds current state of each control above

        private void PopulateCtrlList()
        {
            ctrlset = GetSettingAsCtrlSet<CtrlList>(DefaultSetting);
            alignment = ctrlset[(int)CtrlList.alignright] ? StringAlignment.Far : ctrlset[(int)CtrlList.aligncenter] ? StringAlignment.Center : StringAlignment.Near;
        }

        private bool IsSet(CtrlList v)
        {
            return ctrlset[(int)v];
        }

        protected virtual bool DefaultSetting(CtrlList e)
        {
            bool def = e == CtrlList.autohide || e == CtrlList.alignleft;
            return def;
        }

        private void extButtonShowControl_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();

            // not yet until more than one displayfilter.UC.AddAllNone();
            displayfilter.UC.Add(CtrlList.autohide.ToString(), "Auto Hide".Tx());

            CommonCtrl(displayfilter, extButtonShowControl);
        }

        private void extButtonAlignment_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();

            string lt = CtrlList.alignleft.ToString();
            string ct = CtrlList.aligncenter.ToString();
            string rt = CtrlList.alignright.ToString();

            displayfilter.UC.Add(lt, "Alignment Left".Tx(), global::EDDiscovery.Icons.Controls.AlignLeft, exclusivetags: ct + ";" + rt, disableuncheck: true);
            displayfilter.UC.Add(ct, "Alignment Center".Tx(), global::EDDiscovery.Icons.Controls.AlignCentre, exclusivetags: lt + ";" + rt, disableuncheck: true);
            displayfilter.UC.Add(rt, "Alignment Right".Tx(), global::EDDiscovery.Icons.Controls.AlignRight, exclusivetags: lt + ";" + ct, disableuncheck: true);
            displayfilter.CloseOnChange = true;
            CommonCtrl(displayfilter, extButtonAlignment);
        }

        private void CommonCtrl(ExtendedControls.CheckedIconNewListBoxForm displayfilter, Control under)
        {
            displayfilter.CloseBoundaryRegion = new Size(32, under.Height);
            displayfilter.AllOrNoneBack = false;
            displayfilter.UC.ImageSize = new Size(24, 24);
            displayfilter.UC.ScreenMargin = new Size(0, 0);
            displayfilter.CloseBoundaryRegion = new Size(32, under.Height);

            displayfilter.SaveSettings = (s, o) =>
            {
                PutBoolSettingsFromString(s, displayfilter.UC.TagList());
                PopulateCtrlList();
                DrawAll();
            };

            displayfilter.Show(typeof(CtrlList), ctrlset, under, this.FindForm());
        }

        private void extButtonFont_Click(object sender, EventArgs e)
        {
            Font f = FontHelpers.FontSelection(this.FindForm(), displayfont ?? this.Font);
            string setting = FontHelpers.GetFontSettingString(f);
            //System.Diagnostics.Debug.WriteLine($"Organics Font selected {setting}");
            PutSetting("font", setting);
            displayfont = f;
            DrawBodyInfo();
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("wordwrap", extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
            DrawBodyInfo();
        }

        private void UpdateWordWrap()
        {
            dataGridView.SetWordWrap(extCheckBoxWordWrap.Checked);
            dataViewScrollerPanel.UpdateScroll();
        }

        private void ExtCheckBoxShowIncomplete_Click(object sender, EventArgs e)
        {
            PutSetting("ShowIncomplete", extCheckBoxShowIncomplete.Checked);
            DrawGrid();
        }

        private void comboBoxTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (updatedprogramatically == false)
            {
                var filter = (TravelHistoryFilter)comboBoxTime.SelectedItem ?? TravelHistoryFilter.NoFilter;
                updatedprogramatically = true;
                if (filter.Lastdockflag)
                {
                    HistoryEntry he = DiscoveryForm.History.GetLastHistoryEntry(x=>x.journalEntry.EventTypeID == JournalTypeEnum.Docked);
                    if (he != null)
                    {
                        extDateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC);
                        extDateTimePickerStartDate.Checked = true;
                    }
                    else
                        extDateTimePickerStartDate.Checked = false;
                }
                else if (filter.MaximumDataAge.HasValue)
                {
                    DateTime start = DateTime.UtcNow.Subtract(filter.MaximumDataAge.Value);
                    extDateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(start);
                    extDateTimePickerStartDate.Checked = true;
                }
                else
                    extDateTimePickerStartDate.Checked = false;

                extDateTimePickerEndDate.Checked = false;
                comboBoxTime.Text = "";

                updatedprogramatically = false;
                SaveStartStopDate();
                DrawGrid();
            }

        }

        bool updatedprogramatically = false;
        private void DateTimePicker_ValueChangedStart(object sender, EventArgs e)
        {
            if (!updatedprogramatically)
            {
                SaveStartStopDate();
                DrawGrid();
            }
        }

        private void DateTimePicker_ValueChangedEnd(object sender, EventArgs e)
        {
            if (!updatedprogramatically)
            {
                SaveStartStopDate();
                DrawGrid();
            }
        }

        private void SaveStartStopDate()
        {
            PutSetting(dbStartDate, extDateTimePickerStartDate.Value);
            PutSetting(dbStartDateOn, extDateTimePickerStartDate.Checked);
            PutSetting(dbEndDate, extDateTimePickerEndDate.Value);
            PutSetting(dbEndDateOn, extDateTimePickerEndDate.Checked);

        }

        private void VerifyDates()
        {
            updatedprogramatically = true;

            // if out of range
            if (!EDDConfig.Instance.DateTimeInRangeForGame(extDateTimePickerStartDate.Value) || !EDDConfig.Instance.DateTimeInRangeForGame(extDateTimePickerEndDate.Value))
            {
                extDateTimePickerStartDate.Checked = extDateTimePickerEndDate.Checked = false;
                extDateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(EliteDangerousCore.EliteReleaseDates.GameRelease);
                extDateTimePickerEndDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow.EndOfDay());
            }
            updatedprogramatically = false;
        }


        #endregion

    }
}
