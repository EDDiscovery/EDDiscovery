using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Audio
{
    class AudioDriverDummy : IAudioDriver, IDisposable
    {
        public event AudioStopped AudioStoppedEvent;

        public AudioDriverDummy(string devicestr)     // string would give a hint on device.. not used yet
        {
        }

        public void Dispose()
        {
        }

        public void Dispose(Object o)
        {
        }

        public void Start(Object o, int vol)
        {
        }

        public void Stop()
        {
        }

        public Object Generate(string file, ConditionVariables effects)
        {
            return null;
        }

        public Object Generate(System.IO.Stream audioms, ConditionVariables effects)
        {
            return null;
        }
    }
}
