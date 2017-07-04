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
using CSCore;
using CSCore.CoreAudioAPI;
using CSCore.DirectSound;
using CSCore.SoundOut;
using CSCore.Streams;
using CSCore.Streams.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Audio
{
    class AudioDriverCSCore : IAudioDriver, IDisposable
    {
        public event AudioStopped AudioStoppedEvent;

        ISoundOut aout;

        public AudioDriverCSCore(string dev = null)
        {
            SetAudioEndpoint(dev,true);
        }

        public List<string> GetAudioEndpoints()
        {
            List<string> ep = new List<string>();
            IReadOnlyCollection<DirectSoundDevice> list = DirectSoundDeviceEnumerator.EnumerateDevices();
            foreach ( DirectSoundDevice d in list )
                ep.Add(d.Description);

            return ep;
        }

        public bool SetAudioEndpoint(string dev, bool usedefault = false)
        {
#if true
            System.Collections.ObjectModel.ReadOnlyCollection<DirectSoundDevice> list = DirectSoundDeviceEnumerator.EnumerateDevices();

            DirectSoundDevice dsd = null;
            if (dev != null)       // active selection
            {
                dsd = list.FirstOrDefault(x => x.Description.Equals(dev));        // find
                if (dsd == null && !usedefault) // if not found, and don't use the default (used by constructor)
                    return false;
            }

            DirectSoundOut dso = new DirectSoundOut(200, System.Threading.ThreadPriority.Highest);    // seems good quality at 200 ms latency

            if ( dsd != null )
                dso.Device = dsd.Guid;
#else
            MMDevice def = MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Render, Role.Console);
            ISoundOut dso = new WasapiOut() { Latency = 100, Device = def  }; //BAD breakup
#endif

            if (aout != null)                 // clean up last
            {
                aout.Stopped -= Output_Stopped;
                aout.Stop();
                aout.Dispose();
            }

            aout = dso;
            aout.Stopped += Output_Stopped;

            return true;
        }

        public string GetAudioEndpoint()
        {
            Guid guid = ((DirectSoundOut)aout).Device;
            System.Collections.ObjectModel.ReadOnlyCollection<DirectSoundDevice> list = DirectSoundDeviceEnumerator.EnumerateDevices();
            DirectSoundDevice dsd = list.First(x => x.Guid == guid);
            return dsd.Description;
        }


        private void Output_Stopped(object sender, PlaybackStoppedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString("00000") + "Driver stopped");
            if (AudioStoppedEvent != null)
                AudioStoppedEvent();
        }

        public void Dispose()
        {
            aout.Dispose();
        }

        public void Dispose(Object o)
        {
            IWaveSource iws = o as IWaveSource;
            iws.Dispose();
            //System.Diagnostics.Debug.WriteLine("Audio disposed");
        }

        public void Start(Object o, int vol)
        {
            int t = Environment.TickCount;

            IWaveSource current = o as IWaveSource;
            aout.Initialize(current);
            //System.Diagnostics.Debug.WriteLine((Environment.TickCount-t).ToString("00000") + "Driver Init done");
            aout.Volume = (float)(vol) / 100;
            aout.Play();
            //System.Diagnostics.Debug.WriteLine((Environment.TickCount - t).ToString("00000") + "Driver Play done");
        }

        public void Stop()
        {
            aout.Stop();
        }

        // FROM file
        public Object Generate(string file, ConditionVariables effects)
        {
            try
            {
                IWaveSource s = CSCore.Codecs.CodecFactory.Instance.GetCodec(file);
                ApplyEffects(ref s, effects);
                return s;
            }
            catch
            {
                return null;
            }
        }

        // FROM audio stream
        public Object Generate(System.IO.Stream audioms, ConditionVariables effects, bool ensureaudio)
        {
            try
            {
                audioms.Position = 0;

                IWaveSource s;

                if (audioms.Length == 0)
                {
                    if (ensureaudio)
                        s = new NullWaveSource(50);
                    else
                        return null;
                }
                else
                {
                    s = new CSCore.Codecs.WAV.WaveFileReader(audioms);

                    //System.Diagnostics.Debug.WriteLine("oRIGINAL length " + s.Length);
                    //if (ensureaudio)
                      //  s = s.AppendSource(x => new ExtendWaveSource(x, 100));          // SEEMS to help the click at end.. (removing for now)
                }

                //System.Diagnostics.Debug.WriteLine("Sample length " + s.Length);
                ApplyEffects(ref s, effects);
                //System.Diagnostics.Debug.WriteLine(".. to length " + s.Length);
                return s;
            }
            catch( Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.Message);
                if (ensureaudio)
                    return new NullWaveSource(50);
                else
                    return null;
            }
        }

        public Object Append(Object front, Object end)      // this adds END to Front.   Format is changed to END
        {
            if (front == null || end == null)
                return null;

            IWaveSource frontws = (IWaveSource)front;
            IWaveSource mixws = (IWaveSource)end;

            if (frontws.WaveFormat.Channels < mixws.WaveFormat.Channels)      // need to adapt to previous format
                frontws = frontws.ToStereo();
            else if (frontws.WaveFormat.Channels > mixws.WaveFormat.Channels)
                frontws = frontws.ToMono();

            if (mixws.WaveFormat.SampleRate != frontws.WaveFormat.SampleRate || mixws.WaveFormat.BitsPerSample != frontws.WaveFormat.BitsPerSample)
                frontws = ChangeSampleDepth(frontws, mixws.WaveFormat.SampleRate, mixws.WaveFormat.BitsPerSample);

            return new AppendWaveSource(frontws, mixws);
        }

        public Object Mix(Object front, Object mix)     // This mixes mix with Front.  Format changed to MIX.
        {
            if (front == null || mix == null)
                return null;

            IWaveSource frontws = (IWaveSource)front;
            IWaveSource mixws = (IWaveSource)mix;

            if (frontws.WaveFormat.Channels < mixws.WaveFormat.Channels)      // need to adapt to previous format
                frontws = frontws.ToStereo();
            else if (frontws.WaveFormat.Channels > mixws.WaveFormat.Channels)
                frontws = frontws.ToMono();

            if (mixws.WaveFormat.SampleRate != frontws.WaveFormat.SampleRate || mixws.WaveFormat.BitsPerSample != frontws.WaveFormat.BitsPerSample)
                frontws = ChangeSampleDepth(frontws, mixws.WaveFormat.SampleRate, mixws.WaveFormat.BitsPerSample);

            return new MixWaveSource(frontws, mixws);
        }

        static private void ApplyEffects(ref IWaveSource src, ConditionVariables effect)
        {
            if (effect != null)
            {
                SoundEffectSettings ap = new SoundEffectSettings(effect);

                int extend = 0;
                if (ap.echoenabled)
                    extend = ap.echodelay * 2;
                if (ap.chorusenabled)
                    extend = Math.Max(extend, 50);
                if (ap.reverbenabled)
                    extend = Math.Max(extend, 50);

                if (extend > 0)
                {
                    //System.Diagnostics.Debug.WriteLine("Extend by " + extend + " ms due to effects");
                    src = src.AppendSource(x => new ExtendWaveSource(x, extend));
                }

                if (ap.chorusenabled)
                {
                    src = src.AppendSource(x => new DmoChorusEffect(x) { WetDryMix = ap.chorusmix, Feedback = ap.chorusfeedback, Delay = ap.chorusdelay, Depth = ap.chorusdepth });
                }

                if (ap.reverbenabled)
                {
                    src = src.AppendSource(x => new DmoWavesReverbEffect(x) { InGain = 0, ReverbMix = ap.reverbmix, ReverbTime = ((float)ap.reverbtime) / 1000.0F, HighFrequencyRTRatio = ((float)ap.reverbhfratio) / 1000.0F });
                }

                if (ap.distortionenabled)
                {
                    src = src.AppendSource(x => new DmoDistortionEffect(x) { Gain = ap.distortiongain, Edge = ap.distortionedge, PostEQCenterFrequency = ap.distortioncentrefreq, PostEQBandwidth = ap.distortionfreqwidth });
                }

                if (ap.gargleenabled)
                {
                    src = src.AppendSource(x => new DmoGargleEffect(x) { RateHz = ap.garglefreq });
                }

                if (ap.echoenabled)
                {
                    src = src.AppendSource(x => new DmoEchoEffect(x) { WetDryMix = ap.echomix, Feedback = ap.echofeedback, LeftDelay = ap.echodelay, RightDelay = ap.echodelay });
                }

                if ( ap.pitchshiftenabled )
                {
                    ISampleSource srs = src.ToSampleSource();
                    srs = srs.AppendSource(x => new PitchShifter(x) { PitchShiftFactor = ((float)ap.pitchshift)/100.0F });
                    src = srs.ToWaveSource();
                }
            }
        }

        public static IWaveSource ChangeSampleDepth(IWaveSource input, int destinationSampleRate , int bitdepth)    // replace INPUT with return
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (destinationSampleRate <= 0)
                throw new ArgumentOutOfRangeException("destinationSampleRate");

            if (input.WaveFormat.SampleRate == destinationSampleRate)
                return input;

            WaveFormat w = new WaveFormat(destinationSampleRate, bitdepth, input.WaveFormat.Channels);
            return new CSCore.DSP.DmoResampler(input, w);
        }
    }

    public class ExtendWaveSource : WaveAggregatorBase  // based on the TrimmedWaveSource in the CS Example
    {
        long extrabytes;
        long totalbytes;

        public ExtendWaveSource(IWaveSource ws, int ms) : base(ws)
        {
            extrabytes = ws.WaveFormat.MillisecondsToBytes(ms);
            totalbytes = base.Length + extrabytes;
            //System.Diagnostics.Debug.WriteLine("Extend by " + extrabytes + " at " + ws.WaveFormat.SampleRate);
        }

        public override long Length { get { return totalbytes; } }

        public override int Read(byte[] buffer, int offset, int count)      // PLAY base, until exausted, then play fill
        {
            int read = BaseSource.Read(buffer, offset, count);      // want count, stored at offset
            //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString() + " Extend " + offset + " Read " + count);

            if (read < count)
            {
                int left = count - read;      // what is left
                int totake = Math.Min(left, (int)extrabytes);

                //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString() + " ++read " + read + " left " + left + " extend " + totake + " left " + (extrabytes - totake));

                if (totake > 0)
                {
                    Array.Clear(buffer, offset + read, totake);     // at offset+read, clear down to zero the extra bytes
                    extrabytes -= totake;
                }

                return read + totake;       // returned this total
            }

            return read;
        }
    }


    public class AppendWaveSource : WaveAggregatorBase  // based on the TrimmedWaveSource in the CS Example
    {
        IWaveSource append;

        public AppendWaveSource(IWaveSource ws, IWaveSource o) : base(ws)
        {
            append = o;
        }

        public override long Length { get { return base.Length + append.Length; } }

        public override int Read(byte[] buffer, int offset, int count)      // play BASE, then play append
        {
            int read = BaseSource.Read(buffer, offset, count);      // want count, stored at offset
            //System.Diagnostics.Debug.WriteLine((Environment.TickCount % 10000).ToString() + " Copy:Read " + count);

            if (read < count)
            {
                return append.Read(buffer, read, count - read);
            }
            else
                return read;
        }
    }

    public class MixWaveSource : WaveAggregatorBase  // based on the TrimmedWaveSource in the CS Example
    {
        IWaveSource mix;

        public MixWaveSource(IWaveSource ws, IWaveSource o) : base(ws)
        {
            mix = o;
        }

        public override long Length { get { return base.Length; } }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int readbase = BaseSource.Read(buffer, offset, count);      // want count, stored at offset

            if ( readbase > 0 )         // mix into data the mix source.. ensure the mix source never runs out.
            {
                byte[] buffer2 = new byte[readbase];

                int storepos = 0;
                int left = readbase;

                while (left > 0)
                {
                    //System.Diagnostics.Debug.WriteLine("Read mix at " + mix.Position + " for " + left);
                    int readmix = mix.Read(buffer2, storepos, left);      // and read in the readmix..
                    //System.Diagnostics.Debug.WriteLine(".. read " + readmix);

                    left -= readmix;

                    if (left>0) // if we have any left, means we need to loop it
                    {
                        mix.Position = 0;
                        storepos += readmix;
                    }
                }

                if (BaseSource.WaveFormat.BytesPerSample == 2)                  // FOR NOW, presuming its PCM, cope with a few different formats.
                {
                    for (int i = 0; i < readbase; i += 2)
                    {
                        short v1 = BitConverter.ToInt16(buffer, i + offset);
                        short v2 = BitConverter.ToInt16(buffer2, i);
                        v1 += v2;
                        var bytes = BitConverter.GetBytes(v1);
                        buffer[i + offset] = bytes[0];
                        buffer[i + offset + 1] = bytes[1];
                    }
                }
                else if (BaseSource.WaveFormat.BytesPerSample == 4)
                {
                    for (int i = 0; i < readbase; i += 4)
                    {
                        long v1 = BitConverter.ToInt32(buffer, i + offset);
                        long v2 = BitConverter.ToInt32(buffer2, i);
                        v1 += v2;
                        var bytes = BitConverter.GetBytes(v1);
                        buffer[i + offset] = bytes[0];
                        buffer[i + offset + 1] = bytes[1];
                        buffer[i + offset + 2] = bytes[2];
                        buffer[i + offset + 3] = bytes[3];
                    }
                }
                else 
                {
                    for (int i = 0; i < readbase; i += 1)
                        buffer[i + offset] += buffer2[i];
                }
            }

            return readbase;
        }

    }


    public class NullWaveSource : IWaveSource  // empty audio
    {
        long pos = 0;
        long totalbytes = 0;
        private readonly WaveFormat _waveFormat;

        public NullWaveSource(int ms) 
        {
            _waveFormat = new WaveFormat(22050,16, 1, AudioEncoding.Pcm);       // default format
            totalbytes = _waveFormat.MillisecondsToBytes(ms);
        }

        public WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }


        public int Read(byte[] buffer, int offset, int count)
        {
            if ( pos < totalbytes)
            {
                int totake = Math.Min(count, (int)(totalbytes - pos));      // and we return zeros

                if ( totake > 0 )
                {
                    Array.Clear(buffer, offset , totake);     // at offset+read, clear down to zero the extra bytes
                }

                pos += totake;
                return totake;
            }

            return 0;
        }

        public long Length { get { return totalbytes; } set { } }
        public long Position { get { return pos; } set { } }
        public bool CanSeek
        {
            get { return false; }
        }

        public void Dispose()
        {
        }
    }


}
