using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using TableConstants;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using System.Collections.Specialized;
using TinhLuongThoiVu.App_Model;
using TinhLuongThoiVu.App_Common;

namespace TinhLuongThoiVu.App_Present
{
    public partial class MainForm : Form
    {
        #region - DECLARE -
        double dLuongCanBan = 100000 / 8;
        #endregion

        #region - CONTRUCTOR -
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        #region - EVENT -
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CreateEventSuggestComboBox(txtTenNhanVien);
            reloadComboBox();

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            NhanVien selectedNhanVien = null;
            if (txtTenNhanVien.SelectedItem is NhanVien)
            {
                selectedNhanVien = txtTenNhanVien.SelectedItem as NhanVien;
            }
            else
            {
                selectedNhanVien = new NhanVien { Ten = txtTenNhanVien.Text };
                Global.DBContext.NhanVien.Add(selectedNhanVien);
            }
            ThoiGianLamViec thoigian = new ThoiGianLamViec { NhanVien = selectedNhanVien };
            thoigian.Ngay = txtNgay.Value.ToString("dd/MM/yyyy");
            if (chkCaSang.Checked)
            {
                thoigian.GioBatDauCaSang = long.Parse(txtBatDauCaSang.Text.Substring(0, 2));
                thoigian.PhutBatDauCaSang = long.Parse(txtBatDauCaSang.Text.Substring(3, 2));

                thoigian.GioKetThucCaSang = long.Parse(txtKetThucCaSang.Text.Substring(0, 2));
                thoigian.PhutKetThucCaSang = long.Parse(txtKetThucCaSang.Text.Substring(3, 2));
                if (thoigian.GioKetThucCaSang >= 21 && thoigian.PhutKetThucCaSang > 30)
                {
                    thoigian.GioBatDauTC2 = 21;
                    thoigian.PhutBatDauTC2 = 30;

                    thoigian.GioKetThucTC2 = thoigian.GioKetThucCaSang;
                    thoigian.PhutKetThucTC2 = thoigian.PhutKetThucCaSang;

                    thoigian.GioKetThucCaSang = 21;
                    thoigian.PhutKetThucCaSang = 30;
                }

                if (thoigian.GioKetThucCaSang >= 17 && thoigian.PhutKetThucCaSang > 30)
                {
                    thoigian.GioBatDauTC1 = 17;
                    thoigian.PhutBatDauTC1 = 30;

                    thoigian.GioKetThucTC1 = thoigian.GioKetThucCaSang;
                    thoigian.PhutKetThucTC1 = thoigian.PhutKetThucCaSang;

                    thoigian.GioKetThucCaSang = 17;
                    thoigian.PhutKetThucCaSang = 30;
                }

                if (thoigian.GioKetThucCaSang >= 13 && thoigian.PhutKetThucCaSang > 0)
                {
                    thoigian.GioBatDauCaChieu = 13;
                    thoigian.PhutBatDauCaChieu = 0;

                    thoigian.GioKetThucCaChieu = thoigian.GioKetThucCaSang;
                    thoigian.PhutKetThucCaChieu = thoigian.PhutKetThucCaSang;

                    thoigian.GioKetThucCaSang = 11;
                    thoigian.PhutKetThucCaSang = 30;
                }
            }
            if (chkCaChieu.Checked)
            {
                thoigian.GioBatDauCaChieu = long.Parse(txtBatDauCaChieu.Text.Substring(0, 2));
                thoigian.PhutBatDauCaChieu = long.Parse(txtBatDauCaChieu.Text.Substring(3, 2));

                thoigian.GioKetThucCaChieu = long.Parse(txtKetThucCaChieu.Text.Substring(0, 2));
                thoigian.PhutKetThucCaChieu = long.Parse(txtKetThucCaChieu.Text.Substring(3, 2));
                if (thoigian.GioKetThucCaChieu >= 21 && thoigian.PhutKetThucCaChieu > 30)
                {
                    thoigian.GioBatDauTC2 = 21;
                    thoigian.PhutBatDauTC2 = 30;

                    thoigian.GioKetThucTC2 = thoigian.GioKetThucCaChieu;
                    thoigian.PhutKetThucTC2 = thoigian.PhutKetThucCaChieu;

                    thoigian.GioKetThucCaChieu = 21;
                    thoigian.PhutKetThucCaChieu = 30;
                }

                if (thoigian.GioKetThucCaChieu >= 17 && thoigian.PhutKetThucCaChieu > 30)
                {
                    thoigian.GioBatDauTC1 = 17;
                    thoigian.PhutBatDauTC1 = 30;

                    thoigian.GioKetThucTC1 = thoigian.GioKetThucCaChieu;
                    thoigian.PhutKetThucTC1 = thoigian.PhutKetThucCaChieu;

                    thoigian.GioKetThucCaChieu = 17;
                    thoigian.PhutKetThucCaChieu = 30;
                }
            }
            if (chkTangCa1.Checked)
            {
                thoigian.GioBatDauTC1 = long.Parse(txtBatDauTC1.Text.Substring(0, 2));
                thoigian.PhutBatDauTC1 = long.Parse(txtBatDauTC1.Text.Substring(3, 2));

                thoigian.GioKetThucTC1 = long.Parse(txtKetThucTC1.Text.Substring(0, 2));
                thoigian.PhutKetThucTC1 = long.Parse(txtKetThucTC1.Text.Substring(3, 2));

                if (thoigian.GioKetThucTC1 >= 21 && thoigian.PhutKetThucTC1 > 30)
                {
                    thoigian.GioBatDauTC2 = 21;
                    thoigian.PhutBatDauTC2 = 30;

                    thoigian.GioKetThucTC2 = thoigian.GioKetThucTC1;
                    thoigian.PhutKetThucTC2 = thoigian.PhutKetThucTC1;

                    thoigian.GioKetThucTC1 = 21;
                    thoigian.PhutKetThucTC1 = 30;
                }
            }
            Global.DBContext.ThoiGianLamViec.Add(thoigian);
            Global.DBContext.SaveChanges();
            reloadComboBox();
            txtTenNhanVien.SelectedItem = selectedNhanVien;
            reloadGrid();
            txtTenNhanVien.Focus();
            txtTenNhanVien.SelectAll();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

