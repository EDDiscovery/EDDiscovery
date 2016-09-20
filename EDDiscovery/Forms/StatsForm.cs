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
        public EDDiscoveryForm _discoveryForm;

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

            LogText("Total No of jumps: " + _discoveryForm.history.GetFSDJumps(new TimeSpan(10000, 0, 0, 0)) + Environment.NewLine);
            LogText("Last 24 hours: " + _discoveryForm.history.GetFSDJumps(new TimeSpan(1, 0, 0, 0)) + Environment.NewLine);
            LogText("Last Week: " + _discoveryForm.history.GetFSDJumps(new TimeSpan(7, 0, 0, 0)) + Environment.NewLine);
            LogText("Last 30 days: " + _discoveryForm.history.GetFSDJumps(new TimeSpan(30, 0, 0, 0)) + Environment.NewLine);
            LogText("Last year: " + _discoveryForm.history.GetFSDJumps(new TimeSpan(365, 0, 0, 0)) + Environment.NewLine);
        }


        private void StatMostVisits()
        {
            dataGridView1.Visible = true;

            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            dataGridView1.Columns.Add("Name", "System");
            dataGridView1.Columns.Add("Vists", "Visits");

            //nr = _discoveryForm.history.GetFSDJumps(new TimeSpan(10000, 0, 0, 0));

            var groupeddata = from data in _discoveryForm.history.OrderByDate
                              group data by data.System.name
                                  into grouped
                                  select new
                                  {
                                      Title = grouped.Key,
                                      Count = grouped.Count()
                                  };
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
