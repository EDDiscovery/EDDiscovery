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
    //When Written: whenever materials are collected 
    //Parameters: 
    //•	Category: type of material (Raw/Encoded/Manufactured)
    //•	Name: name of material
    [JournalEntryType(JournalTypeEnum.MaterialCollected)]
    public class JournalMaterialCollected : JournalEntry, IMaterialCommodityJournalEntry
    {
        public JournalMaterialCollected(JObject evt ) : base(evt, JournalTypeEnum.MaterialCollected)
        {
            Category = JSONHelper.GetStringDef(evt["Category"]);
            Name = JSONHelper.GetStringDef(evt["Name"]);
            Count = JSONHelper.GetInt(evt["Count"], 1);
        }
        public string Category { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.materialcollected; } }

        public void MaterialList(EDDiscovery2.DB.MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(Category, Name, Count, 0, conn);
        }
        
    }
}