            if (gridData.CurrentCell != null && gridData[GridConst.ID, gridData.CurrentCell.RowIndex].Value is ThoiGianLamViec)
            {
                ThoiGianLamViec thoigian = gridData[GridConst.ID, gridData.CurrentCell.RowIndex].Value as ThoiGianLamViec;
                Global.DBContext.ThoiGianLamViec.Remove(thoigian);
                Global.DBContext.SaveChanges();
                reloadGrid();
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            NhanVien selectedNhanVien = null;
            if (txtTenNhanVien.SelectedItem is NhanVien)
            {
                selectedNhanVien = txtTenNhanVien.SelectedItem as NhanVien;
            }
            else
            {
                MessageBox.Show("Không thể cập nhật cho nhân viên mới.");
                return;
            }
            ThoiGianLamViec thoigian = null;
            if (gridData.CurrentCell != null && gridData[GridConst.ID, gridData.CurrentCell.RowIndex].Value is ThoiGianLamViec)
            {
                thoigian = gridData[GridConst.ID, gridData.CurrentCell.RowIndex].Value as ThoiGianLamViec;
            }
            thoigian.Ngay = txtNgay.Value.ToString("dd/MM/yyyy");
            if (chkCaSang.Checked)
            {
                thoigian.GioBatDauCaSang = long.Parse(txtBatDauCaSang.Text.Substring(0, 2));
                thoigian.PhutBatDauCaSang = long.Parse(txtBatDauCaSang.Text.Substring(3, 2));

                thoigian.GioKetThucCaSang = long.Parse(txtKetThucCaSang.Text.Substring(0, 2));
                thoigian.PhutKetThucCaSang = long.Parse(txtKetThucCaSang.Text.Substring(3, 2));
                if (thoigian.GioKetThucCaSang >= 21 && thoigian.PhutKetThucCaSang > 30)
                {
                    thoigian.GioBatDauTC2 = 21;
                    thoigian.PhutBatDauTC2 = 30;

                    thoigian.GioKetThucTC2 = thoigian.GioKetThucCaSang;
                    thoigian.PhutKetThucTC2 = thoigian.PhutKetThucCaSang;

                    thoigian.GioKetThucCaSang = 21;
                    thoigian.PhutKetThucCaSang = 30;
                }

                if (thoigian.GioKetThucCaSang >= 17 && thoigian.PhutKetThucCaSang > 30)
                {
                    thoigian.GioBatDauTC1 = 17;
                    thoigian.PhutBatDauTC1 = 30;

                    thoigian.GioKetThucTC1 = thoigian.GioKetThucCaSang;
                    thoigian.PhutKetThucTC1 = thoigian.PhutKetThucCaSang;

                    thoigian.GioKetThucCaSang = 17;
                    thoigian.PhutKetThucCaSang = 30;
                }

                if (thoigian.GioKetThucCaSang >= 13 && thoigian.PhutKetThucCaSang > 0)
                {
                    thoigian.GioBatDauCaChieu = 13;
                    thoigian.PhutBatDauCaChieu = 0;

                    thoigian.GioKetThucCaChieu = thoigian.GioKetThucCaSang;
                    thoigian.PhutKetThucCaChieu = thoigian.PhutKetThucCaSang;

                    thoigian.GioKetThucCaSang = 11;
                    thoigian.PhutKetThucCaSang = 30;
                }
            }
            else
            {
                thoigian.GioBatDauCaSang = null;
                thoigian.PhutBatDauCaSang = null;

                thoigian.GioKetThucCaSang = null;
                thoigian.PhutKetThucCaSang = null;
            }
            if (chkCaChieu.Checked)
            {
                thoigian.GioBatDauCaChieu = long.Parse(txtBatDauCaChieu.Text.Substring(0, 2));
                thoigian.PhutBatDauCaChieu = long.Parse(txtBatDauCaChieu.Text.Substring(3, 2));

                thoigian.GioKetThucCaChieu = long.Parse(txtKetThucCaChieu.Text.Substring(0, 2));
                thoigian.PhutKetThucCaChieu = long.Parse(txtKetThucCaChieu.Text.Substring(3, 2));
                if (thoigian.GioKetThucCaChieu >= 21 && thoigian.PhutKetThucCaChieu > 30)
                {
                    thoigian.GioBatDauTC2 = 21;
                    thoigian.PhutBatDauTC2 = 30;

                    thoigian.GioKetThucTC2 = thoigian.GioKetThucCaChieu;
                    thoigian.PhutKetThucTC2 = thoigian.PhutKetThucCaChieu;

                    thoigian.GioKetThucCaChieu = 21;
                    thoigian.PhutKetThucCaChieu = 30;
                }

                if (thoigian.GioKetThucCaChieu >= 17 && thoigian.PhutKetThucCaChieu > 30)
                {
                    thoigian.GioBatDauTC1 = 17;
                    thoigian.PhutBatDauTC1 = 30;

                    thoigian.GioKetThucTC1 = thoigian.GioKetThucCaChieu;
                    thoigian.PhutKetThucTC1 = thoigian.PhutKetThucCaChieu;

                    thoigian.GioKetThucCaChieu = 17;
                    thoigian.PhutKetThucCaChieu = 30;
                }
            }
            else
            {
                thoigian.GioBatDauCaChieu = null;
                thoigian.PhutBatDauCaChieu = null;

                thoigian.GioKetThucCaChieu = null;
                thoigian.PhutKetThucCaChieu = null;
            }
            if (chkTangCa1.Checked)
            {
                thoigian.GioBatDauTC1 = long.Parse(txtBatDauTC1.Text.Substring(0, 2));
                thoigian.PhutBatDauTC1 = long.Parse(txtBatDauTC1.Text.Substring(3, 2));

                thoigian.GioKetThucTC1 = long.Parse(txtKetThucTC1.Text.Substring(0, 2));
                thoigian.PhutKetThucTC1 = long.Parse(txtKetThucTC1.Text.Substring(3, 2));

                if (thoigian.GioKetThucTC1 >= 21 && thoigian.PhutKetThucTC1 > 30)
                {
                    thoigian.GioBatDauTC2 = 21;
                    thoigian.PhutBatDauTC2 = 30;

                    thoigian.GioKetThucTC2 = thoigian.GioKetThucTC1;
                    thoigian.PhutKetThucTC2 = thoigian.PhutKetThucTC1;

                    thoigian.GioKetThucTC1 = 21;
                    thoigian.PhutKetThucTC1 = 30;
                }
            }
            else
            {
                thoigian.GioBatDauTC1 = null;
                thoigian.PhutBatDauTC1 = null;

                thoigian.GioKetThucTC1 = null;
                thoigian.PhutKetThucTC1 = null;
            }
            Global.DBContext.SaveChanges();
            reloadComboBox();
            txtTenNhanVien.SelectedItem = selectedNhanVien;
            reloadGrid();
        }

