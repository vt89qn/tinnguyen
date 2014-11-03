namespace FB.App_Present
{
    partial class FaceBookManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FaceBookManager));
            this.groupMain = new System.Windows.Forms.GroupBox();
            this.txtAutoMoveToNextPack = new System.Windows.Forms.CheckBox();
            this.btnKetBan = new System.Windows.Forms.Button();
            this.btnUpdateInfo = new System.Windows.Forms.Button();
            this.btnConfirmEmail = new System.Windows.Forms.Button();
            this.btnRegFBAccount = new System.Windows.Forms.Button();
            this.btnPostStatus = new System.Windows.Forms.Button();
            this.groupData = new System.Windows.Forms.GroupBox();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.menuGridData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCopyURL = new System.Windows.Forms.ToolStripMenuItem();
            this.menuXoaTK = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoginAgain = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblPack = new System.Windows.Forms.Label();
            this.txtPackNo = new System.Windows.Forms.ComboBox();
            this.feedAccessTokenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadPhotoAndCoiverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupMain.SuspendLayout();
            this.groupData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.menuGridData.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupMain
            // 
            this.groupMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupMain.Controls.Add(this.txtAutoMoveToNextPack);
            this.groupMain.Controls.Add(this.btnKetBan);
            this.groupMain.Controls.Add(this.btnUpdateInfo);
            this.groupMain.Controls.Add(this.btnConfirmEmail);
            this.groupMain.Controls.Add(this.btnRegFBAccount);
            this.groupMain.Controls.Add(this.btnPostStatus);
            this.groupMain.Location = new System.Drawing.Point(12, 12);
            this.groupMain.Name = "groupMain";
            this.groupMain.Size = new System.Drawing.Size(819, 121);
            this.groupMain.TabIndex = 2;
            this.groupMain.TabStop = false;
            this.groupMain.Text = "Control Panel";
            // 
            // txtAutoMoveToNextPack
            // 
            this.txtAutoMoveToNextPack.AutoSize = true;
            this.txtAutoMoveToNextPack.Location = new System.Drawing.Point(9, 75);
            this.txtAutoMoveToNextPack.Name = "txtAutoMoveToNextPack";
            this.txtAutoMoveToNextPack.Size = new System.Drawing.Size(147, 17);
            this.txtAutoMoveToNextPack.TabIndex = 1;
            this.txtAutoMoveToNextPack.Text = "Auto Move To Next Pack";
            this.txtAutoMoveToNextPack.UseVisualStyleBackColor = true;
            // 
            // btnKetBan
            // 
            this.btnKetBan.Location = new System.Drawing.Point(118, 19);
            this.btnKetBan.Name = "btnKetBan";
            this.btnKetBan.Size = new System.Drawing.Size(50, 50);
            this.btnKetBan.TabIndex = 0;
            this.btnKetBan.Text = "Make Friend";
            this.btnKetBan.UseVisualStyleBackColor = true;
            this.btnKetBan.Click += new System.EventHandler(this.btnKetBan_Click);
            // 
            // btnUpdateInfo
            // 
            this.btnUpdateInfo.Location = new System.Drawing.Point(333, 85);
            this.btnUpdateInfo.Name = "btnUpdateInfo";
            this.btnUpdateInfo.Size = new System.Drawing.Size(156, 30);
            this.btnUpdateInfo.TabIndex = 0;
            this.btnUpdateInfo.Text = "Update Info";
            this.btnUpdateInfo.UseVisualStyleBackColor = true;
            this.btnUpdateInfo.Click += new System.EventHandler(this.btnUpdateInfo_Click);
            // 
            // btnConfirmEmail
            // 
            this.btnConfirmEmail.Location = new System.Drawing.Point(495, 85);
            this.btnConfirmEmail.Name = "btnConfirmEmail";
            this.btnConfirmEmail.Size = new System.Drawing.Size(156, 30);
            this.btnConfirmEmail.TabIndex = 0;
            this.btnConfirmEmail.Text = "Confirm Email";
            this.btnConfirmEmail.UseVisualStyleBackColor = true;
            this.btnConfirmEmail.Click += new System.EventHandler(this.btnConfirmEmail_Click);
            // 
            // btnRegFBAccount
            // 
            this.btnRegFBAccount.Location = new System.Drawing.Point(6, 19);
            this.btnRegFBAccount.Name = "btnRegFBAccount";
            this.btnRegFBAccount.Size = new System.Drawing.Size(50, 50);
            this.btnRegFBAccount.TabIndex = 0;
            this.btnRegFBAccount.Text = "Start Reg Auto";
            this.btnRegFBAccount.UseVisualStyleBackColor = true;
            this.btnRegFBAccount.Click += new System.EventHandler(this.btnRegFBAccount_Click);
            // 
            // btnPostStatus
            // 
            this.btnPostStatus.Location = new System.Drawing.Point(62, 19);
            this.btnPostStatus.Name = "btnPostStatus";
            this.btnPostStatus.Size = new System.Drawing.Size(50, 50);
            this.btnPostStatus.TabIndex = 0;
            this.btnPostStatus.Text = "Check (F1)";
            this.btnPostStatus.UseVisualStyleBackColor = true;
            this.btnPostStatus.Click += new System.EventHandler(this.btnPostStatus_Click);
            // 
            // groupData
            // 
            this.groupData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupData.Controls.Add(this.gridData);
            this.groupData.Controls.Add(this.lblPack);
            this.groupData.Controls.Add(this.txtPackNo);
            this.groupData.Location = new System.Drawing.Point(12, 139);
            this.groupData.Name = "groupData";
            this.groupData.Size = new System.Drawing.Size(819, 345);
            this.groupData.TabIndex = 3;
            this.groupData.TabStop = false;
            this.groupData.Text = "Accounts (F5 How/Hide)";
            // 
            // gridData
            // 
            this.gridData.AllowUserToAddRows = false;
            this.gridData.AllowUserToDeleteRows = false;
            this.gridData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridData.ContextMenuStrip = this.menuGridData;
            this.gridData.Location = new System.Drawing.Point(6, 56);
            this.gridData.Name = "gridData";
            this.gridData.ReadOnly = true;
            this.gridData.Size = new System.Drawing.Size(807, 283);
            this.gridData.TabIndex = 5;
            this.gridData.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridData_CellMouseDown);
            this.gridData.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridData_DataError);
            this.gridData.KeyUp += new System.Windows.Forms.KeyEventHandler(this.gridData_KeyUp);
            // 
            // menuGridData
            // 
            this.menuGridData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopyURL,
            this.menuXoaTK,
            this.menuLoginAgain,
            this.copyPassToolStripMenuItem,
            this.createPageToolStripMenuItem,
            this.feedAccessTokenToolStripMenuItem,
            this.uploadPhotoAndCoiverToolStripMenuItem});
            this.menuGridData.Name = "menuGridData";
            this.menuGridData.Size = new System.Drawing.Size(208, 180);
            this.menuGridData.Opening += new System.ComponentModel.CancelEventHandler(this.menuGridData_Opening);
            // 
            // menuCopyURL
            // 
            this.menuCopyURL.Name = "menuCopyURL";
            this.menuCopyURL.Size = new System.Drawing.Size(207, 22);
            this.menuCopyURL.Text = "Copy URL";
            this.menuCopyURL.Click += new System.EventHandler(this.menuCopyURL_Click);
            // 
            // menuXoaTK
            // 
            this.menuXoaTK.Name = "menuXoaTK";
            this.menuXoaTK.Size = new System.Drawing.Size(207, 22);
            this.menuXoaTK.Text = "Delete this account";
            this.menuXoaTK.Click += new System.EventHandler(this.menuXoaTK_Click);
            // 
            // menuLoginAgain
            // 
            this.menuLoginAgain.Name = "menuLoginAgain";
            this.menuLoginAgain.Size = new System.Drawing.Size(207, 22);
            this.menuLoginAgain.Text = "Try login again";
            this.menuLoginAgain.Click += new System.EventHandler(this.menuLoginAgain_Click);
            // 
            // copyPassToolStripMenuItem
            // 
            this.copyPassToolStripMenuItem.Name = "copyPassToolStripMenuItem";
            this.copyPassToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.copyPassToolStripMenuItem.Text = "Copy Pass";
            this.copyPassToolStripMenuItem.Click += new System.EventHandler(this.copyPassToolStripMenuItem_Click);
            // 
            // createPageToolStripMenuItem
            // 
            this.createPageToolStripMenuItem.Name = "createPageToolStripMenuItem";
            this.createPageToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.createPageToolStripMenuItem.Text = "Create Page";
            this.createPageToolStripMenuItem.Click += new System.EventHandler(this.createPageToolStripMenuItem_Click);
            // 
            // lblPack
            // 
            this.lblPack.AutoSize = true;
            this.lblPack.Location = new System.Drawing.Point(6, 32);
            this.lblPack.Name = "lblPack";
            this.lblPack.Size = new System.Drawing.Size(92, 13);
            this.lblPack.TabIndex = 0;
            this.lblPack.Text = "P#(F6 Next Pack)";
            // 
            // txtPackNo
            // 
            this.txtPackNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtPackNo.FormattingEnabled = true;
            this.txtPackNo.Location = new System.Drawing.Point(104, 29);
            this.txtPackNo.Name = "txtPackNo";
            this.txtPackNo.Size = new System.Drawing.Size(52, 21);
            this.txtPackNo.TabIndex = 1;
            this.txtPackNo.SelectedValueChanged += new System.EventHandler(this.txtPackNo_SelectedValueChanged);
            // 
            // feedAccessTokenToolStripMenuItem
            // 
            this.feedAccessTokenToolStripMenuItem.Name = "feedAccessTokenToolStripMenuItem";
            this.feedAccessTokenToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.feedAccessTokenToolStripMenuItem.Text = "Feed AccessToken";
            this.feedAccessTokenToolStripMenuItem.Click += new System.EventHandler(this.feedAccessTokenToolStripMenuItem_Click);
            // 
            // uploadPhotoAndCoiverToolStripMenuItem
            // 
            this.uploadPhotoAndCoiverToolStripMenuItem.Name = "uploadPhotoAndCoiverToolStripMenuItem";
            this.uploadPhotoAndCoiverToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.uploadPhotoAndCoiverToolStripMenuItem.Text = "Upload Photo and Coiver";
            this.uploadPhotoAndCoiverToolStripMenuItem.Click += new System.EventHandler(this.uploadPhotoAndCoiverToolStripMenuItem_Click);
            // 
            // FaceBookManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 496);
            this.Controls.Add(this.groupMain);
            this.Controls.Add(this.groupData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FaceBookManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FaceBookManager";
            this.Load += new System.EventHandler(this.FaceBookManager_Load);
            this.groupMain.ResumeLayout(false);
            this.groupMain.PerformLayout();
            this.groupData.ResumeLayout(false);
            this.groupData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.menuGridData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupMain;
        private System.Windows.Forms.Button btnPostStatus;
        private System.Windows.Forms.GroupBox groupData;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.Label lblPack;
        private System.Windows.Forms.ComboBox txtPackNo;
        private System.Windows.Forms.ContextMenuStrip menuGridData;
        private System.Windows.Forms.ToolStripMenuItem menuCopyURL;
        private System.Windows.Forms.ToolStripMenuItem menuXoaTK;
        private System.Windows.Forms.ToolStripMenuItem menuLoginAgain;
        private System.Windows.Forms.Button btnRegFBAccount;
        private System.Windows.Forms.Button btnKetBan;
        private System.Windows.Forms.Button btnConfirmEmail;
        private System.Windows.Forms.Button btnUpdateInfo;
        private System.Windows.Forms.CheckBox txtAutoMoveToNextPack;
        private System.Windows.Forms.ToolStripMenuItem copyPassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem feedAccessTokenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadPhotoAndCoiverToolStripMenuItem;
    }
}