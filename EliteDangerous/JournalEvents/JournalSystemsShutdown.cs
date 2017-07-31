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

namespace EliteDangerousCore.JournalEvents
{
    //When written: shortly after a certain scripted hyperdiction event begins.
    //Parameters:
    // •	N/A: No parameters.
    //
    //Example:
    //  {
    //      "timestamp": "2017-01-09T16:21:57Z",
    //      "event": "SystemsShutdown"
    //  }

    [JournalEntryType(JournalTypeEnum.SystemsShutdown)]
    public class JournalSystemsShutdown : JournalEntry
    {
        public JournalSystemsShutdown(JObject evt) : base(evt, JournalTypeEnum.SystemsShutdown) { }

        public override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.scanned;
            }
        }

        public override void FillInformation(out string summary, out string info, out string detailed)
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";
            detailed = "";
        }
    }
}
