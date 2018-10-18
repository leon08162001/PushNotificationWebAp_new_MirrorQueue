using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Common;
using Common.Dictionary;
using Common.LinkLayer;
using Spring.Context;
using Spring.Context.Support;
using Common.HandlerLayer;
using System.Threading;

namespace MQDemoSubscriber
{
    public partial class TibcoServer_Receive_Response_Test : Form
    {
        Form _MainForm;
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ITibcoAdapter JefferiesExcuReportTibco = TopicTibcoFactory.GetTibcoAdapterInstance(TibcoAdapterType.BatchTibcoFixAdapter);
        ITibcoAdapter OTAExportTibco = TopicTibcoFactory.GetTibcoAdapterInstance(TibcoAdapterType.BatchTibcoFixAdapter);
        ITibcoAdapter OTA1ExportTibco = TopicTibcoFactory.GetTibcoAdapterInstance(TibcoAdapterType.BatchTibcoFixAdapter);
        ITibcoAdapter OTA2ExportTibco = TopicTibcoFactory.GetTibcoAdapterInstance(TibcoAdapterType.BatchTibcoFixAdapter);
        JefferiesExcuReportHandler JefferiesExcuReportHandler;
        OTAExportHandler OTAExportHandler;
        OTA1ExportHandler OTA1ExportHandler;
        OTA2ExportHandler OTA2ExportHandler;

        IApplicationContext applicationContext;
        Config config;

        public TibcoServer_Receive_Response_Test()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public TibcoServer_Receive_Response_Test(Form MainForm)
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
                txtURI.Text = config.Tibco_daemon;
                txtReceivedJefferiesTopicName.Text = config.jefferiesExcuReport_Listener_Topic;
                txtReceivedOTATopicName.Text = config.otaExport_Listener_Topic;
                txtResponseJefferiesTopicName.Text = config.jefferiesExcuReport_Sender_Topic;
                txtResponseOTATopicName.Text = config.otaExport_Sender_Topic;

                JefferiesExcuReportTibco.Service = config.Tibco_service;
                JefferiesExcuReportTibco.Network = config.Tibco_network;
                JefferiesExcuReportTibco.Daemon = config.Tibco_daemon;
                JefferiesExcuReportTibco.ListenName = config.jefferiesExcuReport_Listener_Topic;
                JefferiesExcuReportTibco.SendName = config.jefferiesExcuReport_Sender_Topic;
                (JefferiesExcuReportTibco as BatchTibcoFixAdapter).FixTagType = typeof(JefferiesExcuResponseTag);
                (JefferiesExcuReportTibco as BatchTibcoFixAdapter).TibcoBatchFinished += new BatchTibcoFixAdapter.TibcoBatchFinishedEventHandler(JefferiesExcuReportTibco_TibcoBatchFinished);
                JefferiesExcuReportTibco.initialize("JefferiesExcuReport");
                JefferiesExcuReportTibco.Start();

                JefferiesExcuReportHandler = JefferiesExcuReportHandler.getSingleton(config.isUsingThreadPoolThreadForY77);
                JefferiesExcuReportHandler.Priority = ThreadPriority.Highest;
                JefferiesExcuReportHandler.WorkItemQueue.CustomizedQueueCountUpdated += new Common.Utility.CustomizedQueue<DataTable>.CustomizedQueueCountUpdateEventHandler(JefferiesExcuReportHandler_WorkItemQueue_CustomizedQueueCountUpdated);
                TopicController.HandleTopic(this, JefferiesExcuReportHandler, JefferiesExcuReportTibco);

                OTAExportTibco.Service = config.Tibco_service;
                OTAExportTibco.Network = config.Tibco_network;
                OTAExportTibco.Daemon = config.Tibco_daemon;
                OTAExportTibco.ListenName = config.otaExport_Listener_Topic;
                OTAExportTibco.SendName = config.otaExport_Sender_Topic;
                (OTAExportTibco as BatchTibcoFixAdapter).FixTagType = typeof(OTAExportResponseTag);
                (OTAExportTibco as BatchTibcoFixAdapter).TibcoBatchFinished += new BatchTibcoFixAdapter.TibcoBatchFinishedEventHandler(OTAExportTibco_TibcoBatchFinished);
                OTAExportTibco.initialize("OTAExport");
                OTAExportTibco.Start();

                OTAExportHandler = OTAExportHandler.getSingleton(config.isUsingThreadPoolThreadForOTA);
                OTAExportHandler.Priority = ThreadPriority.AboveNormal;
                OTAExportHandler.WorkItemQueue.CustomizedQueueCountUpdated += new Common.Utility.CustomizedQueue<DataTable>.CustomizedQueueCountUpdateEventHandler(OTAExportHandler_WorkItemQueue_CustomizedQueueCountUpdated);
                TopicController.HandleTopic(this, OTAExportHandler, OTAExportTibco);

