using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace vwarDAL
{
    
    
    public enum MessageErrorCode { SenderDoesNotExist, ReceiverDoesNotExist, MustBeOwner, Ok }
    public class Message
    {
        public string FromID;
        public string ToID;
        public string FromName;
        public string ToName;
        public string OwnerID;
        public string Subject;
        public string MessageText;
        public DateTime DateSent;
        public DateTime DateRead;
        public bool Read;
        public string Mailbox;
        public int ID;
        public int ThreadID;

        public Message()
        {
        }
        public Message(string iFromID,string iToID,string iFromName,string iToName,string iOwnerID,string iSubject,string iMessageText,DateTime iDateSent,DateTime iDateRead,bool iRead,string iMailbox,int iID, int iThreadID)
        {
            FromID = iFromID;
            ToID = iToID;
            FromName = iFromName;
            ToName = iToName;
            OwnerID = iOwnerID;
            Subject = iSubject;
            MessageText = iMessageText;
            DateSent = iDateSent;
            DateRead = iDateRead;
            Read = iRead;
            Mailbox = iMailbox;
            ID = iID;
            ThreadID = iThreadID;
        }
    }
    public class MessageList : List<Message>
    {
    }
    //Manage the permissions for users on models
    [Serializable]
    public class MessageManager
    {
        private string ConnectionString;

        private System.Data.Odbc.OdbcConnection mConnection;
        public MessageManager()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["postgreSQLConnectionString"].ConnectionString;
        }
        ~MessageManager()
        {
            KillODBCConnection(mConnection);
        }
        public void Dispose()
        {
            KillODBCConnection(mConnection);
            mConnection = null;
        }
        //check that a connection can be made to the database
        private System.Data.Odbc.OdbcConnection GetConnection()
        {
            if (mConnection == null)
                mConnection = new System.Data.Odbc.OdbcConnection(ConnectionString);
            if (mConnection.State == System.Data.ConnectionState.Closed)
                mConnection.Open();
            return mConnection;
        }
        public static bool KillODBCConnection(System.Data.Odbc.OdbcConnection myConn)
        {
            if (myConn != null)
            {
                if (myConn.State == System.Data.ConnectionState.Closed)
                    return false;

                try
                {
                    string strSQL = "kill connection_id()";
                    System.Data.Odbc.OdbcCommand myCmd = new System.Data.Odbc.OdbcCommand(strSQL, myConn);
                    myCmd.CommandText = strSQL;

                    myCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                }

            }

            return true;
        }
        public MessageErrorCode ReadMessage(int messageID, string UserRequestingChange)
        {
            Message m = GetMessage(messageID);
            if (m.OwnerID != GetUserID(UserRequestingChange))
            {
                return MessageErrorCode.MustBeOwner;
            }

            var mConnection = GetConnection();
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL ReadMessage(?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("messageid", messageID);
                command.ExecuteScalar();
            }
            return MessageErrorCode.Ok;
        }
        //Create a group
        public MessageErrorCode SendMessage(string fromName, string toName, string Subject, string Messagetext, string UserRequestingChange,int thread = -1)
        {
            if (fromName != UserRequestingChange)
                return MessageErrorCode.MustBeOwner;
            if (GetUserID(UserRequestingChange) == null)
                return MessageErrorCode.SenderDoesNotExist;
            if (GetUserID(toName) == null)
                return MessageErrorCode.ReceiverDoesNotExist;

            var mConnection = GetConnection();
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL CreateMessage(?,?,?,?,?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("infromname", fromName);
                command.Parameters.AddWithValue("intoname", toName);
                command.Parameters.AddWithValue("inmessage", Messagetext);
                command.Parameters.AddWithValue("insubject", Subject);
                command.Parameters.AddWithValue("inthreadid", thread);
                command.ExecuteScalar();
            }
            return MessageErrorCode.Ok; ;
        }
        public Message GetMessage(int id)
        {
            var connection = GetConnection();
            Message m = null;
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "{select * from messages where id = "+id.ToString()+";}";
                command.CommandType = System.Data.CommandType.Text;
                
                var results = command.ExecuteReader();
                while (results.Read())
                {

                    m = MessageFromResults(results);
                    
                }

            }
            return m;
        }
        public MessageList GetInbox(string UserRequestingChange)
        {
            if (GetUserID(UserRequestingChange) == null)
                return null;

            MessageList inbox = new MessageList();
            var mConnection = GetConnection();
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL GetInboxMessages(?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("inusername", UserRequestingChange);

                using (var resultSet = command.ExecuteReader())
                {
                    while (resultSet.Read())
                    {
                        inbox.Add(MessageFromResults(resultSet));
                    }
                }

            }
            return inbox;
        }
        public MessageList GetSentbox(string UserRequestingChange)
        {
            if (GetUserID(UserRequestingChange) == null)
                return null;

            MessageList inbox = new MessageList();
            var mConnection = GetConnection();
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL GetSentMessages(?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("inusername", UserRequestingChange);

                using (var resultSet = command.ExecuteReader())
                {
                    while (resultSet.Read())
                    {
                        inbox.Add(MessageFromResults(resultSet));
                    }
                }

            }
            return inbox;
        }
        public MessageList GetUnreadInbox(string UserRequestingChange)
        {
            if (GetUserID(UserRequestingChange) == null)
                return null;

            MessageList inbox = new MessageList();
            var mConnection = GetConnection();
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL GetUnreadMessages(?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("inusername", UserRequestingChange);

                using (var resultSet = command.ExecuteReader())
                {
                    while (resultSet.Read())
                    {
                        inbox.Add(MessageFromResults(resultSet));
                    }
                }

            }
            return inbox;
        }
        private Message MessageFromResults(System.Data.Odbc.OdbcDataReader results)
        {
            Message message = new Message();


            message.FromID = results["FromID"].ToString(); ;
            message.ToID = results["ToID"].ToString(); ;
            message.FromName = results["FromName"].ToString(); ;
            message.ToName = results["ToName"].ToString(); ;
            message.OwnerID = results["ownerid"].ToString(); ;
            message.Subject = results["Subject"].ToString(); ;
            message.MessageText = results["Message"].ToString(); ;
            message.DateSent = DateTime.Parse(results["DateSent"].ToString()) ;
            message.DateRead = DateTime.Parse(results["DateRead"].ToString()); ;
            message.Read = Boolean.Parse(results["viewed"].ToString()); ;
            message.Mailbox = results["mailbox"].ToString(); ;
            message.ID = Int16.Parse(results["ID"].ToString()); ;
            message.ThreadID = Int16.Parse(results["ThreadID"].ToString()); ;
            return message;
        }

        public string GetUserID(string inusername)
        {
            
            var connection = GetConnection();
            string id = null;
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "{select pkid from users where username = "+inusername+";}";
                command.CommandType = System.Data.CommandType.Text;
                
                var results = command.ExecuteReader();
                while (results.Read())
                {

                    id = results["pkid"].ToString();
                    
                }

            }
            return id;
        }

        

    }
}
