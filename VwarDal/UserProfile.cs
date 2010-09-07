using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Odbc;


namespace vwarDAL
{
    public class UserProfile
    {
        //UserProfiles primraryKey column 
        internal int InternalUserID { get; set; }

        //ForeignKey to Membership users table
        internal string InternalMembershipUserGuid { get; set; }

        public int UserID
        {
            get { return this.InternalUserID; }

        }

        public string MembershipUserGuid
        {
            get { return this.InternalMembershipUserGuid; }

        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string WebsiteURL { get; set; }
        public string SponsorName { get; set; }
        public Byte[] SponsorLogo { get; set; }
        public string SponsorLogoContentType { get; set; }
        public string DeveloperName { get; set; }
        public Byte[] DeveloperLogo { get; set; }
        public string DeveloperLogoContentType { get; set; }
        public string ArtistName { get; set; }
        public string Phone { get; set; }
        public string DeveloperLogoFileName { get; set; }
        public string SponsorLogoFileName { get; set; }

        public UserProfile()
        {

        }



    }


    public class UserProfileDB
    {
        #region Queries
        private static string PostgreSQLConnectionString { get { return ConfigurationManager.ConnectionStrings["postgreSQLConnectionString"].ConnectionString; } }

        private const string GetAllUserProfiles_QUERY = "SELECT * FROM `UserProfiles`;";

        private const string GetUserByUserID_QUERY = @"SELECT `UserID`, `UserGuid`, `FirstName`, `LastName`, `Email`, `WebsiteURL`, `SponsorName`,
                                                       `SponsorLogo`, `SponsorLogoContentType`, `DeveloperName`, `DeveloperLogo`, `DeveloperLogoContentType`, `DeveloperLogoFileName`, `SponsorLogoFileName`,
                                                       `ArtistName`, `Phone`, `CreatedDate`, `CreatedBy`, `LastEditedBy`, `UserName` 
                                                        FROM `UserProfiles` 
                                                        WHERE `UserID` = ?;";

        private const string GetUserByUserName_QUERY = @"SELECT `UserID`, `UserGuid`, `FirstName`, `LastName`, `Email`, `WebsiteURL`, `SponsorName`,
                                                       `SponsorLogo`, `SponsorLogoContentType`, `DeveloperName`, `DeveloperLogo`, `DeveloperLogoContentType`, `DeveloperLogoFileName`, `SponsorLogoFileName`,
                                                       `ArtistName`, `Phone`, `CreatedDate`, `CreatedBy`, `LastEditedBy`, `UserName`
                                                        FROM `UserProfiles` 
                                                        WHERE `UserName` = ?;";


        private const string GetUserByMembershipUserGuid_QUERY = @"SELECT `UserID`, `UserGuid`, `FirstName`, `LastName`, `Email`, `WebsiteURL`, `SponsorName`,
                                                                `SponsorLogo`, `SponsorLogoContentType`, `DeveloperName`, `DeveloperLogo`, `DeveloperLogoContentType`, `DeveloperLogoFileName`, `SponsorLogoFileName`,
                                                                `ArtistName`, `Phone`, `CreatedDate`, `CreatedBy`, `LastEditedBy`, `UserName`
                                                                  FROM `UserProfiles` 
                                                                 WHERE `UserGuid` = ?;";

        private const string InsertUserProfile_QUERY = @"INSERT INTO `UserProfiles` 
                                                    (`UserGuid`,`FirstName` , `LastName`, `Email`, `WebsiteURL`, `SponsorName`, `SponsorLogo`,`SponsorLogoContentType`, 
                                                    `DeveloperName`, `DeveloperLogo`,`DeveloperLogoContentType`, `ArtistName`, `Phone`, `CreatedDate`, `CreatedBy`, `UserName`, `DeveloperLogoFileName`, `SponsorLogoFileName`)
                                                     VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,?, ?, ?, ?, ?, ?, ?);";


