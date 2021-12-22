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
        
        private string dbStartDate = "StartDate";
        private string dbStartDateOn = "StartDateChecked";
        private string dbEndDate = "EndDate";
        private string dbEndDateOn = "EndDateChecked";

        private bool intransparent = false;

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

            displayfont = FontHelpers.GetFont(GetSetting("font", ""), null);

            extDateTimePickerStartDate.Value = GetSetting(dbStartDate, new DateTime(2014, 12, 14));
            var startchecked = extDateTimePickerStartDate.Checked = GetSetting(dbStartDateOn, false);
            extDateTimePickerEndDate.Value = GetSetting(dbEndDate, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
            var endchecked = extDateTimePickerEndDate.Checked = GetSetting(dbEndDateOn, false);

            extDateTimePickerStartDate.ValueChanged += DateTimePicker_ValueChangedStart;
            extDateTimePickerEndDate.ValueChanged += DateTimePicker_ValueChangedEnd;

            rollUpPanelTop.PinState = GetSetting("PinState", true);
            extCheckBoxShowIncomplete.Checked = GetSetting("ShowIncomplete", true);
            extCheckBoxShowIncomplete.Click += ExtCheckBoxShowIncomplete_Click;

            labelValue.Text = "";       // as its set to <code>
        }


        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
            DGVLoadColumnLayout(dataGridView);
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
            DrawAll();
        }

        public override void Closing()
        {
            PutSetting("PinState", rollUpPanelTop.PinState);
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            discoveryform.OnNewUIEvent -= Discoveryform_OnNewUIEvent;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DGVSaveColumnLayout(dataGridView);
        }

        public override bool SupportTransparency { get { return true; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            intransparent = on;
            extPictureBoxScroll.BackColor = pictureBox.BackColor = this.BackColor = curcol;
            ControlVisibility();
        }
        private void Discoveryform_OnHistoryChange(HistoryList hl)
        {
            DrawGrid();             // don't do the Body info, its tied to the UCTG
            ControlVisibility();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            // received a new navroute, and we have navroute selected, reload
            if (he.EntryType == JournalTypeEnum.ScanOrganic)
            {
                lasthe = hl.GetLast;      // may be null
                DrawAll();
            }
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (he != null)
            {
                lasthe = he;
                DrawBodyInfo();
                ControlVisibility();
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
            if ( !IsTransparent ||  uistate == EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus || !IsSet(CtrlList.autohide))
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
            System.Diagnostics.Debug.WriteLine($"Organics {displaynumber} Draw {lasthe?.System.Name} {lasthe?.System.HasCoordinate}");

            pictureBox.ClearImageList();

            if (lasthe != null && lasthe.Status.HasBodyID && lasthe.Status.BodyType == "Planet")
            {
                StarScan.SystemNode data = discoveryform.history.StarScan.FindSystemSynchronous(lasthe.System, false);

                if (data != null && data.NodesByID.TryGetValue(lasthe.Status.BodyID.Value, out StarScan.ScanNode node))
                {
                    var picelements = new List<ExtPictureBox.ImageElement>();       // accumulate picture elements in here

                    Font dfont = displayfont ?? this.Font;

                   // System.Diagnostics.Debug.WriteLine($"Organics {displaynumber} Go for draw on {lasthe.WhereAmI} {lasthe.BodyType}");

                    int vpos = 0;
                    StringFormat frmt = new StringFormat(extCheckBoxWordWrap.Checked ? 0 : StringFormatFlags.NoWrap);
                    frmt.Alignment = StringAlignment.Near;
                    var textcolour = IsTransparent ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
                    var backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                    //System.Diagnostics.Debug.WriteLine($"Pbox size {pictureBox.Size}");
                    string l = string.Format("At {0}, {1}, Radius {2}, {3} G, {4}", node.FullName, node.ScanData?.PlanetTypeText ?? "Unknown", node.ScanData?.RadiusText() ?? "Unknown", 
                                                node.ScanData?.nSurfaceGravityG?.ToString("N1")  ?? "Unknown" , node.ScanData?.Atmosphere);

                    string s = node.Organics != null ? JournalScanOrganic.OrganicList(node.Organics) : "No organic scanned";

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
            if ( discoveryform.history != null )
            {
                DateTime? start = extDateTimePickerStartDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(extDateTimePickerStartDate.Value) : default(DateTime?);
                DateTime? end = extDateTimePickerEndDate.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(extDateTimePickerEndDate.Value.EndOfDay()) : default(DateTime?);

                DataGridViewColumn sortcolprev = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
                SortOrder sortorderprev = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Descending;
                object curtag = dataGridView.CurrentCell != null ? dataGridView.Rows[dataGridView.CurrentCell.RowIndex].Tag : null;

                dataGridView.Rows.Clear();
                long totalvalue = 0;

                foreach (var syskvp in discoveryform.history.StarScan.ScanDataByName)
                {
                    foreach( var starkvp in syskvp.Value.StarNodes)
                    {
                        foreach( var body in starkvp.Value.Descendants)
                        {
                            if (body.Organics != null)
                            {
                                var orglist = JournalScanOrganic.SortList(body.Organics);

                                if (start != null || end != null)      // if sorting by date, knock out ones outside range
                                {
                                    orglist = orglist.Where(x => (start == null || x.Item2.EventTimeUTC >= start) && (end == null || x.Item2.EventTimeUTC <= end)).ToList();
                                }

                                foreach (var os in orglist)
                                {
                                    if (os.Item2.ScanType == JournalScanOrganic.ScanTypeEnum.Analyse || extCheckBoxShowIncomplete.Checked)
                                    {
                                        DateTime time = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(os.Item2.EventTimeUTC);

                                        object[] data = new object[]
                                        {
                                                time.ToStringYearFirst(),
                                                syskvp.Key.ToString() + ": " + starkvp.Key.ToString(),
                                                body.FullName,
                                                body.ScanData?.PlanetTypeText ?? "",
                                                os.Item2.Genus_Localised,
                                                os.Item2.Species_Localised,
                                                os.Item2.ScanType,
                                                os.Item2.EstimatedValue?.ToStringInvariant("N0") ??""
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
                if (curtag != null && (rowwithtag = dataGridView.FindRowWithTag(curtag)) >=0)
                {
                    dataGridView.CurrentCell = dataGridView.Rows[rowwithtag].Cells[0];
                }
                else if (dataGridView.RowCount > 0)
                    dataGridView.CurrentCell = dataGridView.Rows[0].Cells[0];

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
            autohide
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

        private void extButtonShowControl_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();

            // not yet until more than one displayfilter.AddAllNone();
            displayfilter.AddStandardOption(CtrlList.autohide.ToString(), "Auto Hide".TxID("UserControlSurveyor.autoHideToolStripMenuItem"));

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
                DrawAll();
            };

            displayfilter.Show(typeof(CtrlList), ctrlset, under, this.FindForm());
        }

        private void extButtonFont_Click(object sender, EventArgs e)
        {
            Font f = FontHelpers.FontSelection(this.FindForm(), displayfont ?? this.Font);
            string setting = FontHelpers.GetFontSettingString(f);
            System.Diagnostics.Debug.WriteLine($"Organics Font selected {setting}");
            PutSetting("font", setting);
            displayfont = f;
            DrawBodyInfo();
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting("wordwrap", extCheckBoxWordWrap.Checked);
            dataGridView.DefaultCellStyle.WrapMode = extCheckBoxWordWrap.Checked ? DataGridViewTriState.True : DataGridViewTriState.False;
            DrawBodyInfo();
        }

        private void ExtCheckBoxShowIncomplete_Click(object sender, EventArgs e)
        {
            PutSetting("ShowIncomplete", extCheckBoxShowIncomplete.Checked);
            DrawGrid();
        }

        private void DateTimePicker_ValueChangedStart(object sender, EventArgs e)
        {
            PutSetting(dbStartDate, extDateTimePickerStartDate.Value);
            PutSetting(dbStartDateOn, extDateTimePickerStartDate.Checked);
            DrawGrid();
        }

        private void DateTimePicker_ValueChangedEnd(object sender, EventArgs e)
        {
            PutSetting(dbEndDate, extDateTimePickerEndDate.Value);
            PutSetting(dbEndDateOn, extDateTimePickerEndDate.Checked);
            DrawGrid();
       }

    }
}
