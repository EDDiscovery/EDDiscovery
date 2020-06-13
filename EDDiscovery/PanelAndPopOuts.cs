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

using EDDiscovery.Icons;
using EDDiscovery.UserControls;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery
{
    static public class PanelInformation
    {
        public enum PanelIDs        // id's.. used in tab controls, and in button pop outs button
        {
            GroupMarker = -1,

            Log = 0,                // BEWARE
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
            Search,                 // 24 (was EDSM)
            ShoppingList,
            Route,
            Expedition,             // 27
            Trilateration,          // 28
            Settings,               // 29
            ScanGrid,               // 30
            Compass,                // 31
            LocalMap,               // 32
            Plot,                   // 33
            PanelSelector,          // 34
            BookmarkManager,        // 35
            CombatPanel,            // 36
            ShipYardPanel,          // 37
            OutfittingPanel,        // 38 Just for Iain i'm keeping this numbering going ;-)
            SplitterControl,        // 39
            MissionOverlay,         // 40
            CaptainsLog,            // 41 Actually its important for debugging purposes so you can recognised the ID as its stored by ID in the DB
            Surveyor,               // 42
            EDSM,                   // 43
            MaterialTrader,         // 44
            // ****** ADD More here DO NOT REORDER *****
        };

        // This is the order they are presented to the user if not using alpha sort..   you can shuffle them to your hearts content
        // description = empty means not user selectable.  Single text string expresses a group.

        static private List<PanelInfo> paneldefinition = new List<PanelInfo>()
        {
            { new PanelInfo( "History") },
            { new PanelInfo( PanelIDs.Log , typeof(UserControlLog),"Log", "Log", "Log of program information" ) },
            { new PanelInfo( PanelIDs.Journal, typeof(UserControlJournalGrid), "Journal", "JournalHistory", "Journal grid view") },
            { new PanelInfo( PanelIDs.TravelGrid, typeof(UserControlTravelGrid), "History", "TravelHistory", "History grid view") },
            { new PanelInfo( PanelIDs.StarList, typeof(UserControlStarList), "Visited Stars", "StarList", "Visited Star list", transparent: false) },

            { new PanelInfo( "Current State") },
            { new PanelInfo( PanelIDs.Materials, typeof(UserControlMaterials) , "Materials", "Materials", "Materials count" ) },
            { new PanelInfo( PanelIDs.Commodities, typeof(UserControlCommodities), "Commodities", "Commodities", "Commodity count") },
            { new PanelInfo( PanelIDs.Ledger, typeof(UserControlLedger), "Ledger", "Ledger", "Ledger of cash related entries") },
            { new PanelInfo( PanelIDs.Missions, typeof(UserControlMissions), "Missions", "Missions", "Mission list") },
            { new PanelInfo( PanelIDs.Modules, typeof(UserControlModules), "Ships/Loadout", "Modules", "Ships and their loadouts plus stored modules") },
            { new PanelInfo( PanelIDs.Statistics, typeof(UserControlStats), "Statistics", "Stats", "Statistics Information") },

            { new PanelInfo( "Shipyard Data") },
            { new PanelInfo( PanelIDs.MarketData, typeof(UserControlMarketData), "Market Data", "MarketData", "Market data view, giving commodity price information where available" ) },
            { new PanelInfo( PanelIDs.ShipYardPanel, typeof(UserControlShipYards), "Ship Yards", "ShipYards", "Ship yards information from places you have visited") },
            { new PanelInfo( PanelIDs.OutfittingPanel, typeof(UserControlOutfitting), "Outfitting", "Outfitting", "Outfitting items in ship yards from places you have visited") },

            { new PanelInfo( "Engineering/Synthesis") },
            { new PanelInfo( PanelIDs.Synthesis, typeof(UserControlSynthesis), "Synthesis", "Synthesis", "Synthesis planner") },
            { new PanelInfo( PanelIDs.Engineering, typeof(UserControlEngineering), "Engineering", "Engineering", "Engineering planner") },
            { new PanelInfo( PanelIDs.ShoppingList, typeof(UserControlShoppingList), "Shopping List", "ShoppingList", "Shopping list planner combining synthesis and engineering") },
            { new PanelInfo( PanelIDs.MaterialTrader, typeof(UserControlMaterialTrader), "Material Trader", "MaterialTrader", "Material trader") },

            { new PanelInfo( "Scans and Stars") },
            { new PanelInfo( PanelIDs.Scan, typeof(UserControlScan), "Scan", "Scan", "Scan data on system", transparent: false) },
            { new PanelInfo( PanelIDs.EDSM, typeof(UserControlEDSM), "EDSM", "EDSM", "Automatic web view of EDSM page on system") },
            { new PanelInfo( PanelIDs.ScanGrid, typeof(UserControlScanGrid), "Scan Grid", "ScanGrid", "Scan data on system in a grid", transparent: false) },
            { new PanelInfo( PanelIDs.StarDistance, typeof(UserControlStarDistance), "Nearest Stars", "StarDistance","Nearest stars from current position") },
            { new PanelInfo( PanelIDs.EstimatedValues, typeof(UserControlEstimatedValues),"Estimated Values", "EstimatedValues", "Estimated Scan values of bodies in system", transparent: false) },
            { new PanelInfo( PanelIDs.LocalMap, typeof(UserControlLocalMap), "Map 3D Local Systems", "LocalMap", "Map in 3D of local systems", transparent: false) },
            { new PanelInfo( PanelIDs.Plot, typeof(UserControlPlot), "Map 2D Local Systems", "Plot", "Map in 2D of local systems", transparent: false) },
            { new PanelInfo( PanelIDs.Search, typeof(UserControlSearch), "Search", "SearchFinder", "Search") },
            { new PanelInfo( PanelIDs.Trilateration, typeof(UserControlTrilateration) ,"Trilateration", "Trilateration", "Trilateration of stars with unknown positions") },

            { new PanelInfo( "Bookmarks and Logs") },
            { new PanelInfo( PanelIDs.BookmarkManager, typeof(UserControlBookmarks), "Bookmarks", "Bookmarks", "Bookmarks on systems and planets", transparent:false)},
            { new PanelInfo( PanelIDs.CaptainsLog, typeof(UserControlCaptainsLog), "Captain's Log", "CaptainsLog", "Captain's Log - notes on your travels", transparent:false)},

            { new PanelInfo( "Combat") },
            { new PanelInfo( PanelIDs.CombatPanel, typeof(UserControlCombatPanel), "Combat", "Combat", "Combat statistics", transparent:false)},

            { new PanelInfo( "Routes and Expeditions") },
            { new PanelInfo( PanelIDs.Route, typeof(UserControlRoute), "Route Finder", "RouteFinder", "Route Finder from stored star data") },
            { new PanelInfo( PanelIDs.Expedition, typeof(UserControlExpedition), "Expedition", "Expedition", "Expedition Planner, make up a expedition route") },
            { new PanelInfo( PanelIDs.Exploration, typeof(UserControlExploration), "Exploration", "Exploration", "Exploration Planner, make a list of the stars to explore") },

            { new PanelInfo( "Overlay Panels") },
            { new PanelInfo( PanelIDs.SystemInformation, typeof(UserControlSysInfo), "System Information", "SystemInfo", "System Information" , transparent:false ) },
            { new PanelInfo( PanelIDs.Spanel, typeof(UserControlSpanel), "Summary Panel", "Spanel", "Summary panel overlay" , transparent: false ) },
            { new PanelInfo( PanelIDs.Surveyor, typeof(UserControlSurveyor), "Surveyor", "Surveyor", "Surveyor - Surface map aid" , transparent: false ) },
            { new PanelInfo( PanelIDs.Trippanel, typeof(UserControlTrippanel), "Trip Computer", "Trippanel", "Trip computer overlay" , transparent: false) },
            { new PanelInfo( PanelIDs.NotePanel, typeof(UserControlNotePanel), "Notes", "NotePanel", "Notes overlay" , transparent: false) },
            { new PanelInfo( PanelIDs.RouteTracker, typeof(UserControlRouteTracker),"Route Tracker", "RouteTracker", "Route tracker overlay", transparent: false) },
            { new PanelInfo( PanelIDs.Compass, typeof(UserControlCompass), "Compass", "Compass", "Compass overlay to show bearing to planetary coordinates", transparent:true) },
            { new PanelInfo( PanelIDs.MissionOverlay, typeof(UserControlMissionOverlay), "Mission Overlay", "MissionOV", "Mission List overlay", transparent:true) },

            { new PanelInfo( "Settings") },
            { new PanelInfo( PanelIDs.Settings, typeof(UserControlSettings), "Settings", "SettingsPanel", "Settings for ED Discovery ") },

            { new PanelInfo( "Screenshots") },
            { new PanelInfo( PanelIDs.ScreenShot, typeof(UserControlScreenshot), "Screen Shot", "ScreenShot", "Screen shot view") },

            { new PanelInfo( "Multi Panels") },
            { new PanelInfo( PanelIDs.Grid, typeof(UserControlContainerGrid), "Grid", "TheGrid", "Grid (allows other panels to be placed in the it)" , transparent:false) },
            { new PanelInfo( PanelIDs.SplitterControl, typeof(UserControlContainerSplitter), "Splitter", "TheSplitter", "Splitter (allows other panels to be placed in the it)" , transparent:false) },

            { new PanelInfo( "Non User Panels") },
            { new PanelInfo( PanelIDs.PanelSelector, typeof(UserControlPanelSelector), "+", "Selector", "") },       // no description, not presented to user
        };

        private static HashSet<PanelIDs> WindowsOnlyPanels = new HashSet<PanelIDs>(new[] {
            PanelIDs.LocalMap, // Depends on System.Windows.Forms.DataVizualization.Charting, not implemented in Mono
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

                    i.WindowTitle = BaseUtils.Translator.Instance.Translate(i.WindowTitle, "PopOutInfo.Group." + i.WindowTitle.FirstAlphaNumericText());
                }
                else if ( i.Description.Length > 0 )        // if selectable, translate and update index
                {
                    i.WindowTitle = BaseUtils.Translator.Instance.Translate(i.WindowTitle, "PopOutInfo." + i.PopoutID + ".Title");
                    i.Description = BaseUtils.Translator.Instance.Translate(i.Description, "PopOutInfo." + i.PopoutID + ".Description");
                    offset++;
                }
            }

            userselectablepanelseperatorlistgroup = separs.ToArray();      
            displayablepanels = (from x in paneldefinition where x.PopoutID != PanelIDs.GroupMarker select x).ToList(); //remove groups..
            userselectablepanellist = (from x in displayablepanels where x.Description.Length>0 select x).ToList(); //remove non selectables..

            PanelTypeIcons = new BaseUtils.Icons.IconGroup<PanelIDs>("Panels");
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
            public bool SupportsTransparency;
            public bool DefaultTransparent;

            public PanelInfo(PanelIDs p, Type t, string wintitle, string refname, string description, bool? transparent = null)
            {
                PopoutID = p;
                PopoutType = t;
                WindowTitle = wintitle;
                WindowRefName = refname;
                Description = description;
                SupportsTransparency = transparent != null;
                DefaultTransparent = transparent ?? false;
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
                return (from x in userselectablepanellist orderby x.Description select x).ToArray();
            else
                return userselectablepanellist.ToArray();
        }

        static public string[] GetUserSelectablePanelDescriptions(bool sortbyname)       // only user selected
        {
            if (sortbyname)
                return (from x in userselectablepanellist orderby x.Description select x.Description).ToArray();
            else
                return userselectablepanellist.Select(x => x.Description).ToArray();
        }

        static public Image[] GetUserSelectablePanelImages(bool sortbyname )                // only user selected
        {
            if (sortbyname)
                return (from x in userselectablepanellist orderby x.Description select x.TabIcon).ToArray();
            else
                return userselectablepanellist.Select(x => x.TabIcon).ToArray();
        }

        static public PanelIDs[] GetUserSelectablePanelIDs(bool sortbyname)                // only user selected
        {
            if (sortbyname)
                return (from x in userselectablepanellist orderby x.Description select x.PopoutID).ToArray();
            else
                return userselectablepanellist.Select(x => x.PopoutID).ToArray();
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
            return displayablepanels.Find(x=>x.WindowRefName.Equals(name, StringComparison.InvariantCultureIgnoreCase))?.PopoutID;
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
            return pi != null ? (UserControls.UserControlCommonBase)Activator.CreateInstance(pi.PopoutType, null) : null;
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

    public class PopOutControl
    {
        public Forms.UserControlFormList usercontrolsforms;
        EDDiscoveryForm discoveryform;

        public PopOutControl( EDDiscoveryForm ed )
        {
            discoveryform = ed;
            usercontrolsforms = new Forms.UserControlFormList(discoveryform);
        }

        public int Count { get { return usercontrolsforms.Count;  } }
        public Forms.UserControlForm GetByWindowsRefName(string name) { return usercontrolsforms.GetByWindowsRefName(name); }
        public Forms.UserControlForm this[int i] { get { return usercontrolsforms[i]; } }

        private static string PopOutSaveID(PanelInformation.PanelIDs p)
        {
            return EDDProfiles.Instance.UserControlsPrefix + "SavedPanelInformation.PopOuts:" + p.ToString();
        }

        public void ShowAllPopOutsInTaskBar()
        {
            usercontrolsforms.ShowAllInTaskBar();
        }

        public void MakeAllPopoutsOpaque()
        {
            usercontrolsforms.MakeAllOpaque();
        }

        public void CloseAllPopouts()
        {
            usercontrolsforms.CloseAll();
        }

        public void SaveCurrentPopouts()
        {
            foreach (PanelInformation.PanelIDs p in Enum.GetValues(typeof(PanelInformation.PanelIDs)))        // in terms of PanelInformation.PopOuts Enum
            {
                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID(p);
                if (pi != null) // paranoia
                {
                    int numopened = usercontrolsforms.CountOf(pi.PopoutType);
                    if ( numopened>0) System.Diagnostics.Debug.WriteLine($"Save Popout {p} {numopened}");
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(PopOutSaveID(p), numopened);
                }
            }
        }

        public void LoadSavedPopouts()
        {
            foreach (PanelInformation.PanelIDs p in Enum.GetValues(typeof(PanelInformation.PanelIDs)))        // in terms of PanelInformation.PopOuts Enum
            {
                int numtoopen = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(PopOutSaveID(p), 0);

                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID(p);

                //System.Diagnostics.Debug.WriteLine("Load panel type " + paneltype.Name + " " + p.ToString() + " " + numtoopen);

                if (pi != null && numtoopen > 0) // paranoia on first..
                {
                    System.Diagnostics.Debug.WriteLine($"Load Popout {p} {numtoopen}");

                    int numopened = usercontrolsforms.CountOf(pi.PopoutType);
                    if (numopened < numtoopen)
                    {
                        for (int i = numopened + 1; i <= numtoopen; i++)
                            PopOut(p);
                    }
                }
            }
        }


        public UserControlCommonBase PopOut(PanelInformation.PanelIDs selected)
        {
            Forms.UserControlForm tcf = usercontrolsforms.NewForm();
            tcf.Icon = Properties.Resources.edlogo_3mo_icon;

            UserControlCommonBase ctrl = PanelInformation.Create(selected);

            PanelInformation.PanelInfo poi = PanelInformation.GetPanelInfoByPanelID(selected);

            if (ctrl != null && poi != null )
            {
                int numopened = usercontrolsforms.CountOf(ctrl.GetType()) + 1;
                string windowtitle = poi.WindowTitle + " " + ((numopened > 1) ? numopened.ToString() : "");
                string refname = poi.WindowRefName + numopened.ToString();

                System.Diagnostics.Trace.WriteLine("PO:Make " + windowtitle + " ucf " + ctrl.GetType().Name);

                //System.Diagnostics.Debug.WriteLine("TCF init");
                tcf.Init(ctrl, windowtitle, discoveryform.theme.WindowsFrame, refname, discoveryform.TopMost,
                            poi.DefaultTransparent, discoveryform.theme.LabelColor, discoveryform.theme.SPanelColor);

                //System.Diagnostics.Debug.WriteLine("UCCB init of " + ctrl.GetType().Name);
                ctrl.Init(discoveryform, UserControls.UserControlCommonBase.DisplayNumberPopOuts + numopened - 1);

                discoveryform.theme.ApplyStd(tcf);  // apply theming/scaling to form before shown, so that it restored back to correct position (done in UCF::onLoad)

                //System.Diagnostics.Debug.WriteLine("Show");
                tcf.Show();                                                     // this ends up, via Form Shown, calls LoadLayout in the UCCB.

                discoveryform.ActionRun(Actions.ActionEventEDList.onPopUp, null, new BaseUtils.Variables(new string[] { "PopOutName", refname , "PopOutTitle", windowtitle, "PopOutIndex", numopened.ToString()} ));
            }

            return ctrl;
        }
    }
}
