using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nits.Common;
using Nits.ENC;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;
/// <summary>
/// Summary description for General
/// </summary>
/// 
namespace Nits
{
    public class GeneralBLL
    {
        
        public static string SchoolName()
        {
            DataSet dsH = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select  top 1 * from OnlineService");
            DataRow dr = dsH.Tables[0].Rows[0];
             return dr["SchoolName"].ToString();

           
        }

   
                
        public static string SchoolLogo()
        {
            return "/logo/logo.png";
        }

        public static string Title()
        {
            return "Easio Class";
        }
        public static int SqlHelperConnection =  Int32.Parse(sqlConnectionNo());

        public static string sqlConnectionNo()
        {
            DataSet dsH = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select  top 1 * from OnlineService");
            DataRow dr = dsH.Tables[0].Rows[0];
            return dr["SQL"].ToString();


        }

        public static DateTime getUTCDate()
        {
            string qr = "dECLARE @UTCTime As DATETIME;SET @UTCTime = GETUTCDATE();SELECT DATEADD(MI, 330, @UTCTime) AS dat";
            DataSet dsH = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, qr);
            DataRow dr = dsH.Tables[0].Rows[0];
            dr["dat"].ToString();
            DateTime dt = new DateTime();


            return Convert.ToDateTime(dr["dat"].ToString());

        }

        public static string getUTCFinal(DateTime publishDate)
        {
            if (DBData.UTCDate == 2)
            {
                string Query = "SELECT  case when @publishDate =CAST(FLOOR(CAST(DATEADD(MI, 330, GETUTCDATE()) AS FLOAT))AS DATETIME) then 'equal'  when @publishDate >CAST(FLOOR(CAST(DATEADD(MI, 330, GETUTCDATE()) AS FLOAT))AS DATETIME) then 'greater' when @publishDate <CAST(FLOOR(CAST(DATEADD(MI, 330, GETUTCDATE()) AS FLOAT))AS DATETIME) then 'less' end";

                return SqlHelper.ExecuteScalar(SqlHelper.Connect, CommandType.Text, Query, new SqlParameter("@publishDate", publishDate)).ToString();
            }

            return DateTime.Now.Date.ToString();

        }
    }
}