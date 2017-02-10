using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.Audio
{
    public interface ISpeechEngine
    {
        string[] GetVoiceNames();
        System.IO.MemoryStream Speak(string phrase, string voice , int volume, int rate);
    }

    public class SpeechSynthesizer
    {
        ISpeechEngine speechengine;
        Random rnd = new Random();

        public SpeechSynthesizer( ISpeechEngine engine )
        {
            speechengine = engine;
        }

        public string[] GetVoiceNames()
        {
            return speechengine.GetVoiceNames();
        }

        public System.IO.MemoryStream Speak(string phraselist, string voice, int rate, out string errlist,
                                            ConditionFunctions f = null, ConditionVariables curvars = null )
        {
            string res = phraselist;
            if (f == null || f.ExpandString(phraselist, curvars, out res) != EDDiscovery.ConditionLists.ExpandResult.Failed)       //Expand out.. and if no errors
            {
                string[] phrasearray = res.Split(';');

                if (phrasearray.Length > 1)     // if we have at least x;y
                {
                    if (phrasearray[0].Length == 0 && phrasearray.Length >= 2)   // first empty, and we have two or more..
                    {
                        res = phrasearray[1];           // say first one
                        if (phrasearray.Length > 2)   // if we have ;first;second;third, pick random at then
                        {
                            res += phrasearray[2 + rnd.Next(phrasearray.Length - 2)];
                        }
                    }
                    else
                        res = phrasearray[rnd.Next(phrasearray.Length)];    // pick randomly
                }

                errlist = null;
                return speechengine.Speak(res, voice, 100, rate);     // samples are always generated at 100 volume
            }
            else
            {
                errlist = res;
                return null;
            }
        }


    }
}
