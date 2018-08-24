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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUtils
{
    static public class FieldBuilder
    {
        // first object = format string
        // second object = data value
        //
        //      if data value null or empty, not printed
        //
        //      if data value is string, format = prefix;postfix
        //      if data value is bool, format = false text;true text
        //      if data value is double/float, format = prefix;postfix[;floatformat]    format = "0" if not present    
        //      if data value is int/long, format = prefix;postfix [;int format]        format = "0" if not present
        //      if data value is date time, format = prefix;postfix [;date format]      format = "g" if not present
        //      if data value is enum, format = prefix;postfix 
        //
        //      if prefix starts with a <, no ,<spc> pad
        //
        // or first object = NewPrefix only, define next pad to use, then go back to standard pad

        public class NewPrefix   // indicator class, use this as first item to indicate the next prefix to use.  After one use, its discarded.
        {
            public string prefix;
            public NewPrefix(string s) { prefix = s; }
        }

        static public string Build(params System.Object[] values)
        {
            return Build(System.Globalization.CultureInfo.CurrentCulture, ", ", values);
        }

        static public string BuildSetPad(string padchars, params System.Object[] values)
        {
            return Build(System.Globalization.CultureInfo.CurrentCulture, padchars, values);
        }

        static public string Build(System.Globalization.CultureInfo ct, string padchars, params System.Object[] values)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(64);

            string overrideprefix = string.Empty;

            for (int i = 0; i < values.Length;)
            {
                Object first = values[i];

                if ( first is NewPrefix )       // first item is special, a new prefix, override
                {
                    overrideprefix = (first as NewPrefix).prefix;
                    i++;
                }
                else if ( first is string )     // normal, string
                {
                    System.Diagnostics.Debug.Assert(i + 2 <= values.Length,"Field Builder missing parameter");

                    string[] fieldnames = ((string)first).Split(';');

                    object value = values[i + 1];
                    i += 2;

                    string pad = padchars;
                    if (fieldnames[0].Length > 0 && fieldnames[0][0] == '<')
                    {
                        fieldnames[0] = fieldnames[0].Substring(1);
                        pad = "";
                    }

                    if (value != null)
                    {
                        if (value is bool)
                        {
                            if (fieldnames.Length != 2)
                            {
                                sb.AppendPrePad("!!REPORT ERROR IN FORMAT STRING " + first + "!!", (overrideprefix.Length > 0) ? overrideprefix : pad);
                                System.Diagnostics.Debug.WriteLine("*************** FIELD BUILDER ERROR" + first);
                            }
                            else
                            {
                                string s = ((bool)value) ? fieldnames[1] : fieldnames[0];
                                sb.AppendPrePad(s, (overrideprefix.Length > 0) ? overrideprefix : pad);
                                overrideprefix = string.Empty;
                            }
                        }
                        else
                        {
                            string format = fieldnames.Length >= 3 ? fieldnames[2] : "0";

                            string output;
                            if (value is string)
                            {
                                output = (string)value;
                            }
                            else if (value is double)
                            {
                                output = ((double)value).ToString(format, ct);
                            }
                            else if (value is float)
                            {
                                output = ((float)value).ToString(format, ct);
                            }
                            else if (value is int)
                            {
                                output = ((int)value).ToString(format,ct);
                            }
                            else if (value is long)
                            {
                                output = ((long)value).ToString(format,ct);
                            }
                            else if (value is double?)
                            {
                                output = ((double?)value).Value.ToString(format,ct);
                            }
                            else if (value is float?)
                            {
                                output = ((float?)value).Value.ToString(format,ct);
                            }
                            else if (value is int?)
                            {
                                output = ((int?)value).Value.ToString(format,ct);
                            }
                            else if (value is long?)
                            {
                                output = ((long?)value).Value.ToString(format,ct);
                            }
                            else if (value is DateTime)
                            {
                                format = fieldnames.Length >= 3 ? fieldnames[2] : "g";
                                output = ((DateTime)value).ToString(format,ct);
                            }
                            else
                            {
                                Type t = value.GetType();
                                if (t.BaseType.Name.Equals("Enum"))
                                {
                                    var ev = Activator.CreateInstance(t);
                                    ev = value;
                                    output = ev.ToString();
                                }
                                else
                                {
                                    output = "";
                                    System.Diagnostics.Debug.Assert(false);
                                }
                            }

                            // if printed something, text must be non null and of length, and it returns true.  Only adds on prefix and prepad if required
                            if (sb.AppendPrePad(output, fieldnames[0], (overrideprefix.Length > 0) ? overrideprefix : pad))
                            {                                                                   // prefix with fieldnames[0], and prefix with newline if defined, or pad
                                if (fieldnames.Length >= 2 && fieldnames[1].Length > 0)
                                    sb.Append(fieldnames[1]);

                                overrideprefix = string.Empty;
                            }
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.Assert(false);
                    return "!!REPORT ERROR IN FORMAT STRING!!";
                }
            }

            return sb.ToString();
        }

    }
}
