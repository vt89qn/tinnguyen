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
                if (!string.IsNullOrEmpty(AppSettings.Seft))
                {
                    int iTry = 0;
                    string t = @"
01868777745 - 21017541
01868988861 - akiravangyen
01868998748 - 12345678
01869285192 - 227799
01869917355 - duong12c5
01882559123 - nkonkinkak
01882566635 - 220398
01882566635 - 220398
01882713644 - anhhanall
01882753961 - loantran9x@
01882907880 - 132125
01883820668 - voyeuchonglun
01884590741 - 06011997
01885256009 - 01885256009
01885999160 - thuthu123
01886262160 - 0967862667
01886646959 - nhikum
01886666758 - yeuthuongxadaulam
01887084222 - 079824086
01887084222 - 079824086
01887302012 - mavuong
01889482398 - gameboycolor
0875297621 - 25451990
0902384457 - 06121973
0903059970 - DVLTnganhuyenh3
0903059970 - DVLTnganhuyenh3
090309xxxx@gmail.com - 01289715353
0904666231 - fuckcaibeep
0906731573 - thuyan123
0908128377 - ngoni558
0912671143 - 0912671143tu
0913064842 - ilovekatyperry299
0913405362 - anhanhanhqwe
0914695380 - 0936866363
0915439167 - vudinhphu
0917617357 - sailam1990
0918694689 - hlove38
0923012181 - 123456789
0924477380 - 041296
0924717611 - tuyen0 viet
0925037753 - 335566
0925077332 - nhan4122512
0925212742 - 0939144186nhuy
0925638441 - 0923333948
0926128893 - 0123456789
0926211484 - 27071978
0927739447 - hoahongden
0929287130 - nocare96
0932685859 - 01213738638
0934433924 - 01654853492
0934783424 - 215403154
0935037753 - 335566
0935329080 - bameyentien0933058232
0936817101 - hongchip
0937353844 - songnhunhungdoahoa
0938288464 - 01021997
0938563582 - 1235789 
0942571933 - saobang
0942718646 - 0942718646
0943050020 - ynhi301094
0946036300 - long love forever
0946232828 - huyki123
0946979299 - betuyet
0948477577 - buungockyanh
0949062069 - wencachyeu
0962013978 - 0962013978
0962037807 - bongyeu
0962676037 - tamdinh
0962730330 - honghanh96
0963076958 - 06052013
0963671355 - 01694448062
0963744429 - trungZxc
0964384440 - Xike1995
0964405237 - maiyeu
0964434255 - 0943075217
0965816675 - 20041995
0967077691 - 12303888
0967200398 - anhyeuem
0967267184 - vananh
0967471651 - 01685994667
0967651986 - huyenngoc
0968304701 - ht25051997
0968512172 - 6111994
0968996099 - 12101990
0972397830 - Pitorao1234
0973027829 - 01686508278
0973583692 - bichdang
0973631127 - 2851005
0973631127 - 2851995
0973762129 - 0973762129
0973917431 - Khanhlinh
0975269785 - 0562212889
0975970195 - duytrung
0976468244 - Nghean86
0976635529 - Kimhien
0976831409 - kq64455062kl
0977734575 - 0975577075
0978193557 - Gjmptw
0979505205 - 240684
0979673482 - 13091994
0979798796 - nguyenthohung
0982375690 - 51219997
0983080277 - 0987654321
0983337702  - minhdangkhoa
0983531795 - Heolun1510
0983741809 - mauyeuanh96
0984208747 - Chiquy123
0984585488 - 0984585488
0984645257 - cotrang
0985561437 - 261101
0986198142 - 0986198142
0987204505 - truong
0987367130 - ukthit0inghe0
0987675621 - 521194
0987904543 - Ngodoanhieu
0988033043 - minh135792468
0988080454 - 0974696424
0988653568 - 199791297
0988738514 - 05051988
0989662734 - money1999
0989839829 - maianhhai1234
4ewitu@gmail.com - baeyongjoon
60187666590 - 06111992
84123049787 - tuanh1010
Binhyennhe 1409@gmail.com - a2family
Dautranlinhdan@ymail.com - linhlinhdandan
Dohuynhngocthuong@gmail.com - 123459
Dung.kute.12345 - anhdung
Hoanganh.tramanh.9@facobook.com - godbye
Hoangnamlonghn@gmail.com - giangvo123
Katenguyen.3720 - laanhdo
Kendynguyen198@yahoo.com - lanquynh
Pemiin@yahoo.com.vn - anhnhudaovotrungkien123456789
Tanh433@ymail.com - 19981998
Thanhthuy20hp@yahoo.com.vn  - 10101984
Tien tjnh tu toj - Nhat 2013
Tinhyeudanhchopokemon@zing.vn - Bao123+
Tuyet Nguyen0907040240 - 240778
Vudoan.1428 - 151172
ady_pitri@yahoo.com - Adygokil
ai_thuong_toi_15@yahoo.com - anhnien
aihaychungo91@yahoo.com - tranvanphong17111991
amynguyen_84@yahoo.com.vn - hoangkimkimkhanh843502
anh995@gmail.com - lien123
anhdepzai12345_sanhdieu@yahoo.com.vn - thenguyentai
anhkhangdeptrai_thu2@yahoo.com - khanbganh
anhphuong01882132006
anhphuong710@gmail.com - 221204
babynguyenhue91@gmail.com - 01663686760";
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
                                PokerController pkController = new PokerController { Models = new Poker { FaceBook = fb, PackageID = 5 } };
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
                Task.Factory.StartNew(ketBan);
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
                if (!string.IsNullOrEmpty(AppSettings.Seft))
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
                            pkSource.LoginWebApp();
                            if (pkSource.bWebLogedIn)
                            {
                                if (txtCheckKyTen.Checked) pkSource.KyTen();
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
                    { btnCheckWeb.Enabled = true; };
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
                        System.Threading.Thread.Sleep(5000);
                        if (txtPackNo.SelectedIndex < txtPackNo.Items.Count - 1)
                        {
                            txtPackNo.SelectedIndex = txtPackNo.SelectedIndex + 1;
                            btnCheckMobile_Click(null, null);
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
            else
            {
                Package selectedPackage = txtPackNo.SelectedItem as Package;
                BindingList<PokerController> listController = new BindingList<PokerController>();
                foreach (Package p in Global.DBContext.Package.ToList())
                {
                    foreach (Poker poker in p.Pokers)
                    {
                        PokerController newPokerController = new PokerController { Models = poker, Status = "Khởi tạo thành công" };
                        newPokerController.GridContainer = gridData;
                        listController.Add(newPokerController);
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
