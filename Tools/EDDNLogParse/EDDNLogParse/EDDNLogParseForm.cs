using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDNLogParse
{
    public partial class EDDNLogParseForm : Form
    {
        private string appdatapath;

        public EDDNLogParseForm()
        {
            InitializeComponent();

            appdatapath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EDDNRecorder");
            if (!Directory.Exists(appdatapath))
                Directory.CreateDirectory(appdatapath);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
