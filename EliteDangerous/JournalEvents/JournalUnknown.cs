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
    [JournalEntryType(JournalTypeEnum.Unknown)] 
    public class JournalUnknown : JournalEntry
    {
        public string json;
        public string eventname;

        public JournalUnknown(JObject evt) : base(evt, JournalTypeEnum.Unknown)
        {
            json = evt.ToString();
            eventname = evt["event"].Str("No event tag");
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = "Unhandled Journal event, Report to EDDiscovery team. ".T(EDTx.JournalUnknown_UnhandledJournalevent) + Environment.NewLine + json;
            detailed = "";
        }
    }

}
