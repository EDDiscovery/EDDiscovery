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
using EDDiscovery.EDSM;
using System.Threading.Tasks;
using EDDiscovery.Controls;
using System.Threading;
using System.Collections.Concurrent;
using EDDiscovery.EliteDangerous;
using EDDiscovery.EDDN;
using EDDiscovery.EliteDangerous.JournalEvents;
using Newtonsoft.Json.Linq;
using EDDiscovery.Export;
using EDDiscovery.UserControls;
using EDDiscovery.Forms;

namespace EDDiscovery
{
    public partial class TravelHistoryControl : UserControl
    {
        public EDDiscoveryForm _discoveryForm;

        string lastclosestname;
        SortedList<double, ISystem> lastclosestsystems;

        Bitmap[] tabbitmaps = new Bitmap[] { EDDiscovery.Properties.Resources.Log,      // Match pop out enum PopOuts, from start, list only ones which should be in tabs
                                        EDDiscovery.Properties.Resources.star,
                                        EDDiscovery.Properties.Resources.material ,
                                        EDDiscovery.Properties.Resources.commodities,
                                        EDDiscovery.Properties.Resources.ledger ,
                                        EDDiscovery.Properties.Resources.journal ,
                                        EDDiscovery.Properties.Resources.travelgrid ,
                                        EDDiscovery.Properties.Resources.screenshot,
                                        EDDiscovery.Properties.Resources.stats,
                                        EDDiscovery.Properties.Resources.scan,
                                        EDDiscovery.Properties.Resources.module,
                                        EDDiscovery.Properties.Resources.sellexplorationdata,
                                        EDDiscovery.Properties.Resources.synthesis,
                                        EDDiscovery.Properties.Resources.missionaccepted,
                                        EDDiscovery.Properties.Resources.engineercraft,
                                        EDDiscovery.Properties.Resources.marketdata,
                                        EDDiscovery.Properties.Resources.ammunition, //TBD
                                        };

        string[] tabtooltips = new string[] { "Display the program log",     // MAtch Pop out enum
                                               "Display the nearest stars to the currently selected entry",
                                               "Display the material count at the currently selected entry",
                                               "Display the commodity count at the currently selected entry",
                                               "Display a ledger of cash related entries",
                                               "Display the journal grid view",
                                               "Display the history grid view",
                                               "Display the screen shot view",
                                               "Display statistics from the history",
                                               "Display scan data",
                                               "Display Loadout for current ships and also stored modules",
                                               "Display Exploration view",
                                               "Display Synthesis planner",
                                               "Display Missions",
                                               "Display Engineering planner",
                                               "Display Market Data (Requires login to Frontier using Commander Frontier log in details)",
                                               "Display System Information panel",
                                            };

        HistoryEntry notedisplayedhe = null;            // remember the particulars of the note displayed, so we can save it later

        public HistoryEntry GetTravelHistoryCurrent {  get { return userControlTravelGrid.GetCurrentHistoryEntry; } }
        public TravelHistoryFilter GetPrimaryFilter { get { return userControlTravelGrid.GetHistoryFilter; } }  // some classes want to know out filter

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

        // Subscribe to these to get various events - layout controls via their Init function do this.

        public delegate void TravelSelectionChanged(HistoryEntry he, HistoryList hl);       // called when current travel sel changed, after the user control informs us
        public event TravelSelectionChanged OnTravelSelectionChanged;

        public delegate void NearestStarList(string name, SortedList<double, ISystem> csl); // called when star computation has a new list
        public event NearestStarList OnNearestStarListChanged;


        #region Initialisation

        public TravelHistoryControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;

            userControlTravelGrid.Init(_discoveryForm, 0);       // primary first instance - this registers with events in discoveryform to get info
                                                        // then this display, to update its own controls..
            userControlTravelGrid.OnRedisplay += uctgRedisplay;        // after the TG has redisplayed..
            userControlTravelGrid.OnAddedNewEntry += UpdatedWithAddNewEntry;        // call back when you've added a new entry..
            userControlTravelGrid.OnChangedSelection += ChangedSelection;   // and if the user clicks on something
            userControlTravelGrid.OnResort += Resort;   // and if he or she resorts
            userControlTravelGrid.OnPopOut += TGPopOut;

            TabConfigure(tabStripBottom,"Bottom",1000);          // codes are used to save info, 0 = primary (journal/travelgrid), 1..N are popups, these are embedded UCs
            TabConfigure(tabStripBottomRight,"Bottom-Right",1001);
            TabConfigure(tabStripMiddleRight, "Middle-Right", 1002);
            TabConfigure(tabStripTopRight, "Top-Right", 1003);

        }

