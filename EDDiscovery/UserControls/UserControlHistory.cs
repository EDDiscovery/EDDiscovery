/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using EliteDangerousCore.EDSM;
using System.Threading.Tasks;
using EDDiscovery.Controls;
using System.Threading;
using System.Collections.Concurrent;
using EliteDangerousCore.EDDN;
using Newtonsoft.Json.Linq;
using EDDiscovery.UserControls;
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlHistory : UserControlCommonBase
    {
        public HistoryEntry GetTravelHistoryCurrent {  get { return userControlTravelGrid.GetCurrentHistoryEntry; } }

        public UserControlTravelGrid GetTravelGrid { get { return userControlTravelGrid; } }

        public ExtendedControls.TabStrip GetTabStrip( string name )
        {
            if (name.Equals(tabStripBottom.Name, StringComparison.InvariantCultureIgnoreCase))
                return tabStripBottom;
            if (name.Equals(tabStripBottomRight.Name, StringComparison.InvariantCultureIgnoreCase))
                return tabStripBottomRight;
            if (name.Equals(tabStripMiddleRight.Name, StringComparison.InvariantCultureIgnoreCase))
                return tabStripMiddleRight;
            if (name.Equals(tabStripTopRight.Name, StringComparison.InvariantCultureIgnoreCase))
                return tabStripTopRight;
            return null;
        }

        #region Initialisation

        public UserControlHistory()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            userControlTravelGrid.Init(discoveryform, userControlTravelGrid, displaynumber);       // primary first instance - this registers with events in discoveryform to get info
                                                        // then this display, to update its own controls..

            TabConfigure(tabStripBottom,"Bottom", DisplayNumberHistoryBotLeft);          // codes are used to save info, 0 = primary (journal/travelgrid), 1..N are popups, these are embedded UCs
            TabConfigure(tabStripBottomRight,"Bottom-Right", DisplayNumberHistoryBotRight);
            TabConfigure(tabStripMiddleRight, "Middle-Right", DisplayNumberHistoryMidRight);
            TabConfigure(tabStripTopRight, "Top-Right", DisplayNumberHistoryTopRight);

            userControlTravelGrid.OnChangedSelection += ChangedSelection;   // and if the user clicks on something
            userControlTravelGrid.OnKeyDownInCell += OnKeyDownInCell;
        }

        #endregion

        #region TAB control

        void TabConfigure(ExtendedControls.TabStrip t, string name, int displayno)
        {
            t.ImageList = PanelInformation.GetPanelImages();
            t.TextList = PanelInformation.GetPanelDescriptions();
            t.TagList = PanelInformation.GetPanelIDs().Cast<Object>().ToArray();
            t.Tag = displayno;             // these are IDs for purposes of identifying different instances of a control.. 0 = main ones (main travel grid, main tab journal). 1..N are popups
            t.OnRemoving += TabRemoved;
            t.OnCreateTab += TabCreate;
            t.OnPostCreateTab += TabPostCreate;
            t.OnPopOut += TabPopOut;
            t.Name = name;
        }

        void TabRemoved(ExtendedControls.TabStrip t, Control ctrl )     // called by tab strip when a control is removed
        {
            UserControlCommonBase uccb = ctrl as UserControlCommonBase;
            uccb.Closing();
        }

        Control TabCreate(ExtendedControls.TabStrip t, int si)        // called by tab strip when selected index changes.. create a new one.. only create.
        {
            PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID((PanelInformation.PanelIDs)t.TagList[si]);
            Control c = PanelInformation.Create(pi.PopoutID);
            c.Name = pi.WindowTitle;        // tabs uses Name field for display, must set it

            discoveryform.ActionRun(Actions.ActionEventEDList.onPanelChange, null, 
                new Conditions.ConditionVariables(new string[] { "PanelTabName", pi.WindowRefName, "PanelTabTitle" , pi.WindowTitle , "PanelName" , t.Name }));

            return c;
        }

        void TabPostCreate(ExtendedControls.TabStrip t, Control ctrl , int i)        // called by tab strip after control has been added..
        {                                                           // now we can do the configure of it, with the knowledge the tab has the right size
            int displaynumber = (int)t.Tag;                         // tab strip - use tag to remember display id which helps us save context.

            UserControlCommonBase uc = ctrl as UserControlCommonBase;

            if (uc != null)
            {
                uc.Init(discoveryform, userControlTravelGrid, displaynumber);
                uc.LoadLayout();
                uc.InitialDisplay();
            }

            //System.Diagnostics.Debug.WriteLine("And theme {0}", i);
            discoveryform.theme.ApplyToControls(t);
        }

        void TabPopOut(ExtendedControls.TabStrip t, int i)        // pop out clicked
        {
            discoveryform.PopOuts.PopOut((PanelInformation.PanelIDs)t.TagList[i]);
        }

        #endregion


