using System;
using System.Windows.Forms;

namespace MQDemoSubscriber
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Threading.Thread.CurrentThread.Name = "MainThread";
            if (args.Length == 0)
            {
                Application.Run(new SelectTestCase());
            }
            else
            {
                if (args.Length == 1)
                {
                    if (args[0].ToUpper().Equals("/MQ"))
                    {
                        MQServer_Receive_Response_Test MQServer = new MQServer_Receive_Response_Test();
                        Application.Run(MQServer);
                    }
                    //else if (args[0].ToUpper().Equals("/T"))
                    //{
                    //    TibcoServer_Receive_Response_Test TibcoServer = new TibcoServer_Receive_Response_Test();
                    //    Application.Run(TibcoServer);
                    //}
                    else if (args[0].ToUpper().Equals("/EMS"))
                    {
                        EMSServer_Receive_Response_Test EMSServer = new EMSServer_Receive_Response_Test();
                        Application.Run(EMSServer);
                    }
                }
            }
        }
    }
}
