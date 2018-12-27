/*
 * Copyright © 2017 EDDiscovery development team
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
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using DirectInputDevices;
using EliteDangerousCore;

namespace EDDiscovery.Actions
{
    class ActionsFromInputDevices
    {
        InputDeviceList devices;
        Actions.ActionController ac;
        BindingsFile bf;

        List<BindingsFile.Assignment> assignmentsinonstate = new List<BindingsFile.Assignment>();

        public ActionsFromInputDevices(InputDeviceList pi, BindingsFile b , Actions.ActionController pc )
        {
            devices = pi;
            bf = b;
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
            foreach (BindingsFile.Device bd in bf)     // for all devices listed in the binding file
            {
                IInputDevice idi = GetInputDeviceFromBindingDevice(bd);

                if (idi == null)
                    ret += "ERROR: Missing physical device for FD Device " + bd.Name + Environment.NewLine;
                else
                    ret += "Match of FD Device " + bd.Name + " to " + idi.ID().Name + Environment.NewLine;
            }
            return ret;
        }

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

            foreach (InputDeviceEvent je in list)
            {
                string match = je.EventName();              // same as bindings name..
                System.Diagnostics.Debug.WriteLine(je.ToString(10) + " " + match);

                ac.ActionRun(Actions.ActionEventEDList.onEliteInputRaw, additionalvars: new BaseUtils.Variables(new string[]
                        { "Device" , je.Device.ID().Name, "EventName", match , "Pressed" , je.Pressed?"1":"0", "Value" , je.Value.ToStringInvariant() }));

                BindingsFile.Device dv = GetBindingDeviceFromInputDeviceIdentifier(je.Device.ID());

                if (dv != null)
                {
                    List<BindingsFile.Assignment> assignlist = dv.Find(match, false);       // get everything associated with this key..

                    if ( assignlist != null)
                    {
                        List<BindingsFile.Assignment> inonstate = new List<BindingsFile.Assignment>();
                        List<bool> ispressable = new List<bool>();
                        
                        foreach (BindingsFile.Assignment a in assignlist)
                        {
                            Tuple<bool, bool> pressstate = IsAllPressed(a);
                            if ( pressstate.Item1 )     // if pressed
                            {
                                //System.Diagnostics.Debug.WriteLine("  Rule Matches " + a.assignedfunc);
                                inonstate.Add(a);       // but it might not be the best rule..
                                ispressable.Add(pressstate.Item2);
                            }
                            else
                            {
                                int isonindex = assignmentsinonstate.IndexOf(a);
                                if ( isonindex != -1 )
                                {
                                    System.Diagnostics.Debug.WriteLine("Action " + a.assignedfunc + "-");
                                    assignmentsinonstate.Remove(a);
                                    ac.ActionRun(Actions.ActionEventEDList.onEliteInputOff, additionalvars: new BaseUtils.Variables(new string[]
                                     { "Binding" , a.assignedfunc }));
                                }
                            }
                        }

                        List<string> bindingstoexecute = new List<string>();

                        for( int i = 0; i < inonstate.Count; i++ )
                        {
                            BindingsFile.Assignment a = inonstate[i];
                            if (a.KeyAssignementLongerThan(inonstate))  // we have the best key list
                            {
                                if ( ispressable[i])
                                    assignmentsinonstate.Add(a);

                                bindingstoexecute.Add(a.assignedfunc);
                                System.Diagnostics.Debug.WriteLine("Action " + a.assignedfunc);
                            }
                            else
                            {
                                //System.Diagnostics.Debug.WriteLine("  Reject Rule due to others are longer " + inonstate[i].assignedfunc);
                            }
                        }

                        //System.Diagnostics.Debug.WriteLine("On state " + assignmentsinonstate.Count);

                        foreach (string s in bindingstoexecute)
                        {
                            ac.ActionRun(Actions.ActionEventEDList.onEliteInput, additionalvars: new BaseUtils.Variables(new string[]
                            { "Device" , je.Device.ID().Name, "Binding" , s , "BindingList" , String.Join(",",bindingstoexecute),
                              "EventName", match , "Pressed" , je.Pressed?"1":"0", "Value" , je.Value.ToStringInvariant() }));
                        }

                    }
                }
            }
        }


        public Tuple<bool,bool> IsAllPressed(BindingsFile.Assignment a)     // return if all okay pressed, second if all are pressable
        {
            bool allpressable = true;

            foreach (BindingsFile.DeviceKeyPair ma in a.keys)
            {
                IInputDevice idi = GetInputDeviceFromBindingDevice(ma.Device);

                if (idi == null )       // no device, false
                    return new Tuple<bool,bool>(false,false);

                bool? v = idi.IsPressed(ma.Key);        // is it pressed, or not pressable?

                if (v.HasValue)         // is pressable
                {
                    if (v.Value == false)     // if it
                        return new Tuple<bool, bool>(false, false);
                }
                else
                    allpressable = false;   // not pressable
            }

            return new Tuple<bool, bool>(true, allpressable);
        }

        BindingsFile.Device GetBindingDeviceFromInputDeviceIdentifier(InputDeviceIdentity i)
        {
            BindingsFile.Device dv = bf.FindDevice(i.Name, i.Instanceguid, i.Productguid, i.ProductId, i.VendorId);
            return dv;
        }

        IInputDevice GetInputDeviceFromBindingDevice(BindingsFile.Device dv)
        {
            IInputDevice i = devices.Find(x =>
            {
                BindingsFile.Device b = bf.FindDevice(x.ID().Name, x.ID().Instanceguid, x.ID().Productguid,x.ID().ProductId,x.ID().VendorId);
                return b != null && b.Name.Equals(dv.Name);
            });

            return i;
        }


    }
}
