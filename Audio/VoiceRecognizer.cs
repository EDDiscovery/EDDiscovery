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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioExtensions
{
    public delegate void SpeechRecognised(string text, float confidence);

    public interface IVoiceRecognition : IDisposable
    {
        event SpeechRecognised SpeechRecognised;
        event SpeechRecognised SpeechNotRecognised;
        bool IsOpen { get; }
        float Confidence { get; set; }
        int BabbleTimeout { get; set; }
        int EndSilenceTimeout { get; set; }
        int EndSilenceTimeoutAmbigious { get; set; }
        int InitialSilenceTimeout { get; set; }

        bool Open(System.Globalization.CultureInfo ctp);     
        bool Add(string s);
        bool AddRange(List<string> s);
        bool Start();       // start recognition
        void Stop(bool waitforstop);    // after stop you can add/start
        bool Clear();       // unload all grammars, must be stopped. then Add/Start
        void Close();   // can close without stop
    }

    /// <summary>
    /// The available speech recognition engines.
    /// </summary>
    public enum VoiceRecognitionEngine
    {
        /// <summary>A NOP voice recognition engine that doesn't do anything.</summary>
        Dummy = -1,
        /// <summary>The best available voice recognition engine supported on the platform.</summary>
        BestAvail = 0,
        /// <summary>The Microsoft <c>System.Speech.Recognition</c> engine. Supported on Windows Vista and newer
        /// when not excluded from compilation. Check <see cref="VoiceRecognizer.Engines"/> for availability.</summary>
        Windows
    }

    /// <summary>
    /// A platform-agnostic voice recognition engine.
    /// </summary>
    public class VoiceRecognizer : IVoiceRecognition, IDisposable
    {
        private IVoiceRecognition _engine;

        /// <summary>
        /// Constructs a new <see cref="VoiceRecognizer"/> using the default parameters.
        /// </summary>
        public VoiceRecognizer() : this(VoiceRecognitionEngine.BestAvail) { }

        /// <summary>
        /// Constructs a new <see cref="VoiceRecognizer"/> using the specified <see cref="VoiceRecognitionEngine"/>.
        /// </summary>
        /// <param name="engine">The <see cref="VoiceRecognitionEngine"/> that should be used.</param>
        /// <exception cref="PlatformNotSupportedException">If <paramref name="engine"/> is not supported on this platform.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="engine"/> was not included in the compilation.</exception>
        public VoiceRecognizer(VoiceRecognitionEngine engine = VoiceRecognitionEngine.BestAvail)
        {
#if !NO_SYSTEM_SPEECH
            if (engine != VoiceRecognitionEngine.Dummy && VoiceRecognizerWindows.IsPlatformSupported)
                _engine = new VoiceRecognizerWindows();
            else if (engine == VoiceRecognitionEngine.Windows)
                throw new PlatformNotSupportedException("The Windows voice recognition engine requires Windows Vista or newer.");
#else
            if (engine == VoiceRecognitionEngine.Windows)
                throw new ArgumentOutOfRangeException(nameof(engine), "The Windows voice recognition engine was not included in this build.");
#endif
            else
                _engine = new VoiceRecognizerDummy();
        }

        /// <summary>
        /// Gets the available <see cref="VoiceRecognitionEngine"/> types that were included in this build, in order of
        /// desirability. Note that some of the available engines may not be supported on this platform.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of available engines.</returns>
        public static List<VoiceRecognitionEngine> Engines
        {
            get
            {
                return new List<VoiceRecognitionEngine>()
                {
                    VoiceRecognitionEngine.BestAvail,
#if !NO_SYSTEM_SPEECH
                    VoiceRecognitionEngine.Windows,
#endif
                    VoiceRecognitionEngine.Dummy
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

        #region IVoiceRecognition support

        public event SpeechRecognised SpeechRecognised { add { if (_engine != null) _engine.SpeechRecognised += value; } remove { if (_engine != null) _engine.SpeechRecognised -= value; } }
        public event SpeechRecognised SpeechNotRecognised { add { if (_engine != null) _engine.SpeechNotRecognised += value; } remove { if (_engine != null) _engine.SpeechNotRecognised -= value; } }

        public bool IsOpen { get { return _engine?.IsOpen ?? false; } }
        public float Confidence { get { return _engine?.Confidence ?? 0.96f; } set { if (_engine != null) _engine.Confidence = value; } }
        public int BabbleTimeout { get { return _engine?.BabbleTimeout ?? -1; } set { if (_engine != null) _engine.BabbleTimeout = value; } }
        public int EndSilenceTimeout { get { return _engine?.EndSilenceTimeout ?? -1; } set { if (_engine != null) _engine.EndSilenceTimeout = value; } }
        public int EndSilenceTimeoutAmbigious { get { return _engine?.EndSilenceTimeoutAmbigious ?? -1; } set { if (_engine != null) _engine.EndSilenceTimeoutAmbigious = value; } }
        public int InitialSilenceTimeout { get { return _engine?.InitialSilenceTimeout ?? -1; } set { if (_engine != null) _engine.InitialSilenceTimeout = value; } }

        public bool Open(CultureInfo ctp) { return _engine?.Open(ctp) ?? false; }
        public bool Add(string s) { return _engine?.Add(s) ?? false; }
        public bool AddRange(List<string> s) { return _engine?.AddRange(s) ?? false; }
        public bool Start() { return _engine?.Start() ?? false; }
        public void Stop(bool waitforstop) { _engine?.Stop(waitforstop); }
        public bool Clear() { return _engine?.Clear() ?? false; }
        public void Close() { _engine?.Close(); }

        #endregion
    }
}
