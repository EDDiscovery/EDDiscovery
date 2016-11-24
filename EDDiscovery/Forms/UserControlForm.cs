using EDDiscovery.DB;
using EDDiscovery.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class UserControlForm : Form
    {
        public UserControlCommonBase UserControl;
        public bool isactive = false;
        public bool norepositionwindow = false;
        public string refname;
        public string wintitle;

        public UserControlForm()
        {
            InitializeComponent();
        }

        public bool windowsborder = true;

        public void Init(string title, bool winborder, bool topmost, string rf)
        {
            wintitle = Text = title;
            refname = rf;
            windowsborder = winborder;
            FormBorderStyle = winborder ? FormBorderStyle.Sizable : FormBorderStyle.None;
            panel_close.Visible = !winborder;
            panel_minimize.Visible = !winborder;
            panel_ontop.Visible = !winborder;
            label_index.Visible = !winborder;
            labelControlText.Visible = false;
            labelControlText.Text = "";
            label_index.Text = this.Text;
            TopMost = topmost;
            panel_ontop.ImageSelected = TopMost ? ExtendedControls.DrawnPanel.ImageType.OnTop : ExtendedControls.DrawnPanel.ImageType.Floating;
            Invalidate();
        }

        public void SetControlText(string text)
        {
            if ( FormBorderStyle == FormBorderStyle.None )
            {
                labelControlText.Location = new Point(label_index.Location.X + label_index.Width + 16, labelControlText.Location.Y);
                labelControlText.Visible = true;
                labelControlText.Text = text;
            }
            else
            {
                this.Text = wintitle + " " + text;
            }
        }

        public void AddUserControl(EDDiscovery.UserControls.UserControlCommonBase c)
        {
            UserControl = c;
            c.Dock = DockStyle.None;
            c.Location = new Point(0, 10);
            c.Size = new Size(200, 200);
            this.Controls.Add(c);
        }

        private void UserControlForm_Activated(object sender, EventArgs e)
        {
            isactive = true;
            if (UserControl != null)
                UserControl.LoadLayout();
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

            if (UserControl != null)
                UserControl.Closing();

            string root = "PopUpForm" + refname;
            SQLiteDBClass.PutSettingInt(root + "Width", this.Width);
            SQLiteDBClass.PutSettingInt(root + "Height", this.Height);
            SQLiteDBClass.PutSettingInt(root + "Top", this.Top);
            SQLiteDBClass.PutSettingInt(root + "Left", this.Left);
        }

        public UserControlCommonBase FindUserControl(Type c)
        {
            if (UserControl.GetType().Equals(c))
                return UserControl;
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

        private void panel_ontop_Click(object sender, EventArgs e)
        {
            TopMost = !TopMost;
            panel_ontop.ImageSelected = TopMost ? ExtendedControls.DrawnPanel.ImageType.OnTop : ExtendedControls.DrawnPanel.ImageType.Floating;
        }

        private void UserControlForm_Layout(object sender, LayoutEventArgs e)
        {
            if (UserControl != null)
            {
                UserControl.Location = new Point(3, windowsborder ? 2 : panel_close.Location.Y+panel_close.Height);
                UserControl.Size = new Size(ClientRectangle.Width - 6, ClientRectangle.Height - UserControl.Location.Y - statusStripCustom1.Height);
            }
        }


        private const int MF_SEPARATOR = 0x800;
        private const int MF_STRING = 0x0;
        private int SYSMENU_ONTOP = 0x1;
        private const int WM_SYSCOMMAND = 0x112;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Get a handle to a copy of this form's system (window) menu
            IntPtr hSysMenu = GetSystemMenu(this.Handle, false);

            // Add a separator
            AppendMenu(hSysMenu, MF_SEPARATOR, 0, string.Empty);

            // Add the About menu item
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_ONTOP, "&On Top");
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
        public const int HT_LEFT = 0xA;
        public const int HT_RIGHT = 0xB;
        public const int HT_BOTTOM = 0xF;
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
            if ((m.Msg == WM_SYSCOMMAND) && ((int)m.WParam == SYSMENU_ONTOP))
            {
                TopMost = !TopMost;
            }

            // Compatibility movement for Mono
            else if (m.Msg == WM_LBUTTONDOWN && (int)m.WParam == 1 && !windowsborder)
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
                    else if (p.Y > this.ClientSize.Height - statusStripCustom1.Height)
                    {
                        m.Result = (IntPtr)HT_BOTTOM;
                    }
                    else if (p.X > this.ClientSize.Width - 5)       // 5 is generous.. really only a few pixels gets thru before the subwindows grabs them
                    {
                        m.Result = (IntPtr)HT_RIGHT;
                    }
                    else if (p.X < 5)
                    {
                        m.Result = (IntPtr)HT_LEFT;
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
