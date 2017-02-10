using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Audio
{
    class SoundEffectSettings
    {
        public EDDiscovery.ConditionVariables values;

        public bool Any { get { return echoenabled || chorusenabled || reverbenabled || distortionenabled || gargleenabled; } }

        public bool echoenabled { get { return values.ContainsKey("EchoMix") || values.ContainsKey("EchoFeedback") || values.ContainsKey("EchoDelay"); } }
        public int echomix { get { return values.GetInt("EchoMix", 50); } set { values["EchoMix"] = value.ToString(); } }
        public int echofeedback { get { return values.GetInt("EchoFeedback", 50); } set { values["EchoFeedback"] = value.ToString(); } }
        public int echodelay { get { return values.GetInt("EchoDelay", 100); } set { values["EchoDelay"] = value.ToString(); } }

        public bool chorusenabled { get { return values.ContainsKey("ChorusMix") || values.ContainsKey("ChorusFeedback") || values.ContainsKey("ChorusDelay") || values.ContainsKey("ChorusDepth"); } }
        public int chorusmix { get { return values.GetInt("ChorusMix", 50); } set { values["ChorusMix"] = value.ToString(); } }
        public int chorusfeedback { get { return values.GetInt("ChorusFeedback", 25); } set { values["ChorusFeedback"] = value.ToString(); } }
        public int chorusdelay { get { return values.GetInt("ChorusDelay", 16); } set { values["ChorusDelay"] = value.ToString(); } }
        public int chorusdepth { get { return values.GetInt("ChorusDepth", 10); } set { values["ChorusDepth"] = value.ToString(); } }

        public bool reverbenabled { get { return values.ContainsKey("ReverbMix") || values.ContainsKey("ReverbTime") || values.ContainsKey("ReverbRatio"); } }
        public int reverbmix { get { return values.GetInt("ReverbMix", 0); } set { values["ReverbMix"] = value.ToString(); } }
        public int reverbtime { get { return values.GetInt("ReverbTime", 1000); } set { values["ReverbTime"] = value.ToString(); } }
        public int reverbhfratio { get { return values.GetInt("ReverbRatio", 1); } set { values["ReverbRatio"] = value.ToString(); } }

        public bool distortionenabled { get { return values.ContainsKey("DistortionGain") || values.ContainsKey("DistortionEdge") || values.ContainsKey("DistortionCF") || values.ContainsKey("DistortionWidth"); } }
        public int distortiongain { get { return values.GetInt("DistortionGain", -18); } set { values["DistortionGain"] = value.ToString(); } }
        public int distortionedge { get { return values.GetInt("DistortionEdge", 15); } set { values["DistortionEdge"] = value.ToString(); } }
        public int distortioncentrefreq { get { return values.GetInt("DistortionCF", 2400); } set { values["DistortionCF"] = value.ToString(); } }
        public int distortionfreqwidth { get { return values.GetInt("DistortionWidth", 2400); } set { values["DistortionWidth"] = value.ToString(); } }

        public bool gargleenabled { get { return values.ContainsKey("GargleFreq"); } }
        public int garglefreq { get { return values.GetInt("GargleFreq", 20); } set { values["GargleFreq"] = value.ToString(); } }

        public SoundEffectSettings()
        { values = new ConditionVariables(); }

        public SoundEffectSettings(ConditionVariables v )
        { values = v; }
    }

}
