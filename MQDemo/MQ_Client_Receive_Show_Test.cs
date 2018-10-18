using Common;
using Common.Dictionary;
using Common.LinkLayer;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Data;
using System.Windows.Forms;

namespace MQDemoSubscriber
{
    public partial class MQ_Client_Receive_Show_Test : Form
    {
        Form _MainForm;
        private DataTable DT;
        private readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //GenericMQAdapter GenericMQAdapter;
        RequestMQAdapter RequestMQAdapter;
        BatchMQAdapter BatchMQAdapter;

        IApplicationContext applicationContext;
        Config config;

        public MQ_Client_Receive_Show_Test()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        public MQ_Client_Receive_Show_Test(Form MainForm)
        {
            InitializeComponent();
            this.CenterToScreen();
            _MainForm = MainForm;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string s = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
            applicationContext = ContextRegistry.GetContext();
            config = (Config)applicationContext.GetObject("Config");
            txtURI.Text = config.MQ_network + ":" + config.MQ_service;
            txtUserName.Text = config.MQUserID;
            txtPassword.Text = config.MQPwd;
            txtTopicName.Text = config.jefferiesExcuReport_Listener_Topic;

            //Common Topic
            //RequestMQAdapter = RequestMQAdapter.getSingleton(txtURI.Text.Trim(), DestinationFeature.Topic, txtTopicName.Text.Trim(), "", txtUserName.Text.Trim(), txtPassword.Text.Trim());
            //Virtual Topic
            //RequestMQAdapter = RequestMQAdapter.getSingleton(txtURI.Text.Trim(), DestinationFeature.VirtualTopic, txtTopicName.Text.Trim(), "", txtUserName.Text.Trim(), txtPassword.Text.Trim());
            //Mirrored Queues
            //RequestMQAdapter = RequestMQAdapter.getSingleton(txtURI.Text.Trim(), DestinationFeature.MirroredQueues, txtTopicName.Text.Trim(), "", txtUserName.Text.Trim(), txtPassword.Text.Trim());
            
            //RequestMQAdapter.RequestID = "9b0808af-5069-467e-9f72-6a5af60a6f5c";
            //RequestMQAdapter.DataType = typeof(JefferiesExcuResponseTag);
            //RequestMQAdapter.MQMessageHandleFinished += new RequestMQAdapter.MQMessageHandleFinishedEventHandler(RequestMQAdapter_MQMessageHandleFinished);
            //RequestMQAdapter.MQResponseFinished += new RequestMQAdapter.MQResponseFinishedEventHandler(RequestMQAdapter_MQResponseFinished);
            //RequestMQAdapter.Start();

            //BatchMQAdapter = new BatchMQAdapter(txtURI.Text.Trim(), DestinationFeature.Topic, txtTopicName.Text.Trim(), "", txtUserName.Text.Trim(), txtPassword.Text.Trim());
            BatchMQAdapter = BatchMQAdapter.getSingleton(txtURI.Text.Trim(), DestinationFeature.Topic, txtTopicName.Text.Trim(), "", txtUserName.Text.Trim(), txtPassword.Text.Trim());
            BatchMQAdapter.DataType = typeof(JefferiesExcuResponseTag);
            BatchMQAdapter.IsEventInUIThread = true;
            BatchMQAdapter.MQMessageHandleFinished += new BatchMQAdapter.MQMessageHandleFinishedEventHandler(BatchMQAdapter_MQMessageHandleFinished);
            BatchMQAdapter.MQBatchFinished += new BatchMQAdapter.MQBatchFinishedEventHandler(BatchMQAdapter_MQBatchFinished);
            System.Runtime.Remoting.Messaging.CallContext.LogicalSetData("item", "item");
            BatchMQAdapter.Start();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //GenericMQAdapter.Close();
            //RequestMQAdapter.Close();
            BatchMQAdapter.MQMessageHandleFinished -= BatchMQAdapter_MQMessageHandleFinished;
            BatchMQAdapter.MQBatchFinished -= BatchMQAdapter_MQBatchFinished;
            BatchMQAdapter.Close();
            _MainForm.Show();
        }

        private void btnClearTable_Click(object sender, EventArgs e)
        {
            if (DT != null)
            {
                DT.Clear();
                lblCount.Text = "";
            }
        }

