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
using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMarketData : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;
        
        private string DbColumnSave { get { return ("MarketDataGrid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }

        #region Init

        public UserControlMarketData()
        {
            InitializeComponent();
            Name = "Market Data";
        }

        public override void Init( EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            displaynumber = vn;

            dataGridViewMarketData.MakeDoubleBuffered();
            dataGridViewMarketData.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewMarketData.RowTemplate.Height = 26;

            discoveryform.OnNewEntry += OnChanged;
            ed.TravelControl.OnTravelSelectionChanged += OnChanged;
        }

        #endregion

        #region Display

        HistoryEntry last_travelselected;
        HistoryEntry last_displayed;        // what we are displaying
        HistoryEntry last_compared;         // what we are comparing with

        private void OnChanged(HistoryEntry he, HistoryList hl)
        {
            // FIND last entry..
            HistoryEntry new_he = hl.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.EDDCommodityPrices, he);       // find, from he, the last market data commodity price

            if (!Object.ReferenceEquals(new_he, last_travelselected))       // if last was null, or new_he has changed, we have a material change to the list
            {
                FillComboBoxes(hl);

                last_travelselected = new_he;

                if (comboBoxCustomFrom.Text.Contains("Travel History"))
                    Display(last_travelselected);
            }
        }

        List<HistoryEntry> otherentries = new List<HistoryEntry>();

        private void FillComboBoxes(HistoryList hl)
        {
            string selfrom = comboBoxCustomFrom.Text;
            string selto = comboBoxCustomTo.Text;

            comboBoxCustomFrom.Items.Clear();
            comboBoxCustomTo.Items.Clear();

            comboBoxCustomFrom.Items.Add("Travel History Entry Last");
            comboBoxCustomTo.Items.Add("None");

            otherentries.Clear();

            List<HistoryEntry> hlcpb = hl.FilterByEDDCommodityPricesBackwards;
            foreach ( HistoryEntry h in hlcpb )
            {
                otherentries.Add(h);
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

        public void Display(HistoryEntry he)
        {
            last_displayed = he;

            dataGridViewMarketData.Rows.Clear();
            labelLocation.Text = "No Data";
            toolTip1.SetToolTip(labelLocation, null);

            if (he != null )       // we know it has a journal entry of EDD commodity..
            {
                JournalEDDCommodityPrices ecp = he.journalEntry as JournalEDDCommodityPrices;
                
                foreach( CCommodities c in ecp.Commodities )
                {
                    object[] rowobj = { c.categoryname ,
                                        c.name ,
                                        (c.sellPrice>0) ? c.sellPrice.ToString() : "" ,
                                        c.buyPrice > 0 ? c.buyPrice.ToString() : "" ,
                                        0,
                                        c.demand > 1 ? c.demand.ToString() : "" ,
                                        c.stock > 0 ? c.stock.ToString() : "" ,
                                        c.meanPrice > 0 ? c.meanPrice.ToString() : "",
                                        "-",
                                        "-"
                                    };

                    int rowno = dataGridViewMarketData.Rows.Add(rowobj);

                    dataGridViewMarketData.Rows[rowno].Cells[0].ToolTipText = 
                    dataGridViewMarketData.Rows[rowno].Cells[1].ToolTipText = c.ToString();
                }

                labelLocation.Text = he.System.name + ":" + he.WhereAmI;
                string r = "Recorded at " + ((EDDiscoveryForm.EDDConfig.DisplayUTC) ? he.EventTimeUTC.ToString() : he.EventTimeLocal.ToString());
                toolTip1.SetToolTip(labelLocation, r);
            }
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
            discoveryform.OnNewEntry -= OnChanged;
            discoveryform.TravelControl.OnTravelSelectionChanged -= OnChanged;
        }

        #endregion

        #region right clicks

        int rightclickrow = -1;
        int leftclickrow = -1;

        private void dataGridViewLedger_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                rightclickrow = -1;
            }
            if (e.Button == MouseButtons.Left)         // right click on travel map, get in before the context menu
            {
                leftclickrow = -1;
            }

            if (dataGridViewMarketData.SelectedCells.Count < 2 || dataGridViewMarketData.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
            {
                DataGridView.HitTestInfo hti = dataGridViewMarketData.HitTest(e.X, e.Y);
                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    dataGridViewMarketData.ClearSelection();                // select row under cursor.
                    dataGridViewMarketData.Rows[hti.RowIndex].Selected = true;

                    if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
                    {
                        rightclickrow = hti.RowIndex;
                    }
                    if (e.Button == MouseButtons.Left)         // right click on travel map, get in before the context menu
                    {
                        leftclickrow = hti.RowIndex;
                    }
                }
            }
        }

        private void toolStripMenuItemGotoItem_Click(object sender, EventArgs e)
        {
            if (rightclickrow != -1)
            {
            }
        }

        #endregion

        private void dataGridViewMarketData_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 2)        // 2 on are numbers
            {
                double v1;
                double v2;
                bool v1hasval = Double.TryParse(e.CellValue1?.ToString(), out v1);
                bool v2hasval = Double.TryParse(e.CellValue2?.ToString(), out v2);

                if (v1hasval || v2hasval)
                {
                    if (!v1hasval)
                    {
                        e.SortResult = 1;
                    }
                    else if (!v2hasval)
                    {
                        e.SortResult = -1;
                    }
                    else
                    {
                        e.SortResult = v1.CompareTo(v2);
                    }

                    e.Handled = true;
                }
            }

        }

        private void comboBoxCustomFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomFrom.Text.Contains("Travel History"))
            {
                if (!Object.ReferenceEquals(last_displayed, last_travelselected))
                    Display(last_travelselected);
            }
            else
            {
                if (!Object.ReferenceEquals(otherentries[comboBoxCustomFrom.SelectedIndex - 1], last_displayed))
                    Display(otherentries[comboBoxCustomFrom.SelectedIndex - 1]);
            }
        }

        private void comboBoxCustomTo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
