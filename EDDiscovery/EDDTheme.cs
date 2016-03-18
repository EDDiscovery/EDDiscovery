using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery.DB;

namespace EDDiscovery2
{
    public class EDDTheme
    {
        public struct Settings
        {
            public Settings( String n , Color f , Color b, Color t, Color th , Color vs, Color mb, bool wf , double op )
            {
                name = n;  forecolor = f; backcolor = b; textcolor = t; texthighlightcolor = th;
                visitedsystemcolor = vs; mapblockcolor = mb;
                windowsframe = wf; formopacity = op;
            }

            public string name;             
            public Color forecolor;
            public Color backcolor;
            public Color textcolor;
            public Color texthighlightcolor;
            public Color visitedsystemcolor;
            public Color mapblockcolor;
            public bool windowsframe;
            public double formopacity;
        };

        private Settings currentsettings;           // if name = custom, then its not a standard theme..
        private Settings[] themelist;
        private SQLiteDBClass db;

        public EDDTheme()
        {
            themelist = new Settings[7];

            themelist[0] = new Settings("Windows Default", SystemColors.MenuText, SystemColors.Menu,
                                                               SystemColors.WindowText, Color.Red, Color.Blue, Color.Red, true, 100);

            themelist[1] = new Settings("Orange Delight", Color.Orange, Color.Black, Color.Orange, Color.Red, Color.White, Color.Red, false, 100);
            themelist[2] = new Settings("Orange Delight Opaque", Color.Orange, Color.Black, Color.Orange, Color.Red, Color.White, Color.Red, false, 90);
            themelist[3] = new Settings("Orange Delight Transparent", Color.Orange, Color.Black, Color.Orange, Color.Red, Color.White, Color.Red, false, 60);

            themelist[4] = new Settings("Blue Wonder", Color.White, Color.DarkBlue, Color.White, Color.Red, Color.Cyan, Color.Red, false, 100);
            themelist[5] = new Settings("Blue Wonder Opaque", Color.White, Color.DarkBlue, Color.White, Color.Red, Color.Cyan, Color.Red, false, 90);

            themelist[6] = new Settings("Green Baize Opaque", Color.White, Color.FromArgb(255,48,121,17), Color.White, Color.Red, Color.Cyan, Color.Red, false, 90);

            currentsettings = themelist[0];             //default old theme
        }

        public void RestoreSettings()
        {
            if (db == null)
                db = new SQLiteDBClass();

            if (db.keyExists( "ThemeForeColor"))                                         // if there.. get the others with a good default in case the db is screwed.
            {
                currentsettings.forecolor = Color.FromArgb(db.GetSettingInt("ThemeForeColor", SystemColors.MenuText.ToArgb()));
                currentsettings.backcolor = Color.FromArgb(db.GetSettingInt("ThemeBackColor", SystemColors.Menu.ToArgb()));
                currentsettings.textcolor = Color.FromArgb(db.GetSettingInt("ThemeTextColor", SystemColors.WindowText.ToArgb()));
                currentsettings.texthighlightcolor = Color.FromArgb(db.GetSettingInt("ThemeTextHighlightColor", Color.Red.ToArgb()));
                currentsettings.visitedsystemcolor = Color.FromArgb(db.GetSettingInt("ThemeVisitedSystemColor", Color.Blue.ToArgb()));
                currentsettings.mapblockcolor = Color.FromArgb(db.GetSettingInt("ThemeMapBlockColor", Color.Red.ToArgb()));
                currentsettings.windowsframe = db.GetSettingBool("ThemeWindowsFrame", true);
                currentsettings.formopacity = db.GetSettingDouble("ThemeFormOpacity", 100);
                currentsettings.name = db.GetSettingString("ThemeName", "Custom");
            }
        }

        public void SaveSettings()
        {
            if (db == null)
                db = new SQLiteDBClass();

            db.PutSettingInt("ThemeForeColor", currentsettings.forecolor.ToArgb());
            db.PutSettingInt("ThemeBackColor", currentsettings.backcolor.ToArgb());
            db.PutSettingInt("ThemeTextColor", currentsettings.textcolor.ToArgb());
            db.PutSettingInt("ThemeTextHighlightColor", currentsettings.texthighlightcolor.ToArgb());
            db.PutSettingInt("ThemeVisitedSystemColor", currentsettings.visitedsystemcolor.ToArgb());
            db.PutSettingInt("ThemeMapBlockColor", currentsettings.mapblockcolor.ToArgb());
            db.PutSettingBool("ThemeWindowsFrame", currentsettings.windowsframe);
            db.PutSettingDouble("ThemeFormOpacity", currentsettings.formopacity);
            db.PutSettingString("ThemeName", currentsettings.name);
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
        {
            currentsettings.name = "Custom";                                // set so custom..
        }

        public void ApplyColors(Form form)
        {
            form.Opacity = currentsettings.formopacity / 100;
            form.ForeColor = currentsettings.forecolor;
            form.BackColor = currentsettings.backcolor;

            foreach (Control c in form.Controls)
            {
                UpdateColorControls(c);
            }
        }

        public void UpdateColorControls(Control myControl)
        {
            if (myControl is RichTextBox)
            {
                myControl.BackColor = currentsettings.backcolor;
                myControl.ForeColor = currentsettings.textcolor;
            }
            else if (myControl is DataGridView)
            {
                DataGridView MyDgv = (DataGridView)myControl;
                MyDgv.ColumnHeadersDefaultCellStyle.BackColor = currentsettings.backcolor;     // NOT WORKING
                MyDgv.ColumnHeadersDefaultCellStyle.ForeColor = currentsettings.forecolor;
                MyDgv.BackgroundColor = currentsettings.backcolor;
                MyDgv.DefaultCellStyle.BackColor = currentsettings.backcolor;
                MyDgv.DefaultCellStyle.ForeColor = currentsettings.forecolor;
            }
            else
            {
                try
                {
                    myControl.BackColor = currentsettings.backcolor;
                    myControl.ForeColor = currentsettings.forecolor;
                }
                catch { }
            }

            foreach (Control subC in myControl.Controls)
            {
                UpdateColorControls(subC);
            }
        }

        public enum EditIndex { Fore,Back,Text,HL,Visited,MapBlock };

        public bool EditColor(EditIndex ex)                      // name is used to index the color. cuts down code in settings
        {
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
                return false;
        }

        public Color ForeColor { get { return currentsettings.forecolor; } set { SetCustom(); currentsettings.forecolor = value; } }
        public Color BackColor { get { return currentsettings.backcolor; } set { SetCustom(); currentsettings.backcolor = value; } }
        public Color TextColor { get { return currentsettings.textcolor; } set { SetCustom(); currentsettings.textcolor = value; } }
        public Color TextHighlightColor { get { return currentsettings.texthighlightcolor; } set { SetCustom(); currentsettings.texthighlightcolor = value; } }
        public Color VisitedSystemColor { get { return currentsettings.visitedsystemcolor; } set { SetCustom(); currentsettings.visitedsystemcolor = value; } }
        public Color MapBlockColor { get { return currentsettings.mapblockcolor; } set { SetCustom(); currentsettings.mapblockcolor = value; } }
        public bool WindowsFrame { get { return currentsettings.windowsframe; } set { SetCustom(); currentsettings.windowsframe = value; } }
        public double Opacity { get { return currentsettings.formopacity; } set { SetCustom(); currentsettings.formopacity = value; } }
        public string Name { get { return currentsettings.name; } }
    }
}
