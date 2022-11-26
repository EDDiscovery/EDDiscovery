/*
 * Copyright © 2017 EDDiscovery development team
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
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EliteDangerousCore.EDSM;
using EliteDangerousCore;
using EliteDangerousCore.DB;

namespace EDDiscovery.UserControls
{
    public partial class FindSystemsUserControl : UserControl
    {
        public Action<List<Tuple<ISystem, double>>> ReturnSystems;      // return may be null

        public Action Excel;                                            // excel pressed
        UserControlCommonBase.DBSettingsSaver dbsaver;
        EDDiscoveryForm discoveryform;

        private string dbStar = "Star"; 
        private string dbRadiusMax = "RadiusMax"; 
        private string dbRadiusMin = "RadiusMin";
        private string dbX = "X";
        private string dbY = "Y";
        private string dbZ = "Z";
        private string dbCube = "Cube"; 
        private string dbEVS = "ExcludeVisitedSystems"; 

        public FindSystemsUserControl()
        {
            InitializeComponent();
        }

        public void Init(UserControlCommonBase.DBSettingsSaver db, bool showexcel , EDDiscoveryForm disc )
        {
            dbsaver = db;
            discoveryform = disc;
            numberBoxMinRadius.Value = dbsaver.GetSetting(dbRadiusMin, 0.0);
            numberBoxMaxRadius.Value = dbsaver.GetSetting(dbRadiusMax, 20.0);
            textBoxSystemName.Text = dbsaver.GetSetting(dbStar, "");
            numberBoxDoubleX.Value = dbsaver.GetSetting(dbX, 0.0);
            numberBoxDoubleY.Value = dbsaver.GetSetting(dbY, 0.0);
            numberBoxDoubleZ.Value = dbsaver.GetSetting(dbZ, 0.0);
            checkBoxCustomCube.Checked = dbsaver.GetSetting(dbCube, false);
            extCheckBoxExcludeVisitedSystems.Checked = dbsaver.GetSetting(dbEVS, false);

            if (textBoxSystemName.Text.Length > 0)
                SetXYZ();

            ValidateEnable();

            textBoxSystemName.SetAutoCompletor(SystemCache.ReturnSystemAutoCompleteList,true);

            this.numberBoxMinRadius.TextChanged += new System.EventHandler(this.textBoxSystemName_RadiusChanged);
            this.numberBoxMaxRadius.TextChanged += new System.EventHandler(this.textBoxSystemName_RadiusChanged);
            this.textBoxSystemName.TextChanged += new System.EventHandler(this.textBoxSystemName_TextChanged);

            this.numberBoxDoubleX.ValidityChanged += ValidityNumberBox;
            this.numberBoxDoubleY.ValidityChanged += ValidityNumberBox;
            this.numberBoxDoubleZ.ValidityChanged += ValidityNumberBox;

            numberBoxMinRadius.SetComparitor(numberBoxMaxRadius, -2);     // need to do this after values are set
            numberBoxMaxRadius.SetComparitor(numberBoxMinRadius, 2);

            buttonExtExcel.Visible = showexcel;

            ValidateEnable();

            var enumlist = new Enum[] { EDTx.FindSystemsUserControl_extCheckBoxExcludeVisitedSystems, EDTx.FindSystemsUserControl_checkBoxCustomCube, EDTx.FindSystemsUserControl_buttonExtNames, EDTx.FindSystemsUserControl_buttonExtVisited, EDTx.FindSystemsUserControl_buttonExtDB, EDTx.FindSystemsUserControl_buttonExtEDSM, EDTx.FindSystemsUserControl_labelRadMax, EDTx.FindSystemsUserControl_labelRadMin, EDTx.FindSystemsUserControl_labelFilter };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, new Control[] { labelX, labelY, labelZ });
        }

        public void Save()
        {
            dbsaver.PutSetting(dbRadiusMin, numberBoxMinRadius.Value);
            dbsaver.PutSetting(dbRadiusMax, numberBoxMaxRadius.Value);
            dbsaver.PutSetting(dbX, numberBoxDoubleX.Value);
            dbsaver.PutSetting(dbY, numberBoxDoubleY.Value);
            dbsaver.PutSetting(dbZ, numberBoxDoubleZ.Value);
            dbsaver.PutSetting(dbStar, textBoxSystemName.Text);
            dbsaver.PutSetting(dbCube, checkBoxCustomCube.Checked);
            dbsaver.PutSetting(dbEVS, extCheckBoxExcludeVisitedSystems.Checked);
        }

        private void buttonExtNamesClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            Task taskEDSM = Task<HashSet<ISystem>>.Factory.StartNew(() =>
            {
                return SystemCache.FindSystemWildcard(textBoxSystemName.Text);

            }).ContinueWith(task => this.Invoke(new Action(() =>
            {
                Cursor = Cursors.Default;
                ReturnSystems((from x in task.Result select new Tuple<ISystem, double>(x, -1)).ToList());
            }
            )));
        }

        private void buttonExtEDSMClick(object sender, EventArgs e)
        {
            if (numberBoxMaxRadius.Value > 100)
            {
                if (ExtendedControls.MessageBoxTheme.Show(FindForm(), "This is a large radius, it make take a long time or not work, are you sure?", "Warning - Large radius", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
                    return;
            }

            Cursor = Cursors.WaitCursor;

            bool spherical = !checkBoxCustomCube.Checked;

            Task taskEDSM = Task<List<Tuple<ISystem, double>>>.Factory.StartNew(() =>
            {
                EDSMClass edsm = new EDSMClass();
                // cube: use *1.412 (sqrt(2)) to reach out to far corner of cube
                // cube: must get centre system, to know what co-ords it is..

                List<Tuple<ISystem, double>> rlist = null;

                if (!string.IsNullOrWhiteSpace(textBoxSystemName.Text))
                {
                    rlist = edsm.GetSphereSystems(textBoxSystemName.Text, numberBoxMaxRadius.Value * (spherical ? 1.00 : 1.412), spherical ? numberBoxMinRadius.Value : 0);
                }
                else if (numberBoxDoubleX.IsValid && numberBoxDoubleY.IsValid && numberBoxDoubleZ.IsValid)
                {
                    rlist = edsm.GetSphereSystems(numberBoxDoubleX.Value, numberBoxDoubleY.Value, numberBoxDoubleZ.Value, numberBoxMaxRadius.Value * (spherical ? 1.00 : 1.412), spherical ? numberBoxMinRadius.Value : 0);
                }
                else
                {
                    rlist = new List<Tuple<ISystem, double>>();
                }

                if (rlist != null && rlist.Count > 0 && !SystemsDatabase.Instance.RebuildRunning)   // if db free for use, ensure they are all in the db
                    SystemsDatabase.Instance.StoreSystems(rlist.Select(x => x.Item1).ToList());     // won't do anything if rebuilding

                return rlist;

            }).ContinueWith(task => this.Invoke(new Action(() =>
            {
                List<Tuple<ISystem, double>> listsphere = task.Result;

                if ( listsphere != null )
                {
                    bool excvisited = extCheckBoxExcludeVisitedSystems.Checked;

                    if (!spherical)       // if cubed, need to filter them out
                    {
                        ISystem centre = listsphere.Find(x => x.Item2 <= 1)?.Item1;     // find centre, i.e less 1 ly distance
                        if (centre != null)
                        {
                            //System.Diagnostics.Debug.WriteLine("From " + listsphere.Count());
                            //foreach (var x in listsphere) System.Diagnostics.Debug.WriteLine("<" + x.Item1.ToString());

                            double mindistsq = numberBoxMinRadius.Value * numberBoxMinRadius.Value;     // mindist is the square line distance, per stardistance use

                            listsphere = (from s in listsphere
                                          where
                                            (s.Item1.X - centre.X) * (s.Item1.X - centre.X) + (s.Item1.Y - centre.Y) * (s.Item1.Y - centre.Y) + (s.Item1.Z - centre.Z) * (s.Item1.Z - centre.Z) >= mindistsq &&
                                            Math.Abs(s.Item1.X - centre.X) <= numberBoxMaxRadius.Value &&
                                            Math.Abs(s.Item1.Y - centre.Y) <= numberBoxMaxRadius.Value &&
                                            Math.Abs(s.Item1.Z - centre.Z) <= numberBoxMaxRadius.Value &&
                                            (!excvisited || discoveryform.history.FindLastFSDCarrierJumpBySystemName(s.Item1.Name) == null)
                                          select s).ToList();

                            //System.Diagnostics.Debug.WriteLine("To " + listsphere.Count());
                            //foreach (var x in listsphere) System.Diagnostics.Debug.WriteLine(">" + x.Item1.ToString());
                        }
                    }
                    else if ( excvisited )  // if exc visited, need to filter them out
                    {
                        listsphere = (from s in listsphere
                                      where discoveryform.history.FindLastFSDCarrierJumpBySystemName(s.Item1.Name) == null
                                      select s).ToList();
                    }
                }

                if (listsphere == null)
                {
                    string resp = String.Format("EDSM did not return any data on {0}\nIt may be a galactic object that it does not know about".T(EDTx.FindSystemsUserControl_EDSM), textBoxSystemName.Text);
                    ExtendedControls.MessageBoxTheme.Show(this.FindForm(), resp, "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                Cursor = Cursors.Default;

                if (listsphere != null)
                {
                    ReturnSystems(listsphere);
                }
            }
            )));
        }

        private void buttonExtVisitedClick(object sender, EventArgs e)
        {
            ISystem sys = textBoxSystemName.Text.Length > 0 ? SystemCache.FindSystem(textBoxSystemName.Text, discoveryform.galacticMapping, true) : new SystemClass("Unknown", numberBoxDoubleX.Value, numberBoxDoubleY.Value, numberBoxDoubleZ.Value);     // find centre, i.e less 1 ly distance

            if (sys != null)
            {
                var list = HistoryList.FindSystemsWithinLy(discoveryform.history.EntryOrder(), sys, numberBoxMinRadius.Value, numberBoxMaxRadius.Value, !checkBoxCustomCube.Checked);
                ReturnSystems((from x in list select new Tuple<ISystem, double>(x, x.Distance(sys))).ToList());
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Cannot find system ".T(EDTx.FindSystemsUserControl_Cannotfindsystem) + textBoxSystemName.Text, "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void buttonExtDBClick(object sender, EventArgs e)
        {
            ISystem sys = textBoxSystemName.Text.Length > 0 ? SystemCache.FindSystem(textBoxSystemName.Text, discoveryform.galacticMapping, true) : new SystemClass("Unknown", numberBoxDoubleX.Value, numberBoxDoubleY.Value, numberBoxDoubleZ.Value);     // find centre, i.e less 1 ly distance

            if (sys != null)
            {
                Cursor = Cursors.WaitCursor;

                // work out the excluded system name list
                HashSet<string> excluded = extCheckBoxExcludeVisitedSystems.Checked ? discoveryform.history.Visited.Values.Select(x=>x.System.Name).ToHashSet() : new HashSet<string>();

                Task<List<Tuple<ISystem,double>>>.Factory.StartNew(() =>
                {
                    BaseUtils.SortedListDoubleDuplicate<ISystem> distlist = new BaseUtils.SortedListDoubleDuplicate<ISystem>();

                    SystemCache.GetSystemListBySqDistancesFrom(distlist, sys.X, sys.Y, sys.Z, 50000,
                                numberBoxMinRadius.Value, numberBoxMaxRadius.Value, !checkBoxCustomCube.Checked, excluded);

                    var res = (from x in distlist select new Tuple<ISystem, double>(x.Value, x.Value.Distance(sys))).ToList();

                    return res;

                }).ContinueWith(task => this.Invoke(new Action(() =>
                {
                    var res = task.Result;
                    ReturnSystems(res);
                    Cursor = Cursors.Default;
                })));
            }
            else
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Cannot find system ".T(EDTx.FindSystemsUserControl_Cannotfindsystem) + textBoxSystemName.Text, "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            Excel?.Invoke();
        }

        bool ignoresystemnamechange = false;
        private void textBoxSystemName_TextChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Text changed " + textBoxSystemName.Text);
            if (!ignoresystemnamechange)
            {
                SetXYZ();
                ValidateEnable();
            }
        }

        private void SetXYZ()
        {
            ISystem sys = SystemCache.FindSystem(textBoxSystemName.Text, discoveryform.galacticMapping, false);      // not doing edsm as done via INIT

            if (sys != null && sys.HasCoordinate)
            {
                numberBoxDoubleX.Value = sys.X;
                numberBoxDoubleY.Value = sys.Y;
                numberBoxDoubleZ.Value = sys.Z;
            }
            else
            {
                numberBoxDoubleX.SetBlank();
                numberBoxDoubleY.SetBlank();
                numberBoxDoubleZ.SetBlank();
            }
        }

        private void textBoxSystemName_RadiusChanged(object sender, EventArgs e)
        {
            ValidateEnable();
        }

        private void numberBoxDoubleXYZ_Enter(object sender, EventArgs e)
        {
            ignoresystemnamechange = true;
            textBoxSystemName.Text = "";
            numberBoxDoubleX.SetNonBlank();
            numberBoxDoubleY.SetNonBlank();
            numberBoxDoubleZ.SetNonBlank();
            ignoresystemnamechange = false;
            ValidateEnable();
        }

        private void ValidityNumberBox(Object box, bool value)
        {
            ValidateEnable();
        }

        void ValidateEnable()
        {
            buttonExtNames.Enabled = textBoxSystemName.Text.Length > 0;

            bool validradius = numberBoxMinRadius.IsValid && numberBoxMaxRadius.IsValid;
            buttonExtEDSM.Enabled = buttonExtNames.Enabled = validradius && (textBoxSystemName.Text.Length > 0 || (numberBoxDoubleX.IsValid && numberBoxDoubleY.IsValid && numberBoxDoubleZ.IsValid));
            buttonExtDB.Enabled = buttonExtVisited.Enabled = validradius && (textBoxSystemName.Text.Length > 0 || (numberBoxDoubleX.IsValid && numberBoxDoubleY.IsValid && numberBoxDoubleZ.IsValid));
        }

    }
}
