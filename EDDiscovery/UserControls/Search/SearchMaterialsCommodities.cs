﻿/*
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
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using EDDiscovery.Controls;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    // Search UCs use the UCCB template BUT are not directly inserted into the normal panels.. they are inserted into the Search UCCB
    // Make sure DB saving has unique names.. they all share the same displayno.

    public partial class SearchMaterialsCommodities : UserControlCommonBase
    {
        private string dbCM1 = "CM1";
        private string dbCM2 = "CM2";
        private string dbCMANDOR = "CMANDOR";

        private MaterialCommodityMicroResourceType[] itemlist;
        #region Init

        public SearchMaterialsCommodities()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "UCSearchMC";

            dataGridView.CheckEDSM = true;
            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.Height = Font.ScalePixels(26);
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            var enumlist = new Enum[] { EDTx.SearchMaterialsCommodities_ColumnDate, EDTx.SearchMaterialsCommodities_ColumnStar, EDTx.SearchMaterialsCommodities_ColumnLocation, EDTx.SearchMaterialsCommodities_ColumnCurrentDistance, EDTx.SearchMaterialsCommodities_ColumnPosition, EDTx.SearchMaterialsCommodities_label2, EDTx.SearchMaterialsCommodities_label1 };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, new Control[] {  });

            var enumlisttt = new Enum[] { EDTx.SearchMaterialsCommodities_buttonExtExcel_ToolTip, EDTx.SearchMaterialsCommodities_buttonExtFind_ToolTip, EDTx.SearchMaterialsCommodities_comboBoxCustomCMANDOR_ToolTip, EDTx.SearchMaterialsCommodities_comboBoxCustomCM1_ToolTip, EDTx.SearchMaterialsCommodities_comboBoxCustomCM2_ToolTip };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            dataGridView.Init(discoveryform);

            itemlist = MaterialCommodityMicroResourceType.GetAll();
            Array.Sort(itemlist, (left, right) => left.Name.CompareTo(right.Name));

            var list = (from x in itemlist select x.Name + " (" + x.TranslatedCategory + ", " + x.TranslatedType + (x.Rarity ? ", Rare Commodity".T(EDTx.SearchMaterialsCommodities_RareCommodity):"") + ")");

            comboBoxCustomCM1.Items.AddRange(list);
            comboBoxCustomCM1.SelectedIndex = Math.Min(GetSetting(dbCM1, 0), list.Count() - 1);
            comboBoxCustomCM2.Items.Add("----");
            comboBoxCustomCM2.Items.AddRange(list);
            comboBoxCustomCM2.SelectedIndex = Math.Min(GetSetting(dbCM2, 0), list.Count() - 1);

            comboBoxCustomCMANDOR.Items.AddRange(new string[] { "AND".T(EDTx.SearchMaterialsCommodities_AND), "OR".T(EDTx.SearchMaterialsCommodities_OR) });
            comboBoxCustomCMANDOR.SelectedIndex = GetSetting(dbCMANDOR, 0);

            dataGridView.GotoEntryClicked += (he) => { uctg.GotoPosByJID(he.Journalid); };
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg = thc;
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView);
            PutSetting(dbCM1, comboBoxCustomCM1.SelectedIndex);
            PutSetting(dbCM2, comboBoxCustomCM2.SelectedIndex);
            PutSetting(dbCMANDOR, comboBoxCustomCMANDOR.SelectedIndex);
        }

        #endregion

        class SysComparer : IEqualityComparer<Tuple<HistoryEntry, string>>
        {
            public bool Equals(Tuple<HistoryEntry, string> x, Tuple<HistoryEntry, string> y )
            {
                if (Object.ReferenceEquals(x, y)) return true;
                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null)) return false;
                return x.Item1.System.Name.Equals(y.Item1.System.Name) && x.Item2.Equals(y.Item2);
            }

            public int GetHashCode(Tuple<HistoryEntry, string> x)
            {
                if (Object.ReferenceEquals(x, null)) return 0;
                return x.Item1.System.Name.GetHashCode() ^ x.Item2.GetHashCode();
            }
        }

        void Search(MaterialCommodityMicroResourceType cm, Dictionary<string, Tuple<HistoryEntry, string, double>> foundlist, 
                                        string prefix = "")
        {
            ISystem cursystem = discoveryform.history.CurrentSystem();        // could be null

            foreach ( var he in discoveryform.history.EntryOrder())      // oldest first..
            {
                Tuple<HistoryEntry, string> found = null;
                bool checkstation = false;

                if (he.EntryType == JournalTypeEnum.Scan)
                {
                    var je = he.journalEntry as JournalScan;
                    if (je.HasMaterial(cm.FDName))
                        found = new Tuple<HistoryEntry, string>(he, prefix + je.BodyName);
                }
                else if (he.EntryType == JournalTypeEnum.Market)
                {
                    var je = he.journalEntry as JournalMarket;
                    if (je.HasCommodityToBuy(cm.FDName))
                    {
                        found = new Tuple<HistoryEntry, string>(he, prefix + je.Station);
                        checkstation = true;
                    }
                }
                else if (he.EntryType == JournalTypeEnum.EDDCommodityPrices)
                {
                    var je = he.journalEntry as JournalEDDCommodityPrices;
                    if (je.HasCommodityToBuy(cm.FDName))
                    { 
                        found = new Tuple<HistoryEntry, string>(he, prefix + je.Station);
                        checkstation = true;
                    }
                }
                else if (he.EntryType == JournalTypeEnum.MaterialDiscovered)
                {
                    var je = he.journalEntry as JournalMaterialDiscovered;
                    if (je.Name.Equals(cm.FDName))
                        found = new Tuple<HistoryEntry, string>(he, prefix + "Discovered at ".T(EDTx.SearchMaterialsCommodities_DIS) + he.WhereAmI);
                }
                else if (he.EntryType == JournalTypeEnum.MaterialCollected)
                {
                    var je = he.journalEntry as JournalMaterialCollected;
                    if (je.Name.Equals(cm.FDName))
                        found = new Tuple<HistoryEntry, string>(he, prefix + "Collected at ".T(EDTx.SearchMaterialsCommodities_COL) + he.WhereAmI);
                }

                else if (he.EntryType == JournalTypeEnum.MissionCompleted)
                {
                    var je = he.journalEntry as JournalMissionCompleted;
                    if (je.HasReceivedReward(cm.FDName))
                        found = new Tuple<HistoryEntry, string>(he, prefix + "Mission Reward at ".T(EDTx.SearchMaterialsCommodities_MR) + he.WhereAmI);
                }

                else if (he.EntryType == JournalTypeEnum.SAASignalsFound)
                {
                    var je = he.journalEntry as JournalSAASignalsFound;
                    if (je.Contains(cm.FDName) > 0)
                        found = new Tuple<HistoryEntry, string>(he, prefix + "Discovered at ".T(EDTx.SearchMaterialsCommodities_DIS) + je.BodyName);
                }

                else if (he.EntryType == JournalTypeEnum.FCMaterials)
                {
                    var je = he.journalEntry as JournalFCMaterials;
                    string tx = " @ " + he.WhereAmI + " " + je.CarrierName;

                    if (je.HasItemToBuy(cm.FDName))
                    {
                        found = new Tuple<HistoryEntry, string>(he, prefix + "Buy".T(EDTx.UserControlMarketData_BuyCol) + tx);
                    }
                    if (je.HasItemToSell(cm.FDName))
                    {
                        if ( found == null )
                            found = new Tuple<HistoryEntry, string>(he, prefix + "Sell".T(EDTx.UserControlMarketData_SellCol) + tx);
                        else
                            found = new Tuple<HistoryEntry, string>(he, prefix + "Sell".T(EDTx.UserControlMarketData_SellCol) +  " " + "Buy".T(EDTx.UserControlMarketData_BuyCol) + tx);
                    }
                    checkstation = true;
                }

                if (found != null)
                {
                    string keyname = he.System.Name + (checkstation ? ":"+he.WhereAmI : "");
                    double dist = cursystem.Distance(he.System);

                    if (foundlist.ContainsKey(keyname))
                    {
                        if (!foundlist[keyname].Item2.Contains(found.Item2))     // don't double repeat
                            foundlist[keyname] = new Tuple<HistoryEntry, string, double>(he, foundlist[keyname].Item2.AppendPrePad(found.Item2, Environment.NewLine), dist);
                    }
                    else 
                        foundlist[keyname] = new Tuple<HistoryEntry, string, double>(he, found.Item2, dist);
                }
            }
        }

        private void StarsFound(List<Tuple<HistoryEntry, string, double>> systems)       // systems may be null
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);

            dataGridView.Rows.Clear();

            if (systems != null)
            {
                DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
                SortOrder sortorder = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Descending;

                ISystem cursystem = discoveryform.history.CurrentSystem();        // could be null

                foreach (var ret in systems)
                {
                    ISystem sys = ret.Item1.System;
                    string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";
                    object[] rowobj = {     EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ret.Item1.EventTimeUTC).ToString(),
                                            sys.Name,
                                            ret.Item2,
                                            (cursystem != null ? cursystem.Distance(sys).ToString("0.#") : ""),
                                            sys.X.ToString("0.#") + sep + sys.Y.ToString("0.#") + sep + sys.Z.ToString("0.#")
                                           };

                    dataGridView.Rows.Add(rowobj);
                    dataGridView.Rows[dataGridView.Rows.Count - 1].Tag = ret.Item1;
                }

                dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
            }

        }

        private void buttonExtFind_Click(object sender, EventArgs e)
        {
            Dictionary<string, Tuple<HistoryEntry, string, double>> foundlist = new Dictionary<string, Tuple<HistoryEntry, string, double>>();
            Search(itemlist[comboBoxCustomCM1.SelectedIndex], foundlist);

            if (comboBoxCustomCM2.SelectedIndex > 0)
            {
                if (comboBoxCustomCMANDOR.SelectedIndex == 0)       // AND
                {
                    Dictionary<string, Tuple<HistoryEntry, string, double>> foundlist2 = new Dictionary<string, Tuple<HistoryEntry, string, double>>();
                    Search(itemlist[comboBoxCustomCM2.SelectedIndex - 1], foundlist2, "(2)");

                    List<string> keyremove = new List<string>();
                    foreach( var kp in foundlist)
                    {
                        if (!foundlist2.ContainsKey(kp.Key))
                            keyremove.Add(kp.Key);
                    }
                    foreach (var keyname in keyremove)
                        foundlist.Remove(keyname);
                }
                else
                {
                    Search(itemlist[comboBoxCustomCM2.SelectedIndex - 1], foundlist, "(2)");
                }

            }

            StarsFound(foundlist.Values.ToList());
        }

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index == 2)
                e.SortDataGridViewColumnNumeric();
        }

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            dataGridView.Excel(dataGridView.ColumnCount);
        }
    }
}
