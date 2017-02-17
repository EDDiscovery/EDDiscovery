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
        public AudioQueue.Priority Priority { get { return (AudioQueue.Priority)Enum.Parse(typeof(AudioQueue.Priority), comboBoxCustomPriority.Text); } }
        public string StartEvent { get { return textBoxBorderStartTrigger.Text; } }
        public string FinishEvent { get { return textBoxBorderEndTrigger.Text; } }
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
                          string title, string caption, EDDiscovery2.EDDTheme th,
                          string defpath,
                          bool waitcomplete,
                          AudioQueue.Priority prio,
                          string startname, string endname,
                          string volume,
                          ConditionVariables ef)
        {
            comboBoxCustomPriority.Items.AddRange(Enum.GetNames(typeof(AudioQueue.Priority)));

            queue = qu;
            theme = th;
            this.Text = caption;
            labelTitle.Text = title;
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
                    queue.Submit(audio, trackBarVolume.Value, AudioQueue.Priority.High);
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
                queue.Submit(a, trackBarVolume.Value, AudioQueue.Priority.High);
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

        private void checkBoxCustomV_CheckedChanged(object sender, EventArgs e)
        {
            trackBarVolume.Enabled = checkBoxCustomV.Checked;
        }
    }
}
