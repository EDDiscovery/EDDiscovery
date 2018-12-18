/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.Controls;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.EDDN;
using EliteDangerousCore.DB;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    // Search UCs use the UCCB template BUT are not directly inserted into the normal panels.. they are inserted into the Search UCCB
    // Make sure DB saving has unique names.. they all share the same displayno.

    public partial class UserControlSearchStars : UserControlCommonBase
    {
        private string DbColumnSave { get { return DBName("UCSearchStars", "DGVCol"); } }

        #region Init

        public UserControlSearchStars()
        {
            InitializeComponent();
            var corner = dataGridViewEDSM.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridViewEDSM.MakeDoubleBuffered();
            dataGridViewEDSM.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewEDSM.RowTemplate.Height = 26;
            dataGridViewEDSM.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            findSystemsUserControl.Init(displaynumber, "SearchFindSys", true, discoveryform);
            findSystemsUserControl.Excel += RunExcel;
            findSystemsUserControl.ReturnSystems += StarsFound;

            BaseUtils.Translator.Instance.Translate(this, new Control[] { findSystemsUserControl });
            BaseUtils.Translator.Instance.Translate(toolTip, this);
            BaseUtils.Translator.Instance.Translate(contextMenu, this);
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewEDSM, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewEDSM, DbColumnSave);
            findSystemsUserControl.Closing();
        }

        #endregion

        private void StarsFound(List<Tuple<ISystem, double>> systems)       // systems may be null
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);

            dataGridViewEDSM.Rows.Clear();

            if (systems != null)
            {
                ISystem cursystem = discoveryform.history.CurrentSystem;        // could be null
                bool centresort = false;

                foreach (Tuple<ISystem, double> ret in systems)
                {
                    ISystem sys = ret.Item1;
                    string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";
                    object[] rowobj = {     sys.Name,
                                            (ret.Item2>=0 ? ret.Item2.ToStringInvariant("0.#") : ""),
                                            (cursystem != null ? cursystem.Distance(sys).ToString("0.#") : ""),
                                            sys.X.ToString("0.#") + sep + sys.Y.ToString("0.#") + sep + sys.Z.ToString("0.#"),
                                            sys.EDSMID.ToStringInvariant() };

                    dataGridViewEDSM.Rows.Add(rowobj);
                    dataGridViewEDSM.Rows[dataGridViewEDSM.Rows.Count - 1].Tag = sys;
                    centresort |= ret.Item2 >= 0;
                }

                dataGridViewEDSM.Sort(centresort ? ColumnCentreDistance : ColumnCurrentDistance, ListSortDirection.Ascending);
            }

        }

        ISystem rightclicksystem = null;
        int rightclickrow = -1;

        private void dataGridViewEDSM_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                rightclicksystem = null;
                rightclickrow = -1;
            }

            if (dataGridViewEDSM.SelectedCells.Count < 2 || dataGridViewEDSM.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
            {
                DataGridView.HitTestInfo hti = dataGridViewEDSM.HitTest(e.X, e.Y);
                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    dataGridViewEDSM.ClearSelection();                // select row under cursor.
                    dataGridViewEDSM.Rows[hti.RowIndex].Selected = true;

                    if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
                    {
                        rightclickrow = hti.RowIndex;
                        rightclicksystem = (ISystem)dataGridViewEDSM.Rows[hti.RowIndex].Tag;
                    }
                }
            }
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
            {
                this.Cursor = Cursors.WaitCursor;
                EDSMClass edsm = new EDSMClass();
                long? id_edsm = rightclicksystem.EDSMID;

                if (id_edsm == 0)
                {
                    id_edsm = null;
                }

                if (!edsm.ShowSystemInEDSM(rightclicksystem.Name, id_edsm))
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable");

                this.Cursor = Cursors.Default;
            }
        }

        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
            {
                this.Cursor = Cursors.WaitCursor;

                if (!discoveryform.Map.Is3DMapsRunning)            // if not running, click the 3dmap button
                    discoveryform.Open3DMap(null);

                this.Cursor = Cursors.Default;

                if (discoveryform.Map.Is3DMapsRunning)             // double check here! for paranoia.
                {
                    if (discoveryform.Map.MoveToSystem(rightclicksystem))
                        discoveryform.Map.Show();
                }
            }
        }

        private void viewScanOfSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicksystem != null)
                ShowScanPopOut(rightclicksystem);
        }

        private void dataGridViewEDSM_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ( e.RowIndex>=0)
                ShowScanPopOut((ISystem)dataGridViewEDSM.Rows[e.RowIndex].Tag);
        }

        void ShowScanPopOut(ISystem sys)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
            int width = 800, height = 800;

            ScanDisplay sd = new ScanDisplay();
            sd.ShowMoons = sd.ShowMaterials = sd.ShowOverlays = sd.CheckEDSM = true;
            sd.SetSize(48);
            sd.Size = new Size(width - 20, 1024);
            sd.DrawSystem(sys, null, discoveryform.history);

            height = Math.Min(800, sd.DisplayAreaUsed.Y) + 100;

            f.Add(new ExtendedControls.ConfigurableForm.Entry("Sys", null, null, new Point(0, 40), new Size(width - 20, height - 85), null) { control = sd });
            f.Add(new ExtendedControls.ConfigurableForm.Entry("OK", typeof(ExtendedControls.ButtonExt), "OK".Tx(), new Point(width - 20 - 80, height - 40), new Size(80, 24), ""));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "OK")
                    f.Close();
            };

            f.Init(this.FindForm().Icon, new Size(width, height), new Point(-999, -999), "System ".Tx(this, "Sys") + ": " + sys.Name, null, null);

            sd.DrawSystem(sys, null, discoveryform.history);

            f.Show(this.FindForm());
        }


        private void dataGridViewEDSM_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 1 || e.Column.Index == 2 || e.Column.Index == 4)
                e.SortDataGridViewColumnNumeric();
           
        }

        #region Excel

        private void RunExcel()
        {
            if (dataGridViewEDSM.Rows.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No data to export", "Export EDSM", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(new string[] { "Export Current View" }, disablestartendtime: true);

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (frm.SelectedIndex == 0)
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                    grd.SetCSVDelimiter(frm.Comma);

                    grd.GetLineStatus += delegate (int r)
                    {
                        if (r < dataGridViewEDSM.Rows.Count)
                        {
                            return BaseUtils.CSVWriteGrid.LineStatus.OK;
                        }
                        else
                            return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                    };

                    string[] colh = new string[] { "Star", "Centre Distance", "Current Distance", "X", "Y", "Z", "ID" };

                    grd.GetHeader += delegate (int c)
                    {
                        return (c < colh.Length && frm.IncludeHeader) ? colh[c] : null;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        DataGridViewRow rw = dataGridViewEDSM.Rows[r];
                        ISystem sys = rw.Tag as ISystem;
                        return new Object[]
                        {
                            rw.Cells[0].Value, rw.Cells[1].Value, rw.Cells[2].Value, sys.X.ToString("0.#"), sys.Y.ToString("0.#"), sys.Z.ToString("0.#"), rw.Cells[4].Value
                        };
                    };

                    if (grd.WriteCSV(frm.Path))
                    {
                        if (frm.AutoOpen)
                            System.Diagnostics.Process.Start(frm.Path);
                    }
                    else
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to write to " + frm.Path, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        #endregion

        private void findSystemsUserControl_Load(object sender, EventArgs e)
        {

        }

    }
}
