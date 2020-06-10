/*
 * Copyright © 2017-2019 EDDiscovery development team
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
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.JournalEvents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlExploration :  UserControlCommonBase
    {
        private string DbColumnSave { get { return DBName("UserControlExploration",  "DGVCol"); } }

        private ExplorationSetClass currentexplorationset;

        public int JounalScan { get; private set; }

        #region Initialisation

        public UserControlExploration()
        {
            InitializeComponent();
            var corner = dataGridViewExplore.TopLeftHeaderCell; // work around #1487
            ColumnSystemName.AutoCompleteGenerator = SystemCache.ReturnSystemAutoCompleteList;
            currentexplorationset = new ExplorationSetClass();
        }

        public override void Init()
        {
            currentexplorationset = new ExplorationSetClass();
            discoveryform.OnNewEntry += NewEntry;
            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolStrip, this);
            BaseUtils.Translator.Instance.Translate(contextMenuCopyPaste, this);
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
            DGVLoadColumnLayout(dataGridViewExplore, DbColumnSave);

            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList += OnNewStars;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList -= OnNewStars;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList += OnNewStars;
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewExplore, DbColumnSave);
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= NewEntry;
            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList -= OnNewStars;
        }

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made.. check to see if its a scan update
        {
            if (he.EntryType == JournalTypeEnum.Scan || he.IsFSDCarrierJump || he.EntryType == JournalTypeEnum.FSSDiscoveryScan )
                UpdateSystemRows();
        }

        public override void InitialDisplay()
        {
            UpdateSystemRows();
        }

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)            // when user clicks around..
        {
            UpdateSystemRows();
        }

        #endregion

        #region Updating info due to new record coming in

        private void OnNewStars(List<string> obj, OnNewStarsPushType command)    // and if a user asked for stars to be added
        {
            if (command == OnNewStarsPushType.Exploration)
                AddSystems(obj);
        }

        private void UpdateSystemRows()
        {
            for (int i = 0; i < dataGridViewExplore.Rows.Count; i++)
            {
                UpdateSystemRow(i);
                dataGridViewExplore.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        private void UpdateSystemRow(int rowindex)
        {
            const int idxVisits = 5;
            const int idxScans = 6;
            const int idxBodies= 7;
            const int idxstars = 8;
            const int idxInfo = 9;
            const int idxNote = 10;

            ISystem currentSystem = discoveryform.history.CurrentSystem; // may be null

            if (rowindex < dataGridViewExplore.Rows.Count && dataGridViewExplore[0, rowindex].Value != null)
            {
                string sysname = dataGridViewExplore[0, rowindex].Value.ToString();
                ISystem sys = (ISystem)dataGridViewExplore[0, rowindex].Tag;

                if (sys == null)
                    sys = discoveryform.history.FindSystem(sysname);

                if (sys != null && currentSystem != null)
                {
                    double dist = sys.Distance(currentSystem);
                    string strdist = dist >= 0 ? ((double)dist).ToString("0.00") : "";
                    dataGridViewExplore[1, rowindex].Value = strdist;
                }

                dataGridViewExplore[0, rowindex].Tag = sys;
                dataGridViewExplore.Rows[rowindex].DefaultCellStyle.ForeColor = (sys != null && sys.HasCoordinate) ? discoveryform.theme.VisitedSystemColor : discoveryform.theme.NonVisitedSystemColor;

                if (sys != null)
                {
                    if (sys.HasCoordinate)
                    {
                        dataGridViewExplore[2, rowindex].Value = sys.X.ToString("0.00");
                        dataGridViewExplore[3, rowindex].Value = sys.Y.ToString("0.00");
                        dataGridViewExplore[4, rowindex].Value = sys.Z.ToString("0.00");
                    }

                    dataGridViewExplore[idxVisits, rowindex].Value = discoveryform.history.GetVisitsCount(sysname).ToString();

                    StarScan.SystemNode sysnode = discoveryform.history.starscan.FindSystemSynchronous(sys,false);
                    
                    if ( sysnode != null)
                    {
                        dataGridViewExplore[idxScans, rowindex].Value = sysnode.StarPlanetsScanned().ToString();
                        if (sysnode.FSSTotalBodies.HasValue)
                            dataGridViewExplore[idxBodies, rowindex].Value = sysnode.FSSTotalBodies.Value.ToString();

                        dataGridViewExplore[idxstars, rowindex].Value = sysnode.StarTypesFound(false);

                        string info = "";
                        foreach( var scan in sysnode.Bodies)
                        {
                            JournalScan sd = scan.ScanData;
                            if (sd != null)
                            {
                                if (sd.IsStar)
                                {
                                    if (sd.StarTypeID == EDStar.AeBe)
                                        info = info + " " + "AeBe";
                                    if (sd.StarTypeID == EDStar.N)
                                        info = info + " " + "NS";
                                    if (sd.StarTypeID == EDStar.H)
                                        info = info + " " + "BH";
                                }
                                else
                                {
                                    if (sd.PlanetTypeID == EDPlanet.Earthlike_body)
                                    {
                                        info = info + " " + (sd.Terraformable ? "T-ELW" : "ELW");
                                    }
                                    else if (sd.PlanetTypeID == EDPlanet.Water_world)
                                        info = info + " " + (sd.Terraformable ? "T-WW" : "WW");
                                    else if (sd.PlanetTypeID == EDPlanet.High_metal_content_body && sd.Terraformable)
                                        info = info + " " + "T-HMC";
                                }
                            }

                        }

                        dataGridViewExplore[idxInfo, rowindex].Value = info.Trim();
                    }

                    string note = "";
                    SystemNoteClass sn = SystemNoteClass.GetNoteOnSystem(sys.Name, sys.EDSMID);
                    if (sn != null && !string.IsNullOrWhiteSpace(sn.Note))
                    {
                        note = sn.Note;
                    }
                    else
                    {
                        BookmarkClass bkmark = GlobalBookMarkList.Instance.FindBookmarkOnSystem(sys.Name);
                        if (bkmark != null && !string.IsNullOrWhiteSpace(bkmark.Note))
                            note = bkmark.Note;
                        else
                        {
                            var gmo = discoveryform.galacticMapping.Find(sys.Name);
                            if (gmo != null && !string.IsNullOrWhiteSpace(gmo.description))
                                note = gmo.description;
                        }
                    }
                    dataGridViewExplore[idxNote, rowindex].Value = note.WordWrap(60);
                }

                if (sys == null && sysname != "")
                {
                    dataGridViewExplore.Rows[rowindex].ErrorText = "System not known".T(EDTx.Systemnotknown);
                }
                else
                {
                    dataGridViewExplore.Rows[rowindex].ErrorText = "";
                }
            }
        }

        #endregion

        #region  Tool strip


        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            string file = currentexplorationset.DialogNew(FindForm());

            if ( file != null )
            {
                textBoxFileName.Text = file;
                UpdateExplorationInfo(currentexplorationset);
                ClearExplorationSet();
            }
        }

        private void toolStripButtonLoad_Click(object sender, EventArgs e)
        {
            string file = currentexplorationset.DialogLoad(this.FindForm());

            if ( file != null )
            {
                textBoxFileName.Text = file;
                textBoxRouteName.Text = currentexplorationset.Name;

                dataGridViewExplore.Rows.Clear();
                foreach (var sysname in currentexplorationset.Systems)
                    dataGridViewExplore.Rows.Add(sysname, "", "");

                UpdateSystemRows();
            }
        }


        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            UpdateExplorationInfo(currentexplorationset);

            if (String.IsNullOrEmpty(textBoxFileName.Text))
            {
                string file = currentexplorationset.DialogSave(FindForm());

                if (file != null)
                    textBoxFileName.Text = file;
            }
            else
            {
                currentexplorationset.Save(textBoxFileName.Text);
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), string.Format("Saved to {0} Exploration Set".T(EDTx.UserControlExploration_Saved), textBoxFileName.Text));
            }
        }


        private void toolStripButtonImportFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files|*.txt";
            ofd.Title = "Select a exploration set file".T(EDTx.UserControlExploration_SelectSet);

            if (ofd.ShowDialog(FindForm()) != System.Windows.Forms.DialogResult.OK)
                return;

            string[] sysnames;

            try
            {
                sysnames = System.IO.File.ReadAllLines(ofd.FileName);
            }
            catch (IOException)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), string.Format("There was a problem opening file {0}".T(EDTx.UserControlExploration_OpenE), ofd.FileName), "Warning".T(EDTx.Warning),
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<String> systems = new List<String>();
            int countunknown = 0;
            foreach (String name in sysnames)
            {
                String sysname = name;
                if (sysname.Contains(","))
                {
                    String[] values = sysname.Split(',');
                    sysname = values[0];
                }
                if (String.IsNullOrWhiteSpace(sysname))
                    continue;
                ISystem sc = SystemCache.FindSystem(sysname.Trim());
                if (sc == null)
                {
                    sc = new SystemClass(sysname.Trim());
                    countunknown++;
                }
                systems.Add(sc.Name);

            }

            if (systems.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(),
                    "The imported file contains no known system names".T(EDTx.UserControlExploration_NoSys), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ClearExplorationSet();

            AddSystems(systems);
        }

        void AddSystems(List<string> systems)
        { 
            foreach (var sysname in systems)
                dataGridViewExplore.Rows.Add(sysname, "", "");

            UpdateSystemRows();
        }


        private void toolStripButtonExport_Click(object sender, EventArgs e)
        {
            if (dataGridViewExplore.Rows.Count == 0
                || (dataGridViewExplore.Rows.Count == 1 && ((string)dataGridViewExplore[0, 0].Value).IsEmpty()))
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(),
                "There is no route to export".T(EDTx.UserControlExploration_NoRoute), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Forms.ExportForm frm = new Forms.ExportForm();
            frm.Init(new string[] { "Route", "Grid" }, disablestartendtime: true, outputext: new string[] { "Text File|*.txt", "CSV export| *.csv" });

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (frm.SelectedIndex == 0)     // old route export
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(frm.Path, false))
                        {
                            for (int i = 0; i < dataGridViewExplore.Rows.Count; i++)
                            {
                                String sysname = (String)dataGridViewExplore[0, i].Value;
                                if (!String.IsNullOrWhiteSpace(sysname))
                                    writer.WriteLine(sysname);
                            }
                        }

                        if (frm.AutoOpen)
                        {
                            try
                            {
                                System.Diagnostics.Process.Start(frm.Path);
                            }
                            catch
                            {
                                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Failed to open " + frm.Path, "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }

                    }
                    catch (IOException)
                    {
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), string.Format("Error exporting route. Is file {0} open?".T(EDTx.UserControlExploration_ErrorW), frm.Path), "Warning".T(EDTx.Warning),
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                    grd.SetCSVDelimiter(frm.Comma);
                    grd.GetLineStatus += delegate (int r)
                    {
                        if (r < dataGridViewExplore.Rows.Count)
                        {
                            return BaseUtils.CSVWriteGrid.LineStatus.OK;
                        }
                        else
                            return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        DataGridViewRow rw = dataGridViewExplore.Rows[r];
                        return new Object[] { rw.Cells[0].Value, rw.Cells[1].Value, rw.Cells[2].Value, rw.Cells[3].Value,
                                              rw.Cells[4].Value, rw.Cells[5].Value, rw.Cells[6].Value, rw.Cells[7].Value,
                                              rw.Cells[8].Value, rw.Cells[9].Value, rw.Cells[10].Value };
                    };

                    grd.GetHeader += delegate (int c)
                    {
                        return (c < 3 && frm.IncludeHeader) ? dataGridViewExplore.Columns[c].HeaderText : null;
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            if (ExtendedControls.MessageBoxTheme.Show(FindForm(), "Are you sure you want to clear the route list?".T(EDTx.UserControlExploration_Clear), "Warning".T(EDTx.Warning), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ClearExplorationSet();
            }
        }


        private void tsbAddSystems_Click(object sender, EventArgs e)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            FindSystemsUserControl usc = new FindSystemsUserControl();
            usc.ReturnSystems = (List<Tuple<ISystem, double>> syslist) =>
            {
                List<String> systems = new List<String>();
                int countunknown = 0;
                foreach (Tuple<ISystem, double> ret in syslist)
                {
                    string name = ret.Item1.Name;

                    ISystem sc = SystemCache.FindSystem(name.Trim());
                    if (sc == null)
                    {
                        sc = new SystemClass(name.Trim());
                        countunknown++;
                    }
                    systems.Add(sc.Name);
                }

                if (systems.Count == 0)
                {
                    ExtendedControls.MessageBoxTheme.Show(FindForm(), "The imported file contains no known system names".T(EDTx.UserControlExploration_NoSys),
                        "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                    AddSystems(systems);

                f.ReturnResult(DialogResult.OK);
            };

            f.Add(new ExtendedControls.ConfigurableForm.Entry("UC", null, "", new Point(5, 30), usc.Size, null) { control = usc });
            f.AddCancel(new Point(4+usc.Width - 80, usc.Height + 50));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "Cancel" || controlname == "Close")
                {
                    f.ReturnResult(DialogResult.Cancel);
                }
            };

            // usc.Font = EDDTheme.Instance.GetScaledFont(0.8f)
            f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "Add Systems".T(EDTx.UserControlExploration_AddSys), 
                                callback: () => {; usc.Init(0, "ExplorationFindSys", false, discoveryform); }, closeicon:true);
            usc.Closing();
        }


        #endregion

        #region Cell data

        private void dataGridViewRouteSystems_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string sysname = e.FormattedValue.ToString();
                var row = dataGridViewExplore.Rows[e.RowIndex];
                var cell = dataGridViewExplore[e.ColumnIndex, e.RowIndex];

                ISystem sys = SystemCache.FindSystem(sysname);

                EDSMClass edsm = new EDSMClass();

                if (sysname != "" && sys == null && !edsm.IsKnownSystem(sysname))
                {
                    row.ErrorText = "System not known to EDSM".T(EDTx.UserControlExploration_EDSMUnk);
                }
                else
                {
                    row.ErrorText = "";
                }
            }
        }

        private void dataGridViewExplore_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                UpdateSystemRow(e.RowIndex);
                dataGridViewExplore.Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex + 1).ToString();
            }
        }

        #endregion

        #region right clicks

        private void contextMenuCopyPaste_Opening(object sender, CancelEventArgs e)
        {
            bool hastext = false;

            try
            {
                hastext = Clipboard.ContainsText();
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Unable to access clipboard");
            }

            if (hastext)
            {
                pasteToolStripMenuItem.Enabled = true;
                insertCopiedToolStripMenuItem.Enabled = true;
            }
            else
            {
                pasteToolStripMenuItem.Enabled = false;
                insertCopiedToolStripMenuItem.Enabled = false;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataObject obj = dataGridViewExplore.GetClipboardContent();

            try
            {
                Clipboard.SetDataObject(obj);
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Unable to access clipboard");
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string data = null;

            try
            {
                data = Clipboard.GetText();
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Unable to access clipboard");
            }

            if (data != null)
            {
                var rows = data.Replace("\r", "").Split('\n').Where(r => r != "").ToArray();
                int[] selectedRows = dataGridViewExplore.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridViewExplore.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
                int insertRow = selectedRows.FirstOrDefault();
                foreach (int index in selectedRows.Reverse())
                {
                    dataGridViewExplore.Rows.RemoveAt(index);
                }
                InsertRows(insertRow, rows);
            }
        }

        private void insertCopiedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string data = null;

            try
            {
                data = Clipboard.GetText();
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Unable to access clipboard");
            }

            if (data != null)
            {
                var rows = data.Replace("\r", "").Split('\n').Where(r => r != "").ToArray();
                int[] selectedRows = dataGridViewExplore.SelectedCells.OfType<DataGridViewCell>().Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
                int insertRow = selectedRows.FirstOrDefault();
                InsertRows(insertRow, rows);
            }
        }

        private void deleteRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridViewExplore.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridViewExplore.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
            foreach (int index in selectedRows.Reverse())
            {
                dataGridViewExplore.Rows.RemoveAt(index);
            }
            UpdateSystemRows();
        }

        private void setTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridViewExplore.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridViewExplore.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();

            if (selectedRows.Length == 0)
                return;
            var obj = dataGridViewExplore[0, selectedRows[0]].Value;

            if (obj == null)
                return;
            TargetHelpers.SetTargetSystem(this, discoveryform, (string)obj);
        }

        private void editBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridViewExplore.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridViewExplore.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();

            if (selectedRows.Length == 0)
                return;
            var obj = dataGridViewExplore[0, selectedRows[0]].Value;

            if (obj == null)
                return;
            ISystem sc = SystemCache.FindSystem((string)obj);
            if (sc == null)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Unknown system, system is without co-ordinates".T(EDTx.UserControlExploration_UnknownS), "Warning".T(EDTx.Warning), MessageBoxButtons.OK);
            }
            else
                TargetHelpers.ShowBookmarkForm(this, discoveryform, sc, null, false);
        }

        #endregion

        #region Helpers

        private void dataGridViewExplore_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 1 && e.Column.Index <= 6)
                e.SortDataGridViewColumnNumeric();

        }


        public void InsertRows(int insertIndex, params string[] sysnames)
        {
            foreach (var row in sysnames)
            {
                dataGridViewExplore.Rows.Insert(insertIndex, row, "", "", "", "", "", "", "", "");
                insertIndex++;
            }
            UpdateSystemRows();
        }

        private void UpdateExplorationInfo(ExplorationSetClass route)
        {
            route.Name = textBoxRouteName.Text.Trim();
            route.Systems.Clear();
            route.Systems.AddRange(dataGridViewExplore.Rows.OfType<DataGridViewRow>().Where(r => !string.IsNullOrEmpty((string)r.Cells[0].Value)).Select(r => r.Cells[0].Value.ToString()));
        }

        private void ClearExplorationSet()
        {
            currentexplorationset = new ExplorationSetClass { Name = "" };
            dataGridViewExplore.Rows.Clear();
            textBoxRouteName.Text = "";
        }

        #endregion
    }

    public class ExplorationSetClass
    {
        public string Name { get; set; }
        public List<string> Systems { get; private set; }

        public ExplorationSetClass()
        {
            this.Systems = new List<string>();
        }

        public void Save(string fileName)
        {
            JObject jo = new JObject();

            if (string.IsNullOrEmpty(fileName))
                return;

            jo["Name"] = Name;
            jo["Systems"] = new JArray(Systems);

            File.WriteAllText(fileName, jo.ToString());
        }


        public bool Load(string fileName)
        {
            try
            {
                JObject jo = JObject.Parse(File.ReadAllText(fileName));

                Name = jo["Name"].ToString();

                Systems.Clear();

                JArray ja = (JArray)jo["Systems"];

                foreach (var jsys in ja)
                {
                    string sysname = jsys.Value<String>();
                    if (!Systems.Contains(sysname))
                        Systems.Add(sysname);
                }

                return true;
            }
            catch { }

            return false;
        }

        public void Clear()
        {
            Name = "";
            Systems.Clear();
        }

        public string DialogLoad(Form parent)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = EDDOptions.Instance.ExploreAppDirectory();
            dlg.DefaultExt = "json";
            dlg.AddExtension = true;
            dlg.Filter = "Explore file| *.json";

            if (dlg.ShowDialog(parent) == DialogResult.OK)
            {
                Clear();
                if (Load(dlg.FileName))
                    return dlg.FileName;
            }

            return null;
        }

        public string DialogNew(Form parent)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.InitialDirectory = EDDOptions.Instance.ExploreAppDirectory();
            dlg.OverwritePrompt = true;
            dlg.DefaultExt = "json";
            dlg.AddExtension = true;
            dlg.Filter = "Explore file| *.json";

            if (dlg.ShowDialog(parent) == DialogResult.OK)
            {
                Clear();
                return dlg.FileName;
            }

            return null;
        }

        public string DialogSave(Form parent)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.InitialDirectory = EDDOptions.Instance.ExploreAppDirectory();
            dlg.OverwritePrompt = true;
            dlg.DefaultExt = "json";
            dlg.AddExtension = true;
            dlg.Filter = "Explore file| *.json";

            if (dlg.ShowDialog(parent) == DialogResult.OK)
            {
                Save(dlg.FileName);
                return dlg.FileName;
            }

            return null;
        }
    }
}
