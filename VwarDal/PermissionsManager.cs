using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace vwarDAL
{
    //The permissions users have to alter the group they belong to
    public enum GroupPolicyLevel { AdminOnlyAdd = 0, UsersAdd = 1, UsersAddRemove = 2 }
    //The permissions that a user has on a model
    public enum ModelPermissionLevel { NotSet = -1, Invisible = 0, Searchable = 1, Fetchable = 2, Editable = 3, Admin = 4 }
    //Return codes
    public enum PermissionErrorCode { AlreadyExists, NotAuthorized, DoesNotExist, OutOfRange, AlreadyIncluded, NotIncluded, Ok }
    //A group of users
    public class UserGroup : IComparable<UserGroup>, IComparable
    {
        public string GroupName { get; set; }
        public String Owner { get; set; }
        public string Description { get; set; }
        public GroupPolicyLevel PolicyLevel { get; set; }
        public IList<String> Users { get; set; }
        public bool Contains(String user)
        {
            return Users.Contains(user);
        }

        public int CompareTo(UserGroup other)
        {
            return this.GroupName.CompareTo(other.GroupName);
        }

        public int CompareTo(object obj)
        {
            UserGroup other = (UserGroup)obj;
            return this.CompareTo(other);
        }
    }
    public class PermissionDescription
    {
        public string Name { get; set; }
        public ModelPermissionLevel PermissionLevel { get; set; }
    }

    //A list of groups
    public class GroupList : List<UserGroup>
    {
        public bool Contains(String user)
        {
            foreach (UserGroup group in this)
            {
                if (group.Users.Contains(user))
                    return true;
            }
            return false;
        }
    }
    //Manage the permissions for users on models
    public class PermissionsManager
    {
        private string ConnectionString;

        public PermissionsManager()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["postgreSQLConnectionString"].ConnectionString;
        }
        //Create a group
        public PermissionErrorCode CreateGroup(string Name, string owner, string Description, GroupPolicyLevel level)
        {

            try
            {
                using (var mConnection = GetConnection())
                using (var command = mConnection.CreateCommand())
                {
                    command.CommandText = "{CALL CreateUserGroup(?,?,?,?)}";
                    command.Parameters.AddWithValue("ingroupname", Name);
                    command.Parameters.AddWithValue("inowner", owner);
                    command.Parameters.AddWithValue("indescription", Description);
                    command.Parameters.AddWithValue("inlevel", level);
                    command.ExecuteScalar();
                }
                AddUserToGroup(owner, Name, owner);
            }
            catch (System.Data.Odbc.OdbcException ex)
            {
                if (ex.Message.Contains("Duplicate entry"))
                {
                    return PermissionErrorCode.AlreadyExists;
                }
                throw;
            }
            return PermissionErrorCode.Ok;
        }
        //Delete a group
        public PermissionErrorCode DeleteGroup(string userRequestingChange, string groupname)
        {
            //must exist
            if (!GroupExists(groupname))
                return PermissionErrorCode.DoesNotExist;
            //caller must be owner
            if (!GetUserGroup(groupname).Owner.Equals(userRequestingChange,StringComparison.CurrentCultureIgnoreCase))
                return PermissionErrorCode.NotAuthorized;

            using (var mConnection = GetConnection())
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL DeleteUserGroup(?)}";
                command.Parameters.AddWithValue("ingroupname", groupname);
                command.ExecuteScalar();
            }
            return PermissionErrorCode.Ok;
        }
        //Change the policy of the group
        public PermissionErrorCode ChangeGroupPolicy(string userRequestingChange, string groupname, GroupPolicyLevel newlevel)
        {
            //it must exist
            if (!GroupExists(groupname))
                return PermissionErrorCode.DoesNotExist;
            //caller must be the owner
            if (!GetUserGroup(groupname).Owner.Equals(userRequestingChange, StringComparison.CurrentCultureIgnoreCase))
                return PermissionErrorCode.NotAuthorized;

            using (var mConnection = GetConnection())
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL SetGroupPolicy(?,?)}";
                command.Parameters.AddWithValue("ingroupname", groupname);
                command.Parameters.AddWithValue("plevel", newlevel);
                command.ExecuteScalar();
            }
            return PermissionErrorCode.Ok;
        }
        public PermissionErrorCode SetModelToUserLevel(string userRequestingChange, string userName, string pid, ModelPermissionLevel level)
        {
            //you must be the model owner, or you must be removing the model
            bool modelauth = false;
            if (GetPermissionLevel(userRequestingChange,pid) >= ModelPermissionLevel.Admin || level == ModelPermissionLevel.Invisible)
                modelauth = true;
            //You must be authorized on both the model and the group
            if (!modelauth)
                return PermissionErrorCode.NotAuthorized;
            using(var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "{CALL SetUserPermission(?,?,?);}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("username", userName);
                command.Parameters.AddWithValue("pid", pid);
                command.Parameters.AddWithValue("plevel", level);

                command.ExecuteScalar();
            }
            return PermissionErrorCode.Ok;
        }
        //Add a model to a group
        public PermissionErrorCode SetModelToGroupLevel(string userRequestingChange, string pid, string groupname, ModelPermissionLevel level)
        {
            //The group must exist
            if (!GroupExists(groupname))
                return PermissionErrorCode.DoesNotExist;
            return SetModelToGroupLevel(userRequestingChange, pid, GetUserGroup(groupname), level);

        }
        public PermissionErrorCode SetModelToGroupLevel(string userRequestingChange, string pid, UserGroup group, ModelPermissionLevel level)
        {

            //you must be the model owner, or you must be removing the model
            bool modelauth = false;
            if (GetModelOwner(pid).Equals(userRequestingChange,StringComparison.CurrentCultureIgnoreCase) || level == ModelPermissionLevel.Invisible)
                modelauth = true;

            //You must be either the group owner, or you must be in the group and the group must allows users to add models
            bool groupauth = false;
            if (group.Owner.Equals(userRequestingChange,StringComparison.CurrentCultureIgnoreCase))
                groupauth = true;
            //if your in the group, the groups allows users to add, and your are not setting it to 0 - which is remove
            if (UserIsInGroup(userRequestingChange, group) && group.PolicyLevel == GroupPolicyLevel.UsersAdd && level > ModelPermissionLevel.Invisible)
                groupauth = true;
            //this you're in the group, the group allows members to remove, and you are removing
            if (UserIsInGroup(userRequestingChange, group) && group.PolicyLevel == GroupPolicyLevel.UsersAddRemove)
                groupauth = true;
            //The owner of the model is always allowed to change it from the group, even if he is no longer in the group

            if (GetModelsInGroup(group).Contains(pid) && GetModelOwner(pid).Equals(userRequestingChange,StringComparison.CurrentCultureIgnoreCase))
            {
                groupauth = true;
            }


            //You must be authorized on both the model and the group
            if (!(groupauth && modelauth))
                return PermissionErrorCode.NotAuthorized;

            using (var mConnection = GetConnection())
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL SetPermission(?,?,?)}";
                command.Parameters.AddWithValue("inpid", pid);
                command.Parameters.AddWithValue("ingroupname", group.GroupName);
                command.Parameters.AddWithValue("plevel", level);
                command.ExecuteScalar();
            }
            return PermissionErrorCode.Ok;

        }
        //Add a user to a group. You must the the group owner
        public PermissionErrorCode AddUserToGroup(string userRequestingChange, UserGroup group, string user)
        {
            return AddUserToGroup(userRequestingChange, group.GroupName, user);
        }
        //Add a user to a group. You must the the group owner
        public PermissionErrorCode AddUserToGroup(string userRequestingChange, string groupname, string user)
        {
            //you must be the group owner
            if (!GetUserGroup(groupname).Owner.Equals(userRequestingChange,StringComparison.CurrentCultureIgnoreCase))
                return PermissionErrorCode.NotAuthorized;
            //the user must be in the group

            using (var mConnection = GetConnection())
            using (var command = mConnection.CreateCommand())
            {
                try
                {
                    command.CommandText = "{CALL AddUserToGroup(?,?)}";
                    command.Parameters.AddWithValue("inusername", user);
                    command.Parameters.AddWithValue("ingroupname", groupname);
                    var result = command.ExecuteScalar();
                }
                catch (System.Data.Odbc.OdbcException ex)
                {
                    if (ex.Message.Contains("Duplicate entry"))
                    {
                        return PermissionErrorCode.AlreadyIncluded;
                    }
                    else if (ex.Message.Contains("foreign key constraint fails"))
                    {
                        return PermissionErrorCode.DoesNotExist;
                    }
                    throw;
                }

            }
            return PermissionErrorCode.Ok;
        }
        public PermissionErrorCode RemoveUserPermission(string userRequestingChange,string pid, string userName)
        {
            if (GetPermissionLevel(userRequestingChange, pid) < ModelPermissionLevel.Admin)
            {
                return PermissionErrorCode.NotAuthorized;
            }
            using(var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "{CALL RemoveUserPermission(?,?);}";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("pid", pid);
                command.Parameters.AddWithValue("username", userName);

                command.ExecuteNonQuery();
            }
            return PermissionErrorCode.Ok;
        }
        public PermissionErrorCode RemoveGroupPermission(string userRequestingChange, string pid, string groupName)
        {

            if (GetPermissionLevel(userRequestingChange,pid) < ModelPermissionLevel.Admin)
            {
                return PermissionErrorCode.NotAuthorized;
            }
            using (var connection = GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "{CALL RemoveGroupPermission(?,?);}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("ingroupName", groupName);
                command.Parameters.AddWithValue("inpid", pid);
                

                command.ExecuteNonQuery();
            }
            return PermissionErrorCode.Ok;
        }
        //Remove a user from a group
        public PermissionErrorCode RemoveUserFromGroup(string userRequestingChange, UserGroup group, string user)
        {
            return RemoveUserFromGroup(userRequestingChange, group.GroupName, user);
        }//Remove a user from a group
        public PermissionErrorCode RemoveUserFromGroup(string userRequestingChange, string groupname, string user)
        {
            //The caller must be the group owner, or the user
            if (!GetUserGroup(groupname).Owner.Equals(userRequestingChange,StringComparison.CurrentCultureIgnoreCase) && !user.Equals(userRequestingChange,StringComparison.CurrentCultureIgnoreCase))
                return PermissionErrorCode.NotAuthorized;

            using (var mConnection = GetConnection())
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL RemoveUserFromGroup(?,?)}";
                command.Parameters.AddWithValue("inusername", user);
                command.Parameters.AddWithValue("ingroupname", groupname);
                command.ExecuteScalar();
            }
            return PermissionErrorCode.Ok;
        }
        //Get all the users in a groups
        public IList<string> GetGroupsUsers(UserGroup Group)
        {
            return GetGroupsUsers(Group.GroupName);
        }
        //Get all the users in a groups
        public IList<string> GetGroupsUsers(string GroupName)
        {

            IList<string> Result = new List<string>();
            using (var mConnection = GetConnection())
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL GetGroupMembers(?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("ingroupname", GroupName);

                using (var resultSet = command.ExecuteReader())
                {
                    while (resultSet.Read())
                    {
                        Result.Add(resultSet["username"].ToString());
                    }
                }
            }
            return Result;
        }
        public GroupList GetGroupsByOwner(string owner)
        {
            GroupList results = new GroupList();
            using (var mConnection = GetConnection())
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL GetGroupsByOwner(?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("inOwner", owner);

                using (var resultSet = command.ExecuteReader())
                {
                    while (resultSet.Read())
                    {
                        results.Add(PopulateUserGroupFromReader(resultSet));
                    }
                }
            }
            return results;
        }
        //Get all the groups a user belongs to
        public GroupList GetUsersGroups(string user)
        {
            GroupList Result = new GroupList();
            using (var mConnection = GetConnection())
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL getUserMembership(?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("inusername", user);

                using (var resultSet = command.ExecuteReader())
                {
                    while (resultSet.Read())
                    {
                        Result.Add(GetUserGroup(resultSet["groupname"].ToString()));
                    }
                }
            }
            return Result;
        }
        public ModelPermissionLevel GetPermissionLevel(string user, string pid)
        {
            if (GetModelOwner(pid).Equals(user,StringComparison.CurrentCultureIgnoreCase))
                return ModelPermissionLevel.Admin;

            //The highest level from all groups
            ModelPermissionLevel UserPermissionsFromGroups = 0;
            List<UserGroup> GroupsContainingThisUser = GetUsersGroups(user);
            foreach (UserGroup g in GroupsContainingThisUser)
            {
                
                ModelPermissionLevel thisgroup =  (CheckGroupPermissions(g, pid));
                if (thisgroup > UserPermissionsFromGroups)
                    UserPermissionsFromGroups = thisgroup;
            }

            ModelPermissionLevel SpecificForThisUser = CheckUserPermissions(user, pid);

            if (SpecificForThisUser != ModelPermissionLevel.NotSet)
                return SpecificForThisUser;

            return UserPermissionsFromGroups;
        }
        //Get the max permission level for this user
        public ModelPermissionLevel CheckUserPermissions(string user, string pid)
        {
            using (var mConnection = GetConnection())
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL CheckPermission(?,?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("inpid", pid);
                command.Parameters.AddWithValue("inusername", user);

                using (var resultSet = command.ExecuteReader())
                {
                    while (resultSet.Read())
                    {
                        return (ModelPermissionLevel)System.Convert.ToInt16(resultSet[0].ToString());
                    }
                }
            }
            return (ModelPermissionLevel) (-1);
        }
        //Get the permission level for a group on a model
        public ModelPermissionLevel CheckGroupPermissions(UserGroup group, string pid)
        {
            using (var mConnection = GetConnection())
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL CheckGroupPermission(?,?) }";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("ingroupname", group.GroupName);
                command.Parameters.AddWithValue("inpid", pid);
                using (var resultSet = command.ExecuteReader())
                {
                    while (resultSet.Read())
                    {
                        return (ModelPermissionLevel)System.Convert.ToInt16(resultSet["PermissionLevel"].ToString());
                    }
                }
            }
            return 0;
        }
        //Get the owner of a model
        public string GetModelOwner(string PID)
        {
            using (var mConnection = GetConnection())
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL GetModelOwner(?) }";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("inpid", PID);
                using (var resultSet = command.ExecuteReader())
                {
                    while (resultSet.Read())
                    {
                        return resultSet["submitter"].ToString();
                    }
                }
            }
            return "";
        }
        //Is a user ins a group?
        public bool UserIsInGroup(string user, UserGroup group)
        {
            if (group.Contains(user))
                return true;
            return false;
        }
        public bool UserIsInGroup(string user, string group)
        {
            return UserIsInGroup(user, GetUserGroup(group));
        }
        //Does a group exist?
        bool GroupExists(String GroupName)
        {
            if (GetUserGroup(GroupName) == null)
                return false;
            return true;
        }
        //Get a usergroup by name
        public UserGroup GetUserGroup(string GroupName)
        {


            using (var mConnection = GetConnection())
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL GetUserGroup(?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("ingroupname", GroupName);

                using (var resultSet = command.ExecuteReader())
                {
                    while (resultSet.Read())
                    {
                        UserGroup group = PopulateUserGroupFromReader(resultSet);
                        return group;
                    }
                }
            }
            return null;
        }
        
        private UserGroup PopulateUserGroupFromReader(System.Data.Odbc.OdbcDataReader resultSet)
        {
            UserGroup group = new UserGroup();
            group.GroupName = resultSet["groupname"].ToString();
            group.Owner = resultSet["Owner"].ToString();
            group.Description = resultSet["Description"].ToString();
            group.PolicyLevel = (GroupPolicyLevel)System.Convert.ToInt16(resultSet["PermissionLevel"].ToString());
            group.Users = GetGroupsUsers(group.GroupName);
            return group;
        }
        //Get all the models that belong to a group - ignores anythihg that is "invisible"
        public IList<PermissionDescription> GetGroupsByPid(string pid)
        {
            List<PermissionDescription> groups = new List<PermissionDescription>();
            using(var con = GetConnection())
            using (var com = con.CreateCommand())
            {
                com.CommandText="{CALL  GetGroupsByPid(?);}";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("inpid", pid);
                var reader = com.ExecuteReader();

                while (reader.Read())
                {
                    groups.Add(new PermissionDescription()
                    {
                        Name = reader["groupname"].ToString(),
                        PermissionLevel = (ModelPermissionLevel)Enum.Parse(typeof(ModelPermissionLevel), reader["permissionlevel"].ToString())
                    });
                }
            }
            return groups;
        }
        public IList<string> GetModelsInGroup(string groupname)
        {
            return GetModelsInGroup(GetUserGroup(groupname));
        }
        public IList<string> GetModelsInGroup(UserGroup group)
        {
            List<string> Result = new List<string>();
            using (var mConnection = GetConnection())
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL GetModelsInGroup(?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("inusername", group.GroupName);

                using (var resultSet = command.ExecuteReader())
                {
                    while (resultSet.Read())
                    {
                        Result.Add(resultSet["pid"].ToString());
                    }
                }
            }
            return Result;
        }
        public IList<PermissionDescription> GetUsersWithModelPermission(string pid)
        {
            List<PermissionDescription> users = new List<PermissionDescription>();
            using (var connection = GetConnection())
            using(var command = connection.CreateCommand())
            {
                command.CommandText = "{CALL GetUserWithModelPermission(?);}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("pid", pid);
                var results = command.ExecuteReader();
                while (results.Read())
                {
                    users.Add(new PermissionDescription()
                    { 
                        Name = results["username"].ToString(),
                        PermissionLevel = (ModelPermissionLevel)Enum.Parse(typeof(ModelPermissionLevel),results["permission"].ToString())
                    });
                }
            }
            return users;
        }
        //check that a connection can be made to the database
        private System.Data.Odbc.OdbcConnection GetConnection()
        {
            var mConnection = new System.Data.Odbc.OdbcConnection(ConnectionString);
            mConnection.Open();
            return mConnection;
        }
    }
}
