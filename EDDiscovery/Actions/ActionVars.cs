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
            vars[prefix + "LocalTime"] = he.EventTimeLocal.ToString("MM/dd/yyyy HH:mm:ss");
            vars[prefix + "DockedState"] = he.IsDocked ? "1" : "0";
            vars[prefix + "LandedState"] = he.IsLanded ? "1" : "0";
            vars[prefix + "StarSystem"] = he.System.name;
            vars[prefix + "StarSystemEDSMID"] = he.System.id_edsm.ToString();
            vars[prefix + "WhereAmI"] = he.WhereAmI;
            vars[prefix + "ShipType"] = he.ShipType;
            vars[prefix + "IndexOf"] = he.Indexno.ToString();
            vars[prefix + "JID"] = he.Journalid.ToString();
            vars.AddPropertiesFieldsOfType(he.journalEntry, prefix + "Class_");
        }
    }
}
