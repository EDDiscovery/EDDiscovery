using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery2.DB;
using EDDiscovery.DB;

namespace EDDiscovery.Forms
{
    public partial class AssignTravelLogSystemForm : Form
    {
        private class SystemLink
        {
            public string Name { get; set; }
            public long Id { get; set; }
            public bool UseOther { get; set; }
            public ISystem LinkSystem { get; set; }

            public string DisplayName
            {
                get
                {
                    if (LinkSystem == null)
                    {
                        return Name;
                    }
                    else if (LinkSystem.HasCoordinate)
                    {
                        return $"{LinkSystem.name} ({LinkSystem.x},{LinkSystem.y},{LinkSystem.z}) #{LinkSystem.id_edsm}";
                    }
                    else
                    {
                        return $"{LinkSystem.name} #{LinkSystem.id_edsm}";
                    }
                }
            }
        }

        public long AssignedEdsmId { get { return _linkSystem == null ? 0 : _linkSystem.id_edsm; } }
        public ISystem AssignedSystem { get { return _linkSystem; } }

        private TravelHistoryControl _travelHistory;
        private ISystem _linkSystem;
        private VisitedSystemsClass _travelLogEntry;

        public AssignTravelLogSystemForm(TravelHistoryControl travelHistory, VisitedSystemsClass vsc)
        {
            InitializeComponent();
            this._travelHistory = travelHistory;
            this._travelLogEntry = vsc;
            this._linkSystem = vsc.curSystem;

            this.tbLogSystemName.Text = vsc.Name;
            this.tbDateVisited.Text = vsc.Time.ToString();
            this.tbLogCoordX.Text = vsc.HasTravelCoordinates ? vsc.X.ToString("0.000") : "?";
            this.tbLogCoordY.Text = vsc.HasTravelCoordinates ? vsc.Y.ToString("0.000") : "?";
            this.tbLogCoordZ.Text = vsc.HasTravelCoordinates ? vsc.Z.ToString("0.000") : "?";
            this.tbLogCoordX.TextAlign = vsc.HasTravelCoordinates ? HorizontalAlignment.Right : HorizontalAlignment.Center;
            this.tbLogCoordY.TextAlign = vsc.HasTravelCoordinates ? HorizontalAlignment.Right : HorizontalAlignment.Center;
            this.tbLogCoordZ.TextAlign = vsc.HasTravelCoordinates ? HorizontalAlignment.Right : HorizontalAlignment.Center;

            this.tbManualSystemName.ReadOnly = true;
            this.btnFindSystem.Enabled = false;

            List<SystemLink> links = new List<SystemLink>();
            links.Add(new SystemLink { Name = "None", Id = 0, LinkSystem = null, UseOther = false });
            
            if (vsc.alternatives != null)
            {
                foreach (var sys in vsc.alternatives)
                {
                    links.Add(new SystemLink { Name = sys.name, Id = sys.id_edsm, LinkSystem = sys, UseOther = false });
                }
            }

            links.Add(new SystemLink { Name = "Other", Id = -1, LinkSystem = null, UseOther = true });

            this.cbSystemLink.DataSource = links;
            this.cbSystemLink.DisplayMember = "DisplayName";
            this.cbSystemLink.ValueMember = "Id";

            if (vsc.curSystem != null && vsc.alternatives.Contains(vsc.curSystem))
            {
                this.cbSystemLink.SelectedItem = links.First(l => l.LinkSystem == vsc.curSystem);
                UpdateLinkedSystem();
            }
        }

        protected void UpdateLinkedSystem()
        {
            SystemLink selectedItem = (SystemLink)cbSystemLink.SelectedItem;
            _linkSystem = selectedItem.LinkSystem;

            if (selectedItem.LinkSystem != null)
            {
                lblEDSMLink.Text = selectedItem.LinkSystem.name;
            }
            else
            {
                lblEDSMLink.Text = "";
            }

            if (selectedItem.LinkSystem != null && selectedItem.LinkSystem.HasCoordinate)
            {
                tbSysCoordX.Text = selectedItem.LinkSystem.x.ToString("0.000");
                tbSysCoordY.Text = selectedItem.LinkSystem.y.ToString("0.000");
                tbSysCoordZ.Text = selectedItem.LinkSystem.z.ToString("0.000");
                tbSysCoordX.TextAlign = HorizontalAlignment.Right;
                tbSysCoordY.TextAlign = HorizontalAlignment.Right;
                tbSysCoordZ.TextAlign = HorizontalAlignment.Right;
            }
            else
            {
                tbSysCoordX.Text = "?";
                tbSysCoordY.Text = "?";
                tbSysCoordZ.Text = "?";
                tbSysCoordX.TextAlign = HorizontalAlignment.Center;
                tbSysCoordY.TextAlign = HorizontalAlignment.Center;
                tbSysCoordZ.TextAlign = HorizontalAlignment.Center;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cbSystemLink_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateLinkedSystem();
        }

        private void lblEDSMLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_linkSystem != null)
            {
                string url = String.Format("https://www.edsm.net/show-system/index/id/{0}/name/{1}", _linkSystem.id_edsm, Uri.EscapeDataString(_linkSystem.name));
                System.Diagnostics.Process.Start(url);
            }
        }
    }
}
