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
    //When written: player has a ship targeted
    //Parameters:
    //* TargetLocked
    //* Ship
    //* Ship_Localised
    //* ScanStage
    //* PilotName
    //* PilotName_Localised
    //* PilotRank

    [JournalEntryType(JournalTypeEnum.ShipTargeted)]
    public class JournalShipTargeted : JournalEntry
    {
        public JournalShipTargeted(JObject evt ) : base(evt, JournalTypeEnum.ShipTargeted)
        {
            TargetLocked = evt["TargetLocked"].Bool();
            Ship = evt["Ship"].StrNull();
            Ship_Localised = evt["Ship_Localised"].StrNull();
            ScanStage = evt["ScanStage"].IntNull();
            PilotName = evt["PilotName"].StrNull();
            PilotName_Localised = evt["PilotName_Localised"].StrNull();
            PilotRank = evt["PilotRank"].StrNull();

            if (Ship != null)
                Ship = JournalFieldNaming.GetBetterShipName(Ship);
        }

        public bool TargetLocked { get; set; }
        public string Ship { get; set; }
        public string Ship_Localised { get; set; }
        public int? ScanStage { get; set; }
        public string PilotName { get; set; }
        public string PilotName_Localised { get; set; }
        public string PilotRank { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";
            detailed = "";
        }

    }
}
