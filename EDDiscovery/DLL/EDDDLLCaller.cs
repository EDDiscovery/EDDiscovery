/*
 * Copyright © 2015 - 2018 EDDiscovery development team
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

using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace EDDiscovery.DLL
{
    public class EDDDLLCaller
    {
        public string Version { get; private set; }
        public string Name { get; private set; }

        private IntPtr pDll = IntPtr.Zero;
        private IntPtr pNewJournalEntry = IntPtr.Zero;
        private IntPtr pActionJournalEntry = IntPtr.Zero;
        private IntPtr pActionCommand = IntPtr.Zero;

        public bool Load(string path)
        {
            if (pDll == IntPtr.Zero)
            {
                pDll = BaseUtils.Win32.UnsafeNativeMethods.LoadLibrary(path);

                if (pDll != IntPtr.Zero)
                {
                    IntPtr peddinit = BaseUtils.Win32.UnsafeNativeMethods.GetProcAddress(pDll, "EDDInitialise");

                    if (peddinit != IntPtr.Zero)        // must have this to be an EDD DLL
                    {
                        Name = System.IO.Path.GetFileNameWithoutExtension(path);
                        pNewJournalEntry = BaseUtils.Win32.UnsafeNativeMethods.GetProcAddress(pDll, "EDDNewJournalEntry");
                        pActionJournalEntry = BaseUtils.Win32.UnsafeNativeMethods.GetProcAddress(pDll, "EDDActionJournalEntry");
                        pActionCommand = BaseUtils.Win32.UnsafeNativeMethods.GetProcAddress(pDll, "EDDActionCommand");
                        return true;
                    }
                    else
                    {
                        BaseUtils.Win32.UnsafeNativeMethods.FreeLibrary(pDll);
                        pDll = IntPtr.Zero;
                    }
                }
            }

            return false;
        }

        public bool Init(string ourversion, string dllfolder, EDDDLLIF.EDDCallBacks callbacks)
        {
            if (pDll != IntPtr.Zero)
            {
                IntPtr peddinit = BaseUtils.Win32.UnsafeNativeMethods.GetProcAddress(pDll, "EDDInitialise");

                EDDDLLIF.EDDInitialise edinit = (EDDDLLIF.EDDInitialise)Marshal.GetDelegateForFunctionPointer(
                                                                                                peddinit,
                                                                                                typeof(EDDDLLIF.EDDInitialise));
                Version = edinit(ourversion, dllfolder, callbacks);

                bool ok = Version != null && Version.Length > 0;

                if (ok)
                    return true;
                else
                {
                    BaseUtils.Win32.UnsafeNativeMethods.FreeLibrary(pDll);
                    pDll = IntPtr.Zero;
                }
            }

            return false;
        }

        public bool UnLoad()
        {
            if (pDll != IntPtr.Zero)
            {
                IntPtr pAddressOfFunctionToCall = BaseUtils.Win32.UnsafeNativeMethods.GetProcAddress(pDll, "EDDTerminate");

                if (pAddressOfFunctionToCall != IntPtr.Zero)
                {
                    EDDDLLIF.EDDTerminate edf = (EDDDLLIF.EDDTerminate)Marshal.GetDelegateForFunctionPointer(
                                                                                        pAddressOfFunctionToCall,
                                                                                        typeof(EDDDLLIF.EDDTerminate));
                    edf();
                }

                BaseUtils.Win32.UnsafeNativeMethods.FreeLibrary(pDll);
                pDll = IntPtr.Zero;
                Version = null;
                return true;
            }

            return false;
        }

        public bool Refresh(string cmdr, EDDDLLIF.JournalEntry je)
        {
            if (pDll != IntPtr.Zero)
            {
                IntPtr pAddressOfFunctionToCall = BaseUtils.Win32.UnsafeNativeMethods.GetProcAddress(pDll, "EDDRefresh");

                if (pAddressOfFunctionToCall != IntPtr.Zero)
                {
                    EDDDLLIF.EDDRefresh edf = (EDDDLLIF.EDDRefresh)Marshal.GetDelegateForFunctionPointer(
                                                                                        pAddressOfFunctionToCall,
                                                                                        typeof(EDDDLLIF.EDDRefresh));
                    edf(cmdr, je);
                    return true;
                }
            }

            return false;
        }

        public bool NewJournalEntry(EDDDLLIF.JournalEntry nje)
        {
            if (pDll != IntPtr.Zero && pNewJournalEntry != IntPtr.Zero)
            {
                EDDDLLIF.EDDNewJournalEntry edf = (EDDDLLIF.EDDNewJournalEntry)Marshal.GetDelegateForFunctionPointer(
                                                                                    pNewJournalEntry,
                                                                                    typeof(EDDDLLIF.EDDNewJournalEntry));
                edf(nje);
                return true;
            }

            return false;
        }

        public bool ActionJournalEntry(EDDDLLIF.JournalEntry je)
        {
            if (pDll != IntPtr.Zero && pActionJournalEntry != IntPtr.Zero)
            {
                EDDDLLIF.EDDActionJournalEntry edf = (EDDDLLIF.EDDActionJournalEntry)Marshal.GetDelegateForFunctionPointer(
                                                                                    pActionJournalEntry,
                                                                                    typeof(EDDDLLIF.EDDActionJournalEntry));
                edf(je);
                return true;
            }

            return false;
        }

        public string ActionCommand(string cmd, string[] paras) // paras must be present..
        {
            if (pDll != IntPtr.Zero && pActionCommand != IntPtr.Zero)
            {
                EDDDLLIF.EDDActionCommand edf = (EDDDLLIF.EDDActionCommand)Marshal.GetDelegateForFunctionPointer(
                                                                                    pActionCommand,
                                                                                    typeof(EDDDLLIF.EDDActionCommand));
                return edf(cmd, paras);
            }

            return null;
        }

    }
}
