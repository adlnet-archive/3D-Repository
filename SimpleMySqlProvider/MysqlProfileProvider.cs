using System.Web.Profile;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Collections;

/*

This provider works with the following schema for the table of user data.

#
# Table structure for table 'profiles'
#

CREATE TABLE `profiles` (
  `UniqueID` int(8) NOT NULL auto_increment,
  `Username` varchar(255) NOT NULL default '',
  `ApplicationName` varchar(255) NOT NULL default '',
  `IsAnonymous` tinyint(1) default '0',
  `LastActivityDate` datetime default NULL,
  `LastUpdatedDate` datetime default NULL,
  PRIMARY KEY  (`UniqueID`),
  UNIQUE KEY `PKProfiles` (`Username`,`ApplicationName`),
  UNIQUE KEY `PKID` (`UniqueID`)
)



*/


namespace Simple.Providers.MySQL
{

    public sealed class MysqlProfileProvider : ProfileProvider
    {
        //
        // Global connection string, generic exception message, event log info.
        //

        private string eventSource = "SimpleMysqlProfileProvider";
        private string eventLog = "Application";
        private string exceptionMessage = "An exception occurred. Please check the event log.";
        private string connectionString;


        //
        // If false, exceptions are thrown to the caller. If true,
        // exceptions are written to the event log.
        //

        private bool pWriteExceptionsToEventLog;

        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }



        //
        // System.Configuration.Provider.ProviderBase.Initialize Method
        //

        public override void Initialize(string name, NameValueCollection config)
        {

            //
            // Initialize values from web.config.
            //

            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "SimpleMysqlProfileProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Simple MySQL Profile provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);


            if (config["applicationName"] == null || config["applicationName"].Trim() == "")
            {
                pApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            }
            else
            {
                pApplicationName = config["applicationName"];
            }


            //
            // Initialize connection string.
            //

            ConnectionStringSettings pConnectionStringSettings = ConfigurationManager.
                ConnectionStrings[config["connectionStringName"]];

            if (pConnectionStringSettings == null ||
                pConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = pConnectionStringSettings.ConnectionString;
        }


        //
        // System.Configuration.SettingsProvider.ApplicationName
        //

        private string pApplicationName;

        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }



        //
        // System.Configuration.SettingsProvider methods.
        //

        //
        // SettingsProvider.GetPropertyValues
        //

        public override SettingsPropertyValueCollection
              GetPropertyValues(SettingsContext context,
                    SettingsPropertyCollection ppc)
        {
            string username = (string)context["UserName"];
            bool isAuthenticated = (bool)context["IsAuthenticated"];

            // The serializeAs attribute is ignored in this provider implementation.

            SettingsPropertyValueCollection svc =
                new SettingsPropertyValueCollection();

            foreach (SettingsProperty prop in ppc)
            {
                SettingsPropertyValue pv = new SettingsPropertyValue(prop);

                switch (prop.Name)
                {
                    // TO DO: If you want to add custom attributes to the user profile, implement them here
                    //
                    // Example on how to add a Theme attribute :
                    //case "Theme":
                    //    pv.PropertyValue = GetTheme(username, isAuthenticated);
                    //    break;
                    /*
                    case "ZipCode":
                        pv.PropertyValue = GetZipCode(username, isAuthenticated);
                        break;
                    case "Customers":
                        pv.PropertyValue = GetCustomers(username, isAuthenticated);
                        break; */
                    default:
                        throw new ProviderException("Unsupported property.");
                }

                svc.Add(pv);
            }

            UpdateActivityDates(username, isAuthenticated, true);

            return svc;
        }


        //
        // SettingsProvider.SetPropertyValues
        //

