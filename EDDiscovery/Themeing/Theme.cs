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
using EDDiscovery.DB;
using Newtonsoft.Json.Linq;
using EDDiscovery;
using System.IO;
using System.Diagnostics;
using ExtendedControls;
using System.Windows.Forms.DataVisualization.Charting;

namespace EDDiscovery
{
    public class EDDTheme : ExtendedControls.ThemeableForms
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

        public static readonly string[] ButtonStyles = "System Flat Gradient".Split();
        public static readonly string[] TextboxBorderStyles = "None FixedSingle Fixed3D Colour".Split();

        private static string buttonstyle_system = ButtonStyles[0];
        private static string buttonstyle_gradient = ButtonStyles[2];
        private static string textboxborderstyle_fixed3D = TextboxBorderStyles[2];
        private static string textboxborderstyle_color = TextboxBorderStyles[3];

        public System.Drawing.Icon MessageBoxWindowIcon { get; set; }

        public struct Settings
        {
            static int MajorThemeID = 1;         // Change if major change made outside of number of colors.
            static public int ThemeID { get { return MajorThemeID * 10000 + Enum.GetNames(typeof(CI)).Length; } }

            public enum CI
            {
                form,
                button_back, button_border, button_text,
                grid_borderback, grid_bordertext,
                grid_cellbackground, grid_celltext, grid_borderlines,
                grid_sliderback, grid_scrollarrow, grid_scrollbutton,
                textbox_fore, textbox_highlight, textbox_success,textbox_back, textbox_border,
                textbox_sliderback, textbox_scrollarrow, textbox_scrollbutton,
                menu_back, menu_fore, menu_dropdownback, menu_dropdownfore,
                group_back, group_text, group_borderlines,
                travelgrid_nonvisted, travelgrid_visited,
                checkbox,checkbox_tick,
                label,
                tabcontrol_borderlines,
                toolstrip_back, toolstrip_border, toolstrip_buttonchecked, s_panel
            };

            public string name;         // name of scheme
            public Dictionary<CI, Color> colors;       // dictionary of colors, indexed by CI.
            public bool windowsframe;
            public double formopacity;
            public string fontname;         // Font.. (empty means don't override)
            public float fontsize;
            public string buttonstyle;
            public string textboxborderstyle;



            public Settings(String n, Color f,
                                        Color bb, Color bf, Color bborder, string bstyle,
                                        Color gb, Color gbt, Color gbck, Color gt, Color gridlines,
                                        Color gsb, Color gst, Color gsbut,
                                        Color tn, Color tv, 
                                        Color tbb, Color tbf, Color tbh, Color tbs, Color tbborder, string tbbstyle ,
                                        Color tbsb, Color tbst, Color tbbut,
                                        Color c, Color ctick,
                                        Color mb, Color mf, Color mdb, Color mdf,
                                        Color l,
                                        Color grpb, Color grpt, Color grlines,
                                        Color tabborderlines,
                                        Color ttb, Color ttborder, Color ttbuttonchecked, Color sPanel,
                                        bool wf, double op, string ft, float fs)            // ft = empty means don't set it
            {
                name = n;
                colors = new Dictionary<CI, Color>();
                colors.Add(CI.form, f);
                colors.Add(CI.button_back, bb); colors.Add(CI.button_text, bf); colors.Add(CI.button_border,bborder);
                colors.Add(CI.grid_borderback, gb); colors.Add(CI.grid_bordertext, gbt);
                colors.Add(CI.grid_cellbackground, gbck); colors.Add(CI.grid_celltext, gt); colors.Add(CI.grid_borderlines, gridlines);
                colors.Add(CI.grid_sliderback, gsb); colors.Add(CI.grid_scrollarrow, gst); colors.Add(CI.grid_scrollbutton, gsbut);
                colors.Add(CI.travelgrid_nonvisted, tn); colors.Add(CI.travelgrid_visited, tv);
                colors.Add(CI.textbox_back, tbb); colors.Add(CI.textbox_fore, tbf);
                colors.Add(CI.textbox_sliderback, tbsb); colors.Add(CI.textbox_scrollarrow, tbst); colors.Add(CI.textbox_scrollbutton, tbbut);
                colors.Add(CI.textbox_highlight, tbh); colors.Add(CI.textbox_success, tbs);
                colors.Add(CI.textbox_border, tbborder);
                colors.Add(CI.checkbox, c); colors.Add(CI.checkbox_tick, ctick);
                colors.Add(CI.menu_back, mb); colors.Add(CI.menu_fore, mf); colors.Add(CI.menu_dropdownback, mdb); colors.Add(CI.menu_dropdownfore, mdf);
                colors.Add(CI.label, l);
                colors.Add(CI.group_back, grpb); colors.Add(CI.group_text, grpt); colors.Add(CI.group_borderlines, grlines);
                colors.Add(CI.tabcontrol_borderlines, tabborderlines);
                colors.Add(CI.toolstrip_back, ttb); colors.Add(CI.toolstrip_border, ttborder); colors.Add(CI.toolstrip_buttonchecked, ttbuttonchecked);
                colors.Add(CI.s_panel, sPanel);
                buttonstyle = bstyle; textboxborderstyle = tbbstyle;
                windowsframe = wf; formopacity = op; fontname = ft; fontsize = fs;
            }

            public Settings(JObject jo, string settingsname, Settings defcols)            // From json. defcols is the colours to use if the json does not have it.
            {
                name = settingsname.Replace(".eddtheme", "");
                colors = new Dictionary<CI, Color>();

                foreach (CI ck in Enum.GetValues(typeof(CI)))           // all enums
                {
                    Color d = defcols.colors[ck];
                    colors.Add(ck, JGetColor(jo, ck.ToString(),d));
                }

                windowsframe = GetBool(jo["windowsframe"],defcols.windowsframe);
                formopacity = GetFloat(jo["formopacity"],(float)defcols.formopacity);
                fontname = GetString(jo["fontname"], defcols.fontname);
                fontsize = GetFloat(jo["fontsize"], defcols.fontsize);
                buttonstyle = GetString(jo["buttonstyle"], defcols.buttonstyle);
                textboxborderstyle = GetString(jo["textboxborderstyle"], defcols.textboxborderstyle);
            }

