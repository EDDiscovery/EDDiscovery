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
    public delegate void SpeechRecognisedHandler(string text, float confidence);

    public interface IVoiceRecogniser
    {
        event SpeechRecognisedHandler SpeechRecognised;
        bool IsOpen { get; }
        float Confidence { get; set; }
        bool Open(CultureInfo ctp);        // Dispose to close
        bool Add(string s);
        bool AddRange(List<string> s);
        bool Start();
        void Stop();    // after stop you can add/start
        void Close();   // can close without stop
    }
}
