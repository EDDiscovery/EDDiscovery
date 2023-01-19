/*
 * Copyright © 2016 - 2023 EDDiscovery development team
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

using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using EDDiscovery.Controls;
using EliteDangerousCore;
using System.Collections.Generic;
using QuickJSON;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMaterialCommodities : UserControlCommonBase
    {
        public const string dbWrapText = "WrapText";
        public const string dbFilter = "Filter2";
        public const string dbClearZero = "ClearZero";
        private const string dbUserGroups = "UserGroups";
        private const string dbWantedList = "WantedList";

        private const int MCDTag = 0;       // MCD structure tag cell index
        private const int MRTag = 1;        // microresource count tag, may be null
        private const int RTag = 2;         // recipe string

        public enum PanelType { Materials, Commodities, MicroResources, All };
        public PanelType PanelMode;

        private JournalFilterSelector cfs;

        private uint? last_mcl = null;

        Dictionary<string, int> wantedamounts;

        #region Init

        public UserControlMaterialCommodities()
        {
            InitializeComponent();
        }


        public override void Init()
        {
            DBBaseName = PanelMode == PanelType.Materials ? "MaterialsGrid" : PanelMode == PanelType.Commodities ? "CommoditiesGrid" : PanelMode == PanelType.All ? "ResourcesGrid" : "MicroResourcesGrid";

            dataGridViewMC.MakeDoubleBuffered();
            extCheckBoxWordWrap.Checked = GetSetting(dbWrapText, false);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            var enumlist = new Enum[] { EDTx.UserControlMaterialCommodities_ColName, EDTx.UserControlMaterialCommodities_ColShortName, EDTx.UserControlMaterialCommodities_ColCategory, 
                                        EDTx.UserControlMaterialCommodities_ColType, EDTx.UserControlMaterialCommodities_ColNumber,EDTx.UserControlMaterialCommodities_ColBackPack,
                                        EDTx.UserControlMaterialCommodities_ColPrice,
                                        EDTx.UserControlMaterialCommodities_ColRecipes,
                                        EDTx.UserControlMaterialCommodities_ColWanted, EDTx.UserControlMaterialCommodities_ColNeed,
                                        };

            var enumlisttt = new Enum[] { EDTx.UserControlMaterialCommodities_buttonFilter_ToolTip, EDTx.UserControlMaterialCommodities_textBoxItems1_ToolTip, 
                                        EDTx.UserControlMaterialCommodities_textBoxItems2_ToolTip, EDTx.UserControlMaterialCommodities_checkBoxShowZeros_ToolTip, 
                                        EDTx.UserControlMaterialCommodities_extCheckBoxWordWrap_ToolTip };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, null, "UserControlMaterialCommodities");
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this, "UserControlMaterialCommodities");

            cfs = new JournalFilterSelector();
            cfs.AddAllNone();

            var matitems = MaterialCommodityMicroResourceType.GetMaterials(true);
            var mattypes = MaterialCommodityMicroResourceType.GetTypes((x) => x.IsMaterial, true);
            var matcats = MaterialCommodityMicroResourceType.GetCategories((x) => x.IsMaterial, true);
            string matpostfix = "";

            var comitems = MaterialCommodityMicroResourceType.GetCommodities(true);
            var comtypes = MaterialCommodityMicroResourceType.GetTypes((x) => x.IsCommodity, true);
            string compostfix = "";

            var mritems = MaterialCommodityMicroResourceType.GetMicroResources(true);
            var mrcats = MaterialCommodityMicroResourceType.GetCategories((x) => x.IsMicroResources, true);
            string mrpostfix = "";

            bool showall = PanelMode == PanelType.All;

            if (showall)
            {
                cfs.AddGroupOption(String.Join(";", matitems.Select(x => x.FDName)) + ";", "All Materials".T(EDTx.UserControlMaterialCommodities_AllMats));
                cfs.AddGroupOption(String.Join(";", comitems.Select(x => x.FDName)) + ";", "All Commodities".T(EDTx.UserControlMaterialCommodities_AllCommods));
                cfs.AddGroupOption(String.Join(";", mritems.Select(x => x.FDName)) + ";", "All Microresources".T(EDTx.UserControlMaterialCommodities_AllMicroresources));
            }

            if (PanelMode == PanelType.Materials || showall)
            {
                foreach (var t in matcats)
                {
                    string[] members = MaterialCommodityMicroResourceType.GetFDNameMembersOfCategory(t.Item1, true);
                    cfs.AddGroupOption(String.Join(";", members) + ";", t.Item2 + matpostfix);
                }

                AddToControls(matitems, mattypes,true,true);
            }

            if (PanelMode == PanelType.Commodities || showall)
            {
                MaterialCommodityMicroResourceType[] rare = comitems.Where(x => x.IsRareCommodity).ToArray();
                cfs.AddGroupOption(String.Join(";", rare.Select(x => x.FDName).ToArray()) + ";", "Rare".T(EDTx.UserControlMaterialCommodities_Rare) + compostfix);

                AddToControls(comitems, comtypes,  false, true );
            }

            if (PanelMode == PanelType.MicroResources || showall)
            {
                foreach (var t in mrcats)
                {
                    string[] members = MaterialCommodityMicroResourceType.GetFDNameMembersOfCategory(t.Item1, true);
                    cfs.AddGroupOption(String.Join(";", members) + ";", t.Item2 + mrpostfix);
                }

                AddToControls(mritems, null, true, false);
            }

            if (showall )      // need to post sort as added above in sub groups
                cfs.SortStandardOptions();

            // Designer has ColName, ColShortName, ColCategory, ColType, ColNumber,   ColBackpack, ColPrice,  ColRecipe, ColWanted, ColNeed
            // Materials:   ColName, ColShortName, ColCategory, ColType, ColNumber,   xxxxxx,      xxxxxxxx,  ColRecipe, ColWanted, ColNeed
            // Commodities: ColName, xxxxxx      , xxxxxx     , ColType, ColNumber,   xxxxxx,      ColPrice,  ColRecipe, ColWanted, ColNeed
            // MicroRes:    ColName, ColShortName, ColCategory, xxxxxx,  Ship locker, ColBackPack, ColPrice,  ColRecipe, ColWanted, ColNeed

            ColPrice.Tag = ColNumber.Tag = ColWanted.Tag = ColNeed.Tag = "Num";     // these tell the sorter to do numeric sorting

            if (PanelMode == PanelType.Materials)
            {
                dataGridViewMC.Columns.Remove(ColPrice);       // do not use price
                dataGridViewMC.Columns.Remove(ColBackPack);       // do not use price

                labelItems1.Text = "Data".T(EDTx.UserControlMaterialCommodities_Data);
                labelItems2.Text = "Mats".T(EDTx.UserControlMaterialCommodities_Mats);
            }
            else
            {
                labelItems1.Text = "Total".T(EDTx.UserControlMaterialCommodities_Total);
                textBoxItems2.Visible = labelItems2.Visible = false;
                checkBoxShowZeros.Location = new Point(textBoxItems1.Right + 8, checkBoxShowZeros.Top);

                if (PanelMode == PanelType.Commodities)
                {
                    dataGridViewMC.Columns.Remove(ColShortName);       // do not use short name
                    dataGridViewMC.Columns.Remove(ColCategory);       // do not use category
                    dataGridViewMC.Columns.Remove(ColBackPack);     // and this

                }
                else if (PanelMode == PanelType.MicroResources)
                {
                    dataGridViewMC.Columns.Remove(ColType);     // no type column 
                    dataGridViewMC.Columns.Remove(ColPrice);     // no type column 
                    ColNumber.HeaderText = "Ship Locker".T(EDTx.UserControlMaterialCommodities_ShipLocker);     // number becomes ship locker
                }
            }

            checkBoxShowZeros.Checked = !GetSetting(dbClearZero, true); // used to be clear zeros, now its show zeros, invert
            checkBoxShowZeros.CheckedChanged += CheckBoxClear_CheckedChanged;

            cfs.AddUserGroups(GetSetting(dbUserGroups, ""));
            cfs.SaveSettings += FilterChanged;

            JToken json = JToken.Parse(GetSetting(dbWantedList, ""), QuickJSON.JToken.ParseOptions.CheckEOL);
            wantedamounts = json != null ? json.ToObject<Dictionary<string, int>>() : new Dictionary<string, int>();
        }

        public override void LoadLayout()
        {
            dataGridViewMC.RowTemplate.MinimumHeight = Font.ScalePixels(26);
      //      DGVLoadColumnLayout(dataGridViewMC);
        }

        public override void Closing()
        {
            JToken tojson = JToken.FromObject(wantedamounts);
            PutSetting(dbWantedList, tojson.ToString());

            //       DGVSaveColumnLayout(dataGridViewMC); tbd
            PutSetting(dbUserGroups, cfs.GetUserGroupDefinition(1));
        }

        public void AddToControls(MaterialCommodityMicroResourceType[] items, Tuple<MaterialCommodityMicroResourceType.ItemType, string>[] types, bool showcat, bool showtype)
        {
            if (types != null)
            {
                foreach (var t in types)
                {
                    string[] members = MaterialCommodityMicroResourceType.GetFDNameMembersOfType(t.Item1, true);
                    cfs.AddGroupOption(String.Join(";", members) + ";", t.Item2);
                }
            }

            foreach (var x in items)
            {

                string postfix = "";
                if (showcat)
                    postfix = x.TranslatedCategory;
                if ( showtype )
                    postfix = postfix.AppendPrePad(x.TranslatedType, ",");

                if (postfix.Length > 0)
                    postfix = " (" + postfix + ")";

                cfs.AddStandardOption(x.FDName, x.Name + postfix);
            }
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            uint mcl = he.MaterialCommodity;
            if (mcl != last_mcl)
                Display(mcl);
        }

        private void Display(uint? mcl)       // update display. mcl can be null
        {
            last_mcl = mcl;

            DataGridViewColumn sortcolprev = dataGridViewMC.SortedColumn != null ? dataGridViewMC.SortedColumn : dataGridViewMC.Columns[0];
            SortOrder sortorderprev = dataGridViewMC.SortedColumn != null ? dataGridViewMC.SortOrder : SortOrder.Ascending;
            int firstline = dataGridViewMC.SafeFirstDisplayedScrollingRowIndex();

            dataGridViewMC.Rows.Clear();

            textBoxItems1.Text = textBoxItems2.Text = "";

            if (mcl == null)
                return;

            //System.Diagnostics.Debug.WriteLine("Display mcl " + mcl.GetHashCode());

            string filters = GetSetting(dbFilter, "All");
            //System.Diagnostics.Debug.WriteLine("Filter is " + filters);
            string[] filter = filters.SplitNoEmptyStartFinish(';');
            bool all = filter.Length > 0 && filter[0] == "All";
            bool showzeros = checkBoxShowZeros.Checked;

            dataViewScrollerPanel.SuspendLayout();

            MaterialCommodityMicroResourceType[] allitems = PanelMode == PanelType.Materials ? MaterialCommodityMicroResourceType.GetMaterials(true) :
                                                            PanelMode == PanelType.MicroResources ? MaterialCommodityMicroResourceType.GetMicroResources(true) :
                                                            PanelMode == PanelType.Commodities ? MaterialCommodityMicroResourceType.GetCommodities(true) :
                                                            MaterialCommodityMicroResourceType.Get(x => true,true);     // get all sorted

            foreach (MaterialCommodityMicroResourceType mcd in allitems)        // we go thru all items..
            {
                if (all || filter.Contains(mcd.FDName) )      // and see if they are in the filter
                {
                    object[] rowobj;

                    MaterialCommodityMicroResource m = DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(mcl.Value, mcd.FDName);      // at generation mcl, find fdname.

                    int[] matcounts = m != null ? m.Counts : MaterialCommodityMicroResource.ZeroCounts;        // if we have some, gets its count array, else return empty array
                    int totalcount = matcounts.Sum();

                    if (showzeros || totalcount>0)       // if display zero, or we have some..
                    {
                        string recipes = Recipes.UsedInRecipesByFDName(mcd.FDName, Environment.NewLine);

                        int wantedamount = 0;
                        wantedamounts.TryGetValue(mcd.FDName, out wantedamount);

                        int need = Math.Max(0, wantedamount - totalcount);

                        if (PanelMode == PanelType.Materials)
                        {
                            int limit = mcd.MaterialLimit() ?? 0;

                            rowobj = new[] { mcd.Name, mcd.Shortname, mcd.TranslatedCategory,
                                                mcd.TranslatedType + ( limit>0 ? " (" + limit.ToString() + ")" : "") ,
                                                matcounts[0].ToString(),
                                                wantedamount.ToStringInvariant(),
                                                need.ToString(),
                                                recipes,
                            };
                        }
                        else if (PanelMode == PanelType.MicroResources)
                        {
                            rowobj = new[] { mcd.Name, mcd.Shortname, mcd.TranslatedCategory,
                                                matcounts[0].ToString(),  // locker
                                                matcounts[1].ToString(),   // backpack
                                                wantedamount.ToStringInvariant(),
                                                need.ToString(),
                                                recipes,
                            };
                        }
                        else if (PanelMode == PanelType.All)
                        {
                            int? limit = mcd.MaterialLimit();

                            rowobj = new[] { mcd.Name, mcd.Shortname, mcd.TranslatedCategory,
                                                mcd.IsMicroResources ? "" : (mcd.TranslatedType + ( limit.HasValue ? " (" + limit.Value.ToString() + ")" : "")) ,
                                                matcounts[0].ToString(),      // number
                                                mcd.IsMicroResources ? matcounts[1].ToString(): "",      // backpack
                                                mcd.IsCommodity ? (m != null ? m.Price.ToString("0.#") : "-") : "",
                                                wantedamount.ToStringInvariant(),
                                                need.ToString(),
                                                recipes,
                            };
                        }
                        else
                        {                                                                       // commodities
                            rowobj = new[] { mcd.Name, mcd.TranslatedType,
                                                matcounts[0].ToString(),
                                                m != null ? m.Price.ToString("0.#") : "-", 
                                                wantedamount.ToStringInvariant(),
                                                need.ToString(),
                                                recipes,
                            };

                        }

                        int rowno = dataGridViewMC.Rows.Add(rowobj);
                        dataGridViewMC.Rows[rowno].Cells[ ColRecipes.Index].ToolTipText = recipes;
                        dataGridViewMC.Rows[rowno].Cells[MCDTag].Tag = mcd;
                        dataGridViewMC.Rows[rowno].Cells[MRTag].Tag = m;
                        dataGridViewMC.Rows[rowno].Cells[RTag].Tag = recipes;
                    }
                }
            }

            dataViewScrollerPanel.ResumeLayout();

            dataGridViewMC.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewMC.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;
            if (firstline >= 0 && firstline < dataGridViewMC.RowCount)
                dataGridViewMC.SafeFirstDisplayedScrollingRowIndex( firstline);

            var mcllist = DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(mcl.Value);
            var counts = MaterialCommoditiesMicroResourceList.Count(mcllist);

            if (PanelMode == PanelType.Materials)
            {
                textBoxItems1.Text = counts[(int)MaterialCommodityMicroResourceType.CatType.Encoded].ToString();
                textBoxItems2.Text = (counts[(int)MaterialCommodityMicroResourceType.CatType.Raw] + counts[(int)MaterialCommodityMicroResourceType.CatType.Manufactured]).ToString();
            }
            else if (PanelMode == PanelType.MicroResources)
            {
                textBoxItems1.Text = (counts[(int)MaterialCommodityMicroResourceType.CatType.Data] + counts[(int)MaterialCommodityMicroResourceType.CatType.Component] +
                                       counts[(int)MaterialCommodityMicroResourceType.CatType.Item] + counts[(int)MaterialCommodityMicroResourceType.CatType.Consumable]).ToString();
            }
            else if (PanelMode == PanelType.Commodities)
            {
                textBoxItems1.Text = counts[(int)MaterialCommodityMicroResourceType.CatType.Commodity].ToString();
            }
            else
            {
                textBoxItems1.Text = counts.Sum().ToString();
            }
        }

        #endregion

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            cfs.Open(GetSetting(dbFilter, "All"), b, this.FindForm());
        }

        private void FilterChanged(string newset, Object e)
        {
            string filters = GetSetting(dbFilter, "All");
            if ( filters != newset )
            {
                PutSetting(dbFilter, newset);
                Display(last_mcl);
            }
        }

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            PutSetting(dbWrapText, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }

        private void UpdateWordWrap()
        {
            dataGridViewMC.SetWordWrap(extCheckBoxWordWrap.Checked);
            dataViewScrollerPanel.UpdateScroll();
        }

        private void CheckBoxClear_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting(dbClearZero, !checkBoxShowZeros.Checked);    // negative because we changed button sense
            Display(last_mcl);
        }

        private void dataGridViewMC_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            object tag = e.Column.Tag;
            if ( tag != null)
                e.SortDataGridViewColumnNumeric();
        }

        private void dataGridViewMC_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rcell = dataGridViewMC.ColumnCount - 1;
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewMC.Rows.Count && e.ColumnIndex == rcell)
            {
                DataGridViewRow row = dataGridViewMC.Rows[e.RowIndex];

                if (row.Height > dataGridViewMC.RowTemplate.Height)
                {
                    row.Height = dataGridViewMC.RowTemplate.Height;
                }
                else
                {

                    using (Graphics g = Parent.CreateGraphics())
                    {
                        using (StringFormat f = new StringFormat())
                        {
                            string ms = (string)row.Cells[rcell].Value + ".";
                            var sz = g.MeasureString(ms, dataGridViewMC.Font, new SizeF(dataGridViewMC.Columns[rcell].Width - 4, 1000), f);
                            sz.Height *= 63.0f / 56.0f; // it underestimates of course, scale it a bit
                            row.Height = (int)sz.Height;
                            //System.Diagnostics.Debug.WriteLine("Measured h" + sz);
                        }

                    }
                }

                dataViewScrollerPanel.UpdateScroll();
            }

        }

        private void dataGridViewMC_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridViewMC[e.ColumnIndex, e.RowIndex];

            string text = cell.Value as string;

            if ( text != null && int.TryParse(text, out int wanted) && wanted >= 0)    
            {
                MaterialCommodityMicroResourceType mcd = dataGridViewMC.Rows[e.RowIndex].Cells[MCDTag].Tag as MaterialCommodityMicroResourceType;
                wantedamounts[mcd.FDName] = wanted;
                System.Diagnostics.Debug.WriteLine($"Set {mcd.FDName} to {wanted}");

                MaterialCommodityMicroResource m = dataGridViewMC.Rows[e.RowIndex].Cells[MRTag].Tag as MaterialCommodityMicroResource;

                int need = wanted;

                if ( m != null)
                    need = Math.Max(0,wanted - m.Total);

                dataGridViewMC[ColNeed.Index, e.RowIndex].Value = need.ToString();
            }
            else
            {
                Console.Beep(512,400);
                cell.Value = "0";
            }

        }


        #region Right click

        private void openRecipeInWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewMC.RightClickRowValid)
            {
                string mats = (string)dataGridViewMC.Rows[dataGridViewMC.RightClickRow].Cells[RTag].Tag;
                if (mats != null)   // sheer paranoia.
                {
                    mats = mats.Replace(": ", Environment.NewLine + "      ");
                    ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                    info.Info(dataGridViewMC.Rows[dataGridViewMC.RightClickRow].Cells[0].Value as string, FindForm().Icon, mats);
                    info.Size = new Size(800, 600);
                    info.StartPosition = FormStartPosition.CenterParent;
                    info.ShowDialog(FindForm());
                }
            }
        }

        #endregion

    }

    public class UserControlMaterials : UserControlMaterialCommodities
    {
        public UserControlMaterials()
        {
            PanelMode = UserControlMaterialCommodities.PanelType.Materials;
        }
    }

    public class UserControlCommodities : UserControlMaterialCommodities
    {
        public UserControlCommodities()
        {
            PanelMode = UserControlMaterialCommodities.PanelType.Commodities;
        }
    }

    public class UserControlMicroResources : UserControlMaterialCommodities
    {
        public UserControlMicroResources()
        {
            PanelMode = UserControlMaterialCommodities.PanelType.MicroResources;
        }
    }
    public class UserControlAllResources : UserControlMaterialCommodities
    {
        public UserControlAllResources()
        {
            PanelMode = UserControlMaterialCommodities.PanelType.All;
        }
    }
}
