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

        public Color BorderColor { get; set; } = Color.Transparent;
        public float BorderColorScaling { get; set; } = 0.5F;           // Popup style only
        public int ScrollBarWidth { get; set; } = 20;
        public bool ShowLineCount { get; set; } = false;                // count lines
        public bool HideScrollBar { get; set; } = true;                   // hide if no scroll needed

        public override string Text { get { return TextBox.Text; } set { TextBox.Text = value; } }                // return only textbox text

        public RichTextBoxBack TextBox;                 // Use these with caution.
        public VScrollBarCustom ScrollBar;

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
            UpdateVscroll();
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
            UpdateVscroll();
        }

        public void CopyFrom( RichTextBoxScroll other )
        {
            TextBox.Rtf = other.TextBox.Rtf;
        }

        #endregion

        #region Implementation

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

            if (BorderColor != Color.Transparent )
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                Color color1 = BorderColor;
                Color color2 = Multiply(BorderColor, BorderColorScaling);

                GraphicsPath g1 = RectCutCorners(1, 1, ClientRectangle.Width - 2, ClientRectangle.Height - 1, 1, 1);
                using (Pen pc1 = new Pen(color1, 1.0F))
                    e.Graphics.DrawPath(pc1, g1);

                GraphicsPath g2 = RectCutCorners(0, 0, ClientRectangle.Width, ClientRectangle.Height - 1, 2, 2);
                using (Pen pc2 = new Pen(color2, 1.0F))
                    e.Graphics.DrawPath(pc2, g2);
            }
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

        bool visibleonlayout = false;

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            int bordersize = (BorderColor != Color.Transparent) ? 3 : 0;
            int controlh = ClientRectangle.Height - bordersize * 2;

            int texth = controlh;
            int pxsize = TextBox.Font.Height;
            if (pxsize < 16)                            // this is what has been measured across FONTs, not sure why, not sure
                pxsize++;                               // if I can progamatically find it out..
            else if (pxsize >= 19)                      // Height lies, at 12 point, it says 19, but its only spacing the lines at 18.
                pxsize--;                               // tested MS Sans, Eurocaps, Calisto

            textboxlinesestimate = texth / pxsize;
            UpdateVscroll();

            visibleonlayout = ScrollBar.IsScrollBarOn || DesignMode || !HideScrollBar;  // Hide must be on, or in design mode, or scroll bar is on due to values

            TextBox.Location = new Point(bordersize, bordersize);
            TextBox.Size = new Size(ClientRectangle.Width - (visibleonlayout? ScrollBarWidth :0) - bordersize * 2, controlh);
            ScrollBar.Location = new Point(ClientRectangle.Width - ScrollBarWidth - bordersize, bordersize);
            ScrollBar.Size = new Size(ScrollBarWidth, controlh);

            //Console.WriteLine("With Font " + TextBox.Font.Name + ":" + TextBox.Font.Size + " Estimate " + textboxlinesestimate + " from " + TextBox.Size.Height + " est size " + pxsize);
        }

        const int EM_GETFIRSTVISIBLELINE = 0x00CE;
        const int EM_LINESCROLL = 0x00B6;
        const int EM_GETLINECOUNT = 0x00BA;

        protected virtual void OnVscrollChanged(object sender, EventArgs e) // comes from TextBox, update scroll..
        {
            UpdateVscroll();
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

        private void UpdateVscroll()            // from the richtext, set the scroll bar
        {
            int firstVisibleLine = unchecked((int)(long)TextBox.SendMessage(EM_GETFIRSTVISIBLELINE, (IntPtr)0, (IntPtr)0));
            int linecount = unchecked((int)(long)TextBox.SendMessage(EM_GETLINECOUNT, (IntPtr)0, (IntPtr)0));
            //Console.WriteLine("Scroll State Lines: " + linecount + " FVL: " + firstVisibleLine + " textlines " + textboxlinesestimate );
            ScrollBar.SetValueMaximumLargeChange(firstVisibleLine, linecount - 1, textboxlinesestimate);

            if (ScrollBar.IsScrollBarOn != visibleonlayout)     // need to relayout if scroll bars pop on
                PerformLayout();
        }

        private void ScrollToBar()              // from the scrollbar, scroll first line to value
        {
            int firstVisibleLine = unchecked((int)(long)TextBox.SendMessage(EM_GETFIRSTVISIBLELINE, (IntPtr)0, (IntPtr)0));
            int scrollvalue = ScrollBar.Value;
            int delta = scrollvalue - firstVisibleLine;

            //Console.WriteLine("Scroll Bar:" + scrollvalue + " FVL: " + firstVisibleLine + " delta " + delta);
            if (delta != 0)
            {
                TextBox.SendMessage(EM_LINESCROLL, (IntPtr)0, (IntPtr)(scrollvalue - firstVisibleLine));
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

        private int textboxlinesestimate = 1;
        private int lc = 1;
        private byte limit(float a) { if (a > 255F) return 255; else return (byte)a; }
        public Color Multiply(Color from, float m) { return Color.FromArgb(from.A, limit((float)from.R * m), limit((float)from.G * m), limit((float)from.B * m)); }

    }
}