            public Settings(string n)                                               // gets you windows default colours
            {
                name = n; 
                colors = new Dictionary<CI, Color>();
                colors.Add(CI.form, SystemColors.Menu);
                colors.Add(CI.button_back, SystemColors.Control); colors.Add(CI.button_text, SystemColors.ControlText); colors.Add(CI.button_border, SystemColors.Menu);
                colors.Add(CI.grid_borderback, SystemColors.Menu); colors.Add(CI.grid_bordertext, SystemColors.MenuText);
                colors.Add(CI.grid_cellbackground, SystemColors.ControlLightLight); colors.Add(CI.grid_celltext, SystemColors.MenuText); colors.Add(CI.grid_borderlines, SystemColors.ControlDark);
                colors.Add(CI.grid_sliderback, SystemColors.ControlLight); colors.Add(CI.grid_scrollarrow, SystemColors.MenuText); colors.Add(CI.grid_scrollbutton, SystemColors.Control);
                colors.Add(CI.travelgrid_nonvisted, Color.Blue); colors.Add(CI.travelgrid_visited, SystemColors.MenuText);
                colors.Add(CI.textbox_back, SystemColors.Window); colors.Add(CI.textbox_fore, SystemColors.WindowText); colors.Add(CI.textbox_highlight, Color.Red); colors.Add(CI.textbox_success, Color.Green); colors.Add(CI.textbox_border, SystemColors.Menu);
                colors.Add(CI.textbox_sliderback, SystemColors.ControlLight); colors.Add(CI.textbox_scrollarrow, SystemColors.MenuText); colors.Add(CI.textbox_scrollbutton, SystemColors.Control);
                colors.Add(CI.checkbox, SystemColors.MenuText); colors.Add(CI.checkbox_tick, SystemColors.MenuHighlight);
                colors.Add(CI.menu_back, SystemColors.Menu); colors.Add(CI.menu_fore, SystemColors.MenuText); colors.Add(CI.menu_dropdownback, SystemColors.ControlLightLight); colors.Add(CI.menu_dropdownfore, SystemColors.MenuText);
                colors.Add(CI.label, SystemColors.MenuText);
                colors.Add(CI.group_back, SystemColors.Menu); colors.Add(CI.group_text, SystemColors.MenuText); colors.Add(CI.group_borderlines, SystemColors.ControlDark);
                colors.Add(CI.tabcontrol_borderlines, SystemColors.ControlDark);
                colors.Add(CI.toolstrip_back, SystemColors.Window); colors.Add(CI.toolstrip_border, SystemColors.Menu); colors.Add(CI.toolstrip_buttonchecked, SystemColors.MenuText);
                colors.Add(CI.s_panel, Color.Orange);
                buttonstyle = buttonstyle_system;
                textboxborderstyle = textboxborderstyle_fixed3D;
                windowsframe = true;
                formopacity = 100;
                fontname = defaultfont;
                fontsize = defaultfontsize;
            }
                                                            // copy constructor, takes a real copy, with overrides
            public Settings(Settings other,string newname = null , string newfont = null, float newfontsize = 0, double opaque = 0)                
            {
                name = (newname!=null) ? newname : other.name;
                fontname = (newfont != null) ? newfont : other.fontname;
                fontsize = (newfontsize != 0) ? newfontsize : other.fontsize;
                windowsframe = other.windowsframe; formopacity = other.formopacity;
                buttonstyle = other.buttonstyle; textboxborderstyle = other.textboxborderstyle;
                formopacity = (opaque > 0) ? opaque: other.formopacity;
                colors = new Dictionary<CI, Color>();
                foreach (CI ck in other.colors.Keys)
                {
                    colors.Add(ck, other.colors[ck]);
                }
            }

            static private Color JGetColor(JObject jo, string name , Color defc)
            {
                string colstr = GetString(jo[name],null);

                if (colstr == null)
                    return defc;

                return System.Drawing.ColorTranslator.FromHtml(colstr);
            }

            static private bool GetBool(JToken jToken, bool def)
            {
                if (IsNullOrEmptyT(jToken))
                    return def;
                return jToken.Value<bool>();
            }

            static private float GetFloat(JToken jToken, float def)
            {
                if (IsNullOrEmptyT(jToken))
                    return def;
                return jToken.Value<float>();
            }


            static private int GetInt(JToken jToken, int def)
            {
                if (IsNullOrEmptyT(jToken))
                    return def;
                return jToken.Value<int>();
            }

            static private string GetString(JToken jToken,string def)
            {
                if (IsNullOrEmptyT(jToken))
                    return def;
                return jToken.Value<string>();
            }
            
            static private bool IsNullOrEmptyT(JToken token)
            {
                return (token == null) ||
                       (token.Type == JTokenType.Array && !token.HasValues) ||
                       (token.Type == JTokenType.Object && !token.HasValues) ||
                       (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                       (token.Type == JTokenType.Null);
            }
            
        }

        public static float minfontsize = 4;
        public static string defaultfont = "Microsoft Sans Serif";
        public static float defaultfontsize = 8.25F;

        public string Name { get { return currentsettings.name; } set { currentsettings.name = value; } }

        public Color LabelColor { get { return currentsettings.colors[Settings.CI.label]; } set { SetCustom(); currentsettings.colors[Settings.CI.label] = value; } }
        public Color SPanelColor { get { return currentsettings.colors[Settings.CI.s_panel]; } set { SetCustom(); currentsettings.colors[Settings.CI.s_panel] = value; } }
        public Color TextBackColor { get { return currentsettings.colors[Settings.CI.textbox_back]; } set { SetCustom(); currentsettings.colors[Settings.CI.textbox_back] = value; } }
        public Color TextBlockColor { get { return currentsettings.colors[Settings.CI.textbox_fore]; } set { SetCustom(); currentsettings.colors[Settings.CI.textbox_fore] = value; } }
        public Color TextBlockHighlightColor { get { return currentsettings.colors[Settings.CI.textbox_highlight]; } set { SetCustom(); currentsettings.colors[Settings.CI.textbox_highlight] = value; } }
        public Color TextBlockSuccessColor { get { return currentsettings.colors[Settings.CI.textbox_success]; } set { SetCustom(); currentsettings.colors[Settings.CI.textbox_success] = value; } }
        public string TextBlockBorderStyle { get { return currentsettings.textboxborderstyle; } set { SetCustom(); currentsettings.textboxborderstyle = value; } }
        public Color VisitedSystemColor { get { return currentsettings.colors[Settings.CI.travelgrid_visited]; } set { SetCustom(); currentsettings.colors[Settings.CI.travelgrid_visited] = value; } }
        public Color NonVisitedSystemColor { get { return currentsettings.colors[Settings.CI.travelgrid_nonvisted]; } set { SetCustom(); currentsettings.colors[Settings.CI.travelgrid_nonvisted] = value; } }
        public Color GridCellText { get { return currentsettings.colors[Settings.CI.grid_celltext]; } set { SetCustom(); currentsettings.colors[Settings.CI.grid_celltext] = value; } }

        public string ButtonStyle { get { return currentsettings.buttonstyle; } set { SetCustom(); currentsettings.buttonstyle= value; } }

