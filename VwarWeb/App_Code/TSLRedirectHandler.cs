using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TSLRedirectHandler
/// </summary>
public class TSLRedirectHandler : IHttpModule
{
    public TSLRedirectHandler()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public String ModuleName
    {
        get { return "TSLRedirectHandler"; }
    }

    // In the Init function, register for HttpApplication 
    // events by adding your handlers.
    public void Init(HttpApplication application)
    {

        application.AuthenticateRequest += new EventHandler(application_AuthenticateRequest);
    }

    private void application_AuthenticateRequest(Object source, EventArgs e)
    {

        HttpApplication application = (HttpApplication)source;
        HttpContext context = application.Context;
        string filePath = context.Request.FilePath;
        string fileExtension =
            VirtualPathUtility.GetExtension(filePath);

        if (filePath.Contains("Login") && context.Request.Url.Scheme != "https")
        {
            context.Response.Redirect(context.Request.Url.ToString().Replace("http", "https"), true);
            return;
        }
        if (!filePath.Contains("Login"))
        {
            if (fileExtension.Equals(".aspx") || fileExtension.Equals(".ashx"))
            {
                if (context.User != null && context.User.Identity.IsAuthenticated == true)
                {
                    if (context.Request.Url.Scheme == "http")
                    {
                        context.Response.Redirect(context.Request.Url.ToString().Replace("http", "https"), true);
                        return;
                    }

                }
                else
                {

                    if (context.Request.Url.Scheme == "https")
                    {
                        context.Response.Redirect(context.Request.Url.ToString().Replace("https", "http"), true);
                        return;
                    }

                }
            }

        }
    }

    public void Dispose() { }
}