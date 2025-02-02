/*
 * Copyright © 2017-2021 EDDiscovery development team
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
 * 
 */

using EDDiscovery.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EDDiscovery
{
    static public class PanelInformation
    {
        public enum PanelIDs          // id's.. used in tab controls, and in button pop outs button
        {
            GroupMarker = -1,

            Log = 0,                  // BEWARE
            StarDistance=1,           // BEWARE       Current user selection is saved as an numeric index, so re-numbering these is not allowed
            Materials=2,              // BEWARE
            Commodities=3,            
            Ledger=4,                 
            Journal=5,
            TravelGrid=6,
            ScreenShot=7,
            Statistics=8,             
            Scan=9,
            Modules=10,
            // Exploration removed for 15.0
            Synthesis=12,             
            Missions=13,
            Engineering=14,
            MarketData=15,
            SystemInformation=16,      
            Spanel=17,
            // Trippanel removed for 15.0
            NotePanel = 19,
            RouteTracker=20,           
            Grid=21,
            StarList=22,
            EstimatedValues=23,
            Search=24,                 // was EDSM
            ShoppingList=25,
            Route=26,
            Expedition=27,             
            Trilateration=28,          
            Settings=29,               
            ScanGrid=30,               
            Compass=31,                
            // plot and LocalMap removed for 15.0
            PanelSelector=34,          
            BookmarkManager=35,        
            CombatPanel=36,            
            ShipYardPanel=37,          
            OutfittingPanel=38,        
            SplitterControl=39,        
            MissionOverlay=40,         
            CaptainsLog=41,           
            Surveyor=42,              
            EDSM=43,                  
            MaterialTrader=44,        
            Map2D=45,                 
            MiningOverlay=46,         
            Factions=47,              
            Spansh=48,                
            // EDDB=49,                   removed for 16.1
            Inara=50,                 
            MicroResources=51,        
            SuitsWeapons=52,
            Map3D = 53,
            LocalMap3D = 54,
            Organics = 55,
            Engineers = 56,
            Discoveries = 57,
            Carrier=58,
            Resources=59,
            SpanshStations=60,
            TestOverlay = 61,
            // ****** ADD More here DO NOT RENUMBER *****
        };

        public const int DLLUserPanelsStart = 20000;

        // This is the order they are presented to the user if not using alpha sort..   you can shuffle them to your hearts content
        // description = empty means not user selectable.  Single text string expresses a group.

        static private List<PanelInfo> paneldefinition = new List<PanelInfo>()
        {
            //                                                      Windows title, windows ref name (used in registry), description 
            { new PanelInfo( "History") },
            { new PanelInfo( PanelIDs.Log , typeof(UserControlLog),"Log", "Log", "Log of program information" ) },
            { new PanelInfo( PanelIDs.Journal, typeof(UserControlJournalGrid), "Journal", "JournalHistory", "Journal grid") },
            { new PanelInfo( PanelIDs.TravelGrid, typeof(UserControlTravelGrid), "History", "TravelHistory", "History grid") },
            { new PanelInfo( PanelIDs.StarList, typeof(UserControlStarList), "Visited Stars", "StarList", "Visited Star list") },

            { new PanelInfo( "Current State") },
            { new PanelInfo( PanelIDs.Materials, typeof(UserControlMaterials) , "Materials", "Materials", "Materials count" ) },
            { new PanelInfo( PanelIDs.Commodities, typeof(UserControlCommodities), "Commodities", "Commodities", "Commodity count") },
            { new PanelInfo( PanelIDs.MicroResources, typeof(UserControlMicroResources), "Micro Resources", "MicroResources", "Micro resource count") },
            { new PanelInfo( PanelIDs.Resources, typeof(UserControlAllResources), "Resources", "AllResources", "Resources: All Materials, Commodity, MicroResources") },
            { new PanelInfo( PanelIDs.Ledger, typeof(UserControlLedger), "Ledger", "Ledger", "Ledger of cash related entries") },
            { new PanelInfo( PanelIDs.Missions, typeof(UserControlMissions), "Missions", "Missions", "Mission list") },
            { new PanelInfo( PanelIDs.Factions, typeof(UserControlFactions), "Factions", "Factions", "Faction rewards and trading tally") },
            { new PanelInfo( PanelIDs.Modules, typeof(UserControlModules), "Ships/Loadout", "Modules", "Ships and their loadouts plus stored modules") },
            { new PanelInfo( PanelIDs.Statistics, typeof(UserControlStats), "Statistics", "Stats", "Statistics Information") },
            { new PanelInfo( PanelIDs.SuitsWeapons, typeof(UserControlSuitsWeapons), "Suits & Weapons", "SuitsWeapons", "Suits, Loadouts, Weapons") },
            { new PanelInfo( PanelIDs.Carrier, typeof(UserControlCarrier), "Carrier", "Carrier", "Data about your fleet carrier") },

            { new PanelInfo( "Station Data") },
            { new PanelInfo( PanelIDs.MarketData, typeof(UserControlMarketData), "Market Data", "MarketData", "Market data, giving commodity price information where available" ) },
            { new PanelInfo( PanelIDs.ShipYardPanel, typeof(UserControlShipYards), "Ship Yards", "ShipYards", "Ship yards information from places you have visited") },
            { new PanelInfo( PanelIDs.OutfittingPanel, typeof(UserControlOutfitting), "Outfitting", "Outfitting", "Outfitting items in ship yards from places you have visited") },
            { new PanelInfo( PanelIDs.SpanshStations, typeof(UserControlSpanshStations), "Spansh Stations", "SpanshStations", "Stations in the system from spansh web data") },

            { new PanelInfo( "Engineering/Synthesis") },
            { new PanelInfo( PanelIDs.Synthesis, typeof(UserControlSynthesis), "Synthesis", "Synthesis", "Synthesis planner") },
            { new PanelInfo( PanelIDs.Engineering, typeof(UserControlEngineering), "Engineering", "Engineering", "Engineering planner") },
            { new PanelInfo( PanelIDs.ShoppingList, typeof(UserControlShoppingList), "Shopping List", "ShoppingList", "Shopping list planner combining synthesis and engineering") },
            { new PanelInfo( PanelIDs.MaterialTrader, typeof(UserControlMaterialTrader), "Material Trader", "MaterialTrader", "Material trader") },
            { new PanelInfo( PanelIDs.Engineers, typeof(UserControlEngineers), "Engineers", "Engineers", "Engineers List, status, recipes") },

            { new PanelInfo( "Scans and Stars") },
            { new PanelInfo( PanelIDs.Scan, typeof(UserControlScan), "Scan", "Scan", "Scan data on system") },
            { new PanelInfo( PanelIDs.EDSM, typeof(UserControlEDSM), "EDSM", "EDSM", "EDSM - Automatic web view of system") },
            { new PanelInfo( PanelIDs.Spansh, typeof(UserControlSpansh), "Spansh", "Spansh", "Spansh - Automatic web view of system") },
            //{ new PanelInfo( PanelIDs.EDDB, typeof(UserControlEDDB), "EDDB", "EDDB", "EDDB - Automatic web view of system") },
            { new PanelInfo( PanelIDs.Inara, typeof(UserControlInara), "Inara", "Inara", "Inara - Automatic web view of system") },
            { new PanelInfo( PanelIDs.ScanGrid, typeof(UserControlScanGrid), "Scan Grid", "ScanGrid", "Scan data on system in a grid") },
            { new PanelInfo( PanelIDs.StarDistance, typeof(UserControlStarDistance), "Nearest Stars", "StarDistance","Nearest stars from current position") },
            { new PanelInfo( PanelIDs.EstimatedValues, typeof(UserControlEstimatedValues),"Estimated Values", "EstimatedValues", "Estimated Scan values of bodies in system") },
            { new PanelInfo( PanelIDs.Search, typeof(UserControlSearch), "Search", "SearchFinder", "Search") },
            { new PanelInfo( PanelIDs.Trilateration, typeof(UserControlTrilateration) ,"Trilateration", "Trilateration", "Trilateration of stars with unknown positions") },
            { new PanelInfo( PanelIDs.Map2D, typeof(UserControl2DMap) ,"2D Map", "map2d", "2D Map of galaxy") },
            { new PanelInfo( PanelIDs.Map3D, typeof(UserControl3DMap) ,"3D Map", "map3d", "3D Map of galaxy") },
            { new PanelInfo( PanelIDs.LocalMap3D, typeof(UserControlLocal3DMap) ,"Local 3D Map", "localmap3d", "Local 3D Map of systems near you") },
            { new PanelInfo( PanelIDs.Organics, typeof(UserControlOrganics) ,"Organic Scans", "OrganicScans", "Organic scans of current body and historic scans") },
            { new PanelInfo( PanelIDs.Discoveries, typeof(UserControlDiscoveries) ,"Discoveries", "Discoveries", "Discoveries Observer List") },

            { new PanelInfo( "Bookmarks and Logs") },
            { new PanelInfo( PanelIDs.BookmarkManager, typeof(UserControlBookmarks), "Bookmarks", "Bookmarks", "Bookmarks on systems and planets")},
            { new PanelInfo( PanelIDs.CaptainsLog, typeof(UserControlCaptainsLog), "Captain's Log", "CaptainsLog", "Captain's Log - notes on your travels")},

            { new PanelInfo( "Combat") },
            { new PanelInfo( PanelIDs.CombatPanel, typeof(UserControlCombatPanel), "Combat", "Combat", "Combat statistics")},

            { new PanelInfo( "Routes and Expeditions") },
            { new PanelInfo( PanelIDs.Route, typeof(UserControlRoute), "Route Finder", "RouteFinder", "Route Finder from stored star data") },
            { new PanelInfo( PanelIDs.Expedition, typeof(UserControlExpedition), "Expedition", "Expedition", "Expedition Planner, make up a expedition route") },

            { new PanelInfo( "Overlay Panels") },
            { new PanelInfo( PanelIDs.SystemInformation, typeof(UserControlSysInfo), "System Information", "SystemInfo", "System Information"  ) },
            { new PanelInfo( PanelIDs.Spanel, typeof(UserControlSpanel), "Summary Panel", "Spanel", "Summary panel overlay"  ) },
            { new PanelInfo( PanelIDs.Surveyor, typeof(UserControlSurveyor), "Surveyor", "Surveyor", "Surveyor - Exploration and route overlay"  ) },
            { new PanelInfo( PanelIDs.NotePanel, typeof(UserControlNotePanel), "Notes", "NotePanel", "Notes overlay" ) },
            { new PanelInfo( PanelIDs.RouteTracker, typeof(UserControlRouteTracker),"Route Tracker", "RouteTracker", "Route tracker overlay") },
            { new PanelInfo( PanelIDs.Compass, typeof(UserControlCompass), "Compass", "Compass", "Compass overlay to show bearing to planetary coordinates") },
            { new PanelInfo( PanelIDs.MissionOverlay, typeof(UserControlMissionOverlay), "Mission Overlay", "MissionOV", "Mission List overlay") },
            { new PanelInfo( PanelIDs.MiningOverlay, typeof(UserControlMiningOverlay), "Mining Overlay", "MiningOV", "Mining overlay") },

#if DEBUG
            { new PanelInfo( PanelIDs.TestOverlay, typeof(UserControlTestOverlay), "TestOverlay", "TestOverlay", "Test overlay" ) },
#endif

            { new PanelInfo( "Settings") },
            { new PanelInfo( PanelIDs.Settings, typeof(UserControlSettings), "Settings", "SettingsPanel", "Settings for ED Discovery ") },

            { new PanelInfo( "Screenshots") },
            { new PanelInfo( PanelIDs.ScreenShot, typeof(UserControlScreenshot), "Screen Shot", "ScreenShot", "Screen shot") },

            { new PanelInfo( "Multi Panels") },
            { new PanelInfo( PanelIDs.Grid, typeof(UserControlContainerGrid), "Grid", "TheGrid", "Grid (allows other panels to be placed in it)" ) },
            { new PanelInfo( PanelIDs.SplitterControl, typeof(UserControlContainerSplitter), "Splitter", "TheSplitter", "Splitter (allows other panels to be placed in it)" ) },

            { new PanelInfo( "Non User Panels") },
            { new PanelInfo( PanelIDs.PanelSelector, typeof(UserControlPanelSelector), "+", "Selector", "") },       // no description, not presented to user
        };

        private static HashSet<PanelIDs> WindowsOnlyPanels = new HashSet<PanelIDs>(new[] {
            PanelIDs.EDSM, // disabled due to error finding libgluezilla, and rob can't find a solution to it. freezes program
            PanelIDs.Spansh, 
            PanelIDs.Inara,
            PanelIDs.Map3D,
            PanelIDs.LocalMap3D,
        });

        static private List<PanelInfo> displayablepanels;   // filled by Init - all panels that can be displayed
        static private List<PanelInfo> userselectablepanellist;   // filled by Init - all panels that the user can select directly

        public static IReadOnlyDictionary<PanelIDs, Image> PanelTypeIcons { get; private set; } = new BaseUtils.Icons.IconGroup<PanelIDs>("Panels");

        public static void Init()
        {
            int offset = 0;
            List<int> separs = new List<int>();

            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                paneldefinition = paneldefinition.Where(e => !WindowsOnlyPanels.Contains(e.PopoutID)).ToList();
            }

            foreach (PanelInfo i in paneldefinition)
            {
                i.TabIcon =  PanelTypeIcons[i.PopoutID];

                if (i.PopoutID == PanelIDs.GroupMarker)     // if group, work separs
                {
                    if ( offset>0)
                        separs.Add(offset);

                    // no point, not presented i.WindowTitle = i.WindowTitle.TxID( "PopOutInfo.Group." ,i.WindowTitle.FirstAlphaNumericText());
                }
                else if ( i.Description.Length > 0 )        // if selectable, translate and update index
                {
                    i.WindowTitle = i.WindowTitle.TxID( "PopOutInfo",  i.PopoutID + ".Title");
                    i.Description = i.Description.TxID( "PopOutInfo",  i.PopoutID + ".Description");
                    offset++;
                }
            }

            displayablepanels = (from x in paneldefinition where x.PopoutID != PanelIDs.GroupMarker select x).ToList(); //remove groups..
            userselectablepanellist = (from x in displayablepanels where x.Description.Length>0 select x).ToList(); //remove non selectables..
        }

        // add panel, indicate if its fresh
        public static PanelInfo AddPanel(int id, Type uccbtype, Object tag, string wintitle, string refname, string description, Image image, bool popoutonly)
        {
            PanelIDs pid = (PanelIDs)id;

            if (GetPanelInfoByPanelID(pid) == null)     // don't double add
            {
                PanelInfo p = new PanelInfo(pid, uccbtype, wintitle, refname, description, image, tag, popoutonly);
                displayablepanels.Add(p);
                if ( description.Length>0)          // description = nothing means not user selectable
                    userselectablepanellist.Add(p);
                return p;
            }
            else
                return null;
        }

        // add panel, indicate if its fresh
        public static bool RemovePanel(PanelInformation.PanelIDs pid)
        {
            var p = GetPanelInfoByPanelID(pid);
            if ( p != null )
            {
                displayablepanels.Remove(p);
                userselectablepanellist.Remove(p);
                return true;
            }
            else
                return false;
        }

        [System.Diagnostics.DebuggerDisplay("{PopoutID} {WindowTitle}")]
        public class PanelInfo      // can hold user selectable, group or non user selectable panels
        {
            public PanelIDs PopoutID;
            public Type PopoutType;
            public string WindowTitle;          // Translated
            public string WindowRefName;        // DB key name to use for window state
            public Image TabIcon;
            public string Description;          // must be non zero length to be user selectable.  Translated
            public Object Tag;                  // any other info
            public bool PopOutOnly;

            public PanelInfo(PanelIDs p, Type t, string wintitle, string refname, string description, Image icon = null, Object tag = null, bool popoutonly = false)
            {
                PopoutID = p;
                PopoutType = t;
                WindowTitle = wintitle;
                WindowRefName = refname;
                Description = description;
                TabIcon = icon;
                Tag = tag;
                PopOutOnly = popoutonly;
            }

            public PanelInfo(string s)
            {
                PopoutID = PanelIDs.GroupMarker;
                WindowTitle = s;
                Description = string.Empty;
            }
        }

        // only user selected, and optionally ignore pop out only panels
        static public PanelInfo[] GetUserSelectablePanelInfo(bool sortbyname, bool ignorepopoutonly = false)       
        {
            var list = userselectablepanellist.Where(x => x.PopOutOnly == false || ignorepopoutonly == false);
            if (sortbyname)
                return list.OrderBy(x=>x.Description).ToArray();        // sort by window title.
            else
                return list.ToArray();
        }
         
        // only user selected, and optionally ignore pop out only panels, return separator list of positions where a seperator should be placed
        static public int[] GetUserSelectableSeperatorIndex(bool sortbyname, bool ignorepopoutonly = false)                
        {
            PanelInfo[] pilist = GetUserSelectablePanelInfo(sortbyname, ignorepopoutonly);
            List<int> separs = new List<int>();
            for(int o = 1; o < pilist.Length; o++)
            {
                if (pilist[o - 1].Description[0] != pilist[o].Description[0])
                    separs.Add(o);
            }

            return separs.ToArray();
        }

        static public PanelIDs? GetPanelIDByWindowsRefName(string name) // null if not found
        {
            return displayablepanels.Find(x => x.WindowRefName.Equals(name, StringComparison.InvariantCultureIgnoreCase))?.PopoutID;
        }

        static public PanelInfo GetPanelInfoByPanelID(PanelIDs p)    // null if p is invalid
        {
            int i = displayablepanels.FindIndex(x => x.PopoutID == p);
            return i>=0 ? displayablepanels[i] : null;
        }

        public static UserControlCommonBase Create(PanelIDs p)  // can fail if P is crap
        {
            PanelInfo pi = GetPanelInfoByPanelID(p);
            if (pi != null)
            {
                var uccb = (UserControls.UserControlCommonBase)Activator.CreateInstance(pi.PopoutType, null);
                uccb.Creation(GetPanelInfoByPanelID(p));
                return uccb;
            }

            return null;
        }

        public static System.Windows.Forms.ToolStripMenuItem MakeToolStripMenuItem(PanelIDs p, System.EventHandler h)
        {
            PanelInformation.PanelInfo pi = GetPanelInfoByPanelID(p);
            System.Windows.Forms.ToolStripMenuItem mi = new System.Windows.Forms.ToolStripMenuItem();
            mi.Text = pi.Description;
            mi.Size = new System.Drawing.Size(250, 22);
            mi.Tag = pi.PopoutID;
            mi.Image = pi.TabIcon;
            mi.Click += h;
            return mi;
        }
    }
}
