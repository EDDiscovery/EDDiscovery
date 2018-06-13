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
using System.Runtime.InteropServices;

namespace EDDiscovery.DLL
{
    public static class EDDDLLIF
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct JournalEntry
        {
            [FieldOffset(0)] public int ver;
            [FieldOffset(4)] public int indexno;
            [FieldOffset(8)] [MarshalAs(UnmanagedType.BStr)] public string utctime;
            [FieldOffset(16)] [MarshalAs(UnmanagedType.BStr)] public string name;
            [FieldOffset(24)] [MarshalAs(UnmanagedType.BStr)] public string info;
            [FieldOffset(32)] [MarshalAs(UnmanagedType.BStr)] public string detailedinfo;

            [FieldOffset(40)] [MarshalAs(UnmanagedType.SafeArray)] public string[] materials;
            [FieldOffset(48)] [MarshalAs(UnmanagedType.SafeArray)] public string[] commodities;

            [FieldOffset(56)] [MarshalAs(UnmanagedType.BStr)] public string systemname;
            [FieldOffset(64)] public double x;
            [FieldOffset(72)] public double y;
            [FieldOffset(80)] public double z;

            [FieldOffset(88)] public double travelleddistance;
            [FieldOffset(96)] public long travelledseconds;

            [FieldOffset(100)] public bool islanded;
            [FieldOffset(101)] public bool isdocked;

            [FieldOffset(104)] [MarshalAs(UnmanagedType.BStr)] public string whereami;
            [FieldOffset(112)] [MarshalAs(UnmanagedType.BStr)] public string shiptype;
            [FieldOffset(120)] [MarshalAs(UnmanagedType.BStr)] public string gamemode;
            [FieldOffset(128)] [MarshalAs(UnmanagedType.BStr)] public string group;
            [FieldOffset(136)] public long credits;

            [FieldOffset(144)] [MarshalAs(UnmanagedType.BStr)] public string eventid;

            [FieldOffset(152)] [MarshalAs(UnmanagedType.SafeArray)] public string[] currentmissions;

            [FieldOffset(160)] public long jid;
            [FieldOffset(168)] public int totalrecords;

            // Version 1 Ends here
        };

        public delegate bool EDDRequestHistory(long index, bool isjid, out JournalEntry f); //index =1..total records, or jid

        public delegate bool EDDRunAction(   [MarshalAs(UnmanagedType.BStr)]string eventname,
                                             [MarshalAs(UnmanagedType.BStr)]string parameters);  // parameters in format v="k",X="k"

        [StructLayout(LayoutKind.Explicit)]
        public struct EDDCallBacks
        {
            [FieldOffset(0)] public int ver;
            [FieldOffset(8)] public EDDRequestHistory RequestHistory;
            [FieldOffset(16)] public EDDRunAction RunAction;
            // Version 1 Ends here
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.BStr)]
        public delegate String EDDInitialise([MarshalAs(UnmanagedType.BStr)]string vstr,
                                             [MarshalAs(UnmanagedType.BStr)]string dllfolder,
                                             EDDCallBacks callbacks);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void EDDRefresh([MarshalAs(UnmanagedType.BStr)]string cmdname, JournalEntry lastje);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void EDDNewJournalEntry(JournalEntry nje);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void EDDTerminate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.BStr)]                         // paras can be an empty array, but is always present
        public delegate String EDDActionCommand([MarshalAs(UnmanagedType.BStr)]string cmdname, [MarshalAs(UnmanagedType.SafeArray)]string[] paras);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void EDDActionJournalEntry(JournalEntry lastje);

        // Version 1 Ends here

    }


}
