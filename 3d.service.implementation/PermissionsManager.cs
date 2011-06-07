using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using User = System.String;
namespace vwar.service.host.permissions
{
    //The permissions users have to alter the group they belong to
    public enum GroupPolicyLevel { AdminOnlyAdd = 0, UsersAdd = 1, UsersAddRemove = 2 }
    //The permissions that a user has on a model
    public enum ModelPermissionLevel { Invisible = 0, Searchable = 1, Fetchable = 2, Editable = 3, Admin = 4 }
    //Return codes
    public enum PermissionErrorCode { AlreadyExists, NotAuthorized, DoesNotExist, OutOfRange, AlreadyIncluded, NotIncluded, Ok }
    //A group of users
    public class UserGroup
    {
        public string GroupName;
        public User Owner;
        public string Description;
        public GroupPolicyLevel PolicyLevel;
        public UserList Users;
        public bool Contains(User user)
        {
            return Users.Contains(user);
        }
    }
    //A list of groups
    public class GroupList : List<UserGroup>
    {
        public bool Contains(User user)
        {
            foreach (UserGroup group in this)
            {
                if (group.Users.Contains(user))
                    return true;
            }
            return false;
        }
        public bool Contains(UserGroup group)
        {
            foreach (UserGroup g in this)
            {
                if(g.GroupName == group.GroupName)
                    return true;
            }
            return false;
        }
    }
    //A list of users
    public class UserList : List<User>
    {
    }
    //A list of models
    public class ModelList : List<string>
    {
    }
    //Manage the permissions for users on models
    class PermissionsManager
    {
        private string ConnectionString;
        private System.Data.Odbc.OdbcConnection mConnection;

        public PermissionsManager()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["postgreSQLConnectionString"].ConnectionString;
            mConnection = new System.Data.Odbc.OdbcConnection(ConnectionString);
            CheckConnection();


            /////////////////////////////////////////////////////////////
            //testing stuff
            PermissionErrorCode code;
            User TestOwner = "permtest@test.com";
            code = CreateGroup("TestGroup", TestOwner, "test", GroupPolicyLevel.UsersAdd);
            //Should succeed, TestOwner ownes TestGroup.
            code = AddUserToGroup(TestOwner, "TestGroup", "psadmin@problemsolutions.net");
            //Should suceed, psadmin ownes adl:65, and is in TestGroup, and TestGroup allows users to add
            code = SetModelToGroupLevel("psadmin@problemsolutions.net", "adl:65", "TestGroup", ModelPermissionLevel.Admin);
            //Should be admin, because psadmin is in testgroup, and testgroup has admin on adl:65
            ModelPermissionLevel level = CheckUserPermissions("psadmin@problemsolutions.net","adl:65");
            code = DeleteGroup(TestOwner, "TestGroup");
            // should be 0, group deleted
            level = CheckUserPermissions("psadmin@problemsolutions.net", "adl:65");





            code = CreateGroup("TestGroup", TestOwner, "test", GroupPolicyLevel.UsersAdd);
            //Should succeed, TestOwner ownes TestGroup.
            code = AddUserToGroup(TestOwner, "TestGroup", "psadmin@problemsolutions.net");
            //Should suceed, psadmin ownes adl:65, and is in TestGroup, and TestGroup allows users to add
            code = SetModelToGroupLevel("psadmin@problemsolutions.net", "adl:65", "TestGroup", ModelPermissionLevel.Admin);
            //Should be admin, because psadmin is in testgroup, and testgroup has admin on adl:65
            level = CheckUserPermissions("psadmin@problemsolutions.net", "adl:65");
            //should work, you can set a model you own
            code = SetModelToGroupLevel("psadmin@problemsolutions.net", "adl:65", "TestGroup", ModelPermissionLevel.Invisible);
            level = CheckUserPermissions("psadmin@problemsolutions.net", "adl:65");

            //Should not work, the admin cannot make it greater than invisible
            code = SetModelToGroupLevel(TestOwner, "adl:65", "TestGroup", ModelPermissionLevel.Editable);
            level = CheckUserPermissions("psadmin@problemsolutions.net", "adl:65");

            //should work, you can set a model you own
            code = SetModelToGroupLevel("psadmin@problemsolutions.net", "adl:65", "TestGroup", ModelPermissionLevel.Editable);
            level = CheckUserPermissions("psadmin@problemsolutions.net", "adl:65");

            //should work, should work, the admin can remove it
            code = SetModelToGroupLevel(TestOwner, "adl:65", "TestGroup", ModelPermissionLevel.Invisible);
            level = CheckUserPermissions("psadmin@problemsolutions.net", "adl:65");

