using Common;
using Common.Dictionary;
using Common.HandlerLayer;
using Common.LinkLayer;
using DBModels;
using MQDemo.Messager;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MQDemoSubscriber
{
    public partial class MQServer_Receive_Response_Test : Form
    {
        Form _MainForm;
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IMQAdapter JefferiesExcuReportMQ = TopicMQFactory.GetMQAdapterInstance(MQAdapterType.BatchMQAdapter);
        JefferiesExcuReportHandler JefferiesExcuReportHandler;
        IMQAdapter OTAExportMQ = TopicMQFactory.GetMQAdapterInstance(MQAdapterType.BatchMQAdapter);
        OTAExportHandler OTAExportHandler;

        IApplicationContext applicationContext;
        Config config;

        public MQServer_Receive_Response_Test()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public MQServer_Receive_Response_Test(Form MainForm)
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
                this.Text = this.Text + " " + config.MQClientID;
                txtURI.Text = config.MQ_network + ":" + config.MQ_service;
                txtUserName.Text = config.MQUserID;
                txtPassword.Text = config.MQPwd;
                txtReceivedJefferiesTopicName.Text = config.jefferiesExcuReport_Listener_Topic;
                txtReceivedOTATopicName.Text = config.otaExport_Listener_Topic;
                txtResponseJefferiesTopicName.Text = config.jefferiesExcuReport_Sender_Topic;
                txtResponseOTATopicName.Text = config.otaExport_Sender_Topic;

                JefferiesExcuReportMQ.UseSharedConnection = true;
                JefferiesExcuReportMQ.Uri = config.MQ_network + ":" + config.MQ_service;
                JefferiesExcuReportMQ.DestinationFeature = cboDestinationFeature.SelectedIndex == 0 ? DestinationFeature.Topic :
                    cboDestinationFeature.SelectedIndex == 1 ? DestinationFeature.Queue : cboDestinationFeature.SelectedIndex == 2 ? DestinationFeature.VirtualTopic : DestinationFeature.MirroredQueues;
                JefferiesExcuReportMQ.ListenName = config.jefferiesExcuReport_Listener_Topic;
                JefferiesExcuReportMQ.SendName = config.jefferiesExcuReport_Sender_Topic;
                JefferiesExcuReportMQ.UserName = config.MQUserID;
                JefferiesExcuReportMQ.PassWord = config.MQPwd;
                JefferiesExcuReportMQ.UseSSL = config.MQ_useSSL;
                JefferiesExcuReportMQ.IsDurableConsumer = true;

                (JefferiesExcuReportMQ as BatchMQAdapter).MQBatchFinished += new BatchMQAdapter.MQBatchFinishedEventHandler(JefferiesExcuReportMQ_MQBatchFinished);

                OTAExportMQ.UseSharedConnection = true;
                OTAExportMQ.Uri = config.MQ_network + ":" + config.MQ_service;
                OTAExportMQ.DestinationFeature = cboDestinationFeature.SelectedIndex == 0 ? DestinationFeature.Topic :
                    cboDestinationFeature.SelectedIndex == 1 ? DestinationFeature.Queue : cboDestinationFeature.SelectedIndex == 2 ? DestinationFeature.VirtualTopic : DestinationFeature.MirroredQueues;
                OTAExportMQ.ListenName = config.otaExport_Listener_Topic;
                OTAExportMQ.SendName = config.otaExport_Sender_Topic;
                OTAExportMQ.UserName = config.MQUserID;
                OTAExportMQ.PassWord = config.MQPwd;
                OTAExportMQ.UseSSL = config.MQ_useSSL;
                OTAExportMQ.IsDurableConsumer = true;

                (OTAExportMQ as BatchMQAdapter).MQBatchFinished += new BatchMQAdapter.MQBatchFinishedEventHandler(OTAExportMQ_MQBatchFinished);

                cboDestinationFeature.SelectedIndex = 0;

                this.Height = Convert.ToInt16(this.Height * 1.18);
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

                (JefferiesExcuReportMQ as BatchMQAdapter).MQBatchFinished -= JefferiesExcuReportMQ_MQBatchFinished;
                //JefferiesExcuReportMQ.Close();
                JefferiesExcuReportMQ.CloseSharedConnection();
                JefferiesExcuReportHandler.Release();

                (OTAExportMQ as BatchMQAdapter).MQBatchFinished -= OTAExportMQ_MQBatchFinished;
                //OTAExportMQ.Close();
                OTAExportMQ.CloseSharedConnection();
                OTAExportHandler.Release();

                TopicController._MQServer_Receive_Response_Test = null;
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

        void JefferiesExcuReportMQ_MQBatchFinished(object sender, MQBatchFinishedEventArgs e)
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

        void OTAExportMQ_MQBatchFinished(object sender, MQBatchFinishedEventArgs e)
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
            JefferiesExcuReportMQ.DestinationFeature = cboDestinationFeature.SelectedIndex == 0 ? DestinationFeature.Topic :
                    cboDestinationFeature.SelectedIndex == 1 ? DestinationFeature.Queue : cboDestinationFeature.SelectedIndex == 2 ? DestinationFeature.VirtualTopic : DestinationFeature.MirroredQueues;
            JefferiesExcuReportMQ.ReStartListener(JefferiesExcuReportMQ.ListenName);
            JefferiesExcuReportMQ.ReStartSender(JefferiesExcuReportMQ.SendName);
            OTAExportMQ.DestinationFeature = cboDestinationFeature.SelectedIndex == 0 ? DestinationFeature.Topic :
                    cboDestinationFeature.SelectedIndex == 1 ? DestinationFeature.Queue : cboDestinationFeature.SelectedIndex == 2 ? DestinationFeature.VirtualTopic : DestinationFeature.MirroredQueues;
            OTAExportMQ.ReStartListener(OTAExportMQ.ListenName);
            OTAExportMQ.ReStartSender(OTAExportMQ.SendName);
        }

        private void cboFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            JefferiesExcuReportMQ.Close();
            JefferiesExcuReportMQ.CloseSharedConnection();
            OTAExportMQ.Close();
            OTAExportMQ.CloseSharedConnection();

            JefferiesExcuReportHandler.Release();
            JefferiesExcuReportHandler = JefferiesExcuReportHandler.getSingleton(config.isUsingThreadPoolThreadForY77);
            JefferiesExcuReportHandler.ResponseTag = typeof(AppUserResponseTag);
            JefferiesExcuReportHandler.WorkItemQueue.CustomizedQueueCountUpdated += new Common.Utility.CustomizedQueue<DataTable>.CustomizedQueueCountUpdateEventHandler(JefferiesExcuReportHandler_WorkItemQueue_CustomizedQueueCountUpdated);
            JefferiesExcuReportMQ.Handler = JefferiesExcuReportHandler;

            OTAExportHandler.Release();
            OTAExportHandler = OTAExportHandler.getSingleton(config.isUsingThreadPoolThreadForOTA);
            OTAExportHandler.ResponseTag = typeof(AppUserResponseTag);
            OTAExportHandler.WorkItemQueue.CustomizedQueueCountUpdated += new Common.Utility.CustomizedQueue<DataTable>.CustomizedQueueCountUpdateEventHandler(OTAExportHandler_WorkItemQueue_CustomizedQueueCountUpdated);
            OTAExportMQ.Handler = OTAExportHandler;

            if (cboFormat.SelectedIndex == 0)
            {
                (JefferiesExcuReportMQ as BatchMQAdapter).DataType = typeof(loanApplication_customer);
                JefferiesExcuReportMQ.ReceiverDBAction = DBAction.Query;
                TopicController.HandleTopicForJson<loanApplication_customer>(this, JefferiesExcuReportMQ);
                //Receive Certificate Sign Data test begin
                //(JefferiesExcuReportMQ as BatchMQAdapter).DataType = typeof(SignMessage);
                //TopicController.HandleTopicForJson<SignMessage>(this, JefferiesExcuReportMQ);
                //Receive Certificate Sign Data test end

                (OTAExportMQ as BatchMQAdapter).DataType = typeof(loanApplication_customer);
                OTAExportMQ.ReceiverDBAction = DBAction.Query;
                TopicController.HandleTopicForJson<loanApplication_customer>(this, OTAExportMQ);
            }
            else if (cboFormat.SelectedIndex == 1)
            {
                (JefferiesExcuReportMQ as BatchMQAdapter).DataType = typeof(JefferiesExcuResponseTag);
                JefferiesExcuReportMQ.ReceiverDBAction = DBAction.Query;
                TopicController.HandleTopic(this, JefferiesExcuReportMQ);

                (OTAExportMQ as BatchMQAdapter).DataType = typeof(JefferiesExcuResponseTag);
                OTAExportMQ.ReceiverDBAction = DBAction.Query;
                TopicController.HandleTopic(this, OTAExportMQ);
            }
            //Topic
            if (cboDestinationFeature.SelectedIndex == 0)
            {
                if (JefferiesExcuReportMQ.IsDurableConsumer)
                {
                    JefferiesExcuReportMQ.Start("Jefferies" + config.MQClientID, true);
                }
                else
                {
                    JefferiesExcuReportMQ.Start();
                }
                if (OTAExportMQ.IsDurableConsumer)
                {
                    OTAExportMQ.Start("OTA" + config.MQClientID, true);
                }
                else
                {
                    OTAExportMQ.Start();
                }
            }
            //Queue
            else if (cboDestinationFeature.SelectedIndex == 1)
            {
                JefferiesExcuReportMQ.Start();
                OTAExportMQ.Start();
            }
            //VirtualTopic
            else if (cboDestinationFeature.SelectedIndex == 2)
            {
                JefferiesExcuReportMQ.Start("Jefferies" + config.MQClientID);
                OTAExportMQ.Start("OTA" + config.MQClientID);
            }
            //MirrorQueue
            else if (cboDestinationFeature.SelectedIndex == 3)
            {
                if (JefferiesExcuReportMQ.IsDurableConsumer)
                {
                    JefferiesExcuReportMQ.Start("Jefferies" + config.MQClientID, true);
                }
                else
                {
                    JefferiesExcuReportMQ.Start();
                }
                if (OTAExportMQ.IsDurableConsumer)
                {
                    OTAExportMQ.Start("OTA" + config.MQClientID, true);
                }
                else
                {
                    OTAExportMQ.Start();
                }
            }
            //持久訂閱者(Durable Topic)
            //JefferiesExcuReportMQ.Start("Jefferies" + config.MQClientID, true);
            //OTAExportMQ.Start("OTA" + config.MQClientID, true);
            //非持久訂閱者(VirtualTopic)
            //JefferiesExcuReportMQ.Start("Jefferies" + config.MQClientID);
            //OTAExportMQ.Start("OTA" + config.MQClientID);
            //一般Queue和Topic
            //JefferiesExcuReportMQ.Start();
            //OTAExportMQ.Start();
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
