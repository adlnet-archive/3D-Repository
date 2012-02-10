using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml;
using System.Net;
using System.Web.Script.Serialization;

enum FederateState { Active, Offline, Unapproved, Banned, Unknown, Delisted };

class FederateRecord
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
class FederateRecordSet
{
    public List<FederateRecord> federates;
}

public class RequestFederationResponse
{
    public int status;
    public string assignedPrefix;
    public string message;
}
public class ModifyFederationResponse
{
    public int status;
    public string assignedPrefix;
    public string message;
}
public class ModifyFederationRequest
{
    public string OrganizationPOCEmail;
    public string OrganizationPOCPassword;
    public string NamespacePrefix;
}

public partial class Administrators_ManageFederation : System.Web.UI.Page
{

    protected void BindDetails()
    {
          APIURL.Text = ConfigurationManager.AppSettings["LR_Integration_APIBaseURL"];
            Namespace.Text = ConfigurationManager.AppSettings["LR_Integration_APIBaseURL"];

            XmlDocument doc = null;

            doc = new XmlDocument();
            doc.Load(ConfigurationManager.AppSettings["APIPath"] + "web.config");
            XmlNode appSettings = doc.SelectSingleNode("//appSettings");
            Namespace.Text = GetAppSetting(appSettings, "fedoraNamespace");

            OrganizationName.Text = ConfigurationManager.AppSettings["CompanyName"];
            OrganizationEmail.Text = ConfigurationManager.AppSettings["SupportEmail"];
            OrganizationURL.Text = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);


            System.Net.WebClient wc = new System.Net.WebClient();
            try
            {
                wc.Credentials = new System.Net.NetworkCredential("AnonymousUser", "");
                string data = wc.DownloadString(APIURL.Text + "/Search/test/json?ID=00-00-00");
                
            }
            catch (Exception ex)
            {
                APIURLstatus.Src = "../styles/images/Icons/xmark.png";
                APIURLHelp.Text = "This API appears to be offline. Visit the <a href='APIControl.aspx'>API page</a> to modify this value.";
                APIStatusIcon.ImageUrl = "../styles/images/big_cancel.png";
                APIStatusIcontext.InnerText = "API Offline";
                EnrollServer.Enabled = false;

            }
            
            EnrollmentStatusIcon.ImageUrl = "../styles/images/big_cancel.png";
            EnrollmentStatusIcontext.InnerText = "Not Enrolled";
            FederationStateIcon.ImageUrl = "../styles/images/big_warning.png";
            FederationStateIcontext.InnerText = "Not Federated";
            DownloadAllowedIcon.ImageUrl = "../styles/images/big_warning.png";
            DownloadAllowedIcontext.InnerText = "Download N/A";
            SearchAllowedIcon.ImageUrl = "../styles/images/big_warning.png";
            SearchAllowedIcontext.InnerText = "Search N/A";
            RequestStatusChange.Enabled = false;
            string federatedata = wc.UploadString("http://3dr.adlnet.gov/federation/3DR_Federation_Mgmt.svc/GetAllFederates", "POST", "");
            FederateRecordSet federates = (new JavaScriptSerializer()).Deserialize<FederateRecordSet>(federatedata);

