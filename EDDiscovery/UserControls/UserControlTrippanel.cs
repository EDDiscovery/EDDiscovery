using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.DB;
using EMK.LightGeometry;

namespace EDDiscovery.UserControls
{
    public partial class UserControlTrippanel : UserControlCommonBase
    {
        private EDDiscoveryForm discoveryform;
        private TravelHistoryControl travelhistorycontrol;

        static String TITLE = "Trip panel";

        private int displaynumber = 0;
        private string DbSave { get { return "TripPanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        HistoryEntry lastHE;

        private Font displayfont;

        public UserControlTrippanel()
        {
            InitializeComponent();
        }

        public override void Init(EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            travelhistorycontrol = ed.TravelControl;
            displaynumber = vn;

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += NewEntry;
            discoveryform.OnNewTarget += NewTarget;

            displayfont = discoveryform.theme.GetFont;
        }


        public override void Closing()
        {
            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= NewEntry;
            discoveryform.OnNewTarget -= NewTarget;
        }

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            displayLastFSDOrScoop(lastHE);
        }

        #region Display

        public override void Display(HistoryEntry current, HistoryList history)
        {
            Display(history);
        }

        public void Display(HistoryList hl)            // when user clicks around..  HE may be null here
        {
            HistoryEntry lfs = hl.GetLastFuelScoop;
            HistoryEntry hex = hl.GetLastFSD;
            HistoryEntry fuel = hl.GetLastRefuel;
            if (lfs != null && lfs.EventTimeUTC >= hex.EventTimeUTC)
                hex = lfs;
            if (fuel != null && fuel.EventTimeUTC >= hex.EventTimeUTC)
                hex = fuel;
            displayLastFSDOrScoop(hex);
        }

        public void NewTarget()
        {
            displayLastFSDOrScoop(lastHE);
        }

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made..
        {
            Display(hl);
        }


        void displayLastFSDOrScoop(HistoryEntry he)
        {
            pictureBox.ClearImageList();

            lastHE = he;

            if (he != null)
            {
                Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                Color backcolour = IsTransparent ? Color.Black : this.BackColor;

                ExtendedControls.PictureBoxHotspot.ImageElement edsm = pictureBox.AddTextFixedSizeC(new Point(5, 5), new Size(80, 20), "EDSM", displayfont, backcolour, textcolour, 0.5F, true, he, "View system on EDSM");
                edsm.SetAlternateImage(ExtendedControls.ControlHelpers.DrawTextIntoFixedSizeBitmapC("EDSM", edsm.img.Size, displayfont, backcolour, ExtendedControls.ButtonExt.Multiply(textcolour, 1.2F), 0.5F, true), edsm.pos, true);

                ExtendedControls.PictureBoxHotspot.ImageElement start = pictureBox.AddTextFixedSizeC(new Point(5, 35), new Size(80, 20), "Start", displayfont, backcolour, textcolour, 0.5F, true, "Start", "Set a journey start point");
                start.SetAlternateImage(ExtendedControls.ControlHelpers.DrawTextIntoFixedSizeBitmapC("Start", edsm.img.Size, displayfont, backcolour, ExtendedControls.ButtonExt.Multiply(textcolour, 1.2F), 0.5F, true), start.pos , true);

                backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                string name;
                Point3D tpos;
                bool targetpresent = TargetClass.GetTargetPosition(out name, out tpos);

                String topline = "  Route Not Set";

                if (targetpresent)
                {
                    double dist = SystemClass.Distance(he.System, tpos.X, tpos.Y, tpos.Z);

                    var jumpRange = SQLiteDBClass.GetSettingDouble(DbSave + "JumpRange", -1.0);
                    string mesg = "Left";
                    if (jumpRange > 0)
                    {
                        int jumps =(int) Math.Ceiling(dist / jumpRange);
                        if(jumps>0)
                            mesg = "@ " + jumps.ToString() + ( (jumps == 1) ? " jump" : " jumps");
                    }
                    topline = String.Format("{0} | {1:N2}ly {2}", name, dist, mesg);
                }

                topline = String.Format("{0} | {1}", he.System.name, topline);

                pictureBox.AddTextAutoSize(new Point(100, 5), new Size(1000, 40), topline, displayfont, textcolour, backcolour, 1.0F);

                string botline = "";

                botline += String.Format("{0:n}{1} @ {2} | {3} | ", he.TravelledDistance, ((he.TravelledMissingjump > 0) ? "ly (*)" : "ly"),
                                        he.Travelledjumps,
                                        he.TravelledSeconds);

                ExtendedControls.PictureBoxHotspot.ImageElement botlineleft = pictureBox.AddTextAutoSize(new Point(100, 35), new Size(1000, 40), botline, displayfont, textcolour, backcolour, 1.0F);

                double tankSize = SQLiteDBClass.GetSettingDouble(DbSave + "TankSize", 32);
                double tankWarning = SQLiteDBClass.GetSettingDouble(DbSave + "TankWarning", 25);
                double fuel=0.0;
                HistoryEntry fuelhe;


                switch (he.journalEntry.EventTypeID)
                { 
                    case EliteDangerous.JournalTypeEnum.FuelScoop:
                       fuel = (he.journalEntry as EliteDangerous.JournalEvents.JournalFuelScoop).Total;
                        break;
                    case EliteDangerous.JournalTypeEnum.FSDJump:
                        fuel = (he.journalEntry as EliteDangerous.JournalEvents.JournalFSDJump).FuelLevel;
                        break;
                    case EliteDangerous.JournalTypeEnum.RefuelAll:
                         fuelhe = discoveryform.history.GetLastFSDOrFuelScoop;
                        if(fuelhe.journalEntry.EventTypeID==EliteDangerous.JournalTypeEnum.FSDJump)
                            fuel = (fuelhe.journalEntry as EliteDangerous.JournalEvents.JournalFSDJump).FuelLevel;
                        else
                            fuel = (fuelhe.journalEntry as EliteDangerous.JournalEvents.JournalFuelScoop).Total;
                        fuel += (he.journalEntry as EliteDangerous.JournalEvents.JournalRefuelAll).Amount;
                        break;
                    case EliteDangerous.JournalTypeEnum.RefuelPartial:
                         fuelhe = discoveryform.history.GetLastFSDOrFuelScoop;
                        if (fuelhe.journalEntry.EventTypeID == EliteDangerous.JournalTypeEnum.FSDJump)
                            fuel = (fuelhe.journalEntry as EliteDangerous.JournalEvents.JournalFSDJump).FuelLevel;
                        else
                            fuel = (fuelhe.journalEntry as EliteDangerous.JournalEvents.JournalFuelScoop).Total;
                        fuel += (he.journalEntry as EliteDangerous.JournalEvents.JournalRefuelPartial).Amount;
                        break;
                    //fuel += (he.journalEntry as EliteDangerous.JournalEvents.JournalRefuelAll).Amount;
                    //case EliteDangerous.JournalTypeEnum.RefuelPartial:
                    ////fuel += (he.journalEntry as EliteDangerous.JournalEvents.JournalRefuelPartial).Amount;
                    default:
                        break;
                }

                botline = String.Format("{0}t / {1}t", fuel.ToString("N1"), tankSize.ToString("N1"));

                if ((fuel / tankSize) < (tankWarning / 100.0))
                {
                    textcolour = discoveryform.theme.TextBlockHighlightColor;
                    botline += String.Format(" < {0}%", tankWarning.ToString("N1"));
                }

                pictureBox.AddTextAutoSize(new Point(botlineleft.pos.Right, 35), new Size(1000, 40), botline, displayfont, textcolour, backcolour, 1.0F);
            }

            pictureBox.Render();
        }

        #endregion

        #region Clicks
        private void pictureBox_ClickElement(object sender, MouseEventArgs eventargs, ExtendedControls.PictureBoxHotspot.ImageElement i, object tag)
        {
            if (i != null)
            {
                HistoryEntry he = tag as HistoryEntry;

                if (he != null)
                {
                    EDDiscovery2.EDSM.EDSMClass edsm = new EDDiscovery2.EDSM.EDSMClass();

                    string url = edsm.GetUrlToEDSMSystem(he.System.name);

                    if (url.Length > 0)         // may pass back empty string if not known, this solves another exception
                        System.Diagnostics.Process.Start(url);
                    else
                        MessageBox.Show("System " + he.System.name + " unknown to EDSM");
                }
                else
                {
                    var list = discoveryform.history.Where(p => p.IsFSDJump).OrderByDescending(p => p.EventTimeUTC).Take(2);
                    if (list.Count() == 0)
                        return;
                    he = list.ToArray()[0];
                    if (he.StartMarker)
                        return;

                    he.StartMarker = true;
                    EliteDangerous.JournalEntry.UpdateSyncFlagBit(he.Journalid, EliteDangerous.SyncFlags.StartMarker, he.StartMarker);
                    if (list.Count() > 1 && he.isTravelling)
                    {
                        he = list.ToArray()[1];
                        he.StopMarker = true;
                        EliteDangerous.JournalEntry.UpdateSyncFlagBit(he.Journalid, EliteDangerous.SyncFlags.StopMarker, he.StopMarker);
                    }
                    discoveryform.RefreshHistoryAsync();
                }
            }
        }

        #endregion

        static class Prompt
        {
            public static string ShowDialog(Form p, string text, String defaultValue, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 440,
                    Height = 160,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen,
                };

                Label textLabel = new Label() { Left = 10, Top = 20, Width = 400, Text = text };
                TextBox textBox = new TextBox() { Left = 10, Top = 50, Width = 400 };
                textBox.Text = defaultValue;
                Button confirmation = new Button() { Text = "Ok", Left = 245, Width = 80, Top = 90, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "Cancel", Left = 330, Width = 80, Top = 90, DialogResult = DialogResult.Cancel };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                cancel.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(cancel);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog(p) == DialogResult.OK ? textBox.Text : null;
            }
        }