                OTA1ExportTibco.Service = config.Tibco_service;
                OTA1ExportTibco.Network = config.Tibco_network;
                OTA1ExportTibco.Daemon = config.Tibco_daemon;
                OTA1ExportTibco.ListenName = config.ota1Export_Listener_Topic;
                OTA1ExportTibco.SendName = config.ota1Export_Sender_Topic;
                (OTA1ExportTibco as BatchTibcoFixAdapter).FixTagType = typeof(JefferiesExcuResponseTag);
                (OTA1ExportTibco as BatchTibcoFixAdapter).TibcoBatchFinished += new BatchTibcoFixAdapter.TibcoBatchFinishedEventHandler(OTA1ExportTibco_TibcoBatchFinished);
                OTA1ExportTibco.initialize("OTA1Export");
                OTA1ExportTibco.Start();

                OTA1ExportHandler = OTA1ExportHandler.getSingleton(config.isUsingThreadPoolThreadForOTA1);
                OTA1ExportHandler.WorkItemQueue.CustomizedQueueCountUpdated += new Common.Utility.CustomizedQueue<DataTable>.CustomizedQueueCountUpdateEventHandler(OTA1ExportHandler_WorkItemQueue_CustomizedQueueCountUpdated);
                TopicController.HandleTopic(this, OTA1ExportHandler, OTA1ExportTibco);

                OTA2ExportTibco.Service = config.Tibco_service;
                OTA2ExportTibco.Network = config.Tibco_network;
                OTA2ExportTibco.Daemon = config.Tibco_daemon;
                OTA2ExportTibco.ListenName = config.ota2Export_Listener_Topic;
                OTA2ExportTibco.SendName = config.ota2Export_Sender_Topic;
                (OTA2ExportTibco as BatchTibcoFixAdapter).FixTagType = typeof(JefferiesExcuResponseTag);
                (OTA2ExportTibco as BatchTibcoFixAdapter).TibcoBatchFinished += new BatchTibcoFixAdapter.TibcoBatchFinishedEventHandler(OTA2ExportTibco_TibcoBatchFinished);
                OTA2ExportTibco.initialize("OTA2Export");
                OTA2ExportTibco.Start();

                OTA2ExportHandler = OTA2ExportHandler.getSingleton(config.isUsingThreadPoolThreadForOTA2);
                OTA2ExportHandler.WorkItemQueue.CustomizedQueueCountUpdated += new Common.Utility.CustomizedQueue<DataTable>.CustomizedQueueCountUpdateEventHandler(OTA2ExportHandler_WorkItemQueue_CustomizedQueueCountUpdated);
                TopicController.HandleTopic(this, OTA2ExportHandler, OTA2ExportTibco);
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
                WorkThreads.LogActualMaxThreads(JefferiesExcuReportHandler);
                WorkThreads.LogActualMaxThreads(OTAExportHandler);
                TopicController.UnregisterWorkThreadsEvent();
                (JefferiesExcuReportTibco as BatchTibcoFixAdapter).TibcoBatchFinished -= JefferiesExcuReportTibco_TibcoBatchFinished;
                JefferiesExcuReportTibco.Close();
                (OTAExportTibco as BatchTibcoFixAdapter).TibcoBatchFinished -= OTAExportTibco_TibcoBatchFinished;
                OTAExportTibco.Close();
                (OTA1ExportTibco as BatchTibcoFixAdapter).TibcoBatchFinished -= OTA1ExportTibco_TibcoBatchFinished;
                OTA1ExportTibco.Close();
                (OTA2ExportTibco as BatchTibcoFixAdapter).TibcoBatchFinished -= OTA2ExportTibco_TibcoBatchFinished;
                OTA2ExportTibco.Close();
                TopicController._TibcoServer_Receive_Response_Test = null;
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
            LvSystemInfo.Items.Clear();
        }

        void JefferiesExcuReportTibco_TibcoBatchFinished(object sender, TibcoBatchFinishedEventArgs e)
        {
            try
            {
                JefferiesExcuReportHandler.WorkItemQueue.Enqueue(e.BatchResultTable);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }

        void OTAExportTibco_TibcoBatchFinished(object sender, TibcoBatchFinishedEventArgs e)
        {
            try
            {
                OTAExportHandler.WorkItemQueue.Enqueue(e.BatchResultTable);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }

        void OTA1ExportTibco_TibcoBatchFinished(object sender, TibcoBatchFinishedEventArgs e)
        {
            try
            {
                OTA1ExportHandler.WorkItemQueue.Enqueue(e.BatchResultTable);
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message, ex);
            }
        }

        void OTA2ExportTibco_TibcoBatchFinished(object sender, TibcoBatchFinishedEventArgs e)
        {
            try
            {
                OTA2ExportHandler.WorkItemQueue.Enqueue(e.BatchResultTable);
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

        void OTA1ExportHandler_WorkItemQueue_CustomizedQueueCountUpdated(object sender, Common.Utility.CustomizedQueueCountUpdateEventArgs<DataTable> e)
        {
            lblOTA1UnhandledCount.Text = e.Count.ToString();
        }

        void OTA2ExportHandler_WorkItemQueue_CustomizedQueueCountUpdated(object sender, Common.Utility.CustomizedQueueCountUpdateEventArgs<DataTable> e)
        {
            lblOTA2UnhandledCount.Text = e.Count.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblJEFFUnhandledCount.Text = JefferiesExcuReportHandler.WorkItemQueue.Count().ToString();
            lblOTAUnhandledCount.Text = OTAExportHandler.WorkItemQueue.Count().ToString();
        }
    }
}
