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
using EDDiscovery.Controls;
using EliteDangerousCore.JournalEvents;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMarketData : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;
        private UserControlTravelGrid uctg;

        private string DbColumnSave { get { return ("MarketDataGrid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbBuyOnly { get { return "MarketDataBuyOnly" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        #region Init

        public UserControlMarketData()
        {
            InitializeComponent();
            Name = "Market Data";
        }

        public override void Init( EDDiscoveryForm ed, UserControlTravelGrid thc, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            uctg = thc;
            displaynumber = vn;

            dataGridViewMarketData.MakeDoubleBuffered();
            dataGridViewMarketData.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewMarketData.RowTemplate.Height = 26;

            discoveryform.OnNewEntry += OnChanged;
            uctg.OnTravelSelectionChanged += OnChanged;

            checkBoxBuyOnly.Enabled = false;
            checkBoxBuyOnly.Checked = SQLiteDBClass.GetSettingBool(DbBuyOnly, false);
            checkBoxBuyOnly.Enabled = true;
        }

        #endregion

        #region Display

        HistoryEntry last_he;           // last HE
        HistoryEntry last_eddmd;        // last edd market data from last_he
        HistoryEntry eddmd_left;        // eddmd left comparision, null means use last_he
        HistoryEntry eddmd_right;       // what we are comparing with, null means none

        List<HistoryEntry> comboboxentries = new List<HistoryEntry>(); // filled by combobox

        private void OnChanged(HistoryEntry he, HistoryList hl)
        {
            if (!Object.ReferenceEquals(he, last_he))       // if last was null, or he has changed, we have a possible change..
            {
                FillComboBoxes(hl);
                HistoryEntry new_last_eddmd = hl.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.EDDCommodityPrices, he);       // find, from he, the last market data commodity price

                bool eddmdchanged = !Object.ReferenceEquals(new_last_eddmd, last_eddmd);
                bool cargochanged = !Object.ReferenceEquals(last_he?.MaterialCommodity, he?.MaterialCommodity); // is cargo different between he and last_he

                last_he = he;
                last_eddmd = new_last_eddmd;

                if (eddmd_left == null)    // if showing travel.. if not, no update due to this.  Need to keep the last_he/last_eddmd going for swapping back
                {
                    System.Diagnostics.Debug.WriteLine("left {0} right {1} eddmchanged {2} cargo {3}", last_eddmd?.Indexno, last_he?.Indexno, eddmdchanged , cargochanged);
                    if (eddmdchanged || cargochanged)        // if last_eddmd changed.. or cargo
                        Display();
                }
            }
        }

        public void Display()
        {
            DataGridViewColumn sortcol = dataGridViewMarketData.SortedColumn != null ? dataGridViewMarketData.SortedColumn : dataGridViewMarketData.Columns[0];
            SortOrder sortorder = dataGridViewMarketData.SortOrder;

            dataGridViewMarketData.Rows.Clear();
            labelLocation.Text = "No Data";
            toolTip1.SetToolTip(labelLocation, null);

            HistoryEntry left = (eddmd_left != null) ? eddmd_left : last_eddmd;       // if we have a selected left, use it, else use the last eddmd
            HistoryEntry cargo = (eddmd_left != null) ? eddmd_left : last_he;           // if we have a selected left, use it, else use the last he

            if (left != null )       // we know it has a journal entry of EDD commodity..
            {
                JournalEDDCommodityPrices ecp = left.journalEntry as JournalEDDCommodityPrices;
                List<CCommodities> list = ecp.Commodities;

                if (eddmd_right != null && !Object.ReferenceEquals(eddmd_right, left))   // if got a comparision, and not the same
                {
                    list = CCommodities.Merge(list, ((JournalEDDCommodityPrices)eddmd_right.journalEntry).Commodities , eddmd_right.WhereAmI);
                }

                List<MaterialCommodities> mclist = cargo.MaterialCommodity.Sort(true);      // stuff we have..  commodities only
                List<MaterialCommodities> notfound = new List<MaterialCommodities>();
                foreach (MaterialCommodities m in mclist)
                {
                    int index = list.FindIndex(x => x.name.EqualsAlphaNumOnlyNoCase(m.name));   // try and match, remove any spaces/_ and lower case it for matching
                    if (index >= 0)
                        list[index].CargoCarried = m.count; // found it, set cargo count..
                    else
                        notfound.Add(m);        // not found, add to list for bottom
                }

                FontFamily ff = new FontFamily(this.Font.Name);
                bool buyonly = checkBoxBuyOnly.Checked;

                foreach (CCommodities c in list)
                {
                    if (!buyonly || (c.buyPrice > 0 || c.ComparisionBuy))
                    {
                        object[] rowobj = { c.type ,
                                            c.name ,
                                            c.sellPrice > 0 ? c.sellPrice.ToString() : "" ,
                                            c.buyPrice > 0 ? c.buyPrice.ToString() : "" ,
                                            c.CargoCarried,
                                            c.demand > 1 ? c.demand.ToString() : "" ,
                                            c.stock > 0 ? c.stock.ToString() : "" ,
                                            c.meanPrice > 0 ? c.meanPrice.ToString() : "",
                                            c.ComparisionLR,
                                            c.ComparisionRL,
                                        };

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
                    if (m.count > 0)
                    {
                        object[] rowobj = {     m.type,
                                                m.name ,
                                                "",
                                                "",
                                                m.count,
                                                "",
                                                "",
                                                "",
                                                "",
                                                "",
                                            };

                        int rowno = dataGridViewMarketData.Rows.Add(rowobj);
                        dataGridViewMarketData.Rows[rowno].Cells[0].ToolTipText =
                        dataGridViewMarketData.Rows[rowno].Cells[1].ToolTipText = "Cargo only, no market data on this item";
                    }
                }

                labelLocation.Text = left.System.name + ":" + left.WhereAmI;
                string r = "Recorded at " + ((EDDiscoveryForm.EDDConfig.DisplayUTC) ? left.EventTimeUTC.ToString() : left.EventTimeLocal.ToString());
                toolTip1.SetToolTip(labelLocation, r);
            }

            dataGridViewMarketData.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewMarketData.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
        }

        private void FillComboBoxes(HistoryList hl)
        {
            string selfrom = comboBoxCustomFrom.Text;
            string selto = comboBoxCustomTo.Text;

            comboBoxCustomFrom.Items.Clear();
            comboBoxCustomTo.Items.Clear();

            comboBoxCustomFrom.Items.Add("Travel History Entry Last");
            comboBoxCustomTo.Items.Add("None");

            comboboxentries.Clear();

            List<HistoryEntry> hlcpb = hl.FilterByEDDCommodityPricesBackwards;
            foreach (HistoryEntry h in hlcpb)
            {
                comboboxentries.Add(h);
                string v = h.System.name + ":" + h.WhereAmI + " on " + ((EDDiscoveryForm.EDDConfig.DisplayUTC) ? h.EventTimeUTC.ToString() : h.EventTimeLocal.ToString());
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

        #region Layout

        public override void LoadLayout()
        {
            //DGVLoadColumnLayout(dataGridViewMarketData, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewMarketData, DbColumnSave);
            SQLiteDBClass.PutSettingBool(DbBuyOnly, checkBoxBuyOnly.Checked);
            discoveryform.OnNewEntry -= OnChanged;
            uctg.OnTravelSelectionChanged -= OnChanged;
        }

        #endregion

        private void dataGridViewMarketData_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 2)        // 2 on are numbers
            {
                double v1;
                double v2;
                bool v1hasval = Double.TryParse(e.CellValue1?.ToString().Replace("cr/t", ""), out v1);
                bool v2hasval = Double.TryParse(e.CellValue2?.ToString().Replace("cr/t", ""), out v2);

                if (v1hasval)
                {
                    if (v2hasval)
                        e.SortResult = v1.CompareTo(v2);
                    else
                        e.SortResult = 1;
                }
                else if (v2hasval)
                    e.SortResult = -1;
                else
                    return;

                e.Handled = true;

            }

        }

        private void comboBoxCustomFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomFrom.Enabled)
            {
                if (comboBoxCustomFrom.Text.Contains("Travel History"))
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
            if ( checkBoxBuyOnly.Enabled )
            {
                Display();
            }
        }
    }
}
