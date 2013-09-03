namespace ScutClientSimulator
{
    partial class Main
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
            this.ScutStatusLayer = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnRequest = new System.Windows.Forms.Button();
            this.txtServerID = new System.Windows.Forms.TextBox();
            this.txtGameType = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAction = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rtxtResponse = new System.Windows.Forms.RichTextBox();
            this.txtPid = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRetailID = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ScutStatusLayer
            // 
            this.ScutStatusLayer.Location = new System.Drawing.Point(0, 525);
            this.ScutStatusLayer.Name = "ScutStatusLayer";
            this.ScutStatusLayer.Size = new System.Drawing.Size(1004, 22);
            this.ScutStatusLayer.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.splitContainer1.Size = new System.Drawing.Size(1004, 522);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPwd);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtPid);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnClearLog);
            this.groupBox1.Controls.Add(this.btnRequest);
            this.groupBox1.Controls.Add(this.txtRetailID);
            this.groupBox1.Controls.Add(this.txtServerID);
            this.groupBox1.Controls.Add(this.txtGameType);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtAction);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtHost);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(397, 192);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Socket请求";
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(300, 151);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(84, 23);
            this.btnClearLog.TabIndex = 10;
            this.btnClearLog.Text = "清空输出";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnRequest
            // 
            this.btnRequest.Location = new System.Drawing.Point(71, 151);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(84, 23);
            this.btnRequest.TabIndex = 9;
            this.btnRequest.Text = "发送请求";
            this.btnRequest.UseVisualStyleBackColor = true;
            this.btnRequest.Click += new System.EventHandler(this.btnLuaRunPath_Click);
            // 
            // txtServerID
            // 
            this.txtServerID.Location = new System.Drawing.Point(329, 56);
            this.txtServerID.Name = "txtServerID";
            this.txtServerID.Size = new System.Drawing.Size(55, 21);
            this.txtServerID.TabIndex = 5;
            this.txtServerID.Text = "0";
            // 
            // txtGameType
            // 
            this.txtGameType.Location = new System.Drawing.Point(212, 56);
            this.txtGameType.Name = "txtGameType";
            this.txtGameType.Size = new System.Drawing.Size(55, 21);
            this.txtGameType.TabIndex = 4;
            this.txtGameType.Text = "0";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(329, 24);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(55, 21);
            this.txtPort.TabIndex = 2;
            this.txtPort.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(281, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "分服ID：";
            // 
            // txtAction
            // 
            this.txtAction.Location = new System.Drawing.Point(71, 56);
            this.txtAction.Name = "txtAction";
            this.txtAction.Size = new System.Drawing.Size(55, 21);
            this.txtAction.TabIndex = 3;
            this.txtAction.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(159, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "游戏ID：";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(71, 24);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(196, 21);
            this.txtHost.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(284, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "端口：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "请求协议：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtxtResponse);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.groupBox2.Size = new System.Drawing.Size(597, 519);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "消息输出";
            // 
            // rtxtResponse
            // 
            this.rtxtResponse.AutoWordSelection = true;
            this.rtxtResponse.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtResponse.Location = new System.Drawing.Point(5, 17);
            this.rtxtResponse.Name = "rtxtResponse";
            this.rtxtResponse.Size = new System.Drawing.Size(587, 499);
            this.rtxtResponse.TabIndex = 0;
            this.rtxtResponse.Text = "";
            this.rtxtResponse.WordWrap = false;
            // 
            // txtPid
            // 
            this.txtPid.Location = new System.Drawing.Point(71, 88);
            this.txtPid.Name = "txtPid";
            this.txtPid.Size = new System.Drawing.Size(196, 21);
            this.txtPid.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "通行证：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 124);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "密码：";
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(71, 121);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(196, 21);
            this.txtPwd.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(281, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "渠道号：";
            // 
            // txtRetailID
            // 
            this.txtRetailID.Location = new System.Drawing.Point(329, 88);
            this.txtRetailID.Name = "txtRetailID";
            this.txtRetailID.Size = new System.Drawing.Size(55, 21);
            this.txtRetailID.TabIndex = 7;
            this.txtRetailID.Text = "0000";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 547);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ScutStatusLayer);
            this.Name = "Main";
            this.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.Text = "Scut客户端模拟器（Lua）";
            this.Load += new System.EventHandler(this.Main_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip ScutStatusLayer;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRequest;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox rtxtResponse;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAction;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.TextBox txtGameType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtServerID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPid;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRetailID;
        private System.Windows.Forms.Label label8;
    }
}