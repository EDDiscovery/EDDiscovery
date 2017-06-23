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

namespace EDDiscovery.Win32Constants
{
    /// <summary>
    /// Edit control message constants, as defined in Winuser.h (via Windows.h).
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ff485923(v=vs.85).aspx"/>
    public static class EM
    {
        /// <summary>
        /// EM_LINESCROLL message: Scrolls the text in a multiline edit control.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb761615(v=vs.85).aspx"/>
        public const int LINESCROLL = 0x00B6;

        /// <summary>
        /// EM_GETLINECOUNT message: Gets the number of lines in a multiline edit control.You can send this message
        /// to either an edit control or a rich edit control.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb761586(v=vs.85).aspx"/>
        public const int GETLINECOUNT = 0x00BA;

        /// <summary>
        /// EM_GETFIRSTVISIBLELINE message: Gets the zero-based index of the uppermost visible line in a multiline
        /// edit control. You can send this message to either an edit control or a rich edit control.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/bb761574(v=vs.85).aspx"/>
        public const int GETFIRSTVISIBLELINE = 0x00CE;
    }

    /// <summary>
    /// <see cref="WM.NCHITTEST"/> message result constants, as defined in Winuser.h (via Windowsx.h).
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645618(v=vs.85).aspx"/>
    /// <seealso href="http://www.pinvoke.net/default.aspx/Structures/HitTestValues.html"/>
    public static class HT
    {
        /// <summary>
        /// HTERROR means the coordinates are on the screen background or on a dividing line between windows (same as
        /// <see cref="HTNOWHERE"/>, except the DefWindowProc function produces a system beep to indicate an error).
        /// </summary>
        public const int ERROR = -2;

        /// <summary>
        /// HTTRANSPARENT means that the window is currently obscured by another window in the same thread. The message
        /// will be sent to the underlying windows in the same thread until one returns a result other than HTTRANSPARENT.
        /// </summary>
        public const int TRANSPARENT = -1;

        /// <summary>
        /// HTNOWHERE means the coordinates are on the screen background or on a dividing line between windows.
        /// </summary>
        public const int NOWHERE = 0;

        /// <summary>
        /// HTCLIENT means the coordinates are in the client area.
        /// </summary>
        public const int CLIENT = 1;

        /// <summary>
        /// HTCAPTION means the coordinates are in a title bar.
        /// </summary>
        public const int CAPTION = 2;

        /// <summary>
        /// HTSYSMENU means the coordinates are in a window menu or in a Close button in a child window.
        /// </summary>
        public const int SYSMENU = 3;

        /// <summary>
        /// HTGROWBOX means the coordinates are in a size box (same as <see cref="SIZE"/>).
        /// </summary>
        public const int GROWBOX = 4;

        /// <summary>
        /// HTSIZE means the coordinates are in a size box (same as <see cref="GROWBOX"/>).
        /// </summary>
        public const int SIZE = GROWBOX;

        /// <summary>
        /// HTMENU means the coordinates are in a menu.
        /// </summary>
        public const int MENU = 5;

        /// <summary>
        /// HTLEFT means the coordinates are in the left border of a resizable window.
        /// </summary>
        public const int LEFT = 10;

        /// <summary>
        /// HTRIGHT means the coordinates are in the right border of a resizable window.
        /// </summary>
        public const int RIGHT = 11;

        /// <summary>
        /// HTTOP means the coordinates are in the upper-horizontal border of a window.
        /// </summary>
        public const int TOP = 12;

        /// <summary>
        /// HTTOPLEFT means the coordinates are in the upper-left corner of a window border.
        /// </summary>
        public const int TOPLEFT = 13;

        /// <summary>
        /// HTTOPRIGHT means the coordinates are in the upper-right corner of a window border.
        /// </summary>
        public const int TOPRIGHT = 14;

        /// <summary>
        /// HTBOTTOM means the coordinates are in the lower-horizontal border of a resizable window.
        /// </summary>
        public const int BOTTOM = 15;

        /// <summary>
        /// HTBOTTOMLEFT means the coordinates are in the lower-left corner of a resizable window.
        /// </summary>
        public const int BOTTOMLEFT = 16;

        /// <summary>
        /// HTBOTTOMRIGHT means the coordinates are in the lower-right corner of a border of a resizable window.
        /// </summary>
        public const int BOTTOMRIGHT = 17;

        /// <summary>
        /// HTBORDER means the coordinates are in the border of a window that does not have a sizing border.
        /// </summary>
        public const int BORDER = 18;
    }

    /// <summary>
    /// Menu flag constants for use with AppendMenu, InsertMenuItem, etc. win32 calls, as defined in Winuser.h (via Windows.h).
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms647616(v=vs.85).aspx"/>
    public static class MF
    {
        /// <summary>
        /// MF_STRING: the menu item contains a string (conflicts with MF_BITMAP and/or MF_OWNERDRAW).
        /// </summary>
        public const int STRING = 0x00000000;

