/*
 * Copyright © 2016 - 2019 EDDiscovery development team
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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMaterialTrader : UserControlCommonBase
    {
        const string PrefixName = "MaterialTrade";

        private string DbColumnSave { get { return (PrefixName + "Grid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbTraderType { get { return DBName(PrefixName, "TraderType"); } }

        HistoryEntry last_he = null;

        #region Init

        public UserControlMaterialTrader()
        {
            InitializeComponent();
            var corner = dataGridViewTrades.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewTrades.MakeDoubleBuffered();
            dataGridViewTrades.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            extComboBoxTraderType.Items.AddRange(new string[] { "Raw".Tx(EDTx.UserControlMaterialTrader_Raw), "Encoded".Tx(EDTx.UserControlMaterialTrader_Encoded), "Manufactured".Tx(EDTx.UserControlMaterialTrader_Manufactured) });
            extComboBoxTraderType.SelectedIndex = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbTraderType, 0);
            extComboBoxTraderType.SelectedIndexChanged += ExtComboBoxTraderType_SelectedIndexChanged;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
        }


        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void LoadLayout()
        {
            dataGridViewTrades.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            uctg.OnTravelSelectionChanged += Display;
            DGVLoadColumnLayout(dataGridViewTrades, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewTrades, DbColumnSave);

            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;

            //            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbOSave, Order.ToString(","));
        }


        #endregion

        #region Display

        public override void InitialDisplay()
        {
            last_he = uctg.GetCurrentHistoryEntry;
            Display();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            if (he.journalEntry is ICommodityJournalEntry || he.journalEntry is IMaterialJournalEntry)
                Display();
        }

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if ( he != last_he  )
            {
                last_he = he;
                Display();
            }
        }
        
        private void Display()
        {
            //            if (last_he == null)
            //return;

            int sel = extComboBoxTraderType.SelectedIndex;

            var mcl = new List<Tuple<MaterialCommodityData.MaterialGroupType, MaterialCommodityData[]>>();

            foreach ( var t in Enum.GetValues(typeof(MaterialCommodityData.MaterialGroupType)))
            {
                var e = (MaterialCommodityData.MaterialGroupType)t;
                bool ok = (sel == 0 && e >= MaterialCommodityData.MaterialGroupType.RawCategory1 && e <= MaterialCommodityData.MaterialGroupType.RawCategory7);
                ok |= (sel == 1 && e >= MaterialCommodityData.MaterialGroupType.EncodedEmissionData && e <= MaterialCommodityData.MaterialGroupType.EncodedFirmware);
                ok |= (sel == 2 && e >= MaterialCommodityData.MaterialGroupType.ManufacturedChemical);
                if ( ok )
                {
                    var list = MaterialCommodityData.GetMaterialGroup(x => x.MaterialGroup == e);
                    mcl.Add(new Tuple<MaterialCommodityData.MaterialGroupType, MaterialCommodityData[]>(e,list));
                }
            }

            extPictureTrades.ClearImageList();

            int vpos = 0;
            foreach( var t in mcl )
            {
                int hpos = 0;

                foreach( var b in t.Item2 )
                {
                    Bitmap background = EDDiscovery.Icons.IconSet.GetIcon("Controls.MaterialTrader.encodedbackground") as Bitmap;

                    Bitmap bmp = background.Clone() as Bitmap;

                    extPictureTrades.AddImage(new Rectangle(hpos, vpos, background.Width, background.Height), bmp, imgowned:true);

                    

                    //using (Graphics p = this.CreateGraphics())
                    //{
                    //    p.DrawImage(background, new Point(10, 10));
                    //}

                    hpos += background.Width + 20;
                }

                vpos += 100;
            }

            extPictureTrades.Render();

        }

        #endregion

        private void buttonClear_Click(object sender, EventArgs e)
        {
            dataGridViewTrades.Rows.Clear();
        }

        private void ExtComboBoxTraderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DbTraderType, extComboBoxTraderType.SelectedIndex );
            Display();
        }
    }
}
