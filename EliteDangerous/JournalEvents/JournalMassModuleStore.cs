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
//    When written: when putting multiple modules into storage
//Parameters:
//•	Ship
//•	ShipId
//•	Items: Array of records
//o   Slot
//o   Name
//o   EngineerModifications(only present if modified)

    [JournalEntryType(JournalTypeEnum.MassModuleStore)]
    public class JournalMassModuleStore : JournalEntry, IShipInformation
    {
        public JournalMassModuleStore(JObject evt) : base(evt, JournalTypeEnum.MassModuleStore)
        {
            ShipFD = evt["Ship"].Str();
            Ship = JournalFieldNaming.GetBetterShipName(evt["Ship"].Str());
            ShipId = evt["ShipID"].Int();
            ModuleItems = evt["Items"]?.ToObjectProtected<ModuleItem[]>();
            MarketID = evt["MarketID"].LongNull();

            if ( ModuleItems != null )
            {
                foreach (ModuleItem i in ModuleItems)
                {
                    i.Slot = JournalFieldNaming.GetBetterSlotName(i.Slot);
                    i.Name = JournalFieldNaming.GetBetterItemNameEvents(i.Name);
                }
            }
        }

        public string Ship { get; set; }
        public string ShipFD { get; set; }
        public int ShipId { get; set; }
        public long? MarketID { get; set; }

        public ModuleItem[] ModuleItems { get; set; }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.MassModuleStore(this);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Total modules:", ModuleItems?.Count());
            detailed = "";

            if ( ModuleItems != null )
                foreach (ModuleItem m in ModuleItems)
                {
                    detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build("", m.Name, ";(Hot)", m.Hot), ", ");
                }
                
        }

        public class ModuleItem
        {
            public string Slot;
            public string Name;
            public string EngineerModifications;
            public double? Quality { get; set; }
            public int? Level { get; set; }
            public bool? Hot { get; set; }
        }
    }
}
