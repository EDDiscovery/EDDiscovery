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
    //When written: when the player requests docking at a station
    //Parameters:
    //•	StationName: name of station
    [JournalEntryType(JournalTypeEnum.DockingRequested)]
    public class JournalDockingRequested : JournalEntry
    {
        public JournalDockingRequested(JObject evt ) : base(evt, JournalTypeEnum.DockingRequested)
        {
            StationName = JSONHelper.GetStringDef(evt["StationName"]);
        }
        public string StationName { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.dockingrequest; } }
    }
}
