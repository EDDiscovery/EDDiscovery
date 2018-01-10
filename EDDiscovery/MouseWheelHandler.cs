using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Scatter
{
    public static class MouseWheelHandler
    {
        public static void Add(Control ctrl, Action<MouseEventArgs> onMouseWheel)
        {
            if (ctrl == null || onMouseWheel == null)
                throw new ArgumentNullException();

            var filter = new MouseWheelMessageFilter(ctrl, onMouseWheel);
            Application.AddMessageFilter(filter);
            ctrl.Disposed += (s, e) => Application.RemoveMessageFilter(filter);
        }

        class MouseWheelMessageFilter
            : IMessageFilter
        {
            private readonly Control _ctrl;
            private readonly Action<MouseEventArgs> _onMouseWheel;

            public MouseWheelMessageFilter(Control ctrl, Action<MouseEventArgs> onMouseWheel)
            {
                _ctrl = ctrl;
                _onMouseWheel = onMouseWheel;
            }

            public bool PreFilterMessage(ref Message m)
            {
                var parent = _ctrl.Parent;
                if (parent != null && m.Msg == 0x20a) // WM_MOUSEWHEEL, find the control at screen position m.LParam
                {
                    var pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);

                    var clientPos = _ctrl.PointToClient(pos);

                    if (_ctrl.ClientRectangle.Contains(clientPos)
                     && ReferenceEquals(_ctrl, parent.GetChildAtPoint(parent.PointToClient(pos))))
                    {
                        var wParam = m.WParam.ToInt32();
                        Func<int, MouseButtons, MouseButtons> getButton =
                            (flag, button) => ((wParam & flag) == flag) ? button : MouseButtons.None;

                        var buttons = getButton(wParam & 0x0001, MouseButtons.Left)
                                    | getButton(wParam & 0x0010, MouseButtons.Middle)
                                    | getButton(wParam & 0x0002, MouseButtons.Right)
                                    | getButton(wParam & 0x0020, MouseButtons.XButton1)
                                    | getButton(wParam & 0x0040, MouseButtons.XButton2)
                                    ; // Not matching for these /*MK_SHIFT=0x0004;MK_CONTROL=0x0008*/

                        var delta = wParam >> 16;
                        var e = new MouseEventArgs(buttons, 0, clientPos.X, clientPos.Y, delta);
                        _onMouseWheel(e);

                        return true;
                    }
                }
                return false;
            }
        }
    }
}
