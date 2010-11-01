using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace vwarDAL
{
    public abstract class MetaDataBase
    {
        public String Serialize()
        {
            XmlSerializer s = new XmlSerializer(this.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                s.Serialize(stream, this);
                XmlDocument doc = new XmlDocument();
                stream.Position = 0;
                doc.Load(stream);
                var xml = doc.OuterXml;
                return xml.Trim();
            }

        }
        public void Deserialize(String data)
        {
            using (MemoryStream rstream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(rstream))
                {
                    writer.Write(data);
                    writer.Flush();
                    rstream.Position = 0;
                    Deserialize(rstream);
                }
            }
        }
        public void Deserialize(Stream data)
        {
            try
            {
                XmlSerializer s = new XmlSerializer(this.GetType());
                PopulateData(s.Deserialize(data));
            }
            catch { }
        }
        public void PopulateData(object data)
        {
            foreach (var info in GetType().GetProperties())
            {
                info.SetValue(this, info.GetValue(data, null), null);
            }
        }

    }
}
