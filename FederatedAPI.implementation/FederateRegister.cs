using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace FederatedAPI.implementation
{
    public enum FederateState { Active, Offline, Unapproved, Banned, Unknown };

    public class FederateRecord
    {
        public string JSONAPI;
        public string XMLAPI;
        public string SOAPAPI;
        public string namespacePrefix;
        public string OrginizationName;
        public string OrganizationURL;
        public string OrganizationPOC;
        public string OrganizationPOCEmail;
        public string OrganizationPOCPassword;
        public FederateState ActivationState;
        public bool AllowFederatedSearch;
        public bool AllowFederatedDownload;
    }
    public class FederateRegister
    {

        private string ConnectionString;
        private System.Data.Odbc.OdbcConnection mConnection;
        public FederateRegister()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["FederateNamespaceDatabaseConnection"].ConnectionString;
            mConnection = new System.Data.Odbc.OdbcConnection(ConnectionString);

            if (!CheckConnection())
                throw (new SystemException("Could not connect to Federate Register Database"));
            

        }
        public bool CheckConnection()
        {
            int sleeptime = 0;
            while( (mConnection.State == System.Data.ConnectionState.Connecting ||
                mConnection.State == System.Data.ConnectionState.Connecting ||
                mConnection.State == System.Data.ConnectionState.Connecting) &&
                sleeptime < 5000
                )
            {
                sleeptime += 100;
                System.Threading.Thread.Sleep(100);
            }
            if (sleeptime > 5000)
                return false;

            if (mConnection.State != System.Data.ConnectionState.Open)
                mConnection.Open();
            
            return true;
        }
        public List<FederateRecord> GetAllFederateRecords()
        {
            List<FederateRecord> results = new List<FederateRecord>();
            if (CheckConnection())
            {
                using (var command = mConnection.CreateCommand())
                {
                    command.CommandText = "{CALL GetAllFederateRecords()}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    using (var resultSet = command.ExecuteReader())
                    {
                        while (resultSet.Read())
                        {
                            FederateRecord fd = new FederateRecord();
                            fd.JSONAPI = resultSet["JSONAPI"].ToString();
                            fd.SOAPAPI = resultSet["SOAPAPI"].ToString();
                            fd.XMLAPI = resultSet["XMLAPI"].ToString();
                            fd.namespacePrefix = resultSet["prefix"].ToString();

                            fd.OrginizationName = resultSet["OrganizationName"].ToString();
                            fd.OrganizationURL = resultSet["OrganizationURL"].ToString();
                            fd.OrganizationPOC = resultSet["OrganizationPOC"].ToString();
                            fd.OrganizationPOCEmail = resultSet["OrganizationPOCEmail"].ToString();
                            fd.OrganizationPOCPassword = resultSet["OrganizationPOCPassword"].ToString();
                            fd.ActivationState = (FederateState)(System.Convert.ToInt16(resultSet["ActivationState"].ToString()));

                            if (resultSet["AllowFederatedSearch"].ToString() == "1")
                                fd.AllowFederatedSearch = true;
                            if (resultSet["AllowFederatedDownload"].ToString() == "1")
                                fd.AllowFederatedDownload = true;

                            results.Add(fd);
                        }
                    }
                }
            }
            else
            {
                throw (new SystemException("Could not connect to Federate Register Database"));
            }

            return results;
        }
        public FederateRecord GetFederateRecord(string prefix)
        {
            FederateRecord fd = null;
            if (CheckConnection())
            {
                using (var command = mConnection.CreateCommand())
                {
                    command.CommandText = "{CALL GetFederateRecord(?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("newprefix",prefix);
                    using (var resultSet = command.ExecuteReader())
                    {
                        while (resultSet.Read())
                        {
                            fd = new FederateRecord();
                            fd.JSONAPI = resultSet["JSONAPI"].ToString();
                            fd.SOAPAPI = resultSet["SOAPAPI"].ToString();
                            fd.XMLAPI = resultSet["XMLAPI"].ToString();
                            fd.namespacePrefix = resultSet["prefix"].ToString();

                            fd.OrginizationName = resultSet["OrganizationName"].ToString();
                            fd.OrganizationURL = resultSet["OrganizationURL"].ToString();
                            fd.OrganizationPOC = resultSet["OrganizationPOC"].ToString();
                            fd.OrganizationPOCEmail = resultSet["OrganizationPOCEmail"].ToString();
                            fd.OrganizationPOCPassword = resultSet["OrganizationPOCPassword"].ToString();
                            fd.ActivationState = (FederateState)(System.Convert.ToInt16(resultSet["ActivationState"].ToString()));

                            if (resultSet["AllowFederatedSearch"].ToString() == "1")
                                fd.AllowFederatedSearch = true;
                            if (resultSet["AllowFederatedDownload"].ToString() == "1")
                                fd.AllowFederatedDownload = true;
                        }
                    }
                }
            }
            else
            {
                throw (new SystemException("Could not connect to Federate Register Database"));
            }

            return fd;
        }
        public FederateRecord CreateFederateRecord(FederateRecord fd)
        {
            if (CheckConnection())
            {
                using (var command = mConnection.CreateCommand())
                {
                    command.CommandText = "{CALL CreateFederateRecord(?,?,?,?,?,?,?,?,?,?,?,?)}";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    command.Parameters.AddWithValue("newPrefix", fd.namespacePrefix);
                    command.Parameters.AddWithValue("newJSONAPI", fd.JSONAPI);
                    command.Parameters.AddWithValue("newXMLAPI", fd.XMLAPI);
                    command.Parameters.AddWithValue("newSOAPAPI", fd.SOAPAPI);
                    command.Parameters.AddWithValue("newOrganizationName", fd.OrginizationName);
                    command.Parameters.AddWithValue("newOrganizationURL", fd.OrganizationURL);
                    command.Parameters.AddWithValue("newOrganizationPOC", fd.OrganizationPOC);
                    command.Parameters.AddWithValue("newOrganizationPOCEmail", fd.OrganizationPOCEmail);
                    command.Parameters.AddWithValue("newOrganizationPOCPassword", fd.OrganizationPOCPassword);
                    command.Parameters.AddWithValue("newActivationState", fd.ActivationState);
                    command.Parameters.AddWithValue("newAllowFederatedSearch", fd.AllowFederatedSearch);
                    command.Parameters.AddWithValue("newAllowFederatedDownload", fd.AllowFederatedDownload);

                    command.ExecuteScalar();
                    
                }
            }
            else
            {
                throw( new SystemException("Could not connect to Federate Register Database"));
            }
            return fd;
        }
    }
}