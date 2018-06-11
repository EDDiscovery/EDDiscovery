using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils.Win32
{
    public class NativeMethods
    {
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public class EVENTMSG
        {
            public int message;
            public int paramL;
            public int paramH;
            public int time;
            public IntPtr hwnd;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public int type;
            public INPUTUNION inputUnion;
        }

        // We need to split the field offset out into a union struct to avoid
        // silent problems in 64 bit
        [StructLayout(LayoutKind.Explicit)]
        public struct INPUTUNION
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }
        public const int INPUT_KEYBOARD = 1;
        public const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const int KEYEVENTF_KEYUP = 0x0002;
        public const int KEYEVENTF_UNICODE = 0x0004;

        public const int VIEW_E_DRAW = unchecked((int)0x80040140),
        VK_PRIOR = 0x21,
        VK_NEXT = 0x22,
        VK_LEFT = 0x25,
        VK_UP = 0x26,
        VK_RIGHT = 0x27,
        VK_DOWN = 0x28,
        VK_TAB = 0x09,
        VK_SHIFT = 0x10,
        VK_CONTROL = 0x11,
        VK_MENU = 0x12,
        VK_CAPITAL = 0x14,
        VK_KANA = 0x15,
        VK_ESCAPE = 0x1B,
        VK_END = 0x23,
        VK_HOME = 0x24,
        VK_NUMLOCK = 0x90,
        VK_SCROLL = 0x91,
        VK_INSERT = 0x002D,
        VK_DELETE = 0x002E;


    }

    public class UnsafeNativeMethods
    {
        public enum GWL
        {
            ExStyle = -20
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern short VkKeyScan(char key);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern short VkKeyScanEx(char key, IntPtr dwhkl);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetKeyNameText(int lParam, [Out] StringBuilder str, int len);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx(int hookid, NativeMethods.HookProc pfnhook, HandleRef hinst, int threadid);
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

        public static int ChangeWindowLong(IntPtr hWnd, GWL nIndex, int mask , int set)     // HELPER!
        {
            int cur = (GetWindowLong(hWnd, nIndex) & ~mask) | set;
            SetWindowLong(hWnd, nIndex, cur);
            //System.Diagnostics.Debug.WriteLine("set exstyle " + cur.ToString("X"));
            return cur;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(HandleRef hhook);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetKeyboardState(byte[] keystate);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetKeyboardState(byte[] keystate);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string modName);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BlockInput([In, MarshalAs(UnmanagedType.Bool)] bool fBlockIt);
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint SendInput(uint nInputs, NativeMethods.INPUT[] pInputs, int cbSize);
        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern short GetAsyncKeyState(int vkey);

        public static object PtrToStructure(IntPtr lparam, Type cls)
        {
            return Marshal.PtrToStructure(lparam, cls);
        }

        public static void PtrToStructure(IntPtr lparam, object data)
        {
            Marshal.PtrToStructure(lparam, data);
        }

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CallNextHookEx(HandleRef hhook, int code, IntPtr wparam, IntPtr lparam);

        [DllImport("User32.dll")]
        public static extern int SetForegroundWindow(IntPtr point);
        [DllImport("User32.dll")]
        public static extern IntPtr GetForegroundWindow();

        public static IntPtr GetForegroundWindowOf(string pname)
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessesByName(pname).FirstOrDefault();
            if (p != null)
                return p.MainWindowHandle;
            else
                return (IntPtr)0;
        }


        [DllImport("User32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        #region Menu API (user32.dll, win2k+) (MF_*) (TPM_*)

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms647616(v=vs.85).aspx
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);
  
        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms647624(v=vs.85).aspx
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateMenu();

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms647636(v=vs.85).aspx
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern bool EnableMenuItem(IntPtr hMenu, int uIDEnableItem, int uEnable);

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms647985(v=vs.85).aspx
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms647987(v=vs.85).aspx
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms647993(v=vs.85).aspx
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void ModifyMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms647996(v=vs.85).aspx
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SetMenuDefaultItem(IntPtr hMenu, int uItem, uint fByPos);

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms648003(v=vs.85).aspx
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, int fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        #endregion

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms724385(v=vs.85).aspx
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int GetSystemMetrics(Win32Constants.SystemMetrics nIndex);

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644944(v=vs.85).aspx
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("Shell32.dll")]
        public static extern uint SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid,
            uint dwFlags,
            IntPtr hToken,
            out IntPtr pszPath  // API uses CoTaskMemAlloc
        );

        [Flags]
        public enum AssocF
        {
            None = 0,
            Init_NoRemapCLSID = 0x1,
            Init_ByExeName = 0x2,
            Open_ByExeName = 0x2,
            Init_DefaultToStar = 0x4,
            Init_DefaultToFolder = 0x8,
            NoUserSettings = 0x10,
            NoTruncate = 0x20,
            Verify = 0x40,
            RemapRunDll = 0x80,
            NoFixUps = 0x100,
            IgnoreBaseClass = 0x200
        }

        public enum AssocStr
        {
            Command = 1,
            Executable,
            FriendlyDocName,
            FriendlyAppName,
            NoOpen,
            ShellNewValue,
            DDECommand,
            DDEIfExec,
            DDEApplication,
            DDETopic
        }

        [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern uint AssocQueryString(AssocF flags, AssocStr str,
           string pszAssoc, string pszExtra, [Out] StringBuilder pszOut, ref uint
           pcchOut);

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms632605.aspx
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            /// <summary>Reserved. DO NOT USE.</summary>
            public System.Drawing.Point ptReserved;
            /// <summary>The size of the window if it were maximized without being moved. This value defaults to the size of the primary monitor.</summary>
            public System.Drawing.Point ptMaxSize;
            /// <summary>The position of the window if it were to be maximized without being moved.</summary>
            public System.Drawing.Point ptMaxPosition;
            /// <summary>The minimum tracking size of the window.</summary>
            public System.Drawing.Point ptMinTrackSize;
            /// <summary>The maximum tracking size of the window. This value defaults to slighter larger than the size of the virtual screen.</summary>
            public System.Drawing.Point ptMaxTrackSize;
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);
    }

    public class SafeNativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int OemKeyScan(short wAsciiVal);
        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int GetTickCount();
    }


}