        #endregion

        #region TAB control

        void TabConfigure(ExtendedControls.TabStrip t, string name, int displayno)
        {
            t.Images = tabbitmaps;
            t.ToolTips = tabtooltips;
            t.Tag = displayno;             // these are IDs for purposes of identifying different instances of a control.. 0 = main ones (main travel grid, main tab journal). 1..N are popups
            t.OnRemoving += TabRemoved;
            t.OnCreateTab += TabCreate;
            t.OnPostCreateTab += TabPostCreate;
            t.OnPopOut += TabPopOut;
            t.Name = name;
        }

        void TabRemoved(ExtendedControls.TabStrip t, Control c )     // called by tab strip when a control is removed
        {
            UserControlCommonBase uccb = c as UserControlCommonBase;
            uccb.Closing();
        }

        Control TabCreate(ExtendedControls.TabStrip t, int si)        // called by tab strip when selected index changes.. create a new one.. only create.
        {
            PopOutControl.PopOuts i = (PopOutControl.PopOuts)(si + PopOutControl.PopOuts.StartTabButtons);

            Control c = PopOutControl.Create(i);

            _discoveryForm.ActionRun("onPanelChange", "UserUIEvent", null, new Conditions.ConditionVariables(new string[] { "PanelTabName", PopOutControl.popoutinfo[i].WindowRefName, "PanelTabTitle" , PopOutControl.popoutinfo[i].WindowTitlePrefix , "PanelName" , t.Name }));

            return c;
        }

        void TabPostCreate(ExtendedControls.TabStrip t, Control ctrl , int i)        // called by tab strip after control has been added..
        {                                                           // now we can do the configure of it, with the knowledge the tab has the right size
            int displaynumber = (int)t.Tag;                         // tab strip - use tag to remember display id which helps us save context.

            UserControlCommonBase uc = ctrl as UserControlCommonBase;

            if (uc != null)
            {
                UserControlPostCreate(displaynumber, uc);
                uc.Display(userControlTravelGrid.GetCurrentHistoryEntry, _discoveryForm.history);
            }

            //System.Diagnostics.Debug.WriteLine("And theme {0}", i);
            _discoveryForm.theme.ApplyToControls(t);
        }

        public void UserControlPostCreate(int displaynumber, UserControlCommonBase ctrl)
        {
            ctrl.Init(_discoveryForm, displaynumber);
            ctrl.LoadLayout();

            if (ctrl is UserControlLog)
            {
                UserControlLog sc = ctrl as UserControlLog;
                sc.AppendText(_discoveryForm.LogText, _discoveryForm.theme.TextBlockColor);
            }
            else if (ctrl is UserControlStarDistance)
            {
                UserControlStarDistance sc = ctrl as UserControlStarDistance;
                if (lastclosestsystems != null)           // if we have some, fill in this grid
                    sc.FillGrid(lastclosestname, lastclosestsystems);
            }
            else if (ctrl is UserControlMaterials)
            {
                UserControlMaterials ucm = ctrl as UserControlMaterials;
            }
            else if (ctrl is UserControlCommodities)
            {
                UserControlCommodities ucm = ctrl as UserControlCommodities;
            }
            else if (ctrl is UserControlLedger)
            {
                UserControlLedger ucm = ctrl as UserControlLedger;
                ucm.OnGotoJID += GotoJID;
            }
            else if (ctrl is UserControlJournalGrid)
            {
                UserControlJournalGrid ucm = ctrl as UserControlJournalGrid;
                ucm.NoHistoryIcon();
                ucm.NoPopOutIcon();
            }
            else if (ctrl is UserControlTravelGrid)
            {
                UserControlTravelGrid ucm = ctrl as UserControlTravelGrid;
                ucm.NoHistoryIcon();
                ucm.NoPopOutIcon();
            }
        }

        void TabPopOut(ExtendedControls.TabStrip t, int i)        // pop out clicked
        {
            _discoveryForm.PopOuts.PopOut((PopOutControl.PopOuts)(i+ PopOutControl.PopOuts.StartTabButtons));
        }

        void GotoJID(long v)
        {
            userControlTravelGrid.GotoPosByJID(v);
        }

        #endregion

        #region Display history

        public void uctgRedisplay(HistoryList hl)                      // called from main travelgrid when refreshed display due to a discoveryform.OhHistoryChange
        {
            GenerateTravelChangeEvent(userControlTravelGrid.GetCurrentRow);
            UpdateDependentsWithSelection();
        }

