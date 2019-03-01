using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AudioExtensions;

namespace EDDiscovery.Actions
{
    /*
     * Add a layer of indirection to prevent a TypeLoadException due to static initializer hoisting
     */

    public static class AudioHelper
    {
        public static ISpeechEngine GetSpeechEngine(Action<string> log)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5)
            {
                try
                {
                    return AudioHelperWindowsProxy.GetSpeechEngine();
                }
                catch (Exception ex)
                {
                    log(String.Format("Error initializing Windows speech synthesis engine: {0}\nSpeech synthesis will be unavailable", ex.Message));
                    return new DummySpeechEngine();
                }
            }
            else
            {
                return new DummySpeechEngine();
            }
        }

        public static IVoiceRecognition GetVoiceRecognition(Action<string> log)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6)
            {
                try
                {
                    return AudioHelperWindowsProxy.GetVoiceRecognition();
                }
                catch (Exception ex)
                {
                    log(String.Format("Error initializing Windows speech recognition engine: {0}\nSpeech recognition will be unavailable", ex.Message));
                    return new VoiceRecognitionDummy();
                }
            }
            else
            {
                return new VoiceRecognitionDummy();
            }
        }

        public static IAudioDriver GetAudioDriver(Action<string> log, string dev = null)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5)
            {
                try
                {
                    return AudioHelperWindowsProxy.GetAudioDriver(dev);
                }
                catch (Exception ex)
                {
                    log(String.Format("Error initializing CSCore Audio driver: {0}\nAudio will be unavailable", ex.Message));
                    return new AudioDriverDummy();
                }
            }
            else
            {
                return new AudioDriverDummy();
            }
        }
    }

    public static class AudioHelperWindowsProxy
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ISpeechEngine GetSpeechEngine()
        {
#if !NO_SYSTEM_SPEECH
            return new WindowsSpeechEngine();
#else
            return new DummySpeechEngine();
#endif
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IVoiceRecognition GetVoiceRecognition()
        {
#if !NO_SYSTEM_SPEECH
            return new VoiceRecognitionWindows();
#else
            return new VoiceRecognitionDummy();
#endif
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static IAudioDriver GetAudioDriver(string dev = null)
        {
#if !NO_SYSTEM_SPEECH
            return new AudioDriverCSCore(dev);
#else
            return new AudioDriverDummy();
#endif
        }
    }
}
