/*
 * Copyright © 2016 - 2024 EDDiscovery development team
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
        private const string dbDisplayInTransparent = "DisplayInTransparent";
        private const string AllNonZeroMarker = "AllNonZero";
        private const string dbSplitter = "Splitter";
        private const string dbMaterialView = "MaterialView";

        private const int MCGrid_MCDType = 0;       // MCD grid MCD tag cell index
        private const int MCGrid_MCMR = 1;           // MCD grid MCD with count tag, may be null

        public enum PanelType { Materials, Commodities, MicroResources, All };
        public PanelType PanelMode;

        private JournalFilterSelector cfs;

        private uint? last_mcl = null;

        private Dictionary<string, int> wantedamounts;      // list of wanted items
        private HashSet<string> displayinshoppinglist;       // list of items to list in transparent mode

        private Font displayfont;


        #region Init

        public UserControlMaterialCommodities()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(this,3);
            colMType.HeaderText = "Type".Tx();     // we exclude MatView, as its mostly non translated things, and just fix type
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip, this);
            BaseUtils.TranslatorMkII.Instance.TranslateToolstrip(contextMenuStrip);
            BaseUtils.TranslatorMkII.Instance.TranslateToolstrip(contextMenuStripSL);
        }


        protected override void Init()
        {
            DBBaseName = PanelMode == PanelType.Materials ? "MaterialsGrid" : PanelMode == PanelType.Commodities ? "CommoditiesGrid" : PanelMode == PanelType.All ? "ResourcesGrid" : "MicroResourcesGrid";

            dataGridViewMC.MakeDoubleBuffered();
            
            //set up word wrap for main grid

            extCheckBoxWordWrap.Checked = GetSetting(dbWrapText, false);
            UpdateWordWrap();
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_Click;

            // now configure grid, filter selector

            var matitems = MaterialCommodityMicroResourceType.GetMaterials(MaterialCommodityMicroResourceType.SortMethod.Alphabetical);
            var mattypes = MaterialCommodityMicroResourceType.GetTypes((x) => x.IsMaterial, true);
            var matcats = MaterialCommodityMicroResourceType.GetCategories((x) => x.IsMaterial, true);
            string matpostfix = "";

            var comitems = MaterialCommodityMicroResourceType.GetCommodities(MaterialCommodityMicroResourceType.SortMethod.AlphabeticalRaresLast);
            var comtypes = MaterialCommodityMicroResourceType.GetTypes((x) => x.IsCommodity, true);
            string compostfix = "";

            var mritems = MaterialCommodityMicroResourceType.GetMicroResources(MaterialCommodityMicroResourceType.SortMethod.Alphabetical);
            var mrcats = MaterialCommodityMicroResourceType.GetCategories((x) => x.IsMicroResources, true);
            string mrpostfix = "";

            bool showall = PanelMode == PanelType.All;      // for panel mode all, everything is shown

            cfs = new JournalFilterSelector();
            cfs.UC.AddAllNone();

            if (showall)
            {
                cfs.UC.AddGroupItem(String.Join(";", matitems.Select(x => x.FDName)) + ";", "All Materials".Tx());
                cfs.UC.AddGroupItem(String.Join(";", comitems.Select(x => x.FDName)) + ";", "All Commodities".Tx());
                cfs.UC.AddGroupItem(String.Join(";", mritems.Select(x => x.FDName)) + ";", "All Microresources".Tx());
            }

            if (PanelMode == PanelType.Materials || showall)        // add materials
            {
                foreach (var t in matcats)
                {
                    string[] members = MaterialCommodityMicroResourceType.GetFDNameMembersOfCategory(t.Item1, true);
                    cfs.UC.AddGroupItem(String.Join(";", members) + ";", t.Item2 + matpostfix);
                }

                AddtoCFS(matitems, mattypes, true, true);
            }

            if (PanelMode == PanelType.Commodities || showall)      // add commodities
            {
                MaterialCommodityMicroResourceType[] rare = comitems.Where(x => x.IsRareCommodity).ToArray();
                cfs.UC.AddGroupItem(String.Join(";", rare.Select(x => x.FDName).ToArray()) + ";", "Rare".Tx()+ compostfix);

                AddtoCFS(comitems, comtypes, false, true);
            }

            if (PanelMode == PanelType.MicroResources || showall)   // add mrs
            {
                foreach (var t in mrcats)
                {
                    string[] members = MaterialCommodityMicroResourceType.GetFDNameMembersOfCategory(t.Item1, true);
                    cfs.UC.AddGroupItem(String.Join(";", members) + ";", t.Item2 + mrpostfix);
                }

                AddtoCFS(mritems, null, true, false);
            }

            if (showall)      // need to post sort as added above in sub groups
                cfs.UC.Sort();

            // Designer has ColName, ColShortName, ColCategory, ColType, ColNumber,   ColBackpack, ColPrice,  ColWanted, ColNeed, ColRecipe
            // Materials:   ColName, ColShortName, ColCategory, ColType, ColNumber,   xxxxxx,      xxxxxxxx,  ColWanted, ColNeed, ColRecipe
            // Commodities: ColName, xxxxxx      , xxxxxx     , ColType, ColNumber,   xxxxxx,      ColPrice,  ColWanted, ColNeed, ColRecipe
            // MicroRes:    ColName, ColShortName, ColCategory, xxxxxx,  Ship locker, ColBackPack, xxxxxxxx,  ColWanted, ColNeed, ColRecipe

            // configure main grid and materials alt view

            if (PanelMode == PanelType.Materials)
            {
                dataGridViewMC.Columns.Remove(ColBackPack);         // do not use back pack

                ColPrice.HeaderText = "Progress".Tx();     // price becomes progress bar

                labelItems1.Text = "Data".Tx();
                labelItems2.Text = "Mats".Tx();

                extCheckBoxMaterialView.Checked = GetSetting(dbMaterialView, true);     // materials alt view selector
                SetMatView();
                this.extCheckBoxMaterialView.CheckedChanged += new System.EventHandler(this.extCheckBoxMaterialView_CheckedChanged);
            }
            else
            {
                // commodities, MRs, All

                // label1 bbecome Total and label2 not used
                labelItems1.Text = "Total".Tx();
                textBoxItems2.Visible = labelItems2.Visible = false;

                extCheckBoxMaterialView.Visible = false;    // only for materials

                checkBoxShowZeros.Location = new Point(textBoxItems1.Right + 8, checkBoxShowZeros.Top); // move into better pos

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
                    ColNumber.HeaderText = "Ship Locker".Tx();     // number becomes ship locker
                }
            }

            // now configure last bits - checkboxzero, cfs

            checkBoxShowZeros.Checked = !GetSetting(dbClearZero, true); // used to be clear zeros, now its show zeros, invert
            checkBoxShowZeros.CheckedChanged += CheckBoxClear_CheckedChanged;

            cfs.AddUserGroups(GetSetting(dbUserGroups, ""), this);
            cfs.SaveSettings += FilterChanged;

            // from the wanted list, set wanted amounts

            JToken json = JToken.Parse(GetSetting(dbWantedList, ""), QuickJSON.JToken.ParseOptions.CheckEOL);
            wantedamounts = json?.ToObject<Dictionary<string, int>>();
            if (wantedamounts == null)
                wantedamounts = new Dictionary<string, int>();

            // from the display list of materials in the upper display panel, configure variable

            json = JToken.Parse(GetSetting(dbDisplayInTransparent, ""), QuickJSON.JToken.ParseOptions.CheckEOL);
            displayinshoppinglist = json?.ToObject<HashSet<string>>();
            if (displayinshoppinglist == null)
                displayinshoppinglist = new HashSet<string>();

            // font 
            displayfont = FontHelpers.GetFont(GetSetting("font", ""), null);        // null if not set
        }

        protected override void LoadLayout()
        {
            dataGridViewMC.RowTemplate.MinimumHeight = Font.ScalePixels(26);
            splitContainerPanel.SplitterDistance(GetSetting(dbSplitter, 0.2));

            DGVLoadColumnLayout(dataGridViewMC);
        }

        protected override void Closing()
        {
            JToken tojson = JToken.FromObject(wantedamounts);
            PutSetting(dbWantedList, tojson.ToString());
            tojson = JToken.FromObject(displayinshoppinglist);
            PutSetting(dbDisplayInTransparent, tojson.ToString());

            PutSetting(dbSplitter, splitContainerPanel.GetSplitterDistance());

            DGVSaveColumnLayout(dataGridViewMC);
            PutSetting(dbUserGroups, cfs.GetUserGroups());
        }

        public override bool SupportTransparency { get { return true; } }
        protected override void SetTransparency(bool on, Color curcol)
        {
            splitContainerPanel.BackColor = this.BackColor =
            splitContainerPanel.Panel1.BackColor =
            splitContainerPanel.Panel2.BackColor =
            extPictureBoxScrollShoppingList.BackColor = extPictureBoxShoppingList.BackColor = curcol;
            panelTop.Visible = dataViewScrollerPanel.Visible = !on;
            extScrollBarShoppingList.AlwaysHideScrollBar = on;
        }

        protected override void TransparencyModeChanged(bool on)
        {
            Display(last_mcl, false);        // need to redraw the shopping list
        }

        public override PanelActionState PerformPanelOperation(UserControlCommonBase sender, object actionobj)
        {
            PushResourceWantedList pr = actionobj as PushResourceWantedList;
            if (pr != null && PanelMode == UserControlMaterialCommodities.PanelType.All)
            {
                MakeVisible();

                var cmd = ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Materials list pushed from panel, set or add to this, or ignore?", "Materials list",
                                                    new string[] { "Cancel".Tx(), "Add".Tx(), "Set".Tx()});
                if (cmd != DialogResult.Ignore)
                {
                    bool add = cmd == DialogResult.Retry;   // Retry is second button, add

                    if (!add)
                        wantedamounts.Clear();

                    foreach (var kvp in pr.Resources)
                    {
                        if (wantedamounts.TryGetValue(kvp.Key.FDName, out int cur))
                        {
                            wantedamounts[kvp.Key.FDName] = add ? (cur + kvp.Value) : kvp.Value;
                        }
                        else
                            wantedamounts[kvp.Key.FDName] = kvp.Value;

                    }

                    Display(last_mcl);
                    return PanelActionState.Success;
                }
                else
                    return PanelActionState.Cancelled;
            }

            HistoryEntry he = actionobj as HistoryEntry;
            if (he != null)
            {
                uint mcl = he.MaterialCommodity;
                if (mcl != last_mcl)
                    Display(mcl);

                return PanelActionState.HandledContinue;
            }

            return PanelActionState.NotHandled;
        }

        #endregion

        #region Display

        protected override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
        }

        // display at this mcl with an optional don't repaint the grid, just the shopping list
        private void Display(uint? mcl, bool repaintgrid = true)       // update display. mcl can be null
        {
            last_mcl = mcl;

            if (mcl == null)
                return;

            if (extCheckBoxMaterialView.Checked == false)       // will be false (default) for everything except materials panel
                DisplayMCMR(repaintgrid);
            else
                DisplayMatView();
        }

        // Display the standard view of the MCMR grid
        private void DisplayMCMR(bool repaintgrid)
        {
            DataGridViewColumn sortcolprev = dataGridViewMC.SortedColumn != null ? dataGridViewMC.SortedColumn : dataGridViewMC.Columns[0];
            SortOrder sortorderprev = dataGridViewMC.SortedColumn != null ? dataGridViewMC.SortOrder : SortOrder.Ascending;
            int firstline = dataGridViewMC.SafeFirstDisplayedScrollingRowIndex();

            string filters = GetSetting(dbFilter, "All");
            string[] filter = filters.SplitNoEmptyStartFinish(';');
            bool all = filter.Length > 0 && filter[0] == "All";
            bool showzeros = checkBoxShowZeros.Checked;

            string shoppinglist = "";
            string contentlist = "";

            if (repaintgrid)
            {
                dataGridViewMC.Rows.Clear();
                textBoxItems1.Text = textBoxItems2.Text = "";
                dataViewScrollerPanel.SuspendLayout();
            }

            MaterialCommodityMicroResourceType[] allitems = PanelMode == PanelType.Materials ? MaterialCommodityMicroResourceType.GetMaterials(MaterialCommodityMicroResourceType.SortMethod.Alphabetical) :
                                                            PanelMode == PanelType.MicroResources ? MaterialCommodityMicroResourceType.GetMicroResources(MaterialCommodityMicroResourceType.SortMethod.Alphabetical) :
                                                            PanelMode == PanelType.Commodities ? MaterialCommodityMicroResourceType.GetCommodities(MaterialCommodityMicroResourceType.SortMethod.AlphabeticalRaresLast) :
                                                            MaterialCommodityMicroResourceType.Get(x => true, MaterialCommodityMicroResourceType.SortMethod.Alphabetical);     // get all sorted

            bool displayallnonzeroitemsinshoppinglist = displayinshoppinglist.Contains(AllNonZeroMarker);

            foreach (MaterialCommodityMicroResourceType mcmrt in allitems)        // we go thru all items..
            {
                MaterialCommodityMicroResource mcmr = DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(last_mcl.Value, mcmrt.FDName);      // at generation mcl, find fdname.

                int[] matcounts = mcmr != null ? mcmr.Counts : MaterialCommodityMicroResource.ZeroCounts;        // if we have some, gets its count array, else return empty array
                int totalcount = matcounts.Sum();

                int wantedamount = 0;
                wantedamounts.TryGetValue(mcmrt.FDName, out wantedamount);

                int need = Math.Max(0, wantedamount - totalcount);

                if (need > 0)
                {
                    shoppinglist = shoppinglist.AppendPrePad(string.Format("Need {0} {1}", mcmrt.TranslatedName, need), Environment.NewLine);
                }

                if (displayinshoppinglist.Contains(mcmrt.FDName) || (totalcount > 0 && displayallnonzeroitemsinshoppinglist))
                {
                    contentlist = contentlist.AppendPrePad(string.Format("{0} {1}", mcmrt.TranslatedName, totalcount), Environment.NewLine);
                }

                if (all || filter.Contains(mcmrt.FDName))      // and see if they are in the filter
                {
                    if (showzeros || totalcount > 0)       // if display zero, or we have some..
                    {
                        string recipes = Recipes.UsedInRecipesByFDName(mcmrt.FDName, Environment.NewLine);   // empty or text
                        object[] rowobj;

                        if (PanelMode == PanelType.Materials)
                        {
                            int limit = mcmrt.MaterialLimitOrNull() ?? 0;

                            rowobj = new[] { mcmrt.TranslatedName, mcmrt.Shortname, mcmrt.TranslatedCategory,
                                                mcmrt.TranslatedType + ( limit>0 ? " (" + limit.ToString() + ")" : "") ,
                                                matcounts[0].ToString(),
                                                "??",
                                                wantedamount.ToStringInvariant(),
                                                need.ToString(),
                                                recipes,
                            };
                        }
                        else if (PanelMode == PanelType.MicroResources)
                        {
                            rowobj = new[] { mcmrt.TranslatedName, mcmrt.Shortname, mcmrt.TranslatedCategory,
                                                matcounts[0].ToString(),  // locker
                                                matcounts[1].ToString(),   // backpack
                                                wantedamount.ToStringInvariant(),
                                                need.ToString(),
                                                recipes,
                            };
                        }
                        else if (PanelMode == PanelType.All)
                        {
                            int? limit = mcmrt.MaterialLimitOrNull();

                            rowobj = new[] { mcmrt.TranslatedName, mcmrt.Shortname, mcmrt.TranslatedCategory,
                                                mcmrt.IsMicroResources ? "" : (mcmrt.TranslatedType + ( limit.HasValue ? " (" + limit.Value.ToString() + ")" : "")) ,
                                                matcounts[0].ToString(),      // number
                                                mcmrt.IsMicroResources ? matcounts[1].ToString(): "",      // backpack
                                                mcmrt.IsCommodity ? (mcmr != null ? mcmr.Price.ToString("0.#") : "-") : "",
                                                wantedamount.ToStringInvariant(),
                                                need.ToString(),
                                                recipes,
                            };
                        }
                        else
                        {                                                                       // commodities
                            rowobj = new[] { mcmrt.TranslatedName, mcmrt.TranslatedType,
                                                matcounts[0].ToString(),
                                                mcmr != null ? mcmr.Price.ToString("0.#") : "-",
                                                wantedamount.ToStringInvariant(),
                                                need.ToString(),
                                                recipes,
                            };

                        }

                        if (repaintgrid)
                        {
                            DataGridViewRow rw = dataGridViewMC.Add(rowobj);        // use extension method to add rowobjs and return

                            if ( PanelMode == PanelType.Materials)
                            {
                                var pcell= new BaseUtils.DataGridViewProgressCell();
                                pcell.BarForeColor = ExtendedControls.Theme.Current.TextBlockSuccessColor;

                                int? limit = mcmrt.MaterialLimitOrNull();
                                pcell.Value = 100 * matcounts[0] / (limit??250);        // just protect it against a rouge synthesised material
                                pcell.TextToRightPreferentially = true;
                                // no need pcell.PercentageTextFormat = "{0:0.#}% (" + limit.ToStringInvariant() + ")";
                                rw.Cells[ColPrice.Index] = pcell;
                            }

                            rw.Cells[ColRecipes.Index].ToolTipText = recipes;   // may be empty, never null
                            rw.Cells[MCGrid_MCDType].Tag = mcmrt;    // always set
                            rw.Cells[MCGrid_MCMR].Tag = mcmr;     // could be null
                        }
                    }
                }
            }

            if (repaintgrid)
            {
                dataViewScrollerPanel.ResumeLayout();

                dataGridViewMC.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridViewMC.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;
                if (firstline >= 0 && firstline < dataGridViewMC.RowCount)
                    dataGridViewMC.SafeFirstDisplayedScrollingRowIndex(firstline);

                var mcllist = DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(last_mcl.Value);
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

            Font dfont = displayfont ?? this.Font;
            Color textcolour = IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
            Color backcolour = IsTransparentModeOn ? Color.Transparent : this.BackColor;

            //System.Diagnostics.Debug.WriteLine($"Draw shopping list with {textcolour} {backcolour}");
            extPictureBoxShoppingList.ClearImageList();

            extPictureBoxShoppingList.AddTextAutoSize(
                new Point(0, 5),
                new Size(10000, 10000),
                shoppinglist.AppendPrePad(contentlist, Environment.NewLine),
                dfont,
                textcolour,
                backcolour,
                1.0F);

            extPictureBoxScrollShoppingList.Render();
        }

        // Display the materials group vview

        private void DisplayMatView()
        {
            dataGridViewMatView.Rows.Clear();

            foreach (MaterialCommodityMicroResourceType.MaterialGroupType groupt in Enum.GetValues(typeof(MaterialCommodityMicroResourceType.MaterialGroupType)))     // relies on MCD being in order
            {
                var matgroup = (MaterialCommodityMicroResourceType.MaterialGroupType)groupt;

                if (matgroup != MaterialCommodityMicroResourceType.MaterialGroupType.NA)
                {
                    var list = MaterialCommodityMicroResourceType.Get(x => x.MaterialGroup == matgroup);

                    var textrow = dataGridViewMatView.RowTemplate.Clone() as DataGridViewRow;
                    var progressrow = dataGridViewMatView.RowTemplate.Clone() as DataGridViewRow;

                    var tbc = new DataGridViewTextBoxCell();
                    tbc.Value = list[0].TranslatedMaterialGroup;
                    textrow.Cells.Add(tbc);
                    tbc = new DataGridViewTextBoxCell();
                    tbc.Value = "";
                    progressrow.Cells.Add(tbc);

                    foreach ( var mcmrt in list)
                    {
                        MaterialCommodityMicroResource m = DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(last_mcl.Value, mcmrt.FDName);      // at generation mcl, find fdname.
                        int count = m != null ? m.Count : 0;
                        int limit = mcmrt.MaterialLimitOrNull() ?? 250;     // protect against a rogue material creeping in due to synth

                        tbc = new DataGridViewTextBoxCell();
                        tbc.Value = $"{mcmrt.TranslatedName} ({count}/{limit})";
                        textrow.Cells.Add(tbc);

                        var pcell = new BaseUtils.DataGridViewProgressCell();
                        pcell.BarForeColor = ExtendedControls.Theme.Current.TextBlockSuccessColor;
                        pcell.Value = 100 * count / limit;
                        pcell.TextToRightPreferentially = true;
                        progressrow.Cells.Add(pcell);

                        string recipes = Recipes.UsedInRecipesByFDName(mcmrt.FDName, Environment.NewLine);    // may be empty. Not null
                        if (recipes.HasChars())
                        {
                            pcell.ToolTipText = tbc.ToolTipText = recipes;
                            pcell.Tag = tbc.Tag = mcmrt.TranslatedName;
                        }
                    }

                    dataGridViewMatView.Rows.Add(textrow);
                    dataGridViewMatView.Rows.Add(progressrow);
                }
            }


            var mcllist = DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(last_mcl.Value);
            var counts = MaterialCommoditiesMicroResourceList.Count(mcllist);
            textBoxItems1.Text = counts[(int)MaterialCommodityMicroResourceType.CatType.Encoded].ToString();
            textBoxItems2.Text = (counts[(int)MaterialCommodityMicroResourceType.CatType.Raw] + counts[(int)MaterialCommodityMicroResourceType.CatType.Manufactured]).ToString();
        }


        #endregion

        #region Toolbar UI for Standard grid

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            cfs.Open(GetSetting(dbFilter, "All"), b, this.FindForm());
        }

        // called back by CFS changing state
        private void FilterChanged(string newset, Object e)
        {
            string filters = GetSetting(dbFilter, "All");
            if (filters != newset)
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
        private void buttonClear_Click(object sender, EventArgs e)
        {
            wantedamounts.Clear();
            Display(last_mcl);
        }

        private void extButtonFont_Click(object sender, EventArgs e)
        {
            Font f = FontHelpers.FontSelection(this.FindForm(), displayfont ?? this.Font);     // will be null on cancel
            string setting = FontHelpers.GetFontSettingString(f);
            //System.Diagnostics.Debug.WriteLine($"Surveyor Font selected {setting}");
            PutSetting("font", setting);
            displayfont = f;
            Display(last_mcl);
        }

        private void buttonExtImport_Click(object sender, EventArgs e)
        {
            var frm = new Forms.ImportExportForm();

            frm.Import(new string[] { "CSV" },
                 new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowImportOptions },
                 new string[] { "CSV|*.csv" }
            );

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                var csv = frm.CSVRead();

                if (csv != null && csv.Rows.Count > 0)
                {
                    var rows = frm.ExcludeHeader ? csv.RowsExcludingHeaderRow : csv.Rows;
                    int amountcol = frm.ExcludeHeader ? csv.CellNumberOfHeaderItem(new string[] { "Qty", "Amount", "Quantity", "Count" }) : -1;

                    wantedamounts.Clear();

                    foreach (var r in rows.EmptyIfNull())
                    {
                        MaterialCommodityMicroResourceType mcrt = null;
                        int count = int.MinValue;

                        if (amountcol >= 0 && amountcol < r.Cells.Count && int.TryParse(r.Cells[amountcol], System.Globalization.NumberStyles.None, csv.FormatCulture, out int cv1))
                            count = cv1;

                        for (int i = 0; i < r.Cells.Count; i++)
                        {
                            var c = r.Cells[i];
                            //System.Diagnostics.Debug.Write($"{c},");

                            var mcd = MaterialCommodityMicroResourceType.GetByEnglishName(c);
                            if (mcd != null)
                                mcrt = mcd;
                            else if (count == int.MinValue && int.TryParse(c, System.Globalization.NumberStyles.None, csv.FormatCulture, out int cv))
                                count = cv;
                        }


                        if (mcrt != null && count > 0)
                            wantedamounts[mcrt.FDName] = count;

                        //System.Diagnostics.Debug.WriteLine($" = {mcrt?.FDName} {count}");
                    }

                    Display(last_mcl);
                }
                else
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to read " + frm.Path, "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        #endregion

        #region Standard grid hooks to grid

        private void dataGridViewMC_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == ColName || e.Column == ColShortName || e.Column == ColRecipes || e.Column == ColCategory || e.Column == ColType)
                e.SortDataGridViewColumnAlpha();
            else
                e.SortDataGridViewColumnNumeric();
        }

        // cell double click on recipe cell 
        private void dataGridViewMC_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewMC.Rows.Count && e.ColumnIndex == ColRecipes.Index)
            {
                DataGridViewRow row = dataGridViewMC.Rows[e.RowIndex];

                // if we are in wrap mode, then set to non wrap
                if (row.Cells[ColRecipes.Index].Style.WrapMode == DataGridViewTriState.True)
                {
                    row.Cells[ColRecipes.Index].Style.WrapMode = DataGridViewTriState.NotSet;
                }
                else
                {
                    // we are in non wrap mode, see if we want to toggle word wrap, or pop out
                    string text = (string)row.Cells[ColRecipes.Index].Value + ".";      // . is just so we have 1 extra padding char
                    double percentage = dataGridViewMC.CalculateTextHeightPercentage(text, ColRecipes.Index);

                    if ( percentage >= 0.75F)
                        OpenRecipeInWindow(row.Cells[0].Value as string, text);
                    else
                        row.Cells[ColRecipes.Index].Style.WrapMode = DataGridViewTriState.True;    //else word wrap
                }

                dataViewScrollerPanel.UpdateScroll();
            }

        }

        // cell end edit, only possible on wanted column
        private void dataGridViewMC_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridViewMC[e.ColumnIndex, e.RowIndex];

            string text = cell.Value as string;

            if (text != null && int.TryParse(text, out int wanted) && wanted >= 0)
            {
                MaterialCommodityMicroResourceType mcd = dataGridViewMC.Rows[e.RowIndex].Cells[MCGrid_MCDType].Tag as MaterialCommodityMicroResourceType;
                wantedamounts[mcd.FDName] = wanted;
                System.Diagnostics.Debug.WriteLine($"Set {mcd.FDName} to {wanted}");

                int need = wanted;

                MaterialCommodityMicroResource mcmr = dataGridViewMC.Rows[e.RowIndex].Cells[MCGrid_MCMR].Tag as MaterialCommodityMicroResource;    // may be null
                if (mcmr != null)
                    need = Math.Max(0, wanted - mcmr.Total);

                dataGridViewMC[ColNeed.Index, e.RowIndex].Value = need.ToString();
                Display(last_mcl, false);
            }
            else
            {
                Console.Beep(512, 400);
                cell.Value = "0";
            }
        }



        #endregion

        #region Right click on standard grid

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewMC.RightClickRowValid)
            {
                string recipe = (string)dataGridViewMC.ClickedRightRow.Cells[ColRecipes.Index].Value; 
                openRecipeInWindowToolStripMenuItem.Enabled = recipe.HasChars();
                var mcd = (MaterialCommodityMicroResourceType)dataGridViewMC.Rows[dataGridViewMC.RightClickRow].Cells[MCGrid_MCDType].Tag;
                displayItemInShoppingListToolStripMenuItem.Checked = displayinshoppinglist.Contains(mcd.FDName);
            }
        }

        private void openRecipeInWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewMC.RightClickRowValid)
                OpenRecipeInWindow(dataGridViewMC.ClickedRightRow.Cells[0].Value as string, dataGridViewMC.ClickedRightRow.Cells[ColRecipes.Index].Value as string);
        }

        private void displayItemInShoppingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewMC.RightClickRowValid)
            {
                var mcd = (MaterialCommodityMicroResourceType)dataGridViewMC.Rows[dataGridViewMC.RightClickRow].Cells[MCGrid_MCDType].Tag;

                if (displayItemInShoppingListToolStripMenuItem.Checked)
                    displayinshoppinglist.Remove(mcd.FDName);
                else
                    displayinshoppinglist.Add(mcd.FDName);

                Display(last_mcl, false);
            }
        }
        private void clearAllDisplayItemsInShoppingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            displayinshoppinglist = new HashSet<string>();
            Display(last_mcl, false);
        }

        private void displayAllInShoppingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!displayinshoppinglist.Contains(AllNonZeroMarker))
                displayinshoppinglist.Add(AllNonZeroMarker);
            Display(last_mcl, false);
        }

        #endregion

        #region Mat group view Grid UI

        private void dataGridViewMatView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewMatView.ClickRowValid && e.ColumnIndex>=0)
                OpenRecipeInWindow(dataGridViewMatView.ClickedRow.Cells[e.ColumnIndex].Tag as string, dataGridViewMatView.ClickedRow.Cells[e.ColumnIndex].ToolTipText);
        }

        #endregion

        #region Mat view mode select
        private void extCheckBoxMaterialView_CheckedChanged(object sender, EventArgs e)
        {
            SetMatView();
            PutSetting(dbMaterialView, extCheckBoxMaterialView.Checked);
            Display(last_mcl);
        }

        private void SetMatView()
        {
            if (extCheckBoxMaterialView.Checked)
            {
                Controls.Remove(dataGridViewMatView);       // disconnect from top level
                dataGridViewMatView.Visible = true;         // visible
                dataViewScrollerPanel.SwapDGV(dataGridViewMatView); // swap in scroller panel, to mat view. Leaves viewMC orphaned
                dataGridViewMC.Visible = false;             // not visible
                dataGridViewMC.Rows.Clear();
                Controls.Add(dataGridViewMC);               // reconnect to top so it can be themed - this is where the inactive DGV is docked to.
                splitContainerPanel.Panel1Collapsed = true;
            }
            else if ( !dataViewScrollerPanel.Controls.Contains(dataGridViewMC))      // initial condition is that it would have this, so don't do anything if so
            {
                Controls.Remove(dataGridViewMC);       // disconnect from top level as it will be here
                dataGridViewMC.Visible = true;
                dataViewScrollerPanel.SwapDGV(dataGridViewMC);
                dataGridViewMatView.Rows.Clear();
                dataGridViewMatView.Visible = false;
                Controls.Add(dataGridViewMatView);          // reconnect back to top as inactive
                splitContainerPanel.Panel1Collapsed = false;
            }

            buttonFilter.Visible =  buttonClear.Visible = checkBoxShowZeros.Visible = extCheckBoxWordWrap.Visible = 
                extButtonFont.Visible = buttonExtImport.Visible = extCheckBoxMaterialView.Checked == false;
        }

        #endregion

        #region Helpers

        // add items to CFS
        public void AddtoCFS(MaterialCommodityMicroResourceType[] items, Tuple<MaterialCommodityMicroResourceType.ItemType, string>[] types, bool showcat, bool showtype)
        {
            if (types != null)
            {
                foreach (var t in types)
                {
                    string[] members = MaterialCommodityMicroResourceType.GetFDNameMembersOfType(t.Item1, true);
                    cfs.UC.AddGroupItem(String.Join(";", members) + ";", t.Item2);
                }
            }

            foreach (var x in items)
            {
                string postfix = "";
                if (showcat)
                    postfix = x.TranslatedCategory;
                if (showtype)
                    postfix = postfix.AppendPrePad(x.TranslatedType, ",");

                if (postfix.Length > 0)
                    postfix = " (" + postfix + ")";

                cfs.UC.Add(x.FDName, x.TranslatedName + postfix);
            }
        }

        void OpenRecipeInWindow(string name, string recipe)
        {
            if (recipe.HasChars())   // may be null on MatView
            {
                recipe = recipe.Replace(": ", Environment.NewLine + "      ");
                ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                info.Info(name, FindForm().Icon, recipe);
                info.Size = new Size(800, 600);
                info.StartPosition = FormStartPosition.CenterParent;
                info.ShowDialog(FindForm());
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
