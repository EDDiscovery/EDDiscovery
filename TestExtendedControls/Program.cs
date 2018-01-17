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
                    f.Init(null, true, " ", "", "", -1, false);
                    sel = f;
                    break;

                case "infoform":
                    ExtendedControls.ThemeStandard th = new ExtendedControls.ThemeStandard();
                    th.LoadBaseThemes();
                    th.SetThemeByName("Elite Verdana");
                    ExtendedControls.ThemeableFormsInstance.Instance = th;
                    ExtendedControls.InfoForm inf = new ExtendedControls.InfoForm();
                    inf.Info("Info form", Properties.Resources._3x3_grid, "This is a nice test\r\nOf the info form\r\n", null, new int[] { 0, 100, 200, 300, 400, 500, 600 }, true);
                    sel = inf;
                    break;

                case "testtabstrip":
                    sel = new TestTabStrip();
                    break;

                case "testdirectinput":
                    sel = new TestDirectInput();
                    break;

                case "testtabcontrolcustom":
                    sel = new TestTabControlCustom();
                    break;

                case "testjournalread":
                    sel = new TestJournalRead();
                    break;
            }


            Application.Run(sel);
        }

    }
}
