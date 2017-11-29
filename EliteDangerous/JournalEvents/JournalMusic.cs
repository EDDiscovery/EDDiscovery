﻿/*
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
using System;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    /*
When written: when the game music 'mood' changes
Parameters:
 MusicTrack: (name)
Possible track names are: NoTrack, MainMenu, CQCMenu, SystemMap, GalaxyMap, GalacticPowers
CQC, DestinationFromHyperspace, DestinationFromSupercruise, Supercruise, Combat_Unknown
Unknown_Encounter, CapitalShip, CombatLargeDogFight, Combat_Dogfight, Combat_SRV
Unknown_Settlement, DockingComputer, Starport, Unknown_Exploration, Exploration
Note: Other music track names may be used in future 
     */

    public enum EDMusicTrackEnum
    {
        None = 0, Unknown = 1,
        NoTrack, MainMenu, CQCMenu, SystemMap, GalaxyMap, GalacticPowers,
        CQC, DestinationFromHyperspace, DestinationFromSupercruise, Supercruise, Combat_Unknown,
        UnknownEncounter, CapitalShip, CombatLargeDogFight, CombatDogfight, CombatSRV,
        UnknownSettlement, DockingComputer, Starport, UnknownExploration, Exploration
    }

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

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("MusicTrack:", MusicTrack);
            detailed = "";
        }

        private EDMusicTrackEnum MusictoID(string str)
        {
            if (str == null)
                return EDMusicTrackEnum.None;

            var searchstr = str.Replace("_", "").Replace(" ", "").Replace("-", "").ToLower();


            foreach (EDMusicTrackEnum atm in Enum.GetValues(typeof(EDMusicTrackEnum)))
            {
                string str2 = atm.ToString().Replace("_", "").ToLower();

                if (searchstr.Equals(str2))
                    return atm;
            }

            return EDMusicTrackEnum.Unknown;
        }

    }
}
