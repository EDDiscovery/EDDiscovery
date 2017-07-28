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
    public partial class TestAutoComplete : Form
    {
        public TestAutoComplete()
        {
            InitializeComponent();
            autoCompleteTextBox1.SetAutoCompletor(AutoList);
        }

        public static List<string> AutoList(string input, AutoCompleteTextBox t)
        {
            List<string> f = new List<string>();
            f.Add("one");
            f.Add("only");
            f.Add("onynx");
            f.Add("two");
            f.Add("three");
            f.Add("four");
            f.Add("five");
            f.Add("Aone");
            f.Add("Btwo");
            f.Add("Cthree");
            f.Add("Dfour");
            f.Add("Efive");

            List<string> res = (from x in f where x.StartsWith(input, StringComparison.InvariantCultureIgnoreCase) select x).ToList();
            return res;
        }

    }
}
