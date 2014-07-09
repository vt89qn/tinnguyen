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
            this.pageUpLevel = new System.Windows.Forms.TabPage();
            this.grbThuThanh = new System.Windows.Forms.GroupBox();
            this.chkChayHetQL = new System.Windows.Forms.CheckBox();
            this.chkDanhLuotCuoi = new System.Windows.Forms.CheckBox();
            this.btnThuThanh = new System.Windows.Forms.Button();
            this.grbLeoThap = new System.Windows.Forms.GroupBox();
            this.btnLeoThap = new System.Windows.Forms.Button();
            this.txtSoThap = new System.Windows.Forms.Label();
            this.txtSoSao = new System.Windows.Forms.Label();
            this.txtLeoThapSoThap = new System.Windows.Forms.TextBox();
            this.txtLeoThapSoSao = new System.Windows.Forms.TextBox();
            this.grbDuoiTuong = new System.Windows.Forms.GroupBox();
            this.btnSaThaiTuong = new System.Windows.Forms.Button();
            this.txtSaThaiTuongLevel = new System.Windows.Forms.TextBox();
            this.txtSaThaiTuongRank = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grbGhepManhTuong = new System.Windows.Forms.GroupBox();
            this.txtGhepManhTuongRank = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tabMain.SuspendLayout();
            this.pageLogin.SuspendLayout();
            this.pageTool.SuspendLayout();
            this.grbThuThanh.SuspendLayout();
            this.grbLeoThap.SuspendLayout();
            this.grbDuoiTuong.SuspendLayout();
            this.grbGhepManhTuong.SuspendLayout();
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
            this.pageTool.Controls.Add(this.grbLeoThap);
            this.pageTool.Controls.Add(this.grbGhepManhTuong);
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
            // pageUpLevel
            // 
            this.pageUpLevel.Location = new System.Drawing.Point(4, 22);
            this.pageUpLevel.Name = "pageUpLevel";
            this.pageUpLevel.Padding = new System.Windows.Forms.Padding(3);
            this.pageUpLevel.Size = new System.Drawing.Size(809, 508);
            this.pageUpLevel.TabIndex = 2;
            this.pageUpLevel.Text = "Up Level";
            this.pageUpLevel.UseVisualStyleBackColor = true;
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
            // btnThuThanh
            // 
            this.btnThuThanh.Location = new System.Drawing.Point(233, 20);
            this.btnThuThanh.Name = "btnThuThanh";
            this.btnThuThanh.Size = new System.Drawing.Size(111, 40);
            this.btnThuThanh.TabIndex = 1;
            this.btnThuThanh.Text = "Thủ Thành";
            this.btnThuThanh.UseVisualStyleBackColor = true;
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
            // btnLeoThap
            // 
            this.btnLeoThap.Location = new System.Drawing.Point(233, 19);
            this.btnLeoThap.Name = "btnLeoThap";
            this.btnLeoThap.Size = new System.Drawing.Size(111, 40);
            this.btnLeoThap.TabIndex = 0;
            this.btnLeoThap.Text = "Leo Tháp";
            this.btnLeoThap.UseVisualStyleBackColor = true;
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
            // txtSoSao
            // 
            this.txtSoSao.AutoSize = true;
            this.txtSoSao.Location = new System.Drawing.Point(6, 44);
            this.txtSoSao.Name = "txtSoSao";
            this.txtSoSao.Size = new System.Drawing.Size(40, 13);
            this.txtSoSao.TabIndex = 2;
            this.txtSoSao.Text = "Số sao";
            // 
            // txtLeoThapSoThap
            // 
            this.txtLeoThapSoThap.Location = new System.Drawing.Point(81, 17);
            this.txtLeoThapSoThap.Name = "txtLeoThapSoThap";
            this.txtLeoThapSoThap.Size = new System.Drawing.Size(100, 20);
            this.txtLeoThapSoThap.TabIndex = 2;
            this.txtLeoThapSoThap.Text = "81";
            // 
            // txtLeoThapSoSao
            // 
            this.txtLeoThapSoSao.Location = new System.Drawing.Point(81, 40);
            this.txtLeoThapSoSao.Name = "txtLeoThapSoSao";
            this.txtLeoThapSoSao.Size = new System.Drawing.Size(100, 20);
            this.txtLeoThapSoSao.TabIndex = 2;
            this.txtLeoThapSoSao.Text = "301";
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
            // btnSaThaiTuong
            // 
            this.btnSaThaiTuong.Location = new System.Drawing.Point(233, 19);
            this.btnSaThaiTuong.Name = "btnSaThaiTuong";
            this.btnSaThaiTuong.Size = new System.Drawing.Size(111, 40);
            this.btnSaThaiTuong.TabIndex = 1;
            this.btnSaThaiTuong.Text = "Sa Thải Tướng";
            this.btnSaThaiTuong.UseVisualStyleBackColor = true;
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
            this.grbThuThanh.ResumeLayout(false);
            this.grbThuThanh.PerformLayout();
            this.grbLeoThap.ResumeLayout(false);
            this.grbLeoThap.PerformLayout();
            this.grbDuoiTuong.ResumeLayout(false);
            this.grbDuoiTuong.PerformLayout();
            this.grbGhepManhTuong.ResumeLayout(false);
            this.grbGhepManhTuong.PerformLayout();
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
    }
}

