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
    public interface ISpeechEngine : IDisposable
    {
        string[] GetVoiceNames();

        MemoryStream Speak(string phrase, string culture, string voice, int volume, int rate);
    }

    /// <summary>
    /// The available text-to-speech output engines.
    /// </summary>
    public enum SpeechEngine
    {
        /// <summary>A NOP text-to-speech engine that doesn't do anything.</summary>
        Dummy = -1,
        /// <summary>The best available text-to-speech engine that is supported on this platform.</summary>
        BestAvail = 0,
        /// <summary>The Microsoft <c>System.Speech.Synthesis</c> engine. Supported on Windows 2000 and newer platforms
        /// when not excluded from compilation. Check <see cref="SpeechSynthesizer.Engines"/> for availability.</summary>
        Windows
    }

    /// <summary>
    /// A platform-agnostic speech synthesis engine.
    /// </summary>
    public class SpeechSynthesizer : ISpeechEngine, IDisposable
    {
        private ISpeechEngine _engine;

        /// <summary>
        /// Constructs a new <see cref="SpeechSynthesizer"/> instance with the default parameters.
        /// </summary>
        public SpeechSynthesizer() : this(SpeechEngine.BestAvail) { }

        /// <summary>
        /// Constructs a new <see cref="SpeechSynthesizer"/> using the specified <see cref="SpeechEngine"/>.
        /// </summary>
        /// <param name="engine">The <see cref="SpeechEngine"/> that should be used.</param>
        /// <exception cref="PlatformNotSupportedException">If <paramref name="engine"/> is not supported on this platform.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="engine"/> was not included in the compilation.</exception>
        public SpeechSynthesizer(SpeechEngine engine = SpeechEngine.BestAvail)
        {
#if !NO_SYSTEM_SPEECH
            if (engine != SpeechEngine.Dummy && SpeechSynthesizerWindows.IsPlatformSupported)
                _engine = new SpeechSynthesizerWindows();
            else if (engine == SpeechEngine.Windows)
                throw new PlatformNotSupportedException("The Windows speech synthesis engine requires Windows 2000 or newer.");
#else
            if (engine == SpeechEngine.Windows)
                throw new ArgumentOutOfRangeException(nameof(engine), "The Windows speech synthesis engine was not included in this build.");
#endif
            else
                _engine = new SpeechSynthesizerDummy();
        }

        /// <summary>
        /// Gets the available <see cref="SpeechEngine"/> types that were included in this build, in order of desirability.
        /// Note that some of the available engines may not be supported on this platform.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of available engines.</returns>
        public static List<SpeechEngine> Engines
        {
            get
            {
                return new List<SpeechEngine>()
                {
                    SpeechEngine.BestAvail,
#if !NO_SYSTEM_SPEECH
                    SpeechEngine.Windows,
#endif
                    SpeechEngine.Dummy
                };
            }
        }

        public MemoryStream Speak(string say, string culture, string voice, int rate)     // may return null
        {
            return Speak(say, culture, voice, 100, rate);     // samples are always generated at 100 volume
        }

        #region IDisposable support

        public void Dispose()
        {
            _engine?.Dispose();
            _engine = null;
        }

        #endregion

        #region ISpeechEngine support

        public string[] GetVoiceNames()
        {
            return _engine?.GetVoiceNames() ?? new string[] { };
        }

        public MemoryStream Speak(string phrase, string culture, string voice, int volume, int rate)
        {
            return _engine?.Speak(phrase, culture, voice, volume, rate);
        }

        #endregion
    }
}
