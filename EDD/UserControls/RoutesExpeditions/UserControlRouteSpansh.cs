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
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlRoute
    {

        #region Spansh

        const int topmargin = 30;
        const int dataleft = 140;
        Size numberboxsize = new Size(100, 24);
        Size comboboxsize = new Size(200, 24);
        Size textboxsize = new Size(200, 28);
        Size checkboxsize = new Size(154, 24);
        Size labelsize = new Size(dataleft - 4, 24);
        System.Windows.Forms.Timer waitforspanshresulttimer = new System.Windows.Forms.Timer();
        string spanshjobname;

        // common queries get an index
        enum Spanshquerytype { RoadToRiches = 0, AmmoniaWorlds = 1, EarthLikes = 2, RockyHMC = 3, Neutron, TradeRouter, FleetCarrier, GalaxyPlotter, ExoMastery };
        Spanshquerytype spanshquerytype;

        private void extButtonSpanshRoadToRiches_Click(object sender, EventArgs e)
        {
            CommonSpanshQuery(Spanshquerytype.RoadToRiches);
        }

        private void extButtonSpanshAmmoniaWorlds_Click(object sender, EventArgs e)
        {
            CommonSpanshQuery(Spanshquerytype.AmmoniaWorlds);
        }

        private void extButtonSpanshEarthLikes_Click(object sender, EventArgs e)
        {
            CommonSpanshQuery(Spanshquerytype.EarthLikes);
        }
        private void extButtonSpanshRockyHMC_Click(object sender, EventArgs e)
        {
            CommonSpanshQuery(Spanshquerytype.RockyHMC);
        }

        double[] csrange = new double[] { -1, -1, -1, -1 };       // size to number of common queries
        int[] csradius = new int[] { 25, 500, 500, 500 };       // size to number of common queries
        int[] csmaxsys = new int[] { 100, 100, 100, 100 };
        int[] csmaxls = new int[] { 1000000, 50000, 50000, 50000 };
        bool[] csavoidt = new bool[] { true, true, true, true };
        int csminvalue = 100000;
        bool csusemap = false;

        private void CommonSpanshQuery(Spanshquerytype qt)
        {
            bool roadtoriches = qt == Spanshquerytype.RoadToRiches;
            int si = (int)qt;         // we use value of enumberation as index into save variables

            ConfigurableForm f = new ConfigurableForm();

            int vpos = topmargin;

            if (csrange[si] < 0)
                csrange[si] = textBox_Range.Value;

            f.AddLabelAndEntry("Range", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("range", csrange[si], new Point(dataleft, 0), numberboxsize, "Maximum jump range of ship or range between systems your happy with") { NumberBoxLongMinimum = 4 });
            f.AddLabelAndEntry("Search radius", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("radius", csradius[si], new Point(dataleft, 0), numberboxsize, "Search radius along path to search for worlds") { NumberBoxLongMinimum = 10 });
            f.AddLabelAndEntry("Max Systems", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("maxsystems", csmaxsys[si], new Point(dataleft, 0), numberboxsize, "Maximum systems to route through") { NumberBoxLongMinimum = 1 });

            if (roadtoriches)
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("mappingvalue", csusemap, "Use mapping value", new Point(4, 0), checkboxsize, "Base on mapping not scan value") { ContentAlign = ContentAlignment.MiddleRight });

            f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("avoidthargoids", csavoidt[si], "Avoid thargoids", new Point(4, 0), checkboxsize, "Avoid Thargoids") { ContentAlign = ContentAlignment.MiddleRight });
            f.AddLabelAndEntry("Max LS", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("maxls", csmaxls[si], new Point(dataleft, 0), numberboxsize, "Maximum LS from arrival to consider") { NumberBoxLongMinimum = 10 });

            if (roadtoriches)
                f.AddLabelAndEntry("Min Scan Value", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("minscan", csminvalue, new Point(dataleft, 0), numberboxsize, "Minimum value of body") { NumberBoxLongMinimum = 100 });

            f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("loop", typeof(ExtCheckBox), "Return to start", new Point(4, 0), checkboxsize, "Return to start system for route") { CheckBoxChecked = textBox_To.Text.IsEmpty(), Enabled = !textBox_To.Text.HasChars(), ContentAlign = ContentAlignment.MiddleRight });

            f.AddOK(new Point(140, vpos + 16), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
            f.InstallStandardTriggers();
            f.Trigger += (name, text, obj) => { f.GetControl("OK").Enabled = f.IsAllValid(); };

            if (f.ShowDialogCentred(FindForm(), FindForm().Icon, qt.ToString().SplitCapsWordFull(), closeicon: true) == DialogResult.OK)
            {
                EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();

                csrange[si] = f.GetDouble("range").Value;
                csradius[si] = f.GetInt("radius").Value;
                csmaxsys[si] = f.GetInt("maxsystems").Value;
                csavoidt[si] = f.GetBool("avoidthargoids").Value;
                csmaxls[si] = f.GetInt("maxls").Value;

                if (roadtoriches)
                {
                    csminvalue = f.GetInt("minscan").Value;
                    csusemap = f.GetBool("mappingvalue").Value;
                }

                bool loop = f.GetBool("loop").Value;

                spanshjobname = sp.RequestRoadToRichesAmmoniaEarthlikes(textBox_From.Text, textBox_To.Text, csrange[si],
                                                    csradius[si], csmaxsys[si],
                                                    csavoidt[si], loop, csmaxls[si],
                                                    roadtoriches ? csminvalue : 1,
                                                    roadtoriches ? csusemap : default(bool?),
                                                    roadtoriches ? null : qt == Spanshquerytype.AmmoniaWorlds ? new string[] { "Ammonia world" } :
                                                                          qt == Spanshquerytype.RockyHMC ? new string[] { "Rocky body", "High metal content world" } :
                                                                          new string[] { "Earth-like world" }
                                                    );
                StartSpanshQueryOp(qt);

                labelRouteName.Text = $"{textBox_From.Text}{(loop ? " Loop" : (" - " + textBox_To.Text))} ({qt.ToString()})";
            }
        }

        double njumprange = -1;
        int nrefficiency = 60;

        private void extButtonNeutronRouter_Click(object sender, EventArgs e)
        {
            Ship si = DiscoveryForm.History.GetLast?.ShipInformation;

            if (si != null)
            {
                ConfigurableForm f = new ConfigurableForm();

                int vpos = topmargin;

                if (njumprange < 0)
                    njumprange = textBox_Range.Value;

                f.AddLabelAndEntry("Range", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("range", njumprange, new Point(dataleft, 0), numberboxsize, "Maximum jump range of ship or range between systems your happy with") { NumberBoxLongMinimum = 4 });
                f.AddLabelAndEntry("Efficiency", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("efficiency", nrefficiency, new Point(dataleft, 0), numberboxsize, "How far off the straight line route to allow. 100 means no deviation") { NumberBoxLongMinimum = 1, NumberBoxLongMaximum = 100 });
                f.AddOK(new Point(140, vpos + 16), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
                f.InstallStandardTriggers();
                f.Trigger += (name, text, obj) => { f.GetControl("OK").Enabled = f.IsAllValid(); };

                if (f.ShowDialogCentred(FindForm(), FindForm().Icon, "Neutron Router", closeicon: true) == DialogResult.OK)
                {
                    EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();
                    njumprange = f.GetDouble("range").Value;
                    nrefficiency = f.GetInt("efficiency").Value;
                    spanshjobname = sp.RequestNeutronRouter(textBox_From.Text, textBox_To.Text, njumprange, nrefficiency, si);
                    if (spanshjobname != null)
                    {
                        StartSpanshQueryOp(Spanshquerytype.Neutron);
                        labelRouteName.Text = $"{textBox_From.Text} - {textBox_To.Text} (Neutron)";
                    }
                    else
                        ExtendedControls.MessageBoxTheme.Show("Ship information does not have FSD Spec - will be corrected when you log into Elite again");

                }
            }
        }

        private double traderange = -1;
        private string tradestation = null;
        private long tradecapital = 1000;
        private int tradecargo = 7;
        private int tradehops = 5;
        private int tradedls = 1000000;
        private double trademage = 30;
        private bool tradelargepad = false;
        private bool tradeallowplanetary = false;
        private bool tradeallowplayerowned = false;
        private bool tradeallowrestrictedaccess = false;
        private bool tradeprohibited = false;
        private bool tradeloops = true;
        private bool tradepermit = false;

        private void extButtonSpanshTradeRouter_Click(object sender, EventArgs e)
        {
            ConfigurableForm f = new ConfigurableForm();

            EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();
            var stationlist = sp.GetStationsByDump(new SystemClass(textBox_From.Text));
            var withmarket = stationlist?.Where(x => x.HasMarket && x.IsFleetCarrier == false).ToList();
            if (withmarket != null && withmarket.Count > 0)
            {
                var stationnames = withmarket.Select(x => x.StationName).OrderBy(x => x).ToList();

                int vpos = topmargin;

                if (tradestation == null)
                    tradestation = stationnames[0];

                if (traderange < 0)
                    traderange = textBox_Range.Value;

                f.AddLabelAndEntry("Source Station", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("station", tradestation, new Point(dataleft, 0), comboboxsize, "Station name", stationnames));
                f.AddLabelAndEntry("Starting Capital", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("capital", tradecapital, new Point(dataleft, 0), numberboxsize, "Starting capital") { NumberBoxLongMinimum = 100 });
                f.AddLabelAndEntry("Max Hop Distance", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("range", traderange, new Point(dataleft, 0), numberboxsize, "Maximum jump range of ship or range between systems your happy with") { NumberBoxLongMinimum = 4 });
                f.AddLabelAndEntry("Max Cargo", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("cargo", tradecargo, new Point(dataleft, 0), numberboxsize, "Maximum cargo you can carry") { NumberBoxLongMinimum = 1 });
                f.AddLabelAndEntry("Max Hops", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("hops", tradehops, new Point(dataleft, 0), numberboxsize, "Maximum hops between stations") { NumberBoxLongMinimum = 1 });
                f.AddLabelAndEntry("Max Arrival Distance", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("dls", tradedls, new Point(dataleft, 0), numberboxsize, "Maximum arrival distance of station") { NumberBoxLongMinimum = 1 });
                f.AddLabelAndEntry("Max Market Age (Days)", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("mage", trademage, new Point(dataleft, 0), numberboxsize, "Maximum age of the station data you accept") { NumberBoxDoubleMinimum = 0.01 });
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("largepad", tradelargepad, "Require Large Pad", new Point(4, 0), checkboxsize, "Ship needs a large pad") { ContentAlign = ContentAlignment.MiddleRight });
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("planetary", tradeallowplanetary, "Allow Planetary", new Point(4, 0), checkboxsize, "Accept planetary ports") { ContentAlign = ContentAlignment.MiddleRight });
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("playerowned", tradeallowplayerowned, "Allow Player Owned", new Point(4, 0), checkboxsize, "Accept player owned ports") { ContentAlign = ContentAlignment.MiddleRight });
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("restricted", tradeallowrestrictedaccess, "Allow Restricted Access", new Point(4, 0), checkboxsize, "Accept restriced access ports") { ContentAlign = ContentAlignment.MiddleRight });
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("prohibited", tradeprohibited, "Allow Prohibited", new Point(4, 0), checkboxsize, "Allow prohibited commodities") { ContentAlign = ContentAlignment.MiddleRight });
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("loop", tradeloops, "Avoid Loops", new Point(4, 0), checkboxsize, "Don't loop back to previous station") { ContentAlign = ContentAlignment.MiddleRight });
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("permit", tradepermit, "Allow Permit Systems", new Point(4, 0), checkboxsize, "You have the permit to these systems") { ContentAlign = ContentAlignment.MiddleRight });
                f.AddOK(new Point(140, vpos + 16), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
                f.InstallStandardTriggers();
                f.Trigger += (name, text, obj) => { f.GetControl("OK").Enabled = f.IsAllValid(); };

                if (f.ShowDialogCentred(FindForm(), FindForm().Icon, "Trade Router", closeicon: true) == DialogResult.OK)
                {
                    tradestation = f.Get("station");
                    traderange = f.GetDouble("range").Value;
                    tradecapital = f.GetLong("capital").Value;
                    tradecargo = f.GetInt("cargo").Value;
                    tradehops = f.GetInt("hops").Value;
                    tradedls = f.GetInt("dls").Value;
                    trademage = f.GetDouble("mage").Value;
                    tradelargepad = f.GetBool("largepad").Value;
                    tradeallowplanetary = f.GetBool("planetary").Value;
                    tradeallowplayerowned = f.GetBool("playerowned").Value;
                    tradeallowrestrictedaccess = f.GetBool("restricted").Value;
                    tradeprohibited = f.GetBool("prohibited").Value;
                    tradeloops = f.GetBool("loop").Value;
                    tradepermit = f.GetBool("permit").Value;

                    spanshjobname = sp.RequestTradeRouter(textBox_From.Text, tradestation,
                        tradehops, traderange, tradecapital, tradecargo, tradedls, (int)(trademage * 86400),
                        tradelargepad, tradeallowplanetary, tradeallowplayerowned, tradeallowrestrictedaccess, tradeprohibited, tradeloops, tradepermit);
                    StartSpanshQueryOp(Spanshquerytype.TradeRouter);

                    labelRouteName.Text = $"{textBox_From.Text} @ {tradestation} (Trade)";
                }
            }
            else
            {
                MessageBoxTheme.Show(this.FindForm(), $"No stations found at {textBox_From.Text}", "Warning".Tx(), MessageBoxButtons.OK);
            }
        }

        private int fccapused = 0;
        private bool fcdeterminetritium = true;
        private bool fcsquadcarrier = false;
        private List<string> fcdestinations = new List<string>() { };

        private void extButtonFleetCarrier_Click(object sender, EventArgs e)
        {
            ConfigurableForm f = new ConfigurableForm();

            int vpos = topmargin;

            f.AddLabelAndEntry("Capacity Used", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("cap", fccapused, new Point(dataleft, 0), numberboxsize, "Capacity in use from upper right corner of carrier management screen") { NumberBoxLongMinimum = 0 });
            f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("squadron", fcsquadcarrier, "Squadron Carrier Not Fleet Carrier", new Point(4, 0), checkboxsize, "Select to route for squadron carrier") { ContentAlign = ContentAlignment.MiddleRight });
            f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("tritium", fcdeterminetritium, "Determine Tritium", new Point(4, 0), checkboxsize, "Calculate how much tritium is needed") { ContentAlign = ContentAlignment.MiddleRight });

            int addvpos = vpos;                         // record where these fields start from
            int controlnumber = 0;                      // a unique ID which always incremements

            foreach (var fc in fcdestinations)          // add any stored destinations
            {
                AddFCDest(f, controlnumber++, vpos, fc);
                vpos += 32;
            }

            f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("add", typeof(ExtButton), "+ Stop", new Point(4, 0), numberboxsize, "Add a new stop"));
            f.AddOK(new Point(140, vpos + 16), "OK");
            f.InstallStandardTriggers();
            f.Trigger += (name, control, obj) =>
            {
                if (control == "add")
                {
                    int numcontrols = f.GetByStartingName("idest").Length;     // get number up there
                    int pos = addvpos + numcontrols * 32;           // work out where next should be
                    f.MoveControls(pos - 10, 32);                     // move anything after this (less a bit for safety) to space
                    AddFCDest(f, controlnumber++, pos);             // and add
                    f.UpdateEntries();
                }
                else if (control.StartsWith("del:"))
                {
                    //System.Diagnostics.Debug.WriteLine($"Delete control {control}");
                    f.MoveControls(control, -32);                   // move everything at or below this up
                    f.Remove(control);                         // and remove the two controls
                    f.Remove("idest:" + control.Substring(4));
                    f.UpdateEntries();
                }
                else
                    f.GetControl("OK").Enabled = f.IsAllValid();
            };

            if (f.ShowDialogCentred(FindForm(), FindForm().Icon, "Fleet Carrier Router", closeicon: true) == DialogResult.OK)
            {
                fccapused = f.GetInt("cap").Value;
                fcdeterminetritium = f.GetBool("tritium").Value;
                fcsquadcarrier = f.GetBool("squadron").Value;
                fcdestinations = f.GetByStartingName("idest").Where(x => x.Length > 0).ToList();

                var destlist = new List<string>(fcdestinations);
                destlist.Add(textBox_To.Text);

                EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();
                spanshjobname = sp.RequestFleetCarrierRouter(textBox_From.Text, destlist, fccapused, fcdeterminetritium, fcsquadcarrier);
                StartSpanshQueryOp(Spanshquerytype.FleetCarrier);

                labelRouteName.Text = $"{textBox_From.Text} - {textBox_To.Text} (FC)";
            }
        }


        private void AddFCDest(ConfigurableForm f, int controlnumber, int vpos, string value = "")
        {
            ExtTextBoxAutoComplete ac = new ExtTextBoxAutoComplete();
            ac.SetAutoCompletor(SystemCache.ReturnSystemAutoCompleteList, true);
            f.Add(new ConfigurableEntryList.Entry("del:" + controlnumber, typeof(ExtButton), "X", new Point(textboxsize.Width + 10, vpos), new Size(24, 24), "Delete stop"));
            f.Add(new ConfigurableEntryList.Entry(ac, "idest:" + controlnumber, value, new Point(4, vpos), textboxsize, "Set an intermediate stop"));
        }

        private int gpcargo = 0;
        private bool gpsupercharged = false;
        private bool gpusesupercharge = true;
        private bool gpusefsdinjections = false;
        private bool gpexcludesecondary = true;
        private bool gprefueleveryscoopable = true;
        private string gpalgo = "Optimistic";

        private void extButtonSpanshGalaxyPlotter_Click(object sender, EventArgs e)
        {
            ConfigurableForm f = new ConfigurableForm();

            Ship si = DiscoveryForm.History.GetLast?.ShipInformation;

            if (si != null)
            {
                int vpos = topmargin;

                f.AddLabelAndEntry("Cargo", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("cargo", gpcargo, new Point(dataleft, 0), numberboxsize, "Amount of cargo to carry") { NumberBoxLongMinimum = 0 });
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("asc", gpsupercharged, "Already supercharged", new Point(4, 0), checkboxsize, "Ship already is neutron boosted") { ContentAlign = ContentAlignment.MiddleRight });
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("usc", gpusesupercharge, "Use supercharge", new Point(4, 0), checkboxsize, "Use neutron boosts") { ContentAlign = ContentAlignment.MiddleRight });
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("fsd", gpusefsdinjections, "Use FSD Injections", new Point(4, 0), checkboxsize, "Use FSD Injections to speed travel") { ContentAlign = ContentAlignment.MiddleRight });
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("ess", gpexcludesecondary, "Exclude secondary stars", new Point(4, 0), checkboxsize, "Exclude secondary stars from consideration for neutron boosting/scooping") { ContentAlign = ContentAlignment.MiddleRight });
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("ref", gprefueleveryscoopable, "Refuel Every Scoopable", new Point(4, 0), checkboxsize, "Every possible time top up tank back to max") { ContentAlign = ContentAlignment.MiddleRight });
                f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("algo", gpalgo, new Point(4, 0), textboxsize, "Pick routing algorithm", new List<string> { "Optimistic", "Fuel", "Fuel Jumps", "Guided", "Pessimistic"}));

                f.AddOK(new Point(140, vpos + 16), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
                f.InstallStandardTriggers();
                f.Trigger += (name, text, obj) => { f.GetControl("OK").Enabled = f.IsAllValid(); };

                if (f.ShowDialogCentred(FindForm(), FindForm().Icon, "Galaxy Plotter", closeicon: true) == DialogResult.OK)
                {
                    EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();

                    gpcargo = f.GetInt("cargo").Value;
                    gpsupercharged = f.GetBool("asc").Value;
                    gpusesupercharge = f.GetBool("usc").Value;
                    gpusefsdinjections = f.GetBool("fsd").Value;
                    gpexcludesecondary = f.GetBool("ess").Value;
                    gprefueleveryscoopable = f.GetBool("ref").Value;
                    gpalgo = f.Get("algo");

                    // this may fail due to not having fsd info
                    spanshjobname = sp.RequestGalaxyPlotter(textBox_From.Text, textBox_To.Text, gpcargo, gpsupercharged, gpusesupercharge, gpusefsdinjections,
                                        gpexcludesecondary, gprefueleveryscoopable, si, gpalgo.ToLowerInvariant().Replace(" ", "_"));
                    if (spanshjobname != null)
                    {
                        StartSpanshQueryOp(Spanshquerytype.GalaxyPlotter);
                        labelRouteName.Text = $"{textBox_From.Text} - {textBox_To.Text} (Plotter)";
                    }
                    else
                        ExtendedControls.MessageBoxTheme.Show("Ship information does not have FSD Spec - will be corrected when you log into Elite again");

                }
            }
        }

        private double exorange = -1;
        private int exosearchradius = 25;
        private int exomaxsystems = 100;
        private int exomaxls = 1000000;
        private int exominvalue = 100000;
        private bool exthargoids = true;
        private void extButtonExoMastery_Click(object sender, EventArgs e)
        {
            ConfigurableForm f = new ConfigurableForm();

            int vpos = topmargin;

            if (exorange < 0)
                exorange = textBox_Range.Value;

            f.AddLabelAndEntry("Range", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("range", exorange, new Point(dataleft, 0), numberboxsize, "Maximum jump range of ship or range between systems your happy with") { NumberBoxLongMinimum = 4 });
            f.AddLabelAndEntry("Search radius", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("radius", exosearchradius, new Point(dataleft, 0), numberboxsize, "Search radius along path to search for worlds") { NumberBoxLongMinimum = 10 });
            f.AddLabelAndEntry("Max Systems", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("maxsystems", exomaxsystems, new Point(dataleft, 0), numberboxsize, "Maximum systems to route through") { NumberBoxLongMinimum = 1 });
            f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("avoidthargoids", exthargoids, "Avoid thargoids", new Point(4, 0), checkboxsize, "Avoid Thargoids") { ContentAlign = ContentAlignment.MiddleRight });
            f.Add(ref vpos, 32, new ConfigurableEntryList.Entry("loop", typeof(ExtCheckBox), "Return to start", new Point(4, 0), checkboxsize, "Return to start system for route") { CheckBoxChecked = textBox_To.Text.IsEmpty(), Enabled = !textBox_To.Text.HasChars(), ContentAlign = ContentAlignment.MiddleRight });
            f.AddLabelAndEntry("Max LS", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("maxls", exomaxls, new Point(dataleft, 0), numberboxsize, "Maximum LS from arrival to consider") { NumberBoxLongMinimum = 10 });
            f.AddLabelAndEntry("Min Value", new Point(4, 4), ref vpos, 32, labelsize, new ConfigurableEntryList.Entry("minv", exominvalue, new Point(dataleft, 0), numberboxsize, "Minimum value of scans") { NumberBoxLongMinimum = 100 });
            f.AddOK(new Point(140, vpos + 16), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
            f.InstallStandardTriggers();
            f.Trigger += (name, text, obj) => { f.GetControl("OK").Enabled = f.IsAllValid(); };

            if (f.ShowDialogCentred(FindForm(), FindForm().Icon, "Expressway to Exomastery", closeicon: true) == DialogResult.OK)
            {
                EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();

                exorange = f.GetDouble("range").Value;
                exosearchradius = f.GetInt("radius").Value;
                exomaxsystems = f.GetInt("maxsystems").Value;
                exomaxls = f.GetInt("maxls").Value;
                exominvalue = f.GetInt("minv").Value;
                bool loop = f.GetBool("loop").Value;

                spanshjobname = sp.RequestExomastery(textBox_From.Text, textBox_To.Text, exorange,
                                                    exosearchradius, exomaxsystems,
                                                    loop, exomaxls, exthargoids, exominvalue);
                StartSpanshQueryOp(Spanshquerytype.ExoMastery);

                labelRouteName.Text = $"{textBox_From.Text}{(loop ? " Loop" : (" - " + textBox_To.Text))} (Exo)";
            }
        }


        private void StartSpanshQueryOp(Spanshquerytype qt)
        {
            if (spanshjobname != null)
            {
                if (spanshjobname.StartsWith("!"))
                {
                    MessageBoxTheme.Show(this.FindForm(), $"Spansh returned error: {spanshjobname.Substring(1)}", "Warning".Tx(), MessageBoxButtons.OK);
                }
                else
                {
                    spanshquerytype = qt;
                    dataGridViewRoute.Rows.Clear();
                    EnableOutputButtons();
                    computing = 2;
                    EnableRouteButtonsIfValid();
                    waitforspanshresulttimer.Interval = 2000;
                    waitforspanshresulttimer.Start();
                }
            }
            else
            {
                MessageBoxTheme.Show(this.FindForm(), $"Spansh failed to return a job id. Try again!", "Warning".Tx(), MessageBoxButtons.OK);
            }
        }

        private void Waitforspanshresulttimer_Tick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Spansh job tick {Environment.TickCount} for {spanshjobname}");
            EliteDangerousCore.Spansh.SpanshClass sp = new EliteDangerousCore.Spansh.SpanshClass();

            string errstring;

            try
            {
                var res = spanshquerytype == Spanshquerytype.TradeRouter ? sp.TryGetTradeRouter(spanshjobname) :
                            spanshquerytype == Spanshquerytype.FleetCarrier ? sp.TryGetFleetCarrierRouter(spanshjobname) :
                            spanshquerytype == Spanshquerytype.GalaxyPlotter ? sp.TryGetGalaxyPlotter(spanshjobname) :
                            spanshquerytype == Spanshquerytype.ExoMastery ? sp.TryGetExomastery(spanshjobname) :
                          spanshquerytype == Spanshquerytype.Neutron ? sp.TryGetNeutronRouter(spanshjobname) : sp.TryGetRoadToRichesAmmonia(spanshjobname);

                if (res.Item1 != null)          // error return
                {
                    errstring = res.Item1;
                }
                else if (res.Item2 == null)      // if not ready, no error
                {
                    waitforspanshresulttimer.Stop();
                    waitforspanshresulttimer.Interval = waitforspanshresulttimer.Interval < 8000 ? waitforspanshresulttimer.Interval * 2 : 8000;
                    waitforspanshresulttimer.Start();
                    return;
                }
                else
                {
                    waitforspanshresulttimer.Stop();

                    ISystem prev = null;
                    foreach (ISystem system in res.Item2)
                    {
                        DataGridViewRow rw = dataGridViewRoute.RowTemplate.Clone() as DataGridViewRow;
                        rw.CreateCells(dataGridViewRoute,
                                system.Name,
                                system.Tag as string ?? "",
                                prev != null ? system.Distance(prev).ToString("0.#") : "",
                                system.X.ToString("0.####"),
                                system.Y.ToString("0.####"),
                                system.Z.ToString("0.####"),
                                "",
                                "",
#if DEBUG
                                system.SystemAddress.ToString()
#else
                                ""
#endif
                                );

                        rw.Tag = system;       // may be null if waypoint or not a system
                        rw.Cells[0].Tag = system.Name;    // write the name of the system into the cells'tag for copying
                        dataGridViewRoute.Rows.Add(rw);
                        prev = system;
                    }

                    labelRouteName.Text += $" {res.Item2.Count} entries";
                    routeSystems = res.Item2;
                    EnableOutputButtons(res.Item2.Count > 0);
                    computing = 0;
                    EnableRouteButtonsIfValid();
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Spansh result exception {ex}");
                errstring = ex.Message;
            }

            waitforspanshresulttimer.Stop();
            MessageBoxTheme.Show(this.FindForm(), $"Spansh returned: {errstring}", "Warning".Tx(), MessageBoxButtons.OK);
            System.Diagnostics.Debug.WriteLine($"Spansh failed with {errstring}");
            computing = 0;
            EnableRouteButtonsIfValid();
        }

        #endregion
    }
}
