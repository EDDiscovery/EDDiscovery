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
    [JournalEntryType(JournalTypeEnum.CargoDepot)]
    public class JournalCargoDepot : JournalEntry, IMaterialCommodityJournalEntry
    {
        public JournalCargoDepot(JObject evt ) : base(evt, JournalTypeEnum.CargoDepot)
        {
            MissionId = evt["MissionID"].Int();
            UpdateType = evt["UpdateType"].Str();        // must be FD name
            System.Enum.TryParse<UpdateTypeEnum>(UpdateType, out UpdateTypeEnum u);
            UpdateEnum = u;
            CargoType = evt["CargoType"].Str();     // item counts
            FriendlyCargoType = JournalFieldNaming.RMat(CargoType);
            Count = evt["Count"].Int(0);
            StartMarketID = evt["StartMarketID"].Long();
            EndMarketID = evt["EndMarketID"].Long();
            ItemsCollected = evt["ItemsCollected"].Int();
            ItemsDelivered = evt["ItemsCollected"].Int();
            TotalItemsToDeliver = evt["TotalItemsToDeliver"].Int();
            ProgressPercent = evt["Progress"].Double()*100;
            MarketID = evt["MarketID"].LongNull();
        }

        public enum UpdateTypeEnum { Unknown, Collect, Deliver, WingUpdate}

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
        public int TotalItemsToDeliver { get; set; }
        public double ProgressPercent { get; set; }

        public long? MarketID { get; set; }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if ( CargoType.Length>0 && Count>0)
                mc.Change(MaterialCommodities.CommodityCategory, CargoType, (UpdateEnum == UpdateTypeEnum.Collect) ? Count : -Count, 0, conn);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            if (UpdateEnum == UpdateTypeEnum.Collect)
            {
                info = BaseUtils.FieldBuilder.Build("Collected:", Count, "< of " , FriendlyCargoType, "Total:", ItemsDelivered, "To Go:", TotalItemsToDeliver - ItemsDelivered, "Progress:;%;N1", ProgressPercent);
            }
            else if (UpdateEnum == UpdateTypeEnum.Deliver)
            {
                info = BaseUtils.FieldBuilder.Build("Delivered:", Count, "< of ", FriendlyCargoType, "Total:", ItemsDelivered, "To Go:", TotalItemsToDeliver - ItemsDelivered, "Progress:;%;N1", ProgressPercent);
            }
            else if (UpdateEnum == UpdateTypeEnum.WingUpdate)
            {
                info = BaseUtils.FieldBuilder.Build("Update, Collected:", ItemsCollected, "Delivered:", ItemsDelivered, "To Go:", TotalItemsToDeliver - ItemsDelivered, "Progress Left:;%;N1", ProgressPercent);
            }
            else
            {
                info = "Unknown CargoDepot type " + UpdateType;
            }

            detailed = "";
        }
    }
}
