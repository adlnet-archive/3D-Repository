//  Copyright 2011 U.S. Department of Defense

//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at

//      http://www.apache.org/licenses/LICENSE-2.0

//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Net;
using AjaxControlToolkit;
using vwarDAL;
using System.Web.Script.Serialization;
using LR;
using System.Web.Script.Serialization;
using System.Collections.Generic;
public partial class Public_Model : Website.Pages.PageBase
{
    private const string VIOLATION_REPORT_SUCCESS = "A message has been sent to the site administator concerning this content.";
    private const string VIOLATION_REPORT_UNAUTHENTICATED = "You must be logged into 3DR to report an offensive content/license violation.";
    private const string VIOLATION_REPORT_EMAIL_ERROR = "An error occurred when trying to notify the administrator. Please try again later.";
    public enum FederateState { Active, Offline, Unapproved, Banned, Unknown, Delisted };
    [Serializable]
    public class FederateRecord
    {

        public string RESTAPI;
        public string SOAPAPI;
        public string namespacePrefix;
        public string OrginizationName;
        public string OrganizationURL;
        public string OrganizationPOC;
        public string OrganizationPOCEmail;
        public string OrganizationPOCPassword;
        public FederateState ActivationState;
        public bool AllowFederatedSearch;
        public bool AllowFederatedDownload;
    }
    [Serializable]
    public class FederateRecordSet
    {
        public List<FederateRecord> federates;
    }
    protected string ContentObjectID
    {
        get
        {
            string rv = "";
            if (Request.QueryString["ContentObjectID"] != null)
            {
                rv = Server.UrlDecode(Request.QueryString["ContentObjectID"].Trim());

            }
            else if (ViewState["ContentObjectID"] != null)
            {
                rv = ViewState["ContentObjectID"].ToString();
            }

            return rv;
        }
        set { ViewState["ContentObjectID"] = value; }
    }
    protected void SearchFederatonButton_Click(object sender, EventArgs args)
    {
        HttpContext.Current.Response.Redirect("~/Public/FederationResults.aspx?SearchTerms=" + SearchFederationTextBox.Text);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SearchPanel.Visible = false;
        if (!Page.IsPostBack)
        {
            // CreateViewOptionTabs();
            this.BindModelDetails();
        }
    }
    public FederateRecordSet GetFederateInfo()
    {
        if (ViewState["FederateRecordSet"] == null)
        {
            System.Net.WebClient wc = new WebClient();
            string federatedata = wc.UploadString("http://3dr.adlnet.gov/federation/3DR_Federation_Mgmt.svc/GetAllFederates", "POST", "");
            FederateRecordSet federates = (new JavaScriptSerializer()).Deserialize<FederateRecordSet>(federatedata);
            ViewState["FederateRecordSet"] = federates;
        }
        return ViewState["FederateRecordSet"] as FederateRecordSet;
    }

    public FederateRecord GetFederateInfo(string pid)
    {
        foreach (FederateRecord f in GetFederateInfo().federates)
        {
            if (String.Equals(f.namespacePrefix, pid, StringComparison.CurrentCultureIgnoreCase))
                return f;
        }
        return null;
    }
    public string PidToNamespace(string pid)
    {
        string nameSpace = null;
        pid = HttpUtility.UrlDecode(pid);
        int colon = pid.IndexOfAny(new char[]{':','_'});
        nameSpace = pid.Substring(0, colon);
        return nameSpace;
    }
    protected void Page_PreRender()
    {
       
    }


    protected void AddHeaderTag(string type, string name, string description)
    {
        if (type == "meta")
        {
            System.Web.UI.HtmlControls.HtmlMeta newTag = new HtmlMeta();
            newTag.Name = name;
            newTag.Content = description;
            Page.Header.Controls.Add(newTag);
        }
        else if (type == "link")
        {
            System.Web.UI.HtmlControls.HtmlLink newLink = new HtmlLink();
            newLink.Attributes.Add("rel", name);
            newLink.Attributes.Add("href", description);
            Page.Header.Controls.Add(newLink);
        }
    }

