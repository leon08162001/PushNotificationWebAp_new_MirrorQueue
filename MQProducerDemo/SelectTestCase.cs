using System;
using System.Windows.Forms;

namespace MQDemoProducer
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
                this.Hide();
                MQSender MQSender = new MQSender(this);
                MQSender.Show();
            }
            else if (CboTestCase.SelectedIndex == 1)
            {
                //this.Hide();
                //TibcoSender TibcoSender = new TibcoSender(this);
                //TibcoSender.Show();
            }
            else if (CboTestCase.SelectedIndex == 2)
            {
                this.Hide();
                EMSSender EMSSender = new EMSSender(this);
                EMSSender.Show();
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
