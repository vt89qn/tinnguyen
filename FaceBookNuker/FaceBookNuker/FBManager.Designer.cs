namespace FaceBookNuker
{
    partial class FBManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FBManager));
            this.gridData = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPostStatus = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnFriendAccept = new System.Windows.Forms.Button();
            this.btnFriendRequest = new System.Windows.Forms.Button();
            this.btnCheckProblemAccount = new System.Windows.Forms.Button();
            this.txtSession = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnNewSession = new System.Windows.Forms.Button();
            this.txtDateStart = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnImportFacebook = new System.Windows.Forms.Button();
            this.btnRegNew = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnFeedMail = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridData
            // 
            this.gridData.AllowUserToAddRows = false;
            this.gridData.AllowUserToDeleteRows = false;
            this.gridData.AllowUserToResizeRows = false;
            this.gridData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridData.Location = new System.Drawing.Point(3, 16);
            this.gridData.Name = "gridData";
            this.gridData.RowHeadersVisible = false;
            this.gridData.Size = new System.Drawing.Size(854, 337);
            this.gridData.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gridData);
            this.groupBox1.Location = new System.Drawing.Point(12, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(860, 356);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "List Account";
            // 
            // btnPostStatus
            // 
            this.btnPostStatus.Location = new System.Drawing.Point(12, 18);
            this.btnPostStatus.Name = "btnPostStatus";
            this.btnPostStatus.Size = new System.Drawing.Size(60, 44);
            this.btnPostStatus.TabIndex = 2;
            this.btnPostStatus.Text = "Post Status";
            this.btnPostStatus.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnFriendAccept);
            this.groupBox2.Controls.Add(this.btnFriendRequest);
            this.groupBox2.Controls.Add(this.btnPostStatus);
            this.groupBox2.Location = new System.Drawing.Point(333, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(213, 75);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Controller";
            // 
            // btnFriendAccept
            // 
            this.btnFriendAccept.Location = new System.Drawing.Point(142, 18);
            this.btnFriendAccept.Name = "btnFriendAccept";
            this.btnFriendAccept.Size = new System.Drawing.Size(60, 44);
            this.btnFriendAccept.TabIndex = 2;
            this.btnFriendAccept.Text = "Friend Accept";
            this.btnFriendAccept.UseVisualStyleBackColor = true;
            // 
            // btnFriendRequest
            // 
            this.btnFriendRequest.Location = new System.Drawing.Point(78, 18);
            this.btnFriendRequest.Name = "btnFriendRequest";
            this.btnFriendRequest.Size = new System.Drawing.Size(60, 44);
            this.btnFriendRequest.TabIndex = 2;
            this.btnFriendRequest.Text = "Friend Request";
            this.btnFriendRequest.UseVisualStyleBackColor = true;
            // 
            // btnCheckProblemAccount
            // 
            this.btnCheckProblemAccount.Location = new System.Drawing.Point(6, 18);
            this.btnCheckProblemAccount.Name = "btnCheckProblemAccount";
            this.btnCheckProblemAccount.Size = new System.Drawing.Size(92, 44);
            this.btnCheckProblemAccount.TabIndex = 2;
            this.btnCheckProblemAccount.Text = "Check Problems Account";
            this.btnCheckProblemAccount.UseVisualStyleBackColor = true;
            // 
            // txtSession
            // 
            this.txtSession.Enabled = false;
            this.txtSession.Location = new System.Drawing.Point(116, 19);
            this.txtSession.Name = "txtSession";
            this.txtSession.Size = new System.Drawing.Size(100, 20);
            this.txtSession.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Current Session";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnNewSession);
            this.groupBox3.Controls.Add(this.txtDateStart);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtSession);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(315, 75);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Working Session";
            // 
            // btnNewSession
            // 
            this.btnNewSession.Location = new System.Drawing.Point(222, 18);
            this.btnNewSession.Name = "btnNewSession";
            this.btnNewSession.Size = new System.Drawing.Size(75, 44);
            this.btnNewSession.TabIndex = 5;
            this.btnNewSession.Text = "New Session";
            this.btnNewSession.UseVisualStyleBackColor = true;
            // 
            // txtDateStart
            // 
            this.txtDateStart.Enabled = false;
            this.txtDateStart.Location = new System.Drawing.Point(116, 42);
            this.txtDateStart.Name = "txtDateStart";
            this.txtDateStart.Size = new System.Drawing.Size(100, 20);
            this.txtDateStart.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Date Start Session";
            // 
            // btnImportFacebook
            // 
            this.btnImportFacebook.Location = new System.Drawing.Point(104, 18);
            this.btnImportFacebook.Name = "btnImportFacebook";
            this.btnImportFacebook.Size = new System.Drawing.Size(64, 44);
            this.btnImportFacebook.TabIndex = 2;
            this.btnImportFacebook.Text = "Import Facebook";
            this.btnImportFacebook.UseVisualStyleBackColor = true;
            // 
            // btnRegNew
            // 
            this.btnRegNew.Location = new System.Drawing.Point(174, 18);
            this.btnRegNew.Name = "btnRegNew";
            this.btnRegNew.Size = new System.Drawing.Size(78, 44);
            this.btnRegNew.TabIndex = 2;
            this.btnRegNew.Text = "Reg FaceBook";
            this.btnRegNew.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.btnImportFacebook);
            this.groupBox4.Controls.Add(this.btnCheckProblemAccount);
            this.groupBox4.Controls.Add(this.btnFeedMail);
            this.groupBox4.Controls.Add(this.btnRegNew);
            this.groupBox4.Location = new System.Drawing.Point(552, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(320, 75);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Account";
            // 
            // btnFeedMail
            // 
            this.btnFeedMail.Location = new System.Drawing.Point(258, 18);
            this.btnFeedMail.Name = "btnFeedMail";
            this.btnFeedMail.Size = new System.Drawing.Size(56, 44);
            this.btnFeedMail.TabIndex = 2;
            this.btnFeedMail.Text = "Feed Mail";
            this.btnFeedMail.UseVisualStyleBackColor = true;
            // 
            // FBManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FBManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FB Nuker";
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnPostStatus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnFriendRequest;
        private System.Windows.Forms.Button btnFriendAccept;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSession;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnNewSession;
        private System.Windows.Forms.TextBox txtDateStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCheckProblemAccount;
        private System.Windows.Forms.Button btnImportFacebook;
        private System.Windows.Forms.Button btnRegNew;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnFeedMail;

    }
}