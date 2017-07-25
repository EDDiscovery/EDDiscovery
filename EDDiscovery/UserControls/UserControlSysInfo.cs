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
using EDDiscovery.EDSM;
using System.Diagnostics;

namespace EDDiscovery.UserControls
{
    public partial class UserControlSysInfo: UserControlCommonBase
    {
        public bool IsNotesShowing { get { return richTextBoxNote.Visible; } }

        private EDDiscoveryForm discoveryform;
        private int displaynumber;
        private string DbSelection { get { return ("SystemInformation") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "Sel"; } }
        private string DbOSave { get { return "SystemInformation" + ((displaynumber > 0) ? displaynumber.ToString() : "Order"); } }

        const int BitSelSystem = 0;
        const int BitSelEDSM = 1;
        const int BitSelVisits = 2;
        const int BitSelBody = 3;
        const int BitSelPosition = 4;
        const int BitSelDistanceFrom = 5;
        const int BitSelSystemState = 6;
        const int BitSelNotes = 7;
        const int BitSelTarget = 8;
        const int BitSelGameMode = 9;
        const int BitSelTravel = 10;
        const int BitSelCargo = 11;

        const int BitSelTotal = 12;

        const int BitSelEDSMButtonsNextLine = 24;
        const int SelDefault = ((1<<BitSelTotal)-1)+(1<<BitSelEDSMButtonsNextLine);

        const int hspacing = 8;

        List<int> Order;        // orderarray is the order to display them on the screen.. same entries as BitSelTotal
        int[] YStart;           // ypos of items in Order.
        int[] YEnd;             // ypos of items in Order.
        ToolStripMenuItem[] toolstriplist;          // ref to toolstrip items for each bit above. in same order as bits BitSel..

        public UserControlSysInfo()
        {
            InitializeComponent();
            Name = "System Information";
        }

        public override void Init(EDDiscoveryForm ed, int displayno)
        {
            discoveryform = ed;
            this.displaynumber = displayno;
            discoveryform.TravelControl.OnTravelSelectionChanged += Display;    // get this whenever current selection or refreshed..
            discoveryform.OnNewTarget += RefreshTargetDisplay;
            discoveryform.OnNoteChanged += OnNoteChanged;
            textBoxTarget.SetAutoCompletor(EDDiscovery.DB.SystemClassDB.ReturnSystemListForAutoComplete);

            // same order as Sel bits are defined in, one bit per selection item.
            toolstriplist = new ToolStripMenuItem[] { toolStripSystem , toolStripEDSM , toolStripVisits, toolStripBody,
                                                        toolStripPosition, toolStripDistanceFrom,
                                                        toolStripSystemState, toolStripNotes, toolStripTarget,
                                                        toolStripGameMode,toolStripTravel,toolStripCargo};
            
            Order = DB.SQLiteDBClass.GetSettingString(DbOSave, "").RestoreIntListFromString(0, BitSelTotal);
            System.Diagnostics.Debug.WriteLine("Ordered " + String.Join(",", Order));
            if (Order.Distinct().Count() != Order.Count)       // if not distinct..
            {
                Order = new List<int>();
                for (int i = 0; i < BitSelTotal; i++)          // reset
                    Order.Add(i);
                System.Diagnostics.Debug.WriteLine("Reset " + String.Join(",", Order));
            }
        }

        public override void Closing()
        {
            discoveryform.TravelControl.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewTarget -= RefreshTargetDisplay;
            DB.SQLiteDBClass.PutSettingString(DbOSave, String.Join(",",Order));
        }

