namespace PokerTexas
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnNhanThuongHangNgay = new System.Windows.Forms.Button();
            this.txtPackNo = new System.Windows.Forms.ComboBox();
            this.lblPack = new System.Windows.Forms.Label();
            this.groupMain = new System.Windows.Forms.GroupBox();
            this.chkNhanThuong = new System.Windows.Forms.CheckBox();
            this.chkNhanCo4La = new System.Windows.Forms.CheckBox();
            this.btnNhanChipMayMan = new System.Windows.Forms.Button();
            this.btnThemPack = new System.Windows.Forms.Button();
            this.btnNhanThuongFanNhom = new System.Windows.Forms.Button();
            this.btnXoaTaiKhoan = new System.Windows.Forms.Button();
            this.btnThemTaiKhoan = new System.Windows.Forms.Button();
            this.btnTangCo4La = new System.Windows.Forms.Button();
            this.groupData = new System.Windows.Forms.GroupBox();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCopyFacebookCookie = new System.Windows.Forms.ToolStripMenuItem();
            this.groupCaptcha = new System.Windows.Forms.GroupBox();
            this.btnNhapCaptcha = new System.Windows.Forms.Button();
            this.txtCaptcha = new System.Windows.Forms.TextBox();
            this.picCaptcha = new System.Windows.Forms.PictureBox();
            this.chkNhanThuongFan = new System.Windows.Forms.CheckBox();
            this.txtHour = new System.Windows.Forms.TextBox();
            this.txtMinute = new System.Windows.Forms.TextBox();
            this.lblGio = new System.Windows.Forms.Label();
            this.txtPackNhanThuong = new System.Windows.Forms.TextBox();
            this.lblPackNhanThuong = new System.Windows.Forms.Label();
            this.txtLinkNhanThuong = new System.Windows.Forms.TextBox();
            this.lblLinhNhanThuong = new System.Windows.Forms.Label();
            this.groupMain.SuspendLayout();
            this.groupData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupCaptcha.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCaptcha)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNhanThuongHangNgay
            // 
            this.btnNhanThuongHangNgay.Location = new System.Drawing.Point(61, 103);
            this.btnNhanThuongHangNgay.Name = "btnNhanThuongHangNgay";
            this.btnNhanThuongHangNgay.Size = new System.Drawing.Size(84, 23);
            this.btnNhanThuongHangNgay.TabIndex = 1;
            this.btnNhanThuongHangNgay.Text = "Nhận Thưởng Hằng Ngày";
            this.btnNhanThuongHangNgay.UseVisualStyleBackColor = true;
            this.btnNhanThuongHangNgay.Click += new System.EventHandler(this.btnNhanThuongHangNgay_Click);
            // 
            // txtPackNo
            // 
            this.txtPackNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtPackNo.FormattingEnabled = true;
            this.txtPackNo.Location = new System.Drawing.Point(61, 19);
            this.txtPackNo.Name = "txtPackNo";
            this.txtPackNo.Size = new System.Drawing.Size(101, 21);
            this.txtPackNo.TabIndex = 0;
            this.txtPackNo.SelectedIndexChanged += new System.EventHandler(this.txtPackNo_SelectedIndexChanged);
            // 
            // lblPack
            // 
            this.lblPack.AutoSize = true;
            this.lblPack.Location = new System.Drawing.Point(7, 22);
            this.lblPack.Name = "lblPack";
            this.lblPack.Size = new System.Drawing.Size(52, 13);
            this.lblPack.TabIndex = 3;
            this.lblPack.Text = "Pack No.";
            // 
            // groupMain
            // 
            this.groupMain.Controls.Add(this.chkNhanThuong);
            this.groupMain.Controls.Add(this.chkNhanCo4La);
            this.groupMain.Controls.Add(this.txtPackNo);
            this.groupMain.Controls.Add(this.btnNhanChipMayMan);
            this.groupMain.Controls.Add(this.btnThemPack);
            this.groupMain.Controls.Add(this.btnNhanThuongFanNhom);
            this.groupMain.Controls.Add(this.btnXoaTaiKhoan);
            this.groupMain.Controls.Add(this.btnThemTaiKhoan);
            this.groupMain.Controls.Add(this.btnTangCo4La);
            this.groupMain.Controls.Add(this.btnNhanThuongHangNgay);
            this.groupMain.Controls.Add(this.lblPack);
            this.groupMain.Location = new System.Drawing.Point(12, 12);
            this.groupMain.Name = "groupMain";
            this.groupMain.Size = new System.Drawing.Size(258, 139);
            this.groupMain.TabIndex = 0;
            this.groupMain.TabStop = false;
            this.groupMain.Text = "Bảng Điều Khiển";
            // 
            // chkNhanThuong
            // 
            this.chkNhanThuong.AutoSize = true;
            this.chkNhanThuong.Location = new System.Drawing.Point(40, 108);
            this.chkNhanThuong.Name = "chkNhanThuong";
            this.chkNhanThuong.Size = new System.Drawing.Size(15, 14);
            this.chkNhanThuong.TabIndex = 6;
            this.chkNhanThuong.UseVisualStyleBackColor = true;
            // 
            // chkNhanCo4La
            // 
            this.chkNhanCo4La.AutoSize = true;
            this.chkNhanCo4La.Location = new System.Drawing.Point(40, 79);
            this.chkNhanCo4La.Name = "chkNhanCo4La";
            this.chkNhanCo4La.Size = new System.Drawing.Size(15, 14);
            this.chkNhanCo4La.TabIndex = 6;
            this.chkNhanCo4La.UseVisualStyleBackColor = true;
            // 
            // btnNhanChipMayMan
            // 
            this.btnNhanChipMayMan.Location = new System.Drawing.Point(61, 46);
            this.btnNhanChipMayMan.Name = "btnNhanChipMayMan";
            this.btnNhanChipMayMan.Size = new System.Drawing.Size(84, 23);
            this.btnNhanChipMayMan.TabIndex = 2;
            this.btnNhanChipMayMan.Text = "Nhận Chip May Mắn";
            this.btnNhanChipMayMan.UseVisualStyleBackColor = true;
            this.btnNhanChipMayMan.Click += new System.EventHandler(this.btnNhanChipMayMan_Click);
            // 
            // btnThemPack
            // 
            this.btnThemPack.Location = new System.Drawing.Point(168, 18);
            this.btnThemPack.Name = "btnThemPack";
            this.btnThemPack.Size = new System.Drawing.Size(79, 23);
            this.btnThemPack.TabIndex = 5;
            this.btnThemPack.Text = "Thêm Pack";
            this.btnThemPack.UseVisualStyleBackColor = true;
            this.btnThemPack.Click += new System.EventHandler(this.btnThemPack_Click);
            // 
            // btnNhanThuongFanNhom
            // 
            this.btnNhanThuongFanNhom.Location = new System.Drawing.Point(151, 104);
            this.btnNhanThuongFanNhom.Name = "btnNhanThuongFanNhom";
            this.btnNhanThuongFanNhom.Size = new System.Drawing.Size(96, 23);
            this.btnNhanThuongFanNhom.TabIndex = 1;
            this.btnNhanThuongFanNhom.Text = "Nhận Fan Nhóm";
            this.btnNhanThuongFanNhom.UseVisualStyleBackColor = true;
            this.btnNhanThuongFanNhom.Visible = false;
            this.btnNhanThuongFanNhom.Click += new System.EventHandler(this.btnNhanThuongFanNhom_Click);
            // 
            // btnXoaTaiKhoan
            // 
            this.btnXoaTaiKhoan.Location = new System.Drawing.Point(151, 75);
            this.btnXoaTaiKhoan.Name = "btnXoaTaiKhoan";
            this.btnXoaTaiKhoan.Size = new System.Drawing.Size(96, 23);
            this.btnXoaTaiKhoan.TabIndex = 4;
            this.btnXoaTaiKhoan.Text = "Xóa Tài Khoản";
            this.btnXoaTaiKhoan.UseVisualStyleBackColor = true;
            this.btnXoaTaiKhoan.Click += new System.EventHandler(this.btnXoaTaiKhoan_Click);
            // 
            // btnThemTaiKhoan
            // 
            this.btnThemTaiKhoan.Location = new System.Drawing.Point(151, 46);
            this.btnThemTaiKhoan.Name = "btnThemTaiKhoan";
            this.btnThemTaiKhoan.Size = new System.Drawing.Size(96, 23);
            this.btnThemTaiKhoan.TabIndex = 3;
            this.btnThemTaiKhoan.Text = "Thêm Tài Khoản";
            this.btnThemTaiKhoan.UseVisualStyleBackColor = true;
            this.btnThemTaiKhoan.Click += new System.EventHandler(this.btnThemTaiKhoan_Click);
            // 
            // btnTangCo4La
            // 
            this.btnTangCo4La.Location = new System.Drawing.Point(61, 75);
            this.btnTangCo4La.Name = "btnTangCo4La";
            this.btnTangCo4La.Size = new System.Drawing.Size(84, 23);
            this.btnTangCo4La.TabIndex = 1;
            this.btnTangCo4La.Text = "Nhận Cỏ 4 Lá";
            this.btnTangCo4La.UseVisualStyleBackColor = true;
            this.btnTangCo4La.Click += new System.EventHandler(this.btnTangCo4La_Click);
            // 
            // groupData
            // 
            this.groupData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupData.Controls.Add(this.gridData);
            this.groupData.Location = new System.Drawing.Point(12, 157);
            this.groupData.Name = "groupData";
            this.groupData.Size = new System.Drawing.Size(542, 262);
            this.groupData.TabIndex = 1;
            this.groupData.TabStop = false;
            this.groupData.Text = "Tài Khoản Trong Pack";
            // 
            // gridData
            // 
            this.gridData.AllowUserToAddRows = false;
            this.gridData.AllowUserToDeleteRows = false;
            this.gridData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridData.ContextMenuStrip = this.contextMenuStrip1;
            this.gridData.Location = new System.Drawing.Point(6, 19);
            this.gridData.Name = "gridData";
            this.gridData.ReadOnly = true;
            this.gridData.Size = new System.Drawing.Size(530, 237);
            this.gridData.TabIndex = 0;
            this.gridData.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridData_CellDoubleClick);
            this.gridData.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridData_CellMouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopyFacebookCookie});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(197, 26);
            // 
            // menuCopyFacebookCookie
            // 
            this.menuCopyFacebookCookie.Name = "menuCopyFacebookCookie";
            this.menuCopyFacebookCookie.Size = new System.Drawing.Size(196, 22);
            this.menuCopyFacebookCookie.Text = "Copy Facebook Cookie";
            this.menuCopyFacebookCookie.Click += new System.EventHandler(this.menuCopyFacebookCookie_Click);
            // 
            // groupCaptcha
            // 
            this.groupCaptcha.Controls.Add(this.btnNhapCaptcha);
            this.groupCaptcha.Controls.Add(this.txtCaptcha);
            this.groupCaptcha.Controls.Add(this.picCaptcha);
            this.groupCaptcha.Location = new System.Drawing.Point(276, 16);
            this.groupCaptcha.Name = "groupCaptcha";
            this.groupCaptcha.Size = new System.Drawing.Size(284, 98);
            this.groupCaptcha.TabIndex = 2;
            this.groupCaptcha.TabStop = false;
            this.groupCaptcha.Text = "Nhập Captcha";
            this.groupCaptcha.Visible = false;
            // 
            // btnNhapCaptcha
            // 
            this.btnNhapCaptcha.Location = new System.Drawing.Point(6, 46);
            this.btnNhapCaptcha.Name = "btnNhapCaptcha";
            this.btnNhapCaptcha.Size = new System.Drawing.Size(69, 23);
            this.btnNhapCaptcha.TabIndex = 3;
            this.btnNhapCaptcha.Text = "Nhập";
            this.btnNhapCaptcha.UseVisualStyleBackColor = true;
            this.btnNhapCaptcha.Click += new System.EventHandler(this.btnNhapCaptcha_Click);
            // 
            // txtCaptcha
            // 
            this.txtCaptcha.Location = new System.Drawing.Point(6, 20);
            this.txtCaptcha.Name = "txtCaptcha";
            this.txtCaptcha.Size = new System.Drawing.Size(69, 20);
            this.txtCaptcha.TabIndex = 2;
            this.txtCaptcha.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCaptcha_KeyUp);
            // 
            // picCaptcha
            // 
            this.picCaptcha.Location = new System.Drawing.Point(80, 18);
            this.picCaptcha.Name = "picCaptcha";
            this.picCaptcha.Size = new System.Drawing.Size(200, 70);
            this.picCaptcha.TabIndex = 1;
            this.picCaptcha.TabStop = false;
            // 
            // chkNhanThuongFan
            // 
            this.chkNhanThuongFan.AutoSize = true;
            this.chkNhanThuongFan.Location = new System.Drawing.Point(276, 119);
            this.chkNhanThuongFan.Name = "chkNhanThuongFan";
            this.chkNhanThuongFan.Size = new System.Drawing.Size(113, 17);
            this.chkNhanThuongFan.TabIndex = 3;
            this.chkNhanThuongFan.Text = "Nhận Thưởng Fan";
            this.chkNhanThuongFan.UseVisualStyleBackColor = true;
            this.chkNhanThuongFan.Visible = false;
            this.chkNhanThuongFan.CheckedChanged += new System.EventHandler(this.chkNhanThuongFan_CheckedChanged);
            // 
            // txtHour
            // 
            this.txtHour.Location = new System.Drawing.Point(415, 116);
            this.txtHour.Name = "txtHour";
            this.txtHour.Size = new System.Drawing.Size(27, 20);
            this.txtHour.TabIndex = 4;
            this.txtHour.Text = "9";
            this.txtHour.Visible = false;
            // 
            // txtMinute
            // 
            this.txtMinute.Location = new System.Drawing.Point(448, 116);
            this.txtMinute.Name = "txtMinute";
            this.txtMinute.Size = new System.Drawing.Size(27, 20);
            this.txtMinute.TabIndex = 4;
            this.txtMinute.Text = "0";
            this.txtMinute.Visible = false;
            // 
            // lblGio
            // 
            this.lblGio.AutoSize = true;
            this.lblGio.Location = new System.Drawing.Point(386, 120);
            this.lblGio.Name = "lblGio";
            this.lblGio.Size = new System.Drawing.Size(23, 13);
            this.lblGio.TabIndex = 5;
            this.lblGio.Text = "Giờ";
            this.lblGio.Visible = false;
            // 
            // txtPackNhanThuong
            // 
            this.txtPackNhanThuong.Location = new System.Drawing.Point(523, 116);
            this.txtPackNhanThuong.Name = "txtPackNhanThuong";
            this.txtPackNhanThuong.Size = new System.Drawing.Size(37, 20);
            this.txtPackNhanThuong.TabIndex = 4;
            this.txtPackNhanThuong.Text = "1,2,3";
            this.txtPackNhanThuong.Visible = false;
            // 
            // lblPackNhanThuong
            // 
            this.lblPackNhanThuong.AutoSize = true;
            this.lblPackNhanThuong.Location = new System.Drawing.Point(485, 120);
            this.lblPackNhanThuong.Name = "lblPackNhanThuong";
            this.lblPackNhanThuong.Size = new System.Drawing.Size(32, 13);
            this.lblPackNhanThuong.TabIndex = 5;
            this.lblPackNhanThuong.Text = "Pack";
            this.lblPackNhanThuong.Visible = false;
            // 
            // txtLinkNhanThuong
            // 
            this.txtLinkNhanThuong.Location = new System.Drawing.Point(380, 140);
            this.txtLinkNhanThuong.Name = "txtLinkNhanThuong";
            this.txtLinkNhanThuong.Size = new System.Drawing.Size(180, 20);
            this.txtLinkNhanThuong.TabIndex = 6;
            this.txtLinkNhanThuong.Visible = false;
            // 
            // lblLinhNhanThuong
            // 
            this.lblLinhNhanThuong.AutoSize = true;
            this.lblLinhNhanThuong.Location = new System.Drawing.Point(533, 143);
            this.lblLinhNhanThuong.Name = "lblLinhNhanThuong";
            this.lblLinhNhanThuong.Size = new System.Drawing.Size(27, 13);
            this.lblLinhNhanThuong.TabIndex = 5;
            this.lblLinhNhanThuong.Text = "Link";
            this.lblLinhNhanThuong.Visible = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 431);
            this.Controls.Add(this.txtLinkNhanThuong);
            this.Controls.Add(this.lblPackNhanThuong);
            this.Controls.Add(this.lblLinhNhanThuong);
            this.Controls.Add(this.lblGio);
            this.Controls.Add(this.txtPackNhanThuong);
            this.Controls.Add(this.txtMinute);
            this.Controls.Add(this.txtHour);
            this.Controls.Add(this.chkNhanThuongFan);
            this.Controls.Add(this.groupCaptcha);
            this.Controls.Add(this.groupData);
            this.Controls.Add(this.groupMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "PTV Nuker - Tinnv@VNIT Solutions";
            this.groupMain.ResumeLayout(false);
            this.groupMain.PerformLayout();
            this.groupData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupCaptcha.ResumeLayout(false);
            this.groupCaptcha.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCaptcha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox txtPackNo;
        private System.Windows.Forms.Button btnNhanThuongHangNgay;
        private System.Windows.Forms.Label lblPack;
        private System.Windows.Forms.GroupBox groupMain;
        private System.Windows.Forms.Button btnNhanChipMayMan;
        private System.Windows.Forms.Button btnThemTaiKhoan;
        private System.Windows.Forms.GroupBox groupData;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.Button btnThemPack;
        private System.Windows.Forms.Button btnXoaTaiKhoan;
        private System.Windows.Forms.GroupBox groupCaptcha;
        private System.Windows.Forms.PictureBox picCaptcha;
        private System.Windows.Forms.TextBox txtCaptcha;
        private System.Windows.Forms.Button btnNhapCaptcha;
        private System.Windows.Forms.Button btnTangCo4La;
        private System.Windows.Forms.Button btnNhanThuongFanNhom;
        private System.Windows.Forms.CheckBox chkNhanThuongFan;
        private System.Windows.Forms.TextBox txtHour;
        private System.Windows.Forms.TextBox txtMinute;
        private System.Windows.Forms.Label lblGio;
        private System.Windows.Forms.TextBox txtPackNhanThuong;
        private System.Windows.Forms.Label lblPackNhanThuong;
        private System.Windows.Forms.TextBox txtLinkNhanThuong;
        private System.Windows.Forms.Label lblLinhNhanThuong;
        private System.Windows.Forms.CheckBox chkNhanThuong;
        private System.Windows.Forms.CheckBox chkNhanCo4La;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuCopyFacebookCookie;
    }
}

