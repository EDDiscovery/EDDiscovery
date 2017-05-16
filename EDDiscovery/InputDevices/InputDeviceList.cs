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

        public void Add(InputDeviceInterface i)
        {
            inputdevices.Add(i);
        }

        public void Dispose()
        {
            foreach (InputDeviceInterface i in inputdevices)
                i.Dispose();
        }

        public List<InputDeviceEvent> Poll()
        {
            List<InputDeviceEvent> list = new List<InputDeviceEvent>();
            foreach (InputDeviceInterface i in inputdevices)
            {
                List<InputDeviceEvent> ilist = i.Poll();
                if (ilist != null)
                    list.AddRange(ilist);
            }

            return list;
        }
    }
}
