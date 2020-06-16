using Nits.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace EasioCore
{
    public class QuestionBLL
    {
        private static Random rng = new Random();
        public static List<QuestionType> getQuestionType()
        {
            List<QuestionType> lstQType = new List<QuestionType>();

            DataSet dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from QuestionType where IsDeleted=0");

            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                lstQType.Add(new QuestionType
                {
                    QTID = dr["QTID"].ToString(),
                    Type = dr["Type"].ToString(),

                });
            }

            return lstQType;
        }

        public static Question getQuestionByQID(string QID)
        {
            Question qs = new Question();

            DataTable dtQ = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Questions where QID =@QID", new SqlParameter("@QID", QID)).Tables[0];

            DataRow dr = dtQ.Rows[0];

            qs.Questn = dr["Question"].ToString();
            qs.OptionA = dr["OptionA"].ToString();
            qs.OptionB = dr["OptionB"].ToString();
            qs.OptionC = dr["OptionC"].ToString();
            qs.OptionD = dr["OptionD"].ToString();
            qs.CorrectAnswer = dr["CorrectAnswer"].ToString();

            return qs;

        }

        public static void updateQuestionByQID(Question question)
        {
            SqlParameter[] param =
            {
                    new SqlParameter("@QID",question.QID),
                    new SqlParameter("@Question",question.Questn),
                    new SqlParameter("@OptionA",question.OptionA),
                    new SqlParameter("@OptionB",question.OptionB),
                    new SqlParameter("@OptionC",question.OptionC),
                    new SqlParameter("@OptionD",question.OptionD),
                    new SqlParameter("@CorrectAnswer",question.CorrectAnswer),
            };

            SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Update Questions set Question =@Question, OptionA=@OptionA, OptionB=@OptionB, OptionC=@OptionC, OptionD=@OptionD, CorrectAnswer=@CorrectAnswer where QID=@QID;Update QuestionSeriesDetails set Question =@Question, OptionA=@OptionA, OptionB=@OptionB, OptionC=@OptionC, OptionD=@OptionD, CorrectAnswer=@CorrectAnswer where QIDFK=@QID and (select Ispublished from QuestionSeries where SID=(select SeriesIDFK from QuestionSeriesDetails where QIDFK=@QID))=0", param);
        }

        public static string AddQuestion(Question question)
        {
            SqlParameter[] param =
            {
                    new SqlParameter("@QID",question.QID),
                    new SqlParameter("@ClassIDFK",question.ClassIDFK),
                    new SqlParameter("@SubjectIDFK",question.SubjectIDFK),
                    new SqlParameter("@TopicIDFK",question.TopicIDFK),
                    new SqlParameter("@QTypeIDFK",question.QTypeIDFK),
                    new SqlParameter("@Question",question.Questn),
                    new SqlParameter("@OptionA",question.OptionA),
                    new SqlParameter("@OptionB",question.OptionB),
                    new SqlParameter("@OptionC",question.OptionC),
                    new SqlParameter("@OptionD",question.OptionD),
                    new SqlParameter("@CorrectAnswer",question.CorrectAnswer),
                    new SqlParameter("@Series",question.Series),
                    new SqlParameter("@Marks",question.Marks),
                    new SqlParameter("@UserName",question.UserName),
                    new SqlParameter("@CreatedOn",DateTime.Now.Date)
            };

            if (CheckIfQuestionAlreadyExists(question))
            {
                return "Question already exists!";
            }
            else
            {
                SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Insert into Questions(ClassIDFK, SubjectIDFK, TopicIDFK, QTypeIDFK, Question, OptionA, OptionB, OptionC, OptionD, CorrectAnswer, UserName, CreatedOn)values(@ClassIDFK, @SubjectIDFK, @TopicIDFK, @QTypeIDFK, @Question, @OptionA, @OptionB, @OptionC, @OptionD, @CorrectAnswer, @UserName, @CreatedOn)", param);

                return "Added sucessfully!";
            }


        }

        public static DataTable getAddedQuestions(Question question)
        {
            List<Question> lstQuestion = new List<Question>();
            SqlParameter[] param =
             {
                    new SqlParameter("@ClassIDFK",question.ClassIDFK),
                    new SqlParameter("@SubjectIDFK",question.SubjectIDFK),
                    new SqlParameter("@TopicIDFK",question.TopicIDFK),
                    new SqlParameter("@QTypeIDFK",question.QTypeIDFK),
            };

            return SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Questions where subjectIDFK=@subjectIDFK and ClassIDFK=@ClassIDFK and TopicIDFK=@TopicIDFK and QtypeIDFK=@QTypeIDFK", param).Tables[0];

        }
        public static bool CheckIfQuestionAlreadyExists(Question question)
        {
            SqlParameter[] param =
           {
                    new SqlParameter("@ClassIDFK",question.ClassIDFK),
                    new SqlParameter("@SubjectIDFK",question.SubjectIDFK),
                    new SqlParameter("@TopicIDFK",question.TopicIDFK),
                    new SqlParameter("@QTypeIDFK",question.QTypeIDFK),
                    new SqlParameter("@Question",question.Questn.Trim())
            };

            if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from Questions where subjectIDFK=@subjectIDFK and ClassIDFK=@ClassIDFK and TopicIDFK=@TopicIDFK and QtypeIDFK=@QTypeIDFK and Question=@Question", param) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckIfMarksRecordAlreadyExists(Question question)
        {
            SqlParameter[] param =
           {
                    new SqlParameter("@ClassIDFK",question.ClassIDFK),
                    new SqlParameter("@SubjectIDFK",question.SubjectIDFK),
                    new SqlParameter("@TopicIDFK",question.TopicIDFK),
                    new SqlParameter("@QTypeIDFK",question.QTypeIDFK),
                    new SqlParameter("@Question",question.Questn.Trim())
            };

            if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from TopicMarks where subjectIDFK=@subjectIDFK and ClassIDFK=@ClassIDFK and TopicIDFK=@TopicIDFK and QtypeIDFK=@QTypeIDFK", param) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Question Series

        public static void GenerateSeries(Question qSeries)
        {
            if (!checkIfSeriesAlreadyExists(qSeries))
            {
                SqlParameter[] param =
                    {
                    new SqlParameter("@ClassIDFK", qSeries.ClassIDFK),
                    new SqlParameter("@SubjectIDFK", qSeries.SubjectIDFK),
                    new SqlParameter("@TopicIDFK", qSeries.TopicIDFK),
                    new SqlParameter("@Series", qSeries.Series),
                    new SqlParameter("@MCQMarks", qSeries.MCQ_Marks),
                    new SqlParameter("@LAQMarks", qSeries.LAQ_Marks),
                    new SqlParameter("@SAQMarks", qSeries.SAQ_Marks),
                    new SqlParameter("@CreatedOn", DateTime.Now.Date),
                    new SqlParameter("@Username", qSeries.UserName),
                    new SqlParameter("@Session", qSeries.Session)
                    };
                if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Insert into QuestionSeries(ClassIDFK,SubjectIDFK,TopicIDFK,Series,MCQMarks,LAQMarks,SAQMarks,CreatedOn,Username,Session) values(@ClassIDFK,@SubjectIDFK,@TopicIDFK,@Series,@MCQMarks,@LAQMarks,@SAQMarks,@CreatedOn,@Username,@Session)", param) > 0)
                {
                    Int64 SeriesIDFK = GetSeriesID(qSeries);
                    qSeries.SeriesIDFK = SeriesIDFK.ToString();
                    // Series 

                    List<QuestionSeriesDetail> newSeries = new List<QuestionSeriesDetail>();

                    //Long Answers
                    if (Convert.ToInt32(qSeries.NoOfLAQ) > 0)
                    {


                        qSeries.QTypeIDFK = QTYPEENUM.LONG_ANSWER;

                        List<Question> listLongAnswerQs = GetQuestions(qSeries);
                        AddRandomQuestions(listLongAnswerQs, newSeries, qSeries);


                    }

                    //Short Answers
                    if (Convert.ToInt32(qSeries.NoOfSAQ) > 0)
                    {
                        qSeries.QTypeIDFK = QTYPEENUM.SHORT_ANSWER;
                        List<Question> listShortAnswerQs = GetQuestions(qSeries);
                        AddRandomQuestions(listShortAnswerQs, newSeries, qSeries);
                    }

                    //MCQ Answers
                    if (Convert.ToInt32(qSeries.NoOfMCQ) > 0)
                    {
                        qSeries.QTypeIDFK = QTYPEENUM.MCQ;
                        List<Question> listMCQAnswerQs = GetQuestions(qSeries);
                        AddRandomQuestions(listMCQAnswerQs, newSeries, qSeries);
                    }

                    if (newSeries.Count > 0)
                    {
                        foreach (QuestionSeriesDetail qDetails in newSeries)
                        {
                            SqlParameter[] parm =
                            {
                                new SqlParameter("@SerIDFK",qDetails.SeriesIDFK),
                                new SqlParameter("@QIDFK",qDetails.QIDFK),
                                new SqlParameter("@QtypeIDFK",qDetails.QtypeIDFK),
                                new SqlParameter("@Question",qDetails.Question),
                                new SqlParameter("@OptionA",qDetails.OptionA),
                                new SqlParameter("@OptionB",qDetails.OptionB),
                                new SqlParameter("@OptionC",qDetails.OptionC),
                                new SqlParameter("@OptionD",qDetails.OptionD),
                                new SqlParameter("@CorrectAnswer",qDetails.CorrectAnswer),
                            };

                            SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "insert into QuestionSeriesDetails(SeriesIDFK,QIDFK,QtypeIDFK,Question,OptionA,OptionB,OptionC,OptionD,CorrectAnswer) values(@SerIDFK,@QIDFK,@QtypeIDFK,@Question,@OptionA,@OptionB,@OptionC,@OptionD,@CorrectAnswer)", parm);
                        }
                    }

                }

            }

        }

        public static void DeleteSeriesWithQuestions(long SID)
        {
            SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Delete from Questionseries where SID=@SID;Delete from QuestionSeriesDetails where SeriesIDFK=@SID", new SqlParameter("@SID",SID));
        }

        public static DataTable GetSeries(long SeriesID)
        {


            return SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select Students.*,Classes.ClassName,sections.SectionName,studentinfo.* from StudentInfo inner join students on students.StudentID=StudentInfo.StudentId inner join Classes on Classes.ClassId=StudentInfo.ClassID inner join Sections on Sections.SectionID=StudentInfo.SectionID where StudentInfo.ClassID=@CID and StudentInfo.StudentId=@SID", new SqlParameter("@SID",SeriesID)).Tables[0];


        }

        public static string GetTopicNamesByTopicIDs(string topics)
        {
            string topicnames = string.Empty;
            DataTable dtTopics= SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select TopicName from Topics where  TID in (select item from dbo.SplitString(@TopicIDFK))", new SqlParameter("@TopicIDFK",topics)).Tables[0];

            if (dtTopics.Rows.Count > 0)
            {
                foreach(DataRow dr in dtTopics.Rows)
                {
                    topicnames = topicnames + dr["TopicName"].ToString() + ",";
                }

                if (topicnames.Length > 0)
                    topicnames = topicnames.Substring(0, topicnames.Length - 1);
            }

            return topicnames;
        }



        public static DataTable GetAllSeriesByClassAndSubject(Question qs)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@SubjectID",qs.SubjectIDFK),
                new SqlParameter("@ClassID",qs.ClassIDFK)
            };

            return SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select QuestionSeries.*,Classes.ClassName,Subjects.SubjectName from QuestionSeries inner join Classes on Classes.ClassId=QuestionSeries.ClassIDFK inner join Subjects on Subjects.SubjectID=QuestionSeries.SubjectIDFK where QuestionSeries.ClassIDFK=@ClassID and QuestionSeries.SubjectIDFK=@SubjectID",param).Tables[0];
        }

 

        public static DataTable GetAllSeries()
        {
            return SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select QuestionSeries.*,Classes.ClassName,Subjects.SubjectName from QuestionSeries inner join Classes on Classes.ClassId=QuestionSeries.ClassIDFK inner join Subjects on Subjects.SubjectID=QuestionSeries.SubjectIDFK order by Classes.ClassID").Tables[0];
        }

        //Get Questions
        private static List<Question> GetQuestions(Question question)
        {
            List<Question> lstQuestion = new List<Question>();
            SqlParameter[] param =
             {
                    new SqlParameter("@ClassIDFK",question.ClassIDFK),
                    new SqlParameter("@SubjectIDFK",question.SubjectIDFK),
                    new SqlParameter("@TopicIDFK",question.TopicIDFK),
                    new SqlParameter("@QTypeIDFK",question.QTypeIDFK)
            };

            DataTable dtQuestions = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Questions where subjectIDFK=@subjectIDFK and ClassIDFK=@ClassIDFK and TopicIDFK in (select Cast(item as bigint) from dbo.SplitString(@TopicIDFK)) and QTypeIDFK=@QTypeIDFK order by QID", param).Tables[0];

            if (dtQuestions.Rows.Count > 0)
            {
                int qno = 1;
                foreach (DataRow dr in dtQuestions.Rows)
                {
                    Question qs = new Question();
                    qs.QNo = qno;
                    qs.QID = dr["QID"].ToString();
                    qs.Questn = dr["Question"].ToString();
                    qs.OptionA = dr["OptionA"].ToString();
                    qs.OptionB = dr["OptionB"].ToString();
                    qs.OptionC = dr["OptionC"].ToString();
                    qs.OptionD = dr["OptionD"].ToString();
                    qs.CorrectAnswer = dr["CorrectAnswer"].ToString();
                    lstQuestion.Add(qs);
                    qno++;
                }
            }

            return lstQuestion;
        }

        //end

        //Generation of Series
        private static void AddRandomQuestions(List<Question> lstQuestions, List<QuestionSeriesDetail> newSeries, Question qParam)
        {


            Int32 totalQs = 0;
            if (qParam.QTypeIDFK.Equals(QTYPEENUM.LONG_ANSWER))
                totalQs = Convert.ToInt32(qParam.NoOfLAQ);
            else if (qParam.QTypeIDFK.Equals(QTYPEENUM.SHORT_ANSWER))
                totalQs = Convert.ToInt32(qParam.NoOfSAQ);
            else if (qParam.QTypeIDFK.Equals(QTYPEENUM.MCQ))
                totalQs = Convert.ToInt32(qParam.NoOfMCQ);

            //Shuffle Questions for series
            ShuffleQuestions(lstQuestions);

            for (int i = 0; i < totalQs; i++)
            {


                //Add Question 

                QuestionSeriesDetail qs = new QuestionSeriesDetail();

                qs.QtypeIDFK = qParam.QTypeIDFK;
                qs.SeriesIDFK = qParam.SeriesIDFK;
                qs.QIDFK = lstQuestions[i].QID;
                qs.Question = lstQuestions[i].Questn;
                qs.OptionA = lstQuestions[i].OptionA;
                qs.OptionB = lstQuestions[i].OptionB;
                qs.OptionC = lstQuestions[i].OptionC;
                qs.OptionD = lstQuestions[i].OptionD;
                qs.CorrectAnswer = lstQuestions[i].CorrectAnswer;

                newSeries.Add(qs);
                //
            }
        }

        //end
        private static long GetSeriesID(Question question)
        {
            SqlParameter[] param =
           {
                    new SqlParameter("@ClassIDFK",question.ClassIDFK),
                    new SqlParameter("@SubjectIDFK",question.SubjectIDFK),
                    new SqlParameter("@TopicIDFK",question.TopicIDFK),
                    new SqlParameter("@Series",question.Series.Trim())
            };

            return (Int64)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select SID from QuestionSeries where subjectIDFK=@subjectIDFK and ClassIDFK=@ClassIDFK and TopicIDFK=@TopicIDFK and Series=@Series", param);
        }

        //check
        private static bool checkIfSeriesAlreadyExists(Question question)
        {
            SqlParameter[] param =
            {
                    new SqlParameter("@ClassIDFK",question.ClassIDFK),
                    new SqlParameter("@SubjectIDFK",question.SubjectIDFK),
                    new SqlParameter("@TopicIDFK",question.TopicIDFK),
                    new SqlParameter("@Series",question.Series.Trim())
            };

            if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from QuestionSeries where subjectIDFK=@subjectIDFK and ClassIDFK=@ClassIDFK and TopicIDFK=@TopicIDFK and Series=@Series", param) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Get Question count
        public static int getQuestionsCountByTopicID(Question question)
        {
            List<Question> lstQuestion = new List<Question>();
            SqlParameter[] param =
             {
                    new SqlParameter("@ClassIDFK",question.ClassIDFK),
                    new SqlParameter("@SubjectIDFK",question.SubjectIDFK),
                    new SqlParameter("@TopicIDFK",question.TopicIDFK),
                    new SqlParameter("@QTypeIDFK",question.QTypeIDFK)
            };

            return (int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from Questions where subjectIDFK=@subjectIDFK and ClassIDFK=@ClassIDFK and TopicIDFK in (select item from dbo.SplitString(@TopicIDFK)) and QTypeIDFK=@QTypeIDFK", param);

        }

        public static int getQuestionsCountBySubjectID(Question question)
        {
            List<Question> lstQuestion = new List<Question>();
            SqlParameter[] param =
             {
                    new SqlParameter("@ClassIDFK",question.ClassIDFK),
                    new SqlParameter("@SubjectIDFK",question.SubjectIDFK),
                     new SqlParameter("@QTypeIDFK",question.QTypeIDFK)
            };

            return (int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from Questions where subjectIDFK=@subjectIDFK and ClassIDFK=@ClassIDFK and QTypeIDFK=@QTypeIDFK", param);

        }

        public static void ShuffleQuestions(List<Question> listQuestions)
        {
            int n = listQuestions.Count;
            Question qs = new Question();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                qs = listQuestions[k];
                listQuestions[k] = listQuestions[n];
                listQuestions[n] = qs;
            }
        }

    }

    public class QTYPEENUM
    {
        public static string MCQ = "1";
        public static string SHORT_ANSWER = "2";
        public static string LONG_ANSWER = "3";
    }
}