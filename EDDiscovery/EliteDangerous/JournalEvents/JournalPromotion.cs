/*
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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{

    public class JournalPromotion : JournalEntry
    {
        //        When written: when the player’s rank increases
        //Parameters: one of the following
        //•	Combat: new rank
        //•	Trade: new rank
        //•	Explore: new rank
        //•	CQC: new rank

        public JournalPromotion(JObject evt) : base(evt, JournalTypeEnum.Promotion)
        {
            Combat = (CombatRank)JSONHelper.GetInt(evt["Combat"]);
            Trade = (TradeRank)JSONHelper.GetInt(evt["Trade"]);
            Explore = (ExplorationRank)JSONHelper.GetInt(evt["Explore"]);
            CQC = (CQCRank)JSONHelper.GetInt(evt["CQC"]);
        }
        public CombatRank Combat { get; set; }
        public TradeRank Trade { get; set; }
        public ExplorationRank Explore { get; set; }
        public CQCRank CQC { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.promotion; } }
    }
}
