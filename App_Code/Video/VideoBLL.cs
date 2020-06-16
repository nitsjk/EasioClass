using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nits.Common;
using Nits.ENC;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Drawing;
using System.IO;

namespace Nits
{
    public class VideoBLL
    {
        public static List<Videos> getAllVideoList()
        {
            List<Videos> VL = new List<Videos>();

            string sql = "select * from Videos  order by VDate desc, VID desc";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, sql);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                VL.Add(new Videos
                {

                    VID = Int64.Parse(dr["VID"].ToString()),
                    VTitle = dr["VTitle"].ToString(),
                    CIDFK = Int64.Parse(dr["VID"].ToString()),
                    SubjectIDFK = Int64.Parse(dr["SubjectIDFK"].ToString()),
                    VDate = Convert.ToDateTime(dr["VDate"].ToString()),
                    VTime = dr["VTime"].ToString(),
                    TIDFK = Int64.Parse(dr["TIDFK"].ToString()),
                    UserName = dr["UserName"].ToString(),
                    VRemarks = dr["VRemarks"].ToString(),
                    VSize = Convert.ToDecimal(dr["VID"].ToString()),
                    ThumbNail = dr["ThumbNail"].ToString(),
                    Vists = Int32.Parse(dr["Vists"].ToString()),
                    VPath = dr["VPath"].ToString(),
                    
                });
            }

