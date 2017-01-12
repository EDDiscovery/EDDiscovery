using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;

namespace EDDiscovery.Actions
{
    public class ActionSay : Action
    {
        static QueuedSynthesizer synth = new QueuedSynthesizer();           // STATIC only one synth throught the whole program

        string flagOnWaitComplete = "WaitComplete";
        string flagVoice = "Voice";
        string flagRate = "Rate";
        string flagVolume = "Volume";

        public ActionSay(string n, ActionType t, List<string> c, string ud, int lu) : base(n, t, c, ud, lu)
        {
        }

        public override bool AllowDirectEditingOfUserData { get { return true; } }

        public override bool ConfigurationMenu(Form parent, EDDiscovery2.EDDTheme theme, List<string> eventvars)
        {
            Tuple<string, bool, string, string, string> promptValue =
                Prompt.ShowDialog(parent, "Set Text to say (use ; to separate randomly selectable phrases)", "Configure Say Command", theme,
                UserData, IsFlag(flagOnWaitComplete), GetFlagAuxData(flagVoice, "Default"), GetFlagAuxData(flagVolume, "Default"), GetFlagAuxData(flagRate, "Default"));

            if (promptValue != null)
            {
                userdata = promptValue.Item1;
                SetFlag(flagOnWaitComplete, promptValue.Item2);
                SetFlag(flagVoice, true, promptValue.Item3);
                SetFlag(flagVolume, true, promptValue.Item4);
                SetFlag(flagRate, true, promptValue.Item5);
                return true;
            }

            return false;
        }

        int GetInt(string value, string vname, Dictionary<string, string> vars, int fallback, int min, int max)
        {
            int i;
            if (!int.TryParse(value, out i) || i < min || i > max)
            {
                if (vars.ContainsKey(vname))
                {
                    if (!int.TryParse(vars[vname], out i) || i < min || i > max)
                        i = fallback;
                }
                else
                    i = fallback;
            }

            return i;
        }

