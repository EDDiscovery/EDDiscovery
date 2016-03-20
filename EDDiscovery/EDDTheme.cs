using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery.DB;

// TODO:
// 1. ComboBoxes need owner draw
// 2. grid borders - why can't I change their colors even though their are members for it - is it being overriden during construction?
// 3. put in panels for all colours, show always, allow editing when on custom..
// 4. Fonts - enable them on visual elements.

namespace EDDiscovery2
{
    public class EDDTheme
    {
        public struct Settings
        {
            public Settings( String n , Color f , 
                                        Color bb, Color bf, 
                                        Color gb, Color gbt, Color gbck, Color gt, 
                                        Color tn, Color tv, Color tm,
                                        Color tbb, Color tbf, Color tbh,
                                        Color c,
                                        Color mb, Color mf,
                                        Color l,
                                        bool wf , double op , string ft , float fs )            // ft = empty means don't set it
            {
                name = n;
                form = f;
                button_back = bb; button_text = bf;
                grid_border = gb; grid_bordertext = gbt; grid_background = gbck; grid_text = gt;
                travelgrid_nonvisted = tn; travelgrid_visited = tv; travelgrid_mapblock = tm;
                textbox_back = tbb; textbox_fore = tbf; textbox_highlight = tbh;
                checkbox = c;
                menu_back = mb; menu_fore = mf;
                label = l;
                windowsframe = wf; formopacity = op; fontname = ft; fontsize = fs;
            }

            public string name;         // name of scheme
            public Color form;          // colour of main form

            public Color button_back;   //  buttons/combo boxes
            public Color button_text;

            public Color grid_border;   // all grid border
            public Color grid_bordertext;     // grid border text 
            public Color grid_background;     // grid area background
            public Color grid_text;     // grid text 

            public Color travelgrid_nonvisted; // text color
            public Color travelgrid_visited; // text color
            public Color travelgrid_mapblock;   // map block

            public Color textbox_back;      // text blocks (richtext) colors
            public Color textbox_fore;
            public Color textbox_highlight; 

            public Color checkbox;          // checkbox text color

            public Color menu_back;         // menu strip
            public Color menu_fore;

            public Color label;             // label text color

            public bool windowsframe;
            public double formopacity;

            public string fontname;         // Font.. (empty means don't override)
            public float fontsize;
        };

        public string Name { get { return currentsettings.name; } }

        public Color TextBlock { get { return currentsettings.textbox_fore; } set { SetCustom(); currentsettings.textbox_fore = value; } }
        public Color TextBlockHighlightColor { get { return currentsettings.textbox_highlight; } set { SetCustom(); currentsettings.textbox_highlight = value; } }

        public Color VisitedSystemColor { get { return currentsettings.travelgrid_visited; } set { SetCustom(); currentsettings.travelgrid_visited = value; } }
        public Color NonVisitedSystemColor { get { return currentsettings.travelgrid_nonvisted; } set { SetCustom(); currentsettings.travelgrid_nonvisted = value; } }
        public Color MapBlockColor { get { return currentsettings.travelgrid_mapblock; } set { SetCustom(); currentsettings.travelgrid_mapblock = value; } }

        public bool WindowsFrame { get { return currentsettings.windowsframe; } set { SetCustom(); currentsettings.windowsframe = value; } }
        public double Opacity { get { return currentsettings.formopacity; } set { SetCustom(); currentsettings.formopacity = value; } }

        private Settings currentsettings;           // if name = custom, then its not a standard theme..
        private Settings[] themelist;
        private SQLiteDBClass db;

