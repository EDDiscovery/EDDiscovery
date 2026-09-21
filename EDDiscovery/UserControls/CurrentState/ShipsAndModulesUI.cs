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

using EliteDangerousCore;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class ShipsAndModules 
    {

        #region UI

        private void comboBoxHistoryWindow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxShips.Enabled)
            {
                PutSetting(dbShipSelect, comboBoxShips.Text);
                Display();
            }
        }

        private void extButtonShowControl_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new ExtendedControls.CheckedIconNewListBoxForm();
            displayfilter.AllOrNoneBack = false;

            // not yet as only one item. displayfilter.UC.AddAllNone();
            displayfilter.UC.Add("fullblueprint", "Show full blueprint information".Tx());
            displayfilter.UC.Add("engineeredvalues", "Show Engineered Values".Tx());

            displayfilter.UC.ImageSize = new Size(24, 24);
            displayfilter.SaveSettings = (s, o) =>
            {
                displayfilters = s.Split(';');
                PutSetting(dbDisplayFilters, string.Join(";", displayfilters));
                Display();
            };
            displayfilter.CloseBoundaryRegion = new Size(32, extButtonShowControl.Height);
            displayfilter.Show(string.Join(";", displayfilters), extButtonShowControl, this.FindForm());
        }

        private void extButtonDrawnResetPips_Click(object sender, EventArgs e)
        {
            multiPipControlEng.Value = multiPipControlSys.Value = multiPipControlWep.Value = 4;
        }

        private void buttonExtCoriolis_Click(object sender, EventArgs e)
        {
            if (last_displayship != null)
            {
                string coriolis = last_displayship.JSONCoriolis(out string errstr).ToString();

                if (errstr.Length > 0)
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), errstr + Environment.NewLine + "This is probably a new or powerplay module" + Environment.NewLine + "Report to EDD Team by Github giving the full text above", "Unknown Module Type");

                System.Diagnostics.Debug.WriteLine("Coriolis Export " + last_displayship.JSONCoriolis(out string error).ToString(true));

                string uri = EDDConfig.Instance.CoriolisURL + "data=" + coriolis.URIGZipBase64Escape() + "&bn=" + Uri.EscapeDataString(last_displayship.Name);

                if (!BaseUtils.BrowserInfo.LaunchBrowser(uri))
                {
                    ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                    info.Info("Cannot launch browser, use this JSON for manual Coriolis import", FindForm().Icon, coriolis);
                    info.ShowDialog(FindForm());
                }
            }
        }

        private void buttonExtCoriolis_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string url = ExtendedControls.PromptSingleLine.ShowDialog(this.FindForm(), "URL:", EDDConfig.Instance.CoriolisURL,
                            "Enter Coriolis URL".Tx(), this.FindForm().Icon, requireinput: true);
                if (url != null)
                    EDDConfig.Instance.CoriolisURL = url;
            }
        }

        private void buttonExtEDShipyard_Click(object sender, EventArgs e)
        {
            if (last_displayship != null)
            {
                string loadoutjournalline = last_displayship.JSONLoadout(false).ToString();

                string uri = EDDConfig.Instance.EDDShipyardURL + "#/I=" + loadoutjournalline.URIGZipBase64Escape();

                if (!BaseUtils.BrowserInfo.LaunchBrowser(uri))
                {
                    ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                    info.Info("Cannot launch browser, use this JSON for manual ED Shipyard import", FindForm().Icon, loadoutjournalline);
                    info.ShowDialog(FindForm());
                }
            }
        }

        private void buttonExtEDShipyard_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string url = ExtendedControls.PromptSingleLine.ShowDialog(this.FindForm(), "URL:", EDDConfig.Instance.EDDShipyardURL, "Enter ED Shipyard URL".Tx(), this.FindForm().Icon, requireinput: true);
                if (url != null)
                    EDDConfig.Instance.EDDShipyardURL = url;
            }
        }

        private void buttonExtConfigure_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.Assert(last_displayship != null);           // must be set for this configure button to be visible

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            int width = 430;
            int ctrlleft = 150;

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("L", typeof(Label), "Fuel Warning".Tx() + ": ", new Point(10, 40), new Size(140, 24), ""));
            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("FuelWarning", typeof(ExtendedControls.NumberBoxDouble),
                last_displayship.FuelWarningPercent.ToString(), new Point(ctrlleft, 40), new Size(width - ctrlleft - 20, 24), "Enter fuel warning level in % (0 = off, 1-100%)".Tx())
            { NumberBoxDoubleMinimum = 0, NumberBoxDoubleMaximum = 100, NumberBoxFormat = "0.##" });

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("Sell", typeof(ExtendedControls.ExtButton), "Force Sell".Tx(), new Point(10, 80), new Size(80, 24), null));

            f.AddOK(new Point(width - 100, 110), "Press to Accept".Tx());
            f.AddCancel(new Point(width - 200, 110), "Press to Cancel".Tx());

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "OK")
                {
                    double? v3 = f.GetDouble("FuelWarning");
                    if (v3.HasValue)
                    {
                        f.ReturnResult(DialogResult.OK);
                    }
                    else
                        ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "A Value is not valid".Tx(), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (controlname == "Cancel" || controlname == "Close")
                {
                    f.ReturnResult(DialogResult.Cancel);
                }
                else if (controlname == "Sell")
                {
                    if (ExtendedControls.MessageBoxTheme.Show(FindForm(), "Confirm sell of ship".Tx() + ": " + Environment.NewLine + last_displayship.ShipNameIdentType, "Warning".Tx(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        var je = new EliteDangerousCore.JournalEvents.JournalShipyardSell(DateTime.UtcNow, last_displayship.ShipFD, last_displayship.ID, 0, EDCommander.CurrentCmdrID);
                        var jo = je.CreateJSON();
                        je.Add(jo);
                        DiscoveryForm.NewEntry(je);
                    }

                    f.ReturnResult(DialogResult.Cancel);
                }
            };

            DialogResult res = f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "Ship Configure".Tx(), closeicon: true);

            if (res == DialogResult.OK)
            {
                last_displayship.FuelWarningPercent = f.GetDouble("FuelWarning").Value;
                Display();
            }
        }

        private void ModuleDisplayClickElement(object sender, MouseEventArgs e, ExtendedControls.ImageElement.Element i, object tag)
        {
            if (i != null)
            {
                if (tag is ShipSlots.Slot ss)
                {
                    foreach (DataGridViewRow rw in dataGridViewModules.Rows)
                    {
                        if (rw.Cells[2].Tag != null && (ShipSlots.Slot)rw.Cells[2].Tag == ss)      // Cells[2] has the tag
                        {
                            dataGridViewModules.SafeFirstDisplayedScrollingRowIndex(rw.Index);
                            dataGridViewModules.ClearSelection();
                            break;
                        }
                    }
                }
                else if (tag is ShipModule sm)
                {
                    if (i.Name == "Enable")
                    {
                        sm.SetEnabled(sm.Enabled != true);
                    }
                    else if (i.Name == "Priority")
                    {
                        sm.CyclePriority();
                    }
                    Display();
                }
            }

        }

        private void dataGridViewModules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // if row tag is ship (ownedshiptext)
                Ship shipinstance = dataGridViewModules.Rows[e.RowIndex].Tag as Ship;
                // if row tag is ship prop (all ship text)
                ItemData.ShipProperties ship = dataGridViewModules.Rows[e.RowIndex].Tag as ItemData.ShipProperties;

                if (shipinstance != null)
                {
                    DisplayModuleDiagram(shipinstance.GetShipProperties(), shipinstance, false);
                }
                else if (ship != null)
                {
                    DisplayModuleDiagram(ship, null, false);
                }
            }
        }
        private void dataGridViewModules_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                Ship shp = dataGridViewModules.Rows[e.RowIndex].Tag as Ship;        // if row tag is ship (ownedshiptext)
                if (shp != null)
                {
                    string ident = shp.ShipNameIdentType;
                    comboBoxShips.SelectedItem = ident;
                }
                else
                {
                    string tt = dataGridViewModules.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText;
                    if (!string.IsNullOrEmpty(tt))      // if we have a tool tip
                    {
                        Form mainform = FindForm();
                        ExtendedControls.InfoForm frm = new ExtendedControls.InfoForm();
                        frm.Info("Module Information".Tx(), mainform.Icon, tt);
                        frm.Size = new Size(600, 400);
                        frm.StartPosition = FormStartPosition.CenterParent;
                        frm.Show(mainform);
                    }
                }
            }
        }

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            if (dataGridViewModules.RowCount > 0)
            {
                Forms.ImportExportForm frm = new Forms.ImportExportForm();


                frm.Export(last_displayship != null ? new string[] { "Export Current View", "Export SLEF Loadout" } : new string[] { "Export Current View" },
                                new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude, Forms.ImportExportForm.ShowFlags.None },
                                new string[] { "CSV export| *.csv", "SLEF Loadout|*.loadout" }
                                );
                if (last_displayship != null)
                {
                    frm.ShowOptionalButton("Loadout", () =>
                    {
                        frm.Close();
                        string s = last_displayship.JSONLoadout(true).ToString(false);
                        ExtendedControls.InfoForm info = new ExtendedControls.InfoForm();
                        info.Info("Loadout", this.FindForm().Icon, s);
                        info.Show(this);
                    });
                }

                if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
                {
                    if (frm.SelectedIndex == 0)
                    {
                        BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid(frm.Delimiter);

                        grd.GetPreHeader += delegate (int r)
                        {
                            if (last_displayship != null)
                            {
                                if (r == 0)
                                    return new Object[] { last_displayship.ShipUserName ?? "", last_displayship.ShipUserIdent ?? "", last_displayship.ShipType ?? "", last_displayship.ID };
                                else if (r == 1)
                                    return new Object[] { };
                            }

                            return null;
                        };

                        grd.GetHeader += delegate (int c)
                        {
                            return (frm.IncludeHeader && c < dataGridViewModules.ColumnCount) ? dataGridViewModules.Columns[c].HeaderText : null;
                        };

                        grd.GetLine += delegate (int r)
                        {
                            if (r < dataGridViewModules.RowCount)
                            {
                                DataGridViewRow rw = dataGridViewModules.Rows[r];
                                return new Object[] { rw.Cells[0].Value, rw.Cells[1].Value, rw.Cells[2].Value, rw.Cells[3].Value, rw.Cells[4].Value, rw.Cells[5].Value, rw.Cells[6].Value, rw.Cells[7].Value };
                            }
                            else
                                return null;
                        };

                        var ownedship = DiscoveryForm.History.ShipInformationList.OwnedSpaceShips().ToArray();
                        int count = 0;

                        grd.GetPostHeader += delegate (int r)
                        {
                            if (r == 0)
                                return new Object[] { };
                            else if (r == 1)
                                return new Object[] { "Ships:" };
                            else if (count < ownedship.Length)
                                return new Object[] { ownedship[count++].Value.ShipFullInfo() };
                            else
                                return null;
                        };

                        grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                    }
                    else
                    {
                        if (!BaseUtils.FileHelpers.TryWriteToFile(frm.Path, last_displayship.JSONLoadout(true).ToString(true)))
                        {
                            CSVHelpers.WriteFailed(FindForm(), frm.Path);
                        }
                    }
                }
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "No Ship Information available".Tx(), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void extButtonLoadLoadout_Click(object sender, EventArgs e)
        {
            var frm = new Forms.ImportExportForm();

            frm.Import(new string[] { "SLEF - Loadout from EDSY or journal loadout event" },
                 new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowPaste | ExtendedForms.ImportExportForm.ShowFlags.ShowImportSaveas },
                 new string[] { "LoadOut|*.loadout|JSON|*.json" }
            );

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                string loadout = frm.ReadSource();
                if (loadout?.Length > 0)
                {
                    Ship si = Ship.CreateFromLoadout(loadout);

                    if (si != null)
                    {
                        string name = frm.SaveImportAs.HasChars() ? frm.SaveImportAs : "Last imported ship";
                        name += ".loadout";
                        string path = System.IO.Path.Combine(EDDOptions.Instance.ShipLoadoutsDirectory(), name);

                        if (BaseUtils.FileHelpers.TryWriteToFile(path, si.JSONLoadout(true).ToString(true)))    // this must work, but check
                        {
                            UpdateComboBox();
                            if (comboBoxShips.Text == name)
                            {
                                Display();
                            }
                            else
                                comboBoxShips.SelectedItem = name;
                        }
                    }
                }
            }
        }

        private void extButtonSaveLoadout_Click(object sender, EventArgs e)
        {
            if (dataGridViewModules.RowCount > 0 && last_displayship != null)
            {
                string name = ExtendedControls.PromptSingleLine.ShowDialog(FindForm(), "Name:", "", "Enter loadout description to save ship with".Tx(), FindForm().Icon, requireinput: true);

                if (name != null)
                {
                    name += ".loadout";
                    string path = System.IO.Path.Combine(EDDOptions.Instance.ShipLoadoutsDirectory(), name);
                    if (BaseUtils.FileHelpers.TryWriteToFile(path, last_displayship.JSONLoadout(true).ToString(true)))
                    {
                        UpdateComboBox();
                        comboBoxShips.SelectedItem = name;
                    }
                }
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "No Ship Information available".Tx(), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void extButtonDeleteLoadout_Click(object sender, EventArgs e)
        {
            string name = comboBoxShips.Text;
            if (ExtendedControls.MessageBoxTheme.Show($"Confirm removal of".Tx() + " " + name, "Warning".Tx(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                string path = System.IO.Path.Combine(EDDOptions.Instance.ShipLoadoutsDirectory(), name);
                BaseUtils.FileHelpers.DeleteFileNoError(path);
                UpdateComboBox();       // this will remove the entry from the combo box and go back to travel history
                Display();
            }
        }

        #endregion


        #region Right clicks

        private void contextMenuStripShipList_Opening(object sender, CancelEventArgs e)
        {
            var ship = dataGridViewModules.ClickedRightRow?.Tag as Ship;        // null if no clicked right row or not ship
            goToCreationEventToolStripMenuItem.Enabled = ship?.CreateEvent != null;      // paranoia here, it must be set, right?
            goToSolddestroyedEventToolStripMenuItem.Enabled = ship?.SoldDestroyedEvent != null;
        }

        private void goToCreationEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ship = dataGridViewModules.ClickedRightRow.Tag as Ship;        // null if no clicked right row or not ship
            if ( RequestPanelOperationOpen(PanelInformation.PanelIDs.HistoryGrid, new RequestHistoryToJID { JID = ship.CreateEvent.Id, MakeVisible = true }) == PanelActionState.Failed)
            {
                ExtendedControls.MessageBoxTheme.Show("Entry filtered out of grid view", "Not in grid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void goToSolddestroyedEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ship = dataGridViewModules.ClickedRightRow.Tag as Ship;        // null if no clicked right row or not ship
            if ( RequestPanelOperationOpen(PanelInformation.PanelIDs.HistoryGrid, new RequestHistoryToJID { JID = ship.SoldDestroyedEvent.Id, MakeVisible = true }) == PanelActionState.Failed)
            {
                ExtendedControls.MessageBoxTheme.Show("Entry filtered out of grid view", "Not in grid", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        #endregion

        #region Word wrap

        private void extCheckBoxWordWrap_Click(object sender, EventArgs e)
        {
            PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
            UpdateWordWrap();
        }

        private void UpdateWordWrap()
        {
            dataGridViewModules.SetWordWrap(extCheckBoxWordWrap.Checked);
            dataViewScrollerPanel.UpdateScroll();
        }

        #endregion



        #region Helpers
        private void UpdateComboBox()
        {
            ShipList shm = DiscoveryForm.History.ShipInformationList;
            string cursel = comboBoxShips.Text;

            comboBoxShips.Items.Clear();
            comboBoxShips.Items.Add(travelhistorytext);
            comboBoxShips.Items.Add(currentownedshipstext);
            comboBoxShips.Items.Add(allownedshipstext);
            comboBoxShips.Items.Add(allshipstext);
            comboBoxShips.Items.Add(storedmoduletext);
            comboBoxShips.Items.Add(allmodulestext);
            comboBoxShips.Items.Add(allknownmodulestext);

            var ownedships = shm.OwnedSpaceShips();
            var soldships = shm.SoldDestroyedSpaceShips();

            var now = (from x1 in ownedships where x1.Value.StoredAtSystem == null select x1.Value.ShipNameIdentType);
            comboBoxShips.Items.AddRange(now);

            var stored = (from x1 in ownedships where x1.Value.StoredAtSystem != null select x1.Value.ShipNameIdentType);
            comboBoxShips.Items.AddRange(stored);


            var loadoutfiles = System.IO.Directory.EnumerateFiles(EDDOptions.Instance.ShipLoadoutsDirectory(), "*.loadout", System.IO.SearchOption.TopDirectoryOnly).
                        Select(f => new System.IO.FileInfo(f)).OrderByDescending(p => p.LastWriteTimeUtc).ToArray();
            foreach (var x in loadoutfiles)
                comboBoxShips.Items.Add(x.Name);

            comboBoxShips.Items.AddRange(soldships.Select(x => x.Value.ShipNameIdentType).ToList());

            //comboBoxShips.Items.AddRange(fightersrvs.Select(x => x.ShipNameIdentType).ToList());

            if (cursel == "")
                cursel = GetSetting(dbShipSelect, "");

            if (cursel == "" || !comboBoxShips.Items.Contains(cursel))
                cursel = travelhistorytext;

            comboBoxShips.Enabled = false;
            comboBoxShips.SelectedItem = cursel;
            comboBoxShips.Enabled = true;
        }

        #endregion

    }
}
