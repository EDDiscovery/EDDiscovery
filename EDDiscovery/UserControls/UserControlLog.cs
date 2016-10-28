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
        public UserControlLog()
        {
            InitializeComponent();
        }

        public void AppendText(string s, Color c)
        {
            richTextBox_History.AppendText(s, c);
        }

        public void CopyTextFrom( UserControlLog other )
        {
            richTextBox_History.CopyFrom(other.richTextBox_History);
        }

        public override void LoadLayout() { }
        public override void SaveLayout() { }

    }
}
