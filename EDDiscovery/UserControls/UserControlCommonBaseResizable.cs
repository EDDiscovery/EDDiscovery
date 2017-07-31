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
    public partial class UserControlCommonBaseResizable : UserControl
    {
        public UserControlCommonBase uc;

        public UserControlCommonBaseResizable()
        {
            InitializeComponent();
        }

        void Init( UserControlCommonBase c)
        {
            uc = c;
            PerformLayout();
        }

        const int margin = 3;

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            uc.Location = new Point(margin, margin);
            uc.Size = new Size(ClientRectangle.Width - margin * 2, ClientRectangle.Height - margin * 2);
        }
    }
}
