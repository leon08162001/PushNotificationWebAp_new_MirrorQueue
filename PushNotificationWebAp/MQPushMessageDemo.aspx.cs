using Common;
using Common.LinkLayer;
using Common.Utility;
using DBModels;
using Newtonsoft.Json;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace PushNotificationWebAp
{
    public partial class MQPushMessageDemo : System.Web.UI.Page
    {
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IMQAdapter JefferiesExcuReportMQ = TopicMQFactory.GetMQAdapterInstance(MQAdapterType.BatchMQAdapter);
        IApplicationContext applicationContext;
        Config config;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Label9.Text = "Web Server:" + GetIPAddress();
                if (!IsPostBack)
                {
                    InitialMQSetting();
                    BindingUIControl();
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }

        protected void btnPushMessage_Click(object sender, EventArgs e)
        {
            try
            {
                JefferiesExcuReportMQ = Session["JefferiesExcuReportMQ"] as IMQAdapter;
                if (JefferiesExcuReportMQ == null)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "activemqNotAlive", "window.alert('Session Timeout')", true);
                }
                else
                {
                    if (JefferiesExcuReportMQ.CheckActiveMQAlive())
                    {
                        OpenMQ();
                        SendData();
                        CloseMQ();
                        Response.Redirect(Request.Url.AbsoluteUri, false);
                    }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "activemqNotAlive", "window.alert('ActiveMQ服務尚未啟動')", true);
                    }
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }

        private void BindingUIControl()
        {
            applicationContext = ContextRegistry.GetContext();
            config = (Config)applicationContext.GetObject("Config");

            txtURI.Text = config.MQ_network + ":" + config.MQ_service;
            txtUserName.Text = config.MQUserID;
            txtPassword.Text = config.MQPwd;
        }
        private void InitialMQSetting()
        {
            applicationContext = ContextRegistry.GetContext();
            config = (Config)applicationContext.GetObject("Config");

            JefferiesExcuReportMQ.Uri = config.MQ_network + ":" + config.MQ_service;
            JefferiesExcuReportMQ.MessageTimeOut = 30;
            JefferiesExcuReportMQ.DestinationFeature = DestinationFeature.VirtualTopic;
            //JefferiesExcuReportMQ.DestinationFeature = DestinationFeature.Queue;
            //JefferiesExcuReportMQ.SendName = txtReceiverID.Text.Trim();
            JefferiesExcuReportMQ.UserName = config.MQUserID;
            JefferiesExcuReportMQ.PassWord = config.MQPwd;
            JefferiesExcuReportMQ.UseSSL = config.MQ_useSSL;
            Session["JefferiesExcuReportMQ"] = JefferiesExcuReportMQ;
        }

        private void SendData()
        {
            string sReceiverID = txtReceiverID.Text.EndsWith(";") ? txtReceiverID.Text.Substring(0, txtReceiverID.Text.Length - 1) : txtReceiverID.Text;
            Int32 len = sReceiverID.Split(new char[] { ';' }).Length;

            for (int h = 0; h < len; h++)
            {
                //若在Start()後設定SendName,必須呼叫ReStartSender才有效;
                JefferiesExcuReportMQ.SendName = sReceiverID.Split(new char[] { ';' })[h];
                JefferiesExcuReportMQ.ReStartSender(JefferiesExcuReportMQ.SendName);
                //後送實際的資料列
                for (int i = 0; i < Convert.ToInt32(txtMessageNums.Text); i++)
                {
                    MessageAddressee Message = new MessageAddressee();
                    Message.Subject = txtOTATopicName.Text.Trim();
                    Message.Message = txtMessage.Text.Trim();
                    Message.PushMessageID = Guid.NewGuid().ToString();
                    //若有申請書編號或合約編號參數則須指定
                    //Message.application_no = "xxxxx";
                    //Message.contract_number = "xxxxx";
                    Message.SendedMessageTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    string json = JsonConvert.SerializeObject(Message);
                    List<Dictionary<string, string>> Rows = Util.ToMessageMapForJson(json);
                    List<List<MessageField>> MultiMQMessage = new List<List<MessageField>>();
                    int iSeqence = 1;
                    foreach (Dictionary<string, string> row in Rows)
                    {
                        List<MessageField> MqMessage = new List<MessageField>();
                        //加入資料序號(因底層元件需要,必須加此欄位)
                        MessageField MessageSeqenceField = new MessageField();
                        MessageSeqenceField.Name = "9999";
                        MessageSeqenceField.Value = iSeqence.ToString();
                        MqMessage.Add(MessageSeqenceField);
                        iSeqence++;
                        //加入資料序號(因底層元件需要,必須加此欄位)
                        foreach (string col in row.Keys)
                        {
                            MessageField MessageField = new MessageField();
                            MessageField.Name = col;
                            MessageField.Value = row[col];
                            MqMessage.Add(MessageField);
                        }
                        MultiMQMessage.Add(MqMessage);
                    }
                    if (JefferiesExcuReportMQ.SendMQMessage("710", MultiMQMessage, 0, 0))
                    {
                        if (log.IsInfoEnabled) log.Info("Send JefferiesExcuReport Message from MQSender");
                    }
                    MultiMQMessage.Clear();
                    System.Threading.Thread.Sleep(100);
                    SendFiles(Message.PushMessageID);
                }
            }
        }
        private void SendFiles(string pushID)
        {
            if (FileUpload1.HasFiles)
            {
                foreach(HttpPostedFile file in FileUpload1.PostedFiles)
                {
                    String fileName = file.FileName;
                    BinaryReader br = new BinaryReader(file.InputStream);
                    br.BaseStream.Position = 0;
                    byte[] bytes = br.ReadBytes(file.ContentLength);
                    bool senFileResult = JefferiesExcuReportMQ.SendFile(fileName, bytes, pushID);
                }
            }
        }
        private void OpenMQ()
        {
            JefferiesExcuReportMQ = Session["JefferiesExcuReportMQ"] as IMQAdapter;
            JefferiesExcuReportMQ.Start();
        }

        private void CloseMQ()
        {
            JefferiesExcuReportMQ = Session["JefferiesExcuReportMQ"] as IMQAdapter;
            JefferiesExcuReportMQ.Close();
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