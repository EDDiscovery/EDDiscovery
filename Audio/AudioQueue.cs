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

namespace AudioExtensions
{
    public class AudioQueue : IDisposable       // Must dispose BEFORE ISoundOut.
    {
        public delegate void SampleStart(AudioQueue sender, Object tag);
        public delegate void SampleOver(AudioQueue sender, Object tag);

        public enum Priority { Low, Normal, High };
        static public Priority GetPriority(string s) { Priority p; if (Enum.TryParse<AudioQueue.Priority>(s, true, out p)) return p; else return Priority.Normal; }

        public class AudioSample
        {
            public List<System.IO.Stream> handlelist = new List<System.IO.Stream>();   // audio samples held in files to free
            public AudioData audiodata;         // the audio data, in the driver format
            public int volume;                  // 0-100
            public Priority priority;           // audio priority

            public event SampleStart sampleStartEvent;
            public object sampleStartTag;

            public void SampleStart(AudioQueue q)
            {
                if (sampleStartEvent != null)
                    sampleStartEvent(q, sampleStartTag);
            }

            public event SampleOver sampleOverEvent;
            public object sampleOverTag;

            public void SampleOver(AudioQueue q)
            {
                if (sampleOverEvent != null)
                    sampleOverEvent(q, sampleOverTag);
            }

            public void FreeHandles()
            {
                foreach (System.IO.Stream i in handlelist)
                    i.Dispose();
                handlelist.Clear();
            }

            public AudioSample linkeds;             // link to another queue. if set, we halt the queue if thissample is not t at the top of the list
            public AudioQueue linkedq;              // of the other queue, then release them to play together.
        }

        List<AudioSample> audioqueue;
        IAudioDriver ad;

        public AudioQueue(IAudioDriver adp)
        {
            ad = adp;
            ad.AudioStoppedEvent += AudioStoppedEvent;
            audioqueue = new List<AudioSample>();
        }

        public IAudioDriver Driver { get { return ad; } }       // gets driver associated with this queue

        public bool SetAudioEndpoint( string dev )
        {
            bool res = ad.SetAudioEndpoint(dev);
            if (res)
                Clear();
            return res;
        }

        public void Clear()     // clear queue, does not call the end functions
        {
            foreach( AudioSample a in audioqueue )
            {
                FinishSample(a,false);
            }

            audioqueue.Clear();
        }

        private void FinishSample(AudioSample a , bool callback )
        {
            if ( callback )
                a.SampleOver(this);     // let callers know a sample is over

            a.FreeHandles();
            ad.Dispose(a.audiodata);        // tell the driver to clean up
        }

        private void AudioStoppedEvent()            //CScore calls then when audio over.
        {
            //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Stopped audio");

            //System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);      // UI thread.

            if (audioqueue.Count > 0) // Normally always have an entry, except on Kill , where queue is gone
            {
                FinishSample(audioqueue[0],true);

                //System.Diagnostics.Debug.WriteLine("Clear audio at 0 depth " + audioqueue.Count);

                audioqueue.RemoveAt(0);
            }

            Queue(null);
        }


