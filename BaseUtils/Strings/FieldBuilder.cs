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
        // first object = format
        // second object = data value
        // if data value null or empty, not printed
        // if data value is bool, format = false text;true text
        // else format is prefix;postfix;[floatdoubleformat] value .  floatDoubleformat must be present for floats/doubles
        // if prefix starts with a <, no ,<spc> pad

        static public string Build(params System.Object[] values)
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
                        else if (value is float)
                        {
                            System.Diagnostics.Debug.Assert(fieldnames.Length >= 3);
                            output = ((float)value).ToString(fieldnames[2]);
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
                        else if (value is float?)
                        {
                            System.Diagnostics.Debug.Assert(fieldnames.Length >= 3);
                            output = ((float?)value).Value.ToString(fieldnames[2]);
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

    }
}