        public override bool ExecuteAction(ActionProgramRun ap)
        {
            string say = UserData;
            bool wait = IsFlag(flagOnWaitComplete);
            string voice = GetFlagAuxData(flagVoice, ap.currentvars.ContainsKey("SpeechVoice") ? ap.currentvars["SpeechVoice"] : "Default");

            string volume;
            if (ap.functions.ExpandString(GetFlagAuxData(flagVolume, "Default"), ap.currentvars, out volume) == EDDiscovery.ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
            {
                ap.ReportError(volume);
                return true;
            }

            int vol = GetInt(volume, "SpeechVolume", ap.currentvars, 60, 0, 100);

            string rate;
            if (ap.functions.ExpandString(GetFlagAuxData(flagRate, "Default"), ap.currentvars, out rate) == EDDiscovery.ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
            {
                ap.ReportError(rate);
                return true;
            }

            int rat = GetInt(rate, "SpeechRate", ap.currentvars, 0, -10, 10);

            string s = synth.Speak(UserData, voice, vol, rat, ap.functions, ap.currentvars, (wait) ? ap : null);

            if (s != null)
            {
                ap.ReportError(s);
                return true;
            }
            else
                return !wait;       //False if wait, meaning terminate and wait for it to complete, true otherwise, continue
        }

        static class Prompt
        {
            public static Tuple<string, bool, string, string, string> ShowDialog(Form p, string title, string caption, EDDiscovery2.EDDTheme theme,
                                       String text, bool waitcomplete, string voicename, string volume, string rate)

            {
                SpeechSynthesizer synth = new SpeechSynthesizer();

                List<string> voices = new List<string>();

                foreach (InstalledVoice voice in synth.GetInstalledVoices())
                {
                    VoiceInfo info = voice.VoiceInfo;
                    voices.Add(info.Name);
                }

                Form prompt = new Form()
                {
                    Width = 440,
                    Height = 300,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen,
                };

                Panel outerpanel = new Panel() { Left = 5, Top = 5, Width = prompt.ClientRectangle.Width - 10, Height = prompt.ClientRectangle.Height - 10, BorderStyle = BorderStyle.FixedSingle };
                prompt.Controls.Add(outerpanel);

                Label textLabel = new Label() { Left = 10, Top = 20, Width = 400, Text = title };
                ExtendedControls.TextBoxBorder textBox = new ExtendedControls.TextBoxBorder() { Left = 10, Top = 50, Width = outerpanel.Width - 20, Text = text };

                ExtendedControls.CheckBoxCustom checkBox1 = new ExtendedControls.CheckBoxCustom() { Left = 10, Top = 80, Width = 400, Height = 20, Text = "Wait until complete", Checked = waitcomplete };

                ExtendedControls.ComboBoxCustom comboboxvoice = new ExtendedControls.ComboBoxCustom() { Left = 10, Top = 110, Width = 200, Height = 24 };
                comboboxvoice.Items.Add("Default");
                comboboxvoice.Items.Add("Female");
                comboboxvoice.Items.Add("Male");
                comboboxvoice.Items.AddRange(voices);
                comboboxvoice.SelectedItem = voicename;

                Label textLabel2 = new Label() { Left = 10, Top = 140, Width = 60, Text = "Volume" };
                ExtendedControls.TextBoxBorder textBoxvolume = new ExtendedControls.TextBoxBorder() { Left = 80, Top = textLabel2.Top, Width = 130, Text = volume };
                Label textLabel2a = new Label() { Left = textBoxvolume.Right + 8, Top = textLabel2.Top, Width = 200, Text = "Default, or 0-100" };

                Label textLabel3 = new Label() { Left = 10, Top = 170, Width = 60, Text = "Rate" };
                ExtendedControls.TextBoxBorder textBoxrate = new ExtendedControls.TextBoxBorder() { Left = 80, Top = textLabel3.Top, Width = 130, Text = rate };
                Label textLabel3a = new Label() { Left = textBoxrate.Right + 8, Top = textLabel3.Top, Width = 200, Text = "Default, or -10 to +10" };

                ExtendedControls.ButtonExt confirmation = new ExtendedControls.ButtonExt() { Text = "Ok", Left = textBox.Right - 80, Width = 80, Top = 210, DialogResult = DialogResult.OK };
                ExtendedControls.ButtonExt cancel = new ExtendedControls.ButtonExt() { Text = "Cancel", Left = confirmation.Left - 100, Width = 80, Top = confirmation.Top, DialogResult = DialogResult.Cancel };

                confirmation.Click += (sender, e) => { prompt.Close(); };
                cancel.Click += (sender, e) => { prompt.Close(); };

                prompt.ShowInTaskbar = false;

                outerpanel.Controls.AddRange(new Control[] { textBox, confirmation, cancel, textLabel, checkBox1, comboboxvoice, textBoxrate, textBoxvolume, textLabel2, textLabel3, textLabel2a, textLabel3a });

                prompt.AcceptButton = confirmation;
                prompt.CancelButton = cancel;

                theme.ApplyToForm(prompt, System.Drawing.SystemFonts.DefaultFont);

                return prompt.ShowDialog(p) == DialogResult.OK ? new Tuple<string, bool, string, string, string>
                            (textBox.Text, checkBox1.Checked, comboboxvoice.Text, textBoxvolume.Text, textBoxrate.Text) : null;
            }
        }
    }

    class QueuedSynthesizer               // if we use it outside of this, may be better as a child of the main form
    {
        class Phrase
        {
            public string phrase;
            public string voice;
            public int volume;
            public int rate;
            public ActionProgramRun ap;
        };
        
        private List<Phrase> phrases;
        SpeechSynthesizer synth;
        Random rnd = new Random();

        public QueuedSynthesizer()
        {
            phrases = new List<Phrase>();
            synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
            synth.SpeakCompleted += Synth_SpeakCompleted;
        }

        public string Speak(string phraselist, string voice, int volume, int rate, 
                            EDDiscovery.Actions.ActionFunctions f, Dictionary<string, string> curvars , EDDiscovery.Actions.ActionProgramRun ap )
        {
            string[] phrasearray = phraselist.Split(';');

            if (phrasearray.Length > 1)
                phraselist = phrasearray[rnd.Next(phrasearray.Length)];

            string res;
            if (f.ExpandString(phraselist, curvars, out res) != EDDiscovery.ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
            {
                phrases.Add(new Phrase() { phrase = res, voice = voice, volume = volume, rate = rate , ap = ap });

                if (synth.State == SynthesizerState.Ready)
                    StartSpeaking();

                return null;
            }
            else
                return res;
        }

        private void StartSpeaking()
        {
            Phrase p = phrases[0];

            if (p.voice.Equals("Female"))
                synth.SelectVoiceByHints(VoiceGender.Female);
            else if (p.voice.Equals("Male"))
                synth.SelectVoiceByHints(VoiceGender.Male);
            else if (!p.voice.Equals("Default"))
                synth.SelectVoice(p.voice);

            synth.Volume = p.volume;
            synth.Rate = p.rate;

            synth.SpeakAsync(p.phrase);
            System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Say " + p.phrase);
        }

        private void Synth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            Phrase current = phrases[0];

            System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Speech finished " + current.phrase);

            phrases.RemoveAt(0);

            if (phrases.Count > 0)                  // more, start next
                StartSpeaking();

            if (current.ap != null)                  // if we want to resume
                current.ap.ResumeAfterPause();
        }
    }
}
