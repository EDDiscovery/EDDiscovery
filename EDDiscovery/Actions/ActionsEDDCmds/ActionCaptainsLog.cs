/*
 * Copyright © 2017-2019 EDDiscovery development team
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
using BaseUtils;
using ActionLanguage;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.Actions
{
    public class ActionCaptainsLog : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(System.Windows.Forms.Form parent, ActionCoreController cp, List<string> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Command string", UserData, "Configure Captains Log Command" , cp.Icon);
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string res;
            if (ap.functions.ExpandString(UserData, out res) != BaseUtils.Functions.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(res);

                string prefix = "CL_";
                string cmdname = sp.NextWord();

                if (cmdname != null && cmdname.Equals("PREFIX", StringComparison.InvariantCultureIgnoreCase))
                {
                    prefix = sp.NextWord();

                    if (prefix == null)
                    {
                        ap.ReportError("Missing name after Prefix");
                        return true;
                    }

                    cmdname = sp.NextWord();
                }

                int cmdrid = EDCommander.CurrentCmdrID;

                if (cmdname != null && cmdname.Equals("CMDR", StringComparison.InvariantCultureIgnoreCase))
                {
                    string name = sp.NextQuotedWord() ?? "-----!";
                    
                    EDCommander cmdr = EDCommander.GetCommander(name);

                    if (cmdr != null)
                        cmdrid = cmdr.Nr;
                    else
                        ap.ReportError("Commander not found");

                    cmdname = sp.NextWord();
                }

                List<CaptainsLogClass> cllist = GlobalCaptainsLogList.Instance.LogEntriesCmdrTimeOrder(cmdrid);

                EDDiscoveryForm discoveryform = (ap.actioncontroller as ActionController).DiscoveryForm;

                if (cmdname != null)
                {
                    if (cmdname.Equals("LIST", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string wildcard = sp.NextQuotedWord() ?? "*";

                        Func<CaptainsLogClass, string, bool> validate = CheckSystemBody;

                        string field = sp.NextQuotedWord() ?? "--";
                        if (field.Equals("Body", StringComparison.InvariantCultureIgnoreCase))
                            validate = CheckBody;
                        else if (field.Equals("System", StringComparison.InvariantCultureIgnoreCase))
                            validate = CheckSystem;
                        else if (field.Equals("Tag", StringComparison.InvariantCultureIgnoreCase))
                            validate = CheckTags;
                        else if (field.Equals("Note", StringComparison.InvariantCultureIgnoreCase))
                            validate = CheckNote;
                        else if ( field != "--")
                        {
                            ap.ReportError("Unknown field type to list");
                            return true;
                        }

                        int count = 1;
                        foreach (CaptainsLogClass cl in cllist)       // only current commander ID considered
                        {
                            if (validate(cl,wildcard))
                            {
                                DumpCL(ap, prefix + count++.ToStringInvariant() + "_", cl);
                            }
                        }

                        ap[prefix + "MatchCount"] = (count-1).ToStringInvariant();
                        ap[prefix + "TotalCount"] = cllist.Count.ToStringInvariant();
                    }
                    else if (cmdname.Equals("ADDHERE", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string note = sp.NextQuotedWord();
                        string taglist = sp.NextQuotedWord();

                        HistoryEntry he = discoveryform.history.GetLast;

                        if ( he != null )
                        {       // taglist can be null.. note must be set.
                            GlobalCaptainsLogList.Instance.AddOrUpdate(null, cmdrid, he.System.Name, he.WhereAmI, DateTime.UtcNow, note ?? "", taglist);
                        }
                        else
                        {
                            ap.ReportError("History has no locations");
                        }
                    }
                    else if (cmdname.Equals("ADD", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string systemname = sp.NextQuotedWord();
                        string bodyname = sp.NextQuotedWord();
                        DateTime? dte = sp.NextDateTime(System.Globalization.CultureInfo.GetCultureInfo("en-us"), System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal);
                        string note = sp.NextQuotedWord();
                        string taglist = sp.NextQuotedWord();

                        if ( systemname != null && bodyname != null && dte != null )
                        {
                            GlobalCaptainsLogList.Instance.AddOrUpdate(null, cmdrid, systemname, bodyname, dte.Value, note ?? "", taglist);
                        }
                        else
                        {
                            ap.ReportError("Missing parameters in ADD");
                        }

                    }
                    else if (cmdname.Equals("TAGLIST", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string tags = EDDConfig.Instance.CaptainsLogTags;
                        ap[prefix + "Tags"] = tags;
                    }
                    else if (cmdname.Equals("SETTAGLIST", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string tags = sp.NextQuotedWord();
                        if (tags != null)
                            EDDConfig.Instance.CaptainsLogTags = tags;
                        else
                            ap.ReportError("Missing tag list");
                    }
                    else
                    {   // ********************** Iterator forms, FROM/LAST/FIRST/TIME [Forward|Backward]
                        long? cid = -1;

                        if (cmdname.Equals("From", StringComparison.InvariantCultureIgnoreCase))
                        {
                            cid = sp.NextWord().InvariantParseLongNull();

                            if (cid == null)
                            {
                                ap.ReportError("Non integer CID after FROM");
                                return true;
                            }
                        }
                        else if (cmdname.Equals("First", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (cllist.Count > 0) //prevent crash if cllist is empty
                            {
                                var mintime = cllist.Min(x => x.TimeUTC);
                                cid = cllist.First(x => x.TimeUTC == mintime).ID;
                            }
                        }
                        else if (cmdname.Equals("Last", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (cllist.Count > 0)  
                            {
                                var maxtime = cllist.Max(x => x.TimeUTC);
                                cid = cllist.Last(x => x.TimeUTC == maxtime).ID;
                            }
                        }
                        else if (cmdname.Equals("Time", StringComparison.InvariantCultureIgnoreCase))
                        {
                            DateTime? dte = sp.NextDateTime(System.Globalization.CultureInfo.GetCultureInfo("en-us"), System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal);
                            if (dte != null)
                            {
                                if (cllist.Count > 0)
                                {
                                    var firstafter = cllist.FirstOrDefault(x => x.TimeUTC >= dte);

                                    if (firstafter != null)
                                        cid = firstafter.ID;
                                }
                            }
                            else
                            {
                                ap.ReportError("Missing US date from Time");
                                return true;
                            }
                        }
                        else
                        { 
                            ap.ReportError("Unknown command");
                            return true;
                        }

                        int indexof = cllist.FindIndex(x => x.ID == cid);   // -1 if not found..

                        string nextcmd = sp.NextWord();

                        if (nextcmd != null)
                        {
                            if (nextcmd.Equals("FORWARD", StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (indexof >= 0)   // don't ruin -1 if set
                                    indexof++;

                                nextcmd = sp.NextWord();
                            }
                            else if (nextcmd.Equals("BACKWARD", StringComparison.InvariantCultureIgnoreCase))
                            {
                                indexof--;      // if -1, its okay to make it -2.

                                nextcmd = sp.NextWord();
                            }
                        }

                        bool validindex = indexof >= 0 && indexof < cllist.Count;

                        if (nextcmd != null)
                        {
                            if (!validindex)     // these must have a valid target..
                            {
                                ap.ReportError("Entry is not found");
                            }
                            else
                            {
                                CaptainsLogClass cl = cllist[indexof];

                                if (nextcmd.Equals("DELETE", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    GlobalCaptainsLogList.Instance.Delete(cl);
                                }
                                else
                                {
                                    string text = sp.NextQuotedWord();

                                    if (text != null)
                                    {
                                        if (nextcmd.Equals("NOTE", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            GlobalCaptainsLogList.Instance.AddOrUpdate(cl, cl.Commander, cl.SystemName, cl.BodyName, cl.TimeUTC,
                                                                                       text, cl.Tags, cl.Parameters);
                                        }
                                        else if (nextcmd.Equals("SYSTEM", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            GlobalCaptainsLogList.Instance.AddOrUpdate(cl, cl.Commander, text, cl.BodyName, cl.TimeUTC,
                                                                                       cl.Note, cl.Tags, cl.Parameters);
                                        }
                                        else if (nextcmd.Equals("BODY", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            GlobalCaptainsLogList.Instance.AddOrUpdate(cl, cl.Commander, cl.SystemName, text, cl.TimeUTC,
                                                                                       cl.Note, cl.Tags, cl.Parameters);
                                        }
                                        else
                                            ap.ReportError("Unknown command " + nextcmd);
                                    }
                                    else
                                        ap.ReportError("Missing text after " + nextcmd);
                                }
                            }

                            return true;
                        }
                        
                                   
                        if (nextcmd != null)
                        {
                            ap.ReportError("Unknown iterator or command " + nextcmd);
                        }
                        else
                        {       // straight report
                            if (validindex)
                            {
                                DumpCL(ap, prefix, cllist[indexof]);
                            }
                            else
                            {
                                ap[prefix + "Id"] = "-1";
                            }
                        }

                        return true;
                    }
                }
                else
                    ap.ReportError("Missing command");
            }
            else
                ap.ReportError(res);

            return true;
        }

        void DumpCL( ActionProgramRun ap, string prefix, CaptainsLogClass cl)
        {
            ap[prefix + "Id"] = cl.ID.ToStringInvariant();
            ap[prefix + "TimeUTC"] = cl.TimeUTC.ToStringUS();
            ap[prefix + "TimeLocal"] = cl.TimeLocal.ToStringUS();
            ap[prefix + "SystemName"] = cl.SystemName ?? "";
            ap[prefix + "BodyName"] = cl.BodyName ?? "";
            ap[prefix + "Note"] = cl.Note ?? "";
            ap[prefix + "Tags"] = cl.Tags ?? "";
        }

        bool CheckSystemBody(CaptainsLogClass cl, string match)
        {
            return cl.SystemName.WildCardMatch(match) || cl.BodyName.WildCardMatch(match);
        }
        bool CheckSystem(CaptainsLogClass cl, string match)
        {
            return cl.SystemName.WildCardMatch(match);
        }
        bool CheckBody(CaptainsLogClass cl, string match)
        {
            return cl.BodyName.WildCardMatch(match);
        }
        bool CheckNote(CaptainsLogClass cl, string match)
        {
            return cl.Note != null && cl.Note.WildCardMatch(match);
        }
        bool CheckTags(CaptainsLogClass cl, string match)
        {
            return cl.Tags != null && cl.Tags.WildCardMatch(match);
        }
    }
}
