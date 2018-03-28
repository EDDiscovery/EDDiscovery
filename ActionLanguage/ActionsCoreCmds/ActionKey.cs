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
using AudioExtensions;
using Conditions;

namespace ActionLanguage
{
    public class ActionKey: ActionBase
    {
        public static string programDefault = "Program Default";        // meaning use the program default vars for the process

        public static string globalvarProcessID = "KeyProcessTo";       // var global
        protected static string ProcessID = "To";       // command tags

        public static string globalvarDelay = "KeyDelay";
        protected static string DelayID = "Delay";

        public static string globalvarUpDelay = "KeyUpDelay";
        protected static string UpDelayID = "UpDelay";

        public static string globalvarShiftDelay = "KeyShiftDelay";
        protected static string ShiftDelayID = "ShiftDelay";

        public static string globalvarSilentOnErrors = "KeySilentOnError";
        protected static string SilentOnError = "SilentOnError";

        public static string globalvarAnnounciateOnError = "KeyAnnounciateOnError";
        protected static string AnnounciateOnError = "AnnounicateOnError";

        protected const int DefaultDelay = 10;

        static public bool FromString(string s, out string keys, out ConditionVariables vars)
        {
            vars = new ConditionVariables();

            StringParser p = new StringParser(s);
            keys = p.NextQuotedWord(", ");        // stop at space or comma..

            if (keys != null && (p.IsEOL || (p.IsCharMoveOn(',') && vars.FromString(p, ConditionVariables.FromMode.MultiEntryComma))))   // normalise variable names (true)
                return true;

            keys = "";
            return false;
        }

        static public string ToString(string keys, ConditionVariables cond )
        {
            if (cond.Count > 0)
                return keys.QuoteString(comma: true) + ", " + cond.ToString();
            else
                return keys.QuoteString(comma: true);
        }

        public override string VerifyActionCorrect()
        {
            string saying;
            ConditionVariables vars;
            return FromString(userdata, out saying, out vars) ? null : "Key command line not in correct format";
        }

        static public string Menu(Form parent, System.Drawing.Icon ic, string userdata, List<string> additionalkeys, BaseUtils.EnhancedSendKeysParser.IAdditionalKeyParser additionalparser)
        {
            ConditionVariables vars;
            string keys;
            FromString(userdata, out keys, out vars);

            ExtendedControls.KeyForm kf = new ExtendedControls.KeyForm();
            int defdelay = vars.Exists(DelayID) ? vars[DelayID].InvariantParseInt(DefaultDelay) : ExtendedControls.KeyForm.DefaultDelayID;
            string process = vars.Exists(ProcessID) ? vars[ProcessID] : "";

            kf.Init(ic, true, " ", keys, process , defdelay:defdelay, additionalkeys:additionalkeys ,parser:additionalparser );      // process="" default, defdelay = DefaultDelayID default

            if (kf.ShowDialog(parent) == DialogResult.OK)
            {
                ConditionVariables vlist = new ConditionVariables();

                if (kf.DefaultDelay != ExtendedControls.KeyForm.DefaultDelayID)                                       // only add these into the command if set to non default
                    vlist[DelayID] = kf.DefaultDelay.ToStringInvariant();
                if (kf.ProcessSelected.Length > 0)
                    vlist[ProcessID] = kf.ProcessSelected;

                return ToString(kf.KeyList, vlist);
            }
            else
                return null;
        }

        public override bool ConfigurationMenu(Form parent, ActionCoreController cp, List<string> eventvars)    // override again to expand any functionality
        {
            string ud = Menu(parent, cp.Icon, userdata, null, null);      // base has no additional keys/parser
            if (ud != null)
            {
                userdata = ud;
                return true;
            }
            else
                return false;
        }

        public override bool ExecuteAction(ActionProgramRun ap)     // standard action.. at this class level in action language we do not have an additional parser.
        {
            return ExecuteAction(ap, null);
        }

        static List<string> errorsreported = new List<string>();

        public bool ExecuteAction(ActionProgramRun ap, BaseUtils.EnhancedSendKeysParser.IAdditionalKeyParser akp )      // additional parser
        { 
            string keys;
            ConditionVariables statementvars;
            if (FromString(userdata, out keys, out statementvars))
            {
                string errlist = null;
                ConditionVariables vars = ap.functions.ExpandVars(statementvars, out errlist);

                if (errlist == null)
                {
                    int delay = vars.Exists(DelayID) ? vars[DelayID].InvariantParseInt(DefaultDelay) : (ap.VarExist(globalvarDelay) ? ap[globalvarDelay].InvariantParseInt(DefaultDelay) : DefaultDelay);
                    int updelay = vars.Exists(UpDelayID) ? vars[UpDelayID].InvariantParseInt(DefaultDelay) : (ap.VarExist(globalvarUpDelay) ? ap[globalvarUpDelay].InvariantParseInt(DefaultDelay) : DefaultDelay);
                    int shiftdelay = vars.Exists(ShiftDelayID) ? vars[ShiftDelayID].InvariantParseInt(DefaultDelay) : (ap.VarExist(globalvarShiftDelay) ? ap[globalvarShiftDelay].InvariantParseInt(DefaultDelay) : DefaultDelay);
                    string process = vars.Exists(ProcessID) ? vars[ProcessID] : (ap.VarExist(globalvarProcessID) ? ap[globalvarProcessID] : "");
                    string silentonerrors = vars.Exists(SilentOnError) ? vars[SilentOnError] : (ap.VarExist(globalvarSilentOnErrors) ? ap[globalvarSilentOnErrors] : "0");
                    string announciateonerrors = vars.Exists(AnnounciateOnError) ? vars[AnnounciateOnError] : (ap.VarExist(globalvarAnnounciateOnError) ? ap[globalvarAnnounciateOnError] : "0");

                    string res = BaseUtils.EnhancedSendKeys.SendToProcess(keys, delay, shiftdelay, updelay, process, akp);

                    if (res.HasChars())
                    {
                        if (silentonerrors.Equals("2") || (errorsreported.Contains(res) && silentonerrors.Equals("1")))
                        {
                            System.Diagnostics.Debug.WriteLine("Swallow key error " + res);
                            ap.actioncontroller.TerminateAll();
                        }
                        else
                        {
                            errorsreported.Add(res);

                            if (announciateonerrors.Equals("1"))
                            {
                                string culture = ap.VarExist(ActionSay.globalvarspeechculture) ? ap[ActionSay.globalvarspeechculture] : "Default";
                                System.IO.MemoryStream ms = ap.actioncontroller.SpeechSynthesizer.Speak("Cannot press key due to " + res, culture, "Default", 0);
                                AudioQueue.AudioSample audio = ap.actioncontroller.AudioQueueSpeech.Generate(ms);
                                ap.actioncontroller.AudioQueueSpeech.Submit(audio, 80, AudioQueue.Priority.Normal);
                            }

                            ap.ReportError(res);
                        }
                    }
                }
                else
                    ap.ReportError(errlist);
            }
            else
                ap.ReportError("Key command line not in correct format");

            return true;
        }


    }
}
