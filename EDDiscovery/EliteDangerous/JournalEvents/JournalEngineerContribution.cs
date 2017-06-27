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
    //{ "timestamp":"2017-06-27T01:32:36Z", "event":"EngineerContribution", "Engineer":"Liz Ryder", "Type":"Commodity", "Commodity":"landmines", "Quantity":200, "TotalQuantity":200 }

    [JournalEntryType(JournalTypeEnum.EngineerContribution)]
    public class JournalEngineerContribution : JournalEntry
    {
        public JournalEngineerContribution(JObject evt ) : base(evt, JournalTypeEnum.EngineerContribution)
        {
            Engineer = evt["Engineer"].Str();
            Type = evt["Type"].Str();
            Commodity = evt["Commodity"].Str();
            Quantity = evt["Quantity"].Int();
            TotalQuantity = evt["TotalQuantity"].Int();
        }

        public string Engineer { get; set; }
        public string Type { get; set; }
        public string Commodity { get; set; }
        public int Quantity { get; set; }
        public int TotalQuantity { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.engineerapply; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("", Engineer, "Type:", Type, "Commodity:", Commodity, "Quantity:", Quantity, "TotalQuantity:", TotalQuantity);
            detailed = "";
        }
    }
}
