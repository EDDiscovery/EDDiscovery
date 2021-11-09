/*
 * Copyright © 2016 - 2020 EDDiscovery development team
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
using BaseUtils;
using BaseUtils.JSON;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using ExtendedControls;
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
    public partial class UserControlExpedition : UserControlCommonBase
    {
        private SavedRouteClass loadedroute;           // this current route, null until created
        private List<ISystem> latestplottedroute;

        #region Standard UC Interfaces

        public UserControlExpedition()
        {
            InitializeComponent();
            SystemName.AutoCompleteGenerator = SystemCache.ReturnSystemAutoCompleteList;
        }

        public override void Init()
        {
            DBBaseName = "UserControlExpedition";

            dateTimePickerEndDate.Value = dateTimePickerEndTime.Value = dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);

            discoveryform.OnNewCalculatedRoute += discoveryForm_OnNewCalculatedRoute;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            discoveryform.OnNoteChanged += Discoveryform_OnNoteChanged;

            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
            BaseUtils.Translator.Instance.Translate(contextMenuCopyPaste, this);
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView);

            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList += OnNewStars;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList -= OnNewStars;
            uctg = thc;
            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList += OnNewStars;
        }


        public override bool AllowClose()
        {
            return PromptAndSaveIfNeeded();
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView);

            discoveryform.OnNewCalculatedRoute -= discoveryForm_OnNewCalculatedRoute;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            discoveryform.OnNoteChanged -= Discoveryform_OnNoteChanged;

            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).OnNewStarList -= OnNewStars;
        }

        private void Discoveryform_OnNoteChanged(object arg1, HistoryEntry arg2, bool arg3)
        {
            UpdateSystemRows();
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            UpdateSystemRows();
        }
        
        private void discoveryForm_OnNewCalculatedRoute(List<ISystem> obj) // called when a new route is calculated
        {
            latestplottedroute = obj;
        }

        private void OnNewStars(List<string> list, OnNewStarsPushType command)    // and if a user asked for stars to be added
        {
            if (command == OnNewStarsPushType.Expedition)
                AppendRows(list);
        }

        #endregion


        #region Grid Display Route and update when required

        private void DisplayRoute(SavedRouteClass route)
        {
            dataGridView.Rows.Clear();

            if (route == null)
                return;

            textBoxRouteName.Text = route.Name;
            if (route.StartDateUTC == null)
            {
                dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
                dateTimePickerStartTime.Checked = dateTimePickerStartDate.Checked = false;
            }
            else
            {
                dateTimePickerStartTime.Checked = dateTimePickerStartDate.Checked = true;
                dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(route.StartDateUTC.Value);
            }

            if (route.EndDateUTC == null)
            {
                dateTimePickerEndTime.Value = dateTimePickerEndDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
                dateTimePickerEndTime.Checked = dateTimePickerEndDate.Checked = false;
            }
            else
            {
                dateTimePickerEndTime.Checked = dateTimePickerEndDate.Checked = true;
                dateTimePickerEndTime.Value = dateTimePickerEndDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(route.EndDateUTC.Value);
            }

            foreach (string sysname in route.Systems)
            {
                dataGridView.Rows.Add(sysname,"","");
            }

            UpdateSystemRows();
        }

        private void UpdateSystemRows()
        {
            Cursor = Cursors.WaitCursor;

            ISystem currentSystem = discoveryform.history.CurrentSystem(); // may be null

            for (int rowindex = 0; rowindex < dataGridView.Rows.Count; rowindex++)
            {
                dataGridView[1, rowindex].ReadOnly = true;
                dataGridView[2, rowindex].ReadOnly = true;
                dataGridView[3, rowindex].ReadOnly = true;
                dataGridView[4, rowindex].ReadOnly = true;
                dataGridView[5, rowindex].ReadOnly = true;

                string sysname = dataGridView[0, rowindex].Value as string;       // value may be null, so protect (2999)

                if (sysname.HasChars())
                {
                    // find in history, and the DB, and EDSM, the system..

                    var sys = discoveryform.history.FindSystem(sysname, discoveryform.galacticMapping, true);      

                    dataGridView[1, rowindex].Value = "";

                    if (rowindex > 0 && dataGridView[0, rowindex - 1].Value != null && dataGridView[0, rowindex].Value != null)
                    {
                        string prevsysname = dataGridView[0, rowindex - 1].Value as string;     // protect against null

                        if (prevsysname.HasChars())
                        {
                            var prevsys = discoveryform.history.FindSystem(prevsysname, discoveryform.galacticMapping, true);       // use EDSM directly

                            if ((sys?.HasCoordinate ?? false) && (prevsys?.HasCoordinate ?? false))
                            {
                                double dist = sys.Distance(prevsys);
                                string strdist = dist >= 0 ? ((double)dist).ToString("0.00") : "";
                                dataGridView[1, rowindex].Value = strdist;
                            }
                        }
                    }

                    dataGridView[0, rowindex].Tag = sys;
                    dataGridView.Rows[rowindex].Cells[0].Style.ForeColor = (sys != null && sys.HasCoordinate) ? Color.Empty : discoveryform.theme.UnknownSystemColor;

                    string note = "";
                    SystemNoteClass sn = SystemNoteClass.GetNoteOnSystem(sysname);
                    if (sn != null && !string.IsNullOrWhiteSpace(sn.Note))
                        note = sn.Note;

                    BookmarkClass bkmark = GlobalBookMarkList.Instance.FindBookmarkOnSystem(sysname);
                    if (bkmark != null && !string.IsNullOrWhiteSpace(bkmark.Note))
                        note = note.AppendPrePad(bkmark.Note, "; ");

                    var gmo = discoveryform.galacticMapping.Find(sysname);
                    if (gmo != null && !string.IsNullOrWhiteSpace(gmo.Description))
                        note = note.AppendPrePad(gmo.Description, "; ");

                    dataGridView[2, rowindex].Value = note;

                    if (sys != null && sys.HasCoordinate)
                    {
                        dataGridView[3, rowindex].Value = sys.X.ToString("0.0.#");
                        dataGridView[4, rowindex].Value = sys.Y.ToString("0.0.#");
                        dataGridView[5, rowindex].Value = sys.Z.ToString("0.0.#");
                        dataGridView.Rows[rowindex].ErrorText = "";

                        if (currentSystem?.HasCoordinate ?? false)
                        {
                            double dist = sys.Distance(currentSystem);
                            string strdist = dist >= 0 ? ((double)dist).ToString("0.00") : "";
                            dataGridView[6, rowindex].Value = strdist;
                        }

                        StarScan.SystemNode sysnode = discoveryform.history.StarScan.FindSystemSynchronous(sys, false);

                        if (sysnode != null)
                        {
                            dataGridView[8, rowindex].Value = sysnode.StarPlanetsScanned().ToString();
                            if (sysnode.FSSTotalBodies.HasValue)
                                dataGridView[9, rowindex].Value = sysnode.FSSTotalBodies.Value.ToString();

                            dataGridView[10, rowindex].Value = sysnode.StarTypesFound(false);

                            string info = "";
                            foreach (var scan in sysnode.Bodies)
                            {
                                EliteDangerousCore.JournalEvents.JournalScan sd = scan.ScanData;
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

                            dataGridView[11, rowindex].Value = info.Trim();
                        }

                    }
                    else
                        dataGridView.Rows[rowindex].ErrorText = "System not known location".T(EDTx.UserControlExpedition_EDSMUnk);



                }
            }

            txtCmlDistance.Text = txtP2PDIstance.Text = "";

            if (dataGridView.Rows.Count > 1)
            {
                double distance = 0;
                ISystem firstSC = null;
                ISystem lastSC = null;
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    if (firstSC == null && dataGridView[0, i].Tag != null)
                        firstSC = (ISystem)dataGridView[0, i].Tag;
                    if (dataGridView[0, i].Tag != null)
                        lastSC = (ISystem)dataGridView[0, i].Tag;
                    String value = dataGridView[1, i].Value as string;
                    if (!String.IsNullOrWhiteSpace(value))
                        distance += Double.Parse(value);
                }

                txtCmlDistance.Text = distance.ToString("0.00") + "LY";

                if (firstSC != null && lastSC != null)
                {
                    distance = firstSC.Distance(lastSC);
                    txtP2PDIstance.Text = distance.ToString("0.00") + "LY";
                }
            }

            Cursor = Cursors.Default;
        }
        public void AppendRows(IEnumerable<string> sysnames)
        {
            foreach (var system in sysnames)
            {
                dataGridView.Rows.Add(system);
            }
            UpdateSystemRows();
        }

        public void InsertRows(int insertIndex, IEnumerable<string> sysnames)
        {
            foreach (var system in sysnames)
            {
                dataGridView.Rows.Insert(insertIndex, system);
                insertIndex++;
            }
            UpdateSystemRows();
        }

        // autopaint the row number..
        private void dataGridViewRouteSystems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {           
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            if (!e.IsLastVisibleRow)
            {
                var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);

                // right alignment might actually make more sense for numbers
                using (var centerFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    using (Brush br = new SolidBrush(grid.RowHeadersDefaultCellStyle.ForeColor))
                        e.Graphics.DrawString(rowIdx, grid.RowHeadersDefaultCellStyle.Font, br, headerBounds, centerFormat);
                }
            }
        }

        private void ClearTable()
        {
            dataGridView.Rows.Clear();
            dateTimePickerEndDate.Value = dateTimePickerEndTime.Value = dateTimePickerStartTime.Value = dateTimePickerStartDate.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
            dateTimePickerEndTime.Checked = dateTimePickerEndDate.Checked = dateTimePickerStartTime.Checked = dateTimePickerStartDate.Checked = false;
            textBoxRouteName.Text = "";
            txtCmlDistance.Text = "";
            txtP2PDIstance.Text = "";
        }


        #endregion

        #region Routes

        // if the data in the grid is set, and different to the loadedroute, or the grid is not empty.  
        // not dirty if the grid is empty (name and systems empty)
        public bool IsDirty()       
        {
            var gridroute = SaveGridIntoRoute();
            return (gridroute != null && loadedroute != null) ? !gridroute.Equals(loadedroute) : gridroute != null;
        }

        // move systems in grid into this route class
        private SavedRouteClass SaveGridIntoRoute()
        {
            SavedRouteClass route = new SavedRouteClass();
            route.Name = textBoxRouteName.Text.Trim();
            route.Systems.Clear();
            route.Systems.AddRange(dataGridView.Rows.OfType<DataGridViewRow>()
                .Where(r => r.Index < dataGridView.NewRowIndex && r.Cells[0].Value != null)
                .Select(r => r.Cells[0].Value as string));

            if (dateTimePickerStartDate.Checked)
            {
                route.StartDateUTC = EDDConfig.Instance.ConvertTimeToUTCFromSelected(dateTimePickerStartDate.Value.Date);
                if (dateTimePickerStartTime.Checked)
                    route.StartDateUTC += dateTimePickerStartTime.Value.TimeOfDay;
            }
            else
            {
                route.StartDateUTC = null;
            }

            if (dateTimePickerEndDate.Checked)
            {
                route.EndDateUTC = EDDConfig.Instance.ConvertTimeToUTCFromSelected(dateTimePickerEndDate.Value.Date);
                route.EndDateUTC += dateTimePickerEndTime.Checked ? dateTimePickerEndTime.Value.TimeOfDay : new TimeSpan(23, 59, 59);
            }
            else
            {
                route.EndDateUTC = null;
            }

            return route.Systems.Count>0 || route.Name.HasChars() ? route : null;           // if systems or name, there is a route
        }

        // Move the current route into the DB
        private bool StoreCurrentRouteIntoDB(SavedRouteClass newrt)
        {
            if (newrt.Name.IsEmpty())
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Please specify a name for the route.".T(EDTx.UserControlExpedition_Specify), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            var savedroutes = SavedRouteClass.GetAllSavedRoutes();
            var edsmroute = savedroutes.Find(x => x.Name.Equals(newrt.Name, StringComparison.InvariantCultureIgnoreCase) && x.EDSM);

            if (edsmroute != null)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), ("The current route name conflicts with a well-known expedition." + Environment.NewLine
                    + "Please specify a new name to save your changes.").T(EDTx.UserControlExpedition_Conflict), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return false;
            }

            var overwriteroute = savedroutes.Where(r => r.Name.Equals(newrt.Name)).FirstOrDefault();

            if (overwriteroute != null)
            {
                if (MessageBoxTheme.Show(FindForm(), "Warning: route already exists. Would you like to overwrite it?".T(EDTx.UserControlExpedition_Overwrite), "Warning".T(EDTx.Warning), MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return false;

                overwriteroute.Delete();
            }

            if (overwriteroute == null)
                return newrt.Add();
            else
            {
                newrt.Id = overwriteroute.Id;
                return newrt.Add();
            }
        }

        // true if grid is empty, or it has saved
        private bool SaveGrid()
        {
            SavedRouteClass route = SaveGridIntoRoute();
            if (route != null)
            {
                if (StoreCurrentRouteIntoDB(route))
                {
                    loadedroute = route;
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }

        private bool PromptAndSaveIfNeeded()
        {
            if (IsDirty())
            {
                var result = ExtendedControls.MessageBoxTheme.Show(FindForm(), ("Expedition - There are unsaved changes to the current route." + Environment.NewLine
                    + "Would you like to save the current route before proceeding?").T(EDTx.UserControlExpedition_Unsaved), "Warning".T(EDTx.Warning), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                switch (result)
                {
                    case DialogResult.Yes:
                        return SaveGrid();

                    case DialogResult.No:
                        return true;

                    case DialogResult.Cancel:
                    default:
                        return false;
                }
            }
            else
                return true;
        }


        #endregion

        #region toolbar ui

        ExtendedControls.ExtListBoxForm dropdown;
        private void extButtonLoadRoute_Click(object sender, EventArgs e)
        {
            ExtendedControls.ExtButton but = sender as ExtendedControls.ExtButton;

            dropdown = new ExtendedControls.ExtListBoxForm("", true);

            var savedroutes = SavedRouteClass.GetAllSavedRoutes();
            foreach( var x in savedroutes )

            dropdown.FitImagesToItemHeight = true;
            dropdown.Items = savedroutes.Select(x => x.Name).ToList();
            dropdown.FlatStyle = FlatStyle.Popup;
            dropdown.PositionBelow(sender as Control);
            dropdown.SelectedIndexChanged += (s, ea) =>
            {
                if (PromptAndSaveIfNeeded())
                {
                    string name = savedroutes[dropdown.SelectedIndex].Name;
                    savedroutes = SavedRouteClass.GetAllSavedRoutes();      // reload, in case reselecting saved route
                    loadedroute = savedroutes.Find(x => x.Name == name);        // if your picking the same route again for some strange reason
                    DisplayRoute(loadedroute);
                }
            };

            EDDTheme.Instance.ApplyDialog(dropdown, true);
            dropdown.Show(this.FindForm());
        }

        private void extButtonNew_Click(object sender, EventArgs e)
        {
            if (PromptAndSaveIfNeeded())
            {
                ClearTable();
                loadedroute = null;
            }
        }

        private void extButtonSave_Click(object sender, EventArgs e)
        {
            SaveGrid();
        }

        private void extButtonDelete_Click(object sender, EventArgs e)
        {
            if ( loadedroute != null && loadedroute.EDSM == false && !IsDirty() )        // if loaded and unchanged, and not EDSM route
            {
                if (ExtendedControls.MessageBoxTheme.Show(FindForm(), "Are you sure you want to delete this route?".T(EDTx.UserControlExpedition_Delete), "Warning".T(EDTx.Warning), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    loadedroute.Delete();
                    loadedroute = null;
                    ClearTable();
                }
            }
        }

        private void extButtonImport_Click(object sender, EventArgs e)
        {
            var frm = new Forms.ExportForm();
            frm.Init(new string[] { "CSV", "CSV No Header Line", "Text File", "JSON"}, disablestartendtime: true, outputext: new string[] { "CSV|*.csv", "CSV|*.csv", "Text |*.txt|All|*.*", "JSON|*.json|All|*.*" },
                                        disableopeninclude:true, import:true);

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (frm.SelectedIndex == 3)     // JSON
                {
                    var list = ReadJSON(frm.Path);
                    if ( list != null )
                    {
                        if (list.Item1.HasChars() && textBoxRouteName.Text.IsEmpty())
                            textBoxRouteName.Text = list.Item1;
                        foreach (var jk in list.Item2)
                            dataGridView.Rows.Add(jk);
                    }
                }
                else
                {
                    CSVFile csv = new CSVFile();
                    if (csv.Read(frm.Path, FileShare.ReadWrite, frm.Comma))
                    {
                        for (int r = frm.SelectedIndex == 0 ? 1 : 0; r < csv.Rows.Count; r++)  // if in mode 0, from line 1, else from line 0
                        {
                            var row = csv[r];                               // column 0 only
                            if (row[0].HasChars())
                                dataGridView.Rows.Add(row[0]);
                        }

                        UpdateSystemRows();
                    }
                }
            }
        }

        private void extButtonImportRoute_Click(object sender, EventArgs e)
        {
            if (latestplottedroute == null || latestplottedroute.Count == 0)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Please create a route on a route panel".T(EDTx.UserControlExpedition_Createroute), "Warning".T(EDTx.Warning));
                return;
            }

            foreach (ISystem s in latestplottedroute)
            {
                dataGridView.Rows.Add(s.Name);
            }

            UpdateSystemRows();
        }

        private void extButtonImportNavRoute_Click(object sender, EventArgs e)
        {
            var route = discoveryform.history.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.NavRoute)?.journalEntry as EliteDangerousCore.JournalEvents.JournalNavRoute;
            if (route?.Route != null)
            {
                List<string> systems = new List<string>();
                foreach (var s in route.Route)
                {
                    if (s.StarSystem.HasChars())
                    {
                        dataGridView.Rows.Add(s.StarSystem);
                        systems.Add(s.StarSystem);
                    }
                }

                SystemCache.UpdateDBWithSystems(systems);           // try and fill DB with them

                UpdateSystemRows();
            }
        }

        private void buttonExtExport_Click(object sender, EventArgs e)
        {
            var rt = SaveGridIntoRoute();
            if (rt == null)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "There is no route to export ".T(EDTx.UserControlExpedition_NoRouteExport),
                    "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var frm = new Forms.ExportForm(); 
            frm.Init(new string[] { "Grid", "System Name Only", "JSON" }, disablestartendtime: true, outputext: new string[] { "CSV export|*.csv", "Text File|*.txt|CSV export|*.csv", "JSON|*.json"});

            if (frm.ShowDialog(FindForm()) == DialogResult.OK)
            {
                if (frm.SelectedIndex == 2)
                {
                    List<string> systems = new List<string>();
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (row.Cells[0] != null)
                        {
                            systems.Add(row.Cells[0].Value as string);
                        }
                    }

                    if ( !WriteJSON(frm.Path, textBoxRouteName.Text, systems))
                        CVSHelpers.WriteFailed(this.FindForm(), frm.Path);
                }
                else
                {
                    BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid();
                    grd.SetCSVDelimiter(frm.Comma);
                    grd.GetLineStatus += delegate (int r)
                    {
                        if (r < dataGridView.Rows.Count)
                        {
                            return BaseUtils.CSVWriteGrid.LineStatus.OK;
                        }
                        else
                            return BaseUtils.CSVWriteGrid.LineStatus.EOF;
                    };

                    grd.GetLine += delegate (int r)
                    {
                        DataGridViewRow rw = dataGridView.Rows[r];
                        if (frm.SelectedIndex == 1)
                            return new Object[] { rw.Cells[0].Value };
                        else
                            return new Object[] { rw.Cells[0].Value, rw.Cells[1].Value, rw.Cells[2].Value, rw.Cells[3].Value, rw.Cells[4].Value, rw.Cells[5].Value };
                    };

                    grd.GetHeader += delegate (int c)
                    {
                        return (c < 6 && frm.IncludeHeader && frm.SelectedIndex == 0) ? dataGridView.Columns[c].HeaderText : null;
                    };

                    grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
                }
            }
        }


        private void extButtonShow3DMap_Click(object sender, EventArgs e)
        {
            var route = dataGridView.Rows.OfType<DataGridViewRow>()
                 .Where(r => r.Index < dataGridView.NewRowIndex && r.Cells[0].Tag != null)
                 .Select(s => s.Cells[0].Tag as ISystem)
                 .Where(s => s.HasCoordinate).ToList();

            if (route.Count >= 2)
            {
                discoveryform.Open3DMap(route[0], route);
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "No route set up. Please add at least two systems.".T(EDTx.UserControlExpedition_NoRoute), "Warning".T(EDTx.Warning), MessageBoxButtons.OK);
                return;
            }
        }

        private void extButtonAddSystems_Click(object sender, EventArgs e)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            FindSystemsUserControl usc = new FindSystemsUserControl();
            usc.ReturnSystems = (List<Tuple<ISystem, double>> syslist) =>
            {
                List<String> systems = new List<String>();
                foreach (Tuple<ISystem, double> ret in syslist)
                {
                    systems.Add(ret.Item1.Name.Trim());
                }

                AppendRows(systems);

                f.ReturnResult(DialogResult.OK);
            };

            f.Add(new ExtendedControls.ConfigurableForm.Entry("UC", null, "", new Point(5, 30), usc.Size, null) { control = usc });
            f.AddCancel(new Point(4 + usc.Width - 80, usc.Height + 50));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "Cancel" || controlname == "Close")
                {
                    f.ReturnResult(DialogResult.Cancel);
                }
            };

            DBSettingsSaver db = new DBSettingsSaver(this, "Sys");

            f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "Add Systems".T(EDTx.UserControlExpedition_AddSys),
                                callback: () =>
                                {
                                    usc.Init(db, false, discoveryform);
                                },
                                closeicon: true);
            usc.Save();

        }

        private void buttonReverseRoute_Click(object sender, EventArgs e)
        {
            var route = SaveGridIntoRoute();
            if (route != null)
            {
                dataGridView.Rows.Clear();
                InsertRows(0, route.Systems.Reverse<string>().ToArray());
            }
        }

        #endregion

        #region Left click on row header mouse UI

        int dragRowIndex;
        Rectangle dragBox;
        private void dataGridViewRouteSystems_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                var hit = dataGridView.HitTest(e.X, e.Y);
                if (hit.Type == DataGridViewHitTestType.RowHeader && hit.RowIndex != -1)
                {
                    dragRowIndex = hit.RowIndex;
                    Size dragsize = SystemInformation.DragSize;
                    dragBox = new Rectangle(e.X - dragsize.Width / 2, e.Y - dragsize.Height / 2, dragsize.Width, dragsize.Height);
                }
                else
                {
                    dragBox = Rectangle.Empty;
                }
            }
        }

        private void dataGridViewRouteSystems_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                if (dragBox != Rectangle.Empty && !dragBox.Contains(e.Location))
                {
                    dataGridView.DoDragDrop(dataGridView.Rows[dragRowIndex], DragDropEffects.Move);
                }
            }
        }

        private void dataGridViewRouteSystems_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
            {
                var data = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                if (data.DataGridView == dataGridView)
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
            else if (e.Data.GetDataPresent(typeof(string)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void dataGridViewRouteSystems_DragDrop(object sender, DragEventArgs e)
        {
            Point p = dataGridView.PointToClient(new Point(e.X, e.Y));
            var insertIndex = dataGridView.HitTest(p.X, p.Y).RowIndex;
            if (insertIndex >= dataGridView.Rows.Count)
            {
                insertIndex = dataGridView.Rows.Count - 1;
            }

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
            {
                if (e.Effect == DragDropEffects.Move)
                {
                    var row = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                    if (row.DataGridView == dataGridView)
                    {
                        dataGridView.Rows.Remove(row);
                        dataGridView.Rows.Insert(insertIndex, row);
                        UpdateSystemRows();
                    }
                }
            }
            else if (e.Data.GetDataPresent(typeof(string)) && e.Effect == DragDropEffects.Copy)
            {
                var data = e.Data.GetData(typeof(string)) as string;
                var rows = data.Replace("\r", "").Split('\n').Where(r => r != "").ToArray();
                InsertRows(insertIndex, rows);
            }
        }

        #endregion

        #region Right Click Context

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
            DataObject obj = dataGridView.GetClipboardContent();

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
                int[] selectedRows = dataGridView.SelectedCells.OfType<DataGridViewCell>().Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
                int insertRow = selectedRows.FirstOrDefault();
                foreach (int index in selectedRows.Reverse())
                {
                    if (index != dataGridView.NewRowIndex)
                    {
                        dataGridView.Rows.RemoveAt(index);
                    }
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
                int[] selectedRows = dataGridView.SelectedCells.OfType<DataGridViewCell>().Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
                int insertRow = selectedRows.FirstOrDefault();
                InsertRows(insertRow, rows);
            }
        }

        private void deleteRowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridView.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridView.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();
            foreach (int index in selectedRows.Reverse())
            {
                dataGridView.Rows.RemoveAt(index);
            }
            UpdateSystemRows();
        }

        private void setTargetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridView.SelectedCells.OfType<DataGridViewCell>().Where(c => c.RowIndex != dataGridView.NewRowIndex).Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();

            if (selectedRows.Length == 0)
                return;
            var obj = dataGridView[0, selectedRows[0]].Value;

            if (obj == null)
                return;
            TargetHelpers.SetTargetSystem(this, discoveryform, (string)obj);
        }

        private void editBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] selectedRows = dataGridView.SelectedCells.OfType<DataGridViewCell>()
                .Where(c => c.RowIndex != dataGridView.NewRowIndex)
                .Select(c => c.RowIndex).OrderBy(v => v).Distinct().ToArray();

            if (selectedRows.Length == 0)
                return;

            var obj = dataGridView[0, selectedRows[0]].Value;

            if (obj == null)
                return;

            ISystem sc = discoveryform.history.FindSystem((string)obj,discoveryform.galacticMapping, true);     // use EDSM directly if required

            if (sc == null)
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "Unknown system, system is without co-ordinates".T(EDTx.UserControlExpedition_UnknownS), "Warning".T(EDTx.Warning), MessageBoxButtons.OK);
            }
            else
            {
                TargetHelpers.ShowBookmarkForm(this, discoveryform, sc, null, false);
                UpdateSystemRows();
            }
        }

        #endregion

        #region Validation

        private void dataGridViewRouteSystems_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex>=0 && e.RowIndex < dataGridView.RowCount)
            {
                string sysname = e.FormattedValue as string;
                var row = dataGridView.Rows[e.RowIndex];

                if (sysname.HasChars() && discoveryform.history.FindSystem(sysname, discoveryform.galacticMapping, true) == null)
                {
                    row.ErrorText = "System not known location".T(EDTx.UserControlExpedition_EDSMUnk);
                }
                else
                {
                    row.ErrorText = "";
                }
            }
        }

        private void dataGridViewRouteSystems_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                UpdateSystemRows();
            }
        }

        private void dataGridViewRouteSystems_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 1 || e.Column.Index >= 3)
                e.SortDataGridViewColumnNumeric();

        }
        #endregion

        #region JSON Output/Input of systems and name

        public static Tuple<string, List<string>> ReadJSON(string path)
        {
            string text = BaseUtils.FileHelpers.TryReadAllTextFromFile(path);
            if (text != null)
            {
                JObject jo = JObject.Parse(text, JToken.ParseOptions.AllowTrailingCommas | JToken.ParseOptions.CheckEOL);

                if (jo != null)
                {
                    var syslist = jo["Systems"].Array();

                    if (syslist != null)
                    {
                        List<string> sys = new List<string>();
                        foreach (var jk in syslist)
                            sys.Add(jk.Str());
                        return new Tuple<string, List<string>>(jo["Name"].Str(), sys);
                    }
                }
            }

            return null;
        }

        public static bool WriteJSON(string path, string name, List<string> systems)
        {
            JObject jn = new JObject();
            jn.Add("Name", name);
            JArray ja = new JArray();
            foreach (var s in systems)
                ja.Add(s);
            jn.Add("Systems", ja);
            try
            {
                File.WriteAllText(path, jn.ToString(true));
                return true;
            }
            catch
            {
                return false;
            }

        }

        #endregion

    }
}
