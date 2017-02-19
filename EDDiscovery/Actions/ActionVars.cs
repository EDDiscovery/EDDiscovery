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
                vars[prefix + "LocalTime"] = he.EventTimeLocal.ToString("MM/dd/yyyy HH:mm:ss");
                vars[prefix + "DockedState"] = he.IsDocked ? "1" : "0";
                vars[prefix + "LandedState"] = he.IsLanded ? "1" : "0";
                vars[prefix + "StarSystem"] = he.System.name;
                vars[prefix + "StarSystemEDSMID"] = he.System.id_edsm.ToString(System.Globalization.CultureInfo.InvariantCulture);
                vars[prefix + "WhereAmI"] = he.WhereAmI;
                vars[prefix + "ShipType"] = he.ShipType;
                vars[prefix + "ShipId"] = he.ShipId.ToString(System.Globalization.CultureInfo.InvariantCulture);
                vars[prefix + "IndexOf"] = he.Indexno.ToString(System.Globalization.CultureInfo.InvariantCulture);
                vars[prefix + "JID"] = he.Journalid.ToString(System.Globalization.CultureInfo.InvariantCulture);
                vars.AddPropertiesFieldsOfType(he.journalEntry, prefix + "Class_");
            }
        }
    }
}
