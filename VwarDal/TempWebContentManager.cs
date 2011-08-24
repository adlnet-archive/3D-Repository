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
using System.Linq;
using System.Text;

namespace vwarDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class TempWebContentManager : ITempContentManager
    {
        /// <summary>
        /// 
        /// </summary>
        private string connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connString"></param>
        public TempWebContentManager(string connString)
        {
            connectionString = connString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="hash"></param>
        public void EnableTempDatastreams(string pid, string hash)
        {
            //Remove existing references that may occur from the user choosing a new file
            //before the upload to Fedora is finished
            DisableTempDatastreams(pid);
            using (System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(connectionString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "CALL AddToCurrentUploads(?,?)";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("targetpid", pid);
                    command.Parameters.AddWithValue("targethash", hash);
                    //if (command.ExecuteNonQuery() != 1) { throw new Exception("Stored procedure call AddToCurrentUploads failed."); }
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        public void DisableTempDatastreams(string pid)
        {
            using (System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(connectionString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "CALL RemoveFromCurrentUploads(?)";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("targetpid", pid);
                    //if (command.ExecuteNonQuery() != 1) { throw new Exception("Stored procedure call RemoveFromCurrentUploads failed."); }
                    command.ExecuteNonQuery();
                }
                conn.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public string GetTempLocation(string pid)
        {
            string location = "";
            using (System.Data.Odbc.OdbcConnection conn = new System.Data.Odbc.OdbcConnection(connectionString))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = "SELECT hash FROM test.current_uploads WHERE pid = ?";
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@PID", pid);
                    try { location = command.ExecuteScalar().ToString(); }
                    catch { }
                }
                conn.Close();
            }
            return location;
        }
    }
}
