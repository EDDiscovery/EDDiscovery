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
using System.Collections.Generic;
using System.Globalization;

namespace AudioExtensions
{
    public class VoiceRecognitionDummy: IVoiceRecogniser
    {
#pragma warning disable 0067        // CS0067: The event is never used.
        public event SpeechRecognisedHandler SpeechRecognised;
#pragma warning restore 0067
        public bool IsOpen { get { return false; } }
        public float Confidence { get; set; } = 0.98F;
        public bool Open(CultureInfo ctp) { return false; }       // Dispose to close
        public bool Start() { return false; }
        public bool Add(string s) { return false; }
        public bool AddRange(List<string> s) { return false; }
        public void Stop() { }
        public void Close() { }
    }

}
