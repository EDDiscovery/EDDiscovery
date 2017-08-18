using BaseUtils.Win32Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class DraggableForm : Form
    {
        public DraggableForm()
        {
            DoubleBuffered = true;
            _dblClickTimer = new Timer { Enabled = false, Tag = Rectangle.Empty };
            _dblClickTimer.Tick += DblClickTimer_Tick;
        }

        protected void OnCaptionMouseDown(Control sender, MouseEventArgs e)
        {
            if (FormBorderStyle != FormBorderStyle.Sizable)
            {
                sender.Capture = false;
                if (e.Button == MouseButtons.Left)
                {
                    SendMessage(WM.NCLBUTTONDOWN, (IntPtr)HT.CAPTION, IntPtr.Zero);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    SendMessage(WM.NCRBUTTONDOWN, (IntPtr)HT.CAPTION, IntPtr.Zero);
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    SendMessage(WM.NCMBUTTONDOWN, (IntPtr)HT.CAPTION, IntPtr.Zero);
                }
            }
        }

        // Mono compatibility
        private bool _window_dragging = false;
        private Point _window_dragMousePos = Point.Empty;
        private Point _window_dragWindowPos = Point.Empty;
        private Timer _dblClickTimer;

        protected override void OnClosed(EventArgs e)
        {
            _dblClickTimer.Tag = null;
            _dblClickTimer.Dispose();
            _dblClickTimer = null;

            base.OnClosed(e);
        }

        protected IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(this.Handle, msg, wparam, lparam);
            this.WndProc(ref message);
            return message.Result;
        }

        protected override void WndProc(ref Message m)
        {
            const int MK_LBUTTON = 0x0001;  // No modifier keys allowed; just the LMB.
            bool windowsborder = this.FormBorderStyle == FormBorderStyle.Sizable;

            // Compatibility movement for Mono
            if (m.Msg == WM.LBUTTONDOWN && m.WParam == (IntPtr)MK_LBUTTON && !windowsborder)
            {
                _window_dragMousePos = new Point((int)m.LParam);
                _window_dragWindowPos = this.Location;
                _window_dragging = true;
                m.Result = IntPtr.Zero;
                this.Capture = true;
            }
            else if (m.Msg == WM.MOUSEMOVE && m.WParam == (IntPtr)MK_LBUTTON && _window_dragging)
            {
                Point curs = new Point((int)m.LParam);
                Point delta = new Point(curs.X - _window_dragMousePos.X, curs.Y - _window_dragMousePos.Y);
                _window_dragWindowPos = new Point(_window_dragWindowPos.X + delta.X, _window_dragWindowPos.Y + delta.Y);
                this.Location = _window_dragWindowPos;
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
            else if (m.Msg == WM.NCLBUTTONDOWN && !windowsborder && m.LParam == IntPtr.Zero)
            {
                var mousePos = MousePosition;
                if (_dblClickTimer.Enabled && ((Rectangle)_dblClickTimer.Tag).Contains(mousePos))
                {
                    _dblClickTimer.Enabled = false;
                    _dblClickTimer.Tag = Rectangle.Empty;

                    SendMessage(WM.NCLBUTTONDBLCLK, (IntPtr)HT.CAPTION, IntPtr.Zero);
                }
                else
                {
                    _dblClickTimer.Enabled = false;
                    _dblClickTimer.Interval = SystemInformation.DoubleClickTime;
                    var dblclksz = SystemInformation.DoubleClickSize;
                    var dblclkrc = new Rectangle(mousePos, dblclksz);
                    dblclkrc.Offset(dblclksz.Width / -2, dblclksz.Height / -2);
                    _dblClickTimer.Tag = dblclkrc;
                    _dblClickTimer.Enabled = true;

                    base.WndProc(ref m);
                }
                m.Result = IntPtr.Zero;
            }
            // Windows honours NCHITTEST; Mono does not
            else if (m.Msg == WM.NCHITTEST)
            {
                base.WndProc(ref m);

                const int CaptionHeight = 32;
                const int edgesz = 5;   // 5 is generous.. really only a few pixels gets thru before the subwindows grabs them
                Point p = PointToClient(new Point((int)m.LParam));

                if (SizeGripStyle != SizeGripStyle.Hide && WindowState != FormWindowState.Maximized && (p.X + p.Y >= ClientSize.Width + ClientSize.Height - (Controls.OfType<StatusStripCustom>().FirstOrDefault()?.Height ?? CaptionHeight)))
                {
                    m.Result = (IntPtr)HT.BOTTOMRIGHT;
                }
                else if (m.Result == (IntPtr)HT.CLIENT && !windowsborder)
                {
                    int rw = 1, col = 1;
                    
                    if (WindowState != FormWindowState.Maximized)
                    {
                        if (p.Y <= edgesz)
                            rw = 0;
                        else if (p.Y >= ClientSize.Height - edgesz)
                            rw = 2;

                        if (p.X <= edgesz)
                            col = 0;
                        else if (p.X >= ClientSize.Width - edgesz)
                            col = 2;
                    }

                    var htarr = new int[][]
                    {
                        new int[] { HT.TOPLEFT, HT.TOP, HT.TOPRIGHT },
                        new int[] { HT.LEFT, p.Y < CaptionHeight ? HT.CAPTION : HT.CLIENT, HT.RIGHT },
                        new int[] { HT.BOTTOMLEFT, HT.BOTTOM, HT.BOTTOMRIGHT },
                    };
                    m.Result = (IntPtr)htarr[rw][col];
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void DblClickTimer_Tick(object sender, EventArgs e)
        {
            _dblClickTimer.Enabled = false;
            _dblClickTimer.Tag = Rectangle.Empty;
        }
    }
}
