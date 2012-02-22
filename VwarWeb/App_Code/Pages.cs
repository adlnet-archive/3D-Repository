//  Copyright 2011 U.S. Department of Defense

//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at

//      http://www.apache.org/licenses/LICENSE-2.0

//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.



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
        /// <summary>
        /// 
        /// </summary>
        protected Panel SearchPanel
        {
            get { return (Panel)this.Master.FindControl("SearchPanel"); }
        }
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        protected TextBox SearchTextBox
        {
            get { return (TextBox)this.Master.FindControl("SearchTextBox"); }
        }
        /// <summary>
        /// 
        /// </summary>
        
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
    /// <summary>
    /// 
    /// </summary>
    public class ControlBase : System.Web.UI.UserControl
    {
        
    }
    /// <summary>
    /// 
    /// </summary>
    public class Types
    {
        /// <summary>
        /// 
        /// </summary>
        public static string Default = "~/Default.aspx";
        /// <summary>
        /// public 
        /// </summary>
        public static string Contact = "~/Public/Contact.aspx";
        /// <summary>
        /// 
        /// </summary>
        public static string ProfileImageHandlerURL = "~/Public/ProfileImageHandler.ashx?UserID={0}&Logo={1}";
        /// <summary>
        /// 
        /// </summary>
        public static string Model = "~/Public/Model.aspx";
        /// <summary>
        /// Administrators 
        /// </summary>
        public static string AdministratorsDefault = "~/Administrators/Default.aspx";
        /// <summary>
        /// 
        /// </summary>
        public static string ManageUsers = "~/Administrators/ManageUsers.aspx";
        /// <summary>
        /// 
        /// </summary>
        public static string ManageAdministrativeUsers = "~/Administrators/ManageAdministrativeUsers.aspx";
        /// <summary>
        /// Users 
        /// </summary>
        public static string AdvancedSearch = "~/Public/AdvancedSearch.aspx";
        /// <summary>
        /// 
        /// </summary>
        public static string Profile = "~/Users/Profile.aspx";
        /// <summary>
        /// 
        /// </summary>
        public static string ChangePassword = "~/Users/ChangePassword.aspx";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailText"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentObjID"></param>
        /// <returns></returns>
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
