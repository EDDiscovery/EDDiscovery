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

        Object Mix(Object last, Object mix);            // either may be null, in which case null
        Object Append(Object front, Object append);      // either may be null, in which case null

        string GetAudioEndpoint();
        List<string> GetAudioEndpoints();
        bool SetAudioEndpoint(string device, bool usedefaultifnotfound = false);
    }
}

