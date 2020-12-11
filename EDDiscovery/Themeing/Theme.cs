/*
 * Copyright © 2016-2020 EDDiscovery development team
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

using BaseUtils.JSON;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace EDDiscovery
{
    public class EDDTheme : ExtendedControls.ThemeStandard
    {
        private static EDDTheme _instance;

        public static EDDTheme Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EDDTheme();
                    ExtendedControls.ThemeableFormsInstance.Instance = _instance;
                }

                return _instance;
            }
        }
        
        private EDDTheme()
        {
            MessageBoxWindowIcon = EDDiscovery.Properties.Resources.edlogo_3mo_icon;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if ok.  False  Means missing colors in theme. </returns>
        public void RestoreSettings()
        {
            if (EliteDangerousCore.DB.UserDatabase.Instance.KeyExists("ThemeNameOf"))           // (keep previous check) if there.. get the others with a good default in case the db is screwed.
            {
                currentsettings.name = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("ThemeNameOf", "Custom");
                currentsettings.windowsframe = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool("ThemeWindowsFrame", true);
                currentsettings.formopacity = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble("ThemeFormOpacity", 100);
                currentsettings.fontname = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("ThemeFont", defaultfont);
                currentsettings.fontsize = (float)EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble("ThemeFontSize", defaultfontsize);
                currentsettings.buttonstyle = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("ButtonStyle", buttonstyle_system);
                currentsettings.textboxborderstyle = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString("TextBoxBorderStyle", textboxborderstyle_fixed3D);

                foreach (Settings.CI ck in themelist[0].colors.Keys)         // use themelist to find the key names, as we modify currentsettings as we go and that would cause an exception
                {
                    int d = themelist[0].colors[ck].ToArgb();               // use windows default colors.
                    Color c = Color.FromArgb(EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt("ThemeColor" + ck.ToString(), d));
                    currentsettings.colors[ck] = c;
                }
            }
        }

        public void SaveSettings(string filename)
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("ThemeNameOf", currentsettings.name);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool("ThemeWindowsFrame", currentsettings.windowsframe);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble("ThemeFormOpacity", currentsettings.formopacity);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("ThemeFont", currentsettings.fontname);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble("ThemeFontSize", currentsettings.fontsize);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("ButtonStyle", currentsettings.buttonstyle);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString("TextBoxBorderStyle", currentsettings.textboxborderstyle);
            
            foreach (Settings.CI ck in currentsettings.colors.Keys)
            {
                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt("ThemeColor" + ck.ToString(), currentsettings.colors[ck].ToArgb());
            }

            if ( filename != null )         
                SaveFile(filename);
        }


        public void LoadThemes()  
        {
            base.LoadBaseThemes();

            string themepath = EDDOptions.Instance.ThemeAppDirectory();
            DirectoryInfo dirInfo = new DirectoryInfo(themepath);
            FileInfo[] allFiles = null;

            try
            {
                allFiles = dirInfo.GetFiles("*.eddtheme");
            }
            catch
            {
            }

            if (allFiles == null)
            {
                return;
            }

            foreach (FileInfo fi in allFiles)
            {
                try
                {
                    JObject jo = JObject.ParseThrowCommaEOL(File.ReadAllText(fi.FullName));
                    {
                        Settings set = new Settings();

                        set.name = fi.Name.Replace(".eddtheme", "");
                        set.colors = new Dictionary<Settings.CI, Color>();
                        System.Diagnostics.Debug.WriteLine("Loading Theme : " + set.name);

                        foreach (Settings.CI ck in Enum.GetValues(typeof(Settings.CI)))           // all enums
                        {
                            bool done = false;

                            try
                            {
                                string s = jo[ck.ToString()].StrNull();
                                if (s != null)
                                {
                                    Color c = System.Drawing.ColorTranslator.FromHtml(s);   // may except if not valid HTML colour
                                    set.colors.Add(ck, c);
                                    done = true;
                                    System.Diagnostics.Debug.WriteLine("Color.FromArgb({0},{1},{2},{3}), // {4}", c.A, c.R, c.G, c.B, ck.ToString());
                                }
                            }
                            catch
                            {
                                System.Diagnostics.Debug.WriteLine("Theme has invalid colour");
                            }

                            try
                            {
                                if (!done)
                                {
                                    string gridtext = jo[Settings.CI.grid_celltext.ToString()].StrNull();
                                    string gridback = jo[Settings.CI.grid_cellbackground.ToString()].StrNull();

                                    if (ck == Settings.CI.grid_altcelltext && gridtext != null)
                                    {
                                        Color c = System.Drawing.ColorTranslator.FromHtml(gridtext);   // may except if not valid HTML colour
                                        set.colors.Add(ck, c);
                                    }
                                    else if (ck == Settings.CI.grid_altcellbackground && gridback != null)
                                    {
                                        Color c = System.Drawing.ColorTranslator.FromHtml(gridback);   // may except if not valid HTML colour
                                        set.colors.Add(ck, c);
                                    }
                                    else
                                    {
                                        Color def = themelist[0].colors[ck];        // 
                                        set.colors.Add(ck, def);
                                    }
                                }
                            }
                            catch
                            {
                                System.Diagnostics.Debug.WriteLine("Theme has invalid colour");
                            }
                        }

                        set.windowsframe = jo["windowsframe"].Bool(themelist[0].windowsframe);
                        set.formopacity = jo["formopacity"].Double(themelist[0].formopacity);
                        set.fontname = jo["fontname"].Str(themelist[0].fontname);
                        set.fontsize = jo["fontsize"].Float(themelist[0].fontsize);
                        set.buttonstyle = jo["buttonstyle"].Str(themelist[0].buttonstyle);
                        set.textboxborderstyle = jo["textboxborderstyle"].Str(themelist[0].textboxborderstyle);

                        System.Diagnostics.Debug.WriteLine("{0},{1},{2},{3})", set.windowsframe, set.formopacity, set.fontname, set.fontsize);

                        themelist.Add(set);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"LoadThemes Exception : {ex.Message}");
                }
            }
        }

        public JObject Settings2Json()
        {
            JObject jo = new JObject();

            foreach (Settings.CI ck in currentsettings.colors.Keys)
            {
                jo.Add(ck.ToString(), System.Drawing.ColorTranslator.ToHtml(currentsettings.colors[ck]));
            }

            jo.Add("windowsframe", currentsettings.windowsframe);
            jo.Add("formopacity", currentsettings.formopacity);
            jo.Add("fontname", currentsettings.fontname);
            jo.Add("fontsize", currentsettings.fontsize);
            jo.Add("buttonstyle", currentsettings.buttonstyle);
            jo.Add("textboxborderstyle", currentsettings.textboxborderstyle);
            return jo;
        }

        public bool SaveFile(string filename)
        {
            string themepath = EDDOptions.Instance.ThemeAppDirectory();
            JObject jo = Settings2Json();

            if (filename == null)
            {
                filename = Path.Combine(themepath, currentsettings.name) + ".eddtheme";
            }   

            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(jo.ToString());
            }

            return true;
        }


    }
}
