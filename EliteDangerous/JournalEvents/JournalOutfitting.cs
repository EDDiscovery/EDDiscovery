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

using EliteDangerousCore;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.Outfitting)]
    public class JournalOutfitting : JournalEntry, IAdditionalFiles
    {
        public JournalOutfitting(JObject evt) : base(evt, JournalTypeEnum.Outfitting)
        {
            Rescan(evt);
        }

        public void Rescan(JObject evt)
        {
            ItemList = new Outfitting(evt["StationName"].Str(), evt["StarSystem"].Str(), EventTimeUTC, evt["Items"]?.ToObjectProtected<Outfitting.OutfittingItem[]>());
            MarketID = evt["MarketID"].LongNull();
            Horizons = evt["Horizons"].BoolNull();
        }

        public bool ReadAdditionalFiles(string directory, bool historyrefreshparse, ref JObject jo)
        {
            JObject jnew = ReadAdditionalFile(System.IO.Path.Combine(directory, "Outfitting.json"), waitforfile: !historyrefreshparse, checktimestamptype: true);
            if (jnew != null)        // new json, rescan
            {
                jo = jnew;      // replace current
                Rescan(jo);
            }
            return jnew != null;
        }

        public Outfitting ItemList;

        public long? MarketID { get; set; }
        public bool? Horizons { get; set; }
        public bool? AllowCobraMkIV { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = "";
            detailed = "";

            if (ItemList.Items != null)
            {
                info = ItemList.Items.Length.ToString() + " items available".Txb(this);
                int itemno = 0;
                foreach (Outfitting.OutfittingItem m in ItemList.Items)
                {
                    detailed = detailed.AppendPrePad(m.Name + ":" + m.BuyPrice.ToString("N0"), (itemno % 3 < 2) ? ", " : System.Environment.NewLine);
                    itemno++;
                }
            }
                
        }
    }
}
