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
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern short VkKeyScan(char key);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx(int hookid, NativeMethods.HookProc pfnhook, HandleRef hinst, int threadid);
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
        [DllImport("User32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);
    }

    public class SafeNativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int OemKeyScan(short wAsciiVal);
        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int GetTickCount();
    }


}
