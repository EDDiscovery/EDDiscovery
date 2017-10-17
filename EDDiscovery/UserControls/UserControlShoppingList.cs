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
using static EDDiscovery.UserControls.Recipes;

namespace EDDiscovery.UserControls
{
    public partial class UserControlShoppingList : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;
        private List<Tuple<MaterialCommoditiesList.Recipe, int>> EngineeringWanted = new List<Tuple<MaterialCommoditiesList.Recipe, int>>();
        private List<Tuple<MaterialCommoditiesList.Recipe, int>> SynthesisWanted = new List<Tuple<MaterialCommoditiesList.Recipe, int>>();
        const int PhysicalInventoryCapacity = 1000;
        const int DataInventoryCapacity = 500;

        #region Init

        public UserControlShoppingList()
        {
            InitializeComponent();
        }

        public override void Init(EDDiscoveryForm ed, UserControlCursorType thc, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            displaynumber = vn;

            //Can use display number for it, because their names for db save are unique between engineering and synthesis.
            userControlEngineering.isEmbedded = true;
            userControlEngineering.Init(ed, thc, displaynumber);

            userControlSynthesis.isEmbedded = true;
            userControlSynthesis.Init(ed, thc, displaynumber);

            // so the way it works, if the panels ever re-display (for whatever reason) they tell us, and we redisplay

            userControlSynthesis.OnDisplayComplete += Synthesis_OnWantedChange;
            userControlEngineering.OnDisplayComplete += Engineering_OnWantedChange;
        }

        public override void Closing()
        {
            RevertToNormalSize();
            userControlEngineering.Closing();
            userControlSynthesis.Closing();
        }

        #endregion

        #region Display

        public override void InitialDisplay()       // on start up, this will have an empty history
        {
            userControlEngineering.InitialDisplay();
            userControlSynthesis.InitialDisplay();
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

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            if (he.journalEntry is IMaterialCommodityJournalEntry)
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

                StringBuilder wantedList = new StringBuilder();

                if (shoppinglist.Any())
                {
                    wantedList.Append("Needed Mats:\n");
                    foreach (MaterialCommodities c in shoppinglist.OrderBy(mat => mat.name))      // and add new..
                    {
                        wantedList.AppendFormat("  {0} {1}\n", c.scratchpad, c.name);
                    }

                    int currentMats = mcl.Where(m => m.category == MaterialCommodityDB.MaterialManufacturedCategory || m.category == MaterialCommodityDB.MaterialRawCategory)
                                         .Sum(i => i.count);
                    int currentData = mcl.Where(m => m.category == MaterialCommodityDB.MaterialEncodedCategory).Sum(i => i.count);
                    int neededMats = shoppinglist.Where(m => m.category == MaterialCommodityDB.MaterialManufacturedCategory || m.category == MaterialCommodityDB.MaterialRawCategory)
                                                 .Sum(i => i.scratchpad);
                    int neededData = shoppinglist.Where(m => m.category == MaterialCommodityDB.MaterialEncodedCategory).Sum(i => i.scratchpad);

                    if (currentMats + neededMats > PhysicalInventoryCapacity || currentData + neededData > DataInventoryCapacity)
                    {
                        wantedList.Append("\nNeeded capacity");
                        if (currentMats + neededMats > PhysicalInventoryCapacity)
                        {
                            wantedList.AppendFormat("\n  {0} materials", currentMats + neededMats - PhysicalInventoryCapacity);
                        }
                        if (currentData + neededData > DataInventoryCapacity)
                        {
                            wantedList.AppendFormat("\n  {0} data", currentData + neededData - DataInventoryCapacity);
                        }
                    }
                }
                else
                {
                    wantedList.Append("No materials currently required.");
                }

                if (1 ==1)  // TO-DO replace with a configurable setting
                {
                    MaterialCommoditiesList.ResetUsed(mcl);
                    Tuple<int, int, string> basic = MaterialCommoditiesList.HowManyLeft(mcl, SynthesisRecipes.First(r => r.name == "FSD" && r.level == "Basic"));
                    Tuple<int, int, string> standard = MaterialCommoditiesList.HowManyLeft(mcl, SynthesisRecipes.First(r => r.name == "FSD" && r.level == "Standard"));
                    Tuple<int, int, string> premium = MaterialCommoditiesList.HowManyLeft(mcl, SynthesisRecipes.First(r => r.name == "FSD" && r.level == "Premium"));
                    wantedList.Append($"\n\nMax Injections Available\n   {basic.Item1} Basic\n   {standard.Item1} Standard\n   {premium.Item1} Premium");
                }

                Font font = discoveryform.theme.GetFont;
                pictureBoxList.ClearImageList();
                PictureBoxHotspot.ImageElement displayList = pictureBoxList.AddTextAutoSize(new Point(0, 0), new Size(1000, 1000), wantedList.ToNullSafeString(), font, textcolour, backcolour, 1.0F);
                pictureBoxList.Render();
                font.Dispose();

                // if transparent, we don't show the eng/synth panels

                userControlEngineering.Visible = userControlSynthesis.Visible = !IsTransparent;
                userControlEngineering.Enabled = userControlSynthesis.Enabled = !IsTransparent;

                splitContainerVertical.Panel1MinSize = displayList.img.Width+8;       // panel left has minimum width to accomodate the text

                if (IsTransparent)
                {
                    RevertToNormalSize();
                    int minWidth = Math.Max(((UserControlForm)this.ParentForm).TitleBarMinWidth(), displayList.img.Width) + 8;
                    RequestTemporaryResize(new Size(minWidth, displayList.img.Height + 4));
                }
                else
                {
                    RevertToNormalSize();       // eng/synth is on, normal size
                }

            }
        }

        #endregion

    }
}