        private const string UpdateUserProfile_QUERY = @"UPDATE `UserProfiles` SET `FirstName` = ?, `LastName` = ?, `Email` = ?, 
                                                            `WebsiteURL` = ?, `SponsorName` = ?, `SponsorLogo` = ?, `SponsorLogoContentType` = ?, 
                                                            `DeveloperName` = ?, `DeveloperLogo` = ?, `DeveloperLogoContentType` = ?, `ArtistName` = ?,
                                                            `Phone` = ?, `LastEditedBy` = ?, `LastEditedDate` = ?,  `DeveloperLogoFileName` = ?, `SponsorLogoFileName` = ?
                                                             WHERE `UserID` = ?;";



        private const string DeleteUserProfileByUserID_QUERY = "DELETE FROM `UserProfiles` WHERE `UserID` = ?;";

        private const string DeleteUserProfileByUserGuid_QUERY = "DELETE FROM `UserProfiles` WHERE `UserGuid` = ?;";

        private const string DeleteUserProfileByUserName_QUERY = "DELETE FROM `UserProfiles` WHERE `UserName` = ?;";

        private const string GetAllAspnetUsersNotApproved_QUERY = @"SELECT `pkid`, `Username`, `Email`, `Comment`, `CreationDate` FROM `Users` WHERE `IsApproved` = FALSE;";

        private const string GetAllAspnetUsersLockedOut_QUERY = @"SELECT `pkid`, `Username`, `Email`, `Comment`,  `CreationDate` FROM `Users` WHERE `IsLockedOut` = TRUE;";

        private const string GetAdministrativeUsers_Query = @"SELECT * FROM users u INNER JOIN usersinroles r on u.Username = r.username WHERE r.rolename = 'Administrators';";



        #endregion

        #region Private Helper Functions
        private static UserProfile GetUserProfileFromDataTable(DataTable dt)
        {
            UserProfile rv = null;

            if (dt != null && dt.Rows.Count > 0)
            {
                rv = new UserProfile();

                DataRow row = dt.Rows[0];

                rv.InternalUserID = Int32.Parse(row["UserID"].ToString());
                rv.InternalMembershipUserGuid = row["UserGuid"].ToString();

                if (row["FirstName"] != DBNull.Value)
                {
                    rv.FirstName = row["FirstName"].ToString();
                }

                if (row["LastName"] != DBNull.Value)
                {
                    rv.LastName = row["LastName"].ToString();
                }

                if (row["Email"] != DBNull.Value)
                {
                    rv.Email = row["Email"].ToString();
                }

                if (row["WebsiteURL"] != DBNull.Value)
                {
                    rv.WebsiteURL = row["WebsiteURL"].ToString();
                }

                if (row["SponsorName"] != DBNull.Value)
                {
                    rv.SponsorName = row["SponsorName"].ToString();
                }

                if (row["SponsorLogo"] != DBNull.Value)
                {
                    rv.SponsorLogo = (byte[])row["SponsorLogo"];
                }

                if (row["SponsorLogoContentType"] != DBNull.Value)
                {
                    rv.SponsorLogoContentType = row["SponsorLogoContentType"].ToString();
                }

                if (row["SponsorLogoFileName"] != DBNull.Value)
                {
                    rv.SponsorLogoFileName = row["SponsorLogoFileName"].ToString();
                }

                if (row["DeveloperName"] != DBNull.Value)
                {
                    rv.DeveloperName = row["DeveloperName"].ToString();
                }

                if (row["DeveloperLogo"] != DBNull.Value)
                {
                    rv.DeveloperLogo = (byte[])row["DeveloperLogo"];
                }

                if (row["DeveloperLogoContentType"] != DBNull.Value)
                {
                    rv.DeveloperLogoContentType = row["DeveloperLogoContentType"].ToString();
                }


                if (row["DeveloperLogoFileName"] != DBNull.Value)
                {
                    rv.DeveloperLogoFileName = row["DeveloperLogoFileName"].ToString();
                }


                if (row["ArtistName"] != DBNull.Value)
                {
                    rv.ArtistName = row["ArtistName"].ToString();
                }

                if (row["Phone"] != DBNull.Value)
                {
                    rv.Phone = row["Phone"].ToString();
                }

                if (row["UserName"] != DBNull.Value)
                {
                    rv.UserName = row["UserName"].ToString();
                }


            }




            return rv;
        }

