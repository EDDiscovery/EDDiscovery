/*
 * Copyright © 2015 - 2018 EDDiscovery development team
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace EDDiscovery.DLL
{
    static public class EDDDLLCallerHE
    {
        static public EDDDLLIF.JournalEntry CreateFromHistoryEntry(EliteDangerousCore.HistoryEntry he)
        {
            if (he == null)
            {
                return new EDDDLLIF.JournalEntry() { ver = 1, indexno = -1 };
            }
            else
            {
                EDDDLLIF.JournalEntry je = new EDDDLLIF.JournalEntry()
                {
                    ver = 1,
                    indexno = he.Indexno,
                    utctime = he.EventTimeUTC.ToStringZulu(),
                    name = he.EventSummary,
                    systemname = he.System.Name,
                    x = he.System.X,
                    y = he.System.Y,
                    z = he.System.Z,
                    travelleddistance = he.TravelledDistance,
                    travelledseconds = (long)he.TravelledSeconds.TotalSeconds,
                    islanded = he.IsLanded,
                    isdocked = he.IsDocked,
                    whereami = he.WhereAmI,
                    shiptype = he.ShipType,
                    gamemode = he.GameMode,
                    group = he.Group,
                    credits = he.Credits,
                    eventid = he.journalEntry.EventTypeStr
                };

                he.journalEntry.FillInformation(out je.info, out je.detailedinfo);

                je.materials = (from x in he.MaterialCommodity.Sort(false) select x.Details.Name + ":" + x.Count.ToStringInvariant() + ":" + x.Details.FDName).ToArray();
                je.commodities = (from x in he.MaterialCommodity.Sort(true) select x.Details.Name + ":" + x.Count.ToStringInvariant() + ":" + x.Details.FDName).ToArray();
                je.currentmissions = he.MissionList.GetAllCurrentMissions(he.EventTimeUTC).Select(x=>x.FullInfo()).ToArray();
                return je;
            }
        }
    }
}
