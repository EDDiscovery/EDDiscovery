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
//    When written: when putting multiple modules into storage
//Parameters:
//•	Ship
//•	ShipId
//•	Items: Array of records
//o   Slot
//o   Name
//o   EngineerModifications(only present if modified)

    [JournalEntryType(JournalTypeEnum.MassModuleStore)]
    public class JournalMassModuleStore : JournalEntry
    {
        public JournalMassModuleStore(JObject evt) : base(evt, JournalTypeEnum.MassModuleStore)
        {
            Ship = JournalFieldNaming.GetBetterShipName(JSONHelper.GetStringDef(evt["Ship"]));
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            ModuleItems = evt["Items"]?.ToObject<ModuleItem[]>();

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
        public int ShipId { get; set; }

        public ModuleItem[] ModuleItems { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.modulestore; } }
    }


    public class ModuleItem
    {
        public string Slot;
        public string Name;
        public string EngineerModifications;
    }

}
