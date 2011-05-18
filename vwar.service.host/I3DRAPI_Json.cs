using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;
using vwar.service.implementation;

namespace vwar.service.host
{
    [ServiceContract]
    public interface I3DRAPI_Json
    {
        [WebGet(UriTemplate = "/Search/{terms}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<vwar.service.implementation.SearchResult> Search(string terms);

        [WebGet(UriTemplate = "/Model/{pid}/{format}/{options}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetModel(string pid, string format, string options);

        [WebGet(UriTemplate = "/{pid}/{format}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetModelSimple(string pid, string format);

        [WebGet(UriTemplate = "/Reviews/{pid}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Review> GetReviews(string pid);

        [WebGet(UriTemplate = "/Screenshot/{pid}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetScreenshot(string pid);

        [WebGet(UriTemplate = "/DeveloperLogo/{pid}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetDeveloperLogo(string pid);

        [WebGet(UriTemplate = "/SponsorLogo/{pid}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Stream GetSponsorLogo(string pid);

        [WebGet(UriTemplate = "/Metadata/{pid}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Metadata GetMetadata(string pid);

    }
}