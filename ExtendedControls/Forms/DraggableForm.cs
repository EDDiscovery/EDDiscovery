/*
 * Copyright © 2017 EDDiscovery development team
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
using BaseUtils.Win32;
using BaseUtils.Win32Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtendedControls
{
    public class DraggableForm : SmartSysMenuForm
    {
        public DraggableForm()
        {
            _dblClickTimer = new Timer();
            _dblClickTimer.Tick += DblClickTimer_Tick;
        }


        protected virtual bool DraggableDisableResize { get; } = false;       // SET true to stop your form from resizing


        protected override void Dispose(bool disposing)
        {
            if (disposing && _dblClickTimer != null)
            {
                _dblClickTimer.Enabled = false;
                _dblClickTimer.Tag = null;
                _dblClickTimer.Dispose();
            }
            _dblClickTimer = null;
            base.Dispose(disposing);
        }

        protected void OnCaptionMouseDown(Control sender, MouseEventArgs e)
        {
            sender.Capture = false;
            if (FormBorderStyle == FormBorderStyle.None)
            {
                var p = sender.PointToScreen(e.Location);
                var lParam = (IntPtr)((ushort)p.X | (p.Y << 16));

                switch (e.Button)
                {
                    case MouseButtons.Left:     SendMessage(WM.NCLBUTTONDOWN, (IntPtr)HT.CAPTION, lParam); break;
                    case MouseButtons.Middle:   SendMessage(WM.NCMBUTTONDOWN, (IntPtr)HT.CAPTION, lParam); break;
                    case MouseButtons.Right:    SendMessage(WM.NCRBUTTONDOWN, (IntPtr)HT.CAPTION, lParam); break;
                }
            }
        }

        protected void OnCaptionMouseUp(Control sender, MouseEventArgs e)
        {
            if (FormBorderStyle == FormBorderStyle.None)
            {
                var p = sender.PointToScreen(e.Location);
                var lParam = (IntPtr)((ushort)p.X | (p.Y << 16));

                switch (e.Button)
                {
                    case MouseButtons.Left:     SendMessage(WM.NCLBUTTONUP, (IntPtr)HT.CAPTION, lParam); break;
                    case MouseButtons.Middle:   SendMessage(WM.NCMBUTTONUP, (IntPtr)HT.CAPTION, lParam); break;
                    case MouseButtons.Right:    SendMessage(WM.NCRBUTTONUP, (IntPtr)HT.CAPTION, lParam); break;
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            bool windowsborder = this.FormBorderStyle == FormBorderStyle.Sizable;

            switch (m.Msg)
            {
                case WM.NCHITTEST:      // Windows honours NCHITTEST; Mono does not 
                    {
                        if (!AllowResize)
                        {
                            m.Result = (IntPtr)HT.CAPTION;
                        }   
                        else
                        {
                            base.WndProc(ref m);

                            var p = PointToClient(new Point((int)m.LParam));
                            const int CaptionHeight = 32;
                            const int edgesz = 5;   // 5 is generous.. really only a few pixels gets thru before the subwindows grabs them

                            if (SizeGripStyle != SizeGripStyle.Hide && WindowState != FormWindowState.Maximized && (p.X + p.Y >= ClientSize.Width + ClientSize.Height -
                                (Controls.OfType<StatusStripCustom>().FirstOrDefault()?.Height ?? Controls.OfType<StatusStrip>().FirstOrDefault()?.Height ?? CaptionHeight)))
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
                                    new int[] { HT.BOTTOMLEFT, HT.BOTTOM, HT.BOTTOMRIGHT }
                                };
                                m.Result = (IntPtr)htarr[rw][col];
                            }
                        }

                        return;
                    }

                case WM.NCLBUTTONDOWN:  // Monitor and intercept double-clicks, ignoring the fact that it may occur over multiple controls with/without capture.
                    {
                        if (!windowsborder && m.WParam == (IntPtr)HT.CAPTION && AllowResize)
                        {
                            var p = new Point((int)m.LParam);
                            if (_dblClickTimer.Enabled && ((Rectangle)_dblClickTimer.Tag).Contains(p))
                            {
                                _dblClickTimer.Enabled = false;
                                _dblClickTimer.Tag = Rectangle.Empty;
                                SendMessage(WM.NCLBUTTONDBLCLK, (IntPtr)HT.CAPTION, m.LParam);
                                m.Result = IntPtr.Zero;
                                return;
                            }
                            else
                            {
                                _dblClickTimer.Enabled = false;
                                _dblClickTimer.Interval = SystemInformation.DoubleClickTime;
                                var dblclksz = SystemInformation.DoubleClickSize;
                                var dblclkrc = new Rectangle(p, dblclksz);
                                dblclkrc.Offset(dblclksz.Width / -2, dblclksz.Height / -2);
                                _dblClickTimer.Tag = dblclkrc;
                                _dblClickTimer.Enabled = true;
                            }
                        }
                        break;
                    }
            }
            base.WndProc(ref m);
        }

        #region Private implementation

        private System.Windows.Forms.Timer _dblClickTimer = null;

        private void DblClickTimer_Tick(object sender, EventArgs e)
        {
            _dblClickTimer.Enabled = false;
        }

        #endregion
    }
}
