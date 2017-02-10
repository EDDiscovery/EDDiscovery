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
        public bool Wait { get { return checkBoxCustomComplete.Checked; } }
        public bool Preempt { get { return checkBoxCustomPreempt.Checked; } }
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
                          string caption, EDDiscovery2.EDDTheme theme,
                          string defpath,
                          bool waitcomplete, bool preempt,
                          string volume,
                          ConditionVariables ef)
        {
            queue = qu;
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
                queue.StopAll();
                buttonExtTest.Text = "Test";
            }
            else
            {
                try
                {
                    queue.Submit(queue.Generate(textBoxBorderText.Text, effects), trackBarVolume.Value);
                    buttonExtTest.Text = "Stop";
                }
                catch
                {
                    MessageBox.Show("Unable to play " + textBoxBorderText.Text);
                }
            }
        }

        private void buttonExtOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonExtEffects_Click(object sender, EventArgs e)
        {
            SoundEffectsDialog sfe = new SoundEffectsDialog();
            sfe.Init(effects);
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

        }
    }
}
