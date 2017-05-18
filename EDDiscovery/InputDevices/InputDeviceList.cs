using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.InputDevices
{
    class InputDeviceList
    {
        public List<InputDeviceInterface> inputdevices { get; private set; } = new List<InputDeviceInterface>();

        System.Threading.AutoResetEvent stophandle = new System.Threading.AutoResetEvent(false);        // used by dispose to tell thread to stop
        System.Threading.Thread waitfordatathread;      // the background worker
        public event Action<List<InputDeviceEvent>> OnNewEvent;

        public void Add(InputDeviceInterface i)
        {
            inputdevices.Add(i);
        }

        public void Dispose()
        {
            stophandle.Set();
            waitfordatathread.Join();
            System.Diagnostics.Debug.WriteLine("Safe to dispose");

            foreach (InputDeviceInterface i in inputdevices)
                i.Dispose();
        }

        public void Start()
        {
            waitfordatathread = new System.Threading.Thread(waitthread);
            waitfordatathread.Start();
        }

        void waitthread()
        {
            System.Threading.WaitHandle[] wh = new System.Threading.WaitHandle[inputdevices.Count+1];
            for (int i = 0; i < inputdevices.Count; i++)
                wh[i] = inputdevices[i].Eventhandle();
            wh[inputdevices.Count] = stophandle;

            System.Diagnostics.Debug.WriteLine("Begin thread");
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
            System.Diagnostics.Debug.WriteLine("Stop thread");
        }
    }
}
