using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Security.Cryptography;
namespace vwar.service.host
{
   
        public class CacheIdentifier
        {
            public string PID;
            public enum FILETYPE { OriginalFile, CompressedDAE, CompressedOBJ, Compressed3DS, CompressedFBX, CompressedJSON, CompressedSKP, SKP, _3DS, OBJ, JSON, DAE, O3D, FBX, METADATA, TEXTURE, SUPPORTINGFILE, REVIEWS, SCREENSHOT, THUMBNAIL,DeveloperLogo,SponsorLogo };
            public FILETYPE Type;
            public string FileName;
            public string ToString()
            {
                string lFileName = FileName;
                if (Type != FILETYPE.TEXTURE && Type != FILETYPE.SUPPORTINGFILE)
                    lFileName = "";

                string lpid = PID.Replace(":", "_");

                string totalidentifier = lpid + lFileName + Enum.GetName(Type.GetType(), Type);
                return Base64EncodeHash(totalidentifier);
            }
            public CacheIdentifier(string p, string n, FILETYPE t)
            {
                PID = p;
                FileName = n;
                Type = t;
            }
            private static string Base64EncodeHash(string url)
            {
                byte[] result;
                HMACSHA1 shaM = new HMACSHA1();
                shaM.Key = new byte[2];

                byte[] ms = new byte[url.Length];

                for (int i = 0; i < url.Length; i++)
                {
                    byte b = Convert.ToByte(url[i]);
                    ms[i] = (b);
                }

                shaM.Initialize();

                result = shaM.ComputeHash(ms, 0, ms.Length);



                string hash = System.Convert.ToBase64String(result);
                return System.Web.HttpUtility.UrlEncode(hash);
                

            }
        }
        public class CacheFile
        {
            public byte[] data;
            public string filename;
            public string hashcode;
            public TimeSpan age;
        }
        public class CacheManager
        {
            public static CacheFile GetCacheFile(CacheIdentifier i)
            {
                CacheFile ret = null;
                string cachedir = ConfigurationManager.AppSettings["cache_dir"];
                string totalfile = System.IO.Path.Combine(cachedir, i.ToString());
                if (System.IO.File.Exists(totalfile))
                {
                    System.IO.FileStream fs = new System.IO.FileStream(totalfile, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    ret = new CacheFile();
                    ret.data = new byte[fs.Length];
                    fs.Read(ret.data, 0, (int)fs.Length);
                    fs.Close();
                    ret.age = DateTime.Now - System.IO.File.GetCreationTime(totalfile);
                    ret.filename = totalfile;
                    ret.hashcode = i.ToString();
                }

                return ret;
            }
            public static bool ExpireCache(CacheIdentifier i)
            {
                string cachedir = ConfigurationManager.AppSettings["cache_dir"];
                string totalfile = System.IO.Path.Combine(cachedir, i.ToString());
                try{
                    System.IO.File.Delete(totalfile);
                    return true;
                }
                catch(Exception e)
                {
                    return false;
                }
                return false;
            }
            public static T CheckCache<T>(CacheIdentifier i)
            {
                string cachedir = ConfigurationManager.AppSettings["cache_dir"];
                string totalfile = System.IO.Path.Combine(cachedir, i.ToString());
                if (!System.IO.File.Exists(totalfile))
                    return default(T);
                System.IO.FileStream fs = new System.IO.FileStream(totalfile, System.IO.FileMode.Open, System.IO.FileAccess.Read);


                if (typeof(T) == typeof(byte[]))
                {
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    fs.Close();
                    return (T)(object)data;
                }
                else
                {
                    System.Runtime.Serialization.Json.DataContractJsonSerializer ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));

                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, data.Length);
                    fs.Close();
                    T ret = (T)ser.ReadObject(new System.IO.MemoryStream(data));
                    return ret;
                }
                fs.Close();
                return default(T);
            }
            public static void Cache<T>(ref T obj, CacheIdentifier i)
            {
                string cachedir = ConfigurationManager.AppSettings["cache_dir"];
                string totalfile = System.IO.Path.Combine(cachedir, i.ToString());
                if (System.IO.File.Exists(totalfile))
                    System.IO.File.Delete(totalfile);
                System.IO.FileStream fs = new System.IO.FileStream(totalfile, System.IO.FileMode.Create, System.IO.FileAccess.Write);


                if (obj.GetType() == typeof(byte[]))
                {
                    byte[] data = (byte[])(Object)obj;
                    fs.Write(data, 0, data.Length);
                }
                else
                {
                    System.Runtime.Serialization.Json.DataContractJsonSerializer ser = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
                    ser.WriteObject(fs,obj);
                }
                fs.Close();
            }

        }
    
}