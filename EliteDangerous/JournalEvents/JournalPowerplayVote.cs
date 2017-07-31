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
    //When written: when voting for a system expansion
    //Parameters:
    //•	Power
    //•	Votes
    //•	System
    [JournalEntryType(JournalTypeEnum.PowerplayVote)]
    public class JournalPowerplayVote : JournalEntry
    {
        public JournalPowerplayVote(JObject evt) : base(evt, JournalTypeEnum.PowerplayVote)
        {
            Power = evt["Power"].Str();
            System = evt["System"].Str();
            Votes = evt["Votes"].Int();
        }
        public string Power { get; set; }
        public string System { get; set; }
        public int Votes { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EliteDangerous.Properties.Resources.powerplayvote; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", Power, "System:", System, "Votes:", Votes);
            detailed = "";
        }
    }
}
