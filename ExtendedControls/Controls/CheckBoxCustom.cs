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
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class CheckBoxCustom : CheckBox
    {
        public Color CheckBoxColor { get; set; } = Color.Gray;       // border
        public Color CheckBoxInnerColor { get; set; } = Color.White;
        public Color CheckColor { get; set; } = Color.DarkBlue;
        public Color MouseOverColor { get; set; } = Color.CornflowerBlue;
        public float FontNerfReduction { get; set; } = 0.5F;
        public int TickBoxReductionSize { get; set; } = 10;          // no of pixels smaller than the height to make the tick box

        public Image ImageUnchecked = null;                         // set both this and Image to draw a image instead of the check. Must set FlatSytle popup and Appearance=normal
        public float ImageButtonDisabledScaling { get; set; } = 0.5F;   // scaling when disabled

        public ImageAttributes DrawnImageAttributesEnabled = null;         // Image override (colour etc) for images using Image
        public ImageAttributes DrawnImageAttributesDisabled = null;         // Image override (colour etc) for images using Image

        public void SetDrawnBitmapRemapTable(ColorMap[] remap, float[][] colormatrix = null)
        {
            ControlHelpersStaticFunc.ComputeDrawnPanel(out DrawnImageAttributesEnabled, out DrawnImageAttributesDisabled, ImageButtonDisabledScaling, remap, colormatrix);
        }

        public CheckBoxCustom() : base()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Appearance == Appearance.Button || FlatStyle == FlatStyle.System || FlatStyle == FlatStyle.Standard || FlatStyle == FlatStyle.Flat)
                base.OnPaint(e);
            else
            {
                Rectangle rect = ClientRectangle;

                using (Brush br = new SolidBrush(this.BackColor))
                    e.Graphics.FillRectangle(br, rect);

                rect.Height -= TickBoxReductionSize;
                rect.Y += TickBoxReductionSize/2;
                rect.Width = rect.Height;

                Rectangle textarea = ClientRectangle;
                textarea.X = rect.Width;
                textarea.Width -= rect.Width;
                Color basecolor = (mouseover) ? MouseOverColor : CheckBoxColor;

                if (Image == null)      // don't drawn when image defined
                {
                    using (Pen outer = new Pen(basecolor))
                        e.Graphics.DrawRectangle(outer, rect);
                }

                rect.Inflate(-1, -1);

                Rectangle checkarea = rect;
                checkarea.Width++; checkarea.Height++;          // convert back to area

                if (Enabled)
                {
                    if (Image == null)
                    {
                        using (Pen second = new Pen(CheckBoxInnerColor, 1F))
                            e.Graphics.DrawRectangle(second, rect);

                        rect.Inflate(-1, -1);

                        if (FlatStyle == FlatStyle.Flat)
                        {
                            using (Brush inner = new SolidBrush(basecolor))
                                e.Graphics.FillRectangle(inner, rect);      // fill slightly over size to make sure all pixels are painted
                        }
                        else
                        {
                            using (Brush inner = new LinearGradientBrush(rect, CheckBoxInnerColor, basecolor, 225))
                                e.Graphics.FillRectangle(inner, rect);      // fill slightly over size to make sure all pixels are painted
                        }

                        using (Pen third = new Pen(basecolor, 1F))
                            e.Graphics.DrawRectangle(third, rect);
                    }
                }
                else
                {
                    using (Brush disabled = new SolidBrush(CheckBoxInnerColor))
                    {
                        e.Graphics.FillRectangle(disabled, checkarea);
                    }
                }

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (StringFormat fmt = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
                using (Brush textb = new SolidBrush((Enabled) ? this.ForeColor : this.ForeColor.Multiply(0.5F)))
                using (Font ft = new Font(this.Font.Name, this.Font.Size - FontNerfReduction)) // font 0.5 points smaller, seems to work, otherwise it just won't fit
                {
                    e.Graphics.DrawString(this.Text, ft, textb, textarea, fmt);
                }

                if (Image != null && ImageUnchecked != null)
                {
                    Image image = Checked ? Image : ImageUnchecked;

                    if (DrawnImageAttributesEnabled != null)
                        e.Graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, (Enabled) ? DrawnImageAttributesEnabled : DrawnImageAttributesDisabled);
                    else
                        e.Graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
                }
                else
                { 
                    if (Checked)
                    { 
                        Point pt1 = new Point(checkarea.X + 2, checkarea.Y + checkarea.Height / 2 - 1);
                        Point pt2 = new Point(checkarea.X + checkarea.Width / 2 - 1, checkarea.Bottom - 2);
                        Point pt3 = new Point(checkarea.X + checkarea.Width - 2, checkarea.Y);

                        Color c1 = Color.FromArgb(200, CheckColor);

                        using (Pen pcheck = new Pen(c1, 2.0F))
                        {
                            e.Graphics.DrawLine(pcheck, pt1, pt2);
                            e.Graphics.DrawLine(pcheck, pt2, pt3);
                        }
                    }
                }

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            }
        }

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            mouseover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            mouseover = false;
            Invalidate();
        }

        private bool mouseover = false;
    }
}

