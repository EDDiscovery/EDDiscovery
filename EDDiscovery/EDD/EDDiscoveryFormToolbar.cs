/*
 * Copyright © 2015 - 2022 EDDiscovery development team
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

using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class EDDiscoveryForm
    {
        public void UpdateCommandersListBox()
        {
            comboBoxCommander.Enabled = false;
            comboBoxCommander.Items.Clear();            // comboBox is nicer with items

            if (EDDOptions.Instance.DisableCommanderSelect) // debug only 
            {
                comboBoxCommander.Items.Add("Jameson");
                comboBoxCommander.SelectedIndex = 0;
            }
            else
            {
                var names = EDCommander.GetListActiveHiddenCommanders().Select(x => x.Name);
                comboBoxCommander.Items.AddRange(names);
                comboBoxCommander.SelectedItem = EDCommander.Current.Name;
                comboBoxCommander.Enabled = true;
            }
        }

        private void comboBoxCommander_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCommander.SelectedIndex >= 0 && comboBoxCommander.Enabled)     // DONT trigger during LoadCommandersListBox
            {
                var itm = (from EDCommander c in EDCommander.GetListActiveHiddenCommanders() where c.Name.Equals(comboBoxCommander.Text) select c).ToList();
                ChangeToCommander(itm[0].Id);
            }
        }

        public void RefreshButton(bool state)
        {
            buttonExtRefresh.Enabled = state;
        }

        private void buttonExtRefresh_Click(object sender, EventArgs e)
        {
            LogLine("Refresh History.".Tx());
            RefreshHistoryAsync();
        }

        private void sendUnsyncedEDSMJournalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EDSMClass edsm = new EDSMClass();

            if (!edsm.ValidCredentials)
            {
                ExtendedControls.MessageBoxTheme.Show(this, "No EDSM API key set".Tx());
                return;
            }

            if (!EDCommander.Current.SyncToEdsm)
            {
                string dlgtext = "You have disabled sync to EDSM for this commander.  Are you sure you want to send unsynced events to EDSM?".Tx();
                string dlgcapt = "Confirm EDSM sync".Tx();

                if (ExtendedControls.MessageBoxTheme.Show(this, dlgtext, dlgcapt, MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            EDSMSend();
        }

        private void ComboBoxCustomProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxCustomProfiles.SelectedIndex >= 0 && comboBoxCustomProfiles.Enabled)
            {
                if (comboBoxCustomProfiles.SelectedIndex == comboBoxCustomProfiles.Items.Count - 1)    // last one if edit profiles
                {
                    Forms.ProfileEditor pe = new ProfileEditor();
                    pe.Init(EDDProfiles.Instance, this.Icon);
                    if (pe.ShowDialog() == DialogResult.OK)
                    {
                        bool removedcurprofile = EDDProfiles.Instance.UpdateProfiles(pe.Result, pe.PowerOnIndex);       // see if the current one has changed...

                        if (removedcurprofile)
                            ChangeToProfileId(EDDProfiles.DefaultId, false);
                    }

                    UpdateProfileComboBox();
                }
                else
                {
                    ChangeToProfileId(EDDProfiles.Instance.IdOfIndex(comboBoxCustomProfiles.SelectedIndex), true);
                }
            }
        }

        private void buttonExtPopOut_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm selection = new ExtendedControls.CheckedIconNewListBoxForm();

            foreach( var pi in PanelInformation.GetUserSelectablePanelInfo(EDDConfig.Instance.SortPanelsByName))
            {
                selection.UC.AddButton("null", pi.Description, pi.TabIcon, usertag: pi.PopoutID);
            }

            selection.CloseBoundaryRegion = new Size(32, 32);
            selection.UC.MultiColumnSlide = true;
            selection.UC.ImageSize = ExtendedControls.Theme.Current.IconSize;

            System.Diagnostics.Debug.WriteLine($"{buttonExtEditAddOns.Size} {selection.UC.ImageSize}");
            selection.PositionBelow(buttonExtPopOut);
            selection.UC.ButtonPressed += (index, tag, text, usertag, barg) =>
            {
                PopOuts.PopOut((PanelInformation.PanelIDs)usertag);
            };

            selection.Show(this);
        }

        private void extButtonDrawnHelp_Click(object sender, EventArgs e)
        {
            tabControlMain.HelpOn(this, extButtonDrawnHelp.PointToScreen(new Point(0, extButtonDrawnHelp.Bottom)), tabControlMain.SelectedIndex);
        }

        private void buttonReloadActions_Click(object sender, EventArgs e)
        {
            actioncontroller.ReLoad();
            actioncontroller.CreatePanelsFromActionFiles();
            actioncontroller.CheckWarn();
            actioncontroller.onStartup();
            Controller.ResetUIStatus();

            // keep for debug:

            //var tx = BaseUtils.Translator.Instance.NotUsed();  foreach (var s in tx) System.Diagnostics.Debug.WriteLine(s); // turn on usetracker at top to use

            //if (FrontierCAPI.Active && !EDCommander.Current.ConsoleCommander)
            //  Controller.DoCAPI(history.GetLast.Status.StationName, history.GetLast.System.Name, false, history.Shipyards.AllowCobraMkIV);
        }

        private void extButtonCAPI_Click(object sender, EventArgs e)
        {
            var he = History.GetLast;
            if (he != null && he.Status.IsDocked)
                Controller.DoCAPI(he.WhereAmI, he.System.Name, History.Shipyards.AllowCobraMkIV);
        }

        private void extButtonSingleStep_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Controller.RunDebugger(1);
            }
            else if (e.Button == MouseButtons.Right)
            {
                contextMenuStripDebugger.Show(extButtonSingleStep.PointToScreen(new Point(extButtonSingleStep.Width, 0)));
            }

        }

        private void debuggerSingleJournalEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Controller.RunDebugger(0);

        }

        private void debuggerNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem c = sender as ToolStripMenuItem;
            if ( Enum.TryParse<JournalTypeEnum>(c.Text.Substring(5), out JournalTypeEnum eventtype))
            {
                var list = new JournalTypeEnum[] { eventtype, JournalTypeEnum.Shutdown };
                Controller.RunDebugger(20000, (x) => list.Contains(x.EntryType) == false );
            }
        }

        private void nextEntryForColonisationDataColonisationDockedFSDJumpLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // not fsdjump, does not add much JournalTypeEnum.FSDJump

            var list = new JournalTypeEnum[] { JournalTypeEnum.Location, JournalTypeEnum.Docked,
                JournalTypeEnum.ColonisationBeaconDeployed, JournalTypeEnum.ColonisationConstructionDepot, JournalTypeEnum.ColonisationContribution,
                 JournalTypeEnum.ColonisationSystemClaim, JournalTypeEnum.ColonisationSystemClaimRelease};
            Controller.RunDebugger(20000, (x) =>
            {
                if (list.Contains(x.EntryType))
                {
                    ILocDocked il = x.journalEntry as ILocDocked;       // if its a loc dock
                    if (il != null )                                    // continue if not docked or not a colonisation station
                        return il.Docked == false || il.MarketClass() < StationDefinitions.Classification.TypesBelowColonisation; // only colonisation class stations count
                    else
                        return false;
                }

                return true;
            });
        }

        private void stepEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem c = sender as ToolStripMenuItem;
            var amount = c.Text.InvariantParseInt(30);
            Controller.RunDebugger(amount);
        }

        private void stepTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var starttime = History.GetLast?.EventTimeUTC;
            if (starttime != null)
            {
                ToolStripMenuItem c = sender as ToolStripMenuItem;
                var tvalue = c.Text.Substring(0, c.Text.IndexOf(' '));
                var amount = tvalue.InvariantParseInt(30);
                if (c.Text.Contains("day"))
                    amount *= 60 * 24;
                var endtime = starttime.Value.AddMinutes(amount);
                System.Diagnostics.Debug.WriteLine($"Debugger run time {amount} mins {starttime} - {endtime}");
                Controller.RunDebugger(20000, (x) => x.EventTimeUTC < endtime);
            }
        }


        private void extButtonStop_Click(object sender, EventArgs e)
        {
            Controller.DebuggerStop();
        }

    }
}
