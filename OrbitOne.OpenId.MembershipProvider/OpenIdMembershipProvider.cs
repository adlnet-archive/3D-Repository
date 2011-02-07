using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Odbc;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Janrain.OpenId.Consumer;
using System.Net;
namespace OrbitOne.OpenId.MembershipProvider
{
    public class OpenIdMembershipProvider : System.Web.Security.MembershipProvider
    {
        #region Members

        private static readonly string EXCEPTION_MESSAGE = "An exception occurred. Please check the Event Log.";
        private string _connectionString;
        private bool _WriteExceptionsToEventLog;
        private string _applicationName;
        private string _nickname;
        private string _email;
        private string _fullname;
        private string _dayOfBirth;
        private string _gender;
        private string _postcode;
        private string _country;
        private string _language;
        private string _timezone;
        private string _optionalInformation;
        private string _loginURL;
        private int _minutesSinceLastActivity;
        private string _nonOpenIdMembershipProviderName;
        private Janrain.OpenId.Session.SimpleSessionState consumerSession;
        #endregion


        #region Properties



        public int MinutesSinceLastActivity
        {
            get { return _minutesSinceLastActivity; }
        }


        public string LoginURL
        {
            get { return _loginURL; }
        }


        public string Nickname
        {
            get { return _nickname; }
        }


        public string Email
        {
            get { return _email; }
        }


        public string Fullname
        {
            get { return _fullname; }
        }


        public string Dob
        {
            get { return _dayOfBirth; }
        }


        public string Gender
        {
            get { return _gender; }
        }


        public string Postcode
        {
            get { return _postcode; }
        }


        public string Country
        {
            get { return _country; }
        }


        public string Language
        {
            get { return _language; }
        }


        public string Timezone
        {
            get { return _timezone; }
        }


        public override string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }


        public bool WriteExceptionsToEventLog
        {
            get { return _WriteExceptionsToEventLog; }
            set { _WriteExceptionsToEventLog = value; }
        }



        public string NonOpenIdMemebershipProviderName
        {
            get { return _nonOpenIdMembershipProviderName; }
            set { _nonOpenIdMembershipProviderName = value; }
        }


        public System.Web.Security.MembershipProvider NonOpenIdMembershiProvider
        {
            get
            {

                System.Web.Security.MembershipProvider provider = Membership.Providers[NonOpenIdMemebershipProviderName];
                if (provider != null)
                { return provider; }
                else
                { throw new ApplicationException("NonOpenIdMembershiProvider does not exist!"); }
            }
        }




        #endregion

        #region Initialize

        public override void Initialize(string name, NameValueCollection config)
        {

            try
            {
                // Initialize values from web.config.
                if (config == null)
                    throw new ArgumentNullException("config");

                if (name == null || name.Length == 0)
                    name = "OpenIDMembershipProvider";

                if (String.IsNullOrEmpty(config["description"]))
                {
                    config.Remove("description");
                    config.Add("description", "OpenID Membership Provider");
                }

                // Initialize the abstract base class.
                base.Initialize(name, config);

                _applicationName = Utility.GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
                _WriteExceptionsToEventLog = Convert.ToBoolean(Utility.GetConfigValue(config["writeExceptionsToEventLog"], "false"));

                // Initialize the OpenID Attributes
                _optionalInformation = Utility.GetConfigValue(config["OptionalInformation"], "");
                NonOpenIdMemebershipProviderName = Utility.GetConfigValue(config["NonOpenIdMembershipProviderName"], "");
                _minutesSinceLastActivity = int.Parse(Utility.GetConfigValue(config["MinutesSinceLastActivity"], "20"));


                // Initialize the SqlConnection.
                ConnectionStringSettings ConnectionStringSettings =
                  ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

                if (ConnectionStringSettings == null || ConnectionStringSettings.ConnectionString.Trim() == "")
                {
                    throw new ProviderException("Connection string cannot be blank.");
                }

                _connectionString = ConnectionStringSettings.ConnectionString;



            }
            catch (Exception e)
            {
                if (this.WriteExceptionsToEventLog)
                {
                    Utility.WriteToEventLog(e, "Initialize");
                }

                throw new OpenIdMembershipProviderException(e.Message, e.Source, e.StackTrace);
            }
        }

        #endregion


