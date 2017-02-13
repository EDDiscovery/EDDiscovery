using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Speech
{
    class Phrase
    {
        public string phrase;
        public string voice;
        public int volume;
        public int rate;
        public Actions.ActionProgramRun ap;
    };

    interface ISpeechEngine
    {
        string[] GetVoiceNames();
        string GetState();
        event EventHandler SpeakingCompleted;
        void SpeakAsync(Phrase p);
        void KillSpeech();
    }

    class DummySpeechEngine : ISpeechEngine
    {
        public event EventHandler SpeakingCompleted;

        public string[] GetVoiceNames()
        {
            return new string[] { };
        }

        public string GetState()
        {
            return null;
        }

        public void SpeakAsync(Phrase p)
        {
            SpeakingCompleted(this, EventArgs.Empty);
        }

        public void KillSpeech()
        {
        }
    }

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

        public string GetState()
        {
            return synth.State.ToString();
        }

        public void SpeakAsync(Phrase p)
        {
            if (p.voice.Equals("Female", StringComparison.InvariantCultureIgnoreCase))
                synth.SelectVoiceByHints(System.Speech.Synthesis.VoiceGender.Female);
            else if (p.voice.Equals("Male", StringComparison.InvariantCultureIgnoreCase))
                synth.SelectVoiceByHints(System.Speech.Synthesis.VoiceGender.Male);
            else if (!p.voice.Equals("Default", StringComparison.InvariantCultureIgnoreCase))
                synth.SelectVoice(p.voice);
            else
                synth.SelectVoice(systemdefaultvoice);

            synth.Volume = p.volume;
            synth.Rate = p.rate;

            synth.SpeakAsync(p.phrase);
        }

        public void KillSpeech()
        {
            synth.SpeakAsyncCancelAll();
        }
    }
#endif

    class QueuedSynthesizer               // if we use it outside of this, may be better as a child of the main form
    {
        private List<Phrase> phrases;
        ISpeechEngine speechengine;
        Random rnd = new Random();

        public QueuedSynthesizer()
        {
            phrases = new List<Phrase>();
#if !__MonoCS__
            // Windows TTS (2000 and above). Speech *recognition* will be Version.Major >= 6 (Vista and above)
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5)
                speechengine = new WindowsSpeechEngine();
            else
                speechengine = new DummySpeechEngine();
#else
            speechengine = new DummySpeechEngine();
#endif
            speechengine.SpeakingCompleted += Synth_SpeakCompleted;
        }

        public string[] GetVoiceNames()
        {
            return speechengine.GetVoiceNames();
        }

        public string Speak(string phraselist, string voice, int volume, int rate,
                            ConditionFunctions f, ConditionVariables curvars, EDDiscovery.Actions.ActionProgramRun ap)
        {
            string res;
            if (f.ExpandString(phraselist, curvars, out res) != EDDiscovery.ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
            {
                string[] phrasearray = res.Split(';');

                if (phrasearray.Length > 1)     // if we have at least x;y
                {
                    if (phrasearray[0].Length == 0 && phrasearray.Length >= 2)   // first empty, and we have two or more..
                    {
                        res = phrasearray[1];           // say first one
                        if (phrasearray.Length > 2)   // if we have ;first;second;third, pick random at then
                        {
                            res += phrasearray[2 + rnd.Next(phrasearray.Length - 2)];
                        }
                    }
                    else
                        res = phrasearray[rnd.Next(phrasearray.Length)];    // pick randomly
                }

                bool silent = phrases.Count == 0;
                phrases.Add(new Phrase() { phrase = res, voice = voice, volume = volume, rate = rate, ap = ap });

                if (silent)
                {
                    System.Diagnostics.Debug.WriteLine("Queue up " + phrases[0].phrase + " Voice " + voice + " Volume " + volume +" Rate" + rate);
                    StartSpeaking();
                }

                return null;
            }
            else
                return res;
        }

        private void StartSpeaking()
        {
            System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " State " + speechengine.GetState());

            Phrase p = phrases[0];

            speechengine.SpeakAsync(p);

            System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Say " + p.phrase);
        }

        private void Synth_SpeakCompleted(object sender, EventArgs e) // We appear to get them even if not playing.. handle it
        {
            if (phrases.Count > 0)
            {
                Phrase current = phrases[0];

                System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Speech finished " + current.phrase);

                phrases.RemoveAt(0);

                if (phrases.Count > 0)                  // more, start next
                    StartSpeaking();

                if (current.ap != null)                  // if we want to resume
                    current.ap.ResumeAfterPause();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Synth complete IGNORE ");
            }
        }

        public void KillSpeech()
        {
            phrases.Clear();
            speechengine.KillSpeech();
        }
    }
}
