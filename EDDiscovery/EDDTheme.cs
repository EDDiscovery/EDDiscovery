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

namespace EDDiscovery2
{
    public class EDDTheme
    {
        public static readonly string[] ButtonStyles = "System Flat Gradient".Split();
        public static readonly string[] TextboxBorderStyles = "None FixedSingle Fixed3D Colour".Split();

        private static string buttonstyle_system = ButtonStyles[0];
        private static string buttonstyle_gradient = ButtonStyles[2];
        private static string textboxborderstyle_fixed3D = TextboxBorderStyles[2];
        private static string textboxborderstyle_color = TextboxBorderStyles[3];


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
                toolstrip_back, toolstrip_border, toolstrip_buttonchecked
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
                                        Color ttb, Color ttborder, Color ttbuttonchecked,
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

        public Color TextBackColor { get { return currentsettings.colors[Settings.CI.textbox_back]; } set { SetCustom(); currentsettings.colors[Settings.CI.textbox_back] = value; } }
        public Color TextBlockColor { get { return currentsettings.colors[Settings.CI.textbox_fore]; } set { SetCustom(); currentsettings.colors[Settings.CI.textbox_fore] = value; } }
        public Color TextBlockHighlightColor { get { return currentsettings.colors[Settings.CI.textbox_highlight]; } set { SetCustom(); currentsettings.colors[Settings.CI.textbox_highlight] = value; } }
        public Color TextBlockSuccessColor { get { return currentsettings.colors[Settings.CI.textbox_success]; } set { SetCustom(); currentsettings.colors[Settings.CI.textbox_success] = value; } }
        public string TextBlockBorderStyle { get { return currentsettings.textboxborderstyle; } set { SetCustom(); currentsettings.textboxborderstyle = value; } }
        public Color VisitedSystemColor { get { return currentsettings.colors[Settings.CI.travelgrid_visited]; } set { SetCustom(); currentsettings.colors[Settings.CI.travelgrid_visited] = value; } }
        public Color NonVisitedSystemColor { get { return currentsettings.colors[Settings.CI.travelgrid_nonvisted]; } set { SetCustom(); currentsettings.colors[Settings.CI.travelgrid_nonvisted] = value; } }

        public string ButtonStyle { get { return currentsettings.buttonstyle; } set { SetCustom(); currentsettings.buttonstyle= value; } }

        public bool WindowsFrame { get { return currentsettings.windowsframe; } set { SetCustom(); currentsettings.windowsframe = value; } }
        public double Opacity { get { return currentsettings.formopacity; } set { SetCustom(); currentsettings.formopacity = value; } }
        public string FontName { get { return currentsettings.fontname; } set { SetCustom(); currentsettings.fontname = value; } }
        public float FontSize { get { return currentsettings.fontsize; } set { SetCustom(); currentsettings.fontsize = value; } }

        private Settings currentsettings;           // if name = custom, then its not a standard theme..
        private List<Settings> themelist;
        
        public EDDToolStripRenderer toolstripRenderer;

        public EDDTheme()
        {
            toolstripRenderer = new EDDToolStripRenderer();
            themelist = new List<Settings>();           // theme list in
            currentsettings = new Settings("Windows Default");  // this is our default
        }


        public void RestoreSettings()
        {

            Console.WriteLine("Theme ID " + Settings.ThemeID);

            int themeidstored = SQLiteDBClass.GetSettingInt("ThemeID", -1);

            if ( themeidstored != -1 && themeidstored != Settings.ThemeID )
            {
                DialogResult res = MessageBox.Show("The theme stored has missing colors or other missing information" + Environment.NewLine +
                      "that this new version of EDDiscovery needs." + Environment.NewLine + Environment.NewLine +
                      "Choose OK to use the stored theme, and then correct the missing colors or other information manually using the Theme Editor in Settings" + Environment.NewLine + Environment.NewLine +
                      "Choose Cancel to go back to windows default, then pick a new standard theme.", "ED Discovery Theme Warning!" , MessageBoxButtons.OKCancel);

                if (res == DialogResult.Cancel)     // if cancel, we abort,
                    return;
            }

            if (SQLiteDBClass.keyExists("ThemeNameOf"))           // (keep previous check) if there.. get the others with a good default in case the db is screwed.
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
                Color.Black, Color.DarkOrange, Color.Orange, // tooltip
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
                Color.Black, Color.DarkOrange, Color.Orange, // tooltip
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
                Color.Black, r2, r1, // tooltip
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
                                        SystemColors.Window, SystemColors.Menu, SystemColors.MenuText, //tooltip
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
                                               Color.DarkBlue, Color.White, Color.Red,
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
                                               baizegreen,
                                               false, 95, "Microsoft Sans Serif", 8.25F));

