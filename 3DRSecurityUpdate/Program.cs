using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Simple.Providers.MySQL;
using System.Data.Odbc;
using System.Web.Security;
using System.Security.Cryptography;
using System.Web.Configuration;
using System.Configuration.Provider;

namespace _3DRSecurityUpdate
{
    class Program
    {
        static MysqlMembershipProvider m;
        static List<string[]> allUsers;

        static void Main(string[] args)
        {
            m = new MysqlMembershipProvider();
            m.connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["postgreSQLConnectionString"].ConnectionString;

            allUsers = new List<string []>(700);
            getAllUsers();

            MachineKeySection machineKey = (MachineKeySection)ConfigurationManager.GetSection("system.web/machineKey");

            if (machineKey.ValidationKey.Contains("AutoGenerate"))
                    throw new ProviderException("Hashed or Encrypted passwords " +
                                                "are not supported with auto-generated keys.");

            HMACSHA1 hash = new HMACSHA1();
            hash.Key = HexToByte(machineKey.ValidationKey);

            for (int i = 0; i < allUsers.Count; i++ )
            {
                allUsers[i][1] = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(allUsers[i][1])));
            }

            Console.WriteLine(allUsers.Count + " users found.\n");

            Console.WriteLine("No changes have been made. Ensure that this process completes uninterrupted.\n"+
                               "Press enter to proceed with the update.\n");

            Console.ReadLine();

            updatePasswords();

            Console.ReadKey();
        }

        public static void updatePasswords()
        {
            string tableName = "users";

            OdbcConnection conn = new OdbcConnection(m.connectionString);
            OdbcCommand cmd = new OdbcCommand("UPDATE `" + tableName + "` " +
                                              " SET Password = ? WHERE Email = ? ", conn);
            try
            {
                conn.Open();
                cmd.Parameters.Add("@Password", OdbcType.VarChar, 128);
                cmd.Parameters.Add("@Email", OdbcType.VarChar, 128);
                int percentage = 0;

                for (int i = 0; i < allUsers.Count; i++)
                {

                    cmd.Parameters[0].Value = allUsers[i][1];
                    cmd.Parameters[1].Value = allUsers[i][0];

                    cmd.ExecuteNonQuery();

                    if (i % 10 == 0)
                    {
                        percentage = (i*100)/allUsers.Count;
                        Console.WriteLine(percentage + "% complete.");
                    }
                }

                if(percentage != 100)
                    Console.WriteLine("Done. Press any key to exit.");
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

        public static void getAllUsers()
        {
            string tableName = "users";

            OdbcConnection conn = new OdbcConnection(m.connectionString);
            OdbcCommand cmd = new OdbcCommand("SELECT Email, Password" +
                                              " FROM `" + tableName + "` " +
                                              " ORDER BY Email Asc", conn);
            OdbcDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();
                string[] userInfo = new string[2];

                while (reader.Read())
                {
                    userInfo[0] = reader["Email"].ToString();

                    //This is the only reason we have a dependence on MembershipProvider
                    userInfo[1] = m.UnEncodePassword(reader["Password"].ToString());

                    allUsers.Add((string[])userInfo.Clone());
                }
            }
            catch (OdbcException e)
            {

              throw e;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
        }

        private static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
}
