namespace MQDemoSubscriber
{
    partial class TibcoServer_Receive_Response_Test
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
            this.components = new System.ComponentModel.Container();
            this.txtReceivedJefferiesTopicName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtURI = new System.Windows.Forms.TextBox();
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
            this.label2 = new System.Windows.Forms.Label();
            this.lblOTAUnhandledCount = new System.Windows.Forms.Label();
            this.lblJEFFUnhandledCount = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblOTA1UnhandledCount = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblOTA2UnhandledCount = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtReceivedJefferiesTopicName
            // 
            this.txtReceivedJefferiesTopicName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReceivedJefferiesTopicName.Location = new System.Drawing.Point(150, 78);
            this.txtReceivedJefferiesTopicName.Name = "txtReceivedJefferiesTopicName";
            this.txtReceivedJefferiesTopicName.ReadOnly = true;
            this.txtReceivedJefferiesTopicName.Size = new System.Drawing.Size(503, 22);
            this.txtReceivedJefferiesTopicName.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(105, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "Daemon";
            // 
            // txtURI
            // 
            this.txtURI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtURI.Location = new System.Drawing.Point(150, 50);
            this.txtURI.Name = "txtURI";
            this.txtURI.ReadOnly = true;
            this.txtURI.Size = new System.Drawing.Size(453, 22);
            this.txtURI.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 12);
            this.label5.TabIndex = 23;
            this.label5.Text = "Received Jefferies Topic Name";
            // 
            // txtReceivedOTATopicName
            // 
            this.txtReceivedOTATopicName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReceivedOTATopicName.Location = new System.Drawing.Point(150, 106);
            this.txtReceivedOTATopicName.Name = "txtReceivedOTATopicName";
            this.txtReceivedOTATopicName.ReadOnly = true;
            this.txtReceivedOTATopicName.Size = new System.Drawing.Size(503, 22);
            this.txtReceivedOTATopicName.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 12);
            this.label4.TabIndex = 25;
            this.label4.Text = "Received OTA Topic Name";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 12);
            this.label6.TabIndex = 27;
            this.label6.Text = "Response OTA Topic Name";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(150, 12);
            this.label7.TabIndex = 26;
            this.label7.Text = "Response Jefferies Topic Name";
            // 
            // txtResponseJefferiesTopicName
            // 
            this.txtResponseJefferiesTopicName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResponseJefferiesTopicName.Location = new System.Drawing.Point(150, 134);
            this.txtResponseJefferiesTopicName.Name = "txtResponseJefferiesTopicName";
            this.txtResponseJefferiesTopicName.ReadOnly = true;
            this.txtResponseJefferiesTopicName.Size = new System.Drawing.Size(503, 22);
            this.txtResponseJefferiesTopicName.TabIndex = 28;
            // 
            // txtResponseOTATopicName
            // 
            this.txtResponseOTATopicName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResponseOTATopicName.Location = new System.Drawing.Point(150, 162);
            this.txtResponseOTATopicName.Name = "txtResponseOTATopicName";
            this.txtResponseOTATopicName.ReadOnly = true;
            this.txtResponseOTATopicName.Size = new System.Drawing.Size(503, 22);
            this.txtResponseOTATopicName.TabIndex = 29;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(28, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(511, 40);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "說明：";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(362, 18);
            this.label9.TabIndex = 0;
            this.label9.Text = "對於表單TibcoSender來說,此程式為Server端的接收及處理回應程式;";
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
            this.LvSystemInfo.Location = new System.Drawing.Point(4, 232);
            this.LvSystemInfo.Name = "LvSystemInfo";
            this.LvSystemInfo.Size = new System.Drawing.Size(653, 242);
            this.LvSystemInfo.TabIndex = 31;
            this.LvSystemInfo.UseCompatibleStateImageBehavior = false;
            this.LvSystemInfo.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "DateTime";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "WorkThreads Info";
            this.columnHeader2.Width = 500;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 202);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 12);
            this.label8.TabIndex = 32;
            this.label8.Text = "Work Thread Info";
            // 
            // btnClearInfo
            // 
            this.btnClearInfo.Location = new System.Drawing.Point(100, 192);
            this.btnClearInfo.Name = "btnClearInfo";
            this.btnClearInfo.Size = new System.Drawing.Size(75, 23);
            this.btnClearInfo.TabIndex = 33;
            this.btnClearInfo.Text = "清除資訊";
            this.btnClearInfo.UseVisualStyleBackColor = true;
            this.btnClearInfo.Click += new System.EventHandler(this.btnClearInfo_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 193);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 12);
            this.label2.TabIndex = 34;
            this.label2.Text = "OTA待處理資料筆數:";
            // 
            // lblOTAUnhandledCount
            // 
            this.lblOTAUnhandledCount.AutoSize = true;
            this.lblOTAUnhandledCount.Location = new System.Drawing.Point(314, 193);
            this.lblOTAUnhandledCount.Name = "lblOTAUnhandledCount";
            this.lblOTAUnhandledCount.Size = new System.Drawing.Size(0, 12);
            this.lblOTAUnhandledCount.TabIndex = 35;
            // 
            // lblJEFFUnhandledCount
            // 
            this.lblJEFFUnhandledCount.AutoSize = true;
            this.lblJEFFUnhandledCount.Location = new System.Drawing.Point(314, 212);
            this.lblJEFFUnhandledCount.Name = "lblJEFFUnhandledCount";
            this.lblJEFFUnhandledCount.Size = new System.Drawing.Size(0, 12);
            this.lblJEFFUnhandledCount.TabIndex = 37;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(195, 212);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 12);
            this.label10.TabIndex = 36;
            this.label10.Text = "JEFF待處理資料筆數:";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblOTA1UnhandledCount
            // 
            this.lblOTA1UnhandledCount.AutoSize = true;
            this.lblOTA1UnhandledCount.Location = new System.Drawing.Point(469, 193);
            this.lblOTA1UnhandledCount.Name = "lblOTA1UnhandledCount";
            this.lblOTA1UnhandledCount.Size = new System.Drawing.Size(0, 12);
            this.lblOTA1UnhandledCount.TabIndex = 39;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(344, 193);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(121, 12);
            this.label11.TabIndex = 38;
            this.label11.Text = "OTA1待處理資料筆數:";
            // 
            // lblOTA2UnhandledCount
            // 
            this.lblOTA2UnhandledCount.AutoSize = true;
            this.lblOTA2UnhandledCount.Location = new System.Drawing.Point(469, 212);
            this.lblOTA2UnhandledCount.Name = "lblOTA2UnhandledCount";
            this.lblOTA2UnhandledCount.Size = new System.Drawing.Size(0, 12);
            this.lblOTA2UnhandledCount.TabIndex = 41;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(344, 212);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(121, 12);
            this.label12.TabIndex = 40;
            this.label12.Text = "OTA2待處理資料筆數:";
            // 
            // TibcoServer_Receive_Response_Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 474);
            this.Controls.Add(this.lblOTA2UnhandledCount);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblOTA1UnhandledCount);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblJEFFUnhandledCount);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblOTAUnhandledCount);
            this.Controls.Add(this.label2);
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
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtURI);
            this.Name = "TibcoServer_Receive_Response_Test";
            this.Text = "Tibco Server Receive & Response Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
            this.Load += new System.EventHandler(this.Form3_Load);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtReceivedJefferiesTopicName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtURI;
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblOTAUnhandledCount;
        private System.Windows.Forms.Label lblJEFFUnhandledCount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblOTA1UnhandledCount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblOTA2UnhandledCount;
        private System.Windows.Forms.Label label12;
    }
}