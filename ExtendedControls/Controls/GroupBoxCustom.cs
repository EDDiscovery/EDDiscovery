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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class GroupBoxCustom : GroupBox
    {
        // call Invalidate if changed

        public Color BorderColor { get; set; } = Color.LightGray;       // border
        public float BackColorScaling { get; set; } = 0.5F;             // Popup style only
        public float BorderColorScaling { get; set; } = 0.5F;           // Popup style only
        public int TextStartPosition { get; set; } = -1;                // -1 left, +1 right, 0 centre, else pixel start pos
        public int TextPadding { get; set; } = 0;                       // pixels at start/end of text

        // if not set, whole client is filled with BackColor as per normal group box.
        public bool FillClientAreaWithAlternateColor { get; set; } = false;  // Fill client area with alternate colour..
        public Color AlternateClientBackColor { get; set; } = Color.Blue;        // area of client.. only used if FillOnlyClientArea is true.  

        public GroupBoxCustom() : base()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (FlatStyle == FlatStyle.System || FlatStyle == FlatStyle.Standard)
                base.OnPaint(e);
            else if ( DisplayRectangle.Width > 0 && DisplayRectangle.Height > 0 ) // Popup and Flat are ours, as long as its got size
            {
                int topline = DisplayRectangle.Y / 2;

                Color colorback = (FillClientAreaWithAlternateColor) ? AlternateClientBackColor : BackColor;

                if (!colorback.IsFullyTransparent())
                {
                    Color color2 = (FlatStyle == FlatStyle.Popup) ? colorback.Multiply(BackColorScaling) : BackColor;

                    Rectangle borderrect = ClientRectangle;
                    if (FillClientAreaWithAlternateColor)
                    {
                        borderrect.Y += topline;
                        borderrect.Height -= topline;
                    }

                    using (Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(borderrect, colorback, color2, 90))
                        e.Graphics.FillRectangle(b, borderrect);
                }

                if (!BorderColor.IsFullyTransparent())
                {
                    Color color1 = BorderColor;
                    Color color2 = BorderColor.Multiply(BorderColorScaling);
                    
                    int textlength = 0;
                    if ( this.Text != "" )
                    {           // +1 for rounding down..
                        textlength = (int)e.Graphics.MeasureString(this.Text, this.Font).Width + TextPadding * 2 + 1;
                    }

                    int textstart = TextStartPosition;
                    if (textstart == 0)                                          // auto centre
                        textstart = ClientRectangle.Width / 2 - textlength / 2;     // centre
                    else if (textstart == -1)                                          // left
                        textstart = 15;
                    else if (textstart == 1)                                          // right
                        textstart = ClientRectangle.Width - 15 - textlength;

                    if (textstart < 4)                                          // need 4 pixels 
                        textstart = 4;

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                    using (GraphicsPath g1 = ControlHelpersStaticFunc.RectCutCorners(1, topline+1, ClientRectangle.Width-2, ClientRectangle.Height - topline - 1, 1,1 , textstart- 1, textlength))
                    using (Pen pc1 = new Pen(color1, 1.0F))
                        e.Graphics.DrawPath(pc1, g1);

                    using (GraphicsPath g2 = ControlHelpersStaticFunc.RectCutCorners(0, topline, ClientRectangle.Width, ClientRectangle.Height - topline - 1, 2, 2 , textstart, textlength))
                    using (Pen pc2 = new Pen(color2, 1.0F))
                        e.Graphics.DrawPath(pc2, g2);

                    if (textlength > 0)
                    {
                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                        int twidth = ClientRectangle.Width - textstart - 4;            // What size have we left..
                        twidth = (textlength<twidth) ? textlength: twidth;              // clip
                        Rectangle textarea = new Rectangle(textstart, 0, twidth, DisplayRectangle.Y);

                        using (Brush textb = new SolidBrush(this.ForeColor))
                        using (StringFormat fmt = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                        {
                            e.Graphics.DrawString(this.Text, this.Font, textb, textarea, fmt);
                        }

                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    }
                }
            }
        }

    }
}
