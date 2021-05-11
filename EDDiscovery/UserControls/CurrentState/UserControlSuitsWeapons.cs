/*
 * Copyright © 2021 - 2021 EDDiscovery development team
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

using EliteDangerousCore;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSuitsWeapons : UserControlCommonBase
    {
        #region Init

        public UserControlSuitsWeapons()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "SuitWeapons";

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
            uctg.OnTravelSelectionChanged += Display;

            DGVLoadColumnLayout(dataGridViewSuits, "Suits");
            DGVLoadColumnLayout(dataGridViewWeapons, "Weapons");
            splitContainerMissions.SplitterDistance(GetSetting("Splitter", 0.4));
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewSuits, "Suits");
            DGVSaveColumnLayout(dataGridViewWeapons, "Weapons");
            PutSetting("Splitter", splitContainerMissions.GetSplitterDistance());

            uctg.OnTravelSelectionChanged -= Display;

        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history,true);
        }

        uint last_weapons = 0;
        uint last_suits = 0;
        uint last_loadout = 0;

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            System.Diagnostics.Debug.WriteLine("Display check");
            uint newweapon = he?.Weapons ?? 0;
            if (newweapon != last_weapons)
            {
                last_weapons = newweapon;
                DisplayWeapons();
            }
            uint newsuits = he?.Suits ?? 0;
            uint newloadout = he?.Loadouts ?? 0;
            if (newsuits != last_suits || newloadout != last_loadout )
            {
                last_suits = newsuits;
                last_loadout = newloadout;
                DisplaySuits();
            }
        }

        private void DisplayWeapons()
        {
            DataGridViewColumn sortcolprev = dataGridViewWeapons.SortedColumn != null ? dataGridViewWeapons.SortedColumn : dataGridViewWeapons.Columns[0];
            SortOrder sortorderprev = dataGridViewWeapons.SortedColumn != null ? dataGridViewWeapons.SortOrder : SortOrder.Ascending;
            int firstline = dataGridViewWeapons.SafeFirstDisplayedScrollingRowIndex();

            dataGridViewWeapons.Rows.Clear();
            System.Diagnostics.Debug.WriteLine("Clear Weapon grid");
            extPanelDataGridViewScrollWeapons.SuspendLayout();


            if (last_weapons >= 0)
            {
                var weaponlist = discoveryform.history.WeaponList.Weapons.Get(last_weapons,x => x.Sold == false); // get unsold weapons

                foreach (var w in weaponlist)
                {
                    var weaponinfo = ItemData.GetWeapon(w.Value.FDName);
                    object[] rowobj = new object[] {  EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(w.Value.EventTime).ToString(),
                                               (w.Value.ID%10000) + ":" + w.Value.FriendlyName,
                                               w.Value.Price.ToString("N0"),
                                               weaponinfo !=null ? weaponinfo.DPS.ToString("N1") : ""};
                   // System.Diagnostics.Debug.WriteLine("Weapon row {0} {1}", w.Value.EventTime, w.Value.FriendlyName);
                    dataGridViewWeapons.Rows.Add(rowobj);
                }
            }

            dataGridViewWeapons.Update();
            extPanelDataGridViewScrollWeapons.ResumeLayout();

            dataGridViewWeapons.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewWeapons.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;
            if (firstline >= 0 && firstline < dataGridViewWeapons.RowCount)
                dataGridViewWeapons.SafeFirstDisplayedScrollingRowIndex(firstline);

        }

        private void DisplaySuits()
        {
            DataGridViewColumn sortcolprev = dataGridViewSuits.SortedColumn != null ? dataGridViewSuits.SortedColumn : dataGridViewSuits.Columns[0];
            SortOrder sortorderprev = dataGridViewSuits.SortedColumn != null ? dataGridViewSuits.SortOrder : SortOrder.Ascending;
            int firstline = dataGridViewSuits.SafeFirstDisplayedScrollingRowIndex();

            dataGridViewSuits.Rows.Clear();
            System.Diagnostics.Debug.WriteLine("Clear Suit grid");
            extPanelDataGridViewScrollSuits.SuspendLayout();

            if (last_suits >= 0)
            {
                var suitlist = discoveryform.history.SuitList.Suits.Get(last_suits,x=>x.Sold==false); // get unsold suits
                var fontscaled = EDDTheme.Instance.GetDialogScaledFont(0.8f);

                foreach (var s in suitlist)
                {
                    string stime = EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(s.Value.EventTime).ToString();
                    string sname =  s.Value.FriendlyName + "(" + (s.Value.ID % 10000) + ")";
                    string sprice = s.Value.Price.ToString("N0");

                    var loadouts = discoveryform.history.SuitLoadoutList.GetLoadoutsForSuit(last_loadout, s.Value.ID);

                    if (loadouts == null || loadouts.Count == 0)
                    {
                        object[] rowobj = new object[] { stime, sname, sprice };
                        dataGridViewSuits.Rows.Add(rowobj);
                        DataGridViewRow r = dataGridViewSuits.Rows[dataGridViewSuits.RowCount - 1];
                        r.Tag = s.Value;
                    }
                    else
                    {
                        int i = 0;
                        foreach (var l in loadouts)
                        {
                            object[] rowobj = new object[] { };

                            var rw = dataGridViewSuits.RowTemplate.Clone() as DataGridViewRow;
                            rw.CreateCells(dataGridViewSuits,
                                                stime,
                                                sname,
                                                sprice,
                                                l.Value.Name + "(" + ((l.Value.ID % 10000).ToString()) + ")",
                                                l.Value.Modules.ContainsKey("primaryweapon1") ? l.Value.Modules["primaryweapon1"].FriendlyName : "",
                                                l.Value.Modules.ContainsKey("primaryweapon2") ? l.Value.Modules["primaryweapon2"].FriendlyName : "",
                                                l.Value.Modules.ContainsKey("secondaryweapon") ? l.Value.Modules["secondaryweapon"].FriendlyName : ""
                                                );

                            dataGridViewSuits.Rows.Add(rw);

                            DataGridViewRow r = dataGridViewSuits.Rows[dataGridViewSuits.RowCount - 1];
                            r.Tag = s.Value;

                            if (i > 0)      // tried emptying the row and using tags to sort but it insists on putting empty cells at top/bottom
                            {
                                r.Cells[0].Style.Alignment = r.Cells[1].Style.Alignment = r.Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                                r.Cells[0].Style.Font = r.Cells[1].Style.Font = r.Cells[2].Style.Font = fontscaled;
                            }

                            i++;
                        }
                    }

                }
            }

            dataGridViewSuits.Update();
            extPanelDataGridViewScrollSuits.ResumeLayout();

            dataGridViewSuits.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewSuits.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;
            if (firstline >= 0 && firstline < dataGridViewSuits.RowCount)
                dataGridViewSuits.SafeFirstDisplayedScrollingRowIndex(firstline);
        }


        #endregion

        #region UI

        private void forceSellSuitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if ( dataGridViewSuits.RightClickRowValid )
            {
                int row = dataGridViewSuits.RightClickRow;
                Suit s = dataGridViewSuits.Rows[row].Tag as Suit;
                System.Diagnostics.Debug.WriteLine("Force Sell Suit {0} {1} {2}", s.EventTime, s.FDName, s.FriendlyName);

                if (ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Confirm selling of ".T(EDTx.TBD) + s.FriendlyName, "Delete".T(EDTx.Delete), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    var je = new EliteDangerousCore.JournalEvents.JournalSellSuit(DateTime.UtcNow, s.ID, s.FDName, s.Name_Localised, 0, EDCommander.CurrentCmdrID);
                    var jo = je.Json();
                    je.Add(jo);
                    discoveryform.NewEntry(je);
                }
            }

        }

        #endregion
    }
}

