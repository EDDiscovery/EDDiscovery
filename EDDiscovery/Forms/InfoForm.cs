/*
 * Copyright © 2016 EDDiscovery development team
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
using System.Windows.Forms;

namespace EDDiscovery2
{
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
        }

        public void Info(string title, string info , Font fnt, int[] array )
        {
            Text = title;
            textBoxInfo.SelectionTabs = array;
            textBoxInfo.Text = info;
            textBoxInfo.Select(0, 0);
            textBoxInfo.Font = fnt;
        }

        private void InfoForm_Resize(object sender, EventArgs e)
        {
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void InfoForm_Layout(object sender, LayoutEventArgs e)
        {
            textBoxInfo.Location = new Point(2, 2);
            textBoxInfo.Size = new Size(ClientRectangle.Width - 4, ClientRectangle.Height - buttonOK.Size.Height*2);
            buttonOK.Location = new Point(ClientRectangle.Width - 100, ClientRectangle.Height - buttonOK.Size.Height*3/2);

        }
    }
}