        public override void SetPropertyValues(SettingsContext context,
                       SettingsPropertyValueCollection ppvc)
        {
            // The serializeAs attribute is ignored in this provider implementation.

            string username = (string)context["UserName"];
            bool isAuthenticated = (bool)context["IsAuthenticated"];
            int uniqueID = GetUniqueID(username, isAuthenticated, false);
            if (uniqueID == 0)
                uniqueID = CreateProfileForUser(username, isAuthenticated);

            foreach (SettingsPropertyValue pv in ppvc)
            {
                switch (pv.Property.Name)
                {
                    // TO DO: If you want to add custom attributes to the user profile, implement them here
                    //
                    // Example on how to add a Theme attribute :
                    //case "Theme":
                    //    SetTheme(uniqueID, (string)pv.PropertyValue);
                    //    break;
                    default:
                        throw new ProviderException("Unsupported property.");
                }
            }

            UpdateActivityDates(username, isAuthenticated, false);
        }

        //
        // UpdateActivityDates
        // Updates the LastActivityDate and LastUpdatedDate values 
        // when profile properties are accessed by the
        // GetPropertyValues and SetPropertyValues methods. 
        // Passing true as the activityOnly parameter will update
        // only the LastActivityDate.
        //

        private void UpdateActivityDates(string username, bool isAuthenticated, bool activityOnly)
        {
            DateTime activityDate = DateTime.Now;

            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand();
            cmd.Connection = conn;

            if (activityOnly)
            {
                cmd.CommandText = "UPDATE Profiles Set LastActivityDate = '"+activityDate.ToString("yyyy-MM-dd HH:mm:ss")+"' " +
                      "WHERE Username = '"+username+"' AND ApplicationName = '"+pApplicationName+"' AND IsAnonymous = "+GetIntFromBool(!isAuthenticated)+"";
            }
            else
            {
                cmd.CommandText = "UPDATE Profiles Set LastActivityDate = '" + activityDate.ToString("yyyy-MM-dd HH:mm:ss") + "', LastUpdatedDate = '" + activityDate.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                      "WHERE Username = '" + username + "' AND ApplicationName = '" + pApplicationName + "' AND IsAnonymous = " + GetIntFromBool(!isAuthenticated) + "";
            }

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateActivityDates");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }
        }

        private int GetIntFromBool(bool p_Bool)
        {
            if (p_Bool)
                return 1;
            else
                return 0;
        }


        //private string GetTheme(string username, bool isAuthenticated)
        //{
        //    int iIsAuth = 0;
        //    if (isAuthenticated)
        //        iIsAuth = 1;
        //    OdbcConnection conn = new OdbcConnection(connectionString);
        //    OdbcCommand cmd = new
        //      OdbcCommand("SELECT Theme FROM Profiles " +
        //        "INNER JOIN Themes ON Profiles.UniqueID = Themes.UniqueID " +
        //        "WHERE Username = '" + username + "' AND ApplicationName = '" + pApplicationName + "' And IsAnonymous = " + iIsAuth + "", conn);
        //    string outList = "Default";

        //    OdbcDataReader reader = null;

        //    try
        //    {
        //        conn.Open();

        //        reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            outList = reader.GetString(0);
        //        }
        //    }
        //    catch (OdbcException e)
        //    {
        //        if (WriteExceptionsToEventLog)
        //        {
        //            WriteToEventLog(e, "GetThemes");
        //            throw new ProviderException(exceptionMessage);
        //        }
        //        else
        //        {
        //            throw e;
        //        }
        //    }
        //    finally
        //    {
        //        if (reader != null) { reader.Close(); }

        //        conn.Close();
        //    }

        //    return outList;
        //}



        //
        // 
        // Example of setting the custom Theme attrinute value
        // 
        //
        //
        //private void SetTheme(int uniqueID, string p_Theme)
        //{
        //    string Theme = "";
        //    OdbcConnection conn = new OdbcConnection(connectionString);
        //    OdbcCommand cmd = new OdbcCommand("DELETE FROM Themes WHERE UniqueID = ?", conn);
        //    cmd.Parameters.Add("@UniqueID", OdbcType.Int).Value = uniqueID;

