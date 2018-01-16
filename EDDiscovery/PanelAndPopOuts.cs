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
    public class PanelInformation
    {
        public enum PanelIDs        // id's.. used in tab controls, and in button pop outs button
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
            ScanGrid,               // 30
            Compass,                // 31
            Map,                    // 32
            Plot,                   // 33
            // ****** ADD More here DO NOT REORDER *****
        };

        // This is the order they are presented to the user..   you can shuffle them to your hearts content
        // ****** DO NOT add UserControlHistory - the display numbers used are special and not okay for a generic panel.  Since you can do the same via a grid, its okay

        static public List<PanelInfo> PanelList = new List<PanelInfo>()
        {
            { new PanelInfo( PanelIDs.Log , typeof(UserControlLog),"Log", "Log", EDDiscovery.Properties.Resources.Log , "Program log" ) },
            { new PanelInfo( PanelIDs.StarDistance, typeof(UserControlStarDistance), "Nearest Stars", "StarDistance", EDDiscovery.Properties.Resources.star,"List of nearest stars") },
            { new PanelInfo( PanelIDs.Materials, typeof(UserControlMaterials) , "Materials", "Materials", EliteDangerous.Properties.Resources.material, "Materials count" ) },
            { new PanelInfo( PanelIDs.Commodities, typeof(UserControlCommodities), "Commodities", "Commodities", EliteDangerous.Properties.Resources.commodities, "Commodity count") },
            { new PanelInfo( PanelIDs.Ledger, typeof(UserControlLedger), "Ledger", "Ledger", EDDiscovery.Properties.Resources.ledger, "Ledger of cash related entries") },
            { new PanelInfo( PanelIDs.Journal, typeof(UserControlJournalGrid), "Journal", "JournalHistory", EDDiscovery.Properties.Resources.journal, "Journal grid view") },
            { new PanelInfo( PanelIDs.TravelGrid, typeof(UserControlTravelGrid), "Travel History", "TravelHistory", EDDiscovery.Properties.Resources.travelgrid, "History grid view") },
            { new PanelInfo( PanelIDs.StarList, typeof(UserControlStarList), "Star List", "StarList", EDDiscovery.Properties.Resources.starlist, "Visited star list", transparent: false) },
            { new PanelInfo( PanelIDs.MarketData, typeof(UserControlMarketData), "Market Data", "MarketData", EliteDangerous.Properties.Resources.marketdata , "Market data (Requires Frontier Commander login)" ) },
            { new PanelInfo( PanelIDs.Missions, typeof(UserControlMissions), "Missions", "Missions", EliteDangerous.Properties.Resources.missionaccepted , "Mission list") },
            { new PanelInfo( PanelIDs.Synthesis, typeof(UserControlSynthesis), "Synthesis", "Synthesis", EliteDangerous.Properties.Resources.synthesis, "Synthesis planner") },
            { new PanelInfo( PanelIDs.Engineering, typeof(UserControlEngineering), "Engineering", "Engineering", EliteDangerous.Properties.Resources.engineercraft , "Engineering planner") },
            { new PanelInfo( PanelIDs.ShoppingList, typeof(UserControlShoppingList), "Shopping List", "ShoppingList", EDDiscovery.Properties.Resources.shoppinglist, "Shopping list planner combining synthesis and engineering") },
            { new PanelInfo( PanelIDs.Scan, typeof(UserControlScan), "Scan", "Scan", EliteDangerous.Properties.Resources.scan, "Scan data on system", transparent: false) },
            { new PanelInfo( PanelIDs.ScanGrid, typeof(UserControlScanGrid), "Scan Grid", "ScanGrid", EDDiscovery.Properties.Resources.scangrid, "Scan data on system in a grid", transparent: false) },
            { new PanelInfo( PanelIDs.EstimatedValues, typeof(UserControlEstimatedValues),"Estimated Values", "EstimatedValues", EliteDangerous.Properties.Resources.estval, "Estimated scan values of bodies in system", transparent: false) },
            { new PanelInfo( PanelIDs.Modules, typeof(UserControlModules), "Loadout", "Modules", EliteDangerous.Properties.Resources.module, "Ship loadout for current ships and stored modules") },
            { new PanelInfo( PanelIDs.Map, typeof(UserControlMap), "Map", "Map", EDDiscovery.Properties.Resources.map, "3D Map of systems in range", transparent: false) },
            { new PanelInfo( PanelIDs.Plot, typeof(UserControlPlot), "2D Plot", "Plot", EDDiscovery.Properties.Resources.plot, "2D Plot of systems in range", transparent: false) },
            { new PanelInfo( PanelIDs.Exploration, typeof(UserControlExploration), "Exploration", "Exploration", EliteDangerous.Properties.Resources.sellexplorationdata, "Exploration Information") },
            { new PanelInfo( PanelIDs.ScreenShot, typeof(UserControlScreenshot), "Screen Shot", "ScreenShot", EliteDangerous.Properties.Resources.screenshot, "Screen shot view") },
            { new PanelInfo( PanelIDs.Statistics, typeof(UserControlStats), "Statistics", "Stats", EDDiscovery.Properties.Resources.stats, "Statistics Information") },
            { new PanelInfo( PanelIDs.SystemInformation, typeof(UserControlSysInfo), "System Information", "SystemInfo", EDDiscovery.Properties.Resources.starsystem , "System Information" , transparent:false ) },
            { new PanelInfo( PanelIDs.EDSM, typeof(UserControlEDSM), "EDSM Star Finder", "EDSMStarFinder", EDDiscovery.Properties.Resources.edsm24, "EDSM Star finder") },
            { new PanelInfo( PanelIDs.Route, typeof(UserControlRoute), "Route Finder", "RouteFinder", EDDiscovery.Properties.Resources.route, "Route Finder from stored star data") },
            { new PanelInfo( PanelIDs.Expedition, typeof(UserControlExpedition), "Expedition", "Expedition", EDDiscovery.Properties.Resources.expedition, "Expedition Planner") },
            { new PanelInfo( PanelIDs.Trilateration, typeof(UserControlTrilateration) ,"Trilateration", "Trilateration", EDDiscovery.Properties.Resources.triangulation, "Trilateration of stars with unknown positions") },
            { new PanelInfo( PanelIDs.Spanel, typeof(UserControlSpanel), "Summary Panel", "Spanel", EDDiscovery.Properties.Resources.spanel, "Summary panel overlay" , transparent: false ) },
            { new PanelInfo( PanelIDs.Trippanel, typeof(UserControlTrippanel), "Trip Computer", "Trippanel", EDDiscovery.Properties.Resources.trippanel, "Trip computer overlay" , transparent: false) },
            { new PanelInfo( PanelIDs.NotePanel, typeof(UserControlNotePanel), "Notes", "NotePanel", EDDiscovery.Properties.Resources.notes, "System notes overlay" , transparent: false) },
            { new PanelInfo( PanelIDs.RouteTracker, typeof(UserControlRouteTracker),"Route Tracker", "RouteTracker", EDDiscovery.Properties.Resources.routetracker, "Route tracker overlay", transparent: false) },
            { new PanelInfo( PanelIDs.Settings, typeof(UserControlSettings), "Settings", "SettingsPanel", EDDiscovery.Properties.Resources.settings, "Settings for ED Discovery ") },
            { new PanelInfo( PanelIDs.Grid, typeof(UserControlContainerGrid), "Grid", "TheGrid", EDDiscovery.Properties.Resources.grid, "Grid (allows other panels to be placed in the it)" , transparent:false) },
            { new PanelInfo( PanelIDs.Compass, typeof(UserControlCompass), "Compass", "Compass", Properties.Resources.compass, "Ground compass navigation panel to work out the bearing between planetary coordinates", transparent:true) },
        };

        public class PanelInfo
        {
            public PanelIDs PopoutID;
            public Type PopoutType;
            public string WindowTitlePrefix;
            public string WindowRefName;
            public Bitmap TabIcon;
            public string Tooltip;
            public bool SupportsTransparency;
            public bool DefaultTransparent;

            public PanelInfo(PanelIDs p, Type t, string prefix, string rf, Bitmap icon, string tooltip, bool? transparent = null)
            {
                PopoutID = p;
                PopoutType = t;
                WindowTitlePrefix = prefix;
                WindowRefName = rf;
                TabIcon = icon;
                Tooltip = tooltip;
                SupportsTransparency = transparent != null;
                DefaultTransparent = transparent ?? false;
            }
        }

        static public string[] GetPanelNames()
        {
            return (from PanelInfo x in PanelList select x.WindowTitlePrefix).ToArray();
        }

        static public string[] GetPanelToolTips()
        {
            return (from PanelInfo x in PanelList select x.Tooltip).ToArray();
        }

        static public Image[] GetPanelImages()
        {
            return (from PanelInfo x in PanelList select x.TabIcon).ToArray();
        }

        static public int GetPanelIndexByWindowsRefName(string name)
        {
            return PanelList.FindIndex(x => x.WindowRefName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        static public int GetPanelIndexByEnum(PanelIDs p)
        {
            return PanelList.FindIndex(x => x.PopoutID == p);
        }

        static public int GetNumberPanels()
        {
            return PanelList.Count;
        }

        static public PanelInfo GetPanelInfoByEnum(PanelIDs p)
        {
            return PanelList[PanelList.FindIndex(x => x.PopoutID == p)];
        }

        static public PanelInfo GetPanelInfoByType(Type t)  // null if not found
        {
            return PanelList.Find(x => x.PopoutType == t);
        }

        static public System.Windows.Forms.ToolStripMenuItem MakeToolStripMenuItem(int i, System.EventHandler h)
        {
            System.Windows.Forms.ToolStripMenuItem mi = new System.Windows.Forms.ToolStripMenuItem();
            mi.Text = PanelList[i].Tooltip;
            mi.Size = new System.Drawing.Size(250, 22);
            mi.Tag = PanelList[i].PopoutID;
            mi.Image = PanelList[i].TabIcon;
            mi.Click += h;
            return mi;
        }

        public static UserControlCommonBase Create(int i)       // index into popoutlist
        {
            return Create(PanelList[i].PopoutID);
        }

        public static UserControlCommonBase Create(PanelIDs p)  // can fail if P is crap
        {
            int index = GetPanelIndexByEnum(p);
            return index>=0 ? (UserControls.UserControlCommonBase)Activator.CreateInstance(PanelList[index].PopoutType, null) : null;
        }
    }

    public class PopOutControl
    {
        public Forms.UserControlFormList usercontrolsforms;
        EDDiscoveryForm discoveryform;

        public PopOutControl( EDDiscoveryForm ed )
        {
            discoveryform = ed;
            usercontrolsforms = new UserControlFormList(discoveryform);
        }

        public int Count { get { return usercontrolsforms.Count;  } }
        public UserControlForm GetByWindowsRefName(string name) { return usercontrolsforms.GetByWindowsRefName(name); }
        public UserControlForm this[int i] { get { return usercontrolsforms[i]; } }

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
            foreach (int i in Enum.GetValues(typeof(PanelInformation.PanelIDs)))        // in terms of PanelInformation.PopOuts Enum
            {
                PanelInformation.PanelIDs p = (PanelInformation.PanelIDs)i;

                UserControlCommonBase ctrl = PanelInformation.Create(p);
                int numopened = ctrl == null ? 0 : usercontrolsforms.CountOf(ctrl.GetType());
                SQLiteConnectionUser.PutSettingInt("SavedPanelInformation.PopOuts:" + ((PanelInformation.PanelIDs)i).ToString(), numopened);
            }
        }

        internal void LoadSavedPopouts()
        {
            foreach (int ip in Enum.GetValues(typeof(PanelInformation.PanelIDs)))     // in terms of PopOut ENUM
            {
                PanelInformation.PanelIDs p = (PanelInformation.PanelIDs)ip;

                int numToOpen = SQLiteConnectionUser.GetSettingInt("SavedPanelInformation.PopOuts:" + p.ToString(), 0);
                if (numToOpen > 0)
                {
                    UserControlCommonBase ctrl = PanelInformation.Create(p);
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

        public void PopOut(PanelInformation.PanelIDs selected)
        {
            int index = PanelInformation.PanelList.FindIndex(x => x.PopoutID == selected);
            PopOut(index);
        }

        public void PopOut(int ix)
        {
            UserControlForm tcf = usercontrolsforms.NewForm(EDDiscovery.EDDOptions.Instance.NoWindowReposition);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EDDiscovery.EDDiscoveryForm));
            tcf.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            UserControlCommonBase ctrl = PanelInformation.Create(ix);

            PanelInformation.PanelInfo poi = PanelInformation.PanelList[ix];

            if (ctrl != null && poi != null )
            {
                int numopened = usercontrolsforms.CountOf(ctrl.GetType()) + 1;
                string windowtitle = poi.WindowTitlePrefix + " " + ((numopened > 1) ? numopened.ToString() : "");
                string refname = poi.WindowRefName + numopened.ToString();
                tcf.Init(ctrl, windowtitle, discoveryform.theme.WindowsFrame, refname, discoveryform.TopMost,
                            poi.DefaultTransparent, discoveryform.theme.LabelColor, discoveryform.theme.SPanelColor);

                ctrl.Init(discoveryform, discoveryform.TravelControl.GetTravelGrid, UserControls.UserControlCommonBase.DisplayNumberPopOuts + numopened - 1);

                tcf.Show();                                                     // this ends up, via Form Shown, calls LoadLayout in the UCCB.

                if (tcf.UserControl != null)
                    tcf.UserControl.Font = discoveryform.theme.GetFont;        // Important. Apply font autoscaling to the user control
                                                                                // ApplyToForm does not apply the font to the actual UC, only
                                                                                // specific children controls.  The TabControl in the discoveryform ends up autoscaling most stuff
                                                                                // the children directly attached to the discoveryform are not autoscaled

                discoveryform.theme.ApplyToForm(tcf);

                ctrl.InitialDisplay();

                discoveryform.ActionRun(Actions.ActionEventEDList.onPopUp, null, new Conditions.ConditionVariables(new string[] { "PopOutName", refname , "PopOutTitle", windowtitle, "PopOutIndex", numopened.ToString()} ));
            }
        }
    }
}
