using System;
using System.Web.Security;

namespace OrbitOne.OpenId.MembershipProvider
{
    public class OpenIdMembershipUser : MembershipUser
    {

        private string _openId;

        public string OpenId
        {
            get { return _openId; }
            set { _openId = value; }
        }


        public OpenIdMembershipUser(
          string providerName, 
          string openId,
          string name,
          object providerUserKey,
          string email,
          string passwordQuestion,
          string comment,
          bool isApproved,
          bool isLockedOut,
          DateTime creationDate,
          DateTime lastLoginDate,
          DateTime lastActivityDate,
          DateTime lastPasswordChangedDate,
          DateTime lastLockoutDate)
            : base(
          providerName,
          name,
          providerUserKey,
          email,
          passwordQuestion,
          comment,
          isApproved,
          isLockedOut,
          creationDate,
          lastLoginDate,
          lastActivityDate,
          lastPasswordChangedDate,
          lastLockoutDate)
        {
            OpenId = openId;
        }


    }
}
