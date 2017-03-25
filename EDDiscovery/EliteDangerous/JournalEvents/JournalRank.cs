﻿/*
 * Copyright © 2016 EDDiscovery development team
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
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: at startup
    //Parameters:
    //•	Combat: rank on scale 0-8
    //•	Trade: rank on scale 0-8
    //•	Explore: rank on scale 0-8
    //•	Empire: military rank
    //•	Federation: military rank
    //•	CQC: rank on scale 0-8

    //Ranks
    //Combat ranks: 0='Harmless', 1='Mostly Harmless', 2='Novice', 3='Competent', 4='Expert', 5='Master', 6='Dangerous', 7='Deadly', 8='Elite’
    //Trade ranks: 0='Penniless', 1='Mostly Pennliess', 2='Peddler', 3='Dealer', 4='Merchant', 5='Broker', 6='Entrepreneur', 7='Tycoon', 8='Elite'
    //Exploration ranks: 0='Aimless', 1='Mostly Aimless', 2='Scout', 3='Surveyor', 4='Explorer', 5='Pathfinder', 6='Ranger', 7='Pioneer', 8='Elite'
    //Federation ranks: 0='None', 1='Recruit', 2='Cadet', 3='Midshipman', 4='Petty Officer', 5='Chief Petty Officer', 6='Warrant Officer', 7='Ensign', 8='Lieutenant', 9='Lt. Commander', 10='Post Commander', 11= 'Post Captain', 12= 'Rear Admiral', 13='Vice Admiral', 14=’Admiral’
    //Empire ranks: 0='None', 1='Outsider', 2='Serf', 3='Master', 4='Squire', 5='Knight', 6='Lord', 7='Baron',  8='Viscount ', 9=’Count', 10= 'Earl', 11='Marquis' 12='Duke', 13='Prince', 14=’King’
    //CQC ranks: 0=’Helpless’, 1=’Mostly Helpless’, 2=’Amateur’, 3=’Semi Professional’, 4=’Professional’, 5=’Champion’, 6=’Hero’, 7=’Legend’, 8=’Elite’
    [JournalEntryType(JournalTypeEnum.Rank)]
    public class JournalRank : JournalEntry
    {

        public JournalRank(JObject evt ) : base(evt, JournalTypeEnum.Rank)
        {
            Combat = (CombatRank)evt["Combat"].Int();
            Trade = (TradeRank)evt["Trade"].Int();
            Explore = (ExplorationRank)evt["Explore"].Int();
            Empire = (EmpireRank)evt["Empire"].Int();
            Federation = (FederationRank)evt["Federation"].Int();
            CQC = (CQCRank)evt["CQC"].Int();

        }
        public CombatRank Combat { get; set; }
        public TradeRank Trade { get; set; }
        public ExplorationRank Explore { get; set; }
        public EmpireRank Empire { get; set; }
        public FederationRank Federation { get; set; }
        public CQCRank CQC { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.rank; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("", Combat.ToString().SplitCapsWord(),
                                      "", Trade.ToString().SplitCapsWord(),
                                      "", Explore.ToString().SplitCapsWord(),
                                      "", Federation.ToString().SplitCapsWord(),
                                      "", Empire.ToString().SplitCapsWord(),
                                      "", CQC.ToString().SplitCapsWord());
            detailed = "";
        }
    }
}
