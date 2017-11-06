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
using BaseUtils;
using ActionLanguage;

namespace EDDiscovery.Actions
{
    public class ActionPerform : ActionBase
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)
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
            string res;
            if (ap.functions.ExpandString(UserData, out res) != Conditions.ConditionFunctions.ExpandResult.Failed)
            {
                StringParser sp = new StringParser(res);
                string cmdname = sp.NextWord(" ", lowercase:true);

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
                    EliteDangerousCore.EDSM.EDSMClass edsm = new EliteDangerousCore.EDSM.EDSMClass();
                    ActionController ac = (ap.actioncontroller as ActionController);
                    ac.DiscoveryForm.EdsmSync.StartSync(edsm, ac.DiscoveryForm.history, EliteDangerousCore.EDCommander.Current.SyncToEdsm, EliteDangerousCore.EDCommander.Current.SyncFromEdsm, EDDiscovery.EDDConfig.Instance.DefaultMapColour);
                }
                else if (cmdname.Equals("refresh"))
                {
                    (ap.actioncontroller as ActionController).DiscoveryForm.RefreshHistoryAsync(checkedsm: true);
                }
                else if (cmdname.Equals("url"))
                {
                    string url = sp.LineLeft;

                    if (url.StartsWith("http:", StringComparison.InvariantCultureIgnoreCase) || url.StartsWith("https:", StringComparison.InvariantCultureIgnoreCase))        // security..
                    {
                        System.Diagnostics.Process.Start(url);
                    }
                    else
                        ap.ReportError("Perform url must start with http");
                }
                else if (cmdname.Equals("configurevoice"))
                    (ap.actioncontroller as ActionController).ConfigureVoice(sp.NextQuotedWord() ?? "Configure Voice Synthesis");
                else if (cmdname.Equals("manageaddons"))
                    (ap.actioncontroller as ActionController).ManageAddOns();
                else if (cmdname.Equals("editaddons"))
                    (ap.actioncontroller as ActionController).EditAddOns();
                else if (cmdname.Equals("editlastpack"))
                    (ap.actioncontroller as ActionController).EditLastPack();
                else if (cmdname.Equals("editspeechtext"))
                    (ap.actioncontroller as ActionController).EditSpeechText();
                else if (cmdname.Equals("configurewave"))
                    (ap.actioncontroller as ActionController).ConfigureWave(sp.NextQuotedWord() ?? "Configure Wave Output");
                else if (cmdname.Equals("enableeliteinput"))
                    (ap.actioncontroller as ActionController).DiscoveryForm.EliteInput(true, true);
                else if (cmdname.Equals("enableeliteinputnoaxis"))
                    (ap.actioncontroller as ActionController).DiscoveryForm.EliteInput(true, false);
                else if (cmdname.Equals("disableeliteinput"))
                    (ap.actioncontroller as ActionController).DiscoveryForm.EliteInput(false, false);
                else if (cmdname.Equals("enablevoicerecognition"))
                    (ap.actioncontroller as ActionController).DiscoveryForm.VoiceRecon(true, sp.NextQuotedWord() ?? "en-gb");
                else if (cmdname.Equals("disablevoicerecognition"))
                    (ap.actioncontroller as ActionController).DiscoveryForm.VoiceRecon(false);
                else if (cmdname.Equals("listeliteinput"))
                {
                    ap["EliteInput"] = (ap.actioncontroller as ActionController).DiscoveryForm.EliteInputList();
                    ap["EliteInputCheck"] = (ap.actioncontroller as ActionController).DiscoveryForm.EliteInputCheck();
                }
                else if (cmdname.Equals("voicenames"))
                {
                    ap["VoiceNames"] = (ap.actioncontroller as ActionController).DiscoveryForm.SpeechSynthesizer.GetVoiceNames().QuoteStrings();
                }
                else
                    ap.ReportError("Unknown command " + cmdname + " in Performaction");
            }
            else
                ap.ReportError(res);

            return true;
        }
    }
}
