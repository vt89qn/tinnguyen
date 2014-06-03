namespace InPhieuChi
{
    partial class InPhieuChi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InPhieuChi));
            this.btnBrowserFile = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowserFolder = new System.Windows.Forms.Button();
            this.txtFolderOut = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.txtNoOfSheet = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRowFrom = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRowTo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.btnReadFile = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBrowserFile
            // 
            this.btnBrowserFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowserFile.Location = new System.Drawing.Point(519, 11);
            this.btnBrowserFile.Name = "btnBrowserFile";
            this.btnBrowserFile.Size = new System.Drawing.Size(75, 22);
            this.btnBrowserFile.TabIndex = 0;
            this.btnBrowserFile.Text = "...";
            this.btnBrowserFile.UseVisualStyleBackColor = true;
            this.btnBrowserFile.Click += new System.EventHandler(this.btnBrowserFile_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileName.Enabled = false;
            this.txtFileName.Location = new System.Drawing.Point(53, 12);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(460, 20);
            this.txtFileName.TabIndex = 1;
            this.txtFileName.Text = "C:\\Users\\Tin\\SkyDrive\\Project\\InPhieuChi\\InPhieuChi\\bin\\Debug\\NKC NĂM 2013  (1).x" +
    "ls";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "File";
            // 
            // btnBrowserFolder
            // 
            this.btnBrowserFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowserFolder.Location = new System.Drawing.Point(519, 333);
            this.btnBrowserFolder.Name = "btnBrowserFolder";
            this.btnBrowserFolder.Size = new System.Drawing.Size(75, 22);
            this.btnBrowserFolder.TabIndex = 0;
            this.btnBrowserFolder.Text = "...";
            this.btnBrowserFolder.UseVisualStyleBackColor = true;
            this.btnBrowserFolder.Click += new System.EventHandler(this.btnBrowserFolder_Click);
            // 
            // txtFolderOut
            // 
            this.txtFolderOut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolderOut.Enabled = false;
            this.txtFolderOut.Location = new System.Drawing.Point(53, 334);
            this.txtFolderOut.Name = "txtFolderOut";
            this.txtFolderOut.Size = new System.Drawing.Size(460, 20);
            this.txtFolderOut.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 337);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Folder";
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.Location = new System.Drawing.Point(53, 360);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(143, 22);
            this.btnExport.TabIndex = 0;
            this.btnExport.Text = "Xuất File";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // txtNoOfSheet
            // 
            this.txtNoOfSheet.Location = new System.Drawing.Point(53, 44);
            this.txtNoOfSheet.Name = "txtNoOfSheet";
            this.txtNoOfSheet.Size = new System.Drawing.Size(179, 20);
            this.txtNoOfSheet.TabIndex = 1;
            this.txtNoOfSheet.Text = "2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Sheet";
            // 
            // txtRowFrom
            // 
            this.txtRowFrom.Location = new System.Drawing.Point(303, 44);
            this.txtRowFrom.Name = "txtRowFrom";
            this.txtRowFrom.Size = new System.Drawing.Size(34, 20);
            this.txtRowFrom.TabIndex = 1;
            this.txtRowFrom.Text = "9";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(248, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Từ Dòng";
            // 
            // txtRowTo
            // 
            this.txtRowTo.Location = new System.Drawing.Point(405, 44);
            this.txtRowTo.Name = "txtRowTo";
            this.txtRowTo.Size = new System.Drawing.Size(34, 20);
            this.txtRowTo.TabIndex = 1;
            this.txtRowTo.Text = "185";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(343, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Đến Dòng";
            // 
            // gridData
            // 
            this.gridData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridData.Location = new System.Drawing.Point(15, 70);
            this.gridData.Name = "gridData";
            this.gridData.Size = new System.Drawing.Size(579, 257);
            this.gridData.TabIndex = 3;
            // 
            // btnReadFile
            // 
            this.btnReadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReadFile.Location = new System.Drawing.Point(451, 42);
            this.btnReadFile.Name = "btnReadFile";
            this.btnReadFile.Size = new System.Drawing.Size(143, 22);
            this.btnReadFile.TabIndex = 0;
            this.btnReadFile.Text = "Đọc File";
            this.btnReadFile.UseVisualStyleBackColor = true;
            this.btnReadFile.Click += new System.EventHandler(this.btnReadFile_Click);
            // 
            // InPhieuChi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 392);
            this.Controls.Add(this.gridData);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRowTo);
            this.Controls.Add(this.txtRowFrom);
            this.Controls.Add(this.txtNoOfSheet);
            this.Controls.Add(this.txtFolderOut);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.btnReadFile);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnBrowserFolder);
            this.Controls.Add(this.btnBrowserFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InPhieuChi";
            this.Text = "Xuất Phiếu Chi";
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowserFile;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowserFolder;
        private System.Windows.Forms.TextBox txtFolderOut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TextBox txtNoOfSheet;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRowFrom;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRowTo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.Button btnReadFile;
    }
}

