﻿/*
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
    //    When written: If you should ever reset your game
    //    Parameters:
    //•	Name: commander name
    [JournalEntryType(JournalTypeEnum.ClearSavedGame)]
    public class JournalClearSavedGame : JournalEntry
    {
        public JournalClearSavedGame(JObject evt) : base(evt, JournalTypeEnum.ClearSavedGame)
        {
            Name = evt["Name"].Str();

        }
        public string Name { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Name;
            detailed = "";
        }
    }
}
