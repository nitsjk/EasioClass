using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Nits.Common;
using Nits;
namespace EasioCore
{
    public class studentBLL
    {

        public static List<StudentData> GetStudentsByClassidSectionID(string classid, string sectionid)
        {
            List<StudentData> listStudent = new List<StudentData>();

            SqlParameter[] param = {
                new SqlParameter("@classid",Convert.ToInt64(classid)),
                new SqlParameter("@sectionid",Convert.ToInt64(sectionid))
            };
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from studentinfo inner join Students on StudentInfo.StudentId = students.StudentID where StudentInfo.ClassID=@classid and studentinfo.SectionID=@sectionid and Discharged=0",param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    listStudent.Add(new StudentData
                    {
                        StudentID = Convert.ToInt64(dr["StudentID"].ToString()),
                        StudentName = dr["StudentName"].ToString(),
                        FathersName = dr["FathersName"].ToString(),
                        MothersName = dr["MothersName"].ToString(),
                        DOB = Convert.ToDateTime(dr["DOB"].ToString()),
                        BOA = Convert.ToDateTime(dr["BOA"].ToString()),
                        AdmissionNo = Convert.ToInt64(dr["AdmissionNo"].ToString()),
                        ClassID = Convert.ToInt64(dr["ClassID"].ToString()),
                        SectionID = Convert.ToInt64(dr["SectionID"].ToString()),
                        Rollno = Convert.ToInt64(dr["Rollno"].ToString()),
                        StudentInfoID = Convert.ToInt64(dr["StudentInfoID"].ToString()),
                        PhoneNo =dr["PhoneNo"].ToString()
                    });
                }
            }

            return listStudent;
        }

        public static List<Student> getStudentsBysession(string session)
        {
            List<Student> st = new List<Student>();
            SqlParameter ses = new SqlParameter("@session", session);
            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from students inner join studentinfo on students.studentid=studentinfo.studentid where studentinfo.current_session=@session", ses);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                st.Add(new Student()
                {
                    AccountNo = dr["AccountNo"].ToString() ?? "",
                    AdmissionNo = Int64.Parse(String.IsNullOrEmpty(dr["AdmissionNo"].ToString()) ? "0" : dr["AdmissionNo"].ToString()),
                    AccountType = dr["AccountType"].ToString() ?? "",
                    BankName = dr["BankName"].ToString() ?? "",
                    BloodGroup = dr["BloodGroup"].ToString() ?? "",
                    BOA = Convert.ToDateTime(dr["BOA"].ToString() ?? DateTime.Now.ToShortDateString()),
                    BPLStatus = Int32.Parse(String.IsNullOrEmpty(dr["BPLStatus"].ToString()) ? "0" : dr["BPLStatus"].ToString()),
                    Discharged = Convert.ToBoolean(dr["Discharged"].ToString() ?? "0"),
                    //DateOfBirth = dr["DateOfBirth"].ToString() ?? DateTime.Now.ToShortDateString(),
                    DOB = Convert.ToDateTime(dr["DOB"].ToString() ?? DateTime.Now.ToShortDateString()),
                    Faadhaarcard = Int64.Parse(String.IsNullOrEmpty(dr["Faadhaarcard"].ToString()) ? "0" : dr["Faadhaarcard"].ToString()),
                    FathersJob = dr["FathersJob"].ToString() ?? "",
                    FathersName = dr["FathersName"].ToString() ?? "",
                    FathersQualification = dr["FathersQualification"].ToString() ?? "",
                    Gender = dr["Gender"].ToString() ?? "",
                    IFCCode = dr["IFCCode"].ToString() ?? "",
                    landlineno = dr["landlineno"].ToString() ?? "",
                    Ledgerid = Int64.Parse(String.IsNullOrEmpty(dr["Ledgerid"].ToString()) ? "0" : dr["Ledgerid"].ToString()),
                    Maadhaarcard = Int64.Parse(String.IsNullOrEmpty(dr["Maadhaarcard"].ToString()) ? "0" : dr["Maadhaarcard"].ToString()),
                    MothersJob = dr["MothersJob"].ToString() ?? "",
                    MothersName = dr["MothersName"].ToString() ?? "",
                    MothersQualification = dr["MothersQualification"].ToString() ?? "",
                    MotherTounge = dr["MotherTounge"].ToString() ?? "",
                    // oldnewmigrated =Int32.Parse( dr["oldnewmigrated"].ToString()??"0"),
                    PerminantAddress = dr["PerminantAddress"].ToString() ?? "",
                    PhoneNo = dr["PhoneNo"].ToString() ?? "",

                    StudentID = Int64.Parse(dr["StudentID"].ToString() ?? "0"),
                    StudentName = dr["StudentName"].ToString() ?? ""





                }

                    );
            }

            return st;

        }

        public static bool insertStudent(Student student)
        {
            return false;
        }

        //Check if the student is already assigned to the class with the given section
        private static bool CheckIfAlreadyAssigned(Student student)
        {
            DataSet rn = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from StudentInfo  inner join students on studentinfo.studentid=students.studentid where rollno='" + "dummy" + "' and studentinfo.Current_Session='" + "dummy" + "'and classid=" + "dummy" + " and sectionid=" + "dummy" + " and discharged='False'");
            if (rn.Tables[0].Rows.Count > 0)
            {
                return false;
            }

            return true;
        }


        public static string addNewStudent(StudentData sd)
        {
            sd.AdmissionNo = Int64.Parse( studentBLL.getMaxAdminNo()); // for Cross Check Validation
            SqlParameter[] param = {

                new SqlParameter("@AdmissionNo",sd.AdmissionNo),
                new SqlParameter("@StudentName",sd.StudentName),
                new SqlParameter("@DOB",null),
                new SqlParameter("@FathersName",sd.FathersName),
                new SqlParameter("@MothersName",sd.MothersName),
                new SqlParameter("@fatherQual",sd.FathersQualification),
                new SqlParameter("@PresentAddress",sd.PresentAddress),
                new SqlParameter("@PerminantAddress",sd.PerminantAddress),
                new SqlParameter("@SessionOfAdmission",sd.SessionOfAdmission),
                new SqlParameter("@PhoneNo",sd.PhoneNo),
                new SqlParameter("@Gender",sd.Gender),
                new SqlParameter("@Discharged",false),
                new SqlParameter("@Ledgerid",0),
                new SqlParameter("@ScategoryID",0),
                new SqlParameter("@SEmail",sd.SEmail),
                new SqlParameter("@Current_Session",sd.Current_Session),
                new SqlParameter("@ClassID",sd.ClassID),
                new SqlParameter("@SectionID",sd.SectionID),
                new SqlParameter("@Rollno",sd.Rollno),
                new SqlParameter("@RouteID",sd.RouteID),
                new SqlParameter("@StreamID",sd.StreamID),
                new SqlParameter("@SessionID",sd.SessionID),

            };

            string sqlStudentDuplicate = "select count(*) from Students where StudentName=@StudentName and FathersName=@FathersName and PresentAddress=@PresentAddress and discharged='false'";

            // Check for Duplicate Entery
            int Exixts = (int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, sqlStudentDuplicate, param);

            if (Exixts == 0)

            {
                string sqlStudents = "insert into Students(AdmissionNo,StudentName,DOB,Gender,FathersName,MothersName,PresentAddress,PerminantAddress,SessionOfAdmission,PhoneNo,discharged,SEmail,Ledgerid)values(@AdmissionNo,@StudentName,@DOB,@Gender,@FathersName,@MothersName,@PresentAddress,@PerminantAddress,@SessionOfAdmission,@PhoneNo,@Discharged,@SEmail,0);Select Isnull(max(StudentID),0) as StudentID from Students ";



                long MaxSID = (long)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, sqlStudents, param);

                string sqlStudentInfo = "insert into studentinfo(StudentId,AdmissionNo,Current_Session,SessionID,ClassID,SectionID,RollNo,RouteID,StreamID)values(" + MaxSID + ",@AdmissionNo,@Current_Session,@SessionID,@ClassID,@SectionID,@Rollno,@RouteID,@StreamID)";
                int rt = SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, sqlStudentInfo, param);

                //select StudentName,FathersName,MothersName,Classes.ClassName,SectionName,RollNo,Students.AdmissionNo,Students.StudentID from studentinfo inner join Students on StudentInfo.StudentId = students.StudentID inner join Classes on StudentInfo.ClassID=Classes.ClassId inner join Sections on Sections.SectionID=StudentInfo.SectionID where  Students.StudentId=2888 and Discharged=0

               if(rt>0)
                {
                    createLogin(sd, MaxSID.ToString());
                    return "1";
                }
               else
                {
                    return "0";
                }
            }
            return "0";
        }

        private static void createLogin(StudentData sd,string SID)
        {
            int pw = (int)(sd.AdmissionNo* sd.AdmissionNo); // Creating Random PW
            string pw2 = "10"+pw;
            if (LoginBLL.checkUserAlreadyExists(sd.AdmissionNo.ToString()) == 0) // Not Exists
            {
                
                        StudentsLogin SL = new StudentsLogin();
                        SL.Encusersname = sd.AdmissionNo.ToString();
                        SL.encpassword = pw2;
                        SL.usertype = "0"; // for Students
                        SL.olstdfk = "0";
                        SL.isbloked = "0";
                        SL.FatherName = sd.FathersName;
                        SL.Address = sd.PresentAddress;
                        SL.classid = sd.ClassID.ToString();
                        SL.sectionid = sd.SectionID.ToString();
                        SL.Session = LiveSessionBLL.Current_Session;
                        SL.studentname = sd.StudentName;
                        SL.studentid = SID;
                        SL.StudentNo = sd.AdmissionNo.ToString();
                        SL.PhoneNo = sd.PhoneNo;
                        SL.fatherphoneno = sd.PhoneNo;
                        SL.isbloked = "0";
                // Create User
                LoginBLL.addNewUser(SL);
                    }
                    
                
               
            
        }

        public static string getMaxAdminNo()
        {

            Int64 AdminNo = (Int64)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "Select Isnull(max(AdmissionNo),0) as AdmissionNo from Students");
            if (String.IsNullOrEmpty(AdminNo.ToString()))
            {
                AdminNo = 0;
            }
            AdminNo = AdminNo + 1;
            return AdminNo.ToString();
        }
    }
}