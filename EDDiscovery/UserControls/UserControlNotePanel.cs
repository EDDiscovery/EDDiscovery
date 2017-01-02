using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using EDDiscovery2.DB;
using EDDiscovery.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlNotePanel : UserControlCommonBase
    {

        private EDDiscoveryForm discoveryform;
        private TravelHistoryControl travelhistorycontrol;


        private int displaynumber = 0;
        private string DbSave { get { return "NotePanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        HistoryEntry lastHE;

        private Font displayfont;
        [Flags]
        enum Configuration
        {
            showSystemNotes = 1,
            showGMPNotes = 2,

        };

        Configuration config = (Configuration)(Configuration.showSystemNotes | Configuration.showGMPNotes);

        bool Config(Configuration c) { return (config & c) != 0; }

        public UserControlNotePanel()
        {
            InitializeComponent();
        }

        public override void Init(EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            config = (Configuration)SQLiteDBClass.GetSettingInt(DbSave + "Config", (int)config);

            discoveryform = ed;
            travelhistorycontrol = ed.TravelControl;
            displaynumber = vn;

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += NewEntry;
            travelhistorycontrol.OnTravelSelectionChanged += DisplaySelected;

            displayfont = discoveryform.theme.GetFont;
        }

        private void DisplaySelected(HistoryEntry he, HistoryList hl)
        {
            if (he != null && he.EntryType == EliteDangerous.JournalTypeEnum.FSDJump)
                Display(he);
            else
                Display(hl);
        }

        public override void Closing()
        {
            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= NewEntry;
            travelhistorycontrol.OnTravelSelectionChanged -= DisplaySelected;
            SQLiteDBClass.PutSettingInt(DbSave + "Config", (int)config);
        }

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made..
        {
            if (he != null && he.EntryType == EliteDangerous.JournalTypeEnum.FSDJump)
                Display(he);
            else
                Display(hl);
        }

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            Display(lastHE);
        }

        public override void Display(HistoryEntry current, HistoryList history)
        {
            Display(history);
        }

        public void Display( HistoryList hl)            // when user clicks around..  HE may be null here
        {
            Display(hl.GetLastFSD);
        }

        void FlipConfig(Configuration item, bool ch, bool redisplay = false)
        {
            if (ch)
                config = (Configuration)((int)config | (int)item);
            else
                config = (Configuration)((int)config & ~(int)item);
        }

        void Display(HistoryEntry he)
        {
            pictureBox.ClearImageList();

            lastHE = he;

            if (he != null)
            {
                Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                Color backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                discoveryform.history.FillEDSM(he, reload: true); // Fill in any EDSM info we have, force it to try again.. in case system db updated
                string botline = "";
                if (Config(Configuration.showGMPNotes))
                {
                    var gmo = discoveryform.galacticMapping.Find(he.System.name);
                    if (gmo != null)
                        botline = Tools.WordWrap("GMP: " + gmo.description, 60) + Environment.NewLine;
                }

                if (Config(Configuration.showSystemNotes))
                {
                    if (he.snc != null)
                        botline += Tools.WordWrap("NOTE: " + he.snc.Note, 60) + Environment.NewLine;
                }

                ExtendedControls.PictureBoxHotspot.ImageElement botlineleft = pictureBox.AddTextAutoSize(
                    new Point(5, 5),
                    new Size(1000, 1000),
                    botline,
                    displayfont,
                    textcolour,
                    backcolour,
                    1.0F);
            }

            pictureBox.Render();
        }

  

        private void miSystemNotes_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showSystemNotes, ((ToolStripMenuItem)sender).Checked, true);
            Display(lastHE);
        }

        private void miGMPNotes_Click(object sender, EventArgs e)
        {
            FlipConfig(Configuration.showGMPNotes, ((ToolStripMenuItem)sender).Checked, true);
            Display(lastHE);
        }
    }
}
