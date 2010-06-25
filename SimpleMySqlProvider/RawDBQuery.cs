using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

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
    }

}
