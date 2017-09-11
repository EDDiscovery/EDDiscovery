/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtendedControls
{
    [DefaultEvent(nameof(MouseClick))]
    public class DrawnPanel : Panel
    {
        // Back, Fore color used
        [DefaultValue(typeof(Color), nameof(Color.White))]
        public Color MouseOverColor { get; set; } = Color.White;
        [DefaultValue(typeof(Color), nameof(Color.Green))]
        public Color MouseSelectedColor { get; set; } = Color.Green;
        [DefaultValue(true)]
        public bool MouseSelectedColorEnable { get; set; } = true;      // set to disable selected colour in some crazy windows situations where clicks are lost
        [DefaultValue(0.25f)]
        public float PanelDisabledScaling { get; set; } = 0.25F;

        public enum ImageType { Close, Minimize, OnTop, Floating, Gripper, EDDB, Ross, InverseText,
                                Move, Text, None ,
                                NotTransparent, Transparent, TransparentClickThru, FullyTransparent,
                                WindowInTaskBar, WindowNotInTaskBar, Captioned, NotCaptioned,
                                Bars, Maximize, Restore };

        [DefaultValue(ImageType.Close)]
        public ImageType ImageSelected { get { return _ImageSelected; } set { _ImageSelected = value; Invalidate(); } }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ImageType _ImageSelected = ImageType.Close;

        [DefaultValue(null)]
        public Image DrawnImage { get; set; } = null;                                   // if not set, an image is drawn . Use None below for a image only

        [DefaultValue(null)]
        public string ImageText { get; set; } = null;       // for Text Type
        [DefaultValue(4)]
        public int MarginSize { get; set; } = 4;                    // margin around icon, 0 =auto, -1 = zero

        #region Public Functions
        public void Captured()                                     // if doing the move capture stuff on this panel, call this
        {
            mousecapture = true;
            Invalidate();
        }

        [Browsable(false)]
        public bool IsCaptured { get { return mousecapture; } }

        private ImageAttributes DrawnImageAttributesEnabled = null;         // Image override (colour etc) for images using Image
        private ImageAttributes DrawnImageAttributesDisabled = null;         // Image override (colour etc) for images using Image

        public void SetDrawnBitmapRemapTable(ColorMap[] remap, float[][] colormatrix = null)
        {
            ControlHelpersStaticFunc.ComputeDrawnPanel(out DrawnImageAttributesEnabled, out DrawnImageAttributesDisabled, PanelDisabledScaling, remap, colormatrix);
        }

        #endregion

        #region Implementation

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //System.Diagnostics.Debug.WriteLine("DP Paint " + this.Name + " MD " + mousedown + " MO "  + mouseover);
            if ( DrawnImage != null )
            {
                if (DrawnImageAttributesEnabled != null)
                    e.Graphics.DrawImage(DrawnImage, new Rectangle(0, 0, DrawnImage.Width, DrawnImage.Height), 0, 0, DrawnImage.Width, DrawnImage.Height, GraphicsUnit.Pixel, DrawnImageAttributesEnabled );
                else
                    e.Graphics.DrawImage(DrawnImage, new Rectangle(0, 0, DrawnImage.Width, DrawnImage.Height), 0, 0, DrawnImage.Width, DrawnImage.Height, GraphicsUnit.Pixel);
            }

            if (ImageSelected != ImageType.None)
            {
                int msize = (MarginSize == -1) ? 0 : ((MarginSize > 0) ? MarginSize : ClientRectangle.Height / 6);
                Color pc = (Enabled) ? ((mousedown || mousecapture) ? MouseSelectedColor : ((mouseover) ? MouseOverColor : this.ForeColor)) : this.ForeColor.Average(this.BackColor, PanelDisabledScaling);
                //Console.WriteLine("Enabled" + Enabled + " Mouse over " + mouseover + " mouse down " + mousedown);

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                Pen p1 = new Pen(pc, 1.0F);
                Pen p2 = new Pen(pc, 2.0F);

                int rightpx = ClientRectangle.Width - 1;
                int bottompx = ClientRectangle.Height - 1;
                int centrehorzpx = (ClientRectangle.Width - 1) / 2;
                int centrevertpx = (ClientRectangle.Height - 1) / 2;

                int leftmarginpx = msize;
                int rightmarginpx = rightpx - msize;
                int topmarginpx = msize;
                int bottommarginpx = bottompx - msize;

                Rectangle workingarea = new Rectangle(leftmarginpx, topmarginpx, rightmarginpx - leftmarginpx + 1, bottommarginpx - topmarginpx + 1);

                int marginwidth = rightmarginpx - leftmarginpx + 1;
                int marginheight = bottommarginpx - topmarginpx + 1;

                if (ImageSelected == ImageType.Close)
                {
                    e.Graphics.DrawLine(p2, new Point(leftmarginpx, topmarginpx), new Point(rightmarginpx, bottommarginpx));
                    e.Graphics.DrawLine(p2, new Point(leftmarginpx, bottommarginpx), new Point(rightmarginpx, topmarginpx));
                }
                else if (ImageSelected == ImageType.Minimize)
                {
                    e.Graphics.DrawLine(p2, new Point(leftmarginpx, bottommarginpx), new Point(rightmarginpx, bottommarginpx));
                }
                else if (ImageSelected == ImageType.OnTop)
                {
                    using (Brush bbck = new SolidBrush(pc))
                    {
                        Rectangle area = new Rectangle(leftmarginpx, topmarginpx, rightmarginpx - leftmarginpx + 1, bottommarginpx - topmarginpx + 1);
                        e.Graphics.FillRectangle(bbck, area);
                    }
                }
                else if (ImageSelected == ImageType.Floating)
                {
                    e.Graphics.DrawRectangle(p2, workingarea);
                }
                else if (ImageSelected == ImageType.TransparentClickThru)
                {
                    e.Graphics.DrawRectangle(p2, workingarea);

                    e.Graphics.DrawLine(p2, new Point(leftmarginpx + 2, topmarginpx + 2), new Point(rightmarginpx - 2, topmarginpx + 2));
                    e.Graphics.DrawLine(p2, new Point(centrehorzpx, topmarginpx + 2), new Point(centrehorzpx, bottommarginpx - 2));

                    e.Graphics.DrawLine(p2, new Point(centrehorzpx + 2, topmarginpx + 6), new Point(centrehorzpx + 6, topmarginpx + 6));
                    e.Graphics.DrawLine(p2, new Point(centrehorzpx + 2, topmarginpx + 6), new Point(centrehorzpx + 2, topmarginpx + 10));
                    e.Graphics.DrawLine(p2, new Point(centrehorzpx + 2, topmarginpx + 10), new Point(centrehorzpx + 6, topmarginpx + 10));
                }
                else if (ImageSelected == ImageType.FullyTransparent)
                {
                    e.Graphics.DrawRectangle(p2, workingarea);

                    e.Graphics.DrawLine(p2, new Point(leftmarginpx + 2, topmarginpx + 2), new Point(rightmarginpx - 2, topmarginpx + 2));
                    e.Graphics.DrawLine(p2, new Point(centrehorzpx, topmarginpx + 2), new Point(centrehorzpx, bottommarginpx - 2));

                    e.Graphics.DrawLine(p2, new Point(centrehorzpx + 2, topmarginpx + 6), new Point(centrehorzpx + 6, topmarginpx + 6));
                    e.Graphics.DrawLine(p2, new Point(centrehorzpx + 2, topmarginpx + 6), new Point(centrehorzpx + 2, topmarginpx + 11));
                    e.Graphics.DrawLine(p2, new Point(centrehorzpx + 2, topmarginpx + 8), new Point(centrehorzpx + 6, topmarginpx + 8));
                }
                else if (ImageSelected == ImageType.Transparent)
                {
                    e.Graphics.DrawRectangle(p2, workingarea);

                    e.Graphics.DrawLine(p2, new Point(leftmarginpx + 2, topmarginpx + 3), new Point(rightmarginpx - 2, topmarginpx + 3));
                    e.Graphics.DrawLine(p2, new Point(centrehorzpx, topmarginpx + 3), new Point(centrehorzpx, bottommarginpx - 3));

                }
                else if (ImageSelected == ImageType.NotTransparent)
                {
                    e.Graphics.DrawLine(p2, new Point(leftmarginpx + 2, topmarginpx + 2), new Point(rightmarginpx - 2, topmarginpx + 2));
                    e.Graphics.DrawLine(p2, new Point(centrehorzpx, topmarginpx + 2), new Point(centrehorzpx, bottommarginpx - 2));
                }
                else if (ImageSelected == ImageType.Captioned)
                {
                    e.Graphics.DrawRectangle(p2, workingarea);

                    e.Graphics.DrawLine(p2, new Point(leftmarginpx + 2, topmarginpx + 2), new Point(rightmarginpx - 2, topmarginpx + 2));
                    e.Graphics.DrawLine(p2, new Point(leftmarginpx + 2, bottommarginpx - 2), new Point(rightmarginpx - 2, bottommarginpx - 2));
                    e.Graphics.DrawLine(p2, new Point(leftmarginpx + 2, topmarginpx + 2), new Point(leftmarginpx + 2, bottommarginpx - 2));
                }
                else if (ImageSelected == ImageType.NotCaptioned)
                {
                    e.Graphics.DrawLine(p2, new Point(leftmarginpx + 2, topmarginpx + 2), new Point(rightmarginpx - 2, topmarginpx + 2));
                    e.Graphics.DrawLine(p2, new Point(leftmarginpx + 2, bottommarginpx - 2), new Point(rightmarginpx - 2, bottommarginpx - 2));
                    e.Graphics.DrawLine(p2, new Point(leftmarginpx + 2, topmarginpx + 2), new Point(leftmarginpx + 2, bottommarginpx - 2));
                }
                else if (ImageSelected == ImageType.Gripper)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        e.Graphics.DrawLine(p1, new Point(rightmarginpx - i * msize, bottompx), new Point(rightpx, bottommarginpx - i * msize));
                    }
                }
                else if (ImageSelected == ImageType.EDDB)
                {
                    Brush bbck = new SolidBrush(pc);
                    Rectangle area = new Rectangle(leftmarginpx, topmarginpx, ClientRectangle.Width - 2 * msize, ClientRectangle.Height - 2 * msize);
                    e.Graphics.FillRectangle(bbck, area);
                    bbck.Dispose();

                    Pen pb = new Pen(this.BackColor, 2.0F);
                    Point pt1 = new Point(rightmarginpx, bottommarginpx - msize);
                    Point pt2 = new Point(centrehorzpx - 1, bottommarginpx - msize);
                    Point pt3 = new Point(centrehorzpx - 1, topmarginpx + msize);
                    Point pt4 = new Point(centrehorzpx - 1 - msize, pt3.Y + 2);
                    Point pt5 = new Point(centrehorzpx - 1 + msize, pt3.Y + 2);

                    e.Graphics.DrawLine(pb, pt1, pt2);
                    e.Graphics.DrawLine(pb, pt2, pt3);
                    e.Graphics.DrawLine(pb, pt2, pt3);
                    e.Graphics.DrawLine(pb, pt4, pt5);

                    pb.Dispose();
                }
                else if (ImageSelected == ImageType.Ross)
                {
                    Pen pb = new Pen(pc, 3.0F);
                    Point pt1 = new Point(leftmarginpx + 2, bottommarginpx);
                    Point pt2 = new Point(pt1.X, topmarginpx + 4);
                    Point pt3 = new Point(centrehorzpx + 2, pt2.Y);

                    e.Graphics.DrawLine(pb, pt1, pt2);
                    e.Graphics.DrawLine(pb, pt2, pt3);

                    pb.Dispose();
                }
                else if (ImageSelected == ImageType.InverseText)
                {
                    if (this.ImageText.Length > 0)
                    {
                        SizeF size = e.Graphics.MeasureString(this.ImageText, this.Font);
                        double scale = (double)(ClientRectangle.Height - topmarginpx * 2) / (double)size.Height;
                        // given the available height, scale the font up if its bigger than the current font height.

                        using (Font fnt = new Font(this.Font.Name, (float)(this.Font.SizeInPoints * scale), this.Font.Style))
                        {
                            size = e.Graphics.MeasureString(this.ImageText, fnt);
                            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;     //MUST turn it off to get a sharp rect

                            using (Brush bbck = new SolidBrush(pc))
                                e.Graphics.FillRectangle(bbck, new Rectangle(leftmarginpx, topmarginpx, ClientRectangle.Width - 2 * msize, ClientRectangle.Height - 2 * msize));

                            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            using (Brush textb = new SolidBrush(this.BackColor))
                                e.Graphics.DrawString(this.ImageText, fnt, textb, new Point(centrehorzpx - (int)(size.Width / 2), topmarginpx));
                        }
                    }
                }
                else if (ImageSelected == ImageType.Text)
                {
                    if (this.ImageText.Length > 0)
                    {
                        SizeF size = e.Graphics.MeasureString(this.ImageText, this.Font);
                        double scale = (double)(ClientRectangle.Height - topmarginpx * 2) / (double)size.Height;
                        // given the available height, scale the font up if its bigger than the current font height.

                        using (Font fnt = new Font(this.Font.Name, (float)(this.Font.SizeInPoints * scale), this.Font.Style))
                        {
                            size = e.Graphics.MeasureString(this.ImageText, fnt);
                            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            using (Brush textb = new SolidBrush(this.ForeColor))
                                e.Graphics.DrawString(this.ImageText, fnt, textb, new Point(centrehorzpx - (int)(size.Width / 2), topmarginpx));
                        }
                    }
                }
                else if (ImageSelected == ImageType.Move)
                {
                    centrehorzpx++;
                    centrevertpx++;

                    int o = ClientRectangle.Width / 8;
                    e.Graphics.DrawLine(p2, new Point(centrehorzpx, bottompx), new Point(centrehorzpx, topmarginpx));
                    e.Graphics.DrawLine(p1, new Point(centrehorzpx - o, bottompx - o), new Point(centrehorzpx, bottompx));
                    e.Graphics.DrawLine(p1, new Point(centrehorzpx + o, bottompx - o), new Point(centrehorzpx, bottompx));
                    e.Graphics.DrawLine(p1, new Point(centrehorzpx - o, topmarginpx + o), new Point(centrehorzpx, topmarginpx));
                    e.Graphics.DrawLine(p1, new Point(centrehorzpx + o, topmarginpx + o), new Point(centrehorzpx, topmarginpx));

                    e.Graphics.DrawLine(p2, new Point(leftmarginpx, centrevertpx), new Point(rightmarginpx, centrevertpx));
                    e.Graphics.DrawLine(p1, new Point(leftmarginpx + o, centrevertpx - o), new Point(leftmarginpx, centrevertpx));
                    e.Graphics.DrawLine(p1, new Point(leftmarginpx + o, centrevertpx + o), new Point(leftmarginpx, centrevertpx));
                    e.Graphics.DrawLine(p1, new Point(rightmarginpx - o, centrevertpx - o), new Point(rightmarginpx, centrevertpx));
                    e.Graphics.DrawLine(p1, new Point(rightmarginpx - o, centrevertpx + o), new Point(rightmarginpx, centrevertpx));
                }
                else if (ImageSelected == ImageType.WindowInTaskBar || ImageSelected == ImageType.WindowNotInTaskBar)
                {
                    int o = 4;
                    int w = 3;
                    int top = centrevertpx - o + 1;

                    e.Graphics.DrawRectangle(p1, new Rectangle(leftmarginpx, top, marginwidth, o * 2));

                    if (ImageSelected == ImageType.WindowInTaskBar)
                    {
                        using (Brush bbck = new SolidBrush(pc))
                        {
                            e.Graphics.FillRectangle(bbck, new Rectangle(leftmarginpx + 2, top + 2, w, o));
                            e.Graphics.FillRectangle(bbck, new Rectangle(leftmarginpx + 2 + w + 2, top + 2, w, o));
                        }
                    }
                }
                else if (ImageSelected == ImageType.Bars)
                {
                    e.Graphics.DrawLine(p1, new Point(leftmarginpx, 0), new Point(rightmarginpx, 0));
                    e.Graphics.DrawLine(p1, new Point(leftmarginpx, 2), new Point(rightmarginpx, 2));
                }
                else if (ImageSelected == ImageType.Maximize)
                {
                    e.Graphics.DrawRectangle(p1, new Rectangle(leftmarginpx, topmarginpx, marginwidth - 1, marginheight));
                    e.Graphics.DrawLine(p2, new Point(leftmarginpx, topmarginpx + 1), new Point(rightmarginpx, topmarginpx + 1));
                }
                else if (ImageSelected == ImageType.Restore)
                {
                    // lower-left foreground "window" clockwise from top-left
                    e.Graphics.DrawLine(p2, new Point(leftmarginpx, topmarginpx + 8), new Point(rightmarginpx - 6, topmarginpx + 8));
                    e.Graphics.DrawLine(p1, new Point(rightmarginpx - 6, topmarginpx + 8), new Point(rightmarginpx - 6, bottommarginpx));
                    e.Graphics.DrawLine(p1, new Point(rightmarginpx - 6, bottommarginpx), new Point(leftmarginpx, bottommarginpx));
                    e.Graphics.DrawLine(p1, new Point(leftmarginpx, bottommarginpx), new Point(leftmarginpx, topmarginpx + 8));

                    // upper-right background "window" clockwise from (obscured!) bottom-left
                    e.Graphics.DrawLine(p1, new Point(leftmarginpx + 6, topmarginpx + 8), new Point(leftmarginpx + 6, topmarginpx + 2));
                    e.Graphics.DrawLine(p2, new Point(leftmarginpx + 6, topmarginpx + 2), new Point(rightmarginpx, topmarginpx + 2));
                    e.Graphics.DrawLine(p1, new Point(rightmarginpx, topmarginpx + 2), new Point(rightmarginpx, bottommarginpx - 6));
                    e.Graphics.DrawLine(p1, new Point(rightmarginpx, bottommarginpx - 6), new Point(rightmarginpx - 6, bottommarginpx - 6));
                }

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                p1.Dispose();
                p2.Dispose();
            }
        }

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            mouseover = true;
            mousedown = false;
            mousecapture = false;                   // mouse enter called after capture finished, so clear it
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseLeave(eventargs);
            mouseover = false;
            mousedown = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            //System.Diagnostics.Debug.WriteLine("DP MD");
            mousedown = true;

            if (MouseSelectedColorEnable) // only invalidate if req.
                Invalidate();
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            //System.Diagnostics.Debug.WriteLine("DP MU");
            mousedown = false;
            if (MouseSelectedColorEnable) // only invalidate if req.
                Invalidate();
            base.OnMouseUp(mevent);
        }

        private bool mouseover = false;
        private bool mousedown = false;
        private bool mousecapture = false;
#endregion
    }

    public class DrawnPanelNoTheme : DrawnPanel     // use if you want the panel to be themed.. sometimes you do, sometimes you don't
    {
        public DrawnPanelNoTheme()
        {

        }
    }

    public class PanelNoTheme: Panel        // slightly random placement..
    {
        public PanelNoTheme()
        {

        }
    }


}
