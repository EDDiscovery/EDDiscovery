/*
 * Copyright © 2016 - 2021 EDDiscovery development team
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
using System;
using System.ComponentModel;
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
        }

        public override void Init()
        {
            DBBaseName = "EstimatedValue";

            discoveryform.OnNewEntry += NewEntry;
            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip,this);
            extPanelRollUp.SetToolTip(toolTip);

            checkBoxEDSM.Checked = GetSetting("EDSM", false);
            checkBoxEDSM.CheckedChanged += CheckBoxEDSM_CheckedChanged;

            dataGridViewEstimatedValues.MakeDoubleBuffered();
            dataGridViewEstimatedValues.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewEstimatedValues.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            dataGridViewEstimatedValues.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);

            checkBoxShowZeros.Checked = GetSetting(dbShowZero, false); 
            checkBoxShowZeros.CheckedChanged += CheckBoxShowZeros_CheckedChanged;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
            DGVLoadColumnLayout(dataGridViewEstimatedValues);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewEstimatedValues);

            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= NewEntry;
        }

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history , true);
        }

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made.. check to see if its a scan update
        {
            // if he valid, and last is null, or not he, or we have a new scan
            if (he != null && (last_he == null || he != last_he || he.journalEntry is IStarScan)) 
            {
                last_he = he;
                DrawSystem();
            }
        }

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)            // Called at first start or hooked to change cursor
        {
            if (he != null && (last_he == null || he.System != last_he.System))
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

            StarScan.SystemNode last_sn = await discoveryform.history.StarScan.FindSystemAsync(last_he.System, checkBoxEDSM.Checked);

            SetControlText((last_sn == null) ? "No Scan".T(EDTx.NoScan) : string.Format("Estimated Scan Values for {0}".T(EDTx.UserControlEstimatedValues_SV), last_sn.System.Name));

            if (last_sn != null)
            {
                foreach( var bodies in last_sn.Bodies )
                {
                    if ( bodies.ScanData != null && bodies.ScanData.BodyName != null && (checkBoxEDSM.Checked || !bodies.ScanData.IsEDSMBody))     // if check edsm, or not edsm body, with scandata
                    {
                        System.Diagnostics.Debug.WriteLine("Recalc for " + bodies.ScanData.BodyName);
                        var ev = bodies.ScanData.RecalcEstimatedValues();
                        if ( !checkBoxShowZeros.Checked && ev.EstimatedValueBase == 0)
                            continue; // skip 0-value things

                        string spclass = bodies.ScanData.IsStar ? bodies.ScanData.StarTypeText : bodies.ScanData.PlanetTypeText;
                        dataGridViewEstimatedValues.Rows.Add(new object[] {
                                        GetBodySimpleName(bodies.ScanData.BodyDesignationOrName, last_he.System.Name),
                                        spclass,
                                        bodies.ScanData.IsEDSMBody ? "EDSM" : "",
                                        (bodies.IsMapped ? Icons.Controls.Scan_Bodies_Mapped : nullimg),
                                        (bodies.ScanData.WasMapped == true? Icons.Controls.Scan_Bodies_Mapped : nullimg),
                                        (bodies.ScanData.WasDiscovered == true ? Icons.Controls.Scan_DisplaySystemAlways : nullimg),
                                        ev.EstimatedValueBase.ToString("N0"),
                                        ev.EstimatedValueMapped>0 ? (ev.EstimatedValueMappedEfficiently.ToString("N0") + " / " + ev.EstimatedValueMapped.ToString("N0")) : "",
                                        ev.EstimatedValueFirstDiscovered>0 ? ev.EstimatedValueFirstDiscovered.ToString("N0") : "",
                                        ev.EstimatedValueFirstMappedEfficiently>0 ? (ev.EstimatedValueFirstMappedEfficiently.ToString("N0") + " / " + ev.EstimatedValueFirstMapped.ToString("N0")) : "",
                                        ev.EstimatedValueFirstDiscoveredFirstMappedEfficiently> 0 ? (ev.EstimatedValueFirstDiscoveredFirstMappedEfficiently.ToString("N0") + " / " + ev.EstimatedValueFirstDiscoveredFirstMapped.ToString("N0")):"" ,
                                        bodies.ScanData.EstimatedValue.ToString("N0") });
                    }
                }

                dataGridViewEstimatedValues.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
                dataGridViewEstimatedValues.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
            }
        }

        private string GetBodySimpleName(string bodyName, string systemName)
        {
            return bodyName.ReplaceIfStartsWith(systemName,musthaveextra:true).Trim();
        }

        private void CheckBoxEDSM_CheckedChanged(object sender, System.EventArgs e)
        {
            PutSetting("EDSM", checkBoxEDSM.Checked);
            DrawSystem();
        }

        private void CheckBoxShowZeros_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting(dbShowZero, checkBoxShowZeros.Checked);    // negative because we changed button sense
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
            if (e.Column.Index >= 6)
                e.SortDataGridViewColumnNumeric();
            else if (e.Column.Index >= 3)
                SortImageColumn(e);
        }

     }
}
