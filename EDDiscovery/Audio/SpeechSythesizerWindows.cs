using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Audio
{
#if !__MonoCS__
    class WindowsSpeechEngine : ISpeechEngine
    {
        private System.Speech.Synthesis.SpeechSynthesizer synth;
        public event EventHandler SpeakingCompleted;
        public string systemdefaultvoice;

        public WindowsSpeechEngine()
        {
            synth = new System.Speech.Synthesis.SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
            synth.SpeakCompleted += (s, e) => SpeakingCompleted?.Invoke(s, e);
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
                    pb = new System.Speech.Synthesis.PromptBuilder(new System.Globalization.CultureInfo(culture));

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

                System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Speak " + phrase + ", Rate " + rate + " culture " + culture);

                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                synth.SetOutputToWaveStream(stream);

                pb.AppendText(phrase);

                pb.EndVoice();

                synth.Speak(pb);

                return stream;
            }
            catch
            {
                return null;
            }
        }
    }
#endif
}
