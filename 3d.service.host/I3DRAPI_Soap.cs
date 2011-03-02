using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;
namespace _3d.service.host
{
    [ServiceContract]
    public interface I3DRAPI_Soap
    {

        [OperationContract]
        Stream GetModel(string pid, string format, string options);

        [OperationContract]
        Stream GetScreenshot(string pid);

        [OperationContract]
        Stream GetDeveloperLogo(string pid);

        [OperationContract]
        Stream GetSponsorLogo(string pid);

        [OperationContract]
        List<SearchResult> Search(string terms);

        [OperationContract]
        Metadata GetMetadata(string pid);

        [OperationContract]
        string UploadFile(byte[] indata, string pid);

        [OperationContract]
        string UploadScreenShot(byte[] indata, string pid, string filename);

        [OperationContract]
        string UpdateMetadata(Metadata m, string pid);

        [OperationContract]
        string DeleteObject(string pid);

        [OperationContract]
        string UploadDeveloperLogo(byte[] indata, string pid, string filename);

        [OperationContract]
        string UploadSponserLogo(byte[] indata, string pid, string filename);

        [OperationContract]
        string UploadSupportingFile(byte[] m, string pid, string filename, string description);

        [OperationContract]
        bool DeleteSupportingFile(string pid, string filename);

        [OperationContract]
        Stream GetSupportingFile(string pid, string filename);

        [OperationContract]
        string UploadMissingTexture(byte[] m, string pid, string filename);

    }
}
