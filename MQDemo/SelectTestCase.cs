using System;
using System.Windows.Forms;

namespace MQDemoSubscriber
{
    public partial class SelectTestCase : Form
    {
        public SelectTestCase()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void SelectTestCase_Load(object sender, EventArgs e)
        {
            CboTestCase.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (CboTestCase.SelectedIndex == 0)
            {
                //this.Hide();
                //TibcoServer_Receive_Response_Test TibcoServer = new TibcoServer_Receive_Response_Test(this);
                //TibcoServer.Show();
            }
            else if (CboTestCase.SelectedIndex == 1)
            {
                this.Hide();
                MQServer_Receive_Response_Test MQServer = new MQServer_Receive_Response_Test(this);
                MQServer.Show();
            }
            else if (CboTestCase.SelectedIndex == 2)
            {
                this.Hide();
                EMSServer_Receive_Response_Test EMSServer = new EMSServer_Receive_Response_Test(this);
                EMSServer.Show();
            }
            else if (CboTestCase.SelectedIndex == 3)
            {
                this.Hide();
                MQ_Client_Receive_Show_Test MQClient = new MQ_Client_Receive_Show_Test(this);
                MQClient.Show();
            }
        }

        private void SelectTestCase_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
