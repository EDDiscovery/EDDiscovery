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
    }
}
