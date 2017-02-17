using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Audio
{
    class DummySpeechEngine : ISpeechEngine
    {
        public string[] GetVoiceNames()
        {
            return new string[] { };
        }

        public System.IO.MemoryStream Speak(string phrase, string culture, string voice, int volume, int rate)
        {
            return null;
        }
    }
}
