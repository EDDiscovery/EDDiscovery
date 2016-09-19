using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery.EliteDangerous;

namespace EDDiscovery
{
    public partial class JournalViewControl : UserControl
    {
        private EDDiscoveryForm _discoveryForm;

        public JournalViewControl()
        {
            InitializeComponent();
        }

        public void InitControl(EDDiscoveryForm discoveryForm)
        {
            _discoveryForm = discoveryForm;
        }


        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            string errmsg;
            bool forceReload = true;

            EDJournalClass journalclass = new EDJournalClass(_discoveryForm);
            var journals = journalclass.ParseJournalFiles(out errmsg, forceReload);   // Parse files stop monitor..

            dataGridViewJournal.Rows.Clear();

            foreach (JournalEntry je in journals)
            {
                AddNewJournalRow(false, je);
            }


        }

        private void AddNewJournalRow(bool insert, JournalEntry item)            // second part of add history row, adds item to view.
        {
            object[] rowobj = { item.EventTimeLocal, item.EventTypeStr, item.ToShortString() };
            int rownr;

            
            if (insert)
            {
                dataGridViewJournal.Rows.Insert(0, rowobj);
                rownr = 0;
            }
            else
            {
                dataGridViewJournal.Rows.Add(rowobj);
                rownr = dataGridViewJournal.Rows.Count - 1;
            }
    
        }

        private void buttonRefresh_Click_1(object sender, EventArgs e)
        {

        }
    }
}
