/*
 * Copyright © 2019 EDDiscovery development team
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

using EliteDangerousCore;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class ScanDisplayUserControl : UserControl
    {
        public SystemDisplay SystemDisplay { get; set; } = new SystemDisplay();

        public int WidthAvailable { get { return this.Width - vScrollBarCustom.Width; } }   // available display width

        #region Init
        public ScanDisplayUserControl()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;            // we are dealing with graphics.. lets turn off dialog scaling.
            rtbNodeInfo.Visible = false;
            toolTip.ShowAlways = true;
            imagebox.ClickElement += ClickElement;
            SystemDisplay.Font = EDDTheme.Instance.GetDialogFont;
            SystemDisplay.FontUnderlined = EDDTheme.Instance.GetDialogScaledFont(1f, FontStyle.Underline);
            SystemDisplay.LargerFont = EDDTheme.Instance.GetFont;
        }

        private void UserControlScan_Resize(object sender, EventArgs e)
        {
            PositionInfo();
        }

        #endregion

        #region Display

        // draw scannode (may be null), 
        // curmats may be null
        public void DrawSystem(StarScan.SystemNode systemnode, List<MaterialCommodityMicroResource> historicmats, 
                                    List<MaterialCommodityMicroResource> curmats,string opttext = null, string[] filter=  null ) 
        {
            HideInfo();
            SystemDisplay.BackColor = this.BackColor;
            SystemDisplay.LabelColor = EDDTheme.Instance.LabelColor;
            SystemDisplay.DrawSystem(imagebox, WidthAvailable, systemnode, historicmats, curmats, opttext, filter);
            imagebox.Render();      // replaces image..
        }

        #endregion

        #region User interaction

        private void ClickElement(object sender, MouseEventArgs e, ExtPictureBox.ImageElement i, object tag)
        {
            if (i != null && tag is string)
                ShowInfo((string)tag, i.Location.Location.X < panelStars.Width / 2);
            else
                HideInfo();
        }

        void ShowInfo(string text, bool onright)
        {
            rtbNodeInfo.Text = text;
            rtbNodeInfo.Tag = onright;
            rtbNodeInfo.Visible = true;
            rtbNodeInfo.Show();
            PositionInfo();
        }

        private void panelStars_MouseDown(object sender, MouseEventArgs e)
        {
            HideInfo();
        }

        public void HideInfo()
        {
            rtbNodeInfo.Visible = false;
        }

        void PositionInfo()
        {
            if (rtbNodeInfo.Visible)
            {
                int y = -panelStars.ScrollOffset;           // invert to get pixels down scrolled
                int width = panelStars.Width * 7 / 16;

                if (rtbNodeInfo.Tag != null && ((bool)rtbNodeInfo.Tag) == true)
                    rtbNodeInfo.Location = new Point(panelStars.Width - panelStars.ScrollBar.Width - 10 - width, y);
                else
                    rtbNodeInfo.Location = new Point(10, y);

                int h = Math.Min(rtbNodeInfo.EstimateVerticalSizeFromText(), panelStars.Height - 20);

                rtbNodeInfo.Size = new Size(width, h);
                rtbNodeInfo.PerformLayout();    // not sure why i need this..
            }
        }

        public void SetBackground(Color c)
        {
            panelStars.BackColor = imagebox.BackColor = vScrollBarCustom.SliderColor = vScrollBarCustom.BackColor = c;
        }


        #endregion
    }
}

