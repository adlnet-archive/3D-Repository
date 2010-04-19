using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Profile;

namespace Website.Pages
{
    /// <summary>
    /// Summary description for Pages
    /// </summary>
    public class PageBase : System.Web.UI.Page
    {
        protected Panel SearchPanel
        {
            get { return (Panel)this.Master.FindControl("SearchPanel"); }
        }
        protected HtmlControl BodyTag
        {
            get
            {
                if (this.Master != null && this.Master.FindControl("bodyTag") != null)
                {
                    return (HtmlControl)this.Master.FindControl("bodyTag");
                }
                else { return (HtmlControl)this.FindControl("bodyTag"); }
            }
        }
        protected TextBox SearchTextBox
        {
            get { return (TextBox)this.Master.FindControl("SearchTextBox"); }
        }

        //protected string GetUserFirstLastNameByEmail(string email)
        //{
        //    string rv = email.Trim();

        //    //creates profile - need to load via email or username
        //    //ProfileCommon muprofile = (ProfileCommon)ProfileBase.Create(email.Trim(), true);

        //    ProfileInfoCollection pic = null;
        //    pic = ProfileManager.FindProfilesByUserName(ProfileAuthenticationOption.Authenticated, email.Trim());

        //    if (pic != null && pic.Count > 0)
        //    {
        //        ProfileInfo pi = pic[email.Trim()];
        //        if (pi != null)
        //        {
        //            string fName = HttpContext.Current.Profile.Properties["FirstName"].ToString();
        //            string lName = HttpContext.Current.Profile.Properties["LastName"].ToString();
        //        }
        //    }

        //    return rv;
        //}
    }

}