        private void Queue(AudioSample newdata, Priority p = Priority.Normal )
        {
            //System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);      // UI thread.

            //for (int q = 0; q < audioqueue.Count; q++) System.Diagnostics.Debug.WriteLine(q.ToStringInvariant() + " " + (audioqueue[q].audiodata.data != null) + " " + audioqueue[q].priority);

            if (newdata != null)
            {
                //System.Diagnostics.Debug.WriteLine("Play " + ad.Lengthms(newdata.audiodata) + " in queue " + InQueuems() + " " + newdata.priority);

                if ( audioqueue.Count > 0 && p > audioqueue[0].priority )       // if something is playing, and we have priority..
                {
                    //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Priority insert " + p + " front " + audioqueue[0].priority);

                    if (audioqueue[0].priority == Priority.Low)                 // if low at front, remove all other lows after it
                    {
                        List<AudioSample> remove = new List<AudioSample>();
                        for (int i = 1; i < audioqueue.Count; i++)
                        {
                            if (audioqueue[i].priority == Priority.Low)
                            {
                                remove.Add(audioqueue[i]);
                                //System.Diagnostics.Debug.WriteLine("Queue to remove " + i);
                            }
                        }
                        foreach (AudioSample a in remove)
                        {
                            FinishSample(a,false);
                            audioqueue.Remove(a);
                        }
                    }

                    if (audioqueue[0].priority == Priority.High)                // High playing, don't interrupt, but this one next
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
                    
                    newdata.priority = p;
                    audioqueue.Add(newdata);
                    if (audioqueue.Count > 1)       // if not the first in queue, no action yet, let stopped handle it
                        return;
                }
            }

            if (audioqueue.Count > 0)
            {
                if (audioqueue[0].linkedq != null && audioqueue[0].linkeds != null)       // linked to another audio q, both must be at front to proceed
                {
                    if (!audioqueue[0].linkedq.IsWaiting(audioqueue[0].linkeds))        // if its not on top, don't play it yet
                        return;

                    audioqueue[0].linkedq.ReleaseHalt();        // it is waiting, so its stopped.. release halt on other one
                }

                ad.Start(audioqueue[0].audiodata, audioqueue[0].volume);    // driver, play this
                audioqueue[0].SampleStart(this);     // let callers know a sample started
            }
        }

        public int InQueuems()       // Length of sound in queue.. does not take account of priority.
        {
            //System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);      // UI thread.

            int len = 0;
            if (audioqueue.Count > 0)
                len = ad.TimeLeftms(audioqueue[0].audiodata);

            for (int i = 1; i < audioqueue.Count; i++)
                len += ad.Lengthms(audioqueue[i].audiodata);
            return len;
        }

        public AudioSample Generate(string file, SoundEffectSettings effects = null)        // from a file (you get a AS back so you have the chance to add events)
        {
            AudioData audio = ad.Generate(file, effects);

            if (audio != null)
            {
                AudioSample a = new AudioSample() { audiodata = audio };
                return a;
            }
            else
                return null;
        }

        public AudioSample Generate(System.IO.Stream audioms, SoundEffectSettings effects = null, bool ensuresomeaudio = false)   // from a memory stream
        {
            if (audioms != null)
            {
                AudioData audio = ad.Generate(audioms, effects, ensuresomeaudio);
                if (audio != null)
                {
                    AudioSample a = new AudioSample() { audiodata = audio };
                    a.handlelist.Add(audioms);
                    return a;
                }
            }

            return null;
        }

        public AudioSample Append(AudioSample last, AudioSample next)
        {
            AudioData audio = ad.Append(last.audiodata, next.audiodata);

            if (audio != null)
            {
                last.audiodata = audio;
                last.handlelist.AddRange(next.handlelist);
                next.handlelist.Clear();
                return last;
            }
            else
                return null;
        }

        public AudioSample Mix(AudioSample last, AudioSample mix)
        {
            AudioData audio = ad.Mix(last.audiodata, mix.audiodata);

            if (audio != null)
            {
                last.audiodata = audio;
                last.handlelist.AddRange(mix.handlelist);
                mix.handlelist.Clear();
                return last;
            }
            else
                return null;
        }

        public void Submit(AudioSample s, int vol, Priority p)       // submit to queue
        {
            s.volume = vol;
            Queue(s, p);
        }

        public void StopCurrent()   // async
        {
            if (audioqueue.Count > 0)       // if we are playing, stop current
            {
                ad.Stop();
                //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Stop current");
            }
        }

        public void StopAll() // async
        {
            if ( audioqueue.Count>0)
            {
                 if (audioqueue.Count > 1)
                    audioqueue.RemoveRange(1, audioqueue.Count - 1);

                ad.Stop();  // async stop
            }
        }

        public void Dispose()
        {
            ad.AudioStoppedEvent -= AudioStoppedEvent;
        }

        public bool IsWaiting(AudioSample s)    // is this at the top of my queue?
        {
            if (audioqueue.Count > 0)
                return Object.ReferenceEquals(audioqueue[0], s);
            else
                return false;
        }

        public void ReleaseHalt()       // other stream is ready, release us for play
        {
            if (audioqueue.Count > 0)
            {
                audioqueue[0].linkedq = null;   // make sure queue now ignores the link
                Queue(null);
            }
        }
    }
}
