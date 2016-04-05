using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtendedControls
{
    class DrawnPanel : Panel
    {
        public enum ImageType { Close, Minimize, Gripper };
        public ImageType Image { get; set; } = ImageType.Close;
        public int MarginSize { get; set; } = 4;                    // margin around icon

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen p1 = new Pen(this.ForeColor, 1.0F);
            Pen p2 = new Pen(this.ForeColor, 2.0F);

            int rightpx = ClientRectangle.Width - 1;
            int bottompx = ClientRectangle.Height - 1;

            int leftmarginpx = MarginSize;
            int rightmarginpx = rightpx - MarginSize;
            int topmarginpx = MarginSize;
            int bottommarginpx = bottompx - MarginSize;

            if (Image == ImageType.Close)
            {
                e.Graphics.DrawLine(p2, new Point(leftmarginpx, topmarginpx), new Point(rightmarginpx, bottommarginpx));
                e.Graphics.DrawLine(p2, new Point(leftmarginpx, bottommarginpx), new Point(rightmarginpx, topmarginpx));
            }
            else if (Image == ImageType.Minimize)
            {
                e.Graphics.DrawLine(p2, new Point(leftmarginpx, bottommarginpx), new Point(rightmarginpx, bottommarginpx));
            }
            else if (Image == ImageType.Gripper)
            {
                for (int i = 0; i < 3; i++)
                {
                    e.Graphics.DrawLine(p1, new Point(rightmarginpx - i * MarginSize, bottompx), new Point(rightpx, bottommarginpx - i * MarginSize));
                }
            }

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            p1.Dispose();
            p2.Dispose();
        }

    }
}
