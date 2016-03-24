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

// TODO:
// 1. ComboBoxes need owner draw
// 2. grid borders - why can't I change their colors even though their are members for it - is it being overriden during construction?
// 3. Fonts - enable them on visual elements.

namespace EDDiscovery2
{
    public class EDDTheme
    {
        public struct Settings
        {
            public enum CI
            {
                form,
                button_back,
                grid_border,
                grid_background,
                textbox_back,
                menu_back,
                group_back,
                button_text,                            // >= means its a text default
                grid_bordertext,
                grid_text,
                travelgrid_nonvisted, travelgrid_visited,
                textbox_fore, textbox_highlight,
                checkbox,
                menu_fore,
                label,
                group_text
            };

            public string name;         // name of scheme
            public Dictionary<CI, Color> colors;       // dictionary of colors, indexed by CI.
            public bool windowsframe;
            public double formopacity;
            public string fontname;         // Font.. (empty means don't override)
            public float fontsize;

            public Settings(String n, Color f,
                                        Color bb, Color bf,
                                        Color gb, Color gbt, Color gbck, Color gt,
                                        Color tn, Color tv, 
                                        Color tbb, Color tbf, Color tbh,
                                        Color c,
                                        Color mb, Color mf,
                                        Color l,
                                        Color grpb, Color grpt,
                                        bool wf, double op, string ft, float fs)            // ft = empty means don't set it
            {
                name = n;
                colors = new Dictionary<CI, Color>();
                colors.Add(CI.form, f);
                colors.Add(CI.button_back, bb); colors.Add(CI.button_text, bf);
                colors.Add(CI.grid_border, gb); colors.Add(CI.grid_bordertext, gbt); colors.Add(CI.grid_background, gbck); colors.Add(CI.grid_text, gt);
                colors.Add(CI.travelgrid_nonvisted, tn); colors.Add(CI.travelgrid_visited, tv);
                colors.Add(CI.textbox_back, tbb); colors.Add(CI.textbox_fore, tbf); colors.Add(CI.textbox_highlight, tbh);
                colors.Add(CI.checkbox, c);
                colors.Add(CI.menu_back, mb); colors.Add(CI.menu_fore, mf);
                colors.Add(CI.label, l);
                colors.Add(CI.group_back, grpb);
                colors.Add(CI.group_text, grpt);

                windowsframe = wf; formopacity = op; fontname = ft; fontsize = fs;
            }

            public Settings(JObject jo, string settingsname)            // From json
            {
                name = settingsname.Replace(".eddtheme", "");
                colors = new Dictionary<CI, Color>();

                foreach (CI ck in Enum.GetValues(typeof(CI)))           // all enums
                {
                    Color d = (ck < Settings.CI.button_text) ? SystemColors.Menu : SystemColors.MenuText;       // pick a good default
                    colors.Add(ck, JGetColor(jo, ck.ToString(),d));
                }

                windowsframe = GetBool(jo["windowsframe"]);
                formopacity = GetFloat(jo["formopacity"]);
                fontname = GetString(jo["fontname"]);
                if (fontname == "")
                    fontname = "Microsoft Sans Serif";
                fontsize = GetFloat(jo["fontsize"]);
                if (fontsize < minfontsize )
                    fontsize = 8.25F;
            }

            static private Color JGetColor(JObject jo, string name , Color defc)
            {
                string colstr = GetString(jo[name]);

                if (colstr == null)
                    return defc;

                return System.Drawing.ColorTranslator.FromHtml(colstr);
            }


            static private bool GetBool(JToken jToken)
            {
                if (IsNullOrEmptyT(jToken))
                    return false;
                return jToken.Value<bool>();
            }

            static private float GetFloat(JToken jToken)
            {
                if (IsNullOrEmptyT(jToken))
                    return 0f;
                return jToken.Value<float>();
            }


            static private int GetInt(JToken jToken)
            {
                if (IsNullOrEmptyT(jToken))
                    return 0;
                return jToken.Value<int>();
            }


            static private string GetString(JToken jToken)
            {
                if (IsNullOrEmptyT(jToken))
                    return null;
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


            public Settings(Settings other)                // copy constructor, takes a real copy.
            {
                name = other.name;
                windowsframe = other.windowsframe; formopacity = other.formopacity; fontname = other.fontname; fontsize = other.fontsize;
                colors = new Dictionary<CI, Color>();
                foreach (CI ck in other.colors.Keys)
                {
                    colors.Add(ck, other.colors[ck]);
                }
            }
        }

