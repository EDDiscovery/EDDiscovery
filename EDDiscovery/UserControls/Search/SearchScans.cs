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
using EliteDangerousCore.JournalEvents;

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
            };

            public int StandardSearches;

            static private Queries instance = null;
            private string DbUserQueries { get { return "UCSearchScansUserQuery"; } }  // not keyed to profile or to panel, global

            private char splitmarker = (char)0x2b1c; // horrible but i can't be bothered to do a better implementation at this point

            private Queries()
            {
                StandardSearches = Searches.Count();
                string[] userqueries = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbUserQueries, "").Split(new char[] { splitmarker });

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

                EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbUserQueries, userqueries);
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

        private string DbColumnSave { get { return DBName("UCSearchScans", "DGVCol"); } }
        private string DbQuerySave { get { return DBName("UCSearchScans", "Query"); } }
        private string DbSplitterSave { get { return DBName("UCSearchScans", "Splitter"); } }

        #region Init

        public SearchScans()
        {
            InitializeComponent();
            var corner = dataGridView.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridView.CheckEDSM = false; // for this, only our data is shown
            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.Height = Font.ScalePixels(26);
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            BaseUtils.Translator.Instance.Translate(this, new Control[] { });
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            List<BaseUtils.TypeHelpers.PropertyNameInfo> classnames = BaseUtils.TypeHelpers.GetPropertyFieldNames(typeof(JournalScan),bf:System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly);
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("EventTimeUTC", "Date Time in UTC", BaseUtils.ConditionEntry.MatchType.DateAfter));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("EventTimeLocal", "Date Time in Local time", BaseUtils.ConditionEntry.MatchType.DateAfter));     // add on a few from the base class..
            classnames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("SyncedEDSM", "Synced to EDSM, 1 = yes, 0 = not", BaseUtils.ConditionEntry.MatchType.IsTrue));     // add on a few from the base class..

            string query = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbQuerySave, "");

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
            DGVLoadColumnLayout(dataGridView, DbColumnSave);
            splitContainer.SplitterDistance(EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSplitterSave, 0.2));
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView, DbColumnSave);
            conditionFilterUC.Check();      // checks, ignore string return errors, fills in Result
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbQuerySave, conditionFilterUC.Result.ToString());
            Queries.Instance.Save();
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSplitterSave, splitContainer.GetSplitterDistance());
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

        private void buttonFind_Click(object sender, EventArgs e)
        {
            BaseUtils.ConditionLists cond = Valid();
            if (cond != null)
            {
                this.Cursor = Cursors.WaitCursor;
                dataGridView.Rows.Clear();

                DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
                SortOrder sortorder = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Descending;

                ISystem cursystem = discoveryform.history.CurrentSystem;        // could be null

                foreach ( var he in discoveryform.history.FilterByScan)
                {
                    JournalScan js = he.journalEntry as JournalScan;

                    BaseUtils.Variables scandata = new BaseUtils.Variables();
                    scandata.AddPropertiesFieldsOfClass(js, "", new Type[] { typeof(System.Drawing.Icon), typeof(System.Drawing.Image), typeof(System.Drawing.Bitmap), typeof(Newtonsoft.Json.Linq.JObject) }, 5);

                    bool? res = cond.CheckAll(scandata, out string errlist, out BaseUtils.ConditionLists.ErrorClass errclass );  // need function handler..

                    if ( res.HasValue && res.Value == true )
                    {
                        ISystem sys = he.System;
                        string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";
                        object[] rowobj = {
                                            EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC).ToString(),
                                            js.BodyName,
                                            js.DisplayString(0),
                                            (cursystem != null ? cursystem.Distance(sys).ToString("0.#") : ""),
                                            sys.X.ToString("0.#") + sep + sys.Y.ToString("0.#") + sep + sys.Z.ToString("0.#")
                                           };

                        dataGridView.Rows.Add(rowobj);
                        dataGridView.Rows[dataGridView.Rows.Count - 1].Tag = sys;
                    }

                    if ( errclass == BaseUtils.ConditionLists.ErrorClass.LeftSideVarUndefined || errclass == BaseUtils.ConditionLists.ErrorClass.RightSideBadFormat )
                    {
                        ExtendedControls.MessageBoxTheme.Show(errlist, "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                }

                dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
                dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
                this.Cursor = Cursors.Default;
            }

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
