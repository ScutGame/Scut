namespace Scut.SMS
{
    partial class SqlQueueForm
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
            this.centerPanel = new System.Windows.Forms.Panel();
            this.msgPanel = new System.Windows.Forms.Panel();
            this.msgListBox = new System.Windows.Forms.ListBox();
            this.barPanel = new System.Windows.Forms.Panel();
            this.lblBar = new System.Windows.Forms.Label();
            this.queueProgressBar = new System.Windows.Forms.ProgressBar();
            this.titleGroupBox = new System.Windows.Forms.GroupBox();
            this.lblErrorCount = new System.Windows.Forms.Label();
            this.lblCompleted = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.removeCheckBox = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.executeQueueWorker = new System.ComponentModel.BackgroundWorker();
            this.centerPanel.SuspendLayout();
            this.msgPanel.SuspendLayout();
            this.barPanel.SuspendLayout();
            this.titleGroupBox.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // centerPanel
            // 
            this.centerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.centerPanel.Controls.Add(this.msgPanel);
            this.centerPanel.Controls.Add(this.barPanel);
            this.centerPanel.Controls.Add(this.titleGroupBox);
            this.centerPanel.Location = new System.Drawing.Point(0, 1);
            this.centerPanel.Name = "centerPanel";
            this.centerPanel.Size = new System.Drawing.Size(473, 340);
            this.centerPanel.TabIndex = 0;
            // 
            // msgPanel
            // 
            this.msgPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.msgPanel.Controls.Add(this.msgListBox);
            this.msgPanel.Location = new System.Drawing.Point(0, 131);
            this.msgPanel.Name = "msgPanel";
            this.msgPanel.Size = new System.Drawing.Size(473, 209);
            this.msgPanel.TabIndex = 4;
            // 
            // msgListBox
            // 
            this.msgListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msgListBox.FormattingEnabled = true;
            this.msgListBox.ItemHeight = 12;
            this.msgListBox.Location = new System.Drawing.Point(0, 0);
            this.msgListBox.Name = "msgListBox";
            this.msgListBox.Size = new System.Drawing.Size(473, 209);
            this.msgListBox.TabIndex = 3;
            // 
            // barPanel
            // 
            this.barPanel.Controls.Add(this.lblBar);
            this.barPanel.Controls.Add(this.queueProgressBar);
            this.barPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.barPanel.Location = new System.Drawing.Point(0, 74);
            this.barPanel.Name = "barPanel";
            this.barPanel.Size = new System.Drawing.Size(473, 51);
            this.barPanel.TabIndex = 2;
            // 
            // lblBar
            // 
            this.lblBar.AutoSize = true;
            this.lblBar.Location = new System.Drawing.Point(439, 32);
            this.lblBar.Name = "lblBar";
            this.lblBar.Size = new System.Drawing.Size(17, 12);
            this.lblBar.TabIndex = 2;
            this.lblBar.Text = "0%";
            // 
            // queueProgressBar
            // 
            this.queueProgressBar.Location = new System.Drawing.Point(3, 5);
            this.queueProgressBar.Name = "queueProgressBar";
            this.queueProgressBar.Size = new System.Drawing.Size(467, 23);
            this.queueProgressBar.TabIndex = 1;
            // 
            // titleGroupBox
            // 
            this.titleGroupBox.Controls.Add(this.lblErrorCount);
            this.titleGroupBox.Controls.Add(this.lblCompleted);
            this.titleGroupBox.Controls.Add(this.lblCount);
            this.titleGroupBox.Controls.Add(this.label3);
            this.titleGroupBox.Controls.Add(this.label2);
            this.titleGroupBox.Controls.Add(this.label1);
            this.titleGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleGroupBox.Location = new System.Drawing.Point(0, 0);
            this.titleGroupBox.Name = "titleGroupBox";
            this.titleGroupBox.Size = new System.Drawing.Size(473, 74);
            this.titleGroupBox.TabIndex = 0;
            this.titleGroupBox.TabStop = false;
            this.titleGroupBox.Text = "Error Queue:";
            // 
            // lblErrorCount
            // 
            this.lblErrorCount.AutoSize = true;
            this.lblErrorCount.Location = new System.Drawing.Point(303, 48);
            this.lblErrorCount.Name = "lblErrorCount";
            this.lblErrorCount.Size = new System.Drawing.Size(11, 12);
            this.lblErrorCount.TabIndex = 1;
            this.lblErrorCount.Text = "0";
            // 
            // lblCompleted
            // 
            this.lblCompleted.AutoSize = true;
            this.lblCompleted.Location = new System.Drawing.Point(88, 48);
            this.lblCompleted.Name = "lblCompleted";
            this.lblCompleted.Size = new System.Drawing.Size(11, 12);
            this.lblCompleted.TabIndex = 1;
            this.lblCompleted.Text = "0";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(88, 23);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(11, 12);
            this.lblCount.TabIndex = 1;
            this.lblCount.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(227, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "Error:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Completed:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Count:";
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.removeCheckBox);
            this.bottomPanel.Controls.Add(this.btnRefresh);
            this.bottomPanel.Controls.Add(this.btnExecute);
            this.bottomPanel.Controls.Add(this.btnCancel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 343);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(473, 45);
            this.bottomPanel.TabIndex = 0;
            // 
            // removeCheckBox
            // 
            this.removeCheckBox.AutoSize = true;
            this.removeCheckBox.Location = new System.Drawing.Point(14, 17);
            this.removeCheckBox.Name = "removeCheckBox";
            this.removeCheckBox.Size = new System.Drawing.Size(120, 16);
            this.removeCheckBox.TabIndex = 1;
            this.removeCheckBox.Text = "Error Is Removed";
            this.removeCheckBox.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(178, 14);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExecute
            // 
            this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecute.Location = new System.Drawing.Point(285, 14);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(75, 23);
            this.btnExecute.TabIndex = 0;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(386, 14);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // executeQueueWorker
            // 
            this.executeQueueWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.executeQueueWorker_DoWork);
            this.executeQueueWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.executeQueueWorker_RunWorkerCompleted);
            // 
            // SqlQueueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 388);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.centerPanel);
            this.Name = "SqlQueueForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database Update Error Queue";
            this.centerPanel.ResumeLayout(false);
            this.msgPanel.ResumeLayout(false);
            this.barPanel.ResumeLayout(false);
            this.barPanel.PerformLayout();
            this.titleGroupBox.ResumeLayout(false);
            this.titleGroupBox.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel centerPanel;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.GroupBox titleGroupBox;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRefresh;
        private System.ComponentModel.BackgroundWorker executeQueueWorker;
        private System.Windows.Forms.Panel barPanel;
        private System.Windows.Forms.ProgressBar queueProgressBar;
        private System.Windows.Forms.Label lblBar;
        private System.Windows.Forms.Label lblCompleted;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblErrorCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel msgPanel;
        private System.Windows.Forms.ListBox msgListBox;
        private System.Windows.Forms.CheckBox removeCheckBox;
    }
}