using Common;
using Common.Dictionary;
using Common.HandlerLayer;
using Common.LinkLayer;
using DBModels;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MQDemoSubscriber
{
    public partial class EMSServer_Receive_Response_Test : Form
    {
        Form _MainForm;
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IEMSAdapter JefferiesExcuReportEMS = TopicEMSFactory.GetEMSAdapterInstance(EMSAdapterType.BatchEMSAdapter);
        JefferiesExcuReportHandler JefferiesExcuReportHandler;
        IEMSAdapter OTAExportEMS = TopicEMSFactory.GetEMSAdapterInstance(EMSAdapterType.BatchEMSAdapter);
        OTAExportHandler OTAExportHandler;

        IApplicationContext applicationContext;
        Config config;

        public EMSServer_Receive_Response_Test()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public EMSServer_Receive_Response_Test(Form MainForm)
        {
            InitializeComponent();
            this.CenterToScreen();
            _MainForm = MainForm;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                TopicController.RegisterWorkThreadsEvent();
                string s = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
                applicationContext = ContextRegistry.GetContext();
                config = (Config)applicationContext.GetObject("Config");
                txtURI.Text = config.EMS_network + ":" + config.EMS_service;
                txtUserName.Text = config.EMSUserID;
                txtPassword.Text = config.EMSPwd;
                txtReceivedJefferiesTopicName.Text = config.jefferiesExcuReport_Listener_Topic;
                txtReceivedOTATopicName.Text = config.otaExport_Listener_Topic;
                txtResponseJefferiesTopicName.Text = config.jefferiesExcuReport_Sender_Topic;
                txtResponseOTATopicName.Text = config.otaExport_Sender_Topic;

                JefferiesExcuReportEMS.UseSharedConnection = false;
                JefferiesExcuReportEMS.Uri = config.EMS_network + ":" + config.EMS_service;
                JefferiesExcuReportEMS.DestinationFeature = cboDestinationFeature.SelectedIndex == 0 ? DestinationFeature.Topic : DestinationFeature.Queue;
                JefferiesExcuReportEMS.ListenName = config.jefferiesExcuReport_Listener_Topic;
                JefferiesExcuReportEMS.SendName = config.jefferiesExcuReport_Sender_Topic;
                JefferiesExcuReportEMS.UserName = config.EMSUserID;
                JefferiesExcuReportEMS.PassWord = config.EMSPwd;
                JefferiesExcuReportEMS.UseSSL = config.EMS_useSSL;
                JefferiesExcuReportEMS.CertsPath = config.EMS_CertsPath;
                (JefferiesExcuReportEMS as BatchEMSAdapter).EMSBatchFinished += new BatchEMSAdapter.EMSBatchFinishedEventHandler(JefferiesExcuReportEMS_EMSBatchFinished);

                OTAExportEMS.UseSharedConnection = false;
                OTAExportEMS.Uri = config.EMS_network + ":" + config.EMS_service;
                OTAExportEMS.DestinationFeature = cboDestinationFeature.SelectedIndex == 0 ? DestinationFeature.Topic : DestinationFeature.Queue;
                OTAExportEMS.ListenName = config.otaExport_Listener_Topic;
                OTAExportEMS.SendName = config.otaExport_Sender_Topic;
                OTAExportEMS.UserName = config.EMSUserID;
                OTAExportEMS.PassWord = config.EMSPwd;
                OTAExportEMS.UseSSL = config.EMS_useSSL;
                OTAExportEMS.CertsPath = config.EMS_CertsPath;
                (OTAExportEMS as BatchEMSAdapter).EMSBatchFinished += new BatchEMSAdapter.EMSBatchFinishedEventHandler(OTAExportEMS_EMSBatchFinished);

                cboDestinationFeature.SelectedIndex = 0;

                this.Height = Convert.ToInt16(this.Height * 1.18);
                //int QQ = System.Text.Encoding.Unicode.GetByteCount("𨉼");
                //QQ = System.Text.Encoding.Unicode.GetByteCount("𨉼");
                string QQ = String2Unicode("𨉼");
                string OO = Unicode2String(QQ);
                string AA = "\ud860\ude7c";
                string PP = Char.ConvertToUtf32(AA, 0).ToString("X");
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //WorkThreads.LogActualMaxThreads(JefferiesExcuReportHandler);
                //WorkThreads.LogActualMaxThreads(OTAExportHandler);

                TopicController.UnregisterWorkThreadsEvent();
                (JefferiesExcuReportEMS as BatchEMSAdapter).EMSBatchFinished -= JefferiesExcuReportEMS_EMSBatchFinished;
                JefferiesExcuReportEMS.Close();
                JefferiesExcuReportEMS.CloseSharedConnection();
                JefferiesExcuReportHandler.Release();

                (OTAExportEMS as BatchEMSAdapter).EMSBatchFinished -= OTAExportEMS_EMSBatchFinished;
                OTAExportEMS.Close();
                OTAExportEMS.CloseSharedConnection();
                OTAExportHandler.Release();

                TopicController._EMSServer_Receive_Response_Test = null;
                if (_MainForm != null)
                {
                    _MainForm.Show();
                }
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
                Application.Exit();
            }
        }

        private void btnClearInfo_Click(object sender, EventArgs e)
        {
            txtMessageCount.Text = "";
            LvSystemInfo.Items.Clear();
        }

        void JefferiesExcuReportEMS_EMSBatchFinished(object sender, EMSBatchFinishedEventArgs e)
        {
            try
            {
                //JefferiesExcuReportHandler.WorkItemQueue.Enqueue(e.BatchResultTable);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }

        void OTAExportEMS_EMSBatchFinished(object sender, EMSBatchFinishedEventArgs e)
        {
            try
            {
                //OTAExportHandler.WorkItemQueue.Enqueue(e.BatchResultTable);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }

        void JefferiesExcuReportHandler_WorkItemQueue_CustomizedQueueCountUpdated(object sender, Common.Utility.CustomizedQueueCountUpdateEventArgs<DataTable> e)
        {
            lblJEFFUnhandledCount.Text = e.Count.ToString();
        }

        void OTAExportHandler_WorkItemQueue_CustomizedQueueCountUpdated(object sender, Common.Utility.CustomizedQueueCountUpdateEventArgs<DataTable> e)
        {
            lblOTAUnhandledCount.Text = e.Count.ToString();
        }

        private void cboDestinationFeature_SelectedIndexChanged(object sender, EventArgs e)
        {
            label4.Text = cboDestinationFeature.SelectedIndex == 0 ? label4.Text.Replace("Queue", "Topic") : label4.Text.Replace("Topic", "Queue");
            label5.Text = cboDestinationFeature.SelectedIndex == 0 ? label5.Text.Replace("Queue", "Topic") : label5.Text.Replace("Topic", "Queue");
            label6.Text = cboDestinationFeature.SelectedIndex == 0 ? label6.Text.Replace("Queue", "Topic") : label6.Text.Replace("Topic", "Queue");
            label7.Text = cboDestinationFeature.SelectedIndex == 0 ? label7.Text.Replace("Queue", "Topic") : label7.Text.Replace("Topic", "Queue");
            JefferiesExcuReportEMS.DestinationFeature = cboDestinationFeature.SelectedIndex == 0 ? DestinationFeature.Topic : DestinationFeature.Queue;
            JefferiesExcuReportEMS.ReStartListener(JefferiesExcuReportEMS.ListenName);
            JefferiesExcuReportEMS.ReStartSender(JefferiesExcuReportEMS.SendName);
            OTAExportEMS.DestinationFeature = cboDestinationFeature.SelectedIndex == 0 ? DestinationFeature.Topic : DestinationFeature.Queue;
            OTAExportEMS.ReStartListener(OTAExportEMS.ListenName);
            OTAExportEMS.ReStartSender(OTAExportEMS.SendName);
        }

        private void cboFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            JefferiesExcuReportEMS.Close();
            JefferiesExcuReportEMS.CloseSharedConnection();
            OTAExportEMS.Close();
            OTAExportEMS.CloseSharedConnection();

            JefferiesExcuReportHandler.Release();
            JefferiesExcuReportHandler = JefferiesExcuReportHandler.getSingleton(config.isUsingThreadPoolThreadForY77);
            JefferiesExcuReportHandler.ResponseTag = typeof(AppUserResponseTag);
            JefferiesExcuReportHandler.WorkItemQueue.CustomizedQueueCountUpdated += new Common.Utility.CustomizedQueue<DataTable>.CustomizedQueueCountUpdateEventHandler(JefferiesExcuReportHandler_WorkItemQueue_CustomizedQueueCountUpdated);
            JefferiesExcuReportEMS.Handler = JefferiesExcuReportHandler;

            OTAExportHandler.Release();
            OTAExportHandler = OTAExportHandler.getSingleton(config.isUsingThreadPoolThreadForOTA);
            OTAExportHandler.ResponseTag = typeof(AppUserResponseTag);
            OTAExportHandler.WorkItemQueue.CustomizedQueueCountUpdated += new Common.Utility.CustomizedQueue<DataTable>.CustomizedQueueCountUpdateEventHandler(OTAExportHandler_WorkItemQueue_CustomizedQueueCountUpdated);
            OTAExportEMS.Handler = OTAExportHandler;

            if (cboFormat.SelectedIndex == 0)
            {
                (JefferiesExcuReportEMS as BatchEMSAdapter).DataType = typeof(loanApplication_customer);
                JefferiesExcuReportEMS.ReceiverDBAction = DBAction.Query;
                TopicController.HandleTopicForJson<loanApplication_customer>(this, JefferiesExcuReportEMS);

                (OTAExportEMS as BatchEMSAdapter).DataType = typeof(loanApplication_customer);
                OTAExportEMS.ReceiverDBAction = DBAction.Query;
                TopicController.HandleTopicForJson<loanApplication_customer>(this, OTAExportEMS);
            }
            else if (cboFormat.SelectedIndex == 1)
            {
                (JefferiesExcuReportEMS as BatchEMSAdapter).DataType = typeof(JefferiesExcuResponseTag);
                JefferiesExcuReportEMS.ReceiverDBAction = DBAction.Query;
                TopicController.HandleTopic(this, JefferiesExcuReportEMS);

                (OTAExportEMS as BatchEMSAdapter).DataType = typeof(JefferiesExcuResponseTag);
                OTAExportEMS.ReceiverDBAction = DBAction.Query;
                TopicController.HandleTopic(this, OTAExportEMS);
            }
            //持久訂閱者
            JefferiesExcuReportEMS.Start("JefferiesDurableConsumer", true);
            OTAExportEMS.Start("OTADurableConsumer", true);
            //非持久訂閱者
            //JefferiesExcuReportEMS.Start();
            //OTAExportEMS.Start();
        }
        public string String2Unicode(string source)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(source);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }
        public string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(
                         source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        private void LvSystemInfo_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender != LvSystemInfo) return;

            if (e.Control && e.KeyCode == Keys.C)
                CopySelectedValuesToClipboard();
        }
        private void CopySelectedValuesToClipboard()
        {
            var builder = new StringBuilder();
            foreach (ListViewItem item in LvSystemInfo.SelectedItems)
                builder.AppendLine(item.SubItems[1].Text);

            Clipboard.SetText(builder.ToString());
        }
    }
}