        public static float minfontsize = 4;

        public string Name { get { return currentsettings.name; } set { currentsettings.name = value; } }

        public Color TextBlock { get { return currentsettings.colors[Settings.CI.textbox_fore]; } set { SetCustom(); currentsettings.colors[Settings.CI.textbox_fore] = value; } }
        public Color TextBlockHighlightColor { get { return currentsettings.colors[Settings.CI.textbox_highlight]; } set { SetCustom(); currentsettings.colors[Settings.CI.textbox_highlight] = value; } }
        public Color VisitedSystemColor { get { return currentsettings.colors[Settings.CI.travelgrid_visited]; } set { SetCustom(); currentsettings.colors[Settings.CI.travelgrid_visited] = value; } }
        public Color NonVisitedSystemColor { get { return currentsettings.colors[Settings.CI.travelgrid_nonvisted]; } set { SetCustom(); currentsettings.colors[Settings.CI.travelgrid_nonvisted] = value; } }

        public bool WindowsFrame { get { return currentsettings.windowsframe; } set { SetCustom(); currentsettings.windowsframe = value; } }
        public double Opacity { get { return currentsettings.formopacity; } set { SetCustom(); currentsettings.formopacity = value; } }
        public string FontName { get { return currentsettings.fontname; } set { SetCustom(); currentsettings.fontname = value; } }
        public float FontSize { get { return currentsettings.fontsize; } set { SetCustom(); currentsettings.fontsize = value; } }

        private Settings currentsettings;           // if name = custom, then its not a standard theme..
        private List<Settings> themelist;
        private SQLiteDBClass db;

        public EDDTheme()
        {
            themelist = new List<Settings>();               // new one every time
            LoadThemes();
            currentsettings = new Settings(themelist[0]);       // copy it, not reference it.
        }

        public void RestoreSettings()
        {
            if (db == null)
                db = new SQLiteDBClass();

            if (db.keyExists( "ThemeNameOf"))           // if there.. get the others with a good default in case the db is screwed.
            {
                currentsettings.name = db.GetSettingString("ThemeNameOf", "Custom");
                currentsettings.windowsframe = db.GetSettingBool("ThemeWindowsFrame", true);
                currentsettings.formopacity = db.GetSettingDouble("ThemeFormOpacity", 100);
                currentsettings.fontname = db.GetSettingString("ThemeFont", "");
                currentsettings.fontsize = (float)db.GetSettingDouble("ThemeFontSize", 8);

                foreach (Settings.CI ck in themelist[0].colors.Keys)         // use themelist to find the key names, as we modify currentsettings as we go and that would cause an exception
                {
                    int d = (ck < Settings.CI.button_text) ? SystemColors.Menu.ToArgb() : SystemColors.MenuText.ToArgb();       // pick a good default
                    Color c = Color.FromArgb(db.GetSettingInt("ThemeColor" + ck.ToString(), d));
                    currentsettings.colors[ck] = c;
                }
            }
        }

        public void SaveSettings(string filename)
        {
            if (db == null)
                db = new SQLiteDBClass();

            db.PutSettingString("ThemeNameOf", currentsettings.name);
            db.PutSettingBool("ThemeWindowsFrame", currentsettings.windowsframe);
            db.PutSettingDouble("ThemeFormOpacity", currentsettings.formopacity);
            db.PutSettingString("ThemeFont", currentsettings.fontname);
            db.PutSettingDouble("ThemeFontSize", currentsettings.fontsize);

            foreach (Settings.CI ck in currentsettings.colors.Keys)
            {
                db.PutSettingInt("ThemeColor" + ck.ToString(), currentsettings.colors[ck].ToArgb());
            }

            if ( filename != null )         
                SaveFile(filename);
        }


        public void LoadThemes()
        {
            themelist.Clear();

            themelist.Add(new Settings("Windows Default", SystemColors.Menu,
                                                           SystemColors.ControlLightLight, SystemColors.MenuText,  // button
                                                           SystemColors.Menu, SystemColors.MenuText,  // grid border
                                                           SystemColors.ControlLightLight, SystemColors.MenuText,  // grid
                                                           Color.Blue, SystemColors.MenuText, // travel
                                                           SystemColors.Menu, SystemColors.MenuText, Color.Red,  // text
                                                           SystemColors.MenuText, // checkbox
                                                           SystemColors.Menu, SystemColors.MenuText,  // menu
                                                           SystemColors.MenuText,  // label
                                                           SystemColors.Menu, SystemColors.MenuText,  // group
                                                           true, 100, "Microsoft Sans Serif", 8.25F));

            themelist.Add(new Settings("Orange Delight", Color.Black,
                                               Color.Black, Color.Orange,  // button
                                               Color.FromArgb(255, 176, 115, 0), Color.Black,  // grid border
                                               Color.Black, Color.Orange, // grid
                                               Color.Orange, Color.White, // travel
                                               Color.Black, Color.Orange, Color.Red,  // text box
                                               Color.Orange, // checkbox
                                               Color.Black, Color.Orange,  // menu
                                               Color.Orange,  // label
                                               Color.FromArgb(255, 32,32,32), Color.Orange, // group
                                               false, 95, "Microsoft Sans Serif", 8.25F));

            themelist.Add(new Settings("Elite EuroCaps", Color.Black,
                                               Color.Black, Color.Orange,  // button
                                               Color.FromArgb(255, 176, 115, 0), Color.Black,  // grid border
                                               Color.Black, Color.Orange, // grid
                                               Color.Orange, Color.White, // travel
                                               Color.Black, Color.Orange, Color.Red,  // text box
                                               Color.Orange, // checkbox
                                               Color.Black, Color.Orange,  // menu
                                               Color.Orange,  // label
                                               Color.FromArgb(255, 32, 32, 32), Color.Orange, // group
                                               false, 95, "Euro Caps", 12F));

            themelist.Add(new Settings("Blue Wonder", Color.DarkBlue,
                                               Color.Blue, Color.White,  // button
                                               Color.DarkBlue, Color.White,  // grid border
                                               Color.DarkBlue, Color.White, // grid
                                               Color.White, Color.DarkBlue, // travel
                                               Color.DarkBlue, Color.White, Color.Red,  // text box
                                               Color.White, // checkbox
                                               Color.DarkBlue, Color.White,  // menu
                                               Color.White,  // label
                                               Color.DarkBlue, Color.White,  // group
                                               false, 95, "Microsoft Sans Serif", 8.25F));

            themelist.Add(new Settings("Green Baize", Color.FromArgb(255, 48, 121, 17),
                                               Color.FromArgb(255, 48, 121, 17), Color.White,  // button
                                               Color.FromArgb(255, 48, 121, 17), Color.White,  // grid border
                                               Color.FromArgb(255, 48, 121, 17), Color.White, // grid
                                               Color.White, Color.FromArgb(255, 48, 121, 17), // travel
                                               Color.FromArgb(255, 48, 121, 17), Color.White, Color.Red,  // text box
                                               Color.White, // checkbox
                                               Color.FromArgb(255, 48, 121, 17), Color.White,  // menu
                                               Color.White,  // label
                                               Color.FromArgb(255, 48, 121, 17), Color.White, // group
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
                    themelist.Add(new Settings(jo, fi.Name));
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
        


        public void FillComboBoxWithThemes(ComboBox comboBoxTheme)          // fill in a combo box with default themes
        {
            comboBoxTheme.Items.Clear();
            for (int i = 0; i < themelist.Count; i++)
                comboBoxTheme.Items.Add(themelist[i].name);
            comboBoxTheme.Items.Add("Custom");                          // and add an extra custom one which enables the individual controls
        }

        public bool SetComboBoxIndex(ComboBox comboBoxTheme)                // set the index of the combo box to the current theme
        {
            for (int i = 0; i < comboBoxTheme.Items.Count; i++)
            {
                if (comboBoxTheme.Items[i].Equals(currentsettings.name))
                {
                    comboBoxTheme.SelectedIndex = i;
                    return true;
                }
            }

            return false;
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

        public bool IsFontAvailableInTheme(string themename, out string fontwanted)
        {
            int i = FindThemeIndex(themename);
            fontwanted = null;

            if (i != -1)
            {
                fontwanted = themelist[i].fontname;
                using (Font fntnew = new Font(fontwanted, themelist[i].fontsize, FontStyle.Regular))
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
        }

        public void UpdateColorControls(Control parent , Control myControl, Font fnt, int level)
        {
#if DEBUG
            //string pad = "                             ".Substring(0, level);
            //Console.WriteLine(pad + parent.Name.ToString() + ":" + myControl.Name.ToString() + " " + myControl.ToString());
#endif
            if (myControl is MenuStrip || myControl is ToolStrip)
            {
                myControl.BackColor = currentsettings.colors[Settings.CI.menu_back];
                myControl.ForeColor = currentsettings.colors[Settings.CI.menu_fore];
                myControl.Font = fnt;
            }
            else if (myControl is RichTextBox || myControl is TextBox)
            {
                myControl.BackColor = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.textbox_back]);
                myControl.ForeColor = currentsettings.colors[Settings.CI.textbox_fore];

                if (myControl.Font.Name.Contains("Courier"))                  // okay if we ordered a fixed font, don't override
                {
                    Font fntf = new Font(myControl.Font.Name, currentsettings.fontsize); // make one of the selected size
                    myControl.Font = fntf;
                }
                else
                    myControl.Font = fnt;
            }
            else if (myControl is NumericUpDown)
            {                                                                   // BACK colour does not work..
                myControl.ForeColor = currentsettings.colors[Settings.CI.textbox_fore]; 
                myControl.Font = fnt;
            }
            else if (myControl is Panel)
            {
                if (!( myControl.Name.Contains("theme") || myControl.Name.Contains("defaultmapcolor") ))                 // theme panels show settings color - don't overwrite
                    myControl.BackColor = currentsettings.colors[Settings.CI.form];
            }
            else if (myControl is Button || myControl is ComboBox)
            {
                myControl.BackColor = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.button_back]);
                myControl.ForeColor = currentsettings.colors[Settings.CI.button_text];
                myControl.Font = fnt;
            }
            else if (myControl is TabControl)
            {
                myControl.Font = fnt;
                // Needs to be owner drawn..
            }
            else if (myControl is ComboBox)
            {
                myControl.Font = fnt;
                // Back/Fore only affects drop down list - need to owner draw..
            }
            else if (myControl is Label)
            {
                myControl.ForeColor = currentsettings.colors[Settings.CI.label];
                myControl.Font = fnt;
            }
            else if (myControl is GroupBox)
            {
                GroupBox MyDgv = (GroupBox)myControl;
                MyDgv.ForeColor = currentsettings.colors[Settings.CI.group_text];
                MyDgv.BackColor = currentsettings.colors[Settings.CI.group_back];
                myControl.Font = fnt;
            }
            else if (myControl is CheckBox || myControl is RadioButton)
            {
                myControl.ForeColor = currentsettings.colors[Settings.CI.checkbox];
                myControl.Font = fnt;
            }
            else if (myControl is DataGridView)
            {
                DataGridView MyDgv = (DataGridView)myControl;
                MyDgv.EnableHeadersVisualStyles = false;            // without this, the colours for the grid are not applied.

                MyDgv.RowHeadersDefaultCellStyle.BackColor = currentsettings.colors[Settings.CI.grid_border];
                MyDgv.RowHeadersDefaultCellStyle.ForeColor = currentsettings.colors[Settings.CI.grid_bordertext];
                MyDgv.ColumnHeadersDefaultCellStyle.BackColor = currentsettings.colors[Settings.CI.grid_border];
                MyDgv.ColumnHeadersDefaultCellStyle.ForeColor = currentsettings.colors[Settings.CI.grid_bordertext];

                MyDgv.BackgroundColor = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.form]);
                MyDgv.DefaultCellStyle.BackColor = GroupBoxOverride(parent, currentsettings.colors[Settings.CI.grid_background]);
                MyDgv.DefaultCellStyle.ForeColor = currentsettings.colors[Settings.CI.grid_text];
                MyDgv.DefaultCellStyle.SelectionBackColor = MyDgv.DefaultCellStyle.ForeColor;
                MyDgv.DefaultCellStyle.SelectionForeColor = MyDgv.DefaultCellStyle.BackColor;

                myControl.Font = fnt;
                if (myControl.Name.Contains("dataGridViewTravel") && fnt.Size > 9F)
                {
                    Font fnt2 = new Font(currentsettings.fontname, 9F);
                    MyDgv.Columns[0].DefaultCellStyle.Font = fnt2;
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
