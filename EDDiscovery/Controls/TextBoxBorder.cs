using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ExtendedControls
{
    public class TextBoxBorder : TextBox
    {
        public Color BorderColor { get; set; } = Color.Transparent;
        public float BorderColorScaling { get; set; } = 0.5F;           // Popup style only

        public TextBoxBorder() : base()
        {
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

                    Rectangle clientborder = new Rectangle(Location.X - 3, Location.Y - 3, ClientRectangle.Width + 6, ClientRectangle.Height + 6);

                    Color color1 = BorderColor;
                    Color color2 = Multiply(BorderColor, BorderColorScaling);
                    
                    GraphicsPath g1 = RectCutCorners(clientborder.X + 1, clientborder.Y+1, clientborder.Width - 2, clientborder.Height - 1, 1, 1);
                    using (Pen pc1 = new Pen(color1, 1.0F))
                        g.DrawPath(pc1, g1);

                    GraphicsPath g2 = RectCutCorners(clientborder.X, clientborder.Y, clientborder.Width, clientborder.Height - 1, 2, 2);
                    using (Pen pc2 = new Pen(color2, 1.0F))
                        g.DrawPath(pc2, g2);

                    g.Dispose();
                }
            }
        }

        private GraphicsPath RectCutCorners(int x, int y, int width, int height, int roundnessleft, int roundnessright)
        {
            GraphicsPath gr = new GraphicsPath();

            gr.AddLine(x + roundnessleft, y, x + width - 1 - roundnessright, y);
            gr.AddLine(x + width - 1, y + roundnessright, x + width - 1, y + height - 1 - roundnessright);
            gr.AddLine(x + width - 1 - roundnessright, y + height - 1, x + roundnessleft, y + height - 1);
            gr.AddLine(x, y + height - 1 - roundnessleft, x, y + roundnessleft);
            gr.AddLine(x, y + roundnessleft, x + roundnessleft, y);         // close figure manually, closing it with a break does not seem to work
            return gr;
        }

        private byte limit(float a) { if (a > 255F) return 255; else return (byte)a; }
        public Color Multiply(Color from, float m) { return Color.FromArgb(from.A, limit((float)from.R * m), limit((float)from.G * m), limit((float)from.B * m)); }
    }

}
