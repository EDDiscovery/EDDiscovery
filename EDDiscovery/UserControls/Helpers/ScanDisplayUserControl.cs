/*
 * Copyright © 2019-2024 EDDiscovery development team
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
        public EliteDangerousCore.StarScan2.SystemDisplay SystemDisplay { get; private set; }

        public int WidthAvailable { get { return this.Width - vScrollBarCustom.Width; } }   // available display width

        #region Init
        public ScanDisplayUserControl()
        {
            InitializeComponent();
            SystemDisplay = new EliteDangerousCore.StarScan2.SystemDisplay();
            this.AutoScaleMode = AutoScaleMode.None;            // we are dealing with graphics.. lets turn off dialog scaling.
            rtbNodeInfo.Visible = false;
            toolTip.ShowAlways = true;
            imagebox.ClickElement += ClickElement;
            SystemDisplay.Font = ExtendedControls.Theme.Current?.GetDialogFont ?? Font;
            SystemDisplay.FontUnderlined = ExtendedControls.Theme.Current?.GetScaledFont(1f, underline:true) ?? Font;
            SystemDisplay.FontLarge = ExtendedControls.Theme.Current?.GetFont ?? Font;
        // tbd    SystemDisplay.ContextMenuStripStars = contextMenuStripBodies;
        }

        #endregion

        #region Display

        // draw scannode (may be null), 
        // curmats may be null
        public void DrawSystem(EliteDangerousCore.StarScan2.SystemNode systemnode, List<MaterialCommodityMicroResource> historicmats, 
                                    List<MaterialCommodityMicroResource> curmats,string opttext = null, string[] filter=  null ) 
        {
            HideInfo();
            SystemDisplay.TextBackColor = this.BackColor;
            SystemDisplay.TextForeColor = ExtendedControls.Theme.Current.LabelColor;
            SystemDisplay.DrawSystemRender(imagebox, WidthAvailable, systemnode, historicmats, curmats, opttext, filter);
            imagebox.Render();      // replaces image..
        }

        #endregion

        #region User interaction

        private void ClickElement(object sender, MouseEventArgs e, ExtendedControls.ImageElement.Element i, object tag)
        {
            if (i != null && tag is string)
                ShowInfo((string)tag, i.Bounds.Location.X < panelStars.Width / 2);
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

        #endregion

        #region Implementation

        public void HideInfo()
        {
            rtbNodeInfo.Visible = false;
        }
        private void UserControlScan_Resize(object sender, EventArgs e)
        {
           // System.Diagnostics.Debug.WriteLine($"System Display size {Size}");
            PositionInfo();
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

        // static as it is used externally to this
        static public int HighValueForm(Form form, int v)
        {
            ConfigurableForm cf = new ConfigurableForm();
            int width = 300;
            int height = 100;

            cf.Add(new ExtendedControls.ConfigurableEntryList.Entry("UC", typeof(ExtendedControls.NumberBoxInt), v.ToStringInvariant(),
                                        new Point(5, 30), new Size(width - 5 - 20, 24), null)
            { NumberBoxLongMinimum = 1, NumberBoxLongMaximum = 1000000000 });

            cf.Add(new ExtendedControls.ConfigurableEntryList.Entry("OK", typeof(ExtendedControls.ExtButton), "OK".Tx(),
                        new Point(width - 20 - 80, height - 40), new Size(80, 24), ""));

            cf.Trigger += (dialogname, controlname, tag) =>
            {
                System.Diagnostics.Debug.WriteLine("control" + controlname);

                if (controlname.Contains("Validity:False"))
                    cf.GetControl("OK").Enabled = false;
                else if (controlname.Contains("Validity:True"))
                    cf.GetControl("OK").Enabled = true;
                else if (controlname == "OK")
                {
                    cf.ReturnResult(DialogResult.OK);
                }
                else if (controlname == "Cancel")
                {
                    cf.ReturnResult(DialogResult.Cancel);
                }
            };

            if (cf.ShowDialogCentred(form, form.Icon, "Set Valuable Minimum".Tx()) == DialogResult.OK)
            {
                return cf.GetInt("UC").Value;
            }
            else
                return int.MinValue;
        }

    }
}

