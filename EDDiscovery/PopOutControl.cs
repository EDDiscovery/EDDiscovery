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
using EDDiscovery.DB;
using EDDiscovery.UserControls;
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
        public PopOuts? GetPopOutTypeByName(string name)
        {
            foreach (KeyValuePair<PopOuts, PopOutInfo> k in popoutinfo)
            {
                if (k.Value.WindowRefName.Equals(name,StringComparison.InvariantCultureIgnoreCase))
                    return k.Key;
            }
            return null;
        }

        public enum PopOuts        // id's.. used in tab controls, and in button pop outs button
        {
            Spanel,
            Trippanel,
            NotePanel,
            RouteTracker,
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

            StartTabButtons = Log,
        };

        public class PopOutInfo
        {
            public string WindowTitlePrefix;
            public string WindowRefName;
            public Bitmap TabIcon;
            public string ToolTip;
            public bool SupportsTransparency;
            public bool DefaultTransparent;

            public PopOutInfo()
            {
                WindowTitlePrefix = "";
                WindowRefName = "Unknown";
            }

            public PopOutInfo(string prefix, string rf, Bitmap icon = null, string tooltip = null, bool? transparent = null)
            {
                WindowTitlePrefix = prefix;
                WindowRefName = rf;
                TabIcon = icon;
                ToolTip = tooltip;
                SupportsTransparency = transparent != null;
                DefaultTransparent = transparent ?? false;
            }
        }

        static public Dictionary<PopOuts, PopOutInfo> popoutinfo = new Dictionary<PopOuts, PopOutInfo>
        {
            { PopOuts.Log, new PopOutInfo("Log", "Log", EDDiscovery.Properties.Resources.Log, "Display the program log") },
            { PopOuts.StarDistance, new PopOutInfo("Nearest Stars", "StarDistance", EDDiscovery.Properties.Resources.star, "Display the nearest stars to the currently selected entry") },
            { PopOuts.Materials, new PopOutInfo("Materials", "Materials", EDDiscovery.Properties.Resources.material, "Display the material count at the currently selected entry") },
            { PopOuts.Commodities, new PopOutInfo("Commodities", "Commodities", EDDiscovery.Properties.Resources.commodities, "Display the commodity count at the currently selected entry") },
            { PopOuts.Ledger, new PopOutInfo("Ledger", "Ledger", EDDiscovery.Properties.Resources.ledger, "Display a ledger of cash related entries") },
            { PopOuts.Journal, new PopOutInfo("Journal History", "JournalHistory", EDDiscovery.Properties.Resources.journal, "Display the journal grid view") },
            { PopOuts.TravelGrid, new PopOutInfo("Travel History", "TravelHistory", EDDiscovery.Properties.Resources.travelgrid, "Display the history grid view") },
            { PopOuts.ScreenShot, new PopOutInfo("ScreenShot", "ScreenShot", EDDiscovery.Properties.Resources.screenshot, "Display the screen shot view") },
            { PopOuts.Statistics, new PopOutInfo("Statistics", "Stats", EDDiscovery.Properties.Resources.stats, "Display statistics from the history") },
            { PopOuts.Scan, new PopOutInfo("Scan", "Scan", EDDiscovery.Properties.Resources.scan, "Display scan data", transparent: false) },
            { PopOuts.Modules, new PopOutInfo("Loadout", "Modules", EDDiscovery.Properties.Resources.module, "Display loadout data") },
            { PopOuts.Synthesis, new PopOutInfo("Synthesis", "Synthesis", EDDiscovery.Properties.Resources.synthesis, "Synthesis Planner") },
            { PopOuts.Exploration, new PopOutInfo("Exploration", "Exploration", null, "Explore a collection of systems") },
            { PopOuts.Spanel, new PopOutInfo("Summary Panel", "Spanel", transparent: true) },
            { PopOuts.Trippanel, new PopOutInfo("Trip Panel", "Trippanel", transparent: true) },
            { PopOuts.NotePanel, new PopOutInfo("Note Panel", "NotePanel", transparent: true) },
            { PopOuts.RouteTracker, new PopOutInfo("Route Tracker", "RouteTracker", transparent: true) },
        };

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
                case PopOuts.Modules: return new UserControlModules();
                case PopOuts.Exploration: return new UserControlExploration();
                case PopOuts.Synthesis: return new UserControlSynthesis();
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
            UserControlForm tcf = usercontrolsforms.NewForm(EDDConfig.Options.NoWindowReposition);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EDDiscovery.EDDiscoveryForm));
            tcf.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));

            UserControlCommonBase ctrl = Create(selected);
            int numopened = ctrl == null ? 0 : usercontrolsforms.CountOf(ctrl.GetType()) + 1;

            if (ctrl != null)
            {
                PopOutInfo poi = popoutinfo.ContainsKey(selected) ? popoutinfo[selected] : new PopOutInfo();
                string windowtitle = poi.WindowTitlePrefix + " " + ((numopened > 1) ? numopened.ToString() : "");
                string refname = poi.WindowRefName + numopened.ToString();
                tcf.Init(ctrl, windowtitle, _discoveryForm.theme.WindowsFrame, refname, EDDConfig.Instance.KeepOnTop);
                if (poi.SupportsTransparency)
                {
                    tcf.InitForTransparency(poi.DefaultTransparent, _discoveryForm.theme.LabelColor, _discoveryForm.theme.SPanelColor);
                }

                _discoveryForm.TravelControl.UserControlPostCreate(numopened, ctrl);        // YUK YUK YUK wire up to some internals.. at some point this needs sorting out
                tcf.Show();

                if (tcf.UserControl != null)
                    tcf.UserControl.Font = _discoveryForm.theme.GetFont;        // Important. Apply font autoscaling to the user control
                                                                                // ApplyToForm does not apply the font to the actual UC, only
                                                                                // specific children controls.  The TabControl in the discoveryform ends up autoscaling most stuff
                                                                                // the children directly attached to the discoveryform are not autoscaled

                _discoveryForm.theme.ApplyToForm(tcf);

                ctrl.Display(_discoveryForm.TravelControl.GetTravelHistoryCurrent, _discoveryForm.history);

                _discoveryForm.ActionRun("onPopUp", "UserUIEvent", null, new ConditionVariables(new string[] { "PopOutName", refname , "PopOutTitle", windowtitle, "PopOutIndex", numopened.ToString()} ));
            }
        }

        public void SetRefreshState(bool state)
        {
            foreach (UserControlCommonBase uc in usercontrolsforms.GetListOfControls(typeof(UserControlJournalGrid)))
                ((UserControlJournalGrid)uc).RefreshButton(state);      // and the journal views need it
        }

        public void UpdateNoteJID(long jid, string text)
        {
            foreach (UserControlCommonBase uc in usercontrolsforms.GetListOfControls(typeof(UserControlTravelGrid)))
                ((UserControlTravelGrid)uc).UpdateNoteJID(jid, text);
        }

    }
}
