using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using LR.RDDD;
using LR.Paradata;
using vwarDAL;
namespace LR
{
    public class LR_3DR_Bridge
    {
        static public bool LR_Integration_Enabled()
        {
            if (ConfigurationManager.AppSettings["LR_Integration_Enabled"] != null) 
                return System.Convert.ToBoolean(ConfigurationManager.AppSettings["LR_Integration_Enabled"]);
            return false;
        }
        static public string LR_Integration_KeyID()
        {
            return (ConfigurationManager.AppSettings["LR_Integration_KeyID"]);
        }
        static public string LR_Integration_PublishURL()
        {
            return (ConfigurationManager.AppSettings["LR_Integration_PublishURL"]);
        }
        static public string LR_Integration_KeyPassPhrase()
        {
            return (ConfigurationManager.AppSettings["LR_Integration_KeyPassPhrase"]);
        }
        static public string LR_Integration_GPGLocation()
        {
            return (ConfigurationManager.AppSettings["LR_Integration_GPGLocation"]);
        }
        static public string LR_Integration_PublicKeyURL()
        {
            return (ConfigurationManager.AppSettings["LR_Integration_PublicKeyURL"]);
        }
        static public string LR_Integration_SubmitterName()
        {
            return (ConfigurationManager.AppSettings["LR_Integration_SubmitterName"]);
        }
        static public string LR_Integration_SignerName()
        {
            return (ConfigurationManager.AppSettings["LR_Integration_SignerName"]);
        }
        static public string LR_Integration_APIBaseURL()
        {
            return (ConfigurationManager.AppSettings["LR_Integration_APIBaseURL"]);
        }
        static public String ModelUploaded(ContentObject co)
        {
            System.Threading.ParameterizedThreadStart t = new System.Threading.ParameterizedThreadStart(ModelUploaded_thread);
            System.Threading.Thread th = new System.Threading.Thread(t);
            th.Start(co);
            return "Thread fired";
        }
        static private void ModelUploaded_thread(object i)
        {
            ModelUploadedInternal((ContentObject)i);
        }
        static public String ModelUploadedInternal(ContentObject co)
        {
            //create a document and an envelop
            lr_Envelope env = new lr_Envelope();
            lr_document doc = new lr_document();
            
            //Add the keys from the contentobject to the keys for the document
            doc.keys.Add("3DR");
            string[] keywords = co.Keywords.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
            foreach (string key in keywords)
                doc.keys.Add(key);

            //This is the URI of the resource this data describes
            doc.resource_locator = LR_Integration_APIBaseURL() + co.PID + "/Format/dae?ID=00-00-00";
            
            //Submitted by the ADL3DR agent
            doc.identity.submitter = LR_Integration_SubmitterName();
            doc.identity.signer = LR_Integration_SignerName() ;
            doc.identity.submitter_type = new lr_submitter_type.agent();

            //The data is paradata
            doc.resource_data_type = new lr_resource_data_type.paradata();
            
            //Set ActivityStream as the paradata schema
            doc.payload_schema.Add(new lr_schema_value.paradata.LR_Paradata_1_0());

            Paradata.lr_Activity activity = new lr_Activity();
            //Create a paradata object
            Paradata.lr_Paradata pd = activity.activity;

            //Create a complex actor type, set to 3dr user
            lr_Actor.lr_Actor_complex mActor = new lr_Actor.lr_Actor_complex();
            mActor.description.Add("AnonymousUser");
            mActor.objectType = "3DR User";

            //Set the paradata actor
            pd.actor = mActor;

            //Create a complex verb type
            lr_Verb.lr_Verb_complex verb = new lr_Verb.lr_Verb_complex();
            verb.action = "Published";
            verb.context.id = "";
            verb.date = DateTime.Now;
            verb.measure = null;

            //Set the paradata verb
            pd.verb = verb;

            //Create a complex object type
            lr_Object.lr_Object_complex _object = new lr_Object.lr_Object_complex();
            _object.id = co.PID;

            //Set the paradata object
            pd._object = _object;

            //A human readable description for the paradata
            pd.content = "The a user uploaded a new model which was assigned the PID " + co.PID;
            
            //The resource_data of this Resource_data_description_document is the inline paradata
            doc.resource_data = activity;
            env.documents.Add(doc);

            //Create a second document
            doc = new lr_document();

            //Submitted by the ADL3DR agent
            doc.identity.submitter = LR_Integration_SubmitterName();
            doc.identity.signer = LR_Integration_SignerName();
            doc.identity.submitter_type = new lr_submitter_type.agent();

            //Add the keys from the content object to the document
            foreach (string key in keywords)
                doc.keys.Add(key);

            //the metadata will be inline
            doc.payload_placement = new lr_payload_placement.inline();

            //This is the resource the data describes
            doc.resource_locator = LR_Integration_APIBaseURL() + co.PID + "/Format/dae?ID=00-00-00";

            //The inline resource data is the contentobject
            doc.resource_data = co;

            //Set the scema to dublin core
            doc.payload_schema.Add(new lr_schema_value.metadata.DublinCore1_1());

            //this is metadata
            doc.resource_data_type = new lr_resource_data_type.metadata();

            //Add the doc to the envelope
            env.documents.Add(doc);

            //sign the envelope 
            env.Sign(LR_Integration_KeyPassPhrase(),LR_Integration_KeyID(),LR_Integration_PublicKeyURL());
            
            //Serialize and publish
            return env.Publish();
        }
        static public String ModelDownloaded(ContentObject co)
        {
            System.Threading.ParameterizedThreadStart t = new System.Threading.ParameterizedThreadStart(ModelDownloaded_thread);
            System.Threading.Thread th = new System.Threading.Thread(t);
            th.Start(co);
            return "Thread fired";
        }
        static private void ModelDownloaded_thread(object i)
        {
            ModelDownloadedInternal((ContentObject)i);
        }
        static public String ModelDownloadedInternal(ContentObject co)
        {
            lr_Envelope env = new lr_Envelope();
            lr_document doc = new lr_document();

            //Add the keys from the contentobject to the keys for the document
            doc.keys.Add("3DR");
            string[] keywords = co.Keywords.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string key in keywords)
                doc.keys.Add(key);

