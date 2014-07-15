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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabMain = new System.Windows.Forms.TabControl();
            this.pageLogin = new System.Windows.Forms.TabPage();
            this.btnLogin = new System.Windows.Forms.Button();
            this.pageThanh = new System.Windows.Forms.TabPage();
            this.grbLeoThap = new System.Windows.Forms.GroupBox();
            this.txtLeoThapSoSao = new System.Windows.Forms.TextBox();
            this.txtLeoThapSoThap = new System.Windows.Forms.TextBox();
            this.txtSoSao = new System.Windows.Forms.Label();
            this.txtSoThap = new System.Windows.Forms.Label();
            this.btnLeoThap = new System.Windows.Forms.Button();
            this.grbNuotQLCC = new System.Windows.Forms.GroupBox();
            this.txtQLCC = new System.Windows.Forms.ComboBox();
            this.txtDungQLCC_SoLuong = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnDungQLCC = new System.Windows.Forms.Button();
            this.grbVuotAi = new System.Windows.Forms.GroupBox();
            this.chkVuotAiChieuHangTuong = new System.Windows.Forms.CheckBox();
            this.txtVuotAiTenAi = new System.Windows.Forms.ComboBox();
            this.txtVuotAiDoKho = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnVuotAi = new System.Windows.Forms.Button();
            this.grbThuThanh = new System.Windows.Forms.GroupBox();
            this.btnThuThanh = new System.Windows.Forms.Button();
            this.chkDanhLuotCuoi = new System.Windows.Forms.CheckBox();
            this.chkChayHetQL = new System.Windows.Forms.CheckBox();
            this.pageTuong = new System.Windows.Forms.TabPage();
            this.grbDuoiTuong = new System.Windows.Forms.GroupBox();
            this.txtSaThaiTuongLevel = new System.Windows.Forms.TextBox();
            this.txtSaThaiTuongRank = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSaThaiTuong = new System.Windows.Forms.Button();
            this.grbGhepManhTuong = new System.Windows.Forms.GroupBox();
            this.txtGhepManhTuongRank = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGhepManhTuongTuDong = new System.Windows.Forms.Button();
            this.pageLinhThachVuKhi = new System.Windows.Forms.TabPage();
            this.grbQuayRuong = new System.Windows.Forms.GroupBox();
            this.txtQuayRuongLoaiRuong = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnQuayRuong = new System.Windows.Forms.Button();
            this.grbGhepNgocVuKhi = new System.Windows.Forms.GroupBox();
            this.txtGhepDoSoLuong = new System.Windows.Forms.ComboBox();
            this.txtGhepDoCap = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtGhepDoTenDo = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnBanVuKhiLinhThach = new System.Windows.Forms.Button();
            this.btnGhepLinhThachVuKhi = new System.Windows.Forms.Button();
            this.pageLevel = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.txtAuthenticationToken = new System.Windows.Forms.TextBox();
            this.btnGiamThoiGianCho = new System.Windows.Forms.Button();
            this.btnUpLevelTheoDinhKy = new System.Windows.Forms.Button();
            this.btnBatDauUpLevel = new System.Windows.Forms.Button();
            this.grbStatus = new System.Windows.Forms.GroupBox();
            this.txtProcess = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabMain.SuspendLayout();
            this.pageLogin.SuspendLayout();
            this.pageThanh.SuspendLayout();
            this.grbLeoThap.SuspendLayout();
            this.grbNuotQLCC.SuspendLayout();
            this.grbVuotAi.SuspendLayout();
            this.grbThuThanh.SuspendLayout();
            this.pageTuong.SuspendLayout();
            this.grbDuoiTuong.SuspendLayout();
            this.grbGhepManhTuong.SuspendLayout();
            this.pageLinhThachVuKhi.SuspendLayout();
            this.grbQuayRuong.SuspendLayout();
            this.grbGhepNgocVuKhi.SuspendLayout();
            this.pageLevel.SuspendLayout();
            this.grbStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.pageLogin);
            this.tabMain.Controls.Add(this.pageThanh);
            this.tabMain.Controls.Add(this.pageTuong);
            this.tabMain.Controls.Add(this.pageLinhThachVuKhi);
            this.tabMain.Controls.Add(this.pageLevel);
            this.tabMain.ItemSize = new System.Drawing.Size(100, 30);
            this.tabMain.Location = new System.Drawing.Point(12, 12);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(760, 225);
            this.tabMain.TabIndex = 0;
            // 
            // pageLogin
            // 
            this.pageLogin.Controls.Add(this.btnLogin);
            this.pageLogin.Location = new System.Drawing.Point(4, 34);
            this.pageLogin.Name = "pageLogin";
            this.pageLogin.Padding = new System.Windows.Forms.Padding(3);
            this.pageLogin.Size = new System.Drawing.Size(752, 187);
            this.pageLogin.TabIndex = 0;
            this.pageLogin.Text = "Đăng Nhập";
            this.pageLogin.UseVisualStyleBackColor = true;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(281, 31);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(190, 124);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            // 
            // pageThanh
            // 
            this.pageThanh.Controls.Add(this.grbLeoThap);
            this.pageThanh.Controls.Add(this.grbNuotQLCC);
            this.pageThanh.Controls.Add(this.grbVuotAi);
            this.pageThanh.Controls.Add(this.grbThuThanh);
            this.pageThanh.Location = new System.Drawing.Point(4, 34);
            this.pageThanh.Name = "pageThanh";
            this.pageThanh.Padding = new System.Windows.Forms.Padding(3);
            this.pageThanh.Size = new System.Drawing.Size(752, 187);
            this.pageThanh.TabIndex = 1;
            this.pageThanh.Text = "     Thành     ";
            this.pageThanh.UseVisualStyleBackColor = true;
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
            // grbNuotQLCC
            // 
            this.grbNuotQLCC.Controls.Add(this.txtQLCC);
            this.grbNuotQLCC.Controls.Add(this.txtDungQLCC_SoLuong);
            this.grbNuotQLCC.Controls.Add(this.label6);
            this.grbNuotQLCC.Controls.Add(this.label7);
            this.grbNuotQLCC.Controls.Add(this.btnDungQLCC);
            this.grbNuotQLCC.Location = new System.Drawing.Point(364, 86);
            this.grbNuotQLCC.Name = "grbNuotQLCC";
            this.grbNuotQLCC.Size = new System.Drawing.Size(350, 92);
            this.grbNuotQLCC.TabIndex = 0;
            this.grbNuotQLCC.TabStop = false;
            this.grbNuotQLCC.Text = "Dùng Quân Lệnh / Cờ Chiến";
            // 
            // txtQLCC
            // 
            this.txtQLCC.FormattingEnabled = true;
            this.txtQLCC.Location = new System.Drawing.Point(81, 17);
            this.txtQLCC.Name = "txtQLCC";
            this.txtQLCC.Size = new System.Drawing.Size(100, 21);
            this.txtQLCC.TabIndex = 6;
            // 
            // txtDungQLCC_SoLuong
            // 
            this.txtDungQLCC_SoLuong.Location = new System.Drawing.Point(81, 40);
            this.txtDungQLCC_SoLuong.Name = "txtDungQLCC_SoLuong";
            this.txtDungQLCC_SoLuong.Size = new System.Drawing.Size(100, 20);
            this.txtDungQLCC_SoLuong.TabIndex = 5;
            this.txtDungQLCC_SoLuong.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Số Lượng";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Tên Đồ";
            // 
            // btnDungQLCC
            // 
            this.btnDungQLCC.Location = new System.Drawing.Point(233, 19);
            this.btnDungQLCC.Name = "btnDungQLCC";
            this.btnDungQLCC.Size = new System.Drawing.Size(111, 40);
            this.btnDungQLCC.TabIndex = 1;
            this.btnDungQLCC.Text = "Dùng";
            this.btnDungQLCC.UseVisualStyleBackColor = true;
            // 
            // grbVuotAi
            // 
            this.grbVuotAi.Controls.Add(this.chkVuotAiChieuHangTuong);
            this.grbVuotAi.Controls.Add(this.txtVuotAiTenAi);
            this.grbVuotAi.Controls.Add(this.txtVuotAiDoKho);
            this.grbVuotAi.Controls.Add(this.label3);
            this.grbVuotAi.Controls.Add(this.label5);
            this.grbVuotAi.Controls.Add(this.btnVuotAi);
            this.grbVuotAi.Location = new System.Drawing.Point(8, 86);
            this.grbVuotAi.Name = "grbVuotAi";
            this.grbVuotAi.Size = new System.Drawing.Size(350, 92);
            this.grbVuotAi.TabIndex = 0;
            this.grbVuotAi.TabStop = false;
            this.grbVuotAi.Text = "Vượt Ải";
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
            // txtVuotAiTenAi
            // 
            this.txtVuotAiTenAi.FormattingEnabled = true;
            this.txtVuotAiTenAi.Location = new System.Drawing.Point(81, 17);
            this.txtVuotAiTenAi.Name = "txtVuotAiTenAi";
            this.txtVuotAiTenAi.Size = new System.Drawing.Size(100, 21);
            this.txtVuotAiTenAi.TabIndex = 6;
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
            // pageTuong
            // 
            this.pageTuong.Controls.Add(this.grbDuoiTuong);
            this.pageTuong.Controls.Add(this.grbGhepManhTuong);
            this.pageTuong.Location = new System.Drawing.Point(4, 34);
            this.pageTuong.Name = "pageTuong";
            this.pageTuong.Padding = new System.Windows.Forms.Padding(3);
            this.pageTuong.Size = new System.Drawing.Size(752, 187);
            this.pageTuong.TabIndex = 3;
            this.pageTuong.Text = "     Tướng     ";
            this.pageTuong.UseVisualStyleBackColor = true;
            // 
            // grbDuoiTuong
            // 
            this.grbDuoiTuong.Controls.Add(this.txtSaThaiTuongLevel);
            this.grbDuoiTuong.Controls.Add(this.txtSaThaiTuongRank);
            this.grbDuoiTuong.Controls.Add(this.label1);
            this.grbDuoiTuong.Controls.Add(this.label2);
            this.grbDuoiTuong.Controls.Add(this.btnSaThaiTuong);
            this.grbDuoiTuong.Location = new System.Drawing.Point(8, 6);
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
            // grbGhepManhTuong
            // 
            this.grbGhepManhTuong.Controls.Add(this.txtGhepManhTuongRank);
            this.grbGhepManhTuong.Controls.Add(this.label4);
            this.grbGhepManhTuong.Controls.Add(this.btnGhepManhTuongTuDong);
            this.grbGhepManhTuong.Location = new System.Drawing.Point(364, 6);
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
            // btnGhepManhTuongTuDong
            // 
            this.btnGhepManhTuongTuDong.Location = new System.Drawing.Point(233, 19);
            this.btnGhepManhTuongTuDong.Name = "btnGhepManhTuongTuDong";
            this.btnGhepManhTuongTuDong.Size = new System.Drawing.Size(111, 40);
            this.btnGhepManhTuongTuDong.TabIndex = 1;
            this.btnGhepManhTuongTuDong.Text = "Ghép Mảnh Tướng Tự Động";
            this.btnGhepManhTuongTuDong.UseVisualStyleBackColor = true;
            // 
            // pageLinhThachVuKhi
            // 
            this.pageLinhThachVuKhi.Controls.Add(this.grbQuayRuong);
            this.pageLinhThachVuKhi.Controls.Add(this.grbGhepNgocVuKhi);
            this.pageLinhThachVuKhi.Location = new System.Drawing.Point(4, 34);
            this.pageLinhThachVuKhi.Name = "pageLinhThachVuKhi";
            this.pageLinhThachVuKhi.Padding = new System.Windows.Forms.Padding(3);
            this.pageLinhThachVuKhi.Size = new System.Drawing.Size(752, 187);
            this.pageLinhThachVuKhi.TabIndex = 4;
            this.pageLinhThachVuKhi.Text = "Linh Thạch / Vũ Khí";
            this.pageLinhThachVuKhi.UseVisualStyleBackColor = true;
            // 
            // grbQuayRuong
            // 
            this.grbQuayRuong.Controls.Add(this.txtQuayRuongLoaiRuong);
            this.grbQuayRuong.Controls.Add(this.label13);
            this.grbQuayRuong.Controls.Add(this.btnQuayRuong);
            this.grbQuayRuong.Location = new System.Drawing.Point(369, 6);
            this.grbQuayRuong.Name = "grbQuayRuong";
            this.grbQuayRuong.Size = new System.Drawing.Size(357, 105);
            this.grbQuayRuong.TabIndex = 1;
            this.grbQuayRuong.TabStop = false;
            this.grbQuayRuong.Text = "Quay Rương";
            // 
            // txtQuayRuongLoaiRuong
            // 
            this.txtQuayRuongLoaiRuong.FormattingEnabled = true;
            this.txtQuayRuongLoaiRuong.Location = new System.Drawing.Point(81, 44);
            this.txtQuayRuongLoaiRuong.Name = "txtQuayRuongLoaiRuong";
            this.txtQuayRuongLoaiRuong.Size = new System.Drawing.Size(100, 21);
            this.txtQuayRuongLoaiRuong.TabIndex = 6;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "Loại Rương";
            // 
            // btnQuayRuong
            // 
            this.btnQuayRuong.Location = new System.Drawing.Point(208, 34);
            this.btnQuayRuong.Name = "btnQuayRuong";
            this.btnQuayRuong.Size = new System.Drawing.Size(111, 40);
            this.btnQuayRuong.TabIndex = 1;
            this.btnQuayRuong.Text = "Quay Rương";
            this.btnQuayRuong.UseVisualStyleBackColor = true;
            // 
            // grbGhepNgocVuKhi
            // 
            this.grbGhepNgocVuKhi.Controls.Add(this.txtGhepDoSoLuong);
            this.grbGhepNgocVuKhi.Controls.Add(this.txtGhepDoCap);
            this.grbGhepNgocVuKhi.Controls.Add(this.label10);
            this.grbGhepNgocVuKhi.Controls.Add(this.txtGhepDoTenDo);
            this.grbGhepNgocVuKhi.Controls.Add(this.label8);
            this.grbGhepNgocVuKhi.Controls.Add(this.label9);
            this.grbGhepNgocVuKhi.Controls.Add(this.btnBanVuKhiLinhThach);
            this.grbGhepNgocVuKhi.Controls.Add(this.btnGhepLinhThachVuKhi);
            this.grbGhepNgocVuKhi.Location = new System.Drawing.Point(6, 6);
            this.grbGhepNgocVuKhi.Name = "grbGhepNgocVuKhi";
            this.grbGhepNgocVuKhi.Size = new System.Drawing.Size(357, 105);
            this.grbGhepNgocVuKhi.TabIndex = 1;
            this.grbGhepNgocVuKhi.TabStop = false;
            this.grbGhepNgocVuKhi.Text = "Ghép, Bán Linh Thạch  Vũ Khí";
            // 
            // txtGhepDoSoLuong
            // 
            this.txtGhepDoSoLuong.FormattingEnabled = true;
            this.txtGhepDoSoLuong.Location = new System.Drawing.Point(81, 71);
            this.txtGhepDoSoLuong.Name = "txtGhepDoSoLuong";
            this.txtGhepDoSoLuong.Size = new System.Drawing.Size(100, 21);
            this.txtGhepDoSoLuong.TabIndex = 6;
            // 
            // txtGhepDoCap
            // 
            this.txtGhepDoCap.FormattingEnabled = true;
            this.txtGhepDoCap.Location = new System.Drawing.Point(81, 44);
            this.txtGhepDoCap.Name = "txtGhepDoCap";
            this.txtGhepDoCap.Size = new System.Drawing.Size(100, 21);
            this.txtGhepDoCap.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 75);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Số Lượng";
            // 
            // txtGhepDoTenDo
            // 
            this.txtGhepDoTenDo.FormattingEnabled = true;
            this.txtGhepDoTenDo.Location = new System.Drawing.Point(81, 17);
            this.txtGhepDoTenDo.Name = "txtGhepDoTenDo";
            this.txtGhepDoTenDo.Size = new System.Drawing.Size(100, 21);
            this.txtGhepDoTenDo.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(26, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Cấp";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Tên Đồ";
            // 
            // btnBanVuKhiLinhThach
            // 
            this.btnBanVuKhiLinhThach.Location = new System.Drawing.Point(289, 34);
            this.btnBanVuKhiLinhThach.Name = "btnBanVuKhiLinhThach";
            this.btnBanVuKhiLinhThach.Size = new System.Drawing.Size(50, 40);
            this.btnBanVuKhiLinhThach.TabIndex = 1;
            this.btnBanVuKhiLinhThach.Text = "Bán";
            this.btnBanVuKhiLinhThach.UseVisualStyleBackColor = true;
            // 
            // btnGhepLinhThachVuKhi
            // 
            this.btnGhepLinhThachVuKhi.Location = new System.Drawing.Point(233, 34);
            this.btnGhepLinhThachVuKhi.Name = "btnGhepLinhThachVuKhi";
            this.btnGhepLinhThachVuKhi.Size = new System.Drawing.Size(50, 40);
            this.btnGhepLinhThachVuKhi.TabIndex = 1;
            this.btnGhepLinhThachVuKhi.Text = "Ghép";
            this.btnGhepLinhThachVuKhi.UseVisualStyleBackColor = true;
            // 
            // pageLevel
            // 
            this.pageLevel.Controls.Add(this.label11);
            this.pageLevel.Controls.Add(this.txtAuthenticationToken);
            this.pageLevel.Controls.Add(this.btnGiamThoiGianCho);
            this.pageLevel.Controls.Add(this.btnUpLevelTheoDinhKy);
            this.pageLevel.Controls.Add(this.btnBatDauUpLevel);
            this.pageLevel.Location = new System.Drawing.Point(4, 34);
            this.pageLevel.Name = "pageLevel";
            this.pageLevel.Padding = new System.Windows.Forms.Padding(3);
            this.pageLevel.Size = new System.Drawing.Size(752, 187);
            this.pageLevel.TabIndex = 2;
            this.pageLevel.Text = "     Level     ";
            this.pageLevel.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(125, 98);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(107, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "authentication_token";
            // 
            // txtAuthenticationToken
            // 
            this.txtAuthenticationToken.Location = new System.Drawing.Point(128, 118);
            this.txtAuthenticationToken.Name = "txtAuthenticationToken";
            this.txtAuthenticationToken.Size = new System.Drawing.Size(264, 20);
            this.txtAuthenticationToken.TabIndex = 3;
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
            // btnUpLevelTheoDinhKy
            // 
            this.btnUpLevelTheoDinhKy.Location = new System.Drawing.Point(8, 98);
            this.btnUpLevelTheoDinhKy.Name = "btnUpLevelTheoDinhKy";
            this.btnUpLevelTheoDinhKy.Size = new System.Drawing.Size(111, 40);
            this.btnUpLevelTheoDinhKy.TabIndex = 2;
            this.btnUpLevelTheoDinhKy.Text = "Tăng cấp theo định kỳ";
            this.btnUpLevelTheoDinhKy.UseVisualStyleBackColor = true;
            this.btnUpLevelTheoDinhKy.Click += new System.EventHandler(this.btnUpLevelTheoDinhKy_Click);
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
            // grbStatus
            // 
            this.grbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbStatus.Controls.Add(this.txtProcess);
            this.grbStatus.Controls.Add(this.txtStatus);
            this.grbStatus.Location = new System.Drawing.Point(12, 243);
            this.grbStatus.Name = "grbStatus";
            this.grbStatus.Size = new System.Drawing.Size(760, 306);
            this.grbStatus.TabIndex = 3;
            this.grbStatus.TabStop = false;
            this.grbStatus.Text = "Status";
            // 
            // txtProcess
            // 
            this.txtProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProcess.Location = new System.Drawing.Point(373, 19);
            this.txtProcess.Multiline = true;
            this.txtProcess.Name = "txtProcess";
            this.txtProcess.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtProcess.Size = new System.Drawing.Size(381, 281);
            this.txtProcess.TabIndex = 0;
            // 
            // txtStatus
            // 
            this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatus.Location = new System.Drawing.Point(6, 19);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(361, 281);
            this.txtStatus.TabIndex = 0;
            // 
            // trayIcon
            // 
            this.trayIcon.BalloonTipText = "Vua Thủ Thành";
            this.trayIcon.BalloonTipTitle = "Vua Thủ Thành";
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Vua Thủ Thành";
            this.trayIcon.Visible = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.grbStatus);
            this.Controls.Add(this.tabMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VTT";
            this.tabMain.ResumeLayout(false);
            this.pageLogin.ResumeLayout(false);
            this.pageThanh.ResumeLayout(false);
            this.grbLeoThap.ResumeLayout(false);
            this.grbLeoThap.PerformLayout();
            this.grbNuotQLCC.ResumeLayout(false);
            this.grbNuotQLCC.PerformLayout();
            this.grbVuotAi.ResumeLayout(false);
            this.grbVuotAi.PerformLayout();
            this.grbThuThanh.ResumeLayout(false);
            this.grbThuThanh.PerformLayout();
            this.pageTuong.ResumeLayout(false);
            this.grbDuoiTuong.ResumeLayout(false);
            this.grbDuoiTuong.PerformLayout();
            this.grbGhepManhTuong.ResumeLayout(false);
            this.grbGhepManhTuong.PerformLayout();
            this.pageLinhThachVuKhi.ResumeLayout(false);
            this.grbQuayRuong.ResumeLayout(false);
            this.grbQuayRuong.PerformLayout();
            this.grbGhepNgocVuKhi.ResumeLayout(false);
            this.grbGhepNgocVuKhi.PerformLayout();
            this.pageLevel.ResumeLayout(false);
            this.pageLevel.PerformLayout();
            this.grbStatus.ResumeLayout(false);
            this.grbStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage pageLogin;
        private System.Windows.Forms.TabPage pageThanh;
        private System.Windows.Forms.TabPage pageLevel;
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
        private System.Windows.Forms.Button btnGhepManhTuongTuDong;
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
        private System.Windows.Forms.GroupBox grbNuotQLCC;
        private System.Windows.Forms.ComboBox txtQLCC;
        private System.Windows.Forms.TextBox txtDungQLCC_SoLuong;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnDungQLCC;
        private System.Windows.Forms.TabPage pageTuong;
        private System.Windows.Forms.TabPage pageLinhThachVuKhi;
        private System.Windows.Forms.GroupBox grbGhepNgocVuKhi;
        private System.Windows.Forms.ComboBox txtGhepDoTenDo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnGhepLinhThachVuKhi;
        private System.Windows.Forms.Button btnBanVuKhiLinhThach;
        private System.Windows.Forms.ComboBox txtGhepDoCap;
        private System.Windows.Forms.ComboBox txtGhepDoSoLuong;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtProcess;
        private System.Windows.Forms.GroupBox grbQuayRuong;
        private System.Windows.Forms.ComboBox txtQuayRuongLoaiRuong;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnQuayRuong;
        private System.Windows.Forms.Button btnUpLevelTheoDinhKy;
        private System.Windows.Forms.TextBox txtAuthenticationToken;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NotifyIcon trayIcon;
    }
}

