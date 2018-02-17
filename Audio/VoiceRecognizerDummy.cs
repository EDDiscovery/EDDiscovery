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
    public class VoiceRecognizerDummy : IVoiceRecognition, IDisposable
    {
        #region IDisposable support

        public void Dispose() { SpeechRecognised = SpeechNotRecognised = null; }

        #endregion

        #region IVoiceRecognition support

        public event SpeechRecognised SpeechRecognised;
#pragma warning disable 0414    // The field is assigned but its value is never used.
        public event SpeechRecognised SpeechNotRecognised;
#pragma warning restore 0414
        public bool IsOpen { get { return false; } }
        public float Confidence { get; set; } = 0.98F;
        public int BabbleTimeout { get; set; }
        public int EndSilenceTimeout { get; set; }
        public int EndSilenceTimeoutAmbigious { get; set; }
        public int InitialSilenceTimeout { get; set; }
        public bool Open(System.Globalization.CultureInfo ctp) { return false; }       // Dispose to close
        public bool Start() { return false; }
        public bool Add(string s) { return false; }
        public bool AddRange(List<string> s) { return false; }
        public void Stop(bool stop) { }
        public bool Clear() { return false; }
        public void Close() { }

        #endregion
    }
}
