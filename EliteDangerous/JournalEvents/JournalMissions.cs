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
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.Missions)]
    public class JournalMissions : JournalEntry
    {
        public JournalMissions(JObject evt) : base(evt, JournalTypeEnum.Missions)
        {
            ActiveMissions = evt["Active"]?.ToObjectProtected<MissionItem[]>();
            Normalise(ActiveMissions);
            FailedMissions = evt["Failed"]?.ToObjectProtected<MissionItem[]>();
            Normalise(FailedMissions);
            CompletedMissions = evt["Complete"]?.ToObjectProtected<MissionItem[]>();
            Normalise(CompletedMissions);
        }

        public MissionItem[] ActiveMissions { get; set; }
        public MissionItem[] FailedMissions { get; set; }
        public MissionItem[] CompletedMissions { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("Active:".Txb(this), ActiveMissions?.Length, "Failed:".Txb(this), FailedMissions?.Length, "Completed:".Txb(this), CompletedMissions?.Length);
            detailed = "";
            if (ActiveMissions != null && ActiveMissions.Length>0)
            {
                detailed = detailed.AppendPrePad("Active:".Txb(this), Environment.NewLine);
                foreach (var x in ActiveMissions)
                    detailed = detailed.AppendPrePad("    " + x.Format(), Environment.NewLine);
            }
            if (FailedMissions != null && FailedMissions.Length>0)
            {
                detailed = detailed.AppendPrePad("Failed:".Txb(this), Environment.NewLine);
                foreach (var x in FailedMissions)
                    detailed = detailed.AppendPrePad("    " + x.Format(), Environment.NewLine);
            }
            if (CompletedMissions != null && CompletedMissions.Length > 0)
            {
                detailed = detailed.AppendPrePad("Completed:".Txb(this), Environment.NewLine);
                foreach (var x in CompletedMissions)
                    detailed = detailed.AppendPrePad("    " + x.Format(), Environment.NewLine);
            }

        }

        public void Normalise(MissionItem[] array)
        {
            if (array != null)
                foreach (var x in array)
                    x.Normalise(EventTimeUTC);
        }

        public class MissionItem
        {
            public long id;
            public string Name;
            public bool PassengerMission;
            public int Expires;

            string FriendlyName;
            DateTime ExpiryTimeUTC;

            public void Normalise(DateTime utcnow)
            {
                ExpiryTimeUTC = utcnow.AddSeconds(Expires);
                FriendlyName = JournalFieldNaming.GetBetterMissionName(Name);
            }

            public string Format()
            {
                return BaseUtils.FieldBuilder.Build("", FriendlyName, "<;(Passenger)".Tx(this), PassengerMission, " " + "Expires:".Tx(this), ExpiryTimeUTC.ToLocalTime());
            }
        }
    }
}