        #region MembershipProvider Methods

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer,
                                                    bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    email = username;
                }
                MembershipUser u = GetUserByOpenId(username, false);
                if (u == null)
                {
                    WebRequest request = HttpWebRequest.Create(Utility.NormalizeIdentityUrl(username));
                    request.Method = "HEAD";
                    var response = request.GetResponse();
                    u = NonOpenIdMembershiProvider.CreateUser(username, password, email, passwordQuestion, passwordAnswer,
                                                    isApproved, providerUserKey, out status);
                    LinkUserWithOpenId(username, u.ProviderUserKey);
                    return u;
                }
                else
                {
                    status = MembershipCreateStatus.DuplicateUserName;
                }
            }
            catch (Exception ex)
            {
                status = MembershipCreateStatus.InvalidUserName;
            }
            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {

            var conn = new OdbcConnection(_connectionString);
            var cmd = new OdbcCommand("{call OpenId_DeleteUserOpenIdLink(?)}", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userId", OdbcType.VarChar, 255).Value = NonOpenIdMembershiProvider.GetUser(username, false).ProviderUserKey.ToString();

            try
            {

                conn.Open();
                cmd.ExecuteNonQuery();
                return NonOpenIdMembershiProvider.DeleteUser(username, deleteAllRelatedData);


            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    Utility.WriteToEventLog(e, "DeleteUser");
                }

                throw new OpenIdMembershipProviderException(e.Message, e.Source, e.StackTrace);
            }
            finally
            {
                conn.Close();
            }


        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            return NonOpenIdMembershiProvider.GetAllUsers(pageIndex, pageSize, out  totalRecords);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return NonOpenIdMembershiProvider.GetUser(username, userIsOnline);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return NonOpenIdMembershiProvider.GetUser(providerUserKey, userIsOnline);
        }


        public override int GetNumberOfUsersOnline()
        {
            return NonOpenIdMembershiProvider.GetNumberOfUsersOnline();
        }


        public override void UpdateUser(MembershipUser user)
        {

            NonOpenIdMembershiProvider.UpdateUser(user);
        }
        public override bool ValidateUser(string username, string password)
        {
            bool ret = true;
            try
            {

                Uri userUri = Utility.NormalizeIdentityUrl(username);
                HttpContext Context = HttpContext.Current;
                HttpSessionState Session = Context.Session;
                HttpRequest Request = Context.Request;
                HttpResponse Response = Context.Response;
                Janrain.OpenId.Consumer.Consumer consumer;

                if (consumerSession == null)
                {
                    consumerSession = new Janrain.OpenId.Session.SimpleSessionState();
                }
                consumer = new Janrain.OpenId.Consumer.Consumer(consumerSession,
                                                                Janrain.OpenId.Store.MemoryStore.GetInstance());
                if (username != null)
                {
                    try
                    {
                        AuthRequest request = consumer.Begin(userUri);
                        // Build the trust root
                        UriBuilder builder = new UriBuilder(Request.Url.AbsoluteUri);
                        builder.Query = null;
                        builder.Password = null;
                        builder.UserName = null;
                        builder.Fragment = null;
                        builder.Path = Request.ApplicationPath;
                        // The following approach does not append port 80 in the
                        // no port case.
                        string trustRoot = (new Uri(builder.ToString())).ToString();
                        // Build the return_to URL
                        builder = new UriBuilder(Request.Url.AbsoluteUri);
                        NameValueCollection col = new NameValueCollection();
                        col["ReturnUrl"] = Request.QueryString["ReturnUrl"];
                        builder.Query = Janrain.OpenId.UriUtil.CreateQueryString(col);
                        Uri returnTo = new Uri(builder.ToString());
                        Uri redirectUrl = request.CreateRedirect(trustRoot, returnTo, AuthRequest.Mode.SETUP);
                        // The following illustrates how to use SREG. 
                        String uriString = redirectUrl.AbsoluteUri + "&openid.sreg.optional=" + _optionalInformation;
                        // Get the current page
                        _loginURL = Context.Request.Url.AbsoluteUri;
                        // Redirect the user to the OpenID provider Page
                        Response.Redirect(uriString, true);
                    }
                    catch (System.Threading.ThreadAbortException)
                    {
                        // Consume. This is normal during redirect.
                    }
                }
                else
                {
                    ret = false;
                }
            }
            catch
            {
                return false;
            }
            return ret;
        }




        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            return NonOpenIdMembershiProvider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
        }
        #endregion MembershipProvider Methods


        #region OpenId Related

        public bool ValidateOpenIDUser()
        {
            bool ret = true;
            HttpContext Context = HttpContext.Current;
            HttpSessionState Session = Context.Session;
            HttpRequest Request = Context.Request;
            Janrain.OpenId.Consumer.Consumer consumer;

            try
            {
                if (consumerSession == null)
                {
                    consumerSession = new Janrain.OpenId.Session.SimpleSessionState();
                }
                consumer = new Janrain.OpenId.Consumer.Consumer(consumerSession,
                                                                Janrain.OpenId.Store.MemoryStore.GetInstance());
            }
            catch
            {
                return false;
            }

            if (Request.QueryString["openid.mode"] != null && Request.QueryString["openid.mode"] != "Cancel")
            {
                try
                {
                    _country = (Request.QueryString["openid.sreg.country"] ?? "");
                    _dayOfBirth = (Request.QueryString["openid.sreg.dob"] ?? "");
                    _email = (Request.QueryString["openid.sreg.email"] ?? "");
                    _fullname = (Request.QueryString["openid.sreg.fullname"] ?? "");
                    _gender = (Request.QueryString["openid.sreg.gender"] ?? "");
                    _language = (Request.QueryString["openid.sreg.language"] ?? "");
                    _nickname = (Request.QueryString["openid.sreg.nickname"] ?? "");
                    _postcode = (Request.QueryString["openid.sreg.postcode"] ?? "");
                    _timezone = (Request.QueryString["openid.sreg.timezone"] ?? "");

                    ConsumerResponse resp = consumer.Complete(Request.QueryString);
                    string userIdentity = Utility.IdentityUrlToDisplayString(resp.IdentityUrl);
                    MembershipUser user = GetUserByOpenId(userIdentity, true);
                    ret = (user != null);
                    if (ret)
                    {
                        FormsAuthentication.RedirectFromLoginPage(user.UserName, false);
                    }
                    else
                    {
                        OpenIdNotLinkedException exception = new OpenIdNotLinkedException(userIdentity);
                        throw exception;
                    }
                }
                catch (FailureException fexc)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        Utility.WriteToEventLog(fexc, "ValidateOpenIDUser");
                    }
                    ret = false;
                }
                catch (OpenIdNotLinkedException nlEx)
                {
                    throw nlEx;
                }
                catch (Exception fe)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        Utility.WriteToEventLog(fe, "ValidateOpenIDUser");
                    }
                    throw new OpenIdMembershipProviderException(fe.Message, fe.Source, fe.StackTrace);
                }
            }
            else
            {
                ret = false;
            }
            return ret;
        }
        public MembershipUser GetUserByOpenId(string openId, bool userIsOnline)
        {

            var conn = new OdbcConnection(_connectionString);
            var cmd = new OdbcCommand("{CALL OpenId_GetUserIdByOpenld(?)}", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("openIdurl", openId);
            OdbcDataReader reader = null;
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    var userId = new Guid(reader.GetValue(0).ToString());
                    var temp = userId.ToString();
                    return Membership.GetUser(userId, userIsOnline); ;
                }

            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    Utility.WriteToEventLog(e, "GetUserByOpenId ");
                }
                throw new OpenIdMembershipProviderException(e.Message, e.Source, e.StackTrace);
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return null;
        }
        public IList<OpenIdMembershipUser> GetAllOpenIdUsers(out int totalRecords)
        {

            var conn = new OdbcConnection(_connectionString);
            var cmd = new OdbcCommand("{call OpenId_Membership_GetAllUsers(?)}", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = ApplicationName;
            IList<OpenIdMembershipUser> users = new List<OpenIdMembershipUser>();
            OdbcDataReader reader = null;
            totalRecords = 0;
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    OpenIdMembershipUser u = GetUserFromReader(reader);
                    users.Add(u);
                    totalRecords++;
                }
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    Utility.WriteToEventLog(e, "GetAllUsers ");

                    throw new ProviderException(EXCEPTION_MESSAGE);
                }
                throw new OpenIdMembershipProviderException(e.Message, e.Source, e.StackTrace);
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }


            return users;
        }
        public void LinkUserWithOpenId(string openId, object userId)
        {

            openId = Utility.NormalizeIdentityUrl(openId).ToString();


            var conn = new OdbcConnection(_connectionString);
            var cmd = new OdbcCommand("{call OpenId_LinkUserWithOpenId(?,?)}", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@OpenId_Url", OdbcType.NVarChar).Value = openId;
            cmd.Parameters.Add("@userId", OdbcType.NVarChar).Value = userId.ToString();
            try
            {

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    Utility.WriteToEventLog(e, "LinkUserWithOpenId ");
                }
                throw new OpenIdMembershipProviderException(e.Message, e.Source, e.StackTrace);
            }
            finally
            {

                conn.Close();
            }
        }
        public void RemoveUserOpenIdLinkByOpenId(string openId)
        {

            var conn = new OdbcConnection(_connectionString);
            var cmd = new OdbcCommand("{call OpenId_DeleteUserOpenIdLink(?)}", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@OpenId_Url", OdbcType.NVarChar).Value = openId;
            cmd.Parameters.Add("@userId", OdbcType.UniqueIdentifier).Value = DBNull.Value;
            try
            {

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    Utility.WriteToEventLog(e, "LinkUserWithOpenId ");
                }
                throw new OpenIdMembershipProviderException(e.Message, e.Source, e.StackTrace);
            }
            finally
            {

                conn.Close();
            }
        }
        public void RemoveUserOpenIdLinkByUserId(object userId)
        {

            var conn = new OdbcConnection(_connectionString);
            var cmd = new OdbcCommand("{call OpenId_DeleteUserOpenIdLink(?)}", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@OpenId_Url", OdbcType.NVarChar).Value = DBNull.Value;
            cmd.Parameters.Add("@userId", OdbcType.NVarChar).Value = userId.ToString();
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    Utility.WriteToEventLog(e, "LinkUserWithOpenId ");
                }
                throw new OpenIdMembershipProviderException(e.Message, e.Source, e.StackTrace);
            }
            finally
            {

                conn.Close();
            }
        }
        public IList<string> GetOpenIdsByUserName(string userName)
        {
            object userId = null;

            userId = NonOpenIdMembershiProvider.GetUser(userName, false).ProviderUserKey;

            if (userId == null)
                return null;

            IList<string> openIds = new List<string>();

            OdbcConnection conn = new OdbcConnection(_connectionString);
            OdbcCommand cmd = new OdbcCommand("{call OpenId_GetOpenIdsByUserId(?)}", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@userId", OdbcType.NVarChar).Value = userId.ToString();
            OdbcDataReader reader;
            try
            {

                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    openIds.Add(reader.GetString(0));
                }

            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    Utility.WriteToEventLog(e, "GetOpenIdsByUserName ");
                }
                throw new OpenIdMembershipProviderException(e.Message, e.Source, e.StackTrace);
            }
            finally
            {

                conn.Close();
            }
            return openIds;
        }
        private OpenIdMembershipUser GetUserFromReader(OdbcDataReader reader)
        {

            string username = reader.GetString(0);

            string openId = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
            string email = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
            string passwordQuestion = "";
            if (reader.GetValue(3) != DBNull.Value)
                passwordQuestion = reader.GetString(3);

            string comment = "";
            if (reader.GetValue(4) != DBNull.Value)
                comment = reader.GetString(4);

            bool isApproved = reader.GetBoolean(5);

            DateTime creationDate = reader.GetDateTime(6);

            DateTime lastLoginDate = new DateTime();
            if (reader.GetValue(7) != DBNull.Value)
                lastLoginDate = reader.GetDateTime(7);

            DateTime lastActivityDate = reader.GetDateTime(8);

            DateTime lastPasswordChangedDate = reader.GetDateTime(9);

            object providerUserKey = reader.GetValue(10);

            bool isLockedOut = reader.GetBoolean(11);

            DateTime lastLockedOutDate = new DateTime();
            if (reader.GetValue(12) != DBNull.Value)
                lastLockedOutDate = reader.GetDateTime(12);

            OpenIdMembershipUser u = new OpenIdMembershipUser(
                                                  this.Name,
                                                  openId,
                                                  username,
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
                                                  lastLockedOutDate);


            return u;
        }

        #endregion OpenId Related



        #region Not Suported Methods
        public override bool EnablePasswordReset
        {
            get { return false; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return false; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 0; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 0; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotSupportedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 0; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return ""; }
        }


        public override bool ChangePassword(string username, string oldPwd, string newPwd)
        {
            throw new NotSupportedException();
        }


        public override bool ChangePasswordQuestionAndAnswer(string username,
                      string password,
                      string newPwdQuestion,
                      string newPwdAnswer)
        {
            throw new NotSupportedException();
        }


        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }


        public override bool UnlockUser(string username)
        {
            throw new NotSupportedException();
        }


        public override string GetUserNameByEmail(string email)
        {
            throw new NotSupportedException();
        }


        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }


        private void UpdateFailureCount(string username, string failureType)
        {
            throw new NotSupportedException();
        }


        private bool CheckPassword(string password, string dbpassword)
        {
            throw new NotSupportedException();
        }


        private string EncodePassword(string password)
        {
            throw new NotSupportedException();
        }


        private string UnEncodePassword(string encodedPassword)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        #endregion



    }
}
