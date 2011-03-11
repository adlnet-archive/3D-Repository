using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vwarDAL
{
    public class DataUtils
    {
        public static string GetMimeType(string fileName)
        {
            if (String.IsNullOrEmpty(fileName)) return "";
           
            string mimeType = "text/plain";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();

            if (ext == ".skp") return "application/octet-stream";
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
    }
}
