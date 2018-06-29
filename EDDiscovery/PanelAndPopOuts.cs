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
            LocalMap,               // 32
            Plot,                   // 33
            PanelSelector,          // 34
            BookmarkManager,        // 35
            CombatPanel,            // 36
            ShipYardPanel,          // 37
            OutfittingPanel,        // 38 Just for Iain i'm keeping this numbering going ;-)
            SplitterControl,        // 39
            // ****** ADD More here DO NOT REORDER *****
        };

        // This is the order they are presented to the user..   you can shuffle them to your hearts content
        // description = empty means not user selectable

        static private List<PanelInfo> PanelList = new List<PanelInfo>()
        {
            // history
            { new PanelInfo( PanelIDs.Log , typeof(UserControlLog),"Log", "Log", "Program log" ) },
            { new PanelInfo( PanelIDs.Journal, typeof(UserControlJournalGrid), "Journal", "JournalHistory", "Journal grid view") },
            { new PanelInfo( PanelIDs.TravelGrid, typeof(UserControlTravelGrid), "Travel History", "TravelHistory", "History grid view") },
            { new PanelInfo( PanelIDs.StarList, typeof(UserControlStarList), "Star List", "StarList", "Visited star list", transparent: false) },

            // about what your stats
            { new PanelInfo( PanelIDs.Materials, typeof(UserControlMaterials) , "Materials", "Materials", "Materials count" ) },
            { new PanelInfo( PanelIDs.Commodities, typeof(UserControlCommodities), "Commodities", "Commodities", "Commodity count") },
            { new PanelInfo( PanelIDs.Ledger, typeof(UserControlLedger), "Ledger", "Ledger", "Ledger of cash related entries") },
            { new PanelInfo( PanelIDs.Missions, typeof(UserControlMissions), "Missions", "Missions", "Mission list") },
            { new PanelInfo( PanelIDs.Modules, typeof(UserControlModules), "Ships/Loadout", "Modules", "Ships and their loadouts plus stored modules") },
            { new PanelInfo( PanelIDs.Statistics, typeof(UserControlStats), "Statistics", "Stats", "Statistics Information") },

            // about where you've been
            { new PanelInfo( PanelIDs.MarketData, typeof(UserControlMarketData), "Market Data", "MarketData", "Market data view, giving commodity price information where available" ) },
            { new PanelInfo( PanelIDs.ShipYardPanel, typeof(UserControlShipYards), "Ship Yards", "ShipYards", "Information on ship yards from places you have visited") },
            { new PanelInfo( PanelIDs.OutfittingPanel, typeof(UserControlOutfitting), "Outfitting", "Outfitting", "Information on outfitting items in ship yards from places you have visited") },

            // Engineering/synth
            { new PanelInfo( PanelIDs.Synthesis, typeof(UserControlSynthesis), "Synthesis", "Synthesis", "Synthesis planner") },
            { new PanelInfo( PanelIDs.Engineering, typeof(UserControlEngineering), "Engineering", "Engineering", "Engineering planner") },
            { new PanelInfo( PanelIDs.ShoppingList, typeof(UserControlShoppingList), "Shopping List", "ShoppingList", "Shopping list planner combining synthesis and engineering") },

            // Scans and stars
            { new PanelInfo( PanelIDs.Scan, typeof(UserControlScan), "Scan", "Scan", "Scan data on system", transparent: false) },
            { new PanelInfo( PanelIDs.ScanGrid, typeof(UserControlScanGrid), "Scan Grid", "ScanGrid", "Scan data on system in a grid", transparent: false) },
            { new PanelInfo( PanelIDs.StarDistance, typeof(UserControlStarDistance), "Nearest Stars", "StarDistance","List of nearest stars") },
            { new PanelInfo( PanelIDs.EstimatedValues, typeof(UserControlEstimatedValues),"Estimated Values", "EstimatedValues", "Estimated scan values of bodies in system", transparent: false) },
            { new PanelInfo( PanelIDs.LocalMap, typeof(UserControlLocalMap), "Local Map", "LocalMap", "3D Map of systems in range", transparent: false) },
            { new PanelInfo( PanelIDs.Plot, typeof(UserControlPlot), "2D Plot", "Plot", "2D Plot of systems in range", transparent: false) },
            { new PanelInfo( PanelIDs.EDSM, typeof(UserControlEDSM), "EDSM Star Finder", "EDSMStarFinder", "EDSM Star finder") },
            { new PanelInfo( PanelIDs.Trilateration, typeof(UserControlTrilateration) ,"Trilateration", "Trilateration", "Trilateration of stars with unknown positions") },
            { new PanelInfo( PanelIDs.BookmarkManager, typeof(UserControlBookmarks), "Bookmarks", "Bookmarks", "Manage System and planetary bookmarks", transparent:false)},

            // Combat
            { new PanelInfo( PanelIDs.CombatPanel, typeof(UserControlCombatPanel), "Combat", "Combat", "Display combat statistics", transparent:false)},

            // Routeplanning
            { new PanelInfo( PanelIDs.Route, typeof(UserControlRoute), "Route Finder", "RouteFinder", "Route Finder from stored star data") },
            { new PanelInfo( PanelIDs.Expedition, typeof(UserControlExpedition), "Expedition", "Expedition", "Expedition Planner, make up a expedition route") },
            { new PanelInfo( PanelIDs.Exploration, typeof(UserControlExploration), "Exploration", "Exploration", "Exploration Planner, make a list of the stars to explore") },

            // Info panels
            { new PanelInfo( PanelIDs.SystemInformation, typeof(UserControlSysInfo), "System Information", "SystemInfo", "System Information" , transparent:false ) },
            { new PanelInfo( PanelIDs.Spanel, typeof(UserControlSpanel), "Summary Panel", "Spanel", "Summary panel overlay" , transparent: false ) },
            { new PanelInfo( PanelIDs.Trippanel, typeof(UserControlTrippanel), "Trip Computer", "Trippanel", "Trip computer overlay" , transparent: false) },
            { new PanelInfo( PanelIDs.NotePanel, typeof(UserControlNotePanel), "Notes", "NotePanel", "System notes overlay" , transparent: false) },
            { new PanelInfo( PanelIDs.RouteTracker, typeof(UserControlRouteTracker),"Route Tracker", "RouteTracker", "Route tracker overlay", transparent: false) },
            { new PanelInfo( PanelIDs.Compass, typeof(UserControlCompass), "Compass", "Compass", "Ground compass overlay to show bearing to planetary coordinates", transparent:true) },

            // settings
            { new PanelInfo( PanelIDs.Settings, typeof(UserControlSettings), "Settings", "SettingsPanel", "Settings for ED Discovery ") },

            // Screenshots
            { new PanelInfo( PanelIDs.ScreenShot, typeof(UserControlScreenshot), "Screen Shot", "ScreenShot", "Screen shot view") },

            // Multi panels
            { new PanelInfo( PanelIDs.Grid, typeof(UserControlContainerGrid), "Grid", "TheGrid", "Grid (allows other panels to be placed in the it)" , transparent:false) },
            { new PanelInfo( PanelIDs.SplitterControl, typeof(UserControlContainerSplitter), "Splitter", "TheSplitter", "Splitter (allows other panels to be placed in the it)" , transparent:false) },

            // Specials changable user panels
            { new PanelInfo( PanelIDs.PanelSelector, typeof(UserControlPanelSelector), "+", "Selector", "") },       // no description, not presented to user
        };

        public static IReadOnlyDictionary<PanelIDs, Image> PanelTypeIcons { get; private set; } = new IconGroup<PanelIDs>("Panels");

        public static void Init()
        {
            PanelTypeIcons = new IconGroup<PanelIDs>("Panels");
            foreach (PanelInfo i in PanelList)
            {
                i.Description = BaseUtils.Translator.Instance.Translate(i.Description, "PopOutInfo." + i.PopoutID + ".Description");
                i.WindowTitle = BaseUtils.Translator.Instance.Translate(i.WindowTitle, "PopOutInfo." + i.PopoutID + ".Title");
            }
        }

        public class PanelInfo
        {
            public PanelIDs PopoutID;
            public Type PopoutType;
            public string WindowTitle;
            public string WindowRefName;
            public Image TabIcon { get { return PanelTypeIcons[PopoutID]; } }
            public string Description;
            public bool SupportsTransparency;
            public bool DefaultTransparent;

            public PanelInfo(PanelIDs p, Type t, string prefix, string rf, string tooltip, bool? transparent = null)
            {
                PopoutID = p;
                PopoutType = t;
                WindowTitle = prefix;
                WindowRefName = rf;
                Description = tooltip;
                SupportsTransparency = transparent != null;
                DefaultTransparent = transparent ?? false;
            }

            public bool IsUserSelectable { get { return Description.Length > 0; } }
        }

        static public string[] GetPanelDescriptions()       // only user selected
        {
            return (from PanelInfo x in PanelList where x.IsUserSelectable select x.Description).ToArray();
        }

        static public Image[] GetPanelImages()                // only user selected
        {
            return (from PanelInfo x in PanelList where x.IsUserSelectable select x.TabIcon).ToArray();
        }

        static public PanelIDs[] GetPanelIDs()                // only user selected
        {
            return (from PanelInfo x in PanelList where x.IsUserSelectable select x.PopoutID).ToArray();
        }

        static public PanelIDs? GetPanelIDByWindowsRefName(string name) // null if not found
        {
            return PanelList.Find(x=>x.WindowRefName.Equals(name, StringComparison.InvariantCultureIgnoreCase))?.PopoutID;
        }

        static public PanelInfo GetPanelInfoByPanelID(PanelIDs p)    // null if p is invalid
        {
            int i = PanelList.FindIndex(x => x.PopoutID == p);
            return i>=0 ? PanelList[i] : null;
        }

        static public PanelInfo GetPanelInfoByType(Type t)  // null if not found
        {
            return PanelList.Find(x => x.PopoutType == t);
        }

        public static UserControlCommonBase Create(PanelIDs p)  // can fail if P is crap
        {
            PanelInfo pi = GetPanelInfoByPanelID(p);
            return pi != null ? (UserControls.UserControlCommonBase)Activator.CreateInstance(pi.PopoutType, null) : null;
        }

        public static System.Windows.Forms.ToolStripMenuItem MakeToolStripMenuItem(PanelIDs p, System.EventHandler h)
        {
            PanelInformation.PanelInfo pi = GetPanelInfoByPanelID(p);
            if (pi.IsUserSelectable)
            {
                System.Windows.Forms.ToolStripMenuItem mi = new System.Windows.Forms.ToolStripMenuItem();
                mi.Text = pi.Description;
                mi.Size = new System.Drawing.Size(250, 22);
                mi.Tag = pi.PopoutID;
                mi.Image = pi.TabIcon;
                mi.Click += h;
                return mi;
            }
            else
                return null;
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
            foreach (PanelInformation.PanelIDs p in Enum.GetValues(typeof(PanelInformation.PanelIDs)))        // in terms of PanelInformation.PopOuts Enum
            {
                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID(p);
                if (pi != null) // paranoia
                {
                    int numopened = usercontrolsforms.CountOf(pi.PopoutType);
                    //System.Diagnostics.Debug.WriteLine("Saved panel type " + paneltype.Name + " " + p.ToString() + " " + numopened);
                    SQLiteConnectionUser.PutSettingInt(EDDProfiles.Instance.UserControlsPrefix + "SavedPanelInformation.PopOuts:" + p.ToString(), numopened);
                }
            }
        }

        internal void LoadSavedPopouts()
        {
            foreach (PanelInformation.PanelIDs p in Enum.GetValues(typeof(PanelInformation.PanelIDs)))        // in terms of PanelInformation.PopOuts Enum
            {
                int numtoopen = SQLiteConnectionUser.GetSettingInt(EDDProfiles.Instance.UserControlsPrefix + "SavedPanelInformation.PopOuts:" + p.ToString(), 0);
                PanelInformation.PanelInfo pi = PanelInformation.GetPanelInfoByPanelID(p);

                //System.Diagnostics.Debug.WriteLine("Load panel type " + paneltype.Name + " " + p.ToString() + " " + numtoopen);

                if (pi != null && numtoopen > 0) // paranoia on first..
                {
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

                tcf.Init(ctrl, windowtitle, discoveryform.theme.WindowsFrame, refname, discoveryform.TopMost,
                            poi.DefaultTransparent, discoveryform.theme.LabelColor, discoveryform.theme.SPanelColor);

                ctrl.Init(discoveryform, UserControls.UserControlCommonBase.DisplayNumberPopOuts + numopened - 1);

                tcf.Show();                                                     // this ends up, via Form Shown, calls LoadLayout in the UCCB.

                if (tcf.UserControl != null)
                    tcf.UserControl.Font = discoveryform.theme.GetFont;        // Important. Apply font autoscaling to the user control
                                                                                // ApplyToForm does not apply the font to the actual UC, only
                                                                                // specific children controls.  The TabControl in the discoveryform ends up autoscaling most stuff
                                                                                // the children directly attached to the discoveryform are not autoscaled

                discoveryform.theme.ApplyToForm(tcf);

                discoveryform.ActionRun(Actions.ActionEventEDList.onPopUp, null, new Conditions.ConditionVariables(new string[] { "PopOutName", refname , "PopOutTitle", windowtitle, "PopOutIndex", numopened.ToString()} ));
            }

            return ctrl;
        }
    }
}
