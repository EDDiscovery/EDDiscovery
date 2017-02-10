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

        int GetInt(string value, string vname, Dictionary<string, string> vars, int fallback, int min, int max)
        {
            int i;
            if (!value.InvariantParse(out i) || i < min || i > max)
            {
                if (vars.ContainsKey(vname))
                {
                    if (!vars[vname].InvariantParse(out i) || i < min || i > max)
                        i = fallback;
                }
                else
                    i = fallback;
            }

            return i;
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string say;
            ConditionVariables vars;
            FromString(userdata, out say, out vars);

            bool wait = vars.ContainsKey(waitname);
            bool priority = vars.ContainsKey(preemptname);

            string voice = vars.ContainsKey(voicename) ? vars[voicename] : (ap.currentvars.ContainsKey(globalvarspeechvoice) ? ap.currentvars[globalvarspeechvoice] : "Default");

            int vol;
            string evalres = vars.GetNumericValue(volumename, 0, 100, -999, out vol, ap.functions.ExpandString, ap.currentvars); // expand this..
            if (evalres != null)
            {
                ap.ReportError(evalres);
                return true;
            }

            if (vol == -999)
                ap.currentvars.GetNumericValue(globalvarspeechvolume, 0, 100, 60, out vol);      // don't care about the return, do not expand, its just a number.. if it fails, use def

            int rate;
            evalres = vars.GetNumericValue(ratename, -10,10,-999, out rate, ap.functions.ExpandString, ap.currentvars); // expand this.. from our own vars
            if (evalres != null)
            {
                ap.ReportError(evalres);
                return true;
            }

            if (rate == -999)
                ap.currentvars.GetNumericValue(globalvarspeechrate, -10,10,0, out rate);      // don't care about the return, do not expand, its just a number.. if it fails, use def

            ConditionVariables effects = vars;

            Audio.SoundEffectSettings ses = new Audio.SoundEffectSettings(effects);
            if ( !ses.Any && ap.currentvars.ContainsKey(globalvarspeecheffects))        // if can't see any, and we have a global, use that
            {
                effects = new ConditionVariables(ap.currentvars[globalvarspeecheffects], ConditionVariables.FromMode.MultiEntryComma);
            }

            string errlist;
            System.IO.MemoryStream ms = ap.actioncontroller.DiscoveryForm.SpeechSynthesizer.Speak(say, voice, rate, out errlist, ap.functions, ap.currentvars);

            if (ms != null)
            {
                Audio.AudioQueue.AudioSample audio = ap.actioncontroller.DiscoveryForm.AudioQueueSpeech.Generate(ms, effects);
                if (audio != null)
                {
                    if ( wait )
                    {
                        audio.sampleOverTag = ap;
                        audio.sampleOverEvent += Audio_sampleOverEvent;
                    }

                    ap.actioncontroller.DiscoveryForm.AudioQueueSpeech.Submit(audio, vol, priority);
                    return !wait;       //False if wait, meaning terminate and wait for it to complete, true otherwise, continue
                }
            }
            else
                ap.ReportError(errlist);

            return true;
                // callback here to call ap.Resume
                // TBD ap call back                
                //            ap.actioncontroller.DiscoveryForm.SpeechSynthesizer.Speak(say, voice, vol, rate, ap.functions, ap.currentvars, (wait) ? ap : null)string s = ;
            }

        private void Audio_sampleOverEvent(Audio.AudioQueue sender, object tag)
        {
            ActionProgramRun ap = tag as ActionProgramRun;
            ap.ResumeAfterPause();
        }
    }
}
