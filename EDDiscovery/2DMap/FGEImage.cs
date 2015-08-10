using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EDDiscovery2
{
    public class FGEImage
    {
        public string Name;
        public Point TopLeft, TopRight, BottomLeft, BottomRight;
        public Point pxTopLeft, pxTopRight, pxBottomLeft, pxBottomRight;

        public FGEImage(string filename)
        {
            Name = filename;
        }

        public Point TransformCoordinate(Point coordinate)
        {
            int diffx1, diffx2, diffy1, diffy2;
            int diffpx1, diffpx2, diffpy1, diffpy2;

            //Transform trans;

            diffx1 = TopRight.X - TopLeft.X;
            diffx2 = BottomRight.X - BottomLeft.X;
            diffy1 = TopLeft.Y - BottomLeft.Y;
            diffy2 = TopRight.Y - BottomRight.Y;

            diffpx1 = pxTopRight.X - pxTopLeft.X;
            diffpx2 = pxBottomRight.X - pxBottomLeft.X;
            diffpy1 = pxTopLeft.Y - pxBottomLeft.Y;
            diffpy2 = pxTopRight.Y - pxBottomRight.Y;

            double dx1, dx2, dy1, dy2;

            dx1 = diffpx1 / (double)diffx1;
            dx2 = diffpx2 / (double)diffx2;
            dy1 = diffpy1 / (double)diffy1;
            dy2 = diffpy2 / (double)diffy2;

            Point newPoint = new Point(coordinate.X - BottomLeft.X, coordinate.Y - BottomLeft.Y);

            // Calculate dx and dy for point;
            double dx, dy;

            dx = dx2 + newPoint.Y / (double)diffy1 * (dx1 - dx2);
            dy = dy2 + newPoint.X / (double)diffx1 * (dy1- dy2);

            return new Point((int)(newPoint.X*dx + pxBottomLeft.X + newPoint.Y / (double)diffy1 * (pxTopLeft.X - pxBottomLeft.X)), 
                             (int)(newPoint.Y*dy + pxBottomLeft.Y + newPoint.X / (double)diffx1 * (pxTopRight.Y - pxTopLeft.Y)));

        }

    }
}
