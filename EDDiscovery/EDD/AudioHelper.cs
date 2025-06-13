/*
 * Copyright 2019 EDDiscovery development team
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
 */

using AudioExtensions;
using AudioTest;
using System;
using System.Runtime.CompilerServices;

namespace EDDiscovery
{
    /*
     * Add a layer of indirection to prevent a TypeLoadException due to static initializer hoisting
     */
    public static class AudioHelper
    {
        public static ISpeechEngine GetWindowsSpeechEngine(Action<string> log)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5)
            {
                try
                {
                    return AudioHelperWindowsProxy.GetWindowsSpeechEngine();
                }
                catch (Exception ex)
                {
                    log($"Error initializing Windows speech synthesis engine: {ex}");
                    return new DummySpeechEngine();
                }
            }
            else
            {
                return new DummySpeechEngine();
            }
        }

        public static ISpeechEngine GetWindowsMediaSpeechEngine(Action<string> log)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5)
            {
                try
                {
                    return AudioHelperWindowsProxy.GetWindowsMediaSpeechEngine();
                }
                catch (Exception ex)
                {
                    log($"Error initializing Windows media speech synthesis engine: {ex}");
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
                    log($"Error initializing Windows speech recognition engine: {ex}");
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
                    log($"Error initializing CSCore Audio driver: {ex}");
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
        public static ISpeechEngine GetWindowsSpeechEngine()
        {
#if !NO_SYSTEM_SPEECH
            return new WindowsSpeechEngine();
#else
            return new DummySpeechEngine();
#endif
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static ISpeechEngine GetWindowsMediaSpeechEngine()
        {
#if !NO_SYSTEM_SPEECH
            return new WindowsMediaSpeechEngine();
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
