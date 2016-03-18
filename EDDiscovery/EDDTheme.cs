using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2
{
    public class EDDTheme
    {
        public struct Settings
        {
            public Settings( String n , Color f , Color b, Color t, Color th , Color vs, bool wf , double op )
            {
                name = n;  forecolor = f; backcolor = b; textcolor = t; texthighlightcolor = th; visitedsystemcolor = vs;
                windowsframe = wf; formopacity = op;
            }

            public string name;             
            public Color forecolor;
            public Color backcolor;
            public Color textcolor;
            public Color texthighlightcolor;
            public Color visitedsystemcolor;
            public bool windowsframe;
            public double formopacity;
        };

        private Settings currentsettings;           // if name = custom, then its not a standard theme..
        private Settings[] themelist;

        public EDDTheme()
        {
            themelist = new Settings[6];

            themelist[0] = new Settings("Windows Default", SystemColors.MenuText, SystemColors.Menu,
                                                               SystemColors.WindowText, Color.Red, Color.Blue, true, 100);

            themelist[1] = new Settings("Orange Delight", Color.Orange, Color.Black, Color.Orange, Color.Red, Color.White, false, 100);
            themelist[2] = new Settings("Orange Delight Opaque", Color.Orange, Color.Black, Color.Orange, Color.Red, Color.White, false, 90);
            themelist[3] = new Settings("Orange Delight Transparent", Color.Orange, Color.Black, Color.Orange, Color.Red, Color.White, false, 60);

            themelist[4] = new Settings("Blue Wonder", Color.White, Color.DarkBlue, Color.White, Color.Red, Color.Cyan, false, 100);
            themelist[5] = new Settings("Blue Wonder Opaque", Color.White, Color.DarkBlue, Color.White, Color.Red, Color.Cyan, false, 90);

            currentsettings = themelist[2];
        }

        public void RestoreSettings()
        {
            // tbd
        }

        public void SaveSettings()
        {
            // tbd
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
            try
            {
                myControl.BackColor = currentsettings.backcolor;
                myControl.ForeColor = currentsettings.forecolor;
            }
            catch { }

            if (myControl is DataGridView)
            {
                DataGridView MyDgv = (DataGridView)myControl;
                MyDgv.ColumnHeadersDefaultCellStyle.BackColor = currentsettings.backcolor;     // NOT WORKING
                MyDgv.ColumnHeadersDefaultCellStyle.ForeColor = currentsettings.forecolor;
                MyDgv.BackgroundColor = currentsettings.backcolor;
                MyDgv.DefaultCellStyle.BackColor = currentsettings.backcolor;
                MyDgv.DefaultCellStyle.ForeColor = currentsettings.forecolor;
            }

            foreach (Control subC in myControl.Controls)
            {
                UpdateColorControls(subC);
            }
        }

        public Color ForeColor { get { return currentsettings.forecolor; } set { currentsettings.forecolor = value; } }
        public Color BackColor { get { return currentsettings.backcolor; } set { currentsettings.backcolor = value; } }
        public Color TextColor { get { return currentsettings.textcolor; } set { currentsettings.textcolor = value; } }
        public Color TextHighlightColor { get { return currentsettings.texthighlightcolor; } set { currentsettings.texthighlightcolor = value; } }
        public Color VisitedSystemColor { get { return currentsettings.visitedsystemcolor; } set { currentsettings.visitedsystemcolor = value; } }
        public bool WindowsFrame { get { return currentsettings.windowsframe; } set { currentsettings.windowsframe = value; } }
        public double Opacity { get { return currentsettings.formopacity; } set { currentsettings.formopacity = value; } }
    }
}
