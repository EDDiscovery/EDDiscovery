﻿/*
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
using EDDiscovery.Controls;
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EliteDangerousCore.JournalEvents;
using static EDDiscovery.UserControls.Recipes;

namespace EDDiscovery.UserControls
{
    public partial class UserControlShoppingList : UserControlCommonBase
    {
        private List<Tuple<MaterialCommoditiesList.Recipe, int>> EngineeringWanted = new List<Tuple<MaterialCommoditiesList.Recipe, int>>();
        private List<Tuple<MaterialCommoditiesList.Recipe, int>> SynthesisWanted = new List<Tuple<MaterialCommoditiesList.Recipe, int>>();
        private bool showMaxInjections;
        private bool showPlanetMats;
        private bool showListAvailability;
        private bool showSystemAvailability;
        private bool useEDSMForSystemAvailability;
        private bool useHistoric = false;
        private string DbShowInjectionsSave { get { return "ShoppingListShowFSD" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbShowAllMatsLandedSave { get { return "ShoppingListShowPlanetMats" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DbHighlightAvailableMats { get { return "ShoppingListHighlightAvailable" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DBShowSystemAvailability { get { return "ShoppingListSystemAvailability" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private string DBUseEDSMForSystemAvailability { get { return "ShoppingListUseEDSM" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        const int PhysicalInventoryCapacity = 1000;
        const int DataInventoryCapacity = 500;

        #region Init

        public UserControlShoppingList()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            //Can use display number for it, because their names for db save are unique between engineering and synthesis.
            userControlEngineering.isEmbedded = true;
            userControlEngineering.Init(discoveryform, uctg, displaynumber);
            useHistoric = userControlEngineering.isHistoric;

            userControlSynthesis.isEmbedded = true;
            userControlSynthesis.Init(discoveryform, uctg, displaynumber);

            // so the way it works, if the panels ever re-display (for whatever reason) they tell us, and we redisplay

            showMaxInjections = SQLiteDBClass.GetSettingBool(DbShowInjectionsSave, true);
            showPlanetMats = SQLiteDBClass.GetSettingBool(DbShowAllMatsLandedSave, true);
            showListAvailability = SQLiteDBClass.GetSettingBool(DbHighlightAvailableMats, true);
            showSystemAvailability = SQLiteDBClass.GetSettingBool(DBShowSystemAvailability, true);
            useEDSMForSystemAvailability = SQLiteDBClass.GetSettingBool(DBUseEDSMForSystemAvailability, false);
            pictureBoxList.ContextMenuStrip = contextMenuConfig;

            userControlSynthesis.OnDisplayComplete += Synthesis_OnWantedChange;
            userControlEngineering.OnDisplayComplete += Engineering_OnWantedChange;
        }

        public override void Closing()
        {
            RevertToNormalSize();
            SQLiteDBClass.PutSettingBool(DbShowInjectionsSave, showMaxInjections);
            SQLiteDBClass.PutSettingBool(DbShowAllMatsLandedSave, showPlanetMats);
            SQLiteDBClass.PutSettingBool(DbHighlightAvailableMats, showListAvailability);
            SQLiteDBClass.PutSettingBool(DBShowSystemAvailability, showSystemAvailability);
            SQLiteDBClass.PutSettingBool(DBUseEDSMForSystemAvailability, useEDSMForSystemAvailability);
            userControlEngineering.Closing();
            userControlSynthesis.Closing();
        }

        #endregion

        #region Display

        public override void InitialDisplay()       // on start up, this will have an empty history
        {
            userControlEngineering.InitialDisplay();
            userControlSynthesis.InitialDisplay();
            showMaxFSDInjectionsToolStripMenuItem.Checked = showMaxInjections;
            showAllMaterialsWhenLandedToolStripMenuItem.Checked = showPlanetMats;
            showAvailableMaterialsInListWhenLandedToolStripMenuItem.Checked = showListAvailability;
            useHistoricMaterialCountsToolStripMenuItem.Checked = useHistoric;
            showSystemAvailabilityOfMaterialsInShoppingListToolStripMenuItem.Checked = showSystemAvailability;
            useEDSMDataInSystemAvailabilityToolStripMenuItem.Checked = useEDSMForSystemAvailability;
            Display();
        }

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBoxList.BackColor = this.BackColor = splitContainerVertical.BackColor = splitContainerRightHorz.BackColor = curcol;
            splitContainerVertical.Panel1.BackColor = splitContainerVertical.Panel2.BackColor = curcol;
            splitContainerRightHorz.Panel1.BackColor = splitContainerRightHorz.Panel2.BackColor = curcol;
            Display();
        }

        private void Engineering_OnWantedChange(List<Tuple<MaterialCommoditiesList.Recipe, int>> wanted)
        {
            EngineeringWanted = wanted;
            Display();
        }

        private void Synthesis_OnWantedChange(List<Tuple<MaterialCommoditiesList.Recipe, int>> wanted)
        {
            SynthesisWanted = wanted;
            Display();
        }

        private void Display(HistoryEntry he, HistoryList hl)
        {
            Display();
        }
        
        private void Display()
        {
            HistoryEntry last_he = userControlSynthesis.CurrentHistoryEntry;        // sync with what its showing

            if (EngineeringWanted != null && SynthesisWanted != null && last_he != null)    // if we have all the ingredients (get it!)
            {
                List<MaterialCommodities> mcl = last_he.MaterialCommodity.Sort(false);
                MaterialCommoditiesList.ResetUsed(mcl);
                Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                Color backcolour = this.BackColor;
                List<Tuple<MaterialCommoditiesList.Recipe, int>> totalWanted = EngineeringWanted.Concat(SynthesisWanted).ToList();

                List<MaterialCommodities> shoppinglist = MaterialCommoditiesList.GetShoppingList(totalWanted, mcl);
                JournalScan sd = null;
                StarScan.SystemNode last_sn = null;

                if (last_he.IsLanded && (showListAvailability || showPlanetMats))
                {
                    sd = discoveryform.history.GetScans(last_he.System.Name).Where(sc => sc.BodyName == last_he.WhereAmI).FirstOrDefault();
                }
                if (!last_he.IsLanded && showSystemAvailability)  //replace true with a setting
                {
                    last_sn = discoveryform.history.starscan.FindSystem(last_he.System, useEDSMForSystemAvailability);
                }

                StringBuilder wantedList = new StringBuilder();

                if (shoppinglist.Any())
                {
                    double available;
                    wantedList.Append("Needed Mats:\n");
                    foreach (MaterialCommodities c in shoppinglist.OrderBy(mat => mat.name))      // and add new..
                    {
                        string present = "";
                        if (showListAvailability)
                        {
                            if (sd != null)
                            {
                                if (sd.Materials.TryGetValue(c.fdname, out available))
                                {
                                    present = $" {available.ToString("N1")}%";
                                }
                                else
                                { present = " -"; }
                            }
                        }
                        wantedList.Append($"  {c.scratchpad} {c.name}{present}");
                        if (!last_he.IsLanded && last_sn != null)
                        {
                            var landables = last_sn.Bodies.Where(b => b.ScanData != null && (!b.ScanData.IsEDSMBody || useEDSMForSystemAvailability) && 
                                                                 b.ScanData.HasMaterials && b.ScanData.Materials.ContainsKey(c.fdname));
                            if (landables.Count() > 0)
                            {
                                wantedList.Append("\n    ");
                                List<Tuple<string, double>> allMats = new List<Tuple<string, double>>();
                                foreach (StarScan.ScanNode sn in landables)
                                {
                                    sn.ScanData.Materials.TryGetValue(c.fdname, out available);
                                    allMats.Add(new Tuple<string, double>(sn.fullname.Replace(last_he.System.Name, "", StringComparison.InvariantCultureIgnoreCase).Trim(), available));
                                }
                                allMats = allMats.OrderByDescending(m => m.Item2).ToList();
                                int n = 1;
                                foreach(Tuple<string, double> m in allMats)
                                {
                                    if (n % 6 == 0) wantedList.Append("\n    ");
                                    wantedList.Append($"{m.Item1.ToUpperInvariant()}: {m.Item2.ToString("N1")}% ");
                                    n++;
                                }
                            }
                        }
                        wantedList.Append("\n");
                    }

                    int currentMats = mcl.Where(m => m.category == MaterialCommodityDB.MaterialManufacturedCategory || m.category == MaterialCommodityDB.MaterialRawCategory)
                                         .Sum(i => i.count);
                    int currentData = mcl.Where(m => m.category == MaterialCommodityDB.MaterialEncodedCategory).Sum(i => i.count);
                    int neededMats = shoppinglist.Where(m => m.category == MaterialCommodityDB.MaterialManufacturedCategory || m.category == MaterialCommodityDB.MaterialRawCategory)
                                                 .Sum(i => i.scratchpad);
                    int neededData = shoppinglist.Where(m => m.category == MaterialCommodityDB.MaterialEncodedCategory).Sum(i => i.scratchpad);

                    if (currentMats + neededMats > PhysicalInventoryCapacity || currentData + neededData > DataInventoryCapacity)
                    {
                        wantedList.Append("\nCapacity Warning:");
                        if (currentMats + neededMats > PhysicalInventoryCapacity)
                        {
                            wantedList.Append($"\n  {PhysicalInventoryCapacity - currentMats}/{neededMats} materials space free");
                        }
                        if (currentData + neededData > DataInventoryCapacity)
                        {
                            wantedList.Append($"\n  {DataInventoryCapacity - currentData}/{neededData} data space free");
                        }
                        wantedList.Append("\n");
                    }
                }
                else
                {
                    wantedList.Append("No materials currently required.");
                }

                if (showMaxInjections)
                {
                    MaterialCommoditiesList.ResetUsed(mcl);
                    Tuple<int, int, string> basic = MaterialCommoditiesList.HowManyLeft(mcl, SynthesisRecipes.First(r => r.name == "FSD" && r.level == "Basic"));
                    Tuple<int, int, string> standard = MaterialCommoditiesList.HowManyLeft(mcl, SynthesisRecipes.First(r => r.name == "FSD" && r.level == "Standard"));
                    Tuple<int, int, string> premium = MaterialCommoditiesList.HowManyLeft(mcl, SynthesisRecipes.First(r => r.name == "FSD" && r.level == "Premium"));
                    wantedList.Append($"\nMax FSD Injections\n   {basic.Item1} Basic\n   {standard.Item1} Standard\n   {premium.Item1} Premium");
                }

                if (showPlanetMats && sd != null)  //for user configurable setting
                {
                    wantedList.Append($"\n\nMaterials on {last_he.WhereAmI}\n");
                    foreach (KeyValuePair<string, double> mat in sd.Materials)
                    {
                        wantedList.AppendFormat("   {0} {1}%\n", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(mat.Key.ToLower(System.Globalization.CultureInfo.InvariantCulture)),
                                                                        mat.Value.ToString("N1"));
                    }
                }

                Font font = discoveryform.theme.GetFont;
                pictureBoxList.ClearImageList();
                PictureBoxHotspot.ImageElement displayList = pictureBoxList.AddTextAutoSize(new Point(0, 0), new Size(1000, 1000), wantedList.ToNullSafeString(), font, textcolour, backcolour, 1.0F);
                pictureBoxList.Render();
                font.Dispose();

                // if transparent, we don't show the eng/synth panels

                userControlEngineering.Visible = userControlSynthesis.Visible = !IsTransparent;
                userControlEngineering.Enabled = userControlSynthesis.Enabled = !IsTransparent;

                splitContainerVertical.Panel1MinSize = displayList.img.Width + 8;       // panel left has minimum width to accomodate the text

                if (IsTransparent)
                {
                    RevertToNormalSize();
                    int minWidth = Math.Max(((UserControlForm)FindForm()).TitleBarMinWidth(), displayList.img.Width) + 8;
                    RequestTemporaryResize(new Size(minWidth, displayList.img.Height + 4));
                }
                else
                {
                    RevertToNormalSize();       // eng/synth is on, normal size
                }

            }
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
    }
}
