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

namespace ExtendedControls
{
    public class RichTextBoxScroll : Panel
    {
        #region Public interfaces

        #region ctors

        public RichTextBoxScroll() : base()
        {
            TextBox.ScrollBars = RichTextBoxScrollBars.None;
            TextBox.BorderStyle = BorderStyle.None;
            TextBox.BackColor = BackColor;
            TextBox.ForeColor = ForeColor;
            TextBox.Show();
            Controls.Add(TextBox);

            ScrollBar.Show();
            Controls.Add(ScrollBar);

            TextBox.VScroll += TextBox_VScroll;
            TextBox.MouseWheel += TextBox_MouseWheel;       // richtextbox without scroll bars do not handle mouse wheels
            TextBox.TextChanged += TextBox_TextChanged;
            ScrollBar.Scroll += ScrollBar_Scroll;
        }

        #endregion // ctors

        #region public class RichTextBoxBack : RichTextBox

        public class RichTextBoxBack : RichTextBox
        {
            public IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
            {
                Message message = Message.Create(Handle, msg, wparam, lparam);
                WndProc(ref message);
                return message.Result;
            }
        }

        #endregion // public class RichTextBoxBack : RichTextBox

        #region Properties

        public VScrollBarCustom ScrollBar { get; } = new VScrollBarCustom();    // ✓ (this.Controls) Use these with caution.

        public RichTextBoxBack TextBox { get; } = new RichTextBoxBack();        // ✓ (this.Controls)


        // BackColor is the colour of the panel. 
        // if BorderColor is set, BackColor gets shown, with BorderColor on top.
        // BorderStyle is also applied by windows around the control, set to None for BorderColor only

        public Color BorderColor { get; set; } = Color.Transparent;
        public float BorderColorScaling { get; set; } = 0.5F;           // Popup style only
        public int ScrollBarWidth { get; set; } = 20;
        public bool ShowLineCount { get; set; } = false;                // count lines
        public bool HideScrollBar { get; set; } = true;                   // hide if no scroll needed

        public override string Text { get { return TextBox.Text; } set { TextBox.Text = value; UpdateScrollBar(); } }                // return only textbox text
        public int LineCount { get { return TextBox.GetLineFromCharIndex(TextBox.Text.Length) + 1; } }

        #endregion // Properties

        #region Events

        public event EventHandler TextBoxChanged;       // ✓

        #endregion // Events

        #region Methods

        public void Clear()
        {
            TextBox.Clear();
            PerformLayout();
        }

        public void AppendText(string s)
        {
            if (ShowLineCount)
            {
                s = lc + ":" + s;
                lc++;
            }
            TextBox.AppendText(s);
            TextBox.ScrollToCaret();
            UpdateScrollBar();
        }

        public void AppendText(string s, Color c)
        {
            if (ShowLineCount)
            {
                s = lc + ":" + s;
                lc++;
            }

            TextBox.SelectionStart = TextBox.TextLength;
            TextBox.SelectionLength = 0;
            TextBox.SelectionColor = c;
            TextBox.AppendText(s);
            TextBox.SelectionColor = TextBox.ForeColor;
            TextBox.SelectionStart = TextBox.TextLength;
            TextBox.SelectionLength = 0;
            TextBox.ScrollToCaret();
            UpdateScrollBar();
        }

        public void CopyFrom(RichTextBoxScroll other)
        {
            TextBox.Rtf = other.TextBox.Rtf;
        }

        public double GetRealFontHeight()
        {
            int h = FontHeight;
            if (h > 16)     // FUDGE - seems to show 1 below. measured.
                h++;
            return h;
        }

        public int EstimateLinesInBox(int height)
        {
            int lines = (int)(height / GetRealFontHeight());
            //System.Diagnostics.Debug.WriteLine("Est Lines " + lines + " on " + height + " on " + FontHeight);
            return lines;
        }

        public int EstimateVerticalSizeFromText()
        {
            int numberlines = TextBox.Lines.Count();
            int bordersize = (!BorderColor.IsFullyTransparent()) ? 3 : 0;
            double fonth = GetRealFontHeight();
            int pixels = (int)(fonth * numberlines) + bordersize * 2 + 4;      // 4 extra for border area of this (bounds-client rect)
            //System.Diagnostics.Debug.WriteLine("Est Box " + numberlines + " " + bordersize + " " + fonth + " " + pixels + " " );
            return pixels;
        }

        #endregion // Methods

        #endregion // Public interfaces


        #region Implementation

        #region Fields

        private int lc = 1;
        private bool visibleonlayout = false;

        #endregion // Fields

        #region OnEvent overrides & event dispatchers

