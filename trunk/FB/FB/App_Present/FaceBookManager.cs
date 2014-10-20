using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FB.App_Common;
using FB.App_Model;
using System.Globalization;
using System.Threading.Tasks;
using FB.App_Controller;
using System.Collections;

namespace FB.App_Present
{
    public partial class FaceBookManager : Form
    {
        private bool isBusy = false;
        public FaceBookManager()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!isBusy)
            {
                if (keyData == Keys.F1)
                {
                    btnPostStatus_Click(null, null);
                    return true;
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
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnUploadProfilePhoto_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnUploadProfilePhoto.Enabled = false;
            Task.Factory.StartNew(() => uploadProfilePhoto(false));
        }

        private void btnUploadProfilePhotoAllPack_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnUploadProfilePhotoAllPack.Enabled = false;
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    uploadProfilePhoto(true);
                    bool bCanMoveNext = false;
                    MethodInvoker action = delegate
                    {
                        try
                        {
                            if (txtPackNo.SelectedIndex < txtPackNo.Items.Count - 1)
                            {
                                txtPackNo.SelectedIndex = txtPackNo.SelectedIndex + 1;
                                bCanMoveNext = true;
                            }
                        }
                        catch
                        {
                        }
                    };
                    this.BeginInvoke(action);
                    System.Threading.Thread.Sleep(1000);
                    if (!bCanMoveNext) break;
                }
            });
        }

        private void btnPostStatus_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnPostStatus.Enabled = false;
            Task.Factory.StartNew(() => postStatus(false));
        }

        private void menuCopyURL_Click(object sender, EventArgs e)
        {
            if (gridData.Rows.Count > 0)
            {
                FaceBook model = gridData.Rows[gridData.CurrentCell.RowIndex].DataBoundItem as FaceBook;
                FaceBookController fbController = new FaceBookController();
                string strURL = fbController.GetFaceBookLoginURL(model, "https://www.facebook.com/me");
                if (!string.IsNullOrEmpty(strURL))
                {
                    Clipboard.SetText(strURL);
                    //MessageBox.Show("Đã copy URL vào clipboad");
                }
            }
        }

        private void btnRegFBAccount_Click(object sender, EventArgs e)
        {
            if (btnRegFBAccount.Text.Contains("Start"))
            {
                btnRegFBAccount.Text = "Stop Reg Auto";
            }
            else
            {
                btnRegFBAccount.Text = "Start Reg Auto";
            }
            Task.Factory.StartNew(() =>
            {
                while (btnRegFBAccount.Text.Contains("Stop"))
                {
                    if (DateTime.Today >= new DateTime(2014, 11, 21))
                    {
                        return;
                    }
                    MobileModermController.Disconnect();
                    MobileModermController.Connect();
                    FaceBook fb = new FaceBookController().RegNewAccount();
                    if (fb != null)
                    {
                        FBPackage package = Global.DBContext.FBPackage.Where(m => m.FaceBooks.Count < 3).FirstOrDefault();
                        if (package == null)
                        {
                            long iMax = Global.DBContext.FBPackage.Max(m => m.ID);
                            package = new FBPackage { Pack = iMax + 1 };
                        }
                        fb.FBPackage = package;
                        Global.DBContext.FaceBook.Add(fb);
                        Global.DBContext.SaveChanges();
                        //var t = Global.DBContext.FaceBook.ToList();
                    }
                }
            });
        }

        private void btnConfirmEmail_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnConfirmEmail.Enabled = false;
            Task.Factory.StartNew(() => confirmEmail(false));
        }

        private void btnUpdateInfo_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnUpdateInfo.Enabled = false;
            Task.Factory.StartNew(() => updateInfo(false));
        }

        private void menuXoaTK_Click(object sender, EventArgs e)
        {

        }

        private void menuLoginAgain_Click(object sender, EventArgs e)
        {
            if (gridData.Rows.Count > 0)
            {
                FaceBook model = gridData.Rows[gridData.CurrentCell.RowIndex].DataBoundItem as FaceBook;
                FaceBookController fbController = new FaceBookController();
                if (fbController.LoginMobile(model))
                {
                    Global.DBContext.SaveChanges();
                    gridData[GridMainFormConst.Status, gridData.CurrentCell.RowIndex].Value = "Đăng nhập thành công";
                }
                else
                {
                    if (MessageBox.Show("Đăng nhập thất bại ! Xóa ?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        Global.DBContext.FaceBook.Remove(model);
                        Global.DBContext.SaveChanges();
                        reloadGrid();
                    }
                    else
                    {
                        gridData[GridMainFormConst.Status, gridData.CurrentCell.RowIndex].Value = "Đăng nhập thất bại";
                    }
                }
            }
        }

        private void btnPostStatusAllPack_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnPostStatusAllPack.Enabled = false;
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    postStatus(true);
                    bool bCanMoveNext = false;
                    MethodInvoker action = delegate
                    {
                        try
                        {
                            if (txtPackNo.SelectedIndex < txtPackNo.Items.Count - 1)
                            {
                                txtPackNo.SelectedIndex = txtPackNo.SelectedIndex + 1;
                                bCanMoveNext = true;
                            }
                        }
                        catch
                        {
                        }
                    };
                    this.BeginInvoke(action);
                    System.Threading.Thread.Sleep(1000);
                    if (!bCanMoveNext) break;


                }
            });
        }

        private void FaceBookManager_Load(object sender, EventArgs e)
        {
            getDataOnload();
        }

        private void txtPackNo_SelectedValueChanged(object sender, EventArgs e)
        {
            reloadGrid();
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

        private void btnKetBan_Click(object sender, EventArgs e)
        {
            try
            {
                if (isBusy) return;
                isBusy = true;
                btnKetBan.Enabled = false;
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        ketBan();
                        bool bCanMoveNext = false;
                        MethodInvoker action = delegate
                        {
                            try
                            {
                                if (txtPackNo.SelectedIndex < txtPackNo.Items.Count - 1)
                                {
                                    txtPackNo.SelectedIndex = txtPackNo.SelectedIndex + 1;
                                    bCanMoveNext = true;
                                }
                            }
                            catch
                            {
                            }
                        };
                        this.BeginInvoke(action);
                        System.Threading.Thread.Sleep(1000);
                        if (!bCanMoveNext) break;
                    }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region - METHOD -
        private void reloadGrid()
        {
            if (txtPackNo.SelectedItem is FBPackage)
            {
                FBPackage selectedPackage = txtPackNo.SelectedItem as FBPackage;
                gridData.DataSource = selectedPackage.FaceBooks;
                for (int iIndex = 0; iIndex < gridData.Columns.Count; iIndex++)
                {
                    gridData.Columns[iIndex].Visible = false;
                }
                gridData.Columns[TableFaceBookConst.Login].Visible = true;
                gridData.Columns[TableFaceBookConst.Login].Width = 200;
                gridData.Columns[TableFaceBookConst.Login].SortMode = DataGridViewColumnSortMode.NotSortable;

                DataGridViewTextBoxColumn colStt = new DataGridViewTextBoxColumn();
                colStt.Name = GridMainFormConst.STT;
                colStt.HeaderText = GridMainFormConst.STT;
                colStt.Width = 50;
                colStt.SortMode = DataGridViewColumnSortMode.NotSortable;
                gridData.Columns.Insert(0, colStt);

                DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
                colStatus.Name = GridMainFormConst.Status;
                colStatus.HeaderText = GridMainFormConst.Status;
                colStatus.Width = 300;
                colStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
                gridData.Columns.Insert(1, colStatus);

                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    FaceBook fb = gridData.Rows[iIndex].DataBoundItem as FaceBook;
                    gridData[GridMainFormConst.STT, iIndex].Value = iIndex + 1;
                }
            }
        }

        private void uploadProfilePhoto(bool bDoAll)
        {
            try
            {
                MobileModermController.Disconnect();
                MobileModermController.Connect();
                List<Task> tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    FaceBook model = gridData.Rows[iIndex].DataBoundItem as FaceBook;
                    //bool bPostOK = new FaceBookController().ChangeProfilePhoto(model);
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        int iIndexPost = iIndex;
                        bool bPostOK = new FaceBookController().ChangeProfilePhoto(model);
                        MethodInvoker action = delegate
                        {
                            if (bPostOK)
                            {
                                gridData[GridMainFormConst.Status, iIndexPost].Value = "Upload Profile Photo OK";
                            }
                            else
                            {
                                gridData[GridMainFormConst.Status, iIndexPost].Value = "Upload Profile Photo Fail";
                            }
                        };
                        this.BeginInvoke(action);

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
                        if (!bDoAll) btnUploadProfilePhoto.Enabled = true;
                    };
                    this.BeginInvoke(action);
                }
            }

        }

        private void postStatus(bool bDoAll)
        {
            try
            {
                MobileModermController.Disconnect();
                MobileModermController.Connect();
                List<Task> tasks = new List<Task>();
                List<FaceBook> listDelete = new List<FaceBook>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    FaceBook model = gridData.Rows[iIndex].DataBoundItem as FaceBook;
                    //new FaceBookController().PostStatus(model);
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        int iIndexPost = iIndex;
                        bool bPostOK = new FaceBookController().PostStatus(model);
                        MethodInvoker action = delegate
                        {
                            if (bPostOK)
                            {
                                gridData[GridMainFormConst.Status, iIndexPost].Value = "Post OK";
                            }
                            else
                            {
                                gridData[GridMainFormConst.Status, iIndexPost].Value = "Post Fail";
                                // Global.DBContext.FaceBook.Remove(model);
                                listDelete.Add(model);
                            }
                        };
                        this.BeginInvoke(action);

                    }));
                    System.Threading.Thread.Sleep(1000);
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
                if (listDelete.Count > 0)
                {
                    listDelete.ForEach(m => Global.DBContext.FaceBook.Remove(m));
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
                        if (!bDoAll)
                        {
                            btnPostStatus.Enabled = true;
                            //reloadGrid();
                        }
                    };
                    this.BeginInvoke(action);
                }
            }

        }

        private void confirmEmail(bool bDoAll)
        {
            try
            {
                MobileModermController.Disconnect();
                MobileModermController.Connect();
                List<Task> tasks = new List<Task>();
                List<FaceBook> listConfirm = new List<FaceBook>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    FaceBook model = gridData.Rows[iIndex].DataBoundItem as FaceBook;
                    //new FaceBookController().PostStatus(model);
                    Task task = Task.Factory.StartNew(() =>
                    {
                        int iIndexPost = iIndex;
                        bool bPostOK = new FaceBookController().ConfirmEmail(model);
                        MethodInvoker action = delegate
                        {
                            if (bPostOK)
                            {
                                gridData[GridMainFormConst.Status, iIndexPost].Value = "Confirm OK";
                                listConfirm.Add(model);
                            }
                            else
                            {
                                gridData[GridMainFormConst.Status, iIndexPost].Value = "Confirm Fail";
                            }
                        };
                        this.BeginInvoke(action);

                    });
                    task.Wait();
                }
                while (tasks.Any(t => !t.IsCompleted))
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                }
                if (listConfirm.Count > 0)
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
                        if (!bDoAll)
                        {
                            btnConfirmEmail.Enabled = true;
                            //reloadGrid();
                        }
                    };
                    this.BeginInvoke(action);
                }
            }

        }

        private void updateInfo(bool bDoAll)
        {
            try
            {
                MobileModermController.Disconnect();
                MobileModermController.Connect();
                List<Task> tasks = new List<Task>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    FaceBook model = gridData.Rows[iIndex].DataBoundItem as FaceBook;
                    //new FaceBookController().PostStatus(model);
                    Task task = Task.Factory.StartNew(() =>
                    {
                        int iIndexPost = iIndex;
                        bool bPostOK = new FaceBookController().UpdateProfileInfo(model);
                        MethodInvoker action = delegate
                        {
                            if (bPostOK)
                            {
                                gridData[GridMainFormConst.Status, iIndexPost].Value = "Update Info OK";
                            }
                            else
                            {
                                gridData[GridMainFormConst.Status, iIndexPost].Value = "Update Info Fail";
                            }
                        };
                        this.BeginInvoke(action);

                    });
                    task.Wait();
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
                        if (!bDoAll)
                        {
                            btnConfirmEmail.Enabled = true;
                            //reloadGrid();
                        }
                    };
                    this.BeginInvoke(action);
                }
            }

        }
        private void getDataOnload()
        {
            try
            {
                //Load Package
                List<FBPackage> listPackage = Global.DBContext.FBPackage.ToList();
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

        private void ketBan()
        {
            try
            {
                MobileModermController.Disconnect();
                MobileModermController.Connect();
                FaceBookController fbController = new FaceBookController();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return;
                    FaceBook model = gridData.Rows[iIndex].DataBoundItem as FaceBook;
                    //Send FriendRequest
                    for (int iSeed = iIndex + 1; iSeed < gridData.Rows.Count; iSeed++)
                    {
                        if (this.IsDisposed) return;
                        FaceBook modelDes = gridData.Rows[iSeed].DataBoundItem as FaceBook;
                        if (fbController.SendFriendRequest(model, modelDes))
                        {
                            gridData[GridMainFormConst.Status, iIndex].Value = "Gửi kết bạn thành công tới " + modelDes.Login;
                        }
                        else
                        {
                            gridData[GridMainFormConst.Status, iIndex].Value = "Gửi kết bạn KHÔNG thành công tới " + modelDes.Login;
                        }
                        System.Threading.Thread.Sleep(2000);
                    }
                    //Accept Friend Request
                    for (int iSeed = iIndex - 1; iSeed >= 0; iSeed--)
                    {
                        if (this.IsDisposed) return;
                        FaceBook modelDes = gridData.Rows[iSeed].DataBoundItem as FaceBook;
                        if (fbController.AcceptFriendRequest(model, modelDes))
                        {
                            gridData[GridMainFormConst.Status, iIndex].Value = "Chấp nhận bạn thành công từ " + modelDes.Login;
                        }
                        else
                        {
                            gridData[GridMainFormConst.Status, iIndex].Value = "Chấp nhận bạn KHÔNG thành công từ " + modelDes.Login;
                        }
                        System.Threading.Thread.Sleep(2000);
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
        #endregion


    }
}
