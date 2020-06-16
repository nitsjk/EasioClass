using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nits.Common;
using Nits.ENC;
using System.Data;
using System.Data.SqlClient;
/// <summary>
/// Summary description for LoginBLL
/// </summary>
/// 
namespace Nits
{
    public class LoginBLL
    {
       public static List<StudentsLogin> getAllUsers(string SectionID)
        {
            List<StudentsLogin> sL = new List<StudentsLogin>();

            SqlParameter SecID = new SqlParameter("@SectionID", SectionID);
            DataSet ds = new DataSet();

            if (GeneralBLL.SqlHelperConnection == 1)
            {

                ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from studentslogin where SectionID=@SectionID order by OLSLOGINID", SecID);
            }
            else

            {
                ds = SqlHelper.ExecuteDataset(SqlHelper.Connect2, CommandType.Text, "select * from studentslogin where SectionID=@SectionID order by OLSLOGINID", SecID);
            }


            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                sL.Add(new StudentsLogin()

                {
                    OLSLOGINID = dr["OLSLOGINID"].ToString(),
                    Encusersname=Encryption.Decrypt( dr["Encusersname"].ToString(),Encryption.key),
                    encpassword= Encryption.Decrypt(dr["encpassword"].ToString(), Encryption.key),
                    studentid= dr["studentid"].ToString(),
                    studentname= dr["studentname"].ToString(),
                    StudentNo = dr["StudentNo"].ToString(),
                    FatherName= dr["FatherName"].ToString(),
                    fatherphoneno= dr["fatherphoneno"].ToString(),
                    classid= dr["classid"].ToString(),
                    sectionid= dr["sectionid"].ToString(),
                    Session = dr["Session"].ToString(),
                    PhoneNo= dr["PhoneNo"].ToString(),
                    photopath= dr["photopath"].ToString(),
                    usertype= dr["usertype"].ToString(),
                    isbloked = dr["isbloked"].ToString(),
                    IsOn = dr["IsOn"].ToString(),




            }
                    );
            }



            return sL;

        }
        public static List<StudentsLogin> getAllUsersOnType(string userType)
        {
            List<StudentsLogin> sL = new List<StudentsLogin>();

            SqlParameter SecID = new SqlParameter("@userType", userType);
            DataSet ds = new DataSet();

            if (GeneralBLL.SqlHelperConnection == 1)
            {
                ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from studentslogin where userType=@userType order by OLSLOGINID", SecID);
            }

            else
            {

                ds = SqlHelper.ExecuteDataset(SqlHelper.Connect2, CommandType.Text, "select * from studentslogin where userType=@userType order by OLSLOGINID", SecID);
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sL.Add(new StudentsLogin()

                {
                    OLSLOGINID = dr["OLSLOGINID"].ToString(),
                    Encusersname = dr["Encusersname"].ToString(),// Encryption.Decrypt(dr["Encusersname"].ToString(), Encryption.key),
                    encpassword = dr["encpassword"].ToString(),// Encryption.Decrypt(dr["encpassword"].ToString(), Encryption.key),
                    studentid = dr["studentid"].ToString(),
                    studentname = dr["studentname"].ToString(),
                    StudentNo = dr["StudentNo"].ToString(),
                    FatherName = dr["FatherName"].ToString(),
                    fatherphoneno = dr["fatherphoneno"].ToString(),
                    classid = dr["classid"].ToString(),
                    sectionid = dr["sectionid"].ToString(),
                    Session = dr["Session"].ToString(),
                    PhoneNo = dr["PhoneNo"].ToString(),
                    photopath = dr["photopath"].ToString(),
                    usertype = dr["usertype"].ToString(),
                    isbloked= dr["isbloked"].ToString(),
                    IsOn = dr["IsOn"].ToString(),




            }
                    );
            }



            return sL;

        }