        public EDDTheme()
        {
            themelist = new Settings[3];

            themelist[0] = new Settings("Windows Default", SystemColors.Menu,
                                                           SystemColors.Menu, SystemColors.MenuText,  // button
                                                           SystemColors.Menu, SystemColors.MenuText, SystemColors.Menu, SystemColors.MenuText,  // grid
                                                           SystemColors.MenuText, Color.Blue, Color.Red, // travel
                                                           SystemColors.Menu, SystemColors.MenuText, Color.Red,  // text
                                                           SystemColors.MenuText, // checkbox
                                                           SystemColors.Menu, SystemColors.MenuText,  // menu
                                                           SystemColors.MenuText,  // label
                                                           true, 100, "", 0);

            themelist[1] = new Settings("Crazy Scheme to show painting", Color.Black,
                                               Color.Gold, Color.Yellow,  // button
                                               Color.Purple, Color.Gray, Color.Beige, Color.Red, // grid
                                               Color.White, Color.Blue, Color.Red, // travel
                                               Color.Green, Color.White, Color.Red,  // text box
                                               Color.Aqua, // checkbox
                                               Color.Black, Color.Red,  // menu
                                               Color.Chocolate,  // label
                                               true, 100, "", 0);

            themelist[2] = new Settings("Crazy Scheme with no border to show painting", Color.Black,
                                               Color.Gold, Color.Yellow,  // button
                                               Color.Purple, Color.Gray, Color.Beige, Color.Red, // grid
                                               Color.White, Color.Blue, Color.Red, // travel
                                               Color.Green, Color.White, Color.Red,  // text box
                                               Color.Aqua, // checkbox
                                               Color.Black, Color.Red,  // menu
                                               Color.Chocolate,  // label
                                               false, 100, "", 0);

            //            themelist[1] = new Settings("Orange Delight", Color.Orange, Color.Black, Color.Orange, Color.Red, Color.White, Color.Red, false, 100);
            //themelist[2] = new Settings("Orange Delight Opaque", Color.Orange, Color.Black, Color.Orange, Color.Red, Color.White, Color.Red, false, 90);
            //themelist[3] = new Settings("Orange Delight Transparent", Color.Orange, Color.Black, Color.Orange, Color.Red, Color.White, Color.Red, false, 60);

            //themelist[4] = new Settings("Blue Wonder", Color.White, Color.DarkBlue, Color.White, Color.Red, Color.Cyan, Color.Red, false, 100);
            //themelist[5] = new Settings("Blue Wonder Opaque", Color.White, Color.DarkBlue, Color.White, Color.Red, Color.Cyan, Color.Red, false, 90);
            //
            //          themelist[6] = new Settings("Green Baize Opaque", Color.White, Color.FromArgb(255,48,121,17), Color.White, Color.Red, Color.Cyan, Color.Red, false, 90);

            currentsettings = themelist[1];             //default old theme
        }

