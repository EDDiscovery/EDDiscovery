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

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            ed.TravelControl.OnTravelSelectionChanged += TravelControl_OnTravelSelectionChanged;
        }

        #endregion

        #region Display

        HistoryEntry last_he;

        private void TravelControl_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl)
        {
            last_he = hl.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.EDDCommodityPrices, he);       // find, from he, the last market data commodity price
            Display(last_he);
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            last_he = hl.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.EDDCommodityPrices, he);
            Display(last_he);
        }

        public void Display(HistoryEntry he)
        {
            dataGridViewMarketData.Rows.Clear();

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
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.TravelControl.OnTravelSelectionChanged -= TravelControl_OnTravelSelectionChanged;
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

        private void dataGridViewLedger_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 3)
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
    }
}
