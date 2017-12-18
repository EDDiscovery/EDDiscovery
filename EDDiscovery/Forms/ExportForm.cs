using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class ExportForm : ExtendedControls.DraggableForm
    {
        public int SelectedIndex { get; private set; }
        public DateTime StartTime { get { return customDateTimePickerFrom.Value; } }
        public DateTime EndTime { get { return customDateTimePickerTo.Value; } }
        public bool Comma { get { return radioButtonComma.Checked; } }
        public bool AutoOpen { get { return checkBoxCustomAutoOpen.Checked; } }
        public bool IncludeHeader { get { return checkBoxIncludeHeader.Checked; } }
        public string Path { get; private set; }

        private Font font;

        public ExportForm()
        {
            InitializeComponent();

        }

        public void Init(string[] exportlist, bool disablestartendtime = false)
        {
            comboBoxCustomExportType.Items.AddRange(exportlist);
            customDateTimePickerFrom.Value = new DateTime(2014, 11, 22, 4, 0, 0, DateTimeKind.Utc); //Gamma start
            customDateTimePickerTo.Value = DateTime.Now;

            if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Equals("."))
                radioButtonComma.Checked = true;
            else
                radioButtonSemiColon.Checked = true;

            checkBoxIncludeHeader.Checked = EliteDangerousCore.DB.SQLiteConnectionUser.GetSettingBool("ExportFormIncludeHeader", true);
            checkBoxCustomAutoOpen.Checked = EliteDangerousCore.DB.SQLiteConnectionUser.GetSettingBool("ExportFormOpenExcel", true);

            comboBoxCustomExportType.SelectedIndex = 0;

            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            font = new Font(theme.FontName, 10);
            bool winborder = theme.ApplyToForm(this, font);
            label_index.Visible = panel_minimize.Visible = panel_close.Visible = !winborder;

            if (disablestartendtime)
            {
                customDateTimePickerFrom.Visible = customDateTimePickerTo.Visible = false;
                int d = panelBottom.Top - customDateTimePickerFrom.Top;
                panelBottom.Top -= d;
                panelOuter.Height -= d;
                Height -= d;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ExportForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            font.Dispose();
            font = null;
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.DB.SQLiteConnectionUser.PutSettingBool("ExportFormIncludeHeader", checkBoxIncludeHeader.Checked);
            EliteDangerousCore.DB.SQLiteConnectionUser.PutSettingBool("ExportFormOpenExcel", checkBoxCustomAutoOpen.Checked);

            SelectedIndex = comboBoxCustomExportType.SelectedIndex;

            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "CSV export| *.csv";
            dlg.Title = "Export current History view to Excel (csv)";

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                Path = dlg.FileName;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void label_index_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void label_index_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
