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
    [JournalEntryType(JournalTypeEnum.EngineerProgress)]
    public class JournalEngineerProgress : JournalEntry
    {
        public class ProgressInformation
        {
            public string Engineer { get; set; }
            public long EngineerID { get; set; }
            public string Progress { get; set; }
        }

        public JournalEngineerProgress(JObject evt ) : base(evt, JournalTypeEnum.EngineerProgress)
        {
            Engineers = evt["Engineers"]?.ToObjectProtected<ProgressInformation[]>().OrderBy(x => x.Engineer)?.ToArray();       // 3.3 introduced this at startup

            if (Engineers == null)
            {
                Engineer = evt["Engineer"].Str();
                EngineerID = evt["EngineerID"].LongNull();
                Rank = evt["Rank"].IntNull();
                Progress = evt["Progress"].Str();
            }
        }

        public ProgressInformation[] Engineers { get; set; }      // may be NULL if not startup or pre 3.3

        public string Engineer { get; set; }            // may be empty if not progress during play
        public long? EngineerID { get; set; }
        public string Progress { get; set; }
        public int? Rank { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            detailed = "";
            if ( Engineers != null )
            {
                info = BaseUtils.FieldBuilder.Build("Progress on ; Engineers".Txb(this), Engineers.Length);

                foreach (var p in Engineers)
                {
                    detailed += BaseUtils.FieldBuilder.Build("", p.Engineer, "", p.Progress) + System.Environment.NewLine;
                }
            }
            else
                info = BaseUtils.FieldBuilder.Build("", Engineer, "Rank:".Txb(this), Rank, "Progress:".Tx(this), Progress);
        }
    }
}