        public bool WindowsFrame { get { return currentsettings.windowsframe; } set { SetCustom(); currentsettings.windowsframe = value; } }
        public double Opacity { get { return currentsettings.formopacity; } set { SetCustom(); currentsettings.formopacity = value; } }
        public string FontName { get { return currentsettings.fontname; } set { SetCustom(); currentsettings.fontname = value; } }
        public float FontSize { get { return currentsettings.fontsize; } set { SetCustom(); currentsettings.fontsize = value; } }

        public Font GetFont
        {
            get
            {
                if (currentsettings.fontname.Equals("") || currentsettings.fontsize < minfontsize)
                {
                    currentsettings.fontname = "Microsoft Sans Serif";          // in case schemes were loaded
                    currentsettings.fontsize = 8.25F;
                }

                return new Font(currentsettings.fontname, currentsettings.fontsize);
            }
        }


        private Settings currentsettings;           // if name = custom, then its not a standard theme..
        private List<Settings> themelist;
        
        public EDDToolStripRenderer toolstripRenderer;

        private EDDTheme()
        {
            toolstripRenderer = new EDDToolStripRenderer();
            themelist = new List<Settings>();           // theme list in
            currentsettings = new Settings("Windows Default");  // this is our default
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

            if (SQLiteConnectionUser.keyExists("ThemeNameOf"))           // (keep previous check) if there.. get the others with a good default in case the db is screwed.
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
            themelist.Clear();

            themelist.Add(new Settings("Windows Default"));         // windows default..

            themelist.Add(new Settings("Orange Delight", Color.Black,
                Color.FromArgb(255, 48, 48, 48), Color.Orange, Color.DarkOrange, buttonstyle_gradient, // button
                Color.FromArgb(255, 176, 115, 0), Color.Black,  // grid border
                Color.Black, Color.Orange, Color.DarkOrange, // grid
                Color.Black, Color.Orange, Color.DarkOrange, // grid back, arrow, button
                Color.Orange, Color.White, // travel
                Color.Black, Color.Orange, Color.Red, Color.Green, Color.DarkOrange, textboxborderstyle_color, // text box
                Color.Black, Color.Orange, Color.DarkOrange, // text back, arrow, button
                Color.Orange, Color.FromArgb(255, 65, 33, 33), // checkbox
                Color.Black, Color.Orange, Color.DarkOrange, Color.Yellow,  // menu
                Color.Orange,  // label
                Color.FromArgb(255, 32, 32, 32), Color.Orange, Color.FromArgb(255, 130, 71, 0), // group
                Color.DarkOrange, // tab control
                Color.Black, Color.DarkOrange, Color.Orange, Color.Orange, // tooltip
                false, 95, "Microsoft Sans Serif", 8.25F));

            themelist.Add(new Settings(themelist[themelist.Count - 1], "Elite EuroCaps", "Euro Caps", 12F, 95));

            Color butback = Color.FromArgb(255,32, 32, 32);
            themelist.Add(new Settings("Elite EuroCaps Less Border", Color.Black,
                Color.FromArgb(255, 64, 64, 64), Color.Orange, Color.FromArgb(255, 96, 96, 96), buttonstyle_gradient, // button
                Color.FromArgb(255, 176, 115, 0), Color.Black,  // grid border
                butback, Color.Orange, Color.DarkOrange, // grid
                butback, Color.Orange, Color.DarkOrange, // grid back, arrow, button
                Color.Orange, Color.White, // travel
                butback, Color.Orange, Color.Red, Color.Green, Color.FromArgb(255,64,64,64), textboxborderstyle_color, // text box
                butback, Color.Orange, Color.DarkOrange, // text back, arrow, button
                Color.Orange, Color.FromArgb(255, 65, 33, 33),// checkbox
                Color.Black, Color.Orange, Color.DarkOrange, Color.Yellow,  // menu
                Color.Orange,  // label
                Color.Black, Color.Orange, Color.FromArgb(255, 130, 71, 0), // group
                Color.DarkOrange, // tab control
                Color.Black, Color.DarkOrange, Color.Orange, Color.Orange, // tooltip
                false, 100, "Euro Caps", 12F));

            themelist.Add(new Settings(themelist[themelist.Count - 1], "Elite Verdana", "Verdana", 8F));
            themelist.Add(new Settings(themelist[themelist.Count - 1], "Elite Calisto", "Calisto MT", 12F));

            Color r1 = Color.FromArgb(255, 160, 0, 0);
            Color r2 = Color.FromArgb(255, 64, 0, 0);
            themelist.Add(new Settings("Night Vision", Color.Black,
                Color.FromArgb(255, 48, 48, 48), r1, r2, buttonstyle_gradient, // button
                r2, Color.Black,  // grid border
                Color.Black, r1, r2, // grid
                Color.Black, r1, r2, // grid back, arrow, button
                r1, Color.Green, // travel
                Color.Black, r1, Color.Orange, Color.Green, r2, textboxborderstyle_color, // text box
                Color.Black, r1, r2, // text back, arrow, button
                r1, Color.FromArgb(255, 65, 33, 33), // checkbox
                Color.Black, r1, r2, Color.Yellow,  // menu
                r1,  // label
                Color.FromArgb(255, 8, 8, 8), r1, Color.FromArgb(255, 130, 71, 0), // group
                r2, // tab control
                Color.Black, r2, r1, r1, // tooltip
                false, 95, "Microsoft Sans Serif", 10F));

            themelist.Add(new Settings(themelist[themelist.Count - 1], "Night Vision EuroCaps", "Euro Caps", 12F, 95));

            themelist.Add(new Settings("EuroCaps Grey",
                                        SystemColors.Menu,
                                        SystemColors.Control, SystemColors.ControlText, Color.DarkGray, buttonstyle_gradient,// button
                                        SystemColors.Menu, SystemColors.MenuText,  // grid border
                                        SystemColors.ControlLightLight, SystemColors.MenuText, SystemColors.ControlDark, // grid
                                        SystemColors.ControlLightLight, SystemColors.MenuText, SystemColors.ControlDark, // grid scroll
                                        Color.Blue, SystemColors.MenuText, // travel
                                        SystemColors.Window, SystemColors.WindowText, Color.Red, Color.Green, Color.DarkGray, textboxborderstyle_color,// text
                                        SystemColors.ControlLightLight, SystemColors.MenuText, SystemColors.ControlDark, // text box
                                        SystemColors.MenuText, SystemColors.MenuHighlight , // checkbox
                                        SystemColors.Menu, SystemColors.MenuText, SystemColors.ControlLightLight, SystemColors.MenuText,  // menu
                                        SystemColors.MenuText,  // label
                                        SystemColors.Menu, SystemColors.MenuText, SystemColors.ControlDark, // group
                                        SystemColors.ControlDark, // tab control
                                        SystemColors.Window, SystemColors.Menu, SystemColors.MenuText, SystemColors.ControlLightLight, //tooltip
                                        false, 95, "Euro Caps", 12F));

            themelist.Add(new Settings(themelist[themelist.Count - 1], "Verdana Grey", "Verdana", 8F));

            themelist.Add(new Settings("Blue Wonder", Color.DarkBlue,
                                               Color.Blue, Color.White, Color.White, buttonstyle_gradient,// button
                                               Color.DarkBlue, Color.White,  // grid border
                                               Color.DarkBlue, Color.White, Color.Blue, // grid
                                               Color.DarkBlue, Color.White, Color.Blue, // grid scroll
                                               Color.White, Color.Cyan, // travel
                                               Color.DarkBlue, Color.White, Color.Red, Color.Green, Color.White, textboxborderstyle_color,// text box
                                               Color.DarkBlue, Color.White, Color.Blue, // text scroll
                                               Color.White, Color.Black , // checkbox
                                               Color.DarkBlue, Color.White, Color.DarkBlue, Color.White,  // menu
                                               Color.White,  // label
                                               Color.DarkBlue, Color.White, Color.Blue, // group
                                               Color.Blue,
                                               Color.DarkBlue, Color.White, Color.Red, Color.LightBlue,
                                               false, 95, "Microsoft Sans Serif", 8.25F));

            Color baizegreen = Color.FromArgb(255, 13, 68, 13);
            themelist.Add(new Settings("Green Baize", baizegreen,
                                               baizegreen, Color.White, Color.White, buttonstyle_gradient,// button
                                               baizegreen, Color.White,  // grid border
                                               baizegreen, Color.White, Color.LightGreen, // grid
                                               baizegreen, Color.White, Color.LightGreen, // grid scroll
                                               Color.White, Color.FromArgb(255, 78, 190, 27), // travel
                                               baizegreen, Color.White, Color.Red, Color.Green, Color.White, textboxborderstyle_color,// text box
                                               baizegreen, Color.White, Color.LightGreen, // text scroll
                                               Color.White, Color.Black, // checkbox
                                               baizegreen, Color.White, baizegreen, Color.White,  // menu
                                               Color.White,  // label
                                               baizegreen, Color.White, Color.LightGreen, // group
                                               Color.LightGreen, Color.White, Color.Red,
                                               baizegreen, baizegreen,
                                               false, 95, "Microsoft Sans Serif", 8.25F));

            string themepath = "";

            try
            {
                themepath = Path.Combine(EDDConfig.Options.AppDataDirectory, "Theme");
                if (!Directory.Exists(themepath))
                {
                    Directory.CreateDirectory(themepath);
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Unable to create the folder '{themepath}'");
                Trace.WriteLine($"Exception: {ex.Message}");

                return;
            }


            // Search for theme files
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
                    themelist.Add(new Settings(jo, fi.Name,themelist[0]));
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
            string themepath = "";
            JObject jo = Settings2Json();

            try
            {
                themepath = Path.Combine(EDDConfig.Options.AppDataDirectory, "Theme") ;
                if (!Directory.Exists(themepath))
                {
                    Directory.CreateDirectory(themepath);
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Unable to create the folder '{themepath}'");
                Trace.WriteLine($"Exception: {ex.Message}");

                return false;
            }

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

        public List<string> GetThemeList()
        {
            List<string> result = new List<string>();

            for (int i = 0; i < themelist.Count; i++)
                result.Add(themelist[i].name);

            return result;
        }

        private int FindThemeIndex(string themename)
        {
            for (int i = 0; i < themelist.Count; i++)
            {
                if (themelist[i].name.Equals(themename))
                    return i;
            }

            return -1;
        }

        public int GetIndexOfCurrentTheme()
        {
            return FindThemeIndex(currentsettings.name);
        }

        public bool IsFontAvailableInTheme(string themename, out string fontwanted)
        {
            int i = FindThemeIndex(themename);
            fontwanted = null;

            if (i != -1)
            {
                int size = (int)themelist[i].fontsize;
                fontwanted = themelist[i].fontname;
                if (size < 1)
                    size = 9;

                using (Font fntnew = new Font(fontwanted, size, FontStyle.Regular))
                {
                    return string.Compare(fntnew.Name, fontwanted, true) == 0;
                }
            }

            return false;
        }

        public bool SetThemeByName(string themename)                           // given a theme name, select it if possible
        {
            int i = FindThemeIndex(themename);
            if (i != -1)
            {
                currentsettings = new Settings(themelist[i]);           // do a copy, not a reference assign..
                return true;
            }
            return false;
        }

        public bool IsCustomTheme()
        { return currentsettings.name.Equals("Custom"); }

        public void SetCustom()
        { currentsettings.name = "Custom"; }                                // set so custom..

        public bool ApplyToForm(Form form, Font fnt= null )                 
        {
            if (fnt == null)
                fnt = GetFont;                                          // do not apply to Form, only to sub controls

            form.FormBorderStyle = WindowsFrame ? FormBorderStyle.Sizable : FormBorderStyle.None;
            form.Opacity = currentsettings.formopacity / 100;
            form.BackColor = currentsettings.colors[Settings.CI.form];
  
            ApplyToControls(form,fnt);        // form is the parent of form!

            UpdateToolsTripRenderer();

            return WindowsFrame;
        }

        public void ApplyToControls(Control parent, Font fnt = null)
        {
            if (fnt == null)
                fnt = GetFont;                                          // do not apply to Form, only to sub controls

            foreach (Control c in parent.Controls)
                UpdateColorControls(parent, c, fnt, 0);
        }

        private void UpdateColorControls(Control parent , Control myControl, Font fnt, int level)
        {
#if DEBUG
            //string pad = "                             ".Substring(0, level);
            //System.Diagnostics.Debug.WriteLine(pad + level + ":" + parent.Name.ToString() + ":" + myControl.Name.ToString() + " " + myControl.ToString());
#endif
            float mouseoverscaling = 1.3F;
            float mouseselectedscaling = 1.5F;

            Type controltype = myControl.GetType();
            Type parentcontroltype = parent.GetType();
            if (!parentcontroltype.Namespace.Equals("ExtendedControls") && (controltype.Name.Equals("Button") || controltype.Name.Equals("RadioButton") || controltype.Name.Equals("GroupBox") ||
                controltype.Name.Equals("CheckBox") || controltype.Name.Equals("TextBox") ||
                controltype.Name.Equals("ComboBox") || (controltype.Name.Equals("RichTextBox") ) )
                )
            {
                Debug.Assert(false, myControl.Name + " of " + controltype.Name + " from " + parent.Name + " !!! Use the new controls in Controls folder - not the non visual themed ones!");
            }
            else if (myControl is RichTextBoxScroll)
            {
                RichTextBoxScroll ctrl = (RichTextBoxScroll)myControl;
                ctrl.BorderColor = Color.Transparent;
                ctrl.BorderStyle = BorderStyle.None;

                ctrl.TextBoxForeColor = currentsettings.colors[Settings.CI.textbox_fore];
                ctrl.TextBoxBackColor = currentsettings.colors[Settings.CI.textbox_back];

                ctrl.ScrollBarFlatStyle = FlatStyle.System;

                if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[1]))
                    ctrl.BorderStyle = BorderStyle.FixedSingle;
                else if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[2]))
                    ctrl.BorderStyle = BorderStyle.Fixed3D;
                else if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[3]))
                {
                    Color c1 = currentsettings.colors[Settings.CI.textbox_scrollbutton];
                    ctrl.BorderColor = currentsettings.colors[Settings.CI.textbox_border];
                    ctrl.ScrollBarBackColor = currentsettings.colors[Settings.CI.textbox_back];
                    ctrl.ScrollBarSliderColor = currentsettings.colors[Settings.CI.textbox_sliderback];
                    ctrl.ScrollBarBorderColor = ctrl.ScrollBarThumbBorderColor = 
                                ctrl.ScrollBarArrowBorderColor = currentsettings.colors[Settings.CI.textbox_border];
                    ctrl.ScrollBarArrowButtonColor = ctrl.ScrollBarThumbButtonColor = c1;
                    ctrl.ScrollBarMouseOverButtonColor = c1.Multiply(mouseoverscaling);
                    ctrl.ScrollBarMousePressedButtonColor = c1.Multiply(mouseselectedscaling);
                    ctrl.ScrollBarForeColor = currentsettings.colors[Settings.CI.textbox_scrollarrow];
                    ctrl.ScrollBarFlatStyle = FlatStyle.Popup;
                }
               
