﻿/*
 * Copyright © 2017 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioExtensions;
using BaseUtils;
using Conditions;

namespace AudioExtensions
{
    public partial class SpeechConfigure : Form
    {
        public string SayText { get { return textBoxBorderText.Text; } }
        public bool Wait { get { return checkBoxCustomComplete.Checked; } }
        public bool Literal { get { return checkBoxCustomLiteral.Checked; } }
        public AudioQueuePriority Priority { get { return (AudioQueuePriority)Enum.Parse(typeof(AudioQueuePriority), comboBoxCustomPriority.Text); } }
        public string StartEvent { get { return textBoxBorderStartTrigger.Text; } }
        public string FinishEvent { get { return textBoxBorderEndTrigger.Text; } }
        public string VoiceName { get { return comboBoxCustomVoice.Text; } }
        public string Volume { get { return (checkBoxCustomV.Checked) ? trackBarVolume.Value.ToString() : "Default"; } }
        public string Rate { get { return (checkBoxCustomR.Checked) ? trackBarRate.Value.ToString() : "Default"; } }
        public ConditionVariables Effects { get { return effects;  } }

        AudioQueue queue;
        SpeechSynthesizer synth;
        ConditionVariables effects;

        public SpeechConfigure()
        {
            InitializeComponent();
        }

        public void Init(AudioQueue qu, SpeechSynthesizer syn,
                            string title, string caption, Icon ic,
                            String text,          // if null, no text box or wait complete
                            bool waitcomplete, bool literal,
                            AudioQueuePriority prio,
                            string startname, string endname,
                            string voicename,
                            string volume,
                            string rate,
                            ConditionVariables ef)     // effects can also contain other vars, it will ignore
        {
            comboBoxCustomPriority.Items.AddRange(Enum.GetNames(typeof(AudioQueuePriority)));

            queue = qu;
            synth = syn;
            this.Text = caption;
            Title.Text = title;
            this.Icon = ic;
            textBoxBorderTest.Text = "The quick brown fox jumped over the lazy dog";

            bool defaultmode = (text == null);

            if (defaultmode)
            {
                textBoxBorderText.Visible = checkBoxCustomComplete.Visible = comboBoxCustomPriority.Visible = labelStartTrigger.Visible = labelEndTrigger.Visible =
                checkBoxCustomLiteral.Visible = textBoxBorderStartTrigger.Visible = checkBoxCustomV.Visible = checkBoxCustomR.Visible = textBoxBorderEndTrigger.Visible = false;

                int offset = comboBoxCustomVoice.Top - textBoxBorderText.Top;
                foreach (Control c in panelOuter.Controls )
                {
                    if (!c.Name.Equals("Title"))
                        c.Location = new Point(c.Left, c.Top - offset);
                }

                this.Height -= offset;
            }
            else
            {
                textBoxBorderText.Text = text;
                checkBoxCustomComplete.Checked = waitcomplete;
                checkBoxCustomLiteral.Checked = literal;
                comboBoxCustomPriority.SelectedItem = prio.ToString();
                textBoxBorderStartTrigger.Text = startname;
                textBoxBorderEndTrigger.Text = endname;
                buttonExtDevice.Visible = false;
            }

            comboBoxCustomVoice.Items.Add("Default");
            comboBoxCustomVoice.Items.Add("Female");
            comboBoxCustomVoice.Items.Add("Male");
            comboBoxCustomVoice.Items.AddRange(synth.GetVoiceNames());
            if (comboBoxCustomVoice.Items.Contains(voicename))
                comboBoxCustomVoice.SelectedItem = voicename;
            else
                comboBoxCustomVoice.SelectedIndex = 0;

            int i;
            if (!defaultmode && volume.Equals("Default", StringComparison.InvariantCultureIgnoreCase))  
            {
                checkBoxCustomV.Checked = false;
                trackBarVolume.Enabled = false;
            }
            else
            {
                checkBoxCustomV.Checked = true;
                if (volume.InvariantParse(out i))
                    trackBarVolume.Value = i;
            }

            if (!defaultmode && rate.Equals("Default", StringComparison.InvariantCultureIgnoreCase))
            {
                checkBoxCustomR.Checked = false;
                trackBarRate.Enabled = false;
            }
            else
            {
                checkBoxCustomR.Checked = true;
                if (rate.InvariantParse(out i))
                    trackBarRate.Value = i;
            }

            effects = ef;

            ExtendedControls.ThemeableFormsInstance.Instance?.ApplyToForm(this, System.Drawing.SystemFonts.DefaultFont);
        }

        private void buttonExtOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonExtEffects_Click(object sender, EventArgs e)
        {
            using (SoundEffectsDialog sfe = new SoundEffectsDialog())
            {
                sfe.Init(this.Icon, effects, textBoxBorderText.Visible);           // give them the none option ONLY if we are allowing text
                sfe.TestSettingEvent += Sfe_TestSettingEvent;           // callback to say test
                sfe.StopTestSettingEvent += Sfe_StopTestSettingEvent;   // callback to say stop
                if (sfe.ShowDialog(this) == DialogResult.OK)
                {
                    effects = sfe.GetEffects();
                }
            }
        }

        private void Sfe_TestSettingEvent(SoundEffectsDialog sfe, ConditionVariables effects)
        {
            System.IO.MemoryStream ms = synth.Speak(textBoxBorderTest.Text, "Default", comboBoxCustomVoice.Text, trackBarRate.Value);
            if (ms != null)
            {
                AudioSample a = queue.Generate(ms, new SoundEffectSettings(effects));
                a.SampleFinished += SfeTest_SampleFinished;
                a.FinishTag = sfe;
                queue.Submit(a, trackBarVolume.Value, AudioQueuePriority.High);
            }
        }

        private void Sfe_StopTestSettingEvent(SoundEffectsDialog sender)
        {
            queue.StopCurrent();
        }

        private void SfeTest_SampleFinished(object sender, AudioSampleEventArgs e)
        {
            (e.Tag as SoundEffectsDialog).TestOver();
        }

        private void checkBoxCustomV_CheckedChanged(object sender, EventArgs e)
        {
            trackBarVolume.Enabled = checkBoxCustomV.Checked;

        }
        private void checkBoxCustomR_CheckedChanged(object sender, EventArgs e)
        {
            trackBarRate.Enabled = checkBoxCustomR.Checked;
        }

        private void buttonExtDevice_Click(object sender, EventArgs e)
        {
            using (AudioDeviceConfigure adc = new AudioDeviceConfigure())
            {
                adc.Init("Configure voice device", queue.Driver);
                if (adc.ShowDialog(this) == DialogResult.OK && !queue.SetAudioEndpoint(adc.Selected))
                {
                    ExtendedControls.MessageBoxTheme.Show(this, "Audio Device Selection failed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void buttonExtTest_Click(object sender, EventArgs e)
        {
            if (buttonExtTest.Text.Equals("Stop"))
            {
                queue.StopCurrent();
            }
            else
            {
                try
                {
                    System.IO.MemoryStream ms = synth.Speak(textBoxBorderTest.Text, "Default", comboBoxCustomVoice.Text, trackBarRate.Value);
                    if (ms != null)
                    {
                        AudioSample audio = queue.Generate(ms, new SoundEffectSettings(effects));
                        audio.SampleFinished += TestAudio_SampleFinished;
                        queue.Submit(audio, trackBarVolume.Value, AudioQueuePriority.High);
                        buttonExtTest.Text = "Stop";
                    }
                }
                catch
                {
                    ExtendedControls.MessageBoxTheme.Show(this,"Unable to play " + textBoxBorderText.Text);
                }
            }
        }

        private void TestAudio_SampleFinished(object sender, AudioSampleEventArgs e)
        {
            buttonExtTest.Text = "Test";
        }

    }
}
