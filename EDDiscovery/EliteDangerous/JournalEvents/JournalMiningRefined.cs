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
    //When Written: when mining fragments are converted unto a unit of cargo by refinery
    //Parameters:
    //•	Type: $cargo_name;
    //•	Type_Localised: cargo type
    [JournalEntryType(JournalTypeEnum.MiningRefined)]
    public class JournalMiningRefined : JournalEntry, IMaterialCommodityJournalEntry, ILedgerNoCashJournalEntry
    {
        public JournalMiningRefined(JObject evt ) : base(evt, JournalTypeEnum.MiningRefined)
        {
            Type = evt["Type"].Str();

            // instances in log on mining only of $item_name; .. fix

            if (Type.Length >= 8 && Type.StartsWith("$") && Type.EndsWith("_name;", System.StringComparison.InvariantCultureIgnoreCase))
            {
                Type = Type.Substring(1, Type.Length - 7); // 1 for '$' plus 6 for '_name;'
            }

            FriendlyType = JournalFieldNaming.RMat(Type);
        }

        public string Type { get; set; }
        public string FriendlyType { get; set; }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(MaterialCommodities.CommodityCategory, Type, 1, 0, conn);
        }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, FriendlyType );
        }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.miningrefined; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = FriendlyType;
            detailed = "";
        }
    }
}
