using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nits
{
    public class UserModel
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string ClassID { get; set; }
        public  bool setPassword { get; set; }
        public string Phone { get; set; }
        public string UserFullName { get; set; }
        public string UserAddress { get; set; }
        public string FatherName { get; set; }
        public string Email { get; set; }
        public string IsLocked { get; set; }
        public string IsOn { get; set; }
        public Nullable< long> StudentNo { get; set; }
        public List<TeacherSubjects> TchrSubjects {get;set;}
    }

    public class TeacherSubjects
    {
        public string UIDFK { get; set; }
        public string ClassID { get; set; }
        public string SubjectIDs { get; set; }
        public string ClassName { get; set; }
        public string SubjectNames { get; set; }
    }
}