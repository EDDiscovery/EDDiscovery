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
        private EDDiscoveryForm discoveryform;
        private int displaynumber;
        private string DbSelection { get { return ("SystemInformation") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "Sel"; } }

        const int SelNotes = 1;
        const int SelBody = 2;
        const int SelSystem = 4;
        const int SelTarget = 8;
        const int SelEDSMButtonsNextLine = 16;
        const int SelEDSM = 32;
        const int SelVisits = 64;
        const int SelSystemState = 128;
        const int SelPosition = 256;
        const int SelDistanceFrom = 512;
        const int SelDefault = SelNotes | SelBody | SelSystem | SelTarget | SelEDSM | SelVisits | SelSystemState | SelPosition | SelDistanceFrom;

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
            textBoxTarget.SetAutoCompletor(EDDiscovery.DB.SystemClassDB.ReturnSystemListForAutoComplete);

            UpdateViewOnSelection(0);       // first turn them all off so they all compress..
            UpdateViewOnSelection(DB.SQLiteConnectionUser.GetSettingInt(DbSelection, SelDefault));  // then turn the right ones on
        }

        public override void Closing()
        {
            discoveryform.TravelControl.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewTarget -= RefreshTargetDisplay;
        }

        HistoryEntry last_he = null;
        public override void Display(HistoryEntry he, HistoryList hl)
        {
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
                richTextBoxNote.Text = he.snc != null ? he.snc.Note : "";

                RefreshTargetDisplay();
            }
            else
            {
                SetControlText("");
                textBoxSystem.Text = textBoxBody.Text = textBoxPosition.Text = 
                                textBoxAllegiance.Text = textBoxEconomy.Text = textBoxGovernment.Text =
                                textBoxVisits.Text = textBoxState.Text = textBoxHomeDist.Text = richTextBoxNote.Text = textBoxSolDist.Text = "";
                buttonRoss.Enabled = buttonEDDB.Enabled = false;
            }
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
//            richTextBox_History.TextBox.Clear();
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
                RoutingUtils.setTargetSystem(discoveryform, textBoxTarget.Text);
            }
        }

        public void RefreshTargetDisplay()              // called when a target has been changed.. via EDDiscoveryform
        {
            string name;
            double x, y, z;

            System.Diagnostics.Debug.WriteLine("Refresh target display");

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

        private void richTextBoxNote_Leave(object sender, EventArgs e)
        {
            if (last_he != null)
                discoveryform.StoreSystemNote(last_he, richTextBoxNote.Text.Trim(), true);
        }

        private void richTextBoxNote_TextBoxChanged(object sender, EventArgs e)
        {
            if (last_he != null)
                discoveryform.StoreSystemNote(last_he, richTextBoxNote.Text.Trim(), false);
        }

        private void toolStripSystem_Click(object sender, EventArgs e)
        {
            ToggleSelection(SelSystem);
        }
        private void toolStripBody_Click(object sender, EventArgs e)
        {
            ToggleSelection(SelBody);
        }
        private void toolStripNotes_Click(object sender, EventArgs e)
        {
            ToggleSelection(SelNotes);
        }
        private void toolStripTarget_Click(object sender, EventArgs e)
        {
            ToggleSelection(SelTarget);
        }
        private void toolStripEDSMButtons_Click(object sender, EventArgs e)
        {
            ToggleSelection(SelEDSMButtonsNextLine);
        }
        private void toolStripEDSM_Click(object sender, EventArgs e)
        {
            ToggleSelection(SelEDSM);
        }
        private void toolStripVisits_Click(object sender, EventArgs e)
        {
            ToggleSelection(SelVisits);
        }

        private void toolStripPosition_Click(object sender, EventArgs e)
        {
            ToggleSelection(SelPosition);
        }

        private void enableDistanceFromToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleSelection(SelDistanceFrom);
        }

        private void toolStripSystemState_Click(object sender, EventArgs e)
        {
            ToggleSelection(SelSystemState);
        }
        void ToggleSelection(int mask)
        {
            int sel = DB.SQLiteConnectionUser.GetSettingInt(DbSelection, SelDefault);
            sel ^= mask;
            DB.SQLiteConnectionUser.PutSettingInt(DbSelection, sel);
            UpdateViewOnSelection(sel);
        }

        void UpdateViewOnSelection(int sel)
        {
            if (buttonEDSM.Tag != null && (bool)buttonEDSM.Tag)     // if previously we shifted EDSM buttons up, we move them back into place for the main calc
                buttonEDSM.Top = buttonEDDB.Top = buttonRoss.Top = buttonEDSM.Top + 24;

            //System.Diagnostics.Debug.WriteLine("============ " + sel);
            bool systemon = (sel & SelSystem) != 0;
            toolStripSystem.Checked = labelSysName.Visible = textBoxSystem.Visible = systemon;
            this.Controls.ShiftControls(labelOpen, 24, systemon);

            bool edsmbuttons = (sel & SelEDSM) != 0;
            bool edsmbuttonsdownoneline = (sel & SelEDSMButtonsNextLine) != 0;
            bool makespace = edsmbuttons && (!systemon || edsmbuttonsdownoneline);  // if buttons are on but system isn't, or we want it down line down
            //System.Diagnostics.Debug.WriteLine("EDSM Selection  " + edsmbuttons + " down " + edsmbuttonsdownoneline + " space "  + makespace);

            toolStripEDSM.Checked = buttonEDDB.Visible = buttonEDSM.Visible = buttonRoss.Visible = edsmbuttons;
            toolStripEDSMDownLine.Checked = edsmbuttonsdownoneline;
            labelOpen.Visible = makespace;
            buttonEDSM.Left = (makespace==true) ? textBoxSystem.Left : textBoxSystem.Right + 8;
            buttonEDDB.Left = buttonEDSM.Right + 8;
            buttonRoss.Left = buttonEDDB.Right + 8;
            buttonEDSM.Tag = (edsmbuttons && !makespace); // remember if we shifted it artifically up
            if ( (bool)buttonEDSM.Tag)      // and if we did, we shift it up
                buttonEDSM.Top = buttonEDDB.Top = buttonRoss.Top = buttonEDSM.Top - 24;
            this.Controls.ShiftControls(labelVisits, 24, makespace);     // shift up/down the ones below dependent on if edsm needs space..

            bool visitson = (sel & SelVisits) != 0;
            toolStripVisits.Checked = labelVisits.Visible = textBoxVisits.Visible = visitson;
            this.Controls.ShiftControls(labelBodyName, 24, visitson);

            bool bodyon = (sel & SelBody) != 0;
            toolStripBody.Checked = labelBodyName.Visible = textBoxBody.Visible = bodyon;
            this.Controls.ShiftControls(labelPosition, 24, bodyon);

            bool poson = (sel & SelPosition) != 0;
            toolStripPosition.Checked = labelPosition.Visible = textBoxPosition.Visible = poson;
            this.Controls.ShiftControls(labelHomeDist, 24, poson);

            bool diston = (sel & SelDistanceFrom) != 0;
            toolStripDistanceFrom.Checked = labelSolDist.Visible = labelHomeDist.Visible = textBoxHomeDist.Visible = textBoxSolDist.Visible = diston;
            this.Controls.ShiftControls(labelState, 24, diston);

            bool stateon = (sel & SelSystemState) != 0;
            toolStripSystemState.Checked = labelState.Visible = labelGov.Visible = labelAllegiance.Visible = labelEconomy.Visible =
                        textBoxState.Visible = textBoxGovernment.Visible = textBoxAllegiance.Visible = textBoxEconomy.Visible = stateon;
            this.Controls.ShiftControls(labelNote, 48, stateon);

            bool noteson = (sel & SelNotes) != 0;
            toolStripNotes.Checked = labelNote.Visible = richTextBoxNote.Visible = noteson;
            this.Controls.ShiftControls(labelTarget, 60, noteson);

            bool targeton = (sel & SelTarget) != 0;
            toolStripTarget.Checked = labelTarget.Visible = textBoxTarget.Visible = textBoxTargetDist.Visible = buttonEDSMTarget.Visible = targeton;

            Refresh();
        }

        void Layout(Control c, int offset , bool on)
        {

            Refresh();
           
        }

        private void UserControlSysInfo_Resize(object sender, EventArgs e)
        {
            richTextBoxNote.Size = new Size(ClientRectangle.Width - richTextBoxNote.Left - 8, richTextBoxNote.Height);

            int left = ClientRectangle.Width - textBoxTarget.Left - buttonEDSMTarget.Width - 8 - 4;
            int targetw = left * 7 / 10;
            int distw = left - targetw;

            textBoxTarget.Size = new Size(targetw, richTextBoxNote.Height);
            textBoxTargetDist.Size = new Size(distw, richTextBoxNote.Height);
            textBoxTargetDist.Location = new Point(textBoxTarget.Right + 4, textBoxTargetDist.Top);
            buttonEDSMTarget.Location = new Point(textBoxTargetDist.Right + 4, buttonEDSMTarget.Top);

        }

    }
}
