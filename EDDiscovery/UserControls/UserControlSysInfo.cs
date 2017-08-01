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
    public partial class UserControlSysInfo: UserControlCommonBase
    {
        public bool IsNotesShowing { get { return richTextBoxNote.Visible; } }

        private EDDiscoveryForm discoveryform;
        private UserControlTravelGrid uctg;

        private int displaynumber;
        private string DbSelection { get { return ("SystemInformation") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "Sel"; } }
        private string DbOSave { get { return "SystemInformation" + ((displaynumber > 0) ? displaynumber.ToString() :"" ) + "Order"; } }

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
        const int BitSelCargo = 10;
        const int BitSelGameMode = 11;
        const int BitSelTravel = 12;

        int[] LongItems = new int[] { BitSelTarget, BitSelShipInfo, BitSelCargo, BitSelTravel };

        const int BitSelTotal = 13;
        const int Positions = BitSelTotal * 2;      // two columns of positions, one at 0, one at +300 pixels ish, 
        const int BitSelEDSMButtonsNextLine = 24;
        const int BitSelSkinny = 25;
        const int BitSelDefault = ((1<<BitSelTotal)-1)+(1<<BitSelEDSMButtonsNextLine);

        const int hspacing = 8;

        int Selection;          // selection bits
        List<int> Order;        // orderarray is the order to display them on the screen.. can be any length
        int[] YStart;           // ypos of items in Order.
        int[] YEnd;             // ypos of items in Order.
        ToolStripMenuItem[] toolstriplist;          // ref to toolstrip items for each bit above. in same order as bits BitSel..

        #region Init

        public UserControlSysInfo()
        {
            InitializeComponent();
            Name = "System Information";
            textBoxTarget.SetToolTip(toolTip1, "Sets the target");
            textBoxTargetDist.SetToolTip(toolTip1, "Distance to target");

        }

        public override void Init(EDDiscoveryForm ed, UserControlTravelGrid thc, int displayno)
        {
            discoveryform = ed;
            uctg = thc;
            this.displaynumber = displayno;
            uctg.OnTravelSelectionChanged += Display;    // get this whenever current selection or refreshed..
            discoveryform.OnNewTarget += RefreshTargetDisplay;
            discoveryform.OnNoteChanged += OnNoteChanged;
            textBoxTarget.SetAutoCompletor(SystemClassDB.ReturnSystemListForAutoComplete);

            // same order as Sel bits are defined in, one bit per selection item.
            toolstriplist = new ToolStripMenuItem[] { toolStripSystem , toolStripEDSM , toolStripVisits, toolStripBody,
                                                        toolStripPosition, toolStripDistanceFrom,
                                                        toolStripSystemState, toolStripNotes, toolStripTarget,
                                                        toolStripShip, toolStripCargo,
                                                        toolStripGameMode,toolStripTravel};

            Selection = SQLiteDBClass.GetSettingInt(DbSelection, BitSelDefault);
            Order = SQLiteDBClass.GetSettingString(DbOSave, "").RestoreIntListFromString(-1, 0);     // no min len
            if (Order.Count < 2)
                Reset();

            System.Diagnostics.Debug.WriteLine("Ordered " + String.Join(",", Order));
        }

        public virtual void ChangeTravelGrid(UserControlTravelGrid thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewTarget -= RefreshTargetDisplay;
            discoveryform.OnNoteChanged -= OnNoteChanged;
            SQLiteDBClass.PutSettingString(DbOSave, String.Join(",", Order));
            SQLiteDBClass.PutSettingInt(DbSelection, Selection);
        }

        #endregion

        #region Display

        bool neverdisplayed = true;
        HistoryEntry last_he = null;
        public override void Display(HistoryEntry he, HistoryList hl)
        {
            if (neverdisplayed)
            {
                UpdateViewOnSelection();  // then turn the right ones on
                neverdisplayed = false;
            }

            last_he = he;

            if ( last_he != null )
            {
                SetControlText(he.System.name);
                textBoxSystem.Text = he.System.name;
                discoveryform.history.FillEDSM(he, reload: true); // Fill in any EDSM info we have, force it to try again.. in case system db updated

                textBoxBody.Text = he.WhereAmI;

                if (he.System.HasCoordinate)         // cursystem has them?
                {
                    string SingleCoordinateFormat = "0.##";

                    textBoxPosition.Text = he.System.x.ToString(SingleCoordinateFormat) + "," + he.System.y.ToString(SingleCoordinateFormat) + "," + he.System.z.ToString(SingleCoordinateFormat);

                    ISystem homesys = discoveryform.GetHomeSystem();

                    textBoxHomeDist.Text = SystemClassDB.Distance(he.System, homesys).ToString(SingleCoordinateFormat);
                    textBoxSolDist.Text = SystemClassDB.Distance(he.System, 0, 0, 0).ToString(SingleCoordinateFormat);
                }
                else
                {
                    textBoxPosition.Text = "?";
                    textBoxHomeDist.Text = "";
                    textBoxSolDist.Text = "";
                }

                int count = discoveryform.history.GetVisitsCount(he.System.name);
                textBoxVisits.Text = count.ToString();

                bool enableedddross = (he.System.id_eddb > 0);  // Only enable eddb/ross for system that it knows about

                buttonRoss.Enabled = buttonEDDB.Enabled = enableedddross;

                textBoxAllegiance.Text = he.System.allegiance.ToNullUnknownString();
                textBoxEconomy.Text = he.System.primary_economy.ToNullUnknownString();
                textBoxGovernment.Text = he.System.government.ToNullUnknownString();
                textBoxState.Text = he.System.state.ToNullUnknownString();
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

                if (he.ShipInformation != null)
                {
                    textBoxShip.Text = he.ShipInformation.ShipFullInfo(cargo: false, fuel: false);
                    if (he.ShipInformation.FuelCapacity > 0 && he.ShipInformation.FuelLevel > 0)
                        textBoxFuel.Text = he.ShipInformation.FuelLevel.ToStringInvariant("0.#") + "/" + he.ShipInformation.FuelCapacity.ToStringInvariant("0.#");
                    else if (he.ShipInformation.FuelCapacity > 0)
                        textBoxFuel.Text = he.ShipInformation.FuelCapacity.ToStringInvariant("0.#");
                    else
                        textBoxFuel.Text = "N/A";
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
            richTextBoxNote.Text = text;
            noteenabled = true;
        }

        #endregion

        #region Clicks

        private void buttonEDDB_Click(object sender, EventArgs e)
        {
            if (last_he != null && last_he.System.id_eddb > 0)
                Process.Start("http://eddb.io/system/" + last_he.System.id_eddb.ToString());
        }

        private void buttonRoss_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                discoveryform.history.FillEDSM(last_he, reload: true);

                if (last_he.System.id_eddb > 0)
                    Process.Start("http://ross.eddb.io/system/update/" + last_he.System.id_eddb.ToString());
            }
        }

        private void buttonEDSM_Click(object sender, EventArgs e)
        {
            if (last_he != null)
            {
                discoveryform.history.FillEDSM(last_he, reload: true);

                if (last_he.System != null) // solve a possible exception
                {
                    if (!String.IsNullOrEmpty(last_he.System.name))
                    {
                        long? id_edsm = last_he.System.id_edsm;
                        if (id_edsm <= 0)
                        {
                            id_edsm = null;
                        }

                        EDSMClass edsm = new EDSMClass();
                        string url = edsm.GetUrlToEDSMSystem(last_he.System.name, id_edsm);

                        if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                            Process.Start(url);
                        else
                            ExtendedControls.MessageBoxTheme.Show("System unknown to EDSM");
                    }
                }
            }
        }

        private void textBoxTarget_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DB.TargetHelpers.setTargetSystem(this,discoveryform, textBoxTarget.Text);
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
                textBoxTargetDist.Text = "No Pos";

                HistoryEntry cs = discoveryform.history.GetLastWithPosition;
                if (cs != null)
                    textBoxTargetDist.Text = SystemClassDB.Distance(cs.System, x, y, z).ToString("0.0");

                textBoxTarget.SetToolTip(toolTip1, "Position is " + x.ToString("0.00") + "," + y.ToString("0.00") + "," + z.ToString("0.00"));
            }
            else
            {
                textBoxTarget.Text = "?";
                textBoxTargetDist.Text = "";
                textBoxTarget.SetToolTip(toolTip1, "On 3D Map right click to make a bookmark, region mark or click on a notemark and then tick on Set Target, or type it here and hit enter");
            }
        }

        private void buttonEDSMTarget_Click(object sender, EventArgs e)
        {
            EDSMClass edsm = new EDSMClass();
            string url = edsm.GetUrlToEDSMSystem(textBoxTarget.Text, null);

            if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                Process.Start(url);
            else
                ExtendedControls.MessageBoxTheme.Show("System unknown to EDSM");

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
        private void toolStripShip_Click(object sender, EventArgs e)
        {
            ToggleSelection(sender, BitSelShipInfo);
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

            YStart = new int[Order.Count];        //-1 not used, else Y coord
            YEnd = new int[Order.Count];

            bool selEDSMonNextLine = (Selection & (1 << BitSelEDSMButtonsNextLine)) != 0;
            toolStripEDSMDownLine.Checked = selEDSMonNextLine;
            toolStripSkinny.Checked = (Selection & (1 << BitSelSkinny)) != 0;

            int data1pos = textBoxSystem.Left - labelSysName.Left;      // basing it on actual pos allow the auto font scale to work
            int lab2pos = labelSolDist.Left - labelHomeDist.Left;
            int data2pos = textBoxSolDist.Left - labelHomeDist.Left;
            int lab3pos = labelFuel.Left - labelShip.Left;
            int data3pos = textBoxFuel.Left - labelShip.Left;

            int maxvert = 0;

            for (int i = 0; i < Order.Count; i++)           // for each position possible (Order may not be as filled as Positions
            {
                int itemno = Order[i];     // this is the item number, which is a bit position

                YStart[i] = -1; // not occupied yet

                if (itemno >= 0)
                {
                    bool ison = (Selection & (1 << itemno)) != 0;

                    toolstriplist[itemno].Enabled = false;
                    toolstriplist[itemno].Checked = ison;
                    toolstriplist[itemno].Enabled = true;

                    if (ison)
                    {
                        Point labpos = new Point(3 + (i % 2) * 300, ver);
                        Point datapos = new Point(labpos.X + data1pos, labpos.Y);
                        Point labpos2 = new Point(labpos.X + lab2pos, labpos.Y);
                        Point datapos2 = new Point(labpos.X + data2pos, labpos.Y);
                        Point labpos3 = new Point(labpos.X + lab3pos, labpos.Y);
                        Point datapos3 = new Point(labpos.X + data3pos, labpos.Y);
                        YStart[i] = ver;

                        switch (itemno)
                        {
                            case BitSelSystem:
                                this.SetPos(ref labpos, labelSysName, datapos, textBoxSystem, vspacing , i);

                                if (!selEDSMonNextLine && (Selection & (1 << BitSelEDSM)) != 0)
                                {
                                    buttonEDSM.Location = new Point(textBoxSystem.Right + hspacing, datapos.Y);
                                    buttonEDDB.Location = new Point(buttonEDSM.Right + hspacing, buttonEDSM.Top);
                                    buttonRoss.Location = new Point(buttonEDDB.Right + hspacing, buttonEDSM.Top);
                                    buttonEDSM.Visible = buttonEDDB.Visible = buttonRoss.Visible = true;
                                    buttonEDSM.Tag = buttonEDDB.Tag = buttonRoss.Tag = i;
                                }

                                break;

                            case BitSelEDSM:
                                if (selEDSMonNextLine)
                                {
                                    labelOpen.Location = labpos;
                                    buttonEDSM.Location = new Point(datapos.X, datapos.Y);
                                    buttonEDDB.Location = new Point(buttonEDSM.Right + hspacing, buttonEDSM.Top);
                                    buttonRoss.Location = new Point(buttonEDDB.Right + hspacing, buttonEDSM.Top);
                                    labelOpen.Tag = buttonEDSM.Tag = buttonEDDB.Tag = buttonRoss.Tag = i;
                                    labelOpen.Visible = buttonEDSM.Visible = buttonEDDB.Visible = buttonRoss.Visible = true;
                                    labpos.Y += vspacing + 4;
                                }
                                break;

                            case BitSelVisits:
                                this.SetPos(ref labpos, labelVisits, datapos, textBoxVisits, vspacing,i);
                                break;

                            case BitSelBody:
                                this.SetPos(ref labpos, labelBodyName, datapos, textBoxBody, vspacing,i);
                                break;

                            case BitSelPosition:
                                this.SetPos(ref labpos, labelPosition, datapos, textBoxPosition, vspacing,i);
                                break;

                            case BitSelDistanceFrom:
                                this.SetPos(ref labpos, labelHomeDist, datapos, textBoxHomeDist, vspacing,i);
                                OffsetPos(labpos2, labelSolDist, datapos2, textBoxSolDist,i);
                                break;

                            case BitSelSystemState:
                                this.SetPos(ref labpos, labelState, datapos, textBoxState, vspacing - 4, i);
                                OffsetPos(labpos2, labelAllegiance, datapos2, textBoxAllegiance, i);
                                datapos.Y = labpos2.Y = datapos2.Y = labpos.Y;
                                this.SetPos(ref labpos, labelGov, datapos, textBoxGovernment, vspacing, i);
                                OffsetPos(labpos2, labelEconomy, datapos2, textBoxEconomy, i);
                                break;

                            case BitSelNotes:
                                SetPos(ref labpos, labelNote, datapos, richTextBoxNote, richTextBoxNote.Height + 8, i);
                                break;

                            case BitSelTarget:
                                this.SetPos(ref labpos, labelTarget, datapos, textBoxTarget, vspacing, i);
                                textBoxTargetDist.Location = new Point(textBoxTarget.Right + hspacing, datapos.Y);
                                buttonEDSMTarget.Location = new Point(textBoxTargetDist.Right + hspacing, datapos.Y);
                                textBoxTargetDist.Tag = buttonEDSMTarget.Tag = i;
                                textBoxTargetDist.Visible = buttonEDSMTarget.Visible = true;
                                break;

                            case BitSelGameMode:
                                this.SetPos(ref labpos, labelGamemode, datapos, textBoxGameMode, vspacing,i);
                                break;

                            case BitSelTravel:
                                this.SetPos(ref labpos, labelTravel, datapos, textBoxTravelDist, vspacing, i);
                                textBoxTravelTime.Location = new Point(textBoxTravelDist.Right + hspacing, datapos.Y);
                                textBoxTravelJumps.Location = new Point(textBoxTravelTime.Right + hspacing, datapos.Y);
                                textBoxTravelTime.Tag = textBoxTravelJumps.Tag = i;
                                textBoxTravelTime.Visible = textBoxTravelJumps.Visible = true;
                                // don't set visible for the last two, may not be if not travelling. Display will deal with it
                                break;

                            case BitSelCargo:
                                this.SetPos(ref labpos, labelCargo, datapos, textBoxCargo, vspacing,i);
                                OffsetPos(labpos2, labelMaterials, datapos2, textBoxMaterials,i);
                                OffsetPos(labpos3, labelData, datapos3, textBoxData,i);
                                break;

                            case BitSelShipInfo:
                                this.SetPos(ref labpos, labelShip, datapos, textBoxShip, vspacing,i);
                                OffsetPos(labpos3, labelFuel, datapos3, textBoxFuel,i);
                                break;

                            default:
                                System.Diagnostics.Debug.WriteLine("Ignoring unknown type");
                                break;
                        }

                        YEnd[i] = labpos.Y - 1;
                        maxvert = Math.Max(labpos.Y, maxvert);        // update vertical

                        //System.Diagnostics.Debug.WriteLine("Sel " + i + " " + Order[i] + " on " + ison + " ypos " + YStart[i] +"-" + YEnd[i]);
                    }
                }

                if ((i % 2) != 0)
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
            Order = new List<int>();
            for (int i = 0; i < BitSelTotal*2; i++)          // reset
                Order.Add(((i%2)==0)?(i/2) : -1);       // fill with 0,-1,1,-3,2,-5 etc aligning them all down the left.

            System.Diagnostics.Debug.WriteLine("Reset " + String.Join(",", Order));
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
                last_he.SetJournalSystemNoteText(richTextBoxNote.Text.Trim(), true);
                discoveryform.NoteChanged(this, last_he, true);
            }
        }

        private void richTextBoxNote_TextBoxChanged(object sender, EventArgs e)
        {
            if (last_he != null && noteenabled)
            {
                last_he.SetJournalSystemNoteText(richTextBoxNote.Text.Trim(), false);
                discoveryform.NoteChanged(this, last_he, false);
            }
        }
        private void OnNoteChanged(Object sender, HistoryEntry he, bool arg)  // BEWARE we do this as well..
        {
            if ( !Object.ReferenceEquals(this,sender) )     // so, make sure this sys info is not sending it
            {
                SetNote(he.snc != null ? he.snc.Note : "");
            }
        }

        public void FocusOnNote( int asciikeycode )     // called if a focus is wanted
        {
            if (IsNotesShowing)
            {
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
                    int movetoy = fromy + e.Y;
                    int xpos = this.PointToClient(Cursor.Position).X;
                    int movetoorder = FindOrder(movetoy) & ~1;          // ignore the 0/1 bit, just the line we want
                    if (xpos > 300 )
                        movetoorder++;
                    bool right = (movetoorder % 2) != 0;

                    if ( movetoorder>=Order.Count)  // if beyond order, insert a new pair
                    {
                        Order.Add(-1);
                        Order.Add(-1);
                        System.Diagnostics.Debug.Assert(movetoorder < Order.Count);
                    }

                    System.Diagnostics.Debug.WriteLine("--");
                    System.Diagnostics.Debug.WriteLine("Move " + fromorder + "(" + Order[fromorder] + ") to " + movetoorder + "(" + Order[movetoorder] + ") Released from Y " + fromy + " to " + xpos + "," + movetoy);
                    
                    if (Order[movetoorder] >= 0 ||      // occupied
                            (!right && Array.IndexOf(LongItems, Order[fromorder]) != -1) || // item is on left and is long
                            (right && Array.IndexOf(LongItems, Order[movetoorder&~1]) != -1 && (movetoorder&~1) != fromorder )   // item on right, but current left item is too long, and we are not moving it!
                            )
                    {
                        int line = (movetoorder / 2) * 2;       // shove two in from of move
                        Order.Insert(line, -1);
                        Order.Insert(line, -1);
                        if ( line<fromorder)            // adjust from down if line is in front of it
                            fromorder += 2;
                        System.Diagnostics.Debug.WriteLine("Insert, " + fromorder + "->" + movetoorder + " " + String.Join(",", Order));
                    }

                    Order[movetoorder] = Order[fromorder];     // now free, insert here
                    Order[fromorder] = -1;      // clear old

                    System.Diagnostics.Debug.WriteLine("Before removal of empty lines " + String.Join(",", Order));

                    for (int i = 0; i < Order.Count; i += 2)        // now clean out empty rows
                    {   
                        if (Order[i] < 0 && Order[i + 1] < 0)  // if line empty..
                        {
                            Order.RemoveRange(i, 2);
                            System.Diagnostics.Debug.WriteLine("Line " + i + " removed, now " + String.Join(",", Order));
                        }
                    }

                    System.Diagnostics.Debug.WriteLine("Now " + String.Join(",", Order));
                    UpdateViewOnSelection();
                    Cursor.Current = Cursors.Default;
                }

                fromorder = -1;
            }
        }

        private int FindOrder(int y)
        {
            int lastfilled = -1;

            for(int i = 0 ; i < Order.Count; i++ )
            {
                if (YStart[i] != -1)
                {
                    if (lastfilled == -1 && y < YStart[i])
                        return 0;

                    if (y >= YStart[i] && y <= YEnd[i])
                        return i;

                    lastfilled = i;
                }
            }

            return (lastfilled / 2) * 2 + 2;
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
            //TBD

            //richTextBoxNote.Size = new Size(ClientRectangle.Width - richTextBoxNote.Left - 8, richTextBoxNote.Height);

            int left = ClientRectangle.Width - textBoxTarget.Left - buttonEDSMTarget.Width - hspacing * 2 - 2;
            int targetw = left * 7 / 10;
            int distw = left - targetw;

            //textBoxTarget.Width = targetw;
            //textBoxTargetDist.Width = distw;
            //textBoxTargetDist.Location = new Point(textBoxTarget.Right + hspacing, textBoxTargetDist.Top);
            //buttonEDSMTarget.Location = new Point(textBoxTargetDist.Right + hspacing, buttonEDSMTarget.Top);

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
