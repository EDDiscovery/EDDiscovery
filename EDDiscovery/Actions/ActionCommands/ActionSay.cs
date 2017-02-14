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
        private static Speech.QueuedSynthesizer synth = new Speech.QueuedSynthesizer();           // STATIC only one synth throught the whole program
        public static void KillSpeech() { synth.KillSpeech(); }

        public static string globalvarspeechvolume = "SpeechVolume";
        public static string globalvarspeechrate = "SpeechRate";
        public static string globalvarspeechvoice = "SpeechVoice";

        static string volumename = "Volume";
        static string voicename = "Voice";
        static string ratename = "Rate";
        static string waitname = "Wait";
        static List<string> validnames = new List<string>() { voicename, volumename, ratename, waitname };

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

                if (saying != null && (p.IsEOL || (p.IsCharMoveOn(',') && vars.FromString(p, ConditionVariables.FromMode.MultiEntryComma, validnames, true))))   // normalise variable names (true)
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

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            string saying;
            ConditionVariables vars;
            FromString(userdata, out saying, out vars);

            Speech.SpeechConfigure cfg = new Speech.SpeechConfigure();
            cfg.Init("Set Text to say (use ; to separate randomly selectable phrases)", "Configure Say Command", theme,
                        saying, vars.ContainsKey(waitname),
                        synth.GetVoiceNames(),
                        vars.ContainsKey(voicename) ? vars[voicename] : "Default",
                        vars.ContainsKey(volumename) ? vars[volumename] : "Default",
                        vars.ContainsKey(ratename) ? vars[ratename] : "Default");

            if ( cfg.ShowDialog(parent) == DialogResult.OK)
            {
                ConditionVariables cond = new ConditionVariables();

                if (cfg.Wait)
                    cond[waitname] = "1";
                if (!cfg.VoiceName.Equals("Default", StringComparison.InvariantCultureIgnoreCase))
                    cond[voicename] = cfg.VoiceName;
                if (!cfg.Volume.Equals("Default", StringComparison.InvariantCultureIgnoreCase))
                    cond[volumename] = cfg.Volume;
                if (!cfg.Rate.Equals("Default", StringComparison.InvariantCultureIgnoreCase))
                    cond[ratename] = cfg.Rate;

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

            string s = synth.Speak(say, voice, vol, rate, ap.functions, ap.currentvars, (wait) ? ap : null);

            if (s != null)
            {
                ap.ReportError(s);
                return true;
            }
            else
                return !wait;       //False if wait, meaning terminate and wait for it to complete, true otherwise, continue
        }
    }
}
