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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupData = new System.Windows.Forms.GroupBox();
            this.gridData = new System.Windows.Forms.DataGridView();
            this.lblPack = new System.Windows.Forms.Label();
            this.txtPackNo = new System.Windows.Forms.ComboBox();
            this.btnThemPack = new System.Windows.Forms.Button();
            this.btnXoaTaiKhoan = new System.Windows.Forms.Button();
            this.btnKetBan = new System.Windows.Forms.Button();
            this.btnThemTaiKhoan = new System.Windows.Forms.Button();
            this.groupMain = new System.Windows.Forms.GroupBox();
            this.chkNhanChip = new System.Windows.Forms.CheckBox();
            this.chkNhanCo4La = new System.Windows.Forms.CheckBox();
            this.btnNhanChipMayMan = new System.Windows.Forms.Button();
            this.btnTangCo4La = new System.Windows.Forms.Button();
            this.btnNhanThuongHangNgay = new System.Windows.Forms.Button();
            this.btnTangQuaBiMat = new System.Windows.Forms.Button();
            this.groupData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
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
            this.groupData.Controls.Add(this.btnXoaTaiKhoan);
            this.groupData.Controls.Add(this.btnKetBan);
            this.groupData.Controls.Add(this.btnThemTaiKhoan);
            this.groupData.Location = new System.Drawing.Point(12, 126);
            this.groupData.Name = "groupData";
            this.groupData.Size = new System.Drawing.Size(664, 393);
            this.groupData.TabIndex = 3;
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
            this.gridData.Location = new System.Drawing.Point(6, 56);
            this.gridData.Name = "gridData";
            this.gridData.ReadOnly = true;
            this.gridData.Size = new System.Drawing.Size(652, 331);
            this.gridData.TabIndex = 0;
            // 
            // lblPack
            // 
            this.lblPack.AutoSize = true;
            this.lblPack.Location = new System.Drawing.Point(6, 32);
            this.lblPack.Name = "lblPack";
            this.lblPack.Size = new System.Drawing.Size(39, 13);
            this.lblPack.TabIndex = 0;
            this.lblPack.Text = "P#(F4)";
            // 
            // txtPackNo
            // 
            this.txtPackNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtPackNo.FormattingEnabled = true;
            this.txtPackNo.Location = new System.Drawing.Point(45, 28);
            this.txtPackNo.Name = "txtPackNo";
            this.txtPackNo.Size = new System.Drawing.Size(63, 21);
            this.txtPackNo.TabIndex = 1;
            this.txtPackNo.SelectedValueChanged += new System.EventHandler(this.txtPackNo_SelectedValueChanged);
            // 
            // btnThemPack
            // 
            this.btnThemPack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThemPack.Location = new System.Drawing.Point(111, 27);
            this.btnThemPack.Name = "btnThemPack";
            this.btnThemPack.Size = new System.Drawing.Size(30, 23);
            this.btnThemPack.TabIndex = 2;
            this.btnThemPack.Text = "+";
            this.btnThemPack.UseVisualStyleBackColor = true;
            this.btnThemPack.Click += new System.EventHandler(this.btnThemPack_Click);
            // 
            // btnXoaTaiKhoan
            // 
            this.btnXoaTaiKhoan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXoaTaiKhoan.Location = new System.Drawing.Point(598, 23);
            this.btnXoaTaiKhoan.Name = "btnXoaTaiKhoan";
            this.btnXoaTaiKhoan.Size = new System.Drawing.Size(60, 30);
            this.btnXoaTaiKhoan.TabIndex = 4;
            this.btnXoaTaiKhoan.Text = "Xóa TK";
            this.btnXoaTaiKhoan.UseVisualStyleBackColor = true;
            this.btnXoaTaiKhoan.Click += new System.EventHandler(this.btnXoaTaiKhoan_Click);
            // 
            // btnKetBan
            // 
            this.btnKetBan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKetBan.Location = new System.Drawing.Point(466, 23);
            this.btnKetBan.Name = "btnKetBan";
            this.btnKetBan.Size = new System.Drawing.Size(60, 30);
            this.btnKetBan.TabIndex = 3;
            this.btnKetBan.Text = "Kết bạn";
            this.btnKetBan.UseVisualStyleBackColor = true;
            this.btnKetBan.Click += new System.EventHandler(this.btnKetBan_Click);
            // 
            // btnThemTaiKhoan
            // 
            this.btnThemTaiKhoan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThemTaiKhoan.Location = new System.Drawing.Point(532, 23);
            this.btnThemTaiKhoan.Name = "btnThemTaiKhoan";
            this.btnThemTaiKhoan.Size = new System.Drawing.Size(60, 30);
            this.btnThemTaiKhoan.TabIndex = 3;
            this.btnThemTaiKhoan.Text = "Thêm TK";
            this.btnThemTaiKhoan.UseVisualStyleBackColor = true;
            this.btnThemTaiKhoan.Click += new System.EventHandler(this.btnThemTaiKhoan_Click);
            // 
            // groupMain
            // 
            this.groupMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupMain.Controls.Add(this.chkNhanChip);
            this.groupMain.Controls.Add(this.chkNhanCo4La);
            this.groupMain.Controls.Add(this.btnNhanChipMayMan);
            this.groupMain.Controls.Add(this.btnTangCo4La);
            this.groupMain.Controls.Add(this.btnTangQuaBiMat);
            this.groupMain.Controls.Add(this.btnNhanThuongHangNgay);
            this.groupMain.Location = new System.Drawing.Point(12, 12);
            this.groupMain.Name = "groupMain";
            this.groupMain.Size = new System.Drawing.Size(664, 108);
            this.groupMain.TabIndex = 4;
            this.groupMain.TabStop = false;
            this.groupMain.Text = "Bảng Điều Khiển";
            // 
            // chkNhanChip
            // 
            this.chkNhanChip.AutoSize = true;
            this.chkNhanChip.Location = new System.Drawing.Point(403, 81);
            this.chkNhanChip.Name = "chkNhanChip";
            this.chkNhanChip.Size = new System.Drawing.Size(15, 14);
            this.chkNhanChip.TabIndex = 3;
            this.chkNhanChip.UseVisualStyleBackColor = true;
            // 
            // chkNhanCo4La
            // 
            this.chkNhanCo4La.AutoSize = true;
            this.chkNhanCo4La.Location = new System.Drawing.Point(519, 81);
            this.chkNhanCo4La.Name = "chkNhanCo4La";
            this.chkNhanCo4La.Size = new System.Drawing.Size(15, 14);
            this.chkNhanCo4La.TabIndex = 5;
            this.chkNhanCo4La.UseVisualStyleBackColor = true;
            // 
            // btnNhanChipMayMan
            // 
            this.btnNhanChipMayMan.Location = new System.Drawing.Point(417, 72);
            this.btnNhanChipMayMan.Name = "btnNhanChipMayMan";
            this.btnNhanChipMayMan.Size = new System.Drawing.Size(84, 30);
            this.btnNhanChipMayMan.TabIndex = 4;
            this.btnNhanChipMayMan.Text = "NCMM(F2)";
            this.btnNhanChipMayMan.UseVisualStyleBackColor = true;
            // 
            // btnTangCo4La
            // 
            this.btnTangCo4La.Location = new System.Drawing.Point(533, 72);
            this.btnTangCo4La.Name = "btnTangCo4La";
            this.btnTangCo4La.Size = new System.Drawing.Size(84, 30);
            this.btnTangCo4La.TabIndex = 6;
            this.btnTangCo4La.Text = "NC4L(F3)";
            this.btnTangCo4La.UseVisualStyleBackColor = true;
            // 
            // btnNhanThuongHangNgay
            // 
            this.btnNhanThuongHangNgay.Location = new System.Drawing.Point(6, 19);
            this.btnNhanThuongHangNgay.Name = "btnNhanThuongHangNgay";
            this.btnNhanThuongHangNgay.Size = new System.Drawing.Size(260, 30);
            this.btnNhanThuongHangNgay.TabIndex = 3;
            this.btnNhanThuongHangNgay.Text = "NTHN";
            this.btnNhanThuongHangNgay.UseVisualStyleBackColor = true;
            this.btnNhanThuongHangNgay.Click += new System.EventHandler(this.btnNhanThuongHangNgay_Click);
            // 
            // btnTangQuaBiMat
            // 
            this.btnTangQuaBiMat.Location = new System.Drawing.Point(6, 55);
            this.btnTangQuaBiMat.Name = "btnTangQuaBiMat";
            this.btnTangQuaBiMat.Size = new System.Drawing.Size(260, 30);
            this.btnTangQuaBiMat.TabIndex = 3;
            this.btnTangQuaBiMat.Text = "TQBM";
            this.btnTangQuaBiMat.UseVisualStyleBackColor = true;
            this.btnTangQuaBiMat.Click += new System.EventHandler(this.btnTangQuaBiMat_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 531);
            this.Controls.Add(this.groupMain);
            this.Controls.Add(this.groupData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PTV Nuker - Tinnv@VNIT Solutions";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupData.ResumeLayout(false);
            this.groupData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            this.groupMain.ResumeLayout(false);
            this.groupMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupData;
        private System.Windows.Forms.DataGridView gridData;
        private System.Windows.Forms.GroupBox groupMain;
        private System.Windows.Forms.ComboBox txtPackNo;
        private System.Windows.Forms.Button btnNhanChipMayMan;
        private System.Windows.Forms.Button btnThemPack;
        private System.Windows.Forms.Button btnXoaTaiKhoan;
        private System.Windows.Forms.Button btnThemTaiKhoan;
        private System.Windows.Forms.Button btnTangCo4La;
        private System.Windows.Forms.Button btnNhanThuongHangNgay;
        private System.Windows.Forms.Label lblPack;
        private System.Windows.Forms.CheckBox chkNhanChip;
        private System.Windows.Forms.CheckBox chkNhanCo4La;
        private System.Windows.Forms.Button btnKetBan;
        private System.Windows.Forms.Button btnTangQuaBiMat;

    }
}

