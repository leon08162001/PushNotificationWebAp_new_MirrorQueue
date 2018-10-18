namespace MQDemoSubscriber
{
    public partial class EMSServer_Receive_Response_Test
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtReceivedJefferiesTopicName = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtURI = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtReceivedOTATopicName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtResponseJefferiesTopicName = new System.Windows.Forms.TextBox();
            this.txtResponseOTATopicName = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.LvSystemInfo = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label8 = new System.Windows.Forms.Label();
            this.btnClearInfo = new System.Windows.Forms.Button();
            this.lblJEFFUnhandledCount = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblOTAUnhandledCount = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cboDestinationFeature = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cboFormat = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtMessageCount = new System.Windows.Forms.TextBox();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtReceivedJefferiesTopicName
            // 
            this.txtReceivedJefferiesTopicName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReceivedJefferiesTopicName.Location = new System.Drawing.Point(179, 139);
            this.txtReceivedJefferiesTopicName.Name = "txtReceivedJefferiesTopicName";
            this.txtReceivedJefferiesTopicName.ReadOnly = true;
            this.txtReceivedJefferiesTopicName.Size = new System.Drawing.Size(491, 23);
            this.txtReceivedJefferiesTopicName.TabIndex = 21;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(502, 52);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.ReadOnly = true;
            this.txtPassword.Size = new System.Drawing.Size(106, 23);
            this.txtPassword.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 14);
            this.label1.TabIndex = 19;
            this.label1.Text = "URI";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 14);
            this.label2.TabIndex = 18;
            this.label2.Text = "Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(443, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 14);
            this.label3.TabIndex = 16;
            this.label3.Text = "Password";
            // 
            // txtURI
            // 
            this.txtURI.Location = new System.Drawing.Point(30, 54);
            this.txtURI.Name = "txtURI";
            this.txtURI.ReadOnly = true;
            this.txtURI.Size = new System.Drawing.Size(248, 23);
            this.txtURI.TabIndex = 15;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(344, 53);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.ReadOnly = true;
            this.txtUserName.Size = new System.Drawing.Size(94, 23);
            this.txtUserName.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(177, 14);
            this.label5.TabIndex = 23;
            this.label5.Text = "Received Jefferies Topic Name";
            // 
            // txtReceivedOTATopicName
            // 
            this.txtReceivedOTATopicName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReceivedOTATopicName.Location = new System.Drawing.Point(179, 168);
            this.txtReceivedOTATopicName.Name = "txtReceivedOTATopicName";
            this.txtReceivedOTATopicName.ReadOnly = true;
            this.txtReceivedOTATopicName.Size = new System.Drawing.Size(491, 23);
            this.txtReceivedOTATopicName.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(159, 14);
            this.label4.TabIndex = 25;
            this.label4.Text = "Received OTA Topic Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 233);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(160, 14);
            this.label6.TabIndex = 27;
            this.label6.Text = "Response OTA Topic Name";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 204);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(178, 14);
            this.label7.TabIndex = 26;
            this.label7.Text = "Response Jefferies Topic Name";
            // 
            // txtResponseJefferiesTopicName
            // 
            this.txtResponseJefferiesTopicName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResponseJefferiesTopicName.Location = new System.Drawing.Point(179, 198);
            this.txtResponseJefferiesTopicName.Name = "txtResponseJefferiesTopicName";
            this.txtResponseJefferiesTopicName.ReadOnly = true;
            this.txtResponseJefferiesTopicName.Size = new System.Drawing.Size(491, 23);
            this.txtResponseJefferiesTopicName.TabIndex = 28;
            // 
            // txtResponseOTATopicName
            // 
            this.txtResponseOTATopicName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResponseOTATopicName.Location = new System.Drawing.Point(179, 230);
            this.txtResponseOTATopicName.Name = "txtResponseOTATopicName";
            this.txtResponseOTATopicName.ReadOnly = true;
            this.txtResponseOTATopicName.Size = new System.Drawing.Size(491, 23);
            this.txtResponseOTATopicName.TabIndex = 29;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(33, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(425, 43);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "說明：";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(9, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(408, 20);
            this.label9.TabIndex = 0;
            this.label9.Text = "對於表單EMSSender來說,此程式為Server端的接收及處理回應程式;";
            // 
            // LvSystemInfo
            // 
            this.LvSystemInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LvSystemInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.LvSystemInfo.FullRowSelect = true;
            this.LvSystemInfo.GridLines = true;
            this.LvSystemInfo.Location = new System.Drawing.Point(5, 302);
            this.LvSystemInfo.Name = "LvSystemInfo";
            this.LvSystemInfo.Size = new System.Drawing.Size(666, 262);
            this.LvSystemInfo.TabIndex = 31;
            this.LvSystemInfo.UseCompatibleStateImageBehavior = false;
            this.LvSystemInfo.View = System.Windows.Forms.View.Details;
            this.LvSystemInfo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LvSystemInfo_KeyUp);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "DateTime";
            this.columnHeader1.Width = 160;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "WorkThreads Info";
            this.columnHeader2.Width = 500;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 272);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 14);
            this.label8.TabIndex = 32;
            this.label8.Text = "Work Thread Info";
            // 
            // btnClearInfo
            // 
            this.btnClearInfo.Location = new System.Drawing.Point(117, 262);
            this.btnClearInfo.Name = "btnClearInfo";
            this.btnClearInfo.Size = new System.Drawing.Size(87, 25);
            this.btnClearInfo.TabIndex = 33;
            this.btnClearInfo.Text = "清除資訊";
            this.btnClearInfo.UseVisualStyleBackColor = true;
            this.btnClearInfo.Click += new System.EventHandler(this.btnClearInfo_Click);
            // 
            // lblJEFFUnhandledCount
            // 
            this.lblJEFFUnhandledCount.AutoSize = true;
            this.lblJEFFUnhandledCount.Location = new System.Drawing.Point(366, 282);
            this.lblJEFFUnhandledCount.Name = "lblJEFFUnhandledCount";
            this.lblJEFFUnhandledCount.Size = new System.Drawing.Size(0, 14);
            this.lblJEFFUnhandledCount.TabIndex = 41;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(227, 282);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(136, 14);
            this.label10.TabIndex = 40;
            this.label10.Text = "JEFF待處理資料筆數:";
            // 
            // lblOTAUnhandledCount
            // 
            this.lblOTAUnhandledCount.AutoSize = true;
            this.lblOTAUnhandledCount.Location = new System.Drawing.Point(366, 262);
            this.lblOTAUnhandledCount.Name = "lblOTAUnhandledCount";
            this.lblOTAUnhandledCount.Size = new System.Drawing.Size(0, 14);
            this.lblOTAUnhandledCount.TabIndex = 39;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(227, 262);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(135, 14);
            this.label11.TabIndex = 38;
            this.label11.Text = "OTA待處理資料筆數:";
            // 
            // cboDestinationFeature
            // 
            this.cboDestinationFeature.FormattingEnabled = true;
            this.cboDestinationFeature.Items.AddRange(new object[] {
            "Topic",
            "Queue"});
            this.cboDestinationFeature.Location = new System.Drawing.Point(179, 111);
            this.cboDestinationFeature.Name = "cboDestinationFeature";
            this.cboDestinationFeature.Size = new System.Drawing.Size(124, 21);
            this.cboDestinationFeature.TabIndex = 43;
            this.cboDestinationFeature.SelectedIndexChanged += new System.EventHandler(this.cboDestinationFeature_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(99, 113);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 14);
            this.label12.TabIndex = 42;
            this.label12.Text = "Queue/Topic";
            // 
            // cboFormat
            // 
            this.cboFormat.FormattingEnabled = true;
            this.cboFormat.Items.AddRange(new object[] {
            "Json",
            "Fix"});
            this.cboFormat.Location = new System.Drawing.Point(180, 83);
            this.cboFormat.Margin = new System.Windows.Forms.Padding(2);
            this.cboFormat.Name = "cboFormat";
            this.cboFormat.Size = new System.Drawing.Size(73, 21);
            this.cboFormat.TabIndex = 45;
            this.cboFormat.SelectedIndexChanged += new System.EventHandler(this.cboFormat_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(80, 84);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 14);
            this.label13.TabIndex = 44;
            this.label13.Text = "Message Format";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(526, 268);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(63, 14);
            this.label14.TabIndex = 51;
            this.label14.Text = "處理筆數";
            // 
            // txtMessageCount
            // 
            this.txtMessageCount.Location = new System.Drawing.Point(595, 264);
            this.txtMessageCount.Name = "txtMessageCount";
            this.txtMessageCount.ReadOnly = true;
            this.txtMessageCount.Size = new System.Drawing.Size(75, 23);
            this.txtMessageCount.TabIndex = 50;
            // 
            // EMSServer_Receive_Response_Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 476);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtMessageCount);
            this.Controls.Add(this.cboFormat);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cboDestinationFeature);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblJEFFUnhandledCount);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblOTAUnhandledCount);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnClearInfo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.LvSystemInfo);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.txtResponseOTATopicName);
            this.Controls.Add(this.txtResponseJefferiesTopicName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtReceivedOTATopicName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtReceivedJefferiesTopicName);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtURI);
            this.Controls.Add(this.txtUserName);
            this.Name = "EMSServer_Receive_Response_Test";
            this.Text = "EMS Server Receive & Response Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
            this.Load += new System.EventHandler(this.Form3_Load);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtReceivedJefferiesTopicName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtURI;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtReceivedOTATopicName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtResponseJefferiesTopicName;
        private System.Windows.Forms.TextBox txtResponseOTATopicName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.ListView LvSystemInfo;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnClearInfo;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label lblJEFFUnhandledCount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblOTAUnhandledCount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboDestinationFeature;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cboFormat;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.TextBox txtMessageCount;
    }
}