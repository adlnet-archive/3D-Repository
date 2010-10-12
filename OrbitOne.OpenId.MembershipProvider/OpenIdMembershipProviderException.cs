using System;
using System.Collections.Generic;
using System.Text;

namespace OrbitOne.OpenId.MembershipProvider
{
    class OpenIdMembershipProviderException : Exception
    {
       
        private string _Message = "";
        private string _Source = "";
        private string _StackTrace = "";

        public override string Message
        {
            get { return _Message; }
        }

        public override string Source
        {
            get { return _Source; }
        }

        public override string StackTrace
        {
            get { return _StackTrace; }
        }

        public OpenIdMembershipProviderException(string pMessage, string Source, string StackTrace)
        {      
            _Message = pMessage;
            _Source = Source;
            _StackTrace = StackTrace;
            
        }
    }
}

