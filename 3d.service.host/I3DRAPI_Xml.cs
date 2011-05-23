using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using vwar.service.host;
namespace vwar.service.host
{
    [ServiceContract]
    public interface I3DRAPI_Xml
    {
        [WebGet(UriTemplate = "/Search/{terms}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]

        List<vwar.service.host.SearchResult> Search(string terms);

        [WebGet(UriTemplate = "/{pid}/Model/{format}/{options}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetModel(string pid, string format, string options);

        [WebGet(UriTemplate = "/{pid}/Format/{format}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetModelSimple(string pid, string format);

        [WebGet(UriTemplate = "/{pid}/Reviews", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        List<Review> GetReviews(string pid);

        [WebGet(UriTemplate = "/{pid}/Screenshot", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetScreenshot(string pid);

        [WebGet(UriTemplate = "/{pid}/DeveloperLogo/", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetDeveloperLogo(string pid);

        [WebGet(UriTemplate = "/{pid}/SponsorLogo/", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetSponsorLogo(string pid);

        [WebGet(UriTemplate = "/{pid}/Metadata/", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Metadata GetMetadata(string pid);

        [WebGet(UriTemplate = "/{pid}/SupportingFiles/{filename}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetSupportingFile(string pid,string filename);

        [WebGet(UriTemplate = "/{pid}/Textures/{filename}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetTextureFile(string pid, string filename);

    }
}