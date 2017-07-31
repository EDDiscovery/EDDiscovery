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
//    When written: when the ‘reboot repair’ function is used
//    Parameters:
//•	Modules: JSON array of names of modules repaired

    [JournalEntryType(JournalTypeEnum.RebootRepair)]
    public class JournalRebootRepair : JournalEntry
    {
        public JournalRebootRepair(JObject evt) : base(evt, JournalTypeEnum.RebootRepair)
        {
            if (!evt["Modules"].Empty())
            {
                Modules = evt.Value<JArray>("Modules").Values<string>().ToArray();
                if (Modules != null)
                {
                    FriendlyModules = new string[Modules.Length];
                    for (int i = 0; i < Modules.Length; i++)
                        FriendlyModules[i] = JournalFieldNaming.GetBetterItemNameEvents(Modules[i]);
                }
            }
        }

        public string[] Modules { get; set; }
        public string[] FriendlyModules { get; set; }
        public override System.Drawing.Bitmap Icon { get { return EliteDangerousCore.Properties.Resources.rebootrepair; } }

        public override void FillInformation(out string summary, out string info, out string detailed)  //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";
            if (FriendlyModules != null)
                info = string.Join(",", FriendlyModules);
            detailed = "";
        }
    }
}


