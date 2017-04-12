using EDDiscovery.Win32Constants;
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
    public partial class CommanderForm : Form
    {
        Font font;

        public CommanderForm()
        {
            InitializeComponent();

            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            font = new Font(theme.FontName, 10);
            bool winborder = theme.ApplyToForm(this, font);
            panelTop.Visible = panelTop.Enabled = !winborder;
        }

        private void CommanderForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            font.Dispose();
            font = null;
        }

        public void Init( EDCommander cmdr)
        {
            textBoxBorderCmdr.Text = cmdr.Name;
            textBoxBorderJournal.Text = cmdr.JournalDir;
            textBoxBorderEDSMName.Text = cmdr.EdsmName;
            textBoxBorderEDSMAPI.Text = cmdr.APIKey;
            checkBoxCustomEDSMFrom.Checked = cmdr.SyncFromEdsm;
            checkBoxCustomEDSMTo.Checked = cmdr.SyncToEdsm;
            checkBoxCustomEDDNTo.Checked = cmdr.SyncToEddn;
            textBoxBorderTravelLog.Text = cmdr.NetLogDir;
        }

        public void Update( EDCommander cmdr )
        {
            cmdr.Name = textBoxBorderCmdr.Text;
            cmdr.JournalDir = textBoxBorderJournal.Text;
            cmdr.EdsmName = textBoxBorderEDSMName.Text;
            cmdr.APIKey = textBoxBorderEDSMAPI.Text;
            cmdr.SyncFromEdsm = checkBoxCustomEDSMFrom.Checked;
            cmdr.SyncToEdsm = checkBoxCustomEDSMTo.Checked;
            cmdr.SyncToEddn = checkBoxCustomEDDNTo.Checked;
            cmdr.NetLogDir = textBoxBorderTravelLog.Text;
        }

        public bool Valid { get { return textBoxBorderCmdr.Text != ""; } }
        public string CommanderName { get { return textBoxBorderCmdr.Text; } }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


        #region Window Control

        private IntPtr SendMessage(int msg, IntPtr wparam, IntPtr lparam)
        {
            Message message = Message.Create(this.Handle, msg, wparam, lparam);
            this.WndProc(ref message);
            return message.Result;
        }

        private void label_index_MouseDown(object sender, MouseEventArgs e)
        {
            ((Control)sender).Capture = false;
            SendMessage(WM.NCLBUTTONDOWN, (IntPtr)HT.CAPTION, IntPtr.Zero);
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
