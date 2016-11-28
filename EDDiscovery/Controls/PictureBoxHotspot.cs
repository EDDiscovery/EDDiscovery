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
            public ImageElement(Rectangle p, Image i, Object t=  null, string tt = null )
            {
                pos = p; img = i; tag = t; tooltip = tt;
            }

            public ImageElement(Graphics gr , Point poscentrehorz, string text, Font dp , Color c, Object t = null , string tt = null)
            {
                img = ControlHelpers.DrawTextIntoAutoSizedBitmap(text, dp, c);
                pos = new Rectangle(poscentrehorz.X - img.Width/2, poscentrehorz.Y, img.Width, img.Height);
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
        }

        public delegate void OnElement(object sender, ImageElement i, object tag);
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

        public void AddRange(List<ImageElement> list)
        {
            elements.AddRange(list);
        }

        public void Clear()
        {
            elements.Clear();
            Image = null;
        }

        public void Render( bool resizecontrol = true )         // taking image elements, draw to main bitmap
        {
            int maxh = 0, maxw = 0;
            foreach( ImageElement i in elements)
            {
                if (i.pos.X + i.pos.Width > maxw)
                    maxw = i.pos.X + i.pos.Width;
                if (i.pos.Y + i.pos.Height > maxh)
                    maxh = i.pos.Y + i.pos.Height;
            }

            Image = new Bitmap(maxw,maxh);                      // size bitmap to contents

            if ( resizecontrol )
                this.Size = new Size(maxw, maxh);

            using (Graphics gr = Graphics.FromImage(Image))
            {
                foreach (ImageElement i in elements)
                {
                    gr.DrawImage(i.img, i.pos);
                }
            }
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
                        if (EnterElement != null)
                            EnterElement(this, elementin, elementin.tag);
                    }
                }
            }
            else
            {
                if (!elementin.pos.Contains(eventargs.Location))
                {
                    if (LeaveElement != null)
                        LeaveElement(this, elementin, elementin.tag);

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

            if (elementin != null)
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
                ClickElement(this, elementin, elementin?.tag);          // null if no element clicked
        }

    }
}

