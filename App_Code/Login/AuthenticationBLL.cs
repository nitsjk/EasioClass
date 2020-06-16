using Nits;
using Nits.Common;
using Nits.ENC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class AuthenticationBLL
{
    public static string CheckAuthentication(string UserName, string Password)
    {
        string returnUrl = "none";

        DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Login where UserName='" + Encryption.Encrypt(UserName, Encryption.key) + "' AND Password='" + Encryption.Encrypt(Password, Encryption.key) + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            int type = Convert.ToInt32(dr["Type"].ToString());
            HttpContext.Current.Session["SessionClassid"] = dr["ClassID"].ToString();
            HttpContext.Current.Session["SessionUserid"] = dr["UserID"].ToString();
            if (type.Equals(0))
            {
                if(dr["IsLocked"].ToString()=="1")
                {
                    returnUrl = "../Gallery/AccountSuspended.aspx";
                }
                else
                { 
                HttpContext.Current.Session["Login"] = "student";
                returnUrl = "../Gallery/VideoGallery.aspx";
                   // returnUrl = "../Gallery/otp.aspx";
                }
            }
            else if (type.Equals(1))
            {
                HttpContext.Current.Session["Login"] = "teacher";
                returnUrl = "../Gallery/TeacherGallery.aspx";
            }
            else if (type.Equals(2))
            {
                HttpContext.Current.Session["Login"] = "admin";
                returnUrl = "../Gallery/AdminGallery.aspx";
            }
        }
        return returnUrl;
    }

    public static string loginkAuthentication(string UserName, string Password)
    {
        string returnUrl = "none";

        DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Login where UserName='" + Encryption.Encrypt(UserName, Encryption.key) + "' AND Password='" + Encryption.Encrypt(Password, Encryption.key) + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            int type = Convert.ToInt32(dr["Type"].ToString());
            HttpContext.Current.Session["SessionClassid"] = dr["ClassID"].ToString();
            HttpContext.Current.Session["SessionUserid"] = dr["UserID"].ToString();
            if (type.Equals(0))
            {
                if (dr["IsLocked"].ToString() == "1")
                {
                    returnUrl = "Gallery/AccountSuspended.aspx";
                }
                else
                {
                    HttpContext.Current.Session["Login"] = "student";
                    returnUrl = "vd.aspx";
                    // returnUrl = "Gallery/otp.aspx";
                }
            }
            else if (type.Equals(1))
            {
                HttpContext.Current.Session["Login"] = "teacher";
                returnUrl = "Gallery/TeacherGallery.aspx";
            }
            else if (type.Equals(2))
            {
                HttpContext.Current.Session["Login"] = "admin";
                returnUrl = "Gallery/AdminGallery.aspx";
            }
        }
        return returnUrl;
    }

    public static void Logout()
    {
        HttpContext.Current.Session["SessionClassid"] = null;
        HttpContext.Current.Session["SessionUserid"] = null;
        HttpContext.Current.Session["SessionSubjectname"] = null;
        HttpContext.Current.Session["SessionSubjectid"] = null;

        //  HttpContext.Current.Response.Redirect("../Login/Login.aspx");
        HttpContext.Current.Response.Redirect("../Login.aspx");

    }

    public static void Logout2(string userID)
    {
        HttpContext.Current.Session["SessionClassid"] = null;
        HttpContext.Current.Session["SessionUserid"] = null;
        HttpContext.Current.Session["SessionSubjectname"] = null;
        HttpContext.Current.Session["SessionSubjectid"] = null;

        //  HttpContext.Current.Response.Redirect("../Login/Login.aspx");
        HttpContext.Current.Response.Redirect("../Login.aspx");

    }
    public static void Logout(string filePath)
    {
        HttpContext.Current.Session["SessionClassid"] = null;
        HttpContext.Current.Session["SessionUserid"] = null;
        HttpContext.Current.Session["SessionSubjectname"] = null;
        HttpContext.Current.Session["SessionSubjectid"] = null;

        HttpContext.Current.Response.Redirect(filePath);

    }

    public static bool IsAuthenticated(string val)
    {
        if(HttpContext.Current.Session["Login"]!=null && HttpContext.Current.Session["Login"]!="")
        {
            return (HttpContext.Current.Session["Login"].ToString().ToLower().Equals(val)) ? true : false;
        }
        else
        {
            HttpContext.Current.Response.Redirect("../Login/Login.aspx");
            return (HttpContext.Current.Session["Login"].ToString().ToLower().Equals(val)) ? true : false;
        }
        
    }

    public static bool CheckIfUserAlreadyExists(UserModel user)
    {

        DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Login where UserName='" + Encryption.Encrypt(user.Username, Encryption.key) + "' AND Password='" + Encryption.Encrypt(user.Password, Encryption.key) + "'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            return true;
        }
        else
        {
            SqlParameter[] param =
            {
                new SqlParameter("@username",Encryption.Encrypt(user.Username, Encryption.key)),
                new SqlParameter("@password",Encryption.Encrypt(user.Password, Encryption.key)),
                new SqlParameter("@type",user.UserType),
                new SqlParameter("@classid",user.ClassID)
            };
            SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "Insert into Login(username,password,type,classid,subjectid,isactive)values(@username,@password,@type,@classid,0,1)", param);

            //if (user.UserType.Equals("1"))
            //{
            //    foreach (TeacherSubjects tc in user.TchrSubjects)
            //    {
            //        SqlParameter[] parm =
            //        {
            //            new SqlParameter("@UIDFK",GetUserID(user)),
            //            new SqlParameter("@Name",user.Username),
            //            new SqlParameter("@ClassID",tc.ClassID),
            //            new SqlParameter("@SubjectIDs",tc.SubjectIDs)
            //        };
            //        SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Insert into Teachers(UIDFK,Name,ClassID,SubjectIDs)Values(@UIDFK,@Name,@ClassID,@SubjectIDs)", parm);
            //    }
            //}
            return false;
        }
    }

    private static string GetUserID(UserModel user)
    {
        return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select UserID from Login where UserName='" + Encryption.Encrypt(user.Username, Encryption.key) + "' AND Password='" + Encryption.Encrypt(user.Password, Encryption.key) + "'").ToString();
    }

    public static List<UserModel> GetUsers()
    {
        List<UserModel> listUsers = new List<UserModel>();
        DataTable dtUsers = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Login").Tables[0];

        foreach (DataRow dr in dtUsers.Rows)
        {
            UserModel user = new UserModel();
            user.UserID = dr["UserID"].ToString();
            user.Username = Encryption.Decrypt(dr["UserName"].ToString(), Encryption.key);
            user.Password = Encryption.Decrypt(dr["Password"].ToString(), Encryption.key);
            user.UserType = dr["Type"].ToString();
            user.ClassID = dr["ClassID"].ToString();
            listUsers.Add(user);
        }
        return listUsers;
    }

    public static bool UpdateUser(UserModel user)
    {
        try
        {
            SqlParameter[] param =
           {
                new SqlParameter("@Userid",user.UserID),
                new SqlParameter("@username",Encryption.Encrypt(user.Username, Encryption.key)),
                new SqlParameter("@password",Encryption.Encrypt(user.Password, Encryption.key)),
                new SqlParameter("@type",user.UserType),
                new SqlParameter("@classid",user.ClassID)
            };

            if (user.setPassword)
            {
                SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "update Login set Password=@password,Classid=@classid,type=@type where UserID=@UserID", param);
            }
            else
            {
                SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "update Login set Classid=@classid,type=@type where UserID=@UserID", param);
            }


            if (user.UserType.Equals("1"))
            {
                foreach (TeacherSubjects tc in user.TchrSubjects)
                {
                    SqlParameter[] parm =
                    {
                        new SqlParameter("@UIDFK",tc.UIDFK),
                        new SqlParameter("@Name",user.Username),
                        new SqlParameter("@ClassID",tc.ClassID),
                        new SqlParameter("@SubjectIDs",tc.SubjectIDs)
                    };

                    if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from Teachers where UIDFK=@UIDFK and ClassID=@ClassID", parm) > 0)
                    {
                        SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "Update Teachers set SubjectIDs=@SubjectIDs where UIDFK=@UIDFK and ClassID=@ClassID", parm);
                    }
                    else
                    {

                        SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Insert into Teachers(UIDFK,Name,ClassID,SubjectIDs)Values(@UIDFK,@Name,@ClassID,@SubjectIDs)", parm);
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static string changePassword(UserModel user)
    {
        try
        {
            SqlParameter[] param =
           {
                new SqlParameter("@Userid",user.UserID),
                new SqlParameter("@password",Encryption.Encrypt(user.Password, Encryption.key)),
                new SqlParameter("@NewPassWord",Encryption.Encrypt(user.Username, Encryption.key)),
               
            };

            Int64 validation = (Int64)SqlHelper.ExecuteScalar(SqlHelper.Connect,CommandType.Text, "select  ISNULL( max(UserID),0) from Login where UserID=@Userid and Password=@password ", param);

            if (validation== Int64.Parse( user.UserID))
            {
              int x=  SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "update Login set Password=@NewPassWord where UserID=@UserID", param);
                if(x>0)
                {
                    return "1";
                }
                else
                {
                    return "-1";
                }
            }
            else
            {
                return "0";
            }

            
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
}