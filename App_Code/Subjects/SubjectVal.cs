using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasioCore
{
    public class SubjectVal
    {
        public static string getInsertSubjectValidation(Subject subject)
        {
            string retMsg = "";
            bool isValid = true;
            if (!validationBLL.IsNumber(subject.ClassID.ToString()))
            {
                isValid = false;
                retMsg = retMsg + "ClassID is not a Number <br/>";
            }

            if (validationBLL.IsNullOrEmpty(subject.ClassID.ToString()))
            {
                isValid = false;
                retMsg = retMsg + "ClassID is Empty/Null <br/>";
            }

            if (validationBLL.IsNullOrEmpty(subject.SubjectName.Trim().ToString()))
            {
                isValid = false;
                retMsg = retMsg + "SubjectName is Empty/Null <br/>";
            }

            if (validationBLL.IsNullOrEmpty(subject.Current_Session.ToString()))
            {
                isValid = false;
                retMsg = retMsg + "Current_Session is Empty/Null <br/>";
            }

            if (isValid)
                return "ok";
            return retMsg;
        }
    }
}