using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using static BaseUtils.UnitTests.CheckerHelpers;

namespace UnitTest
{
    public static class UnitTestBindingsFile
    {
        [BaseUtils.UnitTests.Test(1000)]
        public static void TestBindingsFile()
        {
            CheckSection("Bindings");

            // the short test one
            string folder = $@"..\..\..\UnitTest\Bindings\";

            {
                File.WriteAllText(Path.Combine(folder, "StartPreset.4.Start"), "KeyboardMouseOnly\r\nKeyboardMouseOnly\r\nCustom\r\nKeyboardMouseOnly\r\n");
                string file = BindingsFile.FindBindingsFile(folder, true);
                CheckThat(file).Contains("Custom.4.2.binds");

                File.WriteAllText(Path.Combine(folder, "StartPreset.4.Start"), "KeyboardMouseOnly\r\nKeyboardMouseOnly\r\nKeyboardMouseOnly\r\nRobDirect\r\n");
                file = BindingsFile.FindBindingsFile(folder, true);
                CheckThat(file).Contains("RobDirect.4.2.binds");
            }
            {
                File.WriteAllText(Path.Combine(folder, "StartPreset.4.Start"), "Test1\r\nTest1\r\nTest1\r\nTest1\r\n");
                string file = BindingsFile.FindBindingsFile(folder, true);
                BindingsFile f = new BindingsFile();
                f.Read(file);
                CheckThat(f.FileName).IsNotNull();
                string xml = f.ToXML();
                CompareXML(xml, f.FileName);
            }

            File.WriteAllText(Path.Combine(folder, "StartPreset.4.Start"), "RobDirect\r\nRobDirect\r\nRobDirect\r\nRobDirect\r\n");

            // the longer full one

            //{
            //    File.WriteAllText(Path.Combine(folder, "StartPreset.4.Start"), "RobDirect\r\nRobDirect\r\nRobDirect\r\nRobDirect\r\n");
            //    string file = BindingsFile.FindBindingsFile(folder, true);
            //    BindingsFile f = new BindingsFile();
            //    f.Read(file);
            //    CheckThat(f.FileName).Contains("RobDirect.4.2.binds");
            //    CompareXML(f.ToXML(), f.FileName);
            //}
        }

        public static void CompareXML(string xml, string file)
        {
            string[] xmloutput = xml.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string[] comparefile = File.ReadAllLines(file);
            bool ok = true;
            for (int i = 0; i < comparefile.Length; i++)
            {
                if (xmloutput.Length > i)
                {
                    if (xmloutput[i].Trim() != comparefile[i].Trim()) // need the trim, hence why its here
                    {
                        System.Diagnostics.Debug.WriteLine($"Difference line {i + 1} {xmloutput[i].Trim()} vs {comparefile[i].Trim()}");
                        ok = false;
                        break;
                    }
                }
                else
                    ok = false;
            }
            CheckThat(ok).IsTrue();
        }
    }
}
