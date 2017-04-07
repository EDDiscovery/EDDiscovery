/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EDDiscovery
{
    public class Tools
    {

        public static void TextBox_Numeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char vbBack = '\u0008';

            System.Globalization.NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();

            if (sender is TextBoxBase)
            {
                TextBoxBase tempBox = (TextBox)sender;
                if (Char.IsDigit(e.KeyChar))
                {
                    if (tempBox.Text.Length != 0)
                    {
                        if (tempBox.SelectionStart == 0 && (tempBox.Text[0].ToString()) == negativeSign && tempBox.SelectionLength == 0)
                            e.Handled = true;
                    }


                }
                else if (keyInput.Equals(negativeSign))
                {

                    if (tempBox.SelectionStart != 0 || (tempBox.Text.Contains(negativeSign) && !tempBox.SelectedText.Contains(negativeSign)))
                        e.Handled = true;
                }
                else if (keyInput.Equals(decimalSeparator))
                {

                    if (tempBox.Text.Length != 0)
                    {
                        if (tempBox.SelectionStart == 0 && (tempBox.Text[0].ToString()) == negativeSign && !tempBox.SelectedText.Contains(negativeSign) || tempBox.Text.Contains(decimalSeparator) && !tempBox.SelectedText.Contains(decimalSeparator))
                            e.Handled = true;

                    }
                    // Decimal separator is OK
                }
                else if (e.KeyChar == vbBack)
                {

                    // Backspace key is OK
                }
                else
                {

                    // Consume this invalid key and beep.
                    e.Handled = true;
                }
            }
        }


        public static void TextBox_Int_KeyPress(System.Object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            const char vbBack = '\u0008';

            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();
            if (sender is TextBoxBase)
            {
                TextBoxBase tempBox = (TextBox)sender;
                if (Char.IsDigit(e.KeyChar))
                {
                    if (tempBox.Text.Length != 0)
                    {
                        if (tempBox.SelectionStart == 0 && (tempBox.Text[0].ToString()) == negativeSign && tempBox.SelectionLength == 0)
                            e.Handled = true;
                    }
                }
                else if (keyInput.Equals(negativeSign))
                {
                    // Decimal separator is OK
                    if (tempBox.SelectionStart != 0 || (tempBox.Text.Contains(negativeSign) && !tempBox.SelectedText.Contains(negativeSign)))
                        e.Handled = true;
                }
                else if (e.KeyChar == vbBack)
                {
                    // Backspace key is OK
                }
                else
                {
                    // Consume this invalid key and beep.
                    e.Handled = true;
                }
            }
        }

        static internal string GetAppDataDirectory()
        {
            return EDDConfig.Options.AppDataDirectory;
        }

        static public string WordWrap(string input, int linelen)
        {
            String[] split = input.Split(new char[] { ' ' });

            string ans = "";
            int l = 0;
            for (int i = 0; i < split.Length; i++)
            {
                ans += split[i];
                l += split[i].Length;
                if (l > linelen)
                {
                    ans += Environment.NewLine;
                    l = 0;
                }
                else
                    ans += " ";
            }

            return ans;
        }

        static public string StackTrace(string trace, string enclosingfunc, int lines)
        {
            int offset = trace.IndexOf(enclosingfunc);

            string ret = "";

            if (offset != -1)
            {
                CutLine(ref trace, offset);

                while (lines-- > 0)
                {
                    string l = CutLine(ref trace, 0);
                    if (l != "")
                    {
                        if (ret != "")
                            ret = ret + Environment.NewLine + l;
                        else
                            ret = l;
                    }
                    else
                        break;
                }
            }
            else
                ret = trace;

            return ret;
        }

        static public string CutLine(ref string trace, int offset)
        {
            int nloffset = trace.IndexOf(Environment.NewLine, offset);
            string ret;
            if (nloffset != -1)
            {
                ret = trace.Substring(offset, nloffset - offset);
                trace = trace.Substring(nloffset);
                if (trace.Length >= Environment.NewLine.Length)
                    trace = trace.Substring(Environment.NewLine.Length);
            }
            else
            {
                ret = trace;
                trace = "";
            }

            return ret;
        }

        static StreamWriter debugout = null;
        static Stopwatch debugtimer = null;

        static public void LogToFile(string s)
        {
            if (debugout == null)
            {
                debugout = new StreamWriter(Path.Combine(Tools.GetAppDataDirectory(), "debuglog-" + DateTime.Now.ToString("yyyy-dd-MM-HH-mm-ss") + ".log"));
                debugtimer = new Stopwatch();
                debugtimer.Start();
            }

            debugout.WriteLine((debugtimer.ElapsedMilliseconds % 100000) + ":" + s);
            debugout.Flush();
        }


        public static string TryReadAllTextFromFile(string filename)
        {
            try
            {
                return File.ReadAllText(filename, Encoding.UTF8);
            }
            catch
            {
                return null;
            }
        }

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

        public static string AssocQueryString(AssocStr association, string extension)
        {
            const int S_OK = 0;
            const int S_FALSE = 1;

            uint length = 0;
            uint ret = AssocQueryString(AssocF.None, association, extension, null, null, ref length);
            if (ret != S_FALSE)
            {
                throw new InvalidOperationException("Could not determine associated string");
            }

            var sb = new StringBuilder((int)length); // (length-1) will probably work too as the marshaller adds null termination
            ret = AssocQueryString(AssocF.None, association, extension, null, sb, ref length);
            if (ret != S_OK)
            {
                throw new InvalidOperationException("Could not determine associated string");
            }

            return sb.ToString();
        }

        static public List<string> GetPropertyFieldNames(Type jtype, string prefix = "")       // give a list of properties for a given name
        {
            if (jtype != null)
            {
                List<string> ret = new List<string>();

                foreach (System.Reflection.PropertyInfo pi in jtype.GetProperties())
                {
                    if (pi.GetIndexParameters().GetLength(0) == 0)      // only properties with zero parameters are called
                        ret.Add(prefix + pi.Name);
                }

                foreach (System.Reflection.FieldInfo fi in jtype.GetFields())
                {
                    string name = prefix + fi.Name;
                }
                return ret;
            }
            else
                return null;
        }

        // first object = format
        // second object = data value
        // if data value null or empty, not printed
        // if data value is bool, format = false text;true text
        // else format is prefix;postfix;[doubleformat] value 
        // if prefix starts with a <, no ,<spc> pad

        static public string FieldBuilder(params System.Object[] values)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(64);

            for (int i = 0; i < values.Length;)
            {

                System.Diagnostics.Debug.Assert(i + 2 <= values.Length);

                string[] fieldnames = ((string)values[i]).Split(';');
                object value = values[i + 1];
                i += 2;

                string pad = ", ";
                if (fieldnames[0].Length > 0 && fieldnames[0][0] == '<')
                {
                    fieldnames[0] = fieldnames[0].Substring(1);
                    pad = "";
                }

                if (value != null)
                {
                    if (value is bool)
                    {
                        string s = ((bool)value) ? fieldnames[1] : fieldnames[0];
                        sb.AppendPrePad(s, pad);
                    }
                    else if (value is string)
                    {
                        string text = (string)value;

                        if (sb.AppendPrePad(text, fieldnames[0], pad))      // if printed something, text must be non null and of length
                        {
                            if (fieldnames.Length >= 2 && fieldnames[1].Length > 0)
                                sb.Append(fieldnames[1]);
                        }
                    }
                    else
                    {
                        string output;

                        if (value is double)
                        {
                            System.Diagnostics.Debug.Assert(fieldnames.Length >= 3);
                            output = ((double)value).ToString(fieldnames[2]);
                        }
                        else if (value is int)
                            output = ((int)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
                        else if (value is long)
                            output = ((long)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
                        else if (value is double?)
                        {
                            System.Diagnostics.Debug.Assert(fieldnames.Length >= 3);
                            output = ((double?)value).Value.ToString(fieldnames[2]);
                        }
                        else if (value is int?)
                            output = ((int?)value).Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        else if (value is long?)
                            output = ((long?)value).Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        else if (value is DateTime)
                        {
                            output = ((DateTime)value).ToString();
                        }
                        else
                        { 
                            output = "";
                            System.Diagnostics.Debug.Assert(false);
                        }

                        if (sb.AppendPrePad(output, fieldnames[0], pad))      // if printed something, text must be non null and of length
                        {
                            if (fieldnames.Length >= 2 && fieldnames[1].Length > 0)
                                sb.Append(fieldnames[1]);
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public static string GetDefaultBrowser()
        {
            const string userChoice = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
            using (Microsoft.Win32.RegistryKey userChoiceKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(userChoice))
            {
                if (userChoiceKey != null)
                {
                    object progIdValue = userChoiceKey.GetValue("Progid");
                    if (progIdValue != null)
                        return progIdValue.ToString();
                }
            }

            return null;
        }

        public static string GetBrowserPath(string defbrowser)
        {
            const string exeSuffix = ".exe";
            string path = defbrowser + @"\shell\open\command";

            using (Microsoft.Win32.RegistryKey pathKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(path))
            {
                if (pathKey == null)
                {
                    return null;
                }

                // Trim parameters.
                try
                {
                    path = pathKey.GetValue(null).ToString().ToLower().Replace("\"", "");
                    if (!path.EndsWith(exeSuffix))
                    {
                        path = path.Substring(0, path.LastIndexOf(exeSuffix, StringComparison.Ordinal) + exeSuffix.Length);
                        return path;
                    }
                }
                catch
                {
                    // Assume the registry value is set incorrectly, or some funky browser is used which currently is unknown.
                }
            }

            return null;
        }
                    
    }
}
