﻿namespace MQDemoSubscriber
{
    partial class SelectTestCase
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
            this.label1 = new System.Windows.Forms.Label();
            this.CboTestCase = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "選擇測試案例";
            // 
            // CboTestCase
            // 
            this.CboTestCase.FormattingEnabled = true;
            this.CboTestCase.Items.AddRange(new object[] {
            "Tibco Server",
            "MQ Server",
            "EMS Server",
            "MQ Client"});
            this.CboTestCase.Location = new System.Drawing.Point(93, 23);
            this.CboTestCase.Name = "CboTestCase";
            this.CboTestCase.Size = new System.Drawing.Size(121, 20);
            this.CboTestCase.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(104, 49);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // SelectTestCase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(232, 83);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.CboTestCase);
            this.Controls.Add(this.label1);
            this.Name = "SelectTestCase";
            this.Text = "SelectTestCase";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SelectTestCase_FormClosed);
            this.Load += new System.EventHandler(this.SelectTestCase_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CboTestCase;
        private System.Windows.Forms.Button btnOK;
    }
}