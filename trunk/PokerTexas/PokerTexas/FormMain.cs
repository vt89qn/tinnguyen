using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Net;
using TableConstants;

namespace PokerTexas
{
    public partial class FormMain : Form
    {
        #region - DECLARE -
        DataProvider dbProvider = new DataProvider();
        DataTable dtPack = new DataTable(M_PackageConst.TableName);
        Dictionary<Int64, Poker> dicPoker = new Dictionary<Int64, Poker>();
        bool bInputCaptcha = false;
        Timer timerNhanThuongFanNhom = new Timer();
        Timer timerRefreshGrid = new Timer();
        #endregion
        #region - CONTRUCTOR -
        public FormMain()
        {
            InitializeComponent();
            this.Load += new EventHandler(FormMain_Load);
        }
        #endregion
        #region - EVENT -
        void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                //Check Config
                if (System.Configuration.ConfigurationSettings.AppSettings["CheckNhanCo"].Trim() != "0")
                {
                    chkNhanCo4La.Checked = true;
                }
                if (System.Configuration.ConfigurationSettings.AppSettings["CheckNhanThuong"].Trim() != "0")
                {
                    chkNhanThuong.Checked = true;
                }
                getAndBuildPackNo();
                timerRefreshGrid.Enabled = false;
                timerRefreshGrid.Interval = 500;
                timerRefreshGrid.Tick += (objs, obje) =>
                {
                    //gridData.Refresh();
                };
                timerNhanThuongFanNhom.Enabled = false;
                timerNhanThuongFanNhom.Interval = 100;
                timerNhanThuongFanNhom.Tick += (objs, obje) =>
                {
                    if (!string.IsNullOrEmpty(txtHour.Text.Trim())
                        && !string.IsNullOrEmpty(txtMinute.Text.Trim()))
                    {
                        if (DateTime.Now.Hour >= int.Parse(txtHour.Text.Trim()) && DateTime.Now.Minute >= int.Parse(txtMinute.Text.Trim()))
                        {
                            timerNhanThuongFanNhom.Enabled = false;
                            btnNhanThuongFanNhom_Click(null, null);
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                //
            }
        }
        private void btnNhanThuongFanNhom_Click(object sender, EventArgs e)
        {
            try
            {
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (gridData[GridConst.Poker, iIndex].Value is Poker)
                    {
                        Poker poker = gridData[GridConst.Poker, iIndex].Value as Poker;
                        if (("," + txtPackNhanThuong.Text.Trim() + ",").Contains(poker.RowData[M_AccountConst.PackageXid].ToString()))
                        {
                            new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(poker.NhanThuongFanNhom)).Start(txtLinkNhanThuong.Text.Trim());
                        }
                    }
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {

            }
        }
        void btnTangCo4La_Click(object sender, EventArgs e)
        {
            groupMain.Enabled = false;
            timerRefreshGrid.Enabled = true;
            try
            {
                btnTangCo4La.Enabled = false;
                List<Poker> listPoker = new List<Poker>();
                List<string> listLinkFbID = new List<string>();
                #region - Get Link Chip -
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (gridData.Rows[iIndex].DataBoundItem is Poker)
                    {
                        Poker poker = gridData.Rows[iIndex].DataBoundItem as Poker;
                        if (poker.AccountDie) continue;
                        listPoker.Add(poker);
                        listLinkFbID = new List<string>();
                        DataTable dtFBID = dbProvider.GetData("SELECT * FROM "
                            + M_AccountConst.TableName
                            + " WHERE " + M_AccountConst.PackageXid
                            + "='" + poker.RowData[M_AccountConst.PackageXid].ToString().Trim()
                            + "' AND " + M_AccountConst.FaceBookID + "<>'" + poker.RowData[M_AccountConst.FaceBookID].ToString().Trim()
                            + "'", M_AccountConst.TableName);
                        if (dtFBID != null && dtFBID.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtFBID.Rows)
                            {
                                listLinkFbID.Add(row[M_AccountConst.FaceBookID].ToString().Trim());
                            }
                        }
                        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(poker.NhanCo4La)).Start(listLinkFbID);
                    }
                    for (int iCount = 0; iCount < 10; iCount++)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                }
                #endregion
                #region - Wait for all Poker send chip -
                bool bAllSend = false;
                while (!bAllSend)
                {
                    bAllSend = true;
                    foreach (Poker poker in listPoker)
                    {
                        if (!poker.SendChip && !poker.AccountDie)
                        {
                            bAllSend = false;
                            break;
                        }
                    }
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(200);
                }
                #endregion
                #region - Get Chip -
                for (int iIndex = 0; iIndex < listPoker.Count; iIndex++)
                {
                    new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(listPoker[iIndex].NhanCo4La)).Start("S2");

