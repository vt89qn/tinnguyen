using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Globalization;
using off = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace InPhieuChi
{
    public partial class InPhieuChi : Form
    {
        private const string TB_STT = "TB_STT";
        private const string TB_SOCT = "TB_SOCT";
        private const string TB_NGAYCT = "TB_NGAYCT";
        private const string TB_DIENGIAI = "TB_DIENGIAI";
        private const string TB_TKNO = "TB_TKNO";
        private const string TB_SOTIEN = "TB_SOTIEN";
        private const string TB_SOHD = "TB_SOHD";


        DataTable dtImport = new DataTable();
        public InPhieuChi()
        {
            InitializeComponent();
            this.Load += new EventHandler(InPhieuChi_Load);
        }

        void InPhieuChi_Load(object sender, EventArgs e)
        {
            //btnReadFile_Click(null, null);
        }

        private void btnBrowserFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFD = new OpenFileDialog();
            oFD.Multiselect = false;
            if (oFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFileName.Text = oFD.FileName;
            }
        }

        private void btnBrowserFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fBD = new FolderBrowserDialog();
            if (fBD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFolderOut.Text = fBD.SelectedPath;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Vnit.Excel.ExcelEngine excel = new Vnit.Excel.ExcelEngine();
                string strFileName = string.Format("D:\\output{0}.xls", DateTime.Now.Ticks);
                if (excel.CreateNewObject(strFileName))
                {
                    #region - Fill data -
                    int iRow = 1;
                    int iCount = 0;
                    int iCurrentMonth = 1;
                    for (int iIndex = 1; iIndex < 9; iIndex++)
                    {
                        excel.SetWidth(iIndex, 8);
                    }
                    while (dtImport.Rows.Count > 0)
                    {
                        DataRow[] rows = dtImport.Select(TB_SOCT + "='" + dtImport.Rows[0][TB_SOCT].ToString() + "'");
                        string strMonth = Regex.Match(rows[0][TB_SOCT].ToString().Trim(), @"PC(?<so1>\d\d)", RegexOptions.IgnoreCase).Groups["so1"].Value;

                        //int iMonth = 0;
                        //if (int.TryParse(strMonth, out iMonth) && iMonth != iCurrentMonth)
                        //{
                        //    excel.SetSheetName("T" + iCurrentMonth.ToString("00"));
                        //    iCurrentMonth++;
                        //    iRow = 1;
                        //    iCount = 0;
                        //    excel.AddNewSheet();
                        //}
                        for (int iIndexSetHeight = iRow; iIndexSetHeight <= iRow + 7; iIndexSetHeight++)
                        {
                            excel.SetRowHeight(iIndexSetHeight, 12);
                        }
                        for (int iIndexSetHeight = iRow + 8; iIndexSetHeight <= iRow + 18; iIndexSetHeight++)
                        {
                            excel.SetRowHeight(iIndexSetHeight, 15);
                        }
                        excel.SetRowHeight(iRow + 19, 60);
                        excel.SetRowHeight(iRow + 20, 13);
                        iCount++;

                        //Company name and address
                        excel.Merge("A" + iRow, "D" + (iRow + 1));
                        excel.setAlignAndValign(iRow, 1, off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                        excel.SetValueWithFormat(iRow, 1, "CTY CP - TM - DV Sài Gòn Ánh Dương\r\n126 Đinh Bộ Lĩnh - P.26 - Bình Thạnh - Tp.HCM", true, false, false);
                        //Mau So
                        excel.Merge("F" + iRow, "I" + iRow);
                        excel.setAlignAndValign(iRow, 6, off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                        excel.SetValueWithFormat(iRow, 6, "Mẫu số 01TT", true, false, false);
                        //Ban hanh
                        excel.Merge("F" + (iRow + 1), "I" + (iRow + 2));
                        excel.setAlignAndValign(iRow + 1, 6, off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                        excel.SetValueWithFormat(iRow + 1, 6, "(Ban hành theo QĐ số 48/2006/QĐ-BTC 14/09/2006 của Bộ trưởng BTC)", false, false, false);
                        excel.SetFont("A" + iRow, "I" + (iRow + 3), 9);

                        //Ten phieu chi
                        excel.Merge("D" + (iRow + 3), "F" + (iRow + 4));
                        excel.setAlignAndValign(iRow + 3, 4, off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                        excel.SetValueWithFormat(iRow + 3, 4, "PHIẾU CHI", true, false, false);
                        excel.SetFont("D" + (iRow + 3), "D" + (iRow + 3), 18);
                        //So lien
                        excel.Merge("D" + (iRow + 5), "F" + (iRow + 5));
                        excel.setAlignAndValign(iRow + 5, 4, off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                        excel.SetValueWithFormat(iRow + 5, 4, "Liên 1", true, false, false);
                        excel.SetFont("D" + (iRow + 5), "D" + (iRow + 5), 9);
                        //So phieu chi
                        excel.Merge("D" + (iRow + 6), "F" + (iRow + 6));
                        excel.setAlignAndValign(iRow + 6, 4, off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                        excel.SetValueWithFormat(iRow + 6, 4, "Số " + rows[0][TB_SOCT].ToString().Trim(), true, false, false);
                        excel.SetFont("D" + (iRow + 6), "D" + (iRow + 6), 9);
                        //Ngay phieu chi
                        excel.Merge("C" + (iRow + 7), "G" + (iRow + 7));
                        excel.setAlignAndValign(iRow + 7, 3, off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                        if (rows[0][TB_NGAYCT] is DateTime)
                        {
                            DateTime time = (DateTime)rows[0][TB_NGAYCT];

                            //if (int.TryParse(strMonth, out iMonth) && time.Month != iMonth)
                            //{
                            //    excel.SetValueWithFormat(iRow + 7, 3, "Ngày " + time.ToString("MM") + " tháng " + time.ToString("dd") + " năm " + time.ToString("yyyy") + "", false, false, false);
                            // }
                            //else
                            {
                                excel.SetValueWithFormat(iRow + 7, 3, "Ngày " + time.ToString("dd") + " tháng " + time.ToString("MM") + " năm " + time.ToString("yyyy") + "", false, false, false);
                            }
                        }
                        excel.SetFont("C" + (iRow + 7), "C" + (iRow + 7), 10);
                        //Tai khoan


                        excel.setAlignAndValign(iRow + 4, 8, off.XlHAlign.xlHAlignLeft, off.XlVAlign.xlVAlignCenter);
                        excel.SetValueWithFormat(iRow + 4, 8, "Nợ TK", true, false, false);

                        excel.setAlignAndValign(iRow + 4, 9, off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                        excel.SetValueWithFormat(iRow + 4, 9, rows[0][TB_TKNO].ToString(), true, false, false);
                        if (rows.Length > 1)
                        {
                            excel.setAlignAndValign(iRow + 5, 8, off.XlHAlign.xlHAlignLeft, off.XlVAlign.xlVAlignCenter);
                            excel.SetValueWithFormat(iRow + 5, 8, "Nợ TK", true, false, false);

                            excel.setAlignAndValign(iRow + 5, 9, off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                            excel.SetValueWithFormat(iRow + 5, 9, rows[1][TB_TKNO], true, false, false);

                            excel.setAlignAndValign(iRow + 6, 8, off.XlHAlign.xlHAlignLeft, off.XlVAlign.xlVAlignCenter);
                            excel.SetValueWithFormat(iRow + 6, 8, "Có TK", true, false, false);

                            excel.setAlignAndValign(iRow + 6, 9, off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                            excel.SetValueWithFormat(iRow + 6, 9, "1111", true, false, false);
                        }
                        else
                        {
                            excel.setAlignAndValign(iRow + 5, 8, off.XlHAlign.xlHAlignLeft, off.XlVAlign.xlVAlignCenter);
                            excel.SetValueWithFormat(iRow + 5, 8, "Có TK", true, false, false);

                            excel.setAlignAndValign(iRow + 5, 9, off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                            excel.SetValueWithFormat(iRow + 5, 9, "1111", true, false, false);
                        }

                        excel.SetFont("H" + (iRow + 4), "I" + (iRow + 6), 10);

                        excel.SetFont("A" + (iRow + 9), "I" + (iRow + 18), 11);

                        excel.SetValueWithFormat(iRow + 9, 1, "  Họ tên người nhận tiền", false, false, false);

                        List<string> listName = new List<string> { "Đoàn Ngọc Duy", "Trần Thị Thanh Hiền", "Trương Ti", "Trần Thị Huyền Châu", "Nguyễn Thị Thanh Vân", "Trương Duy Khoa", "Vũ Duy Hòa", "Bùi Xuân Hoàng", "Hồ Ngọc Lâm", "Lê Hoàng Phương" };
                        excel.SetValueWithFormat(iRow + 9, 4, listName[new Random().Next(0, listName.Count)], false, false, false);
                        excel.SetValueWithFormat(iRow + 10, 1, "  Địa chỉ", false, false, false);
                        excel.SetValueWithFormat(iRow + 10, 4, "CTY CP - TM - DV Sài Gòn Ánh Dương", false, false, false);
                        excel.SetValueWithFormat(iRow + 11, 1, "  Lý do chi", false, false, false);
                        excel.SetValueWithFormat(iRow + 11, 4, rows[0][TB_DIENGIAI], false, false, false);
                        excel.SetValueWithFormat(iRow + 12, 1, "  Số tiền", false, false, false);
                        double totalMoney = (double)rows[0][TB_SOTIEN];
                        if (rows.Length > 1) totalMoney += (double)rows[1][TB_SOTIEN];
                        excel.SetValueWithFormat(iRow + 12, 4, totalMoney.ToString("#,##0") + " VNĐ", true, false, false);
                        excel.Merge("A" + (iRow + 13), "C" + (iRow + 14));
                        excel.SetValueWithFormat(iRow + 13, 1, "  Bằng chữ", false, false, false);
                        excel.Merge("D" + (iRow + 13), "I" + (iRow + 14));
                        string strMoney = ChuyenSo(totalMoney.ToString()) + " đồng ./.";
                        strMoney = Regex.Replace(strMoney, "\\s+", " ");
                        strMoney = strMoney.Substring(0, 1).ToUpper() + strMoney.Substring(1);
                        excel.SetValueWithFormat(iRow + 13, 4, strMoney, false, false, false);

                        excel.SetValueWithFormat(iRow + 15, 1, "  Kèm theo HĐ", false, false, false);
                        excel.SetValueWithFormat(iRow + 15, 4, "'" + rows[0][TB_SOHD].ToString(), false, false, false);

                        excel.Merge("G" + (iRow + 16), "I" + (iRow + 16));
                        excel.SetValueWithFormat(iRow + 16, 7, "=C" + (iRow + 7), false, false, false);

                        excel.SetWrapText("A" + iRow, "I" + (iRow + 7), true);
                        excel.SetWrapText("A" + (iRow + 9), "I" + (iRow + 15), false);
                        excel.setAlignAndValign("A" + (iRow + 9), "I" + (iRow + 15), off.XlHAlign.xlHAlignLeft, off.XlVAlign.xlVAlignCenter);
                        excel.SetWrapText("D" + (iRow + 13), "I" + (iRow + 14), true);

                        excel.Merge("A" + (iRow + 17), "C" + (iRow + 17));
                        excel.SetValueWithFormat(iRow + 17, 1, "Giám đốc", false, false, false);
                        excel.Merge("A" + (iRow + 18), "C" + (iRow + 18));
                        excel.SetValueWithFormat(iRow + 18, 1, "(Ký, họ tên, đóng dấu)", false, true, false);
                        excel.Merge("A" + (iRow + 20), "C" + (iRow + 20));
                        excel.SetValueWithFormat(iRow + 20, 1, "Nguyễn Tất Hiệu", false, false, false);

                        excel.Merge("D" + (iRow + 17), "E" + (iRow + 17));
                        excel.SetValueWithFormat(iRow + 17, 4, "Thủ quỹ", false, false, false);
                        excel.Merge("D" + (iRow + 18), "E" + (iRow + 18));
                        excel.SetValueWithFormat(iRow + 18, 4, "(Ký, họ tên)", false, true, false);
                        excel.Merge("D" + (iRow + 20), "E" + (iRow + 20));
                        excel.SetValueWithFormat(iRow + 20, 4, "Vũ Thị Thu Phương", false, false, false);

                        excel.Merge("F" + (iRow + 17), "G" + (iRow + 17));
                        excel.SetValueWithFormat(iRow + 17, 6, "Người lập phiếu", false, false, false);
                        excel.Merge("F" + (iRow + 18), "G" + (iRow + 18));
                        excel.SetValueWithFormat(iRow + 18, 6, "(Ký, họ tên)", false, true, false);
                        excel.Merge("F" + (iRow + 20), "G" + (iRow + 20));
                        excel.SetValueWithFormat(iRow + 20, 6, "Vũ Thị Thu Phương", false, false, false);

                        excel.Merge("H" + (iRow + 17), "I" + (iRow + 17));
                        excel.SetValueWithFormat(iRow + 17, 8, "Người nhận tiền", false, false, false);
                        excel.Merge("H" + (iRow + 18), "I" + (iRow + 18));
                        excel.SetValueWithFormat(iRow + 18, 8, "(Ký, họ tên)", false, true, false);
                        excel.Merge("H" + (iRow + 20), "I" + (iRow + 20));
                        excel.SetValueWithFormat(iRow + 20, 8, "=D" + (iRow + 9), false, false, false);

                        excel.setAlignAndValign("A" + (iRow + 17), "I" + (iRow + 20), off.XlHAlign.xlHAlignCenter, off.XlVAlign.xlVAlignCenter);
                        excel.SetFont("A" + (iRow + 20), "I" + (iRow + 20), 10);
                        if (iCount % 2 == 0)
                        {
                            excel.SetPageBreak(iRow + 21);
                            iRow = iRow + 21;
                        }
                        else
                        {
                            iRow = iRow + 25;
                        }
                        rows[0].Delete();
                        if (rows.Length > 1)
                        {
                            rows[1].Delete();
                        }
                    }
                    excel.SetSheetName("T" + iCurrentMonth.ToString("00"));
                }
                    #endregion
                excel.End_Write();
                if (File.Exists(strFileName))
                {
                    Process.Start(strFileName);
                }
            }
            catch
            {

            }
        }

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            string strFileName = txtFileName.Text;
            if (!File.Exists(strFileName))
            {
                MessageBox.Show("File không tồn tại");
                return;
            }
            off.Application oXL = null;
            off._Workbook oWB = null;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                off._Worksheet Osheet = null;
                oXL = new off.Application();
                oXL.Visible = false;
                oWB = oXL.Workbooks.Add(strFileName);
                try
                {
                    Osheet = (off.Worksheet)oWB.Sheets[int.Parse(txtNoOfSheet.Text.Trim())];
                }
                catch
                {
                }
                dtImport = new DataTable();
                dtImport.Columns.Add(TB_STT, typeof(string));
                dtImport.Columns.Add(TB_SOCT, typeof(string));
                dtImport.Columns.Add(TB_NGAYCT, typeof(DateTime));
                dtImport.Columns.Add(TB_DIENGIAI, typeof(string));
                dtImport.Columns.Add(TB_TKNO, typeof(string));
                dtImport.Columns.Add(TB_SOTIEN, typeof(double));
                dtImport.Columns.Add(TB_SOHD, typeof(string));

                int iRowFrom = int.Parse(txtRowFrom.Text.Trim());
                int iRowTo = int.Parse(txtRowTo.Text.Trim());
                int iCount = 1;
                for (int iIndex = iRowFrom; iIndex < iRowTo; iIndex++)
                {
                    DataRow rowImport = dtImport.NewRow();

                    off.Range cell = (off.Range)Osheet.Cells[iIndex, 2];
                    if (cell != null && !string.IsNullOrEmpty(cell.Text))
                    {
                        rowImport[TB_SOCT] = cell.Value2.ToString().Trim();
                    }
                    if (rowImport[TB_SOCT].ToString().Trim().IndexOf("PC") != 0) continue;

                    cell = (off.Range)Osheet.Cells[iIndex, 3];
                    if (cell != null && !string.IsNullOrEmpty(cell.Text))
                    {
                        DateTime dTime = DateTime.Now;
                        if (DateTime.TryParse(cell.Text.ToString().Trim(), new System.Globalization.DateTimeFormatInfo { ShortDatePattern = "dd/MM/yyyy" }, DateTimeStyles.None, out dTime))
                        {
                            rowImport[TB_NGAYCT] = dTime;
                        }
                        else if (DateTime.TryParse(cell.Text.ToString().Trim(), new System.Globalization.DateTimeFormatInfo { ShortDatePattern = "MM/dd/yyyy" }, DateTimeStyles.None, out dTime))
                        {
                            rowImport[TB_NGAYCT] = dTime;
                        }
                    }
                    cell = (off.Range)Osheet.Cells[iIndex, 4];
                    if (cell != null && !string.IsNullOrEmpty(cell.Text))
                    {
                        rowImport[TB_DIENGIAI] = cell.Value2.ToString().Trim();
                    }
                    cell = (off.Range)Osheet.Cells[iIndex, 5];
                    if (cell != null && !string.IsNullOrEmpty(cell.Text))
                    {
                        rowImport[TB_TKNO] = cell.Value2.ToString().Trim();
                    }
                    cell = (off.Range)Osheet.Cells[iIndex, 7];
                    if (iCount == 162)
                    {

                    }
                    if (cell != null && !string.IsNullOrEmpty(cell.Text))
                    {
                        if (cell.Value2 is double)
                        {
                            rowImport[TB_SOTIEN] = Math.Ceiling((double)cell.Value2);
                        }
                        else
                        {
                            double dSoTien = 0;
                            double.TryParse(cell.Text.ToString().Trim(), out dSoTien);
                            rowImport[TB_SOTIEN] = Math.Ceiling(dSoTien);
                        }
                    }
                    cell = (off.Range)Osheet.Cells[iIndex, 8];
                    if (cell != null && !string.IsNullOrEmpty(cell.Text))
                    {
                        rowImport[TB_SOHD] = cell.Value2.ToString().Trim();
                    }
                    rowImport[TB_STT] = iCount++;
                    dtImport.Rows.Add(rowImport);
                }
                gridData.DataSource = dtImport;
            }
            catch
            {
            }
            finally
            {
                oWB.Close(false, strFileName, Missing.Value);
                oXL.Quit();
                this.Cursor = Cursors.Default;
            }
        }

        private string ChuyenSo(string number)
        {
            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "lẻ ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if (i + j == len - 1)
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += dv[n - j - 1] + " ";
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            return doc;
        }
    }
}
