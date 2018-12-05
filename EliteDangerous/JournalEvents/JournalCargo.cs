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
using System.Linq;

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
            Vessel = evt["Vessel"].Str("Ship");         // ship is the default.. present in 3.3 only.  Other value SRV

            Inventory = evt["Inventory"]?.ToObjectProtected<Cargo[]>().OrderBy(x => x.Name)?.ToArray();

            if (Inventory != null)
            {
                foreach (Cargo c in Inventory)
                    c.Normalise();
            }
        }

        public bool ReadAdditionalFiles(string directory, bool historyrefreshparse, ref JObject jo)
        {
            if (Inventory == null)  // so, if cargo contained info, we use that.. else we try for cargo.json.
            {
                System.Diagnostics.Debug.WriteLine("Cargo with no data, checking file.." + historyrefreshparse);

                JObject jnew = ReadAdditionalFile(System.IO.Path.Combine(directory, "Cargo.json"), waitforfile: !historyrefreshparse, checktimestamptype: true);  // check timestamp..
                if (jnew != null)        // new json, rescan. returns null if cargo in the folder is not related to this entry by time.
                {
                    jo = jnew;      // replace current
                    Rescan(jo);
                }
                return jnew != null;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Cargo with with data, no need to check file.." + historyrefreshparse);
                return true;
            }
        }

        public string Vessel { get; set; }          // always set, Ship or SRV.
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

                info = Vessel.Equals("Ship") ? "Ship".Txb(this,"CargoShip") : "SRV".Txb(this,"CargoSRV");
                info += " ";

                info += string.Format( "Cargo, {0} items".Txb(this), total);
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
            if (Vessel.Equals("Ship"))      // only want ship cargo to change lists..
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

    [JournalEntryType(JournalTypeEnum.CargoDepot)]
    public class JournalCargoDepot : JournalEntry, IMaterialCommodityJournalEntry
    {
        public JournalCargoDepot(JObject evt) : base(evt, JournalTypeEnum.CargoDepot)
        {
            MissionId = evt["MissionID"].Int();
            UpdateType = evt["UpdateType"].Str();        // must be FD name
            System.Enum.TryParse<UpdateTypeEnum>(UpdateType, out UpdateTypeEnum u);
            UpdateEnum = u;
            CargoType = evt["CargoType"].Str();     // item counts
            FriendlyCargoType = MaterialCommodityData.GetNameByFDName(CargoType);
            Count = evt["Count"].Int(0);
            StartMarketID = evt["StartMarketID"].Long();
            EndMarketID = evt["EndMarketID"].Long();
            ItemsCollected = evt["ItemsCollected"].Int();
            ItemsDelivered = evt["ItemsDelivered"].Int();
            TotalItemsToDeliver = evt["TotalItemsToDeliver"].Int();
            ItemsToGo = TotalItemsToDeliver - ItemsDelivered;
            ProgressPercent = evt["Progress"].Double() * 100;
            MarketID = evt["MarketID"].LongNull();

            if (ProgressPercent < 0.01)
                ProgressPercent = ((double)System.Math.Max(ItemsCollected, ItemsDelivered) / (double)TotalItemsToDeliver) * 100;
        }

        public enum UpdateTypeEnum { Unknown, Collect, Deliver, WingUpdate }

        public int MissionId { get; set; }
        public string UpdateType { get; set; }
        public UpdateTypeEnum UpdateEnum { get; set; }

        public string CargoType { get; set; } // 3.03       deliver/collect only    - what you have done now.  Blank if not known (<3.03)
        public string FriendlyCargoType { get; set; }
        public int Count { get; set; }  // 3.03         deliver/collect only.  0 if not known.

        public long StartMarketID { get; set; }
        public long EndMarketID { get; set; }

        public int ItemsCollected { get; set; }             // current total stats
        public int ItemsDelivered { get; set; }
        public int ItemsToGo { get; set; }
        public int TotalItemsToDeliver { get; set; }
        public double ProgressPercent { get; set; }

        public long? MarketID { get; set; }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (CargoType.Length > 0 && Count > 0)
                mc.Change(MaterialCommodityData.CommodityCategory, CargoType, (UpdateEnum == UpdateTypeEnum.Collect) ? Count : -Count, 0, conn);
        }

        public override void FillInformation(out string info, out string detailed)
        {

            if (UpdateEnum == UpdateTypeEnum.Collect)
            {
                info = BaseUtils.FieldBuilder.Build("Collected:".Txb(this), Count, "< of ".Txb(this), FriendlyCargoType, "Total:".Txb(this), ItemsDelivered, "To Go:", ItemsToGo, "Progress:;%;N1".Txb(this), ProgressPercent);
            }
            else if (UpdateEnum == UpdateTypeEnum.Deliver)
            {
                info = BaseUtils.FieldBuilder.Build("Delivered:".Txb(this), Count, "< of ".Txb(this), FriendlyCargoType, "Total:".Txb(this), ItemsDelivered, "To Go:", ItemsToGo, "Progress:;%;N1".Txb(this), ProgressPercent);
            }
            else if (UpdateEnum == UpdateTypeEnum.WingUpdate)
            {
                info = BaseUtils.FieldBuilder.Build("Update, Collected:".Txb(this), ItemsCollected, "Delivered:".Txb(this), ItemsDelivered, "To Go:".Txb(this), ItemsToGo, "Progress Left:;%;N1".Txb(this), ProgressPercent);
            }
            else
            {
                info = "Unknown CargoDepot type " + UpdateType;
            }

            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.CollectCargo)]
    public class JournalCollectCargo : JournalEntry, IMaterialCommodityJournalEntry, ILedgerNoCashJournalEntry
    {
        public JournalCollectCargo(JObject evt) : base(evt, JournalTypeEnum.CollectCargo)
        {
            Type = evt["Type"].Str();                               //FDNAME
            Type = JournalFieldNaming.FDNameTranslation(Type);     // pre-mangle to latest names, in case we are reading old journal records
            FriendlyType = MaterialCommodityData.GetNameByFDName(Type);
            Type_Localised = JournalFieldNaming.CheckLocalisationTranslation(evt["Type_Localised"].Str(), FriendlyType);         // always ensure we have one
            Stolen = evt["Stolen"].Bool();
            MissionID = evt["MissionID"].LongNull();
        }

        public string Type { get; set; }                    // FDNAME..
        public string FriendlyType { get; set; }            // translated name
        public string Type_Localised { get; set; }            // always set
        public bool Stolen { get; set; }
        public long? MissionID { get; set; }             // if applicable

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(MaterialCommodityData.CommodityCategory, Type, 1, 0, conn);
        }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, FriendlyType);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Type_Localised, ";Stolen".Txb(this), Stolen, "<; (Mission Cargo)".Txb(this), MissionID != null);
            detailed = "";
        }
    }


}
