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
    public partial class MessageBoxTheme : DraggableForm
    {
        // null in caption prints "Warning"

        static public DialogResult Show(IWin32Window window, string text, string caption = null , MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None, Icon windowicon = null)
        {
            using (MessageBoxTheme msg = new MessageBoxTheme(text, caption, buttons, icon, windowicon))
            {
                return msg.ShowDialog(window);
            }   
        }

        static public MessageBoxTheme ShowModeless(IWin32Window window, string text, string caption = null, MessageBoxIcon icon = MessageBoxIcon.None, Icon windowicon = null)
        {
            MessageBoxTheme msg = new MessageBoxTheme(text, caption, null, icon, windowicon);
            msg.Show(window);
            return msg;
        }

        static public DialogResult Show(string text, string caption = null, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None, Icon windowicon = null)
        {
            using (MessageBoxTheme msg = new MessageBoxTheme(text, caption, buttons, icon, windowicon))
            {
                return msg.ShowDialog(Application.OpenForms[0]);
            }   
        }

        public string MsgText { get { return msgText; } set { SetText(value); } }     // modeless update

        MessageBoxButtons? buttons;     // The buttons that this dialog will display
        MessageBoxIcon mbIcon;          // The icon that this dialog will show
        string msgText;                 // The text displayed by this form
        Image panelIcon;                // If not null, this icon will be drawn on the left of this form. Set from mbIcon in OnLoad
        Rectangle textarea;             // The area where we will draw the message text
        int ystart;                     // How far down does the text start from this.ClientArea.Top?

        public MessageBoxTheme(string text, string caption = null, MessageBoxButtons? buttons = MessageBoxButtons.OK, MessageBoxIcon messageBoxIcon = MessageBoxIcon.None, Icon formIcon = null)
        {
            InitializeComponent();

            DialogResult = DialogResult.None;

            this.msgText = text;
            this.Text = labelCaption.Text = caption ?? "Warning".Tx(this);
            this.buttons = buttons;
            this.mbIcon = messageBoxIcon;
            if (formIcon != null)
                this.Icon = formIcon;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            panelIcon?.Dispose();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            switch (buttons)
            {
                case null:
                    buttonExt1.Visible = buttonExt2.Visible = buttonExt3.Visible = false;
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    buttonExt1.DialogResult = DialogResult.Ignore; buttonExt1.Text = "Ignore".Tx(this);
                    buttonExt2.DialogResult = DialogResult.Retry; buttonExt2.Text = "Retry".Tx(this);
                    buttonExt3.DialogResult = DialogResult.Abort; buttonExt3.Text = "Abort".Tx(this);
                    this.AcceptButton = buttonExt2;
                    this.CancelButton = buttonExt3;
                    break;
                case MessageBoxButtons.OKCancel:
                    buttonExt1.DialogResult = DialogResult.Cancel; buttonExt1.Text = "Cancel".Tx(this);
                    buttonExt2.DialogResult = DialogResult.OK; buttonExt2.Text = "OK".Tx(this);
                    buttonExt3.Visible = false;
                    this.AcceptButton = buttonExt2;
                    this.CancelButton = buttonExt1;
                    break;
                case MessageBoxButtons.RetryCancel:
                    buttonExt1.DialogResult = DialogResult.Cancel; buttonExt1.Text = "Cancel".Tx(this);
                    buttonExt2.DialogResult = DialogResult.OK; buttonExt2.Text = "Retry".Tx(this);
                    buttonExt3.Visible = false;
                    this.AcceptButton = buttonExt2;
                    this.CancelButton = buttonExt1;
                    break;
                case MessageBoxButtons.YesNo:
                    buttonExt1.DialogResult = DialogResult.No; buttonExt1.Text = "No".Tx(this);
                    buttonExt2.DialogResult = DialogResult.Yes; buttonExt2.Text = "Yes".Tx(this);
                    buttonExt3.Visible = false;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    buttonExt1.DialogResult = DialogResult.Cancel; buttonExt1.Text = "Cancel".Tx(this);
                    buttonExt2.DialogResult = DialogResult.No; buttonExt2.Text = "No".Tx(this);
                    buttonExt3.DialogResult = DialogResult.Yes; buttonExt3.Text = "Yes".Tx(this);
                    this.AcceptButton = this.CancelButton = buttonExt1;
                    break;
                case MessageBoxButtons.OK:
                default:
                    buttonExt1.DialogResult = DialogResult.OK; buttonExt1.Text = "OK".Tx(this);
                    buttonExt2.Visible = false;
                    buttonExt3.Visible = false;
                    this.AcceptButton = this.CancelButton = buttonExt1;
                    break;
            }

            switch (mbIcon)
            {
                // case MessageBoxIcon.Information:
                case MessageBoxIcon.Asterisk:
                    panelIcon = SystemIcons.Asterisk.ToBitmap();
                    break;

                // case MessageBoxIcon.Exclamation:
                case MessageBoxIcon.Warning:
                    panelIcon = SystemIcons.Warning.ToBitmap();
                    break;

                // case MessageBoxIcon.Error:
                // case MessageBoxIcon.Stop:
                case MessageBoxIcon.Hand:
                    panelIcon = SystemIcons.Hand.ToBitmap();
                    break;

                case MessageBoxIcon.Question:
                    panelIcon = SystemIcons.Question.ToBitmap();
                    break;

                case MessageBoxIcon.None:
                default:
                    break;
            }

            bool framed = !(FormBorderStyle == FormBorderStyle.None);
            ITheme theme = ThemeableFormsInstance.Instance;
            if (theme != null)  // paranoid
            {
                this.ForeColor = theme.TextBlockColor;
                framed = theme.ApplyToForm(this,12);
                this.Font = labelCaption.Font;
                if (theme.MessageBoxWindowIcon != null)
                    this.Icon = theme.MessageBoxWindowIcon;
            }
            else
            {
                this.Font = new Font("MS Sans Serif", 12.0F);
                this.ForeColor = Color.Black;
            }

            labelCaption.Visible = !framed;
            ystart = framed ? 30 : 50;

            SetText(msgText);
        }

        private void SetText(string p)
        {
            SuspendLayout();

            msgText = p;
            //System.Diagnostics.Debug.WriteLine("Set text " + text);

            int bordery = Bounds.Height - ClientRectangle.Height;
            int borderx = Bounds.Width - ClientRectangle.Width;

            int left = (panelIcon != null) ? 80 : 20;

            using (Graphics g = CreateGraphics())
            {
                SizeF sizeftext = g.MeasureString(p, this.Font);
                SizeF sizefcaption = g.MeasureString(labelCaption.Text, this.Font);

                Height = (int)sizeftext.Height + 20 + ystart + 50 + bordery;
                int butwidth = buttonExt1.Right - (buttonExt3.Visible ? buttonExt3.Left : (buttonExt2.Visible ? buttonExt2.Left : buttonExt1.Left));
                int textwidth = (int)Math.Max(sizeftext.Width, sizefcaption.Width);
                //System.Diagnostics.Debug.WriteLine("But width {0} text width {1}", butwidth, textwidth);
                Width = Math.Min(Math.Max(butwidth, textwidth) + left + 20, 1880) + borderx;

                textarea = new Rectangle(left, ystart, (int)(sizeftext.Width + 1), (int)(sizeftext.Height + 1));
            }

            ResumeLayout(true);

            if (IsHandleCreated)
            {
                Refresh();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //System.Diagnostics.Debug.WriteLine("Message box paint " + text);

            //      using (Brush b = new SolidBrush(Color.Gray)) e.Graphics.FillRectangle(b, textarea);  // DEBUG

            using (Brush b = new SolidBrush(this.ForeColor))
            using (StringFormat f = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near })
                e.Graphics.DrawString(msgText, this.Font, b, textarea, f);

            if (panelIcon != null)
                e.Graphics.DrawImage(panelIcon, new Rectangle(10, textarea.Top, 48, 48));

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
        }

        private void buttonExt_Click(object sender, EventArgs e)
        {
            Close();
        }
        
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        private void MessageBoxTheme_Shown(object sender, EventArgs e)
        {
            if (StartPosition == FormStartPosition.CenterParent)        // because of Font resizing, because of width control above, lets recenter
            {
                Left = Owner.Left + Owner.Width / 2 - Width / 2;
                Top = Owner.Top + Owner.Height / 2 - Height / 2;
            }
        }

    }
}
