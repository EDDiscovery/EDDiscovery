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
    public abstract class TabStyleCustom
    {
        public abstract void DrawTab(Graphics gr, Rectangle borderrect, int index, bool selected, Color color1, Color color2, Color coloroutline);

        public virtual void DrawText(Graphics gr, Rectangle borderrect, int index, bool selected, Color color, string text, Font ft)        // provide a standard version..
        {
            StringFormat f = new StringFormat();
            f.Alignment = StringAlignment.Center;
            f.LineAlignment = StringAlignment.Center;
            gr.SmoothingMode = SmoothingMode.AntiAlias;
            using (Brush textb = new SolidBrush(color))
                gr.DrawString(text, ft, textb, borderrect, f);
        }
    }

    public class TabStyleSquare : TabStyleCustom
    {
        public override void DrawTab(Graphics gr, Rectangle borderrect, int index, bool selected, Color color1, Color color2, Color coloroutline)
        {
            int additional = 0;                             // extra depth for fill

            if (selected)
            {
                additional = 1;
                borderrect.Height += borderrect.Y;          // this one uses any height to get it bigger
                borderrect.Y = 0;
            }
            else
            {
                borderrect.Height -= 2;                     // shorter by this.
                borderrect.Y += 2;
            }

            int xfar = borderrect.Right - 1;
            int yfar = borderrect.Bottom - 1;

            GraphicsPath fill = new GraphicsPath();
            fill.AddLine(borderrect.X, yfar + additional + 1, borderrect.X, borderrect.Y);
            fill.AddLine(borderrect.X, borderrect.Y, xfar, borderrect.Y);
            fill.AddLine(xfar, borderrect.Y, xfar, yfar + additional + 1);
            GraphicsPath border = new GraphicsPath();
            border.AddLine(borderrect.X, yfar, borderrect.X, borderrect.Y);
            border.AddLine(borderrect.X, borderrect.Y, xfar, borderrect.Y);
            border.AddLine(xfar, borderrect.Y, xfar, yfar);

            gr.SmoothingMode = SmoothingMode.Default;
            using (Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(borderrect, color1, color2, 90))
                gr.FillPath(b, fill);
            using (Pen p = new Pen(coloroutline, 1.0F))
                gr.DrawPath(p, border);
        }
    }

    public class TabStyleRoundedEdge : TabStyleCustom
    {
        public override void DrawTab(Graphics gr, Rectangle borderrect, int index, bool selected, Color color1, Color color2, Color coloroutline)
        {
            int additional = 0;                             // extra depth for fill

            if (selected)
            {
                additional = 1;
                borderrect.Height += borderrect.Y;          // this one uses any height to get it bigger
                borderrect.Y = 0;
            }

            int radius = 10;
            int xfar = borderrect.Right - 1;
            int yfar = borderrect.Bottom - 1;

            GraphicsPath border = new GraphicsPath();
            border.AddLine(borderrect.X, yfar, borderrect.X, borderrect.Y);
            border.AddArc(xfar - radius * 2, borderrect.Y, radius * 2, radius * 2, 270, 90);
            border.AddLine(xfar, yfar, xfar, yfar);

            GraphicsPath fill = new GraphicsPath();
            fill.AddLine(borderrect.X + 1, yfar + 1 + additional, borderrect.X + 1, borderrect.Y);
            fill.AddArc(xfar - radius * 2, borderrect.Y, radius * 2, radius * 2, 270, 90);
            fill.AddLine(xfar, yfar + 1 + additional, xfar, yfar + 1 + additional);

            gr.SmoothingMode = SmoothingMode.Default;

            using (Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(borderrect, color1, color2, 90))
                gr.FillPath(b, fill);

            gr.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen p = new Pen(coloroutline, 1.0F))
                gr.DrawPath(p, border);
        }
    }

    public class TabStyleAngled : TabStyleCustom
    {
        private const int shift = 6;

        public override void DrawTab(Graphics gr, Rectangle borderrect, int index, bool selected, Color color1, Color color2, Color coloroutline)
        {
            int additional = 0;                             // extra depth for fill

            if (selected)
            {
                additional = 1;
                borderrect.Height += borderrect.Y;          // this one uses any height to get it bigger
                borderrect.Y = 0;
            }

            int xfar = borderrect.Right - 1;
            int yfar = borderrect.Bottom - 1;

            GraphicsPath border = new GraphicsPath();
            border.AddLine(borderrect.X, yfar + additional, borderrect.X + shift, borderrect.Y);
            border.AddLine(borderrect.X + shift, borderrect.Y, xfar, borderrect.Y);
            border.AddLine(xfar, borderrect.Y, xfar + shift, yfar + additional);

            GraphicsPath fill = new GraphicsPath();
            fill.AddLine(borderrect.X, yfar + additional + 1, borderrect.X + shift, borderrect.Y);
            fill.AddLine(borderrect.X + shift, borderrect.Y, xfar, borderrect.Y);
            fill.AddLine(xfar, borderrect.Y, xfar + shift, yfar + additional + 1);

            gr.SmoothingMode = SmoothingMode.Default;

            using (Brush b = new System.Drawing.Drawing2D.LinearGradientBrush(borderrect, color1, color2, 90))
                gr.FillPath(b, fill);

            gr.SmoothingMode = SmoothingMode.AntiAlias;

            using (Pen p = new Pen(coloroutline, 1.0F))
                gr.DrawPath(p, border);
        }

        public override void DrawText(Graphics gr, Rectangle borderrect, int index, bool selected, Color color, string text, Font ft)        // provide a standard version..
        {
            //borderrect.X += shift - 1;  // shift, because its sloped.. this looks about right
            borderrect.Width += shift;
            base.DrawText(gr, borderrect, index, selected, color, text, ft);
        }
    }
}

