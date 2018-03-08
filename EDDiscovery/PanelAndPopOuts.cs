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

namespace EDDiscovery.Forms
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
            BookmarkManager,
            // ****** ADD More here DO NOT REORDER *****
        };

        // This is the order they are presented to the user..   you can shuffle them to your hearts content
        // ****** DO NOT add UserControlHistory - the display numbers used are special and not okay for a generic panel.  Since you can do the same via a grid, its okay
        // description = empty means not user selectable

        static private List<PanelInfo> PanelList = new List<PanelInfo>()
        {
            { new PanelInfo( PanelIDs.Log , typeof(UserControlLog),"Log", "Log", "Program log" ) },
            { new PanelInfo( PanelIDs.StarDistance, typeof(UserControlStarDistance), "Nearest Stars", "StarDistance","List of nearest stars") },
            { new PanelInfo( PanelIDs.Materials, typeof(UserControlMaterials) , "Materials", "Materials", "Materials count" ) },
            { new PanelInfo( PanelIDs.Commodities, typeof(UserControlCommodities), "Commodities", "Commodities", "Commodity count") },
            { new PanelInfo( PanelIDs.Ledger, typeof(UserControlLedger), "Ledger", "Ledger", "Ledger of cash related entries") },
            { new PanelInfo( PanelIDs.Journal, typeof(UserControlJournalGrid), "Journal", "JournalHistory", "Journal grid view") },
            { new PanelInfo( PanelIDs.TravelGrid, typeof(UserControlTravelGrid), "Travel History", "TravelHistory", "History grid view") },
            { new PanelInfo( PanelIDs.StarList, typeof(UserControlStarList), "Star List", "StarList", "Visited star list", transparent: false) },
            { new PanelInfo( PanelIDs.MarketData, typeof(UserControlMarketData), "Market Data", "MarketData", "Market data view, giving commodity price information where available" ) },
            { new PanelInfo( PanelIDs.Missions, typeof(UserControlMissions), "Missions", "Missions", "Mission list") },
            { new PanelInfo( PanelIDs.Synthesis, typeof(UserControlSynthesis), "Synthesis", "Synthesis", "Synthesis planner") },
            { new PanelInfo( PanelIDs.Engineering, typeof(UserControlEngineering), "Engineering", "Engineering", "Engineering planner") },
            { new PanelInfo( PanelIDs.ShoppingList, typeof(UserControlShoppingList), "Shopping List", "ShoppingList", "Shopping list planner combining synthesis and engineering") },
            { new PanelInfo( PanelIDs.Scan, typeof(UserControlScan), "Scan", "Scan", "Scan data on system", transparent: false) },
            { new PanelInfo( PanelIDs.ScanGrid, typeof(UserControlScanGrid), "Scan Grid", "ScanGrid", "Scan data on system in a grid", transparent: false) },
            { new PanelInfo( PanelIDs.EstimatedValues, typeof(UserControlEstimatedValues),"Estimated Values", "EstimatedValues", "Estimated scan values of bodies in system", transparent: false) },
            { new PanelInfo( PanelIDs.Modules, typeof(UserControlModules), "Loadout", "Modules", "Ship loadout for current ships and stored modules") },
            { new PanelInfo( PanelIDs.LocalMap, typeof(UserControlLocalMap), "Local Map", "LocalMap", "3D Map of systems in range", transparent: false) },
            { new PanelInfo( PanelIDs.Plot, typeof(UserControlPlot), "2D Plot", "Plot", "2D Plot of systems in range", transparent: false) },
            { new PanelInfo( PanelIDs.Exploration, typeof(UserControlExploration), "Exploration", "Exploration", "Exploration Information") },
            { new PanelInfo( PanelIDs.ScreenShot, typeof(UserControlScreenshot), "Screen Shot", "ScreenShot", "Screen shot view") },
            { new PanelInfo( PanelIDs.Statistics, typeof(UserControlStats), "Statistics", "Stats", "Statistics Information") },
            { new PanelInfo( PanelIDs.SystemInformation, typeof(UserControlSysInfo), "System Information", "SystemInfo", "System Information" , transparent:false ) },
            { new PanelInfo( PanelIDs.EDSM, typeof(UserControlEDSM), "EDSM Star Finder", "EDSMStarFinder", "EDSM Star finder") },
            { new PanelInfo( PanelIDs.Route, typeof(UserControlRoute), "Route Finder", "RouteFinder", "Route Finder from stored star data") },
            { new PanelInfo( PanelIDs.Expedition, typeof(UserControlExpedition), "Expedition", "Expedition", "Expedition Planner") },
            { new PanelInfo( PanelIDs.Trilateration, typeof(UserControlTrilateration) ,"Trilateration", "Trilateration", "Trilateration of stars with unknown positions") },
            { new PanelInfo( PanelIDs.Spanel, typeof(UserControlSpanel), "Summary Panel", "Spanel", "Summary panel overlay" , transparent: false ) },
            { new PanelInfo( PanelIDs.Trippanel, typeof(UserControlTrippanel), "Trip Computer", "Trippanel", "Trip computer overlay" , transparent: false) },
            { new PanelInfo( PanelIDs.NotePanel, typeof(UserControlNotePanel), "Notes", "NotePanel", "System notes overlay" , transparent: false) },
            { new PanelInfo( PanelIDs.RouteTracker, typeof(UserControlRouteTracker),"Route Tracker", "RouteTracker", "Route tracker overlay", transparent: false) },
            { new PanelInfo( PanelIDs.Settings, typeof(UserControlSettings), "Settings", "SettingsPanel", "Settings for ED Discovery ") },
            { new PanelInfo( PanelIDs.Grid, typeof(UserControlContainerGrid), "Grid", "TheGrid", "Grid (allows other panels to be placed in the it)" , transparent:false) },
            { new PanelInfo( PanelIDs.Compass, typeof(UserControlCompass), "Compass", "Compass", "Ground compass navigation panel to work out the bearing between planetary coordinates", transparent:true) },
            { new PanelInfo( PanelIDs.PanelSelector, typeof(UserControlPanelSelector), "+", "Selector", "") },       // no description, not presented to user
            { new PanelInfo( PanelIDs.BookmarkManager, typeof(UserControlBookmarks), "Bookmarks", "Bookmarks", "Manage System and planetary bookmarks", transparent:false)},
        };

        public static IReadOnlyDictionary<PanelIDs, Image> PanelTypeIcons { get; private set; } = new IconGroup<PanelIDs>("Panels");

        public static void InitIcons()
        {
            PanelTypeIcons = new IconGroup<PanelIDs>("Panels");
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

        static public PanelInfo GetPanelInfoByEnum(PanelIDs p)
        {
            return PanelList[PanelList.FindIndex(x => x.PopoutID == p)];
        }

        static public PanelInfo GetPanelInfoByType(Type t)  // null if not found
        {
            return PanelList.Find(x => x.PopoutType == t);
        }

        static public Type GetPanelTypeByEnum(PanelIDs p)       // null if panel ID is bad.
        {
            int index = GetPanelIndexByEnum(p);
            return index >= 0 ? PanelList[index].PopoutType : null;
        }

        public static UserControlCommonBase Create(PanelIDs p)  // can fail if P is crap
        {
            Type t = GetPanelTypeByEnum(p);
            return t != null ? (UserControls.UserControlCommonBase)Activator.CreateInstance(t, null) : null;
        }

        static private int GetPanelIndexByEnum(PanelIDs p)
        {
            return PanelList.FindIndex(x => x.PopoutID == p);
        }

        public static System.Windows.Forms.ToolStripMenuItem MakeToolStripMenuItem(PanelIDs p, System.EventHandler h)
        {
            PanelInformation.PanelInfo pi = GetPanelInfoByEnum(p);
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
            foreach (PanelInformation.PanelIDs p in Enum.GetValues(typeof(PanelInformation.PanelIDs)))        // in terms of PanelInformation.PopOuts Enum
            {
                Type paneltype = PanelInformation.GetPanelTypeByEnum(p);
                if (paneltype != null) // paranoia
                {
                    int numopened = usercontrolsforms.CountOf(paneltype);
                    //System.Diagnostics.Debug.WriteLine("Saved panel type " + paneltype.Name + " " + p.ToString() + " " + numopened);
                    SQLiteConnectionUser.PutSettingInt("SavedPanelInformation.PopOuts:" + p.ToString(), numopened);
                }
            }
        }

        internal void LoadSavedPopouts()
        {
            foreach (PanelInformation.PanelIDs p in Enum.GetValues(typeof(PanelInformation.PanelIDs)))        // in terms of PanelInformation.PopOuts Enum
            {
                int numtoopen = SQLiteConnectionUser.GetSettingInt("SavedPanelInformation.PopOuts:" + p.ToString(), 0);
                Type paneltype = PanelInformation.GetPanelTypeByEnum(p);

                //System.Diagnostics.Debug.WriteLine("Load panel type " + paneltype.Name + " " + p.ToString() + " " + numtoopen);

                if (paneltype != null && numtoopen > 0) // paranoia on first..
                {
                    int numopened = usercontrolsforms.CountOf(paneltype);
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
            UserControlForm tcf = usercontrolsforms.NewForm();
            tcf.Icon = Properties.Resources.edlogo_3mo_icon;

            UserControlCommonBase ctrl = PanelInformation.Create(selected);

            PanelInformation.PanelInfo poi = PanelInformation.GetPanelInfoByEnum(selected);

            if (ctrl != null && poi != null )
            {
                int numopened = usercontrolsforms.CountOf(ctrl.GetType()) + 1;
                string windowtitle = poi.WindowTitle + " " + ((numopened > 1) ? numopened.ToString() : "");
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
            return ctrl;
        }
    }
}
