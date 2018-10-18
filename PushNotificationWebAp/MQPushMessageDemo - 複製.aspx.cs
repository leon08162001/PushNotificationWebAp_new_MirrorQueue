using Common;
using Common.LinkLayer;
using Common.Utility;
using DBContext;
using DBModels;
using Newtonsoft.Json;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace PushNotificationWebAp
{
    public partial class MQPushMessageDemo : System.Web.UI.Page
    {
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //List<IMQAdapter> JefferiesExcuReportMQList = new List<IMQAdapter>();
        IMQAdapter JefferiesExcuReportMQ = TopicMQFactory.GetMQAdapterInstance(MQAdapterType.BatchMQAdapter);
        IMQAdapter OTAExportMQ = TopicMQFactory.GetMQAdapterInstance(MQAdapterType.BatchMQAdapter);

        IApplicationContext applicationContext;
        Config config;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Label9.Text = "Web Server:" + GetIPAddress();

                //string sTopicName = txtJefferiesTopicName.Text.EndsWith(";") ? txtJefferiesTopicName.Text.Substring(0, txtJefferiesTopicName.Text.Length - 1) : txtJefferiesTopicName.Text;
                //Int32 len = sTopicName.Split(new char[] { ';' }).Length;
                //for (int i = 0; i < len; i++)
                //{
                //    JefferiesExcuReportMQList.Add(TopicMQFactory.GetMQAdapterInstance(MQAdapterType.BatchMQAdapter));
                //}

                if (!IsPostBack)
                {
                    cboFormat.SelectedIndex = 0;
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
                OpenMQ();
                if (cboFormat.SelectedIndex == 0)
                {
                    SendData1();
                }
                else if (cboFormat.SelectedIndex == 1)
                {
                    SendData();
                }
                CloseMQ();
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
            //txtJefferiesTopicName.Text = config.jefferiesExcuReport_Sender_Topic;
            //txtOTATopicName.Text = config.otaExport_Sender_Topic;
            SetMessage(cboFormat.SelectedItem.Text);
        }
        private void InitialMQSetting()
        {
            applicationContext = ContextRegistry.GetContext();
            config = (Config)applicationContext.GetObject("Config");

            //string sTopicName = txtJefferiesTopicName.Text.EndsWith(";") ? txtJefferiesTopicName.Text.Substring(0, txtJefferiesTopicName.Text.Length - 1) : txtJefferiesTopicName.Text;

            //for (int i = 0; i < JefferiesExcuReportMQList.Count; i++)
            //{
            //    JefferiesExcuReportMQList[i].Uri = config.MQ_network + ":" + config.MQ_service;
            //    JefferiesExcuReportMQList[i].DestinationFeature = DestinationFeature.Topic;
            //    JefferiesExcuReportMQList[i].SendName = sTopicName.Split(new char[] { ';' })[i];
            //    JefferiesExcuReportMQList[i].UserName = config.MQUserID;
            //    JefferiesExcuReportMQList[i].PassWord = config.MQPwd;
            //    Session["JefferiesExcuReportMQ" + i] = JefferiesExcuReportMQList[i];
            //}
            //Session["MQCount"] = JefferiesExcuReportMQList.Count;

            JefferiesExcuReportMQ.Uri = config.MQ_network + ":" + config.MQ_service;
            //JefferiesExcuReportMQ.MessageTimeOut = (double)1 / 24 * (double)1 / 60;
            JefferiesExcuReportMQ.MessageTimeOut = 30;
            JefferiesExcuReportMQ.DestinationFeature = DestinationFeature.VirtualTopic;
            //JefferiesExcuReportMQ.SendName = txtJefferiesTopicName.Text.Trim();
            JefferiesExcuReportMQ.UserName = config.MQUserID;
            JefferiesExcuReportMQ.PassWord = config.MQPwd;

            OTAExportMQ.Uri = config.MQ_network + ":" + config.MQ_service;
            //OTAExportMQ.MessageTimeOut = (double)1 / 24 * (double)1 / 60;
            OTAExportMQ.DestinationFeature = DestinationFeature.VirtualTopic;
            OTAExportMQ.SendName = config.otaExport_Sender_Topic;
            OTAExportMQ.UserName = config.MQUserID;
            OTAExportMQ.PassWord = config.MQPwd;

            Session["JefferiesExcuReportMQ"] = JefferiesExcuReportMQ;
            Session["OTAExportMQ"] = OTAExportMQ;
        }
        //private void SendData()
        //{
        //    Dictionary<string, string> DicMap = Util.ToMessageMap(txtMessage.Text, Convert.ToChar((byte)0x01).ToString(), "=");
        //    List<List<MessageField>> MultiMQMessage = new List<List<MessageField>>();
        //    //後送實際的資料列
        //    for (int i = 0; i < Convert.ToInt32(txtMessageNums.Text); i++)
        //    {
        //        List<MessageField> MessageFields = new List<MessageField>();
        //        //加入資料序號
        //        MessageField MessageSeqenceField = new MessageField();
        //        MessageSeqenceField.Name = "9999";
        //        MessageSeqenceField.Value = (i + 1).ToString().PadLeft(txtMessageNums.Text.Length, '0');
        //        MessageFields.Add(MessageSeqenceField);
        //        //加入資料序號
        //        foreach (string Dic in DicMap.Keys)
        //        {
        //            MessageField MessageField = new MessageField();
        //            MessageField.Name = Dic;
        //            MessageField.Value = DicMap[Dic];
        //            MessageFields.Add(MessageField);
        //        }
        //        MultiMQMessage.Add(MessageFields);
        //    }
        //    JefferiesExcuReportMQ.SendMQMessage("710", MultiMQMessage);
        //    if (log.IsInfoEnabled) log.InfoFormat("Send JefferiesExcuReport Message from MQSender(Count:{0})", txtMessageNums.Text);
        //    OTAExportMQ.SendMQMessage("710", MultiMQMessage);
        //    if (log.IsInfoEnabled) log.InfoFormat("Send OTAExport Message from MQSender(Count:{0})", txtMessageNums.Text);
        //}

        private void SendData()
        {
            Dictionary<string, string> DicMap = Util.ToMessageMapForFix(txtMessage.Text, Convert.ToChar((byte)0x01).ToString(), "=");
            //後送實際的資料列
            for (int i = 0; i < Convert.ToInt32(txtMessageNums.Text); i++)
            {
                List<MessageField> MqMessage = new List<MessageField>();
                //加入資料序號
                MessageField MessageSeqenceField = new MessageField();
                MessageSeqenceField.Name = "9999";
                MessageSeqenceField.Value = "1";
                MqMessage.Add(MessageSeqenceField);
                //加入資料序號
                foreach (string Dic in DicMap.Keys)
                {
                    MessageField MessageField = new MessageField();
                    MessageField.Name = Dic;
                    MessageField.Value = DicMap[Dic];
                    MqMessage.Add(MessageField);
                }

                //foreach (IMQAdapter JefferiesExcuReportMQ in JefferiesExcuReportMQList)
                //{
                //    if (JefferiesExcuReportMQ.SendMQMessage("710", MqMessage))
                //    {
                //        if (log.IsInfoEnabled) log.Info("Send JefferiesExcuReport Message from MQSender");
                //    }
                //}

                if (JefferiesExcuReportMQ.SendMQMessage("710", MqMessage))
                {
                    if (log.IsInfoEnabled) log.Info("Send JefferiesExcuReport Message from MQSender");
                }
                if (OTAExportMQ.SendMQMessage("710", MqMessage))
                {
                    if (log.IsInfoEnabled) log.Info("Send OTAExport Message from MQSender");
                }
            }
        }

        private void SendData1()
        {
            //btn_Send1.Enabled = false;
            if (cboFormat.SelectedIndex == 0)
            {
                string sTopicName = txtJefferiesTopicName.Text.EndsWith(";") ? txtJefferiesTopicName.Text.Substring(0, txtJefferiesTopicName.Text.Length - 1) : txtJefferiesTopicName.Text;
                Int32 len = sTopicName.Split(new char[] { ';' }).Length;

                for (int h = 0; h < len; h++)
                {
                    JefferiesExcuReportMQ.SendName = sTopicName.Split(new char[] { ';' })[h];
                    JefferiesExcuReportMQ.ReStartSender(JefferiesExcuReportMQ.SendName);
                    List<Dictionary<string, string>> DicMapList = Util.ToMessageMapForJson(txtMessage.Text);
                    List<List<MessageField>> MultiMQMessage = new List<List<MessageField>>();
                    //後送實際的資料列
                    for (int i = 0; i < Convert.ToInt32(txtMessageNums.Text); i++)
                    {
                        foreach (Dictionary<string, string> DicMap in DicMapList)
                        {
                            List<MessageField> MqMessage = new List<MessageField>();

                            //加入訊息ID,主旨,傳送訊息時間
                            MessageField IDMessageField = new MessageField();
                            IDMessageField.Name = "PushMessageID";
                            IDMessageField.Value = Guid.NewGuid().ToString();
                            MqMessage.Add(IDMessageField);

                            MessageField SubMessageField = new MessageField();
                            SubMessageField.Name = "Subject";
                            SubMessageField.Value = txtOTATopicName.Text;
                            MqMessage.Add(SubMessageField);

                            MessageField SendTimeMessageField = new MessageField();
                            SendTimeMessageField.Name = "SendTime";
                            SendTimeMessageField.Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            MqMessage.Add(SendTimeMessageField);

                            //加入資料序號
                            MessageField MessageSeqenceField = new MessageField();
                            MessageSeqenceField.Name = "9999";
                            MessageSeqenceField.Value = "1";
                            MqMessage.Add(MessageSeqenceField);
                            //加入資料序號
                            foreach (string Dic in DicMap.Keys)
                            {
                                MessageField MessageField = new MessageField();
                                MessageField.Name = Dic;
                                MessageField.Value = DicMap[Dic];
                                MqMessage.Add(MessageField);
                            }
                            MultiMQMessage.Add(MqMessage);
                        }

                        //foreach (IMQAdapter JefferiesExcuReportMQ in JefferiesExcuReportMQList)
                        //{
                        //    JefferiesExcuReportMQ.SenderDBAction = GetSenderAction();
                        //    if (JefferiesExcuReportMQ.SendMQMessage("710", MultiMQMessage))
                        //    {
                        //        if (log.IsInfoEnabled) log.Info("Send JefferiesExcuReport Message from MQSender");
                        //    }
                        //}

                        if (JefferiesExcuReportMQ.SendMQMessage("710", MultiMQMessage))
                        {
                            if (log.IsInfoEnabled) log.Info("Send JefferiesExcuReport Message from MQSender");
                        }
                        MultiMQMessage.Clear();
                    }
                }
            }
            else if (cboFormat.SelectedIndex == 1)
            {
                Dictionary<string, string> DicMap = Util.ToMessageMapForFix(txtMessage.Text, Convert.ToChar((byte)0x01).ToString(), "=");
                //後送實際的資料列
                for (int i = 0; i < Convert.ToInt32(txtMessageNums.Text); i++)
                {
                    List<MessageField> MqMessage = new List<MessageField>();
                    //加入資料序號
                    MessageField MessageSeqenceField = new MessageField();
                    MessageSeqenceField.Name = "9999";
                    MessageSeqenceField.Value = "1";
                    MqMessage.Add(MessageSeqenceField);
                    //加入資料序號
                    foreach (string Dic in DicMap.Keys)
                    {
                        MessageField MessageField = new MessageField();
                        MessageField.Name = Dic;
                        MessageField.Value = DicMap[Dic];
                        MqMessage.Add(MessageField);
                    }

                    //foreach (IMQAdapter JefferiesExcuReportMQ in JefferiesExcuReportMQList)
                    //{
                    //    if (JefferiesExcuReportMQ.SendMQMessage("710", MqMessage))
                    //    {
                    //        if (log.IsInfoEnabled) log.Info("Send JefferiesExcuReport Message from MQSender");
                    //    }
                    //}

                    if (JefferiesExcuReportMQ.SendMQMessage("710", MqMessage))
                    {
                        if (log.IsInfoEnabled) log.Info("Send JefferiesExcuReport Message from MQSender");
                    }
                    if (OTAExportMQ.SendMQMessage("710", MqMessage))
                    {
                        if (log.IsInfoEnabled) log.Info("Send OTAExport Message from MQSender");
                    }
                }
            }
        }
        private void OpenMQ()
        {
            //int MQCount = (int)Session["MQCount"];
            //for (int i = 0; i < MQCount; i++)
            //{
            //    JefferiesExcuReportMQList[i] = (Session["JefferiesExcuReportMQ" + i] as IMQAdapter);
            //    JefferiesExcuReportMQList[i].Start();
            //}

            JefferiesExcuReportMQ = Session["JefferiesExcuReportMQ"] as IMQAdapter;
            OTAExportMQ = Session["OTAExportMQ"] as IMQAdapter;
            JefferiesExcuReportMQ.Start();
            OTAExportMQ.Start();
        }

        private void CloseMQ()
        {
            //int MQCount = (int)Session["MQCount"];
            //for (int i = 0; i < MQCount; i++)
            //{
            //    JefferiesExcuReportMQList[i] = (Session["JefferiesExcuReportMQ" + i] as IMQAdapter);
            //    JefferiesExcuReportMQList[i].Close();
            //}

            JefferiesExcuReportMQ = Session["JefferiesExcuReportMQ"] as IMQAdapter;
            OTAExportMQ = Session["OTAExportMQ"] as IMQAdapter;
            JefferiesExcuReportMQ.Close();
            OTAExportMQ.Close();
        }

        protected void cboFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMessage(cboFormat.SelectedItem.Text);
        }

        private void SetMessage(string MessageFormat)
        {
            if (MessageFormat.ToLower() == "json")
            {
                //int MQCount = (int)Session["MQCount"];
                //for (int i = 0; i < MQCount; i++)
                //{
                //    (Session["JefferiesExcuReportMQ" + i] as IMQAdapter).SenderDBAction = GetSenderAction();
                //}

                JefferiesExcuReportMQ.SenderDBAction = GetSenderAction();
                OTAExportMQ.SenderDBAction = GetSenderAction();
                LAS_TWEntities DB = new LAS_TWEntities();
                loanApplication_customer Customer = DB.loanApplication_customer.SingleOrDefault(row => row.pk == 23);
                ObjectContext objectContext = ((IObjectContextAdapter)DB).ObjectContext;
                ObjectSet<loanApplication_customer> set = objectContext.CreateObjectSet<loanApplication_customer>();
                List<string> keyNames = set.EntitySet.ElementType
                                                            .KeyMembers
                                                            .Select(k => k.Name).ToList<string>();

                // Json Encoding
                string sJson = JsonConvert.SerializeObject(Customer);
                // Json Decoding
                Customer = JsonConvert.DeserializeObject<loanApplication_customer>(sJson);
                txtMessage.Text = sJson;
            }
            else if (MessageFormat.ToLower() == "fix")
            {
                //int MQCount = (int)Session["MQCount"];
                //for (int i = 0; i < MQCount; i++)
                //{
                //    (Session["JefferiesExcuReportMQ" + i] as IMQAdapter).SenderDBAction = GetSenderAction();
                //}

                JefferiesExcuReportMQ.SenderDBAction = GetSenderAction();
                OTAExportMQ.SenderDBAction = DBAction.Query;
                txtMessage.Text = "10043=D-140717152215121=9900937.00000010010=3810018=964107=CHIA HSIN CEMENT55=110375=2014-07-17 00:00:00987=2014-07-19 00:00:0014=700044=15.9554=110032=TW0001103000789=11521=select * from APP_USER where USER_ID='leonlee'24=0151=16=058=Pending Replace:Pending Replace52=20140423-05:02:1357=tw137=m-003141=20140423tw1x1082.05911=20140423tw1x1082.06017=1037820=0150=E220=A221=A222=A223=A224=A225=A226=A227=A228=A229=A230=A231=A232=A233=A234=A235=A236=A237=A238=A239=A240=A241=A242=A243=A244=A245=A246=A247=A248=A249=A250=A251=A252=A253=A254=A255=A256=A257=A258=A259=A260=A261=A262=A263=A264=A265=A266=A267=A268=A269=A270=A271=A272=A273=A274=A275=A276=A277=A278=A";
            }
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
        private DBAction GetSenderAction()
        {
            DBAction DBAction = DBAction.None;
            switch (ddlAction.SelectedValue.ToLower())
            {
                case "query":
                    DBAction = DBAction.Query;
                    break;
                case "add":
                    DBAction = DBAction.Add;
                    break;
                case "update":
                    DBAction = DBAction.Update;
                    break;
                case "delete":
                    DBAction = DBAction.Delete;
                    break;
            }
            return DBAction;
        }
    }
}