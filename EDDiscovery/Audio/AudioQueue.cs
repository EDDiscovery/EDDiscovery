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

namespace EDDiscovery.Audio
{
    public class AudioQueue : IDisposable       // Must dispose BEFORE ISoundOut.
    {
        public delegate void SampleStart(AudioQueue sender, Object tag);
        public delegate void SampleOver(AudioQueue sender, Object tag);

        public enum Priority { Low, Normal, High };
        static public Priority GetPriority(string s) { Priority p; if (Enum.TryParse<AudioQueue.Priority>(s, out p)) return p; else return Priority.Normal; }

        public class AudioSample
        {
            public System.IO.Stream ms;
            public Object audiodata;            // held as an object.. driver specific format
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

        public IAudioDriver Driver { get { return ad; } }           // be careful! go thru this class to change anything

        public bool SetAudioEndpoint( string dev )
        {
            bool res = ad.SetAudioEndpoint(dev);
            if (res)
                Clear();
            return res;
        }

        public void Clear()
        {
            foreach( AudioSample a in audioqueue )
            {
                a.SampleOver(this);     // let callers know a sample is over
                ad.Dispose(a.audiodata);        // tell the driver to clean up
                a.ms?.Dispose();            // clean any stream
            }

            audioqueue.Clear();
        }

        private void AudioStoppedEvent()
        {
            //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Stopped audio");

            if (audioqueue.Count > 0) // Normally always have an entry, except on Kill , where queue is gone
            {
                audioqueue[0].SampleOver(this);     // let callers know a sample is over

                ad.Dispose(audioqueue[0].audiodata);        // tell the driver to clean up
                audioqueue[0].ms?.Dispose();        // clean stream

                audioqueue.RemoveAt(0);
            }

            Queue(null);
        }


        private void Queue(AudioSample newdata, Priority p = Priority.Normal )
        {
            if (newdata != null)
            {
                if ( audioqueue.Count > 0 && p > audioqueue[0].priority )       // if something is playing, and we have priority..
                {
                    //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Priority insert " + p + " front " + audioqueue[0].priority);

                    if (audioqueue[0].priority == Priority.Low)                 // if low at front, remove all other lows after it
                    {
                        List<AudioSample> remove = new List<AudioSample>();
                        for (int i = 1; i < audioqueue.Count; i++)
                        {
                            if (audioqueue[i].priority == Priority.Low)
                                remove.Add(audioqueue[i]);
                        }
                        foreach (AudioSample a in remove)
                        {
                            audioqueue.Remove(a);
                            ad.Dispose(a.audiodata);        // tell the driver to clean up
                            a.ms?.Dispose();        // dispose of ms handle
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

        public AudioSample Generate(string file, ConditionVariables effects = null)        // from a file (you get a AS back so you have the chance to add events)
        {
            Object audio = ad.Generate(file, effects);

            if (audio != null)
            {
                AudioSample a = new AudioSample() { ms = null, audiodata = audio };
                return a;
            }
            else
                return null;
        }

        public AudioSample Generate(System.IO.Stream audioms, ConditionVariables effects = null, bool ensuresomeaudio = false)   // from a memory stream
        {
            if (audioms != null)
            {
                Object audio = ad.Generate(audioms, effects, ensuresomeaudio);
                if (audio != null)
                {
                    AudioSample a = new AudioSample() { ms = audioms, audiodata = audio };
                    return a;
                }
            }

            return null;
        }

        public void Submit(AudioSample s, int vol, Priority p)       // submit to queue
        {
            s.volume = vol;
            Queue(s, p);
        }

        public void StopCurrent()
        {
            if (audioqueue.Count > 0)       // if we are playing, stop current
            {
                ad.Stop();
                //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + " Stop current");
            }
        }

        public void StopAll()
        {
            audioqueue.Clear();
            ad.Stop();
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
