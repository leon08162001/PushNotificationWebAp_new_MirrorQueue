using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Common.Dictionary;
using Common.Utility;
using Common.LinkLayer;
using Common.HandlerLayer;
using Spring.Context;
using Spring.Context.Support;


namespace MQDemoProducer
{
    public partial class TibcoSender : Form
    {
        Form _MainForm;
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ITibcoAdapter JefferiesExcuReportTibco = TopicTibcoFactory.GetTibcoAdapterInstance(TibcoAdapterType.BatchTibcoFixAdapter);
        ITibcoAdapter OTAExportTibco = TopicTibcoFactory.GetTibcoAdapterInstance(TibcoAdapterType.BatchTibcoFixAdapter);
        //ITibcoAdapter OTA1ExportTibco = TopicTibcoFactory.GetTibcoAdapterInstance(TibcoAdapterType.BatchTibcoFixAdapter);
        //ITibcoAdapter OTA2ExportTibco = TopicTibcoFactory.GetTibcoAdapterInstance(TibcoAdapterType.BatchTibcoFixAdapter);

        IApplicationContext applicationContext;
        Config config;

        DateTime EndSendTime;

        public TibcoSender()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public TibcoSender(Form MainForm)
        {
            InitializeComponent();
            this.CenterToScreen();
            _MainForm = MainForm;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                applicationContext = ContextRegistry.GetContext();
                config = (Config)applicationContext.GetObject("Config");

                txtURI.Text = config.Tibco_daemon;
                txtJefferiesTopicName.Text = config.jefferiesExcuReport_Sender_Topic;
                txtOTATopicName.Text = config.otaExport_Sender_Topic;
                //txtMessage.Text = "10043=D-140717152215121=9900937.00000010010=3810018=964107=CHIA HSIN CEMENT55=110375=2014-07-17 00:00:00987=2014-07-19 00:00:0014=700044=15.9554=110032=TW0001103000789=11521=select * from APP_USER where USER_ID='leonlee'";
                txtMessage.Text = "10043=D-140717152215121=9900937.00000010010=3810018=964107=CHIA HSIN CEMENT55=110375=2014-07-17 00:00:00987=2014-07-19 00:00:0014=700044=15.9554=110032=TW0001103000789=11521=select * from APP_USER where USER_ID='leonlee'24=0151=16=058=Pending Replace:Pending Replace52=20140423-05:02:1357=tw137=m-003141=20140423tw1x1082.05911=20140423tw1x1082.06017=1037820=0150=E220=A221=A222=A223=A224=A225=A226=A227=A228=A229=A230=A231=A232=A233=A234=A235=A236=A237=A238=A239=A240=A241=A242=A243=A244=A245=A246=A247=A248=A249=A250=A251=A252=A253=A254=A255=A256=A257=A258=A259=A260=A261=A262=A263=A264=A265=A266=A267=A268=A269=A270=A271=A272=A273=A274=A275=A276=A277=A278=A";
                JefferiesExcuReportTibco.Service = config.Tibco_service;
                JefferiesExcuReportTibco.Network = config.Tibco_network;
                JefferiesExcuReportTibco.Daemon = config.Tibco_daemon;
                JefferiesExcuReportTibco.ListenName = "AAA";
                JefferiesExcuReportTibco.SendName = config.jefferiesExcuReport_Sender_Topic;
                JefferiesExcuReportTibco.initialize("JefferiesExcuReport");

                OTAExportTibco.Service = config.Tibco_service;
                OTAExportTibco.Network = config.Tibco_network;
                OTAExportTibco.Daemon = config.Tibco_daemon;
                OTAExportTibco.ListenName = "BBB";
                OTAExportTibco.SendName = config.otaExport_Sender_Topic;
                OTAExportTibco.initialize("OTAExport");

                //OTA1ExportTibco.Service = config.Tibco_service;
                //OTA1ExportTibco.Network = config.Tibco_network;
                //OTA1ExportTibco.Daemon = config.Tibco_daemon;
                //OTA1ExportTibco.ListenName = "CCC";
                //OTA1ExportTibco.SendName = config.ota1Export_Sender_Topic;
                //OTA1ExportTibco.initialize("OTA1Export");

                //OTA2ExportTibco.Service = config.Tibco_service;
                //OTA2ExportTibco.Network = config.Tibco_network;
                //OTA2ExportTibco.Daemon = config.Tibco_daemon;
                //OTA2ExportTibco.ListenName = "DDD";
                //OTA2ExportTibco.SendName = config.ota2Export_Sender_Topic;
                //OTA2ExportTibco.initialize("OTA2Export");
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
                JefferiesExcuReportTibco.Close();
                OTAExportTibco.Close();
                //OTA1ExportTibco.Close();
                //OTA2ExportTibco.Close();
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
            JefferiesExcuReportTibco.Start();
            OTAExportTibco.Start();
            //OTA1ExportTibco.Start();
            //OTA2ExportTibco.Start();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            try
            {
                SendData();
                btn_Send.Enabled = true;
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }

        void JefferiesExcuReportTibco_TibcoMessageAsynSendFinished(object sender, EventArgs e)
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
        private  Dictionary<string, string> ToMessageMap(string Message, string DataSplitChar, string DataMapChar)
        {
            Dictionary<string, string> MessageMap = new Dictionary<string, string>();
            IEnumerable<string> IEnumData = Message.Split(DataSplitChar.ToCharArray()).AsEnumerable();
            string TempLostData = "";
            foreach (string Data in IEnumData)
            {
                if (Data.Contains(DataMapChar))
                {
                    int fixTag;
                    string LeftStr = Data.Substring(0,Data.IndexOf("="));
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
                            TempLostData = "";
                            for (int i = 1; i < AryKeyValue.Length; i++)
                            {
                                TempLostData += AryKeyValue[i] + "=";
                            }
                            TempLostData = TempLostData.Substring(0, TempLostData.Length - 1);
                            MessageMap.Add(AryKeyValue[0], TempLostData);
                            TempLostData = "";
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
            while (EndSendTime > NowTime)
            {
                btn_Send_Click(this, null);
                NowTime = DateTime.Now;
                button1.Enabled = true;
            }
        }

        private void SendData()
        {
            btn_Send.Enabled = false;
            Dictionary<string, string> DicMap = Util.ToMessageMap(txtMessage.Text, Convert.ToChar((byte)0x01).ToString(), "=");
            List<List<MessageField>> MultiTibcoMessage = new List<List<MessageField>>();
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
                MultiTibcoMessage.Add(MessageFields);
            }
            JefferiesExcuReportTibco.SendTibcoMessage("710", MultiTibcoMessage);
            if (log.IsInfoEnabled) log.InfoFormat("Send JefferiesExcuReport Message from TibcoSender(Count:{0})", MessageNums.Value);
            OTAExportTibco.SendTibcoMessage("710", MultiTibcoMessage);
            if (log.IsInfoEnabled) log.InfoFormat("Send OTAExport Message from TibcoSender(Count:{0})", MessageNums.Value);
            //OTA1ExportTibco.SendTibcoMessage("710", MultiTibcoMessage);
            //if (log.IsInfoEnabled) log.InfoFormat("Send OTA1Export Message from TibcoSender(Count:{0})", MessageNums.Value);
            //OTA2ExportTibco.SendTibcoMessage("710", MultiTibcoMessage);
            //if (log.IsInfoEnabled) log.InfoFormat("Send OTA2Export Message from TibcoSender(Count:{0})", MessageNums.Value);
        }
    }
}
