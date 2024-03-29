﻿using Common;
using Common.LinkLayer;
using Common.Utility;
using DBContext;
using DBModels;
using Newtonsoft.Json;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Windows.Forms;

namespace MQDemoProducer
{
    public partial class EMSSender : Form
    {
        Form _MainForm;
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IEMSAdapter JefferiesExcuReportEMS = TopicEMSFactory.GetEMSAdapterInstance(EMSAdapterType.BatchEMSAdapter);
        IEMSAdapter OTAExportEMS = TopicEMSFactory.GetEMSAdapterInstance(EMSAdapterType.BatchEMSAdapter);
        int iMessageCount = 0;

        IApplicationContext applicationContext;
        Config config;

        DateTime EndSendTime;

        public EMSSender()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public EMSSender(Form MainForm)
        {
            InitializeComponent();
            this.CenterToScreen();
            _MainForm = MainForm;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                cboFormat.SelectedIndex = 0;
                cboDestinationFeature.SelectedIndex = 0;
                applicationContext = ContextRegistry.GetContext();
                config = (Config)applicationContext.GetObject("Config");

                txtURI.Text = config.EMS_network + ":" + config.EMS_service;
                txtUserName.Text = config.EMSUserID;
                txtPassword.Text = config.EMSPwd;
                txtJefferiesTopicName.Text = config.jefferiesExcuReport_Sender_Topic;
                txtOTATopicName.Text = config.otaExport_Sender_Topic;
                //Common Topic
                //JefferiesExcuReportEMS = new RequestEMSAdapter(txtURI.Text.Trim(), DestinationFeature.Topic, "", txtTopicName.Text.Trim(), txtUserName.Text.Trim(), txtPassword.Text.Trim());
                //OTAExportEMS = new RequestEMSAdapter(txtURI.Text.Trim(), DestinationFeature.Topic, "", config.otaExport_Sender_Topic, txtUserName.Text.Trim(), txtPassword.Text.Trim());


                JefferiesExcuReportEMS.Uri = config.EMS_network + ":" + config.EMS_service;
                JefferiesExcuReportEMS.DestinationFeature = cboDestinationFeature.SelectedIndex == 0 ? DestinationFeature.Topic : DestinationFeature.Queue;
                //JefferiesExcuReportEMS.ListenName = config.jefferiesExcuReport_Listener_Topic;
                JefferiesExcuReportEMS.SendName = config.jefferiesExcuReport_Sender_Topic;
                JefferiesExcuReportEMS.UserName = config.EMSUserID;
                JefferiesExcuReportEMS.PassWord = config.EMSPwd;
                JefferiesExcuReportEMS.UseSSL = config.EMS_useSSL;
                JefferiesExcuReportEMS.CertsPath = config.EMS_CertsPath;
                JefferiesExcuReportEMS.IsDurableConsumer = true;

                OTAExportEMS.Uri = config.EMS_network + ":" + config.EMS_service;
                OTAExportEMS.DestinationFeature = cboDestinationFeature.SelectedIndex == 0 ? DestinationFeature.Topic : DestinationFeature.Queue;
                //OTAExportEMS.ListenName = config.otaExport_Listener_Topic;
                OTAExportEMS.SendName = config.otaExport_Sender_Topic;
                OTAExportEMS.UserName = config.EMSUserID;
                OTAExportEMS.PassWord = config.EMSPwd;
                OTAExportEMS.UseSSL = config.EMS_useSSL;
                OTAExportEMS.CertsPath = config.EMS_CertsPath;
                OTAExportEMS.IsDurableConsumer = true;
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                JefferiesExcuReportEMS.Close();
                OTAExportEMS.Close();
                JefferiesExcuReportEMS.CloseSharedConnection();
                OTAExportEMS.CloseSharedConnection();
                _MainForm.Show();
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                Application.Exit();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //JefferiesExcuReportEMS.Start();
            //OTAExportEMS.Start();
            if (cboDestinationFeature.SelectedIndex == 0)
            {
                if (JefferiesExcuReportEMS.IsDurableConsumer)
                {
                    JefferiesExcuReportEMS.Start("Jefferies", true);
                }
                else
                {
                    JefferiesExcuReportEMS.Start();
                }
                if (OTAExportEMS.IsDurableConsumer)
                {
                    OTAExportEMS.Start("OTA", true);
                }
                else
                {
                    OTAExportEMS.Start();
                }
            }
            //Queue
            else if (cboDestinationFeature.SelectedIndex == 1)
            {
                JefferiesExcuReportEMS.Start();
                OTAExportEMS.Start();
            }
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            try
            {
                txtMessageCount.Text = "";
                SendData();
                SendFile();
                btn_Send.Enabled = true;
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }

        private void btn_Send1_Click(object sender, EventArgs e)
        {
            try
            {
                txtMessageCount.Text = "";
                Application.DoEvents();
                SendData1();
                btn_Send1.Enabled = true;
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }

        void JefferiesExcuReportEMS_EMSMessageAsynSendFinished(object sender, EventArgs e)
        {
            string s = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
            btn_Send.Enabled = true;
        }

        /// <summary>
        /// 將字串轉換成Dictionary
        /// </summary>
        /// <param name="Message">字串型態資料</param>
        /// <param name="DataSplitChar">資料分隔字元</param>
        /// <param name="DataMapChar">field和value分隔字元</param>
        /// <returns></returns>
        private Dictionary<string, string> ToMessageMap(string Message, string DataSplitChar, string DataMapChar)
        {
            Dictionary<string, string> MessageMap = new Dictionary<string, string>();
            IEnumerable<string> IEnumData = Message.Split(DataSplitChar.ToCharArray()).AsEnumerable();
            string TempLostData = "";
            foreach (string Data in IEnumData)
            {
                if (Data.Contains(DataMapChar))
                {
                    int fixTag;
                    string LeftStr = Data.Substring(0, Data.IndexOf("="));
                    if (int.TryParse(LeftStr, out fixTag))
                    {
                        if (TempLostData != "")
                        {
                            MessageMap[MessageMap.ElementAt(MessageMap.Count - 1).Key] += TempLostData;
                            TempLostData = "";
                        }
                        string[] AryKeyValue = Data.Split(DataMapChar.ToCharArray());
                        if (AryKeyValue.Length == 2 && AryKeyValue[0] != "" && AryKeyValue[1] != "")
                        {
                            MessageMap.Add(AryKeyValue[0], AryKeyValue[1]);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        TempLostData += " " + Data;
                    }
                }
                else
                {
                    TempLostData += " " + Data;
                    continue;
                }
            }
            if (TempLostData != "")
            {
                MessageMap[MessageMap.ElementAt(MessageMap.Count - 1).Key] += TempLostData;
            }
            return MessageMap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            DateTime NowTime = DateTime.Now;
            EndSendTime = NowTime.AddSeconds(Convert.ToDouble(SendSeconds.Value));
            iMessageCount = 0;
            txtMessageCount.Text = "";
            Application.DoEvents();
            while (EndSendTime > NowTime)
            {
                btn_Send_Click(this, null);
                txtMessageCount.Text = iMessageCount.ToString();
                Application.DoEvents();
                NowTime = DateTime.Now;
            }
            button1.Enabled = true;
        }
        private void SendData()
        {
            btn_Send.Enabled = false;
            if (cboFormat.SelectedIndex == 0)
            {
                List<Dictionary<string, string>> DicMapList = Util.ToMessageMapForJson(txtMessage.Text);
                List<List<MessageField>> MultiEMSMessage = new List<List<MessageField>>();
                //後送實際的資料列
                foreach (Dictionary<string, string> DicMap in DicMapList)
                {
                    for (int i = 0; i < MessageNums.Value; i++)
                    {
                        List<MessageField> MessageFields = new List<MessageField>();
                        //加入資料序號
                        MessageField MessageSeqenceField = new MessageField();
                        MessageSeqenceField.Name = "9999";
                        MessageSeqenceField.Value = (i + 1).ToString().PadLeft(MessageNums.Value.ToString().Length, '0');
                        MessageFields.Add(MessageSeqenceField);
                        //加入資料序號
                        foreach (string Dic in DicMap.Keys)
                        {
                            MessageField MessageField = new MessageField();
                            MessageField.Name = Dic;
                            MessageField.Value = DicMap[Dic];
                            MessageFields.Add(MessageField);
                        }
                        MultiEMSMessage.Add(MessageFields);
                    }
                }
                //JefferiesExcuReportEMS.SendEMSMessage("710", MultiEMSMessage, 20, 25);
                JefferiesExcuReportEMS.SendEMSMessage("710", MultiEMSMessage);
                iMessageCount += 1;
                txtMessageCount.Text = txtMessageCount.Text.Trim() == "" ? "1" : (Convert.ToInt32(txtMessageCount.Text.Trim()) + 1).ToString();
                //if (log.IsInfoEnabled) log.InfoFormat("Send JefferiesExcuReport Message from EMSSender(Count:{0})", MessageNums.Value);
                //OTAExportEMS.SendEMSMessage("710", MultiEMSMessage, 20, 25);
                OTAExportEMS.SendEMSMessage("710", MultiEMSMessage);
                iMessageCount += 1;
                txtMessageCount.Text = txtMessageCount.Text.Trim() == "" ? "1" : (Convert.ToInt32(txtMessageCount.Text.Trim()) + 1).ToString();
                //if (log.IsInfoEnabled) log.InfoFormat("Send OTAExport Message from EMSSender(Count:{0})", MessageNums.Value);
            }
            else if (cboFormat.SelectedIndex == 1)
            {
                Dictionary<string, string> DicMap = Util.ToMessageMapForFix(txtMessage.Text, Convert.ToChar((byte)0x01).ToString(), "=");
                List<List<MessageField>> MultiEMSMessage = new List<List<MessageField>>();
                //後送實際的資料列
                for (int i = 0; i < MessageNums.Value; i++)
                {
                    List<MessageField> MessageFields = new List<MessageField>();
                    //加入資料序號
                    MessageField MessageSeqenceField = new MessageField();
                    MessageSeqenceField.Name = "9999";
                    MessageSeqenceField.Value = (i + 1).ToString().PadLeft(MessageNums.Value.ToString().Length, '0');
                    MessageFields.Add(MessageSeqenceField);
                    //加入資料序號
                    foreach (string Dic in DicMap.Keys)
                    {
                        MessageField MessageField = new MessageField();
                        MessageField.Name = Dic;
                        MessageField.Value = DicMap[Dic];
                        MessageFields.Add(MessageField);
                    }
                    MultiEMSMessage.Add(MessageFields);
                }
                //JefferiesExcuReportEMS.SendEMSMessage("710", MultiEMSMessage, 30, 25);
                JefferiesExcuReportEMS.SendEMSMessage("710", MultiEMSMessage);
                iMessageCount += 1;
                txtMessageCount.Text = txtMessageCount.Text.Trim() == "" ? "1" : (Convert.ToInt32(txtMessageCount.Text.Trim()) + 1).ToString();
                //if (log.IsInfoEnabled) log.InfoFormat("Send JefferiesExcuReport Message from EMSSender(Count:{0})", MessageNums.Value);
                //OTAExportEMS.SendEMSMessage("710", MultiEMSMessage, 30, 25);
                OTAExportEMS.SendEMSMessage("710", MultiEMSMessage);
                iMessageCount += 1;
                txtMessageCount.Text = txtMessageCount.Text.Trim() == "" ? "1" : (Convert.ToInt32(txtMessageCount.Text.Trim()) + 1).ToString();
                //if (log.IsInfoEnabled) log.InfoFormat("Send OTAExport Message from EMSSender(Count:{0})", MessageNums.Value);
            }
        }
        private void SendFile()
        {
            //JefferiesExcuReportEMS.SendFile("08.pdf", @"C:\08.pdf");
            //JefferiesExcuReportEMS.SendFile("土地建物謄本.pdf", @"C:\土地建物謄本.pdf");
            //JefferiesExcuReportEMS.SendFile("IRR撥款計算表(合併).xlsx", @"C:\IRR撥款計算表(合併).xlsx");
            //JefferiesExcuReportEMS.SendFile("2018_02_06_142940_signed.pdf", @"C:\2018_02_06_142940_signed.pdf");
        }
        private void SendData1()
        {
            btn_Send1.Enabled = false;
            if (cboFormat.SelectedIndex == 0)
            {
                List<Dictionary<string, string>> DicMapList = Util.ToMessageMapForJson(txtMessage.Text);
                List<List<MessageField>> MultiEMSMessage = new List<List<MessageField>>();
                //後送實際的資料列
                for (int i = 0; i < MessageNums1.Value; i++)
                {
                    foreach (Dictionary<string, string> DicMap in DicMapList)
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
                        MultiEMSMessage.Add(MqMessage);
                    }
                    //JefferiesExcuReportEMS.SendEMSMessage("710", MultiEMSMessage, 3, 25);
                    JefferiesExcuReportEMS.SendEMSMessage("710", MultiEMSMessage);
                    txtMessageCount.Text = txtMessageCount.Text.Trim() == "" ? MultiEMSMessage.Count.ToString() : (Convert.ToInt32(txtMessageCount.Text) + MultiEMSMessage.Count).ToString();
                    Application.DoEvents();
                    MultiEMSMessage.Clear();
                    //if (log.IsInfoEnabled) log.Info("Send JefferiesExcuReport Message from EMSSender");
                }
            }
            else if (cboFormat.SelectedIndex == 1)
            {
                Dictionary<string, string> DicMap = Util.ToMessageMapForFix(txtMessage.Text, Convert.ToChar((byte)0x01).ToString(), "=");
                //後送實際的資料列
                for (int i = 0; i < MessageNums1.Value; i++)
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
                    //JefferiesExcuReportEMS.SendEMSMessage("710", MqMessage, 18, 25);
                    JefferiesExcuReportEMS.SendEMSMessage("710", MqMessage);
                    txtMessageCount.Text = (i + 1).ToString();
                    Application.DoEvents();
                    //if (log.IsInfoEnabled) log.Info("Send JefferiesExcuReport Message from EMSSender");
                }
            }
        }

        private void cboDestinationFeature_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.Text = cboDestinationFeature.SelectedIndex == 0 ? label4.Text.Replace("Queue", "Topic") : label4.Text.Replace("Topic", "Queue");
            label8.Text = cboDestinationFeature.SelectedIndex == 0 ? label8.Text.Replace("Queue", "Topic") : label8.Text.Replace("Topic", "Queue");
            JefferiesExcuReportEMS.DestinationFeature = cboDestinationFeature.SelectedIndex == 0 ? DestinationFeature.Topic : DestinationFeature.Queue;
            JefferiesExcuReportEMS.ReStartListener(JefferiesExcuReportEMS.ListenName);
            JefferiesExcuReportEMS.ReStartSender(JefferiesExcuReportEMS.SendName);
            OTAExportEMS.DestinationFeature = cboDestinationFeature.SelectedIndex == 0 ? DestinationFeature.Topic : DestinationFeature.Queue;
            OTAExportEMS.ReStartListener(OTAExportEMS.ListenName);
            OTAExportEMS.ReStartSender(OTAExportEMS.SendName);
        }

