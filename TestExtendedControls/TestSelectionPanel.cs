using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogTest
{
    public partial class TestSelectionPanel : Form
    {
        static List<string> list = new List<string>();

        public TestSelectionPanel()
        {
            InitializeComponent();
            panelSelectionList1.Items = new List<string>() { "One", "two", "three" };
            panelSelectionList1.SelectedIndexChanged += PanelSelectionList1_SelectedIndexChanged;

            comboBoxCustom1.Items = new List<string>() { "One", "two", "three" , "four" };
            comboBoxCustom1.Repaint();

            list.Add("one");
            list.Add("only");
            list.Add("onynx");
            list.Add("two");
            list.Add("three");
            list.Add("four");
            list.Add("five");
            list.Add("Aone");
            list.Add("Btwo");
            list.Add("Cthree");
            list.Add("Dfour");
            list.Add("Efive");

            autoCompleteTextBox1.SetAutoCompletor(AutoList);

        }

        private void PanelSelectionList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Selected " + panelSelectionList1.SelectedIndex);
        }

        public static List<string> AutoList(string input, AutoCompleteTextBox t)
        {
            List<string> res = (from x in list where x.StartsWith(input, StringComparison.InvariantCultureIgnoreCase) select x).ToList();
            return res;
        }

    }
}
