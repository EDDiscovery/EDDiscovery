/*
 * Copyright © 2017 - 2018 EDDiscovery development team
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioExtensions
{
    public delegate void AudioStopped();

    public interface IAudioDriver : IDisposable
    {
        event AudioStopped AudioStoppedEvent;

        void Start(AudioData o, int vol);      // start with this audio
        void Stop();                           // async stop.. does not stop, just terminates the current audio
        void Dispose(AudioData o);             // finish with this audio

        AudioData Generate(string file, SoundEffectSettings effects = null);       // generate audio samples and return. Effects 
        AudioData Generate(Stream audioms, SoundEffectSettings effects = null, bool ensuresomeaudio = false); // generate audio and return with effect and ensuring audio if req.

        AudioData Mix(AudioData last, AudioData mix);            // either may be null, in which case null.. Converted to mix format
        AudioData Append(AudioData front, AudioData append);      // either may be null, in which case null.. Converted to append format

        int Lengthms(AudioData audio);                     // whats the length?
        int TimeLeftms(AudioData audio);

        string GetAudioEndpoint();
        List<string> GetAudioEndpoints();
        bool SetAudioEndpoint(string device, bool usedefaultifnotfound = false);
    }

    /// <summary>
    /// The available audio output engines.
    /// </summary>
    public enum AudioEngine
    {
        /// <summary>A NOP audio output engine that doesn't do anything.</summary>
        Dummy = -1,
        /// <summary>The best available output engine that is supported on the platform.</summary>
        BestAvail = 0,
        /// <summary>The <c>CSCore.DirectSound</c> engine. Supported on Windows 2000 and greater and compiled into
        /// every build..</summary>
        CSCore
    }

    /// <summary>
    /// A platform-agnostic audio output engine.
    /// </summary>
    public class AudioDriver : IAudioDriver, IDisposable
    {
        private IAudioDriver _engine;

        /// <summary>
        /// Constructs a new <see cref="AudioDriver"/> instance using the default parameters.
        /// </summary>
        public AudioDriver() : this(null) { }

        /// <summary>
        /// Constructs a new <see cref="AudioDriver"/> instance using the specified output device name.
        /// </summary>
        /// <param name="dev">The name of the output device to be used.</param>
        public AudioDriver(string dev = null) : this(dev, AudioEngine.BestAvail) { }

        /// <summary>
        /// Constructs a new <see cref="AudioDriver"/> instance using the specified device and <see cref="AudioEngine"/>.
        /// </summary>
        /// <param name="dev">The name of the output device to be used.</param>
        /// <param name="engine">The <see cref="AudioEngine"/> to be used.</param>
        /// <exception cref="PlatformNotSupportedException">If the specified <paramref name="engine"/> isn't supported on this platform.</exception>
        public AudioDriver(string dev = null, AudioEngine engine = AudioEngine.BestAvail)
        {
            if (engine != AudioEngine.Dummy && AudioDriverCSCore.IsPlatformSupported)
                _engine = new AudioDriverCSCore(dev);
            else if (engine == AudioEngine.CSCore)
                throw new PlatformNotSupportedException("The CSCore output engine is not supported on this platform.");
            else
                _engine = new AudioDriverDummy();
        }

        /// <summary>
        /// Gets the available <see cref="AudioEngine"/> types that were included in this build, in order of desirability.
        /// Note that some of the available engines may not be supported on this platform.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of available engines.</returns>
        public static List<AudioEngine> Engines
        {
            get
            {
                return new List<AudioEngine>() {
                    AudioEngine.BestAvail,
                    AudioEngine.CSCore,
                    AudioEngine.Dummy
                };
            }
        }

        #region IDisposable support

        public void Dispose()
        {
            _engine?.Dispose();
            _engine = null;
        }

        #endregion

        #region IAudioDriver support

        public event AudioStopped AudioStoppedEvent { add { if (_engine != null) _engine.AudioStoppedEvent += value; } remove { if (_engine != null) _engine.AudioStoppedEvent -= value; } }

        public void Start(AudioData o, int vol) { _engine?.Start(o, vol); }
        public void Stop() { _engine?.Stop(); }
        public void Dispose(AudioData o) { _engine?.Dispose(o); }

        public AudioData Generate(string file, SoundEffectSettings effects = null)
        {
            return _engine?.Generate(file, effects);
        }
        public AudioData Generate(Stream audioms, SoundEffectSettings effects = null, bool ensuresomeaudio = false)
        {
            return _engine?.Generate(audioms, effects, ensuresomeaudio);
        }

        public AudioData Mix(AudioData last, AudioData mix) { return _engine?.Mix(last, mix); }
        public AudioData Append(AudioData front, AudioData append) { return _engine?.Append(front, append); }

        public int Lengthms(AudioData audio) { return _engine?.Lengthms(audio) ?? 0; }
        public int TimeLeftms(AudioData audio) { return _engine?.TimeLeftms(audio) ?? 0; }


        public string GetAudioEndpoint() { return _engine?.GetAudioEndpoint(); }
        public List<string> GetAudioEndpoints() { return _engine?.GetAudioEndpoints(); }
        public bool SetAudioEndpoint(string device, bool usedefaultifnotfound = false) { return _engine?.SetAudioEndpoint(device, usedefaultifnotfound) ?? false; }

        #endregion
    }
}
