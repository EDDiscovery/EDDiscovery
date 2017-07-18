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
using AudioExtensions;
using Conditions;

namespace AudioExtensions
{
    public partial class SoundEffectsDialog : Form
    {
        public delegate void TestSettings(SoundEffectsDialog sender, ConditionVariables effect);
        public event TestSettings TestSettingEvent;
        public delegate void StopTestSettings(SoundEffectsDialog sender);
        public event StopTestSettings StopTestSettingEvent;

        public SoundEffectsDialog()
        {
            InitializeComponent();
            comboBoxCustomVoices.Items.AddRange(defaulteffects);
        }

        public void Init(Icon ic, ConditionVariables cv, bool shownone)
        {
            this.Icon = ic;

            if (!shownone)
                checkBoxCustomNone.Visible = false;

            Set(cv);
        }

        void Set(ConditionVariables cv)
        { 
            SoundEffectSettings ap = new SoundEffectSettings(cv);

            // we have no control over values, user could have messed them up, and it excepts if out of range

            trackBarEM.Enabled = trackBarEF.Enabled = trackBarED.Enabled = checkBoxE.Checked = ap.echoenabled;
            try
            {
                trackBarEM.Value = ap.echomix;
                trackBarEF.Value = ap.echofeedback;
                trackBarED.Value = ap.echodelay;
            } catch { }

            trackBarCM.Enabled = trackBarCF.Enabled = trackBarCD.Enabled = trackBarCDp.Enabled = checkBoxC.Checked = ap.chorusenabled;
            try
            {
                trackBarCM.Value = ap.chorusmix;
                trackBarCF.Value = ap.chorusfeedback;
                trackBarCD.Value = ap.chorusdelay;
                trackBarCDp.Value = ap.chorusdepth;
            } catch { }

            trackBarRM.Enabled = trackBarRT.Enabled = trackBarRH.Enabled = checkBoxR.Checked = ap.reverbenabled;
            try
            {
                trackBarRM.Value = ap.reverbmix;
                trackBarRT.Value = ap.reverbtime;
                trackBarRH.Value = ap.reverbhfratio;
            } catch { }

            trackBarDG.Enabled = trackBarDE.Enabled = trackBarDC.Enabled = trackBarDW.Enabled = checkBoxD.Checked = ap.distortionenabled;
            try
            {
                trackBarDG.Value = ap.distortiongain;
                trackBarDE.Value = ap.distortionedge;
                trackBarDC.Value = ap.distortioncentrefreq;
                trackBarDW.Value = ap.distortionfreqwidth;
            } catch { }

            trackBarGF.Enabled = checkBoxG.Checked = ap.gargleenabled;
            try
            {
                trackBarGF.Value = ap.garglefreq;
            }
            catch { }

            trackBarPitch.Enabled = checkBoxP.Checked = ap.pitchshiftenabled;
            try
            {
                trackBarPitch.Value = ap.pitchshift;
            }
            catch { }

            checkBoxCustomNone.Checked = ap.OverrideNone;

            ExtendedControls.ThemeableFormsInstance.Instance.ApplyToForm(this, System.Drawing.SystemFonts.DefaultFont);
        }

        public ConditionVariables GetEffects()
        {
            SoundEffectSettings ap = new SoundEffectSettings();

            if (checkBoxE.Checked)
            {
                ap.echomix = trackBarEM.Value;
                ap.echofeedback = trackBarEF.Value;
                ap.echodelay = trackBarED.Value;
            }

            if (checkBoxC.Checked)
            {
                ap.chorusmix = trackBarCM.Value;
                ap.chorusfeedback = trackBarCF.Value;
                ap.chorusdelay = trackBarCD.Value;
                ap.chorusdepth = trackBarCDp.Value;
            }

            if (checkBoxR.Checked)
            {
                ap.reverbmix = trackBarRM.Value;
                ap.reverbtime = trackBarRT.Value;
                ap.reverbhfratio = trackBarRH.Value;
            }

            if (checkBoxD.Checked)
            {
                ap.distortiongain = trackBarDG.Value;
                ap.distortionedge = trackBarDE.Value;
                ap.distortioncentrefreq = trackBarDC.Value;
                ap.distortionfreqwidth = trackBarDW.Value;
            }

            if (checkBoxG.Checked)
            {
                ap.garglefreq = trackBarGF.Value;
            }

            if (checkBoxP.Checked)
            {
                ap.pitchshift = trackBarPitch.Value;
            }

            if (checkBoxCustomNone.Checked)
                ap.OverrideNone = true;

            return ap.values;
        }

