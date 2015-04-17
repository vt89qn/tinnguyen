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
using System.Net;
using System.Text.RegularExpressions;

namespace PokerTexas.App_Present
{
    public partial class MainForm : Form
    {
        #region - DECLARE -
        Dictionary<long, BindingList<PokerController>> dicPokers = new Dictionary<long, BindingList<PokerController>>();
        private bool isBusy = false;
        long selectedPackageID = 0;
        #endregion

        #region - CONTRUCTOR -
        public MainForm()
        {
            InitializeComponent();
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(new System.ComponentModel.ComponentResourceManager(typeof(MainForm)).GetObject("$this.Icon")));
            if (AppSettings.Seft)
            {
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                DateTime dateLastRun = DateTime.MinValue;
                timer.Interval = 60000;
                timer.Tick += (objs, obje) =>
                {
                    if (!isBusy)
                    {
                        if (DateTime.Now.Hour == 1 && DateTime.Now.Minute >= 1 && DateTime.Today > dateLastRun)
                        {
                            dateLastRun = DateTime.Today;
                            txtPackNo.SelectedIndex = 1;
                            //dicPokers = new Dictionary<long, BindingList<PokerController>>();
                            btnCheckWeb_Click(null, null);
                        }
                    }
                };
                timer.Start();
            }
            gridData.DataError += (objs, obje) => { };
        }
        #endregion

