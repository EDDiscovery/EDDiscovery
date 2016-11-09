using EDDiscovery.DB;
using EDDiscovery.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class UserControlForm : Form
    {
        UserControlCommonBase uc;
        public bool isactive = false;
        public bool norepositionwindow = false;
        public string refname;

        public UserControlForm()
        {
            InitializeComponent();
        }

        public bool windowsborder = true;

        public void Init(string title, bool winborder, bool topmost, string rf)
        {
            Text = title;
            refname = rf;
            windowsborder = winborder;
            FormBorderStyle = winborder ? FormBorderStyle.Sizable : FormBorderStyle.None;
            panel_close.Visible = !winborder;
            panel_minimize.Visible = !winborder;
            label_index.Visible = !winborder;
            label_index.Text = this.Text;
            TopMost = topmost;
            Invalidate();
        }

        public void AddUserControl(EDDiscovery.UserControls.UserControlCommonBase c)
        {
            uc = c;
            c.Dock = DockStyle.None;
            c.Location = new Point(0, 10);
            c.Size = new Size(200, 200);
            this.Controls.Add(c);
        }

        private void UserControlForm_Activated(object sender, EventArgs e)
        {
            isactive = true;
            if (uc != null)
                uc.LoadLayout();
        }

        private void UserControlForm_Load(object sender, EventArgs e)
        {
            string root = "PopUpForm" + refname;

            var top = SQLiteDBClass.GetSettingInt(root + "Top", -1);

            if (top >= 0 && norepositionwindow == false)
            {
                var left = SQLiteDBClass.GetSettingInt(root + "Left", 0);
                var height = SQLiteDBClass.GetSettingInt(root + "Height", 800);
                var width = SQLiteDBClass.GetSettingInt(root + "Width", 800);

                // Adjust so window fits on screen; just in case user unplugged a monitor or something

                var screen = SystemInformation.VirtualScreen;
                if (height > screen.Height) height = screen.Height;
                if (top + height > screen.Height + screen.Top) top = screen.Height + screen.Top - height;
                if (width > screen.Width) width = screen.Width;
                if (left + width > screen.Width + screen.Left) left = screen.Width + screen.Left - width;
                if (top < screen.Top) top = screen.Top;
                if (left < screen.Left) left = screen.Left;

                this.Top = top;
                this.Left = left;
                this.Height = height;
                this.Width = width;

                this.CreateParams.X = this.Left;
                this.CreateParams.Y = this.Top;
                this.StartPosition = FormStartPosition.Manual;
            }
        }

        private void TabControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isactive = false;

            if (uc != null)
                uc.Closing();

            string root = "PopUpForm" + refname;
            SQLiteDBClass.PutSettingInt(root + "Width", this.Width);
            SQLiteDBClass.PutSettingInt(root + "Height", this.Height);
            SQLiteDBClass.PutSettingInt(root + "Top", this.Top);
            SQLiteDBClass.PutSettingInt(root + "Left", this.Left);
        }

        public UserControlCommonBase FindUserControl(Type c)
        {
            if (uc.GetType().Equals(c))
                return uc;
            else
                return null;
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void UserControlForm_Layout(object sender, LayoutEventArgs e)
        {
            if (uc != null)
            {
                uc.Location = new Point(2, windowsborder ? 2 : panel_close.Location.Y+panel_close.Height);
                uc.Size = new Size(ClientRectangle.Width - 4, ClientRectangle.Height - uc.Location.Y - statusStripCustom1.Height);
            }
        }

        public const int WM_MOVE = 3;
        public const int WM_SIZE = 5;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCLBUTTONUP = 0xA2;
        public const int WM_NCMOUSEMOVE = 0xA0;
        public const int HT_CLIENT = 0x1;
        public const int HT_CAPTION = 0x2;
        public const int HT_BOTTOMRIGHT = 0x11;
        public const int WM_NCL_RESIZE = 0x112;
        public const int HT_RESIZE = 61448;
        public const int WM_NCHITTEST = 0x84;

        private IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(this.Handle, msg, wparam, lparam);
            this.WndProc(ref message);
            return message.Result;
        }

        // Mono compatibility
        private bool _window_dragging = false;
        private Point _window_dragMousePos = Point.Empty;
        private Point _window_dragWindowPos = Point.Empty;

        protected override void WndProc(ref Message m)
        {
            // Compatibility movement for Mono
            if (m.Msg == WM_LBUTTONDOWN && (int)m.WParam == 1 && !windowsborder)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                _window_dragMousePos = new Point(x, y);
                _window_dragWindowPos = this.Location;
                _window_dragging = true;
                m.Result = IntPtr.Zero;
                this.Capture = true;
            }
            else if (m.Msg == WM_MOUSEMOVE && (int)m.WParam == 1 && _window_dragging)
            {
                int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                int y = unchecked((short)((uint)m.LParam >> 16));
                Point delta = new Point(x - _window_dragMousePos.X, y - _window_dragMousePos.Y);
                _window_dragWindowPos = new Point(_window_dragWindowPos.X + delta.X, _window_dragWindowPos.Y + delta.Y);
                this.Location = _window_dragWindowPos;
                this.Update();
                m.Result = IntPtr.Zero;
            }
            else if (m.Msg == WM_LBUTTONUP)
            {
                _window_dragging = false;
                _window_dragMousePos = Point.Empty;
                _window_dragWindowPos = Point.Empty;
                m.Result = IntPtr.Zero;
                this.Capture = false;
            }
            // Windows honours NCHITTEST; Mono does not
            else if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);

                if ((int)m.Result == HT_CLIENT)
                {
                    int x = unchecked((short)((uint)m.LParam & 0xFFFF));
                    int y = unchecked((short)((uint)m.LParam >> 16));
                    Point p = PointToClient(new Point(x, y));

                    if (p.X > this.ClientSize.Width - statusStripCustom1.Height && p.Y > this.ClientSize.Height - statusStripCustom1.Height)
                    {
                        m.Result = (IntPtr)HT_BOTTOMRIGHT;
                    }
                    else if (!windowsborder)
                    {
                        m.Result = (IntPtr)HT_CAPTION;
                    }
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

    }

    public class UserControlFormList
    {
        private List<UserControlForm> tabforms;

        public int Count { get { return tabforms.Count; } }

        public UserControlFormList()
        {
            tabforms = new List<UserControlForm>();
        }

        public UserControlForm NewForm(bool noreposition)
        {
            UserControlForm tcf = new UserControlForm();
            tabforms.Add(tcf);

            tcf.norepositionwindow = noreposition;
            tcf.FormClosed += FormClosed;
            return tcf;
        }

        private void FormClosed(Object sender, FormClosedEventArgs e)
        {
            UserControlForm tcf = (UserControlForm)sender;
            tabforms.Remove(tcf);
        }

        public List<UserControlCommonBase> GetListOfControls(Type c)
        {
            List<UserControlCommonBase> lc = new List<UserControlCommonBase>();

            foreach (UserControlForm tcf in tabforms)
            {
                if (tcf.isactive)
                {
                    UserControlCommonBase uc = tcf.FindUserControl(c);
                    if (uc != null)
                        lc.Add(uc);
                }
            }

            return lc;
        }

        public int CountOf(Type c)
        {
            int count = 0;

            foreach (UserControlForm tcf in tabforms)
            {
                if (tcf.FindUserControl(c) != null)
                    count++;
            }

            return count;
        }

    }
}
