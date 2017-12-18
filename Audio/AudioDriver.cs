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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        void Stop();                           // async stop.. does not stop, just terminates the current audio
        void Dispose(AudioData o);             // finish with this audio

        AudioData Generate(string file, SoundEffectSettings effects = null);       // generate audio samples and return. Effects 
        AudioData Generate(System.IO.Stream audioms, SoundEffectSettings effects = null, bool ensuresomeaudio = false); // generate audio and return with effect and ensuring audio if req.

        AudioData Mix(AudioData last, AudioData mix);            // either may be null, in which case null.. Converted to mix format
        AudioData Append(AudioData front, AudioData append);      // either may be null, in which case null.. Converted to append format

        int Lengthms(AudioData audio);                     // whats the length?
        int TimeLeftms(AudioData audio);

        string GetAudioEndpoint();
        List<string> GetAudioEndpoints();
        bool SetAudioEndpoint(string device, bool usedefaultifnotfound = false);
    }
}

