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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.Cargo)]
    public class JournalCargo : JournalEntry, IMaterialCommodityJournalEntry
    {
        public class Cargo
        {
            public string Name { get; set; }            // FDNAME
            public string FriendlyName { get; set; }            // FDNAME
            public int Count { get; set; }
            public int Stolen { get; set; }

            public void Normalise()
            {
                Name = JournalFieldNaming.FDNameTranslation(Name);
                FriendlyName = JournalFieldNaming.RMat(Name);
            }
        }

        public JournalCargo(JObject evt) : base(evt, JournalTypeEnum.Cargo)
        {
            Inventory = evt["Inventory"]?.ToObjectProtected<Cargo[]>().OrderBy(x => x.Name)?.ToArray();
            if (Inventory != null)
            {
                foreach (Cargo c in Inventory)
                    c.Normalise();
            }
        }

        public Cargo[] Inventory { get; set; }      // may be NULL

        public override void FillInformation(out string info, out string detailed) 
        {
            
            info = "No Cargo".Txb(this);
            detailed = "";

            if (Inventory != null && Inventory.Length > 0)
            {
                int total = 0;
                foreach (Cargo c in Inventory)
                    total += c.Count;

                info = string.Format("Cargo, {0} items".Txb(this), total);
                detailed = "";

                foreach (Cargo c in Inventory)
                {
                    if (detailed.Length > 0)
                        detailed += Environment.NewLine;
                    int? stolen = null;
                    if (c.Stolen > 0)
                        stolen = c.Stolen;
                    detailed += BaseUtils.FieldBuilder.Build("", c.FriendlyName, "; items".Txb(this), c.Count , "(;)" , stolen);
                }
            }
        }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            //System.Diagnostics.Debug.WriteLine("Updated at " + this.EventTimeUTC.ToString());
            mc.Clear(true);

            if (Inventory != null)
            {
                foreach (Cargo c in Inventory)
                    mc.Set(MaterialCommodities.CommodityCategory, c.Name, c.Count, 0, conn);
            }
        }
    }
}
