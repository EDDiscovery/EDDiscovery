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
                StringParser sp = new StringParser(res);
                string prefix = "EC_";

                string cmdname = sp.NextWord();

                // [PREFIX varprefix] [FROM JID] Forward/First/Backward/Last [event or (event list,event)] [WHERE conditions list]
                // FROM JID NEXT gives you the one after the JID
                // FROM JID BACKWARD gives you the JID before the JID
                // FROM JID gives you the JID

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if ( prefix == null )
                    {
                        ap.ReportError("Missing name after Prefix in Event");
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

                int jidindex = -1;

                if (cmdname!=null && cmdname.Equals("FROM", StringComparison.InvariantCultureIgnoreCase))
                {
                    string j = sp.NextWord();

                    long jid;
                    if (j == null || !long.TryParse(j, out jid))
                    {
                        ap.ReportError("Non integer JID after FROM in Event");
                        return true;
                    }

                    jidindex = hl.EntryOrder.FindIndex(x => x.Journalid == jid);

                    if ( jidindex == -1 )
                    {
                        ReportEntry(ap, null, 0,prefix);
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

                if (cmdname == null)
                {
                    if (jidindex != -1)
                    {
                        ReportEntry(ap, hl.EntryOrder, jidindex, prefix);
                    }
                    else
                        ap.ReportError("No commands in Event");

                    return true;
                }


                bool fwd = cmdname.Equals("FORWARD", StringComparison.InvariantCultureIgnoreCase) || cmdname.Equals("FIRST", StringComparison.InvariantCultureIgnoreCase);
                bool back = cmdname.Equals("BACKWARD", StringComparison.InvariantCultureIgnoreCase) || cmdname.Equals("LAST", StringComparison.InvariantCultureIgnoreCase);

                if (fwd || back)
                {
                    List<string> eventnames = sp.NextOptionallyBracketedList();
                    bool whereasfirst = eventnames.Count == 1 && eventnames[0].Equals("WHERE", StringComparison.InvariantCultureIgnoreCase);

                    ConditionLists cond = new ConditionLists();
                    string nextword;

                    if ( whereasfirst || ((nextword = sp.NextWord()) != null && nextword.Equals("WHERE", StringComparison.InvariantCultureIgnoreCase) ))
                    {
                        if ( whereasfirst )     // clear out event names if it was WHERE cond..
                            eventnames.Clear();

                        string resc = cond.FromString(sp.LineLeft);       // rest of it is the condition..
                        if (resc != null)
                        {
                            ap.ReportError(resc + " in Where of Event");
                            return true;
                        }
                    }

                    List<HistoryEntry> hltest;

                    if (jidindex == -1)     // if no JID given..
                        hltest = hl.EntryOrder; // the whole list
                    else if (fwd)
                        hltest = hl.EntryOrder.GetRange(jidindex + 1, hl.Count - (jidindex + 1));       // cut down list, excluding this entry
                    else
                        hltest = hl.EntryOrder.GetRange(0, jidindex );

                    if (eventnames.Count > 0)
                        hltest = (from h in hltest where eventnames.Contains(h.journalEntry.EventTypeStr, StringComparer.OrdinalIgnoreCase) select h).ToList();
                    
                    if (cond.Count > 0)     // if we have filters, apply, filter out, true only stays
                        hltest = cond.FilterHistoryOut(hltest, new ConditionVariables()); // apply filter..

                    if (fwd)
                        ReportEntry(ap, hltest, 0, prefix);
                    else
                        ReportEntry(ap, hltest, hltest.Count - 1, prefix);

                    return true;
                }
                else
                    ap.ReportError("Unknown command " + cmdname + " in Event");
            }
            else
                ap.ReportError(res);

            return true;
        }

        void ReportEntry(ActionProgramRun ap, List<HistoryEntry> hl , int pos, string prefix)
        {
            if (hl != null && pos>=0 && pos < hl.Count)     // if within range.. (1 based)
            {
                try
                {
                    ConditionVariables values = new ConditionVariables();
                    values.GetJSONFieldNamesAndValues(hl[pos].journalEntry.EventDataString, prefix + "JS_");
                    ActionVars.HistoryEventVars(values, hl[pos], prefix);
                    ap.currentvars.Add(values);
                    ap.currentvars[prefix + "Count"] = hl.Count.ToString();     // give a count of matches
                    return;
                }
                catch
                {
                }
            }

            ap.currentvars[prefix + "JID"] = "0";
            ap.currentvars[prefix + "Count"] = "0";
        }

    }
}
