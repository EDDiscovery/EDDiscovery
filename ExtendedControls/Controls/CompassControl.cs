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
        public int WidthDegrees { get { return widthdegrees; } set { widthdegrees = value; Restart(); } }   // no of degrees to show
        public bool ShowNegativeDegrees { get { return degreeoffset != 0; } set { degreeoffset = value ? -180 : 0; Restart(); } }      // -180 to +180

        // Colours
        public Color StencilColor { get { return stencilcolor; } set { stencilcolor = value; Restart(); } }
        public Color CentreTickColor { get { return centretickcolor; } set { centretickcolor = value; Invalidate(); } } // Transparent turns it off
        public Color BugColor { get { return bugcolor; } set { bugcolor = value; Invalidate(); } }

        // Stencil tick rate
        public int StencilMajorTicksAt { get { return stencilmajortickat; } set { stencilmajortickat = value; Restart(); } }    // degrees
        public int StencilMinorTicksAt { get { return stencilminortickat; } set { stencilminortickat = value; Restart(); } } // degrees
        public bool AutoSetStencilTicks { get { return autosetstencilticks; } set { autosetstencilticks = value; Restart(); } } 

        // sizes - Percentages of the control, or percentages of the compass
        public int CompassHeightPercentage { get { return compassheightpercentage; } set { compassheightpercentage = value; Restart(); } }
        public int TickHeightPercentage { get { return tickheightpercentage; } set { tickheightpercentage = value; Invalidate(); } }   // of compass
        public int CentreTickHeightPercentage { get { return centreticklengthpercentage; } set { centreticklengthpercentage = value; Invalidate(); } } // of compass

        // Bug
        public double Bug { get { return bug + degreeoffset; } set { MoveBugToBearing(value - degreeoffset); } }    // Nan to disable
        public int BugSizePixels { get { return bugsize; } set { bugsize = value; Invalidate(); } }  

        // +180 or 360 is allowed as a sub for -180 or 0. Argument exception if out of range for both
        public double Bearing { get { return bearing + degreeoffset; } set { MoveToBearing(value - degreeoffset, false); } }   // go direct
        public double SlewToBearing { get { return slewtobearing + degreeoffset; } set { MoveToBearing(value - degreeoffset , true); } } // slew to
        public int SlewRateDegreesSec { get { return slewrate;} set { slewrate = value; } }

        // optional distance
        public double Distance { get { return distance; } set { distancemessage = string.Empty; distance = value; Invalidate(); } }     // NaN to disable, default
        public string DistanceFormat { get { return distanceformat; } set { distanceformat = value; ; } } // as "text {0:0.##} text". DOES NOT INVALIDATE.
        public void DistanceDisable(string msg) { distance = double.NaN; distancemessage = msg; Invalidate(); }

        // optional glideslope
        public double GlideSlope { get { return glideslope; } set { glideslope = value; Invalidate(); } }   // NaN to disable, default

        // optional message during Disable
        public string DisableMessage { get { return disablemessage; } set { disablemessage = value; Invalidate(); } } // below bar

        // optimised setters - do more at once
        public void Set(double bearing, double bug, double distance, bool slewto = false )
        {
            if (double.IsNaN(bearing))
                Enabled = false;
            else
            {
                if (!Enabled)
                    Enabled = true;

                if (this.distance != distance)
                {
                    this.distance = distance;
                    Invalidate();
                }

                if (this.bug != bug )
                {
                    MoveBugToBearing(bug);
                }

                MoveToBearing(bearing, slewto);
            }
        }

        #region Vars and Init

        private Bitmap compass = null;      // holds bitmap of compass
        private double bearing = 0;       // always 0-360 internally
        private double slewtobearing = 0;  // where we are going
        private int slewrate = 10;          // degrees/sec
        private double bug = double.NaN;  // always 0-360 or Nan
        private int degreeoffset = 0;
        private double distance = double.NaN;
        private string distanceformat = "{0:0.##}";
        private string distancemessage = string.Empty;
        private string disablemessage = string.Empty;
        private double glideslope = double.NaN;
        private Color stencilcolor = Color.Red;
        private int stencilmajortickat = 20;
        private int stencilminortickat = 5;
        private bool autosetstencilticks = false;
        private Color centretickcolor = Color.Green;
        private Color bugcolor = Color.White;
        private int bugsize = 10;
        private int widthdegrees = 180;
        private int compassheightpercentage = 60;           // of the control
        private int tickheightpercentage = 60;              // of the compass height
        private int centreticklengthpercentage = 60;       // of the compass height

        private const int pixelstart = -5;
        private double pixelsperdegree = 1;                 // reset on draw, may have called track to first
        private int fontline;
        private Label emergencydesignlabel;
        private System.Drawing.Drawing2D.SmoothingMode textsmoothingmode;      // transparent backgrounds .. mean no antialising

        private System.Windows.Forms.Timer slewtimer;
        private System.Diagnostics.Stopwatch slewstopwatch;
        private double accumulateddegrees;

        private bool lastdrawnenablestate = true;

        public CompassControl()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |        // "Free" double-buffering (1/3).
                ControlStyles.OptimizedDoubleBuffer |       // "Free" double-buffering (2/3).
                ControlStyles.ResizeRedraw |                // Invalidate after a resize or if the Padding changes.
                ControlStyles.SupportsTransparentBackColor, // BackColor.A can be less than 255.
                true);
            slewtimer = new Timer();
            slewtimer.Tick += Slewtimer_Tick;
            slewtimer.Interval = 50;
            slewstopwatch = new System.Diagnostics.Stopwatch();
        }

        public new void Dispose()
        {
            compass?.Dispose();
            slewtimer.Dispose();
        }

        private void MoveToBearing(double v, bool slew)     // always 0-360 incl
        {
            if (v >= 0 && v <= 360)     
            {
                v = v % 360;

                double delta = Math.Abs(bearing - v);
                double pixelstomove = delta * pixelsperdegree;

                //System.Diagnostics.Debug.WriteLine("B {0} to {1} Dist {2} pixels {3} slew {4}", bearing, v, delta, pixelstomove, slew);

                slewstopwatch.Reset();

                if (pixelstomove >= 1) // if visual change..
                {
                    if (slew)
                    {
                        slewtobearing = v;
                        slewtimer.Start();
                        slewstopwatch.Start();
                        accumulateddegrees = 0;
                    }
                    else
                    {                               // setting this stops any slewing and Stop.
                        slewtimer.Stop();
                        slewtobearing = bearing = v;
                        Invalidate();
                    }
                }
                else
                {
                    slewtimer.Stop();
                    slewtobearing = bearing = v;        // no need to repaint.
                }
            }
            else
                throw new ArgumentOutOfRangeException();
        }

        private void MoveBugToBearing(double v)
        {
            if (double.IsNaN(v))
            {
                if (!double.IsNaN(bug))
                {
                    bug = v;
                    Invalidate();
                }
            }
            else if (v >= 0 && v <= 360)
            {
                v = v % 360;

                double delta = Math.Abs(bug - v);       // how far..
                double pixelstomove = delta * pixelsperdegree;

                bug = v;

                if (pixelstomove >= 1)
                    Invalidate();
            }
            else
                throw new ArgumentOutOfRangeException();

            Invalidate();
        }

        private void Slewtimer_Tick(object sender, EventArgs e)
        {
            double degmovenow = (double)slewrate * (double)slewstopwatch.ElapsedMilliseconds / 1000.0;     // at this point we should have moved this number of degrees
            double step = degmovenow - accumulateddegrees;      // so we need to step this
            accumulateddegrees = degmovenow;

            double delta = (slewtobearing - bearing + 360) % 360;     // modulo

            //System.Diagnostics.Debug.WriteLine("B {0} to {1} Delta {2} step {3} degofmovement {4}", bearing, slewtobearing, delta, step, degmovenow);

            if (delta >= step && delta <= 360.0 - step)  // if difference is bigger than step, either way around..
            {
                bearing += (delta < 180) ? step : -step;
                bearing = (bearing + 360) % 360;
            }
            else
            {
                slewtimer.Stop();
                slewstopwatch.Reset();
                bearing = slewtobearing;
                //System.Diagnostics.Debug.WriteLine("..stop at {0}", bearing);
            }
            Invalidate();
        }


        protected override void OnResize(EventArgs e)
        {
            Restart();
        }

        private int ToVisual(double h)      // just correct the visual rep of degree.
        {
            int r = ((int)h + degreeoffset); return (r == -180) ? 180 : r;
        }

        private void Restart()
        {
            compass?.Dispose();
            compass = null;
            Invalidate();
        }

        // debug used to check accuracy of below - keep for now       int[] pixelatdegree = new int[360];       //            return pixelatdegree[(int)degree - pixelstart]; pixelatdegree[d - pixelstart] = x;

        int Bitmappixel(double degree)      // handle the strange -5 pixelstart stuff
        {
            if (degree >= 360 + pixelstart)
                degree -= 360;
            return (int)((degree - pixelstart) * pixelsperdegree);
        }

        #endregion

        #region Paint

        private void PaintCompass()
        {
            compass?.Dispose();     // ensure we are clean
            compass = null;

            pixelsperdegree = (double)this.Width / (double)WidthDegrees;

            int bitmapwidth = (int)(360 * pixelsperdegree);        // size of bitmap
            int bitmapheight = Height * CompassHeightPercentage / 100;

            //System.Diagnostics.Debug.WriteLine("Compass width " + this.Width + " deg width " + WidthDegrees + " pix/deg " + pixelsperdegree);

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

                    int stmajor = stencilmajortickat;
                    int stminor = stencilminortickat;

                    if ( autosetstencilticks )
                    {
                        double minmajorticks = sz.Width / pixelsperdegree;        
                       // System.Diagnostics.Debug.WriteLine("Major min ticks at {0} = {1}", sz.Width, minmajorticks);
                        if (minmajorticks >= 40)
                        {
                            stmajor = 80; stminor = 20;
                        }
                        else if (minmajorticks >= 20)
                        {
                            stmajor = 40; stminor = 10;
                        }
                        else if (minmajorticks >= 10)
                        {
                            stmajor = 20; stminor = 5;
                        }
                        else 
                        {
                            stmajor = 10; stminor = 2;
                        }
                    }

                    Color sc = Enabled ? StencilColor : StencilColor.Multiply(0.5F);
                    Pen p1 = new Pen(sc, 1);
                    Pen p2 = new Pen(sc, 2);
                    Brush textb = new SolidBrush(Enabled ? this.ForeColor : this.ForeColor.Multiply(0.5F));
                    var fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(ContentAlignment.MiddleCenter);

                    for (int d = pixelstart; d < 360 + pixelstart; d++)
                    {
                        int x = (int)((d - pixelstart) * pixelsperdegree);

                        bool majortick = d % stmajor == 0;
                        bool minortick = (d % stminor == 0) && !majortick;

                        if (majortick)
                        {
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                            g.DrawLine(p2, new Point(x, yline), new Point(x, yline + bigtickdepth));
                            g.SmoothingMode = textsmoothingmode;
                            g.DrawString(ToVisual(d).ToStringInvariant(), this.Font, textb, new Rectangle(x - 30, fontline, 60, compass.Height - fontline), fmt);

                            //DEBUG g.DrawLine(p1, x - sz.Width / 2, fontline, x + sz.Width / 2, fontline);
                        }

                        if ( minortick )
                        {
                            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
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
            {           // FromImage above crashes the designer.. 
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

            if (compass == null || Enabled != lastdrawnenablestate)     // if enabled change (which we don't trap directly due to it being a lower control) redraw as well
            {
                PaintCompass();
                lastdrawnenablestate = Enabled;
            }

            if (compass != null)        // designer problems
            {
                //System.Diagnostics.Debug.WriteLine("Compass width " + this.Width + " deg width " + WidthDegrees + " pix/deg " + pixelsperdegree);

                //System.Diagnostics.Debug.WriteLine("Bearing {0} bug {1}", bearing, bug);


                using (Brush textb = new SolidBrush(this.ForeColor))
                {
                    double startdegree = bearing - WidthDegrees / 2;       // where do we start

                    int p1start = Bitmappixel((360 + startdegree) % 360);       // this whole bit was a bit mind warping!
                    int p1width = Math.Min(compass.Width - p1start, this.Width);     // paint all you can up to compass end, limited by control width
                    int left = this.Width - p1width;                        // and this is what we need from the start of the image..

                    //System.Diagnostics.Debug.WriteLine("start {0} First paint {1} w {2} then {3}", startdegree, p1start, p1width, left);

                    pe.Graphics.DrawImage(compass, new Rectangle(0, 0, p1width, compass.Height), p1start, 0, p1width, compass.Height, GraphicsUnit.Pixel);
                    if (left > 0)
                        pe.Graphics.DrawImage(compass, new Rectangle(p1width, 0, left, compass.Height), 0, 0, left, compass.Height, GraphicsUnit.Pixel);

                    if (!Enabled)
                    {
                        if (disablemessage.Length > 0)
                        {
                            var fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(ContentAlignment.MiddleCenter);
                            pe.Graphics.SmoothingMode = textsmoothingmode;
                            pe.Graphics.DrawString(disablemessage, this.Font, textb, new Rectangle(0, compass.Height, this.Width, 24), fmt);
                            fmt.Dispose();
                        }
                    }
                    else
                    {
                        if (!CentreTickColor.IsFullyTransparent())
                        {
                            using (Pen p2 = new Pen(CentreTickColor, 4))
                                pe.Graphics.DrawLine(p2, new Point(this.Width / 2, 0), new Point(this.Width / 2, (compass.Height * centreticklengthpercentage / 100) - 1));
                        }

                        string distancetext = (double.IsNaN(distance) ? "" : string.Format(distanceformat, distance)) + distancemessage;
                        string glideslopetext = double.IsNaN(glideslope) ? "" : " (glideslope " + glideslope.ToString("0") + "°) ";

                        if (!double.IsNaN(bug))
                        {
                            using (Brush bbrush = new SolidBrush(BugColor))
                            {
                                int bugpixel = Bitmappixel(bug);                 // which pixel in the image is this?
                                int bugx = (bugpixel >= p1start) ? (bugpixel - p1start) : (bugpixel + p1width); // adjust to account for image wrap
                                double delta = (bug - bearing + 360) % 360;

                                //System.Diagnostics.Debug.WriteLine("bug {0} {1} => {2} Delta {3}", p1start, bugpixel, bugx, delta);

                                int bugy = compass.Height;

                                if (bugx < BugSizePixels || bugx >= this.Width - BugSizePixels)
                                {
                                    Rectangle r;
                                    StringFormat fmt;
                                    string text;

                                    bool itsleft = bugx < BugSizePixels || (delta > 180 && delta <= 270);
                                    int xmargin = 2;

                                    if (itsleft)
                                    {
                                        pe.Graphics.FillPolygon(bbrush, new Point[3] { new Point(0, bugy + BugSizePixels), new Point(BugSizePixels * 2, bugy), new Point(BugSizePixels * 2, bugy + BugSizePixels * 2) });
                                        xmargin += BugSizePixels * 2;
                                        r = new Rectangle(xmargin, bugy, this.Width - xmargin, BugSizePixels * 2);
                                        fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(ContentAlignment.MiddleLeft);
                                        text = ToVisual(bug) + "° " + distancetext;
                                    }
                                    else
                                    {
                                        pe.Graphics.FillPolygon(bbrush, new Point[3] { new Point(this.Width - 1, bugy + BugSizePixels), new Point(this.Width - 1 - BugSizePixels * 2, bugy), new Point(this.Width - 1 - BugSizePixels * 2, bugy + BugSizePixels * 2) });
                                        r = new Rectangle(0, bugy, this.Width - BugSizePixels * 2 - xmargin, BugSizePixels * 2);
                                        fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(ContentAlignment.MiddleRight);
                                        text = distancetext + " " + ToVisual(bug) + "°";
                                    }

                                    pe.Graphics.SmoothingMode = textsmoothingmode;
                                    pe.Graphics.DrawString(text, this.Font, textb, r, fmt);

                                    fmt.Dispose();
                                }
                                else
                                {
                                    pe.Graphics.FillPolygon(bbrush, new Point[3] { new Point(bugx, bugy), new Point(bugx - BugSizePixels, bugy + BugSizePixels * 2), new Point(bugx + BugSizePixels, bugy + BugSizePixels * 2) });

                                    if (distancetext.Length > 0)  // if anything to paint
                                    {
                                        SizeF sz = pe.Graphics.MeasureString(distancetext, Font);

                                        Rectangle r;
                                        StringFormat fmt;

                                        if (bugx > this.Width / 2)    // left or right, dependent
                                        {
                                            fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(ContentAlignment.MiddleRight);
                                            r = new Rectangle(0, bugy, bugx - BugSizePixels * 1, BugSizePixels * 2);
                                        }
                                        else
                                        {
                                            int xs = bugx + BugSizePixels * 1;
                                            r = new Rectangle(xs, bugy, this.Width - xs, BugSizePixels * 2);
                                            fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(ContentAlignment.MiddleLeft);
                                        }

                                        pe.Graphics.SmoothingMode = textsmoothingmode;
                                        pe.Graphics.DrawString(distancetext + glideslopetext, this.Font, textb, r, fmt);
                                        fmt.Dispose();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (distancetext.Length > 0)
                            {
                                var fmt = ControlHelpersStaticFunc.StringFormatFromContentAlignment(ContentAlignment.MiddleCenter);
                                pe.Graphics.SmoothingMode = textsmoothingmode;
                                pe.Graphics.DrawString(distancetext, this.Font, textb, new Rectangle(0, compass.Height, this.Width, 24), fmt);
                                fmt.Dispose();
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