        public void RestoreSettings()
        {
            if (db == null)
                db = new SQLiteDBClass();

            if (db.keyExists( "ThemeBBColor"))           // if there.. get the others with a good default in case the db is screwed.
            {
                currentsettings.name = db.GetSettingString("ThemeName", "Custom");
                currentsettings.form = Color.FromArgb(db.GetSettingInt("ThemeFormColor", SystemColors.MenuText.ToArgb()));
                currentsettings.button_back = Color.FromArgb(db.GetSettingInt("ThemeBBColor", SystemColors.MenuText.ToArgb()));
                currentsettings.button_text = Color.FromArgb(db.GetSettingInt("ThemeBTColor", SystemColors.MenuText.ToArgb()));
                currentsettings.grid_border = Color.FromArgb(db.GetSettingInt("ThemeGBColor", SystemColors.MenuText.ToArgb()));
                currentsettings.grid_bordertext = Color.FromArgb(db.GetSettingInt("ThemeGBTColor", SystemColors.MenuText.ToArgb()));
                currentsettings.grid_background = Color.FromArgb(db.GetSettingInt("ThemeGBCKColor", SystemColors.MenuText.ToArgb()));
                currentsettings.grid_text = Color.FromArgb(db.GetSettingInt("ThemeGTColor", SystemColors.MenuText.ToArgb()));
                currentsettings.travelgrid_nonvisted = Color.FromArgb(db.GetSettingInt("ThemeTNVColor", SystemColors.MenuText.ToArgb()));
                currentsettings.travelgrid_visited = Color.FromArgb(db.GetSettingInt("ThemeTVColor", SystemColors.MenuText.ToArgb()));
                currentsettings.travelgrid_mapblock = Color.FromArgb(db.GetSettingInt("ThemeTMColor", SystemColors.MenuText.ToArgb()));
                currentsettings.textbox_back = Color.FromArgb(db.GetSettingInt("ThemeTBColor", SystemColors.MenuText.ToArgb()));
                currentsettings.textbox_fore = Color.FromArgb(db.GetSettingInt("ThemeTFColor", SystemColors.MenuText.ToArgb()));
                currentsettings.textbox_highlight = Color.FromArgb(db.GetSettingInt("ThemeTHColor", SystemColors.MenuText.ToArgb()));
                currentsettings.checkbox = Color.FromArgb(db.GetSettingInt("ThemeCBColor", SystemColors.MenuText.ToArgb()));
                currentsettings.menu_back = Color.FromArgb(db.GetSettingInt("ThemeMBColor", SystemColors.MenuText.ToArgb()));
                currentsettings.menu_fore = Color.FromArgb(db.GetSettingInt("ThemeMFColor", SystemColors.MenuText.ToArgb()));
                currentsettings.label = Color.FromArgb(db.GetSettingInt("ThemeLabelColor", SystemColors.MenuText.ToArgb()));
                currentsettings.windowsframe = db.GetSettingBool("ThemeWindowsFrame", true);
                currentsettings.formopacity = db.GetSettingDouble("ThemeFormOpacity", 100);
                currentsettings.fontname = db.GetSettingString("ThemeFont", "");
                currentsettings.fontsize = (float)db.GetSettingDouble("ThemeFontSize", 8);
            }
        }

        public void SaveSettings()
        {
            if (db == null)
                db = new SQLiteDBClass();

            db.PutSettingString("ThemeName", currentsettings.name);

            db.PutSettingInt("ThemeFormColor", currentsettings.form.ToArgb());
            db.PutSettingInt("ThemeBBColor", currentsettings.button_back.ToArgb());
            db.PutSettingInt("ThemeBFColor", currentsettings.button_text.ToArgb());
            db.PutSettingInt("ThemeGBColor", currentsettings.grid_border.ToArgb());
            db.PutSettingInt("ThemeGBTColor", currentsettings.grid_bordertext.ToArgb());
            db.PutSettingInt("ThemeGBCKColor", currentsettings.grid_background.ToArgb());
            db.PutSettingInt("ThemeGTColor", currentsettings.grid_text.ToArgb());
            db.PutSettingInt("ThemeTNVColor", currentsettings.travelgrid_nonvisted.ToArgb());
            db.PutSettingInt("ThemeTVColor", currentsettings.travelgrid_visited.ToArgb());
            db.PutSettingInt("ThemeTMColor", currentsettings.travelgrid_mapblock.ToArgb());
            db.PutSettingInt("ThemeTBColor", currentsettings.textbox_back.ToArgb());
            db.PutSettingInt("ThemeTFColor", currentsettings.textbox_fore.ToArgb());
            db.PutSettingInt("ThemeTHColor", currentsettings.textbox_highlight.ToArgb());
            db.PutSettingInt("ThemeCBColor", currentsettings.checkbox.ToArgb());
            db.PutSettingInt("ThemeMBColor", currentsettings.menu_back.ToArgb());
            db.PutSettingInt("ThemeMFColor", currentsettings.menu_fore.ToArgb());
            db.PutSettingInt("ThemeLabelColor", currentsettings.label.ToArgb());
            db.PutSettingBool("ThemeWindowsFrame", currentsettings.windowsframe);
            db.PutSettingDouble("ThemeFormOpacity", currentsettings.formopacity);
            db.PutSettingString("ThemeFont", currentsettings.fontname);
            db.PutSettingDouble("ThemeFontSize", currentsettings.fontsize);
        }

        public void FillComboBoxWithThemes(ComboBox comboBoxTheme)          // fill in a combo box with default themes
        {
            for (int i = 0; i < themelist.Length; i++)
                comboBoxTheme.Items.Add(themelist[i].name);
        }

