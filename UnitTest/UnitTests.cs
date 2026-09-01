using BaseUtils;
using DirectInputDevices;
using EliteDangerousCore;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace UnitTest
{
    public partial class UnitTests : Form
    {
        ThemeList theme;
        public UnitTests()
        {
            InitializeComponent();
        }

        [System.Diagnostics.DebuggerHidden()]

        private void Log(string x, Font fnt = null)
        {
            if (fnt != null)
                richTextBoxLog.SelectionFont = fnt;
            richTextBoxLog.AppendText(x);
            richTextBoxLog.AppendText(Environment.NewLine);
            richTextBoxLog.Select(richTextBoxLog.Text.Length, richTextBoxLog.Text.Length);
            richTextBoxLog.ScrollToCaret();
          //  System.Diagnostics.Debug.WriteLine($"UnitTest Log : {x}");
            Application.DoEvents();
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

            BaseUtils.UnitTests.Check.TestResult = Test;            // hook up responders to checkers
            BaseUtils.UnitTests.Check.NewSection = Section;

            // all test marked with 
            tests = BaseUtils.UnitTests.Check.GetTests(Assembly.GetExecutingAssembly());

            theme = new ThemeList();
            theme.LoadBaseThemes();
            theme.SetThemeByName("Elite Verdana Small");
            //Theme.Current.WindowsFrame = true;
            Theme.Current.ApplyStd(this);

            timer.Tick += T_Tick;

            {
                foreach( InputLanguage x in InputLanguage.InstalledInputLanguages)
                {
                    System.Diagnostics.Debug.Write($"Tuple.Create({x.LayoutName.AlwaysQuoteString()},{x.Culture.Name.AlwaysQuoteString()}),");
                }
                System.Diagnostics.Debug.WriteLine("");
                System.Diagnostics.Debug.WriteLine($"Input lang {InputLanguage.CurrentInputLanguage.LayoutName} {InputLanguage.CurrentInputLanguage.Culture.Name}");
            }

            string cmdline = Environment.CommandLine;
            if (cmdline.ContainsIIC("Binding"))
            {
                InputDeviceList inputdevices = new DirectInputDevices.InputDeviceList();
                InputDeviceJoystickWindows.CreateJoysticks(inputdevices);

                List<string> devices = new List<string>();
                foreach (var device in inputdevices)
                {
                    System.Diagnostics.Debug.WriteLine($"{device.ID.Name} {device.ID.VendorId} {device.ID.ProductId} {device.ID.VendorProductId}");
                    
                    // does frontier know about it?
                    string bestname = BindingsFile.FrontierDeviceName(device.ID.ProductId, device.ID.VendorId);

                    if (bestname != null)           // if frontier knows it, add its name, else add usb identity which frontier appears to use
                        devices.Add(bestname);
                    else
                        devices.Add(device.ID.VendorProductId);

                    bindingsEditor.ConvertDeviceNameList[device.ID.VendorProductId] = device.ID.Name;       // allow the productvendorid pair to be converted to device name just for the bindings editor
                }

                bindingsEditor.ConvertDeviceNameList["{NoDevice}"] = "---";

                InputDeviceKeyboard.CreateKeyboard(inputdevices);              
                InputDeviceMouse.CreateMouse(inputdevices);

                inputdevices.Start();

                panelTest.Visible = false;
                bindingsEditor.Dock = DockStyle.Fill;

                bindingsEditor.DeviceInput += (bf,entry) =>
                {
                    InputMapDialog im = new InputMapDialog();
                    im.Init(inputdevices);
                    im.AllowAxis = im.AxisOnly= entry.IsBinding;
                    im.ShowPressOrRelease = false;
                    im.ShowOKCancel = false;
                    im.EscapeQuits = true;
                    Theme.Current.ApplyDialog(im);
                    if (im.ShowDialog(this) == DialogResult.OK)
                    {
                        string devicename = im.Device.Name;
                        if ( !bindingsEditor.DevicesNamesConverted.Contains(devicename))
                        {
                            // same way its done in operation to map to a frontier device
                            string bestname = bindingsEditor.FindDevice(im.Device.Name, im.Device.ID.Instanceguid, im.Device.ID.Productguid, im.Device.ID.ProductId, im.Device.ID.VendorId);
                            if (bestname == null)
                                ExtendedControls.MessageBoxTheme.Show($"Cannot find frontier device name for device\r\nUse Frontier editor to add device first", "Cannot find device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            devicename = bestname;
                        }

                        if (devicename != null)
                        {
                            string frontiername = im.Device.Name == "Keyboard" ? FrontierKeyConversion.KeysToFrontier(bf.KeyboardLayout, im.KeyName) : im.KeyName;

                            if (!frontiername.StartsWith("!"))
                            {
                                BindingsFile.DeviceKeyPair dvp = new BindingsFile.DeviceKeyPair(devicename, frontiername);
                                return dvp;
                            }
                            else
                                ExtendedControls.MessageBoxTheme.Show($"Cannot find mapping to key name", "Cannot find device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    return null;
                };

                //string folder = @"c:\code\eddiscovery\unittest\bindings";
                string folder = @"C:\Users\RK\AppData\Local\Frontier Developments\Elite Dangerous\Options\Bindings";
                var frontierpresetfilebindingfilename = EliteDangerousCore.BindingsFile.FindBindingsFile(folder, true);
                bindingsEditor.Init(folder, frontierpresetfilebindingfilename, devices);
            }
            else
            {
                bindingsEditor.Visible = false;
                panelTest.Dock = DockStyle.Fill;
                buttonStart_Click(null, null);
            }
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            timer.Start();
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

                int time = AppTicks.TickCountLapDelta("UnitTests").Item2;

                Section("?");

                Log($"Completed {testset+1}:{tests[testset].Name} in {time}ms Totals failed {testfailures} out of {totaltests} tests");
                

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
            if (testno > 0 )
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


    }

}

