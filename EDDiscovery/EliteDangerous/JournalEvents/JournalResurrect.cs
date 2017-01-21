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
    //When written: when the player restarts after death
    //Parameters:
    //•	Option: the option selected on the insurance rebuy screen
    //•	Cost: the price paid
    //•	Bankrupt: whether the commander declared bankruptcy
    public class JournalResurrect : JournalEntry
    {
        public JournalResurrect(JObject evt ) : base(evt, JournalTypeEnum.Resurrect)
        {
            Option = JSONHelper.GetStringDef(evt["Option"]);
            Cost = JSONHelper.GetLong(evt["Cost"]);
            Bankrupt = JSONHelper.GetBool(evt["Bankrupt"]);

        }
        public string Option { get; set; }
        public long Cost { get; set; }
        public bool Bankrupt { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.ressurect; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Option, -Cost);
        }

    }
}
