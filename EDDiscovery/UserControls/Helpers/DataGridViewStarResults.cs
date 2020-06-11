/*
 * Copyright © 2019-2020 EDDiscovery development team
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
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EDDiscovery.UserControls.Search
{
    public class DataGridViewStarResults : DataGridView
    {
        public bool CheckEDSM { get; set; }
        public Action<HistoryEntry> GotoEntryClicked = null;

        private EDDiscoveryForm discoveryform;

        public DataGridViewStarResults()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            ContextMenuStrip = cms;

            cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            new System.Windows.Forms.ToolStripMenuItem(),
            new System.Windows.Forms.ToolStripMenuItem(),
            new System.Windows.Forms.ToolStripMenuItem(),
            new System.Windows.Forms.ToolStripMenuItem()
            });
            
            cms.Name = "historyContextMenu";
            cms.Size = new System.Drawing.Size(187, 70);

            cms.Items[0].Size = new System.Drawing.Size(186, 22);
            cms.Items[0].Text = "Go to star on 3D Map";
            cms.Items[0].Name = "3d";
            cms.Items[0].Click += new System.EventHandler(this.mapGotoStartoolStripMenuItem_Click);
            cms.Items[1].Size = new System.Drawing.Size(186, 22);
            cms.Items[1].Text = "View on EDSM";
            cms.Items[1].Name = "EDSM";
            cms.Items[1].Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem_Click);
            cms.Items[2].Size = new System.Drawing.Size(186, 22);
            cms.Items[2].Text = "View Data On Entry";
            cms.Items[2].Name = "Data";
            cms.Items[2].Click += new System.EventHandler(this.viewScanOfSystemToolStripMenuItem_Click);
            cms.Items[3].Text = "Go to entry on grid";
            cms.Items[3].Name = "Data";
            cms.Items[3].Click += new System.EventHandler(this.GotoEntryToolStripMenuItem_Click);

            CellDoubleClick += cellDoubleClick;
            MouseDown += mouseDown;

            BaseUtils.Translator.Instance.Translate(cms, this);
        }

        public void Init(EDDiscoveryForm frm)
        {
            discoveryform = frm;
        }

        Object rightclicktag = null;
        int rightclickrow = -1;

        private void mouseDown(object sender, MouseEventArgs e)
        {
            this.HandleClickOnDataGrid(e, out int unusedleftclickrow, out rightclickrow);
            rightclicktag = (rightclickrow != -1) ? Rows[rightclickrow].Tag : null;
            ContextMenuStrip.Items[3].Enabled = rightclicktag is HistoryEntry;
        }

        private ISystem SysFrom(Object t)   // given tag, find the isystem
        {
            if (t is HistoryEntry)
                return ((HistoryEntry)t).System;
            else
                return (ISystem)t;
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicktag != null)
            {
                this.Cursor = Cursors.WaitCursor;
                EDSMClass edsm = new EDSMClass();
                long? id_edsm = SysFrom(rightclicktag).EDSMID;

                if (id_edsm == 0)
                {
                    id_edsm = null;
                }

                if (!edsm.ShowSystemInEDSM(SysFrom(rightclicktag).Name, id_edsm))
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable");

                this.Cursor = Cursors.Default;
            }
        }

        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicktag != null)
            {
                this.Cursor = Cursors.WaitCursor;

                if (!discoveryform.Map.Is3DMapsRunning)            // if not running, click the 3dmap button
                    discoveryform.Open3DMap(null);

                this.Cursor = Cursors.Default;

                if (discoveryform.Map.Is3DMapsRunning)             // double check here! for paranoia.
                {
                    if (discoveryform.Map.MoveToSystem(SysFrom(rightclicktag)))
                        discoveryform.Map.Show();
                }
            }
        }

        private void GotoEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicktag != null)
            {
                GotoEntryClicked?.Invoke(rightclicktag as HistoryEntry);
            }
        }

        private void viewScanOfSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowScanPopOut(rightclicktag);
        }

        private void cellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                ShowScanPopOut(Rows[e.RowIndex].Tag);
        }

        void ShowScanPopOut(Object tag)     // tag can be a Isystem or an He.. output depends on it.
        {
            ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), tag, CheckEDSM, discoveryform.history);
        }

        public void Excel(int columnsout)
        {
            if (Rows.Count == 0)
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
                        if (r < Rows.Count)
                        {
                            return BaseUtils.CSVWriteGrid.LineStatus.OK;
                        }
                        else
                            return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                    };

                    List<string> colh = new List<string>();
                    for (int i = 0; i < columnsout; i++)
                        colh.Add(Columns[i].HeaderText);
                    colh.AddRange(new string[] { "X", "Y", "Z", "ID" });

                    grd.GetHeader += delegate (int c)
                    {
                        return (c < colh.Count && frm.IncludeHeader) ? colh[c] : null;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        DataGridViewRow rw = Rows[r];
                        ISystem sys = SysFrom(rw.Tag);
                        List<Object> data = new List<object>();
                        for (int i = 0; i < columnsout; i++)
                            data.Add(rw.Cells[i].Value);

                        data.Add(sys.X.ToString("0.#"));
                        data.Add(sys.Y.ToString("0.#"));
                        data.Add(sys.Z.ToString("0.#"));
                        data.Add(sys.EDSMID);

                        return data.ToArray();
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }
        }



    }
}
