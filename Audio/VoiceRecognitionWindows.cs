using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioExtensions
{
#if !NO_SYSTEM_SPEECH
    using System.Speech.Recognition;

    public class VoiceRecognitionWindows : IVoiceRecognition
    {
        public float Confidence { get; set; } = 0.96F;

        // WARNING Engine must be started, and they may except if out of range.
        public int BabbleTimeout { get { return (int)engine.BabbleTimeout.TotalMilliseconds; } set { engine.BabbleTimeout = new TimeSpan(0, 0, 0, 0, value); } }
        public int EndSilenceTimeout { get { return (int)engine.EndSilenceTimeout.TotalMilliseconds; } set { engine.EndSilenceTimeout = new TimeSpan(0, 0, 0, 0, value); } }
        public int EndSilenceTimeoutAmbigious { get { return (int)engine.EndSilenceTimeoutAmbiguous.TotalMilliseconds; } set { engine.EndSilenceTimeoutAmbiguous = new TimeSpan(0, 0, 0, 0, value); } }
        public int InitialSilenceTimeout { get { return (int)engine.InitialSilenceTimeout.TotalMilliseconds; } set { engine.InitialSilenceTimeout = new TimeSpan(0, 0, 0, 0, value); } }

        public event SpeechRecognised SpeechRecognised;
        public event SpeechRecognised SpeechNotRecognised;

        public bool IsOpen { get { return engine != null; } }

        private SpeechRecognitionEngine engine;
        private System.Globalization.CultureInfo ct;

        public bool Open(System.Globalization.CultureInfo ctp)
        {
            ct = ctp;

            if (engine != null)
                engine.Dispose();

            try
            {
                engine = new SpeechRecognitionEngine(ct);       // may except if ct is not there on machine
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

            //System.Diagnostics.Debug.WriteLine("Engine {0}", engine.RecognizerInfo.Description);
            //foreach (var x in engine.RecognizerInfo.AdditionalInfo)
            //    System.Diagnostics.Debug.WriteLine(".. " + x.Key + "=" + x.Value);

            return true;
        }

        public void Close()
        {
            if (engine != null )
            {
                Stop(true);
                engine.SpeechRecognized -= Engine_SpeechRecognized;
                engine.Dispose();
                engine = null;
            }
        }

        public bool Start()
        {
            if (engine != null && engine.Grammars.Count > 0 && engine.AudioState == AudioState.Stopped)
            {
                //System.Diagnostics.Debug.WriteLine("Starting with {0} Babble {1} EndSel {2} EndSelAmb {3} Initial {4} MaxAlt {5}", engine.Grammars.Count, engine.BabbleTimeout, engine.EndSilenceTimeout, engine.EndSilenceTimeoutAmbiguous, engine.InitialSilenceTimeout, engine.MaxAlternates);
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
                System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000+ " Voice Recognition Stopping");
                engine.RecognizeAsyncCancel();

                if (waitfor)
                {
                    int max = 100;
                    while (max-- > 0 && engine.AudioState != AudioState.Stopped)
                        System.Threading.Thread.Sleep(10);
                    if (max <= 0)
                        ExtendedControls.MessageBoxTheme.Show("Voice recon did not stop", "Voice Audio Problem");
                }

                System.Diagnostics.Debug.WriteLine(Environment.TickCount % 10000 + " Voice Recognition Stopped");
            }
        }

        public bool Clear()     // clear loaded grammars
        {
            if (engine != null && engine.AudioState == AudioState.Stopped)
            {
                engine.UnloadAllGrammars();
                return true;
            }
            else
                return false;
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

        public bool Add(string s)       // S is in voice prompt format with [] and | and ; seperating
        {
            // System.Diagnostics.Debug.WriteLine("Engine State" + engine?.AudioState);
            if (engine != null && engine.AudioState == AudioState.Stopped && s.Length>0)
            {
                BaseUtils.StringCombinations sb = new BaseUtils.StringCombinations();

                foreach (List<List<string>> groups in sb.ParseGroup(s))     // for each semicolon group, give me the word combinations
                {
                    GrammarBuilder builder = new GrammarBuilder();

                    foreach (List<string> wordlist in groups)     // for each vertical word list
                    {
                        if (wordlist.Count == 1)      // single entry, must be simple, just add
                        {
                            builder.Append(wordlist[0]);
                            //System.Diagnostics.Debug.Write(wordlist[0]);
                        }
                        else
                        {                             // conditional list..
                            List<GrammarBuilder> sub = new List<GrammarBuilder>();

                            int emptycount = (from x in wordlist where x.Length == 0 select x).Count();   // is there any empty ones indicating optionality

                            foreach (string o in wordlist)
                            {
                                if (o.Length > 0)
                                {
                                    sub.Add(new GrammarBuilder());
                                    sub.Last().Append(o, (emptycount > 0) ? 0 : 1, 1);      // indicate number of times, either 1:1 or 0,1 if we have an optional entry
                                    //System.Diagnostics.Debug.Write((wordlist.IndexOf(o) > 0 ? "|" : "") + o);
                                }
                                else
                                {
                                    //System.Diagnostics.Debug.Write("|[]");
                                }
                            }

                            Choices c = new Choices(sub.ToArray());
                            builder.Append(c);
                        }

                        //System.Diagnostics.Debug.Write(" ");
                    }

                    builder.Culture = ct;
                    Grammar gr = new Grammar(builder);
                    engine.LoadGrammar(gr);

                    //System.Diagnostics.Debug.WriteLine("");
                }
                
                return true;
            }
            else
                return false;
        }


        private void Engine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            //DumpInfo("Recognised", e.Result);
            //System.Diagnostics.Debug.WriteLine("Confidence {0} vs threshold {1} for {2}", e.Result.Confidence, Confidence, e.Result.Text);
            if (e.Result.Confidence >= Confidence)
                SpeechRecognised?.Invoke(e.Result.Text, e.Result.Confidence);
            else
                SpeechNotRecognised?.Invoke(e.Result.Text, e.Result.Confidence);
        }

        private void Engine_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            //DumpInfo("Rejected", e.Result);
            SpeechNotRecognised?.Invoke(e.Result.Text, e.Result.Confidence);
        }

        private void Engine_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            //DumpInfo("Hypothesised", e.Result);
        }


        void DumpInfo(string t, RecognitionResult r)
        {
            System.Diagnostics.Debug.WriteLine((Environment.TickCount%10000) + ":" + t + " " + r.Text + " " + r.Confidence.ToString("#.00"));
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
#endif
}
