using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery
{
    public partial class GridControl : UserControl
    {
        public GridControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm ed, UserControls.UserControlCursorType uctg, int displaynumber)
        {
            userControlContainerGrid.Init(ed, uctg, displaynumber);     // pass in the default TG
        }

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            userControlContainerGrid.LoadLayout();
            userControlContainerGrid.InitialDisplay();
        }

        public void SaveSettings()     // called by form when closing
        {
            userControlContainerGrid.Closing();
        }

    }
}
