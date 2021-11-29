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
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    static public class FontHelpers
    {
        public static Font GetFont(string setting, Font deffont)
        {
            string[] values = setting.Split('`');
            if ( values.Length == 3 )
            {
                try
                {
                    return new Font(values[0], values[1].InvariantParseFloat(12),(FontStyle)Enum.Parse(typeof(FontStyle),values[2]));
                }
                catch( Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Font sel exception {ex}");
                }
            }
            return deffont;
        }

        public static string GetFontSettingString(Font n)       // font may be null
        {
            return n == null ? "" : n.Name + '`' + n.SizeInPoints.ToStringInvariant() + '`' + n.Style.ToString();
        }

        public static Font FontSelection(Control parent, Font curfont, int min = 4, int max = 36, bool musthaveregular = false)
        {
            using (FontDialog fd = new FontDialog())
            {
                fd.Font = BaseUtils.FontLoader.GetFont(curfont.Name, curfont.SizeInPoints);
                fd.MinSize = min;
                fd.MaxSize = max;
                DialogResult result;

                try
                {
                    result = fd.ShowDialog(parent);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }

                if (result == DialogResult.OK)
                {
                    if (!musthaveregular || fd.Font.Style == FontStyle.Regular)
                    {
                        return fd.Font;
                    }
                    else
                        ExtendedControls.MessageBoxTheme.Show(parent, "Font does not have regular style");
                }

                return null;
            }

        }
    }

}