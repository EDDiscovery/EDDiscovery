using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string[] cmdlineopts = Environment.GetCommandLineArgs().ToArray();

            string selection = cmdlineopts.Length>1 ? cmdlineopts[1].ToLower() : "default";

            Form sel;

            switch( selection )
            {
                default:
                case "default":
                case "testform":
                    sel = new TestForm();
                    break;

                case "testrolluppanel":
                    sel = new TestRollUpPanel();
                    break;

                case "testautocomplete":
                    sel = new TestAutoComplete();
                    break;

                case "testselectionpanel":
                    sel = new TestSelectionPanel();
                    break;

                case "keyform":
                    ExtendedControls.KeyForm f = new ExtendedControls.KeyForm();
                    f.Init(null, true, " ", "", "KeyLogger", new List<string>() { "{1}", "{2}" }, 100, false);
                    sel = f;
                    break;
            }

            Application.Run(sel);
        }
    }
}
