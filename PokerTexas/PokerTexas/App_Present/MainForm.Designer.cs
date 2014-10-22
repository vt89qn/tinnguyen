namespace PokerTexas.App_Present
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupData = new System.Windows.Forms.GroupBox();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.menuGridData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCopyURL = new System.Windows.Forms.ToolStripMenuItem();
            this.menuXoaTK = new System.Windows.Forms.ToolStripMenuItem();
            this.lblPack = new System.Windows.Forms.Label();
            this.txtPackNo = new System.Windows.Forms.ComboBox();
            this.btnThemPack = new System.Windows.Forms.Button();
            this.btnKetBan = new System.Windows.Forms.Button();
            this.btnKiemTraTaiKhoan = new System.Windows.Forms.Button();
            this.btnThemTaiKhoan = new System.Windows.Forms.Button();
            this.groupMain = new System.Windows.Forms.GroupBox();
            this.btnNhanChipMayMan = new System.Windows.Forms.Button();
            this.btnTangCo4La = new System.Windows.Forms.Button();
            this.btnTangQuaBiMat = new System.Windows.Forms.Button();
            this.btnNhanThuongHangNgay = new System.Windows.Forms.Button();
            this.btnNhanThuongWeb = new System.Windows.Forms.Button();
            this.groupData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.menuGridData.SuspendLayout();
            this.groupMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupData
            // 
            this.groupData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupData.Controls.Add(this.gridData);
            this.groupData.Controls.Add(this.lblPack);
            this.groupData.Controls.Add(this.txtPackNo);
            this.groupData.Controls.Add(this.btnThemPack);
            this.groupData.Controls.Add(this.btnKetBan);
            this.groupData.Controls.Add(this.btnKiemTraTaiKhoan);
            this.groupData.Controls.Add(this.btnThemTaiKhoan);
            this.groupData.Location = new System.Drawing.Point(12, 75);
            this.groupData.Name = "groupData";
            this.groupData.Size = new System.Drawing.Size(763, 360);
            this.groupData.TabIndex = 1;
            this.groupData.TabStop = false;
            this.groupData.Text = "Tài Khoản (F5 How/Hide)";
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
            this.gridData.Size = new System.Drawing.Size(751, 298);
            this.gridData.TabIndex = 6;
            this.gridData.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridData_CellMouseDown);
            this.gridData.KeyUp += new System.Windows.Forms.KeyEventHandler(this.gridData_KeyUp);
            // 
            // menuGridData
            // 
            this.menuGridData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopyURL,
            this.menuXoaTK});
            this.menuGridData.Name = "menuGridData";
            this.menuGridData.Size = new System.Drawing.Size(127, 48);
            this.menuGridData.Opening += new System.ComponentModel.CancelEventHandler(this.menuGridData_Opening);
            // 
            // menuCopyURL
            // 
            this.menuCopyURL.Name = "menuCopyURL";
            this.menuCopyURL.Size = new System.Drawing.Size(126, 22);
            this.menuCopyURL.Text = "Copy URL";
            this.menuCopyURL.Click += new System.EventHandler(this.menuCopyURL_Click);
            // 
            // menuXoaTK
            // 
            this.menuXoaTK.Name = "menuXoaTK";
            this.menuXoaTK.Size = new System.Drawing.Size(126, 22);
            this.menuXoaTK.Text = "Xóa TK";
            this.menuXoaTK.Click += new System.EventHandler(this.menuXoaTK_Click);
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
            // btnThemPack
            // 
            this.btnThemPack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThemPack.Location = new System.Drawing.Point(162, 28);
            this.btnThemPack.Name = "btnThemPack";
            this.btnThemPack.Size = new System.Drawing.Size(30, 23);
            this.btnThemPack.TabIndex = 2;
            this.btnThemPack.Text = "+";
            this.btnThemPack.UseVisualStyleBackColor = true;
            this.btnThemPack.Click += new System.EventHandler(this.btnThemPack_Click);
            // 
            // btnKetBan
            // 
            this.btnKetBan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKetBan.Location = new System.Drawing.Point(657, 23);
            this.btnKetBan.Name = "btnKetBan";
            this.btnKetBan.Size = new System.Drawing.Size(100, 30);
            this.btnKetBan.TabIndex = 5;
            this.btnKetBan.Text = "Kết bạn(F9)";
            this.btnKetBan.UseVisualStyleBackColor = true;
            this.btnKetBan.Click += new System.EventHandler(this.btnKetBan_Click);
            // 
            // btnKiemTraTaiKhoan
            // 
            this.btnKiemTraTaiKhoan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKiemTraTaiKhoan.Location = new System.Drawing.Point(445, 23);
            this.btnKiemTraTaiKhoan.Name = "btnKiemTraTaiKhoan";
            this.btnKiemTraTaiKhoan.Size = new System.Drawing.Size(100, 30);
            this.btnKiemTraTaiKhoan.TabIndex = 3;
            this.btnKiemTraTaiKhoan.Text = "Check Money(F7)";
            this.btnKiemTraTaiKhoan.UseVisualStyleBackColor = true;
            this.btnKiemTraTaiKhoan.Click += new System.EventHandler(this.btnKiemTraTaiKhoan_Click);
            // 
            // btnThemTaiKhoan
            // 
            this.btnThemTaiKhoan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThemTaiKhoan.Location = new System.Drawing.Point(551, 23);
            this.btnThemTaiKhoan.Name = "btnThemTaiKhoan";
            this.btnThemTaiKhoan.Size = new System.Drawing.Size(100, 30);
            this.btnThemTaiKhoan.TabIndex = 4;
            this.btnThemTaiKhoan.Text = "Thêm TK(F8)";
            this.btnThemTaiKhoan.UseVisualStyleBackColor = true;
            this.btnThemTaiKhoan.Click += new System.EventHandler(this.btnThemTaiKhoan_Click);
            // 
            // groupMain
            // 
            this.groupMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupMain.Controls.Add(this.btnNhanThuongWeb);
            this.groupMain.Controls.Add(this.btnNhanChipMayMan);
            this.groupMain.Controls.Add(this.btnTangCo4La);
            this.groupMain.Controls.Add(this.btnTangQuaBiMat);
            this.groupMain.Controls.Add(this.btnNhanThuongHangNgay);
            this.groupMain.Location = new System.Drawing.Point(12, 12);
            this.groupMain.Name = "groupMain";
            this.groupMain.Size = new System.Drawing.Size(763, 57);
            this.groupMain.TabIndex = 0;
            this.groupMain.TabStop = false;
            this.groupMain.Text = "Bảng Điều Khiển";
            // 
            // btnNhanChipMayMan
            // 
            this.btnNhanChipMayMan.Enabled = false;
            this.btnNhanChipMayMan.Location = new System.Drawing.Point(519, 19);
            this.btnNhanChipMayMan.Name = "btnNhanChipMayMan";
            this.btnNhanChipMayMan.Size = new System.Drawing.Size(100, 30);
            this.btnNhanChipMayMan.TabIndex = 3;
            this.btnNhanChipMayMan.Text = "NCMM_Wait";
            this.btnNhanChipMayMan.UseVisualStyleBackColor = true;
            this.btnNhanChipMayMan.Click += new System.EventHandler(this.btnNhanChipMayMan_Click);
            // 
            // btnTangCo4La
            // 
            this.btnTangCo4La.Location = new System.Drawing.Point(413, 19);
            this.btnTangCo4La.Name = "btnTangCo4La";
            this.btnTangCo4La.Size = new System.Drawing.Size(100, 30);
            this.btnTangCo4La.TabIndex = 2;
            this.btnTangCo4La.Text = "NC4L(F4)";
            this.btnTangCo4La.UseVisualStyleBackColor = true;
            this.btnTangCo4La.Click += new System.EventHandler(this.btnTangCo4La_Click);
            // 
            // btnTangQuaBiMat
            // 
            this.btnTangQuaBiMat.Location = new System.Drawing.Point(112, 19);
            this.btnTangQuaBiMat.Name = "btnTangQuaBiMat";
            this.btnTangQuaBiMat.Size = new System.Drawing.Size(100, 30);
            this.btnTangQuaBiMat.TabIndex = 1;
            this.btnTangQuaBiMat.Text = "TQBM(F2)";
            this.btnTangQuaBiMat.UseVisualStyleBackColor = true;
            this.btnTangQuaBiMat.Click += new System.EventHandler(this.btnTangQuaBiMat_Click);
            // 
            // btnNhanThuongHangNgay
            // 
            this.btnNhanThuongHangNgay.Location = new System.Drawing.Point(6, 19);
            this.btnNhanThuongHangNgay.Name = "btnNhanThuongHangNgay";
            this.btnNhanThuongHangNgay.Size = new System.Drawing.Size(100, 30);
            this.btnNhanThuongHangNgay.TabIndex = 0;
            this.btnNhanThuongHangNgay.Text = "NTHN(F1)";
            this.btnNhanThuongHangNgay.UseVisualStyleBackColor = true;
            this.btnNhanThuongHangNgay.Click += new System.EventHandler(this.btnNhanThuongHangNgay_Click);
            // 
            // btnNhanThuongWeb
            // 
            this.btnNhanThuongWeb.Location = new System.Drawing.Point(307, 19);
            this.btnNhanThuongWeb.Name = "btnNhanThuongWeb";
            this.btnNhanThuongWeb.Size = new System.Drawing.Size(100, 30);
            this.btnNhanThuongWeb.TabIndex = 3;
            this.btnNhanThuongWeb.Text = "NCHN2+KT (F3)";
            this.btnNhanThuongWeb.UseVisualStyleBackColor = true;
            this.btnNhanThuongWeb.Click += new System.EventHandler(this.btnNhanThuongWeb_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 447);
            this.Controls.Add(this.groupMain);
            this.Controls.Add(this.groupData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PTV Nuker - Tinnv@VNIT Solutions";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupData.ResumeLayout(false);
            this.groupData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.menuGridData.ResumeLayout(false);
            this.groupMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupData;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.GroupBox groupMain;
        private System.Windows.Forms.ComboBox txtPackNo;
        private System.Windows.Forms.Button btnNhanChipMayMan;
        private System.Windows.Forms.Button btnThemPack;
        private System.Windows.Forms.Button btnThemTaiKhoan;
        private System.Windows.Forms.Button btnTangCo4La;
        private System.Windows.Forms.Button btnNhanThuongHangNgay;
        private System.Windows.Forms.Label lblPack;
        private System.Windows.Forms.Button btnKetBan;
        private System.Windows.Forms.Button btnTangQuaBiMat;
        private System.Windows.Forms.ContextMenuStrip menuGridData;
        private System.Windows.Forms.ToolStripMenuItem menuCopyURL;
        private System.Windows.Forms.ToolStripMenuItem menuXoaTK;
        private System.Windows.Forms.Button btnKiemTraTaiKhoan;
        private System.Windows.Forms.Button btnNhanThuongWeb;

    }
}

