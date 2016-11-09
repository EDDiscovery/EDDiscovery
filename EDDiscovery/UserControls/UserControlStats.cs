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
    public partial class UserControlStats : UserControlCommonBase
    {
        private int displaynumber = 0;
        private TravelHistoryControl travelhistorycontrol;

        public UserControlStats()
        {
            InitializeComponent();
        }

        public override void Init(TravelHistoryControl thc, int vn) //0=primary, 1 = first windowed version, etc
        {
            travelhistorycontrol = thc;
            displaynumber = vn;
        }

        public void Display(HistoryList hl)
        {

        }
    }
}
