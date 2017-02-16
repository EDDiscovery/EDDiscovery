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
 * EDDiscovery is not affiliated with Fronter Developments plc.
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
    public partial class WaveConfigureDialog : Form
    {
        public string Path {  get { return textBoxBorderText.Text; } }
        public bool Wait { get { return checkBoxCustomComplete.Checked; } }
        public bool Preempt { get { return checkBoxCustomPreempt.Checked; } }
        public string Volume { get { return (checkBoxCustomV.Checked) ? trackBarVolume.Value.ToString() : "Default"; } }
        public ConditionVariables Effects { get { return effects; } }

        ConditionVariables effects;
        AudioQueue queue;
        EDDiscovery2.EDDTheme theme;

        public WaveConfigureDialog()
        {
            InitializeComponent();
        }

        public void Init(AudioQueue qu, 
                          bool defaultmode,
                          string caption, EDDiscovery2.EDDTheme th,
                          string defpath,
                          bool waitcomplete, bool preempt,
                          string volume,
                          ConditionVariables ef)
        {
            queue = qu;
            theme = th;
            this.Text = caption;
            textBoxBorderText.Text = defpath;

            if (defaultmode)
            {
                checkBoxCustomComplete.Visible = checkBoxCustomPreempt.Visible = checkBoxCustomV.Visible = false;
            }
            else
            {
                checkBoxCustomComplete.Checked = waitcomplete;
                checkBoxCustomPreempt.Checked = preempt;
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

            theme.ApplyToForm(this, System.Drawing.SystemFonts.DefaultFont);
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
                    Audio.AudioQueue.AudioSample audio = queue.Generate(textBoxBorderText.Text, effects);
                    audio.sampleOverEvent += Audio_sampleOverEvent;
                    queue.Submit(audio, trackBarVolume.Value);
                    buttonExtTest.Text = "Stop";
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

        private void buttonExtOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonExtEffects_Click(object sender, EventArgs e)
        {
            SoundEffectsDialog sfe = new SoundEffectsDialog();
            sfe.Init(effects,true,theme);
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
                AudioQueue.AudioSample a = queue.Generate(textBoxBorderText.Text, effects);
                a.sampleOverEvent += SampleOver;
                a.sampleOverTag = sfe;
                queue.Submit(a, trackBarVolume.Value);
            }
            catch
            {
                MessageBox.Show("Unable to play " + textBoxBorderText.Text);
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

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBoxBorderText.Text = dlg.FileName;
            }
        }
    }
}
