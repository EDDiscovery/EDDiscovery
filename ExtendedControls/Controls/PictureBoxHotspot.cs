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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public partial class PictureBoxHotspot : PictureBox
    {
        // Image elements holds the bitmap and the location, plus its tag and tip

        public class ImageElement
        {
            public ImageElement()
            {
            }

            public ImageElement(Rectangle p, Image i, Object t = null, string tt = null, bool imgowned = true)
            {
                pos = p; img = i; tag = t; tooltip = tt; this.imgowned = imgowned;
            }

            public void Image(Rectangle p, Image i, Object t = null, string tt = null, bool imgowned = true)
            {
                pos = p; img = i; tag = t; tooltip = tt; this.imgowned = imgowned;
            }

            // centred, autosized
            public void TextCentreAutosize(Point poscentrehorz, Size max, string text, Font dp, Color c, Color backcolour, float backscale = 1.0F, Object t = null, string tt = null)
            {
                img = BaseUtils.BitMapHelpers.DrawTextIntoAutoSizedBitmap(text, max, dp, c, backcolour, backscale);
                pos = new Rectangle(poscentrehorz.X - img.Width / 2, poscentrehorz.Y, img.Width, img.Height);
                tag = t;
                tooltip = tt;
            }

            // top left, autosized
            public void TextAutosize(Point topleft, Size max, string text, Font dp, Color c, Color backcolour, float backscale = 1.0F, Object t = null, string tt = null)
            {
                img = BaseUtils.BitMapHelpers.DrawTextIntoAutoSizedBitmap(text, max, dp, c, backcolour, backscale);
                pos = new Rectangle(topleft.X, topleft.Y, img.Width, img.Height);
                tag = t;
                tooltip = tt;
            }

            // top left, sized
            public void TextFixedSizeC(Point topleft, Size size, string text, Font dp, Color c, Color backcolour, 
                                    float backscale = 1.0F, bool centertext = false,
                                    Object t = null, string tt = null)
            {
                img = BaseUtils.BitMapHelpers.DrawTextIntoFixedSizeBitmapC(text, size, dp, c, backcolour, backscale, centertext );
                pos = new Rectangle(topleft.X, topleft.Y, img.Width, img.Height);
                tag = t;
                tooltip = tt;
            }

            public void SetAlternateImage(Image i, Rectangle p, bool mo = false, bool imgowned = true)
            {
                altimg = i;
                altpos = p;
                mouseover = mo;
                altimgowned = imgowned;
            }

            public bool SwapImages(Image surface)           // swap to alternative, optionally, draw to surface
            {
                if (altimg != null)
                {
                    Rectangle r = pos;
                    pos = altpos;
                    altpos = r;

                    Image i = img;
                    img = altimg;
                    altimg = i;

                    bool io = imgowned;     // swap tags
                    imgowned = altimgowned;
                    altimgowned = io;

                    //System.Diagnostics.Debug.WriteLine("Element @ " + pos + " " + inaltimg);
                    if (surface != null)
                    {
                        using (Graphics gr = Graphics.FromImage(surface)) //restore
                        {
                            gr.Clip = new Region(altpos);       // remove former
                            gr.Clear(Color.FromArgb(0, Color.Black));       // set area back to transparent before paint..
                        }

                        using (Graphics gr = Graphics.FromImage(surface)) //restore
                            gr.DrawImage(img, pos);
                    }

                    inaltimg = !inaltimg;
                    return true;
                }
                else
                    return false;
            }

            public Rectangle pos;
            public Image img;
            public bool imgowned;
            public Object tag;
            public string tooltip;

            public Image altimg;
            public bool altimgowned;
            public Rectangle altpos;
            public bool inaltimg = false;

            public bool mouseover;

            public void Translate(int x,int y)
            {
                pos = new Rectangle(pos.X + x, pos.Y + y, pos.Width, pos.Height);
            }

            public void Position(int x, int y)
            {
                pos = new Rectangle(x, y, pos.Width, pos.Height);
            }
        }

        public delegate void OnElement(object sender, MouseEventArgs eventargs, ImageElement i, object tag);
        public event OnElement EnterElement;
        public event OnElement LeaveElement;
        public event OnElement ClickElement;

        ImageElement elementin = null;

        public Color FillColor = Color.Transparent;         // fill the bitmap with this colour before pasting the bitmaps in

        private Timer hovertimer = new Timer();
        ToolTip hovertip = null;
        Point hoverpos;
        private List<ImageElement> elements = new List<ImageElement>();

        #region Interface

        public PictureBoxHotspot()
        {
            hovertimer.Interval = 250;
            hovertimer.Tick += HoverEnd;
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;
        }

        public void Add(ImageElement i)
        {
            elements.Add(i);
        }

        public void AddRange(List<ImageElement> list)
        {
            elements.AddRange(list);
        }

        // topleft, autosized
        public ImageElement AddTextAutoSize(Point topleft, Size max, string label, Font fnt, Color c, Color backcolour, float backscale, Object tag = null, string tiptext = null)
        {
            ImageElement lab = new ImageElement();
            lab.TextAutosize(topleft, max, label, fnt, c, backcolour, backscale, tag, tiptext);
            elements.Add(lab);
            return lab;
        }

        // topleft, sized
        public ImageElement AddTextFixedSizeC(Point topleft, Size size, string label, Font fnt, Color c, Color backcolour, float backscale, bool centered , Object tag = null, string tiptext = null)
        {
            ImageElement lab = new ImageElement();
            lab.TextFixedSizeC(topleft, size, label, fnt, c, backcolour, backscale, centered, tag, tiptext);
            elements.Add(lab);
            return lab;
        }

        // centre pos, autosized
        public ImageElement AddTextCentred(Point poscentrehorz, Size max, string label, Font fnt, Color c, Color backcolour, float backscale, Object tag = null, string tiptext = null)
        {
            ImageElement lab = new ImageElement();
            lab.TextCentreAutosize(poscentrehorz, max, label, fnt, c, backcolour, backscale, tag, tiptext);
            elements.Add(lab);
            return lab;
        }

        public ImageElement AddImage(Rectangle p, Image img, Object tag = null, string tiptext = null, bool imgowned = true)    // make sure pushes it in..
        {
            ImageElement lab = new ImageElement();
            lab.Image(p,img,tag,tiptext,imgowned);
            elements.Add(lab);
            return lab;
        }

        public void ClearImageList()        // clears the element list, not the image.  call render to do this
        {
            if (elements != null && elements.Count >= 1)
            {
                foreach (var e in elements)
                {
                    if (e.imgowned)
                    {
                        e.img?.Dispose();
                    }
                    e.img = null;
                    if (e.altimgowned)
                    {
                        e.altimg?.Dispose();
                    }
                    e.altimg = null;
                    e.tag = null;
                }
                elements.Clear();
            }
        }

        public Size DisplaySize()
        {
            int maxh = 0, maxw = 0;
            foreach (ImageElement i in elements)
            {
                if (i.pos.X + i.pos.Width > maxw)
                    maxw = i.pos.X + i.pos.Width;
                if (i.pos.Y + i.pos.Height > maxh)
                    maxh = i.pos.Y + i.pos.Height;
            }

            return new Size(maxw, maxh);
        }

        // taking image elements, draw to main bitmap. set if resize control, and if we have a min size of bitmap, or a margin
        public void Render( bool resizecontrol = true , Size? minsize = null , Size? margin = null )          
        {
            Size max = DisplaySize();
            Image?.Dispose();
            Image = null;
            if (max.Width > 0 && max.Height > 0 ) // will be zero if no elements
            {
                elementin = null;

                if (minsize.HasValue)           // minimum map size
                {
                    max.Width = Math.Min(max.Width, minsize.Value.Width);
                    max.Height = Math.Min(max.Height, minsize.Value.Height);
                }

                if (margin.HasValue)            // and any margin to allow for control growth
                {
                    max.Width += margin.Value.Width;
                    max.Height += margin.Value.Height;
                }

                Bitmap newrender = new Bitmap(max.Width, max.Height);   // size bitmap to contents

                if (!FillColor.IsFullyTransparent())
                {
                    BaseUtils.BitMapHelpers.FillBitmap(newrender, FillColor);
                }

                using (Graphics gr = Graphics.FromImage(newrender))
                {
                    foreach (ImageElement i in elements)
                    {
                        gr.DrawImage(i.img, i.pos);
                    }
                }

                Image = newrender;      // and replace the image

                if (resizecontrol)
                    this.Size = new Size(max.Width, max.Height);
            }
            else
                Image = null;       // nothing, null image
        }

        public void SwapToAlternateImage(ImageElement i)
        {
            if (i.SwapImages(Image))
                Invalidate();
        }

        public void LeaveCurrentElement()
        {
            if ( elementin != null )
            {
                if (elementin.altimg != null && elementin.mouseover && elementin.inaltimg)
                {
                    elementin.SwapImages(Image);
                    Invalidate();
                }

                elementin = null;
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                EnterElement = LeaveElement = ClickElement = null;
                ClearImageList();
                elements = null;
                ClearHoverTip();
                hovertimer.Dispose();
                hovertimer = null;
            }
            
            base.Dispose(disposing);
        }

        protected override void OnMouseMove(MouseEventArgs eventargs)
        {
            base.OnMouseMove(eventargs);

            if (elementin != null && !elementin.pos.Contains(eventargs.Location))       // go out..
            {
                LeaveCurrentElement();
                if (LeaveElement != null)
                    LeaveElement(this, eventargs, elementin, elementin.tag);
            }

            if (elementin == null)      // is in?
            {
                foreach (ImageElement i in elements)
                {
                    if (i.pos.Contains(eventargs.Location))
                    {
                        elementin = i;

                        //System.Diagnostics.Debug.WriteLine("Enter element " + elements.FindIndex(x=>x==i));

                        if (elementin.altimg != null && elementin.mouseover && !elementin.inaltimg)
                        { 
                            elementin.SwapImages(Image);
                            Invalidate();
                        }

                        if (EnterElement != null)
                            EnterElement(this, eventargs, elementin, elementin.tag );
                    }
                }
            }

            if (Math.Abs(eventargs.X - hoverpos.X) + Math.Abs(eventargs.Y - hoverpos.Y) > 8 || elementin == null)
            {
                ClearHoverTip();
            }

            if ( elementin != null && !hovertimer.Enabled && hovertip == null)
            {
                hoverpos = eventargs.Location;
                hovertimer.Start();
            }
        }

        void ClearHoverTip()
        {
            hovertimer.Stop();

            if (hovertip != null)
            {
                hovertip.Dispose();
                hovertip = null;
            }
        }

        void HoverEnd(object sender, EventArgs e)
        {
            hovertimer.Stop();

            if (elementin != null && elementin.tooltip != null && elementin.tooltip.Length>0)
            {
                hovertip = new ToolTip();

                hovertip.InitialDelay = 0;
                hovertip.AutoPopDelay = 30000;
                hovertip.ReshowDelay = 0;
                hovertip.IsBalloon = true;
                hovertip.ShowAlways = true;
                hovertip.SetToolTip(this, elementin.tooltip);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            Focus();

            ClearHoverTip();

            if (ClickElement != null)                   
                ClickElement(this, e , elementin, elementin?.tag);          // null if no element clicked
        }

    }
}

