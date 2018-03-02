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
    public partial class CompassControl : Control, IDisposable
    {
        public Color StencilColor { get { return stencilcolor; } set { stencilcolor = value; Restart(); } }
        public Color CentreTickColor { get { return centretickcolor; } set { centretickcolor = value; Invalidate(); } }
        public Color BugColor { get { return bugcolor; } set { bugcolor = value; Invalidate(); } }
        public int BugSize { get { return bugsize; } set { bugsize = value; Invalidate(); } }
        public int WidthDegrees { get { return widthdegrees; } set { widthdegrees = value; Restart(); } }
        public bool ShowNegativeDegrees { get { return degreeoffset != 0; } set { degreeoffset = value ? -180 : 0; Restart(); } }      // -180 to +180
        public int CompassHeightPercentage { get { return compassheightpercentage; } set { compassheightpercentage = value; Restart(); } }
        public int TickHeightPercentage { get { return tickheightpercentage; } set { tickheightpercentage = value; Invalidate(); } }   // of space left, after Font area
        public int CentreTickHeightPercentage { get { return centreticklengthpercentage; } set { centreticklengthpercentage = value; Invalidate(); } }

        // +180 or 360 is allowed as a sub for -180 or 0. Argument exception if out of range for both
        public double Bearing { get { return bearing + degreeoffset; } set { MoveToBearing(value - degreeoffset); } }
        public double Bug { get { return bug + degreeoffset; } set { MoveBugToBearing(value - degreeoffset); } }
        public double Distance { get { return distance; } set { distance = value; Invalidate(); } }     // NaN to disable, default
        public string DistanceFormat { get { return distanceformat; } set { distanceformat = value; Invalidate(); } }

        private Bitmap compass = null;      // holds bitmap of compass
        private double bearing = 0;       // always 0-360 internally
        private double bug = 90;            // always 0-360
        private int degreeoffset = 0;
        private double distance = double.NaN;
        private string distanceformat = "{0:0.##}";
        private Color stencilcolor = Color.Red;
        private Color centretickcolor = Color.Green;
        private Color bugcolor = Color.White;
        private int bugsize = 10;
        private int widthdegrees = 180;
        private int compassheightpercentage = 60;           // of the control
        private int tickheightpercentage = 60;              // of the compass height
        private int centreticklengthpercentage = 60;       // of the compass height

        private const int pixelstart = -5;
        private double pixelsperdegree;
        private int fontline;
        private Label emergencydesignlabel;
        private System.Drawing.Drawing2D.SmoothingMode textsmoothingmode;      // transparent backgrounds .. mean no antialising

        private int ToVisual(double h) { int r = ((int)h + degreeoffset); return (r == -180) ? 180 : r; }
        private void Restart() { compass?.Dispose(); compass = null; Invalidate(); }

        public CompassControl()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |        // "Free" double-buffering (1/3).
                ControlStyles.OptimizedDoubleBuffer |       // "Free" double-buffering (2/3).
                ControlStyles.ResizeRedraw |                // Invalidate after a resize or if the Padding changes.
                ControlStyles.SupportsTransparentBackColor, // BackColor.A can be less than 255.
                true);

        }

        public new void Dispose()
        {
            compass?.Dispose();
        }

        private void MoveToBearing(double v)
        {
            if (v >= 0 && v <= 360)     
            {
                bearing = v % 360;
                Invalidate();
            }
            else
                throw new ArgumentOutOfRangeException();
        }

        private void MoveBugToBearing(double v)
        {
            if (v >= 0 && v <= 360)
            {
                bug = v % 360;
                Invalidate();
            }
            else
                throw new ArgumentOutOfRangeException();
        }

        // debug       int[] pixelatdegree = new int[360];       //            return pixelatdegree[(int)degree - pixelstart]; pixelatdegree[d - pixelstart] = x;

        int Bitmappixel(double degree)      // handle the strange -5 pixelstart stuff
        {
            if (degree >= 360 + pixelstart)
                degree -= 360;
            return (int)((degree - pixelstart) * pixelsperdegree);
        }

        private void PaintCompass()
        {
            pixelsperdegree = (double)this.Width / (double)WidthDegrees;

            int bitmapwidth = (int)(360 * pixelsperdegree);        // size of bitmap
            int bitmapheight = Height * CompassHeightPercentage / 100;

            System.Diagnostics.Debug.WriteLine("Compass width " + this.Width + " deg width " + WidthDegrees + " pix/deg " + pixelsperdegree);

            if (!DesignMode)        // for some reason, FromImage craps it out
            {
                compass = new Bitmap(bitmapwidth, bitmapheight);

                using (Graphics g = Graphics.FromImage(compass))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                    if (!BackColor.IsFullyTransparent())
                    {
                        using (Brush b = new SolidBrush(BackColor))
                            g.FillRectangle(b, new Rectangle(0, 0, compass.Width, compass.Height));

                        textsmoothingmode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    }
                    else
                        textsmoothingmode = System.Drawing.Drawing2D.SmoothingMode.None;

                    int yline = 0;

                    SizeF sz = g.MeasureString("360", Font);
                    fontline = bitmapheight - (int)(sz.Height + 1);
                    int bigtickdepth = bitmapheight * TickHeightPercentage / 100;
                    int smalltickdepth = bigtickdepth / 2;

                    Pen p1 = new Pen(StencilColor, 1);
                    Pen p2 = new Pen(StencilColor, 2);
                    Brush textb = new SolidBrush(this.ForeColor);
                    var fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(ContentAlignment.MiddleCenter);

                    for (int d = pixelstart; d < 360 + pixelstart; d++)
                    {
                        int x = (int)((d - pixelstart) * pixelsperdegree);
                       

                        if (d % 5 == 0)
                        {

                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                            System.Diagnostics.Debug.WriteLine("Bitmap {0} to {1}", d, x);

                            if (d % 20 == 0)
                            {
                                g.DrawLine(p2, new Point(x, yline), new Point(x, yline + bigtickdepth));

                                g.SmoothingMode = textsmoothingmode;
                                g.DrawString(ToVisual(d).ToStringInvariant(), this.Font, textb, new Rectangle(x - 30, fontline, 60, compass.Height - fontline), fmt);
                            }
                            else
                                g.DrawLine(p1, new Point(x, yline), new Point(x, yline + smalltickdepth));
                        }
                    }

                    p1.Dispose();
                    p2.Dispose();
                    textb.Dispose();
                    fmt.Dispose();
                }
            }
            else
            {
                emergencydesignlabel = new Label();
                emergencydesignlabel.Location = new Point(10, 10);
                emergencydesignlabel.AutoSize = true;
                emergencydesignlabel.Text = "Compass.. FromImage crashes designer";
                this.BackColor = Color.LightBlue;
                Controls.Add(emergencydesignlabel);
            }
        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (compass == null)
                PaintCompass();

            if (compass != null)        // designer problems
            {
                //System.Diagnostics.Debug.WriteLine("Compass width " + this.Width + " deg width " + WidthDegrees + " pix/deg " + pixelsperdegree);

                System.Diagnostics.Debug.WriteLine("Bearing {0} bug {1}", bearing, bug);

                double startdegree = bearing - WidthDegrees / 2;       // where do we start

                int p1start = Bitmappixel((360 + startdegree) % 360);       // this whole bit was a bit mind warping!
                int p1width = Math.Min(compass.Width - p1start,this.Width);     // paint all you can up to compass end, limited by control width
                int left = this.Width - p1width;                        // and this is what we need from the start of the image..

                System.Diagnostics.Debug.WriteLine("start {0} First paint {1} w {2} then {3}", startdegree, p1start, p1width, left);

                pe.Graphics.DrawImage(compass, new Rectangle(0, 0, p1width, compass.Height), new Rectangle(p1start, 0, p1width, compass.Height), GraphicsUnit.Pixel);
                if (left > 0)
                    pe.Graphics.DrawImage(compass, new Rectangle(p1width, 0, left, compass.Height), new Rectangle(0, 0, left, compass.Height), GraphicsUnit.Pixel);

                pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                using (Pen p2 = new Pen(CentreTickColor, 4))
                    pe.Graphics.DrawLine(p2, new Point(this.Width / 2, 0), new Point(this.Width / 2, (compass.Height * centreticklengthpercentage / 100) - 1));

                using (Brush bbrush = new SolidBrush(BugColor))
                {
                    using (Brush textb = new SolidBrush(this.ForeColor))
                    {
                        int bugpixel = Bitmappixel(bug);                 // which pixel in the image is this?
                        int bugx = (bugpixel >= p1start) ? (bugpixel - p1start) : (bugpixel + p1width); // adjust to account for image wrap
                        double delta = (bug - bearing + 360) % 360;

                        System.Diagnostics.Debug.WriteLine("bug {0} {1} => {2} Delta {3}", p1start, bugpixel, bugx , delta);

                        int bugy = compass.Height;

                        if (bugx < BugSize || bugx >= this.Width - BugSize)
                        {
                            Rectangle r;
                            StringFormat fmt;
                            string text;

                            bool itsleft = bugx < BugSize || (delta > 180 && delta <= 270);
                            int xmargin = 2;

                            if (itsleft)
                            {
                                pe.Graphics.FillPolygon(bbrush, new Point[3] { new Point(0, bugy + BugSize), new Point(BugSize * 2, bugy), new Point(BugSize * 2, bugy + BugSize * 2) });
                                xmargin += BugSize * 2;
                                r = new Rectangle(xmargin, bugy, this.Width - xmargin, BugSize * 2);
                                fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(ContentAlignment.MiddleLeft);
                                text = ToVisual(bug) + "° " + (double.IsNaN(distance) ? "" : string.Format(distanceformat, distance));
                            }
                            else
                            {
                                pe.Graphics.FillPolygon(bbrush, new Point[3] { new Point(this.Width - 1, bugy + BugSize), new Point(this.Width - 1 - BugSize * 2, bugy), new Point(this.Width - 1 - BugSize * 2, bugy + BugSize * 2) });
                                r = new Rectangle(0, bugy, this.Width - BugSize * 2 - xmargin, BugSize * 2);
                                fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(ContentAlignment.MiddleRight);
                                text = (double.IsNaN(distance) ? "" : string.Format(distanceformat, distance)) + " " + ToVisual(bug) + "°";
                            }

                            pe.Graphics.SmoothingMode = textsmoothingmode;
                            pe.Graphics.DrawString(text, this.Font, textb, r, fmt);

                            fmt.Dispose();
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Inside");
                            pe.Graphics.FillPolygon(bbrush, new Point[3] { new Point(bugx, bugy), new Point(bugx - BugSize, bugy + BugSize * 2), new Point(bugx + BugSize, bugy + BugSize * 2) });

                            if (!double.IsNaN(distance))
                            {
                                string text = string.Format(distanceformat, distance);
                                SizeF sz = pe.Graphics.MeasureString(text, Font);

                                Rectangle r;
                                StringFormat fmt;

                                if (bugx > this.Width/2)    // left or right, depe
                                {
                                    fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(ContentAlignment.MiddleRight);
                                    r = new Rectangle(0, bugy, bugx - BugSize * 1, BugSize * 2);
                                }
                                else
                                {
                                    int xs = bugx + BugSize * 1;
                                    r = new Rectangle(xs, bugy, this.Width -xs , BugSize * 2);
                                    fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(ContentAlignment.MiddleLeft);
                                }

                                pe.Graphics.SmoothingMode = textsmoothingmode;
                                pe.Graphics.DrawString(text, this.Font, textb, r, fmt);
                                fmt.Dispose();
                            }
                        }

                    }
                }
            }
        }
    }
}
