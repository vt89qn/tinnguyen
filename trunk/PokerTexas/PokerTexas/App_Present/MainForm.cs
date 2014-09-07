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
using PokerTexas.App_Controller;
using PokerTexas.App_Model;
using PokerTexas.App_Context;
using PokerTexas.App_Common;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;
using System.Collections.Specialized;

namespace PokerTexas.App_Present
{
    public partial class MainForm : Form
    {
        #region - DECLARE -
        Dictionary<long, BindingList<PokerController>> dicPokers = new Dictionary<long, BindingList<PokerController>>();
        private bool isBusy = false;
        #endregion

        #region - CONTRUCTOR -
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        #region - EVENT -
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!isBusy)
            {
                if (keyData == Keys.F1)
                {
                    btnNhanThuongHangNgay_Click(null, null);
                    return true;
                }
                else if (keyData == Keys.F2)
                {
                    btnTangQuaBiMat_Click(null, null);
                    return true;
                }
                else if (keyData == Keys.F3)
                {
                    btnNhanChipMayMan_Click(null, null);
                    return true;
                }
                else if (keyData == Keys.F4)
                {
                    btnTangCo4La_Click(null, null);
                    return true;
                }
                else if (keyData == Keys.F5)
                {
                    if (gridData.Visible)
                    {
                        gridData.Visible = false;
                        this.Height = 180;
                    }
                    else
                    {
                        gridData.Visible = true;
                        this.Height = 480;
                    }
                }
                else if (keyData == Keys.F6)
                {
                    try
                    {
                        if (txtPackNo.SelectedIndex < txtPackNo.Items.Count - 1)
                        {
                            txtPackNo.SelectedIndex = txtPackNo.SelectedIndex + 1;
                        }
                    }
                    catch { }
                    return true;
                }
                else if (keyData == Keys.F7)
                {
                    btnKiemTraTaiKhoan_Click(null, null);
                    return true;
                }
                else if (keyData == Keys.F8)
                {
                    btnThemTaiKhoan_Click(null, null);
                    return true;
                }
                else if (keyData == Keys.F9)
                {
                    btnKetBan_Click(null, null);
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //while (true)
            //{
            //    MobileModermController.Connect();
            //    FaceBook fb = new FaceBookController().RegNewAccount();
            //    if (fb != null)
            //    {
            //        fb.FBPackageID = 3;
            //        Global.DBContext.FaceBook.Add(fb);
            //        Global.DBContext.SaveChanges();
            //        var t = Global.DBContext.FaceBook.ToList();
            //    }
            //    MobileModermController.Disconnect();
            //}
            //getDataOnload();
        }

        private void btnThemTaiKhoan_Click(object sender, EventArgs e)
        {
            try
            {
                AddAccount addAccount = new AddAccount();
                addAccount.ShowDialog();
                FaceBook fb = addAccount.Model;
                if (fb != null)
                {
                    //Global.DBContext.FaceBook.Add(fb);
                    //Global.DBContext.SaveChanges();
                    Package pack = txtPackNo.SelectedItem as Package;
                    PokerController pkController = new PokerController { Models = new Poker { FaceBook = fb, Package = pack } };
                    if (pkController.LoginMobile())
                    {
                        Global.DBContext.Poker.Add(pkController.Models);
                        Global.DBContext.SaveChanges();
                        dicPokers[pack.ID].Add(pkController);
                        pkController.Status = "Thêm thành công";
                        reloadGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnKiemTraTaiKhoan_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnKiemTraTaiKhoan.Enabled = false;
            Task.Factory.StartNew(getMoney);
        }

        private void menuXoaTK_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridData.Rows.Count > 0)
                {
                    PokerController pkController = gridData.Rows[gridData.CurrentCell.RowIndex].DataBoundItem as PokerController;
                    if (MessageBox.Show("Bạn muốn xóa Tài Khoản " + pkController.Models.FaceBook.Login + " ?", "Poker", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        Package pack = txtPackNo.SelectedItem as Package;
                        Global.DBContext.Poker.Remove(pkController.Models);
                        Global.DBContext.SaveChanges();
                        dicPokers[pack.ID].Remove(pkController);
                        reloadGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void menuCopyURL_Click(object sender, EventArgs e)
        {
            if (gridData.Rows.Count > 0)
            {
                PokerController pkController = gridData.Rows[gridData.CurrentCell.RowIndex].DataBoundItem as PokerController;
                FaceBookController fbController = new FaceBookController();
                string strURL = fbController.GetFaceBookLoginURL(pkController.Models.FaceBook, AppSettings.URLToCopy);
                if (!string.IsNullOrEmpty(strURL))
                {
                    Clipboard.SetText(strURL);
                    MessageBox.Show("Đã copy URL vào clipboad");
                }
            }
        }

        bool bOpenByPressAppKey = false;
        private void menuGridData_Opening(object sender, CancelEventArgs e)
        {
            menuCopyURL.Enabled = menuXoaTK.Enabled = gridData.Rows.Count > 0;
            if (gridData.Rows.Count > 0)
            {
                gridData.ClearSelection();
                gridData.Rows[gridData.CurrentCell.RowIndex].Selected = true;
                if (bOpenByPressAppKey)
                {
                    ContextMenuStrip cms = gridData.ContextMenuStrip;
                    if (cms != null)
                    {
                        Rectangle r = gridData.GetCellDisplayRectangle(gridData.CurrentCell.ColumnIndex, gridData.CurrentCell.RowIndex, false);
                        Point p = new Point(r.X, r.Y + r.Height / 2);
                        cms.Show(gridData, p);
                    }
                }
            }
        }

        private void gridData_KeyUp(object sender, KeyEventArgs e)
        {
            if (gridData.RowCount > 0)
            {
                if ((e.KeyCode == Keys.F10 && e.Shift) || e.KeyCode == Keys.Apps)
                {
                    bOpenByPressAppKey = true;
                }
            }
        }

        private void gridData_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                    {
                        gridData.CurrentCell = gridData[e.ColumnIndex, e.RowIndex];
                        bOpenByPressAppKey = false;
                    }
                    else
                    {
                        gridData.CurrentCell = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtPackNo_SelectedValueChanged(object sender, EventArgs e)
        {
            reloadGrid();
        }

        private void btnThemPack_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn muốn thêm Pack mới ?", "Poker", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    Package lastPack = Global.DBContext.Package.OrderByDescending(p => p.Pack).FirstOrDefault();
                    Package newPack = new Package();
                    newPack.Pack = lastPack != null ? lastPack.Pack + 1 : 1;
                    Global.DBContext.Package.Add(newPack);
                    Global.DBContext.SaveChanges();
                    (txtPackNo.DataSource as BindingSource).Add(newPack);
                    txtPackNo.SelectedItem = newPack;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnKetBan_Click(object sender, EventArgs e)
        {
            try
            {
                if (isBusy) return;
                isBusy = true;
                btnKetBan.Enabled = false;
                Task.Factory.StartNew(ketBan);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnNhanThuongHangNgay_Click(object sender, EventArgs e)
        {
            try
            {
                if (isBusy) return;
                isBusy = true;
                btnNhanThuongHangNgay.Enabled = false;
                Task.Factory.StartNew(nhanThuongHangNgay);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnTangQuaBiMat_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnTangQuaBiMat.Enabled = false;
            Task.Factory.StartNew(tangQuaBiMat);
        }

        private void btnNhanChipMayMan_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnNhanChipMayMan.Enabled = false;
            Task.Factory.StartNew(tangChipMayMan);
        }

        private void btnTangCo4La_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnTangCo4La.Enabled = false;
            Task.Factory.StartNew(tangCo4La);
        }
        #endregion

        #region - METHOD -
        private void getMoney()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    //pkSource.TangQuaBiMat();
                    tasks.Add(Task.Factory.StartNew(() => pkSource.GetInitMoney(true)));
                    System.Threading.Thread.Sleep(1000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                isBusy = false;
                if (!this.IsDisposed)
                {
                    MethodInvoker action = delegate
                    { btnKiemTraTaiKhoan.Enabled = true; };
                    this.BeginInvoke(action);
                }
            }
        }

        private void tangChipMayMan()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                List<string> listLink = new List<string>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        string strLink = pkSource.ChiaSeChipMayMan();
                        if (!string.IsNullOrEmpty(strLink))
                        {
                            listLink.Add(strLink);
                        }
                    }));
                    System.Threading.Thread.Sleep(1000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
                System.Threading.Thread.Sleep(1000);

                tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    List<string> listLinkForGet = new List<string>();
                    for (int iPoker = 1; iPoker <= 3; iPoker++)
                    {
                        if (iIndex + iPoker < listLink.Count)
                        {
                            listLinkForGet.Add(listLink[iIndex + iPoker]);
                        }
                        else if (iIndex + iPoker - listLink.Count < listLink.Count)
                        {
                            listLinkForGet.Add(listLink[iIndex + iPoker - listLink.Count]);
                        }
                    }
                    if (listLinkForGet.Count > 0)
                    {
                        if (this.IsDisposed) return;
                        PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                        tasks.Add(Task.Factory.StartNew(() => pkSource.NhanChipMayMan(listLinkForGet)));
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                isBusy = false;
                if (!this.IsDisposed)
                {
                    MethodInvoker action = delegate
                    { btnNhanChipMayMan.Enabled = true; };
                    this.BeginInvoke(action);
                }
            }
        }

        private void tangCo4La()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    tasks.Add(Task.Factory.StartNew(pkSource.TangCo4La));
                    System.Threading.Thread.Sleep(1000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    System.Threading.Thread.Sleep(1000);
                }
                System.Threading.Thread.Sleep(1000);
                tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    tasks.Add(Task.Factory.StartNew(pkSource.NhanCo4La));
                    System.Threading.Thread.Sleep(1000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                isBusy = false;
                if (!this.IsDisposed)
                {
                    MethodInvoker action = delegate
                    { btnTangCo4La.Enabled = true; };
                    this.BeginInvoke(action);
                }
            }
        }

        private void tangQuaBiMat()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    //pkSource.TangQuaBiMat();
                    tasks.Add(Task.Factory.StartNew(pkSource.TangQuaBiMat));
                    System.Threading.Thread.Sleep(1000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
                System.Threading.Thread.Sleep(1000);
                tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    //pkSource.TangQuaBiMat();
                    tasks.Add(Task.Factory.StartNew(pkSource.NhanQuaBiMat));
                    System.Threading.Thread.Sleep(1000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                isBusy = false;
                if (!this.IsDisposed)
                {
                    MethodInvoker action = delegate
                    { btnTangQuaBiMat.Enabled = true; };
                    this.BeginInvoke(action);
                }
            }
        }

        private void nhanThuongHangNgay()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                FaceBookController fbController = new FaceBookController();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    tasks.Add(Task.Factory.StartNew(pkSource.NhanThuongHangNgayMobile));
                    System.Threading.Thread.Sleep(1000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                isBusy = false;
                if (!this.IsDisposed)
                {
                    MethodInvoker action = delegate
                    { btnNhanThuongHangNgay.Enabled = true; };
                    this.BeginInvoke(action);
                }
            }
        }

        private void ketBan()
        {
            try
            {
                FaceBookController fbController = new FaceBookController();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    //Send FriendRequest
                    for (int iSeed = iIndex + 1; iSeed < gridData.Rows.Count; iSeed++)
                    {
                        if (this.IsDisposed) return;
                        PokerController pkDes = gridData.Rows[iSeed].DataBoundItem as PokerController;
                        if (fbController.SendFriendRequest(pkSource.Models.FaceBook, pkDes.Models.FaceBook))
                        {
                            pkSource.Status = "Gửi kết bạn thành công tới " + pkDes.Models.FaceBook.Login;
                        }
                        else
                        {
                            pkSource.Status = "Gửi kết bạn KHÔNG thành công tới " + pkDes.Models.FaceBook.Login;
                        }
                        System.Threading.Thread.Sleep(5000);
                    }
                    //Accept Friend Request
                    for (int iSeed = iIndex - 1; iSeed >= 0; iSeed--)
                    {
                        if (this.IsDisposed) return;
                        PokerController pkDes = gridData.Rows[iSeed].DataBoundItem as PokerController;
                        if (fbController.AcceptFriendRequest(pkSource.Models.FaceBook, pkDes.Models.FaceBook))
                        {
                            pkSource.Status = "Chấp nhận bạn thành công từ " + pkDes.Models.FaceBook.Login;
                        }
                        else
                        {
                            pkSource.Status = "Chấp nhận bạn KHÔNG thành công từ " + pkDes.Models.FaceBook.Login;
                        }
                        System.Threading.Thread.Sleep(5000);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                isBusy = false;
                if (!this.IsDisposed)
                {
                    MethodInvoker action = delegate
                    { btnKetBan.Enabled = true; };
                    this.BeginInvoke(action);
                }
            }
        }

        private void reloadGrid()
        {
            if (txtPackNo.SelectedItem is Package)
            {
                Package selectedPackage = txtPackNo.SelectedItem as Package;
                BindingList<PokerController> listController = new BindingList<PokerController>();
                if (dicPokers.ContainsKey(selectedPackage.ID))
                {
                    listController = dicPokers[selectedPackage.ID];
                }
                else
                {
                    foreach (Poker poker in (txtPackNo.SelectedItem as Package).Pokers)
                    {
                        PokerController newPokerController = new PokerController { Models = poker, Status = "Khởi tạo thành công" };
                        newPokerController.GridContainer = gridData;
                        if (AppSettings.GetMoneyOnLoad == "1")
                        {
                            newPokerController.GetInitMoney(false);
                        }
                        listController.Add(newPokerController);
                    }
                    dicPokers.Add(selectedPackage.ID, listController);
                }
                BindingSource bindingGrid = new BindingSource { DataSource = listController };
                gridData.DataSource = bindingGrid;
                for (int iIndex = 0; iIndex < gridData.Columns.Count; iIndex++)
                {
                    gridData.Columns[iIndex].Visible = false;
                }
                gridData.Columns[GridMainFormConst.Status].Visible = true;
                gridData.Columns[GridMainFormConst.Status].Width = 300;
                gridData.Columns[GridMainFormConst.Status].SortMode = DataGridViewColumnSortMode.NotSortable;

                gridData.Columns[GridMainFormConst.Money].Visible = true;
                gridData.Columns[GridMainFormConst.Money].Width = 100;
                gridData.Columns[GridMainFormConst.Money].SortMode = DataGridViewColumnSortMode.NotSortable;
                gridData.Columns[GridMainFormConst.Money].DefaultCellStyle = new DataGridViewCellStyle();
                gridData.Columns[GridMainFormConst.Money].DefaultCellStyle.Format = string.Format("#{0}###", CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator);

                gridData.Columns[GridMainFormConst.EarnToday].Visible = true;
                gridData.Columns[GridMainFormConst.EarnToday].HeaderText = "Earn Today";
                gridData.Columns[GridMainFormConst.EarnToday].Width = 100;
                gridData.Columns[GridMainFormConst.EarnToday].SortMode = DataGridViewColumnSortMode.NotSortable;
                gridData.Columns[GridMainFormConst.EarnToday].DefaultCellStyle = new DataGridViewCellStyle();
                gridData.Columns[GridMainFormConst.EarnToday].DefaultCellStyle.Format = string.Format("#{0}###", CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator);

                DataGridViewTextBoxColumn colStt = new DataGridViewTextBoxColumn();
                colStt.Name = GridMainFormConst.STT;
                colStt.HeaderText = GridMainFormConst.STT;
                colStt.Width = 50;
                colStt.SortMode = DataGridViewColumnSortMode.NotSortable;
                gridData.Columns.Insert(0, colStt);

                DataGridViewTextBoxColumn colAccount = new DataGridViewTextBoxColumn();
                colAccount.Name = TableFaceBookConst.Login;
                colAccount.HeaderText = TableFaceBookConst.Login;
                colAccount.Width = 150;
                colAccount.SortMode = DataGridViewColumnSortMode.NotSortable;
                gridData.Columns.Insert(1, colAccount);

                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    PokerController pkController = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    gridData[TableFaceBookConst.Login, iIndex].Value = pkController.Models.FaceBook.Login;
                    gridData[GridMainFormConst.STT, iIndex].Value = iIndex + 1;
                }
            }
        }

        private void getDataOnload()
        {
            try
            {
                //Load Package
                List<Package> listPackage = Global.DBContext.Package.ToList();
                BindingSource bindingPackage = new BindingSource { DataSource = listPackage };
                txtPackNo.DataSource = bindingPackage;
                txtPackNo.DisplayMember = TablePackageConst.Pack;
                txtPackNo.ValueMember = TablePackageConst.ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
