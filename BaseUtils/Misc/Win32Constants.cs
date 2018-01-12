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

namespace BaseUtils.Win32Constants
{
    #region window Class Styles (CS_*)

    /// <summary>
    /// Window Class Style constants, as defined in Winuser.h (via Windows.h). Useful for manipulating
    /// <see cref="System.Windows.Forms.CreateParams.ClassStyle"/>.
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ff729176(v=vs.85).aspx"/>
    public static class CS
    {
        /// <summary>
        /// CS_VREDRAW: Redraws the entire window if a movement or size adjustment changes the height of the client
        /// area.
        /// </summary>
        public const int VREDRAW = 0x0001;

        /// <summary>
        /// CS_HREDRAW: Redraws the entire window if a movement or size adjustment changes the width of the client
        /// area.
        /// </summary>
        public const int HREDRAW = 0x0002;

        /// <summary>
        /// CS_DBLCLKS: Sends a double-click message to the window procedure when the user double-clicks the mouse
        /// while the cursor is within a window belonging to the class. Also allows for minimize/restore operations
        /// when clicking a <see cref="System.Windows.Forms.Form"/>'s TaskBar icon.
        /// </summary>
        public const int DBLCLKS = 0x0008;
    }

    #endregion

    #region Edit control Messages (EM_*)

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

    #endregion

    #region window Hook Codes (HC_*)

    /// <summary>
    /// window Hook Codes as defined in Winuser.h (via windows.h), for use in a WH_CALLWNDPROC hook procedure while
    /// conducting unsactioned witchcraft alongside a supervisory greybeard.
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms644959(v=vs.85).aspx"/>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms644990(v=vs.85).aspx"/>
    public static class HC
    {
        public const int
            ACTION = 0,
            GETNEXT = 1,
            SKIP = 2;
    }

    #endregion

    #region HitTest results (HT*)

    /// <summary>
    /// <see cref="WM.NCHITTEST"/> message result constants, as defined in Winuser.h (via Windowsx.h).
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645618(v=vs.85).aspx"/>
    /// <seealso href="http://www.pinvoke.net/default.aspx/Structures/HitTestValues.html"/>
    public static class HT
    {
        /// <summary>
        /// HTERROR means the coordinates are on the screen background or on a dividing line between windows (same as HTNOWHERE,
        /// except that the DefWindowProc function produces a system beep to indicate an error).
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
        public const int SIZE = 4;

        /// <summary>
        /// HTMENU means the coordinates are in a menu.
        /// </summary>
        /// <seealso cref="SYSMENU"/>
        public const int MENU = 5;

        /// <summary>
        /// HTHSCROLL means the coordinates are in the horizontal scroll bar.
        /// </summary>
        public const int HSCROLL = 6;

        /// <summary>
        /// HTVSCROLL means the coordinates are in the vertical scroll bar.
        /// </summary>
        public const int VSCROLL = 7;

        /// <summary>
        /// HTMINBUTTON means the coordinates are in a Minimize button (same as <see cref="REDUCE"/>).
        /// </summary>
        public const int MINBUTTON = 8;

        /// <summary>
        /// HTMINBUTTON means the coordinates are in a Minimize button (same as <see cref="MINBUTTON"/>).
        /// </summary>
        public const int REDUCE = 8;

        /// <summary>
        /// HTMAXBUTTON means the coordinates are in a Maximize button (same as <see cref="ZOOM"/>).
        /// </summary>
        public const int MAXBUTTON = 9;

        /// <summary>
        /// HTMAXBUTTON means the coordinates are in a Maximize button (same as <see cref="MAXBUTTON"/>).
        /// </summary>
        public const int ZOOM = 9;

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
        /// HTBOTTOMLEFT means the coordinates are in the lower-left corner of a border of a resizable window.
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

        /// <summary>
        /// HTCLOSE means the coordinates are in a Close button.
        /// </summary>
        public const int CLOSE = 20;

        /// <summary>
        /// HTHELP means the coordinates are in a Help button.
        /// </summary>
        public const int HELP = 21;
    }

    #endregion

    #region Menu Flags (MF_*)

    /// <summary>
    /// Menu flag constants for use with AppendMenu, InsertMenuItem, etc. win32 calls, as defined in Winuser.h (via Windows.h).
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms647616(v=vs.85).aspx"/>
    public static class MF
    {
        /// <summary>
        /// Indicates that uIDEnableItem gives the identifier of the menu item. If neither the MF_BYCOMMAND nor MF_BYPOSITION flag is specified, the MF_BYCOMMAND flag is the default flag.
        /// </summary>
        public const int BYCOMMAND = 0x00000000;

