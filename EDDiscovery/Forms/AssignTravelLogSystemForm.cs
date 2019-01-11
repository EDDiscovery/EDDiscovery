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
using EliteDangerousCore;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

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
                        return $"{LinkSystem.Name} ({LinkSystem.X:0.00},{LinkSystem.Y:0.00},{LinkSystem.Z:0.00}) #{LinkSystem.EDSMID}";
                    }
                    else
                    {
                        return $"{LinkSystem.Name} #{LinkSystem.EDSMID}";
                    }
                }
            }
        }

        public long AssignedEdsmId { get { return _linkSystem == null ? 0 : _linkSystem.EDSMID; } }
        public ISystem AssignedSystem { get { return _linkSystem; } }

        private ISystem _linkSystem;
        private List<ISystem> _alternatives;
        private string _namestatus;
        private Dictionary<long, SystemLink> _systemLinks;
        private List<SystemLink> _systemLinkList;

        public AssignTravelLogSystemForm(EliteDangerousCore.JournalEvents.JournalLocOrJump vsc)
            : this(new SystemClass { Name = vsc.StarSystem, X = vsc.HasCoordinate ? vsc.StarPos.X : Double.NaN, Y = vsc.HasCoordinate ? vsc.StarPos.Y : Double.NaN, Z = vsc.HasCoordinate ? vsc.StarPos.Z : Double.NaN, EDSMID = vsc.EdsmID, SystemAddress = vsc.SystemAddress }, vsc.EventTimeLocal)
        {
        }

        public AssignTravelLogSystemForm(ISystem refsys, DateTime? visited = null)
        {
            InitializeComponent();
            EliteDangerousCore.EDSM.SystemClassEDSM.CheckSystemAliases();
            SystemClassDB.GetSystemAndAlternatives(refsys, out _linkSystem, out _alternatives, out _namestatus);

            this.tbLogSystemName.Text = refsys.Name;
            this.tbVisitedDate.Text = visited == null ? "-" : visited.ToString();
            this.tbLogCoordX.Text = refsys.HasCoordinate ? refsys.X.ToString("0.00") : "?";
            this.tbLogCoordY.Text = refsys.HasCoordinate ? refsys.Y.ToString("0.00") : "?";
            this.tbLogCoordZ.Text = refsys.HasCoordinate ? refsys.Z.ToString("0.00") : "?";
            this.tbLogCoordX.TextAlign = refsys.HasCoordinate ? HorizontalAlignment.Right : HorizontalAlignment.Center;
            this.tbLogCoordY.TextAlign = refsys.HasCoordinate ? HorizontalAlignment.Right : HorizontalAlignment.Center;
            this.tbLogCoordZ.TextAlign = refsys.HasCoordinate ? HorizontalAlignment.Right : HorizontalAlignment.Center;

            UpdateLinkedSystemList(_linkSystem);
            tbManualSystemName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tbManualSystemName.AutoCompleteSource = AutoCompleteSource.CustomSource;

            tbManualSystemName.SetAutoCompletor(SystemClassDB.ReturnSystemListForAutoComplete);

            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;

            BaseUtils.Translator.Instance.Translate(this);
            theme.ApplyToFormStandardFontSize(this);

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
                    var syslink = new SystemLink { Name = sys.Name, Id = sys.ID, EdsmId = sys.EDSMID, LinkSystem = sys };
                    _systemLinkList.Add(syslink);
                    _systemLinks[sys.ID] = syslink;
                }
            }

            if (othersystems != null)
            {
                foreach (var sys in othersystems)
                {
                    if (!_systemLinks.ContainsKey(sys.ID))
                    {
                        var syslink = new SystemLink
                        {
                            Name = sys.Name,
                            Id = sys.ID,
                            EdsmId = sys.EDSMID,
                            LinkSystem = sys
                        };

                        _systemLinkList.Add(syslink);
                        _systemLinks[sys.ID] = syslink;
                    }
                }
            }

            this.cbSystemLink.DisplayMember = "DisplayName";
            this.cbSystemLink.ValueMember = "Id";
            this.cbSystemLink.DataSource = _systemLinkList;
            this.cbSystemLink.Refresh();

            if (focus != null && _systemLinks.ContainsKey(focus.ID))
            {
                this.cbSystemLink.SelectedItem = _systemLinks[focus.ID];
                UpdateLinkedSystem();
            }
        }

        protected void UpdateLinkedSystem()
        {
            SystemLink selectedItem = (SystemLink)cbSystemLink.SelectedItem;
            _linkSystem = selectedItem.LinkSystem;

            if (selectedItem.LinkSystem != null)
            {
                lblEDSMLink.Text = selectedItem.LinkSystem.Name;
            }
            else
            {
                lblEDSMLink.Text = "";
            }

            if (selectedItem.LinkSystem != null && selectedItem.LinkSystem.HasCoordinate)
            {
                tbSysCoordX.Text = selectedItem.LinkSystem.X.ToString("0.000");
                tbSysCoordY.Text = selectedItem.LinkSystem.Y.ToString("0.000");
                tbSysCoordZ.Text = selectedItem.LinkSystem.Z.ToString("0.000");
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
                var edsm = new EliteDangerousCore.EDSM.EDSMClass();
                string url = edsm.GetUrlToEDSMSystem(_linkSystem.Name, _linkSystem.EDSMID);
                System.Diagnostics.Process.Start(url);
            }
        }

        private void btnFindSystem_Click(object sender, EventArgs e)
        {
            string name = tbManualSystemName.Text.ToLowerInvariant();
            List<ISystem> systems = SystemClassDB.GetSystemsByName(name);

            if (systems.Count != 0)
            {
                UpdateLinkedSystemList(systems[0], systems);
            }
        }
    }
}
