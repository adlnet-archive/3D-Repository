using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vwarDAL
{
    public enum DatastreamsState  { UPLOADING, READY };
    public interface ITempContentManager
    {
        void EnableTempDatastreams(string pid, string hash);
        void DisableTempDatastreams(string pid);
        string GetTempLocation(string pid);
    }
}
