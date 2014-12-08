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
using FB.App_Common;

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
                    btnCheckMobile_Click(null, null);
                    return true;
                }
                else if (keyData == Keys.F2)
                {
                    btnCheckWeb_Click(null, null);
                    return true;
                }
                //else if (keyData == Keys.F4)
                //{
                //    btnTangCo4La_Click(null, null);

                //    return true;
                //}
                //else if (keyData == Keys.F3)
                //{
                //    btnCheckWeb_Click(null, null);
                //    return true;
                //}
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
            getDataOnload();
        }

        private void btnCapNhatNgaySinh_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnCapNhatNgaySinh.Enabled = false;
            Task.Factory.StartNew(getBirthdayInfo);
        }

        private void btnThemTaiKhoan_Click(object sender, EventArgs e)
        {
            try
            {
                if (AppSettings.Seft)
                {
                    int iTry = 0;
                    string t = @"";
                    for (int iIndex = 0; iIndex < t.Split('\n').Length; iIndex++)
                    {
                        string info = t.Split('\n')[iIndex].ToString();
                        if (info.Contains('-'))
                        {
                            if (iTry == 0)
                            {
                                changeIP();
                            }
                            iTry++;
                            if (iTry >= 3) iTry = 0;
                            FaceBook fb = new FaceBook { Login = info.Split('-')[0].Trim(), Pass = info.Split('-')[1].Trim() };
                            FaceBookController fbController = new FaceBookController();
                            if (fbController.LoginMobile(fb))
                            {
                                Package p = Global.DBContext.Package.OrderByDescending(x => x.ID).FirstOrDefault();
                                if (p == null || p.Pokers.Count == 10)
                                {
                                    p = new Package();
                                    Global.DBContext.Package.Add(p);
                                }
                                PokerController pkController = new PokerController { Models = new Poker { FaceBook = fb, Package = p } };
                                if (pkController.LoginMobile())
                                {
                                    Global.DBContext.Poker.Add(pkController.Models);
                                    Global.DBContext.SaveChanges();
                                    iTry = 0;
                                }
                            }
                        }
                    }
                }
                else
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
                    if (MessageBox.Show("Bạn muốn xóa các TK được chọn ?", "Poker", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        List<PokerController> list = new List<PokerController>();
                        for (int iIndex = 0; iIndex < gridData.SelectedCells.Count; iIndex++)
                        {
                            PokerController pkController = gridData.Rows[gridData.SelectedCells[iIndex].RowIndex].DataBoundItem as PokerController;
                            if (list.Contains(pkController)) continue;
                            list.Add(pkController);
                        }
                        Package pack = txtPackNo.SelectedItem as Package;
                        foreach (PokerController pk in list)
                        {
                            Global.DBContext.Poker.Remove(pk.Models);
                            dicPokers[pack.ID].Remove(pk);
                        }
                        Global.DBContext.SaveChanges();
                        reloadGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void menuLoginLaiFacebook_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridData.Rows.Count > 0)
                {
                    //if (MessageBox.Show("Bạn muốn xóa các TK được chọn ?", "Poker", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        List<PokerController> list = new List<PokerController>();
                        for (int iIndex = 0; iIndex < gridData.SelectedCells.Count; iIndex++)
                        {
                            PokerController pkController = gridData.Rows[gridData.SelectedCells[iIndex].RowIndex].DataBoundItem as PokerController;
                            if (list.Contains(pkController)) continue;
                            list.Add(pkController);
                        }
                        foreach (PokerController pk in list)
                        {
                            new FaceBookController().LoginMobile(pk.Models.FaceBook);
                        }
                        Global.DBContext.SaveChanges();
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
                    //MessageBox.Show("Đã copy URL vào clipboad");
                }
            }
        }

        bool bOpenByPressAppKey = false;
        private void menuGridData_Opening(object sender, CancelEventArgs e)
        {
            menuCopyURL.Enabled = menuXoaTK.Enabled = gridData.Rows.Count > 0;
            if (gridData.Rows.Count > 0)
            {
                //gridData.ClearSelection();
                //gridData.Rows[gridData.CurrentCell.RowIndex].Selected = true;
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
                Task.Factory.StartNew(() =>
                {
                    changeIP();
                    ketBan();
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnCheckMobile_Click(object sender, EventArgs e)
        {
            try
            {
                if (isBusy) return;
                isBusy = true;
                btnCheckMobile.Enabled = false;
                if (txtCheckTuDong.Checked)
                {
                    Task.Factory.StartNew(() =>
                    {
                        changeIP();
                        checkMobile();
                    });
                }
                else
                {
                    Task.Factory.StartNew(checkMobile);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnCheckWeb_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnCheckWeb.Enabled = false;
            Task.Factory.StartNew(checkWeb);
        }

        private void btnChuanBiRutFan_Click(object sender, EventArgs e)
        {
            groupRutfan.Visible = true;
            if (!string.IsNullOrEmpty(Clipboard.GetText()) && Clipboard.GetText().Contains("apps.facebook.com/vntexas"))
            {
                txtRutFanLink.Text = Clipboard.GetText().Trim();
            }
            txtPackNo.SelectedIndex = -1;
        }

        private void btnRutThuongFan_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnRutThuongFan.Enabled = false;
            Task.Factory.StartNew(rutThuongFan);
        }

        private void btnAuthenNhanFanChip_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnAuthenNhanFanChip.Enabled = false;
            Task.Factory.StartNew(authenRutThuongFan);
        }
        #endregion

        #region - METHOD -
        private void authenRutThuongFan()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                List<string> listLink = new List<string>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    Task task = Task.Factory.StartNew(
                        () =>
                        {
                            pkSource.LoginWebApp(txtRutFanLink.Text.Trim(), true);
                        });
                    tasks.Add(task);
                    task.Wait(1000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    System.Threading.Thread.Sleep(1000);
                }
                if (Global.DBContext.ChangeTracker.HasChanges())
                {
                    Global.DBContext.SaveChanges();
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
                    { btnAuthenNhanFanChip.Enabled = true; };
                    this.BeginInvoke(action);
                }
            }
        }

        private void rutThuongFan()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                List<string> listLink = new List<string>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    Task task = Task.Factory.StartNew(
                        () =>
                        {
                            if (pkSource.bWebLogedIn) pkSource.RutFanChip(txtRutFanLink.Text.Trim());
                        });
                    tasks.Add(task);
                    task.Wait(100);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    System.Threading.Thread.Sleep(1000);
                }
                if (Global.DBContext.ChangeTracker.HasChanges())
                {
                    Global.DBContext.SaveChanges();
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
                    { btnRutThuongFan.Enabled = true; };
                    this.BeginInvoke(action);
                }
            }
        }

        private void getBirthdayInfo()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    Task t = Task.Factory.StartNew(() => pkSource.getBirthDayInfo());
                    tasks.Add(t);
                    t.Wait();
                    System.Threading.Thread.Sleep(1000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
                if (Global.DBContext.ChangeTracker.HasChanges())
                {
                    Global.DBContext.SaveChanges();
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
                    { btnCapNhatNgaySinh.Enabled = true; };
                    this.BeginInvoke(action);
                }
            }
        }

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
                List<string> listLink = new List<string>();
                List<Task> tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    Task task = Task.Factory.StartNew(() =>
                    {
                        string strLink = pkSource.ChiaSeChipMayMan();
                        if (!string.IsNullOrEmpty(strLink))
                        {
                            listLink.Add(strLink);
                        }
                    });
                    tasks.Add(task);
                    task.Wait(3000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
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
                        Task task = Task.Factory.StartNew(() => pkSource.NhanChipMayMan(listLinkForGet));
                        tasks.Add(task);
                        task.Wait(3000);
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
                //if (!this.IsDisposed)
                //{
                //    MethodInvoker action = delegate
                //    { btnNhanChipMayMan.Enabled = true; };
                //    this.BeginInvoke(action);
                //}
            }
        }

        private void checkWeb()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                List<string> listLink = new List<string>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    Task task = Task.Factory.StartNew(
                        () =>
                        {
                            if (txtCheckKyTen.Checked || txtCheckCo4La.Checked || txtCheckChipMayMan.Checked)
                            {
                                pkSource.LoginWebApp();
                                if (pkSource.bWebLogedIn)
                                {
                                    if (txtCheckKyTen.Checked)
                                    {
                                        if (AppSettings.Seft)
                                        {
                                            pkSource.KyTenWeb();
                                            pkSource.PlayMiniGame();
                                        }
                                        else
                                        {
                                            pkSource.KyTenWeb();
                                        }
                                    }
                                    if (txtCheckCo4La.Checked) pkSource.TangCo4La();
                                    if (txtCheckChipMayMan.Checked)
                                    {
                                        string strLink = pkSource.ChiaSeChipMayMan();
                                        if (!string.IsNullOrEmpty(strLink))
                                        {
                                            listLink.Add(strLink);
                                        }
                                    }
                                }
                            }
                        });
                    tasks.Add(task);
                    task.Wait(3000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    System.Threading.Thread.Sleep(1000);
                }
                tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
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
                    Task task = Task.Factory.StartNew(() =>
                    {
                        if (pkSource.bWebLogedIn)
                        {
                            if (txtCheckCo4La.Checked) pkSource.NhanCo4La();
                            if (txtCheckChipMayMan.Checked && listLinkForGet.Count > 0) pkSource.NhanChipMayMan(listLinkForGet);
                        }
                    });
                    tasks.Add(task);
                    task.Wait(3000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    System.Threading.Thread.Sleep(1000);
                }
                if (Global.DBContext.ChangeTracker.HasChanges())
                {
                    Global.DBContext.SaveChanges();
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
                    {
                        btnCheckWeb.Enabled = true;
                        if (txtCheckTuDong.Checked)
                        {
                            System.Threading.Thread.Sleep(2000);
                            try
                            {
                                if (txtPackNo.SelectedIndex < txtPackNo.Items.Count - 1)
                                {
                                    txtPackNo.SelectedIndex = txtPackNo.SelectedIndex + 1;
                                    btnCheckMobile_Click(null, null);
                                }
                            }
                            catch { }
                        }
                    };
                    this.BeginInvoke(action);
                }
            }
        }

        private void checkMobile()
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

                tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    tasks.Add(Task.Factory.StartNew(pkSource.KyTenMobile));
                    System.Threading.Thread.Sleep(1000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }

                tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
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
                    {
                        btnCheckMobile.Enabled = true;
                        if (txtCheckTuDong.Checked)
                        {
                            btnCheckWeb_Click(null, null);
                        }
                    };
                    this.BeginInvoke(action);
                }
            }
        }

        private void ketBan()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                FaceBookController fbController = new FaceBookController();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = gridData.Rows[iIndex].DataBoundItem as PokerController;
                    int iStart = iIndex;
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        //Send FriendRequest
                        for (int iSeed = iStart + 1; iSeed < gridData.Rows.Count; iSeed++)
                        {
                            if (this.IsDisposed) return;
                            PokerController pkDes = gridData.Rows[iSeed].DataBoundItem as PokerController;
                            if (pkSource.SendFriendRequest(pkDes.Models))
                            {
                                pkSource.Status = "Gửi kết bạn thành công tới " + pkDes.Models.FaceBook.Login;
                            }
                            else
                            {
                                pkSource.Status = "Gửi kết bạn KHÔNG thành công tới " + pkDes.Models.FaceBook.Login;
                            }
                            System.Threading.Thread.Sleep(2000);
                        }
                    }));
                    System.Threading.Thread.Sleep(1000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
                System.Threading.Thread.Sleep(5000);

                tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    int iStart = iIndex;
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        PokerController pkSource = gridData.Rows[iStart].DataBoundItem as PokerController;
                        if (pkSource.AcceptFriendRequest())
                        {
                            pkSource.Status = "Chấp nhận bạn thành công";
                        }
                        else
                        {
                            pkSource.Status = "Chấp nhận bạn KHÔNG thành công";
                        }
                    }));
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
                    {
                        btnKetBan.Enabled = true;
                        try
                        {
                            if (txtPackNo.SelectedIndex < txtPackNo.Items.Count - 1)
                            {
                                txtPackNo.SelectedIndex = txtPackNo.SelectedIndex + 1;
                                btnKetBan_Click(null, null);
                            }
                        }
                        catch { }
                    };
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
            else
            {
                Package selectedPackage = txtPackNo.SelectedItem as Package;
                BindingList<PokerController> listController = new BindingList<PokerController>();
                foreach (Package p in Global.DBContext.Package.ToList())
                {
                    if (p.Pack <= 5)
                    {
                        foreach (Poker poker in p.Pokers)
                        {
                            PokerController newPokerController = new PokerController { Models = poker, Status = "Khởi tạo thành công" };
                            newPokerController.GridContainer = gridData;
                            listController.Add(newPokerController);
                        }
                    }
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
                List<Package> listPackage = Global.DBContext.Package.Where(x => x.Pokers.Count > 0).ToList();
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

        private void changeIP()
        {
            MobileModermController.Disconnect();
            MobileModermController.Connect();
            string strIP = Utilities.GetMyIpAddress();
            var t = Global.DBContext.IPAddress.ToList();
            string strDate = DateTime.Today.ToString("yyyy-MM-dd");
            while (Global.DBContext.IPAddress.Where(x => x.IP == strIP && x.Date == strDate).Count() > 0)
            {
                MobileModermController.Disconnect();
                MobileModermController.Connect();
                strIP = Utilities.GetMyIpAddress();
            }
            Global.DBContext.IPAddress.Add(new IPAddress { IP = strIP, Date = DateTime.Today.ToString("yyyy-MM-dd") });
            Global.DBContext.SaveChanges();
        }

        #endregion
    }
}
