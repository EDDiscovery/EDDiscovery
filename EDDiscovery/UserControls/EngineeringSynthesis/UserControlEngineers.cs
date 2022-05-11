/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    public partial class UserControlEngineers : UserControlCommonBase
    {
        private EngineerStatusPanel[] engineerpanel;
        private bool isHistoric = false;
        private HistoryEntry last_he = null;

        private string dbHistoricMatsSave = "GridHistoricMaterials";

        public UserControlEngineers()
        {
            InitializeComponent();
            DBBaseName = "Engineers";
        }

        public override void Init()
        {
            isHistoric = GetSetting(dbHistoricMatsSave, false);

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= UCTGChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += UCTGChanged;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += UCTGChanged;
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= UCTGChanged;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;

            PutSetting(dbHistoricMatsSave, isHistoric);
        }

        internal void SetHistoric(bool newVal)
        {
            isHistoric = newVal;
            if (isHistoric)
            {
                last_he = uctg.GetCurrentHistoryEntry;
            }
            else
            {
                last_he = discoveryform.history.GetLast;
            }
            UpdateDisplay();
        }

        public override void InitialDisplay()
        {
            SetupDisplay();
            last_he = isHistoric ? uctg.GetCurrentHistoryEntry : discoveryform.history.GetLast;
            UpdateDisplay();
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            last_he = isHistoric ? uctg.GetCurrentHistoryEntry : discoveryform.history.GetLast;
            UpdateDisplay();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            if (!isHistoric)        // only track new items if not historic
            {
                last_he = he;
                if (he.journalEntry is ICommodityJournalEntry || he.journalEntry is IMaterialJournalEntry)
                {
                    UpdateDisplay();
                }
            }
        }

        private void UCTGChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            if (isHistoric || last_he == null)
            {
                last_he = he;
                UpdateDisplay();
            }
        }

        public void SetupDisplay()
        {
            List<string> engineers = Recipes.EngineeringRecipes.SelectMany(r => r.engineers).Distinct().ToList();
            engineers.Sort();
            engineerpanel = new EngineerStatusPanel[engineers.Count];

            panelEngineers.SuspendLayout();

            int panelvspacing = 210;
            int vpos = 0;
            int index = 0;
            foreach (var e in engineers)
            {
                engineerpanel[index] = new EngineerStatusPanel();
                engineerpanel[index].Name = e;
                ItemData.EngineeringInfo ei = ItemData.GetEngineerInfo(e);
                //System.Diagnostics.Debug.WriteLine($"Engineers {e}");

                engineerpanel[index].Init(e, ei?.StarSystem ?? "", ei?.BaseName ?? "", ei?.Planet ?? "", ei);
                panelEngineers.Controls.Add(engineerpanel[index]);
                engineerpanel[index].Bounds = new Rectangle(0, vpos, panelEngineers.Width, panelvspacing);

                vpos += panelvspacing + 4;
                index++;

            }

            panelEngineers.ResumeLayout();

        }

        // last_he is the position, may be null, present if null
        public void UpdateDisplay()
        {
            var lastengprog = discoveryform.history.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.EngineerProgress, last_he); // may be null
            var system = last_he?.System;       // may be null

            for (int i = 0; i < engineerpanel.Length; i++)
            {
                string e = engineerpanel[i].Name;
                string status = "";

                if (lastengprog != null && engineerpanel[i].EngineerInfo != null)
                {
                    var state = (lastengprog.journalEntry as EliteDangerousCore.JournalEvents.JournalEngineerProgress).Progress(e);
                    if (state == EliteDangerousCore.JournalEvents.JournalEngineerProgress.InviteState.UnknownEngineer)
                        state = EliteDangerousCore.JournalEvents.JournalEngineerProgress.InviteState.None;      // frontier are not telling, presume none
                    status = state.ToString();
                }

                engineerpanel[i].UpdateStatus(status,system);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if ( engineerpanel != null )
            {
                for (int i = 0; i < engineerpanel.Length; i++)
                {
                    engineerpanel[i].Width = panelEngineers.Width;
                }
            }
        }
    }
}



//if (ei != null)
//{
//    var sys = EliteDangerousCore.DB.SystemCache.FindSystem(ei.StarSystem);
//    //System.Diagnostics.Debug.WriteLine($"Engineer {e} at {sys.X} {sys.Y} {sys.Z}");
//    System.Diagnostics.Debug.WriteLine(
//        $"{{ \"{e}\", new EngineeringInfo( \"{e}\", \"{ei.StarSystem}\",\"{ei.BaseName}\",\r\n" +
//        $"    {sys.X},{sys.Y},{sys.Z},\"\",\r\n" +
//        $"    \"{ei.DiscoveryRequirements}\",\r\n" +
//        $"    \"{ei.MeetingRequirements}\",\r\n" +
//        $"    \"{ei.UnlockRequirements}\",\r\n" +
//        $"    \"{ei.ReputationGain}\",\r\n" +
//        $"    {ei.PermitRequired},{ei.OdysseyEnginner}) }},\r\n");


//    // string url = Properties.Resources.URLEDDBSystemName + System.Web.HttpUtility.UrlEncode(sys.Name);
//    // BaseUtils.BrowserInfo.LaunchBrowser(url);

//}
//else
//{
//    System.Diagnostics.Debug.WriteLine($"!!!! Unknown {e}");
//}