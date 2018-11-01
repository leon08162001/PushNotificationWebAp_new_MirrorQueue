namespace MQDemoProducer
{
    partial class MQSender
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtURI = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.btn_Send = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtJefferiesTopicName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.MessageNums = new System.Windows.Forms.NumericUpDown();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SendSeconds = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtOTATopicName = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboDestinationFeature = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btn_Send1 = new System.Windows.Forms.Button();
            this.MessageNums1 = new System.Windows.Forms.NumericUpDown();
            this.cboFormat = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtMessageCount = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.MessageNums)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendSeconds)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MessageNums1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.AutoSize = true;
            this.btnConnect.Location = new System.Drawing.Point(120, 359);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(148, 29);
            this.btnConnect.TabIndex = 10;
            this.btnConnect.Text = "MQ Broker Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(453, 122);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.ReadOnly = true;
            this.txtPassword.Size = new System.Drawing.Size(245, 25);
            this.txtPassword.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 94);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "URI";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(103, 128);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(380, 128);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Password";
            // 
            // txtURI
            // 
            this.txtURI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtURI.Location = new System.Drawing.Point(179, 90);
            this.txtURI.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtURI.Name = "txtURI";
            this.txtURI.ReadOnly = true;
            this.txtURI.Size = new System.Drawing.Size(519, 25);
            this.txtURI.TabIndex = 4;
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserName.Location = new System.Drawing.Point(179, 122);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.ReadOnly = true;
            this.txtUserName.Size = new System.Drawing.Size(192, 25);
            this.txtUserName.TabIndex = 5;
            // 
            // btn_Send
            // 
            this.btn_Send.AutoSize = true;
            this.btn_Send.Location = new System.Drawing.Point(53, 60);
            this.btn_Send.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(100, 40);
            this.btn_Send.TabIndex = 11;
            this.btn_Send.Text = "Send";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 162);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "Send Jefferies Topic Name";
            // 
            // txtJefferiesTopicName
            // 
            this.txtJefferiesTopicName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtJefferiesTopicName.Location = new System.Drawing.Point(179, 158);
            this.txtJefferiesTopicName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtJefferiesTopicName.Name = "txtJefferiesTopicName";
            this.txtJefferiesTopicName.ReadOnly = true;
            this.txtJefferiesTopicName.Size = new System.Drawing.Size(519, 25);
            this.txtJefferiesTopicName.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 23);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 14;
            this.label5.Text = "發送筆數";
            // 
            // MessageNums
            // 
            this.MessageNums.AutoSize = true;
            this.MessageNums.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.MessageNums.Location = new System.Drawing.Point(85, 19);
            this.MessageNums.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MessageNums.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.MessageNums.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MessageNums.Name = "MessageNums";
            this.MessageNums.Size = new System.Drawing.Size(124, 25);
            this.MessageNums.TabIndex = 15;
            this.MessageNums.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(123, 254);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(573, 96);
            this.txtMessage.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(63, 259);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 15);
            this.label6.TabIndex = 17;
            this.label6.Text = "Message";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(53, 60);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 29);
            this.button1.TabIndex = 18;
            this.button1.Text = "連續時間發送";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SendSeconds
            // 
            this.SendSeconds.AutoSize = true;
            this.SendSeconds.Location = new System.Drawing.Point(141, 23);
            this.SendSeconds.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.SendSeconds.Maximum = new decimal(new int[] {
            7200,
            0,
            0,
            0});
            this.SendSeconds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SendSeconds.Name = "SendSeconds";
            this.SendSeconds.Size = new System.Drawing.Size(99, 25);
            this.SendSeconds.TabIndex = 19;
            this.SendSeconds.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 27);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 15);
            this.label7.TabIndex = 20;
            this.label7.Text = "發送持續時間(秒)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btn_Send);
            this.groupBox1.Controls.Add(this.MessageNums);
            this.groupBox1.Location = new System.Drawing.Point(5, 394);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(219, 101);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "批次發送測試";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.SendSeconds);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(460, 394);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(243, 101);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "多次發送測試";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(24, 197);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(142, 15);
            this.label8.TabIndex = 23;
            this.label8.Text = "Send OTA Topic Name";
            // 
            // txtOTATopicName
            // 
            this.txtOTATopicName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOTATopicName.Location = new System.Drawing.Point(179, 192);
            this.txtOTATopicName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtOTATopicName.Name = "txtOTATopicName";
            this.txtOTATopicName.ReadOnly = true;
            this.txtOTATopicName.Size = new System.Drawing.Size(519, 25);
            this.txtOTATopicName.TabIndex = 24;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(29, 2);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Size = new System.Drawing.Size(639, 64);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "說明：";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(11, 23);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(523, 34);
            this.label9.TabIndex = 0;
            this.label9.Text = "對於表單MQ Client Receive & Show Test來說,此程式為Server端的發送程式;對於表單MQ Server Receive & Resp" +
    "onse Test來說,此程式為Client端的發送程式";
            // 
            // cboDestinationFeature
            // 
            this.cboDestinationFeature.FormattingEnabled = true;
            this.cboDestinationFeature.Items.AddRange(new object[] {
            "Topic",
            "Queue",
            "VirtualTopic",
            "MirrorQueue"});
            this.cboDestinationFeature.Location = new System.Drawing.Point(384, 362);
            this.cboDestinationFeature.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cboDestinationFeature.Name = "cboDestinationFeature";
            this.cboDestinationFeature.Size = new System.Drawing.Size(141, 23);
            this.cboDestinationFeature.TabIndex = 29;
            this.cboDestinationFeature.SelectedIndexChanged += new System.EventHandler(this.cboDestinationFeature_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(289, 367);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 15);
            this.label10.TabIndex = 28;
            this.label10.Text = "Queue/Topic";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.btn_Send1);
            this.groupBox4.Controls.Add(this.MessageNums1);
            this.groupBox4.Location = new System.Drawing.Point(233, 395);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox4.Size = new System.Drawing.Size(219, 101);
            this.groupBox4.TabIndex = 30;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "多次發送測試";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 23);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 15);
            this.label11.TabIndex = 14;
            this.label11.Text = "發送筆數";
            // 
            // btn_Send1
            // 
            this.btn_Send1.AutoSize = true;
            this.btn_Send1.Location = new System.Drawing.Point(53, 60);
            this.btn_Send1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Send1.Name = "btn_Send1";
            this.btn_Send1.Size = new System.Drawing.Size(100, 40);
            this.btn_Send1.TabIndex = 11;
            this.btn_Send1.Text = "Send";
            this.btn_Send1.UseVisualStyleBackColor = true;
            this.btn_Send1.Click += new System.EventHandler(this.btn_Send1_Click);
            // 
            // MessageNums1
            // 
            this.MessageNums1.AutoSize = true;
            this.MessageNums1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.MessageNums1.Location = new System.Drawing.Point(85, 19);
            this.MessageNums1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MessageNums1.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.MessageNums1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MessageNums1.Name = "MessageNums1";
            this.MessageNums1.Size = new System.Drawing.Size(124, 25);
            this.MessageNums1.TabIndex = 15;
            this.MessageNums1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cboFormat
            // 
            this.cboFormat.FormattingEnabled = true;
            this.cboFormat.Items.AddRange(new object[] {
            "Json",
            "Fix"});
            this.cboFormat.Location = new System.Drawing.Point(179, 224);
            this.cboFormat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboFormat.Name = "cboFormat";
            this.cboFormat.Size = new System.Drawing.Size(83, 23);
            this.cboFormat.TabIndex = 32;
            this.cboFormat.SelectedIndexChanged += new System.EventHandler(this.cboFormat_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(66, 226);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 15);
            this.label12.TabIndex = 31;
            this.label12.Text = "Message Format";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(532, 367);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 15);
            this.label13.TabIndex = 33;
            this.label13.Text = "發送筆數";
            // 
            // txtMessageCount
            // 
            this.txtMessageCount.Location = new System.Drawing.Point(612, 361);
            this.txtMessageCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtMessageCount.Name = "txtMessageCount";
            this.txtMessageCount.ReadOnly = true;
            this.txtMessageCount.Size = new System.Drawing.Size(83, 25);
            this.txtMessageCount.TabIndex = 34;
            // 
            // MQSender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 499);
            this.Controls.Add(this.txtMessageCount);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cboFormat);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.cboDestinationFeature);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.txtOTATopicName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtJefferiesTopicName);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtURI);
            this.Controls.Add(this.txtUserName);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MQSender";
            this.Text = "MQSender";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MessageNums)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SendSeconds)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MessageNums1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtURI;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtJefferiesTopicName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown MessageNums;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown SendSeconds;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtOTATopicName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboDestinationFeature;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn_Send1;
        private System.Windows.Forms.NumericUpDown MessageNums1;
        private System.Windows.Forms.ComboBox cboFormat;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtMessageCount;
    }
}