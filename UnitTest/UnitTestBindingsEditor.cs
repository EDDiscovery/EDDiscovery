using BaseUtils;
using DirectInputDevices;
using EliteDangerousCore;
using EliteDangerousCore.Bindings;
using ExtendedControls;
using QuickJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace UnitTest
{
    public partial class UnitTestBindingsEditor : UserControl
    {
        public UnitTestBindingsEditor()
        {
            InitializeComponent();
        }

        public void Init()
        {
            string pname = @"C:\Games\Elite\Products\elite-dangerous-odyssey-64\EliteDangerous64.exe";
            FrontierDeviceMapping.ReadFrontierDeviceMappingFile(pname);

            inputdevices = new DirectInputDevices.InputDeviceList();
            inputdevices.AddDevices(InputDeviceIdentity.DeviceClass.Controllers);
            inputdevices.AddDevices(InputDeviceIdentity.DeviceClass.Keyboard);
            inputdevices.AddDevices(InputDeviceIdentity.DeviceClass.Mouse);

            List<Device> deviceparas = new List<Device>();
            deviceparas.Add(new Device());

            JArray devicemappings = JTokenExtensionsOther.ReadJSONFile(Path.Combine(testfolder, "DeviceMappings.json")).Array();
            System.Diagnostics.Debug.Assert(devicemappings != null);
            FrontierDeviceMapping.DeviceMappingToPIDVID = devicemappings;

            JArray expmap = new JArray { new JArray { "DemoMerge", 100, 200 }, new JArray { "VPCThrottle", 13124, 408 } };
            FrontierDeviceMapping.MergeMappings(devicemappings, expmap);
            System.Diagnostics.Debug.WriteLine(devicemappings.ToString(true));

            foreach (var device in inputdevices)
            {
                System.Diagnostics.Debug.WriteLine($"{device.ID.Name} {device.ID.VendorId} {device.ID.ProductId} {device.ID.VendorProductId}");

                // does frontier know about it?
                string frontiername = device.ID.Name == "Keyboard" || device.ID.Name == "Mouse" ? device.ID.Name : FrontierDeviceMapping.FindPIDVID(device.ID.ProductId, device.ID.VendorId) ?? device.ID.VendorProductId;

                deviceparas.Add(new Device(frontiername, device.ID.Name, device.AxisPresent,  device.POVCount, device.ButtonCount, true));
            }

            inputdevices.Start();

            bindingsEditor.Dock = DockStyle.Fill;

            bindingsEditor.DeviceInput += (bf, entry) =>
            {
                InputMapDialog im = new InputMapDialog();
                im.Init(inputdevices);
                im.AllowAxis = true;
                im.AllowKeyboard = im.AllowMouse = !entry.IsBinding;
                im.ShowPressOrRelease = false;
                im.ShowOKCancel = false;
                im.EscapeQuits = true;
                Theme.Current.ApplyDialog(im);
                if (im.ShowDialog(this) == DialogResult.OK)
                {
                    string frontierdevicename = bindingsEditor.GetDeviceName(im.Device.Name, im.Device.ID.Instanceguid, im.Device.ID.Productguid, im.Device.ID.ProductId, im.Device.ID.VendorId);

                    if (frontierdevicename != null)
                    {
                        string frontierkeyname = im.Device.Name == "Keyboard" ? FrontierKeyConversion.KeysToFrontier(bf.KeyboardLayout, im.KeyName) : im.KeyName;

                        if (!frontierkeyname.StartsWith("!"))
                        {
                            return Tuple.Create(frontierdevicename, frontierkeyname, im.DirectionPositive);
                        }
                        else
                        {
                            ExtendedControls.MessageBoxTheme.Show($"Cannot find mapping to key name", "Cannot find device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                        ExtendedControls.MessageBoxTheme.Show($"Cannot find frontier device name for device\r\nUse Frontier editor to add device first", "Cannot find device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return null;
            };

            string bindingfolder = @"C:\Users\RK\AppData\Local\Frontier Developments\Elite Dangerous\Options\Bindings";
            var frontierpresetfilebindingfilename = EliteDangerousCore.Bindings.BindingsFile.FindBindingsFile(bindingfolder, true);
            string curset = FileHelpers.TryReadAllTextFromFile(testfolder + "keynames.json");
            string defset = FileHelpers.TryReadAllTextFromFile(testfolder + "defkeynames.json");
           // curset = null;
          //  defset = null;

            DeviceKeyNames keynames = new DeviceKeyNames();
            if (curset != null)
                keynames.Set(curset);

            if (defset != null)           // if we have a default list, see if it needs to populate into standard list
            {
                DeviceKeyNames defrenames = new DeviceKeyNames();
                defrenames.Set(defset);
                foreach (DeviceKeyNames.DeviceNameSet key in defrenames)
                {
                    if (keynames.GetDevice(key.Device) == null)
                    {
                        keynames.Add(key);
                    }
                }
            }

            bindingsEditor.Init(bindingfolder, frontierpresetfilebindingfilename, deviceparas, keynames);

        }
        public void Stop()
        {
            inputdevices?.Stop();
            FileHelpers.TryWriteToFile(testfolder + "keynames.json", bindingsEditor.KeyNames());
        }

        InputDeviceList inputdevices;
        string testfolder = $@"..\..\..\UnitTest\Bindings\";
      
    }
}
