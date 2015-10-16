namespace Scut.SMS
{
    partial class RedisKeyForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblKeyCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnReplaceAll = new System.Windows.Forms.Button();
            this.btnMoveToDb = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.middlePanel = new System.Windows.Forms.Panel();
            this.waitPanel = new System.Windows.Forms.Panel();
            this.lblWait = new System.Windows.Forms.Label();
            this.keyDataGridView = new System.Windows.Forms.DataGridView();
            this.KeyId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Identity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.topPanel = new System.Windows.Forms.Panel();
            this.ckFindOrReplace = new System.Windows.Forms.CheckBox();
            this.replacePanel = new System.Windows.Forms.Panel();
            this.ckMatchCase = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.middlePanel.SuspendLayout();
            this.waitPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.keyDataGridView)).BeginInit();
            this.topPanel.SuspendLayout();
            this.replacePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblKeyCount);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnReplace);
            this.panel1.Controls.Add(this.btnReplaceAll);
            this.panel1.Controls.Add(this.btnMoveToDb);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 407);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(666, 40);
            this.panel1.TabIndex = 0;
            // 
            // lblKeyCount
            // 
            this.lblKeyCount.AutoSize = true;
            this.lblKeyCount.Location = new System.Drawing.Point(55, 15);
            this.lblKeyCount.Name = "lblKeyCount";
            this.lblKeyCount.Size = new System.Drawing.Size(11, 12);
            this.lblKeyCount.TabIndex = 2;
            this.lblKeyCount.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Count:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(579, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReplace.Location = new System.Drawing.Point(122, 10);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(75, 23);
            this.btnReplace.TabIndex = 0;
            this.btnReplace.Text = "Replace";
            this.btnReplace.UseVisualStyleBackColor = true;
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReplaceAll.Location = new System.Drawing.Point(213, 10);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(75, 23);
            this.btnReplaceAll.TabIndex = 0;
            this.btnReplaceAll.Text = "ReplaceAll";
            this.btnReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceAll.Click += new System.EventHandler(this.btnReplaceAll_Click);
            // 
            // btnMoveToDb
            // 
            this.btnMoveToDb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveToDb.Enabled = false;
            this.btnMoveToDb.Location = new System.Drawing.Point(308, 10);
            this.btnMoveToDb.Name = "btnMoveToDb";
            this.btnMoveToDb.Size = new System.Drawing.Size(75, 23);
            this.btnMoveToDb.TabIndex = 0;
            this.btnMoveToDb.Text = "Move to db";
            this.btnMoveToDb.UseVisualStyleBackColor = true;
            this.btnMoveToDb.Visible = false;
            this.btnMoveToDb.Click += new System.EventHandler(this.btnMoveToDb_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(398, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Find";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(489, 10);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // middlePanel
            // 
            this.middlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.middlePanel.Controls.Add(this.waitPanel);
            this.middlePanel.Controls.Add(this.keyDataGridView);
            this.middlePanel.Location = new System.Drawing.Point(0, 87);
            this.middlePanel.Name = "middlePanel";
            this.middlePanel.Size = new System.Drawing.Size(666, 318);
            this.middlePanel.TabIndex = 1;
            // 
            // waitPanel
            // 
            this.waitPanel.Controls.Add(this.lblWait);
            this.waitPanel.Location = new System.Drawing.Point(144, 35);
            this.waitPanel.Name = "waitPanel";
            this.waitPanel.Size = new System.Drawing.Size(308, 32);
            this.waitPanel.TabIndex = 1;
            // 
            // lblWait
            // 
            this.lblWait.AutoSize = true;
            this.lblWait.Location = new System.Drawing.Point(13, 10);
            this.lblWait.Name = "lblWait";
            this.lblWait.Size = new System.Drawing.Size(65, 12);
            this.lblWait.TabIndex = 0;
            this.lblWait.Text = "Loading...";
            // 
            // keyDataGridView
            // 
            this.keyDataGridView.AllowUserToAddRows = false;
            this.keyDataGridView.AllowUserToDeleteRows = false;
            this.keyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.keyDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.KeyId,
            this.Identity,
            this.Key});
            this.keyDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keyDataGridView.Location = new System.Drawing.Point(0, 0);
            this.keyDataGridView.MultiSelect = false;
            this.keyDataGridView.Name = "keyDataGridView";
            this.keyDataGridView.ReadOnly = true;
            this.keyDataGridView.RowTemplate.Height = 23;
            this.keyDataGridView.Size = new System.Drawing.Size(666, 318);
            this.keyDataGridView.TabIndex = 0;
            this.keyDataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.keyDataGridView_CellContentDoubleClick);
            // 
            // KeyId
            // 
            this.KeyId.DataPropertyName = "Id";
            this.KeyId.HeaderText = "";
            this.KeyId.Name = "KeyId";
            this.KeyId.ReadOnly = true;
            this.KeyId.Width = 35;
            // 
            // Identity
            // 
            this.Identity.DataPropertyName = "Identity";
            this.Identity.HeaderText = "Identity";
            this.Identity.Name = "Identity";
            this.Identity.ReadOnly = true;
            this.Identity.Width = 60;
            // 
            // Key
            // 
            this.Key.DataPropertyName = "Key";
            this.Key.HeaderText = "Key Name";
            this.Key.Name = "Key";
            this.Key.ReadOnly = true;
            this.Key.Width = 500;
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(94, 10);
            this.txtKey.Multiline = true;
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(397, 21);
            this.txtKey.TabIndex = 3;
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.ckFindOrReplace);
            this.topPanel.Controls.Add(this.replacePanel);
            this.topPanel.Controls.Add(this.label2);
            this.topPanel.Controls.Add(this.txtKey);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(666, 87);
            this.topPanel.TabIndex = 2;
            // 
            // ckFindOrReplace
            // 
            this.ckFindOrReplace.AutoSize = true;
            this.ckFindOrReplace.Location = new System.Drawing.Point(514, 12);
            this.ckFindOrReplace.Name = "ckFindOrReplace";
            this.ckFindOrReplace.Size = new System.Drawing.Size(84, 16);
            this.ckFindOrReplace.TabIndex = 6;
            this.ckFindOrReplace.Text = "Replace In";
            this.ckFindOrReplace.UseVisualStyleBackColor = true;
            this.ckFindOrReplace.CheckedChanged += new System.EventHandler(this.ckFindOrReplace_CheckedChanged);
            // 
            // replacePanel
            // 
            this.replacePanel.Controls.Add(this.ckMatchCase);
            this.replacePanel.Controls.Add(this.label3);
            this.replacePanel.Controls.Add(this.txtReplace);
            this.replacePanel.Location = new System.Drawing.Point(0, 39);
            this.replacePanel.Name = "replacePanel";
            this.replacePanel.Size = new System.Drawing.Size(663, 40);
            this.replacePanel.TabIndex = 5;
            // 
            // ckMatchCase
            // 
            this.ckMatchCase.AutoSize = true;
            this.ckMatchCase.Checked = true;
            this.ckMatchCase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckMatchCase.Location = new System.Drawing.Point(514, 11);
            this.ckMatchCase.Name = "ckMatchCase";
            this.ckMatchCase.Size = new System.Drawing.Size(84, 16);
            this.ckMatchCase.TabIndex = 7;
            this.ckMatchCase.Text = "Match Case";
            this.ckMatchCase.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Replace Key:";
            // 
            // txtReplace
            // 
            this.txtReplace.Location = new System.Drawing.Point(94, 7);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(398, 21);
            this.txtReplace.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Find Key:";
            // 
            // RedisKeyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 447);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.middlePanel);
            this.Controls.Add(this.panel1);
            this.Name = "RedisKeyForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Redis Key";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.middlePanel.ResumeLayout(false);
            this.waitPanel.ResumeLayout(false);
            this.waitPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.keyDataGridView)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.replacePanel.ResumeLayout(false);
            this.replacePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel middlePanel;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label lblKeyCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.DataGridView keyDataGridView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button btnReplaceAll;
        private System.Windows.Forms.CheckBox ckFindOrReplace;
        private System.Windows.Forms.Panel replacePanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.CheckBox ckMatchCase;
        private System.Windows.Forms.Panel waitPanel;
        private System.Windows.Forms.Label lblWait;
        private System.Windows.Forms.Button btnMoveToDb;
        private System.Windows.Forms.DataGridViewTextBoxColumn KeyId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Identity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Key;
    }
}