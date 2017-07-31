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
    //When written: when receiving payment for powerplay combat
    //Parameters:
    //•	Power
    //•	Systems:[name,name]
    [JournalEntryType(JournalTypeEnum.PowerplayVoucher)]
    public class JournalPowerplayVoucher : JournalEntry
    {
        public JournalPowerplayVoucher(JObject evt) : base(evt, JournalTypeEnum.PowerplayVoucher)
        {
            Power = evt["Power"].Str();

            if ( !evt.Value<JArray>("Systems").Empty() )
                Systems = evt.Value<JArray>("Systems").Values<string>().ToArray();
        }

        public string Power { get; set; }
        public string[] Systems { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EliteDangerous.Properties.Resources.powerplayvoucher; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Power;
            if ( Systems!=null)
            {
                info += ", Systems:";

                bool comma = false;
                foreach( string s in Systems)
                {
                    if (comma)
                        info += ", ";
                    comma = true;
                    info += s;
                }
            }

            detailed = "";
        }
    }
}
