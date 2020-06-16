using Nits;
using Nits.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace EasioCore
{
    public class ExamBLL
    {
        public static DataTable GetStudentDetails(ExamModel emodel)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@SID",emodel.SID),
                new SqlParameter("@CID",emodel.CID)
            };
            return SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select Students.*,Classes.ClassName,sections.SectionName,studentinfo.* from StudentInfo inner join students on students.StudentID=StudentInfo.StudentId inner join Classes on Classes.ClassId=StudentInfo.ClassID inner join Sections on Sections.SectionID=StudentInfo.SectionID where StudentInfo.ClassID=@CID and StudentInfo.StudentId=@SID",param).Tables[0];
        }

        public static DataTable GetAllSeriesByClass(ExamModel emodel)
        {
            SqlParameter[] param =
           {
                new SqlParameter("@SID",Convert.ToInt64(emodel.SID)),
                new SqlParameter("@CID",Convert.ToInt64(emodel.CID))
            };

            return SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select OnlineResult.*,QuestionSeries.*,Subjects.* from OnlineResult inner join QuestionSeries on QuestionSeries.SID=OnlineResult.SeriesIDFK inner join Subjects on Subjects.SubjectID=QuestionSeries.SubjectIDFK  where OnlineResult.SIDFK=@SID and QuestionSeries.ClassIDFK=@CID and QuestionSeries.IsPublished=1 order by QuestionSeries.PublishDate desc", param).Tables[0];
        }

        public static DataSet GetSeriesQuestions(string SeriesID)
        {
            DataSet ds = new DataSet();
            DataTable dtTopInfo = new DataTable();
            dtTopInfo.Columns.Add("Info", typeof(string));
            
            Question qs = new Question();
            String QuestionSeries = string.Empty;
            string schoolName = GeneralBLL.SchoolName();
            string OpenNewTable = "<table width='100%' style='color:black;font-size:16px;' border='0' >";
            string circular = "<b style='border:solid 1px black;padding:2px;paddding-left:12px;padding-right:12px;border-radius:50px;;text-alignment:center;'>&nbsp;";
            string close_circular = "</b>";

            string CloseNewTable = "</table>";

            int longQ, shortQ, mcQ;
            longQ = shortQ = mcQ = 0;

            DataTable dtQ = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select *  from QuestionSeries inner join Classes on Classes.ClassId=QuestionSeries.ClassIDFK inner join subjects on QuestionSeries.SubjectIDFK=Subjects.SubjectID where SID =@SID", new SqlParameter("@SID", SeriesID)).Tables[0];

            if (dtQ.Rows.Count > 0)
            {
                DataRow dr = dtQ.Rows[0];
                qs.LAQ_Marks = dr["LAQMarks"].ToString();
                qs.SAQ_Marks = dr["SAQMarks"].ToString();
                qs.MCQ_Marks = dr["MCQMarks"].ToString();
                qs.ClassName = dr["ClassName"].ToString();
                qs.ClassIDFK = dr["ClassIDFK"].ToString();
                qs.SeriesIDFK = dr["SID"].ToString();
                qs.SubjectName = dr["SubjectName"].ToString();
                qs.Series = dr["Series"].ToString();


                qs.QTypeIDFK = "3";
                longQ = getQuestionsCountBySeriesIDAndTypeID(qs);
                qs.QTypeIDFK = "2";
                shortQ = getQuestionsCountBySeriesIDAndTypeID(qs);
                qs.QTypeIDFK = "1";
                mcQ = getQuestionsCountBySeriesIDAndTypeID(qs);

                QuestionSeries = OpenNewTable + "<tr><td><h2 style='text-align:center;font-weight:bold;'>" + schoolName + "</h2></td></tr><tr><td><h3 style='text-align:center;font-weight:bold;'>ONLINE EXAMINATION</h3><h5 style='text-align:center;font-weight:bold;'>(" + qs.ClassName + " | " + qs.SubjectName + " | Series-" + qs.Series + ")</h5></td></tr>" + CloseNewTable;

                QuestionSeries = QuestionSeries + OpenNewTable + "<tr><td width='100%'><hr/><td></tr>" + CloseNewTable;

                QuestionSeries = QuestionSeries + OpenNewTable;

                if (longQ > 0)
                    QuestionSeries = QuestionSeries + "<tr style='font-weight:bold;height:33px;'><td>Long Answer Questions : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + circular + longQ.ToString() + close_circular + "</td><td style='text-align:right;margin:10px;'>Max. Marks " + circular + qs.LAQ_Marks + close_circular + "</td>";

                if (shortQ > 0)
                    QuestionSeries = QuestionSeries + "<tr style='font-weight:bold;height:33px;'><td>Short Answer Questions :&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + circular + shortQ.ToString() + close_circular + "</td><td style='text-align:right;margin:10px;'>Max. Marks " + circular + qs.SAQ_Marks + close_circular + "</td></tr>";

                if (mcQ > 0)
                    QuestionSeries = QuestionSeries + "<tr style='font-weight:bold;height:33px;'><td>Multiple Choice Questions : " + circular + mcQ.ToString() + close_circular + "</td><td style='text-align:right;margin:10px;'>Max. Marks " + circular + qs.MCQ_Marks + close_circular + "</td></tr>";

                QuestionSeries = QuestionSeries + CloseNewTable;

                QuestionSeries = QuestionSeries + OpenNewTable + "<tr><td width='100%'><hr/><td></tr>" + CloseNewTable;

                DataRow drInfo = dtTopInfo.NewRow();
                drInfo["Info"] = QuestionSeries;

                dtTopInfo.Rows.Add(drInfo);

                ds.Tables.Add(dtTopInfo);

             
                

            }

            return ds;
        }

        public static int getQuestionsCountBySeriesID(String SeriesIDFK)
        {
            SqlParameter[] param =
             {
                    new SqlParameter("@SeriesIDFK",SeriesIDFK),
            };

            return (int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*)  from QuestionSeriesDetails where SeriesIDFK=@SeriesIDFK", param);

        }

        public static int getQuestionsCountBySeriesIDAndTypeID(Question question)
        {
            List<Question> lstQuestion = new List<Question>();
            SqlParameter[] param =
             {
                    new SqlParameter("@SeriesIDFK",question.SeriesIDFK),
                     new SqlParameter("@QTypeIDFK",question.QTypeIDFK)
            };

            return (int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*)  from QuestionSeriesDetails where SeriesIDFK=@SeriesIDFK and QtypeIDFK=@QtypeIDFK", param);

        }

        public static DataTable getQuestionsBySeriesIDAndTypeID(Question question)
        {
            List<Question> lstQuestion = new List<Question>();
            SqlParameter[] param =
             {
                    new SqlParameter("@SeriesIDFK",question.SeriesIDFK),
                     new SqlParameter("@QTypeIDFK",question.QTypeIDFK)
            };

            return SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select *  from QuestionSeriesDetails where SeriesIDFK=@SeriesIDFK and QtypeIDFK=@QtypeIDFK", param).Tables[0];

        }

        public static string GetRemainingTime(string ERID)
        {
            return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select TimeLeft  from OnlineResult where ERID=@ERID", new SqlParameter("@ERID",ERID)).ToString();
        }

        public static void UpdateRemainingTime(string ERID)
        {
             SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Update OnlineResult set TimeLeft=IIF(TimeLeft-1<=0,0,TimeLeft-1),ExamStartedAt=IIF(ExamStartedAt is null,(Select RIGHT(CONVERT(VARCHAR, GETDATE(), 100),7)),ExamStartedAt)   where ERID=@ERID", new SqlParameter("@ERID", ERID));
        }

        public static string GetLongQuestionsMaxMarks(string SeriesID)
        {
            return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select LAQMarks from QuestionSeries where SID=@SID", new SqlParameter("@SID", SeriesID)).ToString();
        }

        public static string GetShortQuestionsMaxMarks(string SeriesID)
        {
            return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select SAQMarks from QuestionSeries where SID=@SID", new SqlParameter("@SID", SeriesID)).ToString();
        }

        public static string GetMCQMaxMarks(string SeriesID)
        {
            return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select MCQMarks from QuestionSeries where SID=@SID", new SqlParameter("@SID", SeriesID)).ToString();
        }

        public static void InsertAnswerDetails(List<Question> lstQuestion)
        {
           foreach(Question qs in lstQuestion)
            {
                SqlParameter[] param =
                {
                    new SqlParameter("@EIDFK",qs.OnlineResultIDFK),
                    new SqlParameter("@SerDIDFK",qs.SeriesDetailsIDFK),
                    new SqlParameter("@Answer",qs.Answer)
                };
                SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "insert into OnlineStudentAnswers(EIDFK,SeriesDetailIDFK,Answer) values(@EIDFK,@SerDIDFK,@Answer)",param);
            }
        }

        public static void UpdateOnlineResultDetails(Question qs)
        {
            SqlParameter[] param =
                {
                    new SqlParameter("@EIDFK",qs.OnlineResultIDFK),
                    new SqlParameter("@MCQMarks",qs.MCQ_Marks),
                    new SqlParameter("@Percentage",qs.MCQpercentage)
                };
            SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "update onlineresult set MCMarks=@MCQMarks,Percentage=@Percentage,ExamTaken=1 where ERID=@EIDFK", param);
        }
    } 



    public class ExamModel
    {
        public string SID { get; set; }
        public string  CID { get; set; }
        public string   SectionID { get; set; }
        public string StudentName { get; set; }
        public string SectionName { get; set; }
        public string ClassName { get; set; }
    }
}