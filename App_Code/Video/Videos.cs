using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nits { 
public class Videos
{
        public long VID { get; set; }
        public string VTitle { get; set; }
        public long CIDFK { get; set; }
        public long SubjectIDFK { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public System.DateTime VDate { get; set; }
        public string VTime { get; set; }
        public long TIDFK { get; set; }
        public string UserName { get; set; }
        public string VRemarks { get; set; }
        public decimal VSize { get; set; }
        public  int IsLocked { get; set; }
        public string ThumbNail { get; set; }
        public Int32 Vists { get; set; }
        public string VPath { get; set; }
    }

    public class SessionVideos
    {
        public string subjectid { get; set; }
        public string subjectname { get; set; }
        public string classid { get; set; }

        public string videoid { get; set; }
        public string TID { get; set; }
        public string UID { get; set; }
        public string Videos { get; set; }
    }
}