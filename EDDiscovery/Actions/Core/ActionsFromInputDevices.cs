/*
 * Copyright 2017-2026 EDDiscovery development team
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
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EDDiscovery.Actions
{
    class ActionsFromInputDevices
    {
        InputDeviceList devices;
        Actions.ActionController ac;
        BindingsFile bindings;

        List<BindingsFile.DeviceKeySet> assignmentsinonstate = new List<BindingsFile.DeviceKeySet>();

        public ActionsFromInputDevices(InputDeviceList pi, BindingsFile b , Actions.ActionController pc )
        {
            devices = pi;
            bindings = b;
            ac = pc;
        }

        public void Start()
        {
            devices.OnNewEvent += Devices_OnNewEvent;
            devices.Start();
        }

        public void Stop()
        {
            devices.OnNewEvent -= Devices_OnNewEvent;
            devices.Stop();
        }

        public string CheckBindings()
        {
            string ret = "";

            foreach (string frontierdevice in bindings.DeviceListNoDevice)     
            {
                IInputDevice idi = GetInputDeviceFromBindingDevice(frontierdevice); // find best match of physical device

                if (idi == null)
                    ret += "ERROR: Missing physical device for FD Device " + frontierdevice + Environment.NewLine;
                else
                    ret += "Match of FD Device " + frontierdevice + " to " + idi.ID().Name + Environment.NewLine;
            }
            return ret;
        }


        // A device has generated a new event in the list
        private void Devices_OnNewEvent(List<InputDeviceEvent> list)
        {
            IntPtr handle = BaseUtils.Win32.UnsafeNativeMethods.GetForegroundWindow();
            Process[] processes = Process.GetProcessesByName("elitedangerous64");//Process.GetProcessesByName("EliteDangerous64");
            bool ed = false;
            foreach (Process p in processes)
            {
                if ( p.MainWindowHandle == handle )     //ED seems to have multiple processes running.. find one
                {
                    ed = true;
                    break;
                }
            }

            if ( !ed )
            {
                //System.Diagnostics.Debug.WriteLine("Rejected keypress " + processes.Length);
                return;
            }

            foreach (InputDeviceEvent ide in list)
            {
                string keyname = ide.EventName();              // same as bindings name..
                                                               // System.Diagnostics.Debug.WriteLine(je.ToString(10) + " " + match);

                System.Diagnostics.Debug.WriteLine($"\r\nActionInputDevice Generate Action EliteInputRaw {ide.Device.ID().Name} {keyname} {ide.Pressed}");
                ac.ActionRun(Actions.ActionEventEDList.onEliteInputRaw, additionalvars: new BaseUtils.Variables(new string[]
                        { "Device" , ide.Device.ID().Name, "EventName", keyname , "Pressed" , ide.Pressed?"1":"0", "Value" , ide.Value.ToStringInvariant() }));

                string frontierdevice = GetBindingDeviceFromInputDeviceIdentifier(ide.Device.ID());     // find best match for 

                System.Diagnostics.Debug.WriteLine($"ActionsInputDevice {frontierdevice}:{keyname}");

                if (frontierdevice != null)
                {
                    // list all entries associated with the device:key pair including mod keys

                    List<BindingsFile.DeviceKeySet> assignlist = bindings.FindDeviceKey(frontierdevice, keyname, false);       

                    foreach(var x in assignlist.EmptyIfNull())
                    {
                        System.Diagnostics.Debug.WriteLine($"ActionInputDevice {frontierdevice} {keyname} matched {x.Entry.Name}");
                    }

                    if (assignlist != null)
                    {
                        var inonstate = new List<BindingsFile.DeviceKeySet>();   // a list of on states
                        List<bool> ispressable = new List<bool>();

                        foreach (var found in assignlist)
                        {
                            // go into the current device states and see if this list of key strokes are assigned and pressable
                            Tuple<bool, bool> pressstate = IsAllPressed(found.Keys);

                            if (pressstate.Item1)     // if all are pressed
                            {
                                System.Diagnostics.Debug.WriteLine($"ActionInputDevice All are pressed for {found.Entry.Name}");
                                inonstate.Add(found);                                 // add to on list the keypresses which worked
                                ispressable.Add(pressstate.Item2);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"ActionInputDevice Not pressed for {found.Entry.Name}");

                                // so this one is not on, lets see if it was previously on. Need to find by name as its a new instance of DeviceKeySet
                                BindingsFile.DeviceKeySet off = assignmentsinonstate.Find(x => x.Entry.Name == found.Entry.Name);
                                if (off!=null)
                                {
                                    System.Diagnostics.Debug.WriteLine($"ActionInputDevice {found.Entry.Name} has turned off");
                                    assignmentsinonstate.Remove(off);
                                    ac.ActionRun(Actions.ActionEventEDList.onEliteInputOff, additionalvars: new BaseUtils.Variables(new string[] { "Binding", found.Entry.Name }));
                                }
                            }
                        }

                        List<string> bindingstoexecute = new List<string>();        // logical list of frontier bindings to action

                        for (int i = 0; i < inonstate.Count; i++)
                        {
                            var onset = inonstate[i];       // list of keypresses
                            if (KeyAssignementLongerThan(onset, inonstate))  // we have the best key list
                            {
                                System.Diagnostics.Debug.WriteLine($"ActionInputDevice {onset.Entry.Name} has the best keylist");

                                if (ispressable[i])     // record it was off, to let it turn off
                                    assignmentsinonstate.Add(onset);

                                bindingstoexecute.Add(onset.Entry.Name);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"ActionInputDevice {onset.Entry.Name} is not the best keylist vs list");
                            }
                        }

                        foreach (string frontierbindingname in bindingstoexecute)
                        {
                            System.Diagnostics.Debug.WriteLine($"ActionInputDevice Run Action BindingList with {frontierbindingname}");
                            ac.ActionRun(Actions.ActionEventEDList.onEliteInput, additionalvars: new BaseUtils.Variables(new string[]
                            { "Device" , ide.Device.ID().Name, "Binding" , frontierbindingname , "BindingList" , String.Join(",",bindingstoexecute),
                                          "EventName", keyname , "Pressed" , ide.Pressed?"1":"0", "Value" , ide.Value.ToStringInvariant() }));
                        }
                    }
                }
            }
        }

        // see if the DKP list is currently pressed
        // return Item1 if all are pressed
        // return Item2 if all are pressable
        public Tuple<bool, bool> IsAllPressed(List<BindingsFile.DeviceKeyPair> dkplist)     
        {
            bool allpressable = true;

            foreach (BindingsFile.DeviceKeyPair ma in dkplist)
            {
                IInputDevice idi = GetInputDeviceFromBindingDevice(ma.Device);

                if (idi == null)       // no device, false
                    return Tuple.Create(false, false);

                bool? v = idi.IsPressed(ma.Key);        // is it pressed, or not pressable?
                System.Diagnostics.Debug.WriteLine($"IsAllPressed {ma.Key}= {v}");

                if (v.HasValue)         // is pressable
                {
                    if (v.Value == false)     // if it
                        return Tuple.Create(false, false);
                }
                else
                    allpressable = false;   // not pressable
            }

            return Tuple.Create(true, allpressable);
        }

        // is ours the best keylist (based on length)
        // compare our list vs all others, return false if we are shorter
        public bool KeyAssignementLongerThan(BindingsFile.DeviceKeySet our, List<BindingsFile.DeviceKeySet> others)  
        {
            foreach (BindingsFile.DeviceKeySet a in others)      // check others
            {
                if (a != our) // don't check ourselves
                {
                    if (BindingsFile.DeviceKeyPair.HasInternalKeyInCommon(our.Keys,a.Keys))        // do we have a clash of keys, other has keys in our key list..
                    {
                        if (our.Keys.Count < a.Keys.Count)  // yes, is our key length less.. then its the others.
                            return false;
                    }
                }
            }
            return true;
        }

        // given a InputDeviceIdendity, what is the bindings name for it?
        string GetBindingDeviceFromInputDeviceIdentifier(InputDeviceIdentity i)
        {
            string frontierdevicename = bindings.FindDevice(i.Name, i.Instanceguid, i.Productguid, i.ProductId, i.VendorId);
            return frontierdevicename;
        }

        // given a frontier device name, get back match or null to a InputDevice
        IInputDevice GetInputDeviceFromBindingDevice(string frontierdevicename)
        {
            IInputDevice i = devices.Find(x =>
            {
                string device = bindings.FindDevice(x.ID().Name, x.ID().Instanceguid, x.ID().Productguid,x.ID().ProductId,x.ID().VendorId);
                return device != null && device.Equals(frontierdevicename);
            });

            return i;
        }


    }
}
