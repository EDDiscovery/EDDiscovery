using EDDiscovery;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2
{
    public partial class ThemeEditor : Form
    {
        private EDDiscoveryForm _discoveryForm;

        public ThemeEditor()
        {
            InitializeComponent();
        }

        public void InitForm(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
            comboBox_TextBorder.DataSource = EDDTheme.TextboxBorderStyles;
            comboBox_ButtonStyle.DataSource = EDDTheme.ButtonStyles;

            SetPanel(panel_theme1, "Form Back Colour", EDDTheme.Settings.CI.form);                  // using tag, and tool tips, hook up patches to enum
            SetPanel(panel_theme2, "Text box Back Colour", EDDTheme.Settings.CI.textbox_back);
            SetPanel(panel_theme3, "Text box Text Colour", EDDTheme.Settings.CI.textbox_fore);
            SetPanel(panel_theme4, "Text box Highlight Colour", EDDTheme.Settings.CI.textbox_highlight);
            SetPanel(panel_theme15, "Text box Success Colour", EDDTheme.Settings.CI.textbox_success);
            SetPanel(panel_theme5, "Button Back Colour", EDDTheme.Settings.CI.button_back);
            SetPanel(panel_theme6, "Button Text Colour", EDDTheme.Settings.CI.button_text);
            SetPanel(panel_theme7, "Grid Border Back Colour", EDDTheme.Settings.CI.grid_borderback);
            SetPanel(panel_theme8, "Grid Border Text Colour", EDDTheme.Settings.CI.grid_bordertext);
            SetPanel(panel_theme9, "Grid Cell Back Colour", EDDTheme.Settings.CI.grid_cellbackground);
            SetPanel(panel_theme10, "Grid Cell Text Colour", EDDTheme.Settings.CI.grid_celltext);
            SetPanel(panel_theme11, "Menu Back Colour", EDDTheme.Settings.CI.menu_back);
            SetPanel(panel_theme12, "Menu Text Colour", EDDTheme.Settings.CI.menu_fore);
            SetPanel(panel_theme13, "Visited system without known position", EDDTheme.Settings.CI.travelgrid_nonvisted);
            SetPanel(panel_theme14, "Visited system with coordinates", EDDTheme.Settings.CI.travelgrid_visited);
            SetPanel(panel_theme16, "Check Box Text Colour", EDDTheme.Settings.CI.checkbox);
            SetPanel(panel_theme17, "Label Text Colour", EDDTheme.Settings.CI.label);
            SetPanel(panel_theme18, "Group box Back Colour", EDDTheme.Settings.CI.group_back);
            SetPanel(panel_theme19, "Group box Text Colour", EDDTheme.Settings.CI.group_text);
            SetPanel(panel_theme30, "Text Box Border Colour", EDDTheme.Settings.CI.textbox_border);
            SetPanel(panel_theme31, "Button Border Colour", EDDTheme.Settings.CI.button_border);
            SetPanel(panel_theme32, "Grid Border Line Colour", EDDTheme.Settings.CI.grid_borderlines);
            SetPanel(panel_theme33, "Group box Border Line Colour", EDDTheme.Settings.CI.group_borderlines);
            SetPanel(panel_theme35, "Tab Control Border Line Colour", EDDTheme.Settings.CI.tabcontrol_borderlines);
            SetPanel(panel_theme40, "Text Box Scroll Bar Slider Colour", EDDTheme.Settings.CI.textbox_sliderback);
            SetPanel(panel_theme41, "Text Box Scroll Bar Arrow Colour", EDDTheme.Settings.CI.textbox_scrollarrow);
            SetPanel(panel_theme42, "Text Box Scroll Bar Button Colour", EDDTheme.Settings.CI.textbox_scrollbutton);
            SetPanel(panel_theme43, "Grid Scroll Bar Slider Colour", EDDTheme.Settings.CI.grid_sliderback);
            SetPanel(panel_theme44, "Grid Scroll Bar Arrow Colour", EDDTheme.Settings.CI.grid_scrollarrow);
            SetPanel(panel_theme45, "Grid Scroll Bar Button Colour", EDDTheme.Settings.CI.grid_scrollbutton);
            SetPanel(panel_theme50, "Menu Dropdown Back Colour", EDDTheme.Settings.CI.menu_dropdownback);
            SetPanel(panel_theme51, "Menu Dropdown Text Colour", EDDTheme.Settings.CI.menu_dropdownfore);
            SetPanel(panel_theme60, "Tool Strip Back Colour", EDDTheme.Settings.CI.toolstrip_back);
            SetPanel(panel_theme61, "Tool Strip Border Colour", EDDTheme.Settings.CI.toolstrip_border);
            SetPanel(panel_theme62, "Tool Strip Checked Colour", EDDTheme.Settings.CI.toolstrip_buttonchecked);
            SetPanel(panel_theme70, "Check Box Tick Color", EDDTheme.Settings.CI.checkbox_tick );

            UpdatePatchesEtc();

            trackBar_theme_opacity.Value = (int)_discoveryForm.theme.Opacity;
            comboBox_TextBorder.SelectedItem = _discoveryForm.theme.TextBlockBorderStyle;
            comboBox_ButtonStyle.SelectedItem = _discoveryForm.theme.ButtonStyle;
        }

        public void UpdatePatchesEtc()                                         // update patch colours..
        {
            _discoveryForm.theme.UpdatePatch(panel_theme1);
            _discoveryForm.theme.UpdatePatch(panel_theme2);
            _discoveryForm.theme.UpdatePatch(panel_theme3);
            _discoveryForm.theme.UpdatePatch(panel_theme4);
            _discoveryForm.theme.UpdatePatch(panel_theme5);
            _discoveryForm.theme.UpdatePatch(panel_theme6);
            _discoveryForm.theme.UpdatePatch(panel_theme7);
            _discoveryForm.theme.UpdatePatch(panel_theme8);
            _discoveryForm.theme.UpdatePatch(panel_theme9);
            _discoveryForm.theme.UpdatePatch(panel_theme10);
            _discoveryForm.theme.UpdatePatch(panel_theme11);
            _discoveryForm.theme.UpdatePatch(panel_theme12);
            _discoveryForm.theme.UpdatePatch(panel_theme13);
            _discoveryForm.theme.UpdatePatch(panel_theme14);
            _discoveryForm.theme.UpdatePatch(panel_theme15);
            _discoveryForm.theme.UpdatePatch(panel_theme16);
            _discoveryForm.theme.UpdatePatch(panel_theme17);
            _discoveryForm.theme.UpdatePatch(panel_theme18);
            _discoveryForm.theme.UpdatePatch(panel_theme19);
            _discoveryForm.theme.UpdatePatch(panel_theme30);
            _discoveryForm.theme.UpdatePatch(panel_theme31);
            _discoveryForm.theme.UpdatePatch(panel_theme32);
            _discoveryForm.theme.UpdatePatch(panel_theme33);
            _discoveryForm.theme.UpdatePatch(panel_theme35);
            _discoveryForm.theme.UpdatePatch(panel_theme40);
            _discoveryForm.theme.UpdatePatch(panel_theme41);
            _discoveryForm.theme.UpdatePatch(panel_theme42);
            _discoveryForm.theme.UpdatePatch(panel_theme43);
            _discoveryForm.theme.UpdatePatch(panel_theme44);
            _discoveryForm.theme.UpdatePatch(panel_theme45);
            _discoveryForm.theme.UpdatePatch(panel_theme50);
            _discoveryForm.theme.UpdatePatch(panel_theme51);
            _discoveryForm.theme.UpdatePatch(panel_theme60);
            _discoveryForm.theme.UpdatePatch(panel_theme61);
            _discoveryForm.theme.UpdatePatch(panel_theme62);
            _discoveryForm.theme.UpdatePatch(panel_theme70);
            textBox_Font.Text = _discoveryForm.theme.FontName;
            checkBox_theme_windowframe.Checked = _discoveryForm.theme.WindowsFrame;
        }

        private void SetPanel(Panel pn, string name, EDDTheme.Settings.CI ex)
        {
            toolTip1.SetToolTip(pn, name);        // assign tool tips and indicate which color to edit
            pn.Tag = ex;
            pn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_theme_Click);
        }

        private void trackBar_theme_opacity_MouseCaptureChanged(object sender, EventArgs e)
        {
            _discoveryForm.theme.Opacity = (double)trackBar_theme_opacity.Value;
            _discoveryForm.ApplyTheme(true);
        }

        private void checkBox_theme_windowframe_MouseClick(object sender, MouseEventArgs e)
        {
            _discoveryForm.theme.WindowsFrame = checkBox_theme_windowframe.Checked;
            _discoveryForm.ApplyTheme(true);
        }

        private void panel_theme_Click(object sender, EventArgs e)
        {
            EDDTheme.Settings.CI ci = (EDDTheme.Settings.CI)(((Control)sender).Tag);        // tag carries the colour we want to edit

            if (_discoveryForm.theme.EditColor(ci))
            {
                _discoveryForm.ApplyTheme(true);
                UpdatePatchesEtc();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox_Font_MouseClick(object sender, MouseEventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = new Font(_discoveryForm.theme.FontName, _discoveryForm.theme.FontSize);
            fd.MinSize = 4;
            fd.MaxSize = 12;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (fd.Font.Style == FontStyle.Regular)
                {
                    _discoveryForm.theme.FontName = fd.Font.Name;
                    _discoveryForm.theme.FontSize = fd.Font.Size;
                    UpdatePatchesEtc();
                    _discoveryForm.ApplyTheme(true);
                }
                else
                    MessageBox.Show("Font does not have regular style");
            }

        }

        private void comboBox_TextBorder_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _discoveryForm.theme.TextBlockBorderStyle = (string)comboBox_TextBorder.SelectedItem;
            _discoveryForm.ApplyTheme(true);

        }

        private void comboBox_ButtonStyle_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _discoveryForm.theme.ButtonStyle = (string)comboBox_ButtonStyle.SelectedItem;
            _discoveryForm.ApplyTheme(true);
        }

    }
}
