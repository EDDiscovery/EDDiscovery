/*
 * Copyright 2025 - 2025 EDDiscovery development team
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
 */

using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System.Windows.Forms;

namespace EDDiscovery.UserControls.Colonisation
{
    public partial class ColonisationPortDisplay: UserControl
    {
        public ColonisationPortData Port { get; set; }
        public ColonisationPortDisplay()
        {
            InitializeComponent();
            extLabelFailed.Location = labelDataProgress.Location;
            extLabelFailed.Visible = false;
        }

        public void Initialise(ColonisationPortData port)
        {
            Port = port;
            extCheckBoxShowContributions.CheckedChanged += (s, e) => { extPanelDataGridViewScrollContributions.Visible = extCheckBoxShowContributions.Checked; };
            extCheckBoxShowRL.CheckedChanged += (s, e) => { extPanelDataGridViewScrollRL.Visible = extCheckBoxShowRL.Checked; };
            dataGridViewContributions.SortCompare += DataGridViewContributions_SortCompare;
            dataGridViewRL.SortCompare += DataGridViewRL_SortCompare;
        }

        public void UpdatePort()
        {
            extLabelStationName.Text = Port.Name + " " + Port.MarketID;

            var je = Port.LastDockedOrLocation as ILocDocked; // may be null

            labelDataProgress.Data0 = Port.State?.ConstructionProgress != null ? (double?)(Port.State.ConstructionProgress * 100.0) : null;
            extLabelFailed.Visible = Port.State != null && Port.State.ConstructionFailed == true;
            labelDataProgress.Visible = Port.State != null && Port.State.ConstructionFailed == false;

            labelDataFaction.Data = new object[] { je?.StationFaction,
                                                        je != null ? FactionDefinitions.ToLocalisedLanguage(je.StationFactionState) : null,
            };

            labelDataGov.Data = new object[] { je!=null ? GovernmentDefinitions.ToLocalisedLanguage(je.StationGovernment) : null,
                                               je!=null ? AllegianceDefinitions.ToLocalisedLanguage(je.StationAllegiance) : null,
                                               je?.StationEconomyList != null ? EconomyDefinitions.ToLocalisedLanguage(je.StationEconomyList[0].Name) : null,
            };

            dataGridViewRL.Rows.Clear();

            if (Port.State != null)
            {
                foreach (JournalColonisationConstructionDepot.ResourcesList p in Port.State.ResourcesRequired.EmptyIfNull())
                {
                    dataGridViewRL.Rows.Add(new object[] {MaterialCommodityMicroResourceType.GetTranslatedNameByFDName(p.Name),
                                                            p.RequiredAmount.ToString("N0"),
                                                            p.ProvidedAmount.ToString("N0"),
                                                            (p.RequiredAmount-p.ProvidedAmount).ToString("N0"),
                                                            p.Payment.ToString("N0")});
                }
            }

            extPanelDataGridViewScrollRL.Visible = dataGridViewRL.Rows.Count > 0 && extCheckBoxShowRL.Checked;     // don't show if nothing
            extCheckBoxShowRL.Visible = dataGridViewRL.Rows.Count > 0;      // no button if nothing

            dataGridViewContributions.Rows.Clear();
            foreach(JournalColonisationContribution ct in Port.Contributions )
            {
                foreach( var c in ct.Contributions.EmptyIfNull() )
                {
                    dataGridViewContributions.Rows.Add(new object[] { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ct.EventTimeUTC),
                        MaterialCommodityMicroResourceType.GetTranslatedNameByFDName(c.Name),
                        c.Amount.ToString("N0") });
                }
            }

            extPanelDataGridViewScrollContributions.Visible = dataGridViewContributions.Rows.Count > 0 && extCheckBoxShowContributions.Checked; // don't show if nothing
            extCheckBoxShowContributions.Visible = dataGridViewContributions.Rows.Count > 0;      // no button if nothing
        }

        private void DataGridViewRL_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == ColRLPayment || e.Column == ColRLProvided || e.Column == ColRLRequired || e.Column == ColRLRemaining)
                e.SortDataGridViewColumnNumeric();
        }

        private void DataGridViewContributions_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == colContributionsTime)
                e.SortDataGridViewColumnDate();
            else if (e.Column == colContributionsAmount)
                e.SortDataGridViewColumnNumeric();
        }

    }
}
