using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 0067

namespace AudioExtensions
{
    public delegate void SpeechRecognised(string text, float confidence);

    public interface VoiceRecognition
    {
        event SpeechRecognised SpeechRecognised;
        bool IsOpen { get; }
        float Confidence { get; set; }
        bool Open(System.Globalization.CultureInfo ctp);        // Dispose to close
        bool Add(string s);
        bool AddRange(List<string> s);
        bool Start();
        void Stop(bool waitforstop);    // after stop you can add/start
        void Close();   // can close without stop
    }


    public class VoiceRecognitionDummy: VoiceRecognition
    {
        public event SpeechRecognised SpeechRecognised;
        public bool IsOpen { get { return false; } }
        public float Confidence { get; set; } = 0.98F;
        public bool Open(System.Globalization.CultureInfo ctp) { return false; }       // Dispose to close
        public bool Start() { return false; }
        public bool Add(string s) { return false; }
        public bool AddRange(List<string> s) { return false; }
        public void Stop(bool stop) { }
        public void Close() { }
    }

}
