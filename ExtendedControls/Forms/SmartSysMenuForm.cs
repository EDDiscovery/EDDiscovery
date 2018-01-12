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
    /// <summary>
    /// A customized form offering a fully functional System Menu for everyone. <see cref="WM.SYSCOMMAND"/> values from
    /// 0x0010 to 0xEFFF are availble for inherited classes to utilize.
    /// values from 0x0001 to 0x000F, leaving inheritors free to work in between 0x0010 to 0x00FF.
    /// </summary>
    public class SmartSysMenuForm : Form
    {
        public List<string> AdditionalSysMenus;         // null means none!  SET by derived class to add more menus!
        public Action<int> AdditionalSysMenuSelected;   // called when additional menu X (0-N-1) selected

        [DefaultValue(false)]
        public new bool TopMost
        {
            get { return base.TopMost; }
            set
            {
                if (base.TopMost != value)
                {
                    base.TopMost = value;
                    OnTopMostChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler TopMostChanged;


        // TODO: managed wrapper to keep track of these all up and down the inhertance stack.
        protected const int SC_ONTOP = 0x0001;
        protected const int SC_OPACITYSUBMENU = 0x0002;    // 100% = 0x3; 90% = 0x4; ...; 10% = 0xC; 0% = NOT USED!
        protected const int SC_ADDITIONALMENU = 0x0020;    
        // 0x000D-0x001F are reserved by us for future expansion, while 0x0000 and 0xF000+ are system reserved.

        protected virtual bool AllowResize { get; } = true;

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                if (_SysMenuCreationHackEnabled)
                    cp.Style |= WS.SYSMENU;
                return cp;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                TopMostChanged = null;
            base.Dispose(disposing);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                UnsafeNativeMethods.GetSystemMenu(Handle, true);
                IntPtr hSysMenu = UnsafeNativeMethods.GetSystemMenu(Handle, false);
                if (hSysMenu != IntPtr.Zero)
                {
                    if (AdditionalSysMenus != null)
                    {
                        int entryno = SC_ADDITIONALMENU;
                        foreach (string t in AdditionalSysMenus )
                            UnsafeNativeMethods.AppendMenu(hSysMenu, MF.STRING, entryno++, t);
                    }

                    UnsafeNativeMethods.AppendMenu(hSysMenu, MF.SEPARATOR, 0, string.Empty);
                    UnsafeNativeMethods.AppendMenu(hSysMenu, MF.STRING, SC_ONTOP, "On &Top");

                    IntPtr hOpacSubmenu = UnsafeNativeMethods.CreateMenu();
                    if (hOpacSubmenu != IntPtr.Zero)
                    {
                        for (int i = 10; i > 0; i--)
                            UnsafeNativeMethods.AppendMenu(hOpacSubmenu, MF.STRING, SC_OPACITYSUBMENU + i, $"{i / 10f:P0}");
                        UnsafeNativeMethods.AppendMenu(hSysMenu, MF.STRING | MF.POPUP, (int)hOpacSubmenu, "&Opacity");

                    }
                }
            }
        }

        protected virtual void OnTopMostChanged(EventArgs e)
        {
            TopMostChanged?.Invoke(this, e);
        }

        protected IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(this.Handle, msg, wparam, lparam);
            this.WndProc(ref message);
            return message.Result;
        }

        protected void ShowSystemMenu(Point screenPt)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && IsHandleCreated)
            {
                var hMenu = UnsafeNativeMethods.GetSystemMenu(Handle, false);
                if (hMenu != IntPtr.Zero)
                {
                    int cmd = UnsafeNativeMethods.TrackPopupMenuEx(hMenu, UnsafeNativeMethods.GetSystemMetrics(SystemMetrics.MENUDROPALIGNMENT) | TPM.RETURNCMD,
                        screenPt.X, screenPt.Y, Handle, IntPtr.Zero);
                    if (cmd != 0)
                        UnsafeNativeMethods.PostMessage(Handle, WM.SYSCOMMAND, (IntPtr)cmd, IntPtr.Zero);
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM.CREATE:         // Win32: set _SysMenuCreationHackEnabled to ensure that we get a system menu at startup.
                    {
                        _SysMenuCreationHackEnabled = Environment.OSVersion.Platform == PlatformID.Win32NT;
                        base.WndProc(ref m);
                        _SysMenuCreationHackEnabled = false;
                        return;
                    }

                case WM.DESTROY:        // Win32: certain events (changing ShowInTaskBar) may destroy us. Re-enable the hack.
                    {
                        _SysMenuCreationHackEnabled = Environment.OSVersion.Platform == PlatformID.Win32NT;
                        break;
                    }

                case WM.INITMENU:       // Win32: refresh the system menu before displaying it.
                    {
                        base.WndProc(ref m);    // Base should always get first crack at this.

                        if (m.WParam != IntPtr.Zero && Environment.OSVersion.Platform == PlatformID.Win32NT && IsHandleCreated)
                        {
                            bool maximized = WindowState == FormWindowState.Maximized;
                            // these don't matter too much, but it helps the user to know if something isn't allowed. WM_SYSCOMMAND is where we verify.
                            UnsafeNativeMethods.EnableMenuItem(m.WParam, SC.RESTORE,  AllowResize &&  maximized ? MF.ENABLED : MF.GRAYED);
                            UnsafeNativeMethods.EnableMenuItem(m.WParam, SC.SIZE,     AllowResize && !maximized ? MF.ENABLED : MF.GRAYED);
                            UnsafeNativeMethods.EnableMenuItem(m.WParam, SC.MINIMIZE, AllowResize               ? MF.ENABLED : MF.GRAYED);
                            UnsafeNativeMethods.EnableMenuItem(m.WParam, SC.MAXIMIZE, AllowResize && !maximized ? MF.ENABLED : MF.GRAYED);

                            if (FormBorderStyle == FormBorderStyle.None) // base.WndProc() is useless...
                            {
                                UnsafeNativeMethods.EnableMenuItem(m.WParam, SC.MOVE, maximized ? MF.GRAYED : MF.ENABLED);
                                UnsafeNativeMethods.EnableMenuItem(m.WParam, SC.CLOSE, MF.ENABLED);
                            }

                            UnsafeNativeMethods.ModifyMenu(m.WParam, SC_ONTOP, MF.BYCOMMAND | (base.TopMost ? MF.CHECKED : MF.UNCHECKED), SC_ONTOP, "On &Top");
                            int opac = (int)Math.Min(10, Math.Round(Opacity * 10));  // 0.000-1.00 => 10-100
                            for (int i = 10; i > 0; i--)
                            {
                                UnsafeNativeMethods.ModifyMenu(m.WParam, SC_OPACITYSUBMENU + i, MF.BYCOMMAND | (opac == i ? MF.CHECKED : MF.UNCHECKED), SC_OPACITYSUBMENU + i, $"{i / 10f:P0}");
                            }

                            // This only works reliably on the application's MainForm.
                            UnsafeNativeMethods.SetMenuDefaultItem(m.WParam, maximized && AllowResize ? SC.RESTORE : (!maximized && AllowResize ? SC.MAXIMIZE : SC.CLOSE), 0);
                            m.Result = IntPtr.Zero;
                        }
                        return;
                    }

                case WM.NCRBUTTONUP:    // Win32: Display the system menu.
                    {
                        if (FormBorderStyle == FormBorderStyle.None && m.WParam == (IntPtr)HT.CAPTION)
                        {
                            ShowSystemMenu(new Point((int)m.LParam));
                            m.Result = IntPtr.Zero;
                            return;
                        }
                        break;
                    }

                case WM.SYSCOMMAND:     // Process any system commands intended for this window (SC_ONTOP / SC_OPACITYSUBMENU).
                    {
                        int wp = unchecked((int)(long)m.WParam);

                        if (m.WParam == (IntPtr)SC_ONTOP)
                            TopMost = !base.TopMost;
                        else if (wp > SC_OPACITYSUBMENU && wp <= SC_OPACITYSUBMENU + 10)
                            Opacity = (wp - SC_OPACITYSUBMENU) / 10f;
                        else if (wp >= SC_ADDITIONALMENU && AdditionalSysMenus != null && wp < SC_ADDITIONALMENU + AdditionalSysMenus.Count)
                            AdditionalSysMenuSelected?.Invoke(wp - SC_ADDITIONALMENU);
                        else if (m.WParam == (IntPtr)SC.KEYMENU && m.LParam == (IntPtr)' ' && (CreateParams.Style & WS.SYSMENU) == 0 && (CreateParams.Style & WS.CAPTION) == 0)
                            ShowSystemMenu(PointToScreen(new Point(5, 5)));
                        else if (!AllowResize && (m.WParam == (IntPtr)SC.MAXIMIZE || m.WParam == (IntPtr)SC.SIZE || m.WParam == (IntPtr)SC.RESTORE))
                            return;     // Access Denied.
                        else
                            break;

                        m.Result = IntPtr.Zero;
                        return;
                    }
            }
            base.WndProc(ref m);
        }

        // If WS.SYSMENU is not active at first WM.CREATE, the menu will not be created. Since FormBorderStyle may clear WS.SYSMENU, we have
        // to fake it during startup. WS.SYSMENU is meaningless to us outside of WM.CREATE. Seealso https://stackoverflow.com/a/16695606
        // CAUTION: if WS.SYSMENU is enabled but WS.CAPTION is not, all hittests, including our sysmenu, min/max/close, etc., will get ignored!
        private bool _SysMenuCreationHackEnabled = Environment.OSVersion.Platform == PlatformID.Win32NT;
    }
}
