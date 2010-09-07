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
using vwarDAL;

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
        protected IDataRepository DAL
        {
            get
            {
                if (Session["DAL"] == null)
                {
                    var factory = new DataAccessFactory();
                    Session["DAL"] = factory.CreateDataRepositorProxy();
                }
                return Session["DAL"] as IDataRepository;
            }
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
    public class ControlBase : System.Web.UI.UserControl
    {
        protected IDataRepository DAL
        {
            get
            {
                if (Session["DAL"] == null)
                {
                    var factory = new DataAccessFactory();
                    Session["DAL"] = factory.CreateDataRepositorProxy();
                }
                return Session["DAL"] as IDataRepository;
            }
        }
    }

    public class Types
    {


        public static string Default = "~/Default.aspx";

        //public
        public static string Contact = "~/Public/Contact.aspx";
        public static string ProfileImageHandlerURL = "~/Public/ProfileImageHandler.ashx?UserID={0}&Logo={1}";
        public static string Model = "~/Public/Model.aspx";


        //Administrators
        public static string AdministratorsDefault = "~/Administrators/Default.aspx";
        public static string ManageUsers = "~/Administrators/ManageUsers.aspx";
        public static string ManageAdministrativeUsers = "~/Administrators/ManageAdministrativeUsers.aspx";

        //Users
        public static string AdvancedSearch = "~/Users/AdvancedSearch.aspx";
        public static string Profile = "~/Users/Profile.aspx";
        public static string ChangePassword = "~/Users/ChangePassword.aspx";

        public static string FormatEmail(object emailText)
        {
            string rv = String.Empty;

            if (emailText != System.DBNull.Value)
            {
                if (!String.IsNullOrEmpty(emailText.ToString().Trim()) && Website.Mail.IsValidEmail(emailText.ToString().Trim()))
                {

                    rv = "mailto:" + emailText;

                }

            }

            return rv;

        }




        public static string FormatProfileUrl(object userID)
        {
            return Types.Profile + "?UserID=" + userID.ToString().Trim();

        }


        /// <summary>
        /// This method formats the image handler url
        /// </summary>
        /// <param name="userID">The UserProfile ID</param>
        /// <param name="logoNameString">Either "Developer" or "Sponsor"</param>
        /// <returns></returns>
        public static string FormatProfileImageHandler(string userID, string logoNameString)
        {
            string rv = "";

            if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(logoNameString))
            {
                rv = string.Format(ProfileImageHandlerURL, userID, logoNameString);

            }

            return rv;

        }


        public static string FormatModel(string contentObjID)
        {
            string rv = Model;

            if (!string.IsNullOrEmpty(contentObjID))
            {

                rv = rv + "?ContentObjectID=" + contentObjID;

            }


            return rv;
        }



    }


}
