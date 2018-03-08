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
    public partial class UserControlEDSM : UserControlCommonBase
    {
        private string DbColumnSave { get { return "UCEDSM" + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }
        private string DbStar { get { return "UCEDSM" + ((displaynumber > 0) ? displaynumber.ToString() : "") + "Star"; } }
        private string DbRadius { get { return "UCEDSM" + ((displaynumber > 0) ? displaynumber.ToString() : "") + "Radius"; } }

        #region Init

        public UserControlEDSM()
        {
            InitializeComponent();
            var corner = dataGridViewEDSM.TopLeftHeaderCell; // work around #1487
            buttonExtEDSMSphere.Enabled = buttonExtDBLookup.Enabled = false;
        }

        public override void Init()
        {
            dataGridViewEDSM.MakeDoubleBuffered();
            dataGridViewEDSM.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewEDSM.RowTemplate.Height = 26;
            dataGridViewEDSM.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            textBoxRadius.Text = SQLiteConnectionUser.GetSettingString(DbRadius, "20");
            textBoxSystemName.Text = SQLiteConnectionUser.GetSettingString(DbStar, "");

            this.textBoxRadius.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            this.textBoxSystemName.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);

            ValidateEnable();
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewEDSM, DbColumnSave);
        }

        public override void InitialDisplay()
        {
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewEDSM, DbColumnSave);
            SQLiteConnectionUser.PutSettingString(DbRadius, textBoxRadius.Text.InvariantParseDoubleNull()!=null ? textBoxRadius.Text : "20");
            SQLiteConnectionUser.PutSettingString(DbStar, textBoxSystemName.Text);
        }

        #endregion

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            ValidateEnable();
        }

        void ValidateEnable()
        { 
            buttonExtEDSMSphere.Enabled = (textBoxSystemName.Text.Length > 0 && textBoxRadius.Text.InvariantParseDoubleNull() != null);
            buttonExtDBLookup.Enabled = textBoxSystemName.Text.Length > 0;
        }

        #region Excel

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            if ( dataGridViewEDSM.Rows.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No data to export", "Export EDSM", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(new string[] { "Export Current View" }, disablestartendtime:true);

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

                    string[] colh = new string[] { "Star", "Distance", "X", "Y", "Z", "ID" };

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
                            rw.Cells[0].Value, rw.Cells[1].Value, sys.X.ToString("0.#"), sys.Y.ToString("0.#"), sys.Z.ToString("0.#"), rw.Cells[3].Value
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

        private void buttonExtEDSMSphere_Click(object sender, EventArgs e)
        {
            double radius = textBoxRadius.Text.InvariantParseDouble(20);
            if ( radius > 100 )
            {
                if (ExtendedControls.MessageBoxTheme.Show(FindForm(), "This is a large radius, it make take a long time or not work, are you sure?", "Warning - Large radius", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
                    return;
            }

            Cursor = Cursors.WaitCursor;

            Task taskEDSM = Task<List<Tuple<ISystem, double>>>.Factory.StartNew(() =>
            {
                EDSMClass edsm = new EDSMClass();
                return edsm.GetSphereSystems(textBoxSystemName.Text, radius);

            }).ContinueWith(task => this.Invoke(new Action(() => { SphereGrid(task); })));

        }

        private void SphereGrid(Task<List<Tuple<ISystem, double>>> task)
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);

            dataGridViewEDSM.Rows.Clear();

            List<Tuple<ISystem, double>> systems = task.Result;

            foreach (Tuple<ISystem, double> ret in systems)
            {
                ISystem sys = ret.Item1;
                string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";
                object[] rowobj = { sys.Name, ret.Item2.ToStringInvariant("0.#"), sys.X.ToString("0.#") + sep + sys.Y.ToString("0.#") + sep + sys.Z.ToString("0.#"), sys.EDSMID.ToStringInvariant() };
                dataGridViewEDSM.Rows.Add(rowobj);
                dataGridViewEDSM.Rows[dataGridViewEDSM.Rows.Count - 1].Tag = sys;
            }

            Cursor = Cursors.Default;

            dataGridViewEDSM.Sort(ColumnDistance, ListSortDirection.Ascending);
        }

        private void buttonExtDBLookup_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Task taskEDSM = Task<List<ISystem>>.Factory.StartNew(() =>
            {
                return SystemClassDB.GetSystemsByName(textBoxSystemName.Text, uselike: true);

            }).ContinueWith(task => this.Invoke(new Action(() => { DBLookup(task); })));
        }

        private void DBLookup(Task<List<ISystem>> task)
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);

            dataGridViewEDSM.Rows.Clear();

            List<ISystem> systems = task.Result;

            HistoryEntry helast = discoveryform.history.GetLast;
            ISystem home = helast != null ? helast.System : new SystemClass("Sol", 0, 0, 0);

            foreach (ISystem sys in systems)
            {
                string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";

                string dist = "";
                if (sys.HasCoordinate)
                    dist = sys.Distance(home).ToString("0.#");

                object[] rowobj = { sys.Name, dist, sys.X.ToString("0.#") + sep + sys.Y.ToString("0.#") + sep + sys.Z.ToString("0.#"), sys.EDSMID.ToStringInvariant() };
                dataGridViewEDSM.Rows.Add(rowobj);
                dataGridViewEDSM.Rows[dataGridViewEDSM.Rows.Count - 1].Tag = sys;
            }

            Cursor = Cursors.Default;

            dataGridViewEDSM.Sort(ColumnStar, ListSortDirection.Ascending);
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

        private void dataGridViewEDSM_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 1 || e.Column.Index == 3)
            {
                double v1, v2;
                string vs1 = e.CellValue1?.ToString();
                string vs2 = e.CellValue2?.ToString();
                if (vs1 != null && vs2 != null && Double.TryParse(vs1, out v1) && Double.TryParse(vs2, out v2))
                {
                    e.SortResult = v1.CompareTo(v2);
                    e.Handled = true;
                }
            }
        }
    }
}