        /// <summary>
        /// Indicates that the menu item is enabled and restored from a grayed state so that it can be selected.
        /// </summary>
        public const int ENABLED = 0x00000000;

        /// <summary>
        /// MF_STRING: the menu item contains a string (conflicts with MF_BITMAP and/or MF_OWNERDRAW).
        /// </summary>
        public const int STRING = 0x00000000;

        /// <summary>
        /// MF_UNCHECKED: Does not place a check mark next to the item.
        /// </summary>
        public const int UNCHECKED = 0x00000000;

        /// <summary>
        /// Indicates that the menu item is disabled and grayed so that it cannot be selected.
        /// </summary>
        public const int GRAYED = 0x00000001;

        /// <summary>
        /// Indicates that the menu item is disabled, but not grayed, so it cannot be selected.
        /// </summary>
        public const int DISABLED = 0x00000002;

        /// <summary>
        /// MF_CHECKED: Places a check mark next to the item.
        /// </summary>
        public const int CHECKED = 0x00000008;

        /// <summary>
        /// MF_POPUP: The menu item opens a drop-down menu or submenu. uIDNewItem should be a handle to the submenu.
        /// </summary>
        public const int POPUP = 0x00000010;

        /// <summary>
        /// Indicates that uIDEnableItem gives the zero-based relative position of the menu item.
        /// </summary>
        public const int BYPOSITION = 0x00000400;

        /// <summary>
        /// MF_SEPARATOR: draws a horizontal dividing line. This flag is used only in a drop-down menu, submenu, or shortcut menu. The line cannot
        /// be grayed, disabled, or highlighted.
        /// </summary>
        public const int SEPARATOR = 0x00000800;
    }

    #endregion

    #region System Commands (SC_*)

    /// <summary>
    /// System Command constants, for use with <see cref="WM.SYSCOMMAND"/>, as defined in Winuser.h (via Windows.h).
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646360(v=vs.85).aspx"/>
    public static class SC
    {
        /// <summary>
        /// Sizes the window.
        /// </summary>
        public const int SIZE = 0xF000;

        /// <summary>
        /// Moves the window.
        /// </summary>
        public const int MOVE = 0xF010;

        /// <summary>
        /// Minimizes the window.
        /// </summary>
        public const int MINIMIZE = 0xF020;

        /// <summary>
        /// Maximizes the window.
        /// </summary>
        public const int MAXIMIZE = 0xF030;

        /// <summary>
        /// Moves to the next window.
        /// </summary>
        public const int NEXTWINDOW = 0xF040;

        /// <summary>
        /// Moves to the previous window.
        /// </summary>
        public const int PREVWINDOW = 0xF050;

        /// <summary>
        /// Closes the window.
        /// </summary>
        public const int CLOSE = 0xF060;

        /// <summary>
        /// Scrolls vertically.
        /// </summary>
        public const int VSCROLL = 0xF070;

        /// <summary>
        /// Scrolls horizontally.
        /// </summary>
        public const int HSCROLL = 0xF080;

        /// <summary>
        /// Retrieves the window menu as a result of a mouse click.
        /// </summary>
        public const int MOUSEMENU = 0xF090;

        /// <summary>
        /// Retrieves the window menu as a result of a keystroke. For more information, see the Remarks section.
        /// </summary>
        public const int KEYMENU = 0xF100;

        public const int ARRANGE = 0xF110;  // TODO: SC_ARRANGE is undocumented

        /// <summary>
        /// Restores the window to its normal position and size.
        /// </summary>
        public const int RESTORE = 0xF120;

        /// <summary>
        /// Activates the Start menu.
        /// </summary>
        public const int TASKLIST = 0xF130;

        /// <summary>
        /// Executes the screen saver application specified in the [boot] section of the System.ini file.
        /// </summary>
        public const int SCREENSAVE = 0xF140;

        /// <summary>
        /// Activates the window associated with the application-specified hot key. The lParam parameter identifies the window to activate.
        /// </summary>
        public const int HOTKEY = 0xF150;

        /// <summary>
        /// Selects the default item; the user double-clicked the window menu.
        /// </summary>
        public const int DEFAULT = 0xF160;

        /// <summary>
        /// Sets the state of the display. This command supports devices that have power-saving features, such as a battery-powered personal computer.
        /// The lParam parameter can have the following values:
        /// -1: the display is powering on; 1: the display is going to low power; 2: the display is being shut off.
        /// </summary>
        public const int MONITORPOWER = 0xF170;