        public static string addNewUser(StudentsLogin SL)
        {
            int rt = 0;
            SqlParameter[] param = {
                new SqlParameter("@encusersname",Encryption.Encrypt( SL.Encusersname.Trim(),Encryption.key)),
                new SqlParameter("@encpassword",Encryption.Encrypt( SL.encpassword,Encryption.key)),
                new SqlParameter("@usertype", SL.usertype),
                new SqlParameter("@studentname", SL.studentname),
                new SqlParameter("@FatherName", SL.FatherName),
                new SqlParameter("@PhoneNo", SL.PhoneNo),
                new SqlParameter("@Address", SL.Address),
                new SqlParameter("@photopath", SL.photopath),
                new SqlParameter("@fatherphoneno", SL.fatherphoneno),
                new SqlParameter("@olstdfk", SL.olstdfk),
                new SqlParameter("@studentid", SL.studentid),
                new SqlParameter("@classid", SL.classid),
                new SqlParameter("@sectionid", SL.sectionid),
                new SqlParameter("@Session", SL.Session),
                new SqlParameter("@StudentNo", SL.StudentNo),
                new SqlParameter("@isbloked", SL.isbloked),
                      };
            if (LoginBLL.checkUserAlreadyExists(SL.Encusersname) == 0)
            {
                string sql = "insert into StudentsLogin (encusersname,encpassword,usertype,studentname,FatherName,PhoneNo,Address,photopath,fatherphoneno,olstdfk,studentid,classid,sectionid,Session,StudentNo,isbloked) values (@encusersname,@encpassword,@usertype,@studentname,@FatherName,@PhoneNo,@Address,@photopath,@fatherphoneno,@olstdfk,@studentid,@classid,@sectionid,@Session,@StudentNo,@isbloked)";

                
                if (GeneralBLL.SqlHelperConnection == 1)
                {
                    rt = SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, sql, param);
                }
                else
                {
                    rt = SqlHelper.ExecuteNonQuery(SqlHelper.Connect2, CommandType.Text, sql, param);
                }
            }
            return rt.ToString();
        }
        public static string userLoginCheck(string UN, string PW)
        {
            StudentsLogin UserLog = new StudentsLogin();

            string returnUrl = "none";
            //HttpContext.Current.Session["Login"] = "1"; // for Admin
            //returnUrl = "admin.aspx";
            SqlParameter[] param = {
                new SqlParameter("@encusersname",Encryption.Encrypt( UN,Encryption.key)),
                new SqlParameter("@encpassword", Encryption.Encrypt(PW,Encryption.key)),

        };
            DataSet ds = new DataSet();

            if (GeneralBLL.SqlHelperConnection == 1)
            {
                ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from studentslogin where encusersname=@encusersname and encpassword=@encpassword", param);
            }

            else
            {

                ds = SqlHelper.ExecuteDataset(SqlHelper.Connect2, CommandType.Text, "select * from studentslogin where encusersname=@encusersname and encpassword=@encpassword", param);
            }

            if(ds.Tables[0].Rows.Count>0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                UserLog.OLSLOGINID = dr["OLSLOGINID"].ToString();
                UserLog.Encusersname = Encryption.Decrypt(dr["Encusersname"].ToString(), Encryption.key);
                UserLog.encpassword = Encryption.Decrypt(dr["encpassword"].ToString(), Encryption.key);
                UserLog.studentid = dr["studentid"].ToString();
                UserLog.studentname = dr["studentname"].ToString();
                //UserLog.StudentNo = dr["StudentNo"].ToString();
               // UserLog.FatherName = dr["FatherName"].ToString();
                UserLog.fatherphoneno = dr["fatherphoneno"].ToString();
                UserLog.classid = dr["classid"].ToString();
                UserLog.sectionid = dr["sectionid"].ToString();
                UserLog.Session = dr["Session"].ToString();
               // UserLog.PhoneNo = dr["PhoneNo"].ToString();
                UserLog.photopath = dr["photopath"].ToString();
                UserLog.usertype = dr["usertype"].ToString();
                UserLog.isbloked = dr["isbloked"].ToString();
               // UserLog.IsOn = dr["IsOn"].ToString();

                int type = Convert.ToInt32(dr["usertype"].ToString());
                HttpContext.Current.Session["StudentID"] = dr["studentid"].ToString();
                HttpContext.Current.Session["CID"] = dr["classid"].ToString();
                HttpContext.Current.Session["UN"] = Encryption.Decrypt(dr["Encusersname"].ToString(), Encryption.key);
                HttpContext.Current.Session["UserID"] = dr["OLSLOGINID"].ToString();
                if (type.Equals(0))
                {
                    HttpContext.Current.Session["Login"] = "0"; // for student
                    returnUrl = "Exam.aspx";
                 
                }
                else if (type.Equals(1))
                {
                    HttpContext.Current.Session["Login"] = "1"; // for Admin
                    returnUrl = "Admin.aspx";
                }
                else if (type.Equals(2))
                {
                    HttpContext.Current.Session["Login"] = "2"; // for Teacher
                    returnUrl = "teacher.aspx";
                }


            }
            //else
            //{
            //    UserLog.OLSLOGINID = "0";
            //    UserLog.Encusersname = "0";
            //    UserLog.encpassword = "0";
            //    UserLog.studentid = "0";
            //    UserLog.studentname = "0";
            //    UserLog.StudentNo = "0";
            //    UserLog.FatherName = "0";
            //    UserLog.fatherphoneno = "0";
            //    UserLog.classid = "0";
            //    UserLog.sectionid = "0";
            //    UserLog.Session = "0";
            //    UserLog.PhoneNo = "0";
            //    UserLog.photopath = "0";
            //    UserLog.usertype = "0";
            //    UserLog.isbloked = "0";
            //    UserLog.IsOn = "0";
            //}

            return returnUrl;
        }


        public static int checkUserAlreadyExists(string UN)
        {
           
                      SqlParameter[] param = {
                new SqlParameter("@encusersname",Encryption.Encrypt( UN,Encryption.key)),
               
        };
            int Is = 0;

            if (GeneralBLL.SqlHelperConnection == 1)
            {
                Is = (int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from studentslogin where encusersname=@encusersname", param);
            }

            else
            {

                Is = (int)SqlHelper.ExecuteScalar(SqlHelper.Connect2, CommandType.Text, "select count(*) from studentslogin where encusersname=@encusersname", param);
            }



            return Is;
        }
        public static StudentsLogin userDetail(string UserID)
        {
            StudentsLogin UserLog = new StudentsLogin();

            

            SqlParameter[] param = {
                new SqlParameter("@UserID",UserID),
                       };
            DataSet ds = new DataSet();

            if (GeneralBLL.SqlHelperConnection == 1)
            {
                ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from studentslogin where OLSLOGINID=@UserID ", param);
            }

            else
            {

                ds = SqlHelper.ExecuteDataset(SqlHelper.Connect2, CommandType.Text, "select * from studentslogin where OLSLOGINID=@UserID ", param);
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                UserLog.OLSLOGINID = dr["OLSLOGINID"].ToString();
                UserLog.Encusersname = Encryption.Decrypt(dr["Encusersname"].ToString(), Encryption.key);
                UserLog.encpassword = Encryption.Decrypt(dr["encpassword"].ToString(), Encryption.key);
                UserLog.studentid = dr["studentid"].ToString();
                UserLog.studentname = dr["studentname"].ToString();
                UserLog.StudentNo = dr["StudentNo"].ToString();
                UserLog.FatherName = dr["FatherName"].ToString();
                UserLog.fatherphoneno = dr["fatherphoneno"].ToString();
                UserLog.classid = dr["classid"].ToString();
                UserLog.sectionid = dr["sectionid"].ToString();
                UserLog.Session = dr["Session"].ToString();
                UserLog.PhoneNo = dr["PhoneNo"].ToString();
                UserLog.photopath = dr["photopath"].ToString();
                UserLog.usertype = dr["usertype"].ToString();
                UserLog.isbloked = dr["isbloked"].ToString();
                UserLog.IsOn = dr["IsOn"].ToString();


            }
            else
            {
                UserLog.OLSLOGINID = "0";
                UserLog.Encusersname = "0";
                UserLog.encpassword = "0";
                UserLog.studentid = "0";
                UserLog.studentname = "0";
                UserLog.StudentNo = "0";
                UserLog.FatherName = "0";
                UserLog.fatherphoneno = "0";
                UserLog.classid = "0";
                UserLog.sectionid = "0";
                UserLog.Session = "0";
                UserLog.PhoneNo = "0";
                UserLog.photopath = "0";
                UserLog.usertype = "0";
                UserLog.isbloked = "0";
                UserLog.IsOn = "0";
            }

            return UserLog;
        }

        public static string logOut(string UserID)
        {
            

            HttpContext.Current.Session["CID"] = null;
            HttpContext.Current.Session["UN"] = null;
            HttpContext.Current.Session["UserID"] = null;
            HttpContext.Current.Session["Login"] = null;

            return "login.aspx";
            
        }
    }
}