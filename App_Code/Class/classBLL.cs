using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Nits.Common;
namespace EasioCore
{
    public class classBLL
    {
        // Get All Classes of Session

        public static List<Classes> getAllClass()
        {
            List<Classes> Cl = new List<Classes>();
            DataSet dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Classes inner join EducationalDepartments on Classes.SubDepartmentID=EducationalDepartments.EduDepartmentID order by SubDepartmentID,ClassId,ClassName ");

            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                Cl.Add(new Classes()
                {
                  ClassId=Int64.Parse(dr["ClassId"].ToString()),
                  ClassName= dr["ClassName"].ToString(),
                  ClassIncharg= dr["ClassName"].ToString(),
                  SubDepartmentID= Int64.Parse(dr["SubDepartmentID"].ToString()),
                  Current_Session= dr["Current_Session"].ToString(),
                  EduDepartmentName= dr["DepartmentName"].ToString()

                });
            }

            return Cl;
        }
        // Get Classes On Session

        public static List<Classes> getClassOnSession( string session)
        {
            List<Classes> Cl = new List<Classes>();
            SqlParameter ses = new SqlParameter("@se",session);

            DataSet dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Classes where Current_Session=@se order by SubDepartmentID,ClassId ",ses);

            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                Cl.Add(new Classes()
                {
                    ClassId = Int64.Parse(dr["ClassId"].ToString()),
                    ClassName = dr["ClassName"].ToString(),
                    ClassIncharg = dr["ClassName"].ToString(),
                    SubDepartmentID = Int64.Parse(dr["SubDepartmentID"].ToString()),
                    Current_Session = dr["Current_Session"].ToString(),
                  //  EduDepartmentName= dr["EduDepartmentName"].ToString()



                });
            }

