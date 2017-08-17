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
    public class NumericUpDownCustom : Control
    {
        // Fore not use, Back used as background of whole control (may show if Autosize=on)
        public Color TextBoxBackColor { get { return tbbackcolor; } set { tb.BackColor = tbbackcolor = value; } }
        public Color TextBoxForeColor { get { return tbforecolor; } set { tb.ForeColor = tbforecolor = value; } }
        public Color BorderColor { get { return bordercolor; } set { bordercolor = value; PerformLayout(); Invalidate(); } }
        public float BorderColorScaling { get; set; } = 0.5F;           // Popup style only
        public int SpinnerSize { get; set; } = 16;
        public bool AutoSizeTextBox { get { return autosize; } set { autosize = value; PerformLayout(); } }
        public UpDown updown = new UpDown();                            // for setting colours..
        public override string Text { get { return (tb != null) ? (tb.Text) : ""; } }

        public int Maximum { get { return max; } set { Set(curvalue, value, min); } }
        public int Minimum { get { return min; } set { Set(curvalue, max, value); } }
        public int Value { get { return curvalue; } set { Set(value, max, min); } }

        #region Events

        public event EventHandler ValueChanged
        {
            add { Events.AddHandler(EVENT_VALUECHANGED, value); }
            remove { Events.RemoveHandler(EVENT_VALUECHANGED, value); }
        }

        #endregion

        #region Implementation

        public NumericUpDownCustom() : base()
        {
            tb = new TextBox();
            tb.BorderStyle = BorderStyle.None;
            tb.BackColor = this.TextBoxBackColor;
            tb.ForeColor = this.TextBoxForeColor;
            Controls.Add(tb);
            tb.TextChanged += WhenTextChanged;
            tb.Text = Value.ToString();
            Controls.Add(updown);
            updown.Selected += OnUpDown;
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            tb.AutoSize = autosize;

            if (ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
            {
                int bordersize = (!BorderColor.IsFullyTransparent()) ? 3 : 0;

                tb.Location = new Point(bordersize, bordersize);
                tb.Size = new Size(ClientRectangle.Width - SpinnerSize - bordersize * 2, ClientRectangle.Height - bordersize * 2);

                updown.Location = new Point(ClientRectangle.Width - SpinnerSize - bordersize, bordersize);
                updown.Size = new Size(SpinnerSize, tb.Height);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!BorderColor.IsFullyTransparent())
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                Color color1 = BorderColor;
                Color color2 = BorderColor.Multiply(BorderColorScaling);

                int hg = tb.Height + 6;
                using (GraphicsPath g1 = ControlHelpersStaticFunc.RectCutCorners(1, 1, ClientRectangle.Width - 2, hg - 1, 1, 1))
                using (Pen pc1 = new Pen(color1, 1.0F))
                    e.Graphics.DrawPath(pc1, g1);

                using (GraphicsPath g2 = ControlHelpersStaticFunc.RectCutCorners(0, 0, ClientRectangle.Width, hg - 1, 2, 2))
                using (Pen pc2 = new Pen(color2, 1.0F))
                    e.Graphics.DrawPath(pc2, g2);
            }
        }


        bool allowspecialkey = false;
        private void WhenKeyDown(object sender, KeyEventArgs e)
        {
            allowspecialkey = (e.KeyCode == Keys.Back);
        }

        private void WhenTextChanged(object sender, EventArgs e)
        {
            if ( ignorechange )         // because fixing text causes another fire of this
                return;

           // Console.WriteLine("Text now " + tb.Text + " curvalue " + curvalue );

            bool specialcase = (tb.Text.Length == 0) || (tb.Text.Equals("-") && min < 0);

            if (!specialcase)
            {
                int oldvalue = curvalue;
                int newvalue;
                if (int.TryParse(tb.Text, out newvalue))
                {
                    if (newvalue >= min && newvalue <= max)
                    {
                        curvalue = newvalue;
                    }
                }

                ignorechange = true;
                tb.Text = curvalue.ToString();
                tb.Select(tb.Text.Length, 0);     // move caret to end 
                ignorechange = false;

                if (oldvalue != curvalue)
                    OnValueChanged(new EventArgs());
            }
        }

        protected void OnUpDown(object sender, MouseEventArgs me)   // up down triggered, delta tells you which way
        {
            int oldvalue = curvalue;
            curvalue += me.Delta;
            curvalue = Math.Max(Math.Min(curvalue, max), min);        // limit in
            ignorechange = true;
            tb.Text = curvalue.ToString();
            ignorechange = false;
            if (oldvalue != curvalue)
                OnValueChanged(new EventArgs());
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[EVENT_VALUECHANGED];
            if (handler != null) handler(this, e);
        }

        private void Set(int cv, int mx, int mi )
        {
            int oldvalue = curvalue;
            max = mx;
            min = mi;
            curvalue = Math.Max(Math.Min(cv, mx), mi);        // limit in
            tb.Text = cv.ToString();
            if (oldvalue != curvalue)
                OnValueChanged(new EventArgs());
        }

        #endregion

        private TextBox tb;
        private bool autosize = true;
        private int max = 100;
        private int min = 0;
        private int curvalue = 0;
        private bool ignorechange = false;
        private static readonly object EVENT_SCROLL = new object();
        private static readonly object EVENT_VALUECHANGED = new object();
        Color tbbackcolor = SystemColors.Window;
        Color tbforecolor = SystemColors.WindowText;
        Color bordercolor = Color.Transparent;
    }
}
