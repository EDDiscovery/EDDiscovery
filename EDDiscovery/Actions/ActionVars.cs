/*
 * Copyright © 2017 EDDiscovery development team
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
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Actions
{
    class ActionVars
    {
        static public void TriggerVars(ConditionVariables vars, string trigname, string triggertype)
        {
            vars["TriggerName"] = trigname;       // Program gets eventname which triggered it.. (onRefresh, LoadGame..)
            vars["TriggerType"] = triggertype;       // type (onRefresh, or OnNew, or ProgramTrigger for all others)
            vars["TriggerLocalTime"] = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");  // time it was started, US format, to match JSON.
            vars["TriggerUTCTime"] = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss");  // time it was started, US format, to match JSON.
        }

        static public void HistoryEventVars(ConditionVariables vars, HistoryEntry he, string prefix)
        {
            if (he != null)
            {
                System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

                vars[prefix + "LocalTime"] = he.EventTimeLocal.ToString("MM/dd/yyyy HH:mm:ss");
                vars[prefix + "DockedState"] = he.IsDocked ? "1" : "0";
                vars[prefix + "LandedState"] = he.IsLanded ? "1" : "0";
                vars[prefix + "StarSystem"] = he.System.name;
                vars[prefix + "StarSystemEDSMID"] = he.System.id_edsm.ToString(ct);
                vars[prefix + "WhereAmI"] = he.WhereAmI;
                vars[prefix + "ShipType"] = he.ShipType;
                vars[prefix + "ShipId"] = he.ShipId.ToString(ct);
                vars[prefix + "IndexOf"] = he.Indexno.ToString(ct);
                vars[prefix + "JID"] = he.Journalid.ToString(ct);
                vars[prefix + "EDSMID"] = he.System.id_edsm.ToString(ct);
                vars[prefix + "xpos"] = he.System.x.ToNANSafeString("0.###");
                vars[prefix + "ypos"] = he.System.y.ToNANSafeString("0.###");
                vars[prefix + "zpos"] = he.System.z.ToNANSafeString("0.###");

                vars[prefix + "EDDBID"] = he.System.id_eddb.ToString(ct);
                vars[prefix + "EDDBGovernment"] = he.System.government.ToNullUnknownString();
                vars[prefix + "EDDBAllegiance"] = he.System.allegiance.ToNullUnknownString();
                vars[prefix + "EDDBState"] = he.System.state.ToNullUnknownString();
                vars[prefix + "EDDBSecurity"] = he.System.security.ToNullUnknownString();
                vars[prefix + "EDDBPrimaryEconomy"] = he.System.primary_economy.ToNullUnknownString();
                vars[prefix + "EDDBFaction"] = he.System.faction.ToNullUnknownString();
                vars[prefix + "EDDBPopulation"] = he.System.population.ToString(ct);
                vars[prefix + "EDDBNeedsPermit"] = (he.System.needs_permit != 0) ? "1" : "0";

                vars.AddPropertiesFieldsOfType(he.journalEntry, prefix + "Class_");
                vars.GetJSONFieldNamesAndValues(he.journalEntry.EventDataString, prefix + "JS_");
            }
        }

        static public void HistoryEventFurtherInfo(ConditionVariables vars, HistoryList hl, HistoryEntry he, string prefix)
        {
            if (he != null)
            {
                System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

                vars[prefix + "VisitCount"] = hl.GetVisitsCount(he.System.name, he.System.id_edsm).ToString(ct);
                vars[prefix + "ScanCount"] = hl.GetScans(he.System.name, he.System.id_edsm).Count.ToString(ct);
                vars[prefix + "FSDJumpsTotal"] = hl.GetFSDJumps(new TimeSpan(100000, 0, 0, 0)).ToString(ct);

                int fsd = hl.GetFSDJumps(new DateTime(1980, 1, 1), he.EventTimeUTC);    // total before
                if (he.IsFSDJump)   // if on an fsd, count this in
                    fsd++;
                vars[prefix + "FSDJump"] = fsd.ToString(ct);
            }
        }
    }
}

