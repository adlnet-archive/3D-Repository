using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
namespace vwarDAL
{
    [Serializable]
    class LocalFileSystemStore:IFileStore
    {
        private string m_BaseDir;
        private string m_DataDir;
        private string m_Namespace;
        public LocalFileSystemStore()
        {
            m_BaseDir = ConfigurationManager.AppSettings["LocalFileStore_BaseDir"];
            m_Namespace = ConfigurationManager.AppSettings["fedoraNamespace"];

            if (m_Namespace == "" || m_Namespace == null)
                    m_Namespace = "Localhost";

            if (m_BaseDir == "" || m_BaseDir == null)
                m_BaseDir = "c:\\3DR_LocalFileStore\\";
            if (!Directory.Exists(m_BaseDir))
                Directory.CreateDirectory(m_BaseDir);
            m_DataDir = m_BaseDir + "data\\";
            if (!Directory.Exists(m_DataDir))
                Directory.CreateDirectory(m_DataDir);
            
        }
        public string AddSupportingFile(System.IO.Stream data, ContentObject co, string filename)
        {
            return SetContentFile(data, co.PID, filename);
        }
        public void DeleteContentObject(ContentObject co)
        {
            string pid = co.PID.Replace(":", "_");
            string dir = m_DataDir + pid +"\\";
            foreach (string file in Directory.GetFiles(dir))
            {
                File.Delete(file);
            }
            Directory.Delete(dir);
        }
        public System.IO.Stream GetContentFile(string pid, string file)
        {
            pid = pid.Replace(":", "_");
            foreach (string f in Directory.GetFiles(m_DataDir + pid + "\\"))
            {
                string f2 = Path.GetFileName(f);
                if (f2.StartsWith(file) || f2 == file)
                {
                    FileStream fs = new FileStream(f, FileMode.Open, FileAccess.Read);
                    MemoryStream ms = new MemoryStream();
                    int b = fs.ReadByte();
                    while (b != -1)
                    {
                        
                        ms.WriteByte((byte)b);
                        b = fs.ReadByte();
                    }
                    fs.Close();
                    ms.Seek(0, SeekOrigin.Begin);
                    return ms;
                }
            }
            return null;
        }
        public byte[] GetContentFileData(string pid, string dsid)
        {
            foreach (string f in Directory.GetFiles(m_DataDir + pid + "\\"))
            {
                string f2 = Path.GetFileName(f);
                if (f2.StartsWith(dsid) || f2 == dsid)
                {
                    FileStream fs = new FileStream(f, FileMode.Open, FileAccess.Read);
                    byte[] ms = new byte[fs.Length];
                    fs.Read(ms, 0, (int)fs.Length);
                    fs.Close();
                    return ms;
                }
            }
            return null;
        }
        public ContentObject GetNewContentObject()
        {
            ContentObject co = new ContentObject();
            string[] pids = Directory.GetDirectories(m_DataDir, "*" + m_Namespace + "*");
            co.PID = m_Namespace + ":" + pids.Length.ToString();
            return co;
        }
        public void InsertContentObject(ContentObject co)
        {
            string[] pids = Directory.GetDirectories(m_DataDir, "*" + m_Namespace + "*");
            co.PID = m_Namespace + ":" + pids.Length.ToString();

            string pid = co.PID.Replace(":", "_");
            if (!Directory.Exists(m_DataDir + pid))
                Directory.CreateDirectory(m_DataDir + pid);
        }
        public void RemoveFile(string pid, string fileName)
        {
            pid = pid.Replace(":", "_");
            string name = m_DataDir + pid + "\\" + fileName;
            if (File.Exists(name))
            {
                File.Delete(name);
            }
        }
        public string SetContentFile(System.IO.Stream data, string pid, string filename)
        {
            pid = pid.Replace(":", "_");
            if (!Directory.Exists(m_DataDir + pid))
                Directory.CreateDirectory(m_DataDir + pid);
            String newfilename = filename + "_" + Path.GetRandomFileName() + Path.GetExtension(filename);
            FileStream fs = new FileStream(m_DataDir + pid + "\\" + newfilename, FileMode.CreateNew, FileAccess.Write);
            data.Seek(0, SeekOrigin.Begin);
            int b = 0;
            while (b != -1)
            {
                
                b = data.ReadByte();
                fs.WriteByte((byte)b);
            }

            fs.Close();
            return newfilename;
        }
        public string UpdateFile(byte[] data, string pid, string fileName, string newFileName = null)
        {
            return SetContentFile(new MemoryStream(data), pid, fileName);
        }
        public string UpdateFile(System.IO.Stream data, string pid, string fileName, string newfileName = null)
        {
            return SetContentFile(data, pid, fileName);
        }
    }
}
