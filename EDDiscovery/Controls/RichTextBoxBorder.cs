using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class RichTextBoxBorder : RichTextBox
    {
        public Color BorderColor { get; set; } = Color.Transparent;
        public int BorderPadding { get; set; } = 1;
        public int BorderSize { get; set; } = 1;

        public RichTextBoxBorder() : base()
        {
            Debug.Assert(false, "Depreciated - use RichTextBoxScroll");
        }

        private const int WM_PAINT = 15;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)          // Stupid control does not have a OnPaint
            {
                if (BorderColor != Color.Transparent)
                {
                    Graphics g = Parent.CreateGraphics();

                    Pen p = new Pen(BorderColor);
                    Pen b = new Pen(BackColor);
                    int offl = (this.BorderStyle != BorderStyle.None) ? 1 : 1;
                    int offr = (this.BorderStyle != BorderStyle.None) ? 5 : 1;

                    Rectangle rect = new Rectangle(Location.X - offl, Location.Y - offl, ClientRectangle.Width + offr, ClientRectangle.Height + offr);

                    for (int i = 0; i < BorderPadding; i++)
                    {
                        g.DrawRectangle(b, rect);
                        rect.Inflate(1, 1);
                    }
                    for (int i = 0; i < BorderSize; i++)
                    {
                        g.DrawRectangle(p, rect);
                        rect.Inflate(1, 1);
                    }

                    p.Dispose();
                    b.Dispose();
                    g.Dispose();
                }
            }
        }
    }
}
