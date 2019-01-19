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
using System.Windows.Forms;
using BaseUtils;
using ActionLanguage;
using EliteDangerousCore;

namespace EDDiscovery.Actions
{
    public class ActionEventCmd : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Event get command", UserData, "Configure Event Command", cp.Icon);
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, out res) != Functions.ExpandResult.Failed)
            {
                HistoryList hl = (ap.actioncontroller as ActionController).HistoryList;
                StringParser sp = new StringParser(res);
                string prefix = "EC_";

                string cmdname = sp.NextWordLCInvariant(" ");

                if (cmdname != null && cmdname.Equals("prefix") )
                {
                    prefix = sp.NextWord();

                    if ( prefix == null )
                    {
                        ap.ReportError("Missing name after Prefix in Event");
                        return true;
                    }

                    cmdname = sp.NextWordLCInvariant(" ");
                }

                int jidindex = -1;

                if (cmdname!=null && (cmdname.Equals("from") || cmdname.Equals("thpos")))
                {
                    long? jid;

                    if (cmdname.Equals("thpos"))
                    {
                        HistoryEntry he = (ap.actioncontroller as ActionController).DiscoveryForm.PrimaryCursor.GetCurrentHistoryEntry;

                        if ( he == null )
                        {
                            ReportEntry(ap, null, 0, prefix);
                            return true;
                        }

                        jid = he.Journalid;
                    }
                    else
                    {
                        jid = sp.NextWord().InvariantParseLongNull();
                        if (!jid.HasValue)
                        {
                            ap.ReportError("Non integer JID after FROM in Event");
                            return true;
                        }
                    }

                    jidindex = hl.GetIndex(jid.Value);

                    if ( jidindex == -1 )
                    {
                        ReportEntry(ap, null, 0, prefix);
                        return true;
                    }

                    cmdname = sp.NextWordLCInvariant(" ");
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

                bool fwd = cmdname.Equals("forward") || cmdname.Equals("first");
                bool back = cmdname.Equals("backward") || cmdname.Equals("last");

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

                        string resc = cond.Read(sp.LineLeft);       // rest of it is the condition..
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
                        hltest = UserControls.FilterHelpers.CheckFilterTrue(hltest, cond, new Variables()); // apply filter..

                    if (fwd)
                        ReportEntry(ap, hltest, 0, prefix);
                    else
                        ReportEntry(ap, hltest, hltest.Count - 1, prefix);

                    return true;
                }
                else
                {
                    if (jidindex == -1)
                        ap.ReportError("Valid JID must be given for command " + cmdname + " in Event");
                    else
                    {
                        HistoryEntry he = hl.EntryOrder[jidindex];
                        ap[prefix + "JID"] = jidindex.ToStringInvariant();

                        if (cmdname.Equals("action"))
                        {
                            int count = (ap.actioncontroller as ActionController).ActionRunOnEntry(he, Actions.ActionEventEDList.EventCmd(he), now: true);
                            ap[prefix + "Count"] = count.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else if (cmdname.Equals("edsm"))
                        {
                            (ap.actioncontroller as ActionController).HistoryList.FillEDSM(he);

                            long? id_edsm = he.System.EDSMID;
                            if (id_edsm <= 0)
                            {
                                id_edsm = null;
                            }

                            EliteDangerousCore.EDSM.EDSMClass edsm = new EliteDangerousCore.EDSM.EDSMClass();
                            string url = edsm.GetUrlToEDSMSystem(he.System.Name, id_edsm);

                            ap[prefix + "URL"] = url;

                            if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                                System.Diagnostics.Process.Start(url);
                        }
                        else if (cmdname.Equals("ross"))
                        {
                            (ap.actioncontroller as ActionController).HistoryList.FillEDSM(he);

                            string url = "";

                            if (he.System.EDDBID > 0)
                            {
                                url = Properties.Resources.URLRossSystem + he.System.EDDBID.ToString();
                                System.Diagnostics.Process.Start(url);
                            }

                            ap[prefix + "URL"] = url;
                        }
                        else if (cmdname.Equals("eddb"))
                        {
                            (ap.actioncontroller as ActionController).HistoryList.FillEDSM(he);

                            string url = "";

                            if (he.System.EDDBID > 0)
                            {
                                url = Properties.Resources.URLEDDBSystem + he.System.EDDBID.ToString();
                                System.Diagnostics.Process.Start(url);
                            }

                            ap[prefix + "URL"] = url;
                        }
                        else if (cmdname.Equals("info"))
                        {
                            ActionVars.HistoryEventFurtherInfo(ap, hl, he, prefix);
                            ActionVars.SystemVarsFurtherInfo(ap, hl, he.System, prefix);
                            ActionVars.ShipModuleInformation(ap, he.ShipInformation, prefix);
                        }
                        else if (cmdname.Equals("missions"))
                        {
                            ActionVars.MissionInformation(ap, he.MissionList, prefix);
                        }
                        else if (cmdname.Equals("note"))
                        {
                            string note = sp.NextQuotedWord();
                            if (note != null)
                            {
                                he.SetJournalSystemNoteText(note, true, EDCommander.Current.SyncToEdsm);
                                (ap.actioncontroller as ActionController).DiscoveryForm.NoteChanged(this, he, true);
                            }
                            else
                                ap.ReportError("Missing note text in Event NOTE");
                        }
                        else
                            ap.ReportError("Unknown command " + cmdname + " in Event");
                    }
                }
            }
            else
                ap.ReportError(res);

            return true;
        }

        static void ReportEntry(ActionProgramRun ap, List<HistoryEntry> hl, int pos, string prefix)
        {
            if (hl != null && pos >= 0 && pos < hl.Count)     // if within range.. (1 based)
            {
                try
                {
                    Variables values = new Variables();
                    ActionVars.HistoryEventVars(values, hl[pos], prefix);
                    ActionVars.ShipBasicInformation(values, hl[pos].ShipInformation, prefix);
                    ActionVars.SystemVars(values, hl[pos].System, prefix);
                    ap.Add(values);
                }
                catch { }

                ap[prefix + "JID"] = hl[pos].Journalid.ToStringInvariant();
                ap[prefix + "Count"] = hl.Count.ToString(System.Globalization.CultureInfo.InvariantCulture);     // give a count of matches
            }
            else
            {
                ap[prefix + "JID"] = "0";
                ap[prefix + "Count"] = "0";
            }
        }
    }
}