            //This is the URI of the resource this data describes
            doc.resource_locator = LR_Integration_APIBaseURL() + co.PID + "/Format/dae?ID=00-00-00";

            //Submitted by the ADL3DR agent
            doc.identity.submitter = LR_Integration_SubmitterName();
            doc.identity.signer = LR_Integration_SignerName();
            doc.identity.submitter_type = new lr_submitter_type.agent();

            //The data is paradata
            doc.resource_data_type = new lr_resource_data_type.paradata();

            //Set ActivityStream as the paradata schema
            doc.payload_schema.Add(new lr_schema_value.paradata.LR_Paradata_1_0());

            Paradata.lr_Activity activity = new lr_Activity();
            //Create a paradata object
            Paradata.lr_Paradata pd = activity.activity;

            //Set the paradata actor
            pd.actor = null;

            //Create a complex verb type
            lr_Verb.lr_Verb_complex verb = new lr_Verb.lr_Verb_complex();
            verb.action = "Downloaded";
            verb.context.id = "";
            verb.date = DateTime.Now;
            lr_Measure measure = new lr_Measure();
            measure.measureType = "count";
            measure.value = co.Downloads.ToString();
            verb.measure = measure;

            //Set the paradata verb
            pd.verb = verb;

            //Create a complex object type
            lr_Object.lr_Object_complex _object = new lr_Object.lr_Object_complex();
            _object.id = co.PID;

            //Set the paradata object
            pd._object = _object;

            //A human readable description for the paradata
            pd.content = "The a user downloaded this model from the ADL 3DR.";

            //The resource_data of this Resource_data_description_document is the inline paradata
            doc.resource_data = activity;
            env.documents.Add(doc);

            //sign the envelope 
            env.Sign(LR_Integration_KeyPassPhrase(), LR_Integration_KeyID(), LR_Integration_PublicKeyURL());     

            //Serialize and publish
            return env.Publish();
        }
        static public String ModelViewed(ContentObject co)
        {
            System.Threading.ParameterizedThreadStart t = new System.Threading.ParameterizedThreadStart(ModelViewed_thread);
            System.Threading.Thread th = new System.Threading.Thread(t);
            th.Start(co);
            return "Thread fired";
        }
        static private void ModelViewed_thread(object i)
        {
            ModelViewedInternal((ContentObject)i);
        }
        static public String ModelViewedInternal(ContentObject co)
        {
            lr_Envelope env = new lr_Envelope();
            lr_document doc = new lr_document();

            //Add the keys from the contentobject to the keys for the document
            doc.keys.Add("3DR");
            string[] keywords = co.Keywords.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string key in keywords)
                doc.keys.Add(key);

            //This is the URI of the resource this data describes
            doc.resource_locator = LR_Integration_APIBaseURL() + co.PID + "/Format/dae?ID=00-00-00";

