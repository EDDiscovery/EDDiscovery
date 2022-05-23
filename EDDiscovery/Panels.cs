﻿/*
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
 * EDDiscovery is not affiliated with Frontier Developments plc.
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
        public enum PanelIDs        // id's.. used in tab controls, and in button pop outs button
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
            EDDB=49,                  
            Inara=50,                 
            MicroResources=51,        
            SuitsWeapons=52,
            Map3D = 53,
            LocalMap3D = 54,
            Organics = 55,
            Engineers = 56,
            // ****** ADD More here DO NOT RENUMBER *****
        };

        // This is the order they are presented to the user if not using alpha sort..   you can shuffle them to your hearts content
        // description = empty means not user selectable.  Single text string expresses a group.

        static private List<PanelInfo> paneldefinition = new List<PanelInfo>()
        {
            { new PanelInfo( "History") },
            { new PanelInfo( PanelIDs.Log , typeof(UserControlLog),"Log", "Log", "Log of program information" ) },
            { new PanelInfo( PanelIDs.Journal, typeof(UserControlJournalGrid), "Journal", "JournalHistory", "Journal grid view") },
            { new PanelInfo( PanelIDs.TravelGrid, typeof(UserControlTravelGrid), "History", "TravelHistory", "History grid view") },
            { new PanelInfo( PanelIDs.StarList, typeof(UserControlStarList), "Visited Stars", "StarList", "Visited Star list") },

            { new PanelInfo( "Current State") },
            { new PanelInfo( PanelIDs.Materials, typeof(UserControlMaterials) , "Materials", "Materials", "Materials count" ) },
            { new PanelInfo( PanelIDs.Commodities, typeof(UserControlCommodities), "Commodities", "Commodities", "Commodity count") },
            { new PanelInfo( PanelIDs.MicroResources, typeof(UserControlMicroResources), "Micro Resources", "MicroResources", "Micro resource count") },
            { new PanelInfo( PanelIDs.Ledger, typeof(UserControlLedger), "Ledger", "Ledger", "Ledger of cash related entries") },
            { new PanelInfo( PanelIDs.Missions, typeof(UserControlMissions), "Missions", "Missions", "Mission list") },
            { new PanelInfo( PanelIDs.Factions, typeof(UserControlFactions), "Factions", "Factions", "Faction rewards and trading tally") },
            { new PanelInfo( PanelIDs.Modules, typeof(UserControlModules), "Ships/Loadout", "Modules", "Ships and their loadouts plus stored modules") },
            { new PanelInfo( PanelIDs.Statistics, typeof(UserControlStats), "Statistics", "Stats", "Statistics Information") },
            { new PanelInfo( PanelIDs.SuitsWeapons, typeof(UserControlSuitsWeapons), "Suits & Weapons", "SuitsWeapons", "Suits, Loadouts, Weapons") },

            { new PanelInfo( "Shipyard Data") },
            { new PanelInfo( PanelIDs.MarketData, typeof(UserControlMarketData), "Market Data", "MarketData", "Market data view, giving commodity price information where available" ) },
            { new PanelInfo( PanelIDs.ShipYardPanel, typeof(UserControlShipYards), "Ship Yards", "ShipYards", "Ship yards information from places you have visited") },
            { new PanelInfo( PanelIDs.OutfittingPanel, typeof(UserControlOutfitting), "Outfitting", "Outfitting", "Outfitting items in ship yards from places you have visited") },

            { new PanelInfo( "Engineering/Synthesis") },
            { new PanelInfo( PanelIDs.Synthesis, typeof(UserControlSynthesis), "Synthesis", "Synthesis", "Synthesis planner") },
            { new PanelInfo( PanelIDs.Engineering, typeof(UserControlEngineering), "Engineering", "Engineering", "Engineering planner") },
            { new PanelInfo( PanelIDs.ShoppingList, typeof(UserControlShoppingList), "Shopping List", "ShoppingList", "Shopping list planner combining synthesis and engineering") },
            { new PanelInfo( PanelIDs.MaterialTrader, typeof(UserControlMaterialTrader), "Material Trader", "MaterialTrader", "Material trader") },
            { new PanelInfo( PanelIDs.Engineers, typeof(UserControlEngineers), "Engineers", "Engineers", "List of Engineers, status, recipes") },

            { new PanelInfo( "Scans and Stars") },
            { new PanelInfo( PanelIDs.Scan, typeof(UserControlScan), "Scan", "Scan", "Scan data on system") },
            { new PanelInfo( PanelIDs.EDSM, typeof(UserControlEDSM), "EDSM", "EDSM", "EDSM - Automatic web view of system") },
            { new PanelInfo( PanelIDs.Spansh, typeof(UserControlSpansh), "Spansh", "Spansh", "Spansh - Automatic web view of system") },
            { new PanelInfo( PanelIDs.EDDB, typeof(UserControlEDDB), "EDDB", "EDDB", "EDDB - Automatic web view of system") },
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
            { new PanelInfo( PanelIDs.Surveyor, typeof(UserControlSurveyor), "Surveyor", "Surveyor", "Surveyor - Surface map aid"  ) },
            { new PanelInfo( PanelIDs.NotePanel, typeof(UserControlNotePanel), "Notes", "NotePanel", "Notes overlay" ) },
            { new PanelInfo( PanelIDs.RouteTracker, typeof(UserControlRouteTracker),"Route Tracker", "RouteTracker", "Route tracker overlay") },
            { new PanelInfo( PanelIDs.Compass, typeof(UserControlCompass), "Compass", "Compass", "Compass overlay to show bearing to planetary coordinates") },
            { new PanelInfo( PanelIDs.MissionOverlay, typeof(UserControlMissionOverlay), "Mission Overlay", "MissionOV", "Mission List overlay") },
            { new PanelInfo( PanelIDs.MiningOverlay, typeof(UserControlMiningOverlay), "Mining Overlay", "MiningOV", "Mining overlay") },

            { new PanelInfo( "Settings") },
            { new PanelInfo( PanelIDs.Settings, typeof(UserControlSettings), "Settings", "SettingsPanel", "Settings for ED Discovery ") },

            { new PanelInfo( "Screenshots") },
            { new PanelInfo( PanelIDs.ScreenShot, typeof(UserControlScreenshot), "Screen Shot", "ScreenShot", "Screen shot view") },

            { new PanelInfo( "Multi Panels") },
            { new PanelInfo( PanelIDs.Grid, typeof(UserControlContainerGrid), "Grid", "TheGrid", "Grid (allows other panels to be placed in it)" ) },
            { new PanelInfo( PanelIDs.SplitterControl, typeof(UserControlContainerSplitter), "Splitter", "TheSplitter", "Splitter (allows other panels to be placed in it)" ) },

            { new PanelInfo( "Non User Panels") },
            { new PanelInfo( PanelIDs.PanelSelector, typeof(UserControlPanelSelector), "+", "Selector", "") },       // no description, not presented to user
        };

        private static HashSet<PanelIDs> WindowsOnlyPanels = new HashSet<PanelIDs>(new[] {
            PanelIDs.EDSM, // disabled due to error finding libgluezilla, and rob can't find a solution to it. freezes program
            PanelIDs.Spansh, // disabled due to error finding libgluezilla, and rob can't find a solution to it. freezes program
            PanelIDs.EDDB, // disabled due to error finding libgluezilla, and rob can't find a solution to it. freezes program
            PanelIDs.Inara, // disabled due to error finding libgluezilla, and rob can't find a solution to it. freezes program
        });

        static private List<PanelInfo> displayablepanels;   // filled by Init - all panels that can be displayed
        static private List<PanelInfo> userselectablepanellist;   // filled by Init - all panels that the user can select directly
        static private int[] userselectablepanelseperatorlistgroup;    // filled by Init - the seperator group index into userselectablepanellist

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

            userselectablepanelseperatorlistgroup = separs.ToArray();
            displayablepanels = (from x in paneldefinition where x.PopoutID != PanelIDs.GroupMarker select x).ToList(); //remove groups..
            userselectablepanellist = (from x in displayablepanels where x.Description.Length>0 select x).ToList(); //remove non selectables..
        }

        [System.Diagnostics.DebuggerDisplay("{PopoutID} {WindowTitle}")]
        public class PanelInfo      // can hold user selectable, group or non user selectable panels
        {
            public PanelIDs PopoutID;
            public Type PopoutType;
            public string WindowTitle;
            public string WindowRefName;
            public Image TabIcon { get { return PanelTypeIcons[PopoutID]; } }
            public string Description;          // must be non zero length to be user selectable

            public PanelInfo(PanelIDs p, Type t, string wintitle, string refname, string description)
            {
                PopoutID = p;
                PopoutType = t;
                WindowTitle = wintitle;
                WindowRefName = refname;
                Description = description;
            }

            public PanelInfo(string s)
            {
                PopoutID = PanelIDs.GroupMarker;
                WindowTitle = s;
                Description = string.Empty;
            }
        }

        static public PanelInfo[] GetUserSelectablePanelInfo(bool sortbyname)       // only user selected
        {
            if (sortbyname)
                return (from x in userselectablepanellist orderby x.WindowTitle select x).ToArray();        // sort by window title.
            else
                return userselectablepanellist.ToArray();
        }

        static public string[] GetUserSelectablePanelDescriptions(bool sortbyname)       // only user selected
        {
            return GetUserSelectablePanelInfo(sortbyname).Select(x => x.Description).ToArray();
        }

        static public Image[] GetUserSelectablePanelImages(bool sortbyname)                // only user selected
        {
            return GetUserSelectablePanelInfo(sortbyname).Select(x => x.TabIcon).ToArray();
        }

        static public PanelIDs[] GetUserSelectablePanelIDs(bool sortbyname)                // only user selected
        {
            return GetUserSelectablePanelInfo(sortbyname).Select(x => x.PopoutID).ToArray();
        }

        static public int[] GetUserSelectableSeperatorIndex(bool sortbyname)                // only user selected
        {
            if (sortbyname)
            {
                PanelInfo[] pilist = GetUserSelectablePanelInfo(true);
                List<int> separs = new List<int>();
                for(int o = 1; o < pilist.Length; o++)
                {
                    if (pilist[o - 1].Description[0] != pilist[o].Description[0])
                        separs.Add(o);
                }

                return separs.ToArray();
            }
            else
                return userselectablepanelseperatorlistgroup;
        }

        static public PanelIDs? GetPanelIDByWindowsRefName(string name) // null if not found
        {
            return displayablepanels.Find(x => x.WindowRefName.Equals(name, StringComparison.InvariantCultureIgnoreCase))?.PopoutID;
        }

        static public PanelIDs? GetPanelIDByControltype( Type ctrl) // null if not found
        {
            return displayablepanels.Find(x => x.PopoutType == ctrl)?.PopoutID;
        }

        static public PanelInfo GetPanelInfoByPanelID(PanelIDs p)    // null if p is invalid
        {
            int i = displayablepanels.FindIndex(x => x.PopoutID == p);
            return i>=0 ? displayablepanels[i] : null;
        }

        static public PanelInfo GetPanelInfoByType(Type t)  // null if not found
        {
            return displayablepanels.Find(x => x.PopoutType == t);
        }

        public static UserControlCommonBase Create(PanelIDs p)  // can fail if P is crap
        {
            PanelInfo pi = GetPanelInfoByPanelID(p);
            if (pi != null)
            {
                var uccb = (UserControls.UserControlCommonBase)Activator.CreateInstance(pi.PopoutType, null);
                uccb.panelid = p;
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
