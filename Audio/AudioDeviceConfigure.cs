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
            bool border = ExtendedControls.ThemeableFormsInstance.Instance.ApplyToForm(this, System.Drawing.SystemFonts.DefaultFont);

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
