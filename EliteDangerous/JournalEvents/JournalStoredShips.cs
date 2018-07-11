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
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
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

        public override void FillInformation(out string info, out string detailed) 
        {
            
            info = BaseUtils.FieldBuilder.Build("At starport:".Tx(this), ShipsHere?.Count(),"Other locations:".Tx(this), ShipsRemote?.Count() );
            detailed = "";
            if (ShipsHere != null)
            {
                foreach (StoredShipInformation m in ShipsHere)
                    detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build("", m.ShipType, "; cr;N0".Tx(this,"SSP"), m.Value, ";(Hot)".Txb(this), m.Hot), System.Environment.NewLine);
            }
            if (ShipsRemote != null)
            {
                detailed = detailed.AppendPrePad("Remote:".Tx(this), System.Environment.NewLine + System.Environment.NewLine);

                foreach (StoredShipInformation m in ShipsRemote)
                {
                    if (m.InTransit)
                    {
                        detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build("; ",m.Name,
                                    "<; in transit".Tx(this), m.ShipType, 
                                    "Value:; cr;N0".Txb(this), m.Value, ";(Hot)".Txb(this), m.Hot), System.Environment.NewLine);

                    }
                    else
                    {
                        detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build(
                            "; ", m.Name,
                            "<", m.ShipType, 
                            "< at ".Txb(this), m.StarSystem, 
                            "Transfer Cost:; cr;N0".Txb(this), m.TransferPrice, "Time:".Txb(this), m.TransferTimeString, 
                            "Value:; cr;N0".Txb(this), m.Value, ";(Hot)".Txb(this), m.Hot), System.Environment.NewLine);
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

    }
}
