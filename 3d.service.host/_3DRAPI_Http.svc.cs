using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.IO;
using System.Configuration;

namespace vwar.service.host
{
    public class _3DRAPI_Http : _3DRAPI_Imp, I3DRAPI_Http
    {
        public string AddReview(Stream indata, string pid)
        {
            //Read in the data as it streams in
            MemoryStream ms = new MemoryStream();
            int pos = 1;
            while (pos != -1)
            {
                pos = indata.ReadByte();
                if (pos > -1)
                    ms.WriteByte((byte)pos);
            }
            //We now have compleated the streamin in operation
            //Read the stream as a string
            ms.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[ms.Length];
            ms.Read(data, 0, (int)ms.Length);
            ms.Seek(0, SeekOrigin.Begin);
            string s = "";
            for (int i = 0; i < data.Length; i++)
                s += Convert.ToChar(data[i]);

            //deSerialize the data
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Review));
            System.IO.TextReader tx = new System.IO.StringReader(s);
            Review md = (Review)xs.Deserialize(tx);

            //Call the base class
            return base.AddReview(md, pid);
        }
        //This must deserialize a metadata object from the 
        //data in an HTTP post
        public string UpdateMetadata(Stream indata, string pid)
        {
            //Read in the data as it streams in
            MemoryStream ms = new MemoryStream();
            int pos = 1;
            while (pos != -1)
            {
                pos = indata.ReadByte();
                if (pos > -1)
                    ms.WriteByte((byte)pos);
            }
            //We now have compleated the streamin in operation
            //Read the stream as a string
            ms.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[ms.Length];
            ms.Read(data, 0, (int)ms.Length);
            ms.Seek(0, SeekOrigin.Begin);
            string s = "";
            for (int i = 0; i < data.Length; i++)
                s += Convert.ToChar(data[i]);

            //deSerialize the data
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Metadata));
            System.IO.TextReader tx = new System.IO.StringReader(s);
            Metadata md = (Metadata)xs.Deserialize(tx);

            //Call the base class
            return base.UpdateMetadata(md, pid);
        }
        //Convert a stream to data
        private byte[] StreamToData(Stream indata)
        {
            //Read in the streaming data
            MemoryStream ms = new MemoryStream();
            int pos = 1;
            while (pos != -1)
            {
                pos = indata.ReadByte();
                ms.WriteByte((byte)pos);
            }
            //Now, the streaming is complete
            ms.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[ms.Length];
            ms.Read(data, 0, (int)ms.Length);
            return data;
        }
        //Add a new content object
        public string UploadFile(Stream indata, string pid)
        {
            //Read the stream then call base class
            return base.UploadFile(StreamToData(indata), pid);
        }
        //Add a developer logo
        public string UploadDeveloperLogo(Stream indata, string pid)
        {
            //Get the name of the file that the client uploaded
            string content = WebOperationContext.Current.IncomingRequest.Headers["Content-disposition"];
            //Read the stream then call base class
            string filename = content.Substring(content.LastIndexOf("=") + 1);
            return base.UploadDeveloperLogo(StreamToData(indata), pid, filename);
        }
        public string UploadSupportingFile(Stream indata, string pid, string description)
        {
            //Get the name of the file that the client uploaded
            string content = WebOperationContext.Current.IncomingRequest.Headers["Content-disposition"];
            //Read the stream then call base class
            string filename = content.Substring(content.LastIndexOf("=") + 1);
            return base.UploadSupportingFile(StreamToData(indata), pid, filename, description);
        }
        public string UploadSponsorLogo(Stream indata, string pid)
        {
            //Get the name of the file that the client uploaded
            string content = WebOperationContext.Current.IncomingRequest.Headers["Content-disposition"];
            //Read the stream then call base class
            string filename = content.Substring(content.LastIndexOf("=") + 1);
            return base.UploadSponsorLogo(StreamToData(indata), pid, filename);
        }
        public string UploadScreenShot(Stream indata, string pid)
        {
            //Get the name of the file that the client uploaded
            string content = WebOperationContext.Current.IncomingRequest.Headers["Content-disposition"];
            //Read the stream then call base class
            string filename = content.Substring(content.LastIndexOf("=") + 1);
            return base.UploadScreenShot(StreamToData(indata), pid, filename);
        }
        public string UploadMissingTexture(Stream indata, string pid)
        {
            //Get the name of the file that the client uploaded
            string content = WebOperationContext.Current.IncomingRequest.Headers["Content-disposition"];
            //Read the stream then call base class
            string filename = content.Substring(content.LastIndexOf("=") + 1);
            return base.UploadMissingTexture(StreamToData(indata), pid, filename);
        }

    }
}