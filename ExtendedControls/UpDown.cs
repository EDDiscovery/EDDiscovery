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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class UpDown : Control
    {
        #region public interfaces

        #region ctors

        public UpDown()
        {
            repeatclick = new Timer(components);
            repeatclick.Tick += new EventHandler(RepeatClick_Tick);
        }

        #endregion // ctors

        #region Properties

        // Call Invalidate if you change these..
        public override Color BackColor { get; set; } = SystemColors.Control;
        public override Color ForeColor { get; set; } = SystemColors.ControlText;
        public Color MouseOverColor { get; set; } = SystemColors.ControlLight;
        public Color MouseSelectedColor { get; set; } = SystemColors.ControlLightLight;
        public Color BorderColor { get; set; } = Color.Gray;
        public float BackColorScaling { get; set; } = 0.5F;
        public float MouseSelectedColorScaling { get; set; } = 1.5F;

        #endregion // Properties

        #region Events

        public event EventHandler<MouseEventArgs> Selected;     // ✓

        #endregion // Events

        #endregion // public interfaces


        #region Implementation

        private enum MouseOver { MouseOverUp, MouseOverDown, MouseOverNone };

        #region Fields

        private IContainer components = new Container();        // ✓
        private Timer repeatclick;                              // ✓ (components)

        private MouseEventArgs mouseargs;                       // ✓

        private MouseOver mouseover = MouseOver.MouseOverNone;
        private MouseOver mousedown = MouseOver.MouseOverNone;
        private Rectangle upper;
        private Rectangle lower;

        #endregion // Fields

        #region OnEvent overrides & dispatchers

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            if (ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
            {
                int halfway = ClientRectangle.Height / 2 - 1;
                upper = new Rectangle(1, 0, ClientRectangle.Width - 2, halfway);
                lower = new Rectangle(1, halfway + 2, ClientRectangle.Width - 2, halfway);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (upper.Width == 0)
                return;

            Color pcup = (Enabled) ? ((mousedown == MouseOver.MouseOverUp) ? MouseSelectedColor : ((mouseover == MouseOver.MouseOverUp) ? MouseOverColor : this.BackColor)) : this.BackColor.Multiply(0.5F);
            Color pcdown = (Enabled) ? ((mousedown == MouseOver.MouseOverDown) ? MouseSelectedColor : ((mouseover == MouseOver.MouseOverDown) ? MouseOverColor : this.BackColor)) : this.BackColor.Multiply(0.5F);

            Rectangle area = new Rectangle(0, 0, lower.Width, lower.Height + 1); // seems to make it linear paint bettwe

            using (Brush b = new LinearGradientBrush(area, pcup, pcup.Multiply(BackColorScaling), 90))
                e.Graphics.FillRectangle(b, upper);

            using (Brush b = new LinearGradientBrush(area, pcdown, pcdown.Multiply(BackColorScaling), 270))
                e.Graphics.FillRectangle(b, lower);

            using (Pen p = new Pen(BorderColor, 1F))
            {
                int right = ClientRectangle.Width - 1;
                int bottom = ClientRectangle.Height - 1;
                e.Graphics.DrawLine(p, 0, 0, 0, bottom);
                e.Graphics.DrawLine(p, right, 0, right, bottom);
                e.Graphics.DrawLine(p, 0, upper.Height, right, upper.Height);
                e.Graphics.DrawLine(p, 0, upper.Height + 1, right, upper.Height + 1);
            }

            if (Enabled)
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (Pen p = new Pen(mousedown == MouseOver.MouseOverUp ? ForeColor.Multiply(MouseSelectedColorScaling) : ForeColor))
                {
                    int hoffset = upper.Width / 3;
                    int voffset = upper.Height / 3;

                    Point arrowpt1u = new Point(upper.X + hoffset, upper.Y + upper.Height - voffset);
                    Point arrowpt2u = new Point(upper.X + upper.Width / 2, upper.Y + voffset);
                    Point arrowpt3u = new Point(upper.X + upper.Width - hoffset, arrowpt1u.Y);
                    e.Graphics.DrawLine(p, arrowpt1u, arrowpt2u);            // the arrow!
                    e.Graphics.DrawLine(p, arrowpt2u, arrowpt3u);
                }

                using (Pen p = new Pen(mousedown == MouseOver.MouseOverDown ? ForeColor.Multiply(MouseSelectedColorScaling) : ForeColor))
                {
                    int hoffset = lower.Width / 3;
                    int voffset = lower.Height / 3;

                    Point arrowpt1d = new Point(lower.X + hoffset, lower.Y + voffset);
                    Point arrowpt2d = new Point(lower.X + lower.Width / 2, lower.Y + lower.Height - voffset);
                    Point arrowpt3d = new Point(lower.X + lower.Width - hoffset, arrowpt1d.Y);

                    e.Graphics.DrawLine(p, arrowpt1d, arrowpt2d);            // the arrow!
                    e.Graphics.DrawLine(p, arrowpt2d, arrowpt3d);

                }
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            }
        }

        protected override void OnMouseMove(MouseEventArgs eventargs)
        {
            base.OnMouseMove(eventargs);

            if (upper.Contains(eventargs.Location))
            {
                if (mouseover != MouseOver.MouseOverUp)
                {
                    mouseover = MouseOver.MouseOverUp;
                    Invalidate();
                }
            }
            else if (lower.Contains(eventargs.Location))
            {
                if (mouseover != MouseOver.MouseOverDown)
                {
                    mouseover = MouseOver.MouseOverDown;
                    Invalidate();
                }
            }
            else if (mouseover != MouseOver.MouseOverNone)
            {
                mouseover = MouseOver.MouseOverNone;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            base.OnMouseEnter(eventargs);
            mouseover = MouseOver.MouseOverNone;
            mousedown = MouseOver.MouseOverNone;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            if (upper.Contains(mevent.Location))
            {
                mousedown = MouseOver.MouseOverUp;
                Invalidate();
                MouseEventArgs me = new MouseEventArgs(mevent.Button, mevent.Clicks, mevent.X, mevent.Y, 1);
                OnSelected(me);
                StartRepeatClick(me);
            }
            else if (lower.Contains(mevent.Location))
            {
                mousedown = MouseOver.MouseOverDown;
                Invalidate();
                MouseEventArgs me = new MouseEventArgs(mevent.Button, mevent.Clicks, mevent.X, mevent.Y, -1);
                OnSelected(me);
                StartRepeatClick(me);
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            repeatclick.Stop();
            base.OnMouseDown(mevent);
            mousedown = MouseOver.MouseOverNone;
            Invalidate();
        }


        protected virtual void OnSelected(MouseEventArgs args)
        {
            Selected?.Invoke(this, args);
        }

        #endregion // OnEvent overrides & dispatchers

        #region Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            components = null;
            mouseargs = null;
            Selected = null;
            base.Dispose(disposing);
        }


        private void StartRepeatClick(MouseEventArgs e)
        {
            if (!repeatclick.Enabled)                       // if not enabled, turn it on.  Since this gets repeatedly called we need to check..
            {
                mouseargs = e;
                repeatclick.Interval = 250;
                repeatclick.Start();
            }
        }

        #endregion // Methods

        #region Event handlers

        private void RepeatClick_Tick(object sender, EventArgs e)
        {
            repeatclick.Interval = 50;                      // resetting interval is okay when enabled
            OnSelected(mouseargs);
        }

        #endregion // Event handlers

        #endregion // Implementation
    }
}
