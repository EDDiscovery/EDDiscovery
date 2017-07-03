using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Audio
{
    public delegate void AudioStopped();

    public interface IAudioDriver : IDisposable
    {
        event AudioStopped AudioStoppedEvent;

        void Start(Object o, int vol);      // start with this audio
        void Stop();
        void Dispose(Object o);             // finish with this audio

        Object Generate(string file, ConditionVariables effects = null);       // generate audio samples and return. Effects 
        Object Generate(System.IO.Stream audioms, ConditionVariables effects = null, bool ensuresomeaudio = false); // generate audio and return with effect and ensuring audio if req.

        Object Add(Object last, string file, ConditionVariables effects = null);       // add to audio object
        Object Add(Object last, System.IO.Stream audioms, ConditionVariables effects = null, bool ensuresomeaudio = false); // add to audio object

        string GetAudioEndpoint();
        List<string> GetAudioEndpoints();
        bool SetAudioEndpoint(string device, bool usedefaultifnotfound = false);
    }
}

