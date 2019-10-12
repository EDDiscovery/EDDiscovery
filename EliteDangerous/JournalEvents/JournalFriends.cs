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
using System.Collections.Generic;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.Friends)]
    public class JournalFriends : JournalEntry
    {
        public JournalFriends(JObject evt) : base(evt, JournalTypeEnum.Friends)
        {
            Status = evt["Status"].Str();
            Name = evt["Name"].Str();
            OfflineCount = Status.Equals("offline", System.StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            OnlineCount = Status.Equals("online", System.StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
        }

        public void AddFriend(JournalFriends next)
        {
            if (StatusList == null)     // if first time we added, move to status list format
            {
                StatusList = new List<string>() { Status };
                NameList = new List<string>() { Name };
                Status = Name = string.Empty;
            }

            string stat = next.Status;

            StatusList.Add(stat);
            NameList.Add(next.Name);

            OfflineCount += stat.Equals("offline", System.StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
            OnlineCount += stat.Equals("online", System.StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
        }

        public string Status { get; set; }      // used for single entries.. empty if list.  Used for VP backwards compat
        public string Name { get; set; }

        public List<string> StatusList { get; set; }        // EDD addition.. used when agregating, null if single entry
        public List<string> NameList { get; set; }

        public int OnlineCount { get; set; }        // always counts
        public int OfflineCount { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            
            detailed = "";

            if (StatusList != null)
            {
                info = "";
                if (OfflineCount + OnlineCount < NameList.Count)
                    info = BaseUtils.FieldBuilder.Build("Number of Statuses:".T(EDTx.JournalEntry_NumberofStatuses), NameList.Count);

                if (OnlineCount > 0)
                    info = info.AppendPrePad("Online:".T(EDTx.JournalEntry_Online) + OnlineCount.ToString(), ", ");

                if (OfflineCount > 0)
                    info = info.AppendPrePad("Offline:".T(EDTx.JournalEntry_Offline) + OfflineCount.ToString(), ", ");

                for ( int i = 0; i < StatusList.Count; i++ )
                    detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build("", NameList[i], "", StatusList[i]) , System.Environment.NewLine);
            }
            else
            {
                info = BaseUtils.FieldBuilder.Build("", Name, "", Status);
            }
        }
    }
}
