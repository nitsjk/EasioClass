using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Nits.Common;
namespace EasioCore
{
    public class topicBLL
    {
      


        // Get Class On ClassID and SessionID

      
        public static List<Topic> getTopicOnSubjectID(string subjectID)
        {
            List<Topic> lstTopics = new List<Topic>();
            SqlParameter param = new SqlParameter("@subjectID", subjectID);

            DataSet dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Topics where subjectIDFK=@subjectID", param);

            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                lstTopics.Add(new Topic
                {
                    TID = Int64.Parse(dr["TID"].ToString()),
                    ClassIDFK = Int64.Parse(dr["ClassIdfk"].ToString()),
                    subjectIDFK = Int64.Parse(dr["subjectIDFK"].ToString()),
                    TopicName = dr["TopicName"].ToString(),
                    UserName = dr["UserName"].ToString(),



                });
            }

            return lstTopics;
        }



        public static Topic gettopicByID(string topicid)
        {
            List<Topic> sb = new List<Topic>();
            SqlParameter param = new SqlParameter("@subjectid", topicid);

            DataSet dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Topics where tid=@subjectid order by subjectIDFK", param);

            DataRow dr = dss.Tables[0].Rows[0];
            return new Topic
            {
                TID = Int64.Parse(dr["TID"].ToString()),
                ClassIDFK = Int64.Parse(dr["ClassIdfk"].ToString()),
                subjectIDFK = Int64.Parse(dr["subjectIDFK"].ToString()),
                TopicName = dr["TopicName"].ToString(),
                UserName = dr["UserName"].ToString(),



            };
        }



        // Add New Topic
        public static string addNewTopic(Topic CL)
        {
            int val = 0;
            SqlParameter[] pa = {
                new SqlParameter("@ClassIDFK",CL.ClassIDFK),
                new SqlParameter("@subjectidfk",CL.subjectIDFK),
                new SqlParameter("@TopicName",CL.TopicName),
                new SqlParameter("@username",CL.UserName)
                 };
            string SqlQuery = "select count(*) as val from Topics where subjectidfk=@subjectidfk  and TopicName=@TopicName";

            string SqlInsert = "insert into Topics (ClassIDFK,subjectIDFK,TopicName, UserName) values (@ClassIDFK,@subjectidfk,@TopicName,@username )";

            SqlDataReader dr = SqlHelper.ExecuteReader(SqlHelper.Connect, CommandType.Text, SqlQuery, pa);

            while (dr.Read())
            {
                val = (int)dr.GetSqlInt32(0);

                if (val > 0)
                {
                    return "0";
                }
                else
                {
                    // add new Class
                    val = SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, SqlInsert, pa);

                    if (val > 0)
                    {
                        return "1";
                    }
                    else
                    {
                        return "-1";
                    }


                }

            }


            return "Class not added!";
        }




        public static string updateTopic(Topic SC)
        {
            int val = 0;
            SqlParameter[] pa = {
                new SqlParameter("@TopicID",SC.TID),
                new SqlParameter("@TopicName",SC.TopicName),

                 };
           // string SqlQuery = "select count(*) as val from Sections where SectionName=@SectionName  and ClassId=@ClassId and SectionID!=@SectionID";

            string SqlInsert = "update  topics set TopicName=@TopicName where TID=@TopicID";

            //int dr = (int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, SqlQuery, pa);


            //if (dr > 0)
            //{
            //    return "0";
            //}
            //else
            //{
                // update Section Name
                val = SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, SqlInsert, pa);

                if (val > 0)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }


            //}



            return "-1";
        }


    }
}