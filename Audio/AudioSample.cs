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
    public class AudioSample : IDisposable
    {
        public AudioSample(IAudioDriver driver, AudioData dat)
        {
            _audioDriver = driver;
            Data = dat;
        }

        public AudioData Data { get; set; } = null;                 // the audio data, in _audioDriver format
        public List<Stream> Handles { get; private set; } = new List<Stream>();  // audio samples held in files to free
        public AudioQueue LinkedQueue { get; set; } = null;         // Link to another sample & queue. If both are set, this sample's queue will halt until the
        public AudioSample LinkedSample { get; set; } = null;       // linked sample is at the top of it's queue, then both are released to play together.
        public AudioQueuePriority Priority { get; set; } = AudioQueuePriority.Normal;
        public List<AudioSample> Sources { get; private set; } = new List<AudioSample>();   // Samples that were mixed/appended together to comprise this sample.
        public int Volume { get; set; } = 60;                       // 0-100

        public object StartTag { get; set; } = null;
        public event EventHandler<AudioSampleEventArgs> SampleStarted;
        public object FinishTag { get; set; } = null;
        public event EventHandler<AudioSampleEventArgs> SampleFinished;

        private IAudioDriver _audioDriver;

        public static void InvokeSampleFinished(AudioSample s, AudioQueue q)
        {
            if (s != null)
                s.SampleFinished?.Invoke(s, new AudioSampleEventArgs(s, q, s.FinishTag));
        }

        public static void InvokeSampleStarted(AudioSample s, AudioQueue q)
        {
            if (s != null)
                s.SampleStarted?.Invoke(s, new AudioSampleEventArgs(s, q, s.StartTag));
        }

        #region IDisposable support

        ~AudioSample()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Data != null && _audioDriver != null)
                _audioDriver.Dispose(Data);

            if (disposing)
            {
                if (Sources != null)
                {
                    foreach (var s in Sources)
                        s.Dispose();
                    Sources.Clear();
                }

                if (Handles != null)
                {
                    foreach (var h in Handles)
                        h.Dispose();
                    Handles.Clear();
                }
            }

            Data = null;
            Handles = null;
            LinkedQueue = null;
            LinkedSample = null;
            Sources = null;
            SampleStarted = SampleFinished = null;
            StartTag = FinishTag = null;
            _audioDriver = null;
        }

        #endregion
    }

    public class AudioSampleEventArgs : EventArgs
    {
        public AudioQueue Queue { get; private set; }
        public AudioSample Sample { get; private set; }
        public object Tag { get; private set; }

        public AudioSampleEventArgs(AudioSample sample, AudioQueue queue, object tag = null)
        {
            this.Sample = sample;
            this.Queue = queue;
            this.Tag = tag;
        }
    }
}
