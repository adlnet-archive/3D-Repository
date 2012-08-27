<%@ Application Language="C#" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Web.Configuration" %>
<script runat="server">
    
    
    void Application_Start(object sender, EventArgs e) 
    {
        Website.Security.CreateRolesAndAdministrativeUser();
        
       
            
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
       
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
        
        //If there is a database error, show the recovery page.
        Exception ex1 = Server.GetLastError();
        System.Data.Odbc.OdbcException ex =  ex1.InnerException as System.Data.Odbc.OdbcException;
        if (ex != null)
        {
            
                Server.Transfer("~/Public/RecoverDatabaseConnection.aspx");
           
        }

    }

    
    void Session_Start(object sender, EventArgs e) 
    {
     
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