        /// <summary>
        /// Changes the cursor to a question mark with a pointer. If the user then clicks a control in the dialog box, the control receives a <see cref="WM.HELP"/> message.
        /// </summary>
        public const int CONTEXTHELP = 0xF180;
    }

    #endregion

    #region System Metrics (SM_*)

    /// <summary>
    /// Constants for use with <see cref="Win32.UnsafeNativeMethods.GetSystemMetrics(SystemMetrics)"/>.
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms724385(v=vs.85).aspx"/>
    public enum SystemMetrics : int
    {
        CXSCREEN = 0,
        CYSCREEN, CXVSCROLL, CYHSCROLL, CYCAPTION, CXBORDER = 5,
        CYBORDER, CXDLGFRAME = 7, CXFIXEDFRAME = 7, CYDLGFRAME = 8, CYFIXEDFRAME = 8, CYVTHUMB, CXHTHUMB = 10,
        CXICON, CYICON, CXCURSOR, CYCURSOR, CYMENU = 15,
        CXFULLSCREEN, CYFULLSCREEN, CYKANJIWINDOW, MOUSEPRESENT, CYVSCROLL = 20,
        CXHSCROLL, DEBUG, SWAPBUTTON, UNSUPPORTED_24, UNSUPPORTED_25 = 25,
        UNSUPPORTED_26, UNSUPPORTED_27, CXMIN, CYMIN, CXSIZE = 30,
        CYSIZE, CXFRAME = 32, CXSIZEFRAME = 32, CYFRAME = 33, CYSIZEFRAME = 33, CXMINTRACK, CYMINTRACK = 35,
        CXDOUBLECLK, CYDOUBLECLK, CXICONSPACING, CYICONSPACING, MENUDROPALIGNMENT = 40,
        PENWINDOWS, DBCSENABLED, CMOUSEBUTTONS, SECURE, CXEDGE = 45,
        CYEDGE, CXMINSPACING, CYMINSPACING, CXSMICON, CYSMICON = 50,
        CYSMCAPTION, CXSMSIZE, CYSMSIZE, CXMENUSIZE, CYMENUSIZE = 55,
        ARRANGE, CXMINIMIZED, CYMINIMIZED, CXMAXTRACK, CYMAXTRACK = 60,
        CXMAXIMIZED, CYMAXIMIZED, NETWORK, UNSUPPORTED_64, UNSUPPORTED_65 = 65,
        UNSUPPORTED_66, CLEANBOOT, CXDRAG, CYDRAG, SHOWSOUNDS = 70,
        CXMENUCHECK, CYMENUCHECK, SLOWMACHINE, MIDEASTENABLED, MOUSEWHEELPRESENT = 75,
        XVIRTUALSCREEN, YVIRTUALSCREEN, CXVIRTUALSCREEN, CYVIRTUALSCREEN, CMONITORS = 80,
        SAMEDISPLAYFORMAT, IMMENABLED, CXFOCUSBORDER, CYFOCUSBORDER, UNSUPPORTED_85 = 85,
        TABLETPC, MEDIACENTER, STARTER, SERVERR2, UNSUPPORTED_90 = 90,
        MOUSEHORIZONTALWHEELPRESENT, CXPADDEDBORDER, UNSUPPORTED_93, DIGITIZER, MAXIMUMTOUCHES = 95,
        REMOTESESSION = 0x1000, SHUTTINGDOWN = 0x2000, REMOTECONTROL = 0x2001, CONVERTIBLESLATEMODE = 0x2003, SYSTEMDOCKED = 0x2004,
    }

    #endregion

    #region Track Popup Menu (TPM_*)

    /// <summary>
    /// Track Popup Menu constants, for use with TrackPopupMenu, as defined in Winuser.h (via Windows.h).
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms648002(v=vs.85).aspx"/>
    public static class TPM
    {
        /// <summary>
        /// Positions the shortcut menu so that its left side is aligned with the coordinate specified by the x parameter.
        /// </summary>
        public const int LEFTALIGN = 0x0000;

        /// <summary>
        /// The user can select menu items with only the left mouse button.
        /// </summary>
        public const int LEFTBUTTON = 0x0000;

        /// <summary>
        /// The user can select menu items with both the left and right mouse buttons.
        /// </summary>
        public const int RIGHTBUTTON = 0x0002;

        /// <summary>
        /// Centers the shortcut menu horizontally relative to the coordinate specified by the x parameter.
        /// </summary>
        public const int CENTERALIGN = 0x0004;

        /// <summary>
        /// Positions the shortcut menu so that its right side is aligned with the coordinate specified by the x parameter.
        /// </summary>
        public const int RIGHTALIGN = 0x0008;

