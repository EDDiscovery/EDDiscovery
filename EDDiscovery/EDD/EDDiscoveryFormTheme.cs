/*
 * Copyright 2015-2025 EDDiscovery development team
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
 */

using EliteDangerousCore.DB;
using ExtendedControls;
using QuickJSON;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class EDDiscoveryForm
    {
        public void ApplyTheme()    
        {
            panel_close.Visible = !ExtendedControls.Theme.Current.WindowsFrame;
            panel_minimize.Visible = !ExtendedControls.Theme.Current.WindowsFrame;
            label_version.Visible = !ExtendedControls.Theme.Current.WindowsFrame && !EDDOptions.Instance.DisableVersionDisplay;

            // note in no border mode, this is not visible on the title bar but it is in the taskbar..
            this.Text = "EDDiscovery" + (EDDOptions.Instance.DisableVersionDisplay ? "" : " " + label_version.Text);

            OnThemeChanging?.Invoke();          // tell anyone that this is happening

            ExtendedControls.Theme.Current.ApplyStd(this);

            statusStripEDD.Font = contextMenuStripTabs.Font = this.Font;

            PopOuts.OnThemeChanged();           // tell the pop out forms that theme has changed

            OnThemeChanged?.Invoke();           // and tell anyone else which is interested
        }

        private Theme GetThemeFromDB()
        {
            if (UserDatabase.Instance.KeyExists("ThemeSelected"))           // new db save method
            {
                string json = UserDatabase.Instance.GetSetting("ThemeSelected", "");
                JToken jo = JToken.Parse(json);
                if (jo != null)
                {
                   //System.Diagnostics.Debug.Assert(jo.Count == 130);

                    Theme tme = Theme.FromJSON(jo);
                    if (tme != null)      // overwrite any variables with ones accumulated
                    {
                        return tme;
                    }
                }
            }
            else if (UserDatabase.Instance.KeyExists("ThemeNameOf"))           // old db save method
            {
                JObject jo = new JObject();

                // we use FromJSON info in 2.6 of QuickJSON to read the JSON attributes under AltFmt to get the old names (clever huh!)
                var dict = QuickJSON.JToken.GetMemberAttributeSettings(typeof(Theme), "AltFmt", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                foreach (var ai in dict)
                {
                    var pi = (PropertyInfo)ai.Value.MemberInfo;

                    if (pi.PropertyType.Name.Contains("Color"))
                    {
                        var cname = "ThemeColor" + ai.Value.Name;
                        int dbv = UserDatabase.Instance.GetSetting(cname, -1);
                        if (dbv != -1)
                        {
                            Color p = Color.FromArgb(dbv);
                            jo[ai.Value.Name] = System.Drawing.ColorTranslator.ToHtml(p);
                        }
                    }
                }

                jo["windowsframe"] = UserDatabase.Instance.GetSetting("ThemeWindowsFrame", true);
                jo["formopacity"] = UserDatabase.Instance.GetSetting("ThemeFormOpacity", 100.0f);
                jo["fontname"] = UserDatabase.Instance.GetSetting("ThemeFont", "Arial");
                jo["fontsize"] = (float)UserDatabase.Instance.GetSetting("ThemeFontSize", 12F);
                jo["buttonstyle"] = UserDatabase.Instance.GetSetting("ButtonStyle", Theme.ButtonstyleGradient);
                jo["textboxborderstyle"] = UserDatabase.Instance.GetSetting("TextBoxBorderStyle", Theme.TextboxborderstyleColor);

                //jo.WriteJSONFile(@"c:\code\eddtheme.json", true);

                Theme tme = Theme.FromJSON(jo);

                if ( tme != null)      // overwrite any variables with ones accumulated
                {
                    //UserDatabase.Instance.DeleteKey("Theme%");  // remove all theme keys
                    //UserDatabase.Instance.DeleteKey("ButtonStyle"); 
                    //UserDatabase.Instance.DeleteKey("TextBoxBorderStyle");
                    //UserDatabase.Instance.PutSetting("ThemeSelected", tme.ToJSON().ToString(true));    // write back immediately in case we crash
                    return tme;
                }
            }

            return null;
        }

        private void SaveThemeToDB(ExtendedControls.Theme theme)
        {
#if true
            UserDatabase.Instance.PutSetting("ThemeSelected", theme.ToJSON().ToString(true));
#else
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("ThemeNameOf", theme.Name);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("ThemeWindowsFrame", theme.WindowsFrame);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("ThemeFormOpacity", theme.Opacity);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("ThemeFont", theme.FontName);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("ThemeFontSize", theme.FontSize);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("ButtonStyle", theme.ButtonStyle);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSetting("TextBoxBorderStyle", theme.TextBoxBorderStyle);

            var dict = QuickJSON.JToken.GetMemberAttributeSettings(typeof(Theme), "AltFmt", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var ai in dict)
            {
                var pi = (PropertyInfo)ai.Value.MemberInfo;

                if (pi.PropertyType.Name.Contains("Color"))
                {
                    var cname = "ThemeColor" + ai.Value.Name;
                    Color p = (Color)pi.GetValue(theme);
                    EliteDangerousCore.DB.UserDatabase.Instance.PutSetting(cname, p.ToArgb());
                }
            }
#endif        
        }

        public void EditTheme()
        {
            var themeeditor = new ThemeEditor() { TopMost = FindForm().TopMost };
            var curtheme = Theme.Current;

            themeeditor.ApplyChanges = (theme) => { Theme.Current = theme; ApplyTheme(); };

            themeeditor.InitForm(curtheme);     // makes a copy
            themeeditor.FormClosing += (sa, ea) =>
            {
                if (themeeditor.DialogResult == DialogResult.OK)
                {
                    Theme.Current = themeeditor.Theme;
                }
                else
                {
                    if (curtheme != Theme.Current)
                    {
                        Theme.Current = curtheme;
                        ApplyTheme();
                    }
                }
            };

            themeeditor.Show(FindForm());
        }


    }
}
