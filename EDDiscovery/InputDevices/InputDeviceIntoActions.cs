using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery.EliteDangerous;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace EDDiscovery.InputDevices
{

    class InputDevicesIntoActions
    {
        InputDeviceList devices;
        Actions.ActionController ac;
        BindingsFile bf;

        List<BindingsFile.Assignment> assignmentsinonstate = new List<BindingsFile.Assignment>();

        public InputDevicesIntoActions(InputDeviceList pi, BindingsFile b , Actions.ActionController pc )
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

        public void Dispose()
        {
            devices.Dispose();
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        private void Devices_OnNewEvent(List<InputDeviceEvent> list)
        {
            IntPtr handle = GetForegroundWindow();
            Process[] processes = Process.GetProcessesByName("4NT");//Process.GetProcessesByName("EliteDangerous64");

            if (processes.Length == 0 || processes[0].MainWindowHandle != handle)
            {
                return;
            }

            foreach (InputDeviceEvent je in list)
            {
                string match = je.EventName();              // same as bindings name..
                System.Diagnostics.Debug.WriteLine(je.ToString(10) + " " + match);

                ac.ActionRun("onEliteInputRaw", "EliteUIEvent", additionalvars: new ConditionVariables(new string[]
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
                                    ac.ActionRun("onEliteInputOff", "EliteUIEvent", additionalvars: new ConditionVariables(new string[]
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
                            ac.ActionRun("onEliteInput", "EliteUIEvent", additionalvars: new ConditionVariables(new string[]
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
                InputDeviceInterface idi = GetInputDeviceFromBindingDevice(ma.Device);

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
            BindingsFile.Device dv = bf.FindDevice(i.Name, i.Instanceguid, i.Productguid);
            return dv;
        }

        InputDeviceInterface GetInputDeviceFromBindingDevice(BindingsFile.Device dv)
        {
            InputDeviceInterface i = devices.Find(x =>
            {
                BindingsFile.Device b = bf.FindDevice(x.ID().Name, x.ID().Instanceguid, x.ID().Productguid);
                return b != null && b.Name.Equals(dv.Name);
            });

            return i;
        }


    }
}
