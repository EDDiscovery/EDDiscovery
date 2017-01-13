using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionEvent : Action
    {
        public ActionEvent(string n, ActionType t, List<string> c, string ud, int lu) : base(n, t, c, ud, lu)
        {
        }

        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string promptValue = PromptSingleLine.ShowDialog(parent, "Event get command", UserData, "Configure Event Command");
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, ap.currentvars, out res) != ConditionLists.ExpandResult.Failed)
            {
                HistoryList hl = ap.historylist;
                res = res.Trim();

                string cmdname = NextWord(ref res);

                bool last = cmdname.Equals("LAST", StringComparison.InvariantCultureIgnoreCase);
                bool first = cmdname.Equals("FIRST", StringComparison.InvariantCultureIgnoreCase);

                if (last || first)
                {
                    string eventname = NextWord(ref res);

                    int count;
                    if (int.TryParse(eventname, out count))
                    {
                        List<HistoryEntry> hle = hl.EntryOrder;
                        FillEventVars(count, hle, ap, last);
                    }
                    else if (eventname.Length > 0)
                    {
                        string countstring = NextWord(ref res);

                        count = 1;
                        if (countstring.Length == 0 || int.TryParse(countstring, out count))
                        {
                            List<HistoryEntry> hle = (from h in hl.EntryOrder where h.journalEntry.EventTypeStr.Equals(eventname, StringComparison.InvariantCultureIgnoreCase) select h).ToList();
                            FillEventVars(count, hle, ap, last);
                        }
                        else
                            ap.ReportError("Non integer count after event name in Event");
                    }
                    else
                        ap.ReportError("Missing event name in Event");
                }
                else
                {
                    long id;        // by JID
                    if (long.TryParse(cmdname, out id))
                    {
                        List<HistoryEntry> hle = (from h in hl.EntryOrder where h.Journalid == id select h).ToList();
                        FillEventVars(1, hle, ap, false);
                    }
                    else
                        ap.ReportError("No an Journal ID Integer in Event");
                }
            }

            else
                ap.ReportError(res);

            return true;
        }

        void FillEventVars( int count, List<HistoryEntry> hl , ActionProgramRun ap, bool revorder )
        {
            string eventname = "Event";

            if (count >= 1 && count <= hl.Count)     // if within range.. (1 based)
            {
                count--;    // zero based now

                if (revorder)
                    count = hl.Count - 1 - count;

                try
                {
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    JSONHelper.GetJSONFieldNamesValues(hl[count].journalEntry.EventDataString, values);

                    ap.currentvars[eventname + "_LocalTime"] = hl[count].EventTimeLocal.ToString("MM/dd/yyyy HH:mm:ss");
                    ap.currentvars[eventname + "_JID"] = hl[count].Journalid.ToString();
                    ap.currentvars[eventname + "_Docked"] = hl[count].IsDocked ? "1" : "0";
                    ap.currentvars[eventname + "_Landed"] = hl[count].IsLanded ? "1" : "0";
                    ap.currentvars[eventname + "_WhereAmI"] = hl[count].WhereAmI;
                    ap.currentvars[eventname + "_ShipType"] = hl[count].ShipType;
                    foreach (KeyValuePair<string, string> k in values)
                        ap.currentvars[eventname + "_" + Tools.SafeFileString(k.Key)] = k.Value;

                    return;
                }
                catch
                {
                }
            }

            ap.currentvars[eventname + "_JID"] = "0";
        }

        string NextWord(ref string cmd)
        {
            int spaceafter = cmd.IndexOf(' ');
            if (spaceafter >= 0)
            {
                string res = cmd.Substring(0, spaceafter);
                cmd = cmd.Substring(spaceafter + 1).Trim();
                return res;
            }
            else
            {
                string res = cmd.Trim();
                cmd = "";
                return res;
            }
        }

    }
}
