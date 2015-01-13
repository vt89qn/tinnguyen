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
            this.menuLoginLaiFacebook = new System.Windows.Forms.ToolStripMenuItem();
            this.lblPack = new System.Windows.Forms.Label();
            this.txtPackNo = new System.Windows.Forms.ComboBox();
            this.btnThemPack = new System.Windows.Forms.Button();
            this.btnCapNhatNgaySinh = new System.Windows.Forms.Button();
            this.btnKetBan = new System.Windows.Forms.Button();
            this.btnKiemTraTaiKhoan = new System.Windows.Forms.Button();
            this.btnThemTaiKhoan = new System.Windows.Forms.Button();
            this.btnCheckWeb = new System.Windows.Forms.Button();
            this.btnCheckMobile = new System.Windows.Forms.Button();
            this.txtCheckCo4La = new System.Windows.Forms.CheckBox();
            this.txtCheckChipMayMan = new System.Windows.Forms.CheckBox();
            this.txtCheckKyTen = new System.Windows.Forms.CheckBox();
            this.btnChuanBiRutFan = new System.Windows.Forms.Button();
            this.btnRutThuongFan = new System.Windows.Forms.Button();
            this.txtRutFanLink = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupRutfan = new System.Windows.Forms.GroupBox();
            this.btnAuthenNhanFanChip = new System.Windows.Forms.Button();
            this.txtCheckTuDong = new System.Windows.Forms.CheckBox();
            this.txtCheckMobile = new System.Windows.Forms.CheckBox();
            this.txtCheckWeb = new System.Windows.Forms.CheckBox();
            this.txtCheckDangNhapLT = new System.Windows.Forms.CheckBox();
            this.txtCheckChipBiMat = new System.Windows.Forms.CheckBox();
            this.txtCheckHangNgay = new System.Windows.Forms.CheckBox();
            this.groupData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.menuGridData.SuspendLayout();
            this.groupRutfan.SuspendLayout();
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
            this.groupData.Controls.Add(this.btnCapNhatNgaySinh);
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
            this.menuXoaTK,
            this.menuLoginLaiFacebook});
            this.menuGridData.Name = "menuGridData";
            this.menuGridData.Size = new System.Drawing.Size(177, 70);
            this.menuGridData.Opening += new System.ComponentModel.CancelEventHandler(this.menuGridData_Opening);
            // 
            // menuCopyURL
            // 
            this.menuCopyURL.Name = "menuCopyURL";
            this.menuCopyURL.Size = new System.Drawing.Size(176, 22);
            this.menuCopyURL.Text = "Copy URL";
            this.menuCopyURL.Click += new System.EventHandler(this.menuCopyURL_Click);
            // 
            // menuXoaTK
            // 
            this.menuXoaTK.Name = "menuXoaTK";
            this.menuXoaTK.Size = new System.Drawing.Size(176, 22);
            this.menuXoaTK.Text = "Xóa TK";
            this.menuXoaTK.Click += new System.EventHandler(this.menuXoaTK_Click);
            // 
            // menuLoginLaiFacebook
            // 
            this.menuLoginLaiFacebook.Name = "menuLoginLaiFacebook";
            this.menuLoginLaiFacebook.Size = new System.Drawing.Size(176, 22);
            this.menuLoginLaiFacebook.Text = "Login Lại FaceBook";
            this.menuLoginLaiFacebook.Click += new System.EventHandler(this.menuLoginLaiFacebook_Click);
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
            // btnCapNhatNgaySinh
            // 
            this.btnCapNhatNgaySinh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCapNhatNgaySinh.Location = new System.Drawing.Point(657, 23);
            this.btnCapNhatNgaySinh.Name = "btnCapNhatNgaySinh";
            this.btnCapNhatNgaySinh.Size = new System.Drawing.Size(100, 30);
            this.btnCapNhatNgaySinh.TabIndex = 5;
            this.btnCapNhatNgaySinh.Text = "Lấy Ngày Sinh";
            this.btnCapNhatNgaySinh.UseVisualStyleBackColor = true;
            this.btnCapNhatNgaySinh.Click += new System.EventHandler(this.btnCapNhatNgaySinh_Click);
            // 
            // btnKetBan
            // 
            this.btnKetBan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKetBan.Location = new System.Drawing.Point(551, 23);
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
            this.btnKiemTraTaiKhoan.Location = new System.Drawing.Point(339, 23);
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
            this.btnThemTaiKhoan.Location = new System.Drawing.Point(445, 23);
            this.btnThemTaiKhoan.Name = "btnThemTaiKhoan";
            this.btnThemTaiKhoan.Size = new System.Drawing.Size(100, 30);
            this.btnThemTaiKhoan.TabIndex = 4;
            this.btnThemTaiKhoan.Text = "Thêm TK(F8)";
            this.btnThemTaiKhoan.UseVisualStyleBackColor = true;
            this.btnThemTaiKhoan.Click += new System.EventHandler(this.btnThemTaiKhoan_Click);
            // 
            // btnCheckWeb
            // 
            this.btnCheckWeb.Location = new System.Drawing.Point(265, 11);
            this.btnCheckWeb.Name = "btnCheckWeb";
            this.btnCheckWeb.Size = new System.Drawing.Size(65, 50);
            this.btnCheckWeb.TabIndex = 3;
            this.btnCheckWeb.Text = "Web(F2)";
            this.btnCheckWeb.UseVisualStyleBackColor = true;
            this.btnCheckWeb.Click += new System.EventHandler(this.btnCheckWeb_Click);
            // 
            // btnCheckMobile
            // 
            this.btnCheckMobile.Location = new System.Drawing.Point(89, 12);
            this.btnCheckMobile.Name = "btnCheckMobile";
            this.btnCheckMobile.Size = new System.Drawing.Size(65, 50);
            this.btnCheckMobile.TabIndex = 0;
            this.btnCheckMobile.Text = "Mobile(F1)";
            this.btnCheckMobile.UseVisualStyleBackColor = true;
            this.btnCheckMobile.Click += new System.EventHandler(this.btnCheckMobile_Click);
            // 
            // txtCheckCo4La
            // 
            this.txtCheckCo4La.AutoSize = true;
            this.txtCheckCo4La.Checked = true;
            this.txtCheckCo4La.CheckState = System.Windows.Forms.CheckState.Checked;
            this.txtCheckCo4La.Location = new System.Drawing.Point(336, 29);
            this.txtCheckCo4La.Name = "txtCheckCo4La";
            this.txtCheckCo4La.Size = new System.Drawing.Size(63, 17);
            this.txtCheckCo4La.TabIndex = 4;
            this.txtCheckCo4La.Text = "Cỏ 4 Lá";
            this.txtCheckCo4La.UseVisualStyleBackColor = true;
            // 
            // txtCheckChipMayMan
            // 
            this.txtCheckChipMayMan.AutoSize = true;
            this.txtCheckChipMayMan.Checked = true;
            this.txtCheckChipMayMan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.txtCheckChipMayMan.Location = new System.Drawing.Point(336, 46);
            this.txtCheckChipMayMan.Name = "txtCheckChipMayMan";
            this.txtCheckChipMayMan.Size = new System.Drawing.Size(94, 17);
            this.txtCheckChipMayMan.TabIndex = 4;
            this.txtCheckChipMayMan.Text = "Chip May Mắn";
            this.txtCheckChipMayMan.UseVisualStyleBackColor = true;
            // 
            // txtCheckKyTen
            // 
            this.txtCheckKyTen.AutoSize = true;
            this.txtCheckKyTen.Checked = true;
            this.txtCheckKyTen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.txtCheckKyTen.Location = new System.Drawing.Point(336, 12);
            this.txtCheckKyTen.Name = "txtCheckKyTen";
            this.txtCheckKyTen.Size = new System.Drawing.Size(60, 17);
            this.txtCheckKyTen.TabIndex = 4;
            this.txtCheckKyTen.Text = "Ký Tên";
            this.txtCheckKyTen.UseVisualStyleBackColor = true;
            // 
            // btnChuanBiRutFan
            // 
            this.btnChuanBiRutFan.Location = new System.Drawing.Point(451, 12);
            this.btnChuanBiRutFan.Name = "btnChuanBiRutFan";
            this.btnChuanBiRutFan.Size = new System.Drawing.Size(100, 50);
            this.btnChuanBiRutFan.TabIndex = 3;
            this.btnChuanBiRutFan.Text = "Chuẩn bị nhận Fan chip";
            this.btnChuanBiRutFan.UseVisualStyleBackColor = true;
            this.btnChuanBiRutFan.Click += new System.EventHandler(this.btnChuanBiRutFan_Click);
            // 
            // btnRutThuongFan
            // 
            this.btnRutThuongFan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRutThuongFan.Location = new System.Drawing.Point(165, 12);
            this.btnRutThuongFan.Name = "btnRutThuongFan";
            this.btnRutThuongFan.Size = new System.Drawing.Size(50, 36);
            this.btnRutThuongFan.TabIndex = 3;
            this.btnRutThuongFan.Text = "NHẬN";
            this.btnRutThuongFan.UseVisualStyleBackColor = true;
            this.btnRutThuongFan.Click += new System.EventHandler(this.btnRutThuongFan_Click);
            // 
            // txtRutFanLink
            // 
            this.txtRutFanLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRutFanLink.Location = new System.Drawing.Point(6, 27);
            this.txtRutFanLink.Name = "txtRutFanLink";
            this.txtRutFanLink.Size = new System.Drawing.Size(99, 20);
            this.txtRutFanLink.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Link";
            // 
            // groupRutfan
            // 
            this.groupRutfan.Controls.Add(this.txtRutFanLink);
            this.groupRutfan.Controls.Add(this.btnAuthenNhanFanChip);
            this.groupRutfan.Controls.Add(this.btnRutThuongFan);
            this.groupRutfan.Controls.Add(this.label1);
            this.groupRutfan.Location = new System.Drawing.Point(557, 7);
            this.groupRutfan.Name = "groupRutfan";
            this.groupRutfan.Size = new System.Drawing.Size(218, 55);
            this.groupRutfan.TabIndex = 7;
            this.groupRutfan.TabStop = false;
            this.groupRutfan.Visible = false;
            // 
            // btnAuthenNhanFanChip
            // 
            this.btnAuthenNhanFanChip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAuthenNhanFanChip.Location = new System.Drawing.Point(111, 12);
            this.btnAuthenNhanFanChip.Name = "btnAuthenNhanFanChip";
            this.btnAuthenNhanFanChip.Size = new System.Drawing.Size(50, 36);
            this.btnAuthenNhanFanChip.TabIndex = 3;
            this.btnAuthenNhanFanChip.Text = "Authen All";
            this.btnAuthenNhanFanChip.UseVisualStyleBackColor = true;
            this.btnAuthenNhanFanChip.Click += new System.EventHandler(this.btnAuthenNhanFanChip_Click);
            // 
            // txtCheckTuDong
            // 
            this.txtCheckTuDong.AutoSize = true;
            this.txtCheckTuDong.Checked = true;
            this.txtCheckTuDong.CheckState = System.Windows.Forms.CheckState.Checked;
            this.txtCheckTuDong.Location = new System.Drawing.Point(8, 12);
            this.txtCheckTuDong.Name = "txtCheckTuDong";
            this.txtCheckTuDong.Size = new System.Drawing.Size(82, 17);
            this.txtCheckTuDong.TabIndex = 4;
            this.txtCheckTuDong.Text = "Check Auto";
            this.txtCheckTuDong.UseVisualStyleBackColor = true;
            // 
            // txtCheckMobile
            // 
            this.txtCheckMobile.AutoSize = true;
            this.txtCheckMobile.Checked = true;
            this.txtCheckMobile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.txtCheckMobile.Location = new System.Drawing.Point(8, 29);
            this.txtCheckMobile.Name = "txtCheckMobile";
            this.txtCheckMobile.Size = new System.Drawing.Size(57, 17);
            this.txtCheckMobile.TabIndex = 4;
            this.txtCheckMobile.Text = "Mobile";
            this.txtCheckMobile.UseVisualStyleBackColor = true;
            // 
            // txtCheckWeb
            // 
            this.txtCheckWeb.AutoSize = true;
            this.txtCheckWeb.Checked = true;
            this.txtCheckWeb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.txtCheckWeb.Location = new System.Drawing.Point(8, 46);
            this.txtCheckWeb.Name = "txtCheckWeb";
            this.txtCheckWeb.Size = new System.Drawing.Size(49, 17);
            this.txtCheckWeb.TabIndex = 4;
            this.txtCheckWeb.Text = "Web";
            this.txtCheckWeb.UseVisualStyleBackColor = true;
            // 
            // txtCheckDangNhapLT
            // 
            this.txtCheckDangNhapLT.AutoSize = true;
            this.txtCheckDangNhapLT.Checked = true;
            this.txtCheckDangNhapLT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.txtCheckDangNhapLT.Location = new System.Drawing.Point(160, 12);
            this.txtCheckDangNhapLT.Name = "txtCheckDangNhapLT";
            this.txtCheckDangNhapLT.Size = new System.Drawing.Size(97, 17);
            this.txtCheckDangNhapLT.TabIndex = 4;
            this.txtCheckDangNhapLT.Text = "Đăng Nhập LT";
            this.txtCheckDangNhapLT.UseVisualStyleBackColor = true;
            // 
            // txtCheckChipBiMat
            // 
            this.txtCheckChipBiMat.AutoSize = true;
            this.txtCheckChipBiMat.Checked = true;
            this.txtCheckChipBiMat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.txtCheckChipBiMat.Location = new System.Drawing.Point(160, 46);
            this.txtCheckChipBiMat.Name = "txtCheckChipBiMat";
            this.txtCheckChipBiMat.Size = new System.Drawing.Size(82, 17);
            this.txtCheckChipBiMat.TabIndex = 4;
            this.txtCheckChipBiMat.Text = "Chip Bí Mật";
            this.txtCheckChipBiMat.UseVisualStyleBackColor = true;
            // 
            // txtCheckHangNgay
            // 
            this.txtCheckHangNgay.AutoSize = true;
            this.txtCheckHangNgay.Location = new System.Drawing.Point(160, 29);
            this.txtCheckHangNgay.Name = "txtCheckHangNgay";
            this.txtCheckHangNgay.Size = new System.Drawing.Size(98, 17);
            this.txtCheckHangNgay.TabIndex = 4;
            this.txtCheckHangNgay.Text = "Hằng Ngày 2M";
            this.txtCheckHangNgay.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 447);
            this.Controls.Add(this.groupRutfan);
            this.Controls.Add(this.txtCheckChipMayMan);
            this.Controls.Add(this.txtCheckWeb);
            this.Controls.Add(this.txtCheckMobile);
            this.Controls.Add(this.txtCheckTuDong);
            this.Controls.Add(this.txtCheckChipBiMat);
            this.Controls.Add(this.txtCheckHangNgay);
            this.Controls.Add(this.txtCheckDangNhapLT);
            this.Controls.Add(this.txtCheckKyTen);
            this.Controls.Add(this.txtCheckCo4La);
            this.Controls.Add(this.btnCheckWeb);
            this.Controls.Add(this.btnChuanBiRutFan);
            this.Controls.Add(this.btnCheckMobile);
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
            this.groupRutfan.ResumeLayout(false);
            this.groupRutfan.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupData;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.ComboBox txtPackNo;
        private System.Windows.Forms.Button btnThemPack;
        private System.Windows.Forms.Button btnThemTaiKhoan;
        private System.Windows.Forms.Button btnCheckMobile;
        private System.Windows.Forms.Label lblPack;
        private System.Windows.Forms.Button btnKetBan;
        private System.Windows.Forms.ContextMenuStrip menuGridData;
        private System.Windows.Forms.ToolStripMenuItem menuCopyURL;
        private System.Windows.Forms.ToolStripMenuItem menuXoaTK;
        private System.Windows.Forms.Button btnKiemTraTaiKhoan;
        private System.Windows.Forms.Button btnCheckWeb;
        private System.Windows.Forms.Button btnCapNhatNgaySinh;
        private System.Windows.Forms.CheckBox txtCheckChipMayMan;
        private System.Windows.Forms.CheckBox txtCheckKyTen;
        private System.Windows.Forms.CheckBox txtCheckCo4La;
        private System.Windows.Forms.Button btnChuanBiRutFan;
        private System.Windows.Forms.Button btnRutThuongFan;
        private System.Windows.Forms.TextBox txtRutFanLink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupRutfan;
        private System.Windows.Forms.Button btnAuthenNhanFanChip;
        private System.Windows.Forms.CheckBox txtCheckTuDong;
        private System.Windows.Forms.ToolStripMenuItem menuLoginLaiFacebook;
        private System.Windows.Forms.CheckBox txtCheckMobile;
        private System.Windows.Forms.CheckBox txtCheckWeb;
        private System.Windows.Forms.CheckBox txtCheckDangNhapLT;
        private System.Windows.Forms.CheckBox txtCheckChipBiMat;
        private System.Windows.Forms.CheckBox txtCheckHangNgay;

    }
}

