using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlTrippanel : UserControlCommonBase
    {
        private EDDiscoveryForm discoveryform;
        private TravelHistoryControl travelhistorycontrol;

        private int displaynumber = 0;
        private string DbSave { get { return "TripPanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        HistoryList current_historylist;

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

            displayfont = discoveryform.theme.GetFont;
        }


        public override void Closing()
        {

        }

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            Display(current_historylist);
        }

        #region Display

        public void Display(HistoryList hl)            // when user clicks around..  HE may be null here
        {
            pictureBox.ClearImageList();

            current_historylist = hl;

            if (hl != null && hl.Count > 0)     // just for safety
            {
                Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
                Color backcolour = IsTransparent ? Color.Transparent : this.BackColor;

                pictureBox.AddTextAutoSize(new Point(0,0),
                                new Size(200,200),
                                "Hello trippanel", displayfont, textcolour, backcolour, 1.0F, null, "tool tip here");
            }

            pictureBox.Render();
        }

        #endregion
    }
}