                    for (int iCount = 0; iCount < 10; iCount++)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
            timerRefreshGrid.Enabled = false;
            groupMain.Enabled = true;

            if (chkNhanThuong.Checked)
            {
                for (int iCount = 1; iCount < 100; iCount++)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                }
                btnNhanThuongHangNgay_Click(null, null);
            }
        }
        void btnNhanChipMayMan_Click(object sender, EventArgs e)
        {
            groupMain.Enabled = false;
            timerRefreshGrid.Enabled = true;
            try
            {
                btnNhanChipMayMan.Enabled = false;
                List<Poker> listPoker = new List<Poker>();
                #region - Get Link Chip -
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (gridData.Rows[iIndex].DataBoundItem is Poker)
                    {
                        Poker poker = gridData.Rows[iIndex].DataBoundItem as Poker;
                        if (poker.AccountDie) continue;
                        poker.HadAuthen = false;
                        listPoker.Add(poker);
                        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(poker.NhanChipMayMan)).Start("S1");
                    }
                    for (int iCount = 0; iCount < 10; iCount++)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                }
                #endregion
                #region - Wait for all Poker get link chip -
                bool bAllGetChip = false;
                while (!bAllGetChip)
                {
                    bAllGetChip = true;
                    foreach (Poker poker in listPoker)
                    {
                        if (!poker.GotLinkChip)
                        {
                            if (poker.AccountDie) continue;
                            bAllGetChip = false;
                            break;
                        }
                    }
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(200);
                }
                #endregion
                #region - Build Params and Get Lucky Chip -
                for (int iIndex = 0; iIndex < listPoker.Count; iIndex++)
                {
                    List<string> listLink = new List<string>();
                    for (int iPoker = 1; iPoker <= 3; iPoker++)
                    {
                        if (iIndex + iPoker < listPoker.Count)
                        {
                            if (listPoker[iIndex + iPoker].ListLinkLuckyChip.Count > 0)
                            {
                                listLink.Add(listPoker[iIndex + iPoker].ListLinkLuckyChip[0]);
                                //listPoker[iIndex + iPoker].ListLinkLuckyChip.RemoveAt(0);
                            }
                        }
                        else
                        {
                            if (listPoker[iIndex + iPoker - listPoker.Count].ListLinkLuckyChip.Count > 0)
                            {
                                listLink.Add(listPoker[iIndex + iPoker - listPoker.Count].ListLinkLuckyChip[0]);
                                //listPoker[iIndex + iPoker - listPoker.Count].ListLinkLuckyChip.RemoveAt(0);
                            }
                        }
                    }
                    //if (listLink.Count > 0)
                    {
                        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(listPoker[iIndex].NhanChipMayMan)).Start(listLink);
                    }
                    for (int iCount = 0; iCount < 10; iCount++)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
            timerRefreshGrid.Enabled = false;
            groupMain.Enabled = true;
            if (chkNhanCo4La.Checked)
            {
                for (int iCount = 1; iCount < 100; iCount++)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                }
                btnTangCo4La_Click(null, null);
            }
        }
        void btnNhanThuongHangNgay_Click(object sender, EventArgs e)
        {
            timerRefreshGrid.Enabled = true;
            groupMain.Enabled = false;
            try
            {
                bInputCaptcha = false;
                btnNhanThuongHangNgay.Enabled = false;
                List<Poker> listPoker = new List<Poker>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    picCaptcha.Image = new Bitmap(200, 70);
                    if (gridData.Rows[iIndex].DataBoundItem is Poker)
                    {
                        Poker poker = gridData.Rows[iIndex].DataBoundItem as Poker;
                        listPoker.Add(poker);
                        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(poker.NhanThuongHangNgay)).Start("S1");
                    }
                    for (int iCount = 0; iCount < 20; iCount++)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                }
                while (listPoker.Count > 0)
                {
                    Poker pokerWorking = null;
                    foreach (Poker poker in listPoker)
                    {
                        if (poker.GotdayMoney || (poker.GotCaptchaDayMoney && !poker.EnterCaptcha) || poker.WrongCaptcha || poker.AccountDie)
                        {
                            pokerWorking = poker;
                            break;
                        }
                    }
                    if (pokerWorking != null)
                    {
                        if (pokerWorking.GotdayMoney || pokerWorking.AccountDie)
                        {
                            listPoker.Remove(pokerWorking);
                            continue;
                        }
                        if (pokerWorking.WrongCaptcha)
                        {
                            pokerWorking.GotdayMoney = false;
                            pokerWorking.GotCaptchaDayMoney = false;
                            pokerWorking.WrongCaptcha = false;
                            pokerWorking.EnterCaptcha = false;
                            new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(pokerWorking.NhanThuongHangNgay)).Start("S2");
                        }
                        if (pokerWorking.GotCaptchaDayMoney)
                        {
                            if (pokerWorking.ImageCaptcha != null)
                            {
                                groupCaptcha.Visible = true;
                                picCaptcha.Image = pokerWorking.ImageCaptcha;
                                txtCaptcha.Enabled = true;
                                txtCaptcha.Text = string.Empty;
                                txtCaptcha.Focus();
                                bInputCaptcha = false;
                                while (!bInputCaptcha)
                                {
                                    Application.DoEvents();
                                    System.Threading.Thread.Sleep(100);
                                }
                                pokerWorking.EnterCaptcha = true;
                                new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(pokerWorking.NhanThuongHangNgay)).Start("S3" + txtCaptcha.Text.Trim());
                            }
                            else
                            {
                                pokerWorking.GotdayMoney = false;
                                pokerWorking.GotCaptchaDayMoney = false;
                                pokerWorking.WrongCaptcha = false;
                                pokerWorking.EnterCaptcha = false;
                                new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(pokerWorking.NhanThuongHangNgay)).Start("S2");
                            }
                        }
                    }
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                }
                groupCaptcha.Visible = false;
            }
            catch (Exception ex)
            {
                //
            }
            timerRefreshGrid.Enabled = false;
            groupMain.Enabled = true;
        }
        void btnThemTaiKhoan_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, object> dicParam = new Dictionary<string, object>();
                AddAccount addAccount = new AddAccount();
                addAccount.ShowDialog();
                dicParam = addAccount.DicParams;
                if (dicParam != null
                    && dicParam.ContainsKey(M_AccountConst.Account)
                    && dicParam.ContainsKey(M_AccountConst.Password)
                    && dicParam.ContainsKey(M_AccountConst.CookieFB)
                    && dicParam.ContainsKey(M_AccountConst.FaceBookID))
                {
                    DataTable dtAccount = dbProvider.GetData("SELECT * FROM " + M_AccountConst.TableName + " WHERE 1=2", M_AccountConst.TableName);
                    if (dtAccount != null && dtAccount.Columns.Count > 0)
                    {
                        DataRow rowNewAccount = dtAccount.NewRow();
                        dtAccount.Rows.Add(rowNewAccount);
                        rowNewAccount[M_AccountConst.Account] = dicParam[M_AccountConst.Account];
                        rowNewAccount[M_AccountConst.CookieFB] = dicParam[M_AccountConst.CookieFB];
                        rowNewAccount[M_AccountConst.FaceBookID] = dicParam[M_AccountConst.FaceBookID];
                        rowNewAccount[M_AccountConst.PackageXid] = txtPackNo.Text.Trim();
                        rowNewAccount[M_AccountConst.Password] = dicParam[M_AccountConst.Password];
                        if (dbProvider.Execute(dtAccount))
                        {
                            getAccountOfPack(txtPackNo.Text.Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void btnXoaTaiKhoan_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridData.CurrentCell != null
                    && gridData.Rows[gridData.CurrentCell.RowIndex].DataBoundItem is DataRowView)
                {
                    DataRow rowDelete = (gridData.Rows[gridData.CurrentCell.RowIndex].DataBoundItem as DataRowView).Row;
                    DataTable dtDelete = dbProvider.GetData("SELECT * FROM " + M_AccountConst.TableName + " WHERE Pid='" + rowDelete[M_AccountConst.Pid].ToString() + "'", M_AccountConst.TableName);
                    if (dtDelete != null && dtDelete.Rows.Count > 0)
                    {
                        dtDelete.Rows[0].Delete();
                        if (dbProvider.Execute(dtDelete))
                        {
                            getAccountOfPack(txtPackNo.Text.Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        void btnThemPack_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtPack != null)
                {
                    if (dtPack.Rows.Count < 15)
                    {
                        if (dtPack.Rows[0][M_PackageConst.Pid].ToString() == "0"
                            || string.IsNullOrEmpty(dtPack.Rows[0][M_PackageConst.Pid].ToString()))
                        {
                            dtPack.Rows.RemoveAt(0);
                        }
                        dtPack.Rows.Add(dtPack.NewRow());
                        if (dbProvider.Execute(dtPack))
                        {
                            getAndBuildPackNo();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Vượt giới hạn DB !, không thể thêm Pack.");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        void txtPackNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtPackNo.SelectedItem is DataRowView)
                {
                    DataRow rowPack = (txtPackNo.SelectedItem as DataRowView).Row;
                    getAccountOfPack(rowPack[M_PackageConst.Pid].ToString());
                }
            }
            catch (Exception ex)
            {
                //Handle Error
            }
        }
        void btnNhapCaptcha_Click(object sender, EventArgs e)
        {
            try
            {
                bInputCaptcha = true;
            }
            catch (Exception ex)
            {
                //
            }
        }
        void txtCaptcha_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnNhapCaptcha_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                //
            }
        }
        private void chkNhanThuongFan_CheckedChanged(object sender, EventArgs e)
        {
            this.timerNhanThuongFanNhom.Enabled = this.chkNhanThuongFan.Checked;
        }

        private void gridData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //return;
            try
            {
                Poker pokerUpdate = gridData.Rows[gridData.CurrentCell.RowIndex].DataBoundItem as Poker;
                Dictionary<string, object> dicParam = new Dictionary<string, object>();
                AddAccount addAccount = new AddAccount();
                addAccount.Account = pokerUpdate.RowData[M_AccountConst.Account].ToString();
                //addAccount.Password = pokerUpdate.RowData[M_AccountConst.Password].ToString();
                addAccount.ShowDialog();
                dicParam = addAccount.DicParams;
                if (dicParam != null
                    && dicParam.ContainsKey(M_AccountConst.Account)
                    && dicParam.ContainsKey(M_AccountConst.Password)
                    && dicParam.ContainsKey(M_AccountConst.CookieFB)
                    && dicParam.ContainsKey(M_AccountConst.FaceBookID))
                {
                    DataTable dtAccount = dbProvider.GetData("SELECT * FROM " + M_AccountConst.TableName + " WHERE Pid = '" + pokerUpdate.RowData[M_AccountConst.Pid].ToString() + "'", M_AccountConst.TableName);
                    if (dtAccount != null && dtAccount.Rows.Count > 0)
                    {
                        DataRow rowAccount = dtAccount.Rows[0];
                        rowAccount[M_AccountConst.Account] = dicParam[M_AccountConst.Account];
                        rowAccount[M_AccountConst.CookieFB] = dicParam[M_AccountConst.CookieFB];
                        rowAccount[M_AccountConst.FaceBookID] = dicParam[M_AccountConst.FaceBookID];
                        rowAccount[M_AccountConst.PackageXid] = pokerUpdate.RowData[M_AccountConst.PackageXid];
                        rowAccount[M_AccountConst.Password] = dicParam[M_AccountConst.Password];
                        if (dbProvider.Execute(dtAccount))
                        {
                            dicPoker.Remove((Int64)pokerUpdate.RowData[M_AccountConst.Pid]);
                            getAccountOfPack(txtPackNo.Text.Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void menuCopyFacebookCookie_Click(object sender, EventArgs e)
        {
            try
            {
                Poker pokerUpdate = gridData.Rows[gridData.CurrentCell.RowIndex].DataBoundItem as Poker;
                if (pokerUpdate.RowData[M_AccountConst.CookieFB] is byte[])
                { //
                    Serializer serial = new Serializer();
                    object objCookie = serial.ConvertBlobToObject(pokerUpdate.RowData[M_AccountConst.CookieFB] as byte[]);
                    if (objCookie is CookieContainer)
                    {
                        string strCookie = string.Empty;
                        foreach (Cookie ck in (objCookie as CookieContainer).GetCookies(new Uri("https://www.facebook.com")))
                        {
                            if (new List<string> { "c_user", "datr", "xs" }.Contains(ck.Name))
                            {
                                strCookie += (string.IsNullOrEmpty(strCookie) ? "" : ",\r\n")
                                    + "{"
                                    + "\r\n\"domain\": \".facebook.com\","
                                    + "\r\n\"expirationDate\": 1395382339,"
                                    + "\r\n\"hostOnly\": false,"
                                    + "\r\n\"httpOnly\": " + (ck.HttpOnly ? "true" : "false") + ","
                                    + "\r\n\"name\": \"" + ck.Name + "\","
                                    + "\r\n\"path\": \"/\","
                                    + "\r\n\"secure\": " + (ck.Secure ? "true" : "false") + ","
                                    + "\r\n\"session\": false,"
                                    + "\r\n\"storeId\": \"0\","
                                    + "\r\n\"value\": \"" + ck.Value + "\""
                                    + "\r\n}";
                            }
                        }
                        strCookie = "[" + strCookie + "]";
                        Clipboard.SetText(strCookie);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
        #region - METHOD -
        private void getAccountOfPack(string strPackNo)
        {
            try
            {
                string strSQL = string.Empty;
                if (!string.IsNullOrEmpty(strPackNo) && strPackNo.Trim() != "0")
                {
                    strSQL = "SELECT * FROM "
                    + M_AccountConst.TableName + " WHERE "
                    + M_AccountConst.PackageXid + "='"
                    + strPackNo + "'";
                }
                else
                {
                    strSQL = "SELECT * FROM "
                    + M_AccountConst.TableName;
                }
                strSQL += " ORDER BY PackageXid,Pid";
                DataTable dtAccount = dbProvider.GetData(strSQL, M_AccountConst.TableName);
                foreach (DataRow rowAccount in dtAccount.Rows)
                {
                    if (rowAccount[M_AccountConst.Pid] is Int64
                        && !dicPoker.ContainsKey((Int64)rowAccount[M_AccountConst.Pid]))
                    {
                        Poker poker = new Poker();
                        poker.RowData = rowAccount;
                        poker.Status = "Khởi tạo thành công.";
                        poker.Account = rowAccount[M_AccountConst.Account].ToString();
                        poker.PackageXid = rowAccount[M_AccountConst.PackageXid].ToString();
                        dicPoker.Add((Int64)rowAccount[M_AccountConst.Pid], poker);
                    }
                }
                fillGridData(strPackNo);
            }
            catch
            {

            }
        }
        private void fillGridData(string strPackNo)
        {
            try
            {
                int iSTT = 0;
                BindingList<Poker> listPoker = new BindingList<Poker>();
                foreach (KeyValuePair<Int64, Poker> item in dicPoker)
                {
                    if (item.Value.PackageXid.Trim() == strPackNo.Trim()
                        || strPackNo == "0")
                    {
                        iSTT++;
                        listPoker.Add(item.Value);
                        item.Value.STT = iSTT;
                    }
                }
                if (gridData.Columns.Count == 0)
                {
                    gridData.AutoGenerateColumns = false;
                    DataGridViewTextBoxColumn colStt = new DataGridViewTextBoxColumn();
                    colStt.Name = GridConst.STT;
                    colStt.HeaderText = GridConst.STT;
                    colStt.Width = 30;
                    colStt.DataPropertyName = GridConst.STT;
                    colStt.SortMode = DataGridViewColumnSortMode.NotSortable;
                    gridData.Columns.Add(colStt);

                    DataGridViewTextBoxColumn colAccount = new DataGridViewTextBoxColumn();
                    colAccount.Name = M_AccountConst.Account;
                    colAccount.HeaderText = M_AccountConst.Account;
                    colAccount.Width = 200;
                    colAccount.DataPropertyName = M_AccountConst.Account;
                    colAccount.SortMode = DataGridViewColumnSortMode.NotSortable;
                    gridData.Columns.Add(colAccount);

                    DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
                    colStatus.Name = GridConst.Status;
                    colStatus.HeaderText = GridConst.Status;
                    colStatus.Width = 150;
                    colStatus.DataPropertyName = GridConst.Status;
                    colStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
                    gridData.Columns.Add(colStatus);

                    DataGridViewTextBoxColumn colPackageNo = new DataGridViewTextBoxColumn();
                    colPackageNo.Name = M_AccountConst.PackageXid;
                    colPackageNo.HeaderText = GridConst.Package;
                    colPackageNo.Width = 75;
                    colPackageNo.DataPropertyName = M_AccountConst.PackageXid;
                    colPackageNo.SortMode = DataGridViewColumnSortMode.NotSortable;
                    gridData.Columns.Add(colPackageNo);

                }
                BindingSource bs = new BindingSource();
                bs.DataSource = listPoker;
                gridData.DataSource = listPoker;
                if (gridData.Rows.Count > 0)
                {
                    btnNhanThuongHangNgay.Enabled = true;
                    btnNhanChipMayMan.Enabled = true;
                    btnXoaTaiKhoan.Enabled = true;
                    btnTangCo4La.Enabled = true;

                    for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                    {
                        if (gridData.Rows[iIndex].DataBoundItem is Poker)
                        {
                            Poker poker = gridData.Rows[iIndex].DataBoundItem as Poker;
                            poker.UpdateInfo += (objs, objValue, AccountXid, strColName) =>
                            {
                                DataTable dtUpdate = dbProvider.GetData("SELECT * FROM " + M_AccountConst.TableName + " WHERE Pid='" + AccountXid + "'", M_AccountConst.TableName);
                                if (dtUpdate != null && dtUpdate.Rows.Count == 1)
                                {
                                    dtUpdate.Rows[0][strColName] = objValue;
                                    dbProvider.Execute(dtUpdate);
                                }
                            };

                        }
                    }
                }
                else
                {
                    btnNhanThuongHangNgay.Enabled = false;
                    btnNhanChipMayMan.Enabled = false;
                    btnXoaTaiKhoan.Enabled = false;
                    btnTangCo4La.Enabled = false;
                }
                gridData.Refresh();
            }
            catch (Exception ex)
            {
                //Handle Error
            }
        }
        private void getAndBuildPackNo()
        {
            try
            {
                string strSQL = "SELECT * FROM " + M_PackageConst.TableName;
                dtPack = dbProvider.GetData(strSQL, M_PackageConst.TableName);
                if (dtPack != null)
                {
                    dtPack.Rows.InsertAt(dtPack.NewRow(), 0);
                    dtPack.Rows[0][M_PackageConst.Pid] = 0;
                    txtPackNo.DisplayMember = M_PackageConst.Pid;
                    txtPackNo.ValueMember = M_PackageConst.Pid;
                    txtPackNo.DataSource = dtPack;
                }
            }
            catch (Exception ex)
            {
                //Handle Error
            }
        }
        #endregion

        private void gridData_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
                {
                    gridData.ClearSelection();
                    gridData.Rows[gridData.CurrentCell.RowIndex].Selected = true;
                }
            }
            catch { }
        }
    }
}