        //    OdbcCommand cmd2 = new OdbcCommand("INSERT INTO Themes (UniqueID, Theme) " +
        //               "Values(?, ?)", conn);
        //    cmd2.Parameters.Add("@UniqueID", OdbcType.Int).Value = uniqueID;
        //    cmd2.Parameters.Add("@Theme", OdbcType.VarChar, 10);

        //    OdbcTransaction tran = null;

        //    try
        //    {
        //        conn.Open();
        //        tran = conn.BeginTransaction();
        //        cmd.Transaction = tran;
        //        cmd2.Transaction = tran;

        //        // Delete any existing values;
        //        cmd.ExecuteNonQuery();
        //        cmd2.Parameters["@Theme"].Value = p_Theme;
        //        cmd2.ExecuteNonQuery();

        //        tran.Commit();
        //    }
        //    catch (OdbcException e)
        //    {
        //        try
        //        {
        //            tran.Rollback();
        //        }
        //        catch
        //        {
        //        }

        //        if (WriteExceptionsToEventLog)
        //        {
        //            WriteToEventLog(e, "SetTheme");
        //            throw new ProviderException(exceptionMessage);
        //        }
        //        else
        //        {
        //            throw e;
        //        }
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        //
        // GetUniqueID
        //   Retrieves the uniqueID from the database for the current user and application.
        //

        private int GetUniqueID(string username, bool isAuthenticated, bool ignoreAuthenticationType)
        {
            OdbcConnection conn = new OdbcConnection(connectionString);
            string sSQL = "SELECT UniqueID FROM Profiles " +
                    "WHERE Username = '" + username + "' AND ApplicationName = '" + ApplicationName + "'";

            if (!ignoreAuthenticationType)
            {
                if (isAuthenticated)
                    sSQL += " AND IsAnonymous = 0";
                else
                    sSQL += " AND IsAnonymous = 1";
            }
            OdbcCommand cmd = new OdbcCommand(sSQL, conn);

            int uniqueID = 0;
            OdbcDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                    uniqueID = reader.GetInt32(0);
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUniqueID");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return uniqueID;
        }


        //
        // CreateProfileForUser
        // If no user currently exists in the database, 
        // a user record is created during
        // the call to the GetUniqueID private method.
        //

