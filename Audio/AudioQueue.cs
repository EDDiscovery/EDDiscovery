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

namespace AudioExtensions
{
    public enum AudioQueuePriority { Normal = 0, Low = -1, High = 1};

    public class AudioQueue : IDisposable
    {
        /// <summary>
        /// Constructs a new <see cref="AudioQueue"/> with the default parameters.
        /// </summary>
        public AudioQueue() : this(AudioEngine.BestAvail) { }

        /// <summary>
        /// Constructs a new <see cref="AudioQueue"/> with the specified output device using the default
        /// <see cref="AudioEngine"/>.
        /// </summary>
        /// <param name="audioDevice">The audio output device to use.</param>
        public AudioQueue(string audioDevice = null) : this(audioDevice, AudioEngine.BestAvail) { }

        /// <summary>
        /// Constructs a new <see cref="AudioQueue"/> using the specified <see cref="AudioEngine"/> output.
        /// </summary>
        /// <param name="engine">The <see cref="AudioEngine"/> to use for playing audio.</param>
        public AudioQueue(AudioEngine engine = AudioEngine.BestAvail) : this(null, engine) { }

        /// <summary>
        /// Constructs a new <see cref="AudioQueue"/> with the specified <see cref="AudioEngine"/> and output device.
        /// </summary>
        /// <param name="audioDevice">The name of the output device to be used.</param>
        /// <param name="engine">The <see cref="AudioEngine"/> to use for output.</param>
        /// <exception cref="PlatformNotSupportedException">If the provided <paramref name="engine"/> is not supported.</exception>
        public AudioQueue(string audioDevice = null, AudioEngine engine = AudioEngine.BestAvail)
        {
            ad = new AudioDriver(audioDevice, engine);
            ad.AudioStoppedEvent += AudioDriver_AudioStoppedEvent;
        }

        #region IDisposable support

        ~AudioQueue()
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
            if (!_isDisposed)
            {
                if (disposing && ad != null)
                {
                    ad.AudioStoppedEvent -= AudioDriver_AudioStoppedEvent;
                    ad.Dispose();
                }
                ad = null;
                audioqueue = null;

                _isDisposed = true;
            }
        }

        #endregion

        static public AudioQueuePriority GetPriority(string s)
        {
            AudioQueuePriority p;
            if (Enum.TryParse(s, true, out p))
                return p;
            return AudioQueuePriority.Normal;
        }


        public IAudioDriver Driver { get { return ad; } }       // gets driver associated with this queue

