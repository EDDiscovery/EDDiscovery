﻿/*
 * Copyright © 2016 - 2021 EDDiscovery development team
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
using EDDiscovery.Controls;
using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    // Search UCs use the UCCB template BUT are not directly inserted into the normal panels.. they are inserted into the Search UCCB
    // Make sure DB saving has unique names.. they all share the same displayno.

    public partial class SearchScans : UserControlCommonBase
    {
        class Queries
        {
            public static Queries Instance      // one instance across all profiles/panels so list is unified.
            {
                get
                {
                    if (instance == null)
                        instance = new Queries();
                    return instance;                }
            }

            public List<Tuple<string, string>> Searches = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Body Name","BodyName contains <name>"),
                new Tuple<string, string>("Scan Type","ScanType contains Detailed"),
                new Tuple<string, string>("Distance (ls)","DistanceFromArrivalLS >= 20"),
                new Tuple<string, string>("Rotation Period (s)","nRotationPeriod >= 30"),
                new Tuple<string, string>("Rotation Period (days)","nRotationPeriodDays >= 1"),
                new Tuple<string, string>("Surface Temperature (K)","nSurfaceTemperature >= 273"),
                new Tuple<string, string>("Radius (m)","nRadius >= 100000"),
                new Tuple<string, string>("Radius (sols)","nRadiusSols >= 1"),
                new Tuple<string, string>("Radius (Earth)","nRadiusEarths >= 1"),
                new Tuple<string, string>("Has Rings","HasRings == 1"),
                new Tuple<string, string>("Semi Major Axis (m)","nSemiMajorAxis >= 20000000"),
                new Tuple<string, string>("Semi Major Axis (AU)","nSemiMajorAxisAU >= 1"),
                new Tuple<string, string>("Eccentricity ","nEccentricity >= 0.1"),
                new Tuple<string, string>("Orbital Inclination (Deg)","nOrbitalInclination > 1"),
                new Tuple<string, string>("Periapsis (Deg)","nPeriapsis > 1"),
                new Tuple<string, string>("Orbital period (s)","nOrbitalPeriod > 200"),
                new Tuple<string, string>("Orbital period (days)","nOrbitalPeriodDays > 200"),
                new Tuple<string, string>("Axial Tilt (Deg)","nAxialTiltDeg > 1"),

                new Tuple<string, string>("Star Type","StarType $== A"),
                new Tuple<string, string>("Star Mass (Sols)","nStellarMass >= 1"),
                new Tuple<string, string>("Star Magnitude","nAbsoluteMagnitude >= 1"),
                new Tuple<string, string>("Star Age (MY)","nAge >= 2000"),
                new Tuple<string, string>("Star Luminosity","Luminosity $== V"),

                new Tuple<string, string>("Planet Mass (Earths)","nMassEM >= 1"),
                new Tuple<string, string>("Planet Materials","MaterialList contains iron"),
                new Tuple<string, string>("Planet Class","PlanetClass $== \"High metal content body\""),
                new Tuple<string, string>("Tidal Lock","nTidalLock == 1"),
                new Tuple<string, string>("Terraformable","TerraformState $== Terraformable"),
                new Tuple<string, string>("Atmosphere","Atmosphere $== \"thin sulfur dioxide atmosphere\""),
                new Tuple<string, string>("Atmosphere ID","AtmosphereID $== Carbon_dioxide"),
                new Tuple<string, string>("Atmosphere Property","AtmosphereProperty $== Rich"),
                new Tuple<string, string>("Volcanism","Volcanism $== \"minor metallic magma volcanism\""),
                new Tuple<string, string>("Volcanism ID","VolcanismID $== Ammonia_Magma"),
                new Tuple<string, string>("Surface Gravity m/s","nSurfaceGravity >= 9.6"),
                new Tuple<string, string>("Surface Gravity G","nSurfaceGravityG >= 1.0"),
                new Tuple<string, string>("Surface Gravity Landable G","nSurfaceGravityG >= 1.0 And IsLandable == 1"),
                new Tuple<string, string>("Surface Pressure (Pa)","nSurfacePressure >= 101325"),
                new Tuple<string, string>("Surface Pressure (Earth Atmos)","nSurfacePressureEarth >= 1"),
                new Tuple<string, string>("Landable","IsLandable == 1"),
                new Tuple<string, string>("Planet inside outer ring","nSemiMajorAxis <= Parent.RingsOuterm And Parent.IsPlanet IsTrue"),
                new Tuple<string, string>("Planet inside inner ring","nSemiMajorAxis <= Parent.RingsInnerm And Parent.IsPlanet IsTrue"),
                new Tuple<string, string>("Planet inside the rings","nSemiMajorAxis >= Parent.RingsInnerm And nSemiMajorAxis <= Parent.RingsOuterm And Parent.IsPlanet IsTrue And IsPlanet IsTrue"),
            };

            public int StandardSearches;

            static private Queries instance = null;
            private string DbUserQueries { get { return "UCSearchScansUserQuery"; } }  // not keyed to profile or to panel, global

            private char splitmarker = (char)0x2b1c; // horrible but i can't be bothered to do a better implementation at this point

            private Queries()
            {
                StandardSearches = Searches.Count();
                string[] userqueries = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbUserQueries, "").Split(new char[] { splitmarker }); // allowed use

                for (int i = 0; i+1 < userqueries.Length; i += 2)
                    Searches.Add(new Tuple<string, string>(userqueries[i], userqueries[i + 1]));
            }

            public void Save()
            {
                string userqueries = "";
                for (int i = StandardSearches; i < Searches.Count(); i++)
                {
                    userqueries += Searches[i].Item1 + splitmarker + Searches[i].Item2 + splitmarker;
                }

                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbUserQueries, userqueries); // allowed use
            }

            public void Update(string name, string expr)
            {
                var entry = Searches.FindIndex(x => x.Item1.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                if (entry != -1)
                    Searches[entry] = new Tuple<string, string>(name, expr);
                else
                    Searches.Add(new Tuple<string, string>(name, expr));
            }

            public void Delete(string name)
            {
                var entry = Searches.FindIndex(x => x.Item1.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                if (entry != -1)
                    Searches.RemoveAt(entry);
            }

        };

        private string dbQuerySave = "Query";
        private string dbSplitterSave = "Splitter";


        #region Init

        public SearchScans()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "UCSearchScans";

            dataGridView.CheckEDSM = false; // for this, only our data is shown
            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.Height = Font.ScalePixels(26);
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            var enumlist = new Enum[] { EDTx.SearchScans_ColumnDate, EDTx.SearchScans_ColumnStar, EDTx.SearchScans_ColumnInformation, EDTx.SearchScans_ColumnCurrentDistance, EDTx.SearchScans_ColumnPosition, EDTx.SearchScans_buttonFind, EDTx.SearchScans_buttonSave, EDTx.SearchScans_buttonDelete };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);

            List<BaseUtils.TypeHelpers.PropertyNameInfo> classnames = BaseUtils.TypeHelpers.GetPropertyFieldNames(typeof(JournalScan), bf: System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly, excludearrayslist:true);
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("EventTimeUTC", "Date Time in UTC", BaseUtils.ConditionEntry.MatchType.DateAfter));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("EventTimeLocal", "Date Time in Local time", BaseUtils.ConditionEntry.MatchType.DateAfter));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("SyncedEDSM", "Synced to EDSM, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..

            // from FSSBodySignals or SAASignalsFound
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsGeoSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsBioSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsThargoidSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsGuardianSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsHumanSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsOtherSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("ContainsUncategorisedSignals", "Bodies with these signals, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..

            string query = GetSetting(dbQuerySave, "");

            conditionFilterUC.VariableNames = classnames;
            conditionFilterUC.InitConditionList(new BaseUtils.ConditionLists(query));   // will ignore if query is bad and return empty query

            dataGridView.Init(discoveryform);

            comboBoxSearches.Items.AddRange(Queries.Instance.Searches.Select(x => x.Item1));
            comboBoxSearches.Text = "Select".T(EDTx.SearchScans_Select);
            comboBoxSearches.SelectedIndexChanged += ComboBoxSearches_SelectedIndexChanged;

        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg = thc;
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView);
            splitContainer.SplitterDistance(GetSetting(dbSplitterSave, 0.2));
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView);
            conditionFilterUC.Check();      // checks, ignore string return errors, fills in Result
            PutSetting(dbQuerySave, conditionFilterUC.Result.ToString());
            Queries.Instance.Save();
            PutSetting(dbSplitterSave, splitContainer.GetSplitterDistance());
        }

        #endregion

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index == 3)
                e.SortDataGridViewColumnNumeric();
        }

        private void ComboBoxSearches_SelectedIndexChanged(object sender, EventArgs e)
        {
            conditionFilterUC.Clear();
            conditionFilterUC.LoadConditions(new BaseUtils.ConditionLists(Queries.Instance.Searches[comboBoxSearches.SelectedIndex].Item2));
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            BaseUtils.ConditionLists cond = Valid();
            if (cond != null)
            {
                string name = ExtendedControls.PromptSingleLine.ShowDialog(this.FindForm(), "Name:".T(EDTx.SearchScans_Name), "", "Enter Search Name:".T(EDTx.SearchScans_SN), this.FindForm().Icon);
                if (name != null)
                {
                    Queries.Instance.Update(name,cond.ToString());
                    comboBoxSearches.Items.Clear();
                    comboBoxSearches.Items.AddRange(Queries.Instance.Searches.Select(x => x.Item1));
                    comboBoxSearches.SelectedIndexChanged -= ComboBoxSearches_SelectedIndexChanged;
                    comboBoxSearches.SelectedItem = name;
                    comboBoxSearches.SelectedIndexChanged += ComboBoxSearches_SelectedIndexChanged;
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string name = comboBoxSearches.Text;
            if (comboBoxSearches.SelectedIndex >= Queries.Instance.StandardSearches && name.HasChars())
            {
                if (ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Confirm deletion of".T(EDTx.SearchScans_DEL) + " " + name, "Delete".T(EDTx.Delete), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Queries.Instance.Delete(name);
                    comboBoxSearches.Items.Clear();
                    comboBoxSearches.Items.AddRange(Queries.Instance.Searches.Select(x => x.Item1));
                }
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Cannot delete this entry".T(EDTx.SearchScans_DELNO), "Delete".T(EDTx.Delete), MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }


        private async void buttonFind_Click(object sender, EventArgs e)
        {
            BaseUtils.ConditionLists cond = Valid();
            if (cond != null)
            {
                this.Cursor = Cursors.WaitCursor;
                dataGridView.Rows.Clear();

                DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
                SortOrder sortorder = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Descending;

                ISystem cursystem = discoveryform.history.CurrentSystem();        // could be null

                var varusedincondition = cond.VariablesUsed(true, true);      // what variables are in use, so we don't enumerate the lots.

                bool parentvars = varusedincondition.StartsWithInList("Parent.", StringComparison.InvariantCultureIgnoreCase) >= 0;      // is there any parents in the condition?

                if ( parentvars )       // if we are referencing one
                    discoveryform.history.FillInScanNode();     // ensure all journal scan entries point to a scan node (expensive, done only when required)

                var helist = discoveryform.history.FilterByScanFSSBodySAASignals();

                var sw = new System.Diagnostics.Stopwatch(); sw.Start();

                var defaultvars = new BaseUtils.Variables();
                defaultvars.AddPropertiesFieldsOfClass(new BodyPhysicalConstants(), "", null, 10);
                System.Diagnostics.Debug.WriteLine(defaultvars.ToString(separ:Environment.NewLine));

                var results = await Find(helist, cond, varusedincondition, defaultvars, cursystem);

                foreach( var r in results)
                {
                    dataGridView.Rows.Add(r.Item2);
                    dataGridView.Rows[dataGridView.Rows.Count - 1].Tag = r.Item1;
                }

                System.Diagnostics.Debug.Write($"Search took {sw.ElapsedMilliseconds}");
                dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
                this.Cursor = Cursors.Default;
            }

        }

        // Async task to find results given cond in helist, using only vars specified.

        private System.Threading.Tasks.Task<List<Tuple<ISystem,object[]>>> Find(List<HistoryEntry> helist, BaseUtils.ConditionLists cond, HashSet<string> varsusedincondition, 
                                    BaseUtils.Variables defaultvars, ISystem cursystem)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                bool parentvars = varsusedincondition.StartsWithInList("Parent.", StringComparison.InvariantCultureIgnoreCase) >= 0;      // is there any parents in the condition?
                HashSet<string> pvars = varsusedincondition.Where(x => x.StartsWith("Parent.")).Select(x=>x.Substring(7)).ToHashSet();    // parent vars, stripped

                List<Tuple<ISystem,object[]>> rows = new List<Tuple<ISystem,object[]>>();
                foreach (var he in helist)
                {
                    BaseUtils.Variables scandatavars = new BaseUtils.Variables(defaultvars);
                    scandatavars.AddPropertiesFieldsOfClass(he.journalEntry, "",
                            new Type[] { typeof(System.Drawing.Icon), typeof(System.Drawing.Image), typeof(System.Drawing.Bitmap), typeof(QuickJSON.JObject) }, 5,
                            varsusedincondition);

                    // for scans, with parent. vars, we need to find the scan data of the parent, if we have it, we can fill them in
                    if ( parentvars && he.journalEntry.EventTypeID == JournalTypeEnum.Scan && he.ScanNode != null) 
                    {
                        var parentjs = he.ScanNode.Parent?.ScanData;
                        if ( parentjs!=null)
                        {
                            scandatavars.AddPropertiesFieldsOfClass(parentjs, "Parent.",
                                    new Type[] { typeof(System.Drawing.Icon), typeof(System.Drawing.Image), typeof(System.Drawing.Bitmap), typeof(QuickJSON.JObject) }, 5,
                                    pvars);
                        }
                    }

                    bool? res = cond.CheckAgainstVariables(scandatavars, out string errlist, out BaseUtils.ConditionLists.ErrorClass errclassunused);  // need function handler..

                    if (res.HasValue && res.Value == true)
                    {
                        //System.Diagnostics.Debug.WriteLine($"{he.System.Name} {scandatavars.ToString(separ: Environment.NewLine)}");

                        ISystem sys = he.System;
                        string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";

                        JournalScan js = he.journalEntry as JournalScan;
                        JournalFSSBodySignals jb = he.journalEntry as JournalFSSBodySignals;
                        JournalSAASignalsFound jbs = he.journalEntry as JournalSAASignalsFound;

                        string name, info;
                        if ( js != null )
                        {
                            name = js.BodyName;
                            info = js.DisplayString(0);
                        }
                        else if ( jb != null )
                        {
                            name = jb.BodyName;
                            jb.FillInformation(he.System, "", out info, out string d);
                        }
                        else
                        {
                            name = jbs.BodyName;
                            jbs.FillInformation(he.System, "", out info, out string d);
                        }

                        object[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC).ToString(),
                                            name,
                                            info,
                                            (cursystem != null ? cursystem.Distance(sys).ToString("0.#") : ""),
                                            sys.X.ToString("0.#") + sep + sys.Y.ToString("0.#") + sep + sys.Z.ToString("0.#")
                                            };
                        rows.Add(new Tuple<ISystem, object[]>(sys, rowobj));
                    }
                }

                return rows;
            });
        }

        private BaseUtils.ConditionLists Valid()
        {
            string errs = conditionFilterUC.Check();
            if (errs.HasChars())
            {
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Condition is not valid".T(EDTx.SearchScans_CNV), "Condition".T(EDTx.SearchScans_CD), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            else
                return conditionFilterUC.Result;
        }

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            dataGridView.Excel(4);
        }
    }
}
