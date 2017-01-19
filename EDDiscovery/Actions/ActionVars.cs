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

            Type jtype = he.journalEntry.GetType();

            foreach (System.Reflection.PropertyInfo pi in jtype.GetProperties())
            {
                if (pi.GetIndexParameters().GetLength(0) == 0)      // only properties with zero parameters are called
                {
                    System.Reflection.MethodInfo getter = pi.GetGetMethod();
                    Type rettype = pi.PropertyType;
                    string name = prefix + "Class_" + pi.Name;
                    Extract(getter.Invoke(he.journalEntry, null), rettype, vars, name);
                }
            }

            foreach (System.Reflection.FieldInfo fi in jtype.GetFields())
            {
                System.Diagnostics.Debug.WriteLine("Field " + fi.Name);
                Object o = fi.GetValue(he.journalEntry);
                string name = prefix + "Class_" + fi.Name;
                Extract(o, fi.FieldType, vars, name);
            }
        }

        static void Extract(Object o, Type rettype, ConditionVariables vars, string name)
        {
            try // just to make sure a strange type does not barfe it
            {
                if (o == null)
                    vars[name] = "";
                else if (!rettype.IsGenericType || rettype.GetGenericTypeDefinition() != typeof(Nullable<>))
                {
                    var v = Convert.ChangeType(o, rettype);
                    vars[name] = v.ToString();
                }
                else if (o is Nullable<double>)
                    ExtractNull<double>((Nullable<double>)o, vars, name);
                else if (o is Nullable<bool>)
                    ExtractNull<bool>((Nullable<bool>)o, vars, name);
                else if (o is Nullable<int>)
                    ExtractNull<int>((Nullable<int>)o, vars, name);
                else if (o is Nullable<long>)
                    ExtractNull<long>((Nullable<long>)o, vars, name);
            }
            catch { }   
        }

        static void ExtractNull<T>(Nullable<T> obj, ConditionVariables vars, string name) where T : struct
        {
            if (obj.HasValue)
                Extract(obj.Value, typeof(T), vars, name);
            else
                vars[name] = "";
        }
    }
}
