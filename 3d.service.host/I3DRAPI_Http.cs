using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;

namespace vwar.service.host
{
    [ServiceContract]
    public interface I3DRAPI_Http
    {
        [WebGet(UriTemplate = "/Search/{terms}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        List<SearchResult> Search(string terms);

        [WebGet(UriTemplate = "/Model/{pid}/{format}/{options}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetModel(string pid, string format, string options);

        [WebGet(UriTemplate = "/{pid}/{format}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetModelSimple(string pid, string format);

        [WebGet(UriTemplate = "/Reviews/{pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        List<Review> GetReviews(string pid);

        [WebGet(UriTemplate = "/Screenshot/{pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetScreenshot(string pid);

        [WebGet(UriTemplate = "/DeveloperLogo/{pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetDeveloperLogo(string pid);

        [WebGet(UriTemplate = "/SponsorLogo/{pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Stream GetSponsorLogo(string pid);

        [WebGet(UriTemplate = "/Metadata/{pid}", ResponseFormat = WebMessageFormat.Xml)]
        [OperationContract]
        Metadata GetMetadata(string pid);

    }
}