            //Submitted by the ADL3DR agent
            doc.identity.submitter = LR_Integration_SubmitterName();
            doc.identity.signer = LR_Integration_SignerName();
            doc.identity.submitter_type = new lr_submitter_type.agent();

            //The data is paradata
            doc.resource_data_type = new lr_resource_data_type.paradata();

            //Set ActivityStream as the paradata schema
            doc.payload_schema.Add(new lr_schema_value.paradata.LR_Paradata_1_0());

            Paradata.lr_Activity activity = new lr_Activity();
            //Create a paradata object
            Paradata.lr_Paradata pd = activity.activity;

            //Set the paradata actor
            pd.actor = null;

            //Create a complex verb type
            lr_Verb.lr_Verb_complex verb = new lr_Verb.lr_Verb_complex();
            verb.action = "Viewed";
            verb.context.id = "";
            verb.date = DateTime.Now;
            lr_Measure measure = new lr_Measure();
            measure.measureType = "count";
            measure.value = co.Views.ToString();
            verb.measure = measure;
            pd.content = "The number of times the model has been previewed on the ADL 3DR website.";
            //Set the paradata verb
            pd.verb = verb;

            //Create a complex object type
            lr_Object.lr_Object_complex _object = new lr_Object.lr_Object_complex();
            _object.id = co.PID;

            //Set the paradata object
            pd._object = _object;

            //A human readable description for the paradata
            pd.content = "The current number of 'views' for this object in the ADL 3DR";

            //The resource_data of this Resource_data_description_document is the inline paradata
            doc.resource_data = activity;
            env.documents.Add(doc);

            //sign the envelope 
            env.Sign(LR_Integration_KeyPassPhrase(), LR_Integration_KeyID(), LR_Integration_PublicKeyURL());

            //Serialize and publish
            return env.Publish();
        }
        static public String ModelRated(ContentObject co)
        {
            System.Threading.ParameterizedThreadStart t = new System.Threading.ParameterizedThreadStart(ModelRated_thread);
            System.Threading.Thread th = new System.Threading.Thread(t);
            th.Start(co);
            return "Thread fired";
        }
        static private void ModelRated_thread(object i)
        {
            ModelRatedInternal((ContentObject)i);
        }
        static public String ModelRatedInternal(ContentObject co)
        {
            lr_Envelope env = new lr_Envelope();
            lr_document doc = new lr_document();

            //Add the keys from the contentobject to the keys for the document
            doc.keys.Add("3DR");
            string[] keywords = co.Keywords.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string key in keywords)
                doc.keys.Add(key);

            //This is the URI of the resource this data describes
            doc.resource_locator = LR_Integration_APIBaseURL() + co.PID + "/Format/dae?ID=00-00-00";

            //Submitted by the ADL3DR agent
            doc.identity.submitter = LR_Integration_SubmitterName();
            doc.identity.signer = LR_Integration_SignerName();
            doc.identity.submitter_type = new lr_submitter_type.agent();

            //The data is paradata
            doc.resource_data_type = new lr_resource_data_type.paradata();

            //Set ActivityStream as the paradata schema
            doc.payload_schema.Add(new lr_schema_value.paradata.LR_Paradata_1_0());

            Paradata.lr_Activity activity = new lr_Activity();
            //Create a paradata object
            Paradata.lr_Paradata pd = activity.activity;

            //Set the paradata actor
            pd.actor = null;

            //Create a complex verb type
            lr_Verb.lr_Verb_complex verb = new lr_Verb.lr_Verb_complex();
            verb.action = "Rated";
            verb.context.id = "";
            verb.date = DateTime.Now;
            lr_Measure measure = new lr_Measure();
            measure.measureType = "value";


            float reviewtotal = 0;
            foreach (Review r in co.Reviews)
            {
                reviewtotal += r.Rating;
            }
            measure.value = (reviewtotal/co.Reviews.Count).ToString();
            verb.measure = measure;
            if (measure.value == "NaN")
                return "";
            //Set the paradata verb
            pd.verb = verb;

            //Create a complex object type
            lr_Object.lr_Object_complex _object = new lr_Object.lr_Object_complex();
            _object.id = co.PID;

            //Set the paradata object
            pd._object = _object;

            //A human readable description for the paradata
            pd.content = "Users on the ADL 3DR rated this model with this average rating.";

            //The resource_data of this Resource_data_description_document is the inline paradata
            doc.resource_data = activity;
            env.documents.Add(doc);

            //sign the envelope 
            env.Sign(LR_Integration_KeyPassPhrase(), LR_Integration_KeyID(), LR_Integration_PublicKeyURL());

            //Serialize and publish
            
                return env.Publish();
        }
    }
}
