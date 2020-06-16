using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace EasioCore
{
    public class Question
    {
        public Int32 QNo { get; set; }
        public string QID { get; set; }
        public string SeriesIDFK { get; set; }
        public string OnlineResultIDFK { get; set; }
        public string SeriesDetailsIDFK { get; set; }
        public string ClassIDFK { get; set; }
        public string ClassName { get; set; }
        public string SubjectIDFK { get; set; }
        public string SubjectName { get; set; }
        public string TopicIDFK { get; set; }
        public string QTypeIDFK { get; set; }
        public string MCQpercentage { get; set; }
        public string Questn { get; set; }
        public string Answer { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string Marks { get; set; }
        public string CorrectAnswer { get; set; }
        public string Series { get; set; }

        public string NoOfSeries { get; set; }
        public string NoOfMCQ  { get; set; }
        public string NoOfSAQ { get; set; }
        public string NoOfLAQ { get; set; }

        public string MCQ_Marks { get; set; }
        public string SAQ_Marks { get; set; }
        public string LAQ_Marks { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Session { get; set; }
    }

    public class QuestionType
    {
        public string QTID { get; set; }
        public string Type { get; set; }
    } 

    public class OnlineResultModel
    {
        public string ERID { get; set; }
        public string SIDFK { get; set; }

        public string SeriesArray { get; set; }
        public string SubjectIDFK { get; set; }
        public string ClassIDFK { get; set; }
        public string SeriesIDFK { get; set; }
        public DateTime PublishDate { get; set; }
        public string TimeDuration { get; set; }
        public string ExamStartedAt { get; set; }
        public string TimeLeft { get; set; }
        public string LAMarks { get; set; }
        public string SAMarks { get; set; }
        public string MCMarks { get; set; }
        public string TotalMarks { get; set; }
        public string Percentage { get; set; }
        public string Result { get; set; }
    }
}