            return VL;
        }

        public static List<Videos> getAllVideoList(long CIDFK)
        {
            List<Videos> VL = new List<Videos>();

            SqlParameter cid = new SqlParameter("@CIDFK", CIDFK);
            string sql = "select * from Videos where CIDFK=@CIDFK  order by VDate desc, VID desc";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, sql, cid);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                VL.Add(new Videos
                {

                    VID = Int64.Parse(dr["VID"].ToString()),
                    VTitle = dr["VTitle"].ToString(),
                    CIDFK = Int64.Parse(dr["VID"].ToString()),
                    SubjectIDFK = Int64.Parse(dr["SubjectIDFK"].ToString()),
                    VDate = Convert.ToDateTime(dr["VDate"].ToString()),
                    VTime = dr["VTime"].ToString(),
                    TIDFK = Int64.Parse(dr["TIDFK"].ToString()),
                    UserName = dr["UserName"].ToString(),
                    VRemarks = dr["VRemarks"].ToString(),
                    VSize = Convert.ToDecimal(dr["VID"].ToString()),
                    ThumbNail = dr["ThumbNail"].ToString(),
                    Vists = Int32.Parse(dr["Vists"].ToString()),
                    VPath = dr["VPath"].ToString(),
                });
            }

            return VL;
        }

        public static List<Videos> getAllVideoListBySubjectID(long SubjectIDFK)
        {
            List<Videos> VL = new List<Videos>();

            SqlParameter cid = new SqlParameter("@SubjectIDFK", SubjectIDFK);
            string sql = "select * from Videos where SubjectIDFK=@SubjectIDFK and IsLocked=0  order by VDate desc, VID desc";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, sql, cid);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                VL.Add(new Videos
                {

                    VID = Int64.Parse(dr["VID"].ToString()),
                    VTitle = dr["VTitle"].ToString(),
                    CIDFK = Int64.Parse(dr["VID"].ToString()),
                    SubjectIDFK = Int64.Parse(dr["SubjectIDFK"].ToString()),
                    VDate = Convert.ToDateTime(dr["VDate"].ToString()),
                    VTime = dr["VTime"].ToString(),
                    TIDFK = Int64.Parse(dr["TIDFK"].ToString()),
                    UserName = dr["UserName"].ToString(),
                    VRemarks = dr["VRemarks"].ToString(),
                    VSize = Convert.ToDecimal(dr["VID"].ToString()),
                    ThumbNail = dr["ThumbNail"].ToString(),
                    Vists = Int32.Parse(dr["Vists"].ToString()),
                    VPath = dr["VPath"].ToString(),
                });
            }

            return VL;
        }

        public static Videos getVideo(long VID)
        {
            Videos VL = new Videos();

            SqlParameter cid = new SqlParameter("@VID", VID);
            string sql = "select * from Videos where VID=@VID  order by VDate desc, VID desc";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, sql, cid);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                VL.VID = Int64.Parse(dr["VID"].ToString());
                VL.VTitle = dr["VTitle"].ToString();
                VL.CIDFK = Int64.Parse(dr["VID"].ToString());
                VL.SubjectIDFK = Int64.Parse(dr["SubjectIDFK"].ToString());
                VL.VDate = Convert.ToDateTime(dr["VDate"].ToString());
                VL.VTime = dr["VTime"].ToString();
                VL.TIDFK = Int64.Parse(dr["TIDFK"].ToString());
                VL.UserName = dr["UserName"].ToString();
                VL.VRemarks = dr["VRemarks"].ToString();
                VL.VSize = Convert.ToDecimal(dr["VID"].ToString());
                VL.ThumbNail = dr["ThumbNail"].ToString();
                VL.IsLocked = Int32.Parse(dr["IsLocked"].ToString());
                VL.Vists = Int32.Parse(dr["Vists"].ToString());
                VL.VPath = dr["VPath"].ToString();

            }

            return VL;
        }

        //Thumbnails
        public static string GetThumbnailsOfSubjects(long classIDFK)
        {
            List<Subjects> subjectList = GetSubjectsByClassID(classIDFK);
            StringBuilder sbVideoFormat = new StringBuilder();
            string[] style = { "rotate_right", "rotate_left" };
            String[] colors = { "darkblue", "orange", "darkred", "darkgreen", "purple", "blue", "yellow", "green" };
            int count = 1;
            foreach (Subjects sub in subjectList)
            {

                sbVideoFormat.Append("<div  id='" + String.Concat(sub.SubjectID, '|', sub.SubjectName, '|', sub.ClassID) + "' onclick='Go_ToVideos(this.id)' class='polaroid " + style[count % 2] + "' style='background-color:" + colors[count % 8] + ";color:white;font-weight:bolder;cursor:pointer;'><h3 width ='150' height ='200'>" + sub.SubjectName.ToUpper().Trim() + "</h3><p class='caption' style='color:black;font-weight:bolder;'>Total Videos :" + GetTotalVideosCountInSubject(sub.SubjectID) + " <input type='button' id='" + String.Concat(sub.SubjectID, '|', sub.SubjectName, '|', sub.ClassID) + "' onclick='Go_ToVideos(this.id)' value='Go!' class='btn btn-round btn-primary'/></p></div>");

                count++;
            }

            if (subjectList.Count <= 0)
                sbVideoFormat.Append("<div class='polaroid rotate_left'><h3 width='150' height='200' style='color:red'>No Video Found!</h3><p class='caption'>Videos will be available soon!</p></div>");

            return sbVideoFormat.ToString();
        }
        public static string GetMenuOfSubjects(long classIDFK)
        {
            List<Subjects> subjectList = GetSubjectsByClassID(classIDFK);
            StringBuilder sbVideoFormat = new StringBuilder();
            string[] style = { "rotate_right", "rotate_left" };
            String[] colors = { "darkblue", "orange", "darkred", "darkgreen", "purple", "blue", "yellow", "green" };
            int count = 1;
            foreach (Subjects sub in subjectList)
            {

                sbVideoFormat.Append("<li>  <a href='vd.aspx' id='" + String.Concat(sub.SubjectID, '|', sub.SubjectName, '|', sub.ClassID)+'|'+ GetTotalVideosCountInSubject(sub.SubjectID) + "' onClick='Go_ToVideos(id); ' > " + sub.SubjectName.ToUpper().Trim()+" ("  + GetTotalVideosCountInSubject(sub.SubjectID) + " ) </a></li>");

                count++;
            }

            if (subjectList.Count <= 0)
                sbVideoFormat.Append("<li>  <a href='#'  > No Suject Found! </a></li>");

            return sbVideoFormat.ToString();
        }

        public static string GetThumbnailsOfAllClasses()
        {
            List<Subjects> classList = GetAllClasses();
            StringBuilder sbVideoFormat = new StringBuilder();
            string[] style = { "rotate_right", "rotate_left" };
            String[] colors = { "darkblue", "orange", "darkred", "darkgreen", "purple", "blue", "yellow", "green" };
            int count = 1;
            foreach (Subjects classes in classList)
            {
                sbVideoFormat.Append("<tr><td><div id='" + String.Concat(classes.ClassID) + "' onclick='Go_ToVideos(this.id)' value='Go!' class='polaroid " + style[count % 2] + "' style='background-color:" + colors[count % 8] + ";color:white;font-weight:bolder;cursor:pointer;'><h3 width ='150' height ='200'>" + classes.ClassName.ToUpper().Trim() + "</h3><p class='caption' style='color:black;font-weight:bolder;'>Total Videos: " + GetTotalVideosCountInClass(classes.ClassID) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <input type='button' id='" + String.Concat(classes.ClassID) + "' onclick='Go_ToVideos(this.id)' value='Go!' class='btn btn-round btn-primary'/></p></div>");
                count++;
            }

            if (classList.Count <= 0)
                sbVideoFormat.Append("<div class='polaroid rotate_left'><h3 width='150' height='200' style='color:red'>No Video Found!</h3><p class='caption'>Videos will be available soon!</p></div>");

            return sbVideoFormat.ToString();
        }

        public static string GetThumbnailsOfClassesWithSpecificTeacher()
        {
            List<Subjects> classList = GetAllClassesRelatedWithTeacher();
            StringBuilder sbVideoFormat = new StringBuilder();
            string[] style = { "rotate_right", "rotate_left" };
            String[] colors = { "darkblue", "orange", "darkred", "darkgreen", "purple", "blue", "yellow", "green" };
            int count = 1;
            foreach (Subjects classes in classList)
            {
                sbVideoFormat.Append("<tr><td><div id='" + String.Concat(classes.ClassID) + "' onclick='Go_ToVideos(this.id)' class='polaroid " + style[count % 2] + "' style='background-color:" + colors[count % 8] + ";color:white;font-weight:bolder;'><h3 width ='150' height ='200'>" + classes.ClassName.ToUpper().Trim() + "</h3><p class='caption' style='color:black;font-weight:bolder;cursor:pointer;'>Total Videos: " + GetTotalVideosCountInClass(classes.ClassID) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <input type='button' id='" + String.Concat(classes.ClassID) + "' onclick='Go_ToVideos(this.id)' value='Go!' class='btn btn-round btn-primary'/></p></div>");
                count++;
            }

            if (classList.Count <= 0)
                sbVideoFormat.Append("<div class='polaroid rotate_left'><h3 width='150' height='200' style='color:red'>No Video Found!</h3><p class='caption'>Videos will be available soon!</p></div>");

            return sbVideoFormat.ToString();
        }
        public static string GetThumbnailsOfAllSubjectsClassWise()
        {
            List<Subjects> subjectList = GetAllSubjectsBySession();
            StringBuilder sbVideoFormat = new StringBuilder();
            string[] style = { "rotate_right", "rotate_left" };
            String[] colors = { "darkblue", "orange", "darkred", "darkgreen", "purple", "blue", "yellow", "green" };
            int count = 1;
            string ClassID = string.Empty;
            sbVideoFormat.Append("<table>");
            foreach (Subjects sub in subjectList)
            {

                if (!ClassID.Equals(sub.ClassID))
                {
                    sbVideoFormat.Append("<tr><td><div class='polaroid " + style[count % 2] + "' style='background-color:" + colors[count % 8] + ";color:white;font-weight:bolder;'><h3 width ='150' height ='200'>" + sub.ClassName.ToUpper().Trim() + "</h3><p class='caption' style='color:black;font-weight:bolder;'>Total Videos: 90</p></div>");
                    sbVideoFormat.Append("<div class='col-md-row'></div></td><tr><td>");
                    ClassID = sub.ClassID;
                }
                else
                {
                    sbVideoFormat.Append("<div id='" + String.Concat(sub.SubjectID, '|', sub.SubjectName, '|', sub.ClassID) + "' onclick='Go_ToVideos(this.id)' class='polaroid " + style[count % 2] + "' style='background-color:" + colors[count % 8] + ";color:white;font-weight:bolder;width:150px;cursor:pointer;'><h6  height ='100'>" + sub.SubjectName.ToUpper().Trim() + "</h6><p class='caption' style='color:black;font-weight:bolder;'>Videos <input type='button' id='" + String.Concat(sub.SubjectID, '|', sub.SubjectName, '|', sub.ClassID) + "' onclick='Go_ToVideos(this.id)' value='Go!' class='btn btn-round btn-primary'/></p></div>");
                }

                count++;
            }

            if (subjectList.Count <= 0)
                sbVideoFormat.Append("<div class='polaroid rotate_left'><h3 width='150' height='200' style='color:red'>No Video Found!</h3><p class='caption'>Videos will be available soon!</p></div>");
            else
                sbVideoFormat.Append("</table");

            return sbVideoFormat.ToString();
        }

        public static string GetVideosWithThumbnailsBySubjectID(long SubjectIDDFK)
        {
            List<Videos> videoList = getAllVideoListBySubjectID(SubjectIDDFK);
            StringBuilder sbVideoFormat = new StringBuilder();
            string[] style = { "rotate_right", "rotate_left" };
            String[] colors = { "darkblue", "orange", "darkred", "darkgreen", "purple", "blue", "yellow", "green" };
            int count = 1;
            // sbVideoFormat.Append("<table class='table table-hover table-responsive table-bordered'>");
            foreach (Videos video in videoList)
            {

                sbVideoFormat.Append("<div class='polaroid " + style[count % 2] + "' id='" + String.Concat(video.VID) + "' onclick='Play_Video(this.id)' style='background-color:" + colors[count % 8] + ";color:white;font-weight:bolder;width:200px;cursor:pointer;'><h4 width ='60' height ='10'>" + video.VTitle.ToUpper().Trim() + "<input type='button' id='" + String.Concat(video.VID) + "' onclick='Play_Video(this.id)' class='btn btn-success btn-round pull-right' value='►'/></h4><p class='caption' style='color:black;font-weight:bolder;'>" + video.VRemarks + "</p></div>");

                //sbVideoFormat.Append("<tr><td style='font-weight:bolder'>" + video.VTitle.ToUpper().Trim() + "<input type='button' id='" + String.Concat(video.VID) + "' onclick='Play_Video(this.id)' class='btn btn-success btn-round pull-right' value='►'/></td></tr><tr><td style='color:black;font-weight:bolder;'>Remarks: " + video.VRemarks + "</td></tr><tr><td>&nbsp;</td></tr>");


                count++;
            }

            if (videoList.Count <= 0)
                sbVideoFormat.Append("<div class='polaroid rotate_left'><h3 width='150' height='200' style='color:red'>No Video Found!<a href='../Gallery/VideoGallery.aspx' class='pull-right'><span class='glyphicon glyphicon-home'></a></h3><p>Videos will be available soon!</p></div></td></tr></table>");
            //else
            //    sbVideoFormat.Append("</table>");

            return sbVideoFormat.ToString();
        }

        public static string GetVideosListWithThumbnailsBySubjectID(long SubjectIDDFK)
        {
            List<Videos> videoList = getAllVideoListBySubjectID(SubjectIDDFK);
            StringBuilder sbVideoFormat = new StringBuilder();
            string[] style = { "rotate_right", "rotate_left" };
            String[] colors = { "darkblue", "orange", "darkred", "darkgreen", "purple", "blue", "yellow", "green" };
            int count = 1;
             //sbVideoFormat.Append("<div class='col-md-12'");
            foreach (Videos video in videoList)
            {

                sbVideoFormat.Append("<div id='" + String.Concat(video.VID,'|',video.VTitle,'|',video.VRemarks) + "' onclick='Play_Video(this.id)' style='background-color:" + colors[count % 8] + ";color:white;font-weight:bolder;cursor:pointer;padding:5px;margin:5px;box-shadow: 5px 8px 4px black;' class='col-md-4'><h5 style='margin:2px;'>" + video.VTitle.Trim() + "</h5><p class='caption' style='color:black;font-weight:bold;margin:2px;padding:2px;'>" + video.VRemarks + "</p><p class='pull-left' style='margin:2px;padding:0px;'>Views:" + video.Vists + "&nbsp;</p></div>");



                count++;
            }

            if (videoList.Count <= 0)
                sbVideoFormat.Append("<div class='polaroid rotate_left'><h3 width='150' height='200' style='color:red'>No Video Found!<a href='../Gallery/VideoGallery.aspx' class='pull-right'><span class='glyphicon glyphicon-home'></a></h3><p>Videos will be available soon!</p></div></td></tr></table>");
            //else
            //    sbVideoFormat.Append("</div>");

            return sbVideoFormat.ToString();
        }
        //End of Thumbnails

        public static List<Videos> GetVideosByClassID(string ClassIDFK)
        {
            List<Videos> VL = new List<Videos>();
            SqlParameter cid = new SqlParameter("@CIDFK", ClassIDFK);
            string sql = "select * from Videos where CIDFK=@CIDFK  order by VDate desc, VID desc";

            DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, sql, cid);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                VL.Add(new Videos
                {

                    VID = Int64.Parse(dr["VID"].ToString()),
                    VTitle = dr["VTitle"].ToString(),
                    CIDFK = Int64.Parse(dr["VID"].ToString()),
                    IsLocked=Convert.ToInt32(dr["IsLocked"].ToString()),
                    SubjectIDFK = Int64.Parse(dr["SubjectIDFK"].ToString()),
                    VDate = Convert.ToDateTime(dr["VDate"].ToString()),
                    VTime = dr["VTime"].ToString(),
                    TIDFK = Int64.Parse(dr["TIDFK"].ToString()),
                    UserName = dr["UserName"].ToString(),
                    VRemarks = dr["VRemarks"].ToString(),
                    VSize = Convert.ToDecimal(dr["VID"].ToString()),
                    ThumbNail = dr["ThumbNail"].ToString(),
                    Vists = Int32.Parse(dr["Vists"].ToString()),
                    VPath = dr["VPath"].ToString(),
                });
            }
            return VL;
        }

        public static List<Subjects> GetSubjectsByClassID(long ClassIDFK)
        {
            DataTable dsSubjects = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "Select * from Subjects where ClassId=@ClassIDFK", new SqlParameter("@ClassIDFK", ClassIDFK)).Tables[0];

            List<Subjects> subjectsList = new List<Subjects>();

            foreach (DataRow dr in dsSubjects.Rows)
            {
                Subjects subjectModel = new Subjects();
                subjectModel.SubjectID = dr["SubjectID"].ToString();
                subjectModel.SubjectName = dr["SubjectName"].ToString();
                subjectModel.ClassID = ClassIDFK.ToString();
                subjectsList.Add(subjectModel);
            }

            return subjectsList;
        }

        public static List<Subjects> GetSubjectsByTeacherID(long ClassIDFK)
        {
            string TeacherSubjects = GetTeacherSubjectsArray();
            SqlParameter[] param =
            {
                new SqlParameter("@TchrSub",TeacherSubjects),
                new SqlParameter("@ClassIDFK",ClassIDFK)
            };

            // DataTable dsSubjects = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "Select * from Subjects where SubjectID in (Select Item from dbo.SplitString(@TchrSub))", param).Tables[0];
            DataTable dsSubjects = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "Select * from Subjects where ClassID=@ClassIDFK", param).Tables[0];

            List<Subjects> subjectsList = new List<Subjects>();

            foreach (DataRow dr in dsSubjects.Rows)
            {
                Subjects subjectModel = new Subjects();
                subjectModel.SubjectID = dr["SubjectID"].ToString();
                subjectModel.SubjectName = dr["SubjectName"].ToString();
                subjectModel.ClassID = ClassIDFK.ToString();
                subjectsList.Add(subjectModel);
            }
            return subjectsList;
        }

        private static string GetTeacherSubjectsArray()
        {
            SqlParameter[] param =
            {
                new SqlParameter("@UID",HttpContext.Current.Session["SessionUserid"].ToString()),
                new SqlParameter("@ClassID",HttpContext.Current.Session["SessionClassid"].ToString())
            };
            //return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select SubjectIDs from Teachers Where ClassID =@ClassID and UIDFK=@UID", param).ToString();

            return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "select SubjectID from Subjects  Where ClassID =@ClassID ", param).ToString();
        }

        public static List<Subjects> GetAllSubjectsBySession()
        {
            DataTable dsSubjects = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select subjects.*,classes.classname from subjects inner join Classes on Classes.ClassId=Subjects.ClassID where subjects.current_session=@Session order by Classid", new SqlParameter("@Session", VideoBLL.Session())).Tables[0];

            List<Subjects> subjectsList = new List<Subjects>();

            foreach (DataRow dr in dsSubjects.Rows)
            {
                Subjects subjectModel = new Subjects();
                subjectModel.SubjectID = dr["SubjectID"].ToString();
                subjectModel.SubjectName = dr["SubjectName"].ToString();
                subjectModel.ClassName = dr["ClassName"].ToString();
                subjectModel.ClassID = dr["ClassID"].ToString();
                subjectsList.Add(subjectModel);
            }

            return subjectsList;
        }

        public static List<Subjects> GetAllSubjects()
        {
            DataTable dsSubjects = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select subjects.*,classes.classname from subjects inner join Classes on Classes.ClassId=Subjects.ClassID  order by Classid").Tables[0];

            List<Subjects> subjectsList = new List<Subjects>();

            foreach (DataRow dr in dsSubjects.Rows)
            {
                Subjects subjectModel = new Subjects();
                subjectModel.SubjectID = dr["SubjectID"].ToString();
                subjectModel.SubjectName = dr["SubjectName"].ToString();
                subjectModel.ClassName = dr["ClassName"].ToString();
                subjectModel.ClassID = dr["ClassID"].ToString();
                subjectsList.Add(subjectModel);
            }

            return subjectsList;
        }
        public static List<Subjects> GetAllClasses()
        {
            DataTable dsSubjects = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Classes ").Tables[0];

            List<Subjects> classList = new List<Subjects>();

            foreach (DataRow dr in dsSubjects.Rows)
            {
                Subjects classModel = new Subjects();
                classModel.ClassName = dr["ClassName"].ToString();
                classModel.ClassID = dr["ClassID"].ToString();
                classList.Add(classModel);
            }

            return classList;
        }

        public static List<Subjects> GetAllClassesRelatedWithTeacher()
        {
            // DataTable dsSubjects = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Classes Where ClassID in (Select ClassID from Teachers where UIDFK=@UIDFK)", new SqlParameter("@UIDFK", HttpContext.Current.Session["SessionUserid"].ToString())).Tables[0];

            DataTable dsSubjects = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Classes ").Tables[0];

            List<Subjects> classList = new List<Subjects>();

            foreach (DataRow dr in dsSubjects.Rows)
            {
                Subjects classModel = new Subjects();
                classModel.ClassName = dr["ClassName"].ToString();
                classModel.ClassID = dr["ClassID"].ToString();
                classList.Add(classModel);
            }

            return classList;
        }

        public static bool IsServiceAvailable()
        {
            if ((int)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "Select Count(*) from OnlineService") > 0)
                return (Int16)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "Select IsActive from OnlineService") > 0 ? true : false;
            else
                return false;
        }

        public static string DataSpaceAllocated()
        {
            return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "Select top 1 DiskSpaceAllocated from OnlineService").ToString();
        }

        public static string DataSpaceAvailable()
        {
            return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "Select top 1 DiskSpaceAvailable from OnlineService").ToString();
        }

        public static string DiskSpaceUsedByVideo(string vid)
        {
            return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "Select VSize  from Videos where vid=@vid", new SqlParameter("@vid", vid)).ToString();
        }
        public static string DataSpaceUsed()
        {
            return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "Select top 1 DiskSpaceUsed from OnlineService").ToString();
        }

        public static void UpdateStorageSpace(string DiskSpaceAvailable, string DiskSpaceUsed)
        {
            SqlParameter[] param =
            {
                new SqlParameter("@DSA",DiskSpaceAvailable),
                new SqlParameter("@DSU",DiskSpaceUsed)
            };

            SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Update OnlineService set Diskspaceavailable=@DSA,DiskSpaceUsed=@DSU", param);
        }

        public static void UpdateStorageAfterVideoDeleted(string VSize)
        {
            string SpaceUsed = (Convert.ToDecimal(DataSpaceUsed()) - Convert.ToDecimal(VSize)).ToString();
            string SpaceRemaining = (Convert.ToDecimal(DataSpaceAvailable()) + Convert.ToDecimal(VSize)).ToString();
            UpdateStorageSpace(SpaceRemaining, SpaceUsed);
        }
        public static string ServiceStartedOn()
        {
            return Convert.ToDateTime(SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "Select top 1 ServiceStartedOn from OnlineService")).ToString("dd/MM/yyyy");
        }

        public static string ServiceEndsOn()
        {
            return Convert.ToDateTime(SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "Select top 1 ServiceExpiresOn from OnlineService")).ToString("dd/MM/yyyy");
        }

        public static string Session()
        {
            //return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "Select top 1 Session from OnlineService").ToString();
            return "2018-19";
        }

        public static string SchoolName()
        {
            return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "Select top 1 SchoolName from OnlineService").ToString();
        }

        public static string GetTotalVideosCountInClass(string ClassID)
        {
            List<Videos> VL = new List<Videos>();

            SqlParameter cid = new SqlParameter("@CIDFK", ClassID);
            string sql = "select IsNull(count(*),0) from Videos where CIDFK=@CIDFK";

            return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, sql, cid).ToString();

        }

        public static string GetTotalVideosCountInSubject(string SubjectID)
        {
            List<Videos> VL = new List<Videos>();

            SqlParameter subid = new SqlParameter("@SubjectID", SubjectID);
            string sql = "select IsNull(count(*),0) from Videos where SubjectIDFK=@SubjectID";

            return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, sql, subid).ToString();

        }

        public static List<TeacherSubjects> GetTeacherSubjects()
        {
            List<TeacherSubjects> listTeacherSub = new List<TeacherSubjects>();
           DataTable dtTeacherSubjects= SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "Select * from Teachers").Tables[0];
            foreach(DataRow dr in dtTeacherSubjects.Rows)
            {
                TeacherSubjects tsub = new TeacherSubjects();
                tsub.ClassID = dr["ClassID"].ToString();
                tsub.UIDFK = dr["UIDFK"].ToString();
                tsub.SubjectIDs = dr["SubjectIDs"].ToString();
                listTeacherSub.Add(tsub);              
            }

            return listTeacherSub;
        }

        //Update /Delete
        public static void UpdateVideo(Videos video)
        {
            SqlParameter[] parma =
             {
                new SqlParameter("@Title",video.VTitle),
                new SqlParameter("@Remarks",video.VRemarks),
                new SqlParameter("@vid",video.VID),
                new SqlParameter("@IsLocked",video.IsLocked)
            };

            SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Update Videos set VTitle=@Title,VRemarks=@Remarks,IsLocked=@IsLocked where VID=@vid", parma);
        }

        public static void DeleteVideo(string videoid)
        {
            string videospace = DiskSpaceUsedByVideo(videoid);
            UpdateStorageAfterVideoDeleted(videospace);
            SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Delete from videos where vid=@Vid", new SqlParameter("@Vid", videoid));

            FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("../Gallery/Videos/" + videoid + ".mp4"));
            file.Delete();
        }

        public static string SaveVideo(Videos video)
        {
            SqlParameter[] parma =
           {
                new SqlParameter("@Title",video.VTitle),
                new SqlParameter("@Remarks",video.VRemarks),
                new SqlParameter("@CID",video.CIDFK),
                new SqlParameter("@SubjectID",video.SubjectIDFK),
                new SqlParameter("@Date",video.VDate),
                new SqlParameter("@Time",video.VTime),
                new SqlParameter("@Size",video.VSize),
                new SqlParameter("@Path",video.VPath),
                new SqlParameter("@TIDFK",video.TIDFK),
                new SqlParameter("@User",video.UserName)
            };

            int result = 0;
            if ((int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Insert into videos(VTitle,CIDFK,SubjectIDFK,VDate,VTime,TIDFK,UserName,VRemarks,VSize,IsLocked,Vists,VPath)Values(@Title,@CID,@SubjectID,@Date,@Time,@TIDFK,@User,@Remarks,@Size,0,0,@Path)", parma) > 0)
            {

                Int64 PathID = (Int64)SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, "Select Max(VID) from Videos");
                string ext = Path.GetExtension(video.VPath);
                string filename = String.Concat(PathID.ToString(), ext);




                SqlParameter[] parameters =
                {
                    new SqlParameter("@VID",PathID),
                    new SqlParameter("@Path",filename)
                };

                result = (int)SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Update Videos set VPath=@Path where Vid=@Vid", parameters);

                string SpaceUsed = (Convert.ToDecimal(video.VSize) + Convert.ToDecimal(DataSpaceUsed())).ToString();
                string SpaceRemaining = (Convert.ToDecimal(DataSpaceAvailable()) - Convert.ToDecimal(video.VSize)).ToString();

                UpdateStorageSpace(SpaceRemaining, SpaceUsed);

                DirectoryInfo d = new DirectoryInfo(HttpContext.Current.Server.MapPath("../Gallery/videos"));
                FileInfo[] infos = d.GetFiles();
                foreach (FileInfo f in infos)
                {
                    if (f.Name.Equals(video.VPath))
                    {
                        File.Move(f.FullName, f.FullName.Replace(f.Name, filename));
                        break;
                    }
                }
            }

            return result > 0 ? "File Saved Sucessfully!" : "Error in Uploading File";
        }

        public static void UpdateViews(String videoid)
        {
            SqlHelper.ExecuteNonQuery(SqlHelper.Connect, CommandType.Text, "Update Videos set Vists=(Vists+1) where VID=@vid", new SqlParameter("@vid",videoid));
        }
    }
}