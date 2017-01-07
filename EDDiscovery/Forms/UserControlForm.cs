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
        public bool isloaded = false;
        public bool norepositionwindow = false;
        public bool istemporaryresized = false;
        public bool istransparent = false;          // we are in transparent mode (but may be showing due to inpanelshow)

        private bool inpanelshow = false;       // if we are in a panel show when we were transparent
        private bool defwindowsborder;
        private bool curwindowsborder;          // applied setting
        private bool displayTitle = true;
        private string dbrefname;
        private string wintitle;
        private Color transparencycolor = Color.Transparent;
        private Color beforetransparency = Color.Transparent;
        private Color tkey = Color.Transparent;
        private Color labelnormalcolour = Color.Transparent, labeltransparentcolour = Color.Transparent;
      
        private Timer timer = new Timer();      // timer to monitor for entry into form when transparent.. only sane way in forms
        private bool deftopmost, deftransparent;
        private Size normalsize;

        public bool IsTransparencySupported { get { return transparencycolor != Color.Transparent; } }

        public UserControlForm()
        {
            InitializeComponent();

            timer.Interval = 500;
            timer.Tick += CheckMouse;
        }

        #region Public Interface

        public void Init(EDDiscovery.UserControls.UserControlCommonBase c, string title, bool winborder, string rf , bool deftopmostp = false , bool defwindowintaskbar = true )
        {
            UserControl = c;
            c.Dock = DockStyle.None;
            c.Location = new Point(0, 10);
            c.Size = new Size(200, 200);
            this.Controls.Add(c);

            transparencycolor = c.ColorTransparency;

            wintitle = label_index.Text = this.Text = title;            // label index always contains the wintitle, but may not be shown

            curwindowsborder = defwindowsborder = winborder;
            dbrefname = "PopUpForm" + rf;
            deftopmost = deftopmostp;
            deftransparent = false;

            labelControlText.Text = "";                                 // always starts blank..

            this.ShowInTaskbar = SQLiteDBClass.GetSettingBool(dbrefname + "Taskbar", defwindowintaskbar);

            displayTitle = SQLiteDBClass.GetSettingBool(dbrefname + "ShowTitle", true);

            UpdateControls();

            Invalidate();
        }

        public void InitForTransparency(bool deftransparentp, Color labeln, Color labelt )
        {
            deftransparent = deftransparentp;
            labelnormalcolour = labeln;
            labeltransparentcolour = labelt;
        }

        public void SetControlText(string text)
        {
            labelControlText.Location = new Point(label_index.Location.X + label_index.Width + 16, labelControlText.Location.Y);
            labelControlText.Text = text;
            this.Text = wintitle + " " + text; 
        }

        public void SetTransparency(bool t)
        {
            if (IsTransparencySupported)
            {
                istransparent = t;
                UpdateTransparency();
                SQLiteDBClass.PutSettingBool(dbrefname + "Transparent", istransparent);
            }
        }

        public void SetShowTitleInTransparency(bool t)
        {
            displayTitle = t;
            UpdateControls();
            SQLiteDBClass.PutSettingBool(dbrefname + "ShowTitle", displayTitle);
        }

        public void SetTopMost(bool t)
        {
            TopMost = t;
            UpdateControls();
            SQLiteDBClass.PutSettingBool(dbrefname + "TopMost", TopMost);
        }

        public void SetShowInTaskBar(bool t)
        {
            this.ShowInTaskbar = t;
            UpdateControls();
            SQLiteDBClass.PutSettingBool(dbrefname + "Taskbar", t);
        }

        public UserControlCommonBase FindUserControl(Type c)
        {
            if (UserControl != null && UserControl.GetType().Equals(c))
                return UserControl;
            else
                return null;
        }

        #endregion

        #region View Implementation

        private void UpdateTransparency()
        {
            curwindowsborder = (!istransparent && defwindowsborder);    // we have a border if not transparent and we have a def border
            bool transparent = istransparent && !inpanelshow;           // are we transparent..

            Color togo;

            if (beforetransparency == Color.Transparent)
            {
                beforetransparency = this.BackColor;
                tkey = this.TransparencyKey;
            }

            UpdateControls();

            this.TransparencyKey = (transparent) ? transparencycolor : tkey;
            togo = (transparent) ? transparencycolor : beforetransparency;

            this.BackColor = togo;
            statusStripBottom.BackColor = togo;
            panel_taskbaricon.BackColor = panel_transparent.BackColor = panel_close.BackColor = 
                    panel_minimize.BackColor = panel_ontop.BackColor = panel_showtitle.BackColor =  panelTop.BackColor = togo;

            System.Diagnostics.Debug.Assert(labeltransparentcolour != Color.Transparent);
            label_index.ForeColor = labelControlText.ForeColor = (istransparent) ? labeltransparentcolour : labelnormalcolour;

            UserControl.SetTransparency(transparent,togo);
            PerformLayout();

            if (transparent || inpanelshow)
                timer.Start();
            else
                timer.Stop();
        }

        private void UpdateControls()
        {
            bool transparent = istransparent && !inpanelshow;           // are we transparent..

            FormBorderStyle = curwindowsborder ? FormBorderStyle.Sizable : FormBorderStyle.None;
            panelTop.Visible = !curwindowsborder;       // this also has the effect of removing the label_ and panel_ buttons

            statusStripBottom.Visible = !transparent && !curwindowsborder;      // status strip on, when not transparent, and when we don't have border
            
            panel_taskbaricon.Visible = panel_close.Visible = panel_minimize.Visible = panel_ontop.Visible = panel_showtitle.Visible = !transparent;

            panel_transparent.Visible = IsTransparencySupported && !transparent;
            panel_transparent.ImageSelected = (istransparent) ? ExtendedControls.DrawnPanel.ImageType.Transparent : ExtendedControls.DrawnPanel.ImageType.NotTransparent;

            label_index.Visible = labelControlText.Visible = (displayTitle || !transparent);   //  titles are on, or transparent is off

            panel_taskbaricon.ImageSelected = this.ShowInTaskbar ? ExtendedControls.DrawnPanel.ImageType.WindowInTaskBar : ExtendedControls.DrawnPanel.ImageType.WindowNotInTaskBar;
            panel_showtitle.ImageSelected = displayTitle ? ExtendedControls.DrawnPanel.ImageType.Captioned : ExtendedControls.DrawnPanel.ImageType.NotCaptioned;
        }

        public void SetFormSize()
        {
            var top = SQLiteDBClass.GetSettingInt(dbrefname + "Top", -1);

            if (top >= 0 && norepositionwindow == false)
            {
                var left = SQLiteDBClass.GetSettingInt(dbrefname + "Left", 0);
                var height = SQLiteDBClass.GetSettingInt(dbrefname + "Height", 800);
                var width = SQLiteDBClass.GetSettingInt(dbrefname + "Width", 800);

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

            if (UserControl != null)
                UserControl.LoadLayout();

            isloaded = true;
        }

        private void UserControlForm_Layout(object sender, LayoutEventArgs e)
        {
            if (UserControl != null)
            {
                UserControl.Location = new Point(3, curwindowsborder ? 2 : panelTop.Location.Y + panelTop.Height);
                UserControl.Size = new Size(ClientRectangle.Width - 6, ClientRectangle.Height - UserControl.Location.Y - (curwindowsborder ? 0 : statusStripBottom.Height));
            }
        }

        private void UserControlForm_Shown(object sender, EventArgs e)          // as launched, it may not be in front (as its launched from a control).. bring to front
        {
            this.BringToFront();

            bool tr = SQLiteDBClass.GetSettingBool(dbrefname + "Transparent", deftransparent);
            if ( tr && IsTransparencySupported)     // the check is for paranoia
                SetTransparency(true);      // only call if transparent.. may not be fully set up so don't merge with above

            SetTopMost(SQLiteDBClass.GetSettingBool(dbrefname + "TopMost", deftopmost));

            SetFormSize();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }

        private void UserControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isloaded = false;

            if (UserControl != null)
                UserControl.Closing();

            Size winsize = (istemporaryresized) ? normalsize : this.Size;
            SQLiteDBClass.PutSettingInt(dbrefname + "Width", winsize.Width);
            SQLiteDBClass.PutSettingInt(dbrefname + "Height", winsize.Height);
            SQLiteDBClass.PutSettingInt(dbrefname + "Top", this.Top);
            SQLiteDBClass.PutSettingInt(dbrefname + "Left", this.Left);
        }

        #endregion

        #region Clicks

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
            SetTopMost(!TopMost);
        }

        private void panel_transparency_Click(object sender, EventArgs e)       // only works if transparency is supported
        {
            inpanelshow = true;
            SetTransparency(!istransparent);
        }

        private void panel_taskbaricon_Click(object sender, EventArgs e)
        {
            SetShowInTaskBar(!this.ShowInTaskbar);
        }

        private void panel_showtitle_Click(object sender, EventArgs e)
        {
            SetShowTitleInTransparency(!displayTitle);
        }

        private void CheckMouse(object sender, EventArgs e)     // best way of knowing your inside the client.. using mouseleave/enter with transparency does not work..
        {
            if (isloaded)
            {
                //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " Tick" + istransparent + " " + inpanelshow);
                if (ClientRectangle.Contains(this.PointToClient(MousePosition)))
                {                                                                                                                                
                    if (!inpanelshow)
                    {
                        inpanelshow = true;
                        UpdateTransparency();
                    }
                }
                else
                {
                    if (inpanelshow)
                    {
                        inpanelshow = false;
                        UpdateTransparency();
                    }
                }
            }
        }

        #endregion

        #region Resizing

        public void RequestTemporaryMinimiumSize(Size w)            // Size w is the client area used by the UserControl..
        {
            int width = ClientRectangle.Width < w.Width ? (w.Width - ClientRectangle.Width) : 0;
            int height = ClientRectangle.Height < w.Height ? (w.Height - ClientRectangle.Height) : 0;

            RequestTemporaryResizeExpand(new Size(width, height));
        }

        public void RequestTemporaryResizeExpand(Size w)            // Size w is the client area above
        {
            if (w.Width != 0 || w.Height != 0)
                RequestTemporaryResize(new Size(ClientRectangle.Width + w.Width, ClientRectangle.Height + w.Height));
        }

        public void RequestTemporaryResize(Size w)                  // Size w is the client area above
        {
            if (!istemporaryresized)
            {
                normalsize = this.Size;
                istemporaryresized = true;                          // we are setting window size, so we need to consider the bounds around the window
                int widthoutsideclient = (Bounds.Size.Width - ClientRectangle.Width);
                int heightoutsideclient = (Bounds.Size.Height - ClientRectangle.Height);
                int heightlosttoothercontrols = UserControl.Location.Y + statusStripBottom.Height; // and the area used by the other bits of the window outside the user control
                this.Size = new Size(w.Width + widthoutsideclient, w.Height + heightlosttoothercontrols + heightoutsideclient);
            }
        }

        public void RevertToNormalSize()
        {
            if (istemporaryresized)
            {
                this.Size = normalsize;
                istemporaryresized = false;
            }
        }

        #endregion

        #region Low level Wndproc

        private const int MF_SEPARATOR = 0x800;
        private const int MF_STRING = 0x0;
        private int SYSMENU_ONTOP = 0x1;
        private int SYSMENU_TRANSPARENT = 0x2;
        private int SYSMENU_TASKBAR = 0x3;
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
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_TRANSPARENT, "&Transparent");
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_TASKBAR, "Show icon in Task&Bar for window");
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
            if (m.Msg == WM_SYSCOMMAND )
            {
                int cmd = (int)m.WParam;
                if (cmd == SYSMENU_ONTOP)
                {
                    panel_ontop_Click(null, null);
                }
                else if (cmd == SYSMENU_TRANSPARENT)
                {
                    if (IsTransparencySupported)
                    {
                        panel_transparency_Click(null, null);
                    }
                    else
                        MessageBox.Show("This panel does not support transparency");
                }
                else if (cmd == SYSMENU_TASKBAR)
                {
                    panel_taskbaricon_Click(null, null);
                }
                else
                    base.WndProc(ref m);
            }
            // Compatibility movement for Mono
            else if (m.Msg == WM_LBUTTONDOWN && (int)m.WParam == 1 && !curwindowsborder)
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

                    if (p.X > this.ClientSize.Width - statusStripBottom.Height && p.Y > this.ClientSize.Height - statusStripBottom.Height)
                    {
                        m.Result = (IntPtr)HT_BOTTOMRIGHT;
                    }
                    else if (p.Y > this.ClientSize.Height - statusStripBottom.Height)
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
                    else if (!curwindowsborder)
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

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            ((Control)sender).Capture = false;
            SendMessage(WM_NCLBUTTONDOWN, (System.IntPtr)HT_CAPTION, (System.IntPtr)0);
        }

        #endregion
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
                if (tcf.isloaded)
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

        public void ShowAllInTaskBar()
        {
            foreach (UserControlForm ucf in tabforms)
            {
                if (ucf.isloaded) ucf.SetShowInTaskBar(true);
            }
        }

        public void MakeAllOpaque()
        {
            foreach (UserControlForm ucf in tabforms)
            {
                if (ucf.isloaded)
                {
                    ucf.SetTransparency(false);
                    ucf.SetShowTitleInTransparency(true);
                }
            }
        }

    }
}
