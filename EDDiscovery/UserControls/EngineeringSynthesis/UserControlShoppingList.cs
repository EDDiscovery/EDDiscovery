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
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static EliteDangerousCore.MaterialCommodityMicroResourceType;

namespace EDDiscovery.UserControls
{
    public partial class UserControlShoppingList : UserControlCommonBase
    {
        private List<Tuple<Recipes.Recipe, int>> EngineeringWanted = new List<Tuple<Recipes.Recipe, int>>();
        private List<Tuple<Recipes.Recipe, int>> SynthesisWanted = new List<Tuple<Recipes.Recipe, int>>();
        private List<Recipes.Recipe> TechBrokerWanted = new List<Recipes.Recipe>();
        private bool showMaxInjections;
        private bool showPlanetMats;
        private bool hidePlanetMatsWithNoCapacity;
        private bool showListAvailability;
        private bool showSystemAvailability;
        private bool useEDSMForSystemAvailability;
        private bool HorizonalSplitContainerPos;

        private bool useHistoric = false;

        private string dbShowInjectionsSave = "ShowFSD"; 
        private string dbShowAllMatsLandedSave = "ShowPlanetMats";
        private string dbHideFullMatsLandedSave = "HideFullMats";
        private string dbHighlightAvailableMats = "HighlightAvailable";
        private string dbShowSystemAvailability = "SystemAvailability";
        private string dbUseEDSMForSystemAvailability = "UseEDSM";
        private string dbToggleShoppingListPosition = "PositionToggle";

        #region Init

        public UserControlShoppingList()
        {
            InitializeComponent();
            // for some reason, designer won't let me set these, so make sure they are in inherit otherwise you get double scaling
            // on a tab page because when you attach this control inside a tab, the scaling occurs.  See MajorTabControl
            this.userControlSynthesis.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.userControlEngineering.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
        }

        public override void Init()
        {
            DBBaseName = "ShoppingList";

            //Can use display number for it, because their names for db save are unique between engineering and synthesis.
            userControlEngineering.isEmbedded = true;
            userControlEngineering.DBBaseName = "SLEngineering";            // makes it unique to the SL
            userControlEngineering.Init(discoveryform, displaynumber);
            useHistoric = userControlEngineering.isHistoric;

            userControlSynthesis.isEmbedded = true;
            userControlSynthesis.DBBaseName = "SLSynthesis";            // makes it unique to the SL
            userControlSynthesis.Init(discoveryform, displaynumber);

            // so the way it works, if the panels ever re-display (for whatever reason) they tell us, and we redisplay

            showMaxInjections = GetSetting(dbShowInjectionsSave, true);
            showPlanetMats = GetSetting(dbShowAllMatsLandedSave, true);
            hidePlanetMatsWithNoCapacity = GetSetting(dbHideFullMatsLandedSave, false);
            showListAvailability = GetSetting(dbHighlightAvailableMats, true);
            showSystemAvailability = GetSetting(dbShowSystemAvailability, true);
            useEDSMForSystemAvailability = GetSetting(dbUseEDSMForSystemAvailability, false);
            HorizonalSplitContainerPos = GetSetting(dbToggleShoppingListPosition, false);

            pictureBoxList.ContextMenuStrip = contextMenuStrip;

            userControlSynthesis.OnDisplayComplete += Synthesis_OnWantedChange;
            userControlEngineering.OnDisplayComplete += Engineering_OnWantedChange;

            var enumlistcms = new Enum[] { EDTx.UserControlShoppingList_showMaxFSDInjectionsToolStripMenuItem, EDTx.UserControlShoppingList_showBodyMaterialsWhenLandedToolStripMenuItem, EDTx.UserControlShoppingList_showBodyMaterialsWhenLandedToolStripMenuItem_onlyCapacityToolStripMenuItem, EDTx.UserControlShoppingList_showAvailableMaterialsInListWhenLandedToolStripMenuItem, EDTx.UserControlShoppingList_showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem, EDTx.UserControlShoppingList_useEDSMDataInSystemAvailabilityToolStripMenuItem, EDTx.UserControlShoppingList_useHistoricMaterialCountsToolStripMenuItem, EDTx.UserControlShoppingList_toggleListPositionToolStripMenuItem };
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);
        }

