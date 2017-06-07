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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.ExtendedControls
{
    public class ButtonExt : Button
    {
        // PROPERTIES Fore,Back,FlatAppearance,FlatStyle inherited

        public float ButtonColorScaling { get; set; } = 0.5F;
        public float BorderColorScaling { get; set; } = 1.25F;
        public float ButtonDisabledScaling { get; set; } = 0.5F;

        // Internal

        public ButtonExt()
        {
        }

        private bool mouseover = false;
        private bool mousedown = false;

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mouseover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouseover = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            mousedown = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            mousedown = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (FlatStyle != FlatStyle.Popup)      // override popup style for us
            {
                base.OnPaint(pe);
            }
            else
            {
                Rectangle border = ClientRectangle;
                border.Width--; border.Height--;

                Rectangle focusrect = border;           // 1 inside it..
                focusrect.Inflate(-1, -1);

                Rectangle buttonarea = ClientRectangle;
                buttonarea.Inflate(-1, -1);                     // inside it.

                //Console.WriteLine("Paint " + this.Name + " " + ClientRectangle.ToString());

                Color back;
                Brush b;
                Pen p;

                if (!Enabled)
                {
                    back = BackColor.Multiply(0.5F);
                    b = new System.Drawing.Drawing2D.LinearGradientBrush(buttonarea, back, back.Multiply(ButtonColorScaling), 90);
                    p = new Pen(FlatAppearance.BorderColor.Multiply(ButtonDisabledScaling));
                }
                else
                {
                    back = (mousedown) ? FlatAppearance.MouseDownBackColor : ((mouseover) ? FlatAppearance.MouseOverBackColor : BackColor);
                    b = new System.Drawing.Drawing2D.LinearGradientBrush(buttonarea, back, back.Multiply(ButtonColorScaling), 90);
                    p = new Pen((mousedown || mouseover) ? FlatAppearance.BorderColor.Multiply(BorderColorScaling) : FlatAppearance.BorderColor);
                }

                pe.Graphics.DrawRectangle(p, border);
                pe.Graphics.FillRectangle(b, buttonarea);

                if (Focused)
                {
                    Pen p1 = new Pen(FlatAppearance.BorderColor);
                    pe.Graphics.DrawRectangle(p1, focusrect);
                    p1.Dispose();
                }

                System.Diagnostics.Debug.Assert(BackgroundImage == null);      // because i've used this and its incorrect

                if (Image != null)
                {
                    pe.Graphics.DrawImage(Image, new Rectangle(ClientRectangle.Width / 2 - Image.Width / 2, ClientRectangle.Height / 2 - Image.Height / 2, Image.Width, Image.Height),
                                    0,0, Image.Width, Image.Height, GraphicsUnit.Pixel);
                }
                else
                {
                    SizeF sz = pe.Graphics.MeasureString(this.Text, this.Font);

                    Brush textb = new SolidBrush((Enabled) ? this.ForeColor : this.ForeColor.Multiply(0.5F));

                    pe.Graphics.DrawString(this.Text, this.Font, textb,
                        buttonarea.Left + (buttonarea.Width - sz.Width) / 2,
                        buttonarea.Top + (buttonarea.Height - sz.Height) / 2);

                    textb.Dispose();
                }

                p.Dispose();
                b.Dispose();
            }
        }
    }
}