        #region - EVENT -
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!isBusy)
            {
                if (keyData == Keys.F1)
                {
                    btnCheckWeb_Click(null, null);
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
                //else if (keyData == Keys.F7)
                //{
                //    btnKiemTraTaiKhoan_Click(null, null);
                //    return true;
                //}
                //else if (keyData == Keys.F8)
                //{
                //    btnThemTaiKhoan_Click(null, null);
                //    return true;
                //}
                //else if (keyData == Keys.F9)
                //{
                //    btnKetBan_Click(null, null);
                //    return true;
                //}
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!AppSettings.Seft)
            {
                menuLayTinNhanHeThong.Visible = false;
                btnDoiIP.Visible = false;
            }
            getDataOnload();
        }

        private void btnDoiIP_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control)
            {
                MobileModermController.Disconnect();
            }
            else
            {
                changeIP();
            }
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
                        if (!string.IsNullOrEmpty(info))
                        {
                            //if (iTry == 0)
                            {
                                changeIP();
                            }
                            iTry++;
                            if (iTry >= 3) iTry = 0;
                            FaceBook fb = new FaceBook { Login = info.Trim(), Pass = "khong9999" };
                            FaceBookController fbController = new FaceBookController();
                            if (fbController.LoginMobile(fb))
                            {
                                FaceBook pexist = Global.DBContext.FaceBook.Where(x => x.FBID == fb.FBID).FirstOrDefault();
                                if (pexist != null)
                                {
                                    pexist.MBLoginText = fb.MBLoginText;
                                    pexist.Login = fb.Login;
                                    pexist.Pass = fb.Pass;
                                    fb = pexist;

                                    Poker pkexist = Global.DBContext.Poker.Where(x => x.FaceBookID == pexist.ID).FirstOrDefault();
                                    if (pkexist != null)
                                    {
                                        continue;
                                    }
                                }
                                Package p = Global.DBContext.Package.Where(x => x.ID >= 265 && x.Pokers.Count < 8).FirstOrDefault();
                                if (p == null)
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
                            PokerController pkController = dicPokers[selectedPackageID][gridData.SelectedCells[iIndex].RowIndex];
                            if (list.Contains(pkController)) continue;
                            list.Add(pkController);
                        }
                        //Package pack = txtPackNo.SelectedItem as Package;
                        foreach (PokerController pk in list)
                        {
                            Global.DBContext.Poker.Remove(pk.Models);
                            dicPokers[selectedPackageID].Remove(pk);
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
                    bool bChangeIP = true;
                    if (Control.ModifierKeys == Keys.Control || Control.ModifierKeys == Keys.Shift)
                    {
                        bChangeIP = false;
                    }
                    //if (MessageBox.Show("Bạn muốn xóa các TK được chọn ?", "Poker", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        List<PokerController> list = new List<PokerController>();
                        List<Poker> listDie = new List<Poker>();
                        for (int iIndex = 0; iIndex < gridData.SelectedCells.Count; iIndex++)
                        {
                            PokerController pkController = dicPokers[selectedPackageID][gridData.SelectedCells[iIndex].RowIndex];
                            if (list.Contains(pkController)) continue;
                            list.Add(pkController);
                        }
                        foreach (PokerController pk in list)
                        {
                            if (bChangeIP)
                            {
                                changeIP();
                            }
                            if (!new FaceBookController().LoginMobile(pk.Models.FaceBook))
                            {
                                listDie.Add(pk.Models);
                            }
                        }
                        if (listDie.Count > 0)
                        {
                            if (MessageBox.Show("Bạn muốn xóa các TK khong login duoc ?", "Poker", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                foreach (Poker pk in listDie)
                                {
                                    Global.DBContext.Poker.Remove(pk);
                                }
                            }
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
                PokerController pkController = dicPokers[selectedPackageID][gridData.CurrentCell.RowIndex];
                FaceBookController fbController = new FaceBookController();
                PokerExData exData = pkController.GetExData();
                string strURL = fbController.GetFaceBookLoginURL(pkController.Models.FaceBook, AppSettings.URLToCopy);
                if (sender == null)
                { //Press F11
                    strURL += "?ip=" + exData.ip_address;
                    gridData.Rows[gridData.CurrentCell.RowIndex].DefaultCellStyle.BackColor = Color.DarkGray;
                }
                if (!string.IsNullOrEmpty(strURL))
                {
                    Clipboard.SetText(strURL);
                    //MessageBox.Show("Đã copy URL vào clipboad");
                }
            }
        }

        private void menuCopyIP_Click(object sender, EventArgs e)
        {
            if (gridData.Rows.Count > 0)
            {
                PokerController pkController = dicPokers[selectedPackageID][gridData.CurrentCell.RowIndex];
                PokerExData exData = pkController.GetExData();
                if (exData != null && !string.IsNullOrEmpty(exData.ip_address))
                {
                    Clipboard.SetText(exData.ip_address);
                }
            }
        }

        private void menuLayTinNhanHeThong_Click(object sender, EventArgs e)
        {
            if (gridData.Rows.Count > 0)
            {
                PokerController pkController = dicPokers[selectedPackageID][gridData.CurrentCell.RowIndex];
                string id = Clipboard.GetText();
                if (!string.IsNullOrEmpty(id)) id = id.Trim();
                if (id != null && id.Length == 8 && (Control.ModifierKeys == Keys.Shift || Control.ModifierKeys == Keys.Control))
                {
                    pkController.GetTNHT(id);
                }
                else
                {
                    pkController.GetTNHT();
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
                else if (e.KeyCode == Keys.F11)
                {
                    menuCopyURL_Click(null, null);
                }
                else if (e.KeyCode == Keys.F12)
                {
                    menuCopyIP_Click(null, null);
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
                else if (e.Button == MouseButtons.Middle)
                {
                    gridData.CurrentCell = gridData[e.ColumnIndex, e.RowIndex];
                    gridData.ClearSelection();
                    gridData.CurrentCell.Selected = true;
                    menuCopyURL_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        bool bEnableSelectedValueChange = true;
        private void txtPackNo_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!bEnableSelectedValueChange) return;
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

        private void btnCheckWeb_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnCheckWeb.Enabled = false;
            if (txtCheckTuDong.Checked)
            {
                Task.Factory.StartNew(() =>
                {
                    changeIP();
                    check();
                });
            }
            else
            {
                Task.Factory.StartNew(check);
            }
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

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (!AppSettings.Seft)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Visible = false;
                    this.notifyIcon1.Visible = true;
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.notifyIcon1.Visible = false;
            this.Activate();
            this.WindowState = FormWindowState.Normal;
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
                    PokerController pkSource = dicPokers[selectedPackageID][iIndex];
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
                    PokerController pkSource = dicPokers[selectedPackageID][iIndex];
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

        private void getMoney()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = dicPokers[selectedPackageID][iIndex];
                    tasks.Add(Task.Factory.StartNew(() => pkSource.GetInitMoney(true)));
                    System.Threading.Thread.Sleep(200);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
                //Set Money After Check
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = dicPokers[selectedPackageID][iIndex];
                    PokerExData exData = pkSource.GetExData();
                    exData.money = pkSource.Money;
                    pkSource.SetExData(exData);
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
                    { btnKiemTraTaiKhoan.Enabled = true; };
                    this.BeginInvoke(action);
                }
            }
        }

        private void check()
        {
            try
            {
                List<long> listKetBan = new List<long>();
                foreach (string pack in AppSettings.PackMakeFriend.Split(','))
                {
                    if (pack.Contains('-'))
                    {
                        long num1 = 0;
                        long num2 = 0;
                        if (long.TryParse(pack.Split('-')[0].Trim(), out num1) && long.TryParse(pack.Split('-')[1].Trim(), out num2))
                        {
                            for (long number = num1; number <= num2; number++)
                            {
                                if (!listKetBan.Contains(number)) listKetBan.Add(number);
                            }
                        }
                    }
                    else
                    {
                        long number = 0;
                        if (long.TryParse(pack.Trim(), out number)) 
                        {
                            if (!listKetBan.Contains(number)) listKetBan.Add(number);
                        }
                    }
                }
                if (listKetBan.Contains(selectedPackageID))
                {
                    ketBan2();
                }
                List<Task> tasks = new List<Task>();
                List<string> listLink = new List<string>();
                List<string> listLinkChipMayMan = new List<string>();
                Dictionary<string, int> dicLinkOfPackage = new Dictionary<string, int>();
                List<string> listLinkOfPackage = new List<string>();
                if (txtCheckWeb.Checked)
                {
                    listLinkChipMayMan = Utilities.DeSerializeObject("linkLuckyChip.obj") as List<string>;
                    if (listLinkChipMayMan == null) listLinkChipMayMan = new List<string>();
                    foreach (string link in listLinkChipMayMan)
                    {
                        if (link == null) continue;
                        if (link.Contains(":p:" + selectedPackageID + ":"))
                        {
                            dicLinkOfPackage.Add(link, 0);
                            listLinkOfPackage.Add(link);
                        }
                    }
                }
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = dicPokers[selectedPackageID][iIndex];
                    Task task = Task.Factory.StartNew(
                        () =>
                        {
                            if (txtCheckWeb.Checked)
                            {
                                #region - Check Web -
                                if (txtCheckKyTen.Checked || txtCheckCo4La.Checked || txtCheckChipMayMan.Checked)
                                {
                                    pkSource.LoginWebApp();
                                    if (pkSource.bWebLogedIn)
                                    {
                                        pkSource.QuayVong();
                                        if (txtCheckKyTen.Checked)
                                        {
                                            //if (AppSettings.Seft)
                                            {
                                                System.Threading.Thread.Sleep(2000);
                                                pkSource.KyTenWeb();
                                            }
                                            System.Threading.Thread.Sleep(1000);
                                            pkSource.PlayMiniGame();
                                            System.Threading.Thread.Sleep(1000);
                                            pkSource.NhanTraiTim();
                                        }

                                        if (txtCheckCo4La.Checked)
                                        {
                                            System.Threading.Thread.Sleep(2000);
                                            pkSource.TangCo4La();
                                            pkSource.NhanCo4La();
                                        }

                                        if (txtCheckChipMayMan.Checked)
                                        {
                                            System.Threading.Thread.Sleep(2000);
                                            string strLink = pkSource.ChiaSeChipMayMan();
                                            if (!string.IsNullOrEmpty(strLink))
                                            {
                                                listLink.Add(":p:" + selectedPackageID + ":pk:" + pkSource.Models.PKID + ":l:" + strLink);
                                            }
                                            //Nhan chip may man
                                            List<string> listLinkForGet = new List<string>();
                                            for (int iCount = 0; iCount < 3; iCount++)
                                            {
                                                int iMinCount = int.MaxValue;
                                                string strMinLink = "";
                                                foreach (var link in listLinkOfPackage)
                                                {
                                                    if (!link.Contains(":pk:" + pkSource.Models.PKID + ":") && dicLinkOfPackage[link] < iMinCount)
                                                    {
                                                        iMinCount = dicLinkOfPackage[link];
                                                        strMinLink = link;
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(strMinLink))
                                                {
                                                    dicLinkOfPackage[strMinLink]++;
                                                    strMinLink = Regex.Match(strMinLink, ":l:(?<val>.+)").Groups["val"].Value;
                                                    if (!string.IsNullOrEmpty(strMinLink))
                                                    {
                                                        listLinkForGet.Add(strMinLink);
                                                    }
                                                }
                                            }
                                            if (listLinkForGet.Count > 0)
                                            {
                                                pkSource.NhanChipMayMan(listLinkForGet);
                                            }
                                        }
                                        System.Threading.Thread.Sleep(2000);
                                        pkSource.GetInitMoney(false);
                                    }
                                }
                                #endregion
                            }
                            if (txtCheckMobile.Checked)
                            {
                                #region - Check Mobile -
                                if (txtCheckDangNhapLT.Checked || txtCheckHangNgay.Checked || txtCheckChipBiMat.Checked)
                                {
                                    if (txtCheckDangNhapLT.Checked)
                                    {
                                        System.Threading.Thread.Sleep(2000);
                                        pkSource.NhanThuongHangNgayMobile();
                                    }
                                    if (txtCheckHangNgay.Checked)
                                    {
                                        System.Threading.Thread.Sleep(2000);
                                        pkSource.NhanThuong2MMobile();
                                    }
                                    if (txtCheckChipBiMat.Checked)
                                    {
                                        System.Threading.Thread.Sleep(2000);
                                        pkSource.NhanQuaBiMat();
                                    }
                                    System.Threading.Thread.Sleep(2000);
                                    pkSource.GetInitMoney(false);
                                }
                                #endregion
                            }
                        });
                    tasks.Add(task);
                    task.Wait(3000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    System.Threading.Thread.Sleep(1000);
                }
                if (txtCheckWeb.Checked && txtCheckChipMayMan.Checked)
                {
                    foreach (var item in dicLinkOfPackage)
                    {
                        listLinkChipMayMan.Remove(item.Key);
                    }
                    foreach (string link in listLink)
                    {
                        listLinkChipMayMan.Add(link);
                    }
                    Utilities.SerializeObject("linkLuckyChip.obj", listLinkChipMayMan);
                }
                //Set Money After Check
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = dicPokers[selectedPackageID][iIndex];
                    PokerExData exData = pkSource.GetExData();
                    exData.money = pkSource.Money;
                    exData.earnmoney = pkSource.EarnToday;
                    pkSource.SetExData(exData);
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
                            MobileModermController.Disconnect();
                            System.Threading.Thread.Sleep(2000);
                            try
                            {
                                if (txtPackNo.SelectedIndex < txtPackNo.Items.Count - 1)
                                {
                                    txtPackNo.SelectedIndex = txtPackNo.SelectedIndex + 1;
                                    btnCheckWeb_Click(null, null);
                                }
                            }
                            catch { }
                        }
                    };
                    this.BeginInvoke(action);
                }
            }
        }

        private void ketBan2()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                FaceBookController fbController = new FaceBookController();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = dicPokers[selectedPackageID][iIndex];
                    int iStart = iIndex;
                    tasks.Add(
                    Task.Factory.StartNew(() =>
                    {
                        pkSource.LoginWebApp();
                        if (pkSource.bWebLogedIn)
                        {
                            pkSource.GetAllFriend();
                            //Send FriendRequest
                            for (int iSeed = iStart + 1; iSeed < gridData.Rows.Count; iSeed++)
                            {
                                if (this.IsDisposed) return;
                                PokerController pkDes = dicPokers[selectedPackageID][iSeed];
                                if (pkSource.ListFriend.Contains(pkDes.Models.PKID)) continue;
                                pkSource.SendFriendRequest(pkDes.Models);
                                System.Threading.Thread.Sleep(2000);
                            }
                        }
                    }));
                    System.Threading.Thread.Sleep(4000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
                //System.Threading.Thread.Sleep(5000);
                tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        PokerController pkSource = dicPokers[selectedPackageID][iIndex];
                        pkSource.AcceptFriendRequest();
                    }));
                    System.Threading.Thread.Sleep(1000);
                }

                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch { }
        }

        private void AnKimCuongVaNhanThuongLinhMoi()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                FaceBookController fbController = new FaceBookController();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    PokerController pkSource = dicPokers[selectedPackageID][iIndex];
                    int iStart = iIndex;
                    tasks.Add(
                    Task.Factory.StartNew(() =>
                    {
                        pkSource.LoginWebApp();
                        if (pkSource.bWebLogedIn)
                        {
                            pkSource.AnKimCuongVaNhanThuongLinhMoi();
                        }
                    }));
                    tasks[tasks.Count - 1].Wait();
                    System.Threading.Thread.Sleep(4000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch { }
        }

        private void reloadGrid()
        {
            try
            {
                if (txtPackNo.SelectedValue is long)
                {
                    selectedPackageID = (long)txtPackNo.SelectedValue;
                    BindingList<PokerController> listController = new BindingList<PokerController>();
                    if (dicPokers.ContainsKey(selectedPackageID))
                    {
                        foreach (var pk in dicPokers[selectedPackageID])
                        {
                            if (pk != null && pk.Models != null && pk.Models.FaceBook != null && pk.Models.PackageID == selectedPackageID)
                            {
                                listController.Add(pk);
                            }
                        }
                    }
                    else
                    {
                        Package package = Global.DBContext.Package.Where(x => x.ID == selectedPackageID).FirstOrDefault();
                        if (package != null)
                        {
                            foreach (Poker poker in package.Pokers)
                            {
                                PokerController newPokerController = new PokerController { Models = poker, Status = "Khởi tạo thành công" };
                                PokerExData exData = newPokerController.GetExData();
                                if (exData.money.HasValue) newPokerController.Money = exData.money.Value;
                                newPokerController.GridContainer = gridData;
                                if (AppSettings.GetMoneyOnLoad == "1")
                                {
                                    newPokerController.GetInitMoney(false);
                                }
                                listController.Add(newPokerController);
                            }
                            dicPokers.Add(selectedPackageID, listController);
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
                        var exData = pkController.GetExData();
                        gridData[TableFaceBookConst.Login, iIndex].Value = pkController.Models.FaceBook.Login;
                        gridData[GridMainFormConst.STT, iIndex].Value = iIndex + 1;
                    }
                }
                else
                {
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void getDataOnload()
        {
            try
            {
                //Load Package
                var listPackage = (from pg in Global.DBContext.Package
                                   join pk in Global.DBContext.Poker on pg.ID equals pk.PackageID
                                   select new { pg.ID }).Distinct().OrderBy(x => x.ID).ToList();
                BindingSource bindingPackage = new BindingSource { DataSource = listPackage };
                bEnableSelectedValueChange = false;
                txtPackNo.DisplayMember = TablePackageConst.ID;
                txtPackNo.ValueMember = TablePackageConst.ID;
                bEnableSelectedValueChange = true;
                txtPackNo.DataSource = bindingPackage;
                bEnableSelectedValueChange = true;
                if (txtPackNo.SelectedValue is long && (long)txtPackNo.SelectedValue != 0 && (long)txtPackNo.SelectedValue != 1)
                {
                    txtPackNo.SelectedValue = 1;
                }
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
        }
        #endregion
    }
}
