//------------------------------------------------------------------------------
// <copyright file="SqlConnectionHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Simple.Providers.MySQL
{

    using System.Web;
    using System;
    using System.Globalization;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Collections.Specialized;
    using System.Web.Util;
    using System.Web.Hosting;
    using System.Web.Configuration;
    using System.Security.Permissions;
    using System.IO;
    using System.Web.Management;
    using System.Threading;
    using System.Configuration.Provider;
    using System.Diagnostics;
    using System.Data;
    using System.Data.Odbc;

    /// <devdoc>
    /// </devdoc>
    internal static class MysqlConnectionHelper
    {
        internal const string s_strUpperDataDirWithToken = "|DATADIRECTORY|";
        private static object s_lock = new object();

        /// <devdoc>
        /// </devdoc>
        internal static MysqlConnectionHolder GetConnection(string connectionString, bool revertImpersonation)
        {
            string strTempConnection = connectionString.ToUpperInvariant();
            //Commented out for source code release.
            //if (strTempConnection.Contains(s_strUpperDataDirWithToken))
            //    EnsureSqlExpressDBFile( connectionString );

            MysqlConnectionHolder holder = new MysqlConnectionHolder(connectionString);
            bool closeConn = true;
            try
            {
                try
                {
                    holder.Open(null, revertImpersonation);
                    closeConn = false;
                }
                finally
                {
                    if (closeConn)
                    {
                        holder.Close();
                        holder = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return holder;
        }

        /// <devdoc>
        /// </devdoc>
        internal static string GetConnectionString(string specifiedConnectionString, bool lookupConnectionString, bool appLevel)
        {
            if (specifiedConnectionString == null || specifiedConnectionString.Length < 1)
                return null;

            string connectionString = null;

            /////////////////////////////////////////
            // Step 1: Check <connectionStrings> config section for this connection string
            if (lookupConnectionString)
            {
                ConnectionStringSettings connObj = ConfigurationManager.ConnectionStrings[specifiedConnectionString];
                if (connObj != null)
                    connectionString = connObj.ConnectionString;

                if (connectionString == null)
                    return null;
            }
            else
            {
                connectionString = specifiedConnectionString;
            }

            return connectionString;
        }
    }

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    internal sealed class MysqlConnectionHolder
    {
        internal OdbcConnection     _Connection;
        private bool               _Opened;

        internal OdbcConnection Connection
        {
            get{ return _Connection; }
        }

        //////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////
        internal MysqlConnectionHolder ( string connectionString )
        {
            try
            {
                _Connection = new OdbcConnection( connectionString );
            }
            catch( ArgumentException e )
            {
                throw new ArgumentException(SR.GetString(SR.SqlError_Connection_String), "connectionString", e);
            }
        }

        //////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////
        internal void Open (HttpContext context, bool revertImpersonate)
        {
            if (_Opened)
                return; // Already opened

            if (revertImpersonate) {
                using (HostingEnvironment.Impersonate()) {
                    Connection.Open();
                }
            }
            else {
                Connection.Open();
            }

            _Opened = true; // Open worked!
        }

        //////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////
        internal void Close ()
        {
            if (!_Opened) // Not open!
                return;
            // Close connection
            Connection.Close ();
            _Opened = false;
        }
    }
}

