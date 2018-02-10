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

    [JournalEntryType(JournalTypeEnum.MaterialTrade)]
    public class JournalMaterialTrade : JournalEntry, IMaterialCommodityJournalEntry
    {
        public JournalMaterialTrade(JObject evt) : base(evt, JournalTypeEnum.MaterialTrade)
        {
            MarketID = evt["MarketID"].LongNull();
            TraderType = evt["TraderType"].Str();

            Paid = evt["Paid"]?.ToObject<Traded>();
            Received = evt["Received"]?.ToObject<Traded>();
        }

        public string TraderType { get; set; }
        public long? MarketID { get; set; }
        public Traded Paid { get; set; }      // may be null
        public Traded Received { get; set; } // may be null

        public class Traded
        {
            public string Material;
            public string Material_Localised;
            public int Quantity;
        }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Paid != null && Received != null)
            {
// TBD experimental
                mc.Change(TraderType, JournalFieldNaming.FDNameTranslation(Paid.Material), -Paid.Quantity, 0, conn);
                mc.Change(TraderType, JournalFieldNaming.FDNameTranslation(Received.Material), Received.Quantity, 0, conn );
            }
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = detailed = "";

            if (Paid != null && Received != null)
            {
                info = BaseUtils.FieldBuilder.Build("Sold: ", Paid.Quantity, "< ", Paid.Material_Localised.Alt(Paid.Material),
                                                    "Received: ", Received.Quantity, "< ", Received.Material_Localised.Alt(Received.Material));
            }
        }
    }
}
