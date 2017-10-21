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
    public partial class TestTabControlCustom : Form
    {
        public TestTabControlCustom()
        {
            InitializeComponent();
            tabControlCustom1.FlatStyle = FlatStyle.Popup;
            tabControlCustom1.TabStyle = new TabStyleAngled();
            tabControlCustom1.AllowDragReorder = true;
        }
    }
}
