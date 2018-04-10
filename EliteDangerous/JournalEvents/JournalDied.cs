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
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    //When written: player was killed
    //Parameters:
    //•	KillerName
    //•	KillerShip
    //•	KillerRank
    //When written: player was killed by a wing
    //Parameters:
    //•	Killers: a JSON array of objects containing player name, ship, and rank
    [JournalEntryType(JournalTypeEnum.Died)]
    public class JournalDied : JournalEntry, IMissions, IMaterialCommodityJournalEntry
    {
        public class Killer
        {
            public string Name;
            public string Name_Localised;
            public string Ship;
            public string Rank;
        }

        public JournalDied(JObject evt ) : base(evt, JournalTypeEnum.Died)
        {
            string killerName = evt["KillerName"].Str();
            if (string.IsNullOrEmpty(killerName))
            {
                if (evt["Killers"] != null)
                    Killers = evt["Killers"].ToObjectProtected<Killer[]>();
            }
            else
            {
                // it was an individual
                Killers = new Killer[1]
                {
                        new Killer
                        {
                            Name = killerName,
                            Name_Localised = evt["KillerName_Localised"].Str(),
                            Ship = evt["KillerShip"].Str(),
                            Rank = evt["KillerRank"].Str()
                        }
                };
            }

            if (Killers != null)
            {
                foreach (Killer k in Killers)
                {
                    k.Ship = JournalFieldNaming.GetBetterShipName(k.Ship);
                    k.Name_Localised = k.Name_Localised.Alt(k.Name);
                }
            }
        }

        public Killer[] Killers { get; set; }

        public void UpdateMissions(MissionListAccumulator mlist, EliteDangerousCore.ISystem sys, string body, DB.SQLiteConnectionUser conn)
        {
            mlist.Died(this.EventTimeUTC);
        }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Died();
        }


        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";
            if (Killers != null)
            {
                info = "Killed by ";
                bool comma = false;

                foreach (Killer k in Killers)
                {
                    if (comma)
                        info += ", ";
                    comma = true;
                    info += k.Name_Localised + " in ship type " + k.Ship + " rank " + k.Rank;
                }
            }

            detailed = "";
        }

    }
}