        public override void SetCursor(IHistoryCursor cur)
        {
            base.SetCursor(cur);
            userControlEngineering.SetCursor(cur);
            userControlSynthesis.SetCursor(cur);
        }

        public override void Closing()
        {
            RevertToNormalSize();
            PutSetting(dbShowInjectionsSave, showMaxInjections);
            PutSetting(dbShowAllMatsLandedSave, showPlanetMats);
            PutSetting(dbHideFullMatsLandedSave, hidePlanetMatsWithNoCapacity);
            PutSetting(dbHighlightAvailableMats, showListAvailability);
            PutSetting(dbShowSystemAvailability, showSystemAvailability);
            PutSetting(dbUseEDSMForSystemAvailability, useEDSMForSystemAvailability);
            PutSetting(dbToggleShoppingListPosition, HorizonalSplitContainerPos);
            userControlEngineering.CloseDown();
            userControlSynthesis.CloseDown();
        }

        #endregion

        #region Display

        public override void InitialDisplay()       // on start up, this will have an empty history
        {
            userControlEngineering.InitialDisplay();
            userControlSynthesis.InitialDisplay();
            showMaxFSDInjectionsToolStripMenuItem.Checked = showMaxInjections;
            showBodyMaterialsWhenLandedToolStripMenuItem.Checked = showPlanetMats;
            onlyCapacityToolStripMenuItem.Checked = hidePlanetMatsWithNoCapacity;
            showAvailableMaterialsInListWhenLandedToolStripMenuItem.Checked = showListAvailability;
            useHistoricMaterialCountsToolStripMenuItem.Checked = useHistoric;
            showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem.Checked = showSystemAvailability;
            useEDSMDataInSystemAvailabilityToolStripMenuItem.Checked = useEDSMForSystemAvailability;
            SetSplitPosition();
            Display();
        }

        public override void LoadLayout()
        {
            userControlEngineering.LoadLayout();
            userControlSynthesis.LoadLayout();
        }

