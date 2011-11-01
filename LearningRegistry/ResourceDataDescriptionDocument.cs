using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.Common;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace LR
{
    
    public class lr_base
    {
        public string Bencode()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            this.serialize(dictionary,null);
            Bencoding.BElement bencoded = bencode(dictionary);
            string b = bencoded.ToBencodedString();
            return b;
        }
        private Bencoding.BElement bencode(object input)
        {
            if (input == null)
                return new Bencoding.BString("null");
            if(input.GetType() == typeof(Dictionary<string, object>))
            {
                Dictionary<string, object> value = (Dictionary<string, object>)input;
                Bencoding.BDictionary bencoded = new Bencoding.BDictionary();
                foreach (KeyValuePair<string, object> pair in value)
                {
                    bencoded.Add(new Bencoding.BString(pair.Key),bencode(pair.Value));
                }
                return bencoded;
            }
            if (input.GetType().IsArray)
            {
                Array value = (Array)input;
                Bencoding.BList list = new Bencoding.BList();
                for (int i = 0; i < value.Length; i++)
                {
                    list.Add(bencode(value.GetValue(i)));
                }
                return list;
            }

            try
            {
                Type inner = input.GetType().GetGenericArguments()[0];


                Bencoding.BList Blist = new Bencoding.BList();
                System.Reflection.MethodInfo function = this.GetType().GetMethod("ToGeneric").MakeGenericMethod(inner);
                object output = function.Invoke(this, new object[] { input });
                List<object> newlist = (List<object>)output;
                foreach (object o in newlist)
                {
                    Blist.Add(bencode(o));
                }
                return Blist;

            }
            catch (Exception e)
            {
            }
           

            if (input.GetType() == typeof(string))
            {
                string value = (string)input;
                Bencoding.BString s = new Bencoding.BString(value);              
                return s;
            }
            if (input.GetType() == typeof(float))
            {
                string value = input.ToString();
                Bencoding.BString s = new Bencoding.BString(value);
                return s;
            }
            if (input.GetType() == typeof(double))
            {
                string value = input.ToString();
                Bencoding.BString s = new Bencoding.BString(value);
                return s;
            }
            if (input.GetType() == typeof(DateTime))
            {
                string value = input.ToString();
                Bencoding.BString s = new Bencoding.BString(value);
                return s;
            }
            if (input.GetType() == typeof(int))
            {
                int value = (int)input;
                Bencoding.BInteger s = new Bencoding.BInteger(value);
                return s;
            }

            Bencoding.BDictionary bdic = new Bencoding.BDictionary();
            Type t = input.GetType();
            foreach (System.Reflection.FieldInfo f in t.GetFields())
            {
                bdic.Add(new Bencoding.BString(f.Name),bencode(f.GetValue(input)));
            }
            foreach (System.Reflection.PropertyInfo f in t.GetProperties())
            {
                System.Reflection.MethodInfo mf = f.GetGetMethod();
                object o = mf.Invoke(input, null);
                bdic.Add(new Bencoding.BString(f.Name), bencode(o));
            }
            return  bdic;

            
        }
        public List<object> ToGeneric<T>(List<T> inlist)
        {
            List<object> newlist = new List<object>();
            foreach (T i in inlist)
                newlist.Add(i);
            return newlist;
        }
        public Dictionary<string, object> extensions;
        public lr_base()
        {
            extensions = new Dictionary<string, object>();
        }
        public virtual void serialize(IDictionary<string, object> dictionary, IDictionary<string, object> parent)
        {
            Type t = this.GetType();

            foreach (System.Reflection.FieldInfo f in t.GetFields())
            {
                object val = f.GetValue(this);
                bool handled = false;

                if(!handled)
                try
                {
                    if (val != null)
                    {
                        string s = ((RDDD.lr_string_enum)val).ToString();
                        if (s != "")
                        {
                            dictionary.Add(f.Name, s);
                            handled = true;
                        }
                    }
                }
                catch (System.InvalidCastException h)
                {
                }

                if (!handled)
                try
                {
                    if (val != null)
                    {
                        string s = ((string)val);
                        if (s != "")
                            dictionary.Add(f.Name, s);
                        handled = true;
                    }
                }
                catch (System.InvalidCastException h)
                {
                }

                if (!handled)
                try
                {
                    List<RDDD.lr_schema_value> values = (List<RDDD.lr_schema_value>)val;
                    if (values != null)
                    {
                        List<string> asstrings = new List<string>();
                        foreach (RDDD.lr_schema_value lrsv in values)
                            asstrings.Add(lrsv.ToString());

                        dictionary.Add(f.Name, asstrings);
                        handled = true;
                    }
                }
                catch (System.InvalidCastException h)
                {  
                }

                if (!handled)
                try
                {
                        lr_base b = ((lr_base)val);
                        if (val != null)
                        {
                            Dictionary<string, object> objs = new Dictionary<string, object>();
                            dictionary.Add(f.Name, objs);
                            b.serialize(objs,dictionary);
                            handled = true;
                        }
                }
                catch (System.InvalidCastException h)
                {
                }

                if (!handled)
                    try
                    {
                       
                        if (val != null)
                        {
                           Boolean b = ((Boolean)val); 
                            if(b==true)
                                dictionary.Add(f.Name,true);
                            else
                                dictionary.Add(f.Name, false);
                            handled = true;
                        }
                    }
                    catch (System.InvalidCastException h)
                    {
                    }

               
                if (!handled)
                    try
                    {
                        
                        if (val != null)
                        {int b = ((int)val);
                            dictionary.Add(f.Name, b);
                            handled = true;
                        }
                    }
                    catch (System.InvalidCastException h)
                    {
                    }

                if (!handled)
                    try
                    {
                        
                        if (val != null)
                        {float b = ((float)val);
                            dictionary.Add(f.Name, b.ToString());
                            handled = true;
                        }
                    }
                    catch (System.InvalidCastException h)
                    {
                    }

                if (!handled)
                    try
                    {

                        if (val != null)
                        {
                            List<string> b = ((List<string>)val);
                            List<object> l = new List<object>();
                            foreach (string s in b)
                            {
                                l.Add(s);
                            }
                            dictionary.Add(f.Name, l);
                            handled = true;
                        }
                    }
                    catch (System.InvalidCastException h)
                    {
                    }
                
                if(!handled && val != null && f.Name !="extensions")    
                    dictionary.Add(f.Name, val);

            }
            if (this.extensions != null)
                foreach (System.Collections.Generic.KeyValuePair<string, object> p in this.extensions)
                {
                    if (p.Value != null)
                        dictionary.Add(p.Key, p.Value);
                }
        }
    }
    namespace RDDD
    {
        public class lr_Converter : JavaScriptConverter
        {
            public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
            {
                return new lr_base();
            }

            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                Dictionary<string, object> objs = new Dictionary<string, object>();
                lr_base base_object = (lr_base)obj;

                base_object.serialize(objs, null);

                return objs;
            }

            public override IEnumerable<Type> SupportedTypes { get { return new Type[] { typeof(lr_base) }; } }
        }
     
        public class lr_string_enum
        {

        }

        public class lr_doc_type : lr_string_enum
        {
            public class resource_data : lr_doc_type
            {
                public override string ToString()
                {
                    return "resource_data";
                }
            }
            public class resource : lr_doc_type
            {
                public override string ToString()
                {
                    return "resource";
                }
            }
            public class assertion : lr_doc_type
            {
                public override string ToString()
                {
                    return "assertion";
                }
            }
            public class paradata : lr_doc_type
            {
                public override string ToString()
                {
                    return "paradata";
                }
            }
        }
        public class lr_doc_version : lr_string_enum
        {
            public class v_0_23_0 : lr_doc_version
            {
                public override string ToString()
                {
                    return "0.23.0";
                }
            }
        }
        public class lr_submitter_type : lr_string_enum
        {
            public class user : lr_submitter_type
            {
                public override string ToString()
                {
                    return "user";
                }
            }
            public class anonymous : lr_submitter_type
            {
                public override string ToString()
                {
                    return "anonymous";
                }
            }
            public class agent : lr_submitter_type
            {
                public override string ToString()
                {
                    return "agent";
                }
            }
        }

        public class lr_payload_placement : lr_string_enum
        {
            public class attached : lr_payload_placement
            {
                public override string ToString()
                {
                    return "attached";
                }
            }
            public class linked : lr_payload_placement
            {
                public override string ToString()
                {
                    return "linked";
                }
            }
            public class inline : lr_payload_placement
            {
                public override string ToString()
                {
                    return "inline";
                }
            }
        }
        public class lr_signing_method : lr_string_enum
        {
            public class LR_PGP10 : lr_signing_method
            {
                public override string ToString()
                {
                    return "LR-PGP.1.0";
                }
            }

        }
        public class lr_resource_data_type : lr_string_enum
        {

            public class metadata : lr_resource_data_type
            {
                public override string ToString()
                {
                    return "metadata";
                }
            }
            public class paradata : lr_resource_data_type
            {
                public override string ToString()
                {
                    return "paradata";
                }
            }
            public class resource : lr_resource_data_type
            {
                public override string ToString()
                {
                    return "resource";
                }
            }
        }
        public class lr_key_location : List<string>
        {

        }

        public class lr_digital_signature : lr_base
        {
            public string signature;
            public lr_key_location key_location;
            public lr_signing_method signing_method;

            public lr_digital_signature()
            {
                signature = "";
                key_location = new lr_key_location();
                signing_method = new lr_signing_method.LR_PGP10();
            }
        }
        public class lr_identity : lr_base
        {
            public string curator;
            public string owner;
            public string submitter;
            public string signer;
            public lr_submitter_type submitter_type;
            public lr_identity()
            {
                curator = "";
                owner = "";
                submitter = "";
                signer = "";
                submitter_type = new lr_submitter_type.agent();
            }
        }
        public class lr_keys : List<string>
        {

        }
        public class lr_schema_value
            {
            public class metadata : lr_schema_value
                {
                  
                public class DublinCore1_1 : lr_schema_value
                    {
                    public override string ToString() { return "DC 1.1"; }
                    }

                public class IEEE_LOM_2002 : lr_schema_value
                    {
                    public override string ToString() { return "IEEE LOM 2002"; }
                    }

                public class OAI_PMH_Dublin_Core : lr_schema_value
                    {
                    public override string ToString() { return "oai_dc"; }
                    }
                }
            public class paradata : lr_schema_value
                {
                //public class ActivityStream : lr_schema_value { public override string ToString() { return "ActivityStream"; } }
                public class LR_Paradata_1_0 : lr_schema_value { public override string ToString() { return "LR Paradata 1.0"; } }
                }
            public class other : lr_schema_value
                {
                public class Resource : lr_schema_value { public override string ToString() { return "Resource"; } }
                }
            }
        public class lr_payload_schema : List<lr_schema_value>
        {
           
        }
        public class lr_TOS : lr_base
        {
            public string submission_TOS;
            public string submission_attribution;
            public lr_TOS()
            {
                submission_TOS = "http://www.learningregistry.org/tos/cc0/v0-5/";
                submission_attribution = "";
            }
        }
        [DataContract]
        public class lr_document : lr_base
        {
          //  [DataMember]
          //  public lr_TOS TOS;
            [DataMember]
            public Boolean active;
            [DataMember]
            public lr_doc_type doc_type;
            [DataMember]
            public lr_doc_version doc_version;
            [DataMember]
            public lr_identity identity;
            [DataMember]
            public lr_keys keys;
            [DataMember]
            public lr_payload_placement payload_placement;
            [DataMember]
            public lr_payload_schema payload_schema;
            [DataMember]
            public Object resource_data;
            [DataMember]
            public lr_resource_data_type resource_data_type;
            [DataMember]
            public String resource_locator;
            [DataMember]
          
            public String payload_schema_locator;
            [DataMember]
            public String payload_schema_format;
            [DataMember]
            public String payload_locator;


            [DataMember]
            public int weight;

            [DataMember]
            public lr_digital_signature digital_signature;

            [DataMember]
            public int resource_TTL;
            public lr_document()
            {
              //  TOS = new lr_TOS();
                active = true;
                doc_type = new lr_doc_type.resource_data();
                doc_version = new lr_doc_version.v_0_23_0();
                identity = new lr_identity();
                keys = new lr_keys();
                payload_placement = new lr_payload_placement.inline();
                payload_schema = new lr_payload_schema();
                resource_data = "";
                resource_data_type = new lr_resource_data_type.metadata();
                resource_locator = "";
                payload_schema_locator = "";
                payload_schema_format = "";
               
                payload_locator = "";
                weight = 100;
                resource_TTL = 0;
                digital_signature = null;
            }
            public void Sign(string passphrase, string keyID, string keyloc)
            {
 
                string data = Bencode();

                string randomname = System.IO.Path.GetRandomFileName();
                randomname = System.IO.Path.GetTempPath() + randomname;
                try
                {
                    System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create();
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(data);

                    byte[] hash = sha256.ComputeHash(buffer);


                    System.IO.StreamWriter sw = new System.IO.StreamWriter(randomname + ".json.bencoded");
                    sw.Write(hash);
                    sw.Close();

                    string pgpfilename =  LR_3DR_Bridge.LR_Integration_GPGLocation() + "gpg.exe" ;

                    string arguements;
                    if (keyID == "")
                        arguements = " -b -a --passphrase-fd 0 " + "\"" + randomname  + ".json.bencoded"+ "\"";
                    else
                        arguements = "--default-key " + keyID + " -b -a --passphrase-fd 0 " + "\"" + randomname + ".json.bencoded" + "\"";
                    System.Diagnostics.ProcessStartInfo sfi = new System.Diagnostics.ProcessStartInfo(pgpfilename, arguements);
                    sfi.RedirectStandardInput = true;
                    sfi.CreateNoWindow = true;
                    sfi.UseShellExecute = false;
                    System.Diagnostics.Process p = System.Diagnostics.Process.Start(sfi);
                    p.StandardInput.WriteLine(passphrase);

                    p.WaitForExit();
                    if (p.ExitCode != 0)
                    {
                      
                        throw new Exception("GPG.exe returned code " + p.ExitCode.ToString());
                    }

                    System.IO.StreamReader sr = new System.IO.StreamReader( randomname + ".json.bencoded.asc");

                    string sig = sr.ReadToEnd();
                    sr.Close();

                    System.IO.File.Delete(randomname + ".json.bencoded.asc" );
                    System.IO.File.Delete(randomname + ".json.bencoded" );

                    digital_signature = new lr_digital_signature();
                    digital_signature.key_location.Add(keyloc);
                    digital_signature.signature = sig;
                    digital_signature.signing_method = new lr_signing_method.LR_PGP10();
                }
                catch (Exception e)
                {
                    try
                    {
                        System.IO.File.Delete(randomname + ".json.bencoded.asc");
                    }
                    catch (Exception j) { }
                    try
                    {
                        System.IO.File.Delete( randomname + ".json.bencoded" );
                     }
                    catch (Exception j) { }
                    throw e;
                }
            }
        }
        [DataContract]
        public class lr_Envelope:lr_base
        {
            public List<lr_document> documents;
            JavaScriptSerializer jsonSer;
            public string PublishURL;
            public lr_Envelope()
            {
                documents = new List<lr_document>();
                jsonSer = new JavaScriptSerializer();
            }
            public string Publish()
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                string result = wc.UploadString(LR_3DR_Bridge.LR_Integration_PublishURL(), Serialize());
                return result;
            }
            public void Sign(string passphrase, string keyID, string keyloc)
            {
                foreach (lr_document doc in documents)
                {
                    doc.Sign( passphrase,  keyID,  keyloc);
                }
            }
            public string Serialize()
            {

                //backup the resource data, then serialize it to a string
                object[] backups = new object[this.documents.Count];
                int i = 0;
                jsonSer.RegisterConverters(new JavaScriptConverter[] { new lr_Converter() });
                foreach (lr_document doc in documents)
                {
                    backups[i] = doc.resource_data;
                    i++;
    
                    string resource_Data = jsonSer.Serialize(doc.resource_data);
                    doc.resource_data = resource_Data;
                }
               
                Dictionary<string, object> objs = new Dictionary<string, object>();
                this.serialize(objs, null);


                string data = jsonSer.Serialize(objs);

                //restore the previous resource data
                i = 0;
                foreach (lr_document doc in documents)
                {
                    doc.resource_data = backups[i]; 
                }
                return data;

            }
        }
    }
}