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
using System;

namespace EliteDangerousCore.JournalEvents
{
    //When written: when collecting powerplay commodities for delivery
    //Parameters:
    //•	Power: name of power
    //•	Type: type of commodity
    //•	Count: number of units
    [JournalEntryType(JournalTypeEnum.Powerplay)]
    public class JournalPowerplay : JournalEntry
    {
        public JournalPowerplay(JObject evt) : base(evt, JournalTypeEnum.Powerplay)
        {
            Power = evt["Power"].Str();
            Rank = evt["Rank"].Int();
            Merits = evt["Merits"].Int();
            Votes = evt["Votes"].Int();
            TimePledged = evt["TimePledged"].Long();
            TimePledgedSpan = new TimeSpan((int)(TimePledged/60/60),(int)((TimePledged/60)%60),(int)(TimePledged%60));
            TimePledgedString = TimePledgedSpan.ToString();
        }

        public string Power { get; set; }
        public int Rank { get; set; }
        public int Merits { get; set; }
        public int Votes { get; set; }
        public long TimePledged { get; set; }
        public TimeSpan TimePledgedSpan { get; set; }
        public string TimePledgedString { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", Power, "Rank:", Rank, "Merits:", Merits, "Votes:", Votes, "Pledged:" , TimePledgedString);
            detailed = "";
        }
    }
}
