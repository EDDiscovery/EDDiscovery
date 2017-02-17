using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Audio
{
    public interface ISpeechEngine
    {
        string[] GetVoiceNames();
        System.IO.MemoryStream Speak(string phrase, string culture, string voice , int volume, int rate);
    }

    public class SpeechSynthesizer
    {
        ISpeechEngine speechengine;

        public SpeechSynthesizer( ISpeechEngine engine )
        {
            speechengine = engine;
        }

        public string[] GetVoiceNames()
        {
            return speechengine.GetVoiceNames();
        }

        public System.IO.MemoryStream Speak(string say, string culture, string voice, int rate)     // may return null
        {
            return speechengine.Speak(say, culture, voice, 100, rate);     // samples are always generated at 100 volume
        }
    }
}
