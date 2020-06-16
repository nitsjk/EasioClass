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

    public class QuestionSeriesBLL
    {
        private static Random rng = new Random();
        public static string GetSeriesQuestions(string SeriesID)
        {
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

                int QNo = 0;
                //Long Questions
                if (longQ > 0)
                {
                    QNo = 0;
                    qs.QTypeIDFK = "3";
                    DataTable dtLongQ = getQuestionsBySeriesIDAndTypeID(qs);
                    string Qs = string.Empty;
                    if (dtLongQ.Rows.Count > 0)
                    {
                        QuestionSeries = QuestionSeries + OpenNewTable + "<tr><td><h5><strong>● LONG ANSWER QUESTIONS</strong></h5></td></tr>" + CloseNewTable;
                        QuestionSeries = QuestionSeries + OpenNewTable + "<tr><td width='100%'><hr/></td></tr>" + CloseNewTable;
                        foreach (DataRow drLQ in dtLongQ.Rows)
                        {
                            QNo++;
                            Qs = "<b>QNo." + QNo.ToString() + ") </b>" + drLQ["Question"].ToString();
                            QuestionSeries = QuestionSeries + OpenNewTable + "<tr><td>" + Qs + "</td></tr><tr><td>&nbsp;</td></tr>" + CloseNewTable;
                        }
                    }
                }

                //Short Questions
                if (shortQ > 0)
                {
                    QNo = 0;
                    qs.QTypeIDFK = "2";
                    DataTable dtSQ = getQuestionsBySeriesIDAndTypeID(qs);
                    string Qs = string.Empty;
                    if (dtSQ.Rows.Count > 0)
                    {
                        QuestionSeries = QuestionSeries + OpenNewTable + "<tr><td width='100%'><hr/></td></tr>" + CloseNewTable;
                        QuestionSeries = QuestionSeries + OpenNewTable + "<tr><td><h5><strong>● SHORT ANSWER QUESTIONS</strong></h5></td></tr>" + CloseNewTable;
                        QuestionSeries = QuestionSeries + OpenNewTable + "<tr><td width='100%'><hr/></td></tr>" + CloseNewTable;
                        foreach (DataRow drSQ in dtSQ.Rows)
                        {
                            QNo++;
                            Qs = "<b>QNo." + QNo.ToString() + ") </b>" + drSQ["Question"].ToString();
                            QuestionSeries = QuestionSeries + OpenNewTable + "<tr><td>" + Qs + "</td></tr><tr><td>&nbsp;</td></tr>" + CloseNewTable;
                        }
                    }
                }


                //MCQ Questions
                if (mcQ > 0)
                {
                    QNo = 0;
                    qs.QTypeIDFK = "1";
                    DataTable dtLongQ = getQuestionsBySeriesIDAndTypeID(qs);
                    string Qs = string.Empty;
                    if (dtLongQ.Rows.Count > 0)
                    {
                        QuestionSeries = QuestionSeries + OpenNewTable + "<tr><td width='100%'><hr/></td></tr>" + CloseNewTable;
                        QuestionSeries = QuestionSeries + OpenNewTable + "<tr><td><h5><strong>● MUTLIPLE CHOICE QUESTIONS</strong></h5><td></tr>" + CloseNewTable;
                        QuestionSeries = QuestionSeries + OpenNewTable + "<tr><td width='100%'><hr/></td></tr>" + CloseNewTable;
                        QuestionSeries = QuestionSeries + OpenNewTable;
                        foreach (DataRow drMQ in dtLongQ.Rows)
                        {
                            QNo++;
                            Qs = "<b>QNo." + QNo.ToString() + ") </b>" + drMQ["Question"].ToString();
                            QuestionSeries = QuestionSeries + "<tr><td>" + Qs + "</td></tr>";
                            QuestionSeries = QuestionSeries + "<tr><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;A) " + drMQ["OptionA"] + "</td></tr><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;B) " + drMQ["OptionB"] + "</td></tr><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;C) " + drMQ["OptionC"] + "</td></tr><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;D) " + drMQ["OptionD"] + "</td></tr><tr><td>&nbsp;</td></tr>";
                        }
                        QuestionSeries = QuestionSeries + CloseNewTable;
                        QuestionSeries = QuestionSeries + OpenNewTable + "<tr><td width='100%'><hr/></td></tr>" + CloseNewTable;
                    }
                }

            }



            return QuestionSeries;
        }

        //Get Count
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

        //Get Questions
        public static DataTable getQuestionsBySeriesID(String SeriesIDFK)
        {
            SqlParameter[] param =
             {
                    new SqlParameter("@SeriesIDFK",SeriesIDFK),
            };

            return SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select count(*)  from QuestionSeriesDetails where SeriesIDFK=@SeriesIDFK", param).Tables[0];

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


        //publish series

        public static int PublishSeries(OnlineResultModel onlineResult)
        {
            DataTable dtStd = GetStudentsByClass(onlineResult);

            String[] Series = onlineResult.SeriesArray.Split(',');
            int totalStudents = dtStd.Rows.Count;
            int RecordsUpdated = 0;
            if (totalStudents > 0)
            {
                foreach (DataRow drS in dtStd.Rows)
                {
                    String SeriesIDFK = GetRandomSeries(Series);
                    SqlParameter[] param =
                    {
                        new SqlParameter("@SeriesIDFK",SeriesIDFK),
                        new SqlParameter("@SIDFK",drS["StudentID"].ToString()),
                        new SqlParameter("@PublishDate",onlineResult.PublishDate),
                        new SqlParameter("@TimeLeft",onlineResult.TimeDuration)
                    };

                    //check if already exists
                    OnlineResultModel olr = new OnlineResultModel();
                    olr.SIDFK = drS["StudentID"].ToString();
                    olr.PublishDate = onlineResult.PublishDate;
                    olr.SeriesIDFK = SeriesIDFK;
                    if (!CheckifRecordAlreadyInserted(olr))
                    {
                        if (SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Insert into OnlineResult (SIDFK,SeriesIDFK,PublishDate,TimeLeft) values(@SIDFK,@SeriesIDFK,@PublishDate,@TimeLeft)", param) > 0)
                        {
                            RecordsUpdated++;
                        }
                    }
                }

                if (RecordsUpdated > 0)
                {
                    foreach (String serID in Series)
                    {
                        SqlParameter[] paramS =
                        {
                            new SqlParameter("@PublishDate",onlineResult.PublishDate),
                            new SqlParameter("@SeriesID",serID),
                            new SqlParameter("@TimeDuration",onlineResult.TimeDuration)
                        };
                        SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "update QuestionSeries set PublishDate=@PublishDate,TimeDuration=@TimeDuration,IsPublished=1 where SID=@SeriesID", paramS);
                    }
                }
            }

            return RecordsUpdated;
        }

        private static DataTable GetStudentsByClass(OnlineResultModel onlineResult)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@ClassID",onlineResult.ClassIDFK)
            };

            return SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "Select * from studentinfo where Classid=@Classid", param).Tables[0];
        }

        public static bool CheckifAnySeriesIsPublishedAlreadyOnDate(OnlineResultModel onlineResult)
        {
            SqlParameter[] param =
            {
                    new SqlParameter("@PublishDate",onlineResult.PublishDate),
                    new SqlParameter("@ClassIDFK",Convert.ToInt64(onlineResult.ClassIDFK)),
                    new SqlParameter("@SubjectIDFK",Convert.ToInt64(onlineResult.SubjectIDFK))
            };

            return (int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*)  from QuestionSeries where ClassIDFK=@ClassIDFK and PublishDate=@PublishDate and IsPublished=1 and SubjectIDFK=@SubjectIDFK", param) > 0 ? true : false;

        }

        public static bool CheckifRecordAlreadyInserted(OnlineResultModel onlineResult)
        {
            SqlParameter[] param =
            {
                    new SqlParameter("@PublishDate",onlineResult.PublishDate),
                    new SqlParameter("@SIDFK",onlineResult.SIDFK),
                    new SqlParameter("@SeriesIDFK",Convert.ToInt64(onlineResult.SeriesIDFK))
            };

            return (int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*)  from OnlineResult where SIDFK=@SIDFK  and PublishDate=@PublishDate and SeriesIDFK=@SeriesIDFK", param) > 0 ? true : false;

        }

        public static String GetRandomSeries(String[] listSeries)
        {
            int n = listSeries.Count();
            if (n > 1)
                return listSeries[rng.Next(n)];
            else
                return listSeries[0];
        }
    }
}