/*
 * Copyright 2026-2026 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 */

using DirectInputDevices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class Bindings : UserControlCommonBase
    {
        public Bindings()
        {
            InitializeComponent();
            DBBaseName = "Bindings";
        }

        protected override void Init()
        {
            var frontierpresetfilebindingfilename = EliteDangerousCore.BindingsFile.FindBindingsFile(EDDOptions.Instance.FrontierBindingsFolder, true);

            List<string> devices = new List<string>();
            foreach (var device in DiscoveryForm.InputDeviceList)
            {
                if (device.ID.GameControl)
                {
                    System.Diagnostics.Debug.WriteLine($"{device.ID.Name} {device.ID.VendorId} {device.ID.ProductId} {device.ID.VendorProductId}");

                    // does frontier know about it?
                    string bestname = EliteDangerousCore.BindingsFile.FrontierDeviceName(device.ID.ProductId, device.ID.VendorId);

                    if (bestname != null) // if frontier knows it, add its name, else add usb identity which frontier appears to use
                        devices.Add(bestname);
                    else
                        devices.Add(device.ID.VendorProductId);

                    // allow the productvendorid pair to be converted to device name
                    bindingsEditor.ConvertDeviceNameList[device.ID.VendorProductId] = device.ID.Name;       
                }
            }

            bindingsEditor.Init(EDDOptions.Instance.FrontierBindingsFolder, frontierpresetfilebindingfilename, new System.Collections.Generic.List<string>());
            bindingsEditor.ChangedBindings += (s) =>
            {
                if (DiscoveryForm.FrontierBindings.FileName.EqualsIIC(s) || !DiscoveryForm.FrontierBindings.IsLoaded)      // if same name, or not loaded, try and load
                    DiscoveryForm.LoadFrontierBindings();       // reload, 
            };
            bindingsEditor.ChangedDefault += (s) =>
            {
                if (!DiscoveryForm.FrontierBindings.FileName.EqualsIIC(s))      // if default is not the same as the current filename.
                    DiscoveryForm.LoadFrontierBindings();       // reload, 
            };

            bindingsEditor.DeviceInput += (bf, entry) =>
            {
                InputMapDialog im = new InputMapDialog();
                im.Init(DiscoveryForm.InputDeviceList);
                im.AllowAxis = im.AxisOnly = entry.IsBinding;
                im.ShowPressOrRelease = false;
                im.ShowOKCancel = false;
                im.EscapeQuits = true;
                ExtendedControls.Theme.Current.ApplyStd(im);
                if (im.ShowDialog(this) == DialogResult.OK)
                {
                    string devicename = im.Device.Name;
                    if (!bindingsEditor.DevicesNamesConverted.Contains(devicename))
                    {
                        // same way its done in operation to map to a frontier device
                        string bestname = bindingsEditor.FindDevice(im.Device.Name, im.Device.ID.Instanceguid, im.Device.ID.Productguid, im.Device.ID.ProductId, im.Device.ID.VendorId);
                        if (bestname == null)
                            ExtendedControls.MessageBoxTheme.Show($"Cannot find frontier device name for device\r\nUse Frontier editor to add device first", "Cannot find device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        devicename = bestname;
                    }

                    if (devicename != null)
                    {
                        string frontiername = im.Device.Name == "Keyboard" ? EliteDangerousCore.FrontierKeyConversion.KeysToFrontier(bf.KeyboardLayout, im.KeyName) : im.KeyName;

                        if (!frontiername.StartsWith("!"))
                        {
                            EliteDangerousCore.BindingsFile.DeviceKeyPair dvp = new EliteDangerousCore.BindingsFile.DeviceKeyPair(devicename, frontiername);
                            return dvp;
                        }
                        else
                            ExtendedControls.MessageBoxTheme.Show($"Cannot find mapping to key name", "Cannot find device", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                return null;
            };

        }

        protected override void InitialDisplay()
        {
        }

        protected override void Closing()
        {
        }

        public override bool AllowClose()
        {
            if ( bindingsEditor.IsDirty)
            {

            }

            return true;
        }

    }
}
