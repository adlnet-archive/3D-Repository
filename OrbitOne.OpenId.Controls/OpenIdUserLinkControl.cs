using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using OrbitOne.OpenId.MembershipProvider;
namespace OrbitOne.OpenId.Controls
{
    public  class OpenIdUserLinkControl : System.Web.UI.UserControl
    {
        private static readonly string HEADTEXT =
            "The OpenId: <b>{0}</b> you are trying to login with is not linked with any site user, please link this OpenId with your site user account:";

        private string _openId;
        private string _openIdmembershipProvider;


        protected Login LinkOpenIdLogin;
        protected Label headLabel;
        
        
        
        
        public string OpenId
        {
            get { return _openId; }
            set { _openId = value; }
        }
        [System.ComponentModel.Browsable(true),
             System.ComponentModel.Description("OpenId MembershipProvider name")]
        public string OpenIdMembershipProvider
        {
            get { return _openIdmembershipProvider; }
            set { _openIdmembershipProvider = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            OpenId = GetOpenIdFromQueryString();
            headLabel.Text = string.Format(HEADTEXT, OpenId);

        }

        protected void LinkOpenIdLogin_LoggedIn(object sender, EventArgs e)
        {
            OpenIdMembershipProvider provider = (OpenIdMembershipProvider)Membership.Providers[OpenIdMembershipProvider];
            MembershipUser user = Membership.Providers[LinkOpenIdLogin.MembershipProvider].GetUser(LinkOpenIdLogin.UserName, false);
            provider.LinkUserWithOpenId(OpenId, user.ProviderUserKey);
            FormsAuthentication.RedirectFromLoginPage(user.UserName, false);

        }



        private string GetOpenIdFromQueryString()
        {
            return Server.UrlDecode(Page.Request.QueryString["openId"]);
        }



    }
}