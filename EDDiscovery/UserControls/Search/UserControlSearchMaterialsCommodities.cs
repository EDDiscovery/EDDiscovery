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

namespace EDDiscovery.UserControls
{
    // Search UCs use the UCCB template BUT are not directly inserted into the normal panels.. they are inserted into the Search UCCB
    // Make sure DB saving has unique names.. they all share the same displayno.

    public partial class UserControlSearchMaterialsCommodities : UserControlCommonBase
    {
        private string DbColumnSave { get { return DBName("UCSearchMC", "DGVCol"); } }
        private string DbCM1 { get { return DBName("UCSearchMC", "CM1"); } }
        private string DbCM2 { get { return DBName("UCSearchMC", "CM2"); } }
        private string DbCMANDOR { get { return DBName("UCSearchMC", "CMANDOR"); } }

        private MaterialCommodityData[] itemlist;
        #region Init

        public UserControlSearchMaterialsCommodities()
        {
            InitializeComponent();
            var corner = dataGridView.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.RowTemplate.Height = 26;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            BaseUtils.Translator.Instance.Translate(this, new Control[] { });
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            dataGridView.Init(discoveryform);

            itemlist = MaterialCommodityData.GetCommoditiesRaw();
            Array.Sort(itemlist, (left, right) => left.Name.CompareTo(right.Name));

            var list = (from x in itemlist select x.Name + " (" + x.Category + ", " + x.Type + ((x.Rarity) ? ", Rare Commodity".Tx(this):"") + ")");

            comboBoxCustomCM1.Items.AddRange(list);
            comboBoxCustomCM1.SelectedIndex = Math.Min(SQLiteConnectionUser.GetSettingInt(DbCM1, 0), list.Count() - 1);
            comboBoxCustomCM2.Items.Add("----");
            comboBoxCustomCM2.Items.AddRange(list);
            comboBoxCustomCM2.SelectedIndex = Math.Min(SQLiteConnectionUser.GetSettingInt(DbCM2, 0), list.Count() - 1);

            comboBoxCustomCMANDOR.Items.AddRange(new string[] { "AND".Tx(this), "OR".Tx(this) });
            comboBoxCustomCMANDOR.SelectedIndex = SQLiteConnectionUser.GetSettingInt(DbCMANDOR, 0);

        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView, DbColumnSave);
            SQLiteConnectionUser.PutSettingInt(DbCM1, comboBoxCustomCM1.SelectedIndex);
            SQLiteConnectionUser.PutSettingInt(DbCM2, comboBoxCustomCM2.SelectedIndex);
            SQLiteConnectionUser.PutSettingInt(DbCMANDOR, comboBoxCustomCMANDOR.SelectedIndex);
        }

        #endregion

        class SysComparer : IEqualityComparer<Tuple<HistoryEntry, string>>
        {
            public bool Equals(Tuple<HistoryEntry, string> x, Tuple<HistoryEntry, string> y )
            {
                if (Object.ReferenceEquals(x, y)) return true;
                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null)) return false;
                return x.Item1.System.Name.Equals(y.Item1.System.Name) && x.Item2.Equals(y.Item2);
            }

            public int GetHashCode(Tuple<HistoryEntry, string> x)
            {
                if (Object.ReferenceEquals(x, null)) return 0;
                return x.Item1.System.Name.GetHashCode() ^ x.Item2.GetHashCode();
            }
        }

        IEnumerable<Tuple<HistoryEntry,string>> Search(MaterialCommodityData cm)
        {
            var scans = (from x in discoveryform.history
                         where x.EntryType == JournalTypeEnum.Scan && ((JournalScan)x.journalEntry).HasMaterial(cm.FDName)
                         select new Tuple<HistoryEntry, string>(x, ((JournalScan)x.journalEntry).BodyName));

            var markets = (from x in discoveryform.history
                           where x.EntryType == JournalTypeEnum.Market && ((JournalMarket)x.journalEntry).HasCommodityToBuy(cm.FDName)
                           select new Tuple<HistoryEntry, string>(x, ((JournalMarket)x.journalEntry).Station));

            return scans.Concat(markets);
        }

        private void buttonExtFind_Click(object sender, EventArgs e)
        {
            var total = Search(itemlist[comboBoxCustomCM1.SelectedIndex]);

            if ( comboBoxCustomCM2.SelectedIndex>0)         
            {
                var other = Search(itemlist[comboBoxCustomCM2.SelectedIndex-1]).ToArray();

                if ( comboBoxCustomCMANDOR.SelectedIndex == 0 )
                {
                    SysComparer cmp = new SysComparer();
                    total = total.Where((x) => Array.Find(other, (y) => cmp.Equals(x, y)) != null);
                }
                else
                    total = total.Concat(other);        // OR
            }

            total = total.Distinct(new SysComparer());

            ISystem cursystem = discoveryform.history.CurrentSystem;        // could be null
            var starlist = (from x in total select new Tuple<HistoryEntry, string, double>(x.Item1, x.Item2, x.Item1.System.Distance(cursystem))).ToList();
            StarsFound(starlist);
        }

        private void StarsFound(List<Tuple<HistoryEntry, string, double>> systems)       // systems may be null
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);

            dataGridView.Rows.Clear();

            if (systems != null)
            {
                ISystem cursystem = discoveryform.history.CurrentSystem;        // could be null

                foreach (var ret in systems)
                {
                    ISystem sys = ret.Item1.System;
                    string sep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";
                    object[] rowobj = {     sys.Name,
                                            ret.Item2,
                                            (cursystem != null ? cursystem.Distance(sys).ToString("0.#") : ""),
                                            sys.X.ToString("0.#") + sep + sys.Y.ToString("0.#") + sep + sys.Z.ToString("0.#")
                                           };

                    dataGridView.Rows.Add(rowobj);
                    dataGridView.Rows[dataGridView.Rows.Count - 1].Tag = ret.Item1;
                }

                dataGridView.Sort(ColumnCurrentDistance, ListSortDirection.Ascending);
            }

        }

        private void dataGridViewEDSM_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 2)
                e.SortDataGridViewColumnNumeric();
        }

    }
}
