namespace VTT
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
            this.tabMain = new System.Windows.Forms.TabControl();
            this.pageLogin = new System.Windows.Forms.TabPage();
            this.btnLogin = new System.Windows.Forms.Button();
            this.pageTool = new System.Windows.Forms.TabPage();
            this.grbLeoThap = new System.Windows.Forms.GroupBox();
            this.txtLeoThapSoSao = new System.Windows.Forms.TextBox();
            this.txtLeoThapSoThap = new System.Windows.Forms.TextBox();
            this.txtSoSao = new System.Windows.Forms.Label();
            this.txtSoThap = new System.Windows.Forms.Label();
            this.btnLeoThap = new System.Windows.Forms.Button();
            this.grbGhepManhTuong = new System.Windows.Forms.GroupBox();
            this.txtGhepManhTuongRank = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.grbDuoiTuong = new System.Windows.Forms.GroupBox();
            this.txtSaThaiTuongLevel = new System.Windows.Forms.TextBox();
            this.txtSaThaiTuongRank = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSaThaiTuong = new System.Windows.Forms.Button();
            this.grbThuThanh = new System.Windows.Forms.GroupBox();
            this.btnThuThanh = new System.Windows.Forms.Button();
            this.chkDanhLuotCuoi = new System.Windows.Forms.CheckBox();
            this.chkChayHetQL = new System.Windows.Forms.CheckBox();
            this.pageUpLevel = new System.Windows.Forms.TabPage();
            this.btnGiamThoiGianCho = new System.Windows.Forms.Button();
            this.btnBatDauUpLevel = new System.Windows.Forms.Button();
            this.grbVuotAi = new System.Windows.Forms.GroupBox();
            this.txtVuotAiDoKho = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnVuotAi = new System.Windows.Forms.Button();
            this.txtVuotAiTenAi = new System.Windows.Forms.ComboBox();
            this.chkVuotAiChieuHangTuong = new System.Windows.Forms.CheckBox();
            this.grbStatus = new System.Windows.Forms.GroupBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.tabMain.SuspendLayout();
            this.pageLogin.SuspendLayout();
            this.pageTool.SuspendLayout();
            this.grbLeoThap.SuspendLayout();
            this.grbGhepManhTuong.SuspendLayout();
            this.grbDuoiTuong.SuspendLayout();
            this.grbThuThanh.SuspendLayout();
            this.pageUpLevel.SuspendLayout();
            this.grbVuotAi.SuspendLayout();
            this.grbStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.pageLogin);
            this.tabMain.Controls.Add(this.pageTool);
            this.tabMain.Controls.Add(this.pageUpLevel);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(817, 534);
            this.tabMain.TabIndex = 0;
            // 
            // pageLogin
            // 
            this.pageLogin.Controls.Add(this.btnLogin);
            this.pageLogin.Location = new System.Drawing.Point(4, 22);
            this.pageLogin.Name = "pageLogin";
            this.pageLogin.Padding = new System.Windows.Forms.Padding(3);
            this.pageLogin.Size = new System.Drawing.Size(809, 508);
            this.pageLogin.TabIndex = 0;
            this.pageLogin.Text = "Đăng Nhập";
            this.pageLogin.UseVisualStyleBackColor = true;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(309, 192);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(190, 124);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            // 
            // pageTool
            // 
            this.pageTool.Controls.Add(this.grbStatus);
            this.pageTool.Controls.Add(this.grbLeoThap);
            this.pageTool.Controls.Add(this.grbGhepManhTuong);
            this.pageTool.Controls.Add(this.grbVuotAi);
            this.pageTool.Controls.Add(this.grbDuoiTuong);
            this.pageTool.Controls.Add(this.grbThuThanh);
            this.pageTool.Location = new System.Drawing.Point(4, 22);
            this.pageTool.Name = "pageTool";
            this.pageTool.Padding = new System.Windows.Forms.Padding(3);
            this.pageTool.Size = new System.Drawing.Size(809, 508);
            this.pageTool.TabIndex = 1;
            this.pageTool.Text = "VTT Tool";
            this.pageTool.UseVisualStyleBackColor = true;
            // 
            // grbLeoThap
            // 
            this.grbLeoThap.Controls.Add(this.txtLeoThapSoSao);
            this.grbLeoThap.Controls.Add(this.txtLeoThapSoThap);
            this.grbLeoThap.Controls.Add(this.txtSoSao);
            this.grbLeoThap.Controls.Add(this.txtSoThap);
            this.grbLeoThap.Controls.Add(this.btnLeoThap);
            this.grbLeoThap.Location = new System.Drawing.Point(364, 6);
            this.grbLeoThap.Name = "grbLeoThap";
            this.grbLeoThap.Size = new System.Drawing.Size(350, 74);
            this.grbLeoThap.TabIndex = 1;
            this.grbLeoThap.TabStop = false;
            this.grbLeoThap.Text = "Leo Tháp";
            // 
            // txtLeoThapSoSao
            // 
            this.txtLeoThapSoSao.Location = new System.Drawing.Point(81, 40);
            this.txtLeoThapSoSao.Name = "txtLeoThapSoSao";
            this.txtLeoThapSoSao.Size = new System.Drawing.Size(100, 20);
            this.txtLeoThapSoSao.TabIndex = 2;
            this.txtLeoThapSoSao.Text = "301";
            // 
            // txtLeoThapSoThap
            // 
            this.txtLeoThapSoThap.Location = new System.Drawing.Point(81, 17);
            this.txtLeoThapSoThap.Name = "txtLeoThapSoThap";
            this.txtLeoThapSoThap.Size = new System.Drawing.Size(100, 20);
            this.txtLeoThapSoThap.TabIndex = 2;
            this.txtLeoThapSoThap.Text = "81";
            // 
            // txtSoSao
            // 
            this.txtSoSao.AutoSize = true;
            this.txtSoSao.Location = new System.Drawing.Point(6, 44);
            this.txtSoSao.Name = "txtSoSao";
            this.txtSoSao.Size = new System.Drawing.Size(40, 13);
            this.txtSoSao.TabIndex = 2;
            this.txtSoSao.Text = "Số sao";
            // 
            // txtSoThap
            // 
            this.txtSoThap.AutoSize = true;
            this.txtSoThap.Location = new System.Drawing.Point(6, 21);
            this.txtSoThap.Name = "txtSoThap";
            this.txtSoThap.Size = new System.Drawing.Size(44, 13);
            this.txtSoThap.TabIndex = 2;
            this.txtSoThap.Text = "Số tháp";
            // 
            // btnLeoThap
            // 
            this.btnLeoThap.Location = new System.Drawing.Point(233, 19);
            this.btnLeoThap.Name = "btnLeoThap";
            this.btnLeoThap.Size = new System.Drawing.Size(111, 40);
            this.btnLeoThap.TabIndex = 0;
            this.btnLeoThap.Text = "Leo Tháp";
            this.btnLeoThap.UseVisualStyleBackColor = true;
            // 
            // grbGhepManhTuong
            // 
            this.grbGhepManhTuong.Controls.Add(this.txtGhepManhTuongRank);
            this.grbGhepManhTuong.Controls.Add(this.label4);
            this.grbGhepManhTuong.Controls.Add(this.button1);
            this.grbGhepManhTuong.Location = new System.Drawing.Point(364, 86);
            this.grbGhepManhTuong.Name = "grbGhepManhTuong";
            this.grbGhepManhTuong.Size = new System.Drawing.Size(350, 74);
            this.grbGhepManhTuong.TabIndex = 0;
            this.grbGhepManhTuong.TabStop = false;
            this.grbGhepManhTuong.Text = "Ghép Mảnh Tướng Tự Động";
            // 
            // txtGhepManhTuongRank
            // 
            this.txtGhepManhTuongRank.Location = new System.Drawing.Point(81, 17);
            this.txtGhepManhTuongRank.Name = "txtGhepManhTuongRank";
            this.txtGhepManhTuongRank.Size = new System.Drawing.Size(100, 20);
            this.txtGhepManhTuongRank.TabIndex = 6;
            this.txtGhepManhTuongRank.Text = "3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Rank <=";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(233, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 40);
            this.button1.TabIndex = 1;
            this.button1.Text = "Sa Thải Tướng";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // grbDuoiTuong
            // 
            this.grbDuoiTuong.Controls.Add(this.txtSaThaiTuongLevel);
            this.grbDuoiTuong.Controls.Add(this.txtSaThaiTuongRank);
            this.grbDuoiTuong.Controls.Add(this.label1);
            this.grbDuoiTuong.Controls.Add(this.label2);
            this.grbDuoiTuong.Controls.Add(this.btnSaThaiTuong);
            this.grbDuoiTuong.Location = new System.Drawing.Point(8, 86);
            this.grbDuoiTuong.Name = "grbDuoiTuong";
            this.grbDuoiTuong.Size = new System.Drawing.Size(350, 74);
            this.grbDuoiTuong.TabIndex = 0;
            this.grbDuoiTuong.TabStop = false;
            this.grbDuoiTuong.Text = "Sa Thải Tướng";
            // 
            // txtSaThaiTuongLevel
            // 
            this.txtSaThaiTuongLevel.Location = new System.Drawing.Point(81, 40);
            this.txtSaThaiTuongLevel.Name = "txtSaThaiTuongLevel";
            this.txtSaThaiTuongLevel.Size = new System.Drawing.Size(100, 20);
            this.txtSaThaiTuongLevel.TabIndex = 5;
            this.txtSaThaiTuongLevel.Text = "1";
            // 
            // txtSaThaiTuongRank
            // 
            this.txtSaThaiTuongRank.Location = new System.Drawing.Point(81, 17);
            this.txtSaThaiTuongRank.Name = "txtSaThaiTuongRank";
            this.txtSaThaiTuongRank.Size = new System.Drawing.Size(100, 20);
            this.txtSaThaiTuongRank.TabIndex = 6;
            this.txtSaThaiTuongRank.Text = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Level <=";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Rank <=";
            // 
            // btnSaThaiTuong
            // 
            this.btnSaThaiTuong.Location = new System.Drawing.Point(233, 19);
            this.btnSaThaiTuong.Name = "btnSaThaiTuong";
            this.btnSaThaiTuong.Size = new System.Drawing.Size(111, 40);
            this.btnSaThaiTuong.TabIndex = 1;
            this.btnSaThaiTuong.Text = "Sa Thải Tướng";
            this.btnSaThaiTuong.UseVisualStyleBackColor = true;
            // 
            // grbThuThanh
            // 
            this.grbThuThanh.Controls.Add(this.btnThuThanh);
            this.grbThuThanh.Controls.Add(this.chkDanhLuotCuoi);
            this.grbThuThanh.Controls.Add(this.chkChayHetQL);
            this.grbThuThanh.Location = new System.Drawing.Point(8, 6);
            this.grbThuThanh.Name = "grbThuThanh";
            this.grbThuThanh.Size = new System.Drawing.Size(350, 74);
            this.grbThuThanh.TabIndex = 0;
            this.grbThuThanh.TabStop = false;
            this.grbThuThanh.Text = "Thủ Thành";
            // 
            // btnThuThanh
            // 
            this.btnThuThanh.Location = new System.Drawing.Point(233, 20);
            this.btnThuThanh.Name = "btnThuThanh";
            this.btnThuThanh.Size = new System.Drawing.Size(111, 40);
            this.btnThuThanh.TabIndex = 1;
            this.btnThuThanh.Text = "Thủ Thành";
            this.btnThuThanh.UseVisualStyleBackColor = true;
            // 
            // chkDanhLuotCuoi
            // 
            this.chkDanhLuotCuoi.AutoSize = true;
            this.chkDanhLuotCuoi.Location = new System.Drawing.Point(6, 42);
            this.chkDanhLuotCuoi.Name = "chkDanhLuotCuoi";
            this.chkDanhLuotCuoi.Size = new System.Drawing.Size(118, 17);
            this.chkDanhLuotCuoi.TabIndex = 0;
            this.chkDanhLuotCuoi.Text = "Đánh luôn lượt cuối";
            this.chkDanhLuotCuoi.UseVisualStyleBackColor = true;
            // 
            // chkChayHetQL
            // 
            this.chkChayHetQL.AutoSize = true;
            this.chkChayHetQL.Location = new System.Drawing.Point(6, 19);
            this.chkChayHetQL.Name = "chkChayHetQL";
            this.chkChayHetQL.Size = new System.Drawing.Size(85, 17);
            this.chkChayHetQL.TabIndex = 0;
            this.chkChayHetQL.Text = "Chạy hết QL";
            this.chkChayHetQL.UseVisualStyleBackColor = true;
            // 
            // pageUpLevel
            // 
            this.pageUpLevel.Controls.Add(this.btnGiamThoiGianCho);
            this.pageUpLevel.Controls.Add(this.btnBatDauUpLevel);
            this.pageUpLevel.Location = new System.Drawing.Point(4, 22);
            this.pageUpLevel.Name = "pageUpLevel";
            this.pageUpLevel.Padding = new System.Windows.Forms.Padding(3);
            this.pageUpLevel.Size = new System.Drawing.Size(809, 508);
            this.pageUpLevel.TabIndex = 2;
            this.pageUpLevel.Text = "Up Level";
            this.pageUpLevel.UseVisualStyleBackColor = true;
            // 
            // btnGiamThoiGianCho
            // 
            this.btnGiamThoiGianCho.Location = new System.Drawing.Point(8, 52);
            this.btnGiamThoiGianCho.Name = "btnGiamThoiGianCho";
            this.btnGiamThoiGianCho.Size = new System.Drawing.Size(111, 40);
            this.btnGiamThoiGianCho.TabIndex = 1;
            this.btnGiamThoiGianCho.Text = "Giảm thời gian chờ";
            this.btnGiamThoiGianCho.UseVisualStyleBackColor = true;
            // 
            // btnBatDauUpLevel
            // 
            this.btnBatDauUpLevel.Location = new System.Drawing.Point(8, 6);
            this.btnBatDauUpLevel.Name = "btnBatDauUpLevel";
            this.btnBatDauUpLevel.Size = new System.Drawing.Size(111, 40);
            this.btnBatDauUpLevel.TabIndex = 0;
            this.btnBatDauUpLevel.Text = "Bắt đầu";
            this.btnBatDauUpLevel.UseVisualStyleBackColor = true;
            // 
            // grbVuotAi
            // 
            this.grbVuotAi.Controls.Add(this.chkVuotAiChieuHangTuong);
            this.grbVuotAi.Controls.Add(this.txtVuotAiTenAi);
            this.grbVuotAi.Controls.Add(this.txtVuotAiDoKho);
            this.grbVuotAi.Controls.Add(this.label3);
            this.grbVuotAi.Controls.Add(this.label5);
            this.grbVuotAi.Controls.Add(this.btnVuotAi);
            this.grbVuotAi.Location = new System.Drawing.Point(8, 166);
            this.grbVuotAi.Name = "grbVuotAi";
            this.grbVuotAi.Size = new System.Drawing.Size(350, 92);
            this.grbVuotAi.TabIndex = 0;
            this.grbVuotAi.TabStop = false;
            this.grbVuotAi.Text = "Vượt Ải";
            // 
            // txtVuotAiDoKho
            // 
            this.txtVuotAiDoKho.Location = new System.Drawing.Point(81, 40);
            this.txtVuotAiDoKho.Name = "txtVuotAiDoKho";
            this.txtVuotAiDoKho.Size = new System.Drawing.Size(100, 20);
            this.txtVuotAiDoKho.TabIndex = 5;
            this.txtVuotAiDoKho.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Độ Khó";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Tên Ải";
            // 
            // btnVuotAi
            // 
            this.btnVuotAi.Location = new System.Drawing.Point(233, 19);
            this.btnVuotAi.Name = "btnVuotAi";
            this.btnVuotAi.Size = new System.Drawing.Size(111, 40);
            this.btnVuotAi.TabIndex = 1;
            this.btnVuotAi.Text = "Vượt Ải";
            this.btnVuotAi.UseVisualStyleBackColor = true;
            // 
            // txtVuotAiTenAi
            // 
            this.txtVuotAiTenAi.FormattingEnabled = true;
            this.txtVuotAiTenAi.Location = new System.Drawing.Point(81, 17);
            this.txtVuotAiTenAi.Name = "txtVuotAiTenAi";
            this.txtVuotAiTenAi.Size = new System.Drawing.Size(100, 21);
            this.txtVuotAiTenAi.TabIndex = 6;
            // 
            // chkVuotAiChieuHangTuong
            // 
            this.chkVuotAiChieuHangTuong.AutoSize = true;
            this.chkVuotAiChieuHangTuong.Checked = true;
            this.chkVuotAiChieuHangTuong.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVuotAiChieuHangTuong.Location = new System.Drawing.Point(81, 67);
            this.chkVuotAiChieuHangTuong.Name = "chkVuotAiChieuHangTuong";
            this.chkVuotAiChieuHangTuong.Size = new System.Drawing.Size(116, 17);
            this.chkVuotAiChieuHangTuong.TabIndex = 7;
            this.chkVuotAiChieuHangTuong.Text = "Chiêu Hàng Tướng";
            this.chkVuotAiChieuHangTuong.UseVisualStyleBackColor = true;
            // 
            // grbStatus
            // 
            this.grbStatus.Controls.Add(this.txtStatus);
            this.grbStatus.Location = new System.Drawing.Point(8, 349);
            this.grbStatus.Name = "grbStatus";
            this.grbStatus.Size = new System.Drawing.Size(793, 151);
            this.grbStatus.TabIndex = 2;
            this.grbStatus.TabStop = false;
            this.grbStatus.Text = "Status";
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(9, 19);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(778, 126);
            this.txtStatus.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 534);
            this.Controls.Add(this.tabMain);
            this.Name = "MainForm";
            this.Text = "VTT";
            this.tabMain.ResumeLayout(false);
            this.pageLogin.ResumeLayout(false);
            this.pageTool.ResumeLayout(false);
            this.grbLeoThap.ResumeLayout(false);
            this.grbLeoThap.PerformLayout();
            this.grbGhepManhTuong.ResumeLayout(false);
            this.grbGhepManhTuong.PerformLayout();
            this.grbDuoiTuong.ResumeLayout(false);
            this.grbDuoiTuong.PerformLayout();
            this.grbThuThanh.ResumeLayout(false);
            this.grbThuThanh.PerformLayout();
            this.pageUpLevel.ResumeLayout(false);
            this.grbVuotAi.ResumeLayout(false);
            this.grbVuotAi.PerformLayout();
            this.grbStatus.ResumeLayout(false);
            this.grbStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage pageLogin;
        private System.Windows.Forms.TabPage pageTool;
        private System.Windows.Forms.TabPage pageUpLevel;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.GroupBox grbThuThanh;
        private System.Windows.Forms.CheckBox chkChayHetQL;
        private System.Windows.Forms.CheckBox chkDanhLuotCuoi;
        private System.Windows.Forms.Button btnThuThanh;
        private System.Windows.Forms.GroupBox grbLeoThap;
        private System.Windows.Forms.Button btnLeoThap;
        private System.Windows.Forms.TextBox txtLeoThapSoSao;
        private System.Windows.Forms.TextBox txtLeoThapSoThap;
        private System.Windows.Forms.Label txtSoSao;
        private System.Windows.Forms.Label txtSoThap;
        private System.Windows.Forms.GroupBox grbDuoiTuong;
        private System.Windows.Forms.Button btnSaThaiTuong;
        private System.Windows.Forms.TextBox txtSaThaiTuongLevel;
        private System.Windows.Forms.TextBox txtSaThaiTuongRank;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grbGhepManhTuong;
        private System.Windows.Forms.TextBox txtGhepManhTuongRank;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnBatDauUpLevel;
        private System.Windows.Forms.Button btnGiamThoiGianCho;
        private System.Windows.Forms.GroupBox grbVuotAi;
        private System.Windows.Forms.TextBox txtVuotAiDoKho;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnVuotAi;
        private System.Windows.Forms.ComboBox txtVuotAiTenAi;
        private System.Windows.Forms.CheckBox chkVuotAiChieuHangTuong;
        private System.Windows.Forms.GroupBox grbStatus;
        private System.Windows.Forms.TextBox txtStatus;
    }
}

