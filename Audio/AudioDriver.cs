using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conditions;

namespace AudioExtensions
{
    public delegate void AudioStopped();

    public class AudioData              // this holds the data object, used to wrap the object to give it a proper named class.
    {
        public AudioData() { }
        public AudioData(Object d) { data = d; }
        public Object data;
    }

    public interface IAudioDriver : IDisposable
    {
        event AudioStopped AudioStoppedEvent;

        void Start(AudioData o, int vol);      // start with this audio
        void Stop();
        void Dispose(AudioData o);             // finish with this audio

        AudioData Generate(string file, ConditionVariables effects = null);       // generate audio samples and return. Effects 
        AudioData Generate(System.IO.Stream audioms, ConditionVariables effects = null, bool ensuresomeaudio = false); // generate audio and return with effect and ensuring audio if req.

        AudioData Mix(AudioData last, AudioData mix);            // either may be null, in which case null.. Converted to mix format
        AudioData Append(AudioData front, AudioData append);      // either may be null, in which case null.. Converted to append format

        int Lengthms(AudioData audio);                     // whats the length?
        int TimeLeftms(AudioData audio);

        string GetAudioEndpoint();
        List<string> GetAudioEndpoints();
        bool SetAudioEndpoint(string device, bool usedefaultifnotfound = false);
    }
}