        private void cboFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboFormat.SelectedIndex == 0)
            {
                JefferiesExcuReportEMS.SenderDBAction = DBAction.Query;
                OTAExportEMS.SenderDBAction = DBAction.Query;
                LAS_TWEntities DB = new LAS_TWEntities();
                loanApplication_customer Customer = DB.loanApplication_customer.SingleOrDefault(row => row.pk == 21);
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
            else if (cboFormat.SelectedIndex == 1)
            {
                JefferiesExcuReportEMS.SenderDBAction = DBAction.Query;
                OTAExportEMS.SenderDBAction = DBAction.Query;
                txtMessage.Text = "10043=D-140717152215121=9900937.00000010010=3810018=964107=CHIA HSIN CEMENT55=110375=2014-07-17 00:00:00987=2014-07-19 00:00:0014=700044=15.9554=110032=TW0001103000789=11521=select * from APP_USER where USER_ID='leonlee'24=0151=16=058=Pending Replace:Pending Replace52=20140423-05:02:1357=tw137=m-003141=20140423tw1x1082.05911=20140423tw1x1082.06017=1037820=0150=E220=A221=A222=A223=A224=A225=A226=A227=A228=A229=A230=A231=A232=A233=A234=A235=A236=A237=A238=A239=A240=A241=A242=A243=A244=A245=A246=A247=A248=A249=A250=A251=A252=A253=A254=A255=A256=A257=A258=A259=A260=A261=A262=A263=A264=A265=A266=A267=A268=A269=A270=A271=A272=A273=A274=A275=A276=A277=A278=A";
            }
        }
    }
}