        public override bool SupportTransparency { get { return true; } }

        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBoxList.BackColor = this.BackColor = splitContainerVertical.BackColor = splitContainerRightHorz.BackColor = curcol;
            splitContainerVertical.Panel1.BackColor = splitContainerVertical.Panel2.BackColor = curcol;
            splitContainerRightHorz.Panel1.BackColor = splitContainerRightHorz.Panel2.BackColor = curcol;
            Display();
        }

        private void Engineering_OnWantedChange(List<Tuple<Recipes.Recipe, int>> wanted)
        {
            EngineeringWanted = wanted;
            Display();
        }

        private void Synthesis_OnWantedChange(List<Tuple<Recipes.Recipe, int>> wanted)
        {
            SynthesisWanted = wanted;
            Display();
        }

        private async void Display()
        {
            HistoryEntry last_he = userControlSynthesis.CurrentHistoryEntry;        // sync with what its showing

            if (EngineeringWanted != null && SynthesisWanted != null && last_he != null)    // if we have all the ingredients (get it!)
            {
                List<MaterialCommodityMicroResource> mcl = discoveryform.history.MaterialCommoditiesMicroResources.GetMaterialsSorted(last_he.MaterialCommodity);

                var totals = MaterialCommoditiesRecipe.TotalList(mcl);                  // start with totals present

                Color textcolour = IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
                Color backcolour = this.BackColor;
                List<Tuple<Recipes.Recipe, int>> totalWanted = EngineeringWanted.Concat(SynthesisWanted).ToList();

                var shoppinglist = MaterialCommoditiesRecipe.GetShoppingList(totalWanted, mcl);

                JournalScan sd = null;
                StarScan.SystemNode last_sn = null;

                if (showListAvailability || showPlanetMats)
                {
                    last_sn = await discoveryform.history.StarScan.FindSystemAsync(last_he.System, useEDSMForSystemAvailability);

                    if (last_he.IsLanded && last_sn != null )       // if found node, and landed
                    {
                        sd = last_sn.Find(last_he.WhereAmI)?.ScanData;  // find scan data for this body
                    }
                }

                StringBuilder wantedList = new StringBuilder();

                if (shoppinglist.Any())
                {
                    double available;
                    wantedList.Append("Needed Mats".T(EDTx.UserControlShoppingList_NM) + ":" + Environment.NewLine);
                    List<string> capExceededMats = new List<string>();
                    foreach (var c in shoppinglist)      // and add new..
                    {
                        string present = "";
                        if (showListAvailability && sd != null && sd.HasMaterials)
                        {
                            if (sd.Materials.TryGetValue(c.Item1.Details.FDName, out available))
                            {
                                present = $" {available.ToString("N1")}%";
                            }
                            else
                            {
                                present = " -";
                            }
                        }

                        wantedList.Append($"  {c.Item2} {c.Item1.Details.Name}{present}");
                        int? onHand = mcl.Where(m => m.Details.Shortname == c.Item1.Details.Shortname).FirstOrDefault()?.Count;
                        int totalReq = c.Item2 + (onHand.HasValue ? onHand.Value : 0);
                        if ((c.Item1.Details.Type == MaterialCommodityMicroResourceType.ItemType.VeryCommon && totalReq > VeryCommonCap) ||
                            (c.Item1.Details.Type == MaterialCommodityMicroResourceType.ItemType.Common && totalReq > CommonCap) ||
                            (c.Item1.Details.Type == MaterialCommodityMicroResourceType.ItemType.Standard && totalReq > StandardCap) ||
                            (c.Item1.Details.Type == MaterialCommodityMicroResourceType.ItemType.Rare && totalReq > RareCap) ||
                            (c.Item1.Details.Type == MaterialCommodityMicroResourceType.ItemType.VeryRare && totalReq > VeryRareCap))
                        {
                            capExceededMats.Add(c.Item1.Details.Name);
                        }
                        if (!last_he.IsLanded && last_sn != null)
                        {
                            var landables = last_sn.Bodies.Where(b => b.ScanData != null && (!b.ScanData.IsEDSMBody || useEDSMForSystemAvailability) &&
                                                                 b.ScanData.HasMaterials && b.ScanData.Materials.ContainsKey(c.Item1.Details.FDName));
                            if (landables.Count() > 0)
                            {
                                wantedList.Append("\n    ");
                                List<Tuple<string, double>> allMats = new List<Tuple<string, double>>();
                                foreach (StarScan.ScanNode sn in landables)
                                {
                                    sn.ScanData.Materials.TryGetValue(c.Item1.Details.FDName, out available);
                                    allMats.Add(new Tuple<string, double>(sn.FullName.Replace(last_he.System.Name, "", StringComparison.InvariantCultureIgnoreCase).Trim(), available));
                                }
                                allMats = allMats.OrderByDescending(m => m.Item2).ToList();
                                int n = 1;
                                foreach (Tuple<string, double> m in allMats)
                                {
                                    if (n % 6 == 0) wantedList.Append("\n    ");
                                    wantedList.Append($"{m.Item1.ToUpperInvariant()}: {m.Item2.ToString("N1")}% ");
                                    n++;
                                }
                            }
                        }
                        wantedList.Append("\n");
                    }

                    if (capExceededMats.Any())
                    {
                        wantedList.Append(Environment.NewLine + "Filling Shopping List would exceed capacity for:".T(EDTx.UserControlShoppingList_FS));
                        foreach (string mat in capExceededMats)
                        {
                            wantedList.Append($"\n  {mat}");
                        }
                    }
                }
                else
                {
                    wantedList.Append("No materials currently required.".T(EDTx.UserControlShoppingList_NoMat));
                }

                if (showMaxInjections)
                {
                    var totals2 = MaterialCommoditiesRecipe.TotalList(mcl);                  // start with totals present

                    var basic = MaterialCommoditiesRecipe.HowManyLeft(mcl, totals2, Recipes.SynthesisRecipes.First(r => r.Name == "FSD" && r.level == "Basic"));
                    var standard = MaterialCommoditiesRecipe.HowManyLeft(mcl, totals2, Recipes.SynthesisRecipes.First(r => r.Name == "FSD" && r.level == "Standard"));
                    var premium = MaterialCommoditiesRecipe.HowManyLeft(mcl, totals2, Recipes.SynthesisRecipes.First(r => r.Name == "FSD" && r.level == "Premium"));
                    wantedList.Append(Environment.NewLine +
                        string.Format("Max FSD Injections\r\n   {0} Basic\r\n   {1} Standard\r\n   {2} Premium".T(EDTx.UserControlShoppingList_FSD), basic.Item1, standard.Item1, premium.Item1));
                }

                if (showPlanetMats && sd != null && sd.HasMaterials)
                {
                    wantedList.Append(Environment.NewLine + Environment.NewLine + string.Format("Materials on {0}".T(EDTx.UserControlShoppingList_MO), last_he.WhereAmI) + Environment.NewLine);
                    foreach (KeyValuePair<string, double> mat in sd.Materials)
                    {
                        int? onHand = mcl.Where(m => m.Details.FDName == mat.Key).FirstOrDefault()?.Count;
                        MaterialCommodityMicroResourceType md = GetByFDName(mat.Key);
                        int max = md.MaterialLimit().Value;
                        if (!hidePlanetMatsWithNoCapacity || (onHand.HasValue ? onHand.Value : 0) < max)
                        {
                            wantedList.AppendFormat("   {0} {1}% ({2}/{3})\n", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(mat.Key.ToLowerInvariant()),
                                                                            mat.Value.ToString("N1"), (onHand.HasValue ? onHand.Value : 0), max);
                        }
                    }
                }

                Font font = ExtendedControls.Theme.Current.GetFont;
                pictureBoxList.ClearImageList();
                ExtPictureBox.ImageElement displayList = pictureBoxList.AddTextAutoSize(new Point(0, 0), new Size(1000, 1000), wantedList.ToNullSafeString(), font, textcolour, backcolour, 1.0F);
                pictureBoxList.Render();
                font.Dispose();

                try
                {
                    splitContainerVertical.SplitterDistance = (HorizonalSplitContainerPos ? displayList.Image.Height : displayList.Image.Width) + 8;       // panel left has minimum width to accomodate the text
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Shop list exception " + e);         // swallow the exception - seen an instance of it but wan't reproduce. #2512.
                }

                if (IsTransparentModeOn)
                {
                    RevertToNormalSize();
                    int minWidth = Math.Max(((UserControlForm)FindForm()).TitleBarMinWidth(), displayList.Image.Width) + 8;
                    RequestTemporaryResize(new Size(minWidth, displayList.Image.Height + 4));
                }
                else
                {
                    RevertToNormalSize();       // eng/synth is on, normal size
                }

            }

            // if transparent, we don't show the eng/synth panels
            userControlEngineering.Visible = userControlSynthesis.Visible = !IsTransparentModeOn;
            userControlEngineering.Enabled = userControlSynthesis.Enabled = !IsTransparentModeOn;
        }

        #endregion

        private void showMaxFSDInjectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showMaxInjections = ((ToolStripMenuItem)sender).Checked;
            Display();
        }

        private void showAllMaterialsWhenLandedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPlanetMats = ((ToolStripMenuItem)sender).Checked;
            Display();
        }

        private void showAvailableMaterialsInListWhenLandedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showListAvailability = ((ToolStripMenuItem)sender).Checked;
            Display();
        }

        private void useHistoricMaterialCountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useHistoric = ((ToolStripMenuItem)sender).Checked;
            userControlSynthesis.SetHistoric(useHistoric);
            userControlEngineering.SetHistoric(useHistoric);
            Display();
        }

        private void showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showSystemAvailability = ((ToolStripMenuItem)sender).Checked;
            Display();
        }

        private void useEDSMDataInSystemAvailabilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useEDSMForSystemAvailability = ((ToolStripMenuItem)sender).Checked;
            Display();
        }

        private void onlyCapacityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hidePlanetMatsWithNoCapacity = ((ToolStripMenuItem)sender).Checked;
            Display();
        }

        private void ToggleListPositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HorizonalSplitContainerPos = !HorizonalSplitContainerPos;
            SetSplitPosition();
        }

        private void SetSplitPosition()
        {
            try
            {       // if its having a bad day, it can except
                splitContainerVertical.Orientation = HorizonalSplitContainerPos ? Orientation.Horizontal : Orientation.Vertical;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception due to split container " + ex.Message);
            }
        }
    }
}
