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
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    public class JournalSquadronBase : JournalEntry
    {
        public JournalSquadronBase(JObject evt, JournalTypeEnum e) : base(evt, e)
        {
            Name = evt["SquadronName"].Str();
        }

        public string Name { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Name;
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.AppliedToSquadron)]
    public class JournalAppliedToSquadron : JournalSquadronBase
    {
        public JournalAppliedToSquadron(JObject evt) : base(evt, JournalTypeEnum.AppliedToSquadron)
        {
        }
    }

    [JournalEntryType(JournalTypeEnum.DisbandedSquadron)]
    public class JournalDisbandedSquadron : JournalSquadronBase
    {
        public JournalDisbandedSquadron(JObject evt) : base(evt, JournalTypeEnum.DisbandedSquadron)
        {
        }
    }

    [JournalEntryType(JournalTypeEnum.InvitedToSquadron)]
    public class JournalInvitedToSquadron : JournalSquadronBase
    {
        public JournalInvitedToSquadron(JObject evt) : base(evt, JournalTypeEnum.InvitedToSquadron)
        {
        }
    }

    [JournalEntryType(JournalTypeEnum.JoinedSquadron)]
    public class JournalJoinedSquadron : JournalSquadronBase
    {
        public JournalJoinedSquadron(JObject evt) : base(evt, JournalTypeEnum.JoinedSquadron)
        {
        }
    }

    [JournalEntryType(JournalTypeEnum.KickedFromSquadron)]
    public class JournalKickedFromSquadron : JournalSquadronBase
    {
        public JournalKickedFromSquadron(JObject evt) : base(evt, JournalTypeEnum.KickedFromSquadron)
        {
        }
    }

    [JournalEntryType(JournalTypeEnum.LeftSquadron)]
    public class JournalLeftSquadron : JournalSquadronBase
    {
        public JournalLeftSquadron(JObject evt) : base(evt, JournalTypeEnum.LeftSquadron)
        {
        }
    }

    [JournalEntryType(JournalTypeEnum.SharedBookmarkToSquadron)]
    public class JournalSharedBookmarkToSquadron : JournalSquadronBase
    {
        public JournalSharedBookmarkToSquadron(JObject evt) : base(evt, JournalTypeEnum.SharedBookmarkToSquadron)
        {
        }
    }

    [JournalEntryType(JournalTypeEnum.SquadronCreated)]
    public class JournalSquadronCreated : JournalSquadronBase
    {
        public JournalSquadronCreated(JObject evt) : base(evt, JournalTypeEnum.SquadronCreated)
        {
        }
    }

    public class JournalSquadronRankBase : JournalSquadronBase
    {
        public JournalSquadronRankBase(JObject evt, JournalTypeEnum e) : base(evt, e)
        {
            OldRank = (SquadronRank)evt["OldRank"].Int();
            NewRank = (SquadronRank)evt["NewRank"].Int();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Name, "Old:".Txb(this), OldRank.ToString().SplitCapsWord(), "New:".Txb(this), NewRank.ToString().SplitCapsWord());
            detailed = "";
        }

        public SquadronRank OldRank { get; set; }
        public SquadronRank NewRank { get; set; }
    }


    [JournalEntryType(JournalTypeEnum.SquadronDemotion)]
    public class JournalSquadronDemotion : JournalSquadronRankBase
    {
        public JournalSquadronDemotion(JObject evt) : base(evt, JournalTypeEnum.SquadronDemotion)
        {
        }
    }

    [JournalEntryType(JournalTypeEnum.SquadronPromotion)]
    public class JournalSquadronPromotion : JournalSquadronRankBase
    {
        public JournalSquadronPromotion(JObject evt) : base(evt, JournalTypeEnum.SquadronPromotion)
        {
        }
    }

    [JournalEntryType(JournalTypeEnum.WonATrophyForSquadron)]
    public class JournalWonATrophyForSquadron : JournalSquadronBase
    {
        public JournalWonATrophyForSquadron(JObject evt) : base(evt, JournalTypeEnum.WonATrophyForSquadron)
        {
        }
    }

    [JournalEntryType(JournalTypeEnum.SquadronStartup)]
    public class JournalSquadronStartup : JournalSquadronBase
    {
        public JournalSquadronStartup(JObject evt) : base(evt, JournalTypeEnum.SquadronStartup)
        {
            CurrentRank = (SquadronRank)evt["CurrentRank"].Int();
        }

        public SquadronRank CurrentRank { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Name, "Rank:".Txb(this), CurrentRank.ToString().SplitCapsWord());
            detailed = "";
        }
    }

}
