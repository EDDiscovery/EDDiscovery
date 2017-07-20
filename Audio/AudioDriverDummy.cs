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

#pragma warning disable 0067

namespace AudioExtensions
{
    public class AudioDriverDummy : IAudioDriver, IDisposable
    {
        public event AudioStopped AudioStoppedEvent;

        public AudioDriverDummy()
        {
        }

        public List<string> GetAudioEndpoints()
        {
            List<string> ep = new List<string>();
            ep.Add("Default");
            return ep;
        }

        public bool SetAudioEndpoint(string dev , bool usedefaultifnotfound = false)
        {
            return true;
        }

        public string GetAudioEndpoint()
        {
            return "Default";
        }

        public void Dispose()
        {
        }

        public void Dispose(AudioData o)
        {
        }

        public void Start(AudioData o, int vol)
        {
        }

        public void Stop()
        {
        }

        public AudioData Generate(string file, SoundEffectSettings effects)
        {
            return null;
        }

        public AudioData Generate(System.IO.Stream audioms, SoundEffectSettings effects, bool ensureaudio)
        {
            return null;
        }

        public AudioData Mix(AudioData last, AudioData mix)
        {
            return null;
        }
        public AudioData Append(AudioData front, AudioData append)
        {
            return null;
        }

        public int Lengthms(AudioData audio)
        {
            return 0;                     // whats the length?
        }

        public int TimeLeftms(AudioData audio)
        {
            return 0;
        }

    }
}
