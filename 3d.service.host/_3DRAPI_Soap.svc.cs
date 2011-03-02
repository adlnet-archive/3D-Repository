using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _3d.service.host
{
    public class _3DRAPI_Soap : _3DRAPI_Imp, I3DRAPI_Soap
    {
        string DeleteObject(string pid)
        {
            return "";
        }
        public string UpdateMetadata(Metadata md, string pid)
        {
            return base.UpdateMetadata(md, pid);
        }
        public string UploadFile(byte[] indata, string pid)
        {
            return base.UploadFile(indata, pid);
        }
        public string UploadScreenShot(byte[] data, string pid, string filename)
        {
            return base.UploadScreenShot(data, pid, filename);
        }
        public string UploadDeveloperLogo(byte[] indata, string pid, string filename)
        {
            return base.UploadDeveloperLogo((indata), pid, filename);
        }
        public string UploadSponserLogo(byte[] indata, string pid, string filename)
        {
            return base.UploadSponsorLogo((indata), pid, filename);
        }
    }
}