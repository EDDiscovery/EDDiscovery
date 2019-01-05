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
using EliteDangerousCore.JournalEvents;

// save splitter
// save query
// load query list from db
// query list is generic across all searches..

namespace EDDiscovery.UserControls
{
    // Search UCs use the UCCB template BUT are not directly inserted into the normal panels.. they are inserted into the Search UCCB
    // Make sure DB saving has unique names.. they all share the same displayno.

    public partial class UserControlSearchScans : UserControlCommonBase
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
                new Tuple<string, string>("Star Mass (Sols)","nStellarMass >= 1"),
                new Tuple<string, string>("Planet Mass (Earths)","nMassEM >= 1"),
                new Tuple<string, string>("Planet Materials","MaterialList contains iron"),
            };
            public int StandardSearches;

            static private Queries instance = null;
            private string DbUserQueries { get { return "UCSearchScansUserQuery"; } }  // not keyed to profile or to panel, global

            private char splitmarker = (char)0x2b1c; // horrible but i can't be bothered to do a better implementation at this point

            private Queries()
            {
                StandardSearches = Searches.Count();
                string[] userqueries = SQLiteConnectionUser.GetSettingString(DbUserQueries, "").Split(new char[] { splitmarker });

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

                SQLiteConnectionUser.PutSettingString(DbUserQueries, userqueries);
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

        public UserControlSearchScans()
        {
            InitializeComponent();
            var corner = dataGridView.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridView.CheckEDSM = false; // for this, only our data is shown
            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.Height = 26;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            BaseUtils.Translator.Instance.Translate(this, new Control[] { });
            BaseUtils.Translator.Instance.Translate(toolTip, this);
            //BaseUtils.Translator.Instance.Translate(dataGridViewEDSM.ContextMenu, this);

            List<string> classnames = BaseUtils.TypeHelpers.GetPropertyFieldNames(typeof(JournalScan),bf:System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.DeclaredOnly);
            classnames.Add("EventTimeUTC");     // add on a few from the base class..
            classnames.Add("EventTimeLocal");
            classnames.Add("SyncedEDSM");

            string query = SQLiteConnectionUser.GetSettingString(DbQuerySave, "");

            conditionFilterUC.InitConditionList(classnames, new BaseUtils.ConditionLists(query));   // will ignore if query is bad and return empty query

            dataGridView.Init(discoveryform);

            comboBoxSearches.Items.AddRange(Queries.Instance.Searches.Select(x => x.Item1));
            comboBoxSearches.Text = "Select".Tx(this);
            comboBoxSearches.SelectedIndexChanged += ComboBoxSearches_SelectedIndexChanged;
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView, DbColumnSave);
            splitContainer.SplitterDistance(SQLiteConnectionUser.GetSettingDouble(DbSplitterSave, 0.2));
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView, DbColumnSave);
            conditionFilterUC.Check();      // checks, ignore string return errors, fills in Result
            SQLiteConnectionUser.PutSettingString(DbQuerySave, conditionFilterUC.Result.ToString());
            Queries.Instance.Save();
            SQLiteConnectionUser.PutSettingDouble(DbSplitterSave, splitContainer.GetSplitterDistance());
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
                string name = ExtendedControls.PromptSingleLine.ShowDialog(this.FindForm(), "Name:".Tx(this), "", "Enter Search Name:".Tx(this, "SN"), this.FindForm().Icon);
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
                if (ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Confirm deletion of".Tx(this, "DEL") + " " + name, "Delete".Tx(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Queries.Instance.Delete(name);
                    comboBoxSearches.Items.Clear();
                    comboBoxSearches.Items.AddRange(Queries.Instance.Searches.Select(x => x.Item1));
                }
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Cannot delete this entry".Tx(this, "DELNO"), "Delete".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Warning);

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

                    bool? res = cond.CheckAll(scandata, out string errlist);  // need function handler..

                    if ( res.HasValue && res.Value == true )
                    {
                        ISystem sys = he.System;
                        string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";
                        object[] rowobj = {
                                            (EDDConfig.Instance.DisplayUTC ? he.EventTimeUTC : he.EventTimeLocal).ToString(),
                                            js.BodyName,
                                            js.DisplayString(0,true),
                                            (cursystem != null ? cursystem.Distance(sys).ToString("0.#") : ""),
                                            sys.X.ToString("0.#") + sep + sys.Y.ToString("0.#") + sep + sys.Z.ToString("0.#")
                                           };

                        dataGridView.Rows.Add(rowobj);
                        dataGridView.Rows[dataGridView.Rows.Count - 1].Tag = sys;
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
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Condition is not valid".Tx(this, "CNV"), "Condition".Tx(this, "CD"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
