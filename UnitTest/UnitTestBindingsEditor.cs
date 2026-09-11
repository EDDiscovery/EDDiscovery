using BaseUtils;
using DirectInputDevices;
using EliteDangerousCore;
using EliteDangerousCore.Bindings;
using ExtendedControls;
using System;
using System.Collections.Generic;
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
            inputdevices = new DirectInputDevices.InputDeviceList();
            InputDeviceJoystickWindows.CreateJoysticks(inputdevices);

            List<Device> deviceparas = new List<Device>();
            deviceparas.Add(new Device());

            foreach (var device in inputdevices)
            {
                System.Diagnostics.Debug.WriteLine($"{device.ID.Name} {device.ID.VendorId} {device.ID.ProductId} {device.ID.VendorProductId}");

                // does frontier know about it?
                string frontiername = FrontierDeviceNames.DeviceName(device.ID.ProductId, device.ID.VendorId) ?? device.ID.VendorProductId;

                deviceparas.Add(new Device(frontiername, device.ID.Name, device.AxisPresent,  device.POVCount, device.ButtonCount));
            }

            deviceparas.Add(new Device("Keyboard", true, false));
            deviceparas.Add(new Device("Mouse", false, true));

            InputDeviceKeyboard.CreateKeyboard(inputdevices);
            InputDeviceMouse.CreateMouse(inputdevices);

            inputdevices.Start();

            bindingsEditor.Dock = DockStyle.Fill;

            bindingsEditor.DeviceInput += (bf, entry) =>
            {
                InputMapDialog im = new InputMapDialog();
                im.Init(inputdevices);
                im.AllowAxis = im.AxisOnly = entry.IsBinding;
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
                            return Tuple.Create(frontierdevicename, frontierkeyname);
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
            //curset = null;
            //defset = null;

            DeviceKeyNames keynames = new DeviceKeyNames();
            if (curset != null)
                keynames.Set(curset);

            if (defset != null)           // if we have a default list, see if it needs to populate into standard list
            {
                DeviceKeyNames defrenames = new DeviceKeyNames();
                defrenames.Set(defset);
                foreach (DeviceKeyNames.DeviceNameSet key in defrenames)
                {
                    if (keynames.GetByDeviceList(key.DeviceList) == null)
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
