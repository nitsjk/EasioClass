using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasioCore
{
    public class OpSubVal
    {
        public static string getInsertSubjectValidation(OptionalSubject opSubject)
        {
            string retMsg = "";
            bool isValid = true;
            if (!validationBLL.IsNumber(opSubject.ClassID.ToString()))
            {
                isValid = false;
                retMsg = retMsg + "ClassID is not a Number <br/>";
            }

            if (validationBLL.IsNullOrEmpty(opSubject.ClassID.ToString()))
            {
                isValid = false;
                retMsg = retMsg + "ClassID is Empty/Null <br/>";
            }

            if (validationBLL.IsNullOrEmpty(opSubject.OptionalSubjectName.Trim().ToString()))
            {
                isValid = false;
                retMsg = retMsg + "SubjectName is Empty/Null <br/>";
            }

            if (validationBLL.IsNullOrEmpty(opSubject.Current_Session.ToString()))
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