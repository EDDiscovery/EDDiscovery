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

        public System.IO.MemoryStream Speak(string phrase, string voice, int volume, int rate)
        {
            if (voice.Equals("Female", StringComparison.InvariantCultureIgnoreCase))
                synth.SelectVoiceByHints(System.Speech.Synthesis.VoiceGender.Female);
            else if (voice.Equals("Male", StringComparison.InvariantCultureIgnoreCase))
                synth.SelectVoiceByHints(System.Speech.Synthesis.VoiceGender.Male);
            else if (!voice.Equals("Default", StringComparison.InvariantCultureIgnoreCase))
                synth.SelectVoice(voice);
            else
                synth.SelectVoice(systemdefaultvoice);

            synth.Volume = volume;
            synth.Rate = rate;

            System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Speak " + phrase + " " + rate);

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            synth.SetOutputToWaveStream(stream);

            synth.Speak(phrase);

            return stream;
        }
    }
#endif
}
