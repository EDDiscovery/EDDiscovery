using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 0067

namespace AudioExtensions
{
    public delegate void SpeechRecognised(string text, float confidence);

    public interface IVoiceRecognition
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


    public class VoiceRecognitionDummy: IVoiceRecognition
    {
        public event SpeechRecognised SpeechRecognised;
        public event SpeechRecognised SpeechNotRecognised;
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
    }

}
