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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.LoadGame)]
    [System.Diagnostics.DebuggerDisplay("{LoadGameCommander} {ShipId} {Ship} {GameMode}")]
    public class JournalLoadGame : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalLoadGame(JObject evt ) : base(evt, JournalTypeEnum.LoadGame)
        {
            LoadGameCommander = JSONHelper.GetStringDef(evt["Commander"]);
            Ship = JournalFieldNaming.GetBetterShipName(JSONHelper.GetStringDef(evt["Ship"]));
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            StartLanded = JSONHelper.GetBool(evt["StartLanded"]);
            StartDead = JSONHelper.GetBool(evt["StartDead"]);
            GameMode = JSONHelper.GetStringDef(evt["GameMode"]);
            Group = JSONHelper.GetStringDef(evt["Group"]);
            Credits = JSONHelper.GetLong(evt["Credits"]);
            Loan = JSONHelper.GetLong(evt["Loan"]);

            ShipName = JSONHelper.GetStringDef(evt["ShipName"]);
            ShipIdent = JSONHelper.GetStringDef(evt["ShipIdent"]);
            FuelLevel = JSONHelper.GetDouble(evt["FuelLevel"]);
            FuelCapacity = JSONHelper.GetDouble(evt["FuelCapacity"]);
        }

        public string LoadGameCommander { get; set; }
        public string Ship { get; set; }        // type, fer-de-lance
        public int ShipId { get; set; }
        public bool StartLanded { get; set; }
        public bool StartDead { get; set; }
        public string GameMode { get; set; }
        public string Group { get; set; }
        public long Credits { get; set; }
        public long Loan { get; set; }

        public string ShipName { get; set; } // : user-defined ship name
        public string ShipIdent { get; set; } //   user-defined ship ID string
        public double FuelLevel { get; set; }
        public double FuelCapacity { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.loadgame; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            if (mcl.CashTotal != Credits)
            {
                mcl.AddEvent(Id, EventTimeUTC, EventTypeID, "Cash total differs, adjustment", Credits - mcl.CashTotal);
            }
        }

        public void ShipInformation(ShipInformationList shp, DB.SQLiteConnectionUser conn)
        {
            shp.LoadGame(ShipId, Ship, ShipName, ShipIdent);
        }

        public override void FillInformation(out string summary, out string info, out string detailed)
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";// NOT DONE
            detailed = "";
        }
    }
}
