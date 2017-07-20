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
using Conditions;

namespace AudioExtensions
{
    public class SoundEffectSettings
    {
        public Conditions.ConditionVariables values;

        public bool Any { get { return echoenabled || chorusenabled || reverbenabled || distortionenabled || gargleenabled || pitchshiftenabled; } }
        public bool OverrideNone { get { return values.Exists("NoEffects"); } set { values["NoEffects"] = (value) ? "1" : "0"; } }
        public bool Merge { get { return values.Exists("MergeEffects"); } set { values["MergeEffects"] = (value) ? "1" : "0"; } }

        public bool echoenabled { get { return values.Exists("EchoMix") || values.Exists("EchoFeedback") || values.Exists("EchoDelay"); } }
        public int echomix { get { return values.GetInt("EchoMix", 50); } set { values["EchoMix"] = value.ToString(); } }
        public int echofeedback { get { return values.GetInt("EchoFeedback", 50); } set { values["EchoFeedback"] = value.ToString(); } }
        public int echodelay { get { return values.GetInt("EchoDelay", 100); } set { values["EchoDelay"] = value.ToString(); } }

        public bool chorusenabled { get { return values.Exists("ChorusMix") || values.Exists("ChorusFeedback") || values.Exists("ChorusDelay") || values.Exists("ChorusDepth"); } }
        public int chorusmix { get { return values.GetInt("ChorusMix", 50); } set { values["ChorusMix"] = value.ToString(); } }
        public int chorusfeedback { get { return values.GetInt("ChorusFeedback", 25); } set { values["ChorusFeedback"] = value.ToString(); } }
        public int chorusdelay { get { return values.GetInt("ChorusDelay", 16); } set { values["ChorusDelay"] = value.ToString(); } }
        public int chorusdepth { get { return values.GetInt("ChorusDepth", 10); } set { values["ChorusDepth"] = value.ToString(); } }

        public bool reverbenabled { get { return values.Exists("ReverbMix") || values.Exists("ReverbTime") || values.Exists("ReverbRatio"); } }
        public int reverbmix { get { return values.GetInt("ReverbMix", 0); } set { values["ReverbMix"] = value.ToString(); } }
        public int reverbtime { get { return values.GetInt("ReverbTime", 1000); } set { values["ReverbTime"] = value.ToString(); } }
        public int reverbhfratio { get { return values.GetInt("ReverbRatio", 1); } set { values["ReverbRatio"] = value.ToString(); } }

        public bool distortionenabled { get { return values.Exists("DistortionGain") || values.Exists("DistortionEdge") || values.Exists("DistortionCF") || values.Exists("DistortionWidth"); } }
        public int distortiongain { get { return values.GetInt("DistortionGain", -18); } set { values["DistortionGain"] = value.ToString(); } }
        public int distortionedge { get { return values.GetInt("DistortionEdge", 15); } set { values["DistortionEdge"] = value.ToString(); } }
        public int distortioncentrefreq { get { return values.GetInt("DistortionCF", 2400); } set { values["DistortionCF"] = value.ToString(); } }
        public int distortionfreqwidth { get { return values.GetInt("DistortionWidth", 2400); } set { values["DistortionWidth"] = value.ToString(); } }

        public bool gargleenabled { get { return values.Exists("GargleFreq"); } }
        public int garglefreq { get { return values.GetInt("GargleFreq", 20); } set { values["GargleFreq"] = value.ToString(); } }

        public bool pitchshiftenabled { get { return values.Exists("PitchShift"); } }
        public int pitchshift { get { return values.GetInt("PitchShift", 100); } set { values["PitchShift"] = value.ToString(); } }

        public SoundEffectSettings()
        { values = new ConditionVariables(); }

        public SoundEffectSettings(ConditionVariables v )
        { values = v; }

        public static SoundEffectSettings Set( ConditionVariables globals, ConditionVariables local)
        {
            SoundEffectSettings ses = new SoundEffectSettings(local);        // use the rest of the vars to place effects

            if (ses.OverrideNone)      // if none
                ses = null;             // no speech effects
            else if (ses.Merge)       // merged
            {
                ConditionVariables merged = new ConditionVariables(globals, local);   // add global settings (if not null) overridden by vars
                ses = new SoundEffectSettings(merged);
            }
            else if (!ses.Any)        // if none on the command line
            {
                ses = (globals != null) ? new SoundEffectSettings(globals) : null;
            }

            return ses;
        }
    }
}
