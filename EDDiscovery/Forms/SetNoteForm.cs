using EDDiscovery.EDSM;
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
    public partial class SetNoteForm : Form
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
            this.labelSystem.Text = he.System.name;
            this.labelSummary.Text = he.EventSummary;
            this.labelDetails.Text = he.EventDescription;

            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            theme.ApplyToForm(this);
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
    }
}
