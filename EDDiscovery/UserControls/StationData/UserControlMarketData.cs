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
using EDDiscovery.Controls;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMarketData : UserControlCommonBase
    {
        private string DbColumnSave { get { return DBName("MarketDataGrid" ,  "DGVCol"); } }
        private string DbBuyOnly { get { return DBName("MarketDataBuyOnly"); } }
        private string DbSellOnly { get { return DBName("MarketDataSellOnly"); } }
        private string DbAutoSwap { get { return DBName("MarketDataAutoSwap" ); } }

        #region Init

        public UserControlMarketData()
        {
            InitializeComponent();
            var corner = dataGridViewMarketData.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewMarketData.MakeDoubleBuffered();
            dataGridViewMarketData.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            checkBoxBuyOnly.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbBuyOnly, false);
            this.checkBoxBuyOnly.CheckedChanged += new System.EventHandler(this.checkBoxBuyOnly_CheckedChanged);

            checkBoxSellOnly.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSellOnly, false);
            this.checkBoxSellOnly.CheckedChanged += new System.EventHandler(this.checkBoxSellOnly_CheckedChanged);

            checkBoxAutoSwap.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbAutoSwap, false);

            discoveryform.OnNewEntry += OnChanged;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= OnChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += OnChanged;
        }

        public override void LoadLayout()
        {
            dataGridViewMarketData.RowTemplate.MinimumHeight= Font.ScalePixels(26);
            DGVLoadColumnLayout(dataGridViewMarketData, DbColumnSave);
            uctg.OnTravelSelectionChanged += OnChanged;
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewMarketData, DbColumnSave);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbBuyOnly, checkBoxBuyOnly.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSellOnly, checkBoxSellOnly.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbAutoSwap, checkBoxAutoSwap.Checked);
            discoveryform.OnNewEntry -= OnChanged;
            uctg.OnTravelSelectionChanged -= OnChanged;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }

        public override void InitialDisplay()
        {
            OnChanged(uctg.GetCurrentHistoryEntry,discoveryform.history);
        }

        #endregion

        #region Display

        HistoryEntry last_he;           // last HE
        HistoryEntry last_eddmd;        // last edd market data from last_he
        HistoryEntry eddmd_left;        // eddmd left comparision, null means use last_he
        HistoryEntry eddmd_right;       // what we are comparing with, null means none
        HistoryEntry current_displayed; // what we are displaying

        List<HistoryEntry> comboboxentries = new List<HistoryEntry>(); // filled by combobox

        private void Discoveryform_OnHistoryChange(HistoryList hl)
        {
            FillComboBoxes(hl);     // display time or list may have changed
        }

        private void OnChanged(HistoryEntry he, HistoryList hl) =>
            OnChanged(he, hl, true);

        private void OnChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (!Object.ReferenceEquals(he, last_he) )       // if last was null, or he has changed, we have a possible change..
            {
                FillComboBoxes(hl);
                // must be a commodity entry, and have items
                HistoryEntry new_last_eddmd = hl.GetLastHistoryEntry(x => x.journalEntry is JournalCommodityPricesBase && (x.journalEntry as JournalCommodityPricesBase).Commodities.Count > 0, he);

                bool eddmdchanged = !Object.ReferenceEquals(new_last_eddmd, last_eddmd);
                bool cargochanged = !Object.ReferenceEquals(last_he?.MaterialCommodity, he?.MaterialCommodity); // is cargo different between he and last_he

                last_he = he;
                last_eddmd = new_last_eddmd;

                //System.Diagnostics.Debug.WriteLine("left {0} right {1} eddmchanged {2} cargo {3}", last_eddmd?.Indexno, last_he?.Indexno, eddmdchanged, cargochanged);

                if (eddmd_left == null)    // if showing travel.. if not, no update due to this.  Need to keep the last_he/last_eddmd going for swapping back
                {
                    if (eddmdchanged || cargochanged)        // if last_eddmd changed.. or cargo
                        Display();
                }
            }
        }

        private void Display()
        {
            DataGridViewColumn sortcol = dataGridViewMarketData.SortedColumn != null ? dataGridViewMarketData.SortedColumn : dataGridViewMarketData.Columns[0];
            SortOrder sortorder = dataGridViewMarketData.SortOrder;

            dataViewScrollerPanel.SuspendLayout();

            int firstdisplayed = dataGridViewMarketData.FirstDisplayedScrollingRowIndex;
            string commodity = (dataGridViewMarketData.CurrentRow != null) ? (string)dataGridViewMarketData.CurrentRow.Cells[1].Value : null;
            int currentoffset = (dataGridViewMarketData.CurrentRow != null) ? Math.Max(0,dataGridViewMarketData.CurrentRow.Index - firstdisplayed) : 0;
            
            dataGridViewMarketData.Rows.Clear();

            HistoryEntry left = (eddmd_left != null) ? eddmd_left : last_eddmd;       // if we have a selected left, use it, else use the last eddmd
            HistoryEntry cargo = (eddmd_left != null) ? eddmd_left : last_he;           // if we have a selected left, use it, else use the last he

            if (left != null)       // we know it has a journal entry of EDD commodity..
            {
                //System.Diagnostics.Debug.WriteLine(Environment.NewLine + "From " + current_displayed?.WhereAmI + " to " + left.WhereAmI);

                JournalCommodityPricesBase ecp = left.journalEntry as JournalCommodityPricesBase;
                List<CCommodities> list = ecp.Commodities;

                //System.Diagnostics.Debug.WriteLine("Test Right " + eddmd_right?.WhereAmI + " vs " + left.WhereAmI);
                if (eddmd_right != null && !Object.ReferenceEquals(eddmd_right, left))   // if got a comparision, and not the same data..
                {
                    if (checkBoxAutoSwap.Checked &&
                        left.System.Name.Equals(eddmd_right.System.Name) &&     // if left system being displayed is same as right system
                        left.WhereAmI.Equals(eddmd_right.WhereAmI))            // that means we can autoswap comparisions around
                    {
                        //System.Diagnostics.Debug.WriteLine("Arrived at last left station, repick " + current_displayed.WhereAmI + " as comparision");

                        int index = comboboxentries.FindIndex(x => x.System.Name.Equals(current_displayed.System.Name) && x.WhereAmI.Equals(current_displayed.WhereAmI));
                        if (index >= 0)       // if found it, swap to last instance of system
                        {
                            comboBoxCustomTo.Enabled = false;
                            comboBoxCustomTo.SelectedIndex = index + 1;
                            comboBoxCustomTo.Enabled = true;
                            eddmd_right = comboboxentries[index];
                            //System.Diagnostics.Debug.WriteLine("Right is now " + eddmd_right.WhereAmI);
                        }

                    }

                    //System.Diagnostics.Debug.WriteLine("Right " + eddmd_right.System.Name + " " + eddmd_right.WhereAmI);
                    list = CCommodities.Merge(list, ((JournalCommodityPricesBase)eddmd_right.journalEntry).Commodities);
                }

                List<MaterialCommodities> mclist = cargo.MaterialCommodity.Sort(true);      // stuff we have..  commodities only
                List<MaterialCommodities> notfound = new List<MaterialCommodities>();
                foreach (MaterialCommodities m in mclist)
                {
                    int index = list.FindIndex(x => x.fdname.EqualsAlphaNumOnlyNoCase(m.Details.FDName));   // try and match, remove any spaces/_ and lower case it for matching
                    if (index >= 0)
                        list[index].CargoCarried = m.Count; // found it, set cargo count..
                    else
                        notfound.Add(m);        // not found, add to list for bottom
                }

                FontFamily ff = new FontFamily(this.Font.Name);
                bool buyonly = checkBoxBuyOnly.Checked;
                bool sellonly = checkBoxSellOnly.Checked;

                foreach (CCommodities c in list)
                {
                    if (sellonly ? c.buyPrice == 0 : (!buyonly || (c.buyPrice > 0 || c.ComparisionBuy)))
                    {
                        MaterialCommodityData mc = MaterialCommodityData.GetByFDName(c.fdname);

                        string name = mc?.Name ?? c.locName;
                        if (c.ComparisionRightOnly)
                            name += " @ " + eddmd_right.WhereAmI;

                        object[] rowobj = { mc?.TranslatedType ?? c.loccategory.Alt(c.category) ,
                                            name,
                                            c.sellPrice > 0 ? c.sellPrice.ToString() : "" ,
                                            c.buyPrice > 0 ? c.buyPrice.ToString() : "" ,
                                            c.CargoCarried,
                                            c.demand > 1 ? c.demand.ToString() : "" ,
                                            c.stock > 0 ? c.stock.ToString() : "" ,
                                            c.meanPrice > 0 ? c.meanPrice.ToString() : "",
                                            c.ComparisionLR,
                                            c.ComparisionRL };

                        int rowno = dataGridViewMarketData.Rows.Add(rowobj);

                        if (c.ComparisionRightOnly && ff != null && ff.IsStyleAvailable(FontStyle.Italic))
                        {
                            for (int i = 1; i < dataGridViewMarketData.Columns.Count; i++)
                                dataGridViewMarketData.Rows[rowno].Cells[i].Style.Font = new Font(this.Font, FontStyle.Italic);
                        }

                        dataGridViewMarketData.Rows[rowno].Cells[0].ToolTipText =
                        dataGridViewMarketData.Rows[rowno].Cells[1].ToolTipText = c.ToString();
                    }
                }

                foreach (MaterialCommodities m in notfound)
                {
                    if (m.Count > 0)
                    {
                        object[] rowobj = {     m.Details.TranslatedType,
                                                m.Details.Name,
                                                "",
                                                "",
                                                m.Count,
                                                "",
                                                "",
                                                "",
                                                "",
                                                "" };

                        int rowno = dataGridViewMarketData.Rows.Add(rowobj);
                        dataGridViewMarketData.Rows[rowno].Cells[0].ToolTipText =
                        dataGridViewMarketData.Rows[rowno].Cells[1].ToolTipText = "Cargo only, no market data on this item".T(EDTx.UserControlMarketData_Conly);
                    }
                }

                current_displayed = left;
                labelLocation.Text = left.System.Name + ":" + left.WhereAmI;
                string r = EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(left.EventTimeUTC).ToString();
                toolTip.SetToolTip(labelLocation, r);

            }
            else
            {
                toolTip.SetToolTip(labelLocation, null);
                labelLocation.Text = "No Data".T(EDTx.NoData);
            }

            dataViewScrollerPanel.ResumeLayout();

            dataGridViewMarketData.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewMarketData.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

            if ( commodity != null )
            {
                foreach( DataGridViewRow rw in dataGridViewMarketData.Rows)
                {
                    string v = (string)rw.Cells[1].Value;
                    if ( v.Equals(commodity))           // Find the commodity, and set it to the same relative position as before.
                    {
                        dataGridViewMarketData.CurrentCell = dataGridViewMarketData.Rows[rw.Index].Cells[1];
                        dataGridViewMarketData.SafeFirstDisplayedScrollingRowIndex( Math.Max(rw.Index - currentoffset,0));
                        break;
                    }
                }

            }

            //System.Diagnostics.Debug.WriteLine("Stop watch" + swp.ElapsedMilliseconds);
        }

        private void FillComboBoxes(HistoryList hl)
        {
            string selfrom = comboBoxCustomFrom.Text;
            string selto = comboBoxCustomTo.Text;

            comboBoxCustomFrom.Items.Clear();
            comboBoxCustomTo.Items.Clear();

            comboBoxCustomFrom.Items.Add("Travel History Entry Last".T(EDTx.UserControlMarketData_LEntry));
            comboBoxCustomTo.Items.Add("None".T(EDTx.None));

            comboboxentries.Clear();

            List<HistoryEntry> hlcpb = hl.FilterByCommodityPricesBackwards;

            foreach (HistoryEntry h in hlcpb)
            {
                comboboxentries.Add(h);
                string v = h.System.Name + ":" + h.WhereAmI + " " + "on".T(EDTx.on) + " " + EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(h.EventTimeUTC).ToString();
                if (h.journalEntry is JournalEDDCommodityPrices)
                    v += " (CAPI)";
                comboBoxCustomFrom.Items.Add(v);
                comboBoxCustomTo.Items.Add(v);
            }

            comboBoxCustomFrom.Enabled = comboBoxCustomTo.Enabled = false;

            if (comboBoxCustomFrom.Items.Contains(selfrom))
                comboBoxCustomFrom.SelectedItem = selfrom;
            else
                comboBoxCustomFrom.SelectedIndex = 0;

            if (comboBoxCustomTo.Items.Contains(selto))
                comboBoxCustomTo.SelectedItem = selto;
            else
                comboBoxCustomTo.SelectedIndex = 0;

            comboBoxCustomFrom.Enabled = comboBoxCustomTo.Enabled = true;
        }

        #endregion

        private void dataGridViewMarketData_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 2)        // 2 on are numbers
                e.SortDataGridViewColumnNumeric();
        }

        private void comboBoxCustomFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomFrom.Enabled)
            {
                if (comboBoxCustomFrom.SelectedIndex == 0 )
                {
                    eddmd_left = null;
                    Display();
                }
                else if (!Object.ReferenceEquals(comboboxentries[comboBoxCustomFrom.SelectedIndex - 1], eddmd_left))
                {
                    eddmd_left = comboboxentries[comboBoxCustomFrom.SelectedIndex - 1];
                    Display();
                }
            }
        }

        private void comboBoxCustomTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomTo.Enabled)
            {
                HistoryEntry new_compare = comboBoxCustomTo.SelectedIndex == 0 ? null : comboboxentries[comboBoxCustomTo.SelectedIndex - 1];

                if (!Object.ReferenceEquals(eddmd_right, new_compare))
                {
                    eddmd_right = new_compare;
                    Display();
                }
            }
        }

        private void checkBoxBuyOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBuyOnly.Checked)
                checkBoxSellOnly.Checked = false;

            Display();
        }

        private void checkBoxSellOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSellOnly.Checked)
                checkBoxBuyOnly.Checked = false;

            Display();
        }

    }
}
