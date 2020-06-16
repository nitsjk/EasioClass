using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Nits.Common;

namespace EasioCore
{
    public class optionalSubjectBLL
    {
        public static List<OptionalSubject> getSubjectsByClassId(string classid)
        {
            List<OptionalSubject> listOpSubjects = new List<OptionalSubject>();

            DataSet dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from OptionalSubjects where ClassID=@Classid order by OptionalSubjectID", new SqlParameter("@Classid", Convert.ToInt64(classid)));

            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                listOpSubjects.Add(new OptionalSubject()
                {
                    OptionalSubjectID = Int64.Parse(dr["OptionalSubjectID"].ToString()),
                    OptionalSubjectName = dr["OptionalSubjectName"].ToString(),
                    ClassID = Int64.Parse(dr["ClassId"].ToString()),
                    SessionID = Int64.Parse(dr["SessionID"].ToString()),
                    Current_Session = dr["Current_Session"].ToString(),
                });
            }
            return listOpSubjects;
        }

        public static String insertNewSubject(OptionalSubject opSubject)
        {
            string msg = OpSubVal.getInsertSubjectValidation(opSubject);
            if (msg.ToUpper().Equals("OK"))
            {
                if (optionalSubjectBLL.checkDuplicateSubject(opSubject))
                {
                    msg = "Subject: " + opSubject.OptionalSubjectName.ToString() + " already Exists!!";
                }
                else
                {
                    SqlParameter[] param = {

                                    new SqlParameter("@subname", opSubject.OptionalSubjectName.Trim()),
                                     new SqlParameter("@classid", Convert.ToInt64(opSubject.ClassID)),
                                     new SqlParameter("@sessid",Convert.ToInt32(opSubject.SessionID.ToString()??"0")),
                                     new SqlParameter("@currentsession",opSubject.Current_Session)

                                     };
                    DataSet dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "Insert into OptionalSubjects(OptionalSubjectName,ClassID,SessionID,Current_Session) Values(@subname,@classid,@sessid,@currentsession)", param);
                    msg = "Optional Subject(s) Added Sucessfully!!";
                }
            }
            return msg;
        }

        public static String updateSubject(OptionalSubject opSubject)
        {
            string msg = "OK";// = OpSubVal.getInsertSubjectValidation(opSubject);
            if (msg.ToUpper().Equals("OK"))
            {
                SqlParameter[] param = {
                                    new SqlParameter("@subid",Convert.ToInt64(opSubject.OptionalSubjectID)),
                                    new SqlParameter("@subname", opSubject.OptionalSubjectName.Trim()),
                                     new SqlParameter("@classid", Convert.ToInt64(opSubject.ClassID)),
                                     new SqlParameter("@currentsession",opSubject.Current_Session)

                                     };
                if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Update OptionalSubjects set OptionalSubjectName=@subname where ClassID=@classid and Current_Session =@currentsession and Optionalsubjectid=@subid", param) > 0)
                {
                    msg = "Subject(s) Updated Sucessfully!!";
                }
                else
                {
                    msg = "Subject(s) Not Updated!!";
                }


            }
            return msg;
        }

        public static String deleteSubject(string id)
        {
            string msg = "";
            if (!validationBLL.IsNumber(id))
            {
                msg = "SubjectId is not number";
            }
            else
            {
                try
                {
                    SqlParameter param = new SqlParameter("@subid", Convert.ToInt64(id));

                    if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Delete from OptionalSubjects  where OptionalSubjectID=@subid", param) > 0)
                    {
                        msg = "Subject(s) Deleted Sucessfully!!";
                    }
                    else
                    {
                        msg = "Subject(s) not Deleted/Exists!!";
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.ToString();
                }
            }
            return msg;
        }

        public static bool checkDuplicateSubject(OptionalSubject opSubject)
        {
            SqlParameter[] subNames = {
             new SqlParameter("@subname", opSubject.OptionalSubjectName.Trim()),
            new SqlParameter("@classid", Convert.ToInt64(opSubject.ClassID))
            };
            if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from OptionalSubjects where ClassID=@classid and OptionalSubjectName=@subname", subNames) > 0)
                return true;
            return false;
        }
    }
}