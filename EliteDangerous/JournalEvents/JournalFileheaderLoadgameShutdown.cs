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
    [JournalEntryType(JournalTypeEnum.Fileheader)]
    public class JournalFileheader : JournalEntry
    {
        public JournalFileheader(JObject evt ) : base(evt, JournalTypeEnum.Fileheader)
        {
            GameVersion = evt["gameversion"].Str();
            Build = evt["build"].Str();
            Language = evt["language"].Str();
            Part = evt["part"].Int();
        }

        public string GameVersion { get; set; }
        public string Build { get; set; }
        public string Language { get; set; }
        public int Part { get; set; }

        public override bool Beta
        {
            get
            {
                if (GameVersion.Contains("Beta"))
                    return true;

                if (GameVersion.Equals("2.2") && (Build.Contains("r121645/r0") || Build.Contains("r129516/r0")))
                    return true;

                return false;
            }
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("Version:".Txb(this), GameVersion , "Build:".Txb(this), Build , "Part:".Txb(this), Part);
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.LoadGame)]
    [System.Diagnostics.DebuggerDisplay("LoadGame {LoadGameCommander} {ShipId} {Ship} {GameMode}")]
    public class JournalLoadGame : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalLoadGame(JObject evt) : base(evt, JournalTypeEnum.LoadGame)
        {
            LoadGameCommander = evt["Commander"].Str();
            ShipFD = evt["Ship"].Str();
            if (ShipFD.Length > 0)      // Vega logs show no ship on certain logs.. handle it to prevent warnings.
            {
                ShipFD = JournalFieldNaming.NormaliseFDShipName(ShipFD);
                Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
                ShipId = evt["ShipID"].Int();
            }
            else
            {       // leave ShipFD as blank.
                Ship = "Unknown";
            }

            StartLanded = evt["StartLanded"].Bool();
            StartDead = evt["StartDead"].Bool();
            GameMode = evt["GameMode"].Str();
            Group = evt["Group"].Str();
            Credits = evt["Credits"].Long();
            Loan = evt["Loan"].Long();

            ShipName = evt["ShipName"].Str();
            ShipIdent = evt["ShipIdent"].Str();
            FuelLevel = evt["FuelLevel"].Double();
            FuelCapacity = evt["FuelCapacity"].Double();

            Horizons = evt["Horizons"].BoolNull();
        }

        public string LoadGameCommander { get; set; }
        public string Ship { get; set; }        // type, fer-de-lance
        public string ShipFD { get; set; }        // type, fd name
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

        public bool? Horizons { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("Cmdr ", LoadGameCommander, "Ship:".Txb(this), Ship, "Name:".Txb(this), ShipName, "Ident:".Txb(this), ShipIdent, "Credits:;;N0".Txb(this), Credits);
            detailed = BaseUtils.FieldBuilder.Build("Mode:".Txb(this), GameMode, "Group:".Txb(this), Group, "Not Landed;Landed".Txb(this), StartLanded, "Fuel Level:;;0.0".Txb(this), FuelLevel, "Capacity:;;0.0".Txb(this), FuelCapacity);
        }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            if (mcl.CashTotal != Credits)
            {
                mcl.AddEvent(Id, EventTimeUTC, EventTypeID, "Cash total differs, adjustment".Txb(this), Credits - mcl.CashTotal);
            }
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            if (ShipFD.HasChars())        // must have a ship name - there have been load games without it.
                shp.LoadGame(ShipId, Ship, ShipFD, ShipName, ShipIdent, FuelLevel, FuelCapacity);
        }

    }



    [JournalEntryType(JournalTypeEnum.Shutdown)]
    public class JournalShutdown : JournalEntry
    {
        public JournalShutdown(JObject evt) : base(evt, JournalTypeEnum.Shutdown)
        {
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "";
            detailed = "";
        }

    }

    [JournalEntryType(JournalTypeEnum.Continued)]
    public class JournalContinued : JournalEntry
    {
        public JournalContinued(JObject evt) : base(evt, JournalTypeEnum.Continued)
        {
            Part = evt["Part"].Int();
        }

        public int Part { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Part.ToString();
            detailed = "";
        }
    }

}