        private int CreateProfileForUser(string username, bool isAuthenticated)
        {
            // Check for valid user name.

            if (username == null)
                throw new ArgumentNullException("User name cannot be null.");
            if (username.Length > 255)
                throw new ArgumentException("User name exceeds 255 characters.");
            if (username.IndexOf(",") > 0)
                throw new ArgumentException("User name cannot contain a comma (,).");


            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand("INSERT INTO Profiles (Username, " +
                    "ApplicationName, LastActivityDate, LastUpdatedDate, " +
                    "IsAnonymous) Values('"+username+"', '"+pApplicationName+"', NOW(), NOW(), "+GetIntFromBool(!isAuthenticated)+")", conn);

            OdbcCommand cmd2 = new OdbcCommand("SELECT @@IDENTITY", conn);

            int uniqueID = 0;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                uniqueID = (int)cmd2.ExecuteScalar();
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "CreateProfileForUser");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return uniqueID;
        }


        //
        // ProfileProvider.DeleteProfiles(ProfileInfoCollection)
        //

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            int deleteCount = 0;

            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                foreach (ProfileInfo p in profiles)
                {
                    if (DeleteProfile(p.UserName, conn, tran))
                        deleteCount++;
                }

                tran.Commit();
            }
            catch (Exception e)
            {
                try
                {
                    tran.Rollback();
                }
                catch
                {
                }

                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteProfiles(ProfileInfoCollection)");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return deleteCount;
        }


        //
        // ProfileProvider.DeleteProfiles(String[])
        //

        public override int DeleteProfiles(String[] usernames)
        {
            int deleteCount = 0;

            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                foreach (string user in usernames)
                {
                    if (DeleteProfile(user, conn, tran))
                        deleteCount++;
                }

                tran.Commit();
            }
            catch (Exception e)
            {
                try
                {
                    tran.Rollback();
                }
                catch
                {
                }

                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteProfiles(String())");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return deleteCount;
        }



        //
        // ProfileProvider.DeleteInactiveProfiles
        //

        public override int DeleteInactiveProfiles(
          ProfileAuthenticationOption authenticationOption,
          DateTime userInactiveSinceDate)
        {
            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand("SELECT Username FROM Profiles " +
                    "WHERE ApplicationName = ? AND " +
                    " LastActivityDate <= ?", conn);
            cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = ApplicationName;
            cmd.Parameters.Add("@LastActivityDate", OdbcType.DateTime).Value = userInactiveSinceDate;

            switch (authenticationOption)
            {
                case ProfileAuthenticationOption.Anonymous:
                    cmd.CommandText += " AND IsAnonymous = ?";
                    cmd.Parameters.Add("@IsAnonymous", OdbcType.TinyInt).Value = true;
                    break;
                case ProfileAuthenticationOption.Authenticated:
                    cmd.CommandText += " AND IsAnonymous = ?";
                    cmd.Parameters.Add("@IsAnonymous", OdbcType.TinyInt).Value = false;
                    break;
                default:
                    break;
            }

            OdbcDataReader reader = null;
            string usernames = "";

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    usernames += reader.GetString(0) + ",";
                }
            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteInactiveProfiles");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            if (usernames.Length > 0)
            {
                // Remove trailing comma.
                usernames = usernames.Substring(0, usernames.Length - 1);
            }


            // Delete profiles.

            return DeleteProfiles(usernames.Split(','));
        }


        //
        // DeleteProfile
        // Deletes profile data from the database for the 
        // specified user name.
        //

        private bool DeleteProfile(string username, OdbcConnection conn, OdbcTransaction tran)
        {
            // Check for valid user name.
            if (username == null)
                throw new ArgumentNullException("User name cannot be null.");
            if (username.Length > 255)
                throw new ArgumentException("User name exceeds 255 characters.");
            if (username.IndexOf(",") > 0)
                throw new ArgumentException("User name cannot contain a comma (,).");


            int uniqueID = GetUniqueID(username, false, true);

            OdbcCommand cmd1 = new OdbcCommand("DELETE * FROM ProfileData WHERE UniqueID = ?", conn);
            cmd1.Parameters.Add("@UniqueID", OdbcType.Int).Value = uniqueID;
            OdbcCommand cmd2 = new OdbcCommand("DELETE * FROM Themes WHERE UniqueID = ?", conn);
            cmd2.Parameters.Add("@UniqueID", OdbcType.Int).Value = uniqueID;
            OdbcCommand cmd3 = new OdbcCommand("DELETE * FROM Profiles WHERE UniqueID = ?", conn);
            cmd3.Parameters.Add("@UniqueID", OdbcType.Int).Value = uniqueID;

            cmd1.Transaction = tran;
            cmd2.Transaction = tran;
            cmd3.Transaction = tran;

            int numDeleted = 0;

            // Exceptions will be caught by the calling method.
            numDeleted += cmd1.ExecuteNonQuery();
            numDeleted += cmd2.ExecuteNonQuery();
            numDeleted += cmd3.ExecuteNonQuery();

            if (numDeleted == 0)
                return false;
            else
                return true;
        }


        //
        // ProfileProvider.FindProfilesByUserName
        //

        public override ProfileInfoCollection FindProfilesByUserName(
          ProfileAuthenticationOption authenticationOption,
          string usernameToMatch,
          int pageIndex,
          int pageSize,
          out int totalRecords)
        {
            CheckParameters(pageIndex, pageSize);

            return GetProfileInfo(authenticationOption, usernameToMatch,
                null, pageIndex, pageSize, out totalRecords);
        }


        //
        // ProfileProvider.FindInactiveProfilesByUserName
        //

        public override ProfileInfoCollection FindInactiveProfilesByUserName(
          ProfileAuthenticationOption authenticationOption,
          string usernameToMatch,
          DateTime userInactiveSinceDate,
          int pageIndex,
          int pageSize,
          out int totalRecords)
        {
            CheckParameters(pageIndex, pageSize);

            return GetProfileInfo(authenticationOption, usernameToMatch, userInactiveSinceDate,
                  pageIndex, pageSize, out totalRecords);
        }


        //
        // ProfileProvider.GetAllProfiles
        //

        public override ProfileInfoCollection GetAllProfiles(
          ProfileAuthenticationOption authenticationOption,
          int pageIndex,
          int pageSize,
          out int totalRecords)
        {
            CheckParameters(pageIndex, pageSize);

            return GetProfileInfo(authenticationOption, null, null,
                  pageIndex, pageSize, out totalRecords);
        }


        //
        // ProfileProvider.GetAllInactiveProfiles
        //

        public override ProfileInfoCollection GetAllInactiveProfiles(
          ProfileAuthenticationOption authenticationOption,
          DateTime userInactiveSinceDate,
          int pageIndex,
          int pageSize,
          out int totalRecords)
        {
            CheckParameters(pageIndex, pageSize);

            return GetProfileInfo(authenticationOption, null, userInactiveSinceDate,
                  pageIndex, pageSize, out totalRecords);
        }



        //
        // ProfileProvider.GetNumberOfInactiveProfiles
        //

        public override int GetNumberOfInactiveProfiles(
          ProfileAuthenticationOption authenticationOption,
          DateTime userInactiveSinceDate)
        {
            int inactiveProfiles = 0;

            ProfileInfoCollection profiles =
              GetProfileInfo(authenticationOption, null, userInactiveSinceDate,
                  0, 0, out inactiveProfiles);

            return inactiveProfiles;
        }



        //
        // CheckParameters
        // Verifies input parameters for page size and page index. 
        // Called by GetAllProfiles, GetAllInactiveProfiles, 
        // FindProfilesByUserName, and FindInactiveProfilesByUserName.
        //

        private void CheckParameters(int pageIndex, int pageSize)
        {
            if (pageIndex < 0)
                throw new ArgumentException("Page index must 0 or greater.");
            if (pageSize < 1)
                throw new ArgumentException("Page size must be greater than 0.");
        }


        //
        // GetProfileInfo
        // Retrieves a count of profiles and creates a 
        // ProfileInfoCollection from the profile data in the 
        // database. Called by GetAllProfiles, GetAllInactiveProfiles,
        // FindProfilesByUserName, FindInactiveProfilesByUserName, 
        // and GetNumberOfInactiveProfiles.
        // Specifying a pageIndex of 0 retrieves a count of the results only.
        //

        private ProfileInfoCollection GetProfileInfo(
          ProfileAuthenticationOption authenticationOption,
          string usernameToMatch,
          object userInactiveSinceDate,
          int pageIndex,
          int pageSize,
          out int totalRecords)
        {
            OdbcConnection conn = new OdbcConnection(connectionString);


            // Command to retrieve the total count.

            OdbcCommand cmd = new OdbcCommand("SELECT COUNT(*) FROM Profiles WHERE ApplicationName = ? ", conn);
            cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = ApplicationName;


            // Command to retrieve the profile data.

            OdbcCommand cmd2 = new OdbcCommand("SELECT Username, LastActivityDate, LastUpdatedDate, " +
                    "IsAnonymous FROM Profiles WHERE ApplicationName = ? ", conn);
            cmd2.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = ApplicationName;


            // If searching for a user name to match, add the command text and parameters.

            if (usernameToMatch != null)
            {
                cmd.CommandText += " AND Username LIKE ? ";
                cmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = usernameToMatch;

                cmd2.CommandText += " AND Username LIKE ? ";
                cmd2.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = usernameToMatch;
            }


            // If searching for inactive profiles, 
            // add the command text and parameters.

            if (userInactiveSinceDate != null)
            {
                cmd.CommandText += " AND LastActivityDate <= ? ";
                cmd.Parameters.Add("@LastActivityDate", OdbcType.DateTime).Value = (DateTime)userInactiveSinceDate;

                cmd2.CommandText += " AND LastActivityDate <= ? ";
                cmd2.Parameters.Add("@LastActivityDate", OdbcType.DateTime).Value = (DateTime)userInactiveSinceDate;
            }


            // If searching for a anonymous or authenticated profiles,    
            // add the command text and parameters.

            switch (authenticationOption)
            {
                case ProfileAuthenticationOption.Anonymous:
                    cmd.CommandText += " AND IsAnonymous = ?";
                    cmd.Parameters.Add("@IsAnonymous", OdbcType.TinyInt).Value = true;
                    cmd2.CommandText += " AND IsAnonymous = ?";
                    cmd2.Parameters.Add("@IsAnonymous", OdbcType.TinyInt).Value = true;
                    break;
                case ProfileAuthenticationOption.Authenticated:
                    cmd.CommandText += " AND IsAnonymous = ?";
                    cmd.Parameters.Add("@IsAnonymous", OdbcType.TinyInt).Value = false;
                    cmd2.CommandText += " AND IsAnonymous = ?";
                    cmd2.Parameters.Add("@IsAnonymous", OdbcType.TinyInt).Value = false;
                    break;
                default:
                    break;
            }


            // Get the data.

            OdbcDataReader reader = null;
            ProfileInfoCollection profiles = new ProfileInfoCollection();

            try
            {
                conn.Open();
                // Get the profile count.
                totalRecords = int.Parse(cmd.ExecuteScalar().ToString());
                // No profiles found.
                if (totalRecords <= 0) { return profiles; }
                // Count profiles only.
                if (pageSize == 0) { return profiles; }

                reader = cmd2.ExecuteReader();

                int counter = 0;
                int startIndex = pageSize * (pageIndex - 1);
                int endIndex = startIndex + pageSize - 1;

                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        ProfileInfo p = GetProfileInfoFromReader(reader);
                        profiles.Add(p);
                    }

                    if (counter >= endIndex)
                    {
                        cmd.Cancel();
                        break;
                    }

                    counter++;
                }

            }
            catch (OdbcException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetProfileInfo");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            return profiles;
        }

        //
        // GetProfileInfoFromReader
        //  Takes the current row from the OdbcDataReader
        // and populates a ProfileInfo object from the values. 
        //

        private ProfileInfo GetProfileInfoFromReader(OdbcDataReader reader)
        {
            string username = reader.GetString(0);

            DateTime lastActivityDate = new DateTime();
            if (reader.GetValue(1) != DBNull.Value)
                lastActivityDate = reader.GetDateTime(1);

            DateTime lastUpdatedDate = new DateTime();
            if (reader.GetValue(2) != DBNull.Value)
                lastUpdatedDate = reader.GetDateTime(2);

            bool isAnonymous = reader.GetBoolean(3);

            // ProfileInfo.Size not currently implemented.
            ProfileInfo p = new ProfileInfo(username,
                isAnonymous, lastActivityDate, lastUpdatedDate, 0);

            return p;
        }


        //
        // WriteToEventLog
        // A helper function that writes exception detail to the event 
        // log. Exceptions are written to the event log as a security 
        // measure to prevent private database details from being 
        // returned to the browser. If a method does not return a 
        // status or Boolean value indicating whether the action succeeded 
        // or failed, the caller also throws a generic exception.
        //

        private void WriteToEventLog(Exception e, string action)
        {
            EventLog log = new EventLog();
            log.Source = eventSource;
            log.Log = eventLog;

            string message = "An exception occurred while communicating with the data source.\n\n";
            message += "Action: " + action + "\n\n";
            message += "Exception: " + e.ToString();

            log.WriteEntry(message);
        }
    }
}