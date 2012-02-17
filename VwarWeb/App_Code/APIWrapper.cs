using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using vwar.service.host;
/// <summary>
/// Overrides some of the functionality of the API_BASE to make compatible with serving from the web project
/// Speciffically, skips HTTP Basic auth, and assumes that the website has delt with authorization. Takes a
/// username in the constructor, in order to check permissions for later operations
/// </summary>
public class APIWrapper : _3DRAPI_Imp
{
    string username;
    HttpContext context;
    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="inusername">The username of the currently authenticated user</param>
    /// <param name="incontext">The HTTP Context for this operation, used to write headers</param>
	public APIWrapper(string inusername,HttpContext incontext)
	{
        username = inusername;
        context = incontext;
	}
    /// <summary>
    /// Overrides API base class validate, uses website user rather than HTTP Basic
    /// </summary>
    /// <param name="type">The transaction type to validate</param>
    /// <param name="co">the content object to validate the operation on</param>
    /// <returns>True if the user may perform this operation on the contentobject</returns>
    public override bool DoValidate(Security.TransactionType type, vwarDAL.ContentObject co)
    {
       
            vwarDAL.PermissionsManager prm = new vwarDAL.PermissionsManager();
            vwarDAL.ModelPermissionLevel Permission = prm.GetPermissionLevel(username, co.PID);
            prm.Dispose();
            if (type == Security.TransactionType.Query && Permission >= vwarDAL.ModelPermissionLevel.Searchable)
            {
                return true;
            }
            if (type == Security.TransactionType.Access && Permission >= vwarDAL.ModelPermissionLevel.Fetchable)
            {
                return true;
            }
            if (type == Security.TransactionType.Modify && Permission >= vwarDAL.ModelPermissionLevel.Editable)
            {
                return true;
            }
            if (type == Security.TransactionType.Delete && Permission >= vwarDAL.ModelPermissionLevel.Admin)
            {
                return true;
            }
            if (type == Security.TransactionType.Create && Permission >= vwarDAL.ModelPermissionLevel.Admin)
            {
                return true;
            }
        
        return false;

    }
    public override string GetUsername()
    {
        return username;
    }
    /// <summary>
    /// Writes the headers for this transaction. Overrides base, using current context rather than WebOperationContext
    /// NOTE: For some reason this fails under the build in dev server, but runs correctly under IIS. Requires App pool to be 
    /// in integrated mode
    /// </summary>
    /// <param name="type">Content Type</param>
    /// <param name="length">Content Lenght</param>
    /// <param name="disposition">Content Disposition</param>
    public override void SetResponseHeaders(string type, int length, string disposition)
    {
        try
        {
            context.Response.Headers["content-disposition"] = disposition;
            context.Response.ContentType = type;
        }
        catch(Exception e){}
    }
}