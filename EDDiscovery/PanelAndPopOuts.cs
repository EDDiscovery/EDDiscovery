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
            // ****** ADD More here DO NOT REORDER *****
        };

        // This is the order they are presented to the user..   you can shuffle them to your hearts content
        // ****** DO NOT add UserControlHistory - the display numbers used are special and not okay for a generic panel.  Since you can do the same via a grid, its okay

        static public List<PanelInfo> PanelList = new List<PanelInfo>()
        {
            { new PanelInfo( PanelIDs.Log , typeof(UserControlLog),"Log", "Log", null, "Display the program log" ) },
            { new PanelInfo( PanelIDs.StarDistance, typeof(UserControlStarDistance), "Nearest Stars", "StarDistance", null,"Display the nearest stars to the currently selected entry") },
            { new PanelInfo( PanelIDs.Materials, typeof(UserControlMaterials) , "Materials", "Materials", null, "Display the material count at the currently selected entry" ) },
            { new PanelInfo( PanelIDs.Commodities, typeof(UserControlCommodities), "Commodities", "Commodities", null, "Display the commodity count at the currently selected entry") },
            { new PanelInfo( PanelIDs.Ledger, typeof(UserControlLedger), "Ledger", "Ledger", null, "Display a ledger of cash related entries") },
            { new PanelInfo( PanelIDs.Journal, typeof(UserControlJournalGrid), "Journal", "JournalHistory", null, "Display the journal grid view") },
            { new PanelInfo( PanelIDs.TravelGrid, typeof(UserControlTravelGrid), "Travel History", "TravelHistory", null, "Display the history grid view") },
            { new PanelInfo( PanelIDs.StarList, typeof(UserControlStarList), "Star List", "StarList", null, "Display the visited star list", transparent: false) },
            { new PanelInfo( PanelIDs.MarketData, typeof(UserControlMarketData), "Market Data", "MarketData", null, "Display Market Data (Requires Frontier Commander login)" ) },
            { new PanelInfo( PanelIDs.Missions, typeof(UserControlMissions), "Missions", "Missions", null, "Display Missions") },
            { new PanelInfo( PanelIDs.Synthesis, typeof(UserControlSynthesis), "Synthesis", "Synthesis", null, "Display Synthesis planner") },
            { new PanelInfo( PanelIDs.Engineering, typeof(UserControlEngineering), "Engineering", "Engineering", null, "Display Engineering planner") },
            { new PanelInfo( PanelIDs.ShoppingList, typeof(UserControlShoppingList), "Shopping List", "ShoppingList", null, "Shopping list of materials combining synthesis and engineering") },
            { new PanelInfo( PanelIDs.Scan, typeof(UserControlScan), "Scan", "Scan", null, "Display scan data", transparent: false) },
            { new PanelInfo( PanelIDs.EstimatedValues, typeof(UserControlEstimatedValues),"Estimated Values", "EstimatedValues", null, "Display estimated scan values bodies in system", transparent: false) },
            { new PanelInfo( PanelIDs.Modules, typeof(UserControlModules), "Loadout", "Modules", null, "Display Loadout for current ships and also stored modules") },
            { new PanelInfo( PanelIDs.Exploration, typeof(UserControlExploration), "Exploration", "Exploration", null, "Display Exploration Information") },
            { new PanelInfo( PanelIDs.ScreenShot, typeof(UserControlScreenshot), "Screen Shot", "ScreenShot", null, "Display the screen shot view") },
            { new PanelInfo( PanelIDs.Statistics, typeof(UserControlStats), "Statistics", "Stats", null, "Display statistics from the history") },
            { new PanelInfo( PanelIDs.SystemInformation, typeof(UserControlSysInfo), "System Information", "SystemInfo", null, "Display System Information" , transparent:false ) },
            { new PanelInfo( PanelIDs.EDSM, typeof(UserControlEDSM), "EDSM Star Finder", "EDSMStarFinder", null, "Display the EDSM Star finder") },
            { new PanelInfo( PanelIDs.Route, typeof(UserControlRoute), "Route Finder", "RouteFinder", null, "Find Routes from star data") },
            { new PanelInfo( PanelIDs.Expedition, typeof(UserControlExpedition), "Expedition", "Expedition", null, "Plan an Expedition") },
            { new PanelInfo( PanelIDs.Trilateration, typeof(UserControlTrilateration) ,"Trilateration", "Trilateration", null, "Trilateration of stars with unknown positions") },
            { new PanelInfo( PanelIDs.Spanel, typeof(UserControlSpanel), "Summary Panel", "Spanel", null, "Display the summary panel overlay" , transparent: false ) },
            { new PanelInfo( PanelIDs.Trippanel, typeof(UserControlTrippanel), "Trip Computer", "Trippanel", null, "Display the trip computer overlay" , transparent: false) },
            { new PanelInfo( PanelIDs.NotePanel, typeof(UserControlNotePanel), "Notes", "NotePanel", null, "Display current notes on a system overlay" , transparent: false) },
            { new PanelInfo( PanelIDs.RouteTracker, typeof(UserControlRouteTracker),"Route Tracker", "RouteTracker", null, "Display the route tracker overlay", transparent: false) },
            { new PanelInfo( PanelIDs.Settings, typeof(UserControlSettings), "Settings", "SettingsPanel", null, "Display the settings panel") },
            { new PanelInfo( PanelIDs.Grid, typeof(UserControlContainerGrid), "Grid", "TheGrid", null, "Display the grid which allows other panels to be placed on it" , transparent:false) },
        };

        public class PanelInfo
        {
            protected Image tabicon;
            public PanelIDs PopoutID;
            public Type PopoutType;
            public string WindowTitlePrefix;
            public string WindowRefName;
            public Image TabIcon { get { return tabicon ?? EDDIconSet.Instance.PanelTypeIcons[PopoutID]; } set { tabicon = value; } }
            public string Tooltip;
            public bool SupportsTransparency;
            public bool DefaultTransparent;

            public PanelInfo(PanelIDs p, Type t, string prefix, string rf, Image icon, string tooltip, bool? transparent = null)
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

        static public int GetPanelIndexByName(string name)
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
        public UserControlForm Get(string name) { return usercontrolsforms.Get(name); }
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
