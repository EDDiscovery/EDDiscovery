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
 * 
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ActionLanguage;
using BaseUtils;
using ExtendedControls;

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

                if (ap.Functions.ExpandStrings(ctrl, out exp) != Functions.ExpandResult.Failed)
                {
                    string cmdname = exp[0].ToLowerInvariant();
                    string nextword = exp.Count >= 2 ? exp[1] : null;
                    string thirdword = exp.Count >= 3 ? exp[2] : null;
                    string fourthword = exp.Count >= 4 ? exp[3] : null;
                    string fifthword = exp.Count >= 5 ? exp[4] : null;
                    string sixthword = exp.Count >= 6 ? exp[5] : null;

                    var ac = ap.ActionController as ActionController;

                    if (cmdname == null)
                    {
                        ap.ReportError("Missing command in Perform");
                    }
                    else if (cmdname.Equals("3dmap"))
                    {
                        ac.DiscoveryForm.Open3DMap();
                    }
                    else if (cmdname.Equals("2dmap"))
                    {
                        ac.DiscoveryForm.PopOuts.PopOut(PanelInformation.PanelIDs.Map2D);
                    }
                    else if (cmdname.Equals("edsm"))
                    {
                        EliteDangerousCore.EDSM.EDSMClass edsm = new EliteDangerousCore.EDSM.EDSMClass();

                        if (edsm.ValidCredentials)
                            ac.DiscoveryForm.EDSMSend();
                        else
                            ap.ReportError("No valid EDSM Credentials");
                    }
                    else if (cmdname.Equals("refresh"))
                    {
                        ac.DiscoveryForm.RefreshHistoryAsync();
                    }

                    else if (cmdname.Equals("configurevoice"))
                    {
                        var res = ac.ConfigureVoice(nextword, thirdword != null, fifthword == "NORATE", thirdword == "NOVOICENAME",
                                                                thirdword, fourthword, fifthword, sixthword);

                        ap["DialogResult"] = res != null ? "1" : "0";
                        if (res != null && thirdword != null)     // if we are getting the config, not globally setting it
                        {
                            ap["VoiceName"] = res.Item1;
                            ap["Volume"] = res.Item2;
                            ap["Rate"] = res.Item3;
                            ap["Effects"] = res.Item4;
                        }
                    }
                    else if (cmdname.Equals("configurewave"))
                    {
                        var res = ac.ConfigureWave(thirdword != null, nextword, thirdword, fourthword);
                        ap["DialogResult"] = res != null ? "1" : "0";
                        if (res != null && thirdword != null)     // if we are getting the config, not globally setting it
                        {
                            ap["Volume"] = res.Item1;
                            ap["Effects"] = res.Item2;
                        }
                    }
                    else if (cmdname.Equals("voicenames"))
                    {
                        ap["VoiceNames"] = ac.SpeechSynthesizer.GetVoiceNames().QuoteStrings();
                    }

                    else if (cmdname.Equals("manageaddons"))
                        ac.ManageAddOns();
                    else if (cmdname.Equals("editaddons"))
                        ac.EditAddOns();
                    else if (cmdname.Equals("editlastpack"))
                        ac.EditLastPack();
                    else if (cmdname.Equals("editpack"))
                    {
                        if (nextword != null)
                        {
                            if (!ac.EditPack(nextword))
                                ap.ReportError("Pack " + nextword + " not found");
                        }
                        else
                            ap.ReportError("EditPack requires a pack name");
                    }
                    else if (cmdname.Equals("editspeechtext"))
                        ac.EditSpeechText();

                    else if (cmdname.Equals("enableeliteinput"))
                        ac.EliteInput(true, true);
                    else if (cmdname.Equals("enableeliteinputnoaxis"))
                        ac.EliteInput(true, false);
                    else if (cmdname.Equals("disableeliteinput"))
                        ac.EliteInput(false, false);
                    else if (cmdname.Equals("listeliteinput"))
                    {
                        ap["EliteInput"] = ac.EliteInputList();
                        ap["EliteInputCheck"] = ac.EliteInputCheck();
                        ap["EliteInputButtons"] = ac.EliteInputButtons();
                    }
                    else if (cmdname.Equals("bindings"))
                    {
                        ap["Bindings"] = ac.FrontierBindings.ListBindings();
                    }
                    else if (cmdname.Equals("bindingvalues"))
                    {
                        ap["BindingValues"] = ac.FrontierBindings.ListValues();
                    }

                    else if (cmdname.Equals("url"))
                    {
                        if (nextword != null && nextword.StartsWith("http:", StringComparison.InvariantCultureIgnoreCase) || nextword.StartsWith("https:", StringComparison.InvariantCultureIgnoreCase))        // security..
                        {
                            BaseUtils.BrowserInfo.LaunchBrowser(nextword);
                        }
                        else
                            ap.ReportError("Perform url must start with http");
                    }
                    else if (cmdname.Equals("datadownload"))
                    {
                        string gitfolder = nextword;
                        string filewildcard = thirdword;
                        string localdirectory = fourthword;
                        string optclean = fifthword;

                        if (gitfolder != null && filewildcard != null && localdirectory != null)
                        {
                            if (System.IO.Directory.Exists(localdirectory))
                            {
                                BaseUtils.GitHubClass ghc = new BaseUtils.GitHubClass(EDDiscovery.Properties.Resources.URLGithubDataDownload);
                                var files = ghc.DownloadFolder(new System.Threading.CancellationToken(), localdirectory, gitfolder, filewildcard, true, optclean != null && optclean == "1");
                                ap["Downloaded"] = (files != null && files.Count > 0).ToStringIntValue();
                                ap["DownloadedCount"] = (files?.Count ?? 0).ToStringInvariant();
                            }
                            else
                                ap.ReportError("Download folder " + localdirectory + " does not exist");
                        }
                        else
                            ap.ReportError("Missing parameters in Perform Datadownload");
                    }

                    else if (cmdname.Equals("enablevoicerecognition"))
                    {
                        if (nextword != null)
                        {
                            ap["VoiceRecognitionEnabled"] = (ac.VoiceReconOn(nextword)).ToStringIntValue();
                        }
                        else
                            ap.ReportError("EnableVoiceRecognition requires a culture");
                    }
                    else if (cmdname.Equals("disablevoicerecognition"))
                        ac.VoiceReconOff();
                    else if (cmdname.Equals("panelaction"))     // send to panel tabs
                    {
                        ac.DiscoveryForm.PerformOperationOnTabs(null, new UserControls.UserControlCommonBase.PanelAction() { Action = nextword });
                    }
                    else if (cmdname.Equals("beginvoicerecognition"))
                        ac.VoiceLoadEvents();
                    else if (cmdname.Equals("voicerecognitionconfidencelevel"))
                    {
                        float? conf = nextword.InvariantParseFloatNull();
                        if (conf != null)
                            ac.VoiceReconConfidence(conf.Value);
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
                            ac.VoiceReconParameters(babble.Value, initialsilence.Value, endsilence.Value, endsilenceambigious.Value);
                        else
                            ap.ReportError("VoiceRecognitionParameters requires four values");
                    }
                    else if (cmdname.Equals("voicerecognitionphrases"))
                    {
                        ap["Phrases"] = ac.VoicePhrases(Environment.NewLine);
                    }
                    else if (cmdname.Equals("voicerecognitionevent"))
                    {
                        if (nextword.Equals("toggle", StringComparison.InvariantCultureIgnoreCase))
                        {
                            ac.EnableVoiceReconEvent = !ac.EnableVoiceReconEvent;
                        }
                        else if (nextword.Equals("status", StringComparison.InvariantCultureIgnoreCase))
                        {
                        }
                        else if (int.TryParse(nextword, out int res))
                        {
                            ac.EnableVoiceReconEvent = res != 0;
                        }
                        else
                            ap.ReportError("VoiceRecognitionEvent parameter invalid");

                        ap["VoiceRecognitionEvent"] = ac.EnableVoiceReconEvent.ToStringIntValue();
                        System.Diagnostics.Debug.WriteLine("Voice recon " + ap["VoiceRecognitionEvent"]);
                    }
                    else if (cmdname.Equals("loadkeys"))
                    {
                        ac.ActionConfigureKeys();
                    }
                    else if (cmdname.Equals("actionfile"))
                    {
                        ActionFile f = ac.Get(nextword);
                        if (f != null)
                        {
                            ap["Events_Count"] = f.InUseEventList.Count.ToStringInvariant();

                            int i = 1;
                            foreach (var x in f.InUseEventList.Enumerable)
                            {
                                ap["Events[" + i++ + "]"] = x.ToString(true);   // list hooked events
                                ap["Events_" + x.EventName] = x.ToString(true);   // list hooked events
                            }

                            ap["Journal_Count"] = Enum.GetNames(typeof(EliteDangerousCore.JournalTypeEnum)).Length.ToStringInvariant();
                            i = 1;
                            foreach (string jname in Enum.GetNames(typeof(EliteDangerousCore.JournalTypeEnum)))
                            {
                                List<Condition> cl = f.InUseEventList.GetConditionListByEventName(jname);

                                if (cl != null)
                                {
                                    int v = 0;
                                    foreach (var c in cl)
                                    {
                                        ap["JEvents[" + i++ + "]"] = c.ToString(true);
                                        ap["JEvents_" + c.EventName + "_" + v++] = c.ToString(true);
                                    }
                                }
                                else
                                {
                                    ap["JEvents[" + i++ + "]"] = jname + ", None";
                                    ap["JEvents_" + jname] = "None";
                                }
                            }

                            ap["UI_Count"] = Enum.GetNames(typeof(EliteDangerousCore.UITypeEnum)).Length.ToStringInvariant();
                            i = 1;
                            foreach (string iname in Enum.GetNames(typeof(EliteDangerousCore.UITypeEnum)))
                            {
                                List<Condition> cl = f.InUseEventList.GetConditionListByEventName("UI" + iname);

                                if (cl != null)
                                {
                                    int v = 0;
                                    foreach (var c in cl)
                                    {
                                        ap["UIEvents[" + i++ + "]"] = c.ToString(true);
                                        ap["UIEvents_" + c.EventName + "_" + v++] = c.ToString(true);
                                    }
                                }
                                else
                                {
                                    ap["UIEvents[" + i++ + "]"] = iname + ", None";
                                    ap["UIEvents_" + iname] = "None";
                                }
                            }

                            ap["Install_Count"] = f.InstallationVariables.Count.ToStringInvariant();
                            i = 1;
                            foreach (var x in f.InstallationVariables.NameEnumuerable)
                                ap["Install[" + i++ + "]"] = x + "," + f.InstallationVariables[x];   // list hooked events

                            ap["FileVar_Count"] = f.FileVariables.Count.ToStringInvariant();
                            i = 1;
                            foreach (var x in f.FileVariables.NameEnumuerable)
                                ap["FileVar[" + i++ + "]"] = x + "," + f.FileVariables[x];   // list hooked events

                            ap["Enabled"] = f.Enabled.ToStringIntValue();

                        }
                        else
                            ap.ReportError("Action file " + nextword + " is not loaded");
                    }

                    else if (cmdname.Equals("generateevent"))
                    {
                        if (nextword != null)
                        {
                            // see if its a triggername

                            ActionEvent f = ActionEventEDList.EventList(excludejournal: true).Find(x => x.TriggerName.Equals(nextword));

                            if (f != null)
                            {
                                Variables c = CollectVars(exp, 2, ap);

                                if (c != null)      // if did not fail..
                                {
                                    // if UI event, then trigger the onUIEvent as well as the nominal event
                                    if (f.TriggerName.StartsWith("UI") || f.TriggerName.Equals("onEliteUIEvent"))
                                    {
                                        c["EventClass_EventTimeUTC"] = DateTime.UtcNow.ToStringUSInvariant();
                                        c["EventClass_EventTypeID"] = c["EventClass_EventTypeStr"] = f.TriggerName.Substring(2);
                                        c["EventClass_UIDisplayed"] = "0";
                                        ac.ActionRun(Actions.ActionEventEDList.onUIEvent, c);
                                    }

                                    ac.ActionRun(f, c, now: true);
                                }
                            }
                            else
                            {
                                try
                                {
                                    // from nextword, which is the JSON of the event, try and make an event

                                    EliteDangerousCore.JournalEntry je = EliteDangerousCore.JournalEntry.CreateJournalEntry(nextword);

                                    ap["GenerateEventName"] = je.EventTypeStr;

                                    if (je is EliteDangerousCore.JournalEvents.JournalUnknown)  // if it returned unknown event, return error
                                    {
                                        ap.ReportError("Unknown journal event");
                                    }
                                    else
                                    {
                                        EliteDangerousCore.HistoryEntry he = EliteDangerousCore.HistoryEntry.FromJournalEntry(je, null, null);   // create a false HE
                                        ac.ActionRunOnEntry(he, Actions.ActionEventEDList.NewEntry(he), now: true);     // and action
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
                    else if (cmdname.Equals("actionevent"))
                    {
                        if (nextword != null && thirdword != null)
                        {
                            Variables c = CollectVars(exp, 3, ap);  // collect vars after 3rd word incl

                            if (c != null)      // if did not fail..
                            {
                                ac.ActionRun(new ActionEvent(nextword, thirdword, "Program", null), c, now: true);
                            }
                        }
                        else
                            ap.ReportError("Missing triggername and/or triggertype after ActionEvent");
                    }
                    else if (cmdname.Equals("mainwindowdimensions"))
                    {
                        ap["X"] = ac.DiscoveryForm.Bounds.X.ToStringInvariant();
                        ap["Y"] = ac.DiscoveryForm.Bounds.Y.ToStringInvariant();
                        ap["Width"] = ac.DiscoveryForm.Bounds.Width.ToStringInvariant();
                        ap["Height"] = ac.DiscoveryForm.Bounds.Height.ToStringInvariant();
                        var sf = ac.DiscoveryForm.CurrentAutoScaleFactor();
                        ap["DUWidth"] = ((int)(ac.DiscoveryForm.Bounds.Width / sf.Width)).ToStringInvariant();
                        ap["DUHeight"] = ((int)(ac.DiscoveryForm.Bounds.Height / sf.Height)).ToStringInvariant();
                        ap["SFX"] = sf.Width.ToStringInvariant();
                        ap["SFY"] = sf.Height.ToStringInvariant();
                    }
                    else if (cmdname.Equals("mainwindowscreendimensions"))
                    {
                        Screen scr = Screen.FromControl(ac.DiscoveryForm);
                        ap["WAX"] = scr.WorkingArea.X.ToStringInvariant();
                        ap["WAY"] = scr.WorkingArea.Y.ToStringInvariant();
                        ap["WAWidth"] = scr.WorkingArea.Width.ToStringInvariant();
                        ap["WAHeight"] = scr.WorkingArea.Height.ToStringInvariant();
                        var sf = ac.DiscoveryForm.CurrentAutoScaleFactor();
                        ap["DUWAWidth"] = ((int)(scr.WorkingArea.Width / sf.Width)).ToStringInvariant();
                        ap["DUWAHeight"] = ((int)(scr.WorkingArea.Height / sf.Height)).ToStringInvariant();
                        ap["X"] = scr.Bounds.X.ToStringInvariant();
                        ap["Y"] = scr.Bounds.Y.ToStringInvariant();
                        ap["Width"] = scr.Bounds.Width.ToStringInvariant();
                        ap["Height"] = scr.Bounds.Height.ToStringInvariant();
                        ap["DUWidth"] = ((int)(scr.Bounds.Width / sf.Width)).ToStringInvariant();
                        ap["DUHeight"] = ((int)(scr.Bounds.Height / sf.Height)).ToStringInvariant();
                        ap["SFX"] = sf.Width.ToStringInvariant();
                        ap["SFY"] = sf.Height.ToStringInvariant();
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


        // Collect vars from list of strings, from start, using ap.
        // if a variable does not exist, complain, and return null
        // can have zero variables - thats okay
        private Variables CollectVars(List<string> exp, int start, ActionProgramRun ap)
        {
            Variables vars = new Variables();

            for (int w = start; w < exp.Count; w++)     // go thru variable names
            {
                string vname = exp[w];
                int asterisk = vname.IndexOf('*');

                if (asterisk >= 0)     // pass in name* no complaining if not there
                {
                    string prefix = vname.Substring(0, asterisk);

                    foreach (string jkey in ap.variables.NameEnumuerable)
                    {
                        if (jkey.StartsWith(prefix))
                            vars[jkey] = ap.variables[jkey];
                    }
                }
                else
                {
                    if (ap.variables.Exists(vname))     // pass in explicit name
                        vars[vname] = ap.variables[vname];
                    else
                    {
                        ap.ReportError("No such variable '" + vname + "'");
                        return null;
                    }
                }
            }
            
            return vars;
        }
    }
}
