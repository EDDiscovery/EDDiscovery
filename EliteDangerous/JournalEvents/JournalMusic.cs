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
    public enum EDMusicTrackEnum
    {
        None = 0, Unknown = 1,
        NoTrack, MainMenu, CQCMenu, SystemMap, GalaxyMap, GalacticPowers,
        CQC, DestinationFromHyperspace, DestinationFromSupercruise, Supercruise, Combat_Unknown,
        UnknownEncounter, CapitalShip, CombatLargeDogFight, CombatDogfight, CombatSRV,
        UnknownSettlement, DockingComputer, Starport, UnknownExploration, Exploration
    }

    // Note as from 10.5.1, Music is converted to UIMusic and issued thru the UI system.  This exists to decode the journal entry before its converted

    [JournalEntryType(JournalTypeEnum.Music)]
    public class JournalMusic : JournalEntry
    {
        public JournalMusic(JObject evt ) : base(evt, JournalTypeEnum.Music)
        {
            MusicTrack = evt["MusicTrack"].Str();
            MusicTrackID = MusictoID(MusicTrack);
        }

        public string MusicTrack { get; set; }
        public EDMusicTrackEnum MusicTrackID { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("Music Track:".Txb(this), MusicTrack);
            detailed = "";
        }

        private EDMusicTrackEnum MusictoID(string str)
        {
            if (str == null)
                return EDMusicTrackEnum.None;

            var searchstr = str.Replace("_", "").Replace(" ", "").Replace("-", "").ToLowerInvariant();


            foreach (EDMusicTrackEnum atm in Enum.GetValues(typeof(EDMusicTrackEnum)))
            {
                string str2 = atm.ToString().Replace("_", "").ToLowerInvariant();

                if (searchstr.Equals(str2))
                    return atm;
            }

            return EDMusicTrackEnum.Unknown;
        }

    }
}
