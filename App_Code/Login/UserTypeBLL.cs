using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserTypeBLL
/// </summary>
/// 
namespace Nits
{
    public class UserTypeBLL
    {
        public static List<UserTypes> getUserTypes()
        {
            List<UserTypes> ut = new List<UserTypes>();// { [] };
            ut.Add(new UserTypes() { UserTypeID = "0", UserType = "Student" });
            ut.Add(new UserTypes() { UserTypeID = "1", UserType = "Admin" });
            ut.Add(new UserTypes() { UserTypeID = "2", UserType = "Teacher" });

            return ut;

        }
    }
}