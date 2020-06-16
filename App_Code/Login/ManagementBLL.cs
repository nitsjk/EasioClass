using Nits.Common;
using Nits.ENC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Nits
{
    public class ManagementBLL
    {
        public static DataTable GetClasses()
        {
            return SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from classes").Tables[0];
        }

        public static string AddNewClass(string ClassName)
        {
            if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from Classes where classname =@classname", new SqlParameter("@classname", ClassName)) > 0)
            {
                return "Class Already Exists";
            }
            else
            {
                SqlParameter[] param =
                {
                    new SqlParameter("@classname",ClassName),
                    new SqlParameter("@Classid",GetMaxClassID())
                };
                if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Insert into classes(ClassID,ClassName) values(@ClassId,@className) ", param) > 0)
                {
                    return "Data Inserted Sucessfully!";
                }
                else
                {
                    return "Insert Record Failed!";
                }
            }
        }

        public static DataTable GetSubjects()
        {
            return SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select classes.*,subjects.* from classes inner join subjects on subjects.classid=classes.classid").Tables[0];
        }

        public static string AddNewSection(string SectionName, string ClassID)
        {
            SqlParameter[] parm =
               {
                    new SqlParameter("@SectionName",SectionName),
                    new SqlParameter("@Classid",ClassID)
                };
            if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from Sections where SectionName =@SectionName and ClassId=@Classid", parm) > 0)
            {
                return "Section Already Exists";
            }
            else
            {
                SqlParameter[] param =
                {
                    new SqlParameter("@sectionname",SectionName),
                    new SqlParameter("@classid",ClassID),
                    new SqlParameter("@sectionid",GetMaxSectionID())
                };
                if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Insert into Sections(sectionID,SectionName,Classid) values(@sectionid,@sectionname,@classid) ", param) > 0)
                {
                    return "Data Inserted Sucessfully!";
                }
                else
                {
                    return "Insert Record Failed!";
                }
            }
        }

        public static string DeleteClass(string Classid)
        {
            if (CountUsers(Classid) > 0)
                return "Class Can't be deleted," + CountUsers(Classid).ToString() + " students found in this class";
            else
            {
                if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "delete from classes where  ClassId=@Classid", new SqlParameter("@ClassID", Classid)) > 0)
                {
                    SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "delete from Sections where  ClassId=@Classid", new SqlParameter("@ClassID", Classid));
                    return "Class Deleted Sucessfully!";
                }
                else
                {
                    return "Class not deleted!";
                }
            }
        }

        public static string AddNewSubject(string subjectname, string classid)
        {
            SqlParameter[] parm =
              {
                    new SqlParameter("@SubName",subjectname),
                    new SqlParameter("@Classid",classid)
                };
            if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from Subjects where SubjectName =@SubName and ClassId=@Classid", parm) > 0)
            {
                return "Subject Already Exists";
            }
            else
            {
                SqlParameter[] param =
                {
                    new SqlParameter("@subname",subjectname),
                    new SqlParameter("@classid",classid),
                    new SqlParameter("@subid",GetMaxSubjectID())
                };
                
                if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Insert into Subjects(SubjectID,SubjectName,Classid) values(@subid,@subname,@classid) ", param) > 0)
                {
                    return "Data Inserted Sucessfully!";
                }
                else
                {
                    return "Insert Record Failed!";
                }
            }
        }

        public static string DeleteSection(string SectionID)
        {
            if (CountUsersInSection(SectionID) > 0)
                return "Section Can't be deleted," + CountUsersInSection(SectionID).ToString() + " students found in this Section";
            else
            {
                if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "delete from Sections where  SectionID=@Sectionid", new SqlParameter("@SectionID", SectionID)) > 0)
                {

                    return "Section Deleted Sucessfully!";
                }
                else
                {
                    return "Section not deleted!";
                }
            }
        }

        public static int CountUsers(string ClassID)
        {
            return (int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from Login where  ClassId=@Classid and type=0", new SqlParameter("@ClassID", ClassID));
        }
        public static DataTable GetSections()
        {
            return SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select classes.classid,classes.classname,sections.sectionid,sections.sectionname from classes inner join sections on classes.classid=sections.classid order by classes.classid").Tables[0];
        }

        private static Int64 GetMaxClassID()
        {
            return (Int64)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select (IsNull(Max(ClassID),0)+1) from Classes");
        }

        private static Int64 GetMaxSectionID()
        {
            return (Int64)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select (IsNull(Max(SectionID),0)+1) from Sections");
        }

        private static Int64 GetMaxSubjectID()
        {
            return (Int64)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select (IsNull(Max(SubjectID),0)+1) from Subjects");
        }



        public static string UpdateClass(string Classid, string ClassName)
        {
            SqlParameter[] param =
                {
                    new SqlParameter("@classname",ClassName),
                    new SqlParameter("@Classid",Classid)
                };

            if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from Classes where classname =@classname and classid !=@Classid", param) > 0)
            {
                return "Class Already Exists";
            }
            else
            {

                if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "update classes set ClassName=@classname where classid=@Classid", param) > 0)
                {
                    return "Updated Sucessfully!";
                }
                else
                {
                    return "fail";
                }
            }
        }

        public static string DeleteSubject(string SubjectID)
        {
            if (Convert.ToInt32(VideoBLL.GetTotalVideosCountInSubject(SubjectID)) > 0)
                return "Subject Can't be deleted," + VideoBLL.GetTotalVideosCountInSubject(SubjectID) + " videos found in this Subject";
            else
            {
                if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "delete from Subjects where  SubjectID=@Subjectid", new SqlParameter("@SubjectID", SubjectID)) > 0)
                {

                    return "Subject Deleted Sucessfully!";
                }
                else
                {
                    return "Subject not deleted!";
                }
            }
        }

        private static int CountUsersInSection(string sectionID)
        {
            return (int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from Login where  SectionId=@Sectionid and type=0", new SqlParameter("@SectionID", sectionID));
        }

        public static string UpdateSection(string ClassID, string SectionID, string SectionName)
        {
            SqlParameter[] param =
               {
                    new SqlParameter("@Classid",ClassID),
                    new SqlParameter("@Sectionid",SectionID),
                    new SqlParameter("@SectionName",SectionName.Trim())
                };

            if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from Sections where SectionName =@SectionName and classid =@Classid", param) > 0)
            {
                return "Section Already Exists";
            }
            else
            {

                if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "update Sections set SectionName=@SectionName where Sectionid=@SectionId", param) > 0)
                {
                    return "Updated Sucessfully!";
                }
                else
                {
                    return "fail";
                }
            }
        }

        public static string UpdateSubject(string ClassID, string SubjectID, string SubjectName)
        {
            SqlParameter[] param =
               {
                    new SqlParameter("@Classid",ClassID),
                    new SqlParameter("@Subjectid",SubjectID),
                    new SqlParameter("@SubjectName",SubjectName.Trim())
                };

            if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select count(*) from Subjects where SubjectName =@SubjectName and classid =@Classid", param) > 0)
            {
                return "Subject Already Exists";
            }
            else
            {

                if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "update Subjects set SubjectName=@SubjectName where Subjectid=@SubjectId", param) > 0)
                {
                    return "Updated Sucessfully!";
                }
                else
                {
                    return "fail";
                }
            }
        }

        public static List<UserModel> UploadUsers(List<UserModel> Users)
        {
            int count = 0;
            UserModel Student = new UserModel();
            List<UserModel> uploadedOnes = new List<UserModel>();
            foreach (UserModel user in Users)
            {
                Student = addStudentData(user); // get StudentNo from Student Table
                SqlParameter[] param =
                   {
                      new SqlParameter("@UserName",Student.Username),
                      new SqlParameter("@ClassID",user.ClassID),
                      new SqlParameter("@Password",Student.Password),
                      new SqlParameter("@Type",user.UserType),
                      new SqlParameter("@Phone",user.Phone),
                      new SqlParameter("@UserFullName",user.UserFullName),
                      new SqlParameter("@UserAddress",user.UserAddress),
                      new SqlParameter("@FatherName",user.FatherName),
                      new SqlParameter("@StudentNo",Student.StudentNo),
                      new SqlParameter("@Email",user.Email),
                   };

                if (!CheckIfUserAlreadyExists(user))
                {
                    if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Insert into Login(UserName,Password,Type,Classid,Phone,IsActive,UserFullName,UserAddress,FatherName,StudentNo,Email) values(@UserName,@Password,@Type,@ClassID,@Phone,1,@UserFullName,@UserAddress,@FatherName,@StudentNo,@Email) ", param) > 0)
                    {
                        count++;
                        user.Password = Encryption.Decrypt(Student.Password, Encryption.key);
                        user.Username =  Encryption.Decrypt(Student.Username, Encryption.key);
                        user.Phone = user.Phone;
                        user.UserFullName = user.UserFullName;
                        uploadedOnes.Add(user);
                    }                   
                }
            }
            return uploadedOnes;
        }

        public static string DeleteUsers(string UserID)
        {
           return (int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect,CommandType.Text,"Delete from Login where Userid=@userid",new SqlParameter("@userid",UserID))>0?"User deleted sucessfully!":"user not deleted";
        }
        public static bool CheckIfUserAlreadyExists(UserModel user)
        {

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Login where UserName='" + user.Username + "'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
           
        }

        public static UserModel addStudentData(UserModel user)
        {
            UserModel umR = new UserModel();
            long adm = getMaxStudentNo();
            SqlParameter[] param =
                  {
                      new SqlParameter("@UserFullName",user.UserFullName),
                      new SqlParameter("@FatherName",user.FatherName),
                      new SqlParameter("@UserAddress",user.UserAddress),
                      new SqlParameter("@Phone",user.Phone),
                      new SqlParameter("@SEmail",user.Email),
                      new SqlParameter("@adm",adm) // Received from getMaxStudentNo();
                   };

            
            if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Insert into Students(StudentName,FathersName,PresentAddress,PhoneNo,SEmail,AdmissionNo) values(@UserFullName,@FatherName,@UserAddress,@Phone,@SEmail,@adm) ", param) > 0)
            {

               
                umR.Username = Encryption.Encrypt(adm.ToString(), Encryption.key);
                umR.Password = Encryption.Encrypt(adm.ToString()+DateTime.Now.Hour.ToString()+DateTime.Now.Year.ToString(), Encryption.key);
                umR.StudentNo = adm;
                // uploadedOnes.Add(user);
            }
     
            return umR;
        }

        public static long getMaxStudentNo()
        {
            long x = 0;

            //SqlParameter[] param =   {
            //    new SqlParameter("@UserFullName", user.UserFullName),
            //          new SqlParameter("@FatherName", user.FatherName),
            //          new SqlParameter("@UserAddress", user.UserAddress),
            //          new SqlParameter("@Phone", user.Phone),
            //          new SqlParameter("@Email", user.Email)
            //       };
       x=(long)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select  isnull(max(admissionNo),0) as adm from students ");

            x = x + 1;

            return x;
        }

        public static List<UserModel> getAllUsers(long type)
        {
            List<UserModel> UL = new List<UserModel>();
            SqlParameter us = new SqlParameter("@Type",type);

            string sql = "select * from Login where Type=@Type";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect,CommandType.Text,sql,us);

            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                UL.Add(new UserModel

                {
                    UserID=dr["UserID"].ToString(),
                    Username= Encryption.Decrypt( dr["Username"].ToString(),Encryption.key),
                    Password = Encryption.Decrypt(dr["Password"].ToString(), Encryption.key),
                    UserFullName = dr["UserFullName"].ToString(),
                    UserAddress = dr["UserAddress"].ToString(),
                    FatherName = dr["FatherName"].ToString(),
                    Phone = dr["Phone"].ToString(),
                    ClassID = dr["ClassID"].ToString(),
                     

                }
                    );
            }

            return UL;
        }
        public static List<UserModel> getAllUsers(long type,string CID)
        {
            List<UserModel> UL = new List<UserModel>();
            SqlParameter[] us = {

                new SqlParameter("@Type", type),
                new SqlParameter("@CID", CID)
            };

            string sql = "select * from Login where Type=@Type and ClassID=@CID";
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, sql, us);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                UL.Add(new UserModel

                {
                    UserID = dr["UserID"].ToString(),
                    Username = Encryption.Decrypt(dr["Username"].ToString(), Encryption.key),
                    Password = Encryption.Decrypt(dr["Password"].ToString(), Encryption.key),
                    UserFullName = dr["UserFullName"].ToString(),
                    UserAddress = dr["UserAddress"].ToString(),
                    FatherName = dr["FatherName"].ToString(),
                    Phone = dr["Phone"].ToString(),
                    ClassID = dr["ClassID"].ToString(),


                }
                    );
            }

            return UL;
        }
        public static UserModel getUser(string id)
        {
           UserModel UL = new UserModel();
            SqlParameter us = new SqlParameter("@id", id);

            string sql = "select * from Login where UserID=@id";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, sql, us);

            DataRow dr = ds.Tables[0].Rows[0];

            UL.UserID = dr["UserID"].ToString();
            UL.Username = Encryption.Decrypt(dr["Username"].ToString(), Encryption.key);
            UL.Password = Encryption.Decrypt(dr["Password"].ToString(), Encryption.key);
            UL.UserFullName = dr["UserFullName"].ToString();
            UL.UserAddress = dr["UserAddress"].ToString();
            UL.FatherName = dr["FatherName"].ToString();
            UL.Phone = dr["Phone"].ToString();
            UL.ClassID = dr["ClassID"].ToString();
            UL.UserType= dr["Type"].ToString();
            UL.IsLocked= dr["IsLocked"].ToString();
            UL.IsOn = dr["IsOn"].ToString();


            return UL;
        }

        public static string UpdateUser(UserModel user)
        {
            
                SqlParameter[] param =
                   {
                      new SqlParameter("@UserID",user.UserID),
                      new SqlParameter("@UserName",user.Username),
                      new SqlParameter("@ClassID",user.ClassID),
                      new SqlParameter("@Password",user.Password),
                      new SqlParameter("@Type",user.UserType),
                      new SqlParameter("@Phone",user.Phone),
                      new SqlParameter("@UserFullName",user.UserFullName),
                      new SqlParameter("@UserAddress",user.UserAddress),
                      new SqlParameter("@FatherName",user.FatherName),
                      new SqlParameter("@StudentNo",user.StudentNo),
                      new SqlParameter("@Email",user.Email),
                      new SqlParameter("@IsLocked",user.IsLocked),
                      new SqlParameter("@IsOn",user.IsOn),
                   };


            if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "update Login set Password =@Password, Phone=@Phone,IsLocked=@IsLocked,IsOn=@IsOn,UserFullName=@UserFullName,UserAddress=@UserAddress,FatherName=@FatherName,Email=@Email where UserID=@UserID ", param) > 0)
            {
                return "1";
            }
            else
            {
                return "0";
            }
                        
                   
                
        }
    }
}