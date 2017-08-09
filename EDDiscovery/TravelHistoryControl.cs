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
using EDDiscovery.DB;
using System.Diagnostics;
using EliteDangerousCore.EDSM;
using System.Threading.Tasks;
using EDDiscovery.Controls;
using System.Threading;
using System.Collections.Concurrent;
using EliteDangerousCore.EDDN;
using Newtonsoft.Json.Linq;
using EDDiscovery.Export;
using EDDiscovery.UserControls;
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery
{
    public partial class TravelHistoryControl : UserControl
    {
        public EDDiscoveryForm _discoveryForm;

        public HistoryEntry GetTravelHistoryCurrent {  get { return userControlTravelGrid.GetCurrentHistoryEntry; } }
        public TravelHistoryFilter GetPrimaryFilter { get { return userControlTravelGrid.GetHistoryFilter; } }  // some classes want to know out filter

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

        public TravelHistoryControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;

            userControlTravelGrid.Init(_discoveryForm, userControlTravelGrid, 0);       // primary first instance - this registers with events in discoveryform to get info
                                                        // then this display, to update its own controls..
            userControlTravelGrid.OnNewEntry += UpdatedWithAddNewEntry;        // call back when you've added a new entry..
            userControlTravelGrid.OnChangedSelection += ChangedSelection;   // and if the user clicks on something
            userControlTravelGrid.OnPopOut += () => { _discoveryForm.PopOuts.PopOut(PopOutControl.PopOuts.TravelGrid); };
            userControlTravelGrid.OnKeyDownInCell += OnKeyDownInCell;
            userControlTravelGrid.ExtraIcons(true, true);

            TabConfigure(tabStripBottom,"Bottom",1000);          // codes are used to save info, 0 = primary (journal/travelgrid), 1..N are popups, these are embedded UCs
            TabConfigure(tabStripBottomRight,"Bottom-Right",1001);
            TabConfigure(tabStripMiddleRight, "Middle-Right", 1002);
            TabConfigure(tabStripTopRight, "Top-Right", 1003);
        }

        #endregion

        #region TAB control

        void TabConfigure(ExtendedControls.TabStrip t, string name, int displayno)
        {
            t.Images = PopOutControl.GetPopOutImages();
            t.ToolTips = PopOutControl.GetPopOutToolTips();
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
            PopOutControl.PopOuts i = (PopOutControl.PopOuts)si;

            Control c = PopOutControl.Create(i);
            c.Name = PopOutControl.GetPopOutName(i);        // tabs uses Name field for display, must set it

            _discoveryForm.ActionRun("onPanelChange", "UserUIEvent", null, new Conditions.ConditionVariables(new string[] { "PanelTabName", PopOutControl.popoutinfo[i].WindowRefName, "PanelTabTitle" , PopOutControl.popoutinfo[i].WindowTitlePrefix , "PanelName" , t.Name }));

            return c;
        }

        void TabPostCreate(ExtendedControls.TabStrip t, Control ctrl , int i)        // called by tab strip after control has been added..
        {                                                           // now we can do the configure of it, with the knowledge the tab has the right size
            int displaynumber = (int)t.Tag;                         // tab strip - use tag to remember display id which helps us save context.

            UserControlCommonBase uc = ctrl as UserControlCommonBase;

            if (uc != null)
            {
                uc.Init(_discoveryForm, userControlTravelGrid, displaynumber);
                uc.LoadLayout();
                uc.Display(userControlTravelGrid.GetCurrentHistoryEntry, _discoveryForm.history);
            }

            //System.Diagnostics.Debug.WriteLine("And theme {0}", i);
            _discoveryForm.theme.ApplyToControls(t);
        }

        void TabPopOut(ExtendedControls.TabStrip t, int i)        // pop out clicked
        {
            _discoveryForm.PopOuts.PopOut((PopOutControl.PopOuts)i);
        }

        #endregion


        #region Grid Layout

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            // ORDER IMPORTANT for right outer/inner splitter, otherwise windows fixes it 

            if (!EDDConfig.Options.NoWindowReposition)
            {
                try
                {
                    splitContainerLeftRight.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterLR", splitContainerLeftRight.SplitterDistance);
                    splitContainerLeft.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterL", splitContainerLeft.SplitterDistance);
                    splitContainerRightOuter.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterRO", splitContainerRightOuter.SplitterDistance);
                    splitContainerRightInner.SplitterDistance = SQLiteDBClass.GetSettingInt("TravelControlSpliterR", splitContainerRightInner.SplitterDistance);
                }
                catch { };          // so splitter can except, if values are strange, but we don't really care, so lets throw away the exception
            }

            userControlTravelGrid.LoadLayout();

            // NO NEED to reload the three tabstrips - code below will cause a LoadLayout on the one selected.

            int max = (int)PopOutControl.PopOuts.EndList;

            tabStripBottom.SelectedIndex = Math.Min(SQLiteDBClass.GetSettingInt("TravelControlBottomTab", (int)(PopOutControl.PopOuts.Scan)), max);
            tabStripBottomRight.SelectedIndex = Math.Min(SQLiteDBClass.GetSettingInt("TravelControlBottomRightTab", (int)(PopOutControl.PopOuts.Log)), max);
            tabStripMiddleRight.SelectedIndex = Math.Min(SQLiteDBClass.GetSettingInt("TravelControlMiddleRightTab", (int)(PopOutControl.PopOuts.StarDistance)), max);
            tabStripTopRight.SelectedIndex = Math.Min(SQLiteDBClass.GetSettingInt("TravelControlTopRightTab", (int)(PopOutControl.PopOuts.SystemInformation)), max);
        }

        public void SaveSettings()     // called by form when closing
        {
            userControlTravelGrid.Closing();
            ((UserControlCommonBase)(tabStripBottom.CurrentControl)).Closing();
            ((UserControlCommonBase)(tabStripBottomRight.CurrentControl)).Closing();
            ((UserControlCommonBase)(tabStripMiddleRight.CurrentControl)).Closing();
            ((UserControlCommonBase)(tabStripTopRight.CurrentControl)).Closing();

            SQLiteDBClass.PutSettingInt("TravelControlSpliterLR", splitContainerLeftRight.SplitterDistance);
            SQLiteDBClass.PutSettingInt("TravelControlSpliterL", splitContainerLeft.SplitterDistance);
            SQLiteDBClass.PutSettingInt("TravelControlSpliterRO", splitContainerRightOuter.SplitterDistance);
            SQLiteDBClass.PutSettingInt("TravelControlSpliterR", splitContainerRightInner.SplitterDistance);

            SQLiteDBClass.PutSettingInt("TravelControlBottomRightTab", tabStripBottomRight.SelectedIndex);
            SQLiteDBClass.PutSettingInt("TravelControlBottomTab", tabStripBottom.SelectedIndex);
            SQLiteDBClass.PutSettingInt("TravelControlMiddleRightTab", tabStripMiddleRight.SelectedIndex);
            SQLiteDBClass.PutSettingInt("TravelControlTopRightTab", tabStripTopRight.SelectedIndex);
        }

        #endregion


        #region Reaction to UCTG changing

        // main travel grid has a new entry due to onNewEntry
        public void UpdatedWithAddNewEntry(HistoryEntry he, HistoryList hl, bool accepted)     
        {
            try
            {   // try is a bit old, probably do not need it.
                if (he.IsFSDJump)
                {
                    int count = _discoveryForm.history.GetVisitsCount(he.System.name);
                    _discoveryForm.LogLine(string.Format("Arrived at system {0} Visit No. {1}", he.System.name, count));

                    System.Diagnostics.Trace.WriteLine("Arrived at system: " + he.System.name + " " + count + ":th visit.");

                    if (EDCommander.Current.SyncToEdsm == true)
                        EDSMSync.SendTravelLog(he);
                }

                hl.SendEDSMStatusInfo(he, true);

                if (he.ISEDDNMessage && he.AgeOfEntry() < TimeSpan.FromDays(1.0))
                {
                    if (EDCommander.Current.SyncToEddn == true)
                    {
                        EDDNSync.SendEDDNEvents(_discoveryForm.LogLine, he);
                    }
                }

                if (he.EntryType == JournalTypeEnum.Scan)
                {
                    if (EDCommander.Current.SyncToEGO)
                    {
                        EDDiscoveryCore.EGO.EGOSync.SendEGOEvents(_discoveryForm.LogLine, he);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception NewPosition: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }

        // history list was repainted, user changed selection, or auto move

        private void ChangedSelection(int rowno, int colno, bool doubleclick, bool note)      // User travel grid call back to say someone clicked somewhere
        {
            if (rowno >= 0)
            {
                HistoryEntry currentsys = userControlTravelGrid.GetCurrentHistoryEntry;

                _discoveryForm.Map.UpdateHistorySystem(currentsys.System);      // update some dumb friends
                _discoveryForm.RouteControl.UpdateHistorySystem(currentsys.System.name);
                _discoveryForm.ExportControl.UpdateHistorySystem(currentsys.System.name);

                if (userControlTravelGrid.GetCurrentHistoryEntry != null)        // paranoia
                    _discoveryForm.ActionRun("onHistorySelection", "UserUIEvent", userControlTravelGrid.GetCurrentHistoryEntry);

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
