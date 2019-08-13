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
    [JournalEntryType(JournalTypeEnum.Progress)]
    public class JournalProgress : JournalEntry
    {
        public JournalProgress(JObject evt ) : base(evt, JournalTypeEnum.Progress)
        {
            Combat = evt["Combat"].Int();
            Trade = evt["Trade"].Int();
            Explore = evt["Explore"].Int();
            Empire = evt["Empire"].Int();
            Federation = evt["Federation"].Int();
            CQC = evt["CQC"].Int();
        }

        public int Combat { get; set; }         // keep ints for backwards compat
        public int Trade { get; set; }
        public int Explore { get; set; }
        public int Empire { get; set; }
        public int Federation { get; set; }
        public int CQC { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("Combat:;%".T(EDTx.JournalProgress_Combat), Combat,
                                      "Trade:;%".T(EDTx.JournalProgress_Trade), Trade,
                                      "Exploration:;%".T(EDTx.JournalProgress_Exploration), Explore,
                                      "Federation:;%".T(EDTx.JournalProgress_Federation), Federation,
                                      "Empire:;%".T(EDTx.JournalProgress_Empire), Empire,
                                      "CQC:;%".T(EDTx.JournalProgress_CQC), CQC);
            detailed = "";
        }
    }
}
