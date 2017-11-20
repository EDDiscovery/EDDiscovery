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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectInputDevices
{
    // list of devices, and main event loop.  Hook to OnNewEvent

    public class InputDeviceList : IEnumerable<IInputDevice>
    {
        public event Action<List<InputDeviceEvent>> OnNewEvent;

        private Action<Action> invokeAsyncOnUiThread;
        private System.Threading.AutoResetEvent stophandle = new System.Threading.AutoResetEvent(false);        // used by dispose to tell thread to stop
        private System.Threading.Thread waitfordatathread;      // the background worker
        private List<IInputDevice> inputdevices { get; set; } = new List<IInputDevice>();

        public InputDeviceList(Action<Action> i)            // Action context is pass to call in.. 
        {
            invokeAsyncOnUiThread = i;
        }

        public void Add(IInputDevice i)
        {
            inputdevices.Add(i);
        }

        public IInputDevice Find(Predicate<IInputDevice> p)
        {
            return inputdevices.Find(p);
        }

        public IEnumerator<IInputDevice> GetEnumerator()
        {
            foreach (var e in inputdevices)
                yield return e;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string ListDevices()
        {
            string ret = "";
            foreach (IInputDevice i in inputdevices)
                ret += i.ToString() + Environment.NewLine;
            return ret;
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

        public void Clear()
        {
            Stop();

            foreach (IInputDevice i in inputdevices)
                i.Dispose();

            inputdevices.Clear();
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

                    invokeAsyncOnUiThread(() => { OnNewEvent?.Invoke(list); }); // call in action context.
                }
            }
        }
    }
}
