/*
 * Copyright © 2016 - 2019 EDDiscovery development team
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
 * 
 */
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using QuickJSON;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlStarDistance : UserControlCommonBase
    {
        public UserControlStarDistance()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateToolstrip(contextMenuStrip);
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip, this);

            DBBaseName = "StarDistancePanel";
        }

        private StarDistanceComputer computer = null;
        private HistoryEntry last_he = null;

        private const double defaultMaxRadius = 100;
        private const double defaultMinRadius = 0;
        private const int maxitems = 500;

        private double lookup_limit = 300;      // we start with a reasonable number, because if your in the bubble, you don't want to be looking up 1000

        protected override void Init()
        {
            computer = new StarDistanceComputer();

            textMinRadius.ValueNoChange = GetSetting("Min", defaultMinRadius);
            textMaxRadius.ValueNoChange = GetSetting("Max", defaultMaxRadius);
            textMinRadius.SetComparitor(textMaxRadius, -2);     // need to do this after values are set
            textMaxRadius.SetComparitor(textMinRadius, 2);

            checkBoxCube.Checked = GetSetting("Behaviour", false);
        }

        protected override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewNearest);
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
        }

        protected override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewNearest);
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            computer.ShutDown();
            PutSetting("Min", textMinRadius.Value);
            PutSetting("Max", textMaxRadius.Value);
            PutSetting("Behaviour", checkBoxCube.Checked);
        }

        protected override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
        }

        private void Discoveryform_OnHistoryChange()
        {
            KickComputation(DiscoveryForm.History.GetLast);   // copes with getlast = null
        }

        // sent by its travel history grid
        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            KickComputation(he);
        }

        private void KickComputation(HistoryEntry he, bool force = false)
        {
            if (he != null && he.System != null && (force || !he.System.Equals(last_he?.System)))
            {
                last_he = he;

                double lookup_max = Math.Min(textMaxRadius.Value, lookup_limit);

                System.Diagnostics.Debug.WriteLine($"Star distance kick with min {textMinRadius.Value} - {textMaxRadius.Value} ll {lookup_limit} = {lookup_max}");

                // Get nearby systems from the systems DB.
                computer.CalculateClosestSystems(he.System,
                    NewStarListComputedAsync,
                    maxitems, textMinRadius.Value, lookup_max, !checkBoxCube.Checked
                    );     // hook here, force closes system update


            }
        }

        private void NewStarListComputedAsync(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem> list)
        {
            //System.Diagnostics.Debug.WriteLine("Computer returned " + list.Count);
            this.ParentForm.BeginInvoke(new Action(() => NewStarListComputed(sys, list)));
        }

        private void NewStarListComputed(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem> list)      // In UI
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);       // check!

            //System.Diagnostics.Debug.WriteLine(BaseUtils.AppTicks.TickCountLapDelta("SD1", true) + "SD main thread");

            double lookup_max = Math.Min(textMaxRadius.Value, lookup_limit);

            // Get nearby systems from our travel history. This will filter out duplicates from the systems DB.
            DiscoveryForm.History.CalculateSqDistances(list, sys.X, sys.Y, sys.Z,
                                maxitems, textMinRadius.Value, lookup_max, !checkBoxCube.Checked, 5000);       // only go back a sensible number of FSD entries

            FillGrid(sys.Name, list);

            //System.Diagnostics.Debug.WriteLine(BaseUtils.AppTicks.TickCountLapDelta("SD1") + "SD  finish");
        }

        private void FillGrid(string name, BaseUtils.SortedListDoubleDuplicate<ISystem> csl)
        {
            DataGridViewColumn sortcolprev = dataGridViewNearest.SortedColumn != null ? dataGridViewNearest.SortedColumn : dataGridViewNearest.Columns[1];
            SortOrder sortorderprev = dataGridViewNearest.SortedColumn != null ? dataGridViewNearest.SortOrder : SortOrder.Ascending;

            dataViewScrollerPanel.Suspend();
            dataGridViewNearest.Rows.Clear();

            if (csl != null && csl.Any())
            {
                SetControlText(string.Format("From {0}".Tx(), name));

                List<DataGridViewRow> rows = new List<DataGridViewRow>();

                int maxdist = 0;
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    double dist = Math.Sqrt(tvp.Key);   // distances are stored squared for speed, back to normal.

                    maxdist = Math.Max(maxdist, (int)dist);

                    if (tvp.Value.Name != name && (checkBoxCube.Checked || (dist >= textMinRadius.Value && dist <= textMaxRadius.Value)))
                    {
                        int visits = DiscoveryForm.History.GetVisitsCount(tvp.Value.Name);
                        object[] rowobj = { tvp.Value.Name, $"{dist:0.00}", $"{visits:n0}" };

                        var rw = dataGridViewNearest.RowTemplate.Clone() as DataGridViewRow;
                        rw.CreateCells(dataGridViewNearest, rowobj);
                        rw.Tag = tvp.Value;
                        rows.Add(rw);
                    }
                }

                dataGridViewNearest.Rows.AddRange(rows.ToArray());

                double proportionfilled = (double)csl.Count / maxitems;

                // scale the limiter dependent on how much we got.
                lookup_limit = (int)(lookup_limit * (3.85 - proportionfilled * 3));         // 0.95 (1+3*0.95 = 3.85) seems a good figure

                lookup_limit = Math.Max(lookup_limit,textMinRadius.Value + 50);     // make sure the limiter is above the min radius by a bit

                System.Diagnostics.Debug.WriteLine($"Star distance Lookup {name} got {csl.Count} prop {proportionfilled} New limit {lookup_limit}");

                if (csl.Count < maxitems / 2 && lookup_limit < textMaxRadius.Value) // if we have a low value, and the limiter is below the radius, try again
                {
                    System.Diagnostics.Debug.WriteLine($"Low lookup limit, try again");
                    KickComputation(last_he,true);
                }
            }
            else
            {
                SetControlText(string.Empty);
            }

            if (sortcolprev != colDistance || sortorderprev != SortOrder.Ascending)   // speed optimising, only sort if not in sort order from distances
                dataGridViewNearest.Sort(sortcolprev, (sortorderprev == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);

            dataGridViewNearest.Columns[sortcolprev.Index].HeaderCell.SortGlyphDirection = sortorderprev;

            dataViewScrollerPanel.Resume();
        }

        private void dataGridViewNearest_MouseDown(object sender, MouseEventArgs e)
        {
            viewSystemToolStripMenuItem.Enabled =
            viewOnSpanshToolStripMenuItem.Enabled = 
            viewOnEDSMToolStripMenuItem1.Enabled = dataGridViewNearest.RightClickRowValid;
        }

        private void addToTrilaterationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddTo(UserControlCommonBase.PushStars.PushType.TriSystems);
        }

        private void addToExpeditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTo(UserControlCommonBase.PushStars.PushType.Expedition);

        }

        private void AddTo(UserControlCommonBase.PushStars.PushType pushtype)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewNearest.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            List<ISystem> syslist = new List<ISystem>();
            foreach (DataGridViewRow r in selectedRows)
                syslist.Add(r.Tag as SystemClass);

            var req = new UserControlCommonBase.PushStars() { PushTo = pushtype, SystemList = syslist, MakeVisible = true };
            RequestPanelOperationOpen(pushtype == PushStars.PushType.Expedition ? PanelInformation.PanelIDs.Expedition : PanelInformation.PanelIDs.Trilateration, req);
        }

        private void viewOnEDSMToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // check above for right click valid
            var rightclicksystem = (ISystem)dataGridViewNearest.Rows[dataGridViewNearest.RightClickRow].Tag;
            this.Cursor = Cursors.WaitCursor;
            EDSMClass edsm = new EDSMClass();
            if (!edsm.ShowSystemInEDSM(rightclicksystem.Name))
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable".Tx());
            }

            this.Cursor = Cursors.Default;
        }

        private void viewOnSpanshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rightclicksystem = (ISystem)dataGridViewNearest.Rows[dataGridViewNearest.RightClickRow].Tag;
            EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForSystem(rightclicksystem);
        }

        private void dataGridViewNearest_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 1)
                e.SortDataGridViewColumnNumeric();
        }

        private void checkBoxCube_CheckedChanged(object sender, EventArgs e)
        {
            KickComputation(last_he, true);
        }

        private void textMinRadius_ValueChanged(object sender, EventArgs e)
        {
            KickComputation(last_he, true);
        }

        private void textMaxRadius_ValueChanged(object sender, EventArgs e)
        {
            KickComputation(last_he, true);
        }

        private void dataGridViewNearest_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0 && e.RowIndex < dataGridViewNearest.Rows.Count)
            {
                SetClipboardText(dataGridViewNearest.Rows[e.RowIndex].Cells[0].Value as string);
            }
        }

        private void viewSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewNearest.RightClickRowValid)
            {
                var rightclicksystem = (ISystem)dataGridViewNearest.Rows[dataGridViewNearest.RightClickRow].Tag;

                if (rightclicksystem != null)
                    ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), rightclicksystem, DiscoveryForm.History);
            }
        }

        private void dataGridViewNearest_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewNearest.Rows.Count)
            {
                var clicksystem = (ISystem)dataGridViewNearest.Rows[e.RowIndex].Tag;
                if (clicksystem != null)
                    ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), clicksystem,  DiscoveryForm.History);
            }

        }

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            if (last_he == null)
                return;

            Forms.ImportExportForm frm = new Forms.ImportExportForm();
            frm.Export( new string[] { "View", "JSON" },
                            new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude, Forms.ImportExportForm.ShowFlags.None },
                            new string[] { "CSV|*.csv", "JSON|*.json" },
                            new string[] { "neareststars", "neareststars" }
                );

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (frm.SelectedIndex == 0)
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid(frm.Delimiter);

                    grd.GetLine += delegate (int r)
                    {
                        if (r < dataGridViewNearest.RowCount+1)
                        {
                            if ( r == 0 )
                            {
                                return new object[] { last_he.System.Name, "0", DiscoveryForm.History.GetVisitsCount(last_he.System.Name),
                                            last_he.System.X.ToString("F2",grd.FormatCulture), last_he.System.Y.ToString("F2",grd.FormatCulture), last_he.System.Z.ToString("F2",grd.FormatCulture) };
                            }
                            else
                            {
                                r--;
                                DataGridViewRow rw = dataGridViewNearest.Rows[r];
                                var clicksystem = (ISystem)rw.Tag;

                                return new Object[] {
                                    rw.Cells[0].Value,
                                    rw.Cells[1].Value,
                                    rw.Cells[2].Value,
                                    clicksystem.X.ToString("F2",grd.FormatCulture) ?? "",
                                    clicksystem.Y.ToString("F2",grd.FormatCulture) ?? "",
                                    clicksystem.Z.ToString("F2",grd.FormatCulture) ?? "",
                                };
                            }
                        }
                        else
                            return null;
                    };

                    grd.GetHeader += delegate (int c)
                    {
                        if (frm.IncludeHeader)
                        {
                            if (c < dataGridViewNearest.ColumnCount)
                                return dataGridViewNearest.Columns[c].HeaderText;
                            else if (c < 6)
                                return new string[] { "", "", "", "X", "Y", "Z" }[c];
                        }
                        return null;
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
                else
                {
                    JObject data = new JObject();
                    data["System"] = new JObject()
                    {
                        ["Name"] = last_he.System.Name,
                        ["X"] = last_he.System.X,
                        ["Y"] = last_he.System.Y,
                        ["Z"] = last_he.System.Z,
                    };

                    var ja = new JArray();
                    foreach (DataGridViewRow rw in dataGridViewNearest.Rows)
                    {
                        var clicksystem = (ISystem)rw.Tag;
                        JObject entry = new JObject()
                        {
                            ["Name"] = rw.Cells[0].Value as string,
                            ["Distance"] = ((string)rw.Cells[1].Value).InvariantParseDouble(0),
                            ["Visits"] = ((string)rw.Cells[2].Value).InvariantParseInt(0),
                            ["X"] = clicksystem.X,
                            ["Y"] = clicksystem.Y,
                            ["Z"] = clicksystem.Z,
                        };

                        ja.Add(entry);
                    }

                    data["Nearest"] = ja;

                    try
                    {
                        System.IO.File.WriteAllText(frm.Path, data.ToString(true)); // failure will be picked up below
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("star distance json " + ex);
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to write to " + frm.Path, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

            }
        }


    }
}
