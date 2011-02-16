using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vwarDAL
{
    public class TempWebContentManager : ITempContentManager
    {
        private string connectionString;

        public TempWebContentManager(string connString)
        {
            connectionString = connString;
        }


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
