/*
 * Copyright 2016 - 2025 EDDiscovery development team
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
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class SurveyorPanel : UserControlCommonBase
    {
        #region UI
        protected enum CtrlList
        {
            allplanets, showAmmonia, showEarthlike, showWaterWorld, showHMC, showMR,
            showTerraformable, showVolcanism, showRinged, showEccentricity, lowradius,
            signals, GeoSignals, BioSignals, isLandable, isLandableWithAtmosphere,
            largelandable, isLandableWithVolcanism,
            // 18
            allstars, beltclusters,
            // 20
            showValues, moreinfo, showGravity, atmos, temp, volcanism, showsignals, autohide, donthidefssmode, hideMapped, showsysinfo, showscansum, showstarclass, showdividers,
            // 31
            alignleft, aligncenter, alignright,
            //34
            showsignalmismatch
        };

        private bool[] ctrlset; // holds current state of each control above

        private bool IsSet(CtrlList v)
        {
            return ctrlset[(int)v];
        }

        // The searches selected and active from the DB setting
        private string[] searchesactivetext;
        private string[] searchesactivevoice;

        // from DB, set up ctrlset, and set the defaults
        private void PopulateCtrlList()
        {
            ctrlset = GetSettingAsCtrlSet<CtrlList>(DefaultSetting);
            alignment = ctrlset[(int)CtrlList.alignright] ? StringAlignment.Far : ctrlset[(int)CtrlList.aligncenter] ? StringAlignment.Center : StringAlignment.Near;
            string sat = GetSetting(dbSearchesText, HistoryListQueries.Instance.DefaultSearches(SettingsSplittingChar), true);
            searchesactivetext = sat.SplitNoEmptyStartFinish('\u2188');
            searchesactivevoice = GetSetting(dbSearchesVoice, sat, true).SplitNoEmptyStartFinish('\u2188');
        }

        protected virtual bool DefaultSetting(CtrlList e)
        {
            bool def = (e != CtrlList.alignright && e != CtrlList.aligncenter && e != CtrlList.autohide && e != CtrlList.lowradius
                && e != CtrlList.allplanets && e != CtrlList.allstars && e != CtrlList.beltclusters);
            return def;
        }

        private void extButtonPlanets_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();

            displayfilter.UC.AddAllNone();
            displayfilter.UC.Add(CtrlList.allplanets.ToString(), "Show All Planets".Tx(), BaseUtils.Icons.IconSet.GetImage("Bodies.Planets.Terrestrial.HMCv10"));
            displayfilter.UC.Add(CtrlList.showAmmonia.ToString(), "Ammonia World".Tx(), BaseUtils.Icons.IconSet.GetImage("Bodies.Planets.Terrestrial.AMWv1"));
            displayfilter.UC.Add(CtrlList.showEarthlike.ToString(), "Earthlike World".Tx(), BaseUtils.Icons.IconSet.GetImage("Bodies.Planets.Terrestrial.ELWv5"));
            displayfilter.UC.Add(CtrlList.showWaterWorld.ToString(), "Water World".Tx(), BaseUtils.Icons.IconSet.GetImage("Bodies.Planets.Terrestrial.WTRv7"));
            displayfilter.UC.Add(CtrlList.showHMC.ToString(), "High metal content body".Tx(), BaseUtils.Icons.IconSet.GetImage("Bodies.Planets.Terrestrial.HMCv3"));
            displayfilter.UC.Add(CtrlList.showMR.ToString(), "Metal-rich body".Tx(), BaseUtils.Icons.IconSet.GetImage("Bodies.Planets.Terrestrial.MRBv5"));
            displayfilter.UC.Add(CtrlList.showTerraformable.ToString(), "Terraformable".Tx(), BaseUtils.Icons.IconSet.GetImage("Bodies.Planets.Terrestrial.ELWv5"));
            displayfilter.UC.Add(CtrlList.showVolcanism.ToString(), "Has volcanism".Tx(), BaseUtils.Icons.IconSet.GetImage("Bodies.Planets.Terrestrial.HMCv37"));
            displayfilter.UC.Add(CtrlList.showRinged.ToString(), "Has Rings".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_RingOnly);
            displayfilter.UC.Add(CtrlList.showEccentricity.ToString(), "High eccentricity".Tx(), global::EDDiscovery.Icons.Controls.Eccentric);
            displayfilter.UC.Add(CtrlList.lowradius.ToString(), "Tiny body".Tx(), global::EDDiscovery.Icons.Controls.Scan_SizeSmall);
            displayfilter.UC.Add(CtrlList.GeoSignals.ToString(), "Has geological signals".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.UC.Add(CtrlList.BioSignals.ToString(), "Has biological signals".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.UC.Add(CtrlList.signals.ToString(), "Has any other signals".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_Signals);
            displayfilter.UC.Add(CtrlList.isLandable.ToString(), "Landable".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);
            displayfilter.UC.Add(CtrlList.isLandableWithAtmosphere.ToString(), "Landable with atmosphere".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);
            displayfilter.UC.Add(CtrlList.largelandable.ToString(), "Landable and large".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);
            displayfilter.UC.Add(CtrlList.isLandableWithVolcanism.ToString(), "Landable with volcanism".Tx(), global::EDDiscovery.Icons.Controls.Scan_Bodies_LandablePlanet);

            CommonCtrl(displayfilter, extButtonPlanets);

        }


        private const char SettingsSplittingChar = '\u2188';     // pick a crazy one soe

        private void extButtonSearches_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();
            displayfilter.UC.AddAllNone(3);
            displayfilter.UC.AddGroupItem(HistoryListQueries.Instance.DefaultSearches(SettingsSplittingChar), "Default".Tx(), checkmap: 3);
            displayfilter.UC.SettingsSplittingChar = '\u2188';     // pick a crazy one soe

            var searches = HistoryListQueries.Instance.Searches.Where(x => x.UserOrBuiltIn).ToList();
            foreach (var s in searches)
                displayfilter.UC.Add(s.Name, s.Name, checkmap: 3, checkbuttontooltiptext: new string[] { "Text Output", "Voice Output" });

            var under = extButtonSearches;

            displayfilter.CloseBoundaryRegion = new Size(32, under.Height);
            displayfilter.AllOrNoneBack = false;
            displayfilter.UC.ImageSize = new Size(24, 24);
            displayfilter.UC.ScreenMargin = new Size(0, 0);
            displayfilter.UC.MultiColumnSlide = true;

            displayfilter.SaveSettings = (s, o) =>
            {
                PutSetting(dbSearchesText, displayfilter.GetChecked(0));
                PutSetting(dbSearchesVoice, displayfilter.GetChecked(1));
                PopulateCtrlList();
                DrawAll(cur_sys);
            };

            string primarysetting = GetSetting(dbSearchesText, "");
            displayfilter.Show(new string[] { primarysetting, GetSetting(dbSearchesVoice, primarysetting) }, under, this.FindForm());
        }

        private void extButtonStars_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();

            displayfilter.UC.AddAllNone();
            displayfilter.UC.Add(CtrlList.allstars.ToString(), "Show All Stars".Tx(), global::EDDiscovery.Icons.Controls.Scan_Star);
            displayfilter.UC.Add(CtrlList.beltclusters.ToString(), "Show Belt Clusters".Tx(), global::EDDiscovery.Icons.Controls.Belt);

            CommonCtrl(displayfilter, extButtonStars);
        }

        private void extButtonShowControl_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();

            displayfilter.UC.AddAllNone();
            displayfilter.UC.Add(CtrlList.showValues.ToString(), "Show values".Tx());
            displayfilter.UC.Add(CtrlList.moreinfo.ToString(), "Show more information".Tx());
            displayfilter.UC.Add(CtrlList.showGravity.ToString(), "Show gravity of landables".Tx());
            displayfilter.UC.Add(CtrlList.atmos.ToString(), "Show atmospheres".Tx());
            displayfilter.UC.Add(CtrlList.temp.ToString(), "Show surface temperature".Tx());
            displayfilter.UC.Add(CtrlList.volcanism.ToString(), "Show volcanism".Tx());
            displayfilter.UC.Add(CtrlList.showsignals.ToString(), "Show signals".Tx());
            displayfilter.UC.Add(CtrlList.showsignalmismatch.ToString(), "Show signal mismatch".Tx());
            displayfilter.UC.Add(CtrlList.autohide.ToString(), "Auto Hide".Tx());
            displayfilter.UC.Add(CtrlList.donthidefssmode.ToString(), "Don't hide in FSS Mode".Tx());
            displayfilter.UC.Add(CtrlList.hideMapped.ToString(), "Hide already mapped bodies".Tx());
            displayfilter.UC.Add(CtrlList.showsysinfo.ToString(), "Show system info always".Tx());
            displayfilter.UC.Add(CtrlList.showscansum.ToString(), "Show scan summary always".Tx());
            displayfilter.UC.Add(CtrlList.showstarclass.ToString(), "Show star class in system info".Tx());
            displayfilter.UC.Add(CtrlList.showdividers.ToString(), "Show dividers".Tx());

            CommonCtrl(displayfilter, extButtonShowControl);
        }

        private void extButtonAlignment_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();

            string lt = CtrlList.alignleft.ToString();
            string ct = CtrlList.aligncenter.ToString();
            string rt = CtrlList.alignright.ToString();

            displayfilter.UC.Add(lt, "Alignment Left".Tx(), global::EDDiscovery.Icons.Controls.AlignLeft, exclusivetags: ct + ";" + rt, disableuncheck: true);
            displayfilter.UC.Add(ct, "Alignment Center".Tx(), global::EDDiscovery.Icons.Controls.AlignCentre, exclusivetags: lt + ";" + rt, disableuncheck: true);
            displayfilter.UC.Add(rt, "Alignment Right".Tx(), global::EDDiscovery.Icons.Controls.AlignRight, exclusivetags: lt + ";" + ct, disableuncheck: true);
            displayfilter.CloseOnChange = true;
            CommonCtrl(displayfilter, extButtonAlignment);
        }

        private void CommonCtrl(ExtendedControls.CheckedIconNewListBoxForm displayfilter, Control under, string saveasstring = null)
        {
            displayfilter.CloseBoundaryRegion = new Size(32, under.Height);
            displayfilter.AllOrNoneBack = false;
            displayfilter.UC.ImageSize = new Size(24, 24);
            displayfilter.UC.ScreenMargin = new Size(0, 0);
            displayfilter.UC.MultiColumnSlide = true;

            displayfilter.SaveSettings = (s, o) =>
            {
                if (saveasstring == null)
                    PutBoolSettingsFromString(s, displayfilter.UC.TagList());
                else
                    PutSetting(saveasstring, s);

                PopulateCtrlList();
                SetVisibility();
                DrawAll(cur_sys);
            };

            if (saveasstring == null)
                displayfilter.Show(typeof(CtrlList), ctrlset, under, this.FindForm());
            else
                displayfilter.Show(GetSetting(saveasstring, ""), under, this.FindForm());
        }

        private void extButtonFont_Click(object sender, EventArgs e)
        {
            Font f = BaseUtils.FontDialog.SelectFont(this.FindForm(), displayfont ?? this.Font, true);
            string setting = BaseUtils.FontHandler.GetFontSettingString(f);
            //System.Diagnostics.Debug.WriteLine($"Surveyor Font selected {setting}");
            PutSetting(dbFont, setting);
            displayfont = f;
            DrawAll(cur_sys, true);
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
            DrawAll(cur_sys, true);
        }

        private void extButtonFSS_Click(object sender, EventArgs e)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            int width = 430;

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("Text", typeof(ExtendedControls.ExtTextBox), fsssignalstodisplay, new Point(10, 40), new Size(width - 10 - 20, 110), "List Names to show") { TextBoxMultiline = true });

            f.AddOK(new Point(width - 100, 180));
            f.AddCancel(new Point(width - 200, 180));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "OK")
                {
                    f.ReturnResult(DialogResult.OK);
                }
                else if (controlname == "Cancel" || controlname == "Close")
                {
                    f.ReturnResult(DialogResult.Cancel);
                }
            };

            DialogResult res = f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "List signals to display, semicolon seperated".Tx(), closeicon: true);
            if (res == DialogResult.OK)
            {
                fsssignalstodisplay = f.Get("Text");
                PutSetting(dbfsssignals, fsssignalstodisplay);
                CalculateThenDrawSystemSignals(cur_sys);
            }
        }

        #endregion

    }
}

  