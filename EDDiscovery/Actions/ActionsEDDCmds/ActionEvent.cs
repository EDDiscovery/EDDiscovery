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

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<BaseUtils.TypeHelpers.PropertyNameInfo> eventvars)
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
            if (ap.Functions.ExpandString(UserData, out res) != Functions.ExpandResult.Failed)
            {
                HistoryList hl = (ap.ActionController as ActionController).HistoryList;
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
                        HistoryEntry he = (ap.ActionController as ActionController).DiscoveryForm.PrimaryCursor.GetCurrentHistoryEntry;

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
                        ReportEntry(ap, hl.EntryOrder(), jidindex, prefix);
                    }
                    else
                        ap.ReportError("No commands in Event");

                    return true;
                }

                bool fwd = cmdname.Equals("forward") || cmdname.Equals("first");
                bool back = cmdname.Equals("backward") || cmdname.Equals("last");

                if (fwd || back)
                {
                    List<string> eventnames = sp.NextOptionallyBracketedList();     // single entry, list of events

                    bool not = eventnames.Count == 1 && eventnames[0].Equals("NOT", StringComparison.InvariantCultureIgnoreCase);       // if it goes NOT

                    if ( not )
                        eventnames = sp.NextOptionallyBracketedList();     // then get another list

                    // is it "WHERE"
                    bool whereasfirst = eventnames.Count == 1 && eventnames[0].Equals("WHERE", StringComparison.InvariantCultureIgnoreCase);

                    ConditionLists cond = new ConditionLists();
                    string nextword;

                    // if WHERE cond, or eventname WHERE cond
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
                        hltest = hl.EntryOrder(); // the whole list
                    else if (fwd)
                        hltest = hl.EntryOrder().GetRange(jidindex + 1, hl.Count - (jidindex + 1));       // cut down list, excluding this entry
                    else
                        hltest = hl.EntryOrder().GetRange(0, jidindex );

                    if (eventnames.Count > 0)       // screen out event names
                        hltest = (from h in hltest where eventnames.Contains(h.journalEntry.EventTypeStr, StringComparer.OrdinalIgnoreCase) == !not select h).ToList();
                    
                    if (cond.Count > 0)     // if we have filters, apply, filter out, true only stays
                        hltest = UserControls.HistoryFilterHelpers.CheckFilterTrue(hltest, cond, new Variables()); // apply filter..

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
                        HistoryEntry he = hl.EntryOrder()[jidindex];
                        ap[prefix + "JID"] = jidindex.ToStringInvariant();

                        if (cmdname.Equals("action"))
                        {
                            var ctrl = (ap.ActionController as ActionController);
                            int count = ctrl.ActionRunOnEntry(he, Actions.ActionEventEDList.EventCmd(he), now: true);
                            ap[prefix + "Count"] = count.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else if (cmdname.Equals("edsm"))
                        {
                            EliteDangerousCore.EDSM.EDSMClass edsm = new EliteDangerousCore.EDSM.EDSMClass();
                            string url = edsm.GetUrlCheckSystemExists(he.System.Name);

                            ap[prefix + "URL"] = url;

                            if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                                BaseUtils.BrowserInfo.LaunchBrowser(url);
                        }
                        else if (cmdname.Equals("ross"))
                        {
                            ap.ReportError("Not implemented");
                        }
                        else if (cmdname.Equals("eddb"))
                        {
                            string url = Properties.Resources.URLEDDBSystemName + System.Web.HttpUtility.UrlEncode(he.System.Name);
                            BaseUtils.BrowserInfo.LaunchBrowser(url);
                            ap[prefix + "URL"] = url;
                        }
                        else if (cmdname.Equals("info"))
                        {
                            ActionVars.SystemVarsFurtherInfo(ap, hl, he.System, prefix);
                            ActionVars.ShipModuleInformation(ap, he.ShipInformation, prefix);       // protected against SI being null
                        }
                        else if (cmdname.Equals("missions"))
                        {
                            ActionVars.MissionInformation(ap, hl.MissionListAccumulator.GetMissionList(he.MissionList), prefix);
                        }
                        else if (cmdname.Equals("setstartmarker"))
                        {
                            he.journalEntry.SetStartFlag();
                        }
                        else if (cmdname.Equals("setstopmarker"))
                        {
                            he.journalEntry.SetEndFlag();
                        }
                        else if (cmdname.Equals("clearstartstopmarker"))
                        {
                            he.journalEntry.ClearStartEndFlag();
                        }
                        else if (cmdname.Equals("note"))
                        {
                            string note = sp.NextQuotedWord();
                            if (note != null && sp.IsEOL)
                            {
                                he.journalEntry.UpdateSystemNote(note, he.System.Name, EDCommander.Current.SyncToEdsm);
                                (ap.ActionController as ActionController).DiscoveryForm.NoteChanged(this, he);
                            }
                            else
                                ap.ReportError("Missing note text or unquoted text in Event NOTE");
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
                    ActionVars.ShipBasicInformation(values, hl[pos].ShipInformation, prefix);   // protected against SI being null
                    ActionVars.SystemVars(values, hl[pos].System, prefix);
                    ap.Add(values);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception reporting entry variables in event " + ex.Message);
                }

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