        void RequestMQAdapter_MQResponseFinished(object sender, MQResponseFinishedEventArgs e)
        {
            //this.UIThread(delegate()
            //{
            //    DataView DV = new DataView(e.ResponseResultTable);
            //    DV.Sort = "Sequence ASC";
            //    DataTable newDT = DV.ToTable();
            //    if (DT == null)
            //    {
            //        DT = newDT;
            //    }
            //    DT.Merge(newDT);

            //    dataGridView1.DataSource = DT;
            //    lblCount.Text = DT.Rows.Count.ToString();
            //});
            string s = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
            DataView DV = new DataView(e.ResponseResultTable);
            DV.Sort = "Sequence ASC";
            DataTable newDT = DV.ToTable();
            if (DT == null)
            {
                DT = newDT;
            }
            DT.Merge(newDT);

            dataGridView1.DataSource = DT;
            lblCount.Text = DT.Rows.Count.ToString();
        }

        void BatchMQAdapter_MQBatchFinished(object sender, MQBatchFinishedEventArgs e)
        {
            //this.UIThread(delegate()
            //{
            //    DataView DV = new DataView(e.BatchResultTable);
            //    DV.Sort = "Sequence ASC";
            //    DataTable newDT = DV.ToTable();
            //    if (DT == null)
            //    {
            //        DT = newDT;
            //    }
            //    DT.Merge(newDT);

            //    dataGridView1.DataSource = DT;
            //    lblCount.Text = DT.Rows.Count.ToString();
            //});
            string s = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
            string item = System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("item").ToString();

            DataView DV = new DataView(e.BatchResultTable);
            DV.Sort = "Sequence ASC";
            DataTable newDT = DV.ToTable();
            if (DT == null)
            {
                DT = newDT;
            }
            DT.Merge(newDT);

            dataGridView1.DataSource = DT;
            lblCount.Text = DT.Rows.Count.ToString();
        }

        void RequestMQAdapter_MQMessageHandleFinished(object sender, MQMessageHandleFinishedEventArgs e)
        {
            try
            {
                //bool IsFinished = RequestMQAdapter.IsResponseFinished;
                //if (e.errorMessage == "")
                //{
                //    if (DT == null)
                //    {
                //        DT = e.MessageRow.Table.Clone();
                //    }
                //    DT.ImportRow(e.MessageRow);
                //    //if (RequestMQAdapter.ListenName == "test")
                //    //{
                //    //    RequestMQAdapter.ListenName = "test1";
                //    //    RequestMQAdapter.Restart();
                //    //}
                //}
                //this.UIThread(delegate()
                //{
                //    dataGridView1.DataSource = DT;
                //    lblCount.Text = DT.Rows.Count.ToString();
                //});
            }
            catch (Exception ex)
            {
                string error;
                error = ex.Message;
            }
        }

        void BatchMQAdapter_MQMessageHandleFinished(object sender, MQMessageHandleFinishedEventArgs e)
        {
            try
            {
                //bool IsFinished = BatchMQAdapter.IsResponseFinished;
                //if (e.errorMessage == "")
                //{
                //    if (DT == null)
                //    {
                //        DT = e.MessageRow.Table.Clone();
                //    }
                //    DT.ImportRow(e.MessageRow);
                //    //if (BatchMQAdapter.ListenName == "test")
                //    //{
                //    //    BatchMQAdapter.ListenName = "test1";
                //    //    BatchMQAdapter.Restart();
                //    //}
                //}
                //this.UIThread(delegate()
                //{
                //    dataGridView1.DataSource = DT;
                //    lblCount.Text = DT.Rows.Count.ToString();
                //});
            }
            catch (Exception ex)
            {
                string error;
                error = ex.Message;
            }
        }

        //void GenericMQAdapter_MQMessageHandleFinished(object sender, MQMessageHandleFinishedEventArgs e)
        //{
        //    if (this != null && this.InvokeRequired == true)
        //    {
        //        if (e.errorMessage == "")
        //        {
        //            if (DT == null)
        //            {
        //                DT = e.MessageRow.Table.Clone();
        //            }
        //            DT.ImportRow(e.MessageRow);
        //        }
        //        BeginInvoke(new MethodInvoker(delegate()
        //        {
        //            GenericMQAdapter_MQMessageHandleFinished(sender, e);
        //        }));
        //    }
        //    else
        //    {
        //        this.dataGridView1.DataSource = DT;
        //    }
        //}
    }
}