        private void buttonExtOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void checkBoxE_CheckedChanged(object sender, EventArgs e)
        {
            trackBarEM.Enabled = checkBoxE.Checked;
            trackBarEF.Enabled = checkBoxE.Checked;
            trackBarED.Enabled = checkBoxE.Checked;
            TurnOffNone();
        }

        private void checkBoxC_CheckedChanged(object sender, EventArgs e)
        {
            trackBarCM.Enabled = checkBoxC.Checked;
            trackBarCF.Enabled = checkBoxC.Checked;
            trackBarCD.Enabled = checkBoxC.Checked;
            trackBarCDp.Enabled = checkBoxC.Checked;
            TurnOffNone();
        }

        private void checkBoxR_CheckedChanged(object sender, EventArgs e)
        {
            trackBarRM.Enabled = checkBoxR.Checked;
            trackBarRT.Enabled = checkBoxR.Checked;
            trackBarRH.Enabled = checkBoxR.Checked;
            TurnOffNone();
        }

        private void checkBoxD_CheckedChanged(object sender, EventArgs e)
        {
            trackBarDG.Enabled = checkBoxD.Checked;
            trackBarDE.Enabled = checkBoxD.Checked;
            trackBarDC.Enabled = checkBoxD.Checked;
            trackBarDW.Enabled = checkBoxD.Checked;
            TurnOffNone();
        }

        private void checkBoxG_CheckedChanged(object sender, EventArgs e)
        {
            trackBarGF.Enabled = checkBoxG.Checked;
            TurnOffNone();
        }

        private void checkBoxP_CheckedChanged(object sender, EventArgs e)
        {
            trackBarPitch.Enabled = checkBoxP.Checked;
            TurnOffNone();
        }

        void TurnOffNone()
        {
            if (checkBoxCustomNone.Visible)
            {
                checkBoxCustomNone.Enabled = false;
                checkBoxCustomNone.Checked = false;
                checkBoxCustomNone.Enabled = true;
            }
        }

        private void checkBoxCustomNone_CheckedChanged(object sender, EventArgs e)
        {
            if ( checkBoxCustomNone.Enabled )
                checkBoxE.Checked = checkBoxC.Checked = checkBoxR.Checked = checkBoxD.Checked = checkBoxG.Checked = false;
        }

        private void buttonExtTest_Click(object sender, EventArgs e)
        {
            if (buttonExtTest.Text.Equals("Stop"))
            {
                if (StopTestSettingEvent != null)
                    StopTestSettingEvent(this);
            }
            else
            {
                ConditionVariables c = GetEffects();
                toolTip1.SetToolTip(buttonExtTest, c.ToString(separ: Environment.NewLine));
                if (TestSettingEvent != null)
                {
                    TestSettingEvent(this, GetEffects() );
                    buttonExtTest.Text = "Stop";
                }
            }
        }

        public void TestOver()
        {
            buttonExtTest.Text = "Test";
        }

        static string[] defaulteffects = new string[]
            {   "Metalic",
                    "Computer",
                    "Computer Echo"
            };

        static string[] defaulteffectsconfig = new string[]
            {   "EchoMix=94,EchoFeedback=50,EchoDelay=13",
                    "ChorusMix=85,ChorusFeedback=61,ChorusDelay=16,ChorusDepth=63,ReverbMix=0,ReverbTime=2603,ReverbRatio=617",
                    "EchoMix=50,EchoFeedback=50,EchoDelay=50,ChorusMix=85,ChorusFeedback=61,ChorusDelay=16,ChorusDepth=63,ReverbMix=0,ReverbTime=2603,ReverbRatio=617,GargleFreq=119"
            };

        private void comboBoxCustomDefaults_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConditionVariables vs = new ConditionVariables(defaulteffectsconfig[comboBoxCustomVoices.SelectedIndex],ConditionVariables.FromMode.MultiEntryComma);
            Set(vs);
        }

    }

}


