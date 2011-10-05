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
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;
using System.Web.UI;


namespace Website
{
    /// <summary>
    /// 
    /// </summary>
    public class Config
    {
        /// <summary>
        /// company details 
        /// </summary>
        public static string SiteName = AppSettings["SiteName"];
        /// <summary>
        /// 
        /// </summary>
        public static string Slogan = AppSettings["Slogan"];
        /// <summary>
        /// 
        /// </summary>
        public static string CompanyName = AppSettings["CompanyName"];
        /// <summary>
        /// 
        /// </summary>
        public static string CompanyEmail = AppSettings["CompanyEmail"];
        /// <summary>
        /// 
        /// </summary>
        public static string SupportEmail = AppSettings["SupportEmail"];
        /// <summary>
        /// 
        /// </summary>
        public static string CompanyAddress = AppSettings["CompanyAddress"].Replace("|", "<BR />");
        /// <summary>
        /// 
        /// </summary>
        public static string CompanyPhone = AppSettings["CompanyPhone"];
        /// <summary>
        /// 
        /// </summary>
        public static string CompanyFax = AppSettings["CompanyFax"];
        /// <summary>
        /// 
        /// </summary>
        public static string ContactUsViewMapUrlText = AppSettings["ContactUsViewMapUrlText"];
        /// <summary>
        /// 
        /// </summary>
        public static string ContactUsViewMapUrl = AppSettings["ContactUsViewMapUrl"];
        /// <summary>
        /// 
        /// </summary>
        public static readonly string ConversionLibarayLocation = AppSettings["LibraryLocation"];
        /// <summary>
        /// page head 
        /// </summary>
        public static string SiteKeywords = AppSettings["SiteKeywords"];
        /// <summary>
        /// 
        /// </summary>
        public static string SiteDescription = AppSettings["SiteDescription"];
        /// <summary>
        /// 
        /// </summary>
        public static string HomeRobotsMetaTagValue = AppSettings["HomeRobotsMetaTagValue"];
        /// <summary>
        /// 
        /// </summary>
        public static string NonHomeRobotsMetaTagValue = AppSettings["NonHomeRobotsMetaTagValue"];
        /// <summary>
        /// 
        /// </summary>
        public static string PageTitleFormatString = AppSettings["PageTitleFormatString"];
        /// <summary>
        /// 
        /// </summary>
        public static string GoogleAnalyticsAccountID = AppSettings["GoogleAnalyticsAccountID"];
        /// <summary>
        /// mail
        /// </summary>
        public static string DefaultEmailFromAddress = AppSettings["DefaultEmailFromAddress"];
        /// <summary>
        /// 
        /// </summary>
        public static string SupportEmailFromAddress = AppSettings["SupportEmailFromAddress"];
        /// <summary>
        /// 
        /// </summary>
        public static string DefaultEmailToAddress = AppSettings["DefaultEmailToAddress"];
        /// <summary>
        /// 
        /// </summary>
        public static bool SendEmailForNewRegistrations = GetSafeBoolean("SendEmailForNewRegistrations");
        /// <summary>
        /// 
        /// </summary>
        public static string NewRegistrationNotificationEmailToAddress = AppSettings["NewRegistrationNotificationEmailToAddress"];
        /// <summary>
        /// support
        /// </summary>
        public static string PSSupportEmail = AppSettings["PSSupportEmail"];
        /// <summary>
        /// 
        /// </summary>
        public static string SupportRequestEmailHeader = AppSettings["SupportRequestEmailHeader"];
        /// <summary>
        /// 
        /// </summary>
        public static string SupportRequestEmailFooter = AppSettings["SupportRequestEmailFooter"];
        /// <summary>
        /// development mail settings
        /// </summary>
        public static bool EmailingActive = GetSafeBoolean("EmailingActive");
        /// <summary>
        /// production mail settings
        /// </summary>
        public static string ProductionEmailSmtpServer = AppSettings["ProductionEmailSmtpServer"];
        /// <summary>
        /// 
        /// </summary>
        public static string ProductionEmailUsername = AppSettings["ProductionEmailUsername"];
        /// <summary>
        /// 
        /// </summary>
        public static string ProductionEmailPassword = AppSettings["ProductionEmailPassword"];
        /// <summary>
        /// 
        /// </summary>
        public static bool UseWebServersBuiltInSmtpMailServer = GetSafeBoolean("UseWebServersBuiltInSmtpMailServer");
        /// <summary>
        /// web parts
        /// </summary>
        public static bool HideHeaderWhenEditingWebPartPage = GetSafeBoolean("HideHeaderWhenEditingWebPartPage");
        /// <summary>
        /// 
        /// </summary>
        public static string WebPartPageQueryStringKey = AppSettings["WebPartPageQueryStringKey"];
        /// <summary>
        /// 
        /// </summary>
        public static string DefaultWebPartAuthorizedViewRoles = AppSettings["DefaultWebPartAuthorizedViewRoles"];
        /// <summary>
        /// 
        /// </summary>
        public static bool EnableWebParts = GetSafeBoolean("EnableWebParts");
        /// <summary>
        /// 
        /// </summary>
        public static bool UseDefaultWebPartChrome = GetSafeBoolean("UseDefaultWebPartChrome");
        /// <summary>
        /// 
        /// </summary>
        public static string WebPartCacheTimes = AppSettings["WebPartCacheTimes"];
        /// <summary>
        /// 
        /// </summary>
        public static int WebPartCatalogTop = GetSafeInteger("WebPartCatalogTop");
        /// <summary>
        /// 
        /// </summary>
        public static int WebPartCatalogLeft = GetSafeInteger("WebPartCatalogLeft");
        /// <summary>
        /// login 
        /// </summary>
        public static string LoginDestinationPageUrl = AppSettings["LoginDestinationPageUrl"];
        /// <summary>
        /// 
        /// </summary>
        public static bool LoginSetPageFocus = GetSafeBoolean("LoginSetPageFocus");
        /// <summary>
        /// Membership 
        /// </summary>
        public static bool MembershipUserApprovedByDefault = GetSafeBoolean("MembershipUserApprovedByDefault");
        /// <summary>
        /// 
        /// </summary>
        public static bool GenerateDefaultAdministratorOnApplicationStartup = GetSafeBoolean("GenerateDefaultAdministratorOnApplicationStartup");
        /// <summary>
        /// cybrarian
        /// </summary>
        public static string CybrarianEmail = AppSettings["CybrarianEmail"];
        /// <summary>
        /// site details
        /// </summary>
        public static string DomainName
        {
            get { return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority); }
        }
        /// <summary>
        /// menu 
        /// </summary>
        public static int PrimaryMenuMaximumDynamicDisplayLevels = GetSafeInteger("PrimaryMenuMaximumDynamicDisplayLevels");
        /// <summary>
        /// 
        /// </summary>
        public static bool PrimaryMenuUseImages = GetSafeBoolean("PrimaryMenuUseImages");
        /// <summary>
        /// FckEditor 
        /// </summary>
        public static string FCKeditorUserFilesPath = AppSettings["FCKeditorUserFilesPath"];
        /// <summary>
        /// page settings
        /// </summary>
        public static bool ShowSubMenuTableRow = GetSafeBoolean("ShowSubMenuTableRow");
        /// <summary>
        /// 
        /// </summary>
        public static bool ShowSiteMapPathTableRow = GetSafeBoolean("ShowSiteMapPathTableRow");
        /// <summary>
        /// content management settings
        /// </summary>
        public static int HomePageID = GetSafeInteger("HomePageID");
        /// <summary>
        /// caching
        /// </summary>
        public static bool EnableCaching = GetSafeBoolean("EnableCaching");
        /// <summary>
        /// 
        /// </summary>
        public static string SiteMapCacheKey = AppSettings["SiteMapCacheKey"];
        /// <summary>
        /// event log
        /// </summary>
        public static int EventLogMaximumRows = GetSafeInteger("EventLogMaximumRows");
        /// <summary>
        /// security 
        /// </summary>
        public static bool QuerystringEncryptionEnabled = GetSafeBoolean("QuerystringEncryptionEnabled");
        /// <summary>
        /// 
        /// </summary>
        public static string AspnetUserAccount = HttpContext.Current.Server.MachineName + "/aspnet";
        /// <summary>
        /// 
        /// </summary>
        public static string PostgreSQLConnectionString = ConfigurationManager.ConnectionStrings["postgreSQLConnectionString"].ConnectionString;
        /// <summary>
        /// 
        /// </summary>
        public static string CommunityUrl = AppSettings["CommunityUrl"];
        /// <summary>
        /// rss 
        /// </summary>
        public static bool EnableRss = GetSafeBoolean("EnableRss");
        /// <summary>
        /// themes 
        /// </summary>
        public static bool AllowUsersToChangeTheme = GetSafeBoolean("AllowUsersToChangeTheme");
        /// <summary>
        /// 
        /// </summary>
        public static string DefaultThemeName = AppSettings["DefaultThemeName"];
        /// <summary>
        /// globalization
        /// </summary>
        public static string DefaultCulture = AppSettings["DefaultCulture"];
        /// <summary>
        /// 
        /// </summary>
        public static string SupportedCultures = AppSettings["SupportedCultures"];
        /// <summary>
        /// 
        /// </summary>
        private static NameValueCollection AppSettings
        {
            get { return ConfigurationManager.AppSettings; }
        }
        /// <summary>
        /// 3d config 
        /// </summary>
        public static int MaxTextureDimension = 512;
        /// <summary>
        /// 
        /// </summary>
        public static int MaxNumberOfPolygons = GetSafeInteger("MaxNumberOfPolygons");
        /// <summary>
        /// 
        /// </summary>
        public static bool IsHomePage
        {
            get
            {
                HttpContext context = HttpContext.Current;
                bool isHome = context.Request.ServerVariables["SCRIPT_NAME"].ToLower().Equals(((Page)context.Handler).ResolveUrl("~/Default.aspx").ToLower());
                isHome = isHome & context.Request.QueryString[Website.Config.WebPartPageQueryStringKey] == null;
                //HttpContext.Current.Trace.Warn("IsHomePage", isHome.ToString)
                return isHome;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool IsAdministratorsDefault
        {
            get
            {
                bool isadmin = false;
                try
                {
                    isadmin = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToLower().Equals(((Page)HttpContext.Current.Handler).ResolveUrl("~/Administrators/Default.aspx").ToLower());
                }
                catch (Exception ex)
                {

                }
                return isadmin;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool IsAnyAdministratorPage
        {
            get
            {
                bool rv = false;
                Page p = null;
                try
                {
                    p = (Page)HttpContext.Current.Handler;
                }
                catch (Exception ex)
                {

                }

                if (p != null)
                {
                    try
                    {
                        rv = p.AppRelativeTemplateSourceDirectory.Replace("/", "").Replace("~", "").ToLower().Equals("administrators");
                    }
                    catch (Exception ex)
                    {

                    }
                }

                return rv;
            }
        }
        /// <summary>
        /// 
        /// </summary>
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private static bool GetSafeBoolean(string keyName)
        {
            bool rv = false;
            string configStringValue = AppSettings[keyName];

            if (!configStringValue.Equals(string.Empty))
            {
                try
                {
                    rv = bool.Parse(configStringValue);
                }
                catch (Exception ex)
                {

                }
            }

            return rv;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        private static int GetSafeInteger(string keyName)
        {
            int rv = 0;
            string configStringValue = AppSettings[keyName];

            if (!configStringValue.Equals(string.Empty))
            {
                try
                {
                    rv = int.Parse(configStringValue);
                }
                catch (Exception ex)
                {

                }
            }

            return rv;
        }
    }
}
