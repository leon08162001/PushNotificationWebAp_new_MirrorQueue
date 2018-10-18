using MoneySQContext;
using DBModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebApi.Proxies;
using WebApi.Proxies.Clients;

namespace WindowsFormsCallWebApi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            List<object> Result = new List<object>();
            DefaultClient Default = new DefaultClient();
            Configuration.MyWebApiProxyBaseAddress = "http://localhost:2180/";
            Stopwatch stopwatch = Stopwatch.StartNew();

            JA_EMPOLYEEClient JA_EMPOLYEE = new JA_EMPOLYEEClient();
            ZZ_APPLICATIONClient ZZ_APPLICATION = new ZZ_APPLICATIONClient();

            Configuration.MyWebApiProxyBaseAddress = "http://localhost:2180/";
            List<object> objs = (List<object>)JA_EMPOLYEE.GetAllEmployees();
            List<JA_EMPOLYEE> employees = new List<JA_EMPOLYEE>();
            foreach (object obj in objs)
            {
                JA_EMPOLYEE emp = JsonConvert.DeserializeObject<JA_EMPOLYEE>(obj.ToString());
                employees.Add(emp);
            }
            string EmployeeID = JA_EMPOLYEE.GetEmployeeID("F123755175");
            bool result = JA_EMPOLYEE.UpdateEmployeePushByID("F123755175");
            string ApplicantID = ZZ_APPLICATION.GetApplicantIDByID("F123755175");
            result = ZZ_APPLICATION.UpdateApplicantPushByID("F123755175");
            //foreach(object emp in employees)
            //{
            //    List<JA_EMPOLYEE> a = JsonConvert.DeserializeObject<List<loanApplication_customer>>(Content[0].ToString());
            //}

            //同步
            //for (int i = 0; i < 1000; i++)
            //{
            //    List<object> Content = (List<object>)Default.GetLoanApplication_customer("王勝達");
            //    if (Content.Count == 3)
            //    {
            //        List<loanApplication_customer> a = JsonConvert.DeserializeObject<List<loanApplication_customer>>(Content[0].ToString());
            //        List<comUser> b = JsonConvert.DeserializeObject<List<comUser>>(Content[1].ToString());
            //        List<UserActivityLog> c = JsonConvert.DeserializeObject<List<UserActivityLog>>(Content[2].ToString());
            //    }
            //    else
            //    {
            //        var a = JsonConvert.DeserializeObject(Content.ToString());
            //        JObject obj = JsonConvert.DeserializeObject<JObject>(Content.ToString());
            //        string sErrorCode = obj.GetValue("errorCode", StringComparison.OrdinalIgnoreCase).Value<string>();
            //        string sErrorMessage = obj.GetValue("errorMessage", StringComparison.OrdinalIgnoreCase).Value<string>();
            //    }
            //}

            //非同步
            for (int i = 0; i < 100; i++)
            {
                HttpResponseMessage Res = await Default.GetLoanApplication_customerAsync("王勝達");
                if (Res.IsSuccessStatusCode)
                {
                    Task<List<object>> Content = Res.Content.ReadAsAsync<List<object>>();
                    if (Content.Result.Count == 3)
                    {
                        List<loanApplication_customer> a = JsonConvert.DeserializeObject<List<loanApplication_customer>>(Content.Result[0].ToString());
                        List<comUser> b = JsonConvert.DeserializeObject<List<comUser>>(Content.Result[1].ToString());
                        List<UserActivityLog> c = JsonConvert.DeserializeObject<List<UserActivityLog>>(Content.Result[2].ToString());
                    }
                }
                else
                {
                    Task<object> Content = Res.Content.ReadAsAsync<object>();
                    var a = JsonConvert.DeserializeObject(Content.Result.ToString());
                    JObject obj = JsonConvert.DeserializeObject<JObject>(Content.Result.ToString());
                    string sErrorCode = obj.GetValue("errorCode", StringComparison.OrdinalIgnoreCase).Value<string>();
                    string sErrorMessage = obj.GetValue("errorMessage", StringComparison.OrdinalIgnoreCase).Value<string>();
                }
            }
            stopwatch.Stop();
            MessageBox.Show(Math.Round(Convert.ToDecimal(stopwatch.ElapsedMilliseconds) / Convert.ToDecimal(1000), 3).ToString() + "秒");
        }
    }
}
