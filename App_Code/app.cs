using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.IO;
/// <summary>
/// Summary description for app
/// </summary>
/// 
namespace Nits.Common
{
    public class appBLL
    {
        public appBLL()
        {
            //
            // TODO: Add constructor logic here
            //
        }



        public static string getApp()
        {
            string pushURL = "http://app.rpschoolmallabagh.com/app.aspx";

            int Status = 0;
            string StatusMessage = "";
            string str = "";
            System.Net.WebClient client = new WebClient();
            Stream data = client.OpenRead(pushURL);
            StreamReader reader = new StreamReader(data);
            str = reader.ReadToEnd();
            data.Close();
            if (str.StartsWith("0,"))
                Status = 0;
            else
                Status = 1;
            StatusMessage = str;
           // str = StatusMessage.Substring(StatusMessage.IndexOf("Sent") + "Sent".Length + 1).Trim();
            // Label1.Text = str;
            return str;
        }
    }


}