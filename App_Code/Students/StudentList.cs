using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasioCore
{
    public class StudentList: StudentInfo
    {
        
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string BusName { get; set; }
        public string RouteName { get; set; }
        public string DepartmentName { get; set; }
        public decimal BusRate { get; set; }
        public string StudentCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Sibling { get; set; }
        public string Sibling2 { get; set; }

    }
}