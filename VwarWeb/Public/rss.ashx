<%@ WebHandler Language="C#" Class="rss" %>
using System.Linq;
using System;
using System.Web;
using System.Collections.Generic;
public class rss : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        var Request = context.Request;
        context.Response.ContentType = "text/xml";
        var xtwFeed = new System.Xml.XmlTextWriter(context.Response.Output);
        xtwFeed.WriteStartDocument();
        // The mandatory rss tag

        xtwFeed.WriteStartElement("rss");

        xtwFeed.WriteAttributeString("version", "2.0");



        // The channel tag contains RSS feed details

        xtwFeed.WriteStartElement("channel");

        xtwFeed.WriteElementString("title", "ADL 3D Repository");

        var homeUrl = VirtualPathUtility.ToAbsolute(Website.Pages.Types.Default);
        var fullHomeUrl = Request.Url.OriginalString.Replace(Request.Url.PathAndQuery, homeUrl);
        xtwFeed.WriteElementString("link", fullHomeUrl);

        xtwFeed.WriteElementString("description", "Knowledge. Experience. Performance. The Power of Insight.");

        xtwFeed.WriteElementString("copyright", "Copyright 2007 Transamerica Occidental Life Insurance Company");

        var fac = new vwarDAL.DataAccessFactory();
        var dal = fac.CreateDataRepositorProxy();
        var contentObjects = dal.GetAllContentObjects() as List<vwarDAL.ContentObject>;


        // Loop through the content of the database and add them to the RSS feed

        foreach (var co in contentObjects.OrderBy((co) => co.UploadedDate).Reverse())
        {
            xtwFeed.WriteStartElement("item");
            xtwFeed.WriteElementString("title", co.Title);
            xtwFeed.WriteElementString("description", co.Description);
            var absoluteURL = VirtualPathUtility.ToAbsolute(Website.Pages.Types.FormatModel(co.PID));
            var url = Request.Url.OriginalString.Replace(Request.Url.PathAndQuery, absoluteURL);
            xtwFeed.WriteElementString("link", url);
            xtwFeed.WriteElementString("pubDate", co.UploadedDate.ToString());
            xtwFeed.WriteEndElement();
        }






        // Close all tags

        xtwFeed.WriteEndElement();

        xtwFeed.WriteEndElement();

        xtwFeed.WriteEndDocument();

        xtwFeed.Flush();

        xtwFeed.Close();

        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}