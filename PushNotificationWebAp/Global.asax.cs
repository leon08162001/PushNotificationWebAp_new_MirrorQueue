using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace PushNotificationWebAp
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 應用程式啟動時執行的程式碼
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            #region 系統Log啟動
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath(@"~\log4net.config")));
            #endregion
        }
    }
}