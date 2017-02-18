using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.Win32Constants;

namespace EDDiscovery.Forms
{
    public partial class MessageBoxTheme : Form
    {
        static public DialogResult Show(IWin32Window window, string text, string caption = "EDDiscovery Message", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            MessageBoxTheme msg = new MessageBoxTheme();
            msg.Init(text, caption, buttons, icon);
            return msg.ShowDialog(window);
        }

        Font fnt;
        string text;
        Rectangle textarea;
        Icon icon;
        Color forecolour;

        public MessageBoxTheme()
        {
            InitializeComponent();
        }

        public void Init(string ptext, string caption , MessageBoxButtons buttons, MessageBoxIcon ic )
        {
            if (buttons == MessageBoxButtons.AbortRetryIgnore)
            {
                buttonExt1.Tag = DialogResult.Ignore; buttonExt1.Text = "Ignore";
                buttonExt2.Tag = DialogResult.Retry; buttonExt2.Text = "Retry";
                buttonExt3.Tag = DialogResult.Abort; buttonExt3.Text = "Abort";
            }
            else if (buttons == MessageBoxButtons.OK)
            {
                buttonExt1.Tag = DialogResult.OK; buttonExt1.Text = "OK";
                buttonExt2.Visible = false;
                buttonExt3.Visible = false;
                this.AcceptButton = this.CancelButton = buttonExt1;
            }
            else if (buttons == MessageBoxButtons.OKCancel)
            {
                buttonExt1.Tag = DialogResult.Cancel; buttonExt1.Text = "Cancel";
                buttonExt2.Tag = DialogResult.OK; buttonExt2.Text = "OK";
                buttonExt3.Visible = false;
            }
            else if (buttons == MessageBoxButtons.RetryCancel)
            {
                buttonExt1.Tag = DialogResult.Cancel; buttonExt1.Text = "Cancel";
                buttonExt2.Tag = DialogResult.OK; buttonExt2.Text = "Retry";
                buttonExt3.Visible = false;
            }
            else if (buttons == MessageBoxButtons.YesNo)
            {
                buttonExt1.Tag = DialogResult.No; buttonExt1.Text = "No";
                buttonExt2.Tag = DialogResult.Yes; buttonExt2.Text = "Yes";
                buttonExt3.Visible = false;
            }
            else if (buttons == MessageBoxButtons.YesNoCancel)
            {
                buttonExt1.Tag = DialogResult.Cancel; buttonExt1.Text = "Cancel";
                buttonExt2.Tag = DialogResult.No; buttonExt2.Text = "No";
                buttonExt3.Tag = DialogResult.Yes; buttonExt3.Text = "Yes";
            }

            labelCaption.Text = this.Text = caption;
            text = ptext;

            if (ic == MessageBoxIcon.Asterisk)
                icon = SystemIcons.Asterisk;
            if (ic == MessageBoxIcon.Error)
                icon = SystemIcons.Error;
            if (ic == MessageBoxIcon.Exclamation)
                icon = SystemIcons.Exclamation;
            if (ic == MessageBoxIcon.Information)
                icon = SystemIcons.Information;
            if (ic == MessageBoxIcon.Question)
                icon = SystemIcons.Question;
            if (ic == MessageBoxIcon.Warning)
                icon = SystemIcons.Warning;

            int ystart = 30;

            EDDiscovery2.EDDTheme theme = EDDiscovery2.EDDTheme.Instance;
            if (theme != null)  // paranoid
            {
                fnt = new Font(theme.FontName, 12.0F);
                forecolour = theme.TextBlockColor;
                bool border = theme.ApplyToForm(this, fnt);
                if (!border)
                {
                    labelCaption.Visible = true;
                    ystart += 20;
                }
            }
            else
            {
                fnt = new Font("MS Sans Serif", 12.0F);
                forecolour = Color.Red;
            }

            int bordery = Bounds.Height - ClientRectangle.Height;
            int borderx = Bounds.Width - ClientRectangle.Width;

            SizeF sizef = CreateGraphics().MeasureString(text, fnt);

            int left = (ic != MessageBoxIcon.None) ? 80 : 20;

            Height = (int)sizef.Height + ystart + 50 + bordery;
            Width = Math.Min(Math.Max(300, left + (int)sizef.Width + 20),1800) + borderx;

            textarea = new Rectangle(left, ystart, (int)(sizef.Width+1), (int)(sizef.Height+1));
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

      //      using (Brush b = new SolidBrush(Color.Gray)) e.Graphics.FillRectangle(b, textarea);  // DEBUG

            using (Brush b = new SolidBrush(forecolour))
            {
                StringFormat f = new StringFormat();
                f.Alignment = StringAlignment.Near;
                f.LineAlignment = StringAlignment.Near;

                e.Graphics.DrawString(text, fnt, b, textarea, f);
            }

            if (icon != null)
                e.Graphics.DrawIcon(icon, new Rectangle(10, textarea.Top, 48, 48));
        }

        private void MessageBoxTheme_FormClosed(object sender, FormClosedEventArgs e)
        {
            fnt?.Dispose();
        }

        private void buttonExt1_Click(object sender, EventArgs e)
        {
            DialogResult = (DialogResult)(((ExtendedControls.ButtonExt)sender).Tag);
            Close();
        }
        private void buttonExt2_Click(object sender, EventArgs e)
        {
            DialogResult = (DialogResult)(((ExtendedControls.ButtonExt)sender).Tag);
            Close();
        }
        private void buttonExt3_Click(object sender, EventArgs e)
        {
            DialogResult = (DialogResult)(((ExtendedControls.ButtonExt)sender).Tag);
            Close();
        }

        #region Window control

        // Mono compatibility
        private bool _window_dragging = false;
        private Point _window_dragMousePos = Point.Empty;
        private Point _window_dragWindowPos = Point.Empty;

        private IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(this.Handle, msg, wparam, lparam);
            this.WndProc(ref message);
            return message.Result;
        }

        protected override void WndProc(ref Message m)
        {
            bool windowsborder = this.FormBorderStyle == FormBorderStyle.Sizable;
            // Compatibility movement for Mono
            if (m.Msg == WM.LBUTTONDOWN && (int)m.WParam == 1 && !windowsborder)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                _window_dragMousePos = new Point(x, y);
                _window_dragWindowPos = this.Location;
                _window_dragging = true;
                m.Result = IntPtr.Zero;
                this.Capture = true;
            }
            else if (m.Msg == WM.MOUSEMOVE && (int)m.WParam == 1 && _window_dragging)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                Point delta = new Point(x - _window_dragMousePos.X, y - _window_dragMousePos.Y);
                _window_dragWindowPos = new Point(_window_dragWindowPos.X + delta.X, _window_dragWindowPos.Y + delta.Y);
                this.Location = _window_dragWindowPos;
                this.Update();
                m.Result = IntPtr.Zero;
            }
            else if (m.Msg == WM.LBUTTONUP)
            {
                _window_dragging = false;
                _window_dragMousePos = Point.Empty;
                _window_dragWindowPos = Point.Empty;
                m.Result = IntPtr.Zero;
                this.Capture = false;
            }
            // Windows honours NCHITTEST; Mono does not
            else
            {
                base.WndProc(ref m);
            }
        }

        #endregion

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ((Control)sender).Capture = false;
            SendMessage(WM.NCLBUTTONDOWN, (System.IntPtr)HT.CAPTION, (System.IntPtr)0);
        }

    }
}
