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

            public ImageElement(Rectangle p, Image i, Object t = null, string tt = null)
            {
                pos = p; img = i; tag = t; tooltip = tt;
            }

            public void Image(Rectangle p, Image i, Object t = null, string tt = null)
            {
                pos = p; img = i; tag = t; tooltip = tt;
            }

            // centred, autosized
            public void TextCentreAutosize(Graphics gr, Point poscentrehorz, Size max, string text, Font dp, Color c, Color backcolour, float backscale = 1.0F, Object t = null, string tt = null)
            {
                img = ControlHelpers.DrawTextIntoAutoSizedBitmap(text, max, dp, c, backcolour, backscale);
                pos = new Rectangle(poscentrehorz.X - img.Width / 2, poscentrehorz.Y, img.Width, img.Height);
                tag = t;
                tooltip = tt;
            }

            // top left, autosized
            public void TextAutosize(Graphics gr, Point topleft, Size max, string text, Font dp, Color c, Color backcolour, float backscale = 1.0F, Object t = null, string tt = null)
            {
                img = ControlHelpers.DrawTextIntoAutoSizedBitmap(text, max, dp, c, backcolour, backscale);
                pos = new Rectangle(topleft.X, topleft.Y, img.Width, img.Height);
                tag = t;
                tooltip = tt;
            }

            // top left, sized
            public void TextFixedSize(Graphics gr, Point topleft, Size size, string text, Font dp, Color c, Color backcolour, float backscale = 1.0F,
                                    Object t = null, string tt = null )
            {
                img = ControlHelpers.DrawTextIntoFixedSizeBitmap(text, size, dp, c, backcolour, backscale );
                pos = new Rectangle(topleft.X, topleft.Y, img.Width, img.Height);
                tag = t;
                tooltip = tt;
            }

            public Rectangle pos;
            public Image img;
            public Object tag;
            public string tooltip;

            public void Translate(int x,int y)
            {
                pos = new Rectangle(pos.X + x, pos.Y + y, pos.Width, pos.Height);
            }

            public void Position(int x, int y)
            {
                pos = new Rectangle(x, y, pos.Width, pos.Height);
            }
        }

        public delegate void OnElement(object sender, MouseEventArgs eventargs, ImageElement i, object tag );
        public event OnElement EnterElement;
        public event OnElement LeaveElement;
        public event OnElement ClickElement;

        private Timer hovertimer = new Timer();
        ToolTip hovertip = null;
        Point hoverpos;
        private List<ImageElement> elements = new List<ImageElement>();

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
            using (Graphics gr = CreateGraphics())
            {
                ImageElement lab = new ImageElement();
                lab.TextAutosize(gr, topleft, max, label, fnt, c, backcolour, backscale, tag, tiptext);
                elements.Add(lab);
                return lab;
            }
        }

        // topleft, sized
        public ImageElement AddTextFixedSize(Point topleft, Size size, string label, Font fnt, Color c, Color backcolour, float backscale, Object tag = null, string tiptext = null)
        {
            using (Graphics gr = CreateGraphics())
            {
                ImageElement lab = new ImageElement();
                lab.TextFixedSize(gr, topleft, size, label, fnt, c, backcolour, backscale, tag, tiptext);
                elements.Add(lab);
                return lab;
            }
        }

        // centre pos, autosized
        public ImageElement AddTextCentred(Point poscentrehorz, Size max, string label, Font fnt, Color c, Color backcolour, float backscale, Object tag = null, string tiptext = null)
        {
            using (Graphics gr = CreateGraphics())
            {
                ImageElement lab = new ImageElement();
                lab.TextCentreAutosize(gr, poscentrehorz, max, label, fnt, c, backcolour, backscale, tag, tiptext);
                elements.Add(lab);
                return lab;
            }
        }

        public ImageElement AddImage(Rectangle p, Image img , Object tag = null, string tiptext = null)
        {
            ImageElement lab = new ImageElement();
            lab.Image(p,img,tag,tiptext);
            elements.Add(lab);
            return lab;
        }

        public void ClearImageList()        // clears the element list, not the image.  call render to do this
        {
            elements.Clear();
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

        public void Render( bool resizecontrol = true )         // taking image elements, draw to main bitmap
        {
            Size max = DisplaySize();

            if (max.Width > 0 && max.Height > 0 ) // will be zero if no elements
            {
                Bitmap newrender = new Bitmap(max.Width, max.Height);   // size bitmap to contents

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


        ImageElement elementin = null;

        protected override void OnMouseMove(MouseEventArgs eventargs)
        {
            base.OnMouseMove(eventargs);

            if (elementin == null)
            {
                foreach (ImageElement i in elements)
                {
                    if (i.pos.Contains(eventargs.Location))
                    {
                        elementin = i;

                        //System.Diagnostics.Debug.WriteLine("Enter element " + elements.FindIndex(x=>x==i));

                        if (EnterElement != null)
                            EnterElement(this, eventargs, elementin, elementin.tag );
                    }
                }
            }
            else
            {
                if (!elementin.pos.Contains(eventargs.Location))
                {
                    //System.Diagnostics.Debug.WriteLine("Leave element ");

                    if (LeaveElement != null)
                        LeaveElement(this, eventargs, elementin, elementin.tag);

                    elementin = null;
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

