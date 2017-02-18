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

namespace EDDiscovery.Audio
{
    public partial class SpeechConfigure : Form
    {
        public bool Wait { get { return checkBoxCustomComplete.Checked; } }

        public string SayText { get { return textBoxBorderText.Text; } }
        public AudioQueue.Priority Priority { get { return (AudioQueue.Priority)Enum.Parse(typeof(AudioQueue.Priority), comboBoxCustomPriority.Text); } }
        public string StartEvent { get { return textBoxBorderStartTrigger.Text; } }
        public string FinishEvent { get { return textBoxBorderEndTrigger.Text; } }
        public string VoiceName { get { return comboBoxCustomVoice.Text; } }
        public string Volume { get { return (checkBoxCustomV.Checked) ? trackBarVolume.Value.ToString() : "Default"; } }
        public string Rate { get { return (checkBoxCustomR.Checked) ? trackBarRate.Value.ToString() : "Default"; } }
        public ConditionVariables Effects { get { return effects;  } }

        AudioQueue queue;
        SpeechSynthesizer synth;
        ConditionVariables effects;
        EDDiscovery2.EDDTheme theme;

        public SpeechConfigure()
        {
            InitializeComponent();
        }

        public void Init(AudioQueue qu, SpeechSynthesizer syn,
                            string title, string caption, EDDiscovery2.EDDTheme th,
                            String text,          // if null, no text box or wait complete
                            bool waitcomplete,
                            AudioQueue.Priority prio,
                            string startname, string endname,
                            string voicename,
                            string volume,
                            string rate,
                            ConditionVariables ef)     // effects can also contain other vars, it will ignore
        {
            comboBoxCustomPriority.Items.AddRange(Enum.GetNames(typeof(AudioQueue.Priority)));

            queue = qu;
            synth = syn;
            theme = th;
            this.Text = caption;
            Title.Text = title;
            textBoxBorderTest.Text = "The quick brown fox jumped over the lazy dog";

            bool defaultmode = (text == null);

            if (defaultmode)
            {
                textBoxBorderText.Visible = checkBoxCustomComplete.Visible = comboBoxCustomPriority.Visible = labelStartTrigger.Visible = labelEndTrigger.Visible =
                textBoxBorderStartTrigger.Visible = checkBoxCustomV.Visible = checkBoxCustomR.Visible = textBoxBorderEndTrigger.Visible = false;

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
                comboBoxCustomPriority.SelectedItem = prio.ToString();
                textBoxBorderStartTrigger.Text = startname;
                textBoxBorderEndTrigger.Text = endname;
            }

            comboBoxCustomVoice.Items.Add("Default");
            comboBoxCustomVoice.Items.Add("Female");
            comboBoxCustomVoice.Items.Add("Male");
            comboBoxCustomVoice.Items.AddRange(synth.GetVoiceNames());
            comboBoxCustomVoice.SelectedItem = voicename;

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

            theme.ApplyToForm(this, System.Drawing.SystemFonts.DefaultFont);
        }

        private void buttonExtOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonExtEffects_Click(object sender, EventArgs e)
        {
            SoundEffectsDialog sfe = new SoundEffectsDialog();
            sfe.Init(effects, textBoxBorderText.Visible, theme);           // give them the none option ONLY if we are allowing text
            sfe.TestSettingEvent += Sfe_TestSettingEvent;           // callback to say test
            sfe.StopTestSettingEvent += Sfe_StopTestSettingEvent;   // callback to say stop
            if ( sfe.ShowDialog(this) == DialogResult.OK )
            {
                effects = sfe.GetEffects();
            }
        }

        private void Sfe_TestSettingEvent(SoundEffectsDialog sfe, ConditionVariables effects)
        {
            System.IO.MemoryStream ms = synth.Speak(textBoxBorderTest.Text, "Default", comboBoxCustomVoice.Text, trackBarRate.Value);
            if (ms != null)
            {
                AudioQueue.AudioSample a = queue.Generate(ms, effects);
                a.sampleOverEvent += SampleOver;
                a.sampleOverTag = sfe;
                queue.Submit(a, trackBarVolume.Value, AudioQueue.Priority.High);
            }
        }

        private void Sfe_StopTestSettingEvent(SoundEffectsDialog sender)
        {
            queue.StopCurrent();
        }

        private void SampleOver(AudioQueue s, Object tag)
        {
            SoundEffectsDialog sfe = tag as SoundEffectsDialog;
            sfe.TestOver();
        }

        private void checkBoxCustomV_CheckedChanged(object sender, EventArgs e)
        {
            trackBarVolume.Enabled = checkBoxCustomV.Checked;

        }
        private void checkBoxCustomR_CheckedChanged(object sender, EventArgs e)
        {
            trackBarRate.Enabled = checkBoxCustomR.Checked;
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
                        Audio.AudioQueue.AudioSample audio = queue.Generate(ms, effects);
                        audio.sampleOverEvent += Audio_sampleOverEvent;
                        queue.Submit(audio, trackBarVolume.Value, AudioQueue.Priority.High);
                        buttonExtTest.Text = "Stop";
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to play " + textBoxBorderText.Text);
                }
            }
        }

        private void Audio_sampleOverEvent(AudioQueue sender, object tag)
        {
            buttonExtTest.Text = "Test";
        }
    }
}
