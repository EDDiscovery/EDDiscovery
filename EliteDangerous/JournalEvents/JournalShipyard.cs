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
    //    When written: when opening shipyard
    //Parameters:
    //•	MarketID
    //• StationName
    //• StarSystem
    //•	PriceList: Array of records
    //o   id
    //o   Name
    //o   ShipPrice

    [JournalEntryType(JournalTypeEnum.Shipyard)]
    public class JournalShipyard : JournalEntry, IAdditionalFiles
    {
        public JournalShipyard(JObject evt) : base(evt, JournalTypeEnum.Shipyard)
        {
            Rescan(evt);
        }

        public void Rescan(JObject evt)
        {
            StationName = evt["StationName"].Str();
            StarSystem = evt["StarSystem"].Str();
            MarketID = evt["MarketID"].LongNull();
            Horizons = evt["Horizons"].BoolNull();
            AllowCobraMkIV = evt["AllowCobraMkIV"].BoolNull();

            ShipyardItems = evt["PriceList"]?.ToObjectProtected<ShipyardItem[]>();

            if (ShipyardItems != null)
            {
                foreach (ShipyardItem i in ShipyardItems)
                    i.Normalise();
            }
        }

        public bool ReadAdditionalFiles(string directory, ref JObject jo)
        {
            JObject jnew = ReadAdditionalFile(System.IO.Path.Combine(directory, "Shipyard.json"));
            if (jnew != null)        // new json, rescan
            {
                jo = jnew;      // replace current
                Rescan(jo);
            }
            return jnew != null;
        }

        public string StationName { get; set; }
        public string StarSystem { get; set; }
        public long? MarketID { get; set; }
        public bool? Horizons { get; set; }
        public bool? AllowCobraMkIV { get; set; }

        public ShipyardItem[] ShipyardItems { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";
            detailed = "";

            if (ShipyardItems != null)
            {
                if (ShipyardItems.Length < 5)
                {
                    foreach (ShipyardItem m in ShipyardItems)
                        info = info.AppendPrePad(m.ShipType_Localised.Alt(m.ShipType), ", ");
                }
                else
                    info = ShipyardItems.Length.ToString() + " ships";

                foreach (ShipyardItem m in ShipyardItems)
                {
                    detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build("",m.ShipType_Localised.Alt(m.ShipType), "; cr;N0", m.ShipPrice), System.Environment.NewLine);
                }
            }
        }

        public class ShipyardItem
        {
            public long id;
            public string FDShipType;
            public string ShipType;
            public string ShipType_Localised;
            public long ShipPrice;

            public void Normalise()
            {
                FDShipType = ShipType;
                ShipType = JournalFieldNaming.GetBetterShipName(ShipType);
                ShipType_Localised = ShipType_Localised.Alt(ShipType);
            }
        }
    }
}