#region Grid Layout

        public override void LoadLayout() 
        {
            // ORDER IMPORTANT for right outer/inner splitter, otherwise windows fixes it 

            if (!EDDOptions.Instance.NoWindowReposition)
            {
                splitContainerLeftRight.SplitterDistance(SQLiteDBClass.GetSettingDouble("TravelControlSpliterLR", 0.75));
                splitContainerLeft.SplitterDistance(SQLiteDBClass.GetSettingDouble("TravelControlSpliterL", 0.75));
                splitContainerRightOuter.SplitterDistance(SQLiteDBClass.GetSettingDouble("TravelControlSpliterRO", 0.4));
                splitContainerRightInner.SplitterDistance(SQLiteDBClass.GetSettingDouble("TravelControlSpliterR", 0.5));

            }

            userControlTravelGrid.LoadLayout();

            // NO NEED to reload the three tabstrips - code below will cause a LoadLayout on the one selected.

            PanelInformation.PanelIDs[] pids = PanelInformation.GetPanelIDs();      // valid PIDs

            // saved as the pop out enum value, for historical reasons. Allow for crap values by using int
            int enum_bottom = SQLiteDBClass.GetSettingInt("TravelControlBottomTab", (int)(PanelInformation.PanelIDs.Scan));
            int enum_bottomright = SQLiteDBClass.GetSettingInt("TravelControlBottomRightTab", (int)(PanelInformation.PanelIDs.Log));
            int enum_middleright = SQLiteDBClass.GetSettingInt("TravelControlMiddleRightTab", (int)(PanelInformation.PanelIDs.StarDistance));
            int enum_topright = SQLiteDBClass.GetSettingInt("TravelControlTopRightTab", (int)(PanelInformation.PanelIDs.SystemInformation));

            int ibottom = Array.IndexOf(pids, (PanelInformation.PanelIDs)enum_bottom);              //given the enum, find it in the list of PIDs
            int ibottomright = Array.IndexOf(pids, (PanelInformation.PanelIDs)enum_bottomright);
            int imiddleright = Array.IndexOf(pids, (PanelInformation.PanelIDs)enum_middleright);
            int itopright = Array.IndexOf(pids, (PanelInformation.PanelIDs)enum_topright);

            tabStripBottom.SelectedIndex = ibottom >= 0 ? ibottom : 0;
            tabStripBottomRight.SelectedIndex = ibottomright >= 0 ? ibottomright: 0;
            tabStripMiddleRight.SelectedIndex = imiddleright >= 0 ? imiddleright : 0;
            tabStripTopRight.SelectedIndex = itopright >= 0 ? itopright : 0;
        }


        public override void Closing()     // called by form when closing
        {
            SQLiteDBClass.PutSettingDouble("TravelControlSpliterLR", splitContainerLeftRight.GetSplitterDistance());
            SQLiteDBClass.PutSettingDouble("TravelControlSpliterL", splitContainerLeft.GetSplitterDistance());
            SQLiteDBClass.PutSettingDouble("TravelControlSpliterRO", splitContainerRightOuter.GetSplitterDistance());
            SQLiteDBClass.PutSettingDouble("TravelControlSpliterR", splitContainerRightInner.GetSplitterDistance());

            PanelInformation.PanelIDs[] pids = PanelInformation.GetPanelIDs();

            SQLiteDBClass.PutSettingInt("TravelControlBottomTab", (int)pids[tabStripBottom.SelectedIndex]);
            SQLiteDBClass.PutSettingInt("TravelControlBottomRightTab", (int)pids[tabStripBottomRight.SelectedIndex]);
            SQLiteDBClass.PutSettingInt("TravelControlMiddleRightTab", (int)pids[tabStripMiddleRight.SelectedIndex]);
            SQLiteDBClass.PutSettingInt("TravelControlTopRightTab", (int)pids[tabStripTopRight.SelectedIndex]);

            userControlTravelGrid.Closing();
            ((UserControlCommonBase)(tabStripBottom.CurrentControl)).Closing();
            ((UserControlCommonBase)(tabStripBottomRight.CurrentControl)).Closing();
            ((UserControlCommonBase)(tabStripMiddleRight.CurrentControl)).Closing();
            ((UserControlCommonBase)(tabStripTopRight.CurrentControl)).Closing();

        }

        #endregion


#region Reaction to UCTG changing

        // history list was repainted, user changed selection, or auto move

        private void ChangedSelection(int rowno, int colno, bool doubleclick, bool note)      // User travel grid call back to say someone clicked somewhere
        {
            if (rowno >= 0)
            {
                HistoryEntry currentsys = userControlTravelGrid.GetCurrentHistoryEntry;

                discoveryform.Map.UpdateHistorySystem(currentsys.System);      // update some dumb friends

                if (userControlTravelGrid.GetCurrentHistoryEntry != null)        // paranoia
                    discoveryform.ActionRun(Actions.ActionEventEDList.onHistorySelection, userControlTravelGrid.GetCurrentHistoryEntry);

                // DEBUG ONLY.. useful for debugging this _discoveryForm.history.SendEDSMStatusInfo(currentsys, true);        // update if required..
            }
        }

        private void OnKeyDownInCell(int keyvalue, int rowno, int colno, bool note)
        {
            if (note)
            {
                UserControlSysInfo si = tabStripTopRight.CurrentControl as UserControlSysInfo;
                if (si == null || !si.IsNotesShowing)
                    si = tabStripMiddleRight.CurrentControl as UserControlSysInfo;
                if (si == null || !si.IsNotesShowing)
                    si = tabStripBottomRight.CurrentControl as UserControlSysInfo;

                if (si != null && si.IsNotesShowing)      // if its note, and we have a system info window
                {
                    si.FocusOnNote(keyvalue);
                }
            }
        }

        #endregion
    }
}
