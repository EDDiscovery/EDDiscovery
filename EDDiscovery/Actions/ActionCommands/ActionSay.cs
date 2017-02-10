using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionSay : Action
    {
        public static string globalvarspeechvolume = "SpeechVolume";
        public static string globalvarspeechrate = "SpeechRate";
        public static string globalvarspeechvoice = "SpeechVoice";
        public static string globalvarspeecheffects = "SpeechEffects";

        static string volumename = "Volume";
        static string voicename = "Voice";
        static string ratename = "Rate";
        static string waitname = "Wait";
        static string preemptname = "Preempt";

        public bool FromString(string s, out string saying, out ConditionVariables vars )
        {
            vars = new ConditionVariables();

            if (s.IndexOf(',') == -1 && s.IndexOf('"') == -1) // no quotes, no commas, just the string, probably typed in..
            {
                saying = s;
                return true;
            }
            else
            {
                StringParser p = new StringParser(s);
                saying = p.NextQuotedWord(", ");        // stop at space or comma..

                if (saying != null && (p.IsEOL || (p.IsCharMoveOn(',') && vars.FromString(p, ConditionVariables.FromMode.MultiEntryComma))))   // normalise variable names (true)
                     return true;

                saying = "";
                return false;
            }
        }

        public string ToString(string saying, ConditionVariables cond)
        {
            if (cond.Count > 0)
                return saying.QuoteString(comma: true) + ", " + cond.ToString();
            else
                return saying.QuoteString(comma: true);
        }

        public override string VerifyActionCorrect()
        {
            string saying;
            ConditionVariables vars;
            return FromString(userdata, out saying, out vars) ? null : "Say not in correct format";
        }

        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string saying;
            ConditionVariables vars;
            FromString(userdata, out saying, out vars);

            Audio.SpeechConfigure cfg = new Audio.SpeechConfigure();
            cfg.Init( discoveryform.AudioQueueSpeech, discoveryform.SpeechSynthesizer,
                        "Set Text to say (use ; to separate randomly selectable phrases)", "Configure Say Command", discoveryform.theme,
                        saying,
                        vars.ContainsKey(waitname),
                        vars.ContainsKey(preemptname),
                        vars.GetString(voicename,"Default"),
                        vars.GetString(volumename,"Default"),
                        vars.GetString(ratename,"Default"),
                        vars
                        );

            if ( cfg.ShowDialog(parent) == DialogResult.OK)
            {
                ConditionVariables cond = new ConditionVariables(cfg.Effects);// add on any effects variables (and may add in some previous variables, since we did not purge
                cond.SetOrRemove(cfg.Wait, waitname, "1");
                cond.SetOrRemove(cfg.Preempt, preemptname, "1");
                cond.SetOrRemove(!cfg.VoiceName.Equals("Default", StringComparison.InvariantCultureIgnoreCase), voicename, cfg.VoiceName);
                cond.SetOrRemove(!cfg.Volume.Equals("Default", StringComparison.InvariantCultureIgnoreCase), volumename, cfg.Volume);
                cond.SetOrRemove(!cfg.Rate.Equals("Default", StringComparison.InvariantCultureIgnoreCase), ratename, cfg.Rate);

                userdata = ToString(cfg.SayText, cond);
                return true;
            }

            return false;
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string say;
            ConditionVariables statementvars;
            FromString(userdata, out say, out statementvars);

            string errlist = null;
            ConditionVariables vars = statementvars.ExpandAll(ap.functions.ExpandString, ap.currentvars, out errlist);

            if (errlist == null)
            {
                bool wait = vars.GetInt(waitname, 0) != 0;
                bool priority = vars.GetInt(preemptname, 0) != 0;
                string voice = vars.ContainsKey(voicename) ? vars[voicename] : (ap.currentvars.ContainsKey(globalvarspeechvoice) ? ap.currentvars[globalvarspeechvoice] : "Default");

                int vol = vars.GetInt(volumename, -999);
                if (vol == -999)
                    vol = ap.currentvars.GetInt(globalvarspeechvolume, 60);

                int rate = vars.GetInt(ratename, -999);
                if ( rate == -999 )
                    rate = ap.currentvars.GetInt(globalvarspeechrate, 0);

                Audio.SoundEffectSettings ses = new Audio.SoundEffectSettings(vars);        // use the rest of the vars to place effects

                if (!ses.Any && !ses.OverrideNone && ap.currentvars.ContainsKey(globalvarspeecheffects))  // if can't see any, and override none if off, and we have a global, use that
                {
                    vars = new ConditionVariables(ap.currentvars[globalvarspeecheffects], ConditionVariables.FromMode.MultiEntryComma);
                }

                string phrase = ap.actioncontroller.DiscoveryForm.SpeechSynthesizer.ToPhrase(say, out errlist, ap.functions, ap.currentvars);

                if (errlist == null)
                {
                    if (phrase.Length == 0) // just abort..
                        return true;

#if true
                    System.IO.MemoryStream ms = ap.actioncontroller.DiscoveryForm.SpeechSynthesizer.Speak(phrase, voice, rate);

                    if (ms != null)
                    {
                        Audio.AudioQueue.AudioSample audio = ap.actioncontroller.DiscoveryForm.AudioQueueSpeech.Generate(ms, vars);

                        if (audio != null)
                        {
                            if (wait)
                            {
                                audio.sampleOverTag = ap;
                                audio.sampleOverEvent += Audio_sampleOverEvent;
                            }

                            ap.actioncontroller.DiscoveryForm.AudioQueueSpeech.Submit(audio, vol, priority);
                            return !wait;       //False if wait, meaning terminate and wait for it to complete, true otherwise, continue
                        }
                        else
                            ap.ReportError("Say could not create audio, check Effects settings");
                    }
#else
                    synth.SelectVoice(voice);
                    synth.Rate = 0;
                    synth.SpeakAsync(phrase);       // for checking quality..
#endif

                }
                else
                    ap.ReportError(errlist);
            }
            else
                ap.ReportError(errlist);

            return true;
        }

//        static System.Speech.Synthesis.SpeechSynthesizer synth = new System.Speech.Synthesis.SpeechSynthesizer();

        private void Audio_sampleOverEvent(Audio.AudioQueue sender, object tag)
        {
            ActionProgramRun ap = tag as ActionProgramRun;
            ap.ResumeAfterPause();
        }
    }
}

