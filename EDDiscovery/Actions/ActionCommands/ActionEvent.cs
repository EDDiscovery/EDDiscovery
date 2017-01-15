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
                Tools.StringParser sp = new Tools.StringParser(res);

                string cmdname = sp.NextWord();

                bool last = cmdname.Equals("LAST", StringComparison.InvariantCultureIgnoreCase);
                bool first = cmdname.Equals("FIRST", StringComparison.InvariantCultureIgnoreCase);
                bool nextjid = cmdname.Equals("NEXT", StringComparison.InvariantCultureIgnoreCase);
                bool previousjid = cmdname.Equals("PREVIOUS", StringComparison.InvariantCultureIgnoreCase);

                if (last || first)
                {
                    List<string> eventnames = sp.NextOptionallyBracketedList();

                    int count;
                    if (eventnames.Count == 1 && int.TryParse(eventnames[0], out count))
                    {
                        List<HistoryEntry> hle = hl.EntryOrder;
                        FillEventVars(count, hle, ap, last);
                    }
                    else if (eventnames.Count>0)
                    {
                        string countstring = sp.NextWord();

                        count = 1;
                        if (countstring == null || int.TryParse(countstring, out count))
                        {
                            List<HistoryEntry> hle = (from h in hl.EntryOrder where eventnames.Contains(h.journalEntry.EventTypeStr, StringComparer.OrdinalIgnoreCase) select h).ToList();

                            FillEventVars(count, hle, ap, last);
                        }
                        else
                            ap.ReportError("Non integer count after event name in Event");
                    }
                    else
                        ap.ReportError("Missing event name or name list in Event");
                }
                else if (nextjid || previousjid )
                {
                    string jidstring = sp.NextWord();

                    int jid;
                    if (jidstring != null && int.TryParse(jidstring, out jid))
                    {
                        string countstring = sp.NextWord();

                        int count = 1;
                        if (countstring == null || int.TryParse(countstring, out count))
                        {
                            int indexof = hl.EntryOrder.FindIndex(x => x.Journalid == jid);
                            if (indexof != -1)
                            {
                                if (nextjid)
                                    indexof += count;
                                else
                                    indexof -= count;
                            }

                            FillEventVars(indexof + 1, hl.EntryOrder, ap, last);    // 1 based system..
                        }
                        else
                            ap.ReportError("Non integer count after JID number in Event");
                    }
                    else
                        ap.ReportError("Missing JID after NEXT or PREVIOUS in Event");
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
                        ap.ReportError("Missing JID in Event");
                }
            }

            else
                ap.ReportError(res);

            return true;
        }

        void FillEventVars( int count, List<HistoryEntry> hl , ActionProgramRun ap, bool revorder )
        {
            string prefix = "EC_";

            if (count >= 1 && count <= hl.Count)     // if within range.. (1 based)
            {
                count--;    // zero based now

                if (revorder)
                    count = hl.Count - 1 - count;

                try
                {
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    JSONHelper.GetJSONFieldNamesValues(hl[count].journalEntry.EventDataString, values,prefix);
                    EDDiscoveryForm.HistoryEntryVars(hl[count], values, prefix);
                    foreach (KeyValuePair<string, string> k in values)
                        ap.currentvars[Tools.SafeFileString(k.Key)] = k.Value;

                    return;
                }
                catch
                {
                }
            }

            ap.currentvars[prefix + "JID"] = "0";
        }

    }
}
