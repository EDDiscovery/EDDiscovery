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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ExtendedControls
{
    public class VScrollBarCustom : Control
    {
        // BackColor = control back colour
        // ForeColor = button arrow color

        // determine style, System, Popup, Flat..
        public FlatStyle FlatStyle { get { return flatstyle; } set { ChangeFlatStyle(value); } }
        public Color BorderColor { get; set; } = Color.White;
        public Color SliderColor { get; set; } = Color.DarkGray;

        public Color ArrowButtonColor { get; set; } = Color.LightGray;
        public Color ArrowBorderColor { get; set; } = Color.LightBlue;
        public float ArrowUpDrawAngle { get; set; } = 90F;                  // Popup
        public float ArrowDownDrawAngle { get; set; } = 270F;               // Popup
        public float ArrowColorScaling { get; set; } = 0.5F;                // Popup

        public Color ThumbButtonColor { get; set; } = Color.DarkBlue;
        public Color ThumbBorderColor { get; set; } = Color.Yellow;
        public float ThumbDrawAngle { get; set; } = 0F;                     // Popup
        public float ThumbColorScaling { get; set; } = 0.5F;                // Popup

        public Color MouseOverButtonColor { get; set; } = Color.Green;
        public Color MousePressedButtonColor { get; set; } = Color.Red;

        public bool HideScrollBar { get; set; } = false;                   // hide if no scroll needed
        public bool IsScrollBarOn { get { return thumbenable; } }           // is it on?

        public int Value { get { return thumbvalue; } set { SetValues(value, maximum, minimum, largechange, smallchange); } }
        public int ValueLimited { get { return thumbvalue; } set { SetValues(value, maximum, minimum, largechange, smallchange,true); } }
        public int Maximum { get { return maximum; } set { SetValues(thumbvalue, value, minimum, largechange, smallchange); } }
        public int Minimum { get { return minimum; } set { SetValues(thumbvalue, maximum, value, largechange, smallchange); } }
        public int LargeChange { get { return largechange; } set { SetValues(thumbvalue, maximum, minimum, value, smallchange); } }
        public int SmallChange { get { return smallchange; } set { SetValues(thumbvalue, maximum, minimum, largechange, value); } }
        public void SetValueMaximum(int v, int m) { SetValues(v, m, minimum, largechange, smallchange); }
        public void SetValueMaximumLargeChange(int v, int m, int lc) { SetValues(v, m, minimum, lc, smallchange); }

        #region Events
        public event ScrollEventHandler Scroll
        {
            add{Events.AddHandler(EVENT_SCROLL, value);}
            remove{Events.RemoveHandler(EVENT_SCROLL, value);}
        }

        public event EventHandler ValueChanged
        {
            add {Events.AddHandler(EVENT_VALUECHANGED, value);}
            remove { Events.RemoveHandler(EVENT_VALUECHANGED, value); }
        }

        #endregion

        #region Implementation

        public VScrollBarCustom() : base()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer,true);
            repeatclick = new Timer();
            repeatclick.Tick += new EventHandler(RepeatClick);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if ( !thumbenable && HideScrollBar && !DesignMode )
            {
                return;
            }

            if (FlatStyle == FlatStyle.System && ScrollBarRenderer.IsSupported)
            {
                ScrollBarArrowButtonState up = (mousepressed == MouseOver.MouseOverUp) ? ScrollBarArrowButtonState.UpPressed : (mouseover == MouseOver.MouseOverUp) ? ScrollBarArrowButtonState.UpHot : ScrollBarArrowButtonState.UpNormal;
                ScrollBarArrowButtonState down = (mousepressed == MouseOver.MouseOverDown) ? ScrollBarArrowButtonState.DownPressed : (mouseover == MouseOver.MouseOverUp) ? ScrollBarArrowButtonState.DownHot : ScrollBarArrowButtonState.DownNormal;
                ScrollBarState thumb = (mousepressed == MouseOver.MouseOverThumb) ? ScrollBarState.Pressed : (mouseover == MouseOver.MouseOverThumb) ? ScrollBarState.Hot : ScrollBarState.Normal;
                ScrollBarState track = ScrollBarState.Normal;

                if (!Enabled || !thumbenable)
                {
                    up = ScrollBarArrowButtonState.UpDisabled;
                    down = ScrollBarArrowButtonState.DownDisabled;
                    thumb = ScrollBarState.Disabled;
                    track = ScrollBarState.Disabled;
                }

                ScrollBarRenderer.DrawArrowButton(e.Graphics, upbuttonarea, up);
                ScrollBarRenderer.DrawArrowButton(e.Graphics, downbuttonarea, down);

                if (Enabled && thumbenable)
                {
                    Rectangle upper = new Rectangle(sliderarea.X, sliderarea.Y, sliderarea.Width, thumbbuttonarea.Y - sliderarea.Y );
                    Rectangle lower = new Rectangle(sliderarea.X, thumbbuttonarea.Bottom , sliderarea.Width, sliderarea.Bottom - thumbbuttonarea.Bottom);
                    //Console.WriteLine("System " + upper + " l: " + lower);
                    ScrollBarRenderer.DrawUpperVerticalTrack(e.Graphics, upper, track);
                    ScrollBarRenderer.DrawLowerVerticalTrack(e.Graphics, lower, track);
                    ScrollBarRenderer.DrawVerticalThumb(e.Graphics, thumbbuttonarea, thumb);
                }
                else
                    ScrollBarRenderer.DrawUpperVerticalTrack(e.Graphics, sliderarea, ScrollBarState.Disabled);
            }
            else
            {
                //Console.WriteLine("Draw " + Name + " slider " + SliderColor + " border " + BorderColor);
                using (Brush br = new SolidBrush(this.SliderColor))
                    e.Graphics.FillRectangle(br, sliderarea);
                using (Pen pr = new Pen(BorderColor))
                    e.Graphics.DrawRectangle(pr, borderrect);

                DrawButton(e.Graphics, upbuttonarea, MouseOver.MouseOverUp);
                DrawButton(e.Graphics, downbuttonarea, MouseOver.MouseOverDown);
                DrawButton(e.Graphics, thumbbuttonarea, MouseOver.MouseOverThumb);
            }
        }

        private void ChangeFlatStyle(FlatStyle v)
        {
            flatstyle = v;
            PerformLayout();
            Invalidate();
        }

        private void DrawButton(Graphics g, Rectangle rect, MouseOver but)
        {
            if (rect.Height < 4 || rect.Width < 4)
                return;

            bool isthumb = (but == MouseOver.MouseOverThumb);
            Color c1, c2;
            float angle;

            if ( isthumb )
            {
                if (!thumbenable)
                    return;

                c1 = (mousepressed == but) ? MousePressedButtonColor : ((mouseover == but) ? MouseOverButtonColor : ThumbButtonColor);
                c2 = (FlatStyle == FlatStyle.Popup) ? c1.Multiply(ThumbColorScaling) : c1;
                angle = ThumbDrawAngle;
            }
            else
            {
                c1 = (mousepressed == but) ? MousePressedButtonColor : ((mouseover == but) ? MouseOverButtonColor : ArrowButtonColor);
                c2 = (FlatStyle == FlatStyle.Popup) ? c1.Multiply(ArrowColorScaling) : c1;
                angle = (but == MouseOver.MouseOverUp) ? ArrowUpDrawAngle : ArrowDownDrawAngle;
            }

            using (Brush bbck = new LinearGradientBrush(rect, c1, c2, angle))
                g.FillRectangle(bbck, rect);

            if (Enabled && thumbenable && !isthumb)
            {
                int hoffset = rect.Width / 3;
                int voffset = rect.Height / 3;
                Point arrowpt1 = new Point(rect.X + hoffset, rect.Y + voffset);
                Point arrowpt2 = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height - voffset);
                Point arrowpt3 = new Point(rect.X + rect.Width - hoffset, arrowpt1.Y);

                Point arrowpt1c = new Point(arrowpt1.X, arrowpt2.Y);
                Point arrowpt2c = new Point(arrowpt2.X, arrowpt1.Y);
                Point arrowpt3c = new Point(arrowpt3.X, arrowpt2.Y);

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (Pen p2 = new Pen(ForeColor))
                {
                    if (but == MouseOver.MouseOverDown)
                    {
                        g.DrawLine(p2, arrowpt1, arrowpt2);            // the arrow!
                        g.DrawLine(p2, arrowpt2, arrowpt3);
                    }
                    else
                    {
                        g.DrawLine(p2, arrowpt1c, arrowpt2c);            // the arrow!
                        g.DrawLine(p2, arrowpt2c, arrowpt3c);
                    }
                }

            }

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            if (but == mouseover || isthumb)
            {
                using (Pen p = new Pen(isthumb ? ThumbBorderColor : ArrowBorderColor))
                {
                    Rectangle border = rect;
                    border.Width--; border.Height--;
                    g.DrawRectangle(p, border);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!Enabled || !thumbenable)
                return;

            if ( thumbmove )                        // if moving thumb, we calculate where we are in value
            {
                int voffset = e.Location.Y - (sliderarea.Y+ thumbmovecaptureoffset);
                //Console.WriteLine("Voffset " + voffset);
                int sliderrangepx = sliderarea.Height - thumbbuttonarea.Height;      // range of values to represent Min-Max.
                voffset = Math.Min(Math.Max(voffset, 0), sliderrangepx);        // bound within slider range
                float percent = (float)voffset / (float)sliderrangepx;          // % in
                int newthumbvalue = minimum + (int)((float)(UserMaximum - minimum) * percent);
                //Console.WriteLine("Slider px" + voffset + " to value " + newthumbvalue);

                if (newthumbvalue != thumbvalue)        // and if changed, apply it.
                {
                    thumbvalue = newthumbvalue;
                    OnScroll(new ScrollEventArgs((newthumbvalue<thumbvalue) ? ScrollEventType.SmallDecrement : ScrollEventType.SmallIncrement, thumbvalue, newthumbvalue, ScrollOrientation.VerticalScroll));
                    OnValueChanged(new EventArgs());
                    CalculateThumb();
                    Invalidate();
                }
            }
            else if (upbuttonarea.Contains(e.Location))
            {
                if (mouseover != MouseOver.MouseOverUp)
                {
                    mouseover = MouseOver.MouseOverUp;
                    Invalidate();
                }
            }
            else if (downbuttonarea.Contains(e.Location))
            {
                if (mouseover != MouseOver.MouseOverDown)
                {
                    mouseover = MouseOver.MouseOverDown;
                    Invalidate();
                }
            }
            else if (thumbbuttonarea.Contains(e.Location))
            {
                if (mouseover != MouseOver.MouseOverThumb)
                {
                    mouseover = MouseOver.MouseOverThumb;
                    Invalidate();
                }
            }
            else if (mouseover != MouseOver.MouseOverNone)
            {
                mouseover = MouseOver.MouseOverNone;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!Enabled || !thumbenable)
                return;

            if (upbuttonarea.Contains(e.Location))
            {
                mousepressed = MouseOver.MouseOverUp;
                Invalidate();
                StartRepeatClick(e);
                MoveThumb(-smallchange);
            }
            else if (downbuttonarea.Contains(e.Location))
            {
                mousepressed = MouseOver.MouseOverDown;
                Invalidate();
                StartRepeatClick(e);
                MoveThumb(smallchange);
            }
            else if (thumbbuttonarea.Contains(e.Location))
            {
                mousepressed = MouseOver.MouseOverThumb;
                Invalidate();
                thumbmove = true;                           // and mouseover should be on as well
                thumbmovecaptureoffset = e.Location.Y - thumbbuttonarea.Y;      // pixels down the thumb when captured..
                //Console.WriteLine("Thumb captured at " + thumbmovecaptureoffset);
            }
            else if (sliderarea.Contains(e.Location))      // slider, but not thumb..
                MoveThumb((e.Location.Y < thumbbuttonarea.Y) ? -largechange : largechange);

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if ( mousepressed != MouseOver.MouseOverNone)
            {
                mousepressed = MouseOver.MouseOverNone;
                Invalidate();
            }

            if (thumbmove)
            {
                thumbmove = false;
                Invalidate();
            }

            repeatclick.Stop();

            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!thumbmove && mouseover != MouseOver.MouseOverNone)
            {
                mouseover = MouseOver.MouseOverNone;
                Invalidate();
            }
        }

        private void MoveThumb(int vchange)
        {
            int oldvalue = thumbvalue;

            if (vchange < 0 && thumbvalue > minimum)
            {
                thumbvalue += vchange;
                thumbvalue = Math.Max(thumbvalue, minimum);
                OnValueChanged(new EventArgs());
                OnScroll(new ScrollEventArgs(ScrollEventType.SmallDecrement, oldvalue, Value, ScrollOrientation.VerticalScroll));
                CalculateThumb();
                Invalidate();
            }
            else if (vchange > 0 && thumbvalue < UserMaximum)
            {
                thumbvalue += vchange;
                thumbvalue = Math.Min(thumbvalue, UserMaximum);
                OnValueChanged(new EventArgs());
                OnScroll(new ScrollEventArgs(ScrollEventType.SmallIncrement, oldvalue, Value, ScrollOrientation.VerticalScroll));
                CalculateThumb();
                Invalidate();
            }

            //Console.WriteLine("Slider is " + thumbvalue + " from " + minimum + " to " + maximum);
        }

        private void SetValues(int v, int max, int min , int lc, int sc , bool limittousermax = false )   // this allows it to be set to maximum..
        {
            smallchange = sc;                                   // has no effect on display of control
            bool iv = false;

            if (max != maximum || min != minimum || lc != largechange) // these do..
            {           // only invalidate if actually changed something
                maximum = max;
                minimum = min;
                largechange = lc;
                iv = true;
            }

            int newthumbvalue = Math.Min(Math.Max(v, minimum), maximum);

            if ( limittousermax)
                newthumbvalue = Math.Min(newthumbvalue, UserMaximum);

            if (newthumbvalue != thumbvalue)        // if changed..
            {
                OnValueChanged(new EventArgs());
                thumbvalue = newthumbvalue;
                iv = true;
            }

            if ( iv )
            {
                CalculateThumb();
                Invalidate();
            }
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            sliderarea = ClientRectangle;

            if ( FlatStyle != FlatStyle.System )                // no border in system..
                sliderarea.Inflate(-1, -1);

            int scrollheight = sliderarea.Width;
            if (scrollheight * 2 > ClientRectangle.Height / 3)  // don't take up too much of the slider with the buttons
                scrollheight = ClientRectangle.Height / 6;

            upbuttonarea = sliderarea;
            upbuttonarea.Height = scrollheight;
            downbuttonarea = sliderarea;
            downbuttonarea.Y = sliderarea.Bottom - scrollheight;
            downbuttonarea.Height = scrollheight;
            sliderarea.Y += scrollheight;
            sliderarea.Height -= 2 * scrollheight;

            borderrect = ClientRectangle;
            borderrect.Width--; borderrect.Height--;

            CalculateThumb();
        }

        private void CalculateThumb()
        {
            int userrange = maximum - minimum+1;           // number of positions..

            if (largechange < userrange)                   // largerange is less than number of individual positions
            {
                float fthumbheight = ((float)largechange / (float)userrange) * sliderarea.Height;    // less than H
                int thumbheight = (int)fthumbheight;
                if (thumbheight < sliderarea.Width)             // too small, adjust
                    thumbheight = sliderarea.Width;

                int sliderrangev = UserMaximum - minimum;       // Usermaximum will be > minimum, due to above < test.
                int lthumb = Math.Min(thumbvalue, UserMaximum);         // values beyond User maximum screened out

                float fposition = (float)(lthumb - minimum) / (float)sliderrangev;

                int sliderrangepx = sliderarea.Height - thumbheight;      // range of values to represent Min-Max.
                int thumboffsetpx = (int)((float)sliderrangepx * fposition);
                thumboffsetpx = Math.Min(thumboffsetpx, sliderrangepx);     // LIMIT, because we can go over slider range if value=maximum

                thumbbuttonarea = new Rectangle(sliderarea.X, sliderarea.Y + thumboffsetpx, sliderarea.Width, thumbheight);
                thumbenable = true;
            }
            else
            {
                thumbenable = false;                        // else disable the thumb and scroll bar
                thumbmove = false;
                mouseover = MouseOver.MouseOverNone;
                mousepressed = MouseOver.MouseOverNone;
            }
        }
                                                           // for a 0-100 scroll bar, with lc=10, positions are 0 to 91 inclusive.
        private int UserMaximum { get { return Math.Max(maximum - largechange + 1, minimum); } }    // make sure it does not go below minimum whatever largechange is set to.

        private void StartRepeatClick(MouseEventArgs e)
        {
            if (!repeatclick.Enabled)                       // if not enabled, turn it on.  Since this gets repeatedly called we need to check..
            {
                mouseargs = e;
                repeatclick.Interval = 250;
                repeatclick.Start();
            }
        }

        private void RepeatClick(object sender, EventArgs e )
        {
            repeatclick.Interval = 50;                      // resetting interval is okay when enabled
            OnMouseDown(mouseargs);
        }

        protected virtual void OnScroll(ScrollEventArgs se)
        {
            ScrollEventHandler handler = (ScrollEventHandler)Events[EVENT_SCROLL];
            if (handler != null) handler(this, se);
        }
        protected virtual void OnValueChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[EVENT_VALUECHANGED];
            if (handler != null) handler(this, e);
        }

        #endregion

        #region Variables

        enum MouseOver { MouseOverNone, MouseOverUp, MouseOverDown, MouseOverThumb };
        private MouseOver mouseover = MouseOver.MouseOverNone;
        private MouseOver mousepressed = MouseOver.MouseOverNone;
        private bool thumbmove = false;
        private bool thumbenable = true;
        private int thumbmovecaptureoffset = 0;     // px down the thumb when captured..
        private int thumbvalue = 0;
        private int maximum = 100;
        private int minimum = 0;
        private int largechange = 10;
        private int smallchange = 1;
        FlatStyle flatstyle = FlatStyle.System;
        Rectangle borderrect;
        Rectangle sliderarea;
        Rectangle upbuttonarea;
        Rectangle downbuttonarea;
        Rectangle thumbbuttonarea;
        Timer repeatclick;
        MouseEventArgs mouseargs;
        private static readonly object EVENT_SCROLL = new object();
        private static readonly object EVENT_VALUECHANGED = new object();

        #endregion
    }
}
