using System;
using System.Configuration.Provider;
using System.Security.Permissions;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.IO;
namespace Simple.Providers.MySQL
{
    public class MysqlPersonalizationProvider : PersonalizationProvider
    {
        private string m_ApplicationName;
        public override string ApplicationName
        {
            get { return m_ApplicationName; }
            set { m_ApplicationName = value; }
        }

        private string m_ConnectionStringName;

        public string ConnectionStringName
        {
            get { return m_ConnectionStringName; }
            set { m_ConnectionStringName = value; }
        }

        public override void Initialize(string name,
            NameValueCollection config)
        {
            // Verify that config isn't null
            if (config == null)
                throw new ArgumentNullException("config");

            // Assign the provider a default name if it doesn't have one
            if (String.IsNullOrEmpty(name))
                name = "SimpleMySqlPersonalizationProvider";

            // Add a default "description" attribute to config if the
            // attribute doesn't exist or is empty
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description",
                    "Simple MySql personalization provider");
            }

            // Call the base class's Initialize method
            base.Initialize(name, config);

            if (string.IsNullOrEmpty(config["connectionStringName"]))
            {
                throw new ProviderException
                    ("ConnectionStringName property has not been specified");
            }
            else
            {
                m_ConnectionStringName = config["connectionStringName"];
                config.Remove("connectionStringName");
            }

            if (string.IsNullOrEmpty(config["applicationName"]))
            {
                throw new ProviderException
                    ("applicationName property has not been specified");
            }
            else
            {
                m_ApplicationName = config["applicationName"];
                config.Remove("applicationName");
            }

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException
                        ("Unrecognized attribute: " + attr);
            }

        }

        protected override void LoadPersonalizationBlobs
            (WebPartManager webPartManager, string path, string userName,
            ref byte[] sharedDataBlob, ref byte[] userDataBlob)
        {
            // Load shared state
            sharedDataBlob = null;
            userDataBlob = null;
            object sharedBlobDataObject = null;
            object userBlobDataObject = null;
            string sSQLShared = null;
            string sSQLUser = null;
            try
            {
                sSQLUser = "SELECT `personalizationblob` FROM `personalization`" + Environment.NewLine +
                    "WHERE `username` = '" + userName + "' AND " + Environment.NewLine +
                    "`path` = '" + path + "' AND " + Environment.NewLine +
                    "`applicationname` = '" + m_ApplicationName + "';";
                sSQLShared = "SELECT `personalizationblob` FROM `personalization`" + Environment.NewLine +
                    "WHERE `username` IS NULL AND " + Environment.NewLine +
                    "`path` = '" + path + "' AND " + Environment.NewLine +
                    "`applicationname` = '" + m_ApplicationName + "';";
                sharedBlobDataObject = RawDBQuery.ExecuteScalarOnDB(sSQLShared, System.Configuration.ConfigurationManager.ConnectionStrings[m_ConnectionStringName].ToString());
                userBlobDataObject = RawDBQuery.ExecuteScalarOnDB(sSQLUser, System.Configuration.ConfigurationManager.ConnectionStrings[m_ConnectionStringName].ToString());
                if (sharedBlobDataObject != null)
                    sharedDataBlob =
                        (byte[])sharedBlobDataObject;
                if (userBlobDataObject != null)
                    userDataBlob =
                        (byte[])userBlobDataObject;
            }
            catch (FileNotFoundException)
            {
                // Not an error if file doesn't exist
            }
            finally
            {
                sSQLUser = null;
                sSQLShared = null;
            }
        }

        protected override void ResetPersonalizationBlob
            (WebPartManager webPartManager, string path, string userName)
        {
            // Delete the specified personalization file
            string sSQL = null;
            try
            {
                sSQL = "DELETE FROM `personalization` WHERE `username` = '"+userName+"' AND `path` = '"+path+"' AND `applicationname` = '"+m_ApplicationName+"';";
                RawDBQuery.ExecuteNonQueryOnDB(sSQL, System.Configuration.ConfigurationManager.ConnectionStrings[m_ConnectionStringName].ToString());
            }
            catch (System.Data.Odbc.OdbcException) { }
        }

        protected override void SavePersonalizationBlob
            (WebPartManager webPartManager, string path, string userName,
            byte[] dataBlob)
        {
            System.Data.Odbc.OdbcCommand updateCommand = null;
            System.Data.Odbc.OdbcConnection updateConnection = null;
            string sSQL = null;
            try
            {
                sSQL = "SELECT COUNT(`username`) FROM `personalization` WHERE `username` = '"+userName+"' AND `path` = '"+path+"' and `applicationname` = '"+m_ApplicationName+"';";
                updateConnection = new System.Data.Odbc.OdbcConnection(System.Configuration.ConfigurationManager.ConnectionStrings[m_ConnectionStringName].ToString());
                if (int.Parse(RawDBQuery.ExecuteScalarOnDB(sSQL, System.Configuration.ConfigurationManager.ConnectionStrings[m_ConnectionStringName].ToString()).ToString()) > 0)
                {
                    sSQL = "UPDATE `personalization` SET `personalizationblob` = ? WHERE `username` = ? AND `applicationname` = ? AND `path` = ?;";
                    updateCommand = new System.Data.Odbc.OdbcCommand(sSQL,updateConnection);
                    updateCommand.Parameters.Clear();
                    updateCommand.Parameters.Add(new System.Data.Odbc.OdbcParameter("personalizationblob",dataBlob));
                    updateCommand.Parameters.Add(new System.Data.Odbc.OdbcParameter("username",userName));
                    updateCommand.Parameters.Add(new System.Data.Odbc.OdbcParameter("applicationname",m_ApplicationName));
                    updateCommand.Parameters.Add(new System.Data.Odbc.OdbcParameter("path",path));
                }
                else
                {
                    sSQL = "INSERT INTO `personalization` (`username`,`path`,`applicationname`,`personalizationblob`) VALUES (?, ?, ?, ?);";
                    updateCommand = new System.Data.Odbc.OdbcCommand(sSQL,updateConnection);
                    updateCommand.Parameters.Clear();
                    updateCommand.Parameters.Add(new System.Data.Odbc.OdbcParameter("username",userName));
                    updateCommand.Parameters.Add(new System.Data.Odbc.OdbcParameter("path",path));
                    updateCommand.Parameters.Add(new System.Data.Odbc.OdbcParameter("applicationname",m_ApplicationName));
                    updateCommand.Parameters.Add(new System.Data.Odbc.OdbcParameter("personalizationblob",dataBlob));
                }
                updateConnection.Open();
                updateCommand.ExecuteNonQuery();
            }
            finally
            {
                if (updateConnection != null)
                    if (updateConnection.State != System.Data.ConnectionState.Closed)
                        updateConnection.Close();
                    else
                        updateConnection.Dispose();
                updateConnection = null;
                if (updateCommand != null) updateCommand.Dispose();
                updateCommand = null;
                sSQL = null;
            }
        }

        public override PersonalizationStateInfoCollection FindState
            (PersonalizationScope scope, PersonalizationStateQuery query,
            int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override int GetCountOfState(PersonalizationScope scope,
            PersonalizationStateQuery query)
        {
            throw new NotSupportedException();
        }

        public override int ResetState(PersonalizationScope scope,
            string[] paths, string[] usernames)
        {
            throw new NotSupportedException();
        }

        public override int ResetUserState(string path,
            DateTime userInactiveSinceDate)
        {
            throw new NotSupportedException();
        }
    }

}
