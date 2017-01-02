using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery.EliteDangerous;
using EDDiscovery.DB;
using EDDiscovery.Controls;
using EDDiscovery2.EDSM;
using EDDiscovery.UserControls;

namespace EDDiscovery
{
    public partial class JournalViewControl : UserControl
    {
        EDDiscoveryForm discoveryform;

        public JournalViewControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm ed, int displaynumber)
        {
            discoveryform = ed;
            userControlJournalGrid.Init(ed,displaynumber);
            userControlJournalGrid.ShowRefresh();
            userControlJournalGrid.OnPopOut += PopOut;
        }

        #region Layout

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            userControlJournalGrid.LoadLayout();
        }

        public void SaveSettings()     // called by form when closing
        {
            userControlJournalGrid.Closing();
        }

        public void RefreshButton(bool state)
        {
            userControlJournalGrid.RefreshButton(state);
        }

        public void PopOut()
        {
            discoveryform.TravelControl.PopOut(PopOuts.Journal);
        }

        #endregion

    }

}
