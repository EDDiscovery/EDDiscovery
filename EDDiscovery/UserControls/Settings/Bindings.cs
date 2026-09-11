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
using EliteDangerousCore.Bindings;
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
            var frontierpresetfilebindingfilename = BindingsFile.FindBindingsFile(EDDOptions.Instance.FrontierBindingsFolder, true);

            List<Device> deviceparas = new List<Device>();
            deviceparas.Add(new Device());

            foreach (var device in DiscoveryForm.InputDeviceList)
            { 
                System.Diagnostics.Debug.WriteLine($"{device.ID.Name} {device.ID.VendorId} {device.ID.ProductId} {device.ID.VendorProductId}");

            // does frontier know about it?
                string frontiername = device.ID.Name == "Keyboard" || device.ID.Name == "Mouse" ? device.ID.Name : FrontierDeviceNames.DeviceName(device.ID.ProductId, device.ID.VendorId) ?? device.ID.VendorProductId;

                deviceparas.Add(new Device(frontiername, device.ID.Name, device.AxisPresent, device.POVCount, device.ButtonCount));
            }

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

            bindingsEditor.ResetKeyNames += () =>
            {
                string defnames2 = Properties.Resources.defkeynames;            // reset the set to the program default
                keynames.Set(defnames2);
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


            string userset = GetSettingGlobal("DeviceKeyNames", "{}");
            //keynames.Set(userset);

            // we ship with a set, if its not present in the user set, update the user set
            string defnames = Properties.Resources.defkeynames;
            DeviceKeyNames defrenames = new DeviceKeyNames();
            defrenames.Set(defnames);
            foreach (DeviceKeyNames.DeviceNameSet key in defrenames)
            {
                if (keynames.GetByDeviceList(key.DeviceList) == null)
                {
                    keynames.Add(key);
                }
            }

            bindingsEditor.Init(EDDOptions.Instance.FrontierBindingsFolder, frontierpresetfilebindingfilename, deviceparas, keynames);
        }

        protected override void Closing()
        {
            string json = keynames.Get();
            PutSettingGlobal("DeviceKeyNames", json);
        }

        public override bool AllowClose()
        {
            if ( bindingsEditor.IsDirty)
            {
                var result = ExtendedControls.MessageBoxTheme.Show(FindForm(), "Unsaved changed to bindings, Do you want to abandon them?", 
                                    "Warning".Tx(), MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                return result == DialogResult.OK;
            }

            return true;
        }

        DeviceKeyNames keynames = new DeviceKeyNames();
    }
}
