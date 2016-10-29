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

namespace EDDiscovery
{
    public partial class JournalViewControl : UserControl
    {
        EDDiscoveryForm discoveryform;

        public JournalViewControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm df)
        {
            discoveryform = df;
            userControlJournalGrid.Init(discoveryform, 0 , true);
        }

        public void Display()
        {
            userControlJournalGrid.Display(discoveryform.history);
        }

        public void AddNewEntry(HistoryEntry he)
        {
            userControlJournalGrid.AddNewEntry(he);
        }

        #region Layout

        public void LoadLayoutSettings() // called by discovery form by us after its adjusted itself
        {
            userControlJournalGrid.LoadLayout();
        }

        public void SaveSettings()     // called by form when closing
        {
            userControlJournalGrid.SaveLayout();
        }

        public void RefreshButton(bool state)
        {
            userControlJournalGrid.RefreshButton(false);
        }

        #endregion

    }

}
