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

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    /*
     When written: when offering items cash or bounties to an Engineer to gain access
Parameters:
•	Engineer: name of engineer
•	Type: type of contribution (Commodity, materials, Credits, Bond, Bounty)
•	Commodity
•	Material
•	Faction (for bond or bounty)
•	Quantity: amount offered this time
•	TotalQuantity: total amount now donated

Example:
{ "timestamp":"2017-05-24T10:41:51Z", "event":"EngineerContribution", "Engineer":"Elvira Martuuk", "Type":"Commodity", "Commodity":"soontillrelics", "Quantity":2, "TotalQuantity":3 }
 
     */

    //{ "timestamp":"2017-06-27T01:32:36Z", "event":"EngineerContribution", "Engineer":"Liz Ryder", "Type":"Commodity", "Commodity":"landmines", "Quantity":200, "TotalQuantity":200 }
    //{ "timestamp":"2017-06-27T03:02:37Z", "event":"EngineerContribution", "Engineer":"Tiana Fortune", "Type":"Materials", "Material":"decodedemissiondata", "Quantity":50, "TotalQuantity":50 }

    [JournalEntryType(JournalTypeEnum.EngineerContribution)]
    public class JournalEngineerContribution : JournalEntry
    {
        public JournalEngineerContribution(JObject evt ) : base(evt, JournalTypeEnum.EngineerContribution)
        {
            Engineer = evt["Engineer"].Str();
            Type = evt["Type"].Str();

            if (Type.Equals("Commodity") || Type.Equals("Materials") || Type.Equals("Credits") ||  Type.Equals("Bond") || Type.Equals("Bounty"))
                unknownType = false;
            else
                unknownType = true;

            Commodity = evt["Commodity"].Str();
            Commodity = JournalFieldNaming.FDNameTranslation(Commodity);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyCommodity = JournalFieldNaming.RMat(Commodity);

            Material = evt["Material"].Str();
            Material = JournalFieldNaming.FDNameTranslation(Material);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyMaterial = JournalFieldNaming.RMat(Material);

            Faction = evt["Faction"].Str();

            Quantity = evt["Quantity"].Int();
            TotalQuantity = evt["TotalQuantity"].Int();
        }

        private bool unknownType;
        public string Engineer { get; set; }
        public string Type { get; set; }
        public string FriendlyCommodity { get; set; }
        public string Commodity { get; set; }
        public string FriendlyMaterial { get; set; }
        public string Material { get; set; }
        public string Faction { get; set; }
        public int Quantity { get; set; }
        public int TotalQuantity { get; set; }

        public override System.Drawing.Bitmap Icon
        {
            get
            {
                if (unknownType)
                    return EDDiscovery.Properties.Resources.genericevent;
                else
                    return EDDiscovery.Properties.Resources.engineerapply;
            }
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            if (unknownType==true)
                info = "Report to EDDiscovery team an unknown EngineerContribution type: " + Type;
             else
                info = Tools.FieldBuilder("", Engineer, "Type:", Type, "Commodity:", FriendlyCommodity, "Material:", FriendlyMaterial, "Faction:", Faction, "Quantity:", Quantity, "TotalQuantity:", TotalQuantity);
            detailed = "";
        }
    }
}
