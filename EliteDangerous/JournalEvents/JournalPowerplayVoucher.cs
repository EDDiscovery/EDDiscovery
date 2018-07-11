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
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.PowerplayVoucher)]
    public class JournalPowerplayVoucher : JournalEntry
    {
        public JournalPowerplayVoucher(JObject evt) : base(evt, JournalTypeEnum.PowerplayVoucher)
        {
            Power = evt["Power"].Str();
            Systems = evt["Systems"]?.ToObjectProtected<string[]>();
        }

        public string Power { get; set; }
        public string[] Systems { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = Power;

            if ( Systems!=null)
            {
                info += ", Systems:".Txb(this);

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
