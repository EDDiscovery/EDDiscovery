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
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtendedControls
{
    public partial class InfoForm : DraggableForm
    {
        public InfoForm()
        {
            InitializeComponent();
        }

        public void Info(string title, Icon ic, string info , Font fnt, int[] array , bool themeit = false)    
        {
            Icon = ic;
            Text = title;
            textBoxInfo.SetTabs(array);
            textBoxInfo.ReadOnly = true;
            textBoxInfo.Text = info;
            textBoxInfo.Select(0, 0);

            labelCaption.Text = title;

            ITheme theme = ThemeableFormsInstance.Instance;

            if (themeit && theme != null)
            {
                if (fnt == null)
                    fnt = new Font(theme.FontName, 12.0F);
                bool winborder = theme.ApplyToForm(this, fnt);
                if (winborder)
                    panelTop.Visible = false;
            }
            else
            {
                panelTop.Visible = false;
                if (fnt == null)
                    fnt = new Font(Font.SystemFontName, 12.0F);
            }

            textBoxInfo.Font = fnt;
         }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel_close_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void panel_minimize_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void labelCaption_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }
    }
}
