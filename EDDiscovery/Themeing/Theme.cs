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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;
using ExtendedControls;
using System.Windows.Forms.DataVisualization.Charting;
using EliteDangerousCore.DB;

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
        public bool RestoreSettings()
        {
            bool ok = true;
            Console.WriteLine("Theme ID " + Settings.ThemeID);

            int themeidstored = SQLiteDBClass.GetSettingInt("ThemeID", -1);

            if ( themeidstored != -1 && themeidstored != Settings.ThemeID )
            {
                //DialogResult res = ExtendedControls.MessageBoxTheme.Show("The theme stored has missing colors or other missing information" + Environment.NewLine +
                //      "that this new version of EDDiscovery needs." + Environment.NewLine + Environment.NewLine +
                //      "Choose OK to use the stored theme, and then correct the missing colors or other information manually using the Theme Editor in Settings" + Environment.NewLine + Environment.NewLine +
                //      "Choose Cancel to go back to windows default, then pick a new standard theme.", "ED Discovery Theme Warning!" , MessageBoxButtons.OKCancel);

                //if (res == DialogResult.Cancel)     // if cancel, we abort,
                //    return;
                ok = false;

            }

            if (UserDatabase.Instance.KeyExists("ThemeNameOf"))           // (keep previous check) if there.. get the others with a good default in case the db is screwed.
            {
                currentsettings.name = SQLiteDBClass.GetSettingString("ThemeNameOf", "Custom");
                currentsettings.windowsframe = SQLiteDBClass.GetSettingBool("ThemeWindowsFrame", true);
                currentsettings.formopacity = SQLiteDBClass.GetSettingDouble("ThemeFormOpacity", 100);
                currentsettings.fontname = SQLiteDBClass.GetSettingString("ThemeFont", defaultfont);
                currentsettings.fontsize = (float)SQLiteDBClass.GetSettingDouble("ThemeFontSize", defaultfontsize);
                currentsettings.buttonstyle = SQLiteDBClass.GetSettingString("ButtonStyle", buttonstyle_system);
                currentsettings.textboxborderstyle = SQLiteDBClass.GetSettingString("TextBoxBorderStyle", textboxborderstyle_fixed3D);

                foreach (Settings.CI ck in themelist[0].colors.Keys)         // use themelist to find the key names, as we modify currentsettings as we go and that would cause an exception
                {
                    int d = themelist[0].colors[ck].ToArgb();               // use windows default colors.
                    Color c = Color.FromArgb(SQLiteDBClass.GetSettingInt("ThemeColor" + ck.ToString(), d));
                    currentsettings.colors[ck] = c;
                }
            }
            return ok;
        }

        public void SaveSettings(string filename)
        {
            SQLiteDBClass.PutSettingInt("ThemeID", Settings.ThemeID);
            SQLiteDBClass.PutSettingString("ThemeNameOf", currentsettings.name);
            SQLiteDBClass.PutSettingBool("ThemeWindowsFrame", currentsettings.windowsframe);
            SQLiteDBClass.PutSettingDouble("ThemeFormOpacity", currentsettings.formopacity);
            SQLiteDBClass.PutSettingString("ThemeFont", currentsettings.fontname);
            SQLiteDBClass.PutSettingDouble("ThemeFontSize", currentsettings.fontsize);
            SQLiteDBClass.PutSettingString("ButtonStyle", currentsettings.buttonstyle);
            SQLiteDBClass.PutSettingString("TextBoxBorderStyle", currentsettings.textboxborderstyle);
            
            foreach (Settings.CI ck in currentsettings.colors.Keys)
            {
                SQLiteDBClass.PutSettingInt("ThemeColor" + ck.ToString(), currentsettings.colors[ck].ToArgb());
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
                    JObject jo = JObject.Parse(File.ReadAllText(fi.FullName));
                    {
                        Settings set = new Settings();

                        set.name = fi.Name.Replace(".eddtheme", "");
                        set.colors = new Dictionary<Settings.CI, Color>();

                        foreach (Settings.CI ck in Enum.GetValues(typeof(Settings.CI)))           // all enums
                        {
                            Color d = themelist[0].colors[ck];
                            set.colors.Add(ck, jo[ck.ToString()].Color(d));
                        }

                        set.windowsframe = jo["windowsframe"].Bool(themelist[0].windowsframe);
                        set.formopacity = jo["formopacity"].Double(themelist[0].formopacity);
                        set.fontname = jo["fontname"].Str(themelist[0].fontname);
                        set.fontsize = jo["fontsize"].Float(themelist[0].fontsize);
                        set.buttonstyle = jo["buttonstyle"].Str(themelist[0].buttonstyle);
                        set.textboxborderstyle = jo["textboxborderstyle"].Str(themelist[0].textboxborderstyle);
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