        /// <summary>
        /// MF_SEPARATOR: draws a horizontal dividing line. This flag is used only in a drop-down menu, submenu, or shortcut menu. The line cannot
        /// be grayed, disabled, or highlighted.
        /// </summary>
        public const int SEPARATOR = 0x00000800;
    }

    /// <summary>
    /// Windows message identifier constants, as defined in Winuser.h (via Windows.h) and as (mostly) implemented in Mono.
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms644927(v=vs.85).aspx"/>
    /// <seealso href="http://www.pinvoke.net/default.aspx/Constants.WM"/>
    public static class WM
    {
        /// <summary>
        /// The WM_MOVE message is sent after a window has been moved.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632631(v=vs.85).aspx"/>
        public const int MOVE = 0x0003;

        /// <summary>
        /// The WM_SIZE message is sent to a window after its size has changed.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632646(v=vs.85).aspx"/>
        public const int SIZE = 0x0005;

        /// <summary>
        /// The WM_PAINT message is sent to a window or control when it needs to be repainted.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd145213(v=vs.85).aspx"/>
        public const int PAINT = 0x000F;

        /// <summary>
        /// An application sends the WM_FONTCHANGE message to all top-level windows in the system after changing the
        /// pool of font resources.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd145211(v=vs.85).aspx"/>
        public const int FONTCHANGE = 0x001D;

        /// <summary>
        /// An application sends a WM_SETFONT message to specify the font that a control is to use when drawing text.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632642(v=vs.85).aspx"/>
        public const int SETFONT = 0x0030;

        /// <summary>
        /// The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or
        /// released. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise,
        /// the message is sent to the window that has captured the mouse.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645618(v=vs.85).aspx"/>
        public const int NCHITTEST = 0x0084;

        /// <summary>
        /// The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the
        /// window. This message is posted to the window that contains the cursor. If a window has captured the mouse,
        /// this message is not posted.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645627(v=vs.85).aspx"/>
        public const int NCMOUSEMOVE = 0x00A0;

        /// <summary>
        /// The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is
        /// within the nonclient area of a window. This message is posted to the window that contains the cursor. If a
        /// window has captured the mouse, this message is not posted.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645620(v=vs.85).aspx"/>
        public const int NCLBUTTONDOWN = 0x00A1;

        /// <summary>
        /// The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is
        /// within the nonclient area of a window. This message is posted to the window that contains the cursor.
        /// If a window has captured the mouse, this message is not posted.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645621(v=vs.85).aspx"/>
        public const int NCLBUTTONUP = 0x00A2;

        /// <summary>
        /// The WM_NCLBUTTONDBLCLK is posted when the user double-clicks the left mouse button while the cursor is
        /// within the nonclient area of a window. This message is posted to the window that contains the cursor.
        /// If a window has captured the mouse, this message is not posted.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645619(v=vs.85).aspx"/>
        public const int NCLBUTTONDBLCLK = 0x00A3;

        // A4 - A6: WM_NC R BUTTON down|up|dblclk
        // A7 - A9: WM_NC M BUTTON down|up|dblclk

        /// <summary>
        /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is pressed.
        /// A nonsystem key is a key that is pressed when the ALT key is not pressed.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646280(v=vs.85).aspx"/>
        public const int KEYDOWN = 0x0100;

        /// <summary>
        /// The WM_CHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated
        /// by the TranslateMessage function. The WM_CHAR message contains the character code of the key that was pressed.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646276(v=vs.85).aspx"/>
        public const int CHAR = 0x0102;

        /// <summary>
        /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user presses the F10
        /// key (which activates the menu bar) or holds down the ALT key and then presses another key. It also occurs
        /// when no window currently has the keyboard focus; in this case, the WM_SYSKEYDOWN message is sent to the
        /// active window. The window that receives the message can distinguish between these two contexts by checking
        /// the context code in the lParam parameter.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646286(v=vs.85).aspx"/>
        public const int SYSKEYDOWN = 0x0104;

        /// <summary>
        /// The WM_SYSCOMMAND message is posted to the window when the user chooses a command from the Window menu
        /// (formerly known as the system or control menu) or when the user chooses the maximize button, minimize
        /// button, restore button, or close button.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646360(v=vs.85).aspx"/>
        public const int SYSCOMMAND = 0x0112;

        /// <summary>
        /// The WM_MOUSEMOVE message is posted to a window when the cursor moves. If the mouse is not captured, the
        /// message is posted to the window that contains the cursor. Otherwise, the message is posted to the window
        /// that has captured the mouse.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645616(v=vs.85).aspx"/>
        public const int MOUSEMOVE = 0x0200;

        /// <summary>
        /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the
        /// client area of a window. If the mouse is not captured, the message is posted to the window beneath the
        /// cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645607(v=vs.85).aspx"/>
        public const int LBUTTONDOWN = 0x0201;

        /// <summary>
        /// The WM_LBUTTONUP message is posted when the user releases the left mouse button while the cursor is in the
        /// client area of a window. If the mouse is not captured, the message is posted to the window beneath the
        /// cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645608(v=vs.85).aspx"/>
        public const int LBUTTONUP = 0x0202;
    }
}
