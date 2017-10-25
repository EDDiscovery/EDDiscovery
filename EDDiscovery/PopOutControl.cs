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
            Log,                    // BEWARE
            StarDistance,           // BEWARE       Current user selection is saved as an index, so re-ordering these is not allowed
            Materials,              // BEWARE
            Commodities,            // 
            Ledger,                 // 4
            Journal,                
            TravelGrid,
            ScreenShot,
            Statistics,             // 8
            Scan,
            Modules,                
            Exploration,
            Synthesis,              // 12
            Missions,
            Engineering,
            MarketData,             
            SystemInformation,      // 16
            Spanel,
            Trippanel,
            NotePanel,
            RouteTracker,           // 20
            Grid,
            StarList,
            EstimatedValues,
            EDSM,                   // 24
            ShoppingList,
            Route,
            Expedition,             // 27
            Trilateration,          // 28
            Settings,               // 29
            // ****** ADD More here DO NOT REORDER *****
        };

        // This is the order they are presented to the user..   you can shuffle them to your hearts content
        // via PopOutNames() for the drop down list
        // and via PopOutImages() for the tab strip

        static public List<PopOutInfo> PopOutList = new List<PopOutInfo>()
        {
            { new PopOutInfo( PopOuts.Log , typeof(UserControlLog),"Log", "Log", EDDiscovery.Properties.Resources.Log , "Display the program log" ) },
            { new PopOutInfo( PopOuts.StarDistance, typeof(UserControlStarDistance), "Nearest Stars", "StarDistance", EDDiscovery.Properties.Resources.star,"Display the nearest stars to the currently selected entry") },
            { new PopOutInfo( PopOuts.Materials, typeof(UserControlMaterials) , "Materials", "Materials", EliteDangerous.Properties.Resources.material, "Display the material count at the currently selected entry" ) },
            { new PopOutInfo( PopOuts.Commodities, typeof(UserControlCommodities), "Commodities", "Commodities", EliteDangerous.Properties.Resources.commodities, "Display the commodity count at the currently selected entry") },
            { new PopOutInfo( PopOuts.Ledger, typeof(UserControlLedger), "Ledger", "Ledger", EDDiscovery.Properties.Resources.ledger, "Display a ledger of cash related entries") },
            { new PopOutInfo( PopOuts.Journal, typeof(UserControlJournalGrid), "Journal", "JournalHistory", EDDiscovery.Properties.Resources.journal, "Display the journal grid view") },
            { new PopOutInfo( PopOuts.TravelGrid, typeof(UserControlTravelGrid), "Travel History", "TravelHistory", EDDiscovery.Properties.Resources.travelgrid, "Display the history grid view") },
            { new PopOutInfo( PopOuts.StarList, typeof(UserControlStarList), "Star List", "StarList", EDDiscovery.Properties.Resources.starlist, "Display the visited star list", transparent: false) },
            { new PopOutInfo( PopOuts.MarketData, typeof(UserControlMarketData), "Market Data", "MarketData", EliteDangerous.Properties.Resources.marketdata , "Display Market Data (Requires Frontier Commander login)" ) },
            { new PopOutInfo( PopOuts.Missions, typeof(UserControlMissions), "Missions", "Missions", EliteDangerous.Properties.Resources.missionaccepted , "Display Missions") },
            { new PopOutInfo( PopOuts.Synthesis, typeof(UserControlSynthesis), "Synthesis", "Synthesis", EliteDangerous.Properties.Resources.synthesis, "Display Synthesis planner") },
            { new PopOutInfo( PopOuts.Engineering, typeof(UserControlEngineering), "Engineering", "Engineering", EliteDangerous.Properties.Resources.engineercraft , "Display Engineering planner") },
            { new PopOutInfo( PopOuts.ShoppingList, typeof(UserControlShoppingList), "Shopping List", "ShoppingList", EDDiscovery.Properties.Resources.shoppinglist, "Shopping list of materials combining synthesis and engineering") },
            { new PopOutInfo( PopOuts.Scan, typeof(UserControlScan), "Scan", "Scan", EliteDangerous.Properties.Resources.scan, "Display scan data", transparent: false) },
            { new PopOutInfo( PopOuts.EstimatedValues, typeof(UserControlEstimatedValues),"Estimated Values", "EstimatedValues", EliteDangerous.Properties.Resources.estval, "Display estimated scan values bodies in system", transparent: false) },
            { new PopOutInfo( PopOuts.Modules, typeof(UserControlModules), "Loadout", "Modules", EliteDangerous.Properties.Resources.module, "Display Loadout for current ships and also stored modules") },
            { new PopOutInfo( PopOuts.Exploration, typeof(UserControlExploration), "Exploration", "Exploration", EliteDangerous.Properties.Resources.sellexplorationdata, "Display Exploration Information") },
            { new PopOutInfo( PopOuts.ScreenShot, typeof(UserControlScreenshot), "Screen Shot", "ScreenShot", EliteDangerous.Properties.Resources.screenshot, "Display the screen shot view") },
            { new PopOutInfo( PopOuts.Statistics, typeof(UserControlStats), "Statistics", "Stats", EDDiscovery.Properties.Resources.stats, "Display statistics from the history") },
            { new PopOutInfo( PopOuts.SystemInformation, typeof(UserControlSysInfo), "System Information", "SystemInfo", EDDiscovery.Properties.Resources.starsystem , "Display System Information" , transparent:false ) },
            { new PopOutInfo( PopOuts.EDSM, typeof(UserControlEDSM), "EDSM Star Finder", "EDSMStarFinder", EDDiscovery.Properties.Resources.edsm24, "Display the EDSM Star finder") },
            { new PopOutInfo( PopOuts.Route, typeof(UserControlRoute), "Route Finder", "RouteFinder", EDDiscovery.Properties.Resources.route, "Find Routes from star data") },
            { new PopOutInfo( PopOuts.Expedition, typeof(UserControlExpedition), "Expedition", "Expedition", EDDiscovery.Properties.Resources.expedition, "Plan an Expedition") },
            { new PopOutInfo( PopOuts.Trilateration, typeof(UserControlTrilateration) ,"Trilateration", "Trilateration", EDDiscovery.Properties.Resources.triangulation, "Trilateration of stars with unknown positions") },
            { new PopOutInfo( PopOuts.Spanel, typeof(UserControlSpanel), "Summary Panel", "Spanel", EDDiscovery.Properties.Resources.spanel, "Display the summary panel overlay" , transparent: false ) },
            { new PopOutInfo( PopOuts.Trippanel, typeof(UserControlTrippanel), "Trip Computer", "Trippanel", EDDiscovery.Properties.Resources.trippanel, "Display the trip computer overlay" , transparent: false) },
            { new PopOutInfo( PopOuts.NotePanel, typeof(UserControlNotePanel), "Notes", "NotePanel", EDDiscovery.Properties.Resources.notes, "Display current notes on a system overlay" , transparent: false) },
            { new PopOutInfo( PopOuts.RouteTracker, typeof(UserControlRouteTracker),"Route Tracker", "RouteTracker", EDDiscovery.Properties.Resources.routetracker, "Display the route tracker overlay", transparent: false) },
            { new PopOutInfo( PopOuts.Settings, typeof(UserControlSettings), "Settings", "SettingsPanel", EDDiscovery.Properties.Resources.coriolis, "Display the settings panel") },
            { new PopOutInfo( PopOuts.Grid, typeof(UserControlContainerGrid), "Grid", "TheGrid", EDDiscovery.Properties.Resources.grid, "Display the grid which allows other panels to be placed on it" , transparent:false) },
        };

        public class PopOutInfo
        {
            public PopOuts popoutid;
            public Type popouttype;
            public string WindowTitlePrefix;
            public string WindowRefName;
            public Bitmap TabIcon;
            public string Tooltip;
            public bool SupportsTransparency;
            public bool DefaultTransparent;

            public PopOutInfo(PopOuts p, Type t , string prefix, string rf, Bitmap icon, string tooltip, bool? transparent = null)
            {
                popoutid = p;
                popouttype = t;
                WindowTitlePrefix = prefix;
                WindowRefName = rf;
                TabIcon = icon;
                Tooltip = tooltip;
                SupportsTransparency = transparent != null;
                DefaultTransparent = transparent ?? false;
            }
        }

        static public string[] GetPopOutNames()
        {
            return (from PopOutInfo x in PopOutList select x.WindowTitlePrefix).ToArray();
        }

        static public string[] GetPopOutToolTips()
        {
            return (from PopOutInfo x in PopOutList select x.Tooltip).ToArray();
        }

        static public Image[] GetPopOutImages()
        {
            return (from PopOutInfo x in PopOutList select x.TabIcon).ToArray();
        }

        static public int GetPopOutIndexByName(string name)
        {
            return PopOutList.FindIndex(x => x.WindowRefName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        static public int GetPopOutIndexByEnum( PopOuts p )
        {
            return PopOutList.FindIndex(x => x.popoutid == p);
        }

        static public int GetNumberPanels()
        {
            return PopOutList.Count;
        }

        static public PopOutInfo GetPopOutInfoByEnum( PopOuts p)
        {
            return PopOutList[PopOutList.FindIndex(x => x.popoutid == p)];
        }

        static public System.Windows.Forms.ToolStripMenuItem MakeToolStripMenuItem(int i , System.EventHandler h )
        {
            System.Windows.Forms.ToolStripMenuItem mi = new System.Windows.Forms.ToolStripMenuItem();
            mi.Text = PopOutList[i].Tooltip;
            mi.Size = new System.Drawing.Size(250, 22);
            mi.Tag = PopOutControl.PopOutList[i].popoutid;
            mi.Image = PopOutControl.PopOutList[i].TabIcon;
            mi.Click += h;
            return mi;
        }

        private static void Mi_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public static UserControlCommonBase Create(int i)       // index into popoutlist
        {
            return Create(PopOutList[i].popoutid);
        }

        public static UserControlCommonBase Create(PopOuts p)
        {
            int index = GetPopOutIndexByEnum(p);
            return (UserControls.UserControlCommonBase)Activator.CreateInstance(PopOutList[index].popouttype, null);
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
            foreach (int i in Enum.GetValues(typeof(PopOuts)))        // in terms of PopOuts Enum
            {
                PopOuts p = (PopOuts)i;

                UserControlCommonBase ctrl = Create(p);
                int numopened = ctrl == null ? 0 : usercontrolsforms.CountOf(ctrl.GetType());
                SQLiteConnectionUser.PutSettingInt("SavedPopouts:" + ((PopOuts)i).ToString(), numopened);
            }
        }

        internal void LoadSavedPopouts()
        {
            foreach (int ip in Enum.GetValues(typeof(PopOuts)))     // in terms of PopOut ENUM
            {
                PopOuts p = (PopOuts)ip;

                int numToOpen = SQLiteConnectionUser.GetSettingInt("SavedPopouts:" + p.ToString(), 0);
                if (numToOpen > 0)
                {
                    UserControlCommonBase ctrl = Create(p);
                    int numOpened = ctrl == null ? 0 : usercontrolsforms.CountOf(ctrl.GetType());
                    if (numOpened < numToOpen)
                    {
                        for (int i = numOpened + 1; i <= numToOpen; i++)
                        {
                            PopOut(p);
                        }
                    }
                }
            }
        }

        public void PopOut(PopOutControl.PopOuts selected)
        {
            int index = PopOutList.FindIndex(x => x.popoutid == selected);
            PopOut(index);
        }

        public void PopOut(int ix)
        {
            UserControlForm tcf = usercontrolsforms.NewForm(EDDiscovery.EDDOptions.Instance.NoWindowReposition);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EDDiscovery.EDDiscoveryForm));
            tcf.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            UserControlCommonBase ctrl = Create(ix);

            PopOutInfo poi = PopOutList[ix];

            if (ctrl != null && poi != null )
            {
                int numopened = usercontrolsforms.CountOf(ctrl.GetType()) + 1;
                string windowtitle = poi.WindowTitlePrefix + " " + ((numopened > 1) ? numopened.ToString() : "");
                string refname = poi.WindowRefName + numopened.ToString();
                tcf.Init(ctrl, windowtitle, _discoveryForm.theme.WindowsFrame, refname, _discoveryForm.TopMost,
                            poi.DefaultTransparent, _discoveryForm.theme.LabelColor, _discoveryForm.theme.SPanelColor);

                ctrl.Init(_discoveryForm, _discoveryForm.TravelControl.GetTravelGrid, numopened);

                tcf.Show();                                                     // this ends up, via Form Shown, calls LoadLayout in the UCCB.

                if (tcf.UserControl != null)
                    tcf.UserControl.Font = _discoveryForm.theme.GetFont;        // Important. Apply font autoscaling to the user control
                                                                                // ApplyToForm does not apply the font to the actual UC, only
                                                                                // specific children controls.  The TabControl in the discoveryform ends up autoscaling most stuff
                                                                                // the children directly attached to the discoveryform are not autoscaled

                _discoveryForm.theme.ApplyToForm(tcf);

                ctrl.InitialDisplay();

                _discoveryForm.ActionRun(Actions.ActionEventEDList.onPopUp, null, new Conditions.ConditionVariables(new string[] { "PopOutName", refname , "PopOutTitle", windowtitle, "PopOutIndex", numopened.ToString()} ));
            }
        }
    }
}