        private static UserProfile GetUserProfileFromDataRow(DataRow row)
        {
            UserProfile rv = null;

            if (row != null)
            {
                rv = new UserProfile();

                rv.InternalUserID = Int32.Parse(row["UserID"].ToString());
                rv.InternalMembershipUserGuid = row["UserGuid"].ToString();

                if (row["FirstName"] != DBNull.Value)
                {
                    rv.FirstName = row["FirstName"].ToString();
                }

                if (row["LastName"] != DBNull.Value)
                {
                    rv.LastName = row["LastName"].ToString();
                }

                if (row["Email"] != DBNull.Value)
                {
                    rv.Email = row["Email"].ToString();
                }

                if (row["WebsiteURL"] != DBNull.Value)
                {
                    rv.WebsiteURL = row["WebsiteURL"].ToString();
                }

                if (row["SponsorName"] != DBNull.Value)
                {
                    rv.SponsorName = row["SponsorName"].ToString();
                }

                if (row["SponsorLogo"] != DBNull.Value)
                {
                    rv.SponsorLogo = (byte[])row["SponsorLogo"];
                }

                if (row["SponsorLogoContentType"] != DBNull.Value)
                {
                    rv.SponsorLogoContentType = row["SponsorLogoContentType"].ToString();
                }

                if (row["SponsorLogoFileName"] != DBNull.Value)
                {
                    rv.SponsorLogoFileName = row["SponsorLogoFileName"].ToString();
                }

                if (row["DeveloperName"] != DBNull.Value)
                {
                    rv.DeveloperName = row["DeveloperName"].ToString();
                }

                if (row["DeveloperLogo"] != DBNull.Value)
                {
                    rv.DeveloperLogo = (byte[])row["DeveloperLogo"];
                }

                if (row["DeveloperLogoContentType"] != DBNull.Value)
                {
                    rv.DeveloperLogoContentType = row["DeveloperLogoContentType"].ToString();
                }

                if (row["DeveloperLogoFileName"] != DBNull.Value)
                {
                    rv.DeveloperLogoFileName = row["DeveloperLogoFileName"].ToString();
                }

                if (row["ArtistName"] != DBNull.Value)
                {
                    rv.ArtistName = row["ArtistName"].ToString();
                }

                if (row["Phone"] != DBNull.Value)
                {
                    rv.Phone = row["Phone"].ToString();
                }

                if (row["UserName"] != DBNull.Value)
                {
                    rv.UserName = row["UserName"].ToString();
                }

            }

            return rv;
        }

        #endregion

        public static List<UserProfile> GetAllUserProfilesList()
        {
            List<UserProfile> allProfilesList = new List<UserProfile>();

            DataTable dt = GetAllUserProfilesDataTable();

            if (dt != null && dt.Rows.Count > 0)
            {

                foreach (DataRow row in dt.Rows)
                {
                    UserProfile p = GetUserProfileFromDataRow(row);

                    allProfilesList.Add(p);

                }

            }



            return allProfilesList;
        }

        public static DataTable GetAllUserProfilesDataTable()
        {
            DataTable dt = new DataTable();

            using (var dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (var da = new OdbcDataAdapter(GetAllUserProfiles_QUERY, dbConn))
                {

                    try
                    {

                        da.Fill(dt);

                    }
                    catch (Exception)
                    {


                    }

                }

            }





            return dt;
        }

