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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtendedControls
{
    // Editor for the standard themes

    public partial class ThemeStandardEditor : Form
    {
        public Action ApplyChanges = null;

        private ThemeStandard theme;

        public ThemeStandardEditor()
        {
            InitializeComponent();
            theme = ThemeableFormsInstance.Instance as ThemeStandard;
        }

        public void InitForm()
        {
            comboBox_TextBorder.DataSource = ThemeStandard.TextboxBorderStyles;
            comboBox_ButtonStyle.DataSource = ThemeStandard.ButtonStyles;

            SetPanel(panel_theme1, "Form Back Colour", ThemeStandard.Settings.CI.form);                  // using tag, and tool tips, hook up patches to enum
            SetPanel(panel_theme2, "Text box Back Colour", ThemeStandard.Settings.CI.textbox_back);
            SetPanel(panel_theme3, "Text box Text Colour", ThemeStandard.Settings.CI.textbox_fore);
            SetPanel(panel_theme4, "Text box Highlight Colour", ThemeStandard.Settings.CI.textbox_highlight);
            SetPanel(panel_theme15, "Text box Success Colour", ThemeStandard.Settings.CI.textbox_success);
            SetPanel(panel_theme5, "Button Back Colour", ThemeStandard.Settings.CI.button_back);
            SetPanel(panel_theme6, "Button Text Colour", ThemeStandard.Settings.CI.button_text);
            SetPanel(panel_theme7, "Grid Border Back Colour", ThemeStandard.Settings.CI.grid_borderback);
            SetPanel(panel_theme8, "Grid Border Text Colour", ThemeStandard.Settings.CI.grid_bordertext);
            SetPanel(panel_theme9, "Grid Cell Back Colour", ThemeStandard.Settings.CI.grid_cellbackground);
            SetPanel(panel_theme10, "Grid Cell Text Colour", ThemeStandard.Settings.CI.grid_celltext);
            SetPanel(panel_theme11, "Menu Back Colour", ThemeStandard.Settings.CI.menu_back);
            SetPanel(panel_theme12, "Menu Text Colour", ThemeStandard.Settings.CI.menu_fore);
            SetPanel(panel_theme13, "Visited system without known position", ThemeStandard.Settings.CI.travelgrid_nonvisted);
            SetPanel(panel_theme14, "Visited system with coordinates", ThemeStandard.Settings.CI.travelgrid_visited);
            SetPanel(panel_theme16, "Check Box Text Colour", ThemeStandard.Settings.CI.checkbox);
            SetPanel(panel_theme17, "Label Text Colour", ThemeStandard.Settings.CI.label);
            SetPanel(panel_theme18, "Group box Back Colour", ThemeStandard.Settings.CI.group_back);
            SetPanel(panel_theme19, "Group box Text Colour", ThemeStandard.Settings.CI.group_text);
            SetPanel(panel_theme30, "Text Box Border Colour", ThemeStandard.Settings.CI.textbox_border);
            SetPanel(panel_theme31, "Button Border Colour", ThemeStandard.Settings.CI.button_border);
            SetPanel(panel_theme32, "Grid Border Line Colour", ThemeStandard.Settings.CI.grid_borderlines);
            SetPanel(panel_theme33, "Group box Border Line Colour", ThemeStandard.Settings.CI.group_borderlines);
            SetPanel(panel_theme35, "Tab Control Border Line Colour", ThemeStandard.Settings.CI.tabcontrol_borderlines);
            SetPanel(panel_theme40, "Text Box Scroll Bar Slider Colour", ThemeStandard.Settings.CI.textbox_sliderback);
            SetPanel(panel_theme41, "Text Box Scroll Bar Arrow Colour", ThemeStandard.Settings.CI.textbox_scrollarrow);
            SetPanel(panel_theme42, "Text Box Scroll Bar Button Colour", ThemeStandard.Settings.CI.textbox_scrollbutton);
            SetPanel(panel_theme43, "Grid Scroll Bar Slider Colour", ThemeStandard.Settings.CI.grid_sliderback);
            SetPanel(panel_theme44, "Grid Scroll Bar Arrow Colour", ThemeStandard.Settings.CI.grid_scrollarrow);
            SetPanel(panel_theme45, "Grid Scroll Bar Button Colour", ThemeStandard.Settings.CI.grid_scrollbutton);
            SetPanel(panel_theme50, "Menu Dropdown Back Colour", ThemeStandard.Settings.CI.menu_dropdownback);
            SetPanel(panel_theme51, "Menu Dropdown Text Colour", ThemeStandard.Settings.CI.menu_dropdownfore);
            SetPanel(panel_theme60, "Tool Strip Back Colour", ThemeStandard.Settings.CI.toolstrip_back);
            SetPanel(panel_theme61, "Tool Strip Border Colour", ThemeStandard.Settings.CI.toolstrip_border);
            SetPanel(panel_theme70, "Check Box Tick Color", ThemeStandard.Settings.CI.checkbox_tick );
            SetPanel(panel_theme71, "S-Panel Text Colour", ThemeStandard.Settings.CI.s_panel);

            UpdatePatchesEtc();

            trackBar_theme_opacity.Value = (int)theme.Opacity;
            comboBox_TextBorder.SelectedItem = theme.TextBlockBorderStyle;
            comboBox_ButtonStyle.SelectedItem = theme.ButtonStyle;
        }

        public void UpdatePatchesEtc()                                         // update patch colours..
        {
            UpdatePatch(panel_theme1);
            UpdatePatch(panel_theme2);
            UpdatePatch(panel_theme3);
            UpdatePatch(panel_theme4);
            UpdatePatch(panel_theme5);
            UpdatePatch(panel_theme6);
            UpdatePatch(panel_theme7);
            UpdatePatch(panel_theme8);
            UpdatePatch(panel_theme9);
            UpdatePatch(panel_theme10);
            UpdatePatch(panel_theme11);
            UpdatePatch(panel_theme12);
            UpdatePatch(panel_theme13);
            UpdatePatch(panel_theme14);
            UpdatePatch(panel_theme15);
            UpdatePatch(panel_theme16);
            UpdatePatch(panel_theme17);
            UpdatePatch(panel_theme18);
            UpdatePatch(panel_theme19);
            UpdatePatch(panel_theme30);
            UpdatePatch(panel_theme31);
            UpdatePatch(panel_theme32);
            UpdatePatch(panel_theme33);
            UpdatePatch(panel_theme35);
            UpdatePatch(panel_theme40);
            UpdatePatch(panel_theme41);
            UpdatePatch(panel_theme42);
            UpdatePatch(panel_theme43);
            UpdatePatch(panel_theme44);
            UpdatePatch(panel_theme45);
            UpdatePatch(panel_theme50);
            UpdatePatch(panel_theme51);
            UpdatePatch(panel_theme60);
            UpdatePatch(panel_theme61);
            UpdatePatch(panel_theme70);
            UpdatePatch(panel_theme71);
            textBox_Font.Text = theme.FontName + " " + theme.FontSize + " points";
            checkBox_theme_windowframe.Checked = theme.WindowsFrame;
        }

        private void UpdatePatch(Panel pn)
        {
            ThemeStandard.Settings.CI ci = (ThemeStandard.Settings.CI)(pn.Tag);
            pn.BackColor = theme.currentsettings.colors[ci];
        }


        private void SetPanel(Panel pn, string name, ThemeStandard.Settings.CI ex)
        {
            toolTip1.SetToolTip(pn, name);        // assign tool tips and indicate which color to edit
            pn.Tag = ex;
            pn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_theme_Click);
        }

        private void trackBar_theme_opacity_MouseCaptureChanged(object sender, EventArgs e)
        {
            theme.Opacity = (double)trackBar_theme_opacity.Value;
            ApplyChanges?.Invoke();
        }

        private void checkBox_theme_windowframe_MouseClick(object sender, MouseEventArgs e)
        {
            theme.WindowsFrame = checkBox_theme_windowframe.Checked;
            ApplyChanges?.Invoke();
        }

        private void panel_theme_Click(object sender, EventArgs e)
        {
            ThemeStandard.Settings.CI ci = (ThemeStandard.Settings.CI)(((Control)sender).Tag);        // tag carries the colour we want to edit

            if (EditColor(ci))
            {
                ApplyChanges?.Invoke();
                UpdatePatchesEtc();
            }
        }

        public bool EditColor(ThemeStandard.Settings.CI ex)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.AllowFullOpen = true;
            MyDialog.FullOpen = true;
            MyDialog.Color = theme.currentsettings.colors[ex];

            if (MyDialog.ShowDialog(this) == DialogResult.OK)
            {
                theme.currentsettings.colors[ex] = MyDialog.Color;
                theme.SetCustom();
                return true;
            }
            else
                return false;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox_Font_MouseClick(object sender, MouseEventArgs e)
        {
            using (FontDialog fd = new FontDialog())
            {
                fd.Font = new Font(theme.FontName, theme.FontSize);
                fd.MinSize = 4;
                fd.MaxSize = 12;
                DialogResult result;

                try
                {
                    result = fd.ShowDialog(this);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (result == DialogResult.OK)
                {
                    if (fd.Font.Style == FontStyle.Regular)
                    {
                        theme.FontName = fd.Font.Name;
                        theme.FontSize = fd.Font.Size;
                        UpdatePatchesEtc();
                        ApplyChanges?.Invoke();
                    }
                    else
                        ExtendedControls.MessageBoxTheme.Show(this, "Font does not have regular style");
                }
            }
        }

        private void comboBox_TextBorder_SelectionChangeCommitted(object sender, EventArgs e)
        {
            theme.TextBlockBorderStyle = (string)comboBox_TextBorder.SelectedItem;
            ApplyChanges?.Invoke();
        }

        private void comboBox_ButtonStyle_SelectionChangeCommitted(object sender, EventArgs e)
        {
            theme.ButtonStyle = (string)comboBox_ButtonStyle.SelectedItem;
            ApplyChanges?.Invoke();
        }

    }
}
