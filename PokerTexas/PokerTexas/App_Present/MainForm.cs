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

namespace PokerTexas.App_Present
{
    public partial class MainForm : Form
    {
        #region - DECLARE -
        Dictionary<long, BindingList<PokerController>> dicPokers = new Dictionary<long, BindingList<PokerController>>();
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
            getDataOnload();
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

        private void btnXoaTaiKhoan_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn muốn xóa Tài Khoản được chọn?", "Poker", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    PokerController pkController = gridData.Rows[gridData.CurrentCell.RowIndex].DataBoundItem as PokerController;
                    Package pack = txtPackNo.SelectedItem as Package;

                    Global.DBContext.Poker.Remove(pkController.Models);
                    Global.DBContext.SaveChanges();
                    dicPokers[pack.ID].Remove(pkController);
                    reloadGrid();
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
                btnKetBan.Enabled = false;
                Task taskKetBan = new Task(ketBan);
                taskKetBan.Wait(1000);
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
                btnNhanThuongHangNgay.Enabled = false;
                Task.Factory.StartNew(nhanThuongHangNgay);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region - METHOD -
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
                    pkSource.NhanThuongHangNgayMobile();
                    //tasks.Add(Task.Factory.StartNew(pkSource.NhanThuongHangNgayMobile));
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
                        listController.Add(new PokerController { Models = poker, Status = "Khởi tạo thành công" });
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

                //DataGridViewTextBoxColumn colPackageNo = new DataGridViewTextBoxColumn();
                //colPackageNo.Name = TablePackageConst.Pack;
                //colPackageNo.HeaderText = TablePackageConst.Pack;
                //colPackageNo.Width = 50;
                //colPackageNo.SortMode = DataGridViewColumnSortMode.NotSortable;
                //colPackageNo.DisplayIndex = 4;
                //gridData.Columns.Insert(2, colPackageNo);

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
