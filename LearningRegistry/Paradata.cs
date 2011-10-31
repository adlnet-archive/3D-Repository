using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LR
{
    namespace Paradata
    {
        public class lr_Measure : lr_base
        {
            public String measureType;
            public String value;
        }
        public class lr_Context : lr_base
        {
            public String id;
        }
        public class lr_Object : lr_base
        {
            public class lr_Object_complex : lr_Object
            {
                public String id;
                public override void serialize(IDictionary<string, object> dictionary, IDictionary<string, object> parent)
                {
                    base.serialize(dictionary,parent);
                    parent["object"] = dictionary;
                    parent.Remove("_object");
                }
            }
            public class lr_Object_simple : lr_Object
            {
                public string value;
                public override void serialize(IDictionary<string, object> dictionary, IDictionary<string, object> parent)
                {
                    parent["object"] = value;
                    parent.Remove("_object");
                }
            }
        }
        public class lr_Actor : lr_base
        {
            public class lr_Actor_complex : lr_Actor
            {
                public String objectType;
                public List<String> description;
                public lr_Actor_complex()
                {
                    description = new List<string>();
                }
            }
            public class lr_Actor_simple : lr_Actor
            {
                public string value;
                public override void serialize(IDictionary<string, object> dictionary, IDictionary<string, object> parent)
                {
                    parent["actor"] = value;
                }
            } 
        }
        public class lr_Verb : lr_base
        {
            public class lr_Verb_simple : lr_Verb
            {
                public string value;
                public override void serialize(IDictionary<string, object> dictionary, IDictionary<string, object> parent)
                {
                    parent["verb"] = value;
                }
            }
            public class lr_Verb_complex : lr_Verb
            {
                public String action;
                public lr_Measure measure;
                public lr_Context context;
                public DateTime date;
                public lr_Verb_complex()
                {
                    measure = new lr_Measure();
                    context = new lr_Context();
                    date = DateTime.Now;
                    action = "";
                }
            }
        }
        public class lr_Activity
        {
            public lr_Paradata activity;
            public lr_Activity()
            {
                activity = new lr_Paradata();
            }
        }
        public class lr_Paradata : lr_base
        {
            public lr_Actor actor;
            public lr_Verb verb;
            public lr_Object _object;
            public List<lr_Object> related;
            public String content;

            public lr_Paradata()
            {
                actor = new lr_Actor();
                verb = new lr_Verb();
                _object = new lr_Object();
                related = new List<lr_Object>();
                content = "";
            }
        }
    }
}
