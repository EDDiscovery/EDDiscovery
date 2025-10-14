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
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EDDiscovery.UserControls.Search
{
    public class DataGridViewStarResults : BaseUtils.DataGridViewColumnControl
    {
        public EliteDangerousCore.WebExternalDataLookup WebLookup { get; set; }
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
            new System.Windows.Forms.ToolStripMenuItem(),
            new System.Windows.Forms.ToolStripMenuItem(),
            });
            
            cms.Name = "historyContextMenu";
            cms.Size = new System.Drawing.Size(187, 70);

            cms.Items[0].Size = new System.Drawing.Size(186, 22);
            cms.Items[0].Text = "View Scan Display";
            cms.Items[0].Name = "Data";
            cms.Items[0].Click += new System.EventHandler(this.viewScanOfSystemToolStripMenuItem_Click);

            cms.Items[1].Size = new System.Drawing.Size(186, 22);
            cms.Items[1].Text = "Go to star on 3D Map";
            cms.Items[1].Name = "3d";
            cms.Items[1].Click += new System.EventHandler(this.mapGotoStartoolStripMenuItem_Click);

            cms.Items[2].Size = new System.Drawing.Size(186, 22);
            cms.Items[2].Text = "View on Spansh";
            cms.Items[2].Name = "Spansh";
            cms.Items[2].Click += new System.EventHandler(this.viewOnSpanshToolStripMenuItem_Click);

            cms.Items[3].Size = new System.Drawing.Size(186, 22);
            cms.Items[3].Text = "View on EDSM";
            cms.Items[3].Name = "EDSM";
            cms.Items[3].Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem_Click);

            cms.Items[4].Text = "View on History Grid";
            cms.Items[4].Name = "Goto";
            cms.Items[4].Click += new System.EventHandler(this.GotoEntryToolStripMenuItem_Click);

            cms.Opening += Cms_Opening;
            CellDoubleClick += cellDoubleClick;

            // we do not translate ourselves, thats up to the parent control to do
            // but we do translate our toolstrips/tooltips
            BaseUtils.TranslatorMkII.Instance.TranslateToolstrip(cms);
        }

        public void Init(EDDiscoveryForm frm)
        {
            discoveryform = frm;
        }

        Object rightclicktag = null;

        private void Cms_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            rightclicktag = RightClickRowValid ? Rows[RightClickRow].Tag : null;

            ContextMenuStrip.Items[2].Visible = SystemClassFrom(rightclicktag)?.SystemAddress.HasValue ?? false;        // spansh
            ContextMenuStrip.Items[4].Visible = GotoEntryClicked != null;
        }

        private ISystem SystemClassFrom(Object t)   // given tag, find the isystem, may be null. 
        {
            if (t is HistoryEntry)
                return ((HistoryEntry)t).System;
            else if (t is ISystem)
                return (ISystem)t;
            else
                return null;
        }

        private void viewOnEDSMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sys = SystemClassFrom(rightclicktag);       // if rightclicktag == null, then we get null.

            if (sys != null)
            {
                this.Cursor = Cursors.WaitCursor;
                EDSMClass edsm = new EDSMClass();
                if (!edsm.ShowSystemInEDSM(sys.Name))
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable");

                this.Cursor = Cursors.Default;
            }
        }
        private void viewOnSpanshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sys = SystemClassFrom(rightclicktag);       // if rightclicktag == null, then we get null.

            if (sys != null && sys.SystemAddress != null)
            {
                EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForSystem(sys.SystemAddress.Value);
            }
        }

        private void mapGotoStartoolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sys = SystemClassFrom(rightclicktag);       // if rightclicktag == null, then we get null.
            if (sys != null)
            {
                discoveryform.Open3DMap(SystemClassFrom(rightclicktag));
            }
        }

        private void GotoEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rightclicktag != null && rightclicktag is HistoryEntry)
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
                    text = cell.Value as string;

                    if (tooltippopout)
                    {
                        text += cell.ToolTipText.HasChars() ? (Environment.NewLine + cell.ToolTipText) : "";
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
            ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), tag, discoveryform.History, forcedlookup: WebLookup);
        }

        public void Excel(int columnsout)
        {
            if (Rows.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No data to export", "Export EDSM", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Forms.ImportExportForm frm = new Forms.ImportExportForm();
            frm.Export( new string[] { "Export Current View" }, new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude },
                suggestedfilenamesp: new string [] { "stargrid.csv" });

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (frm.SelectedIndex == 0)
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid(frm.Delimiter);

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
                    colh.AddRange(new string[] { "X", "Y", "Z" , "System Address"});

                    grd.GetHeader += delegate (int c)
                    {
                        return (c < colh.Count && frm.IncludeHeader) ? colh[c] : null;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        DataGridViewRow rw = Rows[r];
                        List<Object> data = new List<object>();
                        for (int i = 0; i < columnsout; i++)
                        {
                            if (double.TryParse(rw.Cells[i].Value as string, out double v))     // if its a number, export as a number
                                data.Add(v);
                            else
                                data.Add(rw.Cells[i].Value);
                        }

                        ISystem sys = SystemClassFrom(rw.Tag);
                        
                        if (sys != null)        // in case we don't have a valid tag
                        {
                            data.Add(sys.X);
                            data.Add(sys.Y);
                            data.Add(sys.Z);
                            data.Add(sys.SystemAddress ?? 0);
                        }

                        return data.ToArray();
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }
        }



    }
}
