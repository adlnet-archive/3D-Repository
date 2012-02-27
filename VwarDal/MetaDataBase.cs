//  Copyright 2011 U.S. Department of Defense

//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at

//      http://www.apache.org/licenses/LICENSE-2.0

//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace vwarDAL
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class MetaDataBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void Deserialize(Stream data)
        {
            try
            {
                XmlSerializer s = new XmlSerializer(this.GetType());
                PopulateData(s.Deserialize(data));
            }
            catch { }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void PopulateData(object data)
        {
            foreach (var info in GetType().GetProperties())
            {
                info.SetValue(this, info.GetValue(data, null), null);
            }
        }
    }
}
