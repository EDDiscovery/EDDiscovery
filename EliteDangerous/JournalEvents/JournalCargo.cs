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
    public class JournalCargo : JournalEntry, IMaterialCommodityJournalEntry, IAdditionalFiles
    {
        public class Cargo
        {
            public string Name { get; set; }            // FDNAME
            public string FriendlyName { get; set; }            // FDNAME
            public int Count { get; set; }
            public int Stolen { get; set; }
            public long? MissionID { get; set; }             // if applicable

            public void Normalise()
            {
                Name = JournalFieldNaming.FDNameTranslation(Name);
                FriendlyName = MaterialCommodityData.GetNameByFDName(Name);
            }
        }

        public JournalCargo(JObject evt) : base(evt, JournalTypeEnum.Cargo)
        {
            //System.Diagnostics.Debug.WriteLine("Cargo at " + EventTimeUTC);
            Rescan(evt);
        }

        void Rescan(JObject evt)
        { 
            Inventory = evt["Inventory"]?.ToObjectProtected<Cargo[]>().OrderBy(x => x.Name)?.ToArray();
            if (Inventory != null)
            {
                foreach (Cargo c in Inventory)
                    c.Normalise();
            }
        }

        public bool ReadAdditionalFiles(string directory, bool historyrefreshparse, ref JObject jo)
        {
            JObject jnew = ReadAdditionalFile(System.IO.Path.Combine(directory, "Cargo.json"), waitforfile: !historyrefreshparse, checktimestamptype: true);  // check timestamp..
            if (jnew != null)        // new json, rescan. returns null if cargo in the folder is not related to this entry by time.
            {
                jo = jnew;      // replace current
                Rescan(jo);
            }
            return jnew != null;
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
                    detailed += BaseUtils.FieldBuilder.Build("", c.FriendlyName, "; items".Txb(this), c.Count , "(;)" , stolen, "<; (Mission Cargo)".Txb(this), c.MissionID != null);
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
                    mc.Set(MaterialCommodityData.CommodityCategory, c.Name, c.Count, 0, conn);
            }
        }
    }


    [JournalEntryType(JournalTypeEnum.EjectCargo)]
    public class JournalEjectCargo : JournalEntry, IMaterialCommodityJournalEntry, ILedgerNoCashJournalEntry
    {
        public JournalEjectCargo(JObject evt) : base(evt, JournalTypeEnum.EjectCargo)
        {
            Type = evt["Type"].Str();       // fdname
            Type = JournalFieldNaming.FDNameTranslation(Type);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyType = MaterialCommodityData.GetNameByFDName(Type);
            Type_Localised = JournalFieldNaming.CheckLocalisationTranslation(evt["Type_Localised"].Str(), FriendlyType);         // always ensure we have one

            Count = evt["Count"].Int();
            Abandoned = evt["Abandoned"].Bool();
            PowerplayOrigin = evt["PowerplayOrigin"].Str();
            MissionID = evt["MissionID"].LongNull();
        }

        public string Type { get; set; }                    // FDName
        public string FriendlyType { get; set; }            // translated name
        public string Type_Localised { get; set; }            // always set

        public int Count { get; set; }
        public bool Abandoned { get; set; }
        public string PowerplayOrigin { get; set; }
        public long? MissionID { get; set; }             // if applicable

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(MaterialCommodityData.CommodityCategory, Type, -Count, 0, conn);
        }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, FriendlyType + " " + Count);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Type_Localised, "Count:".Txb(this), Count,
                            "<; (Mission Cargo)".Txb(this), MissionID != null,
                            ";Abandoned".Txb(this), Abandoned, "PowerPlay:".Txb(this), PowerplayOrigin);
            detailed = "";
        }
    }

}
