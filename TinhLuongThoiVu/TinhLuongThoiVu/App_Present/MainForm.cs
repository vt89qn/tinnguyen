using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using off = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using TableConstants;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using System.Collections.Specialized;
using TinhLuongThoiVu.App_Model;
using TinhLuongThoiVu.App_Common;
using System.IO;

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
            //var x = new { Ten = "Trần Văn An" };
            //string t = x.Ten.Trim().ToLower().Substring(x.Ten.ToLower().EndsWith(" a") || x.Ten.ToLower().EndsWith(" b") ? x.Ten.Trim().Substring(0, x.Ten.Trim().Length - 2).LastIndexOf(' ') : x.Ten.Trim().LastIndexOf(' ')).Replace(" ", "");
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

                if (thoigian.GioKetThucCaSang > 17
                    || (thoigian.GioKetThucCaSang == 17 && thoigian.PhutKetThucCaSang > 30))
                {
                    thoigian.GioBatDauTC1 = 17;
                    thoigian.PhutBatDauTC1 = 30;

                    thoigian.GioKetThucTC1 = thoigian.GioKetThucCaSang;
                    thoigian.PhutKetThucTC1 = thoigian.PhutKetThucCaSang;

                    thoigian.GioKetThucCaSang = 17;
                    thoigian.PhutKetThucCaSang = 30;
                }

                if (thoigian.GioKetThucCaSang > 13
                    || (thoigian.GioKetThucCaSang == 13 && thoigian.PhutKetThucCaSang > 0))
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

                if (thoigian.GioKetThucCaChieu > 17
                    || (thoigian.GioKetThucCaChieu == 17 && thoigian.PhutKetThucCaChieu > 30))
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

                if (thoigian.GioKetThucCaSang > 17
                    || (thoigian.GioKetThucCaSang == 17 && thoigian.PhutKetThucCaSang > 30))
                {
                    thoigian.GioBatDauTC1 = 17;
                    thoigian.PhutBatDauTC1 = 30;

                    thoigian.GioKetThucTC1 = thoigian.GioKetThucCaSang;
                    thoigian.PhutKetThucTC1 = thoigian.PhutKetThucCaSang;

                    thoigian.GioKetThucCaSang = 17;
                    thoigian.PhutKetThucCaSang = 30;
                }

                if (thoigian.GioKetThucCaSang > 13
                    || (thoigian.GioKetThucCaSang == 13 && thoigian.PhutKetThucCaSang > 0))
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

                if (thoigian.GioKetThucCaChieu > 17
                    || (thoigian.GioKetThucCaChieu == 17 && thoigian.PhutKetThucCaChieu > 30))
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
                List<ThoiGianLamViec> listThoigian = selectedNhanVien.ThoiGianLamViecs.OrderBy(x => x.Ngay).ToList();
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
                    if (giotangca >= 4 && giochinhthuc >= 8)
                    {
                        luongngay = (giochinhthuc + giotangca) * dLuongCanBan * 1.2;
                    }
                    else if (giotangca >= 3 && giochinhthuc >= 8)
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

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Vnit.Excel.ExcelEngine excel = new Vnit.Excel.ExcelEngine();
                string strFileName = string.Format("D:\\LuongThoiVu{0}.xls", DateTime.Now.Ticks);
                if (excel.CreateNewObject(strFileName))
                {
                    List<NhanVien> listNhanVien = Global.DBContext.NhanVien.ToList();
                    listNhanVien = listNhanVien.OrderBy(x => x.Ten.Trim().ToLower().Substring(x.Ten.ToLower().EndsWith(" a") || x.Ten.ToLower().EndsWith(" b") ? x.Ten.Trim().Substring(0, x.Ten.Trim().Length - 2).LastIndexOf(' ') : x.Ten.Trim().LastIndexOf(' ')).Replace(" ", "")).ToList();
                    DateTime dateMin = DateTime.MaxValue;
                    DateTime dateMax = DateTime.MinValue;
                    listNhanVien.ForEach(x => x.ThoiGianLamViecs.ToList().ForEach(a => { DateTime date = DateTime.ParseExact(a.Ngay, "dd/MM/yyyy", new DateTimeFormatInfo { FullDateTimePattern = "dd/MM/yyyy" }); if (date > dateMax) dateMax = date; if (date < dateMin) dateMin = date; }));
                    if (dateMax.Subtract(dateMin).TotalDays <= 40)
                    {
                        #region - Sheet Tong -
                        int iCol = 1, iRow = 1;
                        excel.Merge("A1", "A2");
                        excel.SetWidth(iCol, 25);
                        excel.SetValueWithFormat(iRow, iCol++, "Tên Nhân Viên", true, false, false);
                        for (DateTime date = dateMin; date <= dateMax; date = date.AddDays(1))
                        {
                            iCol = 2 + date.Subtract(dateMin).Days * 3;
                            excel.SetWidth(iCol, 4); excel.SetWidth(iCol + 1, 4); excel.SetWidth(iCol + 2, 4);
                            excel.SetValueWithFormat(iRow + 1, iCol, "S", true, false, false);
                            excel.SetValueWithFormat(iRow + 1, iCol + 1, "C", true, false, false);
                            excel.SetValueWithFormat(iRow + 1, iCol + 2, "OT", true, false, false);
                            excel.Merge(excel.GetColumnName(iCol) + 1, excel.GetColumnName(iCol + 2) + 1);
                            excel.SetValueWithFormat(iRow, iCol, "'" + date.ToString("dd/MM/yyyy"), true, false, false);
                        }
                        iCol += 3;
                        excel.Merge(excel.GetColumnName(iCol) + 1, excel.GetColumnName(iCol) + 2);
                        excel.SetWidth(iCol, 10);
                        excel.SetValueWithFormat(iRow, iCol++, "Ngày Công", true, false, false);

                        excel.Merge(excel.GetColumnName(iCol) + 1, excel.GetColumnName(iCol) + 2);
                        excel.SetWidth(iCol, 10);
                        excel.SetValueWithFormat(iRow, iCol++, "Giờ Tăng Ca", true, false, false);

                        excel.Merge(excel.GetColumnName(iCol) + 1, excel.GetColumnName(iCol) + 2);
                        excel.SetWidth(iCol, 10);
                        excel.SetValueWithFormat(iRow, iCol++, "Phụ Cấp", true, false, false);

                        excel.Merge(excel.GetColumnName(iCol) + 1, excel.GetColumnName(iCol) + 2);
                        excel.SetWidth(iCol, 10);
                        excel.SetValueWithFormat(iRow, iCol++, "Tổng Lương", true, false, false);

                        excel.Merge(excel.GetColumnName(iCol) + 1, excel.GetColumnName(iCol) + 2);
                        excel.SetWidth(iCol, 10);
                        excel.SetValueWithFormat(iRow, iCol++, "Tạm Ứng", true, false, false);

                        excel.Merge(excel.GetColumnName(iCol) + 1, excel.GetColumnName(iCol) + 2);
                        excel.SetWidth(iCol, 10);
                        excel.SetValueWithFormat(iRow, iCol++, "Thực Lãnh", true, false, false);

                        iRow++;
                        Dictionary<NhanVien, List<double>> dicThongTinTungNhanVienAll = new Dictionary<NhanVien, List<double>>();
                        foreach (NhanVien nv in listNhanVien)
                        {
                            iRow++;
                            excel.SetValueWithFormat(iRow, 1, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nv.Ten), true, false, false);
                            Dictionary<DateTime, List<double>> dicTongTien = new Dictionary<DateTime, List<double>>();
                            foreach (ThoiGianLamViec thoigian in nv.ThoiGianLamViecs)
                            {
                                DateTime date = DateTime.ParseExact(thoigian.Ngay, "dd/MM/yyyy", new DateTimeFormatInfo { FullDateTimePattern = "dd/MM/yyyy" });
                                iCol = 2 + date.Subtract(dateMin).Days * 3;
                                if (!dicTongTien.ContainsKey(date))
                                {
                                    dicTongTien.Add(date, new List<double> { 0, 0, 0 });
                                }
                                if (thoigian.GioBatDauCaSang.HasValue && thoigian.GioKetThucCaSang.HasValue)
                                {
                                    TimeSpan sang = new TimeSpan((int)thoigian.GioKetThucCaSang.Value, (int)thoigian.PhutKetThucCaSang.Value, 0).Subtract(new TimeSpan((int)thoigian.GioBatDauCaSang.Value, (int)thoigian.PhutBatDauCaSang.Value, 0));
                                    excel.SetValue(iRow, iCol, sang.TotalHours);
                                    dicTongTien[date][0] = sang.TotalHours;
                                }
                                if (thoigian.GioBatDauCaChieu.HasValue && thoigian.GioKetThucCaChieu.HasValue)
                                {
                                    TimeSpan chieu = new TimeSpan((int)thoigian.GioKetThucCaChieu.Value, (int)thoigian.PhutKetThucCaChieu.Value, 0).Subtract(new TimeSpan((int)thoigian.GioBatDauCaChieu.Value, (int)thoigian.PhutBatDauCaChieu.Value, 0));
                                    excel.SetValue(iRow, iCol + 1, chieu.TotalHours);
                                    dicTongTien[date][1] = chieu.TotalHours;
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
                                    excel.SetValue(iRow, iCol + 2, tc.TotalHours);
                                    dicTongTien[date][2] = tc.TotalHours;
                                }
                            }
                            double tongtien = 0;
                            double tonggiolam = 0;
                            double tonggiotangca = 0;
                            double tongphucap = 0;
                            double tamung = 0;
                            foreach (KeyValuePair<DateTime, List<double>> item in dicTongTien)
                            {
                                double luongngay = 0;
                                double giochinhthuc = item.Value[0] + item.Value[1];
                                double giotangca = item.Value[2];
                                if (giotangca >= 4 && giochinhthuc >= 8)
                                {
                                    luongngay = (giochinhthuc + giotangca) * dLuongCanBan * 1.2;
                                }
                                else if (giotangca >= 3 && giochinhthuc >= 8)
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
                                tongtien += luongngay;
                                tonggiolam += giochinhthuc;
                                tonggiotangca += giotangca;
                            }

                            iCol = 5 + dateMax.Subtract(dateMin).Days * 3;
                            excel.SetValue(iRow, iCol++, (tonggiolam / 8));
                            excel.SetValue(iRow, iCol++, tonggiotangca);
                            excel.SetValue(iRow, iCol++, tongphucap);
                            excel.SetValue(iRow, iCol++, tongtien + tongphucap);
                            excel.SetValue(iRow, iCol++, tamung);
                            excel.SetValue(iRow, iCol, "=" + excel.GetColumnName(iCol - 2) + iRow + "-" + excel.GetColumnName(iCol - 2) + iRow);

                            dicThongTinTungNhanVienAll.Add(nv, new List<double> { tonggiolam / 8, tonggiotangca, tongphucap, tongtien + tongphucap, tamung });
                        }
                        for (DateTime date = dateMin; date <= dateMax; date = date.AddDays(1))
                        {
                            iCol = 2 + date.Subtract(dateMin).Days * 3;
                            excel.SetBackgroundColor(excel.GetColumnName(iCol) + 2, excel.GetColumnName(iCol) + iRow, Color.FromArgb(1, 240, 240, 240));
                        }
                        iCol += 3;
                        iRow++;
                        excel.SetValueWithFormat(iRow, iCol, "=SUM(" + excel.GetColumnName(iCol) + "2:" + excel.GetColumnName(iCol++) + (iRow - 1), true, false, false);
                        excel.SetValueWithFormat(iRow, iCol, "=SUM(" + excel.GetColumnName(iCol) + "2:" + excel.GetColumnName(iCol++) + (iRow - 1), true, false, false);
                        excel.SetValueWithFormat(iRow, iCol, "=SUM(" + excel.GetColumnName(iCol) + "2:" + excel.GetColumnName(iCol++) + (iRow - 1), true, false, false);
                        excel.SetValueWithFormat(iRow, iCol, "=SUM(" + excel.GetColumnName(iCol) + "2:" + excel.GetColumnName(iCol++) + (iRow - 1), true, false, false);
                        excel.SetValueWithFormat(iRow, iCol, "=SUM(" + excel.GetColumnName(iCol) + "2:" + excel.GetColumnName(iCol++) + (iRow - 1), true, false, false);
                        excel.SetValueWithFormat(iRow, iCol, "=SUM(" + excel.GetColumnName(iCol) + "2:" + excel.GetColumnName(iCol++) + (iRow - 1), true, false, false);
                        iRow--;
                        excel.Border_Range("A1", excel.GetColumnName(10 + dateMax.Subtract(dateMin).Days * 3) + iRow, Color.Black);
                        excel.SetFontSize("A1", excel.GetColumnName(10 + dateMax.Subtract(dateMin).Days * 3) + iRow, 10);
                        excel.setAlignAndValign("A1", excel.GetColumnName(10 + dateMax.Subtract(dateMin).Days * 3) + iRow, off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                        excel.setAlignAndValign("A2", "A" + iRow, off.XlHAlign.xlHAlignLeft, off.XlVAlign.xlVAlignCenter);
                        excel.SetFormatCell("B3", excel.GetColumnName(10 + dateMax.Subtract(dateMin).Days * 3) + (iRow + 1), "#,##0.00");
                        excel.SetSheetName("Bảng Lương");
                        #endregion
                        #region - Phieu Luong -
                        excel.SetActiveSheet(2);
                        int iCount = 0;
                        int iCountHeight = 0;
                        iRow = 2;
                        foreach (KeyValuePair<NhanVien, List<double>> item in dicThongTinTungNhanVienAll)
                        {
                            iCount++;
                            if (iCount > 3)
                            {
                                iCount = 1;
                                iRow += 8;
                            }
                            iCol = iCount * 3 - 1;
                            excel.SetWidth(iCol, 12); excel.SetWidth(iCol + 1, 12);
                            excel.Merge(excel.GetColumnName(iCol) + iRow, excel.GetColumnName(iCol + 1) + iRow);
                            excel.SetValueWithFormat(iRow, iCol, System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(item.Key.Ten), true, false, false);
                            excel.SetValue(iRow + 1, iCol, "Ngày Công");
                            excel.SetValue(iRow + 1, iCol + 1, item.Value[0]);
                            excel.SetValue(iRow + 2, iCol, "Giờ Tăng Ca");
                            excel.SetValue(iRow + 2, iCol + 1, item.Value[1]);
                            excel.SetValue(iRow + 3, iCol, "Phụ Cấp");
                            excel.SetValue(iRow + 3, iCol + 1, item.Value[2]);
                            excel.SetValue(iRow + 4, iCol, "Tổng Lương");
                            excel.SetValue(iRow + 4, iCol + 1, item.Value[3]);
                            excel.SetValue(iRow + 5, iCol, "Tạm Ứng");
                            excel.SetValue(iRow + 5, iCol + 1, item.Value[4]);
                            excel.SetValue(iRow + 6, iCol, "Thực Lãnh");
                            excel.SetValue(iRow + 6, iCol + 1, "=" + excel.GetColumnName(iCol + 1) + (iRow + 4) + "-" + excel.GetColumnName(iCol + 1) + (iRow + 5));

                            excel.Border_Range(excel.GetColumnName(iCol) + iRow, excel.GetColumnName(iCol + 1) + (iRow + 6), Color.Black);
                            excel.SetFontSize(excel.GetColumnName(iCol) + iRow, excel.GetColumnName(iCol + 1) + (iRow + 6), 10);
                            excel.setAlignAndValign(excel.GetColumnName(iCol) + iRow, excel.GetColumnName(iCol + 1) + (iRow + 6), off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                            excel.SetFormatCell(excel.GetColumnName(iCol + 1) + (iRow + 1), excel.GetColumnName(iCol + 1) + (iRow + 6), "#,##0.00");
                            iCountHeight++;
                            if (iCountHeight == 18)
                            {
                                iCountHeight = 0;
                                excel.SetPageBreak(iRow + 7);
                                iRow++;
                            }
                        }
                        excel.SetWidth(1, 1);
                        excel.SetWidth(4, 2);
                        excel.SetWidth(7, 2);
                        excel.SetWidth(10, 2);
                        excel.SetWidth(10, 1);
                        excel.SetSheetName("Phiếu Lương");

                        #endregion

                        excel.End_Write();
                        if (File.Exists(strFileName))
                        {
                            Process.Start(strFileName);
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Min : {0} - Max {1}", dateMin.ToString("dd/MM/yyyy"), dateMax.ToString("dd/MM/yyyy")));
                    }
                }
            }
            catch { }

        }

        //ngay du 8h thi 100 + 20k tien com
        //ngay du 8h + >4h tang ca tong gio X1.2 + 20k com
        //ngay du 8h + > 3h tang ca tong gio x1.1 +20k com
    }
}
