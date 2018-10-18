using System;
using System.Windows.Forms;

namespace MQDemoProducer
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Threading.Thread.CurrentThread.Name = "MainThread";
            Application.Run(new SelectTestCase());
        }
    }
}
