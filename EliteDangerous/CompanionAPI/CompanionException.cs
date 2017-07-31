/*
 * Copyright © 2016 EDDiscovery development team
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


namespace EliteDangerousCore.CompanionAPI
{
    /// <summary>Base class for exceptions thrown by the Elite:Dangerous companion app API</summary>
    public class CompanionAppException : Exception
    {
        public CompanionAppException() : base() { }

        public CompanionAppException(string message) : base(message) { }
    }

    /// <summary>Exceptions thrown due to API errors</summary>
    public class CompanionAppErrorException : CompanionAppException
    {
        public CompanionAppErrorException() : base() { }

        public CompanionAppErrorException(string message) : base(message) { }
    }

    /// <summary>Exceptions thrown due to illegal service state</summary>
    public class CompanionAppIllegalStateException : CompanionAppException
    {
        public CompanionAppIllegalStateException() : base() { }

        public CompanionAppIllegalStateException(string message) : base(message) { }
    }


    /// <summary>Exceptions thrown due to authentication errors</summary>
    public class CompanionAppAuthenticationException : CompanionAppException
    {
        public CompanionAppAuthenticationException() : base() { }

        public CompanionAppAuthenticationException(string message) : base(message) { }
    }

}
