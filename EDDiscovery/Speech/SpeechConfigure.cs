using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Speech
{
    public partial class SpeechConfigure : Form
    {
        public bool Wait { get { return checkBoxCustomComplete.Checked; } }
        public string SayText { get { return textBoxBorderText.Text; } }
        public string VoiceName { get { return comboBoxCustomVoice.Text; } }
        public string Volume { get { return textBoxBorderVolume.Text; } }
        public string Rate { get { return textBoxBorderRate.Text; } }

        public SpeechConfigure()
        {
            InitializeComponent();
        }

        public void Init(string title, string caption , EDDiscovery2.EDDTheme theme,
                                  String text,             // if null, no text box or wait complete
                                  bool waitcomplete,
                                  string [] voicenames,
                                  string voicename,
                                  string volume,
                                  string rate)
        {
            this.Text = caption;
            Title.Text = title;

            if (text == null)
            {
                textBoxBorderText.Visible = checkBoxCustomComplete.Visible = false;

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
            }

            comboBoxCustomVoice.Items.Add("Default");
            comboBoxCustomVoice.Items.Add("Female");
            comboBoxCustomVoice.Items.Add("Male");
            comboBoxCustomVoice.Items.AddRange(voicenames);
            comboBoxCustomVoice.SelectedItem = voicename;
            textBoxBorderVolume.Text = volume;
            textBoxBorderRate.Text = rate;

            theme.ApplyToForm(this, System.Drawing.SystemFonts.DefaultFont);
        }

        private void buttonExtOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
