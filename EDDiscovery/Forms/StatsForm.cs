using EDDiscovery;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2
{
    public partial class StatsForm : Form
    {
        public TravelHistoryControl travelhistoryctrl;

        public StatsForm()
        {
            InitializeComponent();

            chart1.Dock = DockStyle.Fill;
            dataGridView1.Dock = DockStyle.Fill;
            
        }



        private void StatsForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.Visible = false;
            chart1.Visible = false;
            dataGridView1.Visible = false;

            if (comboBox1.SelectedIndex == 0)
                StatGeneral();
            else if (comboBox1.SelectedIndex == 1)
                StatMostVisits();
            
        
        }


         public void LogText(string text)
        {
            LogText(text, Color.Black);
        }

         public void LogText(string text, Color color)
        {
            try
            {

                richTextBox1.SelectionStart = richTextBox1.TextLength;
                richTextBox1.SelectionLength = 0;

                richTextBox1.SelectionColor = color;
                richTextBox1.AppendText(text);
                richTextBox1.SelectionColor = richTextBox1.ForeColor;




                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.SelectionLength = 0;
                richTextBox1.ScrollToCaret();
                richTextBox1.Refresh();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception SystemClass: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
            }
        }


        private void StatGeneral()
        {
            richTextBox1.Clear();
            richTextBox1.Visible = true;

            int nr;

            nr = travelhistoryctrl.visitedSystems.Count;

            LogText("Total Nr of jumps: " + nr + Environment.NewLine);

            var queryres = from a in travelhistoryctrl.visitedSystems where a.Time > DateTime.Now.AddDays(-30) select a;
            LogText("Last 30 days: " + queryres.Count().ToString() + Environment.NewLine);

            queryres = from a in travelhistoryctrl.visitedSystems where a.Time > DateTime.Now.AddDays(-7) select a;
            LogText("Last week: " + queryres.Count().ToString() + Environment.NewLine);

            queryres = from a in travelhistoryctrl.visitedSystems where a.Time > DateTime.Now.AddDays(-1) select a;
            LogText("Last 24 hours: " + queryres.Count().ToString() + Environment.NewLine);


        }


        private void StatMostVisits()
        {
            dataGridView1.Visible = true;

            int nr;


            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            dataGridView1.Columns.Add("Name", "System");
            dataGridView1.Columns.Add("Vists", "Visits");

            nr = travelhistoryctrl.visitedSystems.Count;

            var groupeddata = from data in travelhistoryctrl.visitedSystems
                              group data by data.Name
                                  into grouped
                                  select new
                                  {
                                      Title = grouped.Key,
                                      Count = grouped.Count()
                                  };
            nr = 0;
            foreach (var data in from a in groupeddata orderby a.Count descending select a)
            {
                if (data.Count <= 1)
                    break;

                object[] rowobj = new object[] { data.Title, data.Count};
                dataGridView1.Rows.Add(rowobj);
                //LogText(data.Title + "\t Count=" + data.Count + Environment.NewLine);
            }

        }



        private void chart1_Click(object sender, EventArgs e)
        {

        }

    }
}
