using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using System.Xml;

namespace vwar.service.host
{
    [ServiceContract]
    public interface I3DRAPI
    {
        [WebGet(UriTemplate = "/Search/{terms}/json?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<SearchResult> Search(string terms, string key);

        [WebGet(UriTemplate = "/Search/{terms}/xml?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]

        List<SearchResult> Search2(string terms, string key);

        [WebGet(UriTemplate = "/{pid}/Model/{format}/{options}?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetModel(string pid, string format, string options, string key);

        [WebGet(UriTemplate = "/{pid}/Format/{format}?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetModelSimple(string pid, string format, string key);

        [WebGet(UriTemplate = "/{pid}/Reviews/json?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Review> GetReviews(string pid, string key);


        [WebGet(UriTemplate = "/{pid}/Reviews/xml?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        List<Review> GetReviews2(string pid, string key);

        [WebGet(UriTemplate = "/{pid}/Screenshot?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetScreenshot(string pid, string key);

        [WebGet(UriTemplate = "/{pid}/DeveloperLogo?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetDeveloperLogo(string pid, string key);

        [WebGet(UriTemplate = "/{pid}/SponsorLogo?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetSponsorLogo(string pid, string key);

        [WebGet(UriTemplate = "/{pid}/Metadata/json?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Metadata GetMetadata(string pid, string key);

        [WebGet(UriTemplate = "/{pid}/Metadata/xml?ID={key}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Metadata GetMetadata2(string pid, string key);

        [WebGet(UriTemplate = "/{pid}/SupportingFiles/{filename}?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetSupportingFile(string pid, string filename, string key);

        [WebGet(UriTemplate = "/{pid}/Textures/{filename}?ID={key}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetTextureFile(string pid, string filename, string key);
    }
}
