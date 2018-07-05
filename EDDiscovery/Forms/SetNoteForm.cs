using EliteDangerousCore;
using EliteDangerousCore.EDSM;
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
    public partial class SetNoteForm : ExtendedControls.DraggableForm
    {
        private EDDiscoveryForm _discoveryForm;
        public HistoryEntry HistoryEntry { get; private set; }
        public string NoteText { get; private set; }

        public SetNoteForm(HistoryEntry he, EDDiscoveryForm parent)
        {
            InitializeComponent();
            this.Owner = parent;
            this._discoveryForm = parent;
            this.HistoryEntry = he;
            this.NoteText = he.snc?.Note;
            this.textBoxNote.Text = this.NoteText ?? "";
            this.labelTimestamp.Text = he.EventTimeLocal.ToString();
            this.labelSystem.Text = he.System.Name;

            he.journalEntry.FillInformation(out string EventDescription, out string EventDetailedInfo);

            this.labelSummary.Text = he.EventSummary;
            this.labelDetails.Text = EventDescription;

            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            bool winborder = theme.ApplyToFormStandardFontSize(this);
            panelTop.Visible = panelTop.Enabled = !winborder;

            BaseUtils.Translator.Instance.Translate(this, new Control[] { labelTimestamp, labelSystem, labelSummary, labelDetails });
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.NoteText = textBoxNote.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void panelTop_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void panel_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
