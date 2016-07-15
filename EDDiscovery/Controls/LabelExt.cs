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
        public LabelExt()
        {
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            SizeF sz = pe.Graphics.MeasureString(this.Text, this.Font);

            if (sz.Width != 0)
            {
                using (Bitmap mp = new Bitmap((int)sz.Width, (int)sz.Height))   // bitmaps .. drawing directly does not work due to aliasing
                {
                    Graphics mpg = Graphics.FromImage(mp);

                    mpg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    using (Brush b = new SolidBrush(this.ForeColor))
                    {
                        mpg.DrawString(this.Text, this.Font, b, new Point(0, 0));
                    }

                    mpg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                    pe.Graphics.DrawImageUnscaled(mp, 0, 0);
                }
            }
        }
    }
}
