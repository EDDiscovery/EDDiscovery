using BaseUtils;
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace UnitTest
{
    public partial class UnitTests : Form
    {
        public UnitTests()
        {
            InitializeComponent();
        }

        [System.Diagnostics.DebuggerHidden()]

        private void Log(string x, Font fnt = null)
        {
            if (fnt != null)
                richTextBox1.SelectionFont = fnt;
            richTextBox1.AppendText(x);
            richTextBox1.AppendText(Environment.NewLine);
            richTextBox1.Select(richTextBox1.Text.Length, richTextBox1.Text.Length);
            System.Diagnostics.Debug.WriteLine($"UnitTest Log : {x}");
            Application.DoEvents();
        }

        class EliteConfig : IEliteConfig
        {
            public WebExternalDataLookup WebLookup => WebExternalDataLookup.None;
            public DateTime ConvertTimeToSelectedFromUTC(DateTime t)
            {
                return t;
            }
        }

        class EliteOptions : IEliteOptions
        {
            public string SystemDatabasePath => throw new NotImplementedException();

            public string UserDatabasePath => throw new NotImplementedException();

            public bool ForceBetaOnCommander => throw new NotImplementedException();

            public bool DisableJournalMerge => throw new NotImplementedException();

            public bool DisableJournalRemoval => throw new NotImplementedException();

            public bool DisableBetaCommanderCheck => throw new NotImplementedException();

            public string ScanCachePath => throw new NotImplementedException();

            public bool ScanCacheEnabled => throw new NotImplementedException();

            public bool SetEDDNforNewCommanders => throw new NotImplementedException();
        }


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            EDDiscovery.Icons.ForceInclusion.Include();      // Force the assembly into the project by a empty call
            BaseUtils.Icons.IconSet.CreateSingleton();
            System.Reflection.Assembly iconasm = BaseUtils.ResourceHelpers.GetAssemblyByName("EDDiscovery.Icons");
            BaseUtils.Icons.IconSet.Instance.LoadIconsFromAssembly(iconasm);
            BaseUtils.Icons.IconSet.Instance.AddAlias("settings", "Controls.Settings");             // from use by action system..
            BaseUtils.Icons.IconSet.Instance.AddAlias("missioncompleted", "Journal.MissionCompleted");
            BaseUtils.Icons.IconSet.Instance.AddAlias("speaker", "Legacy.speaker");
            BaseUtils.Icons.IconSet.Instance.AddAlias("Default", "Legacy.star");        // MUST be present

            MaterialCommodityMicroResourceType.Initialise();
            ItemData.Initialise();
            Stars.Prepopulate();                                // we do it this way instead of statically because we don't want them autofilled
            Planets.Prepopulate();

            EliteConfigInstance.InstanceOptions = new EliteOptions();
            EliteConfigInstance.InstanceConfig = new EliteConfig();

            timer.Tick += T_Tick;
            timer.Start();

            BaseUtils.UnitTests.Check.TestResult = Test;            // hook up responders to checkers
            BaseUtils.UnitTests.Check.NewSection = Section;

            // all test marked with 
            tests = BaseUtils.UnitTests.Check.GetTests(Assembly.GetExecutingAssembly());

            
            Log("Begin");
        }

        int testset = 0;
        int testno = 0;
        int testfailures = 0;
        int totaltests = 0;
        int totalfailures = 0;
        string section = "";
        Timer timer = new Timer() { Interval = 100 };
        List<MethodInfo> tests;

        private void T_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            if (testset < tests.Count)
            {
                Log($"Execute {testset+1}:{tests[testset].Name}");
                testno = 0;
                testfailures = 0;
                section = "?";

                AppTicks.TickCountLapDelta("UnitTests", true);

                try
                {
                    tests[testset].Invoke(null, new Object[] { });
                }
                catch(Exception ex)
                {
                    Log($"!!Exception running test {testset+1}:{tests[testset].Name}" + Environment.NewLine + ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace);
                    testfailures++;
                }

                Section("?");

                int time = AppTicks.TickCountLapDelta("UnitTests").Item2;

                Log($"Completed {testset+1}:{tests[testset].Name} in {time}ms Failures {testfailures}/{testno}");

                testset++;
                timer.Start();
            }
            else
            {
                Log("");
                if ( totalfailures == 0 )
                    Log($"Finished. Success tests {totaltests}", new Font("Arial", 14));
                else
                    Log($"Finished. Failed {totalfailures}/{totaltests}", new Font("Arial", 14));

            }
        }

        private void Section(string newsection)
        {
            if (testno > 0)
            {
                totaltests += testno;
                totalfailures += testfailures;
                Log($"       Completed Section {section}: Failures {testfailures}/{testno}");
            }

            testno = 0;
            testfailures = 0;
            section = newsection;
        }

        public void Test( bool passed, string msg = "")
        {
            testno++;
            if (!passed)
            {
                testfailures++;
                Log($"...Failed Test {section}:{testno} {msg}");
            }
        }

        public static bool CheckEvent<T>(string s)
        {
            System.Diagnostics.Debug.WriteLine("Event : " + s);  
            var je = JournalEntry.CreateJournalEntry(s);
            bool ret = je != null && je is T;
            return ret;
        }

    }

}

