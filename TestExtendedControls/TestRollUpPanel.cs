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
    public partial class TestRollUpPanel : Form
    {
        public TestRollUpPanel()
        {
            InitializeComponent();
            rolluppanel.HiddenMarkerWidth = -100;
            rolluppanel.SetPinState(true);
            comboBoxCustom1.Items.AddRange(new string[] { "1Fred", "Jim", "Sheila", "George" });
            comboBoxCustom1.DropDownBackgroundColor = Color.Red;
            comboBoxCustom1.FlatStyle = FlatStyle.Popup;
            comboBoxCustom1.Repaint();
            comboBoxCustom2.Items.AddRange(KeyObjectExtensions.KeyListString(true));
            comboBoxCustom2.SelectedItem = Keys.ShiftKey.VKeyToString();
            //comboBoxCustom1.FlatStyle = FlatStyle.Popup;

            System.Drawing.Imaging.ColorMap colormap = new System.Drawing.Imaging.ColorMap();       // any drawn panel with drawn images    
            colormap.OldColor = Color.White;                                                        // white is defined as the forecolour
            colormap.NewColor = Color.Orange;
            System.Drawing.Imaging.ColorMap colormap2 = new System.Drawing.Imaging.ColorMap();       // any drawn panel with drawn images    
            colormap2.OldColor = Color.FromArgb(222,222,222);                                                        // white is defined as the forecolour
            colormap2.NewColor = Color.Orange.Multiply(0.8F);
            foreach ( Control c in rolluppanel.Controls )
            {
                if (c is ExtendedControls.CheckBoxCustom)
                    (c as ExtendedControls.CheckBoxCustom).SetDrawnBitmapRemapTable(new System.Drawing.Imaging.ColorMap[] { colormap, colormap2 });
                else if (c is ExtendedControls.ButtonExt)
                    (c as ExtendedControls.ButtonExt).SetDrawnBitmapRemapTable(new System.Drawing.Imaging.ColorMap[] { colormap, colormap2 });
            }

            KeyObjectExtensions.VerifyKeyOE();
        }

        private void TestRollUpPanel_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Key down " + e.KeyCode + " " + e.KeyData);

        }
    }
}
