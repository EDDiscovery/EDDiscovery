/*
 * Copyright © 2016 EDDiscovery development team
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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
public static class ObjectExtensions
{
    public static string ToNullSafeString(this object obj)
        {
        return (obj ?? string.Empty).ToString();
    }

    public static string QuotedEscapeString(this string obj )
    {
        if (obj.Contains("\"") || obj.Contains(" ") || obj.Contains(")"))       // ) because its used to terminate var lists sometimes
            obj = "\"" + obj.Replace("\"", "\\\"") + "\"";
        return obj;
    }

    public static int FirstCharNonWhiteSpace(this string obj )
    {
        int i = 0;
        while (i < obj.Length && char.IsWhiteSpace(obj[i]))
            i++;
        return i;
    }

    public static string ToString(this System.Windows.Forms.Keys key, System.Windows.Forms.Keys modifier )
    {
        string k = "";

        if ((modifier & System.Windows.Forms.Keys.Shift) != 0)
        {
            k += "Shift+";
        }
        if ((modifier & System.Windows.Forms.Keys.Alt) != 0)
        {
            k += "Alt+";
        }
        if ((modifier & System.Windows.Forms.Keys.Control) != 0)
        {
            k += "Ctrl+";
        }

        string keyname = key.ToString();
        if (keyname.Length == 2 && keyname[0] == 'D')
            keyname = keyname.Substring(1);
        else if (keyname.StartsWith("Oem") && keyname.Length > 4)       // leave oem1-9, they are not standard.
            keyname = keyname.Substring(3);

        return k + keyname;
    }
}

