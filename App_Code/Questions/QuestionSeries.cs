using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasioCore
{
    public class QuestionSeries
    {
        public string SID { get; set; }
        public string ClassIDFK { get; set; }
        public string SubjectIDFK { get; set; }
        public string TopicIDFK { get; set; }
        public string Series { get; set; }
        public string MCQMarks { get; set; }
        public string LAQMarks { get; set; }
        public string SAQMarks { get; set; }
        public string IsPublished { get; set; }
        public string PublishDate { get; set; }
        public string IsDeleted { get; set; }
        public string CreatedOn { get; set; }
        public string Username { get; set; }
        public string Session { get; set; }

    } 

    public class QuestionSeriesDetail
    {
        public string SIDT { get; set; }
        public string SeriesIDFK { get; set; }

        public string QIDFK { get; set; }
        public string QtypeIDFK { get; set; }
        public string Question { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
    }
}