            Update.Enabled = false;
            Update.Style.Add("color", "gray");
            foreach (FederateRecord fed in federates.federates)
            {
                if (String.Equals(fed.namespacePrefix, Namespace.Text, StringComparison.CurrentCultureIgnoreCase) && fed.ActivationState != FederateState.Delisted)
                {
                    RequestStatusChange.Enabled = true;
                    EnrollServer.Enabled = false;
                    Namespacestatus.Src = "../styles/images/Icons/xmark.png";
                    NamespaceHelp.Text = "This namespace is already in use on the Federation! Visit the <a href='APIControl.aspx'>API page</a> to modify this value.";
                    Enrollment.Enabled = false;
                    Enrollment.Style.Add("color","gray");
                    Update.Style.Remove("color");
                    Update.Enabled = true;
                    EnrollmentStatusIcon.ImageUrl = "../styles/images/big_ok.png";
                    EnrollmentStatusIcontext.InnerText = "Namespace Enrolled";

                    if (fed.AllowFederatedDownload)
                    {
                        DownloadAllowedIcon.ImageUrl = "../styles/images/big_ok.png";
                        DownloadAllowedIcontext.InnerText = "Download Enabled";
                    }
                    else
                    {
                        DownloadAllowedIcon.ImageUrl = "../styles/images/big_cancel.png";
                        DownloadAllowedIcontext.InnerText = "Download Disabled";
                    }

                    if (fed.AllowFederatedSearch)
                    {
                        SearchAllowedIcon.ImageUrl = "../styles/images/big_ok.png";
                        SearchAllowedIcontext.InnerText = "Search Enabled";
                    }
                    else
                    {
                        SearchAllowedIcon.ImageUrl = "../styles/images/big_cancel.png";
                        SearchAllowedIcontext.InnerText = "Search Disabled";
                    }

                    if (fed.ActivationState == FederateState.Active)
                    {
                        FederationStateIcon.ImageUrl = "../styles/images/big_ok.png";
                        FederationStateIcontext.InnerText = "Federation Active";
                    }
                    if (fed.ActivationState == FederateState.Offline)
                    {
                        FederationStateIcon.ImageUrl = "../styles/images/big_cancel.png";
                        FederationStateIcontext.InnerText = "Federation Inactive";
                        SearchAllowedIcon.ImageUrl = "../styles/images/big_info.png";
                        SearchAllowedIcontext.InnerText = "Search Inactive";
                        DownloadAllowedIcon.ImageUrl = "../styles/images/big_info.png";
                        DownloadAllowedIcontext.InnerText = "Download Inactive";
                    }
                    if (fed.ActivationState == FederateState.Unapproved)
                    {
                        FederationStateIcon.ImageUrl = "../styles/images/big_info.png";
                        FederationStateIcontext.InnerText = "Awaiting Federation";
                        SearchAllowedIcon.ImageUrl = "../styles/images/big_info.png";
                        SearchAllowedIcontext.InnerText = "Search Inactive";
                        DownloadAllowedIcon.ImageUrl = "../styles/images/big_info.png";
                        DownloadAllowedIcontext.InnerText = "Download Inactive";
                    }
                    if (fed.ActivationState == FederateState.Banned)
                    {
                        FederationStateIcon.ImageUrl = "../styles/images/big_info.png";
                        FederationStateIcontext.InnerText = "Federation Banned";
                    }
                    if (fed.ActivationState == FederateState.Delisted)
                    {
                        FederationStateIcon.ImageUrl = "../styles/images/big_cancel.png";
                        FederationStateIcontext.InnerText = "Federate Delisted";
                    }
                    
                        
                    
                }
                if (fed.RESTAPI == APIURL.Text && fed.ActivationState != FederateState.Delisted)
                {
                    APIURLstatus.Src = "../styles/images/Icons/xmark.png";
                    APIURLHelp.Text = "This API is already registered in the federation. Visit the <a href='APIControl.aspx'>API page</a> to modify this value.";
                    EnrollServer.Enabled = false;
                    Enrollment.Enabled = false;
                }
                if (String.Equals(fed.namespacePrefix, Namespace.Text, StringComparison.CurrentCultureIgnoreCase) && fed.ActivationState == FederateState.Delisted)
                {
                    EnrollmentStatusIcon.ImageUrl = "../styles/images/big_warning.png";
                    EnrollmentStatusIcontext.InnerText = "Namespace taken";
                }
            }
            AllowDownload.Checked = true;
            AllowSearch.Checked = true;
        
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDetails();
        }
    }
    protected string GetAppSetting(XmlNode appSettings, string name)
    {
        foreach (XmlNode c in appSettings.ChildNodes)
        {
            if (c.Attributes["key"].Value == name)
                return c.Attributes["value"].Value;
        }
        return "";
    }
    protected void EnrollServer_Click(object sender, EventArgs e)
    {
        if (FederationPassword1.Text != FederationPassword2.Text)
        {
            RequestFederationStatus.Text = "Passwords do not match!";
            return;
        }
       FederateRecord newFederate = new FederateRecord();
       newFederate.ActivationState = FederateState.Active;
       newFederate.AllowFederatedDownload = AllowDownload.Checked;
       newFederate.AllowFederatedSearch = AllowSearch.Checked;
       newFederate.namespacePrefix = Namespace.Text;
       newFederate.OrganizationPOC = POCName.Text;
       newFederate.OrganizationPOCEmail = OrganizationEmail.Text;
       newFederate.OrganizationPOCPassword = FederationPassword1.Text;
       newFederate.OrganizationURL = OrganizationURL.Text;
       newFederate.OrginizationName = OrganizationName.Text;
       newFederate.RESTAPI = APIURL.Text;
       newFederate.SOAPAPI = APIURL.Text;


       

       System.Net.WebClient wc = new WebClient();
       string request = (new JavaScriptSerializer()).Serialize(newFederate);
    //   request = "{\"ActivationState\":0,\"AllowFederatedDownload\":true,\"AllowFederatedSearch\":true,\"RESTAPI\":\"http://localhost/3DRAPI/_3DRAPI.svc\",\"OrganizationPOC\":\"Admin\",\"OrganizationPOCEmail\":\"admin@somecompany.com\",\"OrganizationPOCPassword\":\"password\",\"OrganizationURL\":\"http://www.somecompany.com\",\"OrginizationName\":\"Some Company\",\"SOAPAPI\":\"\",\"namespacePrefix\":\"adl\"}";
       wc.Headers["Content-Type"] = "application/json; charset=utf-8";
       string response = wc.UploadString("http://3dr.adlnet.gov/federation/3DR_Federation_Mgmt.svc/RequestFederation", "POST", request);
       RequestFederationResponse serverresponse = (new JavaScriptSerializer()).Deserialize<RequestFederationResponse>(response);

       BindDetails();
        
        RequestFederationStatus.Text = serverresponse.message;
    }

    protected void RequestStatusChange_Click(object sender, EventArgs e)
    {
        if (UpdateFederationPassword1.Text != UpdateFederationPassword2.Text)
        {
            UpdateFederationStatus.Text = "Passwords do not match!";
            return;
        }

        ModifyFederationRequest newFederate = new ModifyFederationRequest();
        newFederate.NamespacePrefix = Namespace.Text;
        newFederate.OrganizationPOCEmail = OrganizationEmail.Text;
        newFederate.OrganizationPOCPassword = UpdateFederationPassword1.Text;
        
      
       System.Net.WebClient wc = new WebClient();
       string request = (new JavaScriptSerializer()).Serialize(newFederate);
    
       wc.Headers["Content-Type"] = "application/json; charset=utf-8";
       string response = "";
       if(FederateStateRequest.Text == "Offline")
            response = wc.UploadString("http://3dr.adlnet.gov/federation/3DR_Federation_Mgmt.svc/ModifyFederate/1", "POST", request);

       if (FederateStateRequest.Text == "Online")
           response = wc.UploadString("http://3dr.adlnet.gov/federation/3DR_Federation_Mgmt.svc/ModifyFederate/0", "POST", request);

       if (FederateStateRequest.Text == "Remove From Federation")
           response = wc.UploadString("http://3dr.adlnet.gov/federation/3DR_Federation_Mgmt.svc/ModifyFederate/5", "POST", request);

       RequestFederationResponse serverresponse = (new JavaScriptSerializer()).Deserialize<RequestFederationResponse>(response);

       BindDetails();
        
        UpdateFederationStatus.Text = serverresponse.message;
    }

    
}