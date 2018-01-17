using EliteDangerousCore;
using Newtonsoft.Json.Linq;
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
    public partial class TestJournalRead : Form
    {
        public TestJournalRead()
        {
            InitializeComponent();
            textBoxFile.Text = "C:\\work\\json.txt";
        }

        private void buttonExt1_Click(object sender, EventArgs e)
        {
            System.IO.StreamReader filejr = new System.IO.StreamReader(textBoxFile.Text);
            string line;
            string system = "";
            StarScan ss = new StarScan();

            Dictionary<string, string> items = new Dictionary<string, string>();

            while ((line = filejr.ReadLine()) != null)
            {
                if (line.Equals("END"))
                    break;
                //System.Diagnostics.Trace.WriteLine(line);

                if (line.Length > 0)
                {
                    JObject jo = (JObject)JObject.Parse(line);

                    //JSONPrettyPrint jpp = new JSONPrettyPrint(EliteDangerous.JournalFieldNaming.StandardConverters(), "event;timestamp", "_Localised", (string)jo["event"]);
                    //string s = jpp.PrettyPrintStr(line, 80);
                    //System.Diagnostics.Trace.WriteLine(s);

                    EliteDangerousCore.JournalEntry je = EliteDangerousCore.JournalEntry.CreateJournalEntry(line);
                    //System.Diagnostics.Trace.WriteLine(je.EventTypeStr);

                    if (je.EventTypeID == EliteDangerousCore.JournalTypeEnum.Location)
                    {
                        EliteDangerousCore.JournalEvents.JournalLocOrJump jl = je as EliteDangerousCore.JournalEvents.JournalLocOrJump;
                        system = jl.StarSystem;
                    }
                    else if (je.EventTypeID == EliteDangerousCore.JournalTypeEnum.FSDJump)
                    {
                        EliteDangerousCore.JournalEvents.JournalFSDJump jfsd = je as EliteDangerousCore.JournalEvents.JournalFSDJump;
                        system = jfsd.StarSystem;

                    }
                    else if (je.EventTypeID == EliteDangerousCore.JournalTypeEnum.Scan)
                    {
                        //ss.Process(je as EliteDangerousCore.JournalEvents.JournalScan, new EliteDangerousCore.SystemClass(system));
                    }
                }
            }

        }
    }
}
