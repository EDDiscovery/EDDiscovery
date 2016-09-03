using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class LabelExt : Label               // draws label using a bitmap - solves problems with aliasing over transparent backgrounds
    {
        public bool CentreX = false;
        public bool CentreY = false;
        public Color TextBackColor = Color.Transparent;

        public LabelExt()
        {
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            SizeF sizef = pe.Graphics.MeasureString(this.Text, this.Font);
            Size sz = new Size((int)(sizef.Width + 1), (int)(sizef.Height + 1));

            //Console.WriteLine("Label size {0}", sz);

            if (sz.Width > 0 && sz.Height>0 && this.Text.Length>0)
            {
                using (Bitmap mp = new Bitmap(sz.Width, sz.Height))   // bitmaps .. drawing directly does not work due to aliasing
                {
                    Graphics mpg = Graphics.FromImage(mp);

                    if (this.TextBackColor != Color.Transparent)
                    {
                        using (Brush b = new SolidBrush(this.TextBackColor))
                        {
                            mpg.FillRectangle(b, new Rectangle(0,0,sz.Width,sz.Height) );
                        }
                    }

                    mpg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    using (Brush b = new SolidBrush(this.ForeColor))
                    {
                        mpg.DrawString(this.Text, this.Font, b, new Point(0,0));
                    }

                    mpg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                    if (CentreY || CentreX)
                    {
                        int x = (CentreX) ? (ClientRectangle.Width / 2 - sz.Width / 2) : 0;
                        int y = (CentreY) ? (ClientRectangle.Height / 2 - sz.Height / 2) : 0;
                        if (x < 0)
                            x = 0;
                        if (y < 0)
                            y = 0;
                        pe.Graphics.DrawImageUnscaled(mp, x, y);
                    }
                    else
                        pe.Graphics.DrawImageUnscaled(mp, 0,0);
                }
            }
        }
    }
}
