﻿/*
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
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EDDiscovery.UserControls.Search
{
    public class DataGridViewStarResults : BaseUtils.DataGridViewColumnControl
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
            cms.Items[3].Name = "Goto";
            cms.Items[3].Click += new System.EventHandler(this.GotoEntryToolStripMenuItem_Click);
            cms.Opening += Cms_Opening;
            CellDoubleClick += cellDoubleClick;

            var enumlistcms = new Enum[] { EDTx.DataGridViewStarResults_3d, EDTx.DataGridViewStarResults_EDSM, EDTx.DataGridViewStarResults_Data, EDTx.DataGridViewStarResults_Goto };
            BaseUtils.Translator.Instance.TranslateToolstrip(cms, enumlistcms,this);
        }

        public void Init(EDDiscoveryForm frm)
        {
            discoveryform = frm;
        }

        Object rightclicktag = null;

        private void Cms_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            rightclicktag = RightClickRowValid ? Rows[RightClickRow].Tag : null;
            ContextMenuStrip.Items[3].Visible = rightclicktag is HistoryEntry;
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
                if (!edsm.ShowSystemInEDSM(SysFrom(rightclicktag).Name))
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable");

                this.Cursor = Cursors.Default;
            }
        }

        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicktag != null)
            {
                discoveryform.Open3DMap(SysFrom(rightclicktag));
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
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var coltag = Columns[e.ColumnIndex].Tag as string;
                var cell = Rows[e.RowIndex].Cells[e.ColumnIndex];

                string text = null;

                bool textpopout = coltag?.Contains("TextPopOut") ?? false;
                bool tooltippopout = coltag?.Contains("TooltipPopOut") ?? false;

                if (textpopout)
                {
                    if (tooltippopout)
                    {
                        text = cell.ToolTipText.HasChars() ? cell.ToolTipText : cell.Value as string;
                    }
                    else
                    {
                        text = cell.Value as string;
                    }
                }
                else if ( tooltippopout)
                {
                    text = cell.ToolTipText;
                }

                if (text.HasChars())
                {
                    InfoForm frm = new InfoForm();
                    frm.Info(this.Columns[e.ColumnIndex].HeaderText, FindForm().Icon, text);
                    frm.Show(FindForm());
                }
                else if (Rows[e.RowIndex].Tag != null)
                {
                    ShowScanPopOut(Rows[e.RowIndex].Tag);
                }
            }
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
            frm.Init(false, new string[] { "Export Current View" }, showflags: new Forms.ExportForm.ShowFlags[] { Forms.ExportForm.ShowFlags.DisableDateTime });

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
                    colh.AddRange(new string[] { "X", "Y", "Z" });

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

                        return data.ToArray();
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }
        }



    }
}
