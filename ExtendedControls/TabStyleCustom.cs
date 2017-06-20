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
    #region public abstract class TabStyleCustom : object

    public abstract class TabStyleCustom
    {
        public abstract void DrawTab(Graphics gr, Rectangle borderrect, int index, bool selected, Color color1, Color color2, Color coloroutline, TabAlignment alignment);

        public virtual void DrawText(Graphics gr, Rectangle borderrect, int index, bool selected, Color color, string text, Font ft, Image icon)        // provide a standard version..
        {
            bool textpresent = text.Length > 0;

            if (icon != null)
            {
                int off = 8;
                Point pos = new Point(borderrect.X + off, borderrect.Y + borderrect.Height / 2 - icon.Height / 2);

                if (!textpresent)
                    pos = new Point(borderrect.X + borderrect.Width / 2 - icon.Width / 2, borderrect.Y + borderrect.Height / 2 - icon.Height / 2);

                gr.SmoothingMode = SmoothingMode.Default;
                gr.DrawImage(icon, pos);
                borderrect.X += icon.Width + off / 2;
                borderrect.Width -= icon.Width + off / 2;
            }

            if (textpresent)
            {
                using (StringFormat f = new StringFormat())
                using (Brush textb = new SolidBrush(color))
                {
                    f.Alignment = StringAlignment.Center;
                    f.LineAlignment = StringAlignment.Center;
                    gr.SmoothingMode = SmoothingMode.AntiAlias;
                    gr.DrawString(text, ft, textb, borderrect, f);
                }
            }
        }
    }

    #endregion // public abstract class TabStyleCustom : object

    #region public class TabStyleSquare : TabStyleCustom

    public class TabStyleSquare : TabStyleCustom
    {
        public override void DrawTab(Graphics gr, Rectangle borderrect, int index, bool selected, Color color1, Color color2, Color coloroutline, TabAlignment alignment)
        {
            System.Diagnostics.Debug.Assert(alignment == TabAlignment.Top);

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

            gr.SmoothingMode = SmoothingMode.Default;

            int xfar = borderrect.Right - 1;
            int yfar = borderrect.Bottom - 1;

            using (GraphicsPath fill = new GraphicsPath())
            using (Brush b = new LinearGradientBrush(borderrect, color1, color2, 90))
            {
                fill.AddLine(borderrect.X, yfar + additional + 1, borderrect.X, borderrect.Y);
                fill.AddLine(borderrect.X, borderrect.Y, xfar, borderrect.Y);
                fill.AddLine(xfar, borderrect.Y, xfar, yfar + additional + 1);
                gr.FillPath(b, fill);
            }

            using (GraphicsPath border = new GraphicsPath())
            using (Pen p = new Pen(coloroutline, 1.0F))
            {
                border.AddLine(borderrect.X, yfar, borderrect.X, borderrect.Y);
                border.AddLine(borderrect.X, borderrect.Y, xfar, borderrect.Y);
                border.AddLine(xfar, borderrect.Y, xfar, yfar);
                gr.DrawPath(p, border);
            }
        }
    }

    #endregion // public class TabStyleSquare : TabStyleCustom

    #region public class TabStyleRoundedEdge : TabStyleCustom

    public class TabStyleRoundedEdge : TabStyleCustom
    {
        public override void DrawTab(Graphics gr, Rectangle borderrect, int index, bool selected, Color color1, Color color2, Color coloroutline, TabAlignment alignment)
        {
            System.Diagnostics.Debug.Assert(alignment == TabAlignment.Top);

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

            gr.SmoothingMode = SmoothingMode.Default;
            using (GraphicsPath fill = new GraphicsPath())
            using (Brush b = new LinearGradientBrush(borderrect, color1, color2, 90))
            {
                fill.AddLine(borderrect.X + 1, yfar + 1 + additional, borderrect.X + 1, borderrect.Y);
                fill.AddArc(xfar - radius * 2, borderrect.Y, radius * 2, radius * 2, 270, 90);
                fill.AddLine(xfar, yfar + 1 + additional, xfar, yfar + 1 + additional);
                gr.FillPath(b, fill);
            }

            gr.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath border = new GraphicsPath())
            using (Pen p = new Pen(coloroutline, 1.0F))
            {
                border.AddLine(borderrect.X, yfar, borderrect.X, borderrect.Y);
                border.AddArc(xfar - radius * 2, borderrect.Y, radius * 2, radius * 2, 270, 90);
                border.AddLine(xfar, yfar, xfar, yfar);
                gr.DrawPath(p, border);
            }   
        }
    }

    #endregion // public class TabStyleRoundedEdge : TabStyleCustom

    #region public class TabStyleAngled : TabStyleCustom

    public class TabStyleAngled : TabStyleCustom
    {
        private const int shift = 6;

        public override void DrawTab(Graphics gr, Rectangle borderrect, int index, bool selected, Color color1, Color color2, Color coloroutline, TabAlignment alignment)
        {
            int xfar = borderrect.Right - 1;
            int ybot = (alignment == TabAlignment.Bottom) ? (borderrect.Y) : (borderrect.Bottom - 1);
            int ytop = (alignment == TabAlignment.Bottom) ? (borderrect.Bottom-1-((selected)?0:2)) : (borderrect.Y - ((selected) ? 2 : 0));

            gr.SmoothingMode = SmoothingMode.Default;
            using (GraphicsPath fill = new GraphicsPath())
            using (Brush b = new LinearGradientBrush(borderrect, color1, color2, 90))
            {
                fill.AddLine(borderrect.X, ybot + 1, borderrect.X + shift, ytop);
                fill.AddLine(borderrect.X + shift, ytop, xfar, ytop);
                fill.AddLine(xfar, ytop, xfar + shift, ybot + 1);
                gr.FillPath(b, fill);
            }   

            gr.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath border = new GraphicsPath())
            using (Pen p = new Pen(coloroutline, 1.0F))
            {
                border.AddLine(borderrect.X, ybot, borderrect.X + shift, ytop);
                border.AddLine(borderrect.X + shift, ytop, xfar, ytop);
                border.AddLine(xfar, ytop, xfar + shift, ybot);
                gr.DrawPath(p, border);
            }   
        }

        public override void DrawText(Graphics gr, Rectangle borderrect, int index, bool selected, Color color, string text, Font ft, Image icon)        // provide a standard version..
        {
            borderrect.Width += shift;
            base.DrawText(gr, borderrect, index, selected, color, text, ft,icon);
        }
    }

    #endregion // public class TabStyleAngled : TabStyleCustom
}
