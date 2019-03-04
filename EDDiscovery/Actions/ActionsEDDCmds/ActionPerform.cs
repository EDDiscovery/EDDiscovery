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
using System.Windows.Forms;
using ActionLanguage;
using BaseUtils;

namespace EDDiscovery.Actions
{
    public class ActionPerform : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        List<string> FromString(string input)       // returns in raw esacped mode
        {
            StringParser sp = new StringParser(input);
            List<string> s = sp.NextQuotedWordList(separoptional:true);
            return (s != null && s.Count >= 1 ) ? s : null;
        }

        public override string VerifyActionCorrect()
        {
            return (FromString(userdata) != null) ? null : "Perform command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<BaseUtils.TypeHelpers.PropertyNameInfo> eventvars)
        {
            string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(parent, "Perform command", UserData, "Configure Perform Command" , cp.Icon);
            if (promptValue != null)
            {
                userdata = promptValue;
            }

            return (promptValue != null);
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            List<string> ctrl = FromString(UserData);

            if (ctrl != null)
            {
                List<string> exp;

                if (ap.functions.ExpandStrings(ctrl, out exp) != Functions.ExpandResult.Failed)
                {
                    string cmdname = exp[0].ToLowerInvariant();
                    string nextword = exp.Count >= 2 ? exp[1] : null;
                    string thirdword = exp.Count >= 3 ? exp[2] : null;
                    string fourthword = exp.Count >= 4 ? exp[3] : null;
                    string fifthword = exp.Count >= 5 ? exp[4] : null;

                    if (cmdname == null)
                    {
                        ap.ReportError("Missing command in Perform");
                    }
                    else if (cmdname.Equals("3dmap"))
                    {
                        (ap.actioncontroller as ActionController).DiscoveryForm.Open3DMap(null);
                    }
                    else if (cmdname.Equals("2dmap"))
                    {
                        (ap.actioncontroller as ActionController).DiscoveryForm.Open2DMap();
                    }
                    else if (cmdname.Equals("edsm"))
                    {
                        ActionController ac = (ap.actioncontroller as ActionController);

                        EliteDangerousCore.EDSM.EDSMClass edsm = new EliteDangerousCore.EDSM.EDSMClass();

                        if (edsm.ValidCredentials)
                            EliteDangerousCore.EDSM.EDSMJournalSync.SendEDSMEvents(ap.actioncontroller.LogLine, ac.DiscoveryForm.history);
                        else
                            ap.ReportError("No valid EDSM Credentials");
                    }
                    else if (cmdname.Equals("refresh"))
                    {
                        (ap.actioncontroller as ActionController).DiscoveryForm.RefreshHistoryAsync();
                    }
                    else if (cmdname.Equals("url"))
                    {
                        if (nextword != null && nextword.StartsWith("http:", StringComparison.InvariantCultureIgnoreCase) || nextword.StartsWith("https:", StringComparison.InvariantCultureIgnoreCase))        // security..
                        {
                            System.Diagnostics.Process.Start(nextword);
                        }
                        else
                            ap.ReportError("Perform url must start with http");
                    }
                    else if (cmdname.Equals("configurevoice"))
                    {
                        (ap.actioncontroller as ActionController).ConfigureVoice(nextword ?? "Configure Voice Synthesis");
                    }
                    else if (cmdname.Equals("manageaddons"))
                        (ap.actioncontroller as ActionController).ManageAddOns();
                    else if (cmdname.Equals("editaddons"))
                        (ap.actioncontroller as ActionController).EditAddOns();
                    else if (cmdname.Equals("editlastpack"))
                        (ap.actioncontroller as ActionController).EditLastPack();
                    else if (cmdname.Equals("editpack"))
                    {
                        if (nextword != null)
                        {
                            if (!(ap.actioncontroller as ActionController).EditPack(nextword))
                                ap.ReportError("Pack " + nextword + " not found");
                        }
                        else
                            ap.ReportError("EditPack requires a pack name");
                    }
                    else if (cmdname.Equals("editspeechtext"))
                        (ap.actioncontroller as ActionController).EditSpeechText();
                    else if (cmdname.Equals("configurewave"))
                        (ap.actioncontroller as ActionController).ConfigureWave(nextword ?? "Configure Wave Output");
                    else if (cmdname.Equals("enableeliteinput"))
                        (ap.actioncontroller as ActionController).EliteInput(true, true);
                    else if (cmdname.Equals("enableeliteinputnoaxis"))
                        (ap.actioncontroller as ActionController).EliteInput(true, false);
                    else if (cmdname.Equals("disableeliteinput"))
                        (ap.actioncontroller as ActionController).EliteInput(false, false);
                    else if (cmdname.Equals("enablevoicerecognition"))
                    {
                        if (nextword != null)
                        {
                            ap["VoiceRecognitionEnabled"] = ((ap.actioncontroller as ActionController).VoiceReconOn(nextword)).ToStringIntValue();
                        }
                        else
                            ap.ReportError("EnableVoiceRecognition requires a culture");
                    }
                    else if (cmdname.Equals("disablevoicerecognition"))
                        (ap.actioncontroller as ActionController).VoiceReconOff();
                    else if (cmdname.Equals("beginvoicerecognition"))
                        (ap.actioncontroller as ActionController).VoiceLoadEvents();
                    else if (cmdname.Equals("voicerecognitionconfidencelevel"))
                    {
                        float? conf = nextword.InvariantParseFloatNull();
                        if (conf != null)
                            (ap.actioncontroller as ActionController).VoiceReconConfidence(conf.Value);
                        else
                            ap.ReportError("VoiceRecognitionConfidencelLevel requires a confidence value");
                    }
                    else if (cmdname.Equals("voicerecognitionparameters"))
                    {
                        int? babble = nextword.InvariantParseIntNull();        // babble at end
                        int? initialsilence = thirdword.InvariantParseIntNull(); // silence at end
                        int? endsilence = fourthword.InvariantParseIntNull();        // unambigious timeout
                        int? endsilenceambigious = fifthword.InvariantParseIntNull(); // ambiguous timeout

                        if (babble != null && initialsilence != null && endsilence != null && endsilenceambigious != null)
                            (ap.actioncontroller as ActionController).VoiceReconParameters(babble.Value, initialsilence.Value, endsilence.Value, endsilenceambigious.Value);
                        else
                            ap.ReportError("VoiceRecognitionParameters requires four values");
                    }
                    else if (cmdname.Equals("voicerecognitionphrases"))
                    {
                        ap["Phrases"] = (ap.actioncontroller as ActionController).VoicePhrases(Environment.NewLine);
                    }
                    else if (cmdname.Equals("listeliteinput"))
                    {
                        ap["EliteInput"] = (ap.actioncontroller as ActionController).EliteInputList();
                        ap["EliteInputCheck"] = (ap.actioncontroller as ActionController).EliteInputCheck();
                    }
                    else if (cmdname.Equals("voicenames"))
                    {
                        ap["VoiceNames"] = (ap.actioncontroller as ActionController).SpeechSynthesizer.GetVoiceNames().QuoteStrings();
                    }
                    else if (cmdname.Equals("bindings"))
                    {
                        ap["Bindings"] = (ap.actioncontroller as ActionController).FrontierBindings.ListBindings();
                    }
                    else if (cmdname.Equals("bindingvalues"))
                    {
                        ap["BindingValues"] = (ap.actioncontroller as ActionController).FrontierBindings.ListValues();
                    }
                    else if (cmdname.Equals("actionfile"))
                    {
                        ActionFile f = (ap.actioncontroller as ActionController).Get(nextword);
                        if ( f != null )
                        {
                            int i = 0;
                            foreach( var x in f.actioneventlist.Enumerable )
                            {
                                ap["Events[" + i++ + "]"] = x.ToString(true);   // list hooked events
                                ap["Events_" + x.eventname] = x.ToString(true);   // list hooked events
                            }

                            i = 0;
                            foreach (string jname in Enum.GetNames(typeof(EliteDangerousCore.JournalTypeEnum)))
                            {
                                List<Condition> cl = f.actioneventlist.GetConditionListByEventName(jname);

                                if (cl != null)
                                {
                                    int v = 0;
                                    foreach (var c in cl)
                                    {
                                        ap["JEvents[" + i++ + "]"] = c.ToString(true);
                                        ap["JEvents_" + c.eventname + "_" + v++] = c.ToString(true);
                                    }
                                }
                                else
                                {
                                    ap["JEvents[" + i++ + "]"] = jname + ", None";
                                    ap["JEvents_" + jname] = "None";
                                }
                            }

                            i = 0;
                            foreach (string iname in Enum.GetNames(typeof(EliteDangerousCore.UITypeEnum)))
                            {
                                List<Condition> cl = f.actioneventlist.GetConditionListByEventName("UI" + iname);

                                if (cl != null)
                                {
                                    int v = 0;
                                    foreach (var c in cl)
                                    {
                                        ap["UIEvents[" + i++ + "]"] = c.ToString(true);
                                        ap["UIEvents_" + c.eventname + "_" + v++] = c.ToString(true);
                                    }
                                }
                                else
                                {
                                    ap["UIEvents[" + i++ + "]"] = iname + ", None";
                                    ap["UIEvents_" + iname] = "None";
                                }
                            }

                            i = 0;
                            foreach (var x in f.installationvariables.NameEnumuerable)
                                ap["Install[" + i++ + "]"] = x + "," + f.installationvariables[x];   // list hooked events

                            i = 0;
                            foreach (var x in f.filevariables.NameEnumuerable)
                                ap["FileVar[" + i++ + "]"] = x + "," + f.filevariables[x];   // list hooked events

                            ap["Enabled"] = f.enabled.ToStringIntValue();

                        }
                        else
                            ap.ReportError("Action file " + nextword + " is not loaded");
                    }
                    else if (cmdname.Equals("datadownload"))
                    {
                        string gitfolder = nextword;
                        string filewildcard = thirdword;
                        string directory = fourthword;
                        string optclean = fifthword;

                        if (gitfolder != null && filewildcard != null && directory != null)
                        {
                            if (System.IO.Directory.Exists(directory))
                            {
                                BaseUtils.GitHubClass ghc = new BaseUtils.GitHubClass(EDDiscovery.Properties.Resources.URLGithubDataDownload);
                                bool worked = ghc.Download(directory, gitfolder, filewildcard, optclean != null && optclean == "1");
                                ap["Downloaded"] = worked.ToStringIntValue();
                            }
                            else
                                ap.ReportError("Download folder " + directory + " does not exist");
                        }
                        else
                            ap.ReportError("Missing parameters in Perform Datadownload");

                    }
                    else if (cmdname.Equals("generateevent"))
                    {
                        if (nextword != null)
                        {
                            ActionEvent f = ActionEventEDList.EventList(excludejournal: true).Find(x => x.TriggerName.Equals(nextword));

                            if (f != null)
                            {
                                BaseUtils.Variables c = new BaseUtils.Variables();

                                for ( int w = 2; w < exp.Count; w++ )
                                {
                                    string vname = exp[w];
                                    int asterisk = vname.IndexOf('*');

                                    if ( asterisk >=0 )     // pass in name* no complaining if not there
                                    {
                                        string prefix = vname.Substring(0, asterisk);

                                        foreach (string jkey in ap.variables.NameEnumuerable)
                                        {
                                            if (jkey.StartsWith(prefix))
                                                c[jkey] = ap.variables[jkey];
                                        }
                                    }
                                    else
                                    {
                                        if (ap.variables.Exists(vname))     // pass in explicit name
                                            c[vname] = ap.variables[vname];
                                        else
                                        {
                                            ap.ReportError("No such variable '"  + vname  +"'");
                                            return true;
                                        }
                                    }
                                }
                                
                                if (f.TriggerName.StartsWith("UI") || f.TriggerName.Equals("onEliteUIEvent"))
                                {
                                    c["EventClass_EventTimeUTC"] = DateTime.UtcNow.ToStringUS();
                                    c["EventClass_EventTypeID"] = c["EventClass_EventTypeStr"] = f.TriggerName.Substring(2);
                                    c["EventClass_UIDisplayed"] = "0";
                                    (ap.actioncontroller as ActionController).ActionRun(Actions.ActionEventEDList.onUIEvent, c);
                                }

                                (ap.actioncontroller as ActionController).ActionRun(f, c, now: true);
                            }
                            else
                            {
                                try
                                {
                                    EliteDangerousCore.JournalEntry je = EliteDangerousCore.JournalEntry.CreateJournalEntry(nextword);

                                    ap["GenerateEventName"] = je.EventTypeStr;

                                    if (je is EliteDangerousCore.JournalEvents.JournalUnknown)
                                    {
                                        ap.ReportError("Unknown journal event");
                                    }
                                    else
                                    {
                                        EliteDangerousCore.HistoryEntry he = EliteDangerousCore.HistoryEntry.FromJournalEntry(je, null, out bool journalupdate);
                                        (ap.actioncontroller as ActionController).ActionRunOnEntry(he, Actions.ActionEventEDList.NewEntry(he), now: true);
                                    }
                                }
                                catch
                                {
                                    ap.ReportError("Journal event not in correct JSON form");
                                }
                            }
                        }
                        else
                            ap.ReportError("No journal event or event name after GenerateEvent");
                    }
                    else
                        ap.ReportError("Unknown command " + cmdname + " in Performaction");
                }
                else
                    ap.ReportError(exp[0]);
            }
            else
                ap.ReportError("Perform command line not in correct format");

            return true;
        }
    }
}