        /// <summary>
        /// Positions the shortcut menu so that its top side is aligned with the coordinate specified by the y parameter.
        /// </summary>
        public const int TOPALIGN = 0x0000;

        /// <summary>
        /// Centers the shortcut menu vertically relative to the coordinate specified by the y parameter.
        /// </summary>
        public const int VCENTERALIGN = 0x0010;

        /// <summary>
        /// Positions the shortcut menu so that its bottom side is aligned with the coordinate specified by the y parameter.
        /// </summary>
        public const int BOTTOMALIGN = 0x0020;

        public const int HORIZONTAL = 0x0000;   /* Horz alignment matters more */
        public const int VERTICAL = 0x0040;     /* Vert alignment matters more */

        /// <summary>
        /// The function does not send notification messages when the user clicks a menu item.
        /// </summary>
        public const int NONOTIFY = 0x0080;

        /// <summary>
        /// The function returns the menu item identifier of the user's selection in the return value.
        /// </summary>
        public const int RETURNCMD = 0x0100;
    }

    #endregion

    #region Window Hooks (WH_*)

    /// <summary>
    /// Windows Hook type identifiers as defined in Winuser.h (via windows.h), for use with `SetWindowsHook()`.
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms644959(v=vs.85).aspx"/>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms644990(v=vs.85).aspx"/>
    public static class WH
    {
        /// <summary>
        /// The WH_JOURNALPLAYBACK hook enables an application to insert messages into the system message queue. You
        /// can use this hook to play back a series of mouse and keyboard events recorded earlier. Regular mouse and
        /// keyboard input is disabled as long as a WH_JOURNALPLAYBACK hook is installed. A WH_JOURNALPLAYBACK hook is
        /// a global hook; it cannot be used as a thread-specific hook.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms644959(v=vs.85).aspx#wh_journalplaybackhook"/>
        public const int JOURNALPLAYBACK = 1;
    }

    #endregion

    #region Window Messages (WM_*)

    /// <summary>
    /// Windows message identifier constants, as defined in Winuser.h (via Windows.h) and as (mostly) implemented in Mono.
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms644927(v=vs.85).aspx"/>
    /// <seealso href="http://www.pinvoke.net/default.aspx/Constants.WM"/>
    public static class WM
    {
        /// <summary>
        /// The WM_NULL message performs no operation.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632637(v=vs.85).aspx"/>
        public const int NULL = 0x0000;

        /// <summary>
        /// The WM_CREATE message is sent when an application requests that a window be created. The window procedure
        /// of the new window receives this message after the window is created, but before the window becomes visible.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632619(v=vs.85).aspx"/>
        public const int CREATE = 0x0001;

        /// <summary>
        /// The WM_DESTROY message is sent when a window is being destroyed.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632620(v=vs.85).aspx"/>
        public const int DESTROY = 0x0002;

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
        /// The WM_ACTIVATE message is sent to both the window being activated and the window being deactivated. If the
        /// windows use the same input queue, the message is sent synchronously, first to the window procedure of the
        /// top-level window being deactivated, then to the window procedure of the top-level window being activated.
        /// If the windows use different input queues, the message is sent asynchronously, so the window is activated
        /// immediately.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646274(v=vs.85).aspx"/>
        public const int ACTIVATE = 0x0006;

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
        /// WM_GETMINMAXINFO message is sent to a window when the size or position of the window is about to change. An
        /// application can use this message to override the window's default maximized size and position, or its
        /// default minimum or maximum tracking size. wParam is not used, while lParam is a pointer to a
        /// <see cref="Win32.UnsafeNativeMethods.MINMAXINFO"/> struct.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632626(v=vs.85).aspx"/>
        public const int GETMINMAXINFO = 0x0024;

        /// <summary>
        /// An application sends a WM_SETFONT message to specify the font that a control is to use when drawing text.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632642(v=vs.85).aspx"/>
        public const int SETFONT = 0x0030;

        /// <summary>
        /// The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z order is about
        /// to change as a result of a call to the SetWindowPos function or another window-management function. wParam
        /// is not used, while lParam is a pointer to a WINDOWPOS structure defining the new position/size.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632653(v=vs.85).aspx"/>
        public const int WINDOWPOSCHANGING = 0x0046;

        /// <summary>
        /// The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the Z order has
        /// changed as a result of a call to the SetWindowPos function or another window-management function. wParam is
        /// not used, while lParam is a pointer to a WINDOWPOS structure defining the new position/size.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632652(v=vs.85).aspx"/>
        public const int WINDOWPOSCHANGED = 0x0047;

