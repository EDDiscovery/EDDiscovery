using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace AudioExtensions
{
    public class VoiceRecognitionWindows : VoiceRecognition
    {
        public float Confidence { get; set; } = 0.96F;

        public event SpeechRecognised SpeechRecognised;
        public bool IsOpen { get { return engine != null; } }

        private SpeechRecognitionEngine engine;
        private System.Globalization.CultureInfo ct;

        public bool Open(System.Globalization.CultureInfo ctp)
        {
            ct = ctp;

            if (engine != null)
                engine.Dispose();

            engine = new SpeechRecognitionEngine(ct);

            try
            {
                engine.SetInputToDefaultAudioDevice(); // crashes if no default device..
            }
            catch
            {
                engine = null;
                return false;
            }

            engine.SpeechRecognized += Engine_SpeechRecognized;
            engine.SpeechHypothesized += Engine_SpeechHypothesized;
            engine.SpeechRecognitionRejected += Engine_SpeechRecognitionRejected;

            System.Diagnostics.Debug.WriteLine("Engine {0}", engine.RecognizerInfo.Description);
            foreach (var x in engine.RecognizerInfo.AdditionalInfo)
                System.Diagnostics.Debug.WriteLine(".. " + x.Key + "=" + x.Value);

            return true;
        }

        public void Close()
        {
            if (engine != null )
            {
                Stop(true);
                System.Diagnostics.Debug.WriteLine("Closing");
                engine.SpeechRecognized -= Engine_SpeechRecognized;
                engine.Dispose();
                engine = null;
            }
        }

        public bool Start()
        {
            if (engine != null && engine.Grammars.Count > 0 && engine.AudioState == AudioState.Stopped)
            {
                System.Diagnostics.Debug.WriteLine("Starting with {0} Babble {1} EndSel {2} EndSelAmb {3} Initial {4} MaxAlt {5}", engine.Grammars.Count, engine.BabbleTimeout, engine.EndSilenceTimeout, engine.EndSilenceTimeoutAmbiguous, engine.InitialSilenceTimeout, engine.MaxAlternates);
                engine.RecognizeAsync(RecognizeMode.Multiple);        // got a grammar, start..
                return true;
            }
            else
                return false;
        }

        public void Stop(bool waitfor)
        {
            if (engine != null && engine.AudioState != AudioState.Stopped)
            {
                System.Diagnostics.Debug.WriteLine(Environment.TickCount + " Voice Recognition Stopping");
                engine.UnloadAllGrammars();
                engine.RecognizeAsyncCancel();

                if (waitfor)
                {
                    int max = 100;
                    while (max-- > 0 && engine.AudioState != AudioState.Stopped)
                        System.Threading.Thread.Sleep(10);
                    if (max <= 0)
                        ExtendedControls.MessageBoxTheme.Show("Voice recon did not stop", "Voice Audio Problem");
                }

                System.Diagnostics.Debug.WriteLine(Environment.TickCount + "Voice Recognition Stopped");
            }
        }

        public bool AddRange(List<string> s)
        {
            foreach (string x in s)
            {
                if (x.Length>0 && !Add(x))
                    return false;
            }

            return true;
        }

        public bool Add(string s)
        {
           // System.Diagnostics.Debug.WriteLine("Engine State" + engine?.AudioState);
            if (engine != null && engine.AudioState == AudioState.Stopped && s.Length>0)
            {
                GrammarBuilder builder = new GrammarBuilder(s);
                builder.Culture = ct;

                Grammar gr = new Grammar(builder);
                engine.LoadGrammar(gr);

                System.Diagnostics.Debug.WriteLine("add " + s + " total " + engine.Grammars.Count);
                return true;
            }
            else
                return false;
        }


        private void Engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var x = e.Result;
            DumpInfo("Recognised", e.Result);
            System.Diagnostics.Debug.WriteLine("Confidence {0} vs threshold {1}", e.Result.Confidence, Confidence);
            if (e.Result.Confidence >= Confidence)
                SpeechRecognised?.Invoke(e.Result.Text, e.Result.Confidence);
        }

        private void Engine_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            DumpInfo("Rejected", e.Result);
        }

        private void Engine_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            DumpInfo("Hypothesised", e.Result);
        }


        void DumpInfo(string t, RecognitionResult r)
        {
            System.Diagnostics.Debug.WriteLine(t + " " + r.Text + " " + r.Confidence.ToString("#.00"));
            foreach (RecognizedPhrase p in r.Alternates)
                System.Diagnostics.Debug.WriteLine("... alt " + p.Text + p.Confidence.ToString("#.00"));

            foreach (KeyValuePair<String, SemanticValue> child in r.Semantics)
            {
                System.Diagnostics.Debug.WriteLine("    The {0} city is {1}",
                  child.Key, child.Value.Value ?? "null");
            }
            foreach (RecognizedWordUnit word in r.Words)
            {
                System.Diagnostics.Debug.WriteLine(
                  "    Lexical form ({1})" +
                  " Pronunciation ({0})" +
                  " Display form ({2})",
                  word.Pronunciation, word.LexicalForm, word.DisplayAttributes);
            }

        }

    }
}
