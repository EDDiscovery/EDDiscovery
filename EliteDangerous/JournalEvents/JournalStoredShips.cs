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
    //• ShipsHere: Array of records
    //o   ShipID
    //o   ShipType
    //o   Name
    //o   Value
    //•	ShipsRemote: Array of records
    //o   ShipID
    //o   ShipType
    //o   StarSystem
    //o   ShipMarketID
    //o   TransferPrice
    //o   TransferTime
    //o   Value

    [JournalEntryType(JournalTypeEnum.StoredShips)]
    public class JournalStoredShips : JournalEntry, IShipInformation
    {
        public JournalStoredShips(JObject evt) : base(evt, JournalTypeEnum.StoredShips)
        {
            StationName = evt["StationName"].Str();
            StarSystem = evt["StarSystem"].Str();
            MarketID = evt["MarketID"].LongNull();

            ShipsHere = evt["ShipsHere"]?.ToObjectProtected<StoredShipInformation[]>();
            Normalise(ShipsHere);

            if (ShipsHere!=null)
            {
                foreach (var x in ShipsHere)
                {
                    x.StarSystem = StarSystem;
                    x.StationName = StationName;
                }
            }

            ShipsRemote = evt["ShipsRemote"]?.ToObjectProtected<StoredShipInformation[]>();
            Normalise(ShipsRemote);
        }

        public string StationName { get; set; }
        public string StarSystem { get; set; }
        public long? MarketID { get; set; }

        public StoredShipInformation[] ShipsHere { get; set; }
        public StoredShipInformation[] ShipsRemote { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("At starport:",ShipsHere?.Count(),"Other locations:",ShipsRemote?.Count() );
            detailed = "";
            if (ShipsHere != null)
            {
                foreach (StoredShipInformation m in ShipsHere)
                    detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build("", m.ShipType, "; cr;N0", m.Value, ";(Hot)", m.Hot), System.Environment.NewLine);
            }
            if (ShipsRemote != null)
            {
                detailed = detailed.AppendPrePad("Remote:", System.Environment.NewLine + System.Environment.NewLine);

                foreach (StoredShipInformation m in ShipsRemote)
                {
                    if (m.InTransit)
                    {
                        detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build("; ",m.Name,
                                    "<; in transit", m.ShipType, 
                                    "Value:; cr;N0", m.Value, ";(Hot)", m.Hot), System.Environment.NewLine);

                    }
                    else
                    {
                        detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build(
                            "; ", m.Name,
                            "<", m.ShipType, 
                            "< at ", m.StarSystem, 
                            "Transfer Cost:; cr;N0", m.TransferPrice, "Time:", m.TransferTimeString, 
                            "Value:; cr;N0", m.Value, ";(Hot)", m.Hot), System.Environment.NewLine);
                    }
                }
            }
        }

        public void Normalise(StoredShipInformation[] s)
        {
            if (s != null)
            {
                foreach (StoredShipInformation i in s)
                    i.Normalise();
            }
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            //System.Diagnostics.Debug.WriteLine(EventTimeUTC + " StoredShips");
            if ( ShipsHere!=null)
                shp.StoredShips(ShipsHere);
            if ( ShipsRemote!=null)
                shp.StoredShips(ShipsRemote);
        }

        public class StoredShipInformation
        {
            public int ShipID;      // both
            public string ShipType; // both
            public string ShipType_Localised; // both
            public string Name;     // Both
            public long Value;      // both
            public bool Hot;        // both

            public string StarSystem;   // remote only and when not in transit, but filled in for local
            public long ShipMarketID;   //remote
            public long TransferPrice;  //remote
            public long TransferTime;   //remote
            public bool InTransit;      //remote, and that means StarSystem is not there.

            public string StationName;  // local only, null otherwise
            public string ShipTypeFD; // both, computed
            public System.TimeSpan TransferTimeSpan;        // computed
            public string TransferTimeString; // computed

            public void Normalise()
            {
                ShipTypeFD = JournalFieldNaming.NormaliseFDShipName(ShipType);
                ShipType = JournalFieldNaming.GetBetterShipName(ShipTypeFD);
                ShipType_Localised = ShipType_Localised.Alt(ShipType);
                TransferTimeSpan = new System.TimeSpan((int)(TransferTime / 60 / 60), (int)((TransferTime / 60) % 60), (int)(TransferTime % 60));
                TransferTimeString = TransferTimeSpan.ToString();
            }
        }
    }
}
