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
    public partial class SoundEffectsDialog : Form
    {
        public delegate void TestSettings(SoundEffectsDialog sender, ConditionVariables effect);
        public event TestSettings TestSettingEvent;
        public delegate void StopTestSettings(SoundEffectsDialog sender);
        public event StopTestSettings StopTestSettingEvent;

        public SoundEffectsDialog()
        {
            InitializeComponent();
            comboBoxCustomDefaults.Items.AddRange(defaulteffects);
        }

        public void Init(ConditionVariables cv)
        {
            SoundEffectSettings ap = new SoundEffectSettings(cv);

            trackBarEM.Enabled = trackBarEF.Enabled = trackBarED.Enabled = checkBoxE.Checked = ap.echoenabled;
            trackBarEM.Value = ap.echomix;
            trackBarEF.Value = ap.echofeedback;
            trackBarED.Value = ap.echodelay;

            trackBarCM.Enabled = trackBarCF.Enabled = trackBarCD.Enabled = trackBarCDp.Enabled = checkBoxC.Checked = ap.chorusenabled;
            trackBarCM.Value = ap.chorusmix;
            trackBarCF.Value = ap.chorusfeedback;
            trackBarCD.Value = ap.chorusdelay;
            trackBarCDp.Value = ap.chorusdepth;

            trackBarRM.Enabled = trackBarRT.Enabled = trackBarRH.Enabled = checkBoxR.Checked = ap.reverbenabled;
            trackBarRM.Value = ap.reverbmix;
            trackBarRT.Value = ap.reverbtime;
            trackBarRH.Value = ap.reverbhfratio;

            trackBarDG.Enabled = trackBarDE.Enabled = trackBarDC.Enabled = trackBarDW.Enabled = checkBoxD.Checked = ap.distortionenabled;
            trackBarDG.Value = ap.distortiongain;
            trackBarDE.Value = ap.distortionedge;
            trackBarDC.Value = ap.distortioncentrefreq;
            trackBarDW.Value = ap.distortionfreqwidth;

            trackBarGF.Enabled = checkBoxG.Checked = ap.gargleenabled;
            trackBarGF.Value = ap.garglefreq;

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
        }

        private void checkBoxC_CheckedChanged(object sender, EventArgs e)
        {
            trackBarCM.Enabled = checkBoxC.Checked;
            trackBarCF.Enabled = checkBoxC.Checked;
            trackBarCD.Enabled = checkBoxC.Checked;
            trackBarCDp.Enabled = checkBoxC.Checked;
        }

        private void checkBoxR_CheckedChanged(object sender, EventArgs e)
        {
            trackBarRM.Enabled = checkBoxR.Checked;
            trackBarRT.Enabled = checkBoxR.Checked;
            trackBarRH.Enabled = checkBoxR.Checked;
        }

        private void checkBoxD_CheckedChanged(object sender, EventArgs e)
        {
            trackBarDG.Enabled = checkBoxD.Checked;
            trackBarDE.Enabled = checkBoxD.Checked;
            trackBarDC.Enabled = checkBoxD.Checked;
            trackBarDW.Enabled = checkBoxD.Checked;
        }

        private void checkBoxG_CheckedChanged(object sender, EventArgs e)
        {
            trackBarGF.Enabled = checkBoxG.Checked;
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
            ConditionVariables vs = new ConditionVariables(defaulteffectsconfig[comboBoxCustomDefaults.SelectedIndex],ConditionVariables.FromMode.MultiEntryComma);
            Init(vs);
        }
    }

}