            code = ChangeGroupPolicy(TestOwner, "TestGroup", GroupPolicyLevel.AdminOnlyAdd);
            //should not work, only the group owner may now add models
            code = SetModelToGroupLevel("psadmin@problemsolutions.net", "adl:65", "TestGroup", ModelPermissionLevel.Admin);
            level = CheckUserPermissions("psadmin@problemsolutions.net", "adl:65");

            code = ChangeGroupPolicy(TestOwner, "TestGroup", GroupPolicyLevel.UsersAdd);
            //Should work, now psadmin can add because he is in the group
            code = SetModelToGroupLevel("psadmin@problemsolutions.net", "adl:65", "TestGroup", ModelPermissionLevel.Admin);
            level = CheckUserPermissions("psadmin@problemsolutions.net", "adl:65");

            //Test owner can remove psadmin from testgroup
            code = RemoveUserFromGroup(TestOwner, "TestGroup", "psadmin@problemsolutions.net");
            //Should work, the model owner can remove it even though he is not in the group
            code = SetModelToGroupLevel("psadmin@problemsolutions.net", "adl:65", "TestGroup", ModelPermissionLevel.Invisible);

            //Should not work, psadmin is not in the group, so cant add
            code = SetModelToGroupLevel("psadmin@problemsolutions.net", "adl:65", "TestGroup", ModelPermissionLevel.Searchable);

            code = DeleteGroup(TestOwner, "TestGroup");
            // should be 0, group deleted
            level = CheckUserPermissions("psadmin@problemsolutions.net", "adl:65");





            code = CreateGroup("TestGroup", TestOwner, "test", GroupPolicyLevel.AdminOnlyAdd);
            //Should succeed, TestOwner ownes TestGroup.
            code = AddUserToGroup(TestOwner, "TestGroup", "psadmin@problemsolutions.net");
            //Should fail, psadmin ownes adl:65, and is in TestGroup, but TestGroup does not allow users to add
            code = SetModelToGroupLevel("psadmin@problemsolutions.net", "adl:65", "TestGroup", ModelPermissionLevel.Admin);

            code = DeleteGroup(TestOwner, "TestGroup");





            code = CreateGroup("TestGroup", TestOwner, "test", GroupPolicyLevel.UsersAdd);
            //Should succeed, TestOwner ownes TestGroup.
            code = AddUserToGroup(TestOwner, "TestGroup", "psadmin@problemsolutions.net");
            //Should fail, psadmin is spelled wrong
            code = SetModelToGroupLevel("psadmin@problemsolut ions.net", "adl:65", "TestGroup", ModelPermissionLevel.Admin);

            code = DeleteGroup(TestOwner, "TestGroup");





            //test group create and destroy
            code = CreateGroup("TestGroup", TestOwner, "test", GroupPolicyLevel.UsersAdd);
            //Should succeed
            code = ChangeGroupPolicy(TestOwner, "TestGroup", GroupPolicyLevel.AdminOnlyAdd);
            //Should fail. psadmin is not owner of group
            code = ChangeGroupPolicy("psadmin@problemsolutions.net", "TestGroup", GroupPolicyLevel.AdminOnlyAdd);
            //Should fail. psadmin is not owner of group
            code = DeleteGroup("psadmin@problemsolutions.net", "TestGroup");
            //Should fail. group does not exist
            code = DeleteGroup(TestOwner, "TestGro up");
            //should suceed
            code = DeleteGroup(TestOwner, "TestGroup");



            code = CreateGroup("TestGroup1", TestOwner, "test", GroupPolicyLevel.UsersAdd);
            code = CreateGroup("TestGroup2", TestOwner, "test", GroupPolicyLevel.UsersAdd);
            code = CreateGroup("TestGroup3", TestOwner, "test", GroupPolicyLevel.UsersAdd);

            code = AddUserToGroup(TestOwner, "TestGroup1", "psadmin@problemsolutions.net");
            code = AddUserToGroup(TestOwner, "TestGroup2", "psadmin@problemsolutions.net");
            code = AddUserToGroup(TestOwner, "TestGroup3", "psadmin@problemsolutions.net");