        bool neverdisplayed = true;
        HistoryEntry last_he = null;
        public override void Display(HistoryEntry he, HistoryList hl)
        {
            if (neverdisplayed)
            {
                UpdateViewOnSelection(DB.SQLiteConnectionUser.GetSettingInt(DbSelection, SelDefault));  // then turn the right ones on
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

                    EliteDangerous.ISystem homesys = discoveryform.GetHomeSystem();

                    toolTipEddb.SetToolTip(textBoxHomeDist, $"Distance to home system ({homesys.name})");
                    textBoxHomeDist.Text = DB.SystemClassDB.Distance(he.System, homesys).ToString(SingleCoordinateFormat);
                    textBoxSolDist.Text = DB.SystemClassDB.Distance(he.System, 0, 0, 0).ToString(SingleCoordinateFormat);
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
                    textBoxTravelTime.Visible = textBoxTravelJumps.Visible = true;
                }
                else
                {
                    textBoxTravelDist.Text = "-";
                    textBoxTravelTime.Visible = textBoxTravelJumps.Visible = false;
                }

                RefreshTargetDisplay(this);
            }
            else
            {
                SetControlText("");
                textBoxSystem.Text = textBoxBody.Text = textBoxPosition.Text =
                                textBoxAllegiance.Text = textBoxEconomy.Text = textBoxGovernment.Text =
                                textBoxVisits.Text = textBoxState.Text = textBoxHomeDist.Text = textBoxSolDist.Text =
                                textBoxGameMode.Text = textBoxTravelDist.Text = 
                                "";
                textBoxTravelTime.Visible = textBoxTravelJumps.Visible = false;

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
                RoutingUtils.setTargetSystem(this,discoveryform, textBoxTarget.Text);
            }
        }

