/*
 * Copyright © 2017 EDDiscovery development team
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

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//    When written: when the player's ship has been scanned
//(note the "Scan Detected" indication is at the start of the scan, this is written at the end of a successful scan)
//Parameters:
//•	ScanType: Cargo, Crime, Cabin, Data or Unknown

//Example:
//{ "timestamp":"2017-02-13T12:30:09Z", "event":"Scanned", "ScanType":"Cargo" }

    [JournalEntryType(JournalTypeEnum.Scanned)]
    public class JournalScanned : JournalEntry
    {
        public JournalScanned(JObject evt) : base(evt, JournalTypeEnum.Scanned)
        {
            ScanType = JSONHelper.GetStringDef(evt["Item"]);
        }
        public string ScanType { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.scanned; } }

    }
}
