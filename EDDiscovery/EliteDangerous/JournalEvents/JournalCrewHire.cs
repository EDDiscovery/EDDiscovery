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
    //When Written: when engaging a new member of crew
    //Parameters:
    //•	Name
    //•	Faction
    //•	Cost
    //•	CombatRank

    public class JournalCrewHire : JournalEntry
    {
        public JournalCrewHire(JObject evt) : base(evt, JournalTypeEnum.CrewHire)
        {
            Name = JSONHelper.GetStringDef(evt["Name"]);
            Faction = JSONHelper.GetStringDef(evt["Faction"]);
            Cost = JSONHelper.GetLong(evt["Cost"]);
            CombatRank = (CombatRank)JSONHelper.GetInt(evt["CombatRank"]);
        }
        public string Name { get; set; }
        public string Faction { get; set; }
        public long Cost { get; set; }
        public CombatRank CombatRank { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.crew; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Name + " " + Faction, -Cost);
        }

    }
}
