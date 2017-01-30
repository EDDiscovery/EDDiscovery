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
    //When written: player was killed
    //Parameters: 
    //•	KillerName
    //•	KillerShip
    //•	KillerRank
    //When written: player was killed by a wing
    //Parameters:
    //•	Killers: a JSON array of objects containing player name, ship, and rank
    public class JournalDied : JournalEntry
    {
        public class Killer
        {
            public string Name;
            public string Ship;
            public string Rank;
        }

        public JournalDied(JObject evt ) : base(evt, JournalTypeEnum.Died)
        {
            string killerName = JSONHelper.GetStringDef(evt["KillerName"]);
            if (string.IsNullOrEmpty(killerName))
            {
                if (evt["Killers"]!=null)
                    Killers = evt["Killers"].ToObject<Killer[]>();
            }
            else
            {
                // it was an individual, or a suicide/accident
                Killers = new Killer[1]
                {
                        new Killer
                        {
                            Name = killerName,
                            Ship = JSONHelper.GetStringDef(evt["KillerShip"]),
                            Rank = JSONHelper.GetStringDef(evt["KillerRank"])
                        }
                };
            }

            if (Killers != null)
            {
                foreach (Killer k in Killers)
                    k.Ship = JournalEntry.GetBetterShipName(k.Ship);
            }
        }

        public Killer[] Killers { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.Coffinicon; } }

    }
}