        /// <summary>
        /// The WM_CANCELJOURNAL message is sent when the user cancel's the application's journaling activities. hWnd,
        /// wParam, and lParam are all unused. Important: this value is NOT designed to be used within a
        /// <see cref="System.Windows.Forms.Form.WndProc"/>; see the linked documentation for more information.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms644971(v=vs.85).aspx"/>
        public const int CANCELJOURNAL = 0x004B;

        /// <summary>
        /// An application sends the WM_NCCALCSIZE message when the size and position of a window's client area must be
        /// calculated. By processing this message, an application can control the content of the window's client area
        /// when the size or position of the window changes.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632634(v=vs.85).aspx"/>
        public const int NCCALCSIZE = 0x0083;

        /// <summary>
        /// The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or
        /// released. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise,
        /// the message is sent to the window that has captured the mouse.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645618(v=vs.85).aspx"/>
        public const int NCHITTEST = 0x0084;

        /// <summary>
        /// The WM_NCPAINT message is sent to a window when its frame must be painted.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/dd145212(v=vs.85).aspx"/>
        public const int NCPAINT = 0x0085;

        /// <summary>
        /// The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to indicate an
        /// active or inactive state.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632633(v=vs.85).aspx"/>
        public const int NCACTIVATE = 0x0086;

        /// <summary>
        /// The WM_SYNCPAINT message is used to synchronize painting while avoiding linking independent GUI threads.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/dd145221(v=vs.85).aspx"/>
        public const int SYNCPAINT = 0x0088;

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
        /// The WM_NCLBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the
        /// cursor is within the nonclient area of a window. This message is posted to the window that contains
        /// the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645619(v=vs.85).aspx"/>
        public const int NCLBUTTONDBLCLK = 0x00A3;

        /// <summary>
        /// The WM_NCRBUTTONDOWN message is posted when the user pressed the right mouse button while the cursor
        /// is within the nonclient area of a window. This message is posted to the window that contains the
        /// cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645629(v=vs.85).aspx"/>
        public const int NCRBUTTONDOWN = 0x00A4;

        /// <summary>
        /// The WM_NCRBUTTONUP message is posted when the user releases the right mouse button while the cursor
        /// is within the nonclient area of a window. The message is posted to the window that contains the
        /// cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645630(v=vs.85).aspx"/>
        public const int NCRBUTTONUP = 0x00A5;

        /// <summary>
        /// The WM_NCRBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the
        /// cursor is within the nonclient area of a window. This message is posted to the window that contains
        /// the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645628(v=vs.85).aspx"/>
        public const int NCRBUTTONDBLCLK = 0x00A6;

        /// <summary>
        /// The WM_NCMBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor
        /// is within the nonclient area of a window. This message is posted to the window that contains the
        /// cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645623(v=vs.85).aspx"/>
        public const int NCMBUTTONDOWN = 0x00A7;

        /// <summary>
        /// The WM_NCMBUTTONUP message is posted when the user releases the middle mouse button while the cursor
        /// is within the nonclient area of a window. The message is posted to the window that contains the
        /// cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645624(v=vs.85).aspx"/>
        public const int NCMBUTTONUP = 0x00A8;

        /// <summary>
        /// The WM_NCMBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the
        /// cursor is within the nonclient area of a window. This message is posted to the window that contains
        /// the cursor. If a window has captured the mouse, this message is not posted.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645619(v=vs.85).aspx"/>
        public const int NCMBUTTONDBLCLK = 0x00A9;

        /// <summary>
        /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is pressed.
        /// A nonsystem key is a key that is pressed when the ALT key is not pressed.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646280(v=vs.85).aspx"/>
        public const int KEYDOWN = 0x0100;

        /// <summary>
        /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem key is released. A
        /// nonsystem key is a key that is pressed when the ALT key is not pressed, or a keyboard key that is pressed
        /// when a window has the keyboard focus.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646281(v=vs.85).aspx"/>
        public const int KEYUP = 0x0101;

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
        /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user releases a key that
        /// was pressed while the ALT key was held down.It also occurs when no window currently has the keyboard focus;
        /// in this case, the WM_SYSKEYUP message is sent to the active window.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646287(v=vs.85).aspx"/>
        public const int SYSKEYUP = 0x0105;

        /// <summary>
        /// The WM_SYSCOMMAND message is posted to the window when the user chooses a command from the Window menu
        /// (formerly known as the system or control menu) or when the user chooses the maximize button, minimize
        /// button, restore button, or close button.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646360(v=vs.85).aspx"/>
        public const int SYSCOMMAND = 0x0112;