        protected override void OnGotFocus(EventArgs e)             // Focus on us is given to the text box.
        {
            base.OnGotFocus(e);
            TextBox.Focus();
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            int bordersize = (!BorderColor.IsFullyTransparent()) ? 3 : 0;

            int textboxclienth = ClientRectangle.Height - bordersize * 2;       // border is within Client area
            int linesinbox = EstimateLinesInBox(textboxclienth);

            int firstVisibleLine = unchecked((int)(long)TextBox.SendMessage(Win32Constants.EM.GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero));
            ScrollBar.SetValueMaximumLargeChange(firstVisibleLine, LineCount - 1, linesinbox);

            visibleonlayout = ScrollBar.IsScrollBarOn || DesignMode || !HideScrollBar;  // Hide must be on, or in design mode, or scroll bar is on due to values

            TextBox.Location = new Point(bordersize, bordersize);
            TextBox.Size = new Size(ClientRectangle.Width - (visibleonlayout ? ScrollBarWidth : 0) - bordersize * 2, textboxclienth);

            //System.Diagnostics.Debug.WriteLine("text box size " + textboxclienth + " Lines " + linesinbox );

            ScrollBar.Location = new Point(ClientRectangle.Width - ScrollBarWidth - bordersize, bordersize);
            ScrollBar.Size = new Size(ScrollBarWidth, textboxclienth);

            //System.Diagnostics.Debug.WriteLine("layout Scroll State Lines: " + LineCount + " FVL: " + firstVisibleLine + " textlines " + textboxlinesestimate);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!BorderColor.IsFullyTransparent())
            {
                e.Graphics.SmoothingMode = SmoothingMode.Default;

                Color color1 = BorderColor;
                Color color2 = BorderColor.Multiply(BorderColorScaling);

                using (GraphicsPath g1 = RectCutCorners(1, 1, ClientRectangle.Width - 2, ClientRectangle.Height - 1, 1, 1))
                using (Pen pc1 = new Pen(color1, 1.0F))
                    e.Graphics.DrawPath(pc1, g1);

                using (GraphicsPath g2 = RectCutCorners(0, 0, ClientRectangle.Width, ClientRectangle.Height - 1, 2, 2))
                using (Pen pc2 = new Pen(color2, 1.0F))
                    e.Graphics.DrawPath(pc2, g2);
            }
        }

        //protected override void OnResize(EventArgs eventargs)
        //{
        //    base.OnResize(eventargs);
        //    System.Diagnostics.Debug.WriteLine("Resize" + Size);
        //}


        protected virtual void OnTextBoxChanged(EventArgs args)
        {
            TextBoxChanged?.Invoke(this, args);
        }

        #endregion // OnEvent overrides & event dispatchers

        #region Methods

        protected override void Dispose(bool disposing)
        {
            TextBoxChanged = null;
            base.Dispose(disposing);
        }


        private GraphicsPath RectCutCorners(int x, int y, int width, int height, int roundnessleft, int roundnessright )
        {
            GraphicsPath gr = new GraphicsPath();

            gr.AddLine(x + roundnessleft, y, x + width - 1 - roundnessright, y);
            gr.AddLine(x + width - 1, y + roundnessright, x + width - 1, y + height - 1 - roundnessright);
            gr.AddLine(x + width - 1 - roundnessright, y + height - 1, x + roundnessleft, y + height - 1);
            gr.AddLine(x, y + height - 1 - roundnessleft, x, y + roundnessleft);
            gr.AddLine(x, y + roundnessleft, x + roundnessleft, y);         // close figure manually, closing it with a break does not seem to work
            return gr;
        }

        private void ScrollToBar()              // from the scrollbar, scroll first line to value
        {
            int firstVisibleLine = unchecked((int)(long)TextBox.SendMessage(Win32Constants.EM.GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero));
            int scrollvalue = ScrollBar.Value;
            int delta = scrollvalue - firstVisibleLine;

            //Console.WriteLine("Scroll Bar:" + scrollvalue + " FVL: " + firstVisibleLine + " delta " + delta);
            if (delta != 0)
            {
                TextBox.SendMessage(Win32Constants.EM.LINESCROLL, IntPtr.Zero, (IntPtr)(scrollvalue - firstVisibleLine));
            }
        }

        private void UpdateScrollBar()            // from the richtext, set the scroll bar
        {
            int bordersize = (!BorderColor.IsFullyTransparent()) ? 3 : 0;
            int textboxclienth = ClientRectangle.Height - bordersize * 2;
            int linesinbox = EstimateLinesInBox(textboxclienth);

            int firstVisibleLine = unchecked((int)(long)TextBox.SendMessage(Win32Constants.EM.GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero));

            //System.Diagnostics.Debug.WriteLine("Scroll State Lines: " + LineCount+ " FVL: " + firstVisibleLine + " textlines " + textboxlinesestimate);

            ScrollBar.SetValueMaximumLargeChange(firstVisibleLine, LineCount - 1, linesinbox );

            if (ScrollBar.IsScrollBarOn != visibleonlayout)     // need to relayout if scroll bars pop on
                PerformLayout();
        }

        #endregion // Methods

        #region Event handlers

        private void ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            ScrollToBar();
        }

        private void TextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                ScrollBar.ValueLimited--;                  // control takes care of end limits..
            else
                ScrollBar.ValueLimited++;           // end is UserLimit, not maximum

            ScrollToBar();                          // go to scroll position
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (TextBoxChanged != null)
                TextBoxChanged(this, new EventArgs());
        }

        private void TextBox_VScroll(object sender, EventArgs e) // comes from TextBox, update scroll..
        {
            UpdateScrollBar();
        }

        #endregion // Event handlers

        #endregion // Implementation
    }
}
