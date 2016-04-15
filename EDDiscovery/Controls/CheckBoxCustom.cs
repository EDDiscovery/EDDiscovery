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
    public class CheckBoxCustom : CheckBox
    {
        public Color CheckBoxColor { get; set; } = Color.Gray;       // border
        public Color CheckBoxInnerColor { get; set; } = Color.White;
        public Color CheckColor { get; set; } = Color.DarkBlue;
        public Color MouseOverColor { get; set; } = Color.CornflowerBlue;
        public float FontNerfReduction { get; set; } = 0.5F;
        public int TickBoxReductionSize { get; set; } = 10;          // no of pixels smaller than the height to make the tick box

        public CheckBoxCustom() : base()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (FlatStyle == FlatStyle.System || FlatStyle == FlatStyle.Standard)
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

                using (Pen outer = new Pen(basecolor))
                    e.Graphics.DrawRectangle(outer, rect);

                rect.Inflate(-1, -1);

                Rectangle checkarea = rect;
                checkarea.Width++; checkarea.Height++;          // convert back to area

                if (Enabled)
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
                else
                {
                    using (Brush disabled = new SolidBrush(CheckBoxInnerColor))
                    {
                        e.Graphics.FillRectangle(disabled, checkarea);
                    }
                }

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (Brush textb = new SolidBrush((Enabled) ? this.ForeColor : Multiply(this.ForeColor, 0.5F)))
                {
                    StringFormat fmt = new StringFormat();
                    fmt.Alignment = StringAlignment.Near;
                    fmt.LineAlignment = StringAlignment.Center;

                    using (Font ft = new Font(this.Font.Name, this.Font.Size - FontNerfReduction)) // font 0.5 points smaller, seems to work, otherwise it just won't fit
                    {
                        e.Graphics.DrawString(this.Text, ft, textb, textarea, fmt);
                    }
                }

                if (Checked)
                {
                    Point pt1 = new Point(checkarea.X + 2, checkarea.Y + checkarea.Height / 2 - 1);
                    Point pt2 = new Point(checkarea.X + checkarea.Width / 2 - 1, checkarea.Bottom - 2);
                    Point pt3 = new Point(checkarea.X + checkarea.Width - 2, checkarea.Y);

                    Color c1 = Color.FromArgb(200, CheckColor.R, CheckColor.G, CheckColor.B);

                    using (Pen pcheck = new Pen(c1, 2.0F))
                    {
                        e.Graphics.DrawLine(pcheck, pt1, pt2);
                        e.Graphics.DrawLine(pcheck, pt2, pt3);
                    }
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

        private byte limit(float a) { if (a > 255F) return 255; else return (byte)a; }
        public Color Multiply(Color from, float m) { return Color.FromArgb(from.A, limit((float)from.R * m), limit((float)from.G * m), limit((float)from.B * m)); }

        private bool mouseover = false;
    }
}