        /// <summary>
        /// The WM_INITMENU message is sent when a menu is about to become active. It occurs when the user clicks an
        /// item on the menu bar or presses a menu key. This allows the application to modify the menu before it is
        /// displayed.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646344(v=vs.85).aspx"/>
        public const int INITMENU = 0x0116;

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

        /// <summary>
        /// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is
        /// in the client area of a window. If the mouse is not captured, the message is posted to the window beneath
        /// the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645606(v=vs.85).aspx"/>
        public const int LBUTTONDBLCLK = 0x0203;

        /// <summary>
        /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is in the
        /// client area of a window. If the mouse is not captured, the message is posted to the window beneath the
        /// cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646242(v=vs.85).aspx"/>
        public const int RBUTTONDOWN = 0x0204;

        /// <summary>
        /// The WM_RBUTTONUP message is posted when the user releases the right mouse button while the cursor is in the
        /// client area of a window. If the mouse is not captured, the message is posted to the window beneath the
        /// cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646243(v=vs.85).aspx"/>
        public const int RBUTTONUP = 0x0205;

        /// <summary>
        /// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is
        /// in the client area of a window. If the mouse is not captured, the message is posted to the window beneath
        /// the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646241(v=vs.85).aspx"/>
        public const int RBUTTONDBLCLK = 0x0206;

        /// <summary>
        /// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is in the
        /// client area of a window. If the mouse is not captured, the message is posted to the window beneath the
        /// cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645610(v=vs.85).aspx"/>
        public const int MBUTTONDOWN = 0x0207;

        /// <summary>
        /// The WM_MBUTTONUP message is posted when the user releases the middle mouse button while the cursor is in the
        /// client area of a window. If the mouse is not captured, the message is posted to the window beneath the
        /// cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645611(v=vs.85).aspx"/>
        public const int MBUTTONUP = 0x0208;

        /// <summary>
        /// The WM_MBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is
        /// in the client area of a window. If the mouse is not captured, the message is posted to the window beneath
        /// the cursor. Otherwise, the message is posted to the window that has captured the mouse.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645609(v=vs.85).aspx"/>
        public const int MBUTTONDBLCLK = 0x0209;

        /// <summary>
        /// The WM_NCMOUSEHOVER message is posted to a window when the cursor hovers over the nonclient area of the
        /// window for the period of time specified in a prior call to <c>TrackMouseEvent</c>.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645625(v=vs.85).aspx"/>
        public const int NCMOUSEHOVER = 0x02A0;

        /// <summary>
        /// The WM_MOUSEHOVER message is posted to a window when the cursor hovers over the client area of the window
        /// for the period of time specified in a prior call to <c>TrackMouseEvent</c>.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645613(v=vs.85).aspx"/>
        public const int MOUSEHOVER = 0x02A1;

        /// <summary>
        /// The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the window
        /// specified in a prior call to <c>TrackMouseEvent</c>.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645626(v=vs.85).aspx"/>
        public const int NCMOUSELEAVE = 0x02A2;

        /// <summary>
        /// The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of the window
        /// specified in a prior call to <c>TrackMouseEvent</c>.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms645615(v=vs.85).aspx"/>
        public const int MOUSELEAVE = 0x02A3;

        /// <summary>
        /// The WM_PRINT message is sent to a window to request that it draw itself in the specified device context,
        /// most commonly in a printer device context. wParam contains a handle to the device context to draw in, while
        /// lParam contains the drawing options. See the remarks section for more information.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/dd145216(v=vs.85).aspx"/>
        public const int PRINT = 0x0317;

        /// <summary>
        /// The WM_PRINTCLIENT message is sent to a window to request that it draw its client area in the specified
        /// device context, most commonly in a printer device context. Unlike WM_PRINT, WM_PRINTCLIENT is not processed
        /// by DefWindowProc. A window should process the WM_PRINTCLIENT message through an application-defined
        /// WindowProc function for it to be used properly. wParam contains a handle to the device context to draw in,
        /// while lParam has drawing options. See the remarks section for more information.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/dd145217(v=vs.85).aspx"/>
        public const int PRINTCLIENT = 0x0318;

        /// <summary>
        /// The WM_APPCOMMAND message notifies a window that the user generated an application command event, for
        /// example, by clicking an application command button using the mouse or typing an application command key on
        /// the keyboard. wParam contains a handle to the window where the user clicked the button or pressed the key.
        /// See the remarks section for a descrption of lParam values.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms646275(v=vs.85).aspx"/>
        public const int APPCOMMAND = 0x0319;

