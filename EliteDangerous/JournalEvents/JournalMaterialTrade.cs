/*
 * Copyright © 2016-2018 EDDiscovery development team
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

            Paid = evt["Paid"]?.ToObjectProtected<Traded>();
            if (Paid != null)
                Paid.Normalise();

            Received = evt["Received"]?.ToObjectProtected<Traded>();
            if (Received != null)
                Received.Normalise();
        }

        public string TraderType { get; set; }
        public long? MarketID { get; set; }
        public Traded Paid { get; set; }      // may be null
        public Traded Received { get; set; } // may be null

        public class Traded
        {
            public string Material;     //fdname
            public string FriendlyMaterial; // our name
            public string Material_Localised;   // their localised name if present
            public string Category;     // journal says always there.  If not, use tradertype
            public string Category_Localised;
            public int Quantity;

            public void Normalise()
            {
                Material = JournalFieldNaming.FDNameTranslation(Material);
                FriendlyMaterial = MaterialCommodityData.GetNameByFDName(Material);
                Material_Localised = JournalFieldNaming.CheckLocalisationTranslation(Material_Localised??"",FriendlyMaterial);       // ensure.

                if (Category != null)       // some entries do not have this
                {
                    Category = JournalFieldNaming.NormaliseMaterialCategory(Category);  // fix up any strangeness
                    Category_Localised = JournalFieldNaming.CheckLocalisation(Category_Localised??"",Category);
                }
            }
        }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Paid != null && Received != null)
            {
                mc.Change(Paid.Category.Alt(TraderType), Paid.Material, -Paid.Quantity, 0, conn);
                mc.Change(Received.Category.Alt(TraderType), Received.Material, Received.Quantity, 0, conn );
            }
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = detailed = "";

            if (Paid != null && Received != null)
            {
                info = BaseUtils.FieldBuilder.Build("Sold:".Txb(this), Paid.Quantity, "< ", Paid.Material_Localised,
                                                    "Received:".Txb(this), Received.Quantity, "< ", Received.Material_Localised);
            }
        }
    }
}
