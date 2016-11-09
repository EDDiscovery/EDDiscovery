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
    public partial class UserControlLog : UserControlCommonBase
    {
        private TravelHistoryControl travelhistorycontrol;

        public UserControlLog()
        {
            InitializeComponent();
            Name = "Log";
        }

        public override void Init(TravelHistoryControl thc, int displayno)
        {
            travelhistorycontrol = thc;
            travelhistorycontrol.OnNewLogEntry += AppendText;
        }

        public override void Closing()
        {
            travelhistorycontrol.OnNewLogEntry -= AppendText;
        }

        public void AppendText(string s, Color c)
        {
            richTextBox_History.AppendText(s, c);
        }

        public void CopyTextFrom( UserControlLog other )
        {
            richTextBox_History.CopyFrom(other.richTextBox_History);
        }
    }
}