        /// <summary>
        /// The WM_THEMECHANGED message is broadcast to every window following a theme change event. Examples of theme
        /// change events are the activation of a theme, the deactivation of a theme, or a transition from one theme to
        /// another. Both wParam and lParam are reserved and should not be modified.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632650(v=vs.85).aspx"/>
        public const int THEMECHANGED = 0x031A;

        /// <summary>
        /// The WM_CLIPBOARDUPDATE message is sent when the contents of the clipboard have changed. Neither wParam nor
        /// lParam are used and both must be zero.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms649021(v=vs.85).aspx"/>
        public const int CLIPBOARDUPDATE = 0x031D;

        /// <summary>
        /// The WM_DWMCOMPOSITIONCHANGED message is sent to all top-level windows that Desktop Window Manager (DWM)
        /// composition has been enabled or disabled. Note: as of Windows 8, DWM composition is (almost) always enabled,
        /// so this message is not sent regardless of video mode changes.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd388199(v=vs.85).aspx"/>
        public const int DWMCOMPOSITIONCHANGED = 0x031E;

        /// <summary>
        /// The WM_DWMNCRENDERINGCHANGED message is sent when the non-client area rendering policy has changed.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd388200(v=vs.85).aspx"/>
        public const int DWMNCRENDERINGCHANGED = 0x031F;

        /// <summary>
        /// The WM_DWMCOLORIZATIONCOLORCHANGED message informs all top-level windows that the colorization color
        /// has changed. wParam specifies the new colorization color in 0xAARRGGBB format, while lParam
        /// specifies whether the new color is blended with opacity.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd388198(v=vs.85).aspx"/>
        public const int DWMCOLORIZATIONCOLORCHANGED = 0x0320;

        /// <summary>
        /// The WM_DWMWINDOWMAXIMIZEDCHANGE message is sent when a Desktop Window Manager (DWM) composed window is
        /// maximized. wParam is set to TRUE when the window has been maximized, while lParam is not used.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd388201(v=vs.85).aspx"/>
        public const int DWMWINDOWMAXIMIZEDCHANGE = 0x0321;

        /// <summary>
        /// The WM_DWMSENDICONICTHUMBNAIL message instructs a window to provide a static bitmap to use as a thumbnail
        /// representation of that window.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd938875(v=vs.85).aspx"/>
        public const int DWMSENDICONICTHUMBNAIL = 0x0323;

        /// <summary>
        /// The WM_DWMSENDICONICLIVEPREVIEWBITMAP message instructs a window to provide a static bitmap to use as a
        /// live preview (also known as a Peek preview) of that window.
        /// </summary>
        /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/dd938874(v=vs.85).aspx"/>
        public const int DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326;
    }

    #endregion

    #region Window Styles (WS_*)

    /// <summary>
    /// Window Style constants, as defined in Winuser.h (via Windows.h). Useful for manipulating
    /// <see cref="System.Windows.Forms.CreateParams.Style"/>.
    /// </summary>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms632600(v=vs.85).aspx"/>
    /// <seealso href="https://msdn.microsoft.com/en-us/library/system.windows.forms.control.createparams(v=vs.110).aspx"/>
    public static class WS
    {
        /// <summary>
        /// The window is an overlapped window. An overlapped window has a title bar and a border. Same as the
        /// <see cref="TILED"/> style.
        /// </summary>
        public const int OVERLAPPED = 0x00000000;

        /// <summary>
        /// The windows is a pop-up window. This style cannot be used with the <see cref="CHILD"/> style.
        /// </summary>
        public const int POPUP = unchecked((int)0x80000000);

        /// <summary>
        /// The window is a child window. A window with this style cannot have a menu bar. This style cannot be used
        /// with the <see cref="POPUP"/> style.
        /// </summary>
        public const int CHILD = 0x40000000;

        /// <summary>
        /// The window is initially minimized. Same as the <see cref="ICONIC"/> style.
        /// </summary>
        public const int MINIMIZE = 0x20000000;

        /// <summary>
        /// The window is initially visible.
        /// </summary>
        public const int VISIBLE = 0x10000000;

        /// <summary>
        /// The window is initially disabled. A disabled window cannot receive input from the user. To change this
        /// after a window has been created, use the <c>EnableWindow</c> function.
        /// </summary>
        public const int DISABLED = 0x08000000;

        /// <summary>
        /// Clips child windows relative to each other; that is, when a particular child window receives a <see cref="WM.PAINT"/>
        /// message, the CLIPSIBLINGS style clips all other overlapping child windows out of the region of the child
        /// window to be updated. If CLIPSIBLINGS is not specified and child windows overlap, it is possible, when
        /// drawing within the client area of a child window, to draw within the client area of a neighboring child window.
        /// </summary>
        public const int CLIPSIBLINGS = 0x04000000;

