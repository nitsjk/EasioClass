using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nits;
using System.Data.SqlClient;
using System.Data;
using Nits.Common;
namespace Nits
{
    public class LiveSessionBLL
    {
        public static string getCurrent_Session()
        {
            //DataSet ds = SqlHelper.ExecuteDataset(SqlHelper.Connect, CommandType.Text, "select * from Livesession");
            //if(ds.Tables[0].Rows.Count>0)
            //{
            //    DataRow dr = ds.Tables[0].Rows[0];
            //    return dr["Current_Session"].ToString();

            //}
            //{

            //    return "0";
            //}
            return "2019-20";
        }

        public static string Current_Session = getCurrent_Session();
    }
}