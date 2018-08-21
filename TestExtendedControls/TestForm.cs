using EDDiscovery2;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogTest
{
    public partial class TestForm : Form
    {
        Color Multiply(Color from, float m) { return Color.FromArgb(from.A, (byte)((float)from.R * m), (byte)((float)from.G * m), (byte)((float)from.B * m)); }

        List<EDDiscovery2.EDCommander> commanders = null;
        List<EDDiscovery2.EDCommander> commanders2 = null;

        public static string Underscore(string pascalCasedWord)
        {
            return Regex.Replace(
                Regex.Replace(
                    Regex.Replace(pascalCasedWord, @"([A-Z]+)([A-Z][a-z])", "$1 $2"), @"([a-z\d])([A-Z])",
                    "$1 $2"), @"[-\s]", " ");
        }

        public static string Titleize(string word)
        {
            return Regex.Replace(Humanize(Underscore(word)), @"\b([a-z])",
                                 delegate (Match match)
                                 {
                                     return match.Captures[0].Value.ToUpperInvariant();
                                 });
        }

        public static string Humanize(string lowercaseAndUnderscoredWord)
        {
            return Capitalize(Regex.Replace(lowercaseAndUnderscoredWord, @"_", " "));
        }

        public static string Capitalize(string word)
        {
            return word.Substring(0, 1).ToUpperInvariant() + word.Substring(1).ToLowerInvariant();
        }


        public TestForm()
        {
            InitializeComponent();
            //            TabControlCustom tabControl1 = this;
            tabControl1.TabStyle = new TabStyleAngled();
            tabControl1.FlatStyle = FlatStyle.Flat;
          //  tabControl1.Alignment = TabAlignment.Bottom;
            tabControl1.Font = new Font("Euro Caps", 12.0F);

            //tabControl1.MinimumTabWidth = 100;

            Console.WriteLine(Underscore("ThisAndThat"));
            Console.WriteLine(Underscore("FSDJump"));


            commanders = new List<EDDiscovery2.EDCommander>();
            commanders.Add(new EDCommander(-1, "Hidden log", ""));
            commanders.Add(new EDCommander(1, "Robby1", ""));
            commanders.Add(new EDCommander(2, "Robby2", ""));
            commanders.Add(new EDCommander(3, "Robby3", ""));
            commanders.Add(new EDCommander(4, "Robby4", ""));
            commanders.Add(new EDCommander(6, "Robby6", ""));
            commanders.Add(new EDCommander(7, "Robby7", ""));
            commanders.Add(new EDCommander(8, "Robby8", ""));
            commanders.Add(new EDCommander(9, "Robby9", ""));
            commanders.Add(new EDCommander(10, "Robby10", ""));
            commanders.Add(new EDCommander(11, "Robby11", ""));

            comboBoxCustom1.DataSource = commanders;
            comboBoxCustom1.DisplayMember = "Name";
            comboBoxCustom1.ValueMember = "Nr";
            comboBoxCustom1.FlatStyle = FlatStyle.Popup;
            comboBoxCustom1.Repaint();


            commanders2 = new List<EDDiscovery2.EDCommander>();
            commanders2.Add(new EDCommander(-1, "2Hidden log", ""));
            commanders2.Add(new EDCommander(1, "2Robby1", ""));
            commanders2.Add(new EDCommander(2, "2Robby2", ""));
            commanders2.Add(new EDCommander(3, "2Robby3", ""));
            commanders2.Add(new EDCommander(4, "2Robby4", ""));
            commanders2.Add(new EDCommander(6, "2Robby6", ""));
            commanders2.Add(new EDCommander(7, "2Robby7", ""));
            commanders2.Add(new EDCommander(8, "2Robby8", ""));
            commanders2.Add(new EDCommander(9, "2Robby9", ""));
            commanders2.Add(new EDCommander(10, "2Robby10", ""));
            commanders2.Add(new EDCommander(11, "2Robby11", ""));


            comboBoxCustom2.DataSource = commanders2;
            comboBoxCustom2.DisplayMember = "Name";
            comboBoxCustom2.ValueMember = "Nr";
            comboBoxCustom2.FlatStyle = FlatStyle.System;
            comboBoxCustom2.Repaint();


            listControlCustom1.Items.Add("one");
            listControlCustom1.Items.Add("two");
            listControlCustom1.Items.Add("three");
            listControlCustom1.Items.Add("four");
            listControlCustom1.Items.Add("five");
            listControlCustom1.Items.Add("six");
            listControlCustom1.Items.Add("seven");
            listControlCustom1.Items.Add("eight");
            listControlCustom1.Items.Add("nine");
            listControlCustom1.FlatStyle = FlatStyle.Popup;
            listControlCustom1.ItemSeperators = new int[] { 1, 4 };

            listControlCustom2.Items = listControlCustom1.Items;
            listControlCustom2.FlatStyle = FlatStyle.System;
            listControlCustom2.ItemSeperators = new int[] { 1, 4 };

            richTextBoxScroll1.ScrollBarFlatStyle = FlatStyle.Popup;
            richTextBoxScroll1.HideScrollBar = true;

            dataGridView1.Rows.Clear();

            for( int i = 0; i < 100; i++ )
            {
                string cg1 = "r" + i + " c1";
                string cg2 = "r" + i + " c2";
                string cg3 = "r" + i + " c3";
                string cg4 = "r" + i + " c4";
                string cg5 = "r" + i + " c5";
                object[] rowobj = { cg1, cg2, cg3, cg4, cg5 };
                int rc = dataGridView1.RowCount-1;
                dataGridView1.Rows.Add(rowobj);
                dataGridView1.Rows[rc].HeaderCell.Value = i.ToString();

               // dataGridView1.Rows[rc].HeaderCell.Style.ForeColor = (i % 2 == 0) ? Color.Red : Color.White;
                //dataGridView1.Rows[rc].HeaderCell.Style.BackColor = (i % 2 == 0) ? Color.White : Color.Red;
            }


            Font fnt1 = new Font("Euro Caps", 12F);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = fnt1;
            dataGridView1.RowHeadersDefaultCellStyle.Font = fnt1;
            dataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.Blue;
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.White;

            numericUpDownCustom1.AutoSize = true;
            numericUpDownCustom1.Minimum = -100;

            autoCompleteTextBox1.SetAutoCompletor(AutoList);
            autoCompleteTextBox2.SetAutoCompletor(AutoList);
            autoCompleteTextBox2.FlatStyle = FlatStyle.Flat;
            autoCompleteTextBox2.Invalidate();

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            dateTimePicker1.BackColor = Color.Blue;
            dateTimePicker1.SelectedColor = Color.DarkBlue;

            textBoxDouble1.FormatCulture = CultureInfo.GetCultureInfo("en-gb");
            textBoxDouble2.FormatCulture = CultureInfo.GetCultureInfo("fr");
            numberBoxLong0.FormatCulture = CultureInfo.GetCultureInfo("en-gb");

            numberBoxLong0.Minimum = 1000;
            numberBoxLong0.Maximum = 2000;
            numberBoxLong0.ValueNoChange = 1500;

            numberBoxLong1.Minimum = 1000;
            numberBoxLong1.Maximum = 2000;
            numberBoxLong1.ValueNoChange = 1400;
            numberBoxLong1.SetComparitor(numberBoxLong0,2);

            textBoxDouble1.Minimum = -20.0;
            textBoxDouble1.Maximum = 20.0;

            textBoxDouble2.Minimum = 10.0;
            textBoxDouble2.Maximum = 20.0;
        }


        private void button11_Click(object sender, EventArgs e)
        {
            tabControl1.Enabled = !tabControl1.Enabled;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            FlatStyle fs = tabControl1.FlatStyle;
            if (fs == FlatStyle.System)
                tabControl1.FlatStyle = FlatStyle.Popup;
            if (fs == FlatStyle.Popup)
                tabControl1.FlatStyle = FlatStyle.Flat;
            if (fs == FlatStyle.Flat)
                tabControl1.FlatStyle = FlatStyle.System;


        }

        int tabstyle = 0;
        private void button13_Click(object sender, EventArgs e)
        {
            if (tabstyle == 0)
               tabControl1.TabStyle = new ExtendedControls.TabStyleAngled();
            if (tabstyle == 1)
                tabControl1.TabStyle = new ExtendedControls.TabStyleRoundedEdge();
            if (tabstyle == 2)
                tabControl1.TabStyle = new ExtendedControls.TabStyleSquare();

            tabstyle = (tabstyle + 1) % 3;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Font fnt = tabControl1.Font;
            
            if (fnt.Name.Equals("Euro Caps"))
                fnt = new Font("Microsoft Sans Serif", 8.0F);
            else
                fnt = new Font("Euro Caps", 12.0F);

            tabControl1.Font = fnt;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            commanders = new List<EDDiscovery2.EDCommander>();
            commanders.Add(new EDCommander(-1, "Hidden log", ""));
            commanders.Add(new EDCommander(1, "1Robby", ""));
            commanders.Add(new EDCommander(3, "3Fred", ""));
            commanders.Add(new EDCommander(4, "4Jim", ""));
            commanders.Add(new EDCommander(5, "5Jim", ""));
            commanders.Add(new EDCommander(6, "6Jim", ""));
            commanders.Add(new EDCommander(7, "7Jim", ""));
            commanders.Add(new EDCommander(8, "8Jim", ""));
            commanders.Add(new EDCommander(9, "9Jim", ""));

            comboBoxCustom1.DataSource = commanders;
            comboBoxCustom1.DisplayMember = "Name";
            comboBoxCustom1.ValueMember = "Nr";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (groupBox1.FlatStyle == FlatStyle.System)
                groupBox1.FlatStyle = FlatStyle.Flat;
            else if (groupBox1.FlatStyle == FlatStyle.Flat)
                groupBox1.FlatStyle = FlatStyle.Popup;
            else if (groupBox1.FlatStyle == FlatStyle.Popup)
                groupBox1.FlatStyle = FlatStyle.Standard;
            else if (groupBox1.FlatStyle == FlatStyle.Standard)
                groupBox1.FlatStyle = FlatStyle.System;

            button1.Text = groupBox1.FlatStyle.ToString();
            groupBox1.Invalidate();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.BackColor = Color.DarkBlue;
            groupBox1.FlatStyle = FlatStyle.Flat;
            groupBox1.Font = new Font("Euro Caps", 12F);
        }

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            VScrollBar vs = sender as VScrollBar;
            Console.WriteLine("Vscroll " + vs.Name + " " + vs.Value);
        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Console.WriteLine("Form1:Scroll bar " + e.NewValue + " from " + e.OldValue);
        }

        private void vScrollBarCustom1_ValueChanged_1(object sender, EventArgs e)
        {
            Console.WriteLine("valuechanged custom at " + vScrollBarCustom1.Value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (vScrollBarCustom1.Maximum != 5)
                vScrollBarCustom1.SetValueMaximumLargeChange(0, 5, 10);
            else
                vScrollBarCustom1.SetValueMaximumLargeChange(0, 10, 10);

            if (vScrollBarCustom2.Maximum != 5)
                vScrollBarCustom2.SetValueMaximumLargeChange(0, 5, 10);
            else
                vScrollBarCustom2.SetValueMaximumLargeChange(0, 10, 10);
        }

        int count = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            if ((count % 10) == 5)
            {
                richTextBoxScroll1.AppendText("Add this very long wrapping line on " + count + Environment.NewLine, (count % 2 == 0) ? Color.Red : Color.Blue);
                count++;
            }
            else
                richTextBoxScroll1.AppendText("Add this on " + count + Environment.NewLine, (count % 2 == 0) ? Color.Red : Color.Blue);
            count++;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listControlCustom1.Focus();
        }

        private void vScrollBarCustom1_ScrollSYS(object sender, ScrollEventArgs e)
        {
            Console.WriteLine("SSYS" + e.NewValue + " " + e.OldValue);
        }

        private void textBoxBorder1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        int addindex = 1000;
        private void button6_Click(object sender, EventArgs e)
        {
            string cg1 = "r" + addindex + " c1";
            string cg2 = "r" + addindex + " c2";
            string cg3 = "r" + addindex + " c3";
            string cg4 = "r" + addindex + " c4";
            string cg5 = "r" + addindex + " c5";
            addindex++;
            object[] rowobj = { cg1, cg2, cg3, cg4, cg5 };
            dataGridView1.Rows.Add(rowobj);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(0);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBoxScroll1.Clear();
        }

        private void upDown1_Selected(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Up/Down " + e.Delta);
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDownCustom1_Leave(object sender, EventArgs e)
        {
            Console.WriteLine("Leave Custom numeric");
        }

        private void numericUpDownCustom1_ValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Value changed Custom numeric " + numericUpDownCustom1.Value );

        }

        private void button15_Click(object sender, EventArgs e)
        {
            Control ctl = drawnPanel1;
            ctl.Enabled = !ctl.Enabled;
        }

        
        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //Console.WriteLine("PKD key " + e.KeyCode.ToString());
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //Console.WriteLine("PCK key " + keyData.ToString());

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine("KD key " + e.KeyCode.ToString());
        }

        private void button16_Click(object sender, EventArgs e)
        {
            for (int l = 2; l < 5; l++)
            {
                dataGridView1.Rows[l].Visible =!dataGridView1.Rows[l].Visible;
            }

            HoldingPanel.UpdateScroll();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public static List<string> AutoList(string input, AutoCompleteTextBox t)
        {
            List<string> f = new List<string>();
            f.Add("one");
            f.Add("two");
            f.Add("three");
            f.Add("four");
            f.Add("five");
            f.Add("Aone");
            f.Add("Btwo");
            f.Add("Cthree");
            f.Add("Dfour");
            f.Add("Efive");
            return f;
        }

        CheckedListControlCustom cc;

        private void button17_Click(object sender, EventArgs e)
        {
            if (cc == null)
            {
                Button b = sender as Button;
                cc = new CheckedListControlCustom();
                cc.Items.Add("One");
                cc.Items.Add("Two");
                cc.Items.Add("Three four five six seven eight nine ten");
                cc.Items.Add("Four");
                cc.FormClosed += FormClosed2;
                cc.PositionBelow(b, new Size(b.Width, 400));
                cc.Show();
            }
            else
                cc.Close();
        }

        private void FormClosed2(Object sender, FormClosedEventArgs e )
        {
            cc = null;
        }

        private void checkBoxCustom2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            System.Diagnostics.Debug.WriteLine("Checked " + cb.Checked);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("P3 changed");


        }

        private void textBoxLong1_ValueChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Long is " + numberBoxLong0.Value);
        }
    }
}
