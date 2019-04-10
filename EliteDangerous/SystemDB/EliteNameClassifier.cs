/*
 * Copyright © 2015 - 2019 EDDiscovery development team
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

using System.Linq;

namespace EliteDangerousCore
{
    public class EliteNameClassifier
    {
        public const string NoSectorName = "NoSectorName";        // Sol etc

        public enum NameType        // describes the NID
        {
            NotSet,         // not set
            Named,          // Named type in name table.  NameIDNumeric will hold index
            Numeric,        // numeric name, NameIDNumeric will hold number, NumericDashPos and NumericDigits set to indicate format
            Identifier,     // Pru Eurk CQ-L                                SectorName = sector, StarName = null, L1L2L3 set
            Masscode,       // Pru Eurk CQ-L d                              SectorName = sector, StarName = null, L1L2L3 set, MassCode set
            NValue,         // Pru Eurk CQ-L d2-3 or Pru Eurk CQ-L d2       SectorName = sector, StarName = null, L1L2L3 set, MassCode set, NValue set
            N1ValueOnly     // Pru Eurk CQ-L d2-                            SectorName = sector, StarName = null, L1L2L3 set, MassCode set, NValue set
        };

        public NameType EntryType = NameType.NotSet;
        public string SectorName = null;    // for string inputs, set always, the sector name (Pru Eurk or HIP) or "NotInSector" (Sol).            For numbers, null
        public string StarName = null;      // for string inputs, set for HIP type names and non standard names, else                    For numbers, null
        public uint L1, L2, L3, MassCode, NValue;   // set for standard names
        public long NameIdNumeric = 0;      // NIndex into name table or numeric name
        public uint NumericDashPos = 0;     // Numeric Dash position
        public uint NumericDigits = 0;      // Numeric digits

        private const int StandardPosMarker = 47;   // Standard (L1/Mass/N apply).   47 means its in 6 bytes, fitting within a 6 byte SQL field
        private const int L1Marker = 38;            // Standard: 5 bits 38-42 (1 = A, 26=Z)
        private const int L2Marker = 33;            // Standard: 5 bits 33-47 (1 = A, 26=Z)
        private const int L3Marker = 28;            // Standard: 6 bits 28-32 (aligned for display purposes) (1 = A, 26=Z)
        private const int MassMarker = 24;          // Standard: 3 bits 24-27 (0=A,7=H)
        private const int NMarker = 0;              // Standard: N2 + N1<<16  

        private const int NumericMarker = 46;       // Numeric. bits 0-35 hold value.  
        private const int NumericCountMarker = 42;  // Numeric: 4 bits 42-45 Number of digits in number
        private const int NumericDashMarker = 38;   // Numeric: 4 bits 38-41 position of dash in number (0 = none, 1 = 0 char in, 2 = 1 char in etc) 
        private const long NameIDNumbericMask = 0x3fffffffff;     // 38 bits

        public bool IsStandard { get { return EntryType >= NameType.NValue; } }     // meaning L1L2L3 MassCode NValue is set..
        public bool IsStandardParts { get { return EntryType >= NameType.Identifier; } }
        public bool IsNamed { get { return EntryType == NameType.Named; } }
        public bool IsNumeric { get { return EntryType == NameType.Numeric; } }

        public static bool IsIDStandard(ulong id)
        {
            return (id & (1UL << StandardPosMarker)) != 0;
        }
        public static bool IsIDNumeric(ulong id)
        {
            return (id & (1UL << NumericMarker)) != 0;
        }

        public ulong ID // get the ID code
        {
            get
            {
                if (IsStandardParts)
                {
                    System.Diagnostics.Debug.Assert(L1 < 31 && L2 < 32 && L3 < 32 && NValue < 0xffffff && MassCode < 8);
                    return ((ulong)NValue << NMarker) | ((ulong)(MassCode) << MassMarker) | ((ulong)(L3) << L3Marker) | ((ulong)(L2) << L2Marker) | ((ulong)(L1) << L1Marker) | (1UL << StandardPosMarker);
                }
                else if (IsNumeric)
                    return (ulong)(NameIdNumeric) | (1UL << NumericMarker) | ((ulong)(NumericDashPos) << NumericDashMarker) | ((ulong)(NumericDigits) << NumericCountMarker);
                else
                    return (ulong)(NameIdNumeric);
            }
        }

        public ulong IDHigh // get the ID code, giving parts not set the maximum value.  Useful for wildcard searches when code has been set by a string
        {
            get
            {
                if (IsStandardParts)
                {
                    ulong lcodes = ((ulong)(L3) << L3Marker) | ((ulong)(L2) << L2Marker) | ((ulong)(L1) << L1Marker) | (1UL << StandardPosMarker);

                    if (EntryType == NameType.Identifier)
                        return ((1UL << L3Marker) - 1) | lcodes;

                    lcodes |= ((ulong)(MassCode) << MassMarker);

                    if (EntryType == NameType.Masscode)
                        return ((1UL << MassMarker) - 1) | lcodes;

                    if (EntryType == NameType.N1ValueOnly)
                        return lcodes | ((ulong)NValue << NMarker) | 0xffff; // N1 explicit (d23-) then we can assume a wildcard in the bottom N2
                    else
                        return lcodes | ((ulong)NValue << NMarker); // no wild card here
                }
                else return ID;
            }
        }


        public override string ToString()
        {
            if (IsStandard)
                return (SectorName != null ? (SectorName + " ") : "") + (char)(L1 + 'A' - 1) + (char)(L2 + 'A' - 1) + "-" + (char)(L3 + 'A' - 1) + " " + (char)(MassCode + 'a') + (NValue > 0xffff ? ((NValue / 0x10000).ToStringInvariant() + "-") : "") + (NValue & 0xffff).ToStringInvariant();
            else if (IsNumeric)
            {
                string num = NameIdNumeric.ToStringInvariant("0000000000000000".Substring(0, (int)NumericDigits));
                if (NumericDashPos > 0)
                    num = num.Substring(0, (int)(NumericDashPos - 1)) + "-" + num.Substring((int)(NumericDashPos - 1));

                return (SectorName != null && SectorName != NoSectorName ? (SectorName + " ") : "") + num;
            }
            else
                return (SectorName != null && SectorName != NoSectorName ? (SectorName + " ") : "") + StarName;
        }

        public EliteNameClassifier()
        {
        }

        public EliteNameClassifier(string n)
        {
            Classify(n);
        }

        public EliteNameClassifier(ulong id)
        {
            Classify(id);
        }

        public void Classify(ulong id)     // classify an ID.
        {
            if (IsIDStandard(id))
            {
                NValue = (uint)(id >> NMarker) & 0xffffff;
                MassCode = (char)(((id >> MassMarker) & 7));
                L3 = (char)(((id >> L3Marker) & 31));
                L2 = (char)(((id >> L2Marker) & 31));
                L1 = (char)(((id >> L1Marker) & 31));
                EntryType = NameType.NValue;
                System.Diagnostics.Debug.Assert(L1 < 31 && L2 < 32 && L3 < 32 && NValue < 0xffffff && MassCode < 8);
            }
            else if (IsIDNumeric(id))
            {
                NameIdNumeric = (long)(id & NameIDNumbericMask);
                NumericDashPos = (uint)((id >> NumericDashMarker) & 15);
                NumericDigits = (uint)((id >> NumericCountMarker) & 15);
                EntryType = NameType.Numeric;
            }
            else
            {
                NameIdNumeric = (long)(id & NameIDNumbericMask);
                EntryType = NameType.Named;
            }

            SectorName = StarName = null;
        }

        public void Classify(string starname)   // classify a string
        {
            EntryType = NameType.NotSet;

            string[] nameparts = starname.Split(' ');

            L1 = L2 = L3 = MassCode = NValue = 0;      // unused parts are zero

            for (int i = 0; i < nameparts.Length; i++)
            {
                if (i > 0 && nameparts[i].Length == 4 && nameparts[i][2] == '-' && char.IsLetter(nameparts[i][0]) && char.IsLetter(nameparts[i][1]) && char.IsLetter(nameparts[i][3]))
                {
                    L1 = (uint)(char.ToUpper(nameparts[i][0]) - 'A' + 1);
                    L2 = (uint)(char.ToUpper(nameparts[i][1]) - 'A' + 1);
                    L3 = (uint)(char.ToUpper(nameparts[i][3]) - 'A' + 1);

                    EntryType = NameType.Identifier;

                    if (nameparts.Length > i + 1)
                    {
                        string p = nameparts[i + 1];

                        if (p.Length > 0)
                        {
                            char mc = char.ToLower(p[0]);
                            if (mc >= 'a' && mc <= 'h')
                            {
                                MassCode = (uint)(mc - 'a');
                                EntryType = NameType.Masscode;

                                int slash = p.IndexOf("-");

                                int first = (slash >= 0 ? p.Substring(1, slash - 1) : p.Substring(1)).InvariantParseInt(-1);

                                if (first >= 0)
                                {
                                    if (slash >= 0)
                                    {
                                        System.Diagnostics.Debug.Assert(first < 256);
                                        int second = p.Substring(slash + 1).InvariantParseInt(-1);
                                        System.Diagnostics.Debug.Assert(second < 65536);
                                        if (second >= 0)
                                        {
                                            NValue = (uint)first * 0x10000 + (uint)second;
                                            EntryType = NameType.NValue;
                                        }
                                        else
                                        {               // thats d29-
                                            NValue = (uint)first * 0x10000;
                                            EntryType = NameType.N1ValueOnly;
                                        }
                                    }
                                    else
                                    {       // got to presume its the whole monty, d23
                                        System.Diagnostics.Debug.Assert(first < 65536);
                                        NValue = (uint)first;
                                        EntryType = NameType.NValue;
                                    }
                                }
                            }
                        }
                    }

                    SectorName = nameparts[0];
                    for (int j = 1; j < i; j++)
                        SectorName = SectorName + " " + nameparts[j];

                    StarName = null;
                    break;
                } // end if
            }

            if (EntryType == NameType.NotSet)
            {
                string[] surveys = new string[] { "2MASS", "HD", "LTT", "TYC", "NGC", "HR", "LFT", "LHS", "LP", "Wolf", "IHA2007", "USNO-A2.0", "2547" , "DBP2006" , "NOMAD1", "OJV2009" , "PSR",
                                                "SSTGLMC", "StKM", "UGCS"};

                int dashpos = 0;
                long? namenum = null;
                int countof = 0;

                if (nameparts.Length >= 2)     // see if last is a number or number-number
                {
                    dashpos = nameparts.Last().IndexOf('-');
                    string num = (dashpos >= 0 && nameparts.Last().Count(x => x == '-') == 1) ? nameparts.Last().Replace("-", "") : nameparts.Last();
                    namenum = num.InvariantParseLongNull();
                    countof = num.Length;

                    if (namenum.HasValue && namenum.Value > NameIDNumbericMask)
                        System.Diagnostics.Debug.WriteLine("Starname " + starname + " too big");
                }

                if (namenum.HasValue && namenum.Value <= NameIDNumbericMask)  // if numeric and fits within mask size
                {
                    NumericDashPos = (uint)(dashpos + 1);       // record dash pos AND count of digits (lots have leading zeros)
                    NumericDigits = (uint)countof;

                    SectorName = string.Join(" ", nameparts.RangeSubset(0, nameparts.Length - 1));
                    NameIdNumeric = namenum.Value;
                    EntryType = NameType.Numeric;
                }
                else
                {
                    if (surveys.Contains(nameparts[0]))
                    {
                        SectorName = nameparts[0];
                        StarName = starname.Mid(nameparts[0].Length + 1).Trim();
                    }
                    else
                    {
                        SectorName = NoSectorName;
                        StarName = starname.Trim();
                    }

                    EntryType = NameType.Named;
                }

                System.Diagnostics.Debug.Assert(ToString() == starname.Trim());        // double check conversion
            }
        }
    }
}