        private void setFuelTankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tankSize = SQLiteDBClass.GetSettingDouble(DbSave+"TankSize", 32.0);
            string promptValue = Prompt.ShowDialog(this.FindForm(), "Set fuel tank size", "" + tankSize, TITLE);
            if (String.IsNullOrEmpty(promptValue))
                return;
            double value = 0;
            if (double.TryParse(promptValue, out value))
            {
                SQLiteDBClass.PutSettingDouble(DbSave+"TankSize", value);
                displayLastFSDOrScoop(lastHE);
            }
            else
            {
                MessageBox.Show("Please enter a numeric value", TITLE);
            }

        }

        private void setFuelWarningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tankWarning = SQLiteDBClass.GetSettingDouble(DbSave+"TankWarning", 25.0);
            string promptValue = Prompt.ShowDialog(this.FindForm(), "Set fuel tank warning percentage", "" + tankWarning, TITLE);
            if (String.IsNullOrEmpty(promptValue))
                return;
            double value = 0;
            if (double.TryParse(promptValue, out value) && value >= 0 && value <= 100)
            {
                SQLiteDBClass.PutSettingDouble(DbSave+"TankWarning", value);
                displayLastFSDOrScoop(lastHE);
            }
            else
            {
                MessageBox.Show("Please enter a numeric value between 1-100", TITLE);
            }
        }

        private void setShipDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var jumpRange = SQLiteDBClass.GetSettingDouble(DbSave+"JumpRange", 10.0);
            string promptValue = Prompt.ShowDialog(this.FindForm(), "Set your estimated jump range", "" + jumpRange, TITLE);
            if (String.IsNullOrEmpty(promptValue))
                return;
            double value = 0;
            if (double.TryParse(promptValue, out value) && value >= 0)
            {
                SQLiteDBClass.PutSettingDouble(DbSave+"JumpRange", value);
                displayLastFSDOrScoop(lastHE);
            }
            else
            {
                MessageBox.Show("Please enter a numeric value", TITLE);
            }

        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            displayLastFSDOrScoop(lastHE);
        }
    }
}