            return Cl;
        }


        // Get Class On ClassID and SessionID

        public static Classes getClassOnClassIdSessionId(string classid,string sessionid)
        {
            List<Classes> Cl = new List<Classes>();
            SqlParameter param = new SqlParameter("@classid", classid);

            DataSet dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Classes where ClassId=@classid", param);

            DataRow dr = dss.Tables[0].Rows[0];
           return  new Classes
                {
                    ClassId = Int64.Parse(dr["ClassId"].ToString()),
                    ClassName = dr["ClassName"].ToString(),
                    ClassIncharg = dr["ClassName"].ToString(),
                    SubDepartmentID = Int64.Parse(dr["SubDepartmentID"].ToString()),
                    Current_Session = dr["Current_Session"].ToString(),
                    //  EduDepartmentName= dr["EduDepartmentName"].ToString()



                };
        }
        public static Classes getClassOnClassID(string classid)
        {
            List<Classes> Cl = new List<Classes>();
            SqlParameter param = new SqlParameter("@classid", classid);

            DataSet dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Classes where ClassId=@classid", param);

            DataRow dr = dss.Tables[0].Rows[0];
            return new Classes
            {
                ClassId = Int64.Parse(dr["ClassId"].ToString()),
                ClassName = dr["ClassName"].ToString(),
                ClassIncharg = dr["ClassName"].ToString(),
                SubDepartmentID = Int64.Parse(dr["SubDepartmentID"].ToString()),
                Current_Session = dr["Current_Session"].ToString(),
                //  EduDepartmentName= dr["EduDepartmentName"].ToString()



            };
        }
        // Add New Class
        public static string addNewClass(Classes CL)
        {
            int val = 0;
            SqlParameter[] pa = {
                new SqlParameter("@ClassIncharg",CL.ClassIncharg),
                new SqlParameter("@ClassName",CL.ClassName),
                new SqlParameter("@Current_Session",CL.Current_Session),
                new SqlParameter("@SubDepartmentID",CL.SubDepartmentID)
                 };
            string SqlQuery = "select count(*) as val from Classes where ClassName=@ClassName  and Current_Session=@Current_Session";

            string SqlInsert = "insert into Classes (ClassName,SubDepartmentID,Current_Session ) values (@ClassName,@SubDepartmentID,@Current_Session )";

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


        // Update Class
        public static string updateClass(Classes CL)
        {
            int val = 0;
            SqlParameter[] pa = {
                new SqlParameter("@ClassID",CL.ClassId),
                new SqlParameter("@ClassIncharg",CL.ClassIncharg),
                new SqlParameter("@ClassName",CL.ClassName),
                new SqlParameter("@Current_Session",CL.Current_Session),
                new SqlParameter("@SubDepartmentID",CL.SubDepartmentID)
                 };
            string SqlQuery = "select count(*) as val from Classes where ClassName=@ClassName  and Current_Session=@Current_Session  and ClassID!=@ClassID";

            string SqlUpdate = "update Classes set ClassName=@ClassName  where ClassID=@ClassID ";

            SqlDataReader dr = SqlHelper.ExecuteReader(SqlHelper.Connect, CommandType.Text, SqlQuery, pa);

            while (dr.Read())
            {
                 val =  (int)dr.GetSqlInt32(0);

                if(val>0)
                {
                    return "0";
                }
                else
                {
                    // Update Class
                    val = SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, SqlUpdate, pa );

                    if (val > 0) {
                        return "1";
                    }
                    else
                    {
                        return "-1";
                    }
                    

                }

            }


            return "Class Modify transaction failed";


        }

        // Delete Class
        public static string deleteClass(long CL)
        {
           
            int val = ClassDAL.isClassIDUsed(CL);
            if (val > 0)
            {
                return "This Class can't be deleted as it is used in other files!";
            }
            else
            {
                // Delete Query
                SqlParameter para = new SqlParameter("@CL", CL);
                string SQLRDelete = "delete from Classes where ClassID=@CL";
               int rt = SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, SQLRDelete, para);
                if (rt > 0)
                {
                    return "Class Deleted Successfully";
                }
                else
                {

                    return "Class not deleted or doesn't exist";
                }
            }
        
        }

        // Get Section List on Class ID
        public static List<Section> getAllSectionsOnCID(long CID)
        {
            SqlParameter ss = new SqlParameter("@CID",CID);
            List<Section> Sec = new List<Section>();
            DataSet dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Sections inner join Classes on Classes.ClassId=Sections.ClassId where Sections.ClassID=@CID order by SectionName ", ss);

            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                Sec.Add(new Section()
                {
                    ClassId = Int64.Parse(dr["ClassId"].ToString()),
                    ClassName = dr["ClassName"].ToString(),
                    SectionName = dr["SectionName"].ToString(),
                    SectionID = Int64.Parse(dr["SectionID"].ToString()),
                  

                });
            }

            return Sec;

        }

        // Get Section List on Section ID
        public static Section getSectionOnSecID(long CID)
        {
            SqlParameter ss = new SqlParameter("@CID", CID);
            //  List<Section> Sec = new List<Section>();
            Section Sec = new Section();
            DataSet dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Sections where SectionID=@CID", ss);

            foreach (DataRow dr in dss.Tables[0].Rows)
            {

                Sec.SectionName = dr["SectionName"].ToString();
                Sec.SectionID = Int64.Parse(dr["SectionID"].ToString());
                Sec.ClassId= Int64.Parse(dr["ClassID"].ToString());


            }

            return Sec;

        }


        // Add New Section
        public static string addNewSection(Section SC)
        {
            int val = 0;
            SqlParameter[] pa = {
                new SqlParameter("@SectionName",SC.SectionName),
                new SqlParameter("@ClassId",SC.ClassId),
                new SqlParameter("@Current_Session",SC.Current_Session)
             
                 };
            string SqlQuery = "select count(*) as val from Sections where SectionName=@SectionName  and ClassId=@ClassId";

            string SqlInsert = "insert into Sections (SectionName,ClassId,Current_Session ) values (@SectionName,@ClassId,@Current_Session )";

            int dr = (int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, SqlQuery, pa);

      
                if (dr > 0)
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

          

            return "Section not added!";
        }

        // Update Section
        public static string updateSectionName(Section SC)
        {
            int val = 0;
            SqlParameter[] pa = {
                new SqlParameter("@SectionID",SC.SectionID),
                new SqlParameter("@SectionName",SC.SectionName),
                new SqlParameter("@ClassId",SC.ClassId),
                new SqlParameter("@Current_Session",SC.Current_Session)

                 };
            string SqlQuery = "select count(*) as val from Sections where SectionName=@SectionName  and ClassId=@ClassId and SectionID!=@SectionID";

            string SqlInsert = "update  Sections set SectionName=@SectionName where SectionID=@SectionID";

            int dr = (int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, SqlQuery, pa);


            if (dr > 0)
            {
                return "0";
            }
            else
            {
                // update Section Name
                val = SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, SqlInsert, pa);

                if (val > 0)
                {
                    return "1";
                }
                else
                {
                    return "2";
                }


            }



            return "-1";
        }

        // Delete Section
        public static string deleteSection(long SecID)
        {

            int val = 0;
            SqlParameter para = new SqlParameter("@SID", SecID);
            string SQLR = "select count(*) as val from StudentInfo where SectionID=@SID;";

            // Delete Query
            string SQLRDelete = "delete from Sections where SectionID=@SID";

            SqlDataReader dr = SqlHelper.ExecuteReader(SqlHelper.Connect, CommandType.Text, SQLR, para);
          
                while (dr.Read())
                {
                    val = val + (int)dr.GetSqlInt32(0);
                 
                   
                }

            if (val > 0)
            {
                return "Section name is in use can't be deleted!";
            }
            else
            {
                // Delete from Section Table
                val = SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, SQLRDelete, para);
                if (val > 0)
                { return "Section Deleted Successfully"; }
                else
                { return "Section not deleted or doesn't exist"; }

            }
        }

        public static List<EducationalDepartment> getEducationDepartments()
        {
            List<EducationalDepartment> Cl = new List<EducationalDepartment>();
            DataSet dss = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from  EducationalDepartments ");

            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                Cl.Add(new EducationalDepartment()
                {
                    EduDepartmentID = Int64.Parse(dr["EduDepartmentID"].ToString()),
                    DepartmentName = dr["DepartmentName"].ToString(),
                    

                });
            }

            return Cl;
        }
    }
}