namespace Scut.SMS
{
    partial class RedisForm
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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.settingPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.bttomPanel = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.mainPanel.SuspendLayout();
            this.bttomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Controls.Add(this.settingPropertyGrid);
            this.mainPanel.Location = new System.Drawing.Point(3, 2);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(391, 214);
            this.mainPanel.TabIndex = 2;
            // 
            // settingPropertyGrid
            // 
            this.settingPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.settingPropertyGrid.Name = "settingPropertyGrid";
            this.settingPropertyGrid.Size = new System.Drawing.Size(391, 214);
            this.settingPropertyGrid.TabIndex = 0;
            this.settingPropertyGrid.ToolbarVisible = false;
            // 
            // bttomPanel
            // 
            this.bttomPanel.Controls.Add(this.btnCancel);
            this.bttomPanel.Controls.Add(this.btnOk);
            this.bttomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bttomPanel.Location = new System.Drawing.Point(0, 219);
            this.bttomPanel.Name = "bttomPanel";
            this.bttomPanel.Size = new System.Drawing.Size(398, 42);
            this.bttomPanel.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(311, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(216, 11);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // eventLog1
            // 
            this.eventLog1.SynchronizingObject = this;
            // 
            // RedisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 261);
            this.Controls.Add(this.bttomPanel);
            this.Controls.Add(this.mainPanel);
            this.MaximizeBox = false;
            this.Name = "RedisForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Redis Setting";
            this.mainPanel.ResumeLayout(false);
            this.bttomPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel bttomPanel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PropertyGrid settingPropertyGrid;
        private System.Diagnostics.EventLog eventLog1;
    }
}