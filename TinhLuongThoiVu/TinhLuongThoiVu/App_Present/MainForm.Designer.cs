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
            this.txtGioHanhChinh = new System.Windows.Forms.TextBox();
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
            this.txtGioTangCa = new System.Windows.Forms.TextBox();
            this.txtKetThucTC1 = new System.Windows.Forms.MaskedTextBox();
            this.txtBatDauTC1 = new System.Windows.Forms.MaskedTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chkTangCa1 = new System.Windows.Forms.CheckBox();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.Ngay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CaSang = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CaChieu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TangCa1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LuongNgay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnCapNhat = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.lblTongLuong = new System.Windows.Forms.Label();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.txtThang = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtTamUng = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtPhuCap = new System.Windows.Forms.TextBox();
            this.txtSTT = new System.Windows.Forms.TextBox();
            this.lblThucLanh = new System.Windows.Forms.Label();
            this.btnCapNhatSTT = new System.Windows.Forms.Button();
            this.grbCaSang.SuspendLayout();
            this.grbCaChieu.SuspendLayout();
            this.grbTangCa1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên Nhân Viên";
            // 
            // txtTenNhanVien
            // 
            this.txtTenNhanVien.FormattingEnabled = true;
            this.txtTenNhanVien.Location = new System.Drawing.Point(111, 14);
            this.txtTenNhanVien.Margin = new System.Windows.Forms.Padding(4);
            this.txtTenNhanVien.Name = "txtTenNhanVien";
            this.txtTenNhanVien.Size = new System.Drawing.Size(131, 23);
            this.txtTenNhanVien.TabIndex = 1;
            this.txtTenNhanVien.SelectedValueChanged += new System.EventHandler(this.txtTenNhanVien_SelectedValueChanged);
            this.txtTenNhanVien.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtTenNhanVien_KeyUp);
            // 
            // grbCaSang
            // 
            this.grbCaSang.Controls.Add(this.txtGioHanhChinh);
            this.grbCaSang.Controls.Add(this.txtKetThucCaSang);
            this.grbCaSang.Controls.Add(this.txtBatDauCaSang);
            this.grbCaSang.Controls.Add(this.label3);
            this.grbCaSang.Controls.Add(this.label2);
            this.grbCaSang.Location = new System.Drawing.Point(11, 89);
            this.grbCaSang.Margin = new System.Windows.Forms.Padding(4);
            this.grbCaSang.Name = "grbCaSang";
            this.grbCaSang.Padding = new System.Windows.Forms.Padding(4);
            this.grbCaSang.Size = new System.Drawing.Size(284, 73);
            this.grbCaSang.TabIndex = 8;
            this.grbCaSang.TabStop = false;
            // 
            // txtGioHanhChinh
            // 
            this.txtGioHanhChinh.Location = new System.Drawing.Point(9, 37);
            this.txtGioHanhChinh.Name = "txtGioHanhChinh";
            this.txtGioHanhChinh.Size = new System.Drawing.Size(41, 21);
            this.txtGioHanhChinh.TabIndex = 15;
            this.txtGioHanhChinh.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtGioHanhChinh_KeyUp);
            // 
            // txtKetThucCaSang
            // 
            this.txtKetThucCaSang.Location = new System.Drawing.Point(195, 37);
            this.txtKetThucCaSang.Margin = new System.Windows.Forms.Padding(4);
            this.txtKetThucCaSang.Mask = "00:00";
            this.txtKetThucCaSang.Name = "txtKetThucCaSang";
            this.txtKetThucCaSang.Size = new System.Drawing.Size(81, 21);
            this.txtKetThucCaSang.TabIndex = 1;
            this.txtKetThucCaSang.Text = "1130";
            this.txtKetThucCaSang.ValidatingType = typeof(System.DateTime);
            this.txtKetThucCaSang.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBatDauCaSang_KeyUp);
            this.txtKetThucCaSang.Leave += new System.EventHandler(this.txtBatDauCaSang_Leave);
            // 
            // txtBatDauCaSang
            // 
            this.txtBatDauCaSang.Location = new System.Drawing.Point(80, 37);
            this.txtBatDauCaSang.Margin = new System.Windows.Forms.Padding(4);
            this.txtBatDauCaSang.Mask = "00:00";
            this.txtBatDauCaSang.Name = "txtBatDauCaSang";
            this.txtBatDauCaSang.Size = new System.Drawing.Size(81, 21);
            this.txtBatDauCaSang.TabIndex = 0;
            this.txtBatDauCaSang.Text = "0800";
            this.txtBatDauCaSang.ValidatingType = typeof(System.DateTime);
            this.txtBatDauCaSang.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBatDauCaSang_KeyUp);
            this.txtBatDauCaSang.Leave += new System.EventHandler(this.txtBatDauCaSang_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(195, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "Giờ Kết Thúc";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(80, 19);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Giờ Bắt Đầu";
            // 
            // chkCaSang
            // 
            this.chkCaSang.AutoSize = true;
            this.chkCaSang.Checked = true;
            this.chkCaSang.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCaSang.Location = new System.Drawing.Point(20, 86);
            this.chkCaSang.Margin = new System.Windows.Forms.Padding(4);
            this.chkCaSang.Name = "chkCaSang";
            this.chkCaSang.Size = new System.Drawing.Size(73, 19);
            this.chkCaSang.TabIndex = 4;
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
            this.grbCaChieu.Enabled = false;
            this.grbCaChieu.Location = new System.Drawing.Point(11, 184);
            this.grbCaChieu.Margin = new System.Windows.Forms.Padding(4);
            this.grbCaChieu.Name = "grbCaChieu";
            this.grbCaChieu.Padding = new System.Windows.Forms.Padding(4);
            this.grbCaChieu.Size = new System.Drawing.Size(284, 73);
            this.grbCaChieu.TabIndex = 9;
            this.grbCaChieu.TabStop = false;
            // 
            // txtKetThucCaChieu
            // 
            this.txtKetThucCaChieu.Location = new System.Drawing.Point(195, 37);
            this.txtKetThucCaChieu.Margin = new System.Windows.Forms.Padding(4);
            this.txtKetThucCaChieu.Mask = "00:00";
            this.txtKetThucCaChieu.Name = "txtKetThucCaChieu";
            this.txtKetThucCaChieu.Size = new System.Drawing.Size(81, 21);
            this.txtKetThucCaChieu.TabIndex = 1;
            this.txtKetThucCaChieu.Text = "1730";
            this.txtKetThucCaChieu.ValidatingType = typeof(System.DateTime);
            this.txtKetThucCaChieu.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBatDauCaSang_KeyUp);
            this.txtKetThucCaChieu.Leave += new System.EventHandler(this.txtBatDauCaSang_Leave);
            // 
            // txtBatDauCaChieu
            // 
            this.txtBatDauCaChieu.Location = new System.Drawing.Point(80, 37);
            this.txtBatDauCaChieu.Margin = new System.Windows.Forms.Padding(4);
            this.txtBatDauCaChieu.Mask = "00:00";
            this.txtBatDauCaChieu.Name = "txtBatDauCaChieu";
            this.txtBatDauCaChieu.Size = new System.Drawing.Size(81, 21);
            this.txtBatDauCaChieu.TabIndex = 0;
            this.txtBatDauCaChieu.Text = "1300";
            this.txtBatDauCaChieu.ValidatingType = typeof(System.DateTime);
            this.txtBatDauCaChieu.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBatDauCaSang_KeyUp);
            this.txtBatDauCaChieu.Leave += new System.EventHandler(this.txtBatDauCaSang_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(195, 19);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Giờ Kết Thúc";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(80, 19);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Giờ Bắt Đầu";
            // 
            // chkCaChieu
            // 
            this.chkCaChieu.AutoSize = true;
            this.chkCaChieu.Location = new System.Drawing.Point(20, 181);
            this.chkCaChieu.Margin = new System.Windows.Forms.Padding(4);
            this.chkCaChieu.Name = "chkCaChieu";
            this.chkCaChieu.Size = new System.Drawing.Size(76, 19);
            this.chkCaChieu.TabIndex = 5;
            this.chkCaChieu.Text = "Ca Chiều";
            this.chkCaChieu.UseVisualStyleBackColor = true;
            this.chkCaChieu.CheckedChanged += new System.EventHandler(this.chkCaChieu_CheckedChanged);
            // 
            // txtNgay
            // 
            this.txtNgay.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtNgay.Location = new System.Drawing.Point(111, 49);
            this.txtNgay.Margin = new System.Windows.Forms.Padding(4);
            this.txtNgay.Name = "txtNgay";
            this.txtNgay.Size = new System.Drawing.Size(187, 21);
            this.txtNgay.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 53);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "Ngày";
            // 
            // grbTangCa1
            // 
            this.grbTangCa1.Controls.Add(this.txtGioTangCa);
            this.grbTangCa1.Controls.Add(this.txtKetThucTC1);
            this.grbTangCa1.Controls.Add(this.txtBatDauTC1);
            this.grbTangCa1.Controls.Add(this.label7);
            this.grbTangCa1.Controls.Add(this.label8);
            this.grbTangCa1.Enabled = false;
            this.grbTangCa1.Location = new System.Drawing.Point(11, 278);
            this.grbTangCa1.Margin = new System.Windows.Forms.Padding(4);
            this.grbTangCa1.Name = "grbTangCa1";
            this.grbTangCa1.Padding = new System.Windows.Forms.Padding(4);
            this.grbTangCa1.Size = new System.Drawing.Size(284, 73);
            this.grbTangCa1.TabIndex = 10;
            this.grbTangCa1.TabStop = false;
            // 
            // txtGioTangCa
            // 
            this.txtGioTangCa.Location = new System.Drawing.Point(9, 37);
            this.txtGioTangCa.Name = "txtGioTangCa";
            this.txtGioTangCa.Size = new System.Drawing.Size(41, 21);
            this.txtGioTangCa.TabIndex = 15;
            this.txtGioTangCa.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtGioTangCa_KeyUp);
            // 
            // txtKetThucTC1
            // 
            this.txtKetThucTC1.Location = new System.Drawing.Point(195, 37);
            this.txtKetThucTC1.Margin = new System.Windows.Forms.Padding(4);
            this.txtKetThucTC1.Mask = "00:00";
            this.txtKetThucTC1.Name = "txtKetThucTC1";
            this.txtKetThucTC1.Size = new System.Drawing.Size(81, 21);
            this.txtKetThucTC1.TabIndex = 1;
            this.txtKetThucTC1.Text = "2200";
            this.txtKetThucTC1.ValidatingType = typeof(System.DateTime);
            this.txtKetThucTC1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBatDauCaSang_KeyUp);
            this.txtKetThucTC1.Leave += new System.EventHandler(this.txtBatDauCaSang_Leave);
            // 
            // txtBatDauTC1
            // 
            this.txtBatDauTC1.Location = new System.Drawing.Point(80, 37);
            this.txtBatDauTC1.Margin = new System.Windows.Forms.Padding(4);
            this.txtBatDauTC1.Mask = "00:00";
            this.txtBatDauTC1.Name = "txtBatDauTC1";
            this.txtBatDauTC1.Size = new System.Drawing.Size(81, 21);
            this.txtBatDauTC1.TabIndex = 0;
            this.txtBatDauTC1.Text = "1800";
            this.txtBatDauTC1.ValidatingType = typeof(System.DateTime);
            this.txtBatDauTC1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtBatDauCaSang_KeyUp);
            this.txtBatDauTC1.Leave += new System.EventHandler(this.txtBatDauCaSang_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(195, 19);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 15);
            this.label7.TabIndex = 1;
            this.label7.Text = "Giờ Kết Thúc";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(80, 19);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 15);
            this.label8.TabIndex = 1;
            this.label8.Text = "Giờ Bắt Đầu";
            // 
            // chkTangCa1
            // 
            this.chkTangCa1.AutoSize = true;
            this.chkTangCa1.Location = new System.Drawing.Point(20, 277);
            this.chkTangCa1.Margin = new System.Windows.Forms.Padding(4);
            this.chkTangCa1.Name = "chkTangCa1";
            this.chkTangCa1.Size = new System.Drawing.Size(72, 19);
            this.chkTangCa1.TabIndex = 6;
            this.chkTangCa1.Text = "Tăng Ca";
            this.chkTangCa1.UseVisualStyleBackColor = true;
            this.chkTangCa1.CheckedChanged += new System.EventHandler(this.chkTangCa1_CheckedChanged);
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
            this.ID,
            this.LuongNgay});
            this.gridData.Location = new System.Drawing.Point(325, 107);
            this.gridData.Margin = new System.Windows.Forms.Padding(4);
            this.gridData.Name = "gridData";
            this.gridData.ReadOnly = true;
            this.gridData.RowHeadersVisible = false;
            this.gridData.Size = new System.Drawing.Size(780, 244);
            this.gridData.TabIndex = 14;
            this.gridData.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridData_CellMouseDoubleClick);
            // 
            // Ngay
            // 
            this.Ngay.HeaderText = "Ngày";
            this.Ngay.Name = "Ngay";
            this.Ngay.ReadOnly = true;
            // 
            // CaSang
            // 
            this.CaSang.HeaderText = "Ca Sáng";
            this.CaSang.Name = "CaSang";
            this.CaSang.ReadOnly = true;
            this.CaSang.Width = 150;
            // 
            // CaChieu
            // 
            this.CaChieu.HeaderText = "Ca Chiều";
            this.CaChieu.Name = "CaChieu";
            this.CaChieu.ReadOnly = true;
            this.CaChieu.Width = 150;
            // 
            // TangCa1
            // 
            this.TangCa1.HeaderText = "Tăng Ca";
            this.TangCa1.Name = "TangCa1";
            this.TangCa1.ReadOnly = true;
            this.TangCa1.Width = 150;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // LuongNgay
            // 
            this.LuongNgay.HeaderText = "Lương Ngày";
            this.LuongNgay.Name = "LuongNgay";
            this.LuongNgay.ReadOnly = true;
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(325, 59);
            this.btnThem.Margin = new System.Windows.Forms.Padding(4);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(120, 40);
            this.btnThem.TabIndex = 12;
            this.btnThem.Text = "THÊM";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnCapNhat
            // 
            this.btnCapNhat.Location = new System.Drawing.Point(453, 59);
            this.btnCapNhat.Margin = new System.Windows.Forms.Padding(4);
            this.btnCapNhat.Name = "btnCapNhat";
            this.btnCapNhat.Size = new System.Drawing.Size(120, 40);
            this.btnCapNhat.TabIndex = 13;
            this.btnCapNhat.Text = "CẬP NHẬT";
            this.btnCapNhat.UseVisualStyleBackColor = true;
            this.btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.Location = new System.Drawing.Point(581, 59);
            this.btnXoa.Margin = new System.Windows.Forms.Padding(4);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(120, 40);
            this.btnXoa.TabIndex = 13;
            this.btnXoa.Text = "XÓA";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // lblTongLuong
            // 
            this.lblTongLuong.AutoSize = true;
            this.lblTongLuong.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblTongLuong.Location = new System.Drawing.Point(723, 86);
            this.lblTongLuong.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTongLuong.Name = "lblTongLuong";
            this.lblTongLuong.Size = new System.Drawing.Size(90, 16);
            this.lblTongLuong.TabIndex = 0;
            this.lblTongLuong.Text = "Tổng Lương";
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.Location = new System.Drawing.Point(985, 359);
            this.btnXuatExcel.Margin = new System.Windows.Forms.Padding(4);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(120, 40);
            this.btnXuatExcel.TabIndex = 12;
            this.btnXuatExcel.Text = "XUẤT EXCEL";
            this.btnXuatExcel.UseVisualStyleBackColor = true;
            this.btnXuatExcel.Click += new System.EventHandler(this.btnXuatExcel_Click);
            // 
            // txtThang
            // 
            this.txtThang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtThang.FormattingEnabled = true;
            this.txtThang.Location = new System.Drawing.Point(372, 359);
            this.txtThang.Margin = new System.Windows.Forms.Padding(4);
            this.txtThang.Name = "txtThang";
            this.txtThang.Size = new System.Drawing.Size(187, 23);
            this.txtThang.TabIndex = 20;
            this.txtThang.SelectedValueChanged += new System.EventHandler(this.txtThang_SelectedValueChanged);
            this.txtThang.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtTenNhanVien_KeyUp);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(322, 362);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 15);
            this.label9.TabIndex = 0;
            this.label9.Text = "Tháng";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(642, 17);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 15);
            this.label10.TabIndex = 0;
            this.label10.Text = "Tạm Ứng";
            // 
            // txtTamUng
            // 
            this.txtTamUng.Location = new System.Drawing.Point(707, 15);
            this.txtTamUng.Name = "txtTamUng";
            this.txtTamUng.Size = new System.Drawing.Size(141, 21);
            this.txtTamUng.TabIndex = 15;
            this.txtTamUng.Leave += new System.EventHandler(this.txtPhuCap_Leave);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(386, 17);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 15);
            this.label11.TabIndex = 0;
            this.label11.Text = "Phụ Cấp";
            // 
            // txtPhuCap
            // 
            this.txtPhuCap.Location = new System.Drawing.Point(451, 15);
            this.txtPhuCap.Name = "txtPhuCap";
            this.txtPhuCap.Size = new System.Drawing.Size(141, 21);
            this.txtPhuCap.TabIndex = 15;
            this.txtPhuCap.Leave += new System.EventHandler(this.txtPhuCap_Leave);
            // 
            // txtSTT
            // 
            this.txtSTT.Location = new System.Drawing.Point(249, 15);
            this.txtSTT.Name = "txtSTT";
            this.txtSTT.Size = new System.Drawing.Size(49, 21);
            this.txtSTT.TabIndex = 2;
            this.txtSTT.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSTT_KeyUp);
            // 
            // lblThucLanh
            // 
            this.lblThucLanh.AutoSize = true;
            this.lblThucLanh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblThucLanh.Location = new System.Drawing.Point(923, 86);
            this.lblThucLanh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThucLanh.Name = "lblThucLanh";
            this.lblThucLanh.Size = new System.Drawing.Size(79, 16);
            this.lblThucLanh.TabIndex = 0;
            this.lblThucLanh.Text = "Thực Lãnh";
            // 
            // btnCapNhatSTT
            // 
            this.btnCapNhatSTT.Location = new System.Drawing.Point(300, 13);
            this.btnCapNhatSTT.Margin = new System.Windows.Forms.Padding(4);
            this.btnCapNhatSTT.Name = "btnCapNhatSTT";
            this.btnCapNhatSTT.Size = new System.Drawing.Size(64, 25);
            this.btnCapNhatSTT.TabIndex = 2;
            this.btnCapNhatSTT.Text = "Lưu STT";
            this.btnCapNhatSTT.UseVisualStyleBackColor = true;
            this.btnCapNhatSTT.Click += new System.EventHandler(this.btnCapNhatSTT_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 408);
            this.Controls.Add(this.txtSTT);
            this.Controls.Add(this.txtPhuCap);
            this.Controls.Add(this.txtTamUng);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnCapNhat);
            this.Controls.Add(this.btnCapNhatSTT);
            this.Controls.Add(this.btnXuatExcel);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.gridData);
            this.Controls.Add(this.chkTangCa1);
            this.Controls.Add(this.txtNgay);
            this.Controls.Add(this.grbTangCa1);
            this.Controls.Add(this.chkCaChieu);
            this.Controls.Add(this.chkCaSang);
            this.Controls.Add(this.grbCaChieu);
            this.Controls.Add(this.grbCaSang);
            this.Controls.Add(this.txtThang);
            this.Controls.Add(this.txtTenNhanVien);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblThucLanh);
            this.Controls.Add(this.lblTongLuong);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
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
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnCapNhat;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Label lblTongLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ngay;
        private System.Windows.Forms.DataGridViewTextBoxColumn CaSang;
        private System.Windows.Forms.DataGridViewTextBoxColumn CaChieu;
        private System.Windows.Forms.DataGridViewTextBoxColumn TangCa1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn LuongNgay;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.TextBox txtGioHanhChinh;
        private System.Windows.Forms.TextBox txtGioTangCa;
        private System.Windows.Forms.ComboBox txtThang;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtTamUng;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtPhuCap;
        private System.Windows.Forms.TextBox txtSTT;
        private System.Windows.Forms.Label lblThucLanh;
        private System.Windows.Forms.Button btnCapNhatSTT;


    }
}

