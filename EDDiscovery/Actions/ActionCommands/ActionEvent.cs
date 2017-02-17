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
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionEvent : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string promptValue = Forms.PromptSingleLine.ShowDialog(parent, discoveryform.theme, "Event get command", UserData, "Configure Event Command");
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
                HistoryList hl = ap.actioncontroller.HistoryList;
                StringParser sp = new StringParser(res);
                string prefix = "EC_";

                string cmdname = sp.NextWord(" ", true);

                // [PREFIX varprefix] [FROM JID] Forward/First/Backward/Last [event or (event list,event)] [WHERE conditions list]
                // FROM JID NEXT gives you the one after the JID
                // FROM JID BACKWARD gives you the JID before the JID
                // FROM JID gives you the JID

                if (cmdname != null && cmdname.Equals("prefix") )
                {
                    prefix = sp.NextWord();

                    if ( prefix == null )
                    {
                        ap.ReportError("Missing name after Prefix in Event");
                        return true;
                    }

                    cmdname = sp.NextWord(" " , true);
                }

                int jidindex = -1;

                if (cmdname!=null && (cmdname.Equals("from") || cmdname.Equals("thpos")))
                {
                    long? jid;

                    if (cmdname.Equals("thpos"))
                    {
                        HistoryEntry he = ap.actioncontroller.DiscoveryForm.TravelControl.GetTravelHistoryCurrent;

                        if ( he == null )
                        {
                            ReportEntry(ap, null, 0, prefix);
                            return true;
                        }

                        jid = he.Journalid;
                    }
                    else
                    {
                        jid = sp.GetLong();
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

                    cmdname = sp.NextWord(" ", true);
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
                {
                    if (jidindex == -1)
                        ap.ReportError("Valid JID must be given for command " + cmdname + " in Event");
                    else if (cmdname.Equals("action"))
                    {
                        int count = ap.actioncontroller.ActionRunOnEntry(hl.EntryOrder[jidindex], "ActionProgram", now:true);
                        ap.currentvars[prefix + "Count"] = count.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (cmdname.Equals("edsm"))
                    {
                        HistoryEntry he = hl.EntryOrder[jidindex];
                        ap.actioncontroller.HistoryList.FillEDSM(he, reload: true);

                        long? id_edsm = he.System.id_edsm;
                        if (id_edsm <= 0)
                        {
                            id_edsm = null;
                        }

                        EDDiscovery2.EDSM.EDSMClass edsm = new EDDiscovery2.EDSM.EDSMClass();
                        string url = edsm.GetUrlToEDSMSystem(he.System.name, id_edsm);

                        ap.currentvars[prefix + "URL"] = url;

                        if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                            System.Diagnostics.Process.Start(url);
                    }
                    else if (cmdname.Equals("ross"))
                    {
                        HistoryEntry he = hl.EntryOrder[jidindex];
                        ap.actioncontroller.HistoryList.FillEDSM(he, reload: true);

                        string url = "";

                        if (he.System.id_eddb > 0)
                        {
                            url = "http://ross.eddb.io/system/update/" + he.System.id_eddb.ToString();
                            System.Diagnostics.Process.Start(url);
                        }

                        ap.currentvars[prefix + "URL"] = url;
                    }
                    else
                        ap.ReportError("Unknown command " + cmdname + " in Event");
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
                ReportHistoryEntry(ap, hl[pos], prefix);
                ap.currentvars[prefix + "Count"] = hl.Count.ToString(System.Globalization.CultureInfo.InvariantCulture);     // give a count of matches
            }
            else
            {
                ap.currentvars[prefix + "JID"] = "0";
                ap.currentvars[prefix + "Count"] = "0";
            }
        }

        static void ReportHistoryEntry(ActionProgramRun ap, HistoryEntry he, string prefix)
        {
            try
            {
                ConditionVariables values = new ConditionVariables();
                values.GetJSONFieldNamesAndValues(he.journalEntry.EventDataString, prefix + "JS_");
                ActionVars.HistoryEventVars(values, he, prefix);
                ap.currentvars.Add(values);

                System.Globalization.CultureInfo ct = System.Globalization.CultureInfo.InvariantCulture;

                ap.currentvars[prefix + "JID"] = he.Journalid.ToString(ct);

                ap.currentvars[prefix + "EDSMID"] = he.System.id_edsm.ToString(ct);
                ap.currentvars[prefix + "xpos"] = he.System.x.ToNANSafeString("0.###");
                ap.currentvars[prefix + "ypos"] = he.System.y.ToNANSafeString("0.###");
                ap.currentvars[prefix + "zpos"] = he.System.z.ToNANSafeString("0.###");

                ap.currentvars[prefix + "EDDBID"] = he.System.id_eddb.ToString(ct);
                ap.currentvars[prefix + "EDDBGovernment"] = he.System.government.ToNullUnknownString();
                ap.currentvars[prefix + "EDDBAllegiance"] = he.System.allegiance.ToNullUnknownString();
                ap.currentvars[prefix + "EDDBState"] = he.System.state.ToNullUnknownString();
                ap.currentvars[prefix + "EDDBSecurity"] = he.System.security.ToNullUnknownString();
                ap.currentvars[prefix + "EDDBPrimaryEconomy"] = he.System.primary_economy.ToNullUnknownString();
                ap.currentvars[prefix + "EDDBFaction"] = he.System.faction.ToNullUnknownString();
                ap.currentvars[prefix + "EDDBPopulation"] = he.System.population.ToString(ct);
                ap.currentvars[prefix + "EDDBNeedsPermit"] = (he.System.needs_permit != 0) ? "1" : "0";
            }
            catch { }
        }
    }
}
