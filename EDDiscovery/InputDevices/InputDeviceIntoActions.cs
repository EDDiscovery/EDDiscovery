using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery.EliteDangerous;


namespace EDDiscovery.InputDevices
{
    class InputDevicesIntoActions
    {
        InputDeviceList devices;
        Actions.ActionController ac;
        BindingsFile bf;
        Timer t;

        List<BindingsFile.Assignment> assignmentsinonstate = new List<BindingsFile.Assignment>();

        public InputDevicesIntoActions(InputDeviceList pi, BindingsFile b , Actions.ActionController pc )
        {
            devices = pi;
            bf = b;
            ac = pc;
        }

        public void Start()
        {
            t = new Timer();
            t.Interval = 100;
            t.Tick += T_Tick;
            t.Start();
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

        private void T_Tick(object sender, EventArgs e)
        {
            List<InputDeviceEvent> list = devices.Poll();
            foreach (InputDeviceEvent je in list)
            {
                string match = je.EventName();              // same as bindings name..
                System.Diagnostics.Debug.WriteLine(je.ToString(10) + " " + match);

                if (je.Pressed) // on event..
                {
                    BindingsFile.Device dv = GetBindingDeviceFromInputDeviceIdentifier(je.Device.ID());

                    if (dv != null)
                    {
                        List<BindingsFile.Assignment> assignlist = dv.Find(match,false);

                        if ( assignlist != null)
                        {
                            foreach(BindingsFile.Assignment a in assignlist)
                            {
                                if ( Verify(a))
                                {
                                    System.Diagnostics.Debug.WriteLine("  Pressed " + a.assignedfunc);
                                    assignmentsinonstate.Add(a);
                                }
                            }

                        }
                    }
                }
                else
                {               // off event..
                    List<BindingsFile.Assignment> updatedassignmentsinonstate = new List<BindingsFile.Assignment>();

                    foreach(BindingsFile.Assignment a in assignmentsinonstate)
                    {
                        if (!Verify(a))       // Need to verify all..
                        {
                            System.Diagnostics.Debug.WriteLine("  Released " + a.assignedfunc);
                        }
                        else
                            updatedassignmentsinonstate.Add(a);
                    }

                    assignmentsinonstate = updatedassignmentsinonstate;
                }
            }
        }

        public bool Verify(BindingsFile.Assignment a)
        {
            foreach (BindingsFile.DeviceKeyPair ma in a.keys)
            {
                InputDeviceInterface idi = GetInputDeviceFromBindingDevice(ma.Device);

                if (idi == null || !idi.IsPressed(ma.Key))
                    return false;
            }

            return true;
        }

        public void Stop()
        {
            t.Stop();
        }

        public void Dispose()
        {
            t.Stop();
            devices.Dispose();
        }
    }
}
