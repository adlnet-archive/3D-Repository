using System;
using System.ComponentModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using OrbitOne.OpenId.MembershipProvider;
using System.IO;



namespace OrbitOne.OpenId.Controls
{
    [DefaultProperty("Text"), ToolboxData("<{0}:OpenIDLogin runat=server></{0}:OpenIDLogin>")]
    public class OpenIdLogin : Login
    {
        private string _linkOpenIdPage;


        #region Properties
        public delegate void OnAuthenticatedEvent(object sender, EventArgs args);
        public event OnAuthenticatedEvent OnAuthenticated;
        [Localizable(true), Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Text
        {
            get
            {
                string str = (string)this.ViewState["Text"];
                if (str != null)
                {
                    return str;
                }
                return string.Empty;
            }
            set
            {
                ViewState["Text"] = value;
            }
        }
        public override Orientation Orientation
        {
            get
            {
                return Orientation.Horizontal;
            }
            set
            {
                base.Orientation = Orientation.Horizontal;
            }
        }
        public override string Password
        {
            get
            {
                return "";
            }
        }
        public override string PasswordLabelText
        {
            get
            {
                return "";
            }
            set
            {
                base.PasswordLabelText = "";
            }
        }
        public override bool RememberMeSet
        {
            get
            {
                return false;
            }
            set
            {
                base.RememberMeSet = false;
            }
        }
        public override string RememberMeText
        {
            get
            {
                return "";
            }
            set
            {
                base.RememberMeText = "";
            }
        }
        public override bool DisplayRememberMe
        {
            get
            {
                return false;
            }
            set
            {
                base.DisplayRememberMe = false;
            }
        }
        public string LinkOpenIdPage
        {
            get { return _linkOpenIdPage; }
            set { _linkOpenIdPage = value; }
        }
        #endregion Properties

        #region Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                OpenIdMembershipProvider provider;
                HidePasswordControl();
                if (MembershipProvider == "")
                {
                    provider = (OpenIdMembershipProvider)Membership.Provider;
                }
                else
                {
                    provider = (OpenIdMembershipProvider)Membership.Providers[MembershipProvider];
                }
                if (provider.ValidateOpenIDUser())
                {
                    OnLoggedIn(new EventArgs());
                    if (DestinationPageUrl != "")
                    {
                        Context.Response.Redirect(DestinationPageUrl);
                    }
                    else
                    {
                        Context.Response.Redirect(provider.LoginURL);
                    }
                }
            }
            catch (OpenIdNotLinkedException nlEx)
            {

                Page.Response.Redirect(string.Format("{0}?openId={1}", LinkOpenIdPage, HttpUtility.UrlEncode(nlEx.Message)));
            }
            catch (Exception ex)
            {
                OnLoginError(new EventArgs());
            }
        }
        protected override void OnAuthenticate(AuthenticateEventArgs e)
        {
            try
            {
                base.OnAuthenticate(e);
                if (e.Authenticated && OnAuthenticated != null)
                {
                    OnAuthenticated(this, e);
                }
            }
            catch (OpenIdNotLinkedException nlEx)
            {
                Page.Response.Redirect(string.Format("{0}?openId={1}", LinkOpenIdPage, HttpUtility.UrlEncode(nlEx.Message)));
            }
            catch (Exception ex)
            {
                using (FileStream s = new FileStream(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "log.txt"), FileMode.OpenOrCreate))
                {
                    using (StreamWriter writer = new StreamWriter(s))
                    {
                        writer.WriteLine(ex.Message);
                    }
                }
            }
        }
        protected override void RenderContents(HtmlTextWriter output)
        {
            HidePasswordControl();
            base.RenderContents(output);
        }
        private void HidePasswordControl()
        {
            TextBox box = (TextBox)base.FindControl("Password");
            if (box != null)
            {
                box.Visible = false;
            }
            RequiredFieldValidator validator = (RequiredFieldValidator)base.FindControl("PasswordRequired");
            if (validator != null)
            {
                validator.Visible = false;
            }
        }
        #endregion Methods


    }


}