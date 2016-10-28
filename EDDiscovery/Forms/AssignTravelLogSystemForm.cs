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
            public long EdsmId { get; set; }
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

        private ISystem _linkSystem;
        private List<ISystem> _alternatives;
        private string _namestatus;
        private EliteDangerous.JournalEvents.JournalLocOrJump _travelLogEntry;
        private Dictionary<long, SystemLink> _systemLinks;
        private List<SystemLink> _systemLinkList;

        public AssignTravelLogSystemForm(EliteDangerous.JournalEvents.JournalLocOrJump vsc)
        {
            InitializeComponent();
            this._travelLogEntry = vsc;
            SystemClass.GetSystemAndAlternatives(vsc, out _linkSystem, out _alternatives, out _namestatus);

            this.tbLogSystemName.Text = vsc.StarSystem;
            this.tbVisitedDate.Text = vsc.EventTimeLocal.ToString();
            this.tbLogCoordX.Text = vsc.HasCoordinate ? vsc.StarPos[0].ToString("0.000") : "?";
            this.tbLogCoordY.Text = vsc.HasCoordinate ? vsc.StarPos[1].ToString("0.000") : "?";
            this.tbLogCoordZ.Text = vsc.HasCoordinate ? vsc.StarPos[2].ToString("0.000") : "?";
            this.tbLogCoordX.TextAlign = vsc.HasCoordinate ? HorizontalAlignment.Right : HorizontalAlignment.Center;
            this.tbLogCoordY.TextAlign = vsc.HasCoordinate ? HorizontalAlignment.Right : HorizontalAlignment.Center;
            this.tbLogCoordZ.TextAlign = vsc.HasCoordinate ? HorizontalAlignment.Right : HorizontalAlignment.Center;

            UpdateLinkedSystemList(_linkSystem);
            tbManualSystemName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbManualSystemName.AutoCompleteSource = AutoCompleteSource.CustomSource;

            tbManualSystemName.SetAutoCompletor(EDDiscovery.DB.SystemClass.ReturnSystemListForAutoComplete);
        }

        protected void UpdateLinkedSystemList(ISystem focus = null, List<ISystem> othersystems = null)
        {
            _systemLinkList = new List<SystemLink>();
            _systemLinks = new Dictionary<long, SystemLink>();
            _systemLinkList.Add(new SystemLink { Name = "None", Id = 0, EdsmId = 0, LinkSystem = null });

            if (_alternatives != null)
            {
                foreach (var sys in _alternatives)
                {
                    var syslink = new SystemLink { Name = sys.name, Id = sys.id, EdsmId = sys.id_edsm, LinkSystem = sys };
                    _systemLinkList.Add(syslink);
                    _systemLinks[sys.id] = syslink;
                }
            }

            if (othersystems != null)
            {
                foreach (var sys in othersystems)
                {
                    if (!_systemLinks.ContainsKey(sys.id))
                    {
                        var syslink = new SystemLink
                        {
                            Name = sys.name,
                            Id = sys.id,
                            EdsmId = sys.id_edsm,
                            LinkSystem = sys
                        };

                        _systemLinkList.Add(syslink);
                        _systemLinks[sys.id] = syslink;
                    }
                }
            }

            this.cbSystemLink.DataSource = _systemLinkList;
            this.cbSystemLink.DisplayMember = "DisplayName";
            this.cbSystemLink.ValueMember = "Id";
            this.cbSystemLink.Refresh();

            if (focus != null && _systemLinks.ContainsKey(focus.id))
            {
                this.cbSystemLink.SelectedItem = _systemLinks[focus.id];
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
                var edsm = new EDDiscovery2.EDSM.EDSMClass();
                string url = edsm.GetUrlToEDSMSystem(_linkSystem.name, _linkSystem.id_edsm);
                System.Diagnostics.Process.Start(url);
            }
        }

        private void btnFindSystem_Click(object sender, EventArgs e)
        {
            string name = tbManualSystemName.Text.ToLower();
            List<SystemClass> systems = SystemClass.GetSystemsByName(name);

            if (systems.Count != 0)
            {
                UpdateLinkedSystemList(systems[0], systems.ToList<ISystem>());
            }
        }
    }
}
