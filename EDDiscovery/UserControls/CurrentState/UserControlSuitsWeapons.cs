/*
 * Copyright © 2021 - 2022 EDDiscovery development team
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
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

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

            var enumlist = new Enum[] { EDTx.UserControlSuitsWeapons_CSTime, EDTx.UserControlSuitsWeapons_CSName, EDTx.UserControlSuitsWeapons_CSMods, EDTx.UserControlSuitsWeapons_CSPrice, EDTx.UserControlSuitsWeapons_CSLoadout, EDTx.UserControlSuitsWeapons_CSPrimary1, EDTx.UserControlSuitsWeapons_CSPrimary2, EDTx.UserControlSuitsWeapons_CSSecondary, EDTx.UserControlSuitsWeapons_CWTime, EDTx.UserControlSuitsWeapons_CWName, EDTx.UserControlSuitsWeapons_CWClass, EDTx.UserControlSuitsWeapons_CWMods, EDTx.UserControlSuitsWeapons_CWPrice, EDTx.UserControlSuitsWeapons_CWPrimary, EDTx.UserControlSuitsWeapons_CWWType, EDTx.UserControlSuitsWeapons_CWDamageType, EDTx.UserControlSuitsWeapons_CWFireMode, EDTx.UserControlSuitsWeapons_CWDamage, EDTx.UserControlSuitsWeapons_CWRPS, EDTx.UserControlSuitsWeapons_CWDPS, EDTx.UserControlSuitsWeapons_CWClipSize, EDTx.UserControlSuitsWeapons_CWHopper, EDTx.UserControlSuitsWeapons_CWRange, EDTx.UserControlSuitsWeapons_CWHSD };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            var enumlistcms = new Enum[] { EDTx.UserControlSuitsWeapons_forceSellShipToolStripMenuItem };
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStripSuits, enumlistcms, this);
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
          //  System.Diagnostics.Debug.WriteLine("Display check");
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
            DataGridViewColumn sortcolprev = dataGridViewWeapons.SortedColumn != null ? dataGridViewWeapons.SortedColumn : dataGridViewWeapons.Columns[1];
            SortOrder sortorderprev = dataGridViewWeapons.SortedColumn != null ? dataGridViewWeapons.SortOrder : SortOrder.Ascending;
            int firstline = dataGridViewWeapons.SafeFirstDisplayedScrollingRowIndex();

            extPanelDataGridViewScrollWeapons.SuspendLayout();
            dataGridViewWeapons.SuspendLayout();

            dataGridViewWeapons.Rows.Clear();

            if (last_weapons >= 0)
            {
                var weaponlist = discoveryform.history.WeaponList.Weapons.Get(last_weapons,x => x.Sold == false); // get unsold weapons

                foreach (var w in weaponlist)
                {
                    var weaponinfo = ItemData.GetWeapon(w.Value.FDName);        // may be null
                    var weapondp = weaponinfo?.GetStats(w.Value.Class);     // may be null
                    if (weapondp != null && w.Value.WeaponMods != null)     // apply engineering to weapon stats
                        weapondp = weapondp.ApplyEngineering(w.Value.WeaponMods);

                    string smods = w.Value.WeaponMods != null ? string.Join(", ", w.Value.WeaponMods.Select(x=>Recipes.GetBetterNameForEngineeringRecipe(x))) : "";
                    object[] rowobj = new object[] {  EDDConfig.Instance.ConvertTimeToSelectedFromUTC(w.Value.EventTime).ToString(),
                                               w.Value.FriendlyName, //+ ":" + (w.Value.ID%10000) ,
                                               w.Value.Class,
                                               smods,
                                               w.Value.Price.ToString("N0"),
                                               weaponinfo !=null ? (weaponinfo.Primary?"Primary":"Secondary") : "",    
                                               weaponinfo !=null ? weaponinfo.Class.ToString().SplitCapsWord() : "",
                                               weaponinfo !=null ? weaponinfo.DamageType.ToString().SplitCapsWord() : "",
                                               weaponinfo !=null ? weaponinfo.FireMode.ToString().SplitCapsWord() : "",
                                               weapondp?.Damage.ToString("N1") ?? "",
                                               weapondp?.RatePerSec.ToString("N1") ?? "",
                                               weapondp?.DPS.ToString("N1") ?? "",
                                               weapondp?.ClipSize.ToString("N0") ?? "",
                                               weapondp?.HopperSize.ToString("N0") ?? "",
                                               weapondp?.Range.ToString("N0") ?? "",
                                               weapondp?.HeadShotMultiplier.ToString("N1") ?? "",
                    };
                   // System.Diagnostics.Debug.WriteLine("Weapon row {0} {1}", w.Value.EventTime, w.Value.FriendlyName);
                    dataGridViewWeapons.Rows.Add(rowobj);
                }
            }


            dataGridViewWeapons.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewWeapons.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;
            if (firstline >= 0 && firstline < dataGridViewWeapons.RowCount)
                dataGridViewWeapons.SafeFirstDisplayedScrollingRowIndex(firstline);

            dataGridViewWeapons.ResumeLayout();
            extPanelDataGridViewScrollWeapons.ResumeLayout();
        }

        private void dataGridViewWeapons_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index==2 || e.Column.Index==4 || e.Column.Index>=9)
                e.SortDataGridViewColumnNumeric();
        }

        private void DisplaySuits()
        {
            DataGridViewColumn sortcolprev = dataGridViewSuits.SortedColumn != null ? dataGridViewSuits.SortedColumn : dataGridViewSuits.Columns[1];
            SortOrder sortorderprev = dataGridViewSuits.SortedColumn != null ? dataGridViewSuits.SortOrder : SortOrder.Ascending;
            int firstline = dataGridViewSuits.SafeFirstDisplayedScrollingRowIndex();

            extPanelDataGridViewScrollSuits.SuspendLayout();
            dataGridViewSuits.SuspendLayout();

            dataGridViewSuits.Rows.Clear();
          //  System.Diagnostics.Debug.WriteLine("Clear Suit grid");

            if (last_suits >= 0)
            {
                var suitlist = discoveryform.history.SuitList.Suits(last_suits);
                //foreach (var su in suitlist) System.Diagnostics.Debug.WriteLine($"Suit gen {last_suits}: {su.Value.ID} {su.Value.FDName}");

                var cursuit = discoveryform.history.SuitList.CurrentID(last_suits);                     // get current suit ID, or 0 if none
                var curloadout = discoveryform.history.SuitLoadoutList.CurrentID(last_loadout);         // get current loadout ID, or 0 if none

                foreach (var s in suitlist)
                {
                    string stime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(s.Value.EventTime).ToString();
                    string sname = s.Value.FriendlyName;// + ":"+ (s.Value.ID % 1000000).ToStringInvariant();
                    string sprice = s.Value.Price.ToString("N0");
                    string smods = s.Value.SuitMods != null ? string.Join(", ", s.Value.SuitMods.Select(x=> Recipes.GetBetterNameForEngineeringRecipe(x))) : "";

                    var loadouts = discoveryform.history.SuitLoadoutList.GetLoadoutsForSuit(last_loadout, s.Value.ID);

                    if (loadouts == null || loadouts.Count == 0)
                    {
                        object[] rowobj = new object[] { stime, sname + (cursuit == s.Value.ID ? "*" : ""), smods, sprice };
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
                                                stime,      //0
                                                (cursuit == s.Value.ID && curloadout == l.Value.ID ? "*** " : "") + sname,
                                                smods,
                                                sprice,
                                                l.Value.Name + "(" + ((l.Value.ID % 10000).ToString()) + ")",
                                                l.Value.GetModuleDescription("primaryweapon1"),
                                                l.Value.GetModuleDescription("primaryweapon2"),
                                                l.Value.GetModuleDescription("secondaryweapon")
                                                );

                            rw.Cells[1].ToolTipText = "ID:" + s.Value.ID.ToStringInvariant();
                            rw.Cells[1].Tag = dataGridViewSuits.RowCount.ToStringInvariant();        // use a numeric tag to sort it
                            if (i > 0)
                                rw.Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

                            dataGridViewSuits.Rows.Add(rw);

                            DataGridViewRow r = dataGridViewSuits.Rows[dataGridViewSuits.RowCount - 1];
                            r.Tag = s.Value;

                            i++;
                        }
                    }

                }
            }

            dataGridViewSuits.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewSuits.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;
            if (firstline >= 0 && firstline < dataGridViewSuits.RowCount)
                dataGridViewSuits.SafeFirstDisplayedScrollingRowIndex(firstline);

            dataGridViewSuits.ResumeLayout();
            extPanelDataGridViewScrollSuits.ResumeLayout();

        }

        private void dataGridViewSuits_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index == 1)
                e.SortDataGridViewColumnNumeric(usecelltag: true);  // use numeric tag to sort
            else if (e.Column.Index == 3)
                e.SortDataGridViewColumnNumeric();
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

                if (ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Confirm selling of".T(EDTx.UserControlSuitsWeapons_Confirm) + " " +s.FriendlyName, "Delete".T(EDTx.Delete), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
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

