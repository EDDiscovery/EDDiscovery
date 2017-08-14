using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseUtils.Win32Constants;

namespace ExtendedControls
{
    public partial class MessageBoxTheme : Form
    {
        static public DialogResult Show(IWin32Window window, string text, string caption = "Warning", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None, Icon windowicon = null)
        {
            MessageBoxTheme msg = new MessageBoxTheme();
            msg.StartPosition = FormStartPosition.CenterParent;
            msg.Init(text, caption, buttons, icon, windowicon);
            return msg.ShowDialog(window);
        }

        static public MessageBoxTheme ShowModeless(IWin32Window window, string text, string caption = "Warning", MessageBoxIcon icon = MessageBoxIcon.None, Icon windowicon = null)
        {
            MessageBoxTheme msg = new MessageBoxTheme();
            msg.Init(text, caption, null, icon, windowicon );
            msg.Show(window);
            msg.CenterToParent();
            msg.Update();
            return msg;
        }

        static public DialogResult Show(string text, string caption = "Warning", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None, Icon windowicon = null)
        {
            MessageBoxTheme msg = new MessageBoxTheme();
            msg.StartPosition = FormStartPosition.CenterScreen;
            msg.Init(text, caption, buttons, icon, windowicon);
            Form f = Application.OpenForms[0];
            return msg.ShowDialog(f);
        }

        public string MsgText { get { return text; } set { SetText(value); Invalidate(true); Update(); } }     // modeless update

        string text;
        Font fnt;
        Rectangle textarea;
        Icon icon;
        Color forecolour;
        int ystart;

        public MessageBoxTheme()
        {
            InitializeComponent();
        }

        public void Init(string ptext, string caption, MessageBoxButtons? buttons, MessageBoxIcon ic, System.Drawing.Icon windowicon)
        {
            if (buttons == null)
            {
                buttonExt1.Visible = false;
                buttonExt2.Visible = false;
                buttonExt3.Visible = false;
            }
            else if (buttons == MessageBoxButtons.AbortRetryIgnore)
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

            if (ic == MessageBoxIcon.Asterisk)
                icon = SystemIcons.Asterisk;
            else if (ic == MessageBoxIcon.Error)
                icon = SystemIcons.Error;
            else if (ic == MessageBoxIcon.Exclamation)
                icon = SystemIcons.Exclamation;
            else if (ic == MessageBoxIcon.Information)
                icon = SystemIcons.Information;
            else if (ic == MessageBoxIcon.Question)
                icon = SystemIcons.Question;
            else if (ic == MessageBoxIcon.Warning)
                icon = SystemIcons.Warning;

            ThemeableForms theme = ThemeableFormsInstance.Instance;
            if (theme != null)  // paranoid
            {
                fnt = new Font(theme.FontName, 12.0F);
                forecolour = theme.TextBlockColor;
                bool border = theme.ApplyToForm(this, fnt);
                if (!border)
                    labelCaption.Visible = true;
                ystart = 30 + (!border ? 20 : 0);
                if (windowicon != null)
                    this.Icon = windowicon;
                else if (theme.MessageBoxWindowIcon != null)
                    this.Icon = theme.MessageBoxWindowIcon;
            }
            else
            {
                fnt = new Font("MS Sans Serif", 12.0F);
                forecolour = Color.Black;
                if ( windowicon != null )
                    this.Icon = windowicon;
                ystart = 30;
            }

            SetText(ptext);
        }

        private void SetText(string p)
        {
            text = p;
            //System.Diagnostics.Debug.WriteLine("Set text " + text);

            int bordery = Bounds.Height - ClientRectangle.Height;
            int borderx = Bounds.Width - ClientRectangle.Width;

            int left = (icon != null) ? 80 : 20;

            using (Graphics g = CreateGraphics())
            {
                SizeF sizeftext = g.MeasureString(text, fnt);
                SizeF sizefcaption = g.MeasureString(labelCaption.Text, fnt);

                Height = (int)sizeftext.Height + ystart + 50 + bordery;
                Width = Math.Min(Math.Max(300, left + (int)Math.Max(sizeftext.Width, sizefcaption.Width) + 20), 1800) + borderx;

                textarea = new Rectangle(left, ystart, (int)(sizeftext.Width + 1), (int)(sizeftext.Height + 1));
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //System.Diagnostics.Debug.WriteLine("Message box paint " + text);

            //      using (Brush b = new SolidBrush(Color.Gray)) e.Graphics.FillRectangle(b, textarea);  // DEBUG

            using (Brush b = new SolidBrush(forecolour))
            {
                using (StringFormat f = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near })
                {
                    e.Graphics.DrawString(text, fnt, b, textarea, f);
                }
            }

            if (icon != null)
                e.Graphics.DrawIcon(icon, new Rectangle(10, textarea.Top, 48, 48));

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
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
            if (m.Msg == WM.LBUTTONDOWN && m.WParam == (IntPtr)1 && !windowsborder)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                _window_dragMousePos = new Point(x, y);
                _window_dragWindowPos = this.Location;
                _window_dragging = true;
                m.Result = IntPtr.Zero;
                this.Capture = true;
            }
            else if (m.Msg == WM.MOUSEMOVE && m.WParam == (IntPtr)1 && _window_dragging)
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
