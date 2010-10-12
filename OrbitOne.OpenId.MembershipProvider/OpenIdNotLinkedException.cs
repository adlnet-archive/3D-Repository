using System;

namespace OrbitOne.OpenId.MembershipProvider
{
    public class OpenIdNotLinkedException : Exception
    {
       public OpenIdNotLinkedException(string message):base(message){}
    }
}
