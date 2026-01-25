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
    public partial class UserControlMarketData : UserControlCommonBase
    {
        private string dbBuyOnly = "BuyOnly";
        private string dbHasDemand = "HasDemand";
        private string dbAutoSwap = "AutoSwap";

        #region Init

        public UserControlMarketData()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip, this);

            DBBaseName = "MarketData";
        }

        protected override void Init()
        {
            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            checkBoxBuyOnly.Checked = GetSetting(dbBuyOnly, false);
            this.checkBoxBuyOnly.CheckedChanged += new System.EventHandler(this.checkBoxBuyOnly_CheckedChanged);

            checkBoxHasDemand.Checked = GetSetting(dbHasDemand, false);
            this.checkBoxHasDemand.CheckedChanged += new System.EventHandler(this.checkBoxHasDemand_CheckedChanged);

            checkBoxAutoSwap.Checked = GetSetting(dbAutoSwap, false);

            labelLocation.Text = "No Data".Tx();
            labelComparison.Text = "-";
        }

        protected override void LoadLayout()
        {
            dataGridView.RowTemplate.MinimumHeight= Font.ScalePixels(26);
            DGVLoadColumnLayout(dataGridView);
        }

        protected override void Closing()
        {
            DGVSaveColumnLayout(dataGridView);
            PutSetting(dbBuyOnly, checkBoxBuyOnly.Checked);
            PutSetting(dbHasDemand, checkBoxHasDemand.Checked);
            PutSetting(dbAutoSwap, checkBoxAutoSwap.Checked);
        }


        #endregion

        #region History

        private HistoryEntry last_he;           // last HE seen by receive history entry 
        private HistoryEntry last_commodity_he; // last edd market data from he found
        private HistoryEntry market_left;        // eddmd left comparision, null means use last_commodity_he
        private HistoryEntry market_right;       // what we are comparing with, null means none
        private HistoryEntry last_market_displayed;       // last market displayed

        protected override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestHistoryGridPos());     //request an update 
        }

        // from the travel history, give the current HE we are pointing at.  Never fires if he = null
        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            bool changed = last_he != he;       

            // if we have a last he, and he index is one better than last, and he is last entry
            bool trackingtop = last_he != null && he.Index == last_he.Index + 1 && DiscoveryForm.History.GetLast == he;

            // if we are tracking the top, we

            bool find = last_commodity_he == null ||       // if we don't have an estimate of the last commodity HE
                        (trackingtop && he.journalEntry is JournalCommodityPricesBase) || // or we are tracking the top and the new HE is a commodity one
                        (!trackingtop && last_he != he);

            System.Diagnostics.Debug.WriteLine($"Market Data RHE : changed {changed} trackingtop {trackingtop} find {find} last_he {last_he?.EventTimeUTC} {last_he?.EventSummary} : HE {he.EventTimeUTC} {he.EventSummary}");

            if (find)   // we should refind
            {
                var new_last_commodity_he = DiscoveryForm.History.GetLastHistoryEntry(x => x.journalEntry is JournalCommodityPricesBase && (x.journalEntry as JournalCommodityPricesBase).Commodities.Count > 0, he);

                bool marketchanged = !Object.ReferenceEquals(new_last_commodity_he, last_commodity_he);
                bool cargochanged = last_he?.MaterialCommodity != he.MaterialCommodity; // is cargo different between he and last_he
                
                System.Diagnostics.Debug.WriteLine($".. Market Data RHE : station `{(new_last_commodity_he?.journalEntry as JournalCommodityPricesBase)?.Station}` changed market {marketchanged} cargo changed {cargochanged}");

                last_commodity_he = new_last_commodity_he;
                last_he = he;

                //System.Diagnostics.Debug.WriteLine("left {0} right {1} eddmchanged {2} cargo {3}", last_eddmd?.Indexno, last_he?.Indexno, eddmdchanged, cargochanged);

                if (market_left == null)    // if showing travel.. if not, no update due to this.  Need to keep the last_he/last_eddmd going for swapping back
                {
                    if (marketchanged || cargochanged)        // if last_eddmd changed.. or cargo
                        Display();
                }
            }
            else
            {
                last_he = he;       // we did not find, but we have considered this one, so we record it
            }

        }

        #endregion

        #region Display

        private void Display()
        {
            DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
            SortOrder sortorder = dataGridView.SortOrder;

            dataViewScrollerPanel.SuspendLayout();

            int firstdisplayed = dataGridView.SafeFirstDisplayedScrollingRowIndex();
            string commodity = (dataGridView.CurrentRow != null) ? (string)dataGridView.CurrentRow.Cells[1].Value : null;
            int currentoffset = (dataGridView.CurrentRow != null) ? Math.Max(0,dataGridView.CurrentRow.Index - firstdisplayed) : 0;
            
            dataGridView.Rows.Clear();

            HistoryEntry left = (market_left != null) ? market_left : last_commodity_he;       // if we have a selected left, use it, else use the last eddmd
            HistoryEntry cargo = (market_left != null) ? market_left : last_he;           // if we have a selected left, use it, else use the last he

            if (left != null)       // we know it has a journal entry of EDD commodity..
            {
                JournalCommodityPricesBase ecp = left.journalEntry as JournalCommodityPricesBase;
                List<CCommodities> list = ecp.Commodities;

                //System.Diagnostics.Debug.WriteLine("Test Right " + eddmd_right?.WhereAmI + " vs " + left.WhereAmI);
                if (market_right != null && !Object.ReferenceEquals(market_right, left))   // if got a comparision, and not the same data..
                {
                    if (checkBoxAutoSwap.Checked &&
                        last_market_displayed != null &&
                        left.System.Name.Equals(market_right.System.Name) &&     // if left system being displayed is same as right system
                        left.WhereAmI.Equals(market_right.WhereAmI))            // that means we can autoswap comparisions around
                    {
                        System.Diagnostics.Debug.WriteLine($"Market Data autoswap left station equals right comparision");

                        market_right = last_market_displayed;     // set eddmd right to the last market displayed
                    }

                    list = CCommodities.Merge(list, ((JournalCommodityPricesBase)market_right.journalEntry).Commodities);
                }

                // look thru out mclist and copmare it with market data, and produce a not found list to show

                List<MaterialCommodityMicroResource> mclist = DiscoveryForm.History.MaterialCommoditiesMicroResources.GetCommoditiesSorted(cargo.MaterialCommodity);      // stuff we have..  commodities only
                List<MaterialCommodityMicroResource> notfound = new List<MaterialCommodityMicroResource>();
                foreach (MaterialCommodityMicroResource m in mclist)
                {
                    int index = list.FindIndex(x => x.fdname.EqualsAlphaNumOnlyNoCase(m.Details.FDName));   // try and match, remove any spaces/_ and lower case it for matching
                    if (index >= 0)
                        list[index].CargoCarried = m.Count; // found it, set cargo count..
                    else if ( m.Details.Type != MaterialCommodityMicroResourceType.ItemType.Drones) // don't add drones
                        notfound.Add(m);        // not found, add to list for bottom
                }

                FontFamily ff = new FontFamily(this.Font.Name);
                bool buyonly = checkBoxBuyOnly.Checked;
                bool hasdemand = checkBoxHasDemand.Checked;

                foreach (CCommodities c in list)
                {
                    // logic here is:
                    // if hasdemand is set, we display if we have demand, or if we have a comparision to display
                    // else if not buy toggle is off, or when on its when it can be bought or we have a comparision to display

                    if (hasdemand ? (c.HasGoodDemand || c.ComparisionBuy) : (!buyonly || c.HasStock || c.ComparisionBuy))
                    {
                        MaterialCommodityMicroResourceType mc = MaterialCommodityMicroResourceType.GetByFDName(c.fdname);

                        string name = mc?.TranslatedName ?? c.locName;
                        if (c.ComparisionRightOnly)
                            name += " @ " + market_right.WhereAmI;

                        object[] rowobj = { mc?.TranslatedType ?? c.loccategory.Alt(c.category) ,
                                            name,
                                            c.sellPrice > 0 ? c.sellPrice.ToString() : "" ,
                                            c.buyPrice > 0 ? c.buyPrice.ToString() : "" ,
                                            c.CargoCarried,
                                            c.HasGoodDemand ? c.demand.ToString() : "" ,       
                                            c.HasStock ? c.stock.ToString() : "" ,
                                            c.meanPrice > 0 ? c.meanPrice.ToString() : "",
                                            c.ComparisionLR,
                                            c.ComparisionRL };

                        DataGridViewRow rw = dataGridView.Rows[dataGridView.Rows.Add(rowobj)];

                        if (c.ComparisionRightOnly && ff != null && ff.IsStyleAvailable(FontStyle.Italic))
                        {
                            for (int i = 1; i < dataGridView.Columns.Count; i++)
                                rw.Cells[i].Style.Font = new Font(this.Font, FontStyle.Italic);
                        }

                        // rw.Cells[0].Value = mc?.TranslatedType + " " + rw.Index.ToString(); // debug
                        //System.Diagnostics.Debug.WriteLine($"Display {c.fdname} {c.demandBracket} {c.stockBracket}");

                        rw.Cells[DemandCol.Index].Tag = c.demandBracket;
                        rw.Cells[SupplyCol.Index].Tag = c.stockBracket;
                        rw.Cells[0].ToolTipText =
                        rw.Cells[1].ToolTipText = c.ToString();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Do not display {c.fdname}");
                    }
                }

                // things in your cargo which are not found

                foreach (MaterialCommodityMicroResource m in notfound)
                {
                    if (m.Count > 0)
                    {
                        object[] rowobj = {     m.Details.TranslatedType + "!!!!",
                                                m.Details.TranslatedName,
                                                "",
                                                "",
                                                m.Count,
                                                "",
                                                "",
                                                "",
                                                "",
                                                "" };

                        int rowno = dataGridView.Rows.Add(rowobj);
                        dataGridView.Rows[rowno].Cells[0].ToolTipText =
                        dataGridView.Rows[rowno].Cells[1].ToolTipText = "Cargo only, no market data on this item".Tx();
                    }
                }

                labelLocation.Text = left.System.Name + ":" + left.WhereAmI + " @ " + EDDConfig.Instance.ConvertTimeToSelectedFromUTC(left.EventTimeUTC).ToString();
                labelComparison.Text = market_right == null ? "-" : market_right.System.Name + ":" + market_right.WhereAmI + " @ " + EDDConfig.Instance.ConvertTimeToSelectedFromUTC(market_right.EventTimeUTC).ToString();
                string r = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(left.EventTimeUTC).ToString();
                toolTip.SetToolTip(labelLocation, r);

                last_market_displayed = left;       // record what we are displaying, for the auto swap facility, as we will set right to this in the right cirumstances above
            }
            else
            {
                toolTip.SetToolTip(labelLocation, null);
                labelLocation.Text = "No Data".Tx();
                labelComparison.Text = "-";
            }

            dataViewScrollerPanel.ResumeLayout();

            dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

            if ( commodity != null )
            {
                foreach( DataGridViewRow rw in dataGridView.Rows)
                {
                    string v = (string)rw.Cells[1].Value;
                    if ( v.Equals(commodity))           // Find the commodity, and set it to the same relative position as before.
                    {
                        dataGridView.SetCurrentAndSelectAllCellsOnRow(rw.Index);
                        dataGridView.SafeFirstDisplayedScrollingRowIndex( Math.Max(rw.Index - currentoffset,0));
                        break;
                    }
                }

            }
        }

        static Image[] demandsupplybracketimages = new Image[] { EDDiscovery.Icons.Controls.Bar0, EDDiscovery.Icons.Controls.Bar1,
                                        EDDiscovery.Icons.Controls.Bar2, EDDiscovery.Icons.Controls.Bar3};

        private void dataGridViewMarketData_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewRow rw = dataGridView.Rows[e.RowIndex];

            // if we have a tag, we may not because of the notfound list
            if (rw.Cells[SupplyCol.Index].Tag != null)
            {
                int supplybracket = (int)rw.Cells[SupplyCol.Index].Tag;
                if (supplybracket >= 0 && supplybracket < demandsupplybracketimages.Length)     // defend against frontier dark arts
                {
                    var supplyrect = dataGridView.GetCellDisplayRectangle(SupplyCol.Index, rw.Index, false);
                    supplyrect.X += supplyrect.Width - supplyrect.Height - 4;
                    supplyrect.Width = supplyrect.Height;
                    e.Graphics.DrawImage(demandsupplybracketimages[supplybracket], supplyrect);
                }

                int demandbracket = (int)rw.Cells[DemandCol.Index].Tag;
                if (demandbracket >= 0 && demandbracket < demandsupplybracketimages.Length)
                {
                    var demandrect = dataGridView.GetCellDisplayRectangle(DemandCol.Index, rw.Index, false);
                    demandrect.X += demandrect.Width - demandrect.Height - 4;
                    demandrect.Width = demandrect.Height;
                    e.Graphics.DrawImage(demandsupplybracketimages[demandbracket], demandrect);
                }
            }
            else
            {

            }
        }

        #endregion

        #region UI

        private void checkBoxBuyOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBuyOnly.Checked)
                checkBoxHasDemand.Checked = false;

            Display();
        }

        private void checkBoxHasDemand_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxHasDemand.Checked)
                checkBoxBuyOnly.Checked = false;

            Display();
        }


        private void extButtonSelectWhere_Click(object sender, EventArgs e)
        {
            CheckedIconNewListBoxForm frm = new CheckedIconNewListBoxForm();
            frm.UC.AddButton("th", "Travel History Entry Last".Tx());
            PopulateMenu(frm);
            frm.PositionBelow(extButtonSelectWhere);
            frm.UC.MultipleColumns = true;
            frm.CloseBoundaryRegion = new Size(400, 64);        // allow a lot of X, to allow mouse to be side on to the menu popout, less Y
            frm.UC.ButtonPressed += (index, stag, text, utag, bev) => 
            { 
                if ( stag == "th")
                {
                    System.Diagnostics.Debug.WriteLine($"Left Travel history");
                    market_left = null;
                    Display();
                }
                else if (stag == "st")
                {
                    market_left = ((CheckedIconUserControl.SubForm)utag).Items[0].UserTag as HistoryEntry;
                    System.Diagnostics.Debug.WriteLine($"Left Whereis Button pressed {market_left.EventTimeUTC}");
                    Display();
                }
                else if (stag == "e")
                {
                    market_left = utag as HistoryEntry;
                    System.Diagnostics.Debug.WriteLine($"Left HE Button pressed {market_left.EventTimeUTC}");
                    Display();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Left System button pressed no action");
                }

                frm.Close(); 
            
            };
            frm.Show(this);

        }
        private void extButtonSelectComparision_Click(object sender, EventArgs e)
        {
            CheckedIconNewListBoxForm frm = new CheckedIconNewListBoxForm();
            frm.UC.AddButton("none", "None".Tx());
            PopulateMenu(frm);
            frm.PositionBelow(extButtonSelectWhere);
            frm.UC.MultipleColumns = true;
            frm.CloseBoundaryRegion = new Size(400, 64);        // allow a lot of X, to allow mouse to be side on to the menu popout, less Y
            frm.UC.ButtonPressed += (index, stag, text, utag, bev) =>
            {
                if (stag == "none")
                {
                    System.Diagnostics.Debug.WriteLine($"Right None");
                    market_right = null;
                    Display();
                }
                else if (stag == "st")
                {
                    market_right = ((CheckedIconUserControl.SubForm)utag).Items[0].UserTag as HistoryEntry;
                    System.Diagnostics.Debug.WriteLine($"Right Whereis Button pressed {market_right.EventTimeUTC}");
                    Display();
                }
                else if (stag == "e")
                {
                    market_right = utag as HistoryEntry;
                    System.Diagnostics.Debug.WriteLine($"Right HE Button pressed {market_right.EventTimeUTC}");
                    Display();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Right System button pressed no action");
                }

                frm.Close();

            };
            frm.Show(this);

        }

        private void PopulateMenu(CheckedIconNewListBoxForm frm)
        {
            var list = HistoryList.FilterByCommodityPricesBackwardsSystemWhereAmI(DiscoveryForm.History.EntryOrder(), true);

            foreach (List<System.Linq.IGrouping<string, HistoryEntry>> system in list)
            {
                CheckedIconUserControl.SubForm stations = new CheckedIconUserControl.SubForm();

                frm.UC.AddButton("sys", system[0].First().System.Name, usertag: stations);

                foreach (var stat in system)
                {
                    if (stat.Count() == 1)
                    {
                        var he = stat.First();
                        stations.Items.Add(new CheckedIconUserControl.Item() { Tag = "e", Text = stat.Key + " @ " + EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC).ToString(), Button = true, UserTag = he });
                    }
                    else
                    {
                        CheckedIconUserControl.SubForm timelist = new CheckedIconUserControl.SubForm();

                        stations.Items.Add(new CheckedIconUserControl.Item() { Tag = "st", Text = stat.Key, Button = true, UserTag = timelist });

                        foreach (var he in stat)
                        {
                            timelist.Items.Add(new CheckedIconUserControl.Item() { Tag = "e", Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC).ToString(), Button = true, UserTag = he });
                        }
                    }
                }
            }
        }

    
        #endregion


        #region Grid

        private void dataGridViewMarketData_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 2)        // 2 on are numbers
                e.SortDataGridViewColumnNumeric();
        }



        #endregion

    }
}
