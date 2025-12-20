/*
 * Copyright © 2019-2023 EDDiscovery development team
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
using EliteDangerousCore.EDSM;
using EMK.LightGeometry;
using System;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlRoute
    {
  
        private void UpdateDistance()
        {
            string dist = "";
            if (GetCoordsFrom(out Point3D from) && GetCoordsTo(out Point3D to))
            {
                dist = Point3D.DistanceBetween(from, to).ToString("0.00");
            }

            textBox_Distance.Text = dist;
        }



        #region Excel
        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            if (dataGridViewRoute.Rows.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No Route Plotted", "Route", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Forms.ImportExportForm frm = new Forms.ImportExportForm();
            frm.Export(new string[] { "All" }, new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude },
                suggestedfilenamesp: new string[] {"route.csv"} );

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid(frm.Delimiter);

                grd.GetLineStatus += delegate (int r)
                {
                    if (r < dataGridViewRoute.Rows.Count)
                        return BaseUtils.CSVWriteGrid.LineStatus.OK;
                    else
                        return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                };

                grd.GetLine += delegate (int r)
                {
                    DataGridViewRow rw = dataGridViewRoute.Rows[r];
                    ISystem sys = rw.Tag as ISystem;

                    return new Object[] { rw.Cells[0].Value,rw.Cells[1].Value,
                                          rw.Cells[2].Value,rw.Cells[3].Value,rw.Cells[4].Value,
                                          rw.Cells[5].Value,rw.Cells[6].Value ,rw.Cells[7].Value ,rw.Cells[8].Value ,
                                            sys?.SystemAddress ?? 0 };
                };

                grd.GetHeader += delegate (int c)
                {
                    return c < dataGridViewRoute.Columns.Count ? dataGridViewRoute.Columns[c].HeaderText : 
                        c == dataGridViewRoute.Columns.Count ? "System Address" : null;
                };

                grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
            }
        }

        #endregion

        #region DGV

        private void dataGridViewRoute_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridViewRoute.CurrentCell;
            if (cell != null)
            {
                // If a cell contains a tag (i.e. a system name), copy the string of the tag
                // else, copy whatever text is inside
                string s = "";
                if (cell.Tag != null)
                    s = cell.Tag.ToString();
                else
                    s = (string)cell.Value;
                SetClipboardText(s);
            }
        }

        private void dataGridViewRoute_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column == DistanceCol || e.Column == XCol || e.Column == YCol || e.Column == ZCol || e.Column == WayPointDistCol || e.Column == DeviationCol)
                e.SortDataGridViewColumnNumeric();
        }

        private void dataGridViewRoute_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = dataGridViewRoute.CurrentCell?.RowIndex ?? -1;
            if (row >= 0)
            {
                ISystem sys = dataGridViewRoute.Rows[row].Tag as ISystem;
                ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), sys, DiscoveryForm.History);
            }
        }

        #endregion

        #region Other UI

        private void extButtonExpeditionPush_Click(object sender, EventArgs e)
        {
            RouteHelpers.ExpeditionPush(labelRouteName.Text, routeSystems, this, DiscoveryForm);
        }

        private void extButtonExpeditionSave_Click(object sender, EventArgs e)
        {
            RouteHelpers.ExpeditionSave(this.FindForm(), labelRouteName.Text, routeSystems);
        }

        private void cmd3DMap_Click(object sender, EventArgs e)
        {
            RouteHelpers.Open3DMap(routeSystems, DiscoveryForm);
        }

        private void ExtCheckBoxPermitSystems_Click(object sender, EventArgs e)
        {
            PutSetting(dbPermit, extCheckBoxPermitSystems.Checked);
        }

        private void textBox_Clicked(object sender, EventArgs e)
        {
            ((ExtendedControls.ExtTextBox)sender).SelectAll(); // clicking highlights everything
        }

        private void dataGridViewRoute_MouseDown(object sender, MouseEventArgs e)
        {
            showInEDSMToolStripMenuItem.Enabled = dataGridViewRoute.RightClickRowValid && dataGridViewRoute.Rows[dataGridViewRoute.RightClickRow].Tag != null;
            showScanToolStripMenuItem.Enabled = dataGridViewRoute.RightClickRowValid && dataGridViewRoute.Rows[dataGridViewRoute.RightClickRow].Tag != null;
            copyToolStripMenuItem.Enabled = dataGridViewRoute.GetCellCount(DataGridViewElementStates.Selected) > 0;
        }

        private void showInEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewRoute.RightClickRowValid)
            {
                ISystem sys = dataGridViewRoute.Rows[dataGridViewRoute.RightClickRow].Tag as ISystem;

                if (sys != null) // paranoia because it should not be enabled otherwise
                {
                    this.Cursor = Cursors.WaitCursor;

                    EliteDangerousCore.EDSM.EDSMClass edsm = new EDSMClass();
                    if (!edsm.ShowSystemInEDSM(sys.Name))
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable");

                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void viewOnSpanshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewRoute.RightClickRowValid)
            {
                ISystem sys = dataGridViewRoute.Rows[dataGridViewRoute.RightClickRow].Tag as ISystem;

                if (sys != null) // paranoia because it should not be enabled otherwise
                {
                    EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForSystem(sys);
                }
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewRoute.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                SetClipboard(dataGridViewRoute.GetClipboardContent());
            }
        }

        private void showScanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewRoute.RightClickRowValid)
            {
                ISystem sys = dataGridViewRoute.Rows[dataGridViewRoute.RightClickRow].Tag as ISystem;
                ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), sys, DiscoveryForm.History);    // protected against sys = null
            }
        }


        #endregion

        #region Control validity

        private void ValidityChanges(ExtendedControls.NumberBox<double> nb, bool v)
        {
            EnableRouteButtonsIfValid();
        }

        public void EnableRouteButtonsIfValid()
        {
            bool numsvalid = textBox_ToX.IsValid && textBox_ToY.IsValid && textBox_ToZ.IsValid &&
                        textBox_FromX.IsValid && textBox_FromY.IsValid && textBox_FromZ.IsValid && textBox_Range.IsValid;
            bool fromvalid = textBox_From.Text.HasChars();
            bool tovalid = textBox_To.Text.HasChars();
            bool notcomputing = computing == 0;

            extButtonRoute.Enabled = numsvalid && computing != 2;
            comboBoxRoutingMetric.Enabled = notcomputing;

            extButtonExoMastery.Enabled = extButtonSpanshRoadToRiches.Enabled = extButtonSpanshAmmoniaWorlds.Enabled = 
                   extButtonSpanshRockyHMC.Enabled = extButtonSpanshEarthLikes.Enabled = fromvalid && notcomputing;
            extButtonNeutronRouter.Enabled = fromvalid && tovalid && notcomputing;
            extButtonSpanshTradeRouter.Enabled = fromvalid && notcomputing;
            extButtonSpanshGalaxyPlotter.Enabled = extButtonFleetCarrier.Enabled = fromvalid && tovalid && notcomputing;

            textBox_To.Enabled = textBox_ToName.Enabled = buttonExtTravelTo.Enabled = buttonExtTargetTo.Enabled =
            textBox_From.Enabled = textBox_FromName.Enabled = buttonExtTravelFrom.Enabled = buttonTargetFrom.Enabled =
            textBox_FromX.Enabled = textBox_FromY.Enabled = textBox_FromZ.Enabled =
            textBox_ToX.Enabled = textBox_ToY.Enabled = textBox_ToZ.Enabled =
            extCheckBoxPermitSystems.Enabled =
            edsmSpanshButton.Enabled =
            numberBoxIntCargo.Enabled = 
            textBox_Range.Enabled = checkBox_FsdBoost.Enabled = notcomputing;
        }

        public void EnableOutputButtons(bool en = false)
        {
            extButtonExpeditionSave.Enabled = buttonExtExcel.Enabled = extButtonExpeditionPush.Enabled = cmd3DMap.Enabled = en;
        }

        #endregion
    }
}
