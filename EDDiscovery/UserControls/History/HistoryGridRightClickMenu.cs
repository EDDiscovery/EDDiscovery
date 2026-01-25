/*
 * Copyright 2016 - 2026 EDDiscovery development team
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

using BaseUtils;
using EliteDangerousCore;
using EliteDangerousCore.EDDN;
using EliteDangerousCore.EDSM;
using QuickJSON;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class HistoryGrid
    {
        #region TravelHistoryRightClick

        // uses rightclickhe/leftclickhe from Right/Left Clicks on Grid

        private void historyContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewTravel.SelectedCells.Count == 0)      // need something selected  stops context menu opening on nothing..
                e.Cancel = true;

            if (rightclickhe != null)
            {
                if (rightclickhe.StartMarker)
                {
                    toolStripMenuItemStartStop.Text = "Clear Start marker".Tx();
                }
                else if (rightclickhe.StopMarker)
                {
                    toolStripMenuItemStartStop.Text = "Clear Stop marker".Tx();
                }
                else if (rightclickhe.isTravelling)
                {
                    toolStripMenuItemStartStop.Text = "Set Stop marker for travel calculations".Tx();
                }
                else
                {
                    toolStripMenuItemStartStop.Text = "Set Start marker for travel calculations".Tx();
                }
            }

            toolStripMenuItemStartStop.Visible = rightclickhe != null;
            quickMarkToolStripMenuItem.Visible = rightclickhe != null;
            quickMarkToolStripMenuItem.Checked = rightclickhe != null && quickMarkJIDs.Contains(rightclickhe.journalEntry.Id);              // set the check 
            mapGotoStartoolStripMenuItem.Visible = (rightclickhe != null && rightclickhe.System.HasCoordinate);
            viewOnEDSMToolStripMenuItem.Visible = (rightclickhe != null);
            viewOnSpanshToolStripMenuItem.Visible = (rightclickhe != null);
            viewScanDisplayToolStripMenuItem.Visible = (rightclickhe != null);
            toolStripMenuItemStartStop.Visible = (rightclickhe != null);
            removeJournalEntryToolStripMenuItem.Visible = (rightclickhe != null);
            runActionsOnThisEntryToolStripMenuItem.Visible = (rightclickhe != null);
            setNoteToolStripMenuItem.Visible = (rightclickhe != null);
            copyJournalEntryToClipboardToolStripMenuItem.Visible = (rightclickhe != null);
            createEditBookmarkToolStripMenuItem.Visible = (rightclickhe != null);
            gotoEntryNumberToolStripMenuItem.Visible = dataGridViewTravel.Rows.Count > 0;
            removeSortingOfColumnsToolStripMenuItem.Visible = dataGridViewTravel.SortedColumn != null;
            gotoNextStartStopMarkerToolStripMenuItem.Visible = (rightclickhe != null);

            openInNotepadTheJournalFileToolStripMenuItem.Visible =
            writeEventInfoToLogDebugToolStripMenuItem.Visible = EDDOptions.Instance.EnableTGRightDebugClicks && rightclickhe != null;
            openInNotepadTheJournalFileToolStripMenuItem.Enabled = rightclickhe?.journalEntry.FullPath.HasChars() ?? false;

            runSelectionThroughEDDNThruTestToolStripMenuItem.Visible = EDDOptions.Instance.EnableTGRightDebugClicks && rightclickhe != null && EDDNClass.IsEDDNMessage(rightclickhe.EntryType);

            runActionsAcrossSelectionToolSpeechStripMenuItem.Visible =
            runSelectionThroughInaraSystemToolStripMenuItem.Visible =
            runEntryThroughProfileSystemToolStripMenuItem.Visible =
            runSelectionThroughEDDNThruTestToolStripMenuItem.Visible =
            sendJournalEntriesToDLLsToolStripMenuItem.Visible =
            travelGridInDebugModeToolStripMenuItem.Visible =
            runSelectionThroughEDAstroDebugToolStripMenuItem.Visible = EDDOptions.Instance.EnableTGRightDebugClicks;
        }

        private void removeSortingOfColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Display(current_historylist, true);
        }

        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiscoveryForm.Open3DMap(rightclickhe?.System);
        }

        private void starMapColourToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                           .Select(cell => cell.OwningRow)
                                                           .Distinct();
            ColorDialog mapColorDialog = new ColorDialog();
            mapColorDialog.AllowFullOpen = true;
            mapColorDialog.FullOpen = true;
            HistoryEntry sp2 = (HistoryEntry)selectedRows.First().Tag;
            mapColorDialog.Color = Color.Red;

            if (mapColorDialog.ShowDialog(FindForm()) == DialogResult.OK)
            {
                foreach (DataGridViewRow r in selectedRows)
                {
                    HistoryEntry sp = (HistoryEntry)r.Tag;
                    System.Diagnostics.Debug.Assert(sp != null);
                    sp.journalEntry.UpdateMapColour(mapColorDialog.Color.ToArgb());
                }

                Display(current_historylist, false);
            }
        }

        private void hideSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => cell.OwningRow)
                .Distinct();

            foreach (DataGridViewRow r in selectedRows)
            {
                HistoryEntry sp = (HistoryEntry)r.Tag;
                System.Diagnostics.Debug.Assert(sp != null);
                sp.journalEntry.UpdateCommanderID(-1);
                rowsbyjournalid.Remove(sp.Journalid);
            }

            // Remove rows
            if (selectedRows.Count<DataGridViewRow>() == dataGridViewTravel.Rows.Count)
            {
                dataGridViewTravel.Rows.Clear();
                rowsbyjournalid.Clear();
            }
            else
            {
                foreach (DataGridViewRow row in selectedRows.ToList<DataGridViewRow>())
                {
                    dataGridViewTravel.Rows.Remove(row);
                }
            }
        }

        private void moveToAnotherCommanderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                .Select(cell => cell.OwningRow)
                .Distinct();

            List<HistoryEntry> listsyspos = new List<HistoryEntry>();

            foreach (DataGridViewRow r in selectedRows)
            {
                HistoryEntry sp = (HistoryEntry)r.Tag;
                System.Diagnostics.Debug.Assert(sp != null);
                listsyspos.Add(sp);
                rowsbyjournalid.Remove(sp.Journalid);
            }

            EDDiscovery.Forms.MoveToCommander movefrm = new EDDiscovery.Forms.MoveToCommander();

            movefrm.Init();

            DialogResult red = movefrm.ShowDialog(FindForm());
            if (red == DialogResult.OK)
            {
                foreach (HistoryEntry sp in listsyspos)
                {
                    sp.journalEntry.UpdateCommanderID(movefrm.SelectedCommander.Id);
                }

                foreach (DataGridViewRow row in selectedRows)
                {
                    dataGridViewTravel.Rows.Remove(row);
                }
            }
        }

        private void trilaterationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSystemToOthers(dist: true);
        }

        private void wantedSystemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSystemToOthers(wanted: true);
        }

        private void bothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSystemToOthers(dist: true, wanted: true);
        }

        private void expeditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSystemToOthers(expedition: true);
        }

        private void AddSystemToOthers(bool dist = false, bool wanted = false, bool expedition = false, bool exploration = false)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewTravel.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            List<ISystem> systemlist = new List<ISystem>();

            string lastname = "";
            foreach (DataGridViewRow r in selectedRows)
            {
                HistoryEntry sp = (HistoryEntry)r.Tag;

                if (!sp.System.Name.Equals(lastname))
                {
                    lastname = sp.System.Name;
                    systemlist.Add(sp.System);
                }
            }

            if (dist)
                RequestPanelOperationOpen(PanelInformation.PanelIDs.Trilateration, new UserControlCommonBase.PushStars() { PushTo = UserControlCommonBase.PushStars.PushType.TriSystems, SystemList = systemlist, MakeVisible = true });

            if (wanted)
                RequestPanelOperationOpen(PanelInformation.PanelIDs.Trilateration, new UserControlCommonBase.PushStars() { PushTo = UserControlCommonBase.PushStars.PushType.TriWanted, SystemList = systemlist, MakeVisible = true });

            if (expedition)
                RequestPanelOperationOpen(PanelInformation.PanelIDs.Expedition, new UserControlCommonBase.PushStars() { PushTo = UserControlCommonBase.PushStars.PushType.Expedition, SystemList = systemlist, MakeVisible = true });

        }
        private void viewScanDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), rightclickhe, DiscoveryForm.History);
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EDSMClass edsm = new EDSMClass();
            if (!edsm.ShowSystemInEDSM(rightclickhe.System.Name))
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable".Tx());
        }

        private void viewOnSpanshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickhe.System.SystemAddress.HasValue)
                EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForSystem(rightclickhe.System.SystemAddress.Value);
        }

        private void removeJournalEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string warning = ("Confirm you wish to remove this entry" + Environment.NewLine + "It may reappear if the logs are rescanned").Tx();
            if (ExtendedControls.MessageBoxTheme.Show(FindForm(), warning, "Warning".Tx(), MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                JournalEntry.Delete(rightclickhe.Journalid);
                DiscoveryForm.RefreshHistoryAsync();
            }
        }

        private void setNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditNoteInWindow(rightclickhe);
        }

        private void EditNoteInWindow(HistoryEntry he)
        {
            using (Forms.SetNoteForm noteform = new Forms.SetNoteForm(he))
            {
                if (noteform.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    System.Diagnostics.Trace.Assert(noteform.NoteText != null && he.System != null);
                    he.journalEntry.UpdateSystemNote(noteform.NoteText, he.System.Name, EDCommander.Current.SyncToEdsm);
                    DiscoveryForm.NoteChanged(this, he);
                }
            }
        }

        private void writeEventInfoToLogDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // copies the work of action run for he's
            BaseUtils.Variables eventvars = new BaseUtils.Variables();
            var ev = Actions.ActionEventEDList.NewEntry(rightclickhe);
            Actions.ActionVars.TriggerVars(eventvars, ev.TriggerName, ev.TriggerType);
            Actions.ActionVars.HistoryEventVars(eventvars, rightclickhe, "Event");     // if HE is null, ignored
            Actions.ActionVars.ShipBasicInformation(eventvars, rightclickhe?.ShipInformation, "Event");     // if He null, or si null, ignore
            Actions.ActionVars.SystemVars(eventvars, rightclickhe?.System, "Event");
            DiscoveryForm.LogLine(eventvars.ToString(separ: Environment.NewLine));
        }

        private void copyJournalEntryToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string json = rightclickhe.journalEntry.GetJsonString();
            if (json != null)
            {
                SetClipboardText(json);
                DiscoveryForm.LogLine(json);
            }
        }

        private void createEditBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookmarkHelpers.ShowBookmarkForm(DiscoveryForm, DiscoveryForm, rightclickhe.System, null);
        }

        private void gotoEntryNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int curi = rightclickhe != null ? (EDDConfig.Instance.OrderRowsInverted ? rightclickhe.EntryNumber : (DiscoveryForm.History.Count - rightclickhe.EntryNumber + 1)) : 0;
            int selrow = dataGridViewTravel.JumpToDialog(this.FindForm(), curi, r =>
            {
                HistoryEntry he = r.Tag as HistoryEntry;
                return EDDConfig.Instance.OrderRowsInverted ? he.EntryNumber : (DiscoveryForm.History.Count - he.EntryNumber + 1);
            });

            if (selrow >= 0)
            {
                dataGridViewTravel.ClearSelection();
                dataGridViewTravel.SetCurrentAndSelectAllCellsOnRow(selrow);
                FireChangeSelection();
            }
        }

        private void toolStripMenuItemStartStop_Click(object sender, EventArgs e)       // sync with Journal Grid call
        {
            this.dataGridViewTravel.Cursor = Cursors.WaitCursor;

            rightclickhe.SetStartStop();                                        // change flag
            DiscoveryForm.History.RecalculateTravel();                          // recalculate all

            foreach (DataGridViewRow row in dataGridViewTravel.Rows)            // dgv could be in any sort order, we have to do the lot
            {
                HistoryEntry he = row.Tag as HistoryEntry;
                if (he.IsFSD || he.StopMarker || he == rightclickhe)
                {
                    row.Cells[ColumnInformation.Index].Value = he.GetInfo();
                    row.Cells[ColumnInformation.Index].Style.WrapMode = DataGridViewTriState.NotSet;        // in case it was in expanded state (see cell double click)
                    row.Cells[ColumnInformation.Index].Tag = null;
                }
            }

            dataGridViewTravel.Refresh();       // to make the start/stop marker appear, refresh

            RequestPanelOperation(this, new TravelHistoryStartStopChanged());        // tell others

            this.dataGridViewTravel.Cursor = Cursors.Default;
        }

        private void gotoNextStartStopMarkerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int rown = dataGridViewTravel.RightClickRow + 1; rown < dataGridViewTravel.Rows.Count; rown++)
            {
                DataGridViewRow r = dataGridViewTravel.Rows[rown];
                HistoryEntry h = r.Tag as HistoryEntry;
                if (h.StartMarker || h.StopMarker)
                {
                    if (r.Visible)
                    {
                        dataGridViewTravel.DisplayRow(r.Index, true);
                        dataGridViewTravel.ClearSelection();
                        dataGridViewTravel.Rows[r.Index].Selected = true;
                        return;
                    }
                    else
                    {
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), "Next start/stop marker is hidden".Tx());
                    }
                    break;
                }
            }

            ExtendedControls.MessageBoxTheme.Show(FindForm(), "No start/stop marker found below".Tx());
        }

        // quickmarks.
        private void quickMarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (quickMarkToolStripMenuItem.Checked)
                quickMarkJIDs.Add(rightclickhe.journalEntry.Id);
            else
                quickMarkJIDs.Remove(rightclickhe.journalEntry.Id);

            var str = quickMarkJIDs.Select(x => x.ToStringInvariant()).ToArray().Join(';');
            PutSetting(dbBookmarks + ":" + DiscoveryForm.History.CommanderId.ToStringInvariant(), str);
            UpdateQuickMarkComboBox();
        }

        private void UpdateQuickMarkComboBox()
        {
            // quick marks are commander dependent
            var str = GetSetting(dbBookmarks + ":" + DiscoveryForm.History.CommanderId.ToStringInvariant(), "").Split(';');
            quickMarkJIDs = str.Select(x => x.InvariantParseLong(-1)).ToList().ToHashSet();

            extComboBoxQuickMarks.Items.Clear();
            List<long> jids = new List<long>();
            foreach (var j in quickMarkJIDs)
            {
                if (rowsbyjournalid.TryGetValue(j, out DataGridViewRow row)) // if it parses and its in view, add it to combo box.
                {
                    extComboBoxQuickMarks.Items.Add((string)row.Cells[0].Value + ":" + (string)row.Cells[2].Value);
                    jids.Add(j);
                }
            }
            extComboBoxQuickMarks.Tag = jids;
            extComboBoxQuickMarks.Text = "Marked".Tx();      // only works for custom
        }

        #endregion


        #region DEBUG clicks - only for special people who build the debug version!

        private void runSelectionThroughInaraSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickhe != null)
            {
                var mcmr = DiscoveryForm.History.MaterialCommoditiesMicroResources.GetDict(rightclickhe.MaterialCommodity);
                List<QuickJSON.JToken> list = EliteDangerousCore.Inara.InaraSync.NewEntryList(rightclickhe, DateTime.UtcNow, mcmr);

                foreach (var j in list)
                {
                    j["eventTimestamp"] = DateTime.UtcNow.ToStringZulu();       // mangle time to now to allow it to send.
                    if (j["eventName"].Str() == "addCommanderMission")
                    {
                        j["eventData"]["missionExpiry"] = DateTime.UtcNow.AddDays(1).ToStringZulu();       // mangle mission time to now to allow it to send.
                    }
                    if (j["eventName"].Str() == "setCommunityGoal")
                    {
                        j["eventData"]["goalExpiry"] = DateTime.UtcNow.AddDays(5).ToStringZulu();       // mangle expiry time
                    }
                }

                string json = rightclickhe.journalEntry.GetJsonString();
                DiscoveryForm.LogLine(json);

                EliteDangerousCore.Inara.InaraClass inara = new EliteDangerousCore.Inara.InaraClass(EDCommander.Current);
                string str = inara.ToJSONString(list);
                DiscoveryForm.LogLine(str);
                //System.IO.File.WriteAllText(@"c:\code\inaraentry.json", str);

                if (list.Count > 0)
                {
                    string strres = inara.Send(list);
                    DiscoveryForm.LogLine(strres);
                }
                else
                    DiscoveryForm.LogLine("No Events");
            }
        }

        private void runEntryThroughProfileSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiscoveryForm.CheckActionProfile(rightclickhe);
        }

        private void runSelectionThroughEDDNDebugNoSendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickhe != null)
            {
                EDDNSync.SendToEDDN(rightclickhe, true);
            }

        }
        private void runSelectionThroughEDAstroDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickhe != null)
            {
                EliteDangerousCore.EDAstro.EDAstroSync.SendEDAstroEvents(new List<HistoryEntry>() { rightclickhe });
            }
        }

        private void sendJournalEntriesToDLLsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclickhe != null)
            {
                DiscoveryForm.DLLManager.NewJournalEntry(EliteDangerousCore.DLL.EDDDLLCallerHE.CreateFromHistoryEntry(DiscoveryForm.History, rightclickhe), true);
            }

        }
        private void travelGridInDebugModeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting(dbDebugMode, travelGridInDebugModeToolStripMenuItem.Checked);
            Display(current_historylist, false);
        }

        private void runActionsAcrossSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string laststring = "";
            string lasttype = "";
            int lasttypecount = 0;

            DiscoveryForm.ActionController.AsyncMode = false;     // to force it to do all the action code before returning..

            if (dataGridViewTravel.SelectedRows.Count > 0)
            {
                List<DataGridViewRow> rows = (from DataGridViewRow x in dataGridViewTravel.SelectedRows where x.Visible orderby x.Index select x).ToList();
                foreach (DataGridViewRow rw in rows)
                {
                    HistoryEntry he = rw.Tag as HistoryEntry;
                    // System.Diagnostics.Debug.WriteLine("Row " + rw.Index + " " + he.EventSummary + " " + he.EventDescription);


                    bool same = he.journalEntry.EventTypeStr.Equals(lasttype);
                    if (!same || lasttypecount < 10)
                    {
                        lasttype = he.journalEntry.EventTypeStr;
                        lasttypecount = (same) ? ++lasttypecount : 0;

                        DiscoveryForm.ActionController.SetPeristentGlobal("GlobalSaySaid", "");
                        BaseUtils.FunctionHandlers.SetRandom(new Random(rw.Index + 1));
                        DiscoveryForm.ActionRunOnEntry(he, Actions.ActionEventEDList.UserRightClick(he));

                        string json = he.journalEntry.GetJsonString();

                        string s = DiscoveryForm.ActionController.Globals["GlobalSaySaid"];

                        if (s.Length > 0 && !s.Equals(laststring))
                        {
                            //System.Diagnostics.Debug.WriteLine("Call ts(j='" + json?.Replace("'", "\\'") + "',s='" + s.Replace("'", "\\'") + "',r=" + (rw.Index + 1).ToStringInvariant() + ")");
                            laststring = s;
                        }
                    }
                }
            }
        }

        private void openInNotepadTheJournalFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = rightclickhe.journalEntry.FullPath;
            if (file.HasChars())
            {
                string find = $"{{ \"timestamp\":\"{rightclickhe.EventTimeUTC.ToStringZulu()}\", \"event\":\"{rightclickhe.journalEntry.EventTypeID.ToString()}\"";
                Processes.OpenEditorForTextFileAtText(file, find);
            }
        }

        private void runActionsOnThisEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiscoveryForm.ActionRunOnEntry(rightclickhe, Actions.ActionEventEDList.UserRightClick(rightclickhe));
        }

        #endregion

    }
}
