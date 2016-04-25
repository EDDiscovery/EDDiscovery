using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
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

        private static byte limit(float a) { if (a > 255F) return 255; else return (byte)a; }
        public static Color Multiply(Color from, float m) { return Color.FromArgb(from.A, limit((float)from.R * m), limit((float)from.G * m), limit((float)from.B * m)); }

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
                    back = Multiply(BackColor, 0.5F);
                    b = new System.Drawing.Drawing2D.LinearGradientBrush(buttonarea, back, Multiply(back, ButtonColorScaling), 90);
                    p = new Pen(Multiply(FlatAppearance.BorderColor, ButtonDisabledScaling));
                }
                else
                {
                    back = (mousedown) ? FlatAppearance.MouseDownBackColor : ((mouseover) ? FlatAppearance.MouseOverBackColor : BackColor);
                    b = new System.Drawing.Drawing2D.LinearGradientBrush(buttonarea, back, Multiply(back, ButtonColorScaling), 90);
                    p = new Pen((mousedown || mouseover) ? Multiply(FlatAppearance.BorderColor, BorderColorScaling) : FlatAppearance.BorderColor);
                }

                pe.Graphics.DrawRectangle(p, border);
                pe.Graphics.FillRectangle(b, buttonarea);

                if (Focused)
                {
                    Pen p1 = new Pen(FlatAppearance.BorderColor);
                    pe.Graphics.DrawRectangle(p1, focusrect);
                    p1.Dispose();
                }

                SizeF sz = pe.Graphics.MeasureString(this.Text, this.Font);

                Brush textb = new SolidBrush((Enabled)?this.ForeColor:Multiply(this.ForeColor,0.5F));

                pe.Graphics.DrawString(this.Text, this.Font, textb,
                    buttonarea.Left + (buttonarea.Width - sz.Width) / 2,
                    buttonarea.Top + (buttonarea.Height - sz.Height) / 2);

                textb.Dispose();
                p.Dispose();
                b.Dispose();
            }
        }
    }
}
