/*
 * Copyright © 2017 EDDiscovery development team
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

 using EDDiscovery.UserControls;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Forms
{
    public class PopOutControl
    {
        public Forms.UserControlFormList usercontrolsforms;
        EDDiscoveryForm _discoveryForm;

        public PopOutControl( EDDiscoveryForm ed )
        {
            _discoveryForm = ed;
            usercontrolsforms = new UserControlFormList(_discoveryForm);
        }

        public int Count { get { return usercontrolsforms.Count;  } }
        public UserControlForm Get(string name) { return usercontrolsforms.Get(name); }
        public UserControlForm this[int i] { get { return usercontrolsforms[i]; } }
        public enum PopOuts        // id's.. used in tab controls, and in button pop outs button
        {
            Log,                    
            StarDistance,
            Materials,
            Commodities,
            Ledger,                 
            Journal,                
            TravelGrid,
            ScreenShot,
            Statistics,
            Scan,
            Modules,                
            Exploration,
            Synthesis,
            Missions,
            Engineering,
            MarketData,             
            SystemInformation,
            Spanel,
            Trippanel,
            NotePanel,
            RouteTracker,
            StarList,
            Grid,
            EstimatedValues,

            EndList,                // Keep here, used to work out MaxTabButtons
        };

        // MUST match above order
        static public Dictionary<PopOuts, PopOutInfo> popoutinfo = new Dictionary<PopOuts, PopOutInfo>
        {
            { PopOuts.Log, new PopOutInfo("Log", "Log", EDDiscovery.Properties.Resources.Log , "Display the program log" ) },
            { PopOuts.StarDistance, new PopOutInfo("Nearest Stars", "StarDistance", EDDiscovery.Properties.Resources.star,"Display the nearest stars to the currently selected entry") },
            { PopOuts.Materials, new PopOutInfo("Materials", "Materials", EliteDangerous.Properties.Resources.material, "Display the material count at the currently selected entry" ) },
            { PopOuts.Commodities, new PopOutInfo("Commodities", "Commodities", EliteDangerous.Properties.Resources.commodities, "Display the commodity count at the currently selected entry") },
            { PopOuts.Ledger, new PopOutInfo("Ledger", "Ledger", EDDiscovery.Properties.Resources.ledger, "Display a ledger of cash related entries") },
            { PopOuts.Journal, new PopOutInfo("Journal History", "JournalHistory", EDDiscovery.Properties.Resources.journal, "Display the journal grid view") },
            { PopOuts.TravelGrid, new PopOutInfo("Travel History", "TravelHistory", EDDiscovery.Properties.Resources.travelgrid, "Display the history grid view") },
            { PopOuts.ScreenShot, new PopOutInfo("Screen Shot", "ScreenShot", EliteDangerous.Properties.Resources.screenshot, "Display the screen shot view") },
            { PopOuts.Statistics, new PopOutInfo("Statistics", "Stats", EDDiscovery.Properties.Resources.stats, "Display statistics from the history") },
            { PopOuts.Scan, new PopOutInfo("Scan", "Scan", EliteDangerous.Properties.Resources.scan, "Display scan data", transparent: false) },
            { PopOuts.EstimatedValues, new PopOutInfo("Estimated Values", "EstimatedValues", EliteDangerous.Properties.Resources.estval, "Display estimated scan values for all bodies in current system", transparent: false) },
            { PopOuts.Modules, new PopOutInfo("Loadout", "Modules", EliteDangerous.Properties.Resources.module, "Display Loadout for current ships and also stored modules") },
            { PopOuts.Exploration, new PopOutInfo("Exploration", "Exploration", EliteDangerous.Properties.Resources.sellexplorationdata, "Display Exploration Information") },
            { PopOuts.Synthesis, new PopOutInfo("Synthesis", "Synthesis", EliteDangerous.Properties.Resources.synthesis, "Display Synthesis planner") },
            { PopOuts.Missions, new PopOutInfo("Missions", "Missions", EliteDangerous.Properties.Resources.missionaccepted , "Display Missions") },
            { PopOuts.Engineering, new PopOutInfo("Engineering", "Engineering", EliteDangerous.Properties.Resources.engineercraft , "Display Engineering planner") },
            { PopOuts.MarketData, new PopOutInfo("Market Data", "MarketData", EliteDangerous.Properties.Resources.marketdata , "Display Market Data (Requires login to Frontier using Commander Frontier log in details)" ) },
            { PopOuts.SystemInformation, new PopOutInfo("System Information", "SystemInfo", EDDiscovery.Properties.Resources.starsystem , "Display System Information" , transparent:false ) },
            { PopOuts.Spanel, new PopOutInfo("Summary", "Spanel", EDDiscovery.Properties.Resources.spanel, "Display the travel system panel" , transparent: false ) },
            { PopOuts.Trippanel, new PopOutInfo("Trip Computer", "Trippanel", EDDiscovery.Properties.Resources.trippanel, "Display the trip computer" , transparent: false) },
            { PopOuts.NotePanel, new PopOutInfo("Notes", "NotePanel", EDDiscovery.Properties.Resources.notes, "Display current notes on a system" , transparent: false) },
            { PopOuts.RouteTracker, new PopOutInfo("Route Tracker", "RouteTracker", EDDiscovery.Properties.Resources.routetracker, "Display the route tracker", transparent: false) },
            { PopOuts.StarList, new PopOutInfo("Star List", "StarList", EDDiscovery.Properties.Resources.dustbinshorter, "Display the visited star list", transparent: false) },
            { PopOuts.Grid, new PopOutInfo("The Grid", "TheGrid", EDDiscovery.Properties.Resources.grid, "Display the grid which allows other panels to be placed on it" , transparent:false) },
        };

        public class PopOutInfo
        {
            public string WindowTitlePrefix;
            public string WindowRefName;
            public Bitmap TabIcon;
            public string Tooltip;
            public bool SupportsTransparency;
            public bool DefaultTransparent;

            public PopOutInfo(string prefix, string rf, Bitmap icon, string tooltip, bool? transparent = null)
            {
                WindowTitlePrefix = prefix;
                WindowRefName = rf;
                TabIcon = icon;
                Tooltip = tooltip;
                SupportsTransparency = transparent != null;
                DefaultTransparent = transparent ?? false;
            }
        }


        static public PopOuts? GetPopOutTypeByName(string name)
        {
            foreach (KeyValuePair<PopOuts, PopOutInfo> k in popoutinfo)
            {
                if (k.Value.WindowRefName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return k.Key;
            }
            return null;
        }

        static public string[] GetPopOutNames()
        {
            return (from PopOutInfo x in popoutinfo.Values select x.WindowTitlePrefix).ToArray();
        }
        static public string GetPopOutName( PopOuts p)
        {
            return popoutinfo[p].WindowTitlePrefix;
        }
        static public string[] GetPopOutToolTips()
        {
            return (from PopOutInfo x in popoutinfo.Values select x.Tooltip).ToArray();
        }
        static public Bitmap[] GetPopOutImages()
        {
            return (from PopOutInfo x in popoutinfo.Values select x.TabIcon).ToArray();
        }

        public static UserControlCommonBase Create(PopOuts i)
        {
            switch (i)
            {
                case PopOuts.Spanel: return new UserControlSpanel();
                case PopOuts.Trippanel: return new UserControlTrippanel();
                case PopOuts.NotePanel: return new UserControlNotePanel();
                case PopOuts.RouteTracker: return new UserControlRouteTracker();
                case PopOuts.Log: return new UserControlLog();
                case PopOuts.StarDistance: return new UserControlStarDistance();
                case PopOuts.Materials: return new UserControlMaterials();
                case PopOuts.Commodities: return new UserControlCommodities();
                case PopOuts.Ledger: return new UserControlLedger();
                case PopOuts.Journal: return new UserControlJournalGrid();
                case PopOuts.TravelGrid: return new UserControlTravelGrid();
                case PopOuts.ScreenShot: return new UserControlScreenshot();
                case PopOuts.Statistics: return new UserControlStats();
                case PopOuts.Scan: return new UserControlScan();
                case PopOuts.EstimatedValues: return new UserControlEstimatedValues();
                case PopOuts.Modules: return new UserControlModules();
                case PopOuts.Exploration: return new UserControlExploration();
                case PopOuts.Synthesis: return new UserControlSynthesis();
                case PopOuts.Missions: return new UserControlMissions();
                case PopOuts.Engineering: return new UserControlEngineering();
                case PopOuts.MarketData: return new UserControlMarketData();
                case PopOuts.SystemInformation: return new UserControlSysInfo();
                case PopOuts.StarList: return new UserControlStarList();
                case PopOuts.Grid: return new UserControlContainerGrid();
                default: return null;
            }
        }

        public void ShowAllPopOutsInTaskBar()
        {
            usercontrolsforms.ShowAllInTaskBar();
        }

        public void MakeAllPopoutsOpaque()
        {
            usercontrolsforms.MakeAllOpaque();
        }

        internal void SaveCurrentPopouts()
        {
            foreach (int i in Enum.GetValues(typeof(PopOutControl.PopOuts)))
            {
                UserControlCommonBase ctrl = Create((PopOuts)i);
                int numopened = ctrl == null ? 0 : usercontrolsforms.CountOf(ctrl.GetType());
                SQLiteConnectionUser.PutSettingInt("SavedPopouts:" + ((PopOuts)i).ToString(), numopened);
            }
        }

        internal void LoadSavedPopouts()
        {
            foreach (int popout in Enum.GetValues(typeof(PopOuts)))
            {
                int numToOpen = SQLiteConnectionUser.GetSettingInt("SavedPopouts:" + ((PopOuts)popout).ToString(), 0);
                if (numToOpen > 0)
                {
                    UserControlCommonBase ctrl = Create((PopOuts)popout);
                    int numOpened = ctrl == null ? 0 : usercontrolsforms.CountOf(ctrl.GetType());
                    if (numOpened < numToOpen)
                    {
                        for (int i = numOpened + 1; i <= numToOpen; i++)
                        {
                            PopOut((PopOuts)popout);
                        }
                    }
                }
            }
        }


        public void PopOut(PopOutControl.PopOuts selected)
        {
            UserControlForm tcf = usercontrolsforms.NewForm(EDDiscovery.EDDOptions.Instance.NoWindowReposition);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EDDiscovery.EDDiscoveryForm));
            tcf.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            UserControlCommonBase ctrl = Create(selected);

            PopOutInfo poi = popoutinfo.ContainsKey(selected) ? popoutinfo[selected] : null;

            if (ctrl != null && poi != null )
            {
                int numopened = usercontrolsforms.CountOf(ctrl.GetType()) + 1;
                string windowtitle = poi.WindowTitlePrefix + " " + ((numopened > 1) ? numopened.ToString() : "");
                string refname = poi.WindowRefName + numopened.ToString();
                tcf.Init(ctrl, windowtitle, _discoveryForm.theme.WindowsFrame, refname, _discoveryForm.TopMost);
                if (poi.SupportsTransparency)
                {
                    tcf.InitForTransparency(poi.DefaultTransparent, _discoveryForm.theme.LabelColor, _discoveryForm.theme.SPanelColor);
                }

                ctrl.Init(_discoveryForm, _discoveryForm.TravelControl.GetTravelGrid, numopened);

                tcf.Show();                                                     // this ends up, via Form Shown, calls LoadLayout in the UCCB.

                if (tcf.UserControl != null)
                    tcf.UserControl.Font = _discoveryForm.theme.GetFont;        // Important. Apply font autoscaling to the user control
                                                                                // ApplyToForm does not apply the font to the actual UC, only
                                                                                // specific children controls.  The TabControl in the discoveryform ends up autoscaling most stuff
                                                                                // the children directly attached to the discoveryform are not autoscaled

                _discoveryForm.theme.ApplyToForm(tcf);

                ctrl.Display(_discoveryForm.TravelControl.GetTravelHistoryCurrent, _discoveryForm.history);

                _discoveryForm.ActionRun(Actions.ActionEventEDList.onPopUp, null, new Conditions.ConditionVariables(new string[] { "PopOutName", refname , "PopOutTitle", windowtitle, "PopOutIndex", numopened.ToString()} ));
            }
        }
    }
}
