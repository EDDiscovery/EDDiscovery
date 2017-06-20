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
        #region Public interfaces

        #region ctors

        public NumericUpDownCustom()
        {
            tb = new TextBox() { BorderStyle = BorderStyle.None, BackColor = tbbackcolor, ForeColor = tbforecolor, Text = _Value.ToString() };
            tb.TextChanged += Tb_TextChanged;
            Controls.Add(tb);

            updown.Selected += Updown_Selected;
            Controls.Add(updown);
        }

        #endregion // ctors

        #region Properties

        public UpDown updown { get; private set; } = new UpDown();      // ✓ (this.Controls) for setting colours..

        // Fore not use, Back used as background of whole control (may show if Autosize=on)
        public Color TextBoxBackColor { get { return tbbackcolor; } set { tb.BackColor = tbbackcolor = value; } }
        public Color TextBoxForeColor { get { return tbforecolor; } set { tb.ForeColor = tbforecolor = value; } }
        public Color BorderColor { get { return bordercolor; } set { bordercolor = value; PerformLayout(); Invalidate(); } }
        public float BorderColorScaling { get; set; } = 0.5F;           // Popup style only
        public int SpinnerSize { get; set; } = 16;
        public bool AutoSizeTextBox { get { return autosize; } set { autosize = value; PerformLayout(); } }
        
        public override string Text { get { return (tb != null) ? tb.Text : string.Empty; } }
        public int Maximum { get { return _Maximum; } set { Set(_Value, value, _Minimum); } }
        public int Minimum { get { return _Minimum; } set { Set(_Value, _Maximum, value); } }
        public int Value { get { return _Value; } set { Set(value, _Maximum, _Minimum); } }

        #endregion // Properties

        #region Events

        public event EventHandler<EventArgs> ValueChanged;  // ✓

        #endregion // Events

        #endregion // Public interfaces


        #region Implementation

        #region Fields

        private TextBox tb;         // ✓ (this.Controls)

        private int _Maximum = 100;
        private int _Minimum = 0;
        private int _Value = 0;
        private bool autosize = true;
        private Color tbbackcolor = SystemColors.Window;
        private Color tbforecolor = SystemColors.WindowText;
        private Color bordercolor = Color.Transparent;

        #endregion // Fields

        #region OnEvent overrides & dispatchers

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

                int hg = tb.Height + 6;
                using (GraphicsPath g1 = RectCutCorners(1, 1, ClientRectangle.Width - 2, hg - 1, 1, 1))
                using (Pen pc1 = new Pen(BorderColor, 1.0F))
                    e.Graphics.DrawPath(pc1, g1);

                using (GraphicsPath g2 = RectCutCorners(0, 0, ClientRectangle.Width, hg - 1, 2, 2))
                using (Pen pc2 = new Pen(BorderColor.Multiply(BorderColorScaling), 1.0F))
                    e.Graphics.DrawPath(pc2, g2);
            }
        }


        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        #endregion // OnEvent overrides & dispatchers

        #region Methods

        protected override void Dispose(bool disposing)
        {
            ValueChanged = null;
            base.Dispose(disposing);
        }


        private GraphicsPath RectCutCorners(int x, int y, int width, int height, int roundnessleft, int roundnessright)
        {
            GraphicsPath gr = new GraphicsPath();

            gr.AddLine(x + roundnessleft, y, x + width - 1 - roundnessright, y);
            gr.AddLine(x + width - 1, y + roundnessright, x + width - 1, y + height - 1 - roundnessright);
            gr.AddLine(x + width - 1 - roundnessright, y + height - 1, x + roundnessleft, y + height - 1);
            gr.AddLine(x, y + height - 1 - roundnessleft, x, y + roundnessleft);
            gr.AddLine(x, y + roundnessleft, x + roundnessleft, y);         // close figure manually, closing it with a break does not seem to work
            return gr;
        }

        private void Set(int cv, int mx, int mi)
        {
            int oldvalue = _Value;
            _Maximum = mx;
            _Minimum = mi;
            _Value = Math.Max(Math.Min(cv, mx), mi);        // limit in
            tb.Text = cv.ToString();
            if (oldvalue != _Value)
                OnValueChanged(new EventArgs());
        }

        #endregion // Methods

        #region Event handlers

        private void Tb_TextChanged(object sender, EventArgs e)
        {
            // Console.WriteLine("Text now " + tb.Text + " curvalue " + curvalue );

            bool specialcase = (tb.Text.Length == 0) || (tb.Text.Equals("-") && _Minimum < 0);

            if (!specialcase)
            {
                int oldvalue = _Value;
                int newvalue;
                if (int.TryParse(tb.Text, out newvalue) && newvalue != _Value && newvalue >= _Minimum && newvalue <= _Maximum)
                {
                    _Value = newvalue;
                    OnValueChanged(new EventArgs());
                }   
            }
        }

        private void Updown_Selected(object sender, MouseEventArgs e)   // up down triggered, delta tells you which way
        {
            int newvalue = Math.Max(Math.Min(_Value + e.Delta, _Maximum), _Minimum);
            if (newvalue != _Value)
            {
                _Value = newvalue;
                tb.Text = newvalue.ToString();
                OnValueChanged(EventArgs.Empty);
            }
            int oldvalue = _Value;
            _Value += e.Delta;
            _Value = Math.Max(Math.Min(_Value, _Maximum), _Minimum);        // limit in
            tb.Text = _Value.ToString();
            if (oldvalue != _Value)
                OnValueChanged(new EventArgs());
        }

        #endregion // Event handlers

        #endregion // Implementation
    }
}
