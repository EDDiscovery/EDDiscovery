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

namespace EDDiscovery.UserControls
{
    public partial class UserControlShoppingList : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;
        private UserControlCursorType uctg;
        private List<Tuple<MaterialCommoditiesList.Recipe, int>> EngineeringWanted = new List<Tuple<MaterialCommoditiesList.Recipe, int>>();
        private List<Tuple<MaterialCommoditiesList.Recipe, int>> SynthesisWanted = new List<Tuple<MaterialCommoditiesList.Recipe, int>>();
        private Font displayfont;
        HistoryEntry last_he = null;
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
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            uctg = thc;
            displaynumber = vn;
            displayfont = discoveryform.theme.GetFont;

            userControlEngineering.isEmbedded = true;
            userControlEngineering.OnChangedEngineeringWanted += Engineering_OnWantedChange;
            //Can use display number for it, because their names for db save are unique between engineering and synthesis.
            userControlEngineering.Init(ed, thc, displaynumber);
            userControlSynthesis.isEmbedded = true;
            userControlSynthesis.OnChangedSynthesisWanted += Synthesis_OnWantedChange;
            userControlSynthesis.Init(ed, thc, displaynumber);
        }

        public override void ChangeCursorType(UserControlCursorType thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            last_he = uctg.GetCurrentHistoryEntry;
            userControlEngineering.InitialDisplay();
            userControlSynthesis.InitialDisplay();
            Display();
        }

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBoxList.BackColor = this.BackColor = splitContainer1.BackColor = splitContainer2.BackColor = curcol;
            splitContainer1.Panel1.BackColor = splitContainer1.Panel2.BackColor = curcol;
            splitContainer2.Panel1.BackColor = splitContainer2.Panel2.BackColor = curcol;
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
            last_he = he;
            if (he is IMaterialCommodityJournalEntry)
                Display();
        }

        private void Display(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            Display();
        }
        
        private void Display()
        {
            if (last_he != null)
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
                    foreach (MaterialCommodities c in shoppinglist.OrderBy(mat => mat.name))      // and add new..
                    {
                        wantedList.AppendFormat("{0} {1} required\n", c.scratchpad, c.name);
                    }

                    int currentMats = mcl.Where(m => m.category == MaterialCommodityDB.MaterialManufacturedCategory || m.category == MaterialCommodityDB.MaterialRawCategory)
                                         .Sum(i => i.count);
                    int currentData = mcl.Where(m => m.category == MaterialCommodityDB.MaterialEncodedCategory).Sum(i => i.count);
                    int neededMats = shoppinglist.Where(m => m.category == MaterialCommodityDB.MaterialManufacturedCategory || m.category == MaterialCommodityDB.MaterialRawCategory)
                                                 .Sum(i => i.scratchpad);
                    int neededData = shoppinglist.Where(m => m.category == MaterialCommodityDB.MaterialEncodedCategory).Sum(i => i.scratchpad);

                    if (currentMats + neededMats > PhysicalInventoryCapacity || currentData + neededData > DataInventoryCapacity)
                    {
                        wantedList.Append("\nWarning");
                        if (currentMats + neededMats > PhysicalInventoryCapacity)
                        {
                            wantedList.AppendFormat("\n   Needed space for materials is {0}\n   Current available is {1}", neededMats, PhysicalInventoryCapacity - currentMats);
                        }
                        if (currentData + neededData > DataInventoryCapacity)
                        {
                            wantedList.AppendFormat("\n   Needed space for data is {0}\n   Current available is {1}", neededData, DataInventoryCapacity - currentData);
                        }
                    }
                }
                else
                { wantedList.Append("No materials currently required."); }

                pictureBoxList.ClearImageList();
                PictureBoxHotspot.ImageElement displayList = pictureBoxList.AddTextAutoSize(new Point(0, 0), new Size(1000,1000), wantedList.ToNullSafeString(), displayfont, textcolour, backcolour, 1.0F);
                pictureBoxList.Render();
                userControlEngineering.Visible = userControlSynthesis.Visible = !IsTransparent;
                userControlEngineering.Enabled = userControlSynthesis.Enabled = !IsTransparent;
                if (IsTransparent)
                {
                    RevertToNormalSize();
                    int minWidth = ((UserControlForm)this.ParentForm).TitleBarMinWidth();
                    splitContainer1.Panel2MinSize = 0;
                    RequestTemporaryResize(new Size(Math.Max(minWidth, displayList.img.Width) + 8, displayList.img.Height + 4));
                }
                else
                {
                    RevertToNormalSize();
                }
                splitContainer1.Panel1MinSize = 0;
                if (displayList.img.Width < splitContainer1.Width - splitContainer1.Panel2MinSize)
                {
                    splitContainer1.SplitterDistance = displayList.img.Width;
                    splitContainer1.Panel1MinSize = displayList.img.Width;
                }
                else
                    splitContainer1.SplitterDistance = Math.Max(0, splitContainer1.Width - splitContainer1.Panel2MinSize);
            }
        }

        #endregion

        #region Layout

        public override void Closing()
        {
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            RevertToNormalSize();
            userControlEngineering.Closing();
            userControlSynthesis.Closing();
        }

        #endregion
    }
}
