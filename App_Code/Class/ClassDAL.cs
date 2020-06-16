using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Nits.Common;
namespace EasioCore
{
    public class ClassDAL
    {
        // Check whether classID used in other tables
        public static int isClassIDUsed(long CID)
        {
            int val = 0;
            SqlParameter para = new SqlParameter("@CID", CID);
            string SQLR = "select count(*) as val from sections where ClassID=@CID;select count(*) as val from StudentInfo where ClassID=@CID";
            SqlDataReader dr = SqlHelper.ExecuteReader(SqlHelper.Connect, CommandType.Text, SQLR, para);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    val = val + (int)dr.GetSqlInt32(0);
                   
                }
                return val;
            }
            else
            {

                return val;
            }
        }
    }
}