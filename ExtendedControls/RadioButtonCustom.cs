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
    public class RadioButtonCustom : RadioButton
    {
        public Color RadioButtonColor { get; set; } = Color.Gray;       // border of
        public Color RadioButtonInnerColor { get; set; } = Color.White; // inner border of

        public Color SelectedColor { get; set; } = Color.DarkBlue;      // the bullit eye color
        public Color SelectedColorRing { get; set; } = Color.Black;     // ring around it, Transparent for off

        public Color MouseOverColor { get; set; } = Color.CornflowerBlue;   // mouse over colour

        public float FontNerfReduction { get; set; } = 0.5F;              // When windows paints control, seems to use a slightly smaller font than what has been ordered

        public RadioButtonCustom() : base()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Console.WriteLine("RB " + Name + ":" + ClientRectangle.ToString());

            if (FlatStyle == FlatStyle.System || FlatStyle == FlatStyle.Standard)
                base.OnPaint(e);
            else
            {
                Rectangle rect = ClientRectangle;

                using (Brush br = new SolidBrush(this.BackColor))
                    e.Graphics.FillRectangle(br, rect);

                rect.Height -= 6;
                rect.Y += 2;
                rect.Width = rect.Height;

                Rectangle textarea = ClientRectangle;
                textarea.X = rect.Width;
                textarea.Width -= rect.Width;

                Color basecolor = (mouseover) ? MouseOverColor : RadioButtonColor;

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (Brush outer = new SolidBrush(basecolor))
                    e.Graphics.FillEllipse(outer, rect);

                rect.Inflate(-1, -1);

                if (Enabled)
                {
                    using (Brush second = new SolidBrush(RadioButtonInnerColor))
                        e.Graphics.FillEllipse(second, rect);

                    rect.Inflate(-1, -1);

                    if (FlatStyle == FlatStyle.Flat)
                    {
                        using (Brush inner = new SolidBrush(basecolor))
                            e.Graphics.FillEllipse(inner, rect);      // fill slightly over size to make sure all pixels are painted
                    }
                    else
                    {
                        using (Brush inner = new LinearGradientBrush(rect, RadioButtonInnerColor, basecolor, 225))
                            e.Graphics.FillEllipse(inner, rect);      // fill slightly over size to make sure all pixels are painted
                    }
                }
                else
                {
                    using (Brush disabled = new SolidBrush(RadioButtonInnerColor))
                    {
                        e.Graphics.FillEllipse(disabled, rect);
                    }
                }

                rect.Inflate(-1, -1);

                if (Checked)
                {
                    Color c1 = Color.FromArgb(255, SelectedColor);

                    if (FlatStyle == FlatStyle.Flat)
                    {
                        using (Brush inner = new SolidBrush(c1))
                            e.Graphics.FillEllipse(inner, rect);      // fill slightly over size to make sure all pixels are painted
                    }
                    else
                    {
                        using (Brush inner = new LinearGradientBrush(rect, RadioButtonInnerColor, c1, 45))
                            e.Graphics.FillEllipse(inner, rect);      // fill slightly over size to make sure all pixels are painted
                    }

                    using (Pen ring = new Pen(SelectedColorRing))
                    {
                        e.Graphics.DrawEllipse(ring, rect);

                    }
                }

                using (Brush textb = new SolidBrush((Enabled) ? this.ForeColor : this.ForeColor.Multiply(0.5F)))
                using (StringFormat fmt = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
                using (Font ft = new Font(this.Font.Name, this.Font.Size - FontNerfReduction)) // font 0.5 points smaller, seems to work, otherwise it just won't fit
                {
                    e.Graphics.DrawString(this.Text, ft, textb, textarea, fmt);
                }

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            }
        }

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            mouseover = true;
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            mouseover = false;
        }

        private bool mouseover = false;

    }
}
