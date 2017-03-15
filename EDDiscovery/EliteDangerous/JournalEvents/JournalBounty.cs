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
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: player is awarded a bounty for a kill
    //Parameters:
    //•	Faction: the faction awarding the bounty
    //•	Reward: the reward value
    //•	VictimFaction: the victim’s faction
    //•	SharedWithOthers: whether shared with other players
    [JournalEntryType(JournalTypeEnum.Bounty)]
    public class JournalBounty : JournalEntry, ILedgerNoCashJournalEntry
    {
        public class BountyReward
        {
            public string Faction;
            public long Reward;
        }

        public JournalBounty(JObject evt) : base(evt, JournalTypeEnum.Bounty)
        {
            TotalReward = JSONHelper.GetLong(evt["TotalReward"]);     // others of them..

            VictimFaction = JSONHelper.GetStringDef(evt["VictimFaction"]);
            VictimFactionLocalised = JSONHelper.GetStringDef(evt["VictimFaction_Localised"]); // may not be present

            SharedWithOthers = JSONHelper.GetBool(evt["SharedWithOthers"], false);
            Rewards = evt["Rewards"]?.ToObject<BountyReward[]>();
        }

        public long TotalReward { get; set; }
        public string VictimFaction { get; set; }
        public string VictimFactionLocalised { get; set; }
        public bool SharedWithOthers { get; set; }
        public BountyReward[] Rewards { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.bounty; } }

        public void LedgerNC(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            string n = (VictimFactionLocalised.Length > 0) ? VictimFactionLocalised : VictimFaction;
            n += " total " + TotalReward.ToString("N0");

            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, n);
        }

        public override void FillInformation(out string summary, out string info, out string detailed)
        {
            summary = EventTypeStr.SplitCapsWord();
            //info = FieldBuilder("; credits",TotalReward,"Victim faction:" , VictimFactionLocalised.Length>0 ? VictimFactionLocalised : VictimFaction);
            info = ToOld();
            detailed = "";
        }

        // prefix;postfix value [if double,format]
        // if bool, prefix;postfix is true/false

        public string FieldBuilder(params System.Object[] values)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(64);
            for( int i = 0; i < values.Length; )
            {
                System.Diagnostics.Debug.Assert(i + 2 <= values.Length);

                string[] fieldnames = ((string)values[i]).Split(';');
                object value = values[i + 1];
                i += 2;

                if (value is bool)
                {
                    sb.Append(((bool)value) ? fieldnames[0] : fieldnames[1]);
                }
                else
                {
                    if (fieldnames[0].Length > 0)
                        sb.Append(fieldnames[0]);

                    if (value is string)
                        sb.Append((string)value);
                    else if (value is double)
                    {
                        System.Diagnostics.Debug.Assert(i < values.Length);
                        sb.Append(((double)value).ToString((string)values[i++]));
                    }
                    else if (value is int)
                        sb.Append(((int)value).ToString(System.Globalization.CultureInfo.InvariantCulture));
                    else if (value is long)
                        sb.Append(((long)value).ToString(System.Globalization.CultureInfo.InvariantCulture));
                    else if (value is double?)
                    {
                        System.Diagnostics.Debug.Assert(i < values.Length);
                        sb.Append(((double?)value).Value.ToString((string)values[i++]));
                    }
                    else if (value is int?)
                        sb.Append(((int?)value).Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    else if (value is long?)
                        sb.Append(((long?)value).Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    else
                        System.Diagnostics.Debug.Assert(false);

                    if (fieldnames.Length >= 2 && fieldnames[1].Length > 0)
                        sb.Append(fieldnames[1]);
                }

                sb.Append(' ');
            }

            return sb.ToString();
        }

    }


}