        public static UserProfile GetUserProfileByUserID(int userID)
        {
            UserProfile rv = null;

            using (var dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (var da = new OdbcDataAdapter(GetUserByUserID_QUERY, dbConn))
                {

                    da.SelectCommand.Parameters.AddWithValue("@UserID", userID);

                    DataTable dt = new DataTable();

                    try
                    {
                        da.Fill(dt);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            rv = GetUserProfileFromDataTable(dt);
                        }


                    }
                    catch
                    {


                    }

                }

            }

            return rv;
        }

        public static UserProfile GetUserProfileByUserName(string uName)
        {
            UserProfile rv = null;

            using (var dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (var da = new OdbcDataAdapter(GetUserByUserName_QUERY, dbConn))
                {

                    da.SelectCommand.Parameters.AddWithValue("@UserName", uName);

                    DataTable dt = new DataTable();

                    try
                    {
                        da.Fill(dt);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            rv = GetUserProfileFromDataTable(dt);
                        }


                    }
                    catch
                    {


                    }

                }

            }

            return rv;
        }


        public static UserProfile GetUserProfileByMembershipUserGUID(string membershipUserGUID)
        {
            UserProfile rv = null;

            using (OdbcConnection dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (OdbcDataAdapter da = new OdbcDataAdapter(GetUserByMembershipUserGuid_QUERY, dbConn))
                {

                    da.SelectCommand.Parameters.AddWithValue("@UserGuid", membershipUserGUID);

                    DataTable dt = new DataTable();

                    try
                    {
                        da.Fill(dt);

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            rv = GetUserProfileFromDataTable(dt);
                        }


                    }
                    catch
                    {


                    }

                }

            }


            return rv;
        }

        public static UserProfile InsertUserProfile(string membershipUserGUID, string firstName, string lastName, string email, string createdBy, string userName, string websiteURL = "", string sponsorName = "", string developerName = "", string artistName = "", string phone = "", byte[] sponsorLogo = null, string sponsorLogoContentType = "", string sponsorLogoFileName = "", byte[] developerLogo = null, string developerLogoContentType = "", string developerLogoFileName = "")
        {

            UserProfile rv = null;

            //check if user profile already exists by membershipuserGuid, if so return
            rv = GetUserProfileByMembershipUserGUID(membershipUserGUID);
            if (rv != null)
                return rv;


            using (var dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {

                using (var dbCommand = dbConn.CreateCommand())
                {

                    //set command text
                    dbCommand.CommandText = InsertUserProfile_QUERY;

                    //TODO: Validate args

                    //parameters

                    dbCommand.Parameters.AddWithValue("@UserGuid", membershipUserGUID);
                    dbCommand.Parameters.AddWithValue("@FirstName", firstName); ;
                    dbCommand.Parameters.AddWithValue("@LastName", lastName);
                    dbCommand.Parameters.AddWithValue("@Email", email);
                    dbCommand.Parameters.AddWithValue("@WebsiteURL", websiteURL);
                    dbCommand.Parameters.AddWithValue("@SponsorName", sponsorName);



                    if (sponsorLogo != null)
                    {
                        dbCommand.Parameters.AddWithValue("@SponsorLogo", sponsorLogo);
                    }
                    else
                    {
                        dbCommand.Parameters.AddWithValue("@SponsorLogo", DBNull.Value);
                    }


                    if (!string.IsNullOrEmpty(sponsorLogoContentType))
                    {
                        dbCommand.Parameters.AddWithValue("@SponsorLogoContentType", sponsorLogoContentType);
                    }
                    else
                    {
                        dbCommand.Parameters.AddWithValue("@SponsorLogoContentType", DBNull.Value);
                    }

                    dbCommand.Parameters.AddWithValue("@DeveloperName", developerName);

                    if (developerLogo != null)
                    {
                        dbCommand.Parameters.AddWithValue("@DeveloperLogo", developerLogo);
                    }
                    else
                    {
                        dbCommand.Parameters.AddWithValue("@DeveloperLogo", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(developerLogoContentType))
                    {
                        dbCommand.Parameters.AddWithValue("@DeveloperLogoContentType", developerLogoContentType);
                    }
                    else
                    {
                        dbCommand.Parameters.AddWithValue("@DeveloperLogoContentType", DBNull.Value);
                    }



                    dbCommand.Parameters.AddWithValue("@ArtistName", artistName);
                    dbCommand.Parameters.AddWithValue("@Phone", phone);

                    dbCommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now.Date);
                    dbCommand.Parameters.AddWithValue("@CreatedBy", createdBy);

                    dbCommand.Parameters.AddWithValue("@UserName", userName);


                    if (!string.IsNullOrEmpty(developerLogoFileName))
                    {
                        dbCommand.Parameters.AddWithValue("@DeveloperLogoFileName", developerLogoContentType);
                    }
                    else
                    {
                        dbCommand.Parameters.AddWithValue("@DeveloperLogoFileName", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(sponsorLogoFileName))
                    {
                        dbCommand.Parameters.AddWithValue("@SponsorLogoFileName", sponsorLogoContentType);
                    }
                    else
                    {
                        dbCommand.Parameters.AddWithValue("@SponsorLogoFileName", DBNull.Value);
                    }




                    try
                    {
                        dbConn.Open();
                        dbCommand.ExecuteScalar();

                        rv = GetUserProfileByUserName(userName.Trim());

                    }
                    catch (Exception ex)
                    {

                        string error = ex.InnerException.ToString();
                    }

                }
            }

            return rv;
        }

        public static bool UpdateUserProfile(UserProfile p, string editedBy = "")
        {
            bool success = false;

            using (var dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (var dbCommand = dbConn.CreateCommand())
                {
                    dbCommand.CommandText = UpdateUserProfile_QUERY;

                    dbCommand.Parameters.AddWithValue("@FirstName", p.FirstName);
                    dbCommand.Parameters.AddWithValue("@LastName", p.LastName);
                    dbCommand.Parameters.AddWithValue("@Email", p.Email);
                    dbCommand.Parameters.AddWithValue("@WebsiteURL", p.WebsiteURL);
                    dbCommand.Parameters.AddWithValue("@SponsorName", p.SponsorName);



                    if (p.SponsorLogo != null)
                    {
                        dbCommand.Parameters.AddWithValue("@SponsorLogo", p.SponsorLogo);
                    }
                    else
                    {

                        dbCommand.Parameters.AddWithValue("@SponsorLogo", DBNull.Value);

                    }


                    if (!string.IsNullOrEmpty(p.SponsorLogoContentType))
                    {
                        dbCommand.Parameters.AddWithValue("@SponsorLogoContentType", p.SponsorLogoContentType);
                    }
                    else
                    {

                        dbCommand.Parameters.AddWithValue("@SponsorLogoContentType", DBNull.Value);
                    }




                    dbCommand.Parameters.AddWithValue("@DeveloperName", p.DeveloperName);



                    if (p.DeveloperLogo != null)
                    {
                        dbCommand.Parameters.AddWithValue("@DeveloperLogo", p.DeveloperLogo);
                    }
                    else
                    {
                        dbCommand.Parameters.AddWithValue("@DeveloperLogo", DBNull.Value);
                    }


                    if (!string.IsNullOrEmpty(p.DeveloperLogoContentType))
                    {
                        dbCommand.Parameters.AddWithValue("@DeveloperLogoContentType", p.DeveloperLogoContentType);
                    }
                    else
                    {
                        dbCommand.Parameters.AddWithValue("@DeveloperLogoContentType", DBNull.Value);

                    }




                    dbCommand.Parameters.AddWithValue("@ArtistName", p.ArtistName);
                    dbCommand.Parameters.AddWithValue("@Phone", p.Phone);



                    dbCommand.Parameters.AddWithValue("@LastEditedBy", editedBy);
                    dbCommand.Parameters.AddWithValue("@LastEditedDate", DateTime.Now.Date);


                    if (!string.IsNullOrEmpty(p.DeveloperLogoFileName))
                    {
                        dbCommand.Parameters.AddWithValue("@DeveloperLogoFileName", p.DeveloperLogoFileName);
                    }
                    else
                    {

                        dbCommand.Parameters.AddWithValue("@DeveloperLogoFileName", DBNull.Value);
                    }

                    if (!string.IsNullOrEmpty(p.SponsorLogoFileName))
                    {
                        dbCommand.Parameters.AddWithValue("@SponsorLogoFileName", p.SponsorLogoFileName);
                    }
                    else
                    {

                        dbCommand.Parameters.AddWithValue("@SponsorLogoFileName", DBNull.Value);
                    }



                    dbCommand.Parameters.AddWithValue("@UserID", p.UserID);

                    try
                    {
                        dbConn.Open();

                        int rowsAffected = -1;

                        //update
                        rowsAffected = dbCommand.ExecuteNonQuery();

                        if (rowsAffected > -1)
                        {
                            success = true;
                        }



                    }
                    catch (Exception ex)
                    {
                        string error = ex.InnerException.ToString();
                    }


                }
            }

            return success;
        }

        public static bool DeleteUserProfileByID(int userID)
        {
            bool rv = false;

            using (var dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (var dbCommand = dbConn.CreateCommand())
                {
                    //set command text
                    dbCommand.CommandText = DeleteUserProfileByUserID_QUERY;

                    //parameters
                    dbCommand.Parameters.AddWithValue("@UserID", OdbcType.Int).Value = userID;

                    try
                    {
                        dbConn.Open();

                        int rowsAffected = 0;

                        //update
                        rowsAffected = dbCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            rv = true;
                        }




                    }
                    catch (Exception ex)
                    {
                        //TODO: Remove
                        string error = ex.InnerException.ToString();
                    }


                }

            }



            return rv;
        }

        public static bool DeleteUserProfileByUserGuid(string userGuid)
        {
            bool rv = false;

            using (var dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (var dbCommand = dbConn.CreateCommand())
                {
                    //set command text
                    dbCommand.CommandText = DeleteUserProfileByUserGuid_QUERY;

                    //parameters
                    dbCommand.Parameters.AddWithValue("@UserGuid", OdbcType.Text).Value = userGuid;

                    try
                    {
                        dbConn.Open();

                        int rowsAffected = 0;

                        //update
                        rowsAffected = dbCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            rv = true;
                        }




                    }
                    catch (Exception ex)
                    {
                        //TODO: Remove
                        string error = ex.InnerException.ToString();
                    }


                }

            }



            return rv;
        }

        public static bool DeleteUserProfileByUserName(string uName)
        {
            bool rv = false;

            using (var dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (var dbCommand = dbConn.CreateCommand())
                {
                    //set command text
                    dbCommand.CommandText = DeleteUserProfileByUserName_QUERY;

                    //parameters
                    dbCommand.Parameters.AddWithValue("@UserName", uName);

                    try
                    {
                        dbConn.Open();

                        int rowsAffected = 0;

                        //update
                        rowsAffected = dbCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            rv = true;
                        }




                    }
                    catch (Exception ex)
                    {
                        //TODO: Remove
                        string error = ex.InnerException.ToString();
                    }


                }

            }



            return rv;
        }

        public static DataTable GetAllAdministrativeUsers()
        {
            DataTable dt = new DataTable();

            using (OdbcConnection dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (OdbcDataAdapter da = new OdbcDataAdapter(GetAdministrativeUsers_Query, dbConn))
                {

                    try
                    {

                        da.Fill(dt);

                    }
                    catch (Exception)
                    {


                    }

                }

            }
            return dt;



        }

        public static DataTable GetUserProfileSponsorLogoByUserID(string userID)
        {

            string sql = "SELECT SponsorLogo as Logo, SponsorLogoContentType as LogoContentType, SponsorLogoFileName as FileName FROM UserProfiles WHERE UserID = ?";

            DataTable dt = new DataTable();
            dt.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
            dt.Columns.Add("LogoContentType");
            dt.Columns.Add("FileName");


            using (var dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (var da = new OdbcDataAdapter(sql, dbConn))
                {

                    try
                    {
                        da.SelectCommand.Parameters.AddWithValue("@UserID", userID);
                        da.Fill(dt);


                    }
                    catch
                    {


                    }

                }

            }

            return dt;
        }


        public static DataTable GetUserProfileDeveloperLogoByUserID(string userID)
        {

            string sql = "SELECT DeveloperLogo as Logo, DeveloperLogoContentType as LogoContentType, DeveloperLogoFileName as FileName FROM UserProfiles WHERE UserID = ?";

            DataTable dt = new DataTable();
            dt.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
            dt.Columns.Add("LogoContentType");
            dt.Columns.Add("FileName");


            using (var dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (var da = new OdbcDataAdapter(sql, dbConn))
                {

                    try
                    {
                        da.SelectCommand.Parameters.AddWithValue("@UserID", userID);
                        da.Fill(dt);


                    }
                    catch
                    {


                    }

                }

            }

            return dt;
        }

        public static DataTable GetUserProfileSponsorLogoByUserName(string userName)
        {

            string sql = "";

            sql = "SELECT SponsorLogo as Logo, SponsorLogoContentType as LogoContentType, SponsorLogoFileName as FileName FROM UserProfiles WHERE UserName = ?";


            DataTable dt = new DataTable();
            dt.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
            dt.Columns.Add("LogoContentType");
            dt.Columns.Add("FileName");


            using (var dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (var da = new OdbcDataAdapter(sql, dbConn))
                {

                    try
                    {
                        da.SelectCommand.Parameters.AddWithValue("@UserName", userName);
                        da.Fill(dt);


                    }
                    catch
                    {


                    }

                }

            }

            return dt;
        }



        public static DataTable GetUserProfileDeveloperLogoByUserName(string userName)
        {

            string sql = "";

            sql = "SELECT DeveloperLogo as Logo, DeveloperLogoContentType as LogoContentType, DeveloperLogoFileName as FileName FROM UserProfiles WHERE UserName = ?";


            DataTable dt = new DataTable();
            dt.Columns.Add("Logo", System.Type.GetType("System.Byte[]"));
            dt.Columns.Add("LogoContentType");
            dt.Columns.Add("FileName");


            using (var dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (var da = new OdbcDataAdapter(sql, dbConn))
                {

                    try
                    {
                        da.SelectCommand.Parameters.AddWithValue("@UserName", userName);
                        da.Fill(dt);


                    }
                    catch
                    {


                    }

                }

            }

            return dt;
        }



        #region MembershipUser Functions

        public static DataTable GetAllAspnetUsersNotApprovedDataTable()
        {
            DataTable dt = new DataTable();

            using (OdbcConnection dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (OdbcDataAdapter da = new OdbcDataAdapter(GetAllAspnetUsersNotApproved_QUERY, dbConn))
                {

                    try
                    {

                        da.Fill(dt);

                    }
                    catch (Exception)
                    {


                    }

                }

            }
            return dt;



        }


        public static DataTable GetAllAspnetUsersLockedOutDataTable()
        {
            DataTable dt = new DataTable();

            using (OdbcConnection dbConn = new OdbcConnection(PostgreSQLConnectionString))
            {
                using (OdbcDataAdapter da = new OdbcDataAdapter(GetAllAspnetUsersLockedOut_QUERY, dbConn))
                {

                    try
                    {

                        da.Fill(dt);

                    }
                    catch (Exception)
                    {


                    }

                }

            }
            return dt;



        }

        #endregion

    }


}
