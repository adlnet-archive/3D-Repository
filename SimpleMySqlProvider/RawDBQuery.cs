using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Web.Security;

namespace Simple.Providers.MySQL
{
    public static class RawDBQuery
    {
        public static object ExecuteScalarOnDB(string sSQL, string s_ConnectionString)
        {
            System.Data.Odbc.OdbcConnection QConnection = null;
            System.Data.Odbc.OdbcCommand QCommand = null;
            try
            {
                QConnection = new System.Data.Odbc.OdbcConnection(s_ConnectionString);
                QCommand = new System.Data.Odbc.OdbcCommand(sSQL, QConnection);

                QConnection.Open();

                return QCommand.ExecuteScalar();
            }
            finally
            {
                if (QCommand != null) QCommand.Dispose();
                QCommand = null;
                if (QConnection != null && QConnection.State != System.Data.ConnectionState.Closed) QConnection.Close();
                if (QConnection != null) QConnection.Dispose();
                QConnection = null;
            }
        }

        public static void ExecuteNonQueryOnDB(string sSQL, string s_ConnectionString)
        {
            System.Data.Odbc.OdbcConnection QConnection = null;
            System.Data.Odbc.OdbcCommand QCommand = null;
            try
            {
                QConnection = new System.Data.Odbc.OdbcConnection(s_ConnectionString);
                QCommand = new System.Data.Odbc.OdbcCommand(sSQL, QConnection);

                QConnection.Open();

                QCommand.ExecuteNonQuery();
            }
            finally
            {
                if (QCommand != null) QCommand.Dispose();
                QCommand = null;
                if (QConnection != null && QConnection.State != System.Data.ConnectionState.Closed) QConnection.Close();
                if (QConnection != null) QConnection.Dispose();
                QConnection = null;
            }
        }

        public static System.Data.Odbc.OdbcDataReader ExecuteReaderQueryOnDB(string sSQL, string s_ConnectionString)
        {
            System.Data.Odbc.OdbcConnection QConnection = null;
            System.Data.Odbc.OdbcCommand QCommand = null;
            try
            {
                QConnection = new System.Data.Odbc.OdbcConnection(s_ConnectionString);
                QCommand = new System.Data.Odbc.OdbcCommand(sSQL, QConnection);

                QConnection.Open();

                return QCommand.ExecuteReader();
            }
            finally
            {
                if (QCommand != null) QCommand.Dispose();
                QCommand = null;
                if (QConnection != null && QConnection.State != System.Data.ConnectionState.Closed) QConnection.Close();
                if (QConnection != null) QConnection.Dispose();
                QConnection = null;
            }
        }

        public static System.Data.DataSet ExecuteDatasetQueryOnDB(string sSQL, string sConnectionString)
        {
            System.Data.Odbc.OdbcDataAdapter QDataAdapter = null;
            DataSet QDataSet = null;
            try
            {
                QDataSet = new DataSet();
                QDataAdapter = new System.Data.Odbc.OdbcDataAdapter(sSQL, sConnectionString);

                QDataAdapter.Fill(QDataSet);

                return QDataSet;
            }
            finally
            {
                if (QDataSet != null) QDataSet.Dispose();
                QDataSet = null;
                if (QDataAdapter != null) QDataAdapter.Dispose();
                QDataAdapter = null;
            }
        }


        //
        // MembershipProvider.StoreNewToken
        //

        public static void StoreNewToken(string email, string token, DateTime expire, string connectionString)
        {
            string tokenTableName = "usertokens";
            OdbcConnection conn = new OdbcConnection(connectionString);

            OdbcCommand delete = new OdbcCommand(" DELETE FROM `" + tokenTableName + "`" +
                                                 " WHERE email = ? ", conn);

            OdbcCommand cmd = new OdbcCommand(" INSERT INTO `" + tokenTableName + "`" +
                                              " (email, token, expire)" +
                                              " VALUES(?, ?, ?)", conn);

            delete.Parameters.Add("@email", OdbcType.VarChar, 128).Value = email;
            cmd.Parameters.Add("@email", OdbcType.VarChar, 128).Value = email;
            cmd.Parameters.Add("@token", OdbcType.Char, 64).Value = token;          
            cmd.Parameters.Add("@expire", OdbcType.DateTime).Value = expire;
             
            try
            {
                conn.Open();
                delete.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
            }
            catch (OdbcException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool deleteUserTokens(string email, string connectionString)
        {
            bool returnVal = false;
            string tokenTableName = "usertokens";
            OdbcConnection conn = new OdbcConnection(connectionString);
            OdbcCommand delete = new OdbcCommand(" DELETE FROM `" + tokenTableName + "`" +
                                     " WHERE email = ? OR expire < ? ", conn);

            delete.Parameters.Add("@email", OdbcType.VarChar, 128).Value = email;
            delete.Parameters.Add("@expire", OdbcType.DateTime).Value = DateTime.Now;

            try
            {
                conn.Open();
                delete.ExecuteNonQuery();
                returnVal = true;
            }
            catch (OdbcException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return returnVal;
        }

        public static bool checkUserToken(string email, string token, DateTime now, string connectionString)
        {
            string tokenTableName = "usertokens";
            bool returnVal = false;
            OdbcConnection conn = new OdbcConnection(connectionString);

            OdbcCommand cmd = new OdbcCommand("SELECT Count(*) FROM `" + tokenTableName + "` " +
                                              "WHERE email = ? AND token = ? AND expire > ? ", conn);

            cmd.Parameters.Add("@email", OdbcType.VarChar, 128).Value = email;
            cmd.Parameters.Add("@token", OdbcType.Char, 64).Value = token;
            cmd.Parameters.Add("@expire", OdbcType.DateTime).Value = now;

            int totalRecords = 0;

            try
            {
                conn.Open();
                totalRecords = int.Parse(cmd.ExecuteScalar().ToString());

                if (totalRecords > 0)
                    returnVal = true;
            }
            catch (OdbcException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }

            return returnVal;
        }
    }

}