        public void SetComboBoxIndex(ComboBox comboBoxTheme)                // set the index of the combo box to the current theme
        {
            for (int i = 0; i < comboBoxTheme.Items.Count; i++)
            {
                if ( comboBoxTheme.Items[i].Equals(currentsettings.name))
                    comboBoxTheme.SelectedIndex = i;
            }
        }

        public bool SetThemeByName( string name )                           // given a theme name, select it if possible
        {
            for (int i = 0; i < themelist.Length; i++)
            {
                if ( themelist[i].name.Equals(name))
                {
                    currentsettings = themelist[i];
                    return true;
                }
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
            form.BackColor = currentsettings.form;

            foreach (Control c in form.Controls)
                UpdateColorControls(c);
        }

        public void UpdateColorControls(Control myControl)
        {
            if (myControl is MenuStrip)
            {
                myControl.BackColor = currentsettings.menu_back;
                myControl.ForeColor = currentsettings.menu_fore;
            }
            else if (myControl is RichTextBox || myControl is TextBox)
            {
                myControl.BackColor = currentsettings.textbox_back;
                myControl.ForeColor = currentsettings.textbox_fore;
            }
            else if (myControl is Panel )
            {
                myControl.BackColor = currentsettings.form;
            }
            else if (myControl is Button || myControl is ComboBox)
            {
                myControl.BackColor = currentsettings.button_back;
                myControl.ForeColor = currentsettings.button_text;
            }
            else if (myControl is ComboBox)
            {
                // Back/Fore only affects drop down list - need to owner draw..
            }
            else if (myControl is ListView)
            {
                myControl.BackColor = currentsettings.grid_background;
                myControl.ForeColor = currentsettings.grid_text;
            }
            else if (myControl is Label || myControl is GroupBox )
            {
                myControl.ForeColor = currentsettings.label;
            }
            else if (myControl is CheckBox || myControl is RadioButton )
            {
                myControl.ForeColor = currentsettings.checkbox;
            }
            else if (myControl is DataGridView)
            {
                DataGridView MyDgv = (DataGridView)myControl;
                //MyDgv.ColumnHeadersDefaultCellStyle.BackColor = currentsettings.grid_border;   // NOT WORKING
                //MyDgv.ColumnHeadersDefaultCellStyle.ForeColor = currentsettings.grid_bordertext;
                MyDgv.BackgroundColor = currentsettings.form;
                MyDgv.DefaultCellStyle.BackColor = currentsettings.grid_background;
                MyDgv.DefaultCellStyle.ForeColor = currentsettings.grid_text;
            }

            foreach (Control subC in myControl.Controls)
            {
                UpdateColorControls(subC);
            }
        }

        public enum EditIndex { Fore,Back,Text,HL,Visited,MapBlock };

        public bool EditColor(EditIndex ex)                      // name is used to index the color. cuts down code in settings
        {
#if false
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;

            if (ex == EditIndex.Fore)
                MyDialog.Color = ForeColor;
            else if (ex == EditIndex.Text)
                MyDialog.Color = TextColor;
            else if (ex == EditIndex.HL)
                MyDialog.Color = TextHighlightColor;
            else if (ex == EditIndex.Visited)
                MyDialog.Color = VisitedSystemColor;
            else if (ex == EditIndex.MapBlock)
                MyDialog.Color = MapBlockColor;
            else
                MyDialog.Color = BackColor;

            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                if (ex == EditIndex.Fore)
                    ForeColor = MyDialog.Color;
                else if (ex == EditIndex.Text)
                    TextColor = MyDialog.Color;
                else if (ex == EditIndex.HL)
                    TextHighlightColor = MyDialog.Color;
                else if (ex == EditIndex.Visited)
                    VisitedSystemColor = MyDialog.Color;
                else if (ex == EditIndex.MapBlock)
                    MapBlockColor = MyDialog.Color;
                else
                    BackColor = MyDialog.Color;

                return true;
            }
            else
#endif
            return false;
        }

    }
}
