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
    [JournalEntryType(JournalTypeEnum.CrewAssign)]
    public class JournalCrewAssign : JournalEntry
    {
        public JournalCrewAssign(JObject evt) : base(evt, JournalTypeEnum.CrewAssign)
        {
            Name = evt["Name"].Str();
            Role = evt["Role"].Str();
            NpcCrewID = evt["CrewID"].Long();
        }

        public long NpcCrewID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            
            info = BaseUtils.FieldBuilder.Build("", Name, "< to role ;".T(EDTx.JournalEntry_torole), Role);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CrewFire)]
    public class JournalCrewFire : JournalEntry
    {
        public JournalCrewFire(JObject evt) : base(evt, JournalTypeEnum.CrewFire)
        {
            Name = evt["Name"].Str();
            NpcCrewID = evt["CrewID"].Long();
        }

        public long NpcCrewID { get; set; }
        public string Name { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("; fired".T(EDTx.JournalEntry_fired), Name);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CrewHire)]
    public class JournalCrewHire : JournalEntry, ILedgerJournalEntry
    {
        public JournalCrewHire(JObject evt) : base(evt, JournalTypeEnum.CrewHire)
        {
            Name = evt["Name"].Str();
            Faction = evt["Faction"].Str();
            Cost = evt["Cost"].Long();
            CombatRank = (CombatRank)evt["CombatRank"].Int();
            NpcCrewID = evt["CrewID"].Long();
        }

        public long NpcCrewID { get; set; }
        public string Name { get; set; }
        public string Faction { get; set; }
        public long Cost { get; set; }
        public CombatRank CombatRank { get; set; }

        public void Ledger(Ledger mcl, DB.IUserDatabase conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Name + " " + Faction, -Cost);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Hired:;".T(EDTx.JournalEntry_Hired), Name, "< of faction ".T(EDTx.JournalEntry_offaction), Faction, "Rank:".T(EDTx.JournalEntry_Rank), CombatRank.ToString().SplitCapsWord(), "Cost:; cr;N0".T(EDTx.JournalEntry_Cost), Cost);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CrewLaunchFighter)]
    public class JournalCrewLaunchFighter : JournalEntry
    {
        public JournalCrewLaunchFighter(JObject evt) : base(evt, JournalTypeEnum.CrewLaunchFighter)
        {
            Crew = evt["Crew"].Str();
            ID = evt["ID"].IntNull();

        }
        public string Crew { get; set; }
        public int? ID { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Crew;
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CrewMemberJoins)]
    public class JournalCrewMemberJoins : JournalEntry
    {
        public JournalCrewMemberJoins(JObject evt) : base(evt, JournalTypeEnum.CrewMemberJoins)
        {
            Crew = evt["Crew"].Str();

        }
        public string Crew { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Crew;
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CrewMemberQuits)]
    public class JournalCrewMemberQuits : JournalEntry
    {
        public JournalCrewMemberQuits(JObject evt) : base(evt, JournalTypeEnum.CrewMemberQuits)
        {
            Crew = evt["Crew"].Str();

        }
        public string Crew { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Crew;
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CrewMemberRoleChange)]
    public class JournalCrewMemberRoleChange : JournalEntry
    {
        public JournalCrewMemberRoleChange(JObject evt) : base(evt, JournalTypeEnum.CrewMemberRoleChange)
        {
            Crew = evt["Crew"].Str();
            Role = evt["Role"].Str().SplitCapsWord();
        }

        public string Crew { get; set; }
        public string Role { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Crew:".T(EDTx.JournalEntry_Crew), Crew, "Role:".T(EDTx.JournalEntry_Role), Role);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.KickCrewMember)]
    public class JournalKickCrewMember : JournalEntry
    {
        public JournalKickCrewMember(JObject evt) : base(evt, JournalTypeEnum.KickCrewMember)
        {
            Crew = evt["Crew"].Str();
            OnCrime = evt["OnCrime"].Bool();
        }

        public string Crew { get; set; }
        public bool OnCrime { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Crew Member:".T(EDTx.JournalEntry_CrewMember), Crew, "; Due to Crime".T(EDTx.JournalEntry_DuetoCrime), OnCrime);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.JoinACrew)]
    public class JournalJoinACrew : JournalEntry
    {
        public JournalJoinACrew(JObject evt) : base(evt, JournalTypeEnum.JoinACrew)
        {
            Captain = evt["Captain"].Str();

        }
        public string Captain { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("Captain:".T(EDTx.JournalEntry_Captain), Captain);
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.ChangeCrewRole)]
    public class JournalChangeCrewRole : JournalEntry
    {
        public JournalChangeCrewRole(JObject evt) : base(evt, JournalTypeEnum.ChangeCrewRole)
        {
            Role = evt["Role"].Str().SplitCapsWordFull();
        }

        public string Role { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Role;
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.EndCrewSession)]
    public class JournalEndCrewSession : JournalEntry
    {
        public JournalEndCrewSession(JObject evt) : base(evt, JournalTypeEnum.EndCrewSession)
        {
            OnCrime = evt["OnCrime"].Bool();

        }
        public bool OnCrime { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("; Due to Crime".T(EDTx.JournalEntry_DuetoCrime), OnCrime);
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.QuitACrew)]
    public class JournalQuitACrew : JournalEntry
    {
        public JournalQuitACrew(JObject evt) : base(evt, JournalTypeEnum.QuitACrew)
        {
            Captain = evt["Captain"].Str();
        }

        public string Captain { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Captain:".T(EDTx.JournalEntry_Captain), Captain);
            detailed = "";
        }

    }


}
