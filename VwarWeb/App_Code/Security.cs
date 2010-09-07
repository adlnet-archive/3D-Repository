using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Website
{

    /// <summary>
    /// Summary description for Security
    /// </summary>
    /// 
    public class Security
    {


        public static bool IsAdministrator()
        {
            return HttpContext.Current.User.IsInRole("Administrators");
        }
        public static void CreateRolesAndAdministrativeUser()
        {
            if (Website.Config.GenerateDefaultAdministratorOnApplicationStartup)
            {
                try
                {
                    var userName = "psAdmin@problemsolutions.net";
                    if (!Roles.RoleExists("Administrators"))
                    {
                        Roles.CreateRole("Administrators");
                    }
                    if (!Roles.RoleExists("Users"))
                    {
                        Roles.CreateRole("Users");
                    }
                    if (Membership.FindUsersByName(userName).Count == 0)
                    {
                        Membership.CreateUser(userName, "password", userName);
                    }

                    if (!Roles.IsUserInRole(userName, "Administrators"))
                    {
                        Roles.AddUserToRole(userName, "Administrators");
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Unable to connect to the membership database. Please contact support");
                }
            }  
        }
    }
}
