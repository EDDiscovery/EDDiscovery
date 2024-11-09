/*
 * Copyright © 2016 - 2023 EDDiscovery development team
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
 */

using EDDiscovery.Controls;
using EliteDangerousCore;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlEstimatedValues : UserControlCommonBase
    {
        private HistoryEntry last_he = null;
        const string dbShowZero = "ShowZeros";
        private static readonly System.Drawing.Image nullimg = EmptyImage();

        private static System.Drawing.Image EmptyImage()
        {
            var img = new System.Drawing.Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            img.MakeTransparent();
            return img;
        }

        public UserControlEstimatedValues()
        {
            InitializeComponent();
            labelControlText.Text = "";     // clear label control text, only used by SetControlText if no other place is available
        }

        public override void Init()
        {
            DBBaseName = "EstimatedValue";

            DiscoveryForm.OnNewEntry += NewEntry;

            var enumlist = new Enum[] { EDTx.UserControlEstimatedValues_BodyName, EDTx.UserControlEstimatedValues_BodyType, EDTx.UserControlEstimatedValues_EDSM, EDTx.UserControlEstimatedValues_Mapped, EDTx.UserControlEstimatedValues_WasMapped, EDTx.UserControlEstimatedValues_WasDiscovered, EDTx.UserControlEstimatedValues_EstBase, EDTx.UserControlEstimatedValues_MappedValue, EDTx.UserControlEstimatedValues_FirstMappedEff, EDTx.UserControlEstimatedValues_FirstDiscMapped, EDTx.UserControlEstimatedValues_EstValue };
            var enumlisttt = new Enum[] { EDTx.UserControlEstimatedValues_checkBoxShowZeros_ToolTip, EDTx.UserControlEstimatedValues_extCheckBoxShowImpossible_ToolTip };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            extPanelRollUp.SetToolTip(toolTip);

            extCheckBoxShowImpossible.Checked = GetSetting("Impossible", false);
            extCheckBoxShowImpossible.CheckedChanged += ExtCheckBoxShowImpossible_CheckedChanged;

            dataGridViewEstimatedValues.MakeDoubleBuffered();
            dataGridViewEstimatedValues.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewEstimatedValues.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            dataGridViewEstimatedValues.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);

            checkBoxShowZeros.Checked = GetSetting(dbShowZero, false); 
            checkBoxShowZeros.CheckedChanged += CheckBoxShowZeros_CheckedChanged;

            extPanelRollUp.PinState = GetSetting("PinState", true);

            edsmSpanshButton.Init(this, "EDSMSpansh", "");
            edsmSpanshButton.ValueChanged += (s, e) =>
            {
                DrawSystem();
            };
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewEstimatedValues);
        }

        public override void Closing()
        {
            PutSetting("PinState", extPanelRollUp.PinState);
            DGVSaveColumnLayout(dataGridViewEstimatedValues);

            DiscoveryForm.OnNewEntry -= NewEntry;
        }

        public override bool SupportTransparency { get { return true; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            dataViewScrollerPanel.BackColor = this.BackColor = curcol;
            DGVTransparent(dataGridViewEstimatedValues, on, curcol);
            flowLayoutPanelTop.Visible = !on;

        }
        public override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
        }

        public void NewEntry(HistoryEntry he)               // called when a new entry is made.. check to see if its a scan update
        {
            // if he valid, and last is null, or not he, or we have a new scan
            if (he != null && (last_he == null || he != last_he || he.journalEntry is IStarScan)) 
            {
                last_he = he;
                DrawSystem();
            }
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            if (last_he == null || he.System != last_he.System)
            {
                last_he = he;
                DrawSystem();
            }
        }

        async void DrawSystem()   // draw last_he
        {
            DataGridViewColumn sortcol = dataGridViewEstimatedValues.SortedColumn != null ? dataGridViewEstimatedValues.SortedColumn : dataGridViewEstimatedValues.Columns[6];
            SortOrder sortorder = dataGridViewEstimatedValues.SortOrder != SortOrder.None ? dataGridViewEstimatedValues.SortOrder : SortOrder.Descending;

            dataGridViewEstimatedValues.Rows.Clear();

            if (last_he == null)
            {
                SetControlText("No Scan".T(EDTx.NoScan));
                return;
            }

            StarScan.SystemNode last_sn = await DiscoveryForm.History.StarScan.FindSystemAsync(last_he.System, edsmSpanshButton.WebLookup);

            if (last_sn != null)
            {
                long totalvalue = 0;

                foreach (var bodies in last_sn.Bodies)
                {
                    // we have scan data, and a name, and either edsm spansh set or no web bordies

                    if (bodies.ScanData != null && bodies.ScanData.BodyName != null && (edsmSpanshButton.IsAnySet || !bodies.ScanData.IsWebSourced))     
                    {
                        string spclass = bodies.ScanData.IsStar ? bodies.ScanData.StarTypeText : bodies.ScanData.PlanetTypeText;

                        bodies.ScanData.GetPossibleEstimatedValues(extCheckBoxShowImpossible.Checked,
                                            out long basevalue,
                                            out long mappedvalue, out long mappedefficiently,                  
                                            out long firstmappedvalue, out long firstmappedefficiently,        
                                            out long firstdiscoveredmappedvalue, out long firstdiscoveredmappedefficiently,  
                                            out long _
                            );

                        string mappedstr = mappedvalue > 0 ? (mappedefficiently.ToString("N0") + " / " + mappedvalue.ToString("N0")) : "";
                        string firstmappedeffstr = firstmappedvalue > 0 ? (firstmappedefficiently.ToString("N0") + " / " + firstmappedvalue.ToString("N0")) : "";
                        string fdmappedstr = firstdiscoveredmappedvalue > 0 ? (firstdiscoveredmappedefficiently.ToString("N0") + " / " + firstdiscoveredmappedvalue.ToString("N0")) : "";

                        // System.Diagnostics.Debug.WriteLine($"EV was map {bodies.ScanData.IsPreviouslyMapped} was dis {bodies.ScanData.IsPreviouslyDiscovered} we map {bodies.ScanData.Mapped}");

                        int estimatedvalue = bodies.ScanData.EstimatedValue;

                        dataGridViewEstimatedValues.Rows.Add(new object[] {
                                        GetBodySimpleName(bodies.ScanData.BodyDesignationOrName, last_he.System.Name),
                                        spclass,
                                        bodies.ScanData.DataSourceName,
                                        (bodies.IsMapped ? Icons.Controls.Scan_Bodies_Mapped : nullimg),
                                        (bodies.ScanData.WasMapped == true? Icons.Controls.Scan_Bodies_Mapped : nullimg),
                                        bodies.ScanData.PR31State ? Icons.Controls.Scan_NotDiscoveredButMapped : bodies.ScanData.WasDiscovered == true ? Icons.Controls.Scan_DisplaySystemAlways : nullimg,
                                        basevalue.ToString("N0"),
                                        mappedstr,
                                        firstmappedeffstr,
                                        fdmappedstr ,
                                        estimatedvalue>0 ? estimatedvalue.ToString("N0") : "" });

                        // column 0 is sorted by tag and has the full name in it.
                        dataGridViewEstimatedValues.Rows[dataGridViewEstimatedValues.RowCount - 1].Cells[0].Tag = bodies.ScanData.BodyDesignationOrName;
                        totalvalue += estimatedvalue;
                    }
                }

                dataGridViewEstimatedValues.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
                dataGridViewEstimatedValues.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

                SetControlText(string.Format("Estimated Scan Values for {0}".T(EDTx.UserControlEstimatedValues_SV) + ": " + totalvalue.ToString("N0") + " cr", last_sn.System.Name));
            }
            else
            {
                SetControlText("No Scan".T(EDTx.NoScan));
            }
        }

        private string GetBodySimpleName(string bodyName, string systemName)
        {
            return bodyName.ReplaceIfStartsWith(systemName,musthaveextra:true).Trim();
        }

        private void CheckBoxShowZeros_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting(dbShowZero, checkBoxShowZeros.Checked);    // negative because we changed button sense
            DrawSystem();
        }

        private void ExtCheckBoxShowImpossible_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting("Impossible", extCheckBoxShowImpossible.Checked);
            DrawSystem();
        }

        private void SortImageColumn(DataGridViewSortCompareEventArgs e)
        {
            bool cv1 = !object.ReferenceEquals(e.CellValue1, nullimg);
            bool cv2 = !object.ReferenceEquals(e.CellValue2, nullimg);
            e.SortResult = cv1.CompareTo(cv2);
            e.Handled = true;
        }

        private void dataGridViewEstimatedValues_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnAlphaInt(true);     // use cell tag which has full name
            else if (e.Column.Index >= 6)
                e.SortDataGridViewColumnNumeric();
            else if (e.Column.Index >= 3)
                SortImageColumn(e);
        }

     }
}
