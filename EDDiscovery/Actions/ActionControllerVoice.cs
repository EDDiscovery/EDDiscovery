/*
 * Copyright © 2019 EDDiscovery development team
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

using ActionLanguage;
using BaseUtils;
using System;
using System.Collections.Generic;

namespace EDDiscovery.Actions
{
    public partial class ActionController : ActionCoreController
    {
        public AudioExtensions.IVoiceRecognition VoiceRecognition { get { return voicerecon; } }

        public bool EnableVoiceReconEvent { get; set; } = true;           // set to false to stop VoiceRecognition Events being generated

        private AudioExtensions.IVoiceRecognition voicerecon;

        public bool VoiceReconOn(string culture = null)     // perform enableVR
        {
            voicerecon.Close(); // can close without stopping
            voicerecon.Open(System.Globalization.CultureInfo.GetCultureInfo(culture));
            return voicerecon.IsOpen;
        }

        public void VoiceReconOff()                         // perform disableVR
        {
            voicerecon.Close();
        }

        public void VoiceReconConfidence(float conf)
        {
            voicerecon.Confidence = conf;
        }

        public void VoiceReconParameters(int babble, int initialsilence, int endsilence, int endsilenceambigious)
        {
            if (voicerecon.IsOpen)
            {
                voicerecon.Stop(true);
                try
                {
                    voicerecon.BabbleTimeout = babble;
                    voicerecon.InitialSilenceTimeout = initialsilence;
                    voicerecon.EndSilenceTimeout = endsilence;
                    voicerecon.EndSilenceTimeoutAmbigious = endsilenceambigious;
                }
                catch { };

                voicerecon.Start();
            }
        }

        public void VoiceLoadEvents()       // kicked by Action.Perform so synchornised with voice pack (or via editor)
        {
            if (voicerecon.IsOpen)
            {
                voicerecon.Stop(true);

                voicerecon.Clear(); // clear grammars

                var ret = actionfiles.ReturnSpecificConditions(ActionEventEDList.onVoiceInput.TriggerName, "VoiceInput", new List<ConditionEntry.MatchType>() { ConditionEntry.MatchType.MatchSemicolonList, ConditionEntry.MatchType.MatchSemicolon });        // need these to decide

                if (ret.Count > 0)
                {
                    foreach (var vp in ret)
                    {
                        System.Diagnostics.Debug.WriteLine($"Voice Add {vp.Item1}:{vp.Item2.MatchString}");
                        voicerecon.Add(vp.Item2.MatchString);
                    }

                    voicerecon.Start();
                }
            }
        }

        public string VoicePhrases(string sep)
        {
            var ret = actionfiles.ReturnSpecificConditions(ActionEventEDList.onVoiceInput.TriggerName, "VoiceInput", new List<ConditionEntry.MatchType>() { ConditionEntry.MatchType.MatchSemicolonList, ConditionEntry.MatchType.MatchSemicolon });        // need these to decide

            string s = "";
            foreach (var vp in ret)
            {
                BaseUtils.StringCombinations sb = new BaseUtils.StringCombinations();
                sb.ParseString(vp.Item2.MatchString);
                s += String.Join(",", sb.Permutations.ToArray()) + sep;
            }

            return s;
        }

        private void Voicerecon_SpeechRecognised(string text, float confidence)
        {
            if (EnableVoiceReconEvent)
            {
                System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + " Recognised " + text + " " + confidence.ToStringInvariant("0.000"));
                ActionRun(ActionEventEDList.onVoiceInput, new Variables(new string[] { "VoiceInput", text, "VoiceConfidence", (confidence * 100F).ToStringInvariant("0.00") }));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + " Ignored " + text + " " + confidence.ToStringInvariant("0.0"));
            }
        }

        private void Voicerecon_SpeechNotRecognised(string text, float confidence)
        {
            if (EnableVoiceReconEvent)
            {
                System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + " Failed recognition " + text + " " + confidence.ToStringInvariant("0.00"));
                ActionRun(ActionEventEDList.onVoiceInputFailed, new Variables(new string[] { "VoiceInput", text, "VoiceConfidence", (confidence * 100F).ToStringInvariant("0.00") }));
            }

        }

    }
}
