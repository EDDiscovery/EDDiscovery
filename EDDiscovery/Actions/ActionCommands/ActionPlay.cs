using EDDiscovery.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Actions
{
    public class ActionPlay : Action
    {
        public override bool AllowDirectEditingOfUserData { get { return true; } }    // and allow editing?

        public static string globalvarplayvolume = "WaveVolume";
        public static string globalvarplayeffects = "WaveEffects";

        static string volumename = "Volume";
        static string waitname = "Wait";
        static string preemptname = "Preempt";

        public bool FromString(string s, out string path, out ConditionVariables vars)
        {
            vars = new ConditionVariables();

            if (s.IndexOf(',') == -1 && s.IndexOf('"') == -1) // no quotes, no commas, just the string, probably typed in..
            {
                path = s;
                return true;
            }
            else
            {
                StringParser p = new StringParser(s);
                path = p.NextQuotedWord(", ");        // stop at space or comma..

                if (path != null && (p.IsEOL || (p.IsCharMoveOn(',') && vars.FromString(p, ConditionVariables.FromMode.MultiEntryComma))))   // normalise variable names (true)
                    return true;

                path = "";
                return false;
            }
        }

        public string ToString(string path, ConditionVariables cond)
        {
            if (cond.Count > 0)
                return path.QuoteString(comma: true) + ", " + cond.ToString();
            else
                return path.QuoteString(comma: true);
        }

        public override string VerifyActionCorrect()
        {
            string path;
            ConditionVariables vars;
            return FromString(userdata, out path, out vars) ? null : "Play command line not in correct format";
        }

        public override bool ConfigurationMenu(Form parent, EDDiscoveryForm discoveryform, List<string> eventvars)
        {
            string path;
            ConditionVariables vars;
            FromString(userdata, out path, out vars);

            WaveConfigureDialog dlg = new WaveConfigureDialog();
            dlg.Init(discoveryform.AudioQueueWave, false, "Configure Play Command", discoveryform.theme, path,
                        vars.ContainsKey(waitname),
                        vars.ContainsKey(preemptname),
                        vars.GetString(volumename, "Default"),
                        vars);

            if (dlg.ShowDialog(parent) == DialogResult.OK)
            {
                ConditionVariables cond = new ConditionVariables(dlg.Effects);// add on any effects variables (and may add in some previous variables, since we did not purge)
                cond.SetOrRemove(dlg.Wait, waitname, "1");
                cond.SetOrRemove(dlg.Preempt, preemptname, "1");
                cond.SetOrRemove(!dlg.Volume.Equals("Default", StringComparison.InvariantCultureIgnoreCase), volumename, dlg.Volume);
                userdata = ToString(dlg.Path, cond);
                return true;
            }

            return false;
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string pathunexpanded;
            ConditionVariables statementvars;
            if (FromString(userdata, out pathunexpanded, out statementvars))
            {
                string errlist = null;
                ConditionVariables vars = statementvars.ExpandAll(ap.functions.ExpandString, ap.currentvars, out errlist);

                if (errlist == null)
                {
                    string path;
                    if (ap.functions.ExpandString(pathunexpanded, ap.currentvars, out path) != ConditionLists.ExpandResult.Failed)
                    {
                        if (System.IO.File.Exists(path))
                        {
                            bool wait = vars.GetInt(waitname, 0) != 0;
                            bool priority = vars.GetInt(preemptname, 0) != 0;

                            int vol = vars.GetInt(volumename, -999);
                            if (vol == -999)
                                vol = ap.currentvars.GetInt(globalvarplayvolume, 60);

                            Audio.SoundEffectSettings ses = new Audio.SoundEffectSettings(vars);        // use the rest of the vars to place effects

                            if (!ses.Any && !ses.OverrideNone && ap.currentvars.ContainsKey(globalvarplayeffects))  // if can't see any, and override none if off, and we have a global, use that
                            {
                                vars = new ConditionVariables(ap.currentvars[globalvarplayeffects], ConditionVariables.FromMode.MultiEntryComma);
                            }

                            Audio.AudioQueue.AudioSample audio = ap.actioncontroller.DiscoveryForm.AudioQueueWave.Generate(path, vars);

                            if (audio != null)
                            {
                                if (wait)
                                {
                                    audio.sampleOverTag = ap;
                                    audio.sampleOverEvent += Audio_sampleOverEvent;
                                }

                                ap.actioncontroller.DiscoveryForm.AudioQueueWave.Submit(audio, vol, priority);
                                return !wait;       //False if wait, meaning terminate and wait for it to complete, true otherwise, continue
                            }
                            else
                                ap.ReportError("Play could not create audio, check audio file format is supported and effects settings");
                        }
                        else
                            ap.ReportError("Play could not find file " + path);
                    }
                    else
                        ap.ReportError(path);
                }
                else
                    ap.ReportError(errlist);
            }
            else
                ap.ReportError("Play command line not in correct format");

            return true;
        }

        private void Audio_sampleOverEvent(Audio.AudioQueue sender, object tag)
        {
            ActionProgramRun ap = tag as ActionProgramRun;
            ap.ResumeAfterPause();
        }
    }
}