            //should be TestGroup1,TestGroup3,TestGroup2
            GroupList groups = GetUsersGroups("psadmin@problemsolutions.net");
            //should fail noone is neither admin nor user
            code = RemoveUserFromGroup("noone", "TestGroup1", "psadmin@problemsolutions.net");
            //should succeed - group owner can remove people
            code = RemoveUserFromGroup(TestOwner, "TestGroup1", "psadmin@problemsolutions.net");
            //should suceed, you can remove yourself from a group
            code = RemoveUserFromGroup("psadmin@problemsolutions.net", "TestGroup2", "psadmin@problemsolutions.net");
            //should just be just testgroup3
            groups = GetUsersGroups("psadmin@problemsolutions.net");
            //should succeed
            code = DeleteGroup(TestOwner, "TestGroup3");
            //should not contain testgroup3, since it was deleted
            groups = GetUsersGroups("psadmin@problemsolutions.net");
        }
        //Create a group
        public PermissionErrorCode CreateGroup(string Name, User owner, string Description, GroupPolicyLevel level)
        {
            //Must not exist
            if (GroupExists(Name))
                return PermissionErrorCode.AlreadyExists;

            CheckConnection();
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL CreateUserGroup(?,?,?,?)}";
                command.Parameters.AddWithValue("ingroupname", Name);
                command.Parameters.AddWithValue("inowner", owner);
                command.Parameters.AddWithValue("indescription", Description);
                command.Parameters.AddWithValue("inlevel", level);
                command.ExecuteScalar();
            }
            return PermissionErrorCode.Ok;
        }
        //Delete a group
        public PermissionErrorCode DeleteGroup(User userRequestingChange, string groupname)
        {
            //must exist
            if (!GroupExists(groupname))
                return PermissionErrorCode.DoesNotExist;
            //caller must be owner
            if (GetUserGroup(groupname).Owner != userRequestingChange)
                return PermissionErrorCode.NotAuthorized;

            CheckConnection();
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL DeleteUserGroup(?)}";
                command.Parameters.AddWithValue("ingroupname", groupname);
                command.ExecuteScalar();
            }
            return PermissionErrorCode.Ok;
        }
        //Change the policy of the group
        public PermissionErrorCode ChangeGroupPolicy(User userRequestingChange, string groupname, GroupPolicyLevel newlevel)
        {
            //it must exist
            if (!GroupExists(groupname))
                return PermissionErrorCode.DoesNotExist;
            //caller must be the owner
            if (GetUserGroup(groupname).Owner != userRequestingChange)
                return PermissionErrorCode.NotAuthorized;

            CheckConnection();
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL SetGroupPolicy(?,?)}";
                command.Parameters.AddWithValue("ingroupname", groupname);
                command.Parameters.AddWithValue("plevel", newlevel);
                command.ExecuteScalar();
            }
            return PermissionErrorCode.Ok;
        }
        //Add a model to a group
        public PermissionErrorCode SetModelToGroupLevel(User userRequestingChange, string pid, string groupname, ModelPermissionLevel level)
        {
            //The group must exist
            if (!GroupExists(groupname))
                return PermissionErrorCode.DoesNotExist;
            return SetModelToGroupLevel(userRequestingChange, pid, GetUserGroup(groupname), level);
            
        }
        public PermissionErrorCode SetModelToGroupLevel(User userRequestingChange, string pid, UserGroup group, ModelPermissionLevel level)
        {

            //you must be the model owner, or you must be removing the model
            bool modelauth = false;
            if (GetModelOwner(pid) == userRequestingChange || level == ModelPermissionLevel.Invisible)
                modelauth = true;

            //You must be either the group owner, or you must be in the group and the group must allows users to add models
            bool groupauth = false;
            if (group.Owner == userRequestingChange)
                groupauth = true;
            //if your in the group, the groups allows users to add, and your are not setting it to 0 - which is remove
            if (UserIsInGroup(userRequestingChange, group) && group.PolicyLevel == GroupPolicyLevel.UsersAdd && level > ModelPermissionLevel.Invisible)
                groupauth = true;
            //this you're in the group, the group allows members to remove, and you are removing
            if (UserIsInGroup(userRequestingChange, group) && group.PolicyLevel == GroupPolicyLevel.UsersAddRemove && level == ModelPermissionLevel.Invisible)
                groupauth = true;
            //The owner of the model is always allowed to change it from the group, even if he is no longer in the group

            if (GetModelsInGroup(group).Contains(pid) && GetModelOwner(pid) == userRequestingChange)
            {
                groupauth = true;
            }
           

            //You must be authorized on both the model and the group
            if (!(groupauth && modelauth))
                return PermissionErrorCode.NotAuthorized;

            CheckConnection();
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL SetPermission(?,?,?)}";
                command.Parameters.AddWithValue("inpid", pid);
                command.Parameters.AddWithValue("inusername", group.GroupName);
                command.Parameters.AddWithValue("plevel", level);
                command.ExecuteScalar();
            }
            return PermissionErrorCode.Ok;

        }
        //Add a user to a group. You must the the group owner
        public PermissionErrorCode AddUserToGroup(User userRequestingChange, UserGroup group, User user)
        {
            return AddUserToGroup( userRequestingChange,group.GroupName, user);
        }
        //Add a user to a group. You must the the group owner
        public PermissionErrorCode AddUserToGroup(User userRequestingChange, string groupname, User user)
        {
            //The group must exist
            if (!GroupExists(groupname))
                return PermissionErrorCode.DoesNotExist;
            //you must be the group owner
            if (GetUserGroup(groupname).Owner != userRequestingChange)
                return PermissionErrorCode.NotAuthorized;
            //the user must be in the group
            if (GetUsersGroups(user).Contains(GetUserGroup(groupname)))
                return PermissionErrorCode.AlreadyIncluded;

            CheckConnection();
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL AddUserToGroup(?,?)}";
                command.Parameters.AddWithValue("inusername", user);
                command.Parameters.AddWithValue("ingroupname", groupname);
                command.ExecuteScalar();
            }
            return PermissionErrorCode.Ok;
        }
        //Remove a user from a group
        public PermissionErrorCode RemoveUserFromGroup(User userRequestingChange, UserGroup group, User user)
        {
            return RemoveUserFromGroup( userRequestingChange,group.GroupName, user);
        }//Remove a user from a group
        public PermissionErrorCode RemoveUserFromGroup(User userRequestingChange, string groupname, User user)
        {
            //The group must exist
            if (!GroupExists(groupname))
                return PermissionErrorCode.DoesNotExist;
            //The caller must be the group owner, or the user
            if (GetUserGroup(groupname).Owner != userRequestingChange && user != userRequestingChange)
                return PermissionErrorCode.NotAuthorized;
            //the user msut be in the group
            if (!GetUsersGroups(user).Contains(GetUserGroup(groupname)))
                return PermissionErrorCode.NotIncluded;

            CheckConnection();
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
        public UserList GetGroupsUsers(UserGroup Group)
        {
            return GetGroupsUsers(Group.GroupName);
        }
        //Get all the users in a groups
        public UserList GetGroupsUsers(string GroupName)
        {
            CheckConnection();
            UserList Result = new UserList();
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
        //Get all the groups a user belongs to
        public GroupList GetUsersGroups(User user)
        {
            CheckConnection();
            GroupList Result = new GroupList();
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
        //Get the max permission level for this user
        public ModelPermissionLevel CheckUserPermissions(User user, string pid)
        {
            CheckConnection();

            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{SELECT CheckPermission(?,?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("inpid", pid);
                command.Parameters.AddWithValue("inusername", user);

                using (var resultSet = command.ExecuteReader())
                {
                    while (resultSet.Read())
                    { 
                        return (ModelPermissionLevel) System.Convert.ToInt16(resultSet[0].ToString());
                    }
                }
            }
            return 0;
        }
        //Get the permission level for a group on a model
        public ModelPermissionLevel CheckGroupPermissions(UserGroup group, string pid)
        {
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
        public User GetModelOwner(string PID)
        {
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
        public bool UserIsInGroup(User user, UserGroup group)
        {
            if (group.Contains(user))
                return true;
            return false;
        }
        public bool UserIsInGroup(User user, string group)
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
            

            CheckConnection();
            
            using (var command = mConnection.CreateCommand())
            {
                command.CommandText = "{CALL GetUserGroup(?)}";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("ingroupname", GroupName);

                using (var resultSet = command.ExecuteReader())
                {
                    while (resultSet.Read())
                    {
                        UserGroup group = new UserGroup();
                        group.GroupName = resultSet["groupname"].ToString();
                        group.Owner = resultSet["Owner"].ToString();
                        group.Description = resultSet["Description"].ToString();
                        group.PolicyLevel = (GroupPolicyLevel)System.Convert.ToInt16(resultSet["PermissionLevel"].ToString());
                        group.Users = GetGroupsUsers(GroupName);
                        return group;
                    }
                }
            }
            return null;
        }
        //Get all the models that belong to a group - ignores anythihg that is "invisible"
        public ModelList GetModelsInGroup(string groupname)
        {
            return GetModelsInGroup(GetUserGroup(groupname));
        }
        public ModelList GetModelsInGroup(UserGroup group)
        {
            CheckConnection();
            ModelList Result = new ModelList();
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
        //check that a connection can be made to the database
        private bool CheckConnection()
        {
            int sleeptime = 0;
            while ((mConnection.State == System.Data.ConnectionState.Connecting ||
                mConnection.State == System.Data.ConnectionState.Connecting ||
                mConnection.State == System.Data.ConnectionState.Connecting) &&
                sleeptime < 5000
                )
            {
                sleeptime += 100;
                System.Threading.Thread.Sleep(100);
            }
            if (sleeptime > 5000)
                throw new System.Net.WebException("Could not connect to database");

            if (mConnection.State != System.Data.ConnectionState.Open)
                mConnection.Open();

            return true;
        }
    }
}
