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
using System.IO;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlOutfitting : UserControlCommonBase
    {
        private string DbColumnSave { get { return DBName("OutfittingGrid" ,  "DGVCol"); } }
        private string DbYardSave { get { return DBName("OutfittingSelect" ); } }

        #region Init

        public UserControlOutfitting()
        {
            InitializeComponent();
            var corner = dataGridViewOutfitting.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewOutfitting.MakeDoubleBuffered();
            dataGridViewOutfitting.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange; ;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;

            Col1.HeaderText = Col2.HeaderText = Col3.HeaderText = Col4.HeaderText = ColPrice.HeaderText = ""; // zero because no values until a display selected

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void LoadLayout()
        {
            dataGridViewOutfitting.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            uctg.OnTravelSelectionChanged += Display;
            DGVLoadColumnLayout(dataGridViewOutfitting, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewOutfitting, DbColumnSave);
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
        }



        #endregion

        #region Display

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            Discoveryform_OnHistoryChange(hl);
        }

        private void Discoveryform_OnHistoryChange(HistoryList hl)
        {
            UpdateComboBox(hl);
        }

        private void UpdateComboBox(HistoryList hl)
        {
            OutfittingList ofl = hl.outfitting;
            string cursel = comboBoxYards.Text;

            string the = "Travel History Entry".T(EDTx.UserControlOutfitting_TravelHistoryEntry);

            comboBoxYards.Items.Clear();
            comboBoxYards.Items.Add(the);

            comboBoxYards.Items.AddRange(ShipModuleData.Instance.GetAllModTypes());

            var list = (from x in ofl.GetFilteredList() select x.Ident()).ToList();
            comboBoxYards.Items.AddRange(list);

            if (cursel == "")
                cursel = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbYardSave, "");

            if (cursel == "" || !comboBoxYards.Items.Contains(cursel))
                cursel = the;

            comboBoxYards.Enabled = false;
            comboBoxYards.SelectedItem = cursel;
            comboBoxYards.Enabled = true;
        }

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        HistoryEntry last_he = null;

        private void Display(HistoryEntry he, HistoryList hl) =>
            Display(he, hl, true);

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if ( comboBoxYards.Items.Count == 0 )
                UpdateComboBox(hl);

            last_he = he;
            Display();
        }

        private void Display()
        {
            DataGridViewColumn sortcol = dataGridViewOutfitting.SortedColumn != null ? dataGridViewOutfitting.SortedColumn : dataGridViewOutfitting.Columns[0];
            SortOrder sortorder = dataGridViewOutfitting.SortOrder;

            dataGridViewOutfitting.Rows.Clear();
            dataViewScrollerPanel.SuspendLayout();

            labelYard.Visible = false;

            Outfitting yard = null;

            if (comboBoxYards.SelectedIndex == 0 || comboBoxYards.Text.Length == 0)  // second is due to the order History gets called vs this on start
            {
                HistoryEntry lastshipyard = discoveryform.history.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.Outfitting, last_he);
                if (lastshipyard != null)
                    yard = (lastshipyard.journalEntry as EliteDangerousCore.JournalEvents.JournalOutfitting).ItemList;
            }
            else
            {
                yard = discoveryform.history.outfitting.GetFilteredList().Find(x => x.Ident().Equals(comboBoxYards.Text));
            }

            if (yard != null)
            {
                DisplayYard(yard);
            }
            else
            {
                List<Tuple<Outfitting, List<Outfitting.OutfittingItem>>> itemlist = discoveryform.history.outfitting.GetItemTypeLocationsFromYardsWithoutRepeat(comboBoxYards.Text,nolocrepeats:true);
                if ( itemlist.Count > 0 )
                    DisplayItems(itemlist, comboBoxYards.Text);
            }

            dataViewScrollerPanel.ResumeLayout();

            dataGridViewOutfitting.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewOutfitting.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
        }

        private void DisplayItems(List<Tuple<Outfitting, List<Outfitting.OutfittingItem>>> itemlist, string moduletype)
        {
            ISystem cursys = discoveryform.history.CurrentSystem;

            foreach (var yard in itemlist)
            {
                double distance = discoveryform.history.DistanceCurrentTo(yard.Item1.StarSystem);
                string dte = EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(yard.Item1.Datetime).ToString();
                string yardname = yard.Item1.Location;

                foreach (var item in yard.Item2)
                {
                    string itemname = item.Name.StartsWith(item.ModType) ? item.Name.Mid(item.ModType.Length+1) : item.Name;
                    ShipModuleData.ShipModule sm = ShipModuleData.Instance.GetItemProperties(item.FDName);
                    itemname = itemname.AppendPrePad(sm.InfoMassPower(true), ", ");

                    object[] rowobj = { dte, yardname, itemname, (distance > -1) ? (distance.ToString("N1") + "ly") : "Unknown".T(EDTx.Unknown), item.BuyPrice.ToString("N1") + "cr" };
                    dataGridViewOutfitting.Rows.Add(rowobj);
                }
            }

            labelYard.Text = moduletype;
            labelYard.Visible = true;
            Col1.HeaderText = "Date".T(EDTx.UserControlOutfitting_Date);
            Col1.Tag = "DT";
            Col2.HeaderText = "Yard".T(EDTx.UserControlOutfitting_Yard);
            Col2.Tag = null;
            Col3.HeaderText = "Item".T(EDTx.UserControlOutfitting_Item);
            Col3.Tag = null;
            Col4.HeaderText = "Distance".T(EDTx.UserControlOutfitting_Distance);
            Col4.Tag = "ly";
            ColPrice.HeaderText = "Price".T(EDTx.UserControlOutfitting_Price);
            ColPrice.Tag = "cr";
        }

        private void DisplayYard(Outfitting yard)
        {
            foreach (var i in yard.Items)
            {
                ShipModuleData.ShipModule sm = ShipModuleData.Instance.GetItemProperties(i.FDName);
                //string namepart = i.Name.Left("Class", StringComparison.InvariantCultureIgnoreCase, true), classpart = i.Name.Mid("Class", StringComparison.InvariantCultureIgnoreCase, false);

                string info = sm.InfoMassPower(false);

                object[] rowobj = { i.ModType, i.Name, info, sm.Mass.ToString("0.#t"),i.BuyPrice.ToString("N1") + "cr" };
                dataGridViewOutfitting.Rows.Add(rowobj);
            }

            double distance = discoveryform.history.DistanceCurrentTo(yard.StarSystem);

            labelYard.Text = yard.Ident() + (distance > -1 ? (" @ " + distance.ToString("N1") + "ly") : "");
            labelYard.Visible = true;

            Col1.HeaderText = "Type".T(EDTx.UserControlOutfitting_Type);
            Col1.Tag = null;
            Col2.HeaderText = "Item".T(EDTx.UserControlOutfitting_Item);
            Col2.Tag = null;
            Col3.HeaderText = "Info".T(EDTx.UserControlOutfitting_Info);
            Col3.Tag = null;
            Col4.HeaderText = "Mass".T(EDTx.UserControlOutfitting_Mass);
            Col4.Tag = "t";
        }

        #endregion


        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxYards.Enabled)
            {
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbYardSave, comboBoxYards.Text);
                Display();
            }
        }

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if ( e.Column.Tag != null )
            {
                string t = (string)e.Column.Tag;

                if (t == "DT")
                    e.SortDataGridViewColumnDate();
                else
                    e.SortDataGridViewColumnNumeric(t);
            }
        }

    }
}
