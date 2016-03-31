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
    public partial class ThemeDialog : Form
    {
        private EDDiscoveryForm _discoveryForm;

        public ThemeDialog()
        {
            InitializeComponent();
        }

        public void InitForm(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;

            SetPanel(panel_theme1, "Form Back Colour", EDDTheme.Settings.CI.form);                  // using tag, and tool tips, hook up patches to enum
            SetPanel(panel_theme2, "Text box Back Colour", EDDTheme.Settings.CI.textbox_back);
            SetPanel(panel_theme3, "Text box Text Colour", EDDTheme.Settings.CI.textbox_fore);
            SetPanel(panel_theme4, "Text box Highlight Colour", EDDTheme.Settings.CI.textbox_highlight);
            SetPanel(panel_theme15, "Text box Success Colour", EDDTheme.Settings.CI.textbox_success);
            SetPanel(panel_theme5, "Button Back Colour", EDDTheme.Settings.CI.button_back);
            SetPanel(panel_theme6, "Button Text Colour", EDDTheme.Settings.CI.button_text);
            SetPanel(panel_theme7, "Grid Border Back Colour", EDDTheme.Settings.CI.grid_border);
            SetPanel(panel_theme8, "Grid Border Text Colour", EDDTheme.Settings.CI.grid_bordertext);
            SetPanel(panel_theme9, "Grid Data Back Colour", EDDTheme.Settings.CI.grid_background);
            SetPanel(panel_theme10, "Grid Data Text Colour", EDDTheme.Settings.CI.grid_text);
            SetPanel(panel_theme11, "Menu Back Colour", EDDTheme.Settings.CI.menu_back);
            SetPanel(panel_theme12, "Menu Text Colour", EDDTheme.Settings.CI.menu_fore);
            SetPanel(panel_theme13, "Visited system without known position", EDDTheme.Settings.CI.travelgrid_nonvisted);
            SetPanel(panel_theme14, "Visited system with coordinates", EDDTheme.Settings.CI.travelgrid_visited);
            SetPanel(panel_theme16, "Check Box Text Colour", EDDTheme.Settings.CI.checkbox);
            SetPanel(panel_theme17, "Label Text Colour", EDDTheme.Settings.CI.label);
            SetPanel(panel_theme18, "Group box Back Colour", EDDTheme.Settings.CI.group_back);
            SetPanel(panel_theme19, "Group box Text Colour", EDDTheme.Settings.CI.group_text);

            UpdatePatchesEtc();

            trackBar_theme_opacity.Value = (int)_discoveryForm.theme.Opacity;
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
            textBox_Font.Text = _discoveryForm.theme.FontName;
            checkBox_theme_windowframe.Checked = _discoveryForm.theme.WindowsFrame;
        }

        private void SetPanel(Panel pn, string name, EDDTheme.Settings.CI ex)
        {
            toolTip1.SetToolTip(pn, name);        // assign tool tips and indicate which color to edit
            pn.Tag = ex;
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
    }
}