        private void chkCaSang_CheckedChanged(object sender, EventArgs e)
        {
            grbCaSang.Enabled = chkCaSang.Checked;
        }

        private void chkCaChieu_CheckedChanged(object sender, EventArgs e)
        {
            grbCaChieu.Enabled = chkCaChieu.Checked;
        }

        private void chkTangCa1_CheckedChanged(object sender, EventArgs e)
        {
            grbTangCa1.Enabled = chkTangCa1.Checked;
        }

        private void txtTenNhanVien_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtTenNhanVien.Text.Trim()))
                {
                    txtTenNhanVien.DroppedDown = false;
                    List<NhanVien> listNhanVien = Global.DBContext.NhanVien.Where(x => x.Ten.ToLower().Contains(txtTenNhanVien.Text.Trim().ToLower())).ToList();
                    if (listNhanVien.Count > 0)
                    {
                        BindingSource bindingNhanVien = new BindingSource { DataSource = listNhanVien };
                        txtTenNhanVien.DataSource = bindingNhanVien;
                        txtTenNhanVien.DisplayMember = "Ten";
                        txtTenNhanVien.ValueMember = "ID";
                        if (listNhanVien.Count == 1)
                        {
                            txtTenNhanVien.SelectedItem = listNhanVien[0];
                        }
                        else
                        {
                            txtTenNhanVien.DroppedDown = true;
                        }
                    }
                    else
                    {
                        txtTenNhanVien.DataSource = new BindingSource { DataSource = new List<NhanVien>() };
                        reloadGrid();
                    }
                }
            }
        }
        #endregion

        #region - METHOD -
        private void reloadGrid()
        {
            gridData.Rows.Clear();
            if (txtTenNhanVien.SelectedItem is NhanVien)
            {
                NhanVien selectedNhanVien = txtTenNhanVien.SelectedItem as NhanVien;
                List<ThoiGianLamViec> listThoigian = selectedNhanVien.ThoiGianLamViecs.ToList();
                double tongluong = 0;
                foreach (ThoiGianLamViec thoigian in listThoigian)
                {
                    gridData.Rows.Add();
                    int iCount = gridData.Rows.Count - 1;
                    gridData[GridConst.Ngay, iCount].Value = thoigian.Ngay;
                    gridData[GridConst.ID, iCount].Value = thoigian;
                    double giochinhthuc = 0;
                    double giotangca = 0;
                    if (thoigian.GioBatDauCaSang.HasValue && thoigian.GioKetThucCaSang.HasValue)
                    {
                        TimeSpan sang = new TimeSpan((int)thoigian.GioKetThucCaSang.Value, (int)thoigian.PhutKetThucCaSang.Value, 0).Subtract(new TimeSpan((int)thoigian.GioBatDauCaSang.Value, (int)thoigian.PhutBatDauCaSang.Value, 0));
                        giochinhthuc += sang.TotalHours;
                        gridData[GridConst.CaSang, iCount].Value = string.Format("{0}:{1} - {2}:{3} = {4} giờ"
                            , thoigian.GioBatDauCaSang.Value.ToString("00")
                            , thoigian.PhutBatDauCaSang.Value.ToString("00")
                            , thoigian.GioKetThucCaSang.Value.ToString("00")
                            , thoigian.PhutKetThucCaSang.Value.ToString("00")
                            , sang.TotalHours.ToString("0.#"));
                    }
                    if (thoigian.GioBatDauCaChieu.HasValue && thoigian.GioKetThucCaChieu.HasValue)
                    {
                        TimeSpan chieu = new TimeSpan((int)thoigian.GioKetThucCaChieu.Value, (int)thoigian.PhutKetThucCaChieu.Value, 0).Subtract(new TimeSpan((int)thoigian.GioBatDauCaChieu.Value, (int)thoigian.PhutBatDauCaChieu.Value, 0));
                        giochinhthuc += chieu.TotalHours;
                        gridData[GridConst.CaChieu, iCount].Value = string.Format("{0}:{1} - {2}:{3} = {4} giờ"
                            , thoigian.GioBatDauCaChieu.Value.ToString("00")
                            , thoigian.PhutBatDauCaChieu.Value.ToString("00")
                            , thoigian.GioKetThucCaChieu.Value.ToString("00")
                            , thoigian.PhutKetThucCaChieu.Value.ToString("00")
                            , chieu.TotalHours.ToString("0.#"));
                    }
                    if (thoigian.GioBatDauTC1.HasValue && thoigian.GioKetThucTC1.HasValue)
                    {
                        TimeSpan tc = new TimeSpan();
                        if (thoigian.GioKetThucTC1.Value > thoigian.GioBatDauTC1.Value)
                        {
                            tc = new TimeSpan((int)thoigian.GioKetThucTC1.Value, (int)thoigian.PhutKetThucTC1.Value, 0).Subtract(new TimeSpan((int)thoigian.GioBatDauTC1.Value, (int)thoigian.PhutBatDauTC1.Value, 0));

                        }
                        else
                        {
                            tc = new TimeSpan(1, (int)thoigian.GioKetThucTC1.Value, (int)thoigian.PhutKetThucTC1.Value, 0).Subtract(new TimeSpan((int)thoigian.GioBatDauTC1.Value, (int)thoigian.PhutBatDauTC1.Value, 0));
                        }
                        giotangca = tc.TotalHours;
                        gridData[GridConst.TangCa1, iCount].Value = string.Format("{0}:{1} - {2}:{3} = {4} giờ"
                            , thoigian.GioBatDauTC1.Value.ToString("00")
                            , thoigian.PhutBatDauTC1.Value.ToString("00")
                            , thoigian.GioKetThucTC1.Value.ToString("00")
                            , thoigian.PhutKetThucTC1.Value.ToString("00")
                            , tc.TotalHours.ToString("0.#"));
                    }
                    double luongngay = 0;
                    if (giotangca > 4 && giochinhthuc >= 8)
                    {
                        luongngay = (giochinhthuc + giotangca) * dLuongCanBan * 1.2;
                    }
                    else if (giotangca > 3 && giochinhthuc >= 8)
                    {
                        luongngay = (giochinhthuc + giotangca) * dLuongCanBan * 1.1;
                    }
                    else
                    {
                        luongngay = (giochinhthuc + giotangca) * dLuongCanBan;
                    }
                    if (giochinhthuc >= 8)
                    {
                        luongngay += 20000;
                    }
                    tongluong += luongngay;
                    gridData[GridConst.LuongNgay, iCount].Value = luongngay.ToString("#,###");
                }
                lblTongLuong.Text = "Tổng Lương : " + tongluong.ToString("#,###");
            }
            else
            {
                lblTongLuong.Text = "Tổng Lương ";
            }
        }

        private void reloadComboBox()
        {
            List<NhanVien> listNhanVien = Global.DBContext.NhanVien.ToList();
            BindingSource bindingNhanVien = new BindingSource { DataSource = listNhanVien };
            txtTenNhanVien.DataSource = bindingNhanVien;
            txtTenNhanVien.DisplayMember = "Ten";
            txtTenNhanVien.ValueMember = "ID";
            List<string> listSource = new List<string>();
            listNhanVien.ForEach(x => listSource.Add(x.Ten));
            //CreateSuggestComboBox(listSource, txtTenNhanVien);
        }
        #endregion

        private void txtTenNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            //reloadGrid();
        }

        private void gridData_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (gridData[GridConst.ID, e.RowIndex].Value is ThoiGianLamViec)
                {
                    var thoigian = gridData[GridConst.ID, e.RowIndex].Value as ThoiGianLamViec;
                    try
                    {
                        txtNgay.Value = DateTime.ParseExact(thoigian.Ngay, "dd/MM/yyyy", new DateTimeFormatInfo { FullDateTimePattern = "dd/MM/yyyy" });
                    }
                    catch { }
                    if (thoigian.GioBatDauCaSang.HasValue && thoigian.GioKetThucCaSang.HasValue)
                    {
                        txtBatDauCaSang.Text = thoigian.GioBatDauCaSang.Value.ToString("00") + thoigian.PhutBatDauCaSang.Value.ToString("00");
                        txtKetThucCaSang.Text = thoigian.GioKetThucCaSang.Value.ToString("00") + thoigian.PhutKetThucCaSang.Value.ToString("00");
                        chkCaSang.Checked = true;
                    }
                    else
                    {
                        chkCaSang.Checked = false;
                    }
                    if (thoigian.GioBatDauCaChieu.HasValue && thoigian.GioKetThucCaChieu.HasValue)
                    {
                        txtBatDauCaChieu.Text = thoigian.GioBatDauCaChieu.Value.ToString("00") + thoigian.PhutBatDauCaChieu.Value.ToString("00");
                        txtKetThucCaChieu.Text = thoigian.GioKetThucCaChieu.Value.ToString("00") + thoigian.PhutKetThucCaChieu.Value.ToString("00");
                        chkCaChieu.Checked = true;
                    }
                    else
                    {
                        chkCaChieu.Checked = false;
                    }
                    if (thoigian.GioBatDauTC1.HasValue && thoigian.GioKetThucTC1.HasValue)
                    {
                        txtBatDauTC1.Text = thoigian.GioBatDauTC1.Value.ToString("00") + thoigian.PhutBatDauTC1.Value.ToString("00");
                        txtKetThucTC1.Text = thoigian.GioKetThucTC1.Value.ToString("00") + thoigian.PhutKetThucTC1.Value.ToString("00");
                        chkTangCa1.Checked = true;
                    }
                    else
                    {
                        chkTangCa1.Checked = false;
                    }
                }
            }
        }


        private static void Combo_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender is ComboBox)
            {
                if ((((ComboBox)sender).DroppedDown && ((ComboBox)sender).AutoCompleteMode != System.Windows.Forms.AutoCompleteMode.None))
                {
                    ((ComboBox)sender).AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
                }
                else if ((!((ComboBox)sender).DroppedDown && ((ComboBox)sender).AutoCompleteMode != System.Windows.Forms.AutoCompleteMode.Suggest))
                {
                    if (((ComboBox)sender).AutoCompleteCustomSource.Count > 0)
                    {
                        ((ComboBox)sender).AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                        ((ComboBox)sender).Select(((ComboBox)sender).Text.Trim().Length, 0);
                    }

                }
            }
        }

        private static void Combo_DropDown(object sender, EventArgs e)
        {
            if (sender is ComboBox && ((ComboBox)sender).AutoCompleteMode != System.Windows.Forms.AutoCompleteMode.None)
            {
                ((ComboBox)sender).AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            }
        }

        private static void Combo_SetAutoWidth(object sender, EventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox senderComboBox = (ComboBox)sender;
                senderComboBox.DropDownWidth = 1;
                int width = senderComboBox.DropDownWidth;
                Graphics g = senderComboBox.CreateGraphics();
                Font font = senderComboBox.Font;
                int vertScrollBarWidth =
                    (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                    ? SystemInformation.VerticalScrollBarWidth : 0;

                int newWidth;
                foreach (object s in senderComboBox.Items)
                {
                    if (s is DataRowView && (s as DataRowView).Row != null && (s as DataRowView).Row.Table.Columns.Contains(senderComboBox.DisplayMember))
                    {
                        newWidth = (int)g.MeasureString((s as DataRowView).Row[senderComboBox.DisplayMember].ToString(), font).Width
                            + vertScrollBarWidth;
                        if (width < newWidth)
                        {
                            width = newWidth;
                        }
                    }
                }
                senderComboBox.DropDownWidth = width;
            }
        }

        public static void CreateSuggestComboBox(List<string> listSource, ComboBox control)
        {
            try
            {
                if (listSource != null && listSource.Count > 0)
                {
                    control.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                    control.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    control.AutoCompleteCustomSource.Clear();
                    string[] arrstr = new string[listSource.Count];
                    for (int iIndex = 0; iIndex < listSource.Count; iIndex++)
                    {
                        arrstr[iIndex] = listSource[iIndex].Trim();
                    }
                    control.AutoCompleteCustomSource.AddRange(arrstr);
                }
                else
                {
                    control.AutoCompleteCustomSource.Clear();
                }
            }
            catch { }
        }

        public static void CreateEventSuggestComboBox(ComboBox control)
        {
            control.DropDown += new EventHandler(Combo_DropDown);
            control.KeyUp += new KeyEventHandler(Combo_KeyUp);
        }

        public static void SetAutoWidthComboBox(ComboBox control)
        {
            control.DropDown += new EventHandler(Combo_SetAutoWidth);
        }

        private void txtBatDauCaSang_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                txtBatDauCaSang_Leave(sender, null);
                btnThem_Click(null, null);
            }
        }

        private void txtTenNhanVien_SelectedValueChanged(object sender, EventArgs e)
        {
            reloadGrid();
        }

        private void txtBatDauCaSang_Leave(object sender, EventArgs e)
        {
            if (sender is MaskedTextBox)
            {
                var txt = sender as MaskedTextBox;
                if (txt.Text.Length != 5)
                {
                    int iHours = 0;
                    int iMinutes = 0;
                    int.TryParse(txt.Text.Split(':')[0].Trim(), out iHours);
                    int.TryParse(txt.Text.Split(':')[1].Trim(), out iMinutes);
                    txt.Text = iHours.ToString("00") + ":" + iMinutes.ToString("00");
                }
            }
        }
        //ngay du 8h thi 100 + 20k tien com
        //ngay du 8h + >4h tang ca tong gio X1.2 + 20k com
        //ngay du 8h + > 3h tang ca tong gio x1.1 +20k com


    }
}
