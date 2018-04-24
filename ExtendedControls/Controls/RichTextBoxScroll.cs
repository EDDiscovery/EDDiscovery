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
using BaseUtils.Win32Constants;
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
        public class RichTextBoxBack : RichTextBox
        {
            public IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
            {
                Message message = Message.Create(this.Handle, msg, wparam, lparam);
                this.WndProc(ref message);
                return message.Result;
            }
        }

        // BackColor is the colour of the panel. 
        // if BorderColor is set, BackColor gets shown, with BorderColor on top.
        // BorderStyle is also applied by windows around the control, set to None for BorderColor only

        public Color TextBoxForeColor { get { return TextBox.ForeColor; } set { TextBox.ForeColor = value; } }
        public Color TextBoxBackColor { get { return TextBox.BackColor; } set { TextBox.BackColor = value; } }
        public Color BorderColor { get; set; } = Color.Transparent;
        public float BorderColorScaling { get; set; } = 0.5F;           // Popup style only
        public int ScrollBarWidth { get; set; } = 20;
        public bool ShowLineCount { get; set; } = false;                // count lines
        public bool HideScrollBar { get; set; } = true;                   // hide if no scroll needed

        public FlatStyle ScrollBarFlatStyle { get { return ScrollBar.FlatStyle; } set { ScrollBar.FlatStyle = value; } }
        public Color ScrollBarBackColor { get { return ScrollBar.BackColor; } set { ScrollBar.BackColor = value; } }
        public Color ScrollBarSliderColor { get { return ScrollBar.SliderColor; } set { ScrollBar.SliderColor = value; } }
        public Color ScrollBarBorderColor { get { return ScrollBar.BorderColor; } set { ScrollBar.BorderColor = value; } }
        public Color ScrollBarThumbBorderColor { get { return ScrollBar.ThumbBorderColor; } set { ScrollBar.ThumbBorderColor = value; } }
        public Color ScrollBarArrowBorderColor { get { return ScrollBar.ArrowBorderColor; } set { ScrollBar.ArrowBorderColor = value; } }
        public Color ScrollBarArrowButtonColor { get { return ScrollBar.ArrowButtonColor; } set { ScrollBar.ArrowButtonColor = value; } }
        public Color ScrollBarThumbButtonColor { get { return ScrollBar.ThumbButtonColor; } set { ScrollBar.ThumbButtonColor = value; } }
        public Color ScrollBarMouseOverButtonColor { get { return ScrollBar.MouseOverButtonColor; } set { ScrollBar.MouseOverButtonColor = value; } }
        public Color ScrollBarMousePressedButtonColor { get { return ScrollBar.MousePressedButtonColor; } set { ScrollBar.MousePressedButtonColor = value; } }
        public Color ScrollBarForeColor { get { return ScrollBar.ForeColor; } set { ScrollBar.ForeColor = value; } }

        public override string Text { get { return TextBox.Text; } set { TextBox.Text = value; UpdateScrollBar(); } }                // return only textbox text
        public int LineCount { get { return TextBox.GetLineFromCharIndex(TextBox.Text.Length) + 1; } }

        public delegate void OnTextBoxChanged(object sender, EventArgs e);
        public event OnTextBoxChanged TextBoxChanged;

        #region Public Functions

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

        public void SetTabs(int[] array)
        {
            TextBox.SelectionTabs = array;
        }

        public bool ReadOnly { get { return TextBox.ReadOnly; } set { TextBox.ReadOnly = value; } }

        public string SelectedText { get { return TextBox.SelectedText; } }
        public void Select(int s, int e) { TextBox.Select(s, e); }
        public void CursorToEnd() { TextBox.Select(TextBox.TextLength, TextBox.TextLength); }
        public void ScrollToCaret() { TextBox.ScrollToCaret(); }
        public new void Focus() { TextBox.Focus(); }

        public override ContextMenuStrip ContextMenuStrip { get { return TextBox.ContextMenuStrip; } set { TextBox.ContextMenuStrip = value; } }

        #endregion

        #region Implementation

        private RichTextBoxBack TextBox;                 // Use these with caution.
        private VScrollBarCustom ScrollBar;

        public RichTextBoxScroll() : base()
        {
            TextBox = new RichTextBoxBack();
            ScrollBar = new VScrollBarCustom();
            Controls.Add(TextBox);
            Controls.Add(ScrollBar);
            TextBox.ScrollBars = RichTextBoxScrollBars.None;
            TextBox.BorderStyle = BorderStyle.None;
            TextBox.BackColor = BackColor;
            TextBox.ForeColor = ForeColor;
            TextBox.MouseUp += TextBox_MouseUp;
            TextBox.MouseDown += TextBox_MouseDown;
            TextBox.MouseMove += TextBox_MouseMove;
            TextBox.MouseEnter += TextBox_MouseEnter;
            TextBox.MouseLeave += TextBox_MouseLeave;
            TextBox.Show();
            ScrollBar.Show();
            TextBox.VScroll += OnVscrollChanged;
            TextBox.MouseWheel += new MouseEventHandler(MWheel);        // richtextbox without scroll bars do not handle mouse wheels
            TextBox.TextChanged += TextChangeEventHandler;
            ScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(OnScrollBarChanged);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!BorderColor.IsFullyTransparent())
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                Color color1 = BorderColor;
                Color color2 = BorderColor.Multiply(BorderColorScaling);

                GraphicsPath g1 = ControlHelpersStaticFunc.RectCutCorners(1, 1, ClientRectangle.Width - 2, ClientRectangle.Height - 1, 1, 1);
                using (Pen pc1 = new Pen(color1, 1.0F))
                    e.Graphics.DrawPath(pc1, g1);

                GraphicsPath g2 = ControlHelpersStaticFunc.RectCutCorners(0, 0, ClientRectangle.Width, ClientRectangle.Height - 1, 2, 2);
                using (Pen pc2 = new Pen(color2, 1.0F))
                    e.Graphics.DrawPath(pc2, g2);
            }
        }


        bool visibleonlayout = false;

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            int bordersize = (!BorderColor.IsFullyTransparent()) ? 3 : 0;

            int textboxclienth = ClientRectangle.Height - bordersize * 2;       // border is within Client area
            int linesinbox = EstimateLinesInBox(textboxclienth);

            int firstVisibleLine = unchecked((int)(long)TextBox.SendMessage(EM.GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero));
            ScrollBar.SetValueMaximumLargeChange(firstVisibleLine, LineCount - 1, linesinbox);

            visibleonlayout = ScrollBar.IsScrollBarOn || DesignMode || !HideScrollBar;  // Hide must be on, or in design mode, or scroll bar is on due to values

            TextBox.Location = new Point(bordersize, bordersize);
            TextBox.Size = new Size(ClientRectangle.Width - (visibleonlayout ? ScrollBarWidth : 0) - bordersize * 2, textboxclienth);

            //System.Diagnostics.Debug.WriteLine("text box size " + textboxclienth + " Lines " + linesinbox );

            ScrollBar.Location = new Point(ClientRectangle.Width - ScrollBarWidth - bordersize, bordersize);
            ScrollBar.Size = new Size(ScrollBarWidth, textboxclienth);
            
            //System.Diagnostics.Debug.WriteLine("layout Scroll State Lines: " + LineCount + " FVL: " + firstVisibleLine + " textlines " + textboxlinesestimate);
        }

        private void UpdateScrollBar()            // from the richtext, set the scroll bar
        {
            int bordersize = (!BorderColor.IsFullyTransparent()) ? 3 : 0;
            int textboxclienth = ClientRectangle.Height - bordersize * 2;
            int linesinbox = EstimateLinesInBox(textboxclienth);

            int firstVisibleLine = unchecked((int)(long)TextBox.SendMessage(EM.GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero));

            //System.Diagnostics.Debug.WriteLine("Scroll State Lines: " + LineCount+ " FVL: " + firstVisibleLine + " textlines " + textboxlinesestimate);

            ScrollBar.SetValueMaximumLargeChange(firstVisibleLine, LineCount - 1, linesinbox );

            if (ScrollBar.IsScrollBarOn != visibleonlayout)     // need to relayout if scroll bars pop on
                PerformLayout();
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
            int lines = (int)(height/ GetRealFontHeight());
            //System.Diagnostics.Debug.WriteLine("Est Lines " + lines + " on " + height + " on " + FontHeight);
            return lines;
        }

        public int EstimateVerticalSizeFromText()
        {
            int numberlines = TextBox.Lines.Count();
            int bordersize = (!BorderColor.IsFullyTransparent()) ? 3 : 0;
            double fonth = GetRealFontHeight();
            int pixels = (int)( fonth * numberlines) + bordersize * 2 + 4;      // 4 extra for border area of this (bounds-client rect)
            //System.Diagnostics.Debug.WriteLine("Est Box " + numberlines + " " + bordersize + " " + fonth + " " + pixels + " " );
            return pixels;
        }

        protected virtual void OnVscrollChanged(object sender, EventArgs e) // comes from TextBox, update scroll..
        {
            UpdateScrollBar();
        }

        protected virtual void OnScrollBarChanged(object sender, ScrollEventArgs e)
        {
            ScrollToBar();
        }

        protected override void OnGotFocus(EventArgs e)             // Focus on us is given to the text box.
        {
            base.OnGotFocus(e);
            TextBox.Focus();
        }

        private void ScrollToBar()              // from the scrollbar, scroll first line to value
        {
            int firstVisibleLine = unchecked((int)(long)TextBox.SendMessage(EM.GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero));
            int scrollvalue = ScrollBar.Value;
            int delta = scrollvalue - firstVisibleLine;

            //Console.WriteLine("Scroll Bar:" + scrollvalue + " FVL: " + firstVisibleLine + " delta " + delta);
            if (delta != 0)
            {
                TextBox.SendMessage(EM.LINESCROLL, IntPtr.Zero, (IntPtr)(scrollvalue - firstVisibleLine));
            }
        }

        protected virtual void MWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0 )
                ScrollBar.ValueLimited--;                  // control takes care of end limits..
            else
                ScrollBar.ValueLimited++;           // end is UserLimit, not maximum

            ScrollToBar();                          // go to scroll position
        }

        protected void TextChangeEventHandler(object sender, EventArgs e)
        {
            if ( TextBoxChanged!=null)
                TextBoxChanged(this, new EventArgs());
        }

        #endregion

        private int lc = 1;

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RichTextBoxScroll
            // 
            this.Resize += new System.EventHandler(this.RichTextBoxScroll_Resize);
            this.ResumeLayout(false);

        }

        private void RichTextBoxScroll_Resize(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("Resize" + Size);
        }

        private void TextBox_MouseLeave(object sender, EventArgs e)             // using the text box mouse actions, pass thru to ours so registered handlers work
        {
            base.OnMouseLeave(e);
        }

        private void TextBox_MouseEnter(object sender, EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        private void TextBox_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        private void TextBox_MouseUp(object sender, MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }


    }
}

