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
using System.Diagnostics;
using EliteDangerousCore.DB;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSysInfo : UserControlCommonBase
    {
        public bool IsNotesShowing { get { return richTextBoxNote.Visible; } }

        private string DbSelection { get { return DBName("SystemInformationPanel" ,  "Sel"); } }
        private string DbOSave { get { return DBName("SystemInformationPanel" ,  "Order"); } }

        const int BitSelSystem = 0;
        const int BitSelEDSM = 1;
        const int BitSelVisits = 2;
        const int BitSelBody = 3;
        const int BitSelPosition = 4;
        const int BitSelDistanceFrom = 5;
        const int BitSelSystemState = 6;
        const int BitSelNotes = 7;
        const int BitSelTarget = 8;
        const int BitSelShipInfo = 9;
        const int BitSelFuel = 10;
        const int BitSelCargo = 11;
        const int BitSelMats = 12;
        const int BitSelData = 13;
        const int BitSelCredits = 14;
        const int BitSelGameMode = 15;
        const int BitSelTravel = 16;
        const int BitSelMissions = 17;

        int[] SmallItems = new int[] {BitSelFuel,BitSelCargo,BitSelVisits, BitSelMats, BitSelData , BitSelCredits };

        const int BitSelTotal = 18;
        const int Positions = BitSelTotal * 2;      // two columns of positions, one at 0, one at +300 pixels ish, 
        const int BitSelEDSMButtonsNextLine = 28;
        const int BitSelSkinny = 29;
        const int BitSelDefault = ((1<<BitSelTotal)-1)+(1<<BitSelEDSMButtonsNextLine);
        const int hspacing = 8;
        ToolStripMenuItem[] toolstriplist;          // ref to toolstrip items for each bit above. in same order as bits BitSel..
        public const int HorzPositions = 8;

        int Selection;          // selection bits
        List<BaseUtils.LineStore> Lines;      // Lines on the screen

        #region Init

        public UserControlSysInfo()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            textBoxTarget.SetAutoCompletor(SystemClassDB.ReturnSystemListForAutoComplete);

            // same order as Sel bits are defined in, one bit per selection item.
            toolstriplist = new ToolStripMenuItem[] { toolStripSystem , toolStripEDSM , toolStripVisits, toolStripBody,
                                                        toolStripPosition, toolStripDistanceFrom,
                                                        toolStripSystemState, toolStripNotes, toolStripTarget,
                                                        toolStripShip, toolStripFuel , toolStripCargo, toolStripMaterialCounts,  toolStripDataCount,
                                                        toolStripCredits,
                                                        toolStripGameMode,toolStripTravel, toolStripMissionList };

            Selection = SQLiteDBClass.GetSettingInt(DbSelection, BitSelDefault);
            string rs = SQLiteDBClass.GetSettingString(DbOSave, "-");
            if (rs == "-")
                Reset();
            else
                Lines = BaseUtils.LineStore.Restore(rs, HorzPositions);

            for (int bit = 0; bit < BitSelTotal; bit++)     // new bits added will not be in older lists, need to add on in!
            {
                if (BaseUtils.LineStore.FindValue(Lines, bit + 1) == null)   // if can't find
                {
                    //System.Diagnostics.Debug.WriteLine("Missing item " + bit);
                    Lines.Add(new BaseUtils.LineStore() { Items = new int[HorzPositions] { bit + 1, 0, 0, 0, 0, 0, 0, 0 } });
                }
            }

            discoveryform.OnNewTarget += RefreshTargetDisplay;
            discoveryform.OnNoteChanged += OnNoteChanged;
            discoveryform.OnEDSMSyncComplete += Discoveryform_OnEDSMSyncComplete;

            panelFD.BackgroundImage = EDDiscovery.Icons.Controls.notfirstdiscover;      // just to hide it during boot up

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip1,this);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
            //System.Diagnostics.Debug.WriteLine("UCTG changed in sysinfo to " + uctg.GetHashCode());
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;    // get this whenever current selection or refreshed..
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewTarget -= RefreshTargetDisplay;
            discoveryform.OnNoteChanged -= OnNoteChanged;
            discoveryform.OnEDSMSyncComplete -= Discoveryform_OnEDSMSyncComplete;
            SQLiteDBClass.PutSettingString(DbOSave, BaseUtils.LineStore.ToString(Lines));
            SQLiteDBClass.PutSettingInt(DbSelection, Selection);
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        private void Discoveryform_OnEDSMSyncComplete(int count, string syslist)     // EDSM ~MAY~ have updated the last discovery flag, so redisplay
        {
            //System.Diagnostics.Debug.WriteLine("EDSM SYNC COMPLETED with " + count + " '" + syslist + "'");
            Display(last_he, discoveryform.history);
        }

        bool neverdisplayed = true;
        HistoryEntry last_he = null;

        private void Display(HistoryEntry he, HistoryList hl) =>
            Display(he, hl, true);

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            //System.Diagnostics.Debug.WriteLine("SI:Display ");

            if (neverdisplayed)
            {
                UpdateViewOnSelection();  // then turn the right ones on
                neverdisplayed = false;
            }

            last_he = he;

            if ( last_he != null )
            {
                SetControlText(he.System.Name);

                HistoryEntry lastfsd = hl.GetLastHistoryEntry(x => x.journalEntry is EliteDangerousCore.JournalEvents.JournalFSDJump, he);

                textBoxSystem.Text = he.System.Name;
                panelFD.BackgroundImage = (lastfsd != null && (lastfsd.journalEntry as EliteDangerousCore.JournalEvents.JournalFSDJump).EDSMFirstDiscover) ? EDDiscovery.Icons.Controls.firstdiscover : EDDiscovery.Icons.Controls.notfirstdiscover;

                discoveryform.history.FillEDSM(he); // Fill in any EDSM info we have

                //textBoxBody.Text = he.WhereAmI + ((he.IsInHyperSpace) ? " (HS)": "");
                textBoxBody.Text = he.WhereAmI + " (" + he.BodyType + ")";

                if (he.System.HasCoordinate)         // cursystem has them?
                {
                    string SingleCoordinateFormat = "0.##";

                    string separ = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " ";

                    textBoxPosition.Text = he.System.X.ToString(SingleCoordinateFormat) + separ + he.System.Y.ToString(SingleCoordinateFormat) + separ + he.System.Z.ToString(SingleCoordinateFormat);

                    ISystem homesys = EDDConfig.Instance.HomeSystem;

                    textBoxHomeDist.Text = he.System.Distance(homesys).ToString(SingleCoordinateFormat);
                    textBoxSolDist.Text = he.System.Distance(0, 0, 0).ToString(SingleCoordinateFormat);
                }
                else
                {
                    textBoxPosition.Text = "?";
                    textBoxHomeDist.Text = "";
                    textBoxSolDist.Text = "";
                }

                int count = discoveryform.history.GetVisitsCount(he.System.Name);
                textBoxVisits.Text = count.ToString();

                bool enableedddross = (he.System.EDDBID > 0);  // Only enable eddb/ross for system that it knows about

                buttonRoss.Enabled = buttonEDDB.Enabled = enableedddross;

                string allegiance, economy, gov, faction, factionstate, security;
                hl.ReturnSystemInfo(he, out allegiance, out economy, out gov, out faction, out factionstate, out security);

                textBoxAllegiance.Text = allegiance;
                textBoxEconomy.Text = economy;
                textBoxGovernment.Text = gov;
                textBoxState.Text = factionstate;

                List<MissionState> mcurrent = (from MissionState ms in he.MissionList.Missions.Values where ms.InProgressDateTime(last_he.EventTimeUTC) orderby ms.Mission.EventTimeUTC descending select ms).ToList();

                if (mcurrent == null || mcurrent.Count == 0)
                    richTextBoxScrollMissions.Text = "No Missions".Tx(this, "NoMissions");
                else
                {
                    string t = "";
                    foreach (MissionState ms in mcurrent)
                    {
                        t = ObjectExtensionsStrings.AppendPrePad(t,
                            JournalFieldNaming.ShortenMissionName(ms.Mission.Name) 
                            + " Exp:" + (EDDiscoveryForm.EDDConfig.DisplayUTC ? ms.Mission.Expiry : ms.Mission.Expiry.ToLocalTime())
                            + " @ " + ms.DestinationSystemStation(),
                            Environment.NewLine);
                    }

                    richTextBoxScrollMissions.Text = t;
                }

                SetNote(he.snc != null ? he.snc.Note : "");
                textBoxGameMode.Text = he.GameModeGroup;
                if (he.isTravelling)
                {
                    textBoxTravelDist.Text = he.TravelledDistance.ToStringInvariant("0.0") + "ly";
                    textBoxTravelTime.Text = he.TravelledSeconds.ToString();
                    textBoxTravelJumps.Text = he.TravelledJumpsAndMisses;
                }
                else
                {
                    textBoxTravelDist.Text = textBoxTravelTime.Text = textBoxTravelJumps.Text = "";
                }

                int cc = (he.ShipInformation) != null ? he.ShipInformation.CargoCapacity() : 0;
                if (cc > 0)
                    textBoxCargo.Text = he.MaterialCommodity.CargoCount.ToStringInvariant() + "/" + cc.ToStringInvariant();
                else
                    textBoxCargo.Text = he.MaterialCommodity.CargoCount.ToStringInvariant();

                textBoxMaterials.Text = he.MaterialCommodity.MaterialsCount.ToStringInvariant();
                textBoxData.Text = he.MaterialCommodity.DataCount.ToStringInvariant();
                textBoxCredits.Text = he.Credits.ToString("N0");

                if (he.ShipInformation != null)
                {
                    textBoxShip.Text = he.ShipInformation.ShipFullInfo(cargo: false, fuel: false);
                    if (he.ShipInformation.FuelCapacity > 0 && he.ShipInformation.FuelLevel > 0)
                        textBoxFuel.Text = he.ShipInformation.FuelLevel.ToStringInvariant("0.#") + "/" + he.ShipInformation.FuelCapacity.ToStringInvariant("0.#");
                    else if (he.ShipInformation.FuelCapacity > 0)
                        textBoxFuel.Text = he.ShipInformation.FuelCapacity.ToStringInvariant("0.#");
                    else
                        textBoxFuel.Text = "N/A".Tx(this,"NA");
                }
                else
                    textBoxShip.Text = textBoxFuel.Text = "";

                RefreshTargetDisplay(this);
            }
            else
            {
                SetControlText("");
                textBoxSystem.Text = textBoxBody.Text = textBoxPosition.Text =
                                textBoxAllegiance.Text = textBoxEconomy.Text = textBoxGovernment.Text =
                                textBoxVisits.Text = textBoxState.Text = textBoxHomeDist.Text = textBoxSolDist.Text =
                                textBoxGameMode.Text = textBoxTravelDist.Text = textBoxTravelTime.Text = textBoxTravelJumps.Text =
                                textBoxCargo.Text = textBoxMaterials.Text = textBoxData.Text = textBoxShip.Text = textBoxFuel.Text =
                                "";

                buttonRoss.Enabled = buttonEDDB.Enabled = false;
                SetNote("");
            }
        }

        private void SetNote(string text)
        {
            noteenabled = false;
            //System.Diagnostics.Debug.WriteLine("SI:Note text " + text);
            richTextBoxNote.Text = text;
            noteenabled = true;
        }

        #endregion

        #region Clicks

        private void buttonEDDB_Click(object sender, EventArgs e)
        {
            if (last_he != null && last_he.System.EDDBID > 0)
                Process.Start("http://eddb.io/system/" + last_he.System.EDDBID.ToString());
        }

        private void buttonRoss_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                discoveryform.history.FillEDSM(last_he);

                if (last_he.System.EDDBID > 0)
                    Process.Start("http://ross.eddb.io/system/update/" + last_he.System.EDDBID.ToString());
            }
        }

        private void buttonEDSM_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                discoveryform.history.FillEDSM(last_he);

                if (last_he.System != null) // solve a possible exception
                {
                    if (!String.IsNullOrEmpty(last_he.System.Name))
                    {
                        long? id_edsm = last_he.System.EDSMID;
                        if (id_edsm <= 0)
                        {
                            id_edsm = null;
                        }

                        EDSMClass edsm = new EDSMClass();
                        string url = edsm.GetUrlToEDSMSystem(last_he.System.Name, id_edsm);

                        if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                            Process.Start(url);
                        else
                            ExtendedControls.MessageBoxTheme.Show(FindForm(), "System unknown to EDSM".Tx(this,"SysUnk"));
                    }
                }
            }
        }

        private void textBoxTarget_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TargetHelpers.setTargetSystem(this,discoveryform, textBoxTarget.Text);
            }
        }

        private void RefreshTargetDisplay(Object sender)              // called when a target has been changed.. via EDDiscoveryform
        {
            string name;
            double x, y, z;

            //System.Diagnostics.Debug.WriteLine("Refresh target display");

            if (TargetClass.GetTargetPosition(out name, out x, out y, out z))
            {
                textBoxTarget.Text = name;
                textBoxTarget.Select(textBoxTarget.Text.Length, textBoxTarget.Text.Length);
                textBoxTargetDist.Text = "No Pos".Tx();

                HistoryEntry cs = discoveryform.history.GetLastWithPosition;
                if (cs != null)
                    textBoxTargetDist.Text = cs.System.Distance(x, y, z).ToString("0.0");

                textBoxTarget.SetTipDynamically(toolTip1, string.Format("Position is {0:0.00},{1:0.00},{2:0.00}".Tx(this, "Pos"), x,y,z));
            }
            else
            {
                textBoxTarget.Text = "?";
                textBoxTargetDist.Text = "";
                textBoxTarget.SetTipDynamically(toolTip1, "On 3D Map right click to make a bookmark, region mark or click on a notemark and then tick on Set Target, or type it here and hit enter".Tx(this,"Target"));
            }
        }

        private void buttonEDSMTarget_Click(object sender, EventArgs e)
        {
            string name;
            long? edsmid = null;
            double x, y, z;

            if (TargetClass.GetTargetPosition(out name, out x, out y, out z))
            {
                ISystem sc = this.discoveryform.history.FindSystem(TargetClass.GetNameWithoutPrefix(name), discoveryform.galacticMapping);

                if (sc != null)
                {
                    name = sc.Name;
                    edsmid = sc.EDSMID;
                }
            }

            EDSMClass edsm = new EDSMClass();
            string url = edsm.GetUrlToEDSMSystem(name, edsmid);

            if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                Process.Start(url);
            else
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System unknown to EDSM".Tx(this,"SysUnk"));

        }

        private void clickTextBox(object sender, EventArgs e)
        {
            SetClipboardText(((Control)sender).Text);
        }

        private void toolStripSystem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelSystem);
        }
        private void toolStripBody_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelBody);
        }
        private void toolStripNotes_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelNotes);
        }
        private void toolStripTarget_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelTarget);
        }
        private void toolStripEDSMButtons_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelEDSMButtonsNextLine);
        }
        private void toolStripEDSM_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelEDSM);
        }
        private void toolStripVisits_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelVisits);
        }
        private void toolStripPosition_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelPosition);
        }
        private void enableDistanceFromToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelDistanceFrom);
        }
        private void toolStripSystemState_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelSystemState);
        }
        private void toolStripGameMode_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelGameMode);
        }
        private void toolStripTravel_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelTravel);
        }
        private void toolStripCargo_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelCargo);
        }
        private void toolStripMaterialCount_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelMats);
        }
        private void toolStripDataCount_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelData);
        }
        private void toolStripCredits_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelCredits);
        }
        private void toolStripShip_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelShipInfo);
        }
        private void toolStripFuel_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelFuel);
        }

        private void toolStripMissionsList_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelMissions);
        }

        private void toolStripRemoveAll_Click(object sender, EventArgs e)
        {
            Selection = (Selection | ((1 << BitSelTotal) - 1)) ^ ((1 << BitSelTotal) - 1);
            UpdateViewOnSelection();
        }

        private void whenTransparentUseSkinnyLookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelSkinny);
            UpdateSkinny();
        }

        void ToggleSelection(Object sender, int bit)
        {
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            if (mi.Enabled)
            {
                Selection ^= (1 << bit);
                UpdateViewOnSelection();
            }
        }

        void UpdateViewOnSelection()
        {
            SuspendLayout();
            int ver = 3;

            foreach (Control c in this.Controls)
                c.Visible = false;

            int textboxh = EDDTheme.Instance.FontSize > 10 ? 24 : 20;
            int vspacing = textboxh+4;

            //System.Diagnostics.Debug.WriteLine("Selection is " + sel);

            bool selEDSMonNextLine = (Selection & (1 << BitSelEDSMButtonsNextLine)) != 0;
            toolStripEDSMDownLine.Checked = selEDSMonNextLine;
            toolStripSkinny.Checked = (Selection & (1 << BitSelSkinny)) != 0;

            int data1offset = textBoxCredits.Left - labelCredits.Left;      // offset between first item to initial label - basing it on actual pos allow the auto font scale to work
            int lab2offset = textBoxCredits.Right + 4 - labelCredits.Left;  // offset between second label and initial label
            int data2offset = lab2offset + data1offset;                     // offset between data 2 pos and initial label
            int coloffset = lab2offset;                                     // offset between each column

            //System.Diagnostics.Debug.WriteLine("Sys Info first data {0} second lab {1} second data {2} col offset {3}", data1offset, lab2offset, data2offset, coloffset);

            int maxvert = 0;

            for( int r = 0; r < Lines.Count; r++ )
            {
                Lines[r].YStart = -1;

                for ( int c = 0; c < HorzPositions; c++ )
                { 
                    Point labpos = new Point(3 + c * coloffset, ver);
                    Point datapos = new Point(labpos.X + data1offset, labpos.Y);
                    Point labpos2 = new Point(labpos.X + lab2offset, labpos.Y);
                    Point datapos2 = new Point(labpos.X + data2offset, labpos.Y);

                   // System.Diagnostics.Debug.WriteLine("R{0}C{1} {2} {3} ( {4} {5} )", r,c, labpos, datapos, labpos2, datapos2);

                    int bitno = Lines[r].Items[c]-1;    // stored +1

                    if (bitno >= 0)
                    {
                        bool ison = (Selection & (1 << bitno)) != 0;

                        toolstriplist[bitno].Enabled = false;
                        toolstriplist[bitno].Checked = ison;
                        toolstriplist[bitno].Enabled = true;
                       // System.Diagnostics.Debug.WriteLine("Toolstrip " + bitno + " set to " + ison);

                        if (ison)
                        {
                            Lines[r].YStart = ver;
                            int si = r * HorzPositions + c;

                            switch (bitno)
                            {
                                case BitSelSystem:
                                    this.SetPos(ref labpos, labelSysName, datapos, textBoxSystem, vspacing, si);
                                    panelFD.Location = new Point(textBoxSystem.Right, textBoxSystem.Top);
                                    panelFD.Visible = true;

                                    if (!selEDSMonNextLine && (Selection & (1 << BitSelEDSM)) != 0)
                                    {
                                        buttonEDSM.Location = new Point(textBoxSystem.Right + hspacing, datapos.Y);
                                        buttonEDDB.Location = new Point(buttonEDSM.Right + hspacing, buttonEDSM.Top);
                                        buttonRoss.Location = new Point(buttonEDDB.Right + hspacing, buttonEDSM.Top);
                                        buttonEDSM.Visible = buttonEDDB.Visible = buttonRoss.Visible = true;
                                        buttonEDSM.Tag = buttonEDDB.Tag = buttonRoss.Tag = si;
                                    }

                                    break;

                                case BitSelEDSM:
                                    if (selEDSMonNextLine)
                                    {
                                        labelOpen.Location = labpos;
                                        buttonEDSM.Location = new Point(datapos.X, datapos.Y);
                                        buttonEDDB.Location = new Point(buttonEDSM.Right + hspacing, buttonEDSM.Top);
                                        buttonRoss.Location = new Point(buttonEDDB.Right + hspacing, buttonEDSM.Top);
                                        labelOpen.Tag = buttonEDSM.Tag = buttonEDDB.Tag = buttonRoss.Tag = si;
                                        labelOpen.Visible = buttonEDSM.Visible = buttonEDDB.Visible = buttonRoss.Visible = true;
                                        labpos.Y += vspacing + 4;
                                    }
                                    break;

                                case BitSelVisits:
                                    this.SetPos(ref labpos, labelVisits, datapos, textBoxVisits, vspacing, si);
                                    break;

                                case BitSelBody:
                                    this.SetPos(ref labpos, labelBodyName, datapos, textBoxBody, vspacing, si);
                                    break;

                                case BitSelPosition:
                                    this.SetPos(ref labpos, labelPosition, datapos, textBoxPosition, vspacing, si);
                                    break;

                                case BitSelDistanceFrom:
                                    this.SetPos(ref labpos, labelHomeDist, datapos, textBoxHomeDist, vspacing, si);
                                    OffsetPos(labpos2, labelSolDist, datapos2, textBoxSolDist, si);
                                    break;

                                case BitSelSystemState:
                                    this.SetPos(ref labpos, labelState, datapos, textBoxState, vspacing - 4, si);
                                    OffsetPos(labpos2, labelAllegiance, datapos2, textBoxAllegiance, si);
                                    datapos.Y = labpos2.Y = datapos2.Y = labpos.Y;
                                    this.SetPos(ref labpos, labelGov, datapos, textBoxGovernment, vspacing, si);
                                    OffsetPos(labpos2, labelEconomy, datapos2, textBoxEconomy, si);
                                    break;

                                case BitSelNotes:
                                    SetPos(ref labpos, labelNote, datapos, richTextBoxNote, richTextBoxNote.Height + 8, si);
                                    break;

                                case BitSelTarget:
                                    this.SetPos(ref labpos, labelTarget, datapos, textBoxTarget, vspacing, si);
                                    textBoxTargetDist.Location = new Point(textBoxTarget.Right + hspacing, datapos.Y);
                                    buttonEDSMTarget.Location = new Point(textBoxTargetDist.Right + hspacing, datapos.Y);
                                    textBoxTargetDist.Tag = buttonEDSMTarget.Tag = si;
                                    textBoxTargetDist.Visible = buttonEDSMTarget.Visible = true;
                                    break;

                                case BitSelGameMode:
                                    this.SetPos(ref labpos, labelGamemode, datapos, textBoxGameMode, vspacing, si);
                                    break;

                                case BitSelTravel:
                                    this.SetPos(ref labpos, labelTravel, datapos, textBoxTravelDist, vspacing, si);
                                    textBoxTravelTime.Location = new Point(textBoxTravelDist.Right + hspacing, datapos.Y);
                                    textBoxTravelJumps.Location = new Point(textBoxTravelTime.Right + hspacing, datapos.Y);
                                    textBoxTravelTime.Tag = textBoxTravelJumps.Tag = si;
                                    textBoxTravelTime.Visible = textBoxTravelJumps.Visible = true;
                                    // don't set visible for the last two, may not be if not travelling. Display will deal with it
                                    break;

                                case BitSelCargo:
                                    this.SetPos(ref labpos, labelCargo, datapos, textBoxCargo, vspacing, si);
                                    break;

                                case BitSelMats:
                                    this.SetPos(ref labpos, labelMaterials, datapos, textBoxMaterials, vspacing, si);
                                    break;

                                case BitSelData:
                                    this.SetPos(ref labpos, labelData, datapos, textBoxData, vspacing, si);
                                    break;

                                case BitSelShipInfo:
                                    this.SetPos(ref labpos, labelShip, datapos, textBoxShip, vspacing, si);
                                    break;

                                case BitSelFuel:
                                    this.SetPos(ref labpos, labelFuel, datapos, textBoxFuel, vspacing, si);
                                    break;

                                case BitSelCredits:
                                    this.SetPos(ref labpos, labelCredits, datapos, textBoxCredits, vspacing, si);
                                    break;

                                case BitSelMissions:
                                    this.SetPos(ref labpos, labelMissions, datapos, richTextBoxScrollMissions, richTextBoxScrollMissions.Height + 8, si);
                                    break;

                                default:
                                    System.Diagnostics.Debug.WriteLine("Ignoring unknown type");
                                    break;
                            }

                            Lines[r].YEnd = labpos.Y - 1;
                            maxvert = Math.Max(labpos.Y, maxvert);        // update vertical
                        }
                    }
                }

                ver = maxvert;
            }

            ResumeLayout();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
            UpdateViewOnSelection();
        }

        public void Reset()
        {
            Selection = BitSelDefault;
            Lines = new List<BaseUtils.LineStore>();
            for (int i = 0; i < BitSelTotal; i++)
                Lines.Add(new BaseUtils.LineStore() { Items = new int[HorzPositions] { i + 1, 0, 0, 0, 0, 0, 0, 0 } });

            Lines[BitSelFuel].Items[1] = BitSelCargo + 1;
            Lines[BitSelCargo].Items[0] = 0;
            Lines[BitSelMats].Items[1] = BitSelData + 1;
            Lines[BitSelData].Items[0] = 0;
            Lines[BitSelVisits].Items[1] = BitSelCredits + 1;
            Lines[BitSelCredits].Items[0] = 0;

            BaseUtils.LineStore.CompressOrder(Lines);
            BaseUtils.LineStore.DumpOrder(Lines, "Reset");
        }

        void SetPos(ref Point lp, Label lab, Point tp, ExtendedControls.TextBoxBorder box, int vspacing , int i )
        {
            lab.Location = lp;
            box.Location = tp;
            box.Tag = lab.Tag = i;
            lab.Visible = box.Visible = true;
            lp.Y += vspacing;
        }

        void SetPos(ref Point lp, Label lab, Point tp, ExtendedControls.RichTextBoxScroll box, int vspacing , int i)
        {
            lab.Location = lp;
            box.Location = tp;
            box.Tag = lab.Tag = i;
            lab.Visible = box.Visible = true;
            lp.Y += vspacing;
        }

        void OffsetPos(Point lp, Label lab, Point tp, ExtendedControls.TextBoxBorder box , int i)
        {
            lab.Location = lp;
            box.Location = tp;
            box.Tag = lab.Tag = i;
            lab.Visible = box.Visible = true;
        }

        #endregion

        #region Notes

        bool noteenabled = true;
        private void richTextBoxNote_Leave(object sender, EventArgs e)
        {
            if (last_he != null && noteenabled)
            {
                //System.Diagnostics.Debug.WriteLine("SI:leave note changed: " + richTextBoxNote.Text);
                last_he.SetJournalSystemNoteText(richTextBoxNote.Text.Trim(), true , EDCommander.Current.SyncToEdsm);   // commit, maybe send to edsm
                discoveryform.NoteChanged(this, last_he, true);
            }
        }

        private void richTextBoxNote_TextBoxChanged(object sender, EventArgs e)
        {
            if (last_he != null && noteenabled)
            {
                //System.Diagnostics.Debug.WriteLine("SI:type note changed: " + richTextBoxNote.Text);
                last_he.SetJournalSystemNoteText(richTextBoxNote.Text.Trim(), false, false);        // no commit, no send to edsm..
                discoveryform.NoteChanged(this, last_he, false);
            }
        }

        private void OnNoteChanged(Object sender, HistoryEntry he, bool arg)  // BEWARE we do this as well..
        {
            if ( !Object.ReferenceEquals(this,sender) )     // so, make sure this sys info is not sending it
            {
                //System.Diagnostics.Debug.WriteLine("SI:On note changed: " + he.snc.Note);
                SetNote(he.snc != null ? he.snc.Note : "");
            }
        }

        public void FocusOnNote( int asciikeycode )     // called if a focus is wanted
        {
            if (IsNotesShowing)
            {
                //System.Diagnostics.Debug.WriteLine("SI:Focus on Note due to key");

                richTextBoxNote.Select(richTextBoxNote.Text.Length, 0);     // move caret to end and focus.
                richTextBoxNote.ScrollToCaret();
                richTextBoxNote.Focus();

                string s = null;
                if (asciikeycode == 8)      // strange old sendkeys
                    s = "{BACKSPACE}";
                else if (asciikeycode == '+' || asciikeycode == '^' || asciikeycode == '%' || asciikeycode == '(' || asciikeycode == ')' || asciikeycode == '~')
                    s = "{" + (new string((char)asciikeycode, 1)) + "}";
                else if ( asciikeycode >= 32 && asciikeycode <= 126 )
                    s = new string((char)asciikeycode, 1);

                //System.Diagnostics.Debug.WriteLine("Send " + s);
                if (s != null)
                    SendKeys.Send(s);
            }
        }

        #endregion

        #region Move around

        int fromorder = -1;
        int fromy = -1;
        bool inmovedrag = false;

        private void controlMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Control.ModifierKeys.HasFlag(Keys.Control))
            {
                Control c = sender as Control;
                fromorder = (int)c.Tag;
                fromy = c.Top;
                inmovedrag = false;
            }
           // System.Diagnostics.Debug.WriteLine("Control " + inmove.Name + " grabbed");
        }

        private void controlMouseUp(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;

            if (fromorder != -1 )
            {
                if (inmovedrag)
                {
                    int xpos = this.PointToClient(Cursor.Position).X;

                    int fromrow = fromorder / HorzPositions;
                    int fromcol = fromorder % HorzPositions;

                    int col2pos = textBoxCredits.Right + 4 - labelCredits.Left;

                    int movetoy = fromy + e.Y;
                    int torow = BaseUtils.LineStore.FindRow(Lines,movetoy);       // may be -1 if can't find

                    if (torow == -1)    // if at end
                    {
                        torow = Lines.Count;        // fresh row here
                        Lines.Add(new BaseUtils.LineStore() { Items = new int[HorzPositions] });
                    }

                    int tocol = Math.Min(xpos / col2pos, HorzPositions - 1);

                    System.Diagnostics.Debug.WriteLine("Move " + fromrow +":"+ fromcol+" -> " + torow +":" + tocol);

                    bool oneleftisnotshort = tocol > 0 && Lines[torow].Items[tocol - 1] > 0 && Array.IndexOf(SmallItems, Lines[torow].Items[tocol - 1] - 1) == -1;
                    bool onerightisnotblank = tocol < HorzPositions - 1 &&    // not last
                                            Array.IndexOf(SmallItems, Lines[fromrow].Items[fromcol]) == -1 && // one moving in is two columns
                                            Lines[torow].Items[tocol + 1] > 0;      // and occupied

                    if (Lines[torow].Items[tocol] > 0 || oneleftisnotshort || onerightisnotblank )      // occupied
                    {
                        Lines.Insert(torow, new BaseUtils.LineStore() { Items = new int[HorzPositions] });    // fresh row in here

                        if (fromrow > torow)            // adjust from down if torow is in front of it
                            fromrow++;
                    }

                    Lines[torow].Items[tocol] = Lines[fromrow].Items[fromcol];
                    Lines[fromrow].Items[fromcol] = -1;

                    BaseUtils.LineStore.CompressOrder(Lines);
                    BaseUtils.LineStore.DumpOrder(Lines, "Move");

                    UpdateViewOnSelection();
                    Cursor.Current = Cursors.Default;
                }

                fromorder = -1;
            }
        }

        private void controlMouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;

            if (fromorder != -1 )
            {
                if (e.Y < -4 || e.Y > c.Height + 4 || e.X < -100 || e.X > 100)
                {
                    if (!inmovedrag)
                    {
                        inmovedrag = true;
                        Cursor.Current = Cursors.Hand;
                    }
                }
                else if ( inmovedrag )
                {
                    inmovedrag = false;
                    Cursor.Current = Cursors.Default;
                }

                //System.Diagnostics.Debug.WriteLine("Control " + inmove.Name + " Drag " + e.X + "," + e.Y);
            }
        }

        #endregion

        #region Display control

        private void UserControlSysInfo_Resize(object sender, EventArgs e)
        {
            //  later we may resize to width if other column is not used, but not now
        }

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            this.BackColor = curcol;
            UpdateSkinny();
        }

        void UpdateSkinny()
        { 
            if (IsTransparent && (Selection & (1<<BitSelSkinny))!=0)
            {
                foreach (Control c in Controls)
                {
                    if (c is ExtendedControls.TextBoxBorder)
                    {
                        ExtendedControls.TextBoxBorder b = c as ExtendedControls.TextBoxBorder;
                        b.ControlBackground = Color.Red;
                        b.BorderStyle = BorderStyle.None;
                        b.BorderColor = Color.Transparent;
                    }
                }
            }
            else
                EDDTheme.Instance.ApplyToControls(this);

        }

        #endregion

    }
}
