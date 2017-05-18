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

        public void Dispose()
        {
            devices.Dispose();
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private void Devices_OnNewEvent(List<InputDeviceEvent> list)
        {
            IntPtr handle = GetForegroundWindow();

            if ( handle != null )
            { 
                StringBuilder Buffer = new StringBuilder(256);
                if (GetWindowText(handle, Buffer, 256) > 0)
                {
                    System.Diagnostics.Debug.WriteLine("From " + Buffer.ToString());
                }
            }

            Process[] all = Process.GetProcesses();
            foreach( Process pa in all )
            {
                System.Diagnostics.Debug.WriteLine("Process " + pa.ProcessName);
            }

            Process[] processes = Process.GetProcessesByName("EliteDangerous64");

            if ( processes.Length>0)
            {
                Process p = processes[0];
                if (p.MainWindowHandle == handle)
                {
                    System.Diagnostics.Debug.WriteLine("Elite Dangerous selected");
                }
            }





            foreach (InputDeviceEvent je in list)
            {
                string match = je.EventName();              // same as bindings name..
                System.Diagnostics.Debug.WriteLine(je.ToString(10) + " " + match);

                BindingsFile.Device dv = GetBindingDeviceFromInputDeviceIdentifier(je.Device.ID());

                if (dv != null)
                {
                    List<BindingsFile.Assignment> assignlist = dv.Find(match, false);

                    if ( assignlist != null)
                    {
                        List<BindingsFile.Assignment> inonstate = new List<BindingsFile.Assignment>();
                        
                        foreach (BindingsFile.Assignment a in assignlist)
                        {
                            if ( IsAllPressed(a))
                            {
                                System.Diagnostics.Debug.WriteLine("  Rule Matches " + a.assignedfunc);
                                inonstate.Add(a);       // but it might not be the best rule..
                            }
                            else
                            {
                                int isonindex = assignmentsinonstate.IndexOf(a);
                                if ( isonindex != -1 )
                                {
                                    System.Diagnostics.Debug.WriteLine("  Rule Inactive " + a.assignedfunc);
                                    assignmentsinonstate.Remove(a);
                                }
                            }
                        }

                        foreach(BindingsFile.Assignment a in inonstate)
                        {
                            if ( a.KeyAssignementLongerThan(inonstate))  // we have the best key list
                            {
                                assignmentsinonstate.Add(a);
                                System.Diagnostics.Debug.WriteLine("  Rule Active " + a.assignedfunc);
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("  Reject Rule due to others are longer " + a.assignedfunc);
                            }
                        }

                    }
                }
            }
        }


        public bool IsAllPressed(BindingsFile.Assignment a)
        {
            foreach (BindingsFile.DeviceKeyPair ma in a.keys)
            {
                InputDeviceInterface idi = GetInputDeviceFromBindingDevice(ma.Device);

                if (idi == null || !idi.IsPressed(ma.Key))
                    return false;
            }

            return true;
        }

        BindingsFile.Device GetBindingDeviceFromInputDeviceIdentifier(InputDeviceIdentity i)
        {
            BindingsFile.Device dv = bf.FindDevice(i.Name, i.Instanceguid, i.Productguid);
            return dv;
        }

        InputDeviceInterface GetInputDeviceFromBindingDevice(BindingsFile.Device dv)
        {
            InputDeviceInterface i = devices.inputdevices.Find(x =>
            {
                BindingsFile.Device b = bf.FindDevice(x.ID().Name, x.ID().Instanceguid, x.ID().Productguid);
                return b != null && b.Name.Equals(dv.Name);
            });

            return i;
        }


    }
}