        private void RefreshTargetDisplay(Object sender)              // called when a target has been changed.. via EDDiscoveryform
        {
            string name;
            double x, y, z;

            //System.Diagnostics.Debug.WriteLine("Refresh target display");

            if (DB.TargetClass.GetTargetPosition(out name, out x, out y, out z))
            {
                textBoxTarget.Text = name;
                textBoxTargetDist.Text = "No Pos";

                HistoryEntry cs = discoveryform.history.GetLastWithPosition;
                if (cs != null)
                    textBoxTargetDist.Text = DB.SystemClassDB.Distance(cs.System, x, y, z).ToString("0.0");

                toolTipEddb.SetToolTip(textBoxTarget, "Position is " + x.ToString("0.00") + "," + y.ToString("0.00") + "," + z.ToString("0.00"));
            }
            else
            {
                textBoxTarget.Text = "Set target";
                textBoxTargetDist.Text = "";
                toolTipEddb.SetToolTip(textBoxTarget, "On 3D Map right click to make a bookmark, region mark or click on a notemark and then tick on Set Target, or type it here and hit enter");
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

        void ToggleSelection(Object sender, int bit)
        {
            ToolStripMenuItem mi = sender as ToolStripMenuItem;
            if (mi.Enabled)
            {
                int sel = DB.SQLiteConnectionUser.GetSettingInt(DbSelection, SelDefault);
                sel ^= (1 << bit);
                DB.SQLiteConnectionUser.PutSettingInt(DbSelection, sel);
                UpdateViewOnSelection(sel);
            }
        }

        void UpdateViewOnSelection(int sel)
        {
            SuspendLayout();
            Point pos = new Point(3, 3);

            foreach (Control c in this.Controls)
                c.Visible = false;

            int textboxh = EDDTheme.Instance.FontSize > 10 ? 24 : 20;
            int vspacing = textboxh+4;

            System.Diagnostics.Debug.WriteLine("Selection is " + sel);

            YStart = new int[Order.Count];
            YEnd = new int[Order.Count];

            bool selEDSMonNextLine = (sel & (1 << BitSelEDSMButtonsNextLine)) != 0;
            toolStripEDSMDownLine.Checked = selEDSMonNextLine;

            for (int i = 0; i < Order.Count; i++)
            {
                int bit = Order[i];
                bool ison = (sel & (1 << bit)) != 0;

                toolstriplist[bit].Enabled = false;
                toolstriplist[bit].Checked = ison;
                toolstriplist[bit].Enabled = true;

                YStart[i] = (ison) ? pos.Y : -1;

                if (ison)
                {
                    Point datapos = new Point(textBoxSystem.Left, pos.Y);
                    Point labpos2 = new Point(labelSolDist.Left, pos.Y);
                    Point datapos2 = new Point(textBoxSolDist.Left, pos.Y);

                    switch (Order[i])
                    {
                        case BitSelSystem:
                            this.SetPos(ref pos, labelSysName, datapos, textBoxSystem, vspacing);

                            if ( !selEDSMonNextLine && (sel & (1<<BitSelEDSM))!=0)
                            {
                                buttonEDSM.Location = new Point(textBoxSystem.Right + hspacing, datapos.Y);
                                buttonEDDB.Location = new Point(buttonEDSM.Right + hspacing, buttonEDSM.Top);
                                buttonRoss.Location = new Point(buttonEDDB.Right + hspacing, buttonEDSM.Top);
                                buttonEDSM.Visible = buttonEDDB.Visible = buttonRoss.Visible = true;
                            }

                            break;

                        case BitSelEDSM:
                            if ( selEDSMonNextLine)
                            {
                                labelOpen.Location = pos;
                                buttonEDSM.Location = new Point(datapos.X, datapos.Y);
                                buttonEDDB.Location = new Point(buttonEDSM.Right + hspacing, buttonEDSM.Top);
                                buttonRoss.Location = new Point(buttonEDDB.Right + hspacing, buttonEDSM.Top);
                                labelOpen.Visible = buttonEDSM.Visible = buttonEDDB.Visible = buttonRoss.Visible = true;
                                pos.Y += vspacing + 4;
                            }
                            break;

                        case BitSelVisits:
                            this.SetPos(ref pos, labelVisits, datapos, textBoxVisits, vspacing);
                            break;

                        case BitSelBody:
                            this.SetPos(ref pos, labelBodyName, datapos, textBoxBody, vspacing);
                            break;

                        case BitSelPosition:
                            this.SetPos(ref pos, labelPosition, datapos, textBoxPosition, vspacing);
                            break;

                        case BitSelDistanceFrom:
                            this.SetPos(ref pos, labelHomeDist, datapos, textBoxHomeDist, vspacing);
                            OffsetPos(labpos2, labelSolDist, datapos2, textBoxSolDist);
                            break;

                        case BitSelSystemState:
                            this.SetPos(ref pos, labelState, datapos, textBoxState, vspacing-4);
                            OffsetPos(labpos2, labelAllegiance, datapos2, textBoxAllegiance);
                            datapos.Y = labpos2.Y = datapos2.Y = pos.Y;
                            this.SetPos(ref pos, labelGov, datapos, textBoxGovernment, vspacing);
                            OffsetPos(labpos2, labelEconomy, datapos2, textBoxEconomy);
                            break;

                        case BitSelNotes:
                            SetPos(ref pos, labelNote, datapos, richTextBoxNote, richTextBoxNote.Height + 8);
                            break;

                        case BitSelTarget:
                            this.SetPos(ref pos, labelTarget, datapos, textBoxTarget, vspacing);
                            textBoxTargetDist.Location = new Point(textBoxTarget.Right + hspacing, datapos.Y);
                            buttonEDSMTarget.Location = new Point(textBoxTargetDist.Right + hspacing, datapos.Y);
                            textBoxTargetDist.Visible = buttonEDSMTarget.Visible = true;
                            break;

                        case BitSelGameMode:
                            this.SetPos(ref pos, labelGamemode, datapos, textBoxGameMode, vspacing);
                            break;

                        case BitSelTravel:
                            this.SetPos(ref pos, labelTravel, datapos, textBoxTravelDist, vspacing);
                            textBoxTravelTime.Location = new Point(textBoxTravelDist.Right + hspacing, datapos.Y);
                            textBoxTravelJumps.Location = new Point(textBoxTravelTime.Right + hspacing, datapos.Y);
                            // don't set visible for the last two, may not be if not travelling. Display will deal with it
                            break;

                        case BitSelCargo:
                            this.SetPos(ref pos, labelCargo, datapos, textBoxCargo, vspacing);
                            OffsetPos(labpos2, labelMaterials, datapos2, textBoxMaterials);
                            break;
                    }

                    YEnd[i] = pos.Y - 1;
                    System.Diagnostics.Debug.WriteLine("Sel " + i + " " + Order[i] + " on " + ison + " ypos " + YStart[i] +"-" + YEnd[i]);
                }
            }

            ResumeLayout();
            Refresh();
        }


        void SetPos(ref Point lp, Label lab, Point tp, ExtendedControls.TextBoxBorder box, int vspacing = 0)
        {
            lab.Location = lp;
            box.Location = tp;
            lab.Visible = box.Visible = true;
            lp.Y += vspacing;
        }

        void SetPos(ref Point lp, Label lab, Point tp, ExtendedControls.RichTextBoxScroll box, int vspacing = 0)
        {
            lab.Location = lp;
            box.Location = tp;
            lab.Visible = box.Visible = true;
            lp.Y += vspacing;
        }

        void OffsetPos(Point lp, Label lab, Point tp, ExtendedControls.TextBoxBorder box)
        {
            lab.Location = lp;
            box.Location = tp;
            lab.Visible = box.Visible = true;
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

        private void UserControlSysInfo_Resize(object sender, EventArgs e)
        {
            richTextBoxNote.Size = new Size(ClientRectangle.Width - richTextBoxNote.Left - 8, richTextBoxNote.Height);

            int left = ClientRectangle.Width - textBoxTarget.Left - buttonEDSMTarget.Width - hspacing*2 - 2;
            int targetw = left * 7 / 10;
            int distw = left - targetw;

            textBoxTarget.Size = new Size(targetw, richTextBoxNote.Height);
            textBoxTargetDist.Size = new Size(distw, richTextBoxNote.Height);
            textBoxTargetDist.Location = new Point(textBoxTarget.Right + hspacing, textBoxTargetDist.Top);
            buttonEDSMTarget.Location = new Point(textBoxTargetDist.Right + hspacing, buttonEDSMTarget.Top);

        }

        Control inmove = null;
        bool inmovedrag = false;

        private void controlMouseDown(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;

            inmove = c;
            inmovedrag = false;
            System.Diagnostics.Debug.WriteLine("Control " + inmove.Name + " grabbed");
        }

        private void controlMouseUp(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;

            if (inmove != null)
            {
                if (inmovedrag)
                {
                    int movefromy = inmove.Top;
                    int movetoy = movefromy + e.Y;
                    int movefromorder = FindOrder(movefromy);
                    int movetoorder = FindOrder(movetoy);
                    System.Diagnostics.Debug.WriteLine("Control " + inmove.Name + " Released from Y " + movefromy + " to " + movetoy + " " + movefromorder + " " + movetoorder);

                    if (movefromorder >= 0 && movefromorder < Order.Count)        // valid
                    {
                        int selbit = Order[movefromorder];

                        Order.RemoveAt(movefromorder);
                        System.Diagnostics.Debug.WriteLine("Removed " + String.Join(",", Order));

                        if (movetoorder == -1)
                            Order.Insert(0, selbit);
                        else if (movetoorder == 999)
                            Order.Add(selbit);
                        else
                            Order.Insert(movetoorder, selbit);

                        System.Diagnostics.Debug.WriteLine("Re-ordered " + String.Join(",", Order));
                        UpdateViewOnSelection(DB.SQLiteConnectionUser.GetSettingInt(DbSelection, SelDefault));  // then turn the right ones on

                    }

                    Cursor.Current = Cursors.Default;
                }

                inmove = null;
                
            }
        }

        private int FindOrder(int y)
        {
            for(int i = 0 ; i < Order.Count; i++ )
            {
                if (YStart[i] != -1)
                {
                    if (i == 0 && y < YStart[i])
                        return -1;

                    if (y >= YStart[i] && y <= YEnd[i])
                        return i;
                }
            }

            return 999;
        }

        private void controlMouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;

            if (inmove != null)
            {
                if (e.Y < -4 || e.Y > c.Height + 4)
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

    }
}
