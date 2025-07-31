/*
 * Copyright © 2015 - 2021 EDDiscovery development team
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

namespace EDDiscovery
{
    public partial class EDDiscoveryForm
    {
        private void UpdateProfileComboBox()
        {
            comboBoxCustomProfiles.Enabled = false;                         // and update this box, making sure we don't renter
            comboBoxCustomProfiles.Items.Clear();
            comboBoxCustomProfiles.Items.AddRange(EDDProfiles.Instance.Names());
            comboBoxCustomProfiles.Items.Add("Edit Profiles".Tx());
            comboBoxCustomProfiles.SelectedIndex = EDDProfiles.Instance.IndexOf(EDDProfiles.Instance.Current.Id);
            comboBoxCustomProfiles.Enabled = true;
        }

        private void ChangeToProfileId(int id, bool checksavecur)
        {
            if (!checksavecur || EDDProfiles.Instance.Current.Id != id)
            {
                System.Diagnostics.Debug.WriteLine(BaseUtils.AppTicks.TickCountLap("ProfT") + " *************************** CHANGE To profile " + id);

                if (checksavecur)
                {
                    if (tabControlMain.AllowClose() == false)       // if we don't allow closing, we can't change profile
                        return;
                    else if (PopOuts.AllowClose() == false)
                        return;

                    tabControlMain.CloseSaveTabs();
                    PopOuts.SaveCurrentPopouts();
                }

                PopOuts.CloseAllPopouts();

                comboBoxCustomProfiles.Enabled = false;                         // and update the selection box, making sure we don't trigger a change
                comboBoxCustomProfiles.SelectedIndex = EDDProfiles.Instance.IndexOf(id);
                comboBoxCustomProfiles.Enabled = true;

                EDDProfiles.Instance.ChangeToId(id);

                UserControls.UserControlContainerSplitter.CheckPrimarySplitterControlSettings(false); // use a nonsense name to make sure we just get the default set if we don't have a valid save

                tabControlMain.TabPages.Clear();
                tabControlMain.CreateTabs(this, EDDOptions.Instance.TabsReset, "0, -1,0, 26,0, 27,0, 29,0, 34,0");      // numbers from popouts, which are FIXED!
                tabControlMain.LoadTabs();
                ApplyTheme();

                PopOuts.LoadSavedPopouts();

                System.Diagnostics.Debug.WriteLine(BaseUtils.AppTicks.TickCountLap("ProfT") + " *************************** Finished Profile " + id);
                LogLine(string.Format("Profile {0} Loaded".Tx(), EDDProfiles.Instance.Current.Name));
            }
        }

        public void CheckActionProfile(HistoryEntry he)
        {
            BaseUtils.Variables eventvars = new BaseUtils.Variables();
            Actions.ActionVars.TriggerVars(eventvars, he.journalEntry.EventTypeStr, "JournalEvent");
            Actions.ActionVars.HistoryEventVars(eventvars, he, "Event");     // if HE is null, ignored
            Actions.ActionVars.ShipBasicInformation(eventvars, he?.ShipInformation, "Event");     // if He null, or si null, ignore
            Actions.ActionVars.SystemVars(eventvars, he?.System, "Event");

            int i = EDDProfiles.Instance.ActionOn(eventvars, out string errlist);
            if (i >= 0)
                ChangeToProfileId(i, true);

            if (errlist.HasChars())
                LogLine("Profile reports errors in triggers".Tx()+": "+ errlist);
        }



    }
}