        public bool SetAudioEndpoint( string dev )
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(AudioQueue));

            bool res = ad.SetAudioEndpoint(dev);
            if (res)
                Clear();

            return res;
        }

        public void Clear()     // clear queue, does not call the end functions
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(AudioQueue));

            foreach (var a in audioqueue)
            {
                FinishSample(a, false);
            }

            audioqueue.Clear();
        }

        public int InQueuems()       // Length of sound in queue.. does not take account of priority.
        {
            int len = 0;

            //System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);      // UI thread.
            if (!_isDisposed && audioqueue.Count > 0)
            {
                len += ad.TimeLeftms(audioqueue[0].Data);

                for (int i = 1; i < audioqueue.Count; i++)
                    len += ad.Lengthms(audioqueue[i].Data);
            }

            return len;
        }

        public AudioSample Generate(string file, SoundEffectSettings effects = null)        // from a file (you get a AS back so you have the chance to add events)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(AudioQueue));
            AudioData audio = ad.Generate(file, effects);

            if (audio != null)
                return new AudioSample(ad, audio);
            else
                return null;
        }

        public AudioSample Generate(Stream audioms, SoundEffectSettings effects = null, bool ensuresomeaudio = false)   // from a memory stream
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(AudioQueue));

            if (audioms != null)
            {
                AudioData audio = ad.Generate(audioms, effects, ensuresomeaudio);
                if (audio != null)
                {
                    AudioSample a = new AudioSample(ad, audio);
                    a.Handles.Add(audioms);
                    return a;
                }
            }

            return null;
        }

        public AudioSample Append(AudioSample last, AudioSample next)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(AudioQueue));
            AudioData audio = ad.Append(last.Data, next.Data);

            if (audio != null)
            {
                last.Data = audio;
                last.Sources.Add(next);
                return last;
            }
            else
                return null;
        }

        public AudioSample Mix(AudioSample last, AudioSample mix)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(AudioQueue));
            AudioData audio = ad.Mix(last.Data, mix.Data);

            if (audio != null)
            {
                last.Data = audio;
                last.Sources.Add(mix);
                return last;
            }
            else
                return null;
        }

        public void Submit(AudioSample s, int vol, AudioQueuePriority p)       // submit to queue
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(AudioQueue));
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            s.Volume = vol;
            Queue(s, p);
        }

        public void StopCurrent()   // async
        {
            if (_isDisposed)
                return;

            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            if (audioqueue.Count > 0)       // if we are playing, stop current
            {
                ad.Stop();
                //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Stop current");
            }
        }

        public void StopAll() // async
        {
            if (_isDisposed)
                return;

            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            if (audioqueue.Count > 0)
            {
                if (audioqueue.Count > 1)
                    audioqueue.RemoveRange(1, audioqueue.Count - 1);

                ad.Stop();  // async stop
            }
        }

        public bool IsWaiting(AudioSample s)    // is this at the top of my queue?
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(AudioQueue));
            if (audioqueue.Count > 0)
                return Object.ReferenceEquals(audioqueue[0], s);
            else
                return false;
        }

        public void ReleaseHalt()       // other stream is ready, release us for play
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(AudioQueue));
            if (audioqueue.Count > 0)
            {
                audioqueue[0].LinkedQueue = null;   // make sure queue now ignores the link
                Queue(null);
            }
        }


        List<AudioSample> audioqueue = new List<AudioSample>();
        IAudioDriver ad;
        private bool _isDisposed = false;

        private void FinishSample(AudioSample a, bool callback)
        {
            if (callback)
                AudioSample.InvokeSampleFinished(a, this);  // let callers know a sample is over
            a.Dispose();
        }

        private void Queue(AudioSample newdata, AudioQueuePriority p = AudioQueuePriority.Normal)
        {
            //System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);      // UI thread.

            //for (int q = 0; q < audioqueue.Count; q++) System.Diagnostics.Debug.WriteLine(q.ToStringInvariant() + " " + (audioqueue[q].audiodata.data != null) + " " + audioqueue[q].priority);

            if (newdata != null)
            {
                //System.Diagnostics.Debug.WriteLine("Play " + ad.Lengthms(newdata.audiodata) + " in queue " + InQueuems() + " " + newdata.priority);

                if (audioqueue.Count > 0 && p > audioqueue[0].Priority)       // if something is playing, and we have priority..
                {
                    //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Priority insert " + p + " front " + audioqueue[0].priority);

                    if (audioqueue[0].Priority == AudioQueuePriority.Low)                 // if low at front, remove all other lows after it
                    {
                        List<AudioSample> remove = new List<AudioSample>();
                        for (int i = 1; i < audioqueue.Count; i++)
                        {
                            if (audioqueue[i].Priority == AudioQueuePriority.Low)
                            {
                                remove.Add(audioqueue[i]);
                                //System.Diagnostics.Debug.WriteLine("Queue to remove " + i);
                            }
                        }
                        foreach (AudioSample a in remove)
                        {
                            FinishSample(a, false);
                            audioqueue.Remove(a);
                        }
                    }

                    if (audioqueue[0].Priority == AudioQueuePriority.High)                // High playing, don't interrupt, but this one next
                        audioqueue.Insert(1, newdata);  // add one past front
                    else
                    {
                        audioqueue.Insert(1, newdata);  // add one past front
                        ad.Stop();                      // stopping makes it stop, does the callback, this gets called again, audio plays
                    }
                    return;
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine(Environment.TickCount + "AUDIO queue");

                    newdata.Priority = p;
                    audioqueue.Add(newdata);
                    if (audioqueue.Count > 1)       // if not the first in queue, no action yet, let stopped handle it
                        return;
                }
            }

            if (audioqueue.Count > 0)
            {
                var sample = audioqueue[0];
                if (sample.LinkedQueue != null && sample.LinkedSample != null)      // linked to another audio q, both must be at front to proceed
                {
                    if (!sample.LinkedQueue.IsWaiting(sample.LinkedSample))         // if its not on top, don't play it yet
                        return;

                    sample.LinkedQueue.ReleaseHalt();                               // it is waiting, so its stopped.. release halt on other one
                }

                ad.Start(sample.Data, sample.Volume);           // driver, play this
                AudioSample.InvokeSampleStarted(sample, this);  // let callers know a sample started
            }
        }

        private void AudioDriver_AudioStoppedEvent()            //CScore calls then when audio over.
        {
            //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Stopped audio");

            if (!_isDisposed)
            {
                System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);      // UI thread.

                if (audioqueue.Count > 0) // Normally always have an entry, except on Kill , where queue is gone
                {
                    //System.Diagnostics.Debug.WriteLine("Clear audio at 0 depth " + audioqueue.Count);
                    FinishSample(audioqueue[0], true);
                }
                // We may have been disposed of through FinishSample(). don't let that cause problems.
                if (!_isDisposed)
                {
                    audioqueue.RemoveAt(0);
                    Queue(null);
                }
            }
        }
    }
}
