/*
 * Copyright © 2022 - 2022 EDDiscovery development team
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
using ExtendedControls;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlDiscoveries : UserControlCommonBase
    {
        string dbTimeWindow = "TimeWindow";
        string dbSearches = "Searches";

        #region Init
        public UserControlDiscoveries()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "Discoveries";

            dataGridView.CheckEDSM = false; // for this, only our data is shown
            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.Height = Font.ScalePixels(26);
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            var enumlist = new Enum[] { EDTx.SearchScans_ColumnDate, EDTx.SearchScans_ColumnStar, EDTx.SearchScans_ColumnInformation,
                                        EDTx.SearchScans_ColumnCurrentDistance,  EDTx.SearchScans_ColumnSearches,  EDTx.SearchScans_ColumnPosition,  EDTx.SearchScans_ColumnParent, 
                                        EDTx.SearchScans_labelTime , EDTx.SearchScans_labelSearch};
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, subname: "SearchScans");

            discoveryform.OnNewEntry += NewEntry;

            rollUpPanelTop.SetToolTip(toolTip);     // set after translater

            TravelHistoryFilter.InitaliseComboBox(comboBoxTime, GetSetting(dbTimeWindow, ""), incldockstartend: true);

            PopulateCtrlList();
        }

        public override void LoadLayout()
        {
        }

        public override void InitialDisplay()
        {
            Draw();
        }

        public override void Closing()
        {
            PutSetting("PinState", rollUpPanelTop.PinState);
            discoveryform.OnNewEntry -= NewEntry;
        }

        #endregion

        #region Display

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made.. check to see if its a scan update
        {
            // Star scan type, or material entry type, or a bodyname/id entry, or not set, or not same system
            if (he.journalEntry is IStarScan)
            {
                Draw();
            }
        }

        void Draw()
        {
        }

        #endregion

        #region UI

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            PutSetting(dbTimeWindow, comboBoxTime.Text);
            Draw();
        }

        private void extButtonSearches_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();
            displayfilter.AddAllNone();
            displayfilter.SettingsSplittingChar = '\u2188';     // pick a crazy one soe

            var searches = HistoryListQueries.Instance.Searches.Where(x => x.Standard || x.User).ToList();
            foreach (var s in searches)
                displayfilter.AddStandardOption(s.Name, s.Name);

            CommonCtrl(displayfilter, extButtonSearches, dbSearches);
        }

        private string[] searchesactive;
        private void PopulateCtrlList()
        {
            searchesactive = GetSetting(dbSearches, "").SplitNoEmptyStartFinish('\u2188');
        }

        private void CommonCtrl(ExtendedControls.CheckedIconListBoxFormGroup displayfilter, Control under, string saveasstring)
        {
            displayfilter.AllOrNoneBack = false;
            displayfilter.ImageSize = new Size(24, 24);
            displayfilter.ScreenMargin = new Size(0, 0);

            displayfilter.SaveSettings = (s, o) =>
            {
                if (saveasstring == null)
                    PutBoolSettingsFromString(s, displayfilter.SettingsTagList());
                else
                    PutSetting(saveasstring, s);

                PopulateCtrlList();
                Draw();
            };

            displayfilter.Show(GetSetting(saveasstring, ""), under, this.FindForm());
        }

        #endregion

        #region Export

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            dataGridView.Excel(dataGridView.ColumnCount);
        }
        #endregion

    }
}