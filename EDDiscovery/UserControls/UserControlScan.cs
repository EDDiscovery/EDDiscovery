using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EDDiscovery.EliteDangerous.JournalEvents;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScan : UserControlCommonBase
    {
        private TravelHistoryControl travelhistorycontrol;
        private int displaynumber = 0;

        #region Init
        public UserControlScan()
        {
            InitializeComponent();
        }

        public override void Init(EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            travelhistorycontrol = ed.TravelControl;
            displaynumber = vn;

            travelhistorycontrol.OnTravelSelectionChanged += Display;
        }

        #endregion

        public void Display(HistoryEntry he, HistoryList hl)
        {
            if (he == null)
                richTextBox1.Text = "No selection";
            else
            {
                StarScan.SystemNode sn = hl.starscan.FindSystem(he.System);

                if (sn != null)
                {
                    richTextBox1.Clear();

                    foreach (StarScan.ScanNode starnode in sn.starnodes)        // always has scan nodes
                    {
                        AddText(string.Format("Star Name " + starnode.rootname));

                        if (starnode.scandata != null)
                            AddText(starnode.scandata.DisplayString(false , 4));

                        if (starnode.children != null)
                        {
                            foreach (StarScan.ScanNode planetnode in starnode.children)
                            {
                                AddText(string.Format("    Planet " + planetnode.rootname));
                                if (planetnode.scandata != null)
                                    AddText(planetnode.scandata.DisplayString(false,8));

                                if (planetnode.children != null)
                                {
                                    foreach (StarScan.ScanNode moonnode in planetnode.children)
                                    {
                                        AddText(string.Format("        Moon " + moonnode.rootname));
                                        if (moonnode.scandata != null)
                                            AddText(moonnode.scandata.DisplayString( false , 12));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    richTextBox1.Text = "Not Found";
            }
        }

        void AddText(string s)
        {
            richTextBox1.AppendText(s + Environment.NewLine);
        }

        #region Layout

        public override void LoadLayout()
        {
        }

        public override void Closing()
        {
            travelhistorycontrol.OnTravelSelectionChanged -= Display;
        }

        #endregion
    }
}