            string themepath = "";

            try
            {
                themepath = Path.Combine(Tools.GetAppDataDirectory(), "Theme");
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
                themepath = Path.Combine(Tools.GetAppDataDirectory(), "Theme") ;
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

        public void ApplyColors(Form form)
        {
            form.Opacity = currentsettings.formopacity / 100;
            form.BackColor = currentsettings.colors[Settings.CI.form];

            if (currentsettings.fontname.Equals("") || currentsettings.fontsize < minfontsize)
            {
                currentsettings.fontname = "Microsoft Sans Serif";          // in case schemes were loaded
                currentsettings.fontsize = 8.25F;
            }

            Font fnt = new Font(currentsettings.fontname, currentsettings.fontsize);

            foreach (Control c in form.Controls)
                UpdateColorControls(form,c, fnt, 0);
            
            UpdateToolsTripRenderer();
        }

        private void UpdateToolsTripRenderer()
        {
            // Menu
            toolstripRenderer.colMenuBackground = currentsettings.colors[Settings.CI.menu_back];
            toolstripRenderer.colMenuText = currentsettings.colors[Settings.CI.menu_fore];
            toolstripRenderer.colMenuSelectedBack = currentsettings.colors[Settings.CI.menu_dropdownback];
            toolstripRenderer.colMenuSelectedText = currentsettings.colors[Settings.CI.menu_dropdownfore];


            toolstripRenderer.Dark = Color.Pink; // currentsettings.colors[Settings.CI.menu_dropdownback];
            //Bitmap bmp = new Bitmap(1, 1);
            toolstripRenderer.ButtonSelectedBorder = currentsettings.colors[Settings.CI.textbox_success]; ;
            toolstripRenderer.ButtonSelectBackLight = currentsettings.colors[Settings.CI.button_text];

            // Need a button/Menu highlight
            toolstripRenderer.ButtonSelectBackDark = currentsettings.colors[Settings.CI.menu_dropdownback];

            // ToolStrip
            toolstripRenderer.colToolStripBorder = currentsettings.colors[Settings.CI.textbox_border];  // change to tool_Strip border
            toolstripRenderer.colToolStripBackGround = currentsettings.colors[Settings.CI.toolstrip_back];
            toolstripRenderer.colToolStripSeparator = currentsettings.colors[Settings.CI.textbox_border];  // change to tool_Strip border



        }



        public void UpdateColorControls(Control parent , Control myControl, Font fnt, int level)
        {
#if DEBUG
            //string pad = "                             ".Substring(0, level);
            //Console.WriteLine(pad + parent.Name.ToString() + ":" + myControl.Name.ToString() + " " + myControl.ToString());
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
            else if (myControl is MenuStrip || myControl is ToolStrip)
            {
                myControl.BackColor = currentsettings.colors[Settings.CI.menu_back];
                myControl.ForeColor = currentsettings.colors[Settings.CI.menu_fore];
                myControl.Font = fnt;
            }
            else if (myControl is RichTextBoxScroll)
            {
                RichTextBoxScroll MyDgv = (RichTextBoxScroll)myControl;
                MyDgv.BorderColor = Color.Transparent;
                MyDgv.BorderStyle = BorderStyle.None;

                MyDgv.TextBox.ForeColor = currentsettings.colors[Settings.CI.textbox_fore];
                MyDgv.TextBox.BackColor = currentsettings.colors[Settings.CI.textbox_back];

                MyDgv.ScrollBar.FlatStyle = FlatStyle.System;

                if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[1]))
                    MyDgv.BorderStyle = BorderStyle.FixedSingle;
                else if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[2]))
                    MyDgv.BorderStyle = BorderStyle.Fixed3D;
                else if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[3]))
                {
                    Color c1 = currentsettings.colors[Settings.CI.textbox_scrollbutton];
                    MyDgv.BorderColor = currentsettings.colors[Settings.CI.textbox_border];
                    MyDgv.ScrollBar.BackColor = currentsettings.colors[Settings.CI.textbox_back];
                    MyDgv.ScrollBar.SliderColor = currentsettings.colors[Settings.CI.textbox_sliderback];
                    MyDgv.ScrollBar.BorderColor = MyDgv.ScrollBar.ThumbBorderColor = 
                                MyDgv.ScrollBar.ArrowBorderColor = currentsettings.colors[Settings.CI.textbox_border];
                    MyDgv.ScrollBar.ArrowButtonColor = MyDgv.ScrollBar.ThumbButtonColor = c1;
                    MyDgv.ScrollBar.MouseOverButtonColor = ButtonExt.Multiply(c1, mouseoverscaling);
                    MyDgv.ScrollBar.MousePressedButtonColor = ButtonExt.Multiply(c1, mouseselectedscaling);
                    MyDgv.ScrollBar.ForeColor = currentsettings.colors[Settings.CI.textbox_scrollarrow];
                    MyDgv.ScrollBar.FlatStyle = FlatStyle.Popup;
                }

                if (myControl.Font.Name.Contains("Courier"))                  // okay if we ordered a fixed font, don't override
                {
                    Font fntf = new Font(myControl.Font.Name, currentsettings.fontsize); // make one of the selected size
                    myControl.Font = fntf;
                }
                else
                    myControl.Font = fnt;
            }
            else if (myControl is TextBoxBorder)
            {
                TextBoxBorder MyDgv = (TextBoxBorder)myControl;
                myControl.ForeColor = currentsettings.colors[Settings.CI.textbox_fore];
                myControl.BackColor = currentsettings.colors[Settings.CI.textbox_back];
                MyDgv.BorderColor = Color.Transparent;
                MyDgv.BorderStyle = BorderStyle.None;
                MyDgv.AutoSize = true;

                if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[0]))
                    MyDgv.AutoSize = false;                                                 // with no border, the autosize clips the bottom of chars..
                else if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[1]))
                    MyDgv.BorderStyle = BorderStyle.FixedSingle;
                else if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[2]))
                    MyDgv.BorderStyle = BorderStyle.Fixed3D;
                else if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[3]))
                    MyDgv.BorderColor = currentsettings.colors[Settings.CI.textbox_border];

                myControl.Font = fnt;
            }
            else if (myControl is ButtonExt)
            {
                ButtonExt MyDgv = (ButtonExt)myControl;
                MyDgv.ForeColor = currentsettings.colors[Settings.CI.button_text];

                if (currentsettings.buttonstyle.Equals(ButtonStyles[0])) // system
                {
                    MyDgv.FlatStyle = FlatStyle.System;
                    MyDgv.UseVisualStyleBackColor = true;           // this makes it system..
                }
                else
                {
                    MyDgv.BackColor = currentsettings.colors[Settings.CI.button_back];
                    MyDgv.FlatAppearance.BorderColor = currentsettings.colors[Settings.CI.button_border];
                    MyDgv.FlatAppearance.BorderSize = 1;
                    MyDgv.FlatAppearance.MouseOverBackColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.button_back], mouseoverscaling);
                    MyDgv.FlatAppearance.MouseDownBackColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.button_back], mouseselectedscaling);

                    if (currentsettings.buttonstyle.Equals(ButtonStyles[1])) // flat
                        MyDgv.FlatStyle = FlatStyle.Flat;
                    else
                        MyDgv.FlatStyle = FlatStyle.Popup;
                }

                myControl.Font = fnt;
            }
            else if (myControl is TabControlCustom)
            {
                TabControlCustom MyDgv = (TabControlCustom)myControl;

                if (!currentsettings.buttonstyle.Equals(ButtonStyles[0])) // not system
                {
                    MyDgv.FlatStyle = (currentsettings.buttonstyle.Equals(ButtonStyles[1])) ? FlatStyle.Flat : FlatStyle.Popup;
                    MyDgv.TabControlBorderColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.tabcontrol_borderlines], 0.6F);
                    MyDgv.TabControlBorderBrightColor = currentsettings.colors[Settings.CI.tabcontrol_borderlines];
                    MyDgv.TabNotSelectedBorderColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.tabcontrol_borderlines], 0.4F);
                    MyDgv.TabNotSelectedColor = currentsettings.colors[Settings.CI.button_back];
                    MyDgv.TabSelectedColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.button_back], mouseselectedscaling);
                    MyDgv.TabMouseOverColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.button_back], mouseoverscaling);
                    MyDgv.TextSelectedColor = currentsettings.colors[Settings.CI.button_text];
                    MyDgv.TextNotSelectedColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.button_text], 0.8F);
                    MyDgv.TabStyle = new ExtendedControls.TabStyleAngled();
                }
                else
                    MyDgv.FlatStyle = FlatStyle.System;

                MyDgv.Font = fnt;
            }
            else if (myControl is ComboBoxCustom)
            {
                ComboBoxCustom MyDgv = (ComboBoxCustom)myControl;
                MyDgv.ForeColor = currentsettings.colors[Settings.CI.button_text];

                if (currentsettings.buttonstyle.Equals(ButtonStyles[0])) // system
                {
                    MyDgv.FlatStyle = FlatStyle.System;
                }
                else
                {
                    MyDgv.BackColor = MyDgv.DropDownBackgroundColor = currentsettings.colors[Settings.CI.button_back];
                    MyDgv.BorderColor = currentsettings.colors[Settings.CI.button_border];
                    MyDgv.MouseOverBackgroundColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.button_back], mouseoverscaling);
                    MyDgv.ScrollBarButtonColor = currentsettings.colors[Settings.CI.textbox_scrollbutton];
                    MyDgv.ScrollBarColor = currentsettings.colors[Settings.CI.textbox_sliderback];

                    if (currentsettings.buttonstyle.Equals(ButtonStyles[1])) // flat
                        MyDgv.FlatStyle = FlatStyle.Flat;
                    else
                        MyDgv.FlatStyle = FlatStyle.Popup;
                }

                myControl.Font = fnt;
                MyDgv.Repaint();            // force a repaint as the individual settings do not by design.
            }
            else if (myControl is NumericUpDown)
            {                                                                   // BACK colour does not work..
                myControl.ForeColor = currentsettings.colors[Settings.CI.textbox_fore];
                myControl.Font = fnt;
            }
            else if (myControl is DrawnPanel)
            {
                DrawnPanel MyDgv = (DrawnPanel)myControl;
                MyDgv.BackColor = currentsettings.colors[Settings.CI.form];
                MyDgv.ForeColor = currentsettings.colors[Settings.CI.label];
                MyDgv.MouseOverColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.label], mouseoverscaling);
                MyDgv.MouseSelectedColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.label], mouseselectedscaling);
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
                GroupBoxCustom MyDgv = (GroupBoxCustom)myControl;
                MyDgv.ForeColor = currentsettings.colors[Settings.CI.group_text];
                MyDgv.BackColor = currentsettings.colors[Settings.CI.group_back];
                MyDgv.BorderColor = currentsettings.colors[Settings.CI.group_borderlines];
                MyDgv.FlatStyle = FlatStyle.Flat;           // always in Flat, always apply our border.
                MyDgv.Font = fnt;
            }
            else if (myControl is CheckBoxCustom)
            {
                CheckBoxCustom MyDgv = (CheckBoxCustom)myControl;

                if (currentsettings.buttonstyle.Equals(ButtonStyles[0])) // system
                    MyDgv.FlatStyle = FlatStyle.System;
                else if (currentsettings.buttonstyle.Equals(ButtonStyles[1])) // flat
                    MyDgv.FlatStyle = FlatStyle.Flat;
                else
                    MyDgv.FlatStyle = FlatStyle.Popup;

                MyDgv.BackColor = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.form]);
                MyDgv.ForeColor = currentsettings.colors[Settings.CI.checkbox];
                MyDgv.CheckBoxColor = currentsettings.colors[Settings.CI.checkbox];
                MyDgv.CheckBoxInnerColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.checkbox], 1.5F);
                MyDgv.CheckColor = currentsettings.colors[Settings.CI.checkbox_tick];
                MyDgv.MouseOverColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.checkbox], 1.4F);
                MyDgv.TickBoxReductionSize = (fnt.SizeInPoints > 10) ? 10 : 6;
                MyDgv.Font = fnt;
            }
            else if (myControl is RadioButtonCustom)
            {
                RadioButtonCustom MyDgv = (RadioButtonCustom)myControl;

                MyDgv.FlatStyle = FlatStyle.System;
                MyDgv.Font = fnt;

                if (currentsettings.buttonstyle.Equals(ButtonStyles[0])) // system
                    MyDgv.FlatStyle = FlatStyle.System;
                else if (currentsettings.buttonstyle.Equals(ButtonStyles[1])) // flat
                    MyDgv.FlatStyle = FlatStyle.Flat;
                else
                    MyDgv.FlatStyle = FlatStyle.Popup;

                //Console.WriteLine("RB:" + myControl.Name + " Apply style " + currentsettings.buttonstyle);

                MyDgv.BackColor = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.form]);
                MyDgv.ForeColor = currentsettings.colors[Settings.CI.checkbox];
                MyDgv.RadioButtonColor = currentsettings.colors[Settings.CI.checkbox];
                MyDgv.RadioButtonInnerColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.checkbox], 1.5F);
                MyDgv.SelectedColor = ButtonExt.Multiply(MyDgv.BackColor, 0.75F);
                MyDgv.MouseOverColor = ButtonExt.Multiply(currentsettings.colors[Settings.CI.checkbox], 1.4F);
            }
            else if (myControl is DataGridView)                     // we theme this directly
            {
                DataGridView MyDgv = (DataGridView)myControl;
                MyDgv.EnableHeadersVisualStyles = false;            // without this, the colours for the grid are not applied.

                MyDgv.RowHeadersDefaultCellStyle.BackColor = currentsettings.colors[Settings.CI.grid_borderback];
                MyDgv.RowHeadersDefaultCellStyle.ForeColor = currentsettings.colors[Settings.CI.grid_bordertext];
                MyDgv.ColumnHeadersDefaultCellStyle.BackColor = currentsettings.colors[Settings.CI.grid_borderback];
                MyDgv.ColumnHeadersDefaultCellStyle.ForeColor = currentsettings.colors[Settings.CI.grid_bordertext];

                MyDgv.BackgroundColor = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.form]);
                MyDgv.DefaultCellStyle.BackColor = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.grid_cellbackground]);
                MyDgv.DefaultCellStyle.ForeColor = currentsettings.colors[Settings.CI.grid_celltext];
                MyDgv.DefaultCellStyle.SelectionBackColor = MyDgv.DefaultCellStyle.ForeColor;
                MyDgv.DefaultCellStyle.SelectionForeColor = MyDgv.DefaultCellStyle.BackColor;

                MyDgv.GridColor = currentsettings.colors[Settings.CI.grid_borderlines];
                MyDgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                MyDgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                MyDgv.Font = fnt;           
                Font fnt2;
                
                if (myControl.Name.Contains("dataGridViewTravel") && fnt.Size > 10F)
                    fnt2 = new Font(currentsettings.fontname, 10F);
                else
                    fnt2 = fnt;

                MyDgv.ColumnHeadersDefaultCellStyle.Font = fnt2;
                MyDgv.RowHeadersDefaultCellStyle.Font = fnt2;
                MyDgv.Columns[0].DefaultCellStyle.Font = fnt2;

                using (Graphics g = MyDgv.CreateGraphics())
                {
                    SizeF sz = g.MeasureString("99999", fnt2);
                    MyDgv.RowHeadersWidth = (int)(sz.Width + 6);        // size it to the text, need a little more for rounding
                }
            }
            else if (myControl is VScrollBarCustom && parent is DataViewScrollerPanel)
            {                   // a VScrollbarCustom inside a dataview scroller panel themed as a scroller panel
                VScrollBarCustom MyDgv = (VScrollBarCustom)myControl;

                if (currentsettings.textboxborderstyle.Equals(TextboxBorderStyles[3]))
                {
                    Color c1 = currentsettings.colors[Settings.CI.grid_scrollbutton];
                    MyDgv.BorderColor = currentsettings.colors[Settings.CI.grid_borderlines];
                    MyDgv.BackColor = currentsettings.colors[Settings.CI.form];
                    MyDgv.SliderColor = currentsettings.colors[Settings.CI.grid_sliderback];
                    MyDgv.BorderColor = MyDgv.ThumbBorderColor =
                            MyDgv.ArrowBorderColor = currentsettings.colors[Settings.CI.grid_borderlines];
                    MyDgv.ArrowButtonColor = MyDgv.ThumbButtonColor = c1;
                    MyDgv.MouseOverButtonColor = ButtonExt.Multiply(c1, mouseoverscaling);
                    MyDgv.MousePressedButtonColor = ButtonExt.Multiply(c1, mouseselectedscaling);
                    MyDgv.ForeColor = currentsettings.colors[Settings.CI.grid_scrollarrow];
                    MyDgv.FlatStyle = FlatStyle.Popup;
                }
                else
                    MyDgv.FlatStyle = FlatStyle.System;
            }
            else if ( myControl is NumericUpDownCustom )
            {
                NumericUpDownCustom MyDgv = (NumericUpDownCustom)myControl;

                MyDgv.TextBoxForeColor = currentsettings.colors[Settings.CI.textbox_fore];
                MyDgv.TextBoxBackColor = currentsettings.colors[Settings.CI.textbox_back];
                MyDgv.BorderColor = currentsettings.colors[Settings.CI.textbox_border];

                Color c1 = currentsettings.colors[Settings.CI.textbox_scrollbutton];
                MyDgv.updown.BackColor = c1;
                MyDgv.updown.ForeColor = currentsettings.colors[Settings.CI.textbox_scrollarrow];
                MyDgv.updown.MouseOverColor = ButtonExt.Multiply(c1, mouseoverscaling);
                MyDgv.updown.MouseSelectedColor = ButtonExt.Multiply(c1, mouseselectedscaling);
                MyDgv.Invalidate();
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