        /// <summary>
        /// Excludes the area occupied by child windows when drawing occurs within the parent window. This style is
        /// used when creating the parent window.
        /// </summary>
        public const int CLIPCHILDREN = 0x02000000;

        /// <summary>
        /// The window is initially maximized.
        /// </summary>
        public const int MAXIMIZE = 0x01000000;

        /// <summary>
        /// The window has a title bar (includes the <see cref="BORDER"/> and <see cref="DLGFRAME"/> styles).
        /// </summary>
        public const int CAPTION = (BORDER | DLGFRAME); // 00C0 0000

        /// <summary>
        /// The window has a thin-line border.
        /// </summary>
        public const int BORDER = 0x00800000;

        /// <summary>
        /// The window has a border of a style typically used with dialog boxes. A window with this style cannot have a
        /// title bar.
        /// </summary>
        public const int DLGFRAME = 0x00400000;

        /// <summary>
        /// The window has a vertical scroll bar.
        /// </summary>
        public const int VSCROLL = 0x00200000;

        /// <summary>
        /// The window has a horizontal scroll bar.
        /// </summary>
        public const int HSCROLL = 0x00100000;

        /// <summary>
        /// The window has a window menu on its title bar. The <see cref="CAPTION"/> style must also be specified.
        /// </summary>
        public const int SYSMENU = 0x00080000;

        /// <summary>
        /// The window has a sizing border. Same as the <see cref="SIZEBOX"/> style.
        /// </summary>
        public const int THICKFRAME = 0x00040000;

        /// <summary>
        /// The window is the first control of a group of controls. The group consists of this first control and all
        /// controls defined after it, up to the next control with the GROUP style. The first control in each group
        /// usually has the <see cref="TABSTOP"/> style so that the user can move from group to group. The user can
        /// subsequently change the keyboard focus from one control in the group to the next control in the group by
        /// using the direction keys.
        /// </summary>
        public const int GROUP = 0x00020000;

        /// <summary>
        /// The window is a control that can receive the keyboard focus when the user presses the TAB key. Pressing the
        /// TAB key changes the keyboard focus to the next control with the <c>TABSTOP</c> style.
        /// </summary>
        public const int TABSTOP = 0x00010000;

        /// <summary>
        /// The window has a minimize button. Cannot be combined with the <see cref="WS_EX.CONTEXTHELP"/> style. The
        /// <see cref="SYSMENU"/> style must also be specified.
        /// </summary>
        public const int MINIMIZEBOX = 0x00020000;

        /// <summary>
        /// The window has a maximize button. Cannot be combined with the <see cref="WS_EX.CONTEXTHELP"/> style. The
        /// <see cref="SYSMENU"/> style must also be specified.
        /// </summary>
        public const int MAXIMIZEBOX = 0x00010000;

        /// <summary>
        /// The window is an overlapped window. An overlapped window has a title bar and a border. Same as the
        /// <see cref="OVERLAPPED"/> style.
        /// </summary>
        public const int TILED = (OVERLAPPED);

        /// <summary>
        /// The window is initially minimized. Same as the <see cref="MINIMIZE"/> style.
        /// </summary>
        public const int ICONIC = (MINIMIZE);

        /// <summary>
        /// The window has a sizing border. Same as the <see cref="THICKFRAME"/> style.
        /// </summary>
        public const int SIZEBOX = (THICKFRAME);

        /// <summary>
        /// The window is an overlapped window. Same as the <see cref="OVERLAPPEDWINDOW"/> style.
        /// </summary>
        public const int TILEDWINDOW = (OVERLAPPEDWINDOW);

        /// <summary>
        /// The window is an overlapped window. Same as the <see cref="TILEDWINDOW"/> style.
        /// </summary>
        public const int OVERLAPPEDWINDOW = (OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX);

        /// <summary>
        /// The window is a pop-up window. The <see cref="CAPTION"/> and <see cref="POPUPWINDOW"/> styles must be
        /// combined to make the window menu visible.
        /// </summary>
        public const int POPUPWINDOW = (POPUP | BORDER | SYSMENU);

        /// <summary>
        /// Same as the <see cref="CHILD"/> style.
        /// </summary>
        public const int CHILDWINDOW = (CHILD);
    }

    public static class WS_EX
    {
        public const int TOPMOST = 0x8;
        public const int TRANSPARENT = 0x20;
        public const int LAYERED = 0x80000;
    }

    #endregion
}
