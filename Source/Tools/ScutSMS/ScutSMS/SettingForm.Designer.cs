namespace Scut.SMS
{
    partial class SettingForm
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
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageEntityModel = new System.Windows.Forms.TabPage();
            this.entityGroupBox = new System.Windows.Forms.GroupBox();
            this.btnEntityOpen2 = new System.Windows.Forms.Button();
            this.btnEntityOpen1 = new System.Windows.Forms.Button();
            this.btnEntityImport = new System.Windows.Forms.Button();
            this.txtEntityAsmPath = new System.Windows.Forms.TextBox();
            this.radioEntityAsmPath = new System.Windows.Forms.RadioButton();
            this.radioEntityPath = new System.Windows.Forms.RadioButton();
            this.txtEntityPath = new System.Windows.Forms.TextBox();
            this.tabPageContract = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCaseOutpath = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCaseOutPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCaseName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ddDbType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtContractDatabase = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtContractPwd = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtContractUid = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblConServer = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtContractServer = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageEntityModel.SuspendLayout();
            this.entityGroupBox.SuspendLayout();
            this.tabPageContract.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 383);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(596, 45);
            this.panel2.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(418, 10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(512, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Location = new System.Drawing.Point(0, 2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(596, 385);
            this.panel1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageEntityModel);
            this.tabControl1.Controls.Add(this.tabPageContract);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(5, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(586, 375);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPageEntityModel
            // 
            this.tabPageEntityModel.AutoScroll = true;
            this.tabPageEntityModel.Controls.Add(this.entityGroupBox);
            this.tabPageEntityModel.Location = new System.Drawing.Point(4, 22);
            this.tabPageEntityModel.Name = "tabPageEntityModel";
            this.tabPageEntityModel.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEntityModel.Size = new System.Drawing.Size(578, 349);
            this.tabPageEntityModel.TabIndex = 0;
            this.tabPageEntityModel.Text = "Entity Model";
            this.tabPageEntityModel.UseVisualStyleBackColor = true;
            // 
            // entityGroupBox
            // 
            this.entityGroupBox.Controls.Add(this.btnEntityOpen2);
            this.entityGroupBox.Controls.Add(this.btnEntityOpen1);
            this.entityGroupBox.Controls.Add(this.btnEntityImport);
            this.entityGroupBox.Controls.Add(this.txtEntityAsmPath);
            this.entityGroupBox.Controls.Add(this.radioEntityAsmPath);
            this.entityGroupBox.Controls.Add(this.radioEntityPath);
            this.entityGroupBox.Controls.Add(this.txtEntityPath);
            this.entityGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.entityGroupBox.Location = new System.Drawing.Point(3, 3);
            this.entityGroupBox.Name = "entityGroupBox";
            this.entityGroupBox.Size = new System.Drawing.Size(572, 139);
            this.entityGroupBox.TabIndex = 0;
            this.entityGroupBox.TabStop = false;
            this.entityGroupBox.Text = "Entity Model";
            // 
            // btnEntityOpen2
            // 
            this.btnEntityOpen2.Location = new System.Drawing.Point(418, 60);
            this.btnEntityOpen2.Name = "btnEntityOpen2";
            this.btnEntityOpen2.Size = new System.Drawing.Size(40, 23);
            this.btnEntityOpen2.TabIndex = 3;
            this.btnEntityOpen2.Text = "...";
            this.btnEntityOpen2.UseVisualStyleBackColor = true;
            this.btnEntityOpen2.Click += new System.EventHandler(this.btnEntityOpen2_Click);
            // 
            // btnEntityOpen1
            // 
            this.btnEntityOpen1.Location = new System.Drawing.Point(418, 26);
            this.btnEntityOpen1.Name = "btnEntityOpen1";
            this.btnEntityOpen1.Size = new System.Drawing.Size(40, 23);
            this.btnEntityOpen1.TabIndex = 3;
            this.btnEntityOpen1.Text = "...";
            this.btnEntityOpen1.UseVisualStyleBackColor = true;
            this.btnEntityOpen1.Click += new System.EventHandler(this.btnEntityOpen1_Click);
            // 
            // btnEntityImport
            // 
            this.btnEntityImport.Location = new System.Drawing.Point(19, 97);
            this.btnEntityImport.Name = "btnEntityImport";
            this.btnEntityImport.Size = new System.Drawing.Size(75, 23);
            this.btnEntityImport.TabIndex = 2;
            this.btnEntityImport.Text = "Import";
            this.btnEntityImport.UseVisualStyleBackColor = true;
            this.btnEntityImport.Click += new System.EventHandler(this.btnEntityImport_Click);
            // 
            // txtEntityAsmPath
            // 
            this.txtEntityAsmPath.Location = new System.Drawing.Point(174, 62);
            this.txtEntityAsmPath.Name = "txtEntityAsmPath";
            this.txtEntityAsmPath.Size = new System.Drawing.Size(236, 21);
            this.txtEntityAsmPath.TabIndex = 1;
            // 
            // radioEntityAsmPath
            // 
            this.radioEntityAsmPath.AutoSize = true;
            this.radioEntityAsmPath.Location = new System.Drawing.Point(19, 63);
            this.radioEntityAsmPath.Name = "radioEntityAsmPath";
            this.radioEntityAsmPath.Size = new System.Drawing.Size(149, 16);
            this.radioEntityAsmPath.TabIndex = 0;
            this.radioEntityAsmPath.TabStop = true;
            this.radioEntityAsmPath.Text = "Import Assembly Path:";
            this.radioEntityAsmPath.UseVisualStyleBackColor = true;
            this.radioEntityAsmPath.CheckedChanged += new System.EventHandler(this.radioEntityAsmPath_CheckedChanged);
            // 
            // radioEntityPath
            // 
            this.radioEntityPath.AutoSize = true;
            this.radioEntityPath.Location = new System.Drawing.Point(19, 29);
            this.radioEntityPath.Name = "radioEntityPath";
            this.radioEntityPath.Size = new System.Drawing.Size(137, 16);
            this.radioEntityPath.TabIndex = 0;
            this.radioEntityPath.TabStop = true;
            this.radioEntityPath.Text = "Import Script Path:";
            this.radioEntityPath.UseVisualStyleBackColor = true;
            this.radioEntityPath.CheckedChanged += new System.EventHandler(this.radioEntityPath_CheckedChanged);
            // 
            // txtEntityPath
            // 
            this.txtEntityPath.Location = new System.Drawing.Point(174, 28);
            this.txtEntityPath.Name = "txtEntityPath";
            this.txtEntityPath.Size = new System.Drawing.Size(236, 21);
            this.txtEntityPath.TabIndex = 1;
            // 
            // tabPageContract
            // 
            this.tabPageContract.AutoScroll = true;
            this.tabPageContract.Controls.Add(this.groupBox2);
            this.tabPageContract.Controls.Add(this.groupBox1);
            this.tabPageContract.Location = new System.Drawing.Point(4, 22);
            this.tabPageContract.Name = "tabPageContract";
            this.tabPageContract.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageContract.Size = new System.Drawing.Size(578, 349);
            this.tabPageContract.TabIndex = 1;
            this.tabPageContract.Text = "Contract";
            this.tabPageContract.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCaseOutpath);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtCaseOutPath);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtCaseName);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 193);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(572, 105);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Test Case";
            // 
            // btnCaseOutpath
            // 
            this.btnCaseOutpath.Location = new System.Drawing.Point(441, 60);
            this.btnCaseOutpath.Name = "btnCaseOutpath";
            this.btnCaseOutpath.Size = new System.Drawing.Size(40, 23);
            this.btnCaseOutpath.TabIndex = 9;
            this.btnCaseOutpath.Text = "...";
            this.btnCaseOutpath.UseVisualStyleBackColor = true;
            this.btnCaseOutpath.Click += new System.EventHandler(this.btnCaseOutpath_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "Case output path:";
            // 
            // txtCaseOutPath
            // 
            this.txtCaseOutPath.Location = new System.Drawing.Point(174, 62);
            this.txtCaseOutPath.Name = "txtCaseOutPath";
            this.txtCaseOutPath.Size = new System.Drawing.Size(260, 21);
            this.txtCaseOutPath.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "Case Name Format:";
            // 
            // txtCaseName
            // 
            this.txtCaseName.Location = new System.Drawing.Point(174, 31);
            this.txtCaseName.Name = "txtCaseName";
            this.txtCaseName.Size = new System.Drawing.Size(260, 21);
            this.txtCaseName.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ddDbType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtContractDatabase);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtContractPwd);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtContractUid);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lblConServer);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.txtContractServer);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(572, 190);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Proxy Server";
            // 
            // ddDbType
            // 
            this.ddDbType.FormattingEnabled = true;
            this.ddDbType.Items.AddRange(new object[] {
            "SQL",
            "MySql"});
            this.ddDbType.Location = new System.Drawing.Point(174, 21);
            this.ddDbType.Name = "ddDbType";
            this.ddDbType.Size = new System.Drawing.Size(260, 20);
            this.ddDbType.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "Database Name:";
            // 
            // txtContractDatabase
            // 
            this.txtContractDatabase.Location = new System.Drawing.Point(174, 144);
            this.txtContractDatabase.Name = "txtContractDatabase";
            this.txtContractDatabase.Size = new System.Drawing.Size(260, 21);
            this.txtContractDatabase.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Login Password:";
            // 
            // txtContractPwd
            // 
            this.txtContractPwd.Location = new System.Drawing.Point(174, 114);
            this.txtContractPwd.Name = "txtContractPwd";
            this.txtContractPwd.PasswordChar = '*';
            this.txtContractPwd.Size = new System.Drawing.Size(260, 21);
            this.txtContractPwd.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Login Acount:";
            // 
            // txtContractUid
            // 
            this.txtContractUid.Location = new System.Drawing.Point(174, 85);
            this.txtContractUid.Name = "txtContractUid";
            this.txtContractUid.Size = new System.Drawing.Size(260, 21);
            this.txtContractUid.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(327, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Port:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "Database Type:";
            // 
            // lblConServer
            // 
            this.lblConServer.AutoSize = true;
            this.lblConServer.Location = new System.Drawing.Point(17, 59);
            this.lblConServer.Name = "lblConServer";
            this.lblConServer.Size = new System.Drawing.Size(77, 12);
            this.lblConServer.TabIndex = 0;
            this.lblConServer.Text = "Data Server:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(368, 56);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(66, 21);
            this.txtPort.TabIndex = 3;
            // 
            // txtContractServer
            // 
            this.txtContractServer.Location = new System.Drawing.Point(174, 56);
            this.txtContractServer.Name = "txtContractServer";
            this.txtContractServer.Size = new System.Drawing.Size(147, 21);
            this.txtContractServer.TabIndex = 2;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 428);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SettingForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SCUT Server Setting";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageEntityModel.ResumeLayout(false);
            this.entityGroupBox.ResumeLayout(false);
            this.entityGroupBox.PerformLayout();
            this.tabPageContract.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox entityGroupBox;
        private System.Windows.Forms.RadioButton radioEntityAsmPath;
        private System.Windows.Forms.RadioButton radioEntityPath;
        private System.Windows.Forms.TextBox txtEntityAsmPath;
        private System.Windows.Forms.TextBox txtContractServer;
        private System.Windows.Forms.Button btnEntityImport;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnEntityOpen2;
        private System.Windows.Forms.Button btnEntityOpen1;
        private System.Windows.Forms.TextBox txtEntityPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtContractPwd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtContractUid;
        private System.Windows.Forms.Label lblConServer;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageEntityModel;
        private System.Windows.Forms.TabPage tabPageContract;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtContractDatabase;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCaseOutPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCaseName;
        private System.Windows.Forms.Button btnCaseOutpath;
        private System.Windows.Forms.ComboBox ddDbType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPort;
    }
}