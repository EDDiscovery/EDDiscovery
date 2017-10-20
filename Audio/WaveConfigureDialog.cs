/*
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
using BaseUtils;
using Conditions;

namespace AudioExtensions
{
    public partial class WaveConfigureDialog : Form
    {
        public string Path {  get { return textBoxBorderText.Text; } }
        public bool Wait { get { return checkBoxCustomComplete.Checked; } }
        public AudioQueue.Priority Priority { get { return (AudioQueue.Priority)Enum.Parse(typeof(AudioQueue.Priority), comboBoxCustomPriority.Text); } }
        public string StartEvent { get { return textBoxBorderStartTrigger.Text; } }
        public string FinishEvent { get { return textBoxBorderEndTrigger.Text; } }
        public string Volume { get { return (checkBoxCustomV.Checked) ? trackBarVolume.Value.ToString() : "Default"; } }
        public ConditionVariables Effects { get { return effects; } }

        ConditionVariables effects;
        AudioQueue queue;

        public WaveConfigureDialog()
        {
            InitializeComponent();
        }

        public void Init(AudioQueue qu, 
                          bool defaultmode,
                          string title, string caption, Icon ic,
                          string defpath,
                          bool waitcomplete,
                          AudioQueue.Priority prio,
                          string startname, string endname,
                          string volume,
                          ConditionVariables ef)
        {
            comboBoxCustomPriority.Items.AddRange(Enum.GetNames(typeof(AudioQueue.Priority)));

            queue = qu;
            this.Text = caption;
            labelTitle.Text = title;
            this.Icon = ic;
            textBoxBorderText.Text = defpath;

            if (defaultmode)
            {
                checkBoxCustomComplete.Visible = comboBoxCustomPriority.Visible =
                textBoxBorderStartTrigger.Visible = textBoxBorderEndTrigger.Visible = checkBoxCustomV.Visible = labelStartTrigger.Visible = labelEndTrigger.Visible = false;
            }
            else
            {
                checkBoxCustomComplete.Checked = waitcomplete;
                comboBoxCustomPriority.SelectedItem = prio.ToString();
                textBoxBorderStartTrigger.Text = startname;
                textBoxBorderEndTrigger.Text = endname;
                buttonExtDevice.Visible = false;
            }

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

            effects = ef;

            ExtendedControls.ThemeableFormsInstance.Instance.ApplyToForm(this, System.Drawing.SystemFonts.DefaultFont);
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
                    AudioQueue.AudioSample audio = queue.Generate(textBoxBorderText.Text, new SoundEffectSettings(effects));
                    audio.sampleOverEvent += Audio_sampleOverEvent;
                    queue.Submit(audio, trackBarVolume.Value, AudioQueue.Priority.High);
                    buttonExtTest.Text = "Stop";
                }
                catch
                {
                    ExtendedControls.MessageBoxTheme.Show(this,"Unable to play " + textBoxBorderText.Text);
                }
            }
        }

        private void Audio_sampleOverEvent(AudioQueue sender, object tag)
        {
            buttonExtTest.Text = "Test";
        }

        private void buttonExtOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonExtEffects_Click(object sender, EventArgs e)
        {
            SoundEffectsDialog sfe = new SoundEffectsDialog();
            sfe.Init(this.Icon, effects,true);
            sfe.TestSettingEvent += Sfe_TestSettingEvent;           // callback to say test
            sfe.StopTestSettingEvent += Sfe_StopTestSettingEvent;   // callback to say stop
            if (sfe.ShowDialog(this) == DialogResult.OK)
            {
                effects = sfe.GetEffects();
            }
        }

        private void Sfe_TestSettingEvent(SoundEffectsDialog sfe, ConditionVariables effects)
        {
            try
            {
                AudioQueue.AudioSample a = queue.Generate(textBoxBorderText.Text, new SoundEffectSettings(effects));
                a.sampleOverEvent += SampleOver;
                a.sampleOverTag = sfe;
                queue.Submit(a, trackBarVolume.Value, AudioQueue.Priority.High);
            }
            catch
            {
                ExtendedControls.MessageBoxTheme.Show(this,"Unable to play " + textBoxBorderText.Text);
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

        private void buttonExtBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = "mp3";
            dlg.AddExtension = true;
            dlg.Filter = "MP3 Files (*.mp3)|*.mp3|WAV files (*.wav)|*.wav|Audio Files (*.mp3;*.wav)|*.mp3;*.wav|All files (*.*)|*.*";

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                textBoxBorderText.Text = dlg.FileName;
            }
        }

        private void checkBoxCustomV_CheckedChanged(object sender, EventArgs e)
        {
            trackBarVolume.Enabled = checkBoxCustomV.Checked;
        }

        private void buttonExtDevice_Click(object sender, EventArgs e)
        {
            AudioDeviceConfigure adc = new AudioDeviceConfigure();
            adc.Init("Configure wave device", queue.Driver);
            if (adc.ShowDialog(this) == DialogResult.OK)
            {
                if (!queue.SetAudioEndpoint(adc.Selected))
                    ExtendedControls.MessageBoxTheme.Show(this, "Audio Device Selection failed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
