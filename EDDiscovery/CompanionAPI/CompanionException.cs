using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.CompanionAPI
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
