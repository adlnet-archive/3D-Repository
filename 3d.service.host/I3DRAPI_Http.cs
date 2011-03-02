using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;

namespace _3d.service.host
{
    [ServiceContract]
    public interface I3DRAPI_Http
    {
        [WebGet(UriTemplate = "/GetModel?PID={pid}&Format={format}&Options={options}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetModel(string pid, string format, string options);

        [WebGet(UriTemplate = "/{pid}/{format}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetModelSimple(string pid, string format);

        [WebGet(UriTemplate = "/GetReviews?PID={pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        List<Review> GetReviews(string pid);

        [WebInvoke(UriTemplate = "/AddReview?PID={pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string AddReview(Stream data, string pid);

        [WebGet(UriTemplate = "/GetScreenshot?PID={pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetScreenshot(string pid);

        [WebGet(UriTemplate = "/GetDeveloperLogo?PID={pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetDeveloperLogo(string pid);

        [WebGet(UriTemplate = "/GetSponsorLogo?PID={pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetSponsorLogo(string pid);

        [WebGet(UriTemplate = "/Search?SearchTerms={terms}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        List<SearchResult> Search(string terms);

        [WebGet(UriTemplate = "/GetMetadata?PID={pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Metadata GetMetadata(string pid);

        [WebInvoke(UriTemplate = "/UploadModel?PID={pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string UploadFile(Stream indata, string pid);

        [WebInvoke(UriTemplate = "/UploadScreenshot?PID={pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string UploadScreenShot(Stream indata, string pid);

        [WebInvoke(UriTemplate = "/UploadDeveloperLogo?PID={pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string UploadDeveloperLogo(Stream indata, string pid);

        [WebInvoke(UriTemplate = "/UploadSponsorLogo?PID={pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string UploadSponsorLogo(Stream indata, string pid);

        [WebInvoke(UriTemplate = "/UpdateMetadata?PID={pid}", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string UpdateMetadata(Stream m, string pid);

        [WebInvoke(UriTemplate = "/UploadSupportingFile?PID={pid}&Description={description}", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string UploadSupportingFile(Stream m, string pid, string description);

        [WebGet(UriTemplate = "/GetSupportingFile?PID={pid}&Filename={filename}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetSupportingFile(string pid, string filename);

        [WebInvoke(UriTemplate = "/DeleteSupportingFile?PID={pid}&Filename={filename}", RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        bool DeleteSupportingFile(string pid, string filename);

        [WebInvoke(UriTemplate = "/DeleteObject?PID={pid}", RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string DeleteObject(string pid);

        [WebInvoke(UriTemplate = "/UploadMissingTexture?PID={pid}", Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Xml)]
        [OperationContract]
        string UploadMissingTexture(Stream m, string pid);
    }
}