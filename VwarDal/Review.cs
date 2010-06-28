using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vwarDAL
{
    public class Review : MetaDataBase
    {
        public int Rating { get; set; }
        public string Text { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}
