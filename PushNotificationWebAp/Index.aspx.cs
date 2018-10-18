using System;
using System.Net;
using System.Net.Sockets;

namespace PushNotificationWebAp
{
    public partial class Index1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["ServerName"] != null)
            //{
            //    Label1.Text = "Web Server:" + Session["ServerName"].ToString();
            //}
            if (Session["ServerName"] == null)
            {
                Class1 cls = new Class1();
                cls.Name = "leonlee";
                cls.NickName = "BigLio";
                Session["Class1"] = cls;
                Session["ServerName"] = GetServerName();
            }
            Label1.Text = "Web Server:" + Session["ServerName"].ToString();
        }
        private string GetServerName()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            return ipHostInfo.HostName;
        }
        private string GetIPAddress()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress address in ipHostInfo.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                    return address.ToString();
            }

            return string.Empty;
        }
    }
}