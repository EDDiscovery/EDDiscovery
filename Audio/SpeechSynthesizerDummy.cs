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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioExtensions
{
    public class SpeechSynthesizerDummy : ISpeechEngine, IDisposable
    {
        #region IDisposable support

        public void Dispose() { }

        #endregion

        #region ISpeechEngine support

        public string[] GetVoiceNames() { return new string[] { }; }

        public MemoryStream Speak(string phrase, string culture, string voice, int volume, int rate) { return null; }

        #endregion
    }
}
