using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.ScreenShots
{
    public partial class ScreenShotConfigureForm : ExtendedControls.DraggableForm
    {
        public string ScreenshotsDir { get { return textBoxScreenshotsDir.Text; } }
        public string OutputDir { get { return textBoxOutputDir.Text; } }
        public ScreenShotImageConverter.InputTypes InputFileExtension { get { return (ScreenShotImageConverter.InputTypes)comboBoxScanFor.SelectedIndex; } }
        public ScreenShotImageConverter.OutputTypes OutputFileExtension { get { return (ScreenShotImageConverter.OutputTypes)comboBoxOutputAs.SelectedIndex; } }
        public int FileNameFormat { get { return comboBoxFileNameFormat.SelectedIndex; } }
        public int FolderNameFormat { get { return comboBoxSubFolder.SelectedIndex; } }
        public bool CropImage { get { return checkBoxCropImage.Checked; } }
        public Rectangle CropArea { get { return new Rectangle(numericUpDownLeft.Value,numericUpDownTop.Value,numericUpDownWidth.Value,numericUpDownHeight.Value); } }

        string initialssfolder;
        Font font;
        bool hires; // for file name presentation only

        public ScreenShotConfigureForm()
        {
            InitializeComponent();
            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            font = new Font(theme.FontName, 10);
            bool winborder = theme.ApplyToForm(this, font);
            panelTop.Visible = panelTop.Enabled = !winborder;
        }

        public void Init(ScreenShotConverter cf , bool hires)
        {
            this.hires = hires;
            comboBoxOutputAs.Items.AddRange(Enum.GetNames(typeof(ScreenShotImageConverter.OutputTypes)));
            comboBoxOutputAs.SelectedIndex = (int)cf.OutputFileExtension;
            comboBoxScanFor.Items.AddRange(Enum.GetNames(typeof(ScreenShotImageConverter.InputTypes)));
            comboBoxScanFor.SelectedIndex = (int)cf.InputFileExtension;
            initialssfolder = textBoxScreenshotsDir.Text = cf.ScreenshotsDir;
            textBoxOutputDir.Text = cf.OutputDir;
            comboBoxSubFolder.Items.AddRange(ScreenShotImageConverter.SubFolderSelections);
            comboBoxSubFolder.SelectedIndex = cf.FolderNameFormat;
            comboBoxFileNameFormat.Items.AddRange(ScreenShotImageConverter.FileNameFormats);
            comboBoxFileNameFormat.SelectedIndex = cf.FileNameFormat;

            checkBoxCropImage.Checked = cf.CropImage;
            numericUpDownTop.Value = cf.CropArea.Top;
            numericUpDownLeft.Value = cf.CropArea.Left;
            numericUpDownWidth.Value = cf.CropArea.Width;
            numericUpDownHeight.Value = cf.CropArea.Height;
            SetNumEnabled();

            textBoxFileNameExample.Text = ScreenShotImageConverter.CreateFileName("Sol", "Earth", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, hires, DateTime.Now);
        }

        private void ScreenShotConfigureForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            font.Dispose();
            font = null;
        }


        private void SetNumEnabled()
        {
            numericUpDownTop.Enabled = numericUpDownLeft.Enabled = numericUpDownWidth.Enabled = numericUpDownHeight.Enabled = checkBoxCropImage.Checked;
        }

        private void panel_close_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void panel_minimize_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label_index_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void buttonChangeEDScreenshot_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            dlg.Description = "Select ED screenshot folder";
            dlg.SelectedPath = textBoxScreenshotsDir.Text;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                initialssfolder = textBoxScreenshotsDir.Text = dlg.SelectedPath;
            }
        }

        private void buttonChangeOutputFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            dlg.Description = "Select converted screenshot folder";
            dlg.SelectedPath = textBoxOutputDir.Text;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                textBoxOutputDir.Text = dlg.SelectedPath;
            }
        }

        private void checkBoxCropImage_CheckedChanged(object sender, EventArgs e)
        {
            SetNumEnabled();
        }

        private void textBoxScreenshotsDir_Leave(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBoxScreenshotsDir.Text))
            {
                ExtendedControls.MessageBoxTheme.Show(this, "Folder specified does not exist");
                textBoxScreenshotsDir.Text = initialssfolder;
            }
            else
                initialssfolder = textBoxScreenshotsDir.Text;
        }

        private void textBoxScreenshotsDir_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBoxScreenshotsDir_Leave(sender, e);
            }
        }

        private void buttonExtOK_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBoxScreenshotsDir.Text))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this, "Folder specified for scanning does not exist, correct or cancel");
        }

        private void buttonExtCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void comboBoxFileNameFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxFileNameExample.Text = ScreenShotImageConverter.CreateFileName("Sol", "Earth", "HighResScreenshot_0000.bmp", comboBoxFileNameFormat.SelectedIndex, hires, DateTime.Now);
        }
    }
}