                if (myControl.Font.Name.Contains("Courier"))                  // okay if we ordered a fixed font, don't override
                {
                    Font fntf = new Font(myControl.Font.Name, currentsettings.fontsize); // make one of the selected size
                    myControl.Font = fntf;
                }
                else
                    myControl.Font = fnt;

                ctrl.Invalidate();
                ctrl.PerformLayout();
            }
            else if (myControl is TextBoxBorder)
            {
                TextBoxBorder ctrl = (TextBoxBorder)myControl;
                ctrl.ForeColor = currentsettings.colors[Settings.CI.textbox_fore];
                ctrl.BackColor = currentsettings.colors[Settings.CI.textbox_back];
                ctrl.ControlBackground = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.form]);
                ctrl.BorderColor = Color.Transparent;
                ctrl.BorderStyle = BorderStyle.None;
                ctrl.AutoSize = true;

                //TBD
                if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[0]))
                    ctrl.AutoSize = false;                                                 // with no border, the autosize clips the bottom of chars..
                else if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[1]))
                    ctrl.BorderStyle = BorderStyle.FixedSingle;
                else if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[2]))
                    ctrl.BorderStyle = BorderStyle.Fixed3D;
                else if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[3]))
                    ctrl.BorderColor = currentsettings.colors[Settings.CI.textbox_border];

                myControl.Font = fnt;

                if (myControl is AutoCompleteTextBox || myControl is AutoCompleteDGVColumn.CellEditControl) // derived from text box
                {
                    AutoCompleteTextBox actb = myControl as AutoCompleteTextBox;
                    actb.DropDownBackgroundColor = currentsettings.colors[Settings.CI.button_back];
                    actb.DropDownBorderColor = currentsettings.colors[Settings.CI.textbox_border];
                    actb.DropDownScrollBarButtonColor = currentsettings.colors[Settings.CI.textbox_scrollbutton];
                    actb.DropDownScrollBarColor = currentsettings.colors[Settings.CI.textbox_sliderback];
                    actb.DropDownMouseOverBackgroundColor = currentsettings.colors[Settings.CI.button_back].Multiply(mouseoverscaling);

                    if (currentsettings.buttonstyle.Equals(ButtonStyles[0]))
                        actb.FlatStyle = FlatStyle.System;
                    else if (currentsettings.buttonstyle.Equals(ButtonStyles[1])) // flat
                        actb.FlatStyle = FlatStyle.Flat;
                    else
                        actb.FlatStyle = FlatStyle.Popup;
                }

                ctrl.Invalidate();
            }
            else if (myControl is ButtonExt)
            {
                ButtonExt ctrl = (ButtonExt)myControl;
                ctrl.ForeColor = currentsettings.colors[Settings.CI.button_text];

                if (currentsettings.buttonstyle.Equals(ButtonStyles[0])) // system
                {
                    ctrl.FlatStyle = (ctrl.Image != null) ? FlatStyle.Standard : FlatStyle.System;
                    ctrl.UseVisualStyleBackColor = true;           // this makes it system..
                }
                else
                {
                    if (ctrl.Image != null)
                    {
                        //System.Diagnostics.Debug.WriteLine("Theme Image in " + ctrl.Name + " Map white to " + ctrl.ForeColor);
                        System.Drawing.Imaging.ColorMap colormap = new System.Drawing.Imaging.ColorMap();       // any drawn panel with drawn images    
                        colormap.OldColor = Color.FromArgb(134, 134, 134);                                        // gray is defined as the forecolour to use in system mode
                        colormap.NewColor = ctrl.ForeColor;
                        ctrl.SetDrawnBitmapRemapTable(new System.Drawing.Imaging.ColorMap[] { colormap });
                    }

                    ctrl.BackColor = (ctrl.Image != null) ? currentsettings.colors[Settings.CI.form] : currentsettings.colors[Settings.CI.button_back];
                    ctrl.FlatAppearance.BorderColor = (ctrl.Image != null) ? currentsettings.colors[Settings.CI.form] : currentsettings.colors[Settings.CI.button_border];
                    ctrl.FlatAppearance.BorderSize = 1;
                    ctrl.FlatAppearance.MouseOverBackColor = currentsettings.colors[Settings.CI.button_back].Multiply(mouseoverscaling);
                    ctrl.FlatAppearance.MouseDownBackColor = currentsettings.colors[Settings.CI.button_back].Multiply(mouseselectedscaling);

                    if (currentsettings.buttonstyle.Equals(ButtonStyles[1])) // flat
                        ctrl.FlatStyle = FlatStyle.Flat;
                    else
                        ctrl.FlatStyle = FlatStyle.Popup;
                }

                myControl.Font = fnt;
            }
            else if (myControl is TabControlCustom)
            {
                TabControlCustom ctrl = (TabControlCustom)myControl;

                if (!currentsettings.buttonstyle.Equals(ButtonStyles[0])) // not system
                {
                    ctrl.FlatStyle = (currentsettings.buttonstyle.Equals(ButtonStyles[1])) ? FlatStyle.Flat : FlatStyle.Popup;
                    ctrl.TabControlBorderColor = currentsettings.colors[Settings.CI.tabcontrol_borderlines].Multiply(0.6F);
                    ctrl.TabControlBorderBrightColor = currentsettings.colors[Settings.CI.tabcontrol_borderlines];
                    ctrl.TabNotSelectedBorderColor = currentsettings.colors[Settings.CI.tabcontrol_borderlines].Multiply(0.4F);
                    ctrl.TabNotSelectedColor = currentsettings.colors[Settings.CI.button_back];
                    ctrl.TabSelectedColor = currentsettings.colors[Settings.CI.button_back].Multiply(mouseselectedscaling);
                    ctrl.TabMouseOverColor = currentsettings.colors[Settings.CI.button_back].Multiply(mouseoverscaling);
                    ctrl.TextSelectedColor = currentsettings.colors[Settings.CI.button_text];
                    ctrl.TextNotSelectedColor = currentsettings.colors[Settings.CI.button_text].Multiply(0.8F);
                    ctrl.TabStyle = new ExtendedControls.TabStyleAngled();
                }
                else
                    ctrl.FlatStyle = FlatStyle.System;

                ctrl.Font = fnt;
            }
            else if (myControl is ListControlCustom)
            {
                ListControlCustom ctrl = (ListControlCustom)myControl;
                ctrl.ForeColor = currentsettings.colors[Settings.CI.button_text];

                if (currentsettings.buttonstyle.Equals(ButtonStyles[0]))
                {
                    ctrl.FlatStyle = FlatStyle.System;
                }
                else
                {
                    ctrl.BackColor = currentsettings.colors[Settings.CI.button_back];
                    ctrl.BorderColor = currentsettings.colors[Settings.CI.button_border];
                    ctrl.ScrollBarButtonColor = currentsettings.colors[Settings.CI.textbox_scrollbutton];
                    ctrl.ScrollBarColor = currentsettings.colors[Settings.CI.textbox_sliderback];

                    if (currentsettings.buttonstyle.Equals(ButtonStyles[1])) // flat
                        ctrl.FlatStyle = FlatStyle.Flat;
                    else
                        ctrl.FlatStyle = FlatStyle.Popup;
                }

                myControl.Font = fnt;
                ctrl.Repaint();            // force a repaint as the individual settings do not by design.
            }
            else if (myControl is ComboBoxCustom)
            {
                ComboBoxCustom ctrl = (ComboBoxCustom)myControl;
                ctrl.ForeColor = currentsettings.colors[Settings.CI.button_text];

                if (currentsettings.buttonstyle.Equals(ButtonStyles[0])) // system
                {
                    ctrl.FlatStyle = FlatStyle.System;
                }
                else
                {
                    ctrl.BackColor = ctrl.DropDownBackgroundColor = currentsettings.colors[Settings.CI.button_back];
                    ctrl.BorderColor = currentsettings.colors[Settings.CI.button_border];
                    ctrl.MouseOverBackgroundColor = currentsettings.colors[Settings.CI.button_back].Multiply(mouseoverscaling);
                    ctrl.ScrollBarButtonColor = currentsettings.colors[Settings.CI.textbox_scrollbutton];
                    ctrl.ScrollBarColor = currentsettings.colors[Settings.CI.textbox_sliderback];

                    if (currentsettings.buttonstyle.Equals(ButtonStyles[1])) // flat
                        ctrl.FlatStyle = FlatStyle.Flat;
                    else
                        ctrl.FlatStyle = FlatStyle.Popup;
                }

                myControl.Font = fnt;
                ctrl.Repaint();            // force a repaint as the individual settings do not by design.
            }
            else if (myControl is NumericUpDown)
            {                                                                   // BACK colour does not work..
                myControl.ForeColor = currentsettings.colors[Settings.CI.textbox_fore];
                myControl.Font = fnt;
            }
            else if (myControl is DrawnPanelNoTheme)        // ignore these..
            {
            }
            else if (myControl is DrawnPanel)
            {
                DrawnPanel ctrl = (DrawnPanel)myControl;
                ctrl.BackColor = currentsettings.colors[Settings.CI.form];
                ctrl.ForeColor = currentsettings.colors[Settings.CI.label];
                ctrl.MouseOverColor = currentsettings.colors[Settings.CI.label].Multiply(mouseoverscaling);
                ctrl.MouseSelectedColor = currentsettings.colors[Settings.CI.label].Multiply(mouseselectedscaling);

                System.Drawing.Imaging.ColorMap colormap = new System.Drawing.Imaging.ColorMap();       // any drawn panel with drawn images    
                colormap.OldColor = Color.White;                                                        // white is defined as the forecolour
                colormap.NewColor = ctrl.ForeColor;
                ctrl.SetDrawnBitmapRemapTable(new System.Drawing.Imaging.ColorMap[] { colormap });
            }
            else if (myControl is Panel)
            {
                if (!(myControl.Name.Contains("defaultmapcolor")))                 // theme panels show settings color - don't overwrite
                {
                    myControl.BackColor = currentsettings.colors[Settings.CI.form];
                    myControl.ForeColor = currentsettings.colors[Settings.CI.label];
                }
            }
            else if (myControl is Label)
            {
                myControl.ForeColor = currentsettings.colors[Settings.CI.label];
                myControl.Font = fnt;
            }
            else if (myControl is GroupBoxCustom)
            {
                GroupBoxCustom ctrl = (GroupBoxCustom)myControl;
                ctrl.ForeColor = currentsettings.colors[Settings.CI.group_text];
                ctrl.BackColor = currentsettings.colors[Settings.CI.group_back];
                ctrl.BorderColor = currentsettings.colors[Settings.CI.group_borderlines];
                ctrl.FlatStyle = FlatStyle.Flat;           // always in Flat, always apply our border.
                ctrl.Font = fnt;
            }
            else if (myControl is CheckBoxCustom)
            {
                CheckBoxCustom ctrl = (CheckBoxCustom)myControl;

                if (ctrl.Appearance != Appearance.Button)          // NOT Button
                {
                    ctrl.BackColor = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.form]);
                    ctrl.ForeColor = currentsettings.colors[Settings.CI.checkbox];
                    ctrl.CheckBoxColor = currentsettings.colors[Settings.CI.checkbox];
                    ctrl.CheckBoxInnerColor = currentsettings.colors[Settings.CI.checkbox].Multiply(1.5F);
                    ctrl.MouseOverColor = currentsettings.colors[Settings.CI.checkbox].Multiply(1.4F);
                    ctrl.TickBoxReductionSize = (fnt.SizeInPoints > 10) ? 10 : 6;
                    ctrl.CheckColor = currentsettings.colors[Settings.CI.checkbox_tick];
                    ctrl.Font = fnt;

                    if (ctrl.Image == null)       // only for unimage ones
                    {
                        if (currentsettings.buttonstyle.Equals(ButtonStyles[0])) // system
                            ctrl.FlatStyle = FlatStyle.System;
                        else if (currentsettings.buttonstyle.Equals(ButtonStyles[1])) // flat
                            ctrl.FlatStyle = FlatStyle.Flat;
                        else
                            ctrl.FlatStyle = FlatStyle.Popup;
                    }
                    else
                    {
                        System.Drawing.Imaging.ColorMap colormap = new System.Drawing.Imaging.ColorMap();       // any drawn panel with drawn images    
                        colormap.OldColor = Color.White;                                                        // white is defined as the forecolour
                        colormap.NewColor = ctrl.ForeColor;
                        System.Drawing.Imaging.ColorMap colormap2 = new System.Drawing.Imaging.ColorMap();
                        colormap2.OldColor = Color.FromArgb(222,222,222);                                                  
                        colormap2.NewColor = ctrl.ForeColor.Multiply(0.85F);
                        ctrl.SetDrawnBitmapRemapTable(new System.Drawing.Imaging.ColorMap[] { colormap , colormap2  });
                        //System.Diagnostics.Debug.WriteLine("Recoloured" + ctrl.Name +" of " + ctrl.GetType().Name + " " + colormap.OldColor + ">" + colormap.NewColor);
                    }

                }
                else if ( ctrl.FlatStyle == FlatStyle.Flat )           // BUTTON and FLAT
                {
                    ctrl.BackColor = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.form]);
                    ctrl.FlatAppearance.CheckedBackColor = currentsettings.colors[Settings.CI.checkbox];
                    ctrl.FlatAppearance.MouseOverBackColor = currentsettings.colors[Settings.CI.checkbox].Multiply(1.4F);
                    ctrl.FlatAppearance.BorderColor = currentsettings.colors[Settings.CI.button_border];
                }
            }
            else if (myControl is RadioButtonCustom)
            {
                RadioButtonCustom ctrl = (RadioButtonCustom)myControl;

                ctrl.FlatStyle = FlatStyle.System;
                ctrl.Font = fnt;

                if (currentsettings.buttonstyle.Equals(ButtonStyles[0])) // system
                    ctrl.FlatStyle = FlatStyle.System;
                else if (currentsettings.buttonstyle.Equals(ButtonStyles[1])) // flat
                    ctrl.FlatStyle = FlatStyle.Flat;
                else
                    ctrl.FlatStyle = FlatStyle.Popup;

                //Console.WriteLine("RB:" + myControl.Name + " Apply style " + currentsettings.buttonstyle);

                ctrl.BackColor = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.form]);
                ctrl.ForeColor = currentsettings.colors[Settings.CI.checkbox];
                ctrl.RadioButtonColor = currentsettings.colors[Settings.CI.checkbox];
                ctrl.RadioButtonInnerColor = currentsettings.colors[Settings.CI.checkbox].Multiply(1.5F);
                ctrl.SelectedColor = ctrl.BackColor.Multiply(0.75F);
                ctrl.MouseOverColor = currentsettings.colors[Settings.CI.checkbox].Multiply(1.4F);
            }
            else if (myControl is DataGridView)                     // we theme this directly
            {
                DataGridView ctrl = (DataGridView)myControl;
                ctrl.EnableHeadersVisualStyles = false;            // without this, the colours for the grid are not applied.

                ctrl.RowHeadersDefaultCellStyle.BackColor = currentsettings.colors[Settings.CI.grid_borderback];
                ctrl.RowHeadersDefaultCellStyle.ForeColor = currentsettings.colors[Settings.CI.grid_bordertext];
                ctrl.ColumnHeadersDefaultCellStyle.BackColor = currentsettings.colors[Settings.CI.grid_borderback];
                ctrl.ColumnHeadersDefaultCellStyle.ForeColor = currentsettings.colors[Settings.CI.grid_bordertext];

                ctrl.BackgroundColor = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.form]);
                ctrl.DefaultCellStyle.BackColor = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.grid_cellbackground]);
                ctrl.DefaultCellStyle.ForeColor = currentsettings.colors[Settings.CI.grid_celltext];
                ctrl.DefaultCellStyle.SelectionBackColor = ctrl.DefaultCellStyle.ForeColor;
                ctrl.DefaultCellStyle.SelectionForeColor = ctrl.DefaultCellStyle.BackColor;

                ctrl.GridColor = currentsettings.colors[Settings.CI.grid_borderlines];
                ctrl.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                ctrl.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                ctrl.Font = fnt;           
                Font fnt2;
                
                if (myControl.Name.Contains("dataGridViewTravel") && fnt.Size > 10F)
                    fnt2 = new Font(currentsettings.fontname, 10F);
                else
                    fnt2 = fnt;

                ctrl.ColumnHeadersDefaultCellStyle.Font = fnt2;
                ctrl.RowHeadersDefaultCellStyle.Font = fnt2;
                ctrl.Columns[0].DefaultCellStyle.Font = fnt2;

                using (Graphics g = ctrl.CreateGraphics())
                {
                    SizeF sz = g.MeasureString("99999", fnt2);
                    ctrl.RowHeadersWidth = (int)(sz.Width + 6);        // size it to the text, need a little more for rounding
                }
            }
            else if (myControl is VScrollBarCustom && !(parent is ListControlCustom || parent is RichTextBoxScroll))
            {                   // selected items need VScroll controlled here. Others control it themselves
                VScrollBarCustom ctrl = (VScrollBarCustom)myControl;

                //System.Diagnostics.Debug.WriteLine("VScrollBarCustom Theme " + level + ":" + parent.Name.ToString() + ":" + myControl.Name.ToString() + " " + myControl.ToString() + " " + parentcontroltype.Name);
                if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[3]))
                {
                    Color c1 = currentsettings.colors[Settings.CI.grid_scrollbutton];
                    ctrl.BorderColor = currentsettings.colors[Settings.CI.grid_borderlines];
                    ctrl.BackColor = currentsettings.colors[Settings.CI.form];
                    ctrl.SliderColor = currentsettings.colors[Settings.CI.grid_sliderback];
                    ctrl.BorderColor = ctrl.ThumbBorderColor =
                            ctrl.ArrowBorderColor = currentsettings.colors[Settings.CI.grid_borderlines];
                    ctrl.ArrowButtonColor = ctrl.ThumbButtonColor = c1;
                    ctrl.MouseOverButtonColor = c1.Multiply(mouseoverscaling);
                    ctrl.MousePressedButtonColor = c1.Multiply(mouseselectedscaling);
                    ctrl.ForeColor = currentsettings.colors[Settings.CI.grid_scrollarrow];
                    ctrl.FlatStyle = FlatStyle.Popup;
                }
                else
                    ctrl.FlatStyle = FlatStyle.System;
            }
            else if (myControl is NumericUpDownCustom)
            {
                NumericUpDownCustom ctrl = (NumericUpDownCustom)myControl;

                ctrl.TextBoxForeColor = currentsettings.colors[Settings.CI.textbox_fore];
                ctrl.TextBoxBackColor = currentsettings.colors[Settings.CI.textbox_back];
                ctrl.BorderColor = currentsettings.colors[Settings.CI.textbox_border];

                Color c1 = currentsettings.colors[Settings.CI.textbox_scrollbutton];
                ctrl.updown.BackColor = c1;
                ctrl.updown.ForeColor = currentsettings.colors[Settings.CI.textbox_scrollarrow];
                ctrl.updown.MouseOverColor = c1.Multiply(mouseoverscaling);
                ctrl.updown.MouseSelectedColor = c1.Multiply(mouseselectedscaling);
                ctrl.Invalidate();
            }
            else if (myControl is Chart)
            {
                Chart ctrl = (Chart)myControl;
                ctrl.BackColor = Color.Transparent;
            }
            else if (myControl is CustomDateTimePicker)
            {
                CustomDateTimePicker ctrl = (CustomDateTimePicker)myControl;
                ctrl.BorderColor = currentsettings.colors[Settings.CI.grid_borderlines];
                ctrl.ForeColor = currentsettings.colors[Settings.CI.textbox_fore];
                ctrl.TextBackColor = currentsettings.colors[Settings.CI.textbox_back];
                ctrl.BackColor = currentsettings.colors[Settings.CI.form];
                ctrl.SelectedColor = currentsettings.colors[Settings.CI.textbox_fore].MultiplyBrightness(0.6F);

                if (currentsettings.buttonstyle.Equals(ButtonStyles[0])) // system
                    ctrl.checkbox.FlatStyle = FlatStyle.System;
                else
                    ctrl.checkbox.FlatStyle = FlatStyle.Popup;

                ctrl.checkbox.TickBoxReductionSize = 4;
                ctrl.checkbox.ForeColor = currentsettings.colors[Settings.CI.checkbox];
                ctrl.checkbox.CheckBoxColor = currentsettings.colors[Settings.CI.checkbox];
                Color inner = currentsettings.colors[Settings.CI.checkbox].Multiply(1.5F);
                if (inner.GetBrightness() < 0.1)        // double checking
                    inner = Color.Gray;
                ctrl.checkbox.CheckBoxInnerColor = inner;
                ctrl.checkbox.CheckColor = currentsettings.colors[Settings.CI.checkbox_tick];
                ctrl.checkbox.MouseOverColor = currentsettings.colors[Settings.CI.checkbox].Multiply(1.4F);
                return;     // don't do sub controls - we are in charge of them
            }
            else if (myControl is StatusStrip)
            {
                myControl.BackColor = currentsettings.colors[Settings.CI.form];
            }
            else if ( myControl is ToolStrip )
            {
                foreach( ToolStripItem i in ((ToolStrip)myControl).Items)   // make sure any buttons have the button back colour set
                {
                    if (i is ToolStripButton )
                    {           // theme the back colour, this is the way its done.. not via the tool strip renderer
                        i.BackColor = currentsettings.colors[Settings.CI.button_back];
                    }
                }
            }
            else
            {
                if (!parentcontroltype.Namespace.Equals("ExtendedControls"))
                {
                    //Console.WriteLine("THEME: Unhandled control " + controltype.Name + ":" + myControl.Name + " from " + parent.Name);
                }
            }

            foreach (Control subC in myControl.Controls)
            {
                UpdateColorControls(myControl,subC,fnt,level+1);
            }
        }


        private void UpdateToolsTripRenderer()
        {
            Color menuback = currentsettings.colors[Settings.CI.menu_back];
            bool toodark = (menuback.GetBrightness() < 0.1);

            toolstripRenderer.colortable.colMenuText = currentsettings.colors[Settings.CI.menu_fore];              // and the text
            toolstripRenderer.colortable.colMenuSelectedText = currentsettings.colors[Settings.CI.menu_dropdownfore];
            toolstripRenderer.colortable.colMenuBackground = menuback;
            toolstripRenderer.colortable.colMenuBarBackground = currentsettings.colors[Settings.CI.form];
            toolstripRenderer.colortable.colMenuBorder = currentsettings.colors[Settings.CI.button_border];
            toolstripRenderer.colortable.colMenuSelectedBack = currentsettings.colors[Settings.CI.menu_dropdownback];

            toolstripRenderer.colortable.colMenuHighlightBorder = currentsettings.colors[Settings.CI.button_border];
            toolstripRenderer.colortable.colMenuHighlightBack = toodark ? currentsettings.colors[Settings.CI.menu_dropdownback].Multiply(0.7F) : currentsettings.colors[Settings.CI.menu_back].Multiply(1.3F);        // whole menu back

            Color menuchecked = toodark ? currentsettings.colors[Settings.CI.menu_dropdownback].Multiply(0.6F) : currentsettings.colors[Settings.CI.menu_back].Multiply(1.5F);        // whole menu back

            toolstripRenderer.colortable.colToolStripButtonCheckedBack = menuchecked;
            toolstripRenderer.colortable.colToolStripButtonPressedBack = 
            toolstripRenderer.colortable.colToolStripButtonSelectedBack = menuchecked.Multiply(1.1F);

            toolstripRenderer.colortable.colCheckButtonChecked = menuchecked;
            toolstripRenderer.colortable.colCheckButtonPressed = 
            toolstripRenderer.colortable.colCheckButtonHighlighted = menuchecked.Multiply(1.1F);

            toolstripRenderer.colortable.colLeftSelectionMargin = currentsettings.colors[Settings.CI.menu_back].Multiply(0.9F);

            toolstripRenderer.colortable.colToolStripBackground = toodark ? currentsettings.colors[Settings.CI.menu_fore].Multiply(0.2F) : menuback;

            toolstripRenderer.colortable.colGripper =
            toolstripRenderer.colortable.colToolStripSeparator =
            toolstripRenderer.colortable.colToolStripBorder = currentsettings.colors[Settings.CI.button_border];
        }

        public Color GroupBoxOverride(Control parent, Color d)      // if its a group box behind the control, use the group box back color..
        {
            return (parent is GroupBox) ? currentsettings.colors[Settings.CI.group_back] : d;
        }

        public void UpdatePatch( Panel pn )
        {
            Settings.CI ci = (Settings.CI)(pn.Tag);
            pn.BackColor = currentsettings.colors[ci];
        }

        public bool EditColor(Settings.CI ex)                      
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.FullOpen = true;
            MyDialog.Color = currentsettings.colors[ex];

            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                currentsettings.colors[ex] = MyDialog.Color;
                SetCustom();
                return true;
            }
            else
                return false;
        }

    }
}