    private void BindModelDetails()
    {
        if (String.IsNullOrEmpty(ContentObjectID))
        {
            Response.Redirect("~/Default.aspx");
        }
       
        var uri = Request.Url;
        //string proxyTemplate = "Model.ashx?pid={0}&file={1}&fileid={2}";
        vwar.service.host.Metadata co = null;
        try
        {
            string request = "http://3dr.adlnet.gov/Federation/3DR_Federation.svc/" + Request.QueryString["ContentObjectID"] + "/Metadata/json?ID=00-00-00";
            FederateRecord fed = GetFederateInfo(PidToNamespace(Request.QueryString["ContentObjectID"]));
            OrganizationContact.Text = fed.OrganizationPOC;
            OrganizationContact.NavigateUrl = "mailto:"+fed.OrganizationPOCEmail;
            OrganizationName.Text = fed.OrginizationName;
            OrganizationName.NavigateUrl = fed.OrganizationURL;
            
            System.Net.WebRequest wr = WebRequest.Create(request);
            wr.Credentials = new System.Net.NetworkCredential("AnonymousUser", "");
            wr.Method = "GET";
            wr.PreAuthenticate = true;

            string data;

            var response = (HttpWebResponse)wr.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                data = sr.ReadToEnd();
            }
            co = (new JavaScriptSerializer()).Deserialize<vwar.service.host.Metadata>(data);
        }
        catch (System.Net.WebException ex)
        {
            System.Net.WebRequest wr = WebRequest.Create(ex.Response.ResponseUri);
            wr.Credentials = new System.Net.NetworkCredential("AnonymousUser", "");
            wr.Method = "GET";
            wr.PreAuthenticate = true;

            string data;

            var response = (HttpWebResponse)wr.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                data = sr.ReadToEnd();
            }
            co = (new JavaScriptSerializer()).Deserialize<vwar.service.host.Metadata>(data);
        }

        LoginToDlLabel.Visible = false;
        

        //model screenshot
        if (co != null)
        {
            
            if ("Model".Equals(co.AssetType, StringComparison.InvariantCultureIgnoreCase) || true)
            {
                //if the content object file is null, dont' try to display
            //    if (co.DisplayFile != string.Empty && co.Location != string.Empty && Permission > ModelPermissionLevel.Searchable)
            //    {
            //        Page.ClientScript.RegisterClientScriptBlock(GetType(), "vload", string.Format("vLoader = new ViewerLoader('{0}', '{1}', '{2}', '{3}', {4});", Page.ResolveClientUrl("~/Public/Serve.ashx?mode=PreviewModel"),
            //                                                                                               (co.UpAxis != null) ? co.UpAxis : "",
            //                                                                                               (co.UnitScale != null) ? co.UnitScale : "", co.NumPolygons, "\"" + co.PID.Replace(':','_') + "\""), true);

//                    BodyTag.Attributes["onunload"] += "vLoader.DestroyViewer();";

  //              }

                ScreenshotImage.ImageUrl = "http://3dr.adlnet.gov/Federation/3DR_Federation.svc/" + Request.QueryString["ContentObjectID"] + "/Screenshot?ID=00-00-00";
                }
                AddHeaderTag("link", "og:image", ScreenshotImage.ImageUrl);
            }
            else if ("Texture".Equals(co.AssetType, StringComparison.InvariantCultureIgnoreCase))
            {
                ScreenshotImage.ImageUrl = "http://3dr.adlnet.gov/Federation/3DR_Federation.svc/" + Request.QueryString["ContentObjectID"] + "/Screenshot?ID=00-00-00";
            }

            
            TitleLabel.Text = co.Title;
            AddHeaderTag("meta", "og:title", co.Title);
            //show hide edit link
           
           
           
                LoginToDlLabel.Visible = false;
              
                vwar.service.host.Review[] reviews = null;
                try
                {
                    String request = "http://3dr.adlnet.gov/Federation/3DR_Federation.svc/" + Request.QueryString["ContentObjectID"] + "/Reviews/json?ID=00-00-00";

                    System.Net.WebRequest wr = WebRequest.Create(request);
                    wr.Credentials = new System.Net.NetworkCredential("AnonymousUser", "");
                    wr.Method = "GET";
                    wr.PreAuthenticate = true;

                    wr.GetResponse().ToString();

                    var response = (HttpWebResponse)wr.GetResponse();
                    string data;
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                       data = sr.ReadToEnd();
                    }

                     reviews = (new JavaScriptSerializer()).Deserialize<vwar.service.host.Review[]>(data);
                }
                catch(System.Net.WebException ex)
                {
                   
                    System.Net.WebRequest wr = WebRequest.Create(ex.Response.ResponseUri);
                    wr.Credentials = new System.Net.NetworkCredential("AnonymousUser", "");
                    wr.Method = "GET";
                    wr.PreAuthenticate = true;

                    wr.GetResponse().ToString();

                    var response = (HttpWebResponse)wr.GetResponse();
                    string data;
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        data = sr.ReadToEnd();
                    }

                    reviews = (new JavaScriptSerializer()).Deserialize<vwar.service.host.Review[]>(data);
                }

            //rating
                
                int rating = 0;
                foreach (var review in reviews)
                {
                    rating += review.Rating;
                }
                if (reviews.Count() > 0)
                {
                    rating = rating / reviews.Count();
                }
               

            ir.CurrentRating = rating;
            

            //description
            DescriptionLabel.Text = String.IsNullOrEmpty(co.Description) ? "No description available." : co.Description;
            AddHeaderTag("meta", "og:description", co.Description);
            upAxis.Value = co.UpAxis;
            unitScale.Value = co.UnitScale;
            //keywords
            var keywordsList = string.IsNullOrEmpty(co.Keywords) ? new String[0] : co.Keywords.Split(new char[] { ',' });
            foreach (var keyword in keywordsList)
            {
                HyperLink link = new HyperLink()
                {
                    Text = keyword,
                    NavigateUrl = "~/Public/FederationResults.aspx?SearchTerms=" + Server.UrlEncode(keyword.Trim()),
                    CssClass = "Hyperlink"
                };
                keywords.Controls.Add(link);
                keywords.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            }
            this.keywordLabel.Visible = !string.IsNullOrEmpty(co.Keywords);

            //more details
            this.MoreDetailsHyperLink.NavigateUrl = co.MoreInformationURL;
            this.MoreDetailsHyperLink.Text = co.MoreInformationURL;
            this.MoreDetailsRow.Visible = !string.IsNullOrEmpty(co.MoreInformationURL);

            string submitterFullName = Website.Common.GetFullUserName(co.DeveloperName);
            if (co.UploadedDate != null)
            {
                UploadedDateLabel.Text = "Uploaded by: " + submitterFullName + " on " + co.UploadedDate.ToString();
            }


            this.SponsorLogoImage.ImageUrl = "http://3dr.adlnet.gov/Federation/3DR_Federation.svc/" + Request.QueryString["ContentObjectID"] + "/SponsorLogo?ID=00-00-00";
                
                this.SponsorLogoRow.Visible = true;
                this.SponsorNameLabel.Text = co.SponsorName;
                this.SponsorNameRow.Visible = !string.IsNullOrEmpty(co.SponsorName);



                this.DeveloperLogoImage.ImageUrl = "http://3dr.adlnet.gov/Federation/3DR_Federation.svc/" + Request.QueryString["ContentObjectID"] + "/DeveloperLogo?ID=00-00-00";
              

                //developer name
                this.DeveloperNameHyperLink.NavigateUrl = "~/Public/FederationResults.aspx?SearchTerms=" + Server.UrlEncode(co.DeveloperName);
                this.DeveloperNameHyperLink.Text = co.DeveloperName;

                if (String.IsNullOrEmpty(co.ArtistName))
                {
                    this.ArtistRow.Visible = false;
                }
                else
                {
                    this.ArtistNameHyperLink.NavigateUrl = "~/Public/FederationResults.aspx?SearchTerms=" + Server.UrlEncode(co.ArtistName);
                    this.ArtistNameHyperLink.Text = co.ArtistName;
                }

            this.DeveloperRow.Visible = !string.IsNullOrEmpty(co.DeveloperName);
          
            this.FormatLabel.Text = "Native format: " + ((string.IsNullOrEmpty(co.Format)) ? "Unknown" : co.Format);

            //num polygons   
            this.NumPolygonsLabel.Text = co.NumPolygons.ToString();
            this.NumPolygonsRow.Visible = !string.IsNullOrEmpty(co.NumPolygons.ToString());

            //num textures
            this.NumTexturesLabel.Text = co.NumTextures.ToString();
            this.NumTexturesRow.Visible = !string.IsNullOrEmpty(co.NumTextures.ToString());

            //cclrow
            this.CCLHyperLink.Visible = !string.IsNullOrEmpty(co.License);
            this.CCLHyperLink.NavigateUrl = co.License;


            if (!string.IsNullOrEmpty(co.License))
            {
                switch (co.License.ToLower().Trim())
                {
                    case "http://creativecommons.org/licenses/by-nc-sa/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/by-nc-sa/3.0/88x31.png";
                        this.CCLHyperLink.ToolTip = "by-nc-sa";
                        break;

                    case "http://creativecommons.org/licenses/by-nc-nd/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/by-nc-nd/3.0/88x31.png";
                        this.CCLHyperLink.ToolTip = "by-nc-nd";
                        break;

                    case "http://creativecommons.org/licenses/by-nc/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/by-nc/3.0/88x31.png";
                        this.CCLHyperLink.ToolTip = "by-nc";
                        break;

                    case "http://creativecommons.org/licenses/by-nd/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/by-nd/3.0/88x31.png";
                        this.CCLHyperLink.ToolTip = "by-nd";
                        break;

                    case "http://creativecommons.org/licenses/by-sa/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/by-sa/3.0/88x31.png";
                        this.CCLHyperLink.ToolTip = "by-sa";
                        break;
                    case "http://creativecommons.org/publicdomain/mark/1.0/":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/publicdomain/88x31.png";
                        this.CCLHyperLink.ToolTip = "Public Domain";
                        break;
                    case "http://creativecommons.org/licenses/by/3.0/legalcode":
                        this.CCLHyperLink.ImageUrl = "http://i.creativecommons.org/l/by/3.0/88x31.png";
                        this.CCLHyperLink.ToolTip = "by";
                        break;
                }

            }

            //downloads
            DownloadsLabel.Text = co.Downloads.ToString();
            this.DownloadsRow.Visible = !string.IsNullOrEmpty(co.Downloads.ToString());

            //views
            ViewsLabel.Text = co.Views.ToString();
            this.ViewsRow.Visible = !string.IsNullOrEmpty(co.Views.ToString());

            //download buton
            //this.DownloadButton.Visible = Context.User.Identity.IsAuthenticated;

            this.CommentsGridView.DataSource = reviews;
            this.CommentsGridView.DataBind();
        
    }


    private const string RATINGKEY = "rating";

    protected void Rating_Set(object sender, RatingEventArgs args)
    {
        ViewState[RATINGKEY] = args.Value;
    }

    protected void ContentObjectFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "DownloadZip":
                
                Label LocationLabel = (Label)FindControl("LocationLabel");
               
                HttpContext.Current.Response.Redirect("http://3dr.adlnet.gov/Federation/3DR_Federation.svc/" + Request.QueryString["ContentObjectID"] + "/OriginalUpload?ID=00-00-00");
                
                break;
        }
    }

    [System.Web.Services.WebMethod()]
    public static void DownloadButton_Click_Impl(string format, string ContentObjectID)
    {
        var factory = new DataAccessFactory();
        IDataRepository vd = factory.CreateDataRepositorProxy();
        var co = vd.GetContentObjectById(ContentObjectID, false);

        if (LR_3DR_Bridge.LR_Integration_Enabled())
            LR_3DR_Bridge.ModelDownloaded(co);

        vd.IncrementDownloads(ContentObjectID);
        try
        {
            if (String.IsNullOrEmpty(format))
            {
                string clientFileName = (!String.IsNullOrEmpty(co.OriginalFileName)) ? co.OriginalFileName : co.Location;
                var data = vd.GetContentFile(co.PID, clientFileName);

                Website.Documents.ServeDocument(data, clientFileName);
            }
            else
            {
                var data = vd.GetContentFile(co.PID, co.Location);
                if (format == ".dae")
                {
                    Website.Documents.ServeDocument(data, co.Location);
                }
                else if (format != ".O3Dtgz")
                {
                    Website.Documents.ServeDocument(data, co.Location, null, format);
                }
                else
                {
                    var displayFile = vd.GetContentFile(co.PID, co.DisplayFile);
                    Website.Documents.ServeDocument(displayFile, co.DisplayFile);
                }


            }
        }
        catch (System.Threading.ThreadAbortException tabexc) { }
        catch (Exception f)
        {
        }
        vd.Dispose();
    }
}
