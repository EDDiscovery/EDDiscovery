using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.InputDevices
{
    // list of devices, and main event loop.  Hook to OnNewEvent

    class InputDeviceList
    {
        public event Action<List<InputDeviceEvent>> OnNewEvent;

        private System.Threading.AutoResetEvent stophandle = new System.Threading.AutoResetEvent(false);        // used by dispose to tell thread to stop
        private System.Threading.Thread waitfordatathread;      // the background worker
        private List<InputDeviceInterface> inputdevices { get; set; } = new List<InputDeviceInterface>();

        public void Add(InputDeviceInterface i)
        {
            inputdevices.Add(i);
        }

        public InputDeviceInterface Find(Predicate<InputDeviceInterface> p)
        {
            return inputdevices.Find(p);
        }

        public void Start()
        {
            if (waitfordatathread == null)
            {
                waitfordatathread = new System.Threading.Thread(waitthread);
                waitfordatathread.Start();
            }
        }

        public void Stop()
        {
            if (waitfordatathread != null)
            {
                stophandle.Set();
                waitfordatathread.Join();
                waitfordatathread = null;
                System.Diagnostics.Debug.WriteLine("IDL Stop");
            }
        }

        public void Dispose()       // stop and dispose of all input devices
        {
            Stop();

            foreach (InputDeviceInterface i in inputdevices)
                i.Dispose();

            System.Diagnostics.Debug.WriteLine("IDL Dispose");
        }


        private void waitthread()
        {
            System.Threading.WaitHandle[] wh = new System.Threading.WaitHandle[inputdevices.Count+1];
            for (int i = 0; i < inputdevices.Count; i++)
                wh[i] = inputdevices[i].Eventhandle();
            wh[inputdevices.Count] = stophandle;

            System.Diagnostics.Debug.WriteLine("IDL start");

            while (true)
            {
                int hhit = System.Threading.WaitHandle.WaitAny(wh);

                if (hhit == inputdevices.Count)  // stop!
                    break;

                List<InputDeviceEvent> list = inputdevices[hhit].GetEvents();

                if (list != null)
                {
                    //System.Diagnostics.Debug.WriteLine(Environment.TickCount + " Handle hit " + hhit + " " + inputdevices[hhit].ID().Name);
                    OnNewEvent?.Invoke(list);
                }
            }
        }
    }
}
