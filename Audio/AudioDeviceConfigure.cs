using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioExtensions
{
    public partial class AudioDeviceConfigure : Form
    {
        public string Selected { get { return comboBoxCustomDevice.SelectedIndex >= 0 ? (string)comboBoxCustomDevice.SelectedItem : null; } }

        public AudioDeviceConfigure()
        {
            InitializeComponent();
        }

        public void Init( string title, AudioExtensions.IAudioDriver dr )
        {
            comboBoxCustomDevice.Items.AddRange(dr.GetAudioEndpoints().ToArray());
            comboBoxCustomDevice.SelectedItem = dr.GetAudioEndpoint();
            bool border = BaseUtils.ThemeAbleFormsInstance.Instance.ApplyToForm(this, System.Drawing.SystemFonts.DefaultFont);

            this.Text = title;
            if (!border)
                label1.Text = title;
        }

        private void buttonExtOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
