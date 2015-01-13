namespace TinhLuongThoiVu.App_Present
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txtTenNhanVien = new System.Windows.Forms.ComboBox();
            this.grbCaSang = new System.Windows.Forms.GroupBox();
            this.txtKetThucCaSang = new System.Windows.Forms.MaskedTextBox();
            this.txtBatDauCaSang = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkCaSang = new System.Windows.Forms.CheckBox();
            this.grbCaChieu = new System.Windows.Forms.GroupBox();
            this.txtKetThucCaChieu = new System.Windows.Forms.MaskedTextBox();
            this.txtBatDauCaChieu = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkCaChieu = new System.Windows.Forms.CheckBox();
            this.txtNgay = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.grbTangCa1 = new System.Windows.Forms.GroupBox();
            this.txtKetThucTC1 = new System.Windows.Forms.MaskedTextBox();
            this.txtBatDauTC1 = new System.Windows.Forms.MaskedTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chkTangCa1 = new System.Windows.Forms.CheckBox();
            this.grbTangCa2 = new System.Windows.Forms.GroupBox();
            this.txtKetThucTC2 = new System.Windows.Forms.MaskedTextBox();
            this.txtBatDauTC2 = new System.Windows.Forms.MaskedTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.chkTangCa2 = new System.Windows.Forms.CheckBox();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnCapNhat = new System.Windows.Forms.Button();
            this.Ngay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CaSang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CaChieu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TangCa1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TangCa2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grbCaSang.SuspendLayout();
            this.grbCaChieu.SuspendLayout();
            this.grbTangCa1.SuspendLayout();
            this.grbTangCa2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên Nhân Viên";
            // 
            // txtTenNhanVien
            // 
            this.txtTenNhanVien.FormattingEnabled = true;
            this.txtTenNhanVien.Location = new System.Drawing.Point(95, 12);
            this.txtTenNhanVien.Name = "txtTenNhanVien";
            this.txtTenNhanVien.Size = new System.Drawing.Size(161, 21);
            this.txtTenNhanVien.TabIndex = 1;
            this.txtTenNhanVien.SelectedIndexChanged += new System.EventHandler(this.txtTenNhanVien_SelectedIndexChanged);
            // 
            // grbCaSang
            // 
            this.grbCaSang.Controls.Add(this.txtKetThucCaSang);
            this.grbCaSang.Controls.Add(this.txtBatDauCaSang);
            this.grbCaSang.Controls.Add(this.label3);
            this.grbCaSang.Controls.Add(this.label2);
            this.grbCaSang.Location = new System.Drawing.Point(10, 77);
            this.grbCaSang.Name = "grbCaSang";
            this.grbCaSang.Size = new System.Drawing.Size(243, 63);
            this.grbCaSang.TabIndex = 2;
            this.grbCaSang.TabStop = false;
            // 
            // txtKetThucCaSang
            // 
            this.txtKetThucCaSang.Location = new System.Drawing.Point(167, 32);
            this.txtKetThucCaSang.Mask = "00:00";
            this.txtKetThucCaSang.Name = "txtKetThucCaSang";
            this.txtKetThucCaSang.Size = new System.Drawing.Size(70, 20);
            this.txtKetThucCaSang.TabIndex = 3;
            this.txtKetThucCaSang.Text = "1130";
            this.txtKetThucCaSang.ValidatingType = typeof(System.DateTime);
            // 
            // txtBatDauCaSang
            // 
            this.txtBatDauCaSang.Location = new System.Drawing.Point(69, 32);
            this.txtBatDauCaSang.Mask = "00:00";
            this.txtBatDauCaSang.Name = "txtBatDauCaSang";
            this.txtBatDauCaSang.Size = new System.Drawing.Size(70, 20);
            this.txtBatDauCaSang.TabIndex = 3;
            this.txtBatDauCaSang.Text = "0730";
            this.txtBatDauCaSang.ValidatingType = typeof(System.DateTime);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(167, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Giờ Kết Thúc";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(69, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Giờ Bắt Đầu";
            // 
            // chkCaSang
            // 
            this.chkCaSang.AutoSize = true;
            this.chkCaSang.Checked = true;
            this.chkCaSang.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCaSang.Location = new System.Drawing.Point(17, 75);
            this.chkCaSang.Name = "chkCaSang";
            this.chkCaSang.Size = new System.Drawing.Size(67, 17);
            this.chkCaSang.TabIndex = 0;
            this.chkCaSang.Text = "Ca Sáng";
            this.chkCaSang.UseVisualStyleBackColor = true;
            this.chkCaSang.CheckedChanged += new System.EventHandler(this.chkCaSang_CheckedChanged);
            // 
            // grbCaChieu
            // 
            this.grbCaChieu.Controls.Add(this.txtKetThucCaChieu);
            this.grbCaChieu.Controls.Add(this.txtBatDauCaChieu);
            this.grbCaChieu.Controls.Add(this.label4);
            this.grbCaChieu.Controls.Add(this.label5);
            this.grbCaChieu.Location = new System.Drawing.Point(10, 159);
            this.grbCaChieu.Name = "grbCaChieu";
            this.grbCaChieu.Size = new System.Drawing.Size(243, 63);
            this.grbCaChieu.TabIndex = 2;
            this.grbCaChieu.TabStop = false;
            // 
            // txtKetThucCaChieu
            // 
            this.txtKetThucCaChieu.Location = new System.Drawing.Point(167, 32);
            this.txtKetThucCaChieu.Mask = "00:00";
            this.txtKetThucCaChieu.Name = "txtKetThucCaChieu";
            this.txtKetThucCaChieu.Size = new System.Drawing.Size(70, 20);
            this.txtKetThucCaChieu.TabIndex = 3;
            this.txtKetThucCaChieu.Text = "1130";
            this.txtKetThucCaChieu.ValidatingType = typeof(System.DateTime);
            // 
            // txtBatDauCaChieu
            // 
            this.txtBatDauCaChieu.Location = new System.Drawing.Point(69, 32);
            this.txtBatDauCaChieu.Mask = "00:00";
            this.txtBatDauCaChieu.Name = "txtBatDauCaChieu";
            this.txtBatDauCaChieu.Size = new System.Drawing.Size(70, 20);
            this.txtBatDauCaChieu.TabIndex = 3;
            this.txtBatDauCaChieu.Text = "1300";
            this.txtBatDauCaChieu.ValidatingType = typeof(System.DateTime);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(167, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Giờ Kết Thúc";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(69, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Giờ Bắt Đầu";
            // 
            // chkCaChieu
            // 
            this.chkCaChieu.AutoSize = true;
            this.chkCaChieu.Location = new System.Drawing.Point(17, 157);
            this.chkCaChieu.Name = "chkCaChieu";
            this.chkCaChieu.Size = new System.Drawing.Size(69, 17);
            this.chkCaChieu.TabIndex = 0;
            this.chkCaChieu.Text = "Ca Chiều";
            this.chkCaChieu.UseVisualStyleBackColor = true;
            this.chkCaChieu.CheckedChanged += new System.EventHandler(this.chkCaChieu_CheckedChanged);
            // 
            // txtNgay
            // 
            this.txtNgay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtNgay.Location = new System.Drawing.Point(95, 42);
            this.txtNgay.Name = "txtNgay";
            this.txtNgay.Size = new System.Drawing.Size(161, 20);
            this.txtNgay.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Ngày";
            // 
            // grbTangCa1
            // 
            this.grbTangCa1.Controls.Add(this.txtKetThucTC1);
            this.grbTangCa1.Controls.Add(this.txtBatDauTC1);
            this.grbTangCa1.Controls.Add(this.label7);
            this.grbTangCa1.Controls.Add(this.label8);
            this.grbTangCa1.Location = new System.Drawing.Point(10, 241);
            this.grbTangCa1.Name = "grbTangCa1";
            this.grbTangCa1.Size = new System.Drawing.Size(243, 63);
            this.grbTangCa1.TabIndex = 2;
            this.grbTangCa1.TabStop = false;
            // 
            // txtKetThucTC1
            // 
            this.txtKetThucTC1.Location = new System.Drawing.Point(167, 32);
            this.txtKetThucTC1.Mask = "00:00";
            this.txtKetThucTC1.Name = "txtKetThucTC1";
            this.txtKetThucTC1.Size = new System.Drawing.Size(70, 20);
            this.txtKetThucTC1.TabIndex = 3;
            this.txtKetThucTC1.Text = "2130";
            this.txtKetThucTC1.ValidatingType = typeof(System.DateTime);
            // 
            // txtBatDauTC1
            // 
            this.txtBatDauTC1.Location = new System.Drawing.Point(69, 32);
            this.txtBatDauTC1.Mask = "00:00";
            this.txtBatDauTC1.Name = "txtBatDauTC1";
            this.txtBatDauTC1.Size = new System.Drawing.Size(70, 20);
            this.txtBatDauTC1.TabIndex = 3;
            this.txtBatDauTC1.Text = "1730";
            this.txtBatDauTC1.ValidatingType = typeof(System.DateTime);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(167, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Giờ Kết Thúc";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(69, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Giờ Bắt Đầu";
            // 
            // chkTangCa1
            // 
            this.chkTangCa1.AutoSize = true;
            this.chkTangCa1.Location = new System.Drawing.Point(17, 240);
            this.chkTangCa1.Name = "chkTangCa1";
            this.chkTangCa1.Size = new System.Drawing.Size(76, 17);
            this.chkTangCa1.TabIndex = 0;
            this.chkTangCa1.Text = "Tăng Ca 1";
            this.chkTangCa1.UseVisualStyleBackColor = true;
            this.chkTangCa1.CheckedChanged += new System.EventHandler(this.chkTangCa1_CheckedChanged);
            // 
            // grbTangCa2
            // 
            this.grbTangCa2.Controls.Add(this.txtKetThucTC2);
            this.grbTangCa2.Controls.Add(this.txtBatDauTC2);
            this.grbTangCa2.Controls.Add(this.label9);
            this.grbTangCa2.Controls.Add(this.label10);
            this.grbTangCa2.Location = new System.Drawing.Point(10, 323);
            this.grbTangCa2.Name = "grbTangCa2";
            this.grbTangCa2.Size = new System.Drawing.Size(243, 63);
            this.grbTangCa2.TabIndex = 2;
            this.grbTangCa2.TabStop = false;
            // 
            // txtKetThucTC2
            // 
            this.txtKetThucTC2.Location = new System.Drawing.Point(167, 32);
            this.txtKetThucTC2.Mask = "00:00";
            this.txtKetThucTC2.Name = "txtKetThucTC2";
            this.txtKetThucTC2.Size = new System.Drawing.Size(70, 20);
            this.txtKetThucTC2.TabIndex = 3;
            this.txtKetThucTC2.Text = "2330";
            this.txtKetThucTC2.ValidatingType = typeof(System.DateTime);
            // 
            // txtBatDauTC2
            // 
            this.txtBatDauTC2.Location = new System.Drawing.Point(69, 32);
            this.txtBatDauTC2.Mask = "00:00";
            this.txtBatDauTC2.Name = "txtBatDauTC2";
            this.txtBatDauTC2.Size = new System.Drawing.Size(70, 20);
            this.txtBatDauTC2.TabIndex = 3;
            this.txtBatDauTC2.Text = "2130";
            this.txtBatDauTC2.ValidatingType = typeof(System.DateTime);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(167, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Giờ Kết Thúc";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(69, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Giờ Bắt Đầu";
            // 
            // chkTangCa2
            // 
            this.chkTangCa2.AutoSize = true;
            this.chkTangCa2.Location = new System.Drawing.Point(17, 322);
            this.chkTangCa2.Name = "chkTangCa2";
            this.chkTangCa2.Size = new System.Drawing.Size(76, 17);
            this.chkTangCa2.TabIndex = 0;
            this.chkTangCa2.Text = "Tăng Ca 2";
            this.chkTangCa2.UseVisualStyleBackColor = true;
            this.chkTangCa2.CheckedChanged += new System.EventHandler(this.chkTangCa2_CheckedChanged);
            // 
            // gridData
            // 
            this.gridData.AllowUserToAddRows = false;
            this.gridData.AllowUserToDeleteRows = false;
            this.gridData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Ngay,
            this.CaSang,
            this.CaChieu,
            this.TangCa1,
            this.TangCa2,
            this.ID});
            this.gridData.Location = new System.Drawing.Point(278, 93);
            this.gridData.Name = "gridData";
            this.gridData.RowHeadersVisible = false;
            this.gridData.Size = new System.Drawing.Size(565, 293);
            this.gridData.TabIndex = 4;
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(278, 46);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(120, 40);
            this.btnThem.TabIndex = 5;
            this.btnThem.Text = "THÊM";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnCapNhat
            // 
            this.btnCapNhat.Location = new System.Drawing.Point(404, 47);
            this.btnCapNhat.Name = "btnCapNhat";
            this.btnCapNhat.Size = new System.Drawing.Size(120, 40);
            this.btnCapNhat.TabIndex = 5;
            this.btnCapNhat.Text = "CẬP NHẬT";
            this.btnCapNhat.UseVisualStyleBackColor = true;
            this.btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click);
            // 
            // Ngay
            // 
            this.Ngay.HeaderText = "Ngày";
            this.Ngay.Name = "Ngay";
            // 
            // CaSang
            // 
            this.CaSang.HeaderText = "Ca Sáng";
            this.CaSang.Name = "CaSang";
            // 
            // CaChieu
            // 
            this.CaChieu.HeaderText = "Ca Chiều";
            this.CaChieu.Name = "CaChieu";
            // 
            // TangCa1
            // 
            this.TangCa1.HeaderText = "Tăng Ca 1";
            this.TangCa1.Name = "TangCa1";
            // 
            // TangCa2
            // 
            this.TangCa2.HeaderText = "Tăng Ca 2";
            this.TangCa2.Name = "TangCa2";
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 447);
            this.Controls.Add(this.btnCapNhat);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.gridData);
            this.Controls.Add(this.chkTangCa2);
            this.Controls.Add(this.chkTangCa1);
            this.Controls.Add(this.txtNgay);
            this.Controls.Add(this.grbTangCa2);
            this.Controls.Add(this.grbTangCa1);
            this.Controls.Add(this.chkCaChieu);
            this.Controls.Add(this.chkCaSang);
            this.Controls.Add(this.grbCaChieu);
            this.Controls.Add(this.grbCaSang);
            this.Controls.Add(this.txtTenNhanVien);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tính Lương Thời Vụ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grbCaSang.ResumeLayout(false);
            this.grbCaSang.PerformLayout();
            this.grbCaChieu.ResumeLayout(false);
            this.grbCaChieu.PerformLayout();
            this.grbTangCa1.ResumeLayout(false);
            this.grbTangCa1.PerformLayout();
            this.grbTangCa2.ResumeLayout(false);
            this.grbTangCa2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox txtTenNhanVien;
        private System.Windows.Forms.GroupBox grbCaSang;
        private System.Windows.Forms.CheckBox chkCaSang;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox txtBatDauCaSang;
        private System.Windows.Forms.MaskedTextBox txtKetThucCaSang;
        private System.Windows.Forms.GroupBox grbCaChieu;
        private System.Windows.Forms.MaskedTextBox txtKetThucCaChieu;
        private System.Windows.Forms.MaskedTextBox txtBatDauCaChieu;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkCaChieu;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker txtNgay;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox grbTangCa1;
        private System.Windows.Forms.MaskedTextBox txtKetThucTC1;
        private System.Windows.Forms.MaskedTextBox txtBatDauTC1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkTangCa1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox grbTangCa2;
        private System.Windows.Forms.MaskedTextBox txtKetThucTC2;
        private System.Windows.Forms.MaskedTextBox txtBatDauTC2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkTangCa2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnCapNhat;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ngay;
        private System.Windows.Forms.DataGridViewTextBoxColumn CaSang;
        private System.Windows.Forms.DataGridViewTextBoxColumn CaChieu;
        private System.Windows.Forms.DataGridViewTextBoxColumn TangCa1;
        private System.Windows.Forms.DataGridViewTextBoxColumn TangCa2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;


    }
}

