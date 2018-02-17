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

namespace AudioExtensions
{
    public class AudioDriverDummy : IAudioDriver, IDisposable
    {
        public AudioDriverDummy() { }

        #region IDisposable support

        public void Dispose() { AudioStoppedEvent = null; }

        #endregion

        #region IAudioDriver support

#pragma warning disable 0414    // The field is assigned but its value is never used.
        public event AudioStopped AudioStoppedEvent;
#pragma warning restore 0414

        public void Start(AudioData o, int vol) { }
        public void Stop() { }
        public void Dispose(AudioData o) { o.Data = null; }

        public AudioData Generate(string file, SoundEffectSettings effects) { return null; }
        public AudioData Generate(System.IO.Stream audioms, SoundEffectSettings effects, bool ensureaudio) { return null; }
        public AudioData Mix(AudioData last, AudioData mix) { return null; }
        public AudioData Append(AudioData front, AudioData append) { return null; }

        public int Lengthms(AudioData audio) { return 0; }      // whats the length?
        public int TimeLeftms(AudioData audio) { return 0; }

        public string GetAudioEndpoint() { return "Default"; }
        public List<string> GetAudioEndpoints() { return new List<string>(new[] { "Default" }); }
        public bool SetAudioEndpoint(string dev , bool usedefaultifnotfound = false) { return true; }

        #endregion
    }
}
