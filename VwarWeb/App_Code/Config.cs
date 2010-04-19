using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;
using System.Web.UI;


namespace Website
{

    public class Config
    {

        //company details
        public static string SiteName = AppSettings["SiteName"];
        public static string Slogan = AppSettings["Slogan"];
        public static string CompanyName = AppSettings["CompanyName"];
        public static string CompanyEmail = AppSettings["CompanyEmail"];
        public static string SupportEmail = AppSettings["SupportEmail"];
        public static string CompanyAddress = AppSettings["CompanyAddress"].Replace("|", "<BR />");
        public static string CompanyPhone = AppSettings["CompanyPhone"];
        public static string CompanyFax = AppSettings["CompanyFax"];
        public static string ContactUsViewMapUrlText = AppSettings["ContactUsViewMapUrlText"];
        public static string ContactUsViewMapUrl = AppSettings["ContactUsViewMapUrl"];

        //page head
        public static string SiteKeywords = AppSettings["SiteKeywords"];
        public static string SiteDescription = AppSettings["SiteDescription"];
        public static string HomeRobotsMetaTagValue = AppSettings["HomeRobotsMetaTagValue"];
        public static string NonHomeRobotsMetaTagValue = AppSettings["NonHomeRobotsMetaTagValue"];
        public static string PageTitleFormatString = AppSettings["PageTitleFormatString"];
        public static string GoogleAnalyticsAccountID = AppSettings["GoogleAnalyticsAccountID"];

        //mail
        public static string PSTestEmail = AppSettings["PSTestEmail"];
        public static string DefaultEmailFromAddress = AppSettings["DefaultEmailFromAddress"];
        public static string SupportEmailFromAddress = AppSettings["SupportEmailFromAddress"];
        public static string DefaultEmailToAddress = AppSettings["DefaultEmailToAddress"];
        public static bool SendEmailForNewRegistrations = GetSafeBoolean("SendEmailForNewRegistrations");
        public static string NewRegistrationNotificationEmailToAddress = AppSettings["NewRegistrationNotificationEmailToAddress"];

        //support
        public static string PSSupportEmail = AppSettings["PSSupportEmail"];
        public static string SupportRequestEmailHeader = AppSettings["SupportRequestEmailHeader"];
        public static string SupportRequestEmailFooter = AppSettings["SupportRequestEmailFooter"];

        //development mail settings
        public static bool EmailingActive = GetSafeBoolean("EmailingActive");
        public static bool TestEmailingActive = GetSafeBoolean("TestEmailingActive");
        public static string TestEmailSmtpServer = AppSettings["TestEmailSmtpServer"];
        public static string TestEmailUsername = AppSettings["TestEmailUsername"];
        public static string TestEmailPassword = AppSettings["TestEmailPassword"];

        //production mail settings
        public static string ProductionEmailSmtpServer = AppSettings["ProductionEmailSmtpServer"];
        public static string ProductionEmailUsername = AppSettings["ProductionEmailUsername"];
        public static string ProductionEmailPassword = AppSettings["ProductionEmailPassword"];
        public static bool UseWebServersBuiltInSmtpMailServer = GetSafeBoolean("UseWebServersBuiltInSmtpMailServer");

        //web parts
        public static bool HideHeaderWhenEditingWebPartPage = GetSafeBoolean("HideHeaderWhenEditingWebPartPage");
        public static string WebPartPageQueryStringKey = AppSettings["WebPartPageQueryStringKey"];
        public static string DefaultWebPartAuthorizedViewRoles = AppSettings["DefaultWebPartAuthorizedViewRoles"];
        public static bool EnableWebParts = GetSafeBoolean("EnableWebParts");
        public static bool UseDefaultWebPartChrome = GetSafeBoolean("UseDefaultWebPartChrome");
        public static string WebPartCacheTimes = AppSettings["WebPartCacheTimes"];
        public static int WebPartCatalogTop = GetSafeInteger("WebPartCatalogTop");
        public static int WebPartCatalogLeft = GetSafeInteger("WebPartCatalogLeft");

        //login
        public static string LoginDestinationPageUrl = AppSettings["LoginDestinationPageUrl"];
        public static bool LoginSetPageFocus = GetSafeBoolean("LoginSetPageFocus");

        //site details
        public static string DomainName
        {
            get { return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority); }
        }

        //menu
        public static int PrimaryMenuMaximumDynamicDisplayLevels = GetSafeInteger("PrimaryMenuMaximumDynamicDisplayLevels");
        public static bool PrimaryMenuUseImages = GetSafeBoolean("PrimaryMenuUseImages");

        //FckEditor
        public static string FCKeditorUserFilesPath = AppSettings["FCKeditorUserFilesPath"];

        //page settings
        public static bool ShowSubMenuTableRow = GetSafeBoolean("ShowSubMenuTableRow");
        public static bool ShowSiteMapPathTableRow = GetSafeBoolean("ShowSiteMapPathTableRow");

        //content management settings
        public static int HomePageID = GetSafeInteger("HomePageID");

        //caching
        public static bool EnableCaching = GetSafeBoolean("EnableCaching");
        public static string SiteMapCacheKey = AppSettings["SiteMapCacheKey"];

        //event log
        public static int EventLogMaximumRows = GetSafeInteger("EventLogMaximumRows");

        //security
        public static bool QuerystringEncryptionEnabled = GetSafeBoolean("QuerystringEncryptionEnabled");
        public static string AspnetUserAccount = HttpContext.Current.Server.MachineName + "/aspnet";
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public static string EntityConnectionString = ConfigurationManager.ConnectionStrings["vwarEntities"].ConnectionString;

        //rss
        public static bool EnableRss = GetSafeBoolean("EnableRss");

        //themes
        public static bool AllowUsersToChangeTheme = GetSafeBoolean("AllowUsersToChangeTheme");
        public static string DefaultThemeName = AppSettings["DefaultThemeName"];

        //globalization
        public static string DefaultCulture = AppSettings["DefaultCulture"];
        public static string SupportedCultures = AppSettings["SupportedCultures"];

        private static NameValueCollection AppSettings
        {
            get { return ConfigurationManager.AppSettings; }
        }

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

        public static bool IsAnyAdministratorPage
        {
            get {
            bool rv = false;
            Page p = null;
            try {
                p = (Page)HttpContext.Current.Handler;
            }
            catch (Exception ex) {
               
            }
           
            if (p != null) {
                try {
                    rv = p.AppRelativeTemplateSourceDirectory.Replace("/", "").Replace("~", "").ToLower().Equals("administrators");
                }
                catch (Exception ex) {
                   
                }
            }
           
            return rv;
        }
        }

        public static bool IsProductionEnvironment
        {
            get { return DomainName.ToLower().IndexOf("localhost") == -1; }
        }



        private static bool GetSafeBoolean(string keyName) 
    {
        bool rv = false;
        string configStringValue = AppSettings[keyName];
       
        if (!configStringValue.Equals(string.Empty)) {
            try {
                rv = bool.Parse(configStringValue);
            }
            catch (Exception ex) {
               
            }
        }
       
        return rv;
    }

        private static int GetSafeInteger(string keyName)    {
        int rv = 0;
        string configStringValue = AppSettings[keyName];
       
        if (!configStringValue.Equals(string.Empty)) {
            try {
                rv = int.Parse(configStringValue);
            }
            catch (Exception ex) {
               
            }
        }
       
        return rv;
    }

    }
}