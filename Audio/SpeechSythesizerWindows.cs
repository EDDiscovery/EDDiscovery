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

namespace AudioExtensions
{
#if !NO_SYSTEM_SPEECH
    public class WindowsSpeechEngine : ISpeechEngine
    {
        private System.Speech.Synthesis.SpeechSynthesizer synth;
        public string systemdefaultvoice;

        public WindowsSpeechEngine()
        {
            synth = new System.Speech.Synthesis.SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
            systemdefaultvoice = synth.Voice.Name;
        }

        public string[] GetVoiceNames()
        {
            return synth.GetInstalledVoices().Select(v => v.VoiceInfo.Name).ToArray();
        }


        public System.IO.MemoryStream Speak(string phrase, string culture, string voice, int volume, int rate)
        {
            try
            {                                                   // paranoia here..
                System.Speech.Synthesis.PromptBuilder pb;

                if (culture.Equals("Default"))
                    pb = new System.Speech.Synthesis.PromptBuilder();
                else
                {
                    try
                    {
                        pb = new System.Speech.Synthesis.PromptBuilder(new System.Globalization.CultureInfo(culture)); // may except if crap culture for machine
                    }
                    catch
                    {
                        pb = new System.Speech.Synthesis.PromptBuilder();
                    }
                }
                   

                if (voice.Equals("Female", StringComparison.InvariantCultureIgnoreCase))
                    pb.StartVoice(System.Speech.Synthesis.VoiceGender.Female);
                else if (voice.Equals("Male", StringComparison.InvariantCultureIgnoreCase))
                    pb.StartVoice(System.Speech.Synthesis.VoiceGender.Male);
                else if (!voice.Equals("Default", StringComparison.InvariantCultureIgnoreCase))
                    pb.StartVoice(voice);
                else
                    pb.StartVoice(systemdefaultvoice);

                synth.Volume = volume;
                synth.Rate = rate;

                //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Speak " + phrase + ", Rate " + rate + " culture " + culture);

                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                synth.SetOutputToWaveStream(stream);

                string[] ssmlstart = new string[] { "<say-as ", "<emphasis" , "<phoneme" , "<sub" , "<prosody"};
                string[] ssmlend = new string[] { "</say-as>", "</emphasis>" , "</phoneme>" , "</sub>" , "</prosody>" };

                phrase = phrase.Trim();

                while (phrase.Length > 0)
                {
                    int ssmlindex;
                    int foundpos = phrase.IndexOf(ssmlstart, out ssmlindex);        // find one of the ssml phrases
                    if (foundpos == -1)     // no more, task on rest as normal text
                    {
                        pb.AppendText(phrase);
                        break;
                    }
                    else
                    {
                        if (foundpos > 0)
                        {
                            pb.AppendText(phrase.Substring(0, foundpos));       // tack on front
                            phrase = phrase.Substring(foundpos);
                        }

                        int indexofend = phrase.IndexOf(ssmlend[ssmlindex]);

                        if (indexofend == -1) // allowed as a shortcut to drop the last one
                        {
                            indexofend = phrase.Length;
                            phrase += ssmlend[ssmlindex];
                        }

                        indexofend += ssmlend[ssmlindex].Length; // move to end of it

                        string ssmlcmd = phrase.Substring(0, indexofend).Replace('\'', '"');

                        //for (int i = 0; i < ssmlcmd.Length; i++) System.Diagnostics.Debug.WriteLine("SSML :" + (int)ssmlcmd[i] + " = " + ssmlcmd[i]);

                        try
                        {
                            pb.AppendSsmlMarkup(ssmlcmd);
                        }
                        catch       // bad markup
                        {
                            pb.AppendText("Bad SSML Markup when added, contact developers");
                        }

                        phrase = phrase.Substring(indexofend).Trim();
                    }
                }

                pb.EndVoice();

                try
                {
                    synth.Speak(pb);
                }
                catch
                {
                    synth.Speak("Bad SSML Markup in phrase, your chosen voice may not support all options. See voice configuration menu to disable SSML");
                }

                //System.Diagnostics.Debug.WriteLine("Speech " + stream.Length);
                return stream;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.ToString());
                return null;
            }
        }
    }
#endif
}
