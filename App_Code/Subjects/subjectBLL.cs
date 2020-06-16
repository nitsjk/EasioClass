using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Nits.Common;
using Nits;
namespace EasioCore
{
    public class subjectBLL
    {
        
       
        public static List<Subject> getSubjectsByClassId(string classid)
        {
            DataSet dss = new DataSet();
            List<Subject> listSubjects = new List<Subject>();
            if (GeneralBLL.SqlHelperConnection == 1)
            {
                 dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Subjects where ClassID=@Classid order by SubjectID", new SqlParameter("@Classid", Convert.ToInt64(classid)));
            }
            else
            {
                dss = SqlHelper.ExecuteDataset(SqlHelper.Connect2, CommandType.Text, "select * from Subjects where ClassID=@Classid order by SubjectID", new SqlParameter("@Classid", Convert.ToInt64(classid)));

            }
           

            if (dss.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dss.Tables[0].Rows)
                {
                   
                        Subject subject = new Subject();
                    subject.SubjectID = Int64.Parse(dr["SubjectID"].ToString());
                    subject.SubjectName = dr["SubjectName"].ToString();
                    subject.ClassID = Int64.Parse(dr["ClassId"].ToString());
            
                    subject.Current_Session = dr["Current_Session"].ToString();
                   listSubjects.Add(subject);
                }
            }
            return listSubjects;
        }


        public static Subject getsubjectByID(string subjectid)
        {
            DataSet dss = new DataSet();
            List<Subject> sb = new List<Subject>();
            SqlParameter param = new SqlParameter("@subjectid", subjectid);
            if (GeneralBLL.SqlHelperConnection == 1)
            {
                dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Subjects where subjectid=@subjectid order by SubjectID", param);
            }
            else
            {

                dss = SqlHelper.ExecuteDataset(SqlHelper.Connect2, CommandType.Text, "select * from Subjects where subjectid=@subjectid order by SubjectID", param);
            }

            DataRow dr = dss.Tables[0].Rows[0];
            return new Subject
            {
                SubjectID = Int64.Parse(dr["SubjectID"].ToString()),
                SubjectName = dr["SubjectName"].ToString(),
               // ClassID = Int64.Parse(dr["ClassID"].ToString()),
               // SessionID = Int64.Parse(dr["SessionID"].ToString()),
               // Current_Session = dr["Current_Session"].ToString(),
                //  EduDepartmentName= dr["EduDepartmentName"].ToString()



            };
        }

        public static String insertNewSubject(Subject subject)
        {
            string msg = SubjectVal.getInsertSubjectValidation(subject);
            if (msg.ToUpper().Equals("OK"))
            {
                if (subjectBLL.checkDuplicateSubject(subject))
                {
                    msg = "0";
                }
                else
                {
                    SqlParameter[] param = {

                                    new SqlParameter("@subname", subject.SubjectName.Trim()),
                                     new SqlParameter("@classid", Convert.ToInt64(subject.ClassID)),
                                     new SqlParameter("@sessid",Convert.ToInt32(subject.SessionID.ToString()??"0")),
                                     new SqlParameter("@currentsession",subject.Current_Session)

                                     };

                    DataSet dss = new DataSet();
                    if (GeneralBLL.SqlHelperConnection == 1)
                    {
                        dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "Insert into Subjects(SubjectName,ClassID,SessionID,Current_Session) Values(@subname,@classid,@sessid,@currentsession)", param);
                        msg = "1";
                    }
                    else
                    {
                        dss = SqlHelper.ExecuteDataset(SqlHelper.Connect2, CommandType.Text, "Insert into Subjects(SubjectName,ClassID,SessionID,Current_Session) Values(@subname,@classid,@sessid,@currentsession)", param);
                        msg = "1";
                    }
                }
            }
            return msg;
        }

        public static String updateSubject(Subject subject)
        {
            string msg = SubjectVal.getInsertSubjectValidation(subject);
            if (msg.ToUpper().Equals("OK"))
            {
                SqlParameter[] param = {
            new SqlParameter("@subid",Convert.ToInt64(subject.SubjectID)),
            new SqlParameter("@subname", subject.SubjectName.Trim()),

                                    // new SqlParameter("@classid", Convert.ToInt64(subject.ClassID)),
                                    // new SqlParameter("@currentsession",subject.Current_Session)

                                     };
                if(GeneralBLL.SqlHelperConnection==1)
                { 
                if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Update Subjects set SubjectName=@subname where  subjectid=@subid", param) > 0)
                {
                    msg = "1";
                }
                else
                {
                    msg = "0";
                }
                }
                else
                {
                    if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect2, CommandType.Text, "Update Subjects set SubjectName=@subname where  subjectid=@subid", param) > 0)
                    {
                        msg = "1";
                    }
                    else
                    {
                        msg = "0";
                    }

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
                    if(GeneralBLL.SqlHelperConnection==1)
                    {
                        if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Delete from Subjects  where SubjectID=@subid", param) > 0)
                        {
                            msg = "Subject(s) Deleted Sucessfully!!";
                        }
                        else
                        {
                            msg = "Subject(s) not Deleted/Exists!!";
                        }
                    }
                   else
                    {
                        if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect2, CommandType.Text, "Delete from Subjects  where SubjectID=@subid", param) > 0)
                        {
                            msg = "Subject(s) Deleted Sucessfully!!";
                        }
                        else
                        {
                            msg = "Subject(s) not Deleted/Exists!!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.ToString();
                }
            }
            return msg;
        }

        public static bool checkDuplicateSubject(Subject subject)
        {
            SqlParameter[] subNames = {
             new SqlParameter("@subname", subject.SubjectName.Trim()),
            new SqlParameter("@classid", Convert.ToInt64(subject.ClassID))
            };
            if (GeneralBLL.SqlHelperConnection == 1)
            {
                if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from Subjects where ClassID=@classid and SubjectName=@subname", subNames) > 0)
                    return true;
                return false;
            }
            else
            {
                if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect2, CommandType.Text, "select count(*) from Subjects where ClassID=@classid and SubjectName=@subname", subNames) > 0)
                    return true;
                return false;

            }
        }
    }
}