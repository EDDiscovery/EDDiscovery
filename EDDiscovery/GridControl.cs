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
        EDDiscoveryForm discoveryForm;

        public GridControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm ed, int displaynumber)
        {
            discoveryForm = ed;
            userControlContainerGrid.Init(ed, ed.TravelControl.GetTravelGrid, displaynumber);     // pass in the default TG
        }

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            userControlContainerGrid.LoadLayout();
            userControlContainerGrid.Display(discoveryForm.TravelControl.GetTravelHistoryCurrent, discoveryForm.history);
        }

        public void SaveSettings()     // called by form when closing
        {
            userControlContainerGrid.Closing();
        }

    }
}