        public void UpdatedWithAddNewEntry(HistoryEntry he, HistoryList hl, bool accepted)     // main travel grid calls after getting a new entry
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

                if (he.ISEDDNMessage)
                {
                    if (EDCommander.Current.SyncToEddn == true)
                    {
                        EDDNSync.SendEDDNEvents(_discoveryForm, he);
                    }
                }

                if ( accepted )                                                 // if accepted it on main grid..
                {
                    if (EDDiscoveryForm.EDDConfig.FocusOnNewSystem)   // Move focus to new row
                    {
                        userControlTravelGrid.SelectTopRow();
                        GenerateTravelChangeEvent(userControlTravelGrid.GetCurrentRow);
                        UpdateDependentsWithSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception NewPosition: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }

        public void GenerateTravelChangeEvent(DataGridViewRow rw)
        {
            HistoryEntry he = null;

            if (rw != null)
            {
                he = userControlTravelGrid.GetHistoryEntry(rw.Index);     // reload, it may have changed
                Debug.Assert(he != null);
                _discoveryForm.CalculateClosestSystems(he.System, (s, d) => NewStarListComputed(s.name, d));        // hook here, force closes system update
            }

            if (OnTravelSelectionChanged != null)
                OnTravelSelectionChanged(he, _discoveryForm.history);
        }

        private void NewStarListComputed(string name, SortedList<double, ISystem> csl)      // thread..
        {
            BeginInvoke((MethodInvoker)delegate
            {
                lastclosestname = name;
                lastclosestsystems = csl;

                if (OnNearestStarListChanged != null)
                    OnNearestStarListChanged(name, csl);
            });
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

            int max = (int)PopOutControl.PopOuts.MaxTabButtons;

            tabStripBottom.SelectedIndex = Math.Min( SQLiteDBClass.GetSettingInt("TravelControlBottomTab", (int)(PopOutControl.PopOuts.Scan - PopOutControl.PopOuts.StartTabButtons)), max);
            tabStripBottomRight.SelectedIndex = Math.Min(SQLiteDBClass.GetSettingInt("TravelControlBottomRightTab", (int)(PopOutControl.PopOuts.Log - PopOutControl.PopOuts.StartTabButtons) ), max );
            tabStripMiddleRight.SelectedIndex = Math.Min(SQLiteDBClass.GetSettingInt("TravelControlMiddleRightTab", (int)(PopOutControl.PopOuts.StarDistance - PopOutControl.PopOuts.StartTabButtons)), max);
            tabStripTopRight.SelectedIndex = Math.Min(SQLiteDBClass.GetSettingInt("TravelControlTopRightTab", (int)(PopOutControl.PopOuts.SystemInformation - PopOutControl.PopOuts.StartTabButtons)), max);
        }

        public void SaveSettings()     // called by form when closing
        {
            userControlTravelGrid.Closing();
            ((UserControlCommonBase)(tabStripBottom.CurrentControl)).Closing();
            ((UserControlCommonBase)(tabStripBottomRight.CurrentControl)).Closing();
            ((UserControlCommonBase)(tabStripMiddleRight.CurrentControl)).Closing();

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

        #region Clicks

        private void Resort()       // user travel grid to say it resorted
        {
            UpdateDependentsWithSelection();        //TBD does this do enough.. what about dependents should we call changed selection..
        }

        private void ChangedSelection(int rowno, int colno , bool doubleclick , bool note)      // User travel grid call back to say someone clicked somewhere
        {
            if (rowno >= 0)
            {
                GenerateTravelChangeEvent(userControlTravelGrid.GetRow(rowno));
                UpdateDependentsWithSelection();

                if (userControlTravelGrid.GetCurrentHistoryEntry!= null)        // paranoia
                    _discoveryForm.ActionRun("onHistorySelection", "UserUIEvent", userControlTravelGrid.GetCurrentHistoryEntry);
            }
        }

        private void UpdateDependentsWithSelection()        // they really should do it themselves.. but
        {
            if (userControlTravelGrid.currentGridRow >= 0)
            {
                HistoryEntry currentsys = userControlTravelGrid.GetCurrentHistoryEntry;
                _discoveryForm.Map.UpdateHistorySystem(currentsys.System);
                _discoveryForm.RouteControl.UpdateHistorySystem(currentsys.System.name);
                _discoveryForm.ExportControl.UpdateHistorySystem(currentsys.System.name);
            }
        }

        void TGPopOut()
        {
            _discoveryForm.PopOuts.PopOut(PopOutControl.PopOuts.TravelGrid);
        }

        #endregion
    }
}
