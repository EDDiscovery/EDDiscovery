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
                vars[prefix + "WhereAmI"] = he.WhereAmI;
                vars[prefix + "ShipType"] = he.ShipType;
                vars[prefix + "ShipId"] = he.ShipId.ToString(ct);
                vars[prefix + "IndexOf"] = he.Indexno.ToString(ct);
                vars[prefix + "JID"] = he.Journalid.ToString(ct);

                vars.AddPropertiesFieldsOfClass(he.journalEntry, prefix + "Class_", new Type[] { typeof(System.Drawing.Bitmap), typeof(Newtonsoft.Json.Linq.JObject) } , 5);      //depth seems good enough
                vars.GetJSONFieldNamesAndValues(he.journalEntry.EventDataString, prefix + "JS_");
            }
        }

        static public void SystemVars(ConditionVariables vars, EDDiscovery2.DB.ISystem s, string prefix )
        {
            if (s != null)
            {
                System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

                vars[prefix + "StarSystem"] = s.name;
                vars[prefix + "StarSystemEDSMID"] = s.id_edsm.ToString(ct);
                vars[prefix + "xpos"] = s.x.ToNANSafeString("0.###");
                vars[prefix + "ypos"] = s.y.ToNANSafeString("0.###");
                vars[prefix + "zpos"] = s.z.ToNANSafeString("0.###");
                vars[prefix + "EDDBID"] = s.id_eddb.ToString(ct);
                vars[prefix + "EDDBGovernment"] = s.government.ToNullUnknownString();
                vars[prefix + "EDDBAllegiance"] = s.allegiance.ToNullUnknownString();
                vars[prefix + "EDDBState"] = s.state.ToNullUnknownString();
                vars[prefix + "EDDBSecurity"] = s.security.ToNullUnknownString();
                vars[prefix + "EDDBPrimaryEconomy"] = s.primary_economy.ToNullUnknownString();
                vars[prefix + "EDDBFaction"] = s.faction.ToNullUnknownString();
                vars[prefix + "EDDBPopulation"] = s.population.ToString(ct);
                vars[prefix + "EDDBNeedsPermit"] = (s.needs_permit != 0) ? "1" : "0";
            }
        }

        static public void SystemVarsFurtherInfo(ActionProgramRun vars, HistoryList hl, EDDiscovery2.DB.ISystem s, string prefix)
        {
            System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

            vars[prefix + "VisitCount"] = hl.GetVisitsCount(s.name).ToString(ct);
            vars[prefix + "ScanCount"] = hl.GetScans(s.name).Count.ToString(ct);
            vars[prefix + "FSDJumpsTotal"] = hl.GetFSDJumps(new TimeSpan(100000, 0, 0, 0)).ToString(ct);
        }

        static public void HistoryEventFurtherInfo(ActionProgramRun vars, HistoryList hl, HistoryEntry he, string prefix)
        {
            if (he != null)
            {
                System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

                int fsd = hl.GetFSDJumps(new DateTime(1980, 1, 1), he.EventTimeUTC);    // total before
                if (he.IsFSDJump)   // if on an fsd, count this in
                    fsd++;
                vars[prefix + "FSDJump"] = fsd.ToString(ct);
            }
        }

    }
}

