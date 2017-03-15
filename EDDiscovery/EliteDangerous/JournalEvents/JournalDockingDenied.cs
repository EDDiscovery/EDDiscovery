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

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when the station denies a docking request
    //Parameters:
    //•	StationName: name of station
    //•	Reason: reason for denial
    //
    //Reasons include: NoSpace, TooLarge, Hostile, Offences, Distance, ActiveFighter, NoReason
    [JournalEntryType(JournalTypeEnum.DockingDenied)]
    public class JournalDockingDenied : JournalEntry
    {
        public JournalDockingDenied(JObject evt ) : base(evt, JournalTypeEnum.DockingDenied)
        {
            StationName = JSONHelper.GetStringDef(evt["StationName"]);
            Reason = JSONHelper.GetStringDef(evt["Reason"]);
        }
        public string StationName { get; set; }
        public string Reason { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.dockingdenied; } }

        public override void FillInformation(out string summary, out string info, out string detailed)
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";// NOT DONE
            detailed = "";
        }
    }
}
