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
    public partial class ColonisationSystemDisplay: UserControl
    {
        public ColonisationSystemData SystemData { get; set; }

        public bool ScanDisplayVisible { get { return scanDisplayUserControl.Visible; } }

        public ColonisationSystemDisplay()
        {
            InitializeComponent();
        }

        public void Initialise(ColonisationSystemData systemdata, HistoryList hl)
        {
            SystemData = systemdata;

            EliteDangerousCore.DB.UserDatabaseSettingsSaver db = new EliteDangerousCore.DB.UserDatabaseSettingsSaver(EliteDangerousCore.DB.UserDatabase.Instance, "ColonisationUC_");
            edsmSpanshButton.Init(db, "EDSMSpansh", "");

            labelDataName.Data0 = SystemData.System.Name;

            extCheckBoxSystemShow.CheckedChanged += (s, e) => {
                scanDisplayConfigureButton.Visible =
                    edsmSpanshButton.Visible = scanDisplayBodyFiltersButton.Visible = scanDisplayUserControl.Visible = extCheckBoxSystemShow.Checked;
            };

            var sd = scanDisplayUserControl;               // done to keep the code the same as scandisplayform
            var filterbut = scanDisplayBodyFiltersButton;
            var configbut = scanDisplayConfigureButton;

            StarScan.SystemNode nodedata = null;

            edsmSpanshButton.ValueChanged += (s, e) =>
            {
                nodedata = hl.StarScan.FindSystemSynchronous(SystemData.System, edsmSpanshButton.WebLookup);    // look up system, unfort must be sync due to limitations in c#
                sd.SystemDisplay.ShowWebBodies = edsmSpanshButton.WebLookup != WebExternalDataLookup.None;
                sd.DrawSystem(nodedata, null, hl.MaterialCommoditiesMicroResources.GetLast(), filter: filterbut.BodyFilters);
            };

            scanDisplayUserControl.SystemDisplay.ShowWebBodies = edsmSpanshButton.WebLookup != WebExternalDataLookup.None;
            int selsize = (int)(ExtendedControls.Theme.Current.GetFont.Height / 10.0f * 48.0f);
            sd.SystemDisplay.SetSize(selsize);

            filterbut.Init(db, "BodyFilter");
            filterbut.Image = EDDiscovery.Icons.Controls.EventFilter;
            filterbut.ValueChanged += (s, e) =>
            {
                sd.DrawSystem(nodedata, null, hl.MaterialCommoditiesMicroResources.GetLast(), filter: filterbut.BodyFilters);
            };

            configbut.Init(db, "DisplayFilter");
            configbut.Image = EDDiscovery.Icons.Controls.DisplayFilters;
            configbut.ValueChanged += (s, e) =>
            {
                configbut.ApplyDisplayFilters(sd);
                sd.DrawSystem(nodedata, null, hl.MaterialCommoditiesMicroResources.GetLast(), filter: filterbut.BodyFilters);
            };
            
            configbut.ApplyDisplayFilters(sd);

        }

        public void UpdateSystem()
        {
            labelDataPosition.Data = new object[] { SystemData.System.X, SystemData.System.Y, SystemData.System.Z };

            var je = SystemData.LastLocOrJump?.journalEntry as JournalLocOrJump; // may be null

            labelDataFaction.Data = new object[] { je?.Faction, 
                                                        je != null ? FactionDefinitions.ToLocalisedLanguage(je.FactionState) : null,
            };

            labelDataGov.Data = new object[] { je!=null ? GovernmentDefinitions.ToLocalisedLanguage(je.Government) : null,
                                               je!=null ? AllegianceDefinitions.ToLocalisedLanguage(je.Allegiance) : null,
                                               je != null ? EconomyDefinitions.ToLocalisedLanguage(je.Economy) : null,
                                               je != null ? SecurityDefinitions.ToLocalisedLanguage(je.Security) : null,
            };
            
            extLabelClaimReleased.Visible = SystemData.ClaimReleased;
            extLabelBeaconDeployed.Visible = SystemData.BeaconDeployed;
        }

        public async void UpdateSystemDiagramAsync(HistoryList hl)
        {
            StarScan.SystemNode nodedata = await hl.StarScan.FindSystemAsync(SystemData.System, edsmSpanshButton.WebLookup);    // look up system async
            scanDisplayUserControl.DrawSystem(nodedata, null, hl.MaterialCommoditiesMicroResources.GetLast(), filter: scanDisplayBodyFiltersButton.BodyFilters);
        }

    }
}
