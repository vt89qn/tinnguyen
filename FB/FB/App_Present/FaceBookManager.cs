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
using System.Diagnostics;
using System.Web.Script.Serialization;
using FB.App_UserControl;
using System.Text.RegularExpressions;

namespace FB.App_Present
{
    public partial class FaceBookManager : Form
    {
        private bool isBusy = false;
        Timer t = new Timer();
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

        private void btnPostStatus_Click(object sender, EventArgs e)
        {
            if (isBusy) return;
            isBusy = true;
            btnPostStatus.Enabled = false;
            bool bFail = false;
            Task task = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (!bFail)
                    {
                        //MobileModermController.Disconnect();
                        //MobileModermController.Connect();
                    }
                    bFail = !checkFaceBook();
                    if (txtAutoMoveToNextPack.Checked)
                    {
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
                        System.Threading.Thread.Sleep(500);
                        if (!bCanMoveNext) break;
                    }
                    else break;
                }
            });
            isBusy = false;
            btnPostStatus.Enabled = true;
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
        private void menuLoginAgainAll_Click(object sender, EventArgs e)
        {
            for (int iINdex = 0; iINdex < gridData.Rows.Count; iINdex++)
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    FaceBook model = gridData.Rows[iINdex].DataBoundItem as FaceBook;
                    FaceBookController fbController = new FaceBookController();
                    if (fbController.LoginMobile(model))
                    {
                        //gridData[GridMainFormConst.Status, iINdex].Value = "Đăng nhập thành công";
                    }
                    else
                    {
                        model.FBPackageID = 1;
                    }
                });
                task.Wait();
            }
            Global.DBContext.SaveChanges();
        }

        private void menuCreatePageAll_Click(object sender, EventArgs e)
        {

        }

        private void menuFeedPageAccessTokenAll_Click(object sender, EventArgs e)
        {

        }

        private void menuUpdatePhotoAndCoverAll_Click(object sender, EventArgs e)
        {
            bool bFind = true;
            while (bFind)
            {
                bFind = false;
                for (int iINdex = 0; iINdex < gridData.Rows.Count; iINdex++)
                {
                    Task task = Task.Factory.StartNew(() =>
                    {
                        FaceBookController fbController = new FaceBookController();
                        FaceBook model = gridData.Rows[iINdex].DataBoundItem as FaceBook;
                        Page page = model.Pages.Where(x => (x.PageData == null || !x.PageData.Contains("\"LUPP\":\"\\/Date("))).FirstOrDefault();
                        if (page != null)
                        {
                            fbController.UpdatePhotoAndCover(page);
                            bFind = true;
                        }
                    });
                    task.Wait();
                    if (Global.DBContext.ChangeTracker.HasChanges())
                    {
                        Global.DBContext.SaveChanges();
                    }
                }
            }
            //for (int iINdex = 0; iINdex < gridData.Rows.Count; iINdex++)
            //{
            //    Task task = Task.Factory.StartNew(() =>
            //    {
            //        FaceBookController fbController = new FaceBookController();
            //        FaceBook model = gridData.Rows[iINdex].DataBoundItem as FaceBook;

            //        fbController.Like(model, "1420890824837381");
            //    });
            //    task.Wait();
            //}

        }

        private void copyPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridData.Rows.Count > 0)
            {
                FaceBook model = gridData.Rows[gridData.CurrentCell.RowIndex].DataBoundItem as FaceBook;
                Clipboard.SetText(model.Pass);
            }
        }

        private void createPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int iINdex = 0; iINdex < gridData.Rows.Count; iINdex++)
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    FaceBookController fbController = new FaceBookController();
                    FaceBook model = gridData.Rows[iINdex].DataBoundItem as FaceBook;
                    fbController.CreateNewPage(model);
                    if (Global.DBContext.ChangeTracker.HasChanges()) Global.DBContext.SaveChanges();
                });
                System.Threading.Thread.Sleep(5000);
            }
        }

        private void feedAccessTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int iINdex = 0; iINdex < gridData.Rows.Count; iINdex++)
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    FaceBookController fbController = new FaceBookController();
                    FaceBook model = gridData.Rows[iINdex].DataBoundItem as FaceBook;
                    fbController.FeedAccessToken(model);
                });
                task.Wait();
            }
        }

        private void uploadPhotoAndCoiverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                FaceBookController fbController = new FaceBookController();
                FaceBook model = gridData.Rows[gridData.SelectedCells[0].RowIndex].DataBoundItem as FaceBook;
                fbController.UpdatePhotoAndCover(model);
            });
            task.Wait();
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

        private void menuCheckLive_Click(object sender, EventArgs e)
        {
            if (gridData.Rows.Count > 0)
            {
                FaceBook model = gridData.Rows[gridData.CurrentCell.RowIndex].DataBoundItem as FaceBook;
                FaceBookController fbController = new FaceBookController();
                if (fbController.checkFaceBook(model))
                {
                    //Global.DBContext.SaveChanges();
                    gridData[GridMainFormConst.Status, gridData.CurrentCell.RowIndex].Value = "Live";
                }
                else
                {
                    gridData[GridMainFormConst.Status, gridData.CurrentCell.RowIndex].Value = "Die";
                }
            }
        }

        private void menuCheckLiveAll_Click(object sender, EventArgs e)
        {
            for (int iINdex = 0; iINdex < gridData.Rows.Count; iINdex++)
            {
                Task task = Task.Factory.StartNew(() =>
                {
                    FaceBook model = gridData.Rows[iINdex].DataBoundItem as FaceBook;
                    FaceBookController fbController = new FaceBookController();
                    if (fbController.checkFaceBook(model))
                    {
                        //Global.DBContext.SaveChanges();
                        gridData[GridMainFormConst.Status, iINdex].Value = "Live";
                    }
                    else
                    {
                        gridData[GridMainFormConst.Status, iINdex].Value = "Die";
                    }
                });
                task.Wait();
            }
        }

        private void menuReNameToUS_Click(object sender, EventArgs e)
        {
            if (gridData.Rows.Count > 0)
            {
                FaceBook model = gridData.Rows[gridData.CurrentCell.RowIndex].DataBoundItem as FaceBook;
                FaceBookController fbController = new FaceBookController();
                fbController.RenameToUS(model);
                if (Global.DBContext.ChangeTracker.HasChanges())
                {
                    Global.DBContext.SaveChanges();
                }
            }
        }

        private void menuReNameToUSAll_Click(object sender, EventArgs e)
        {
            for (int iINdex = 0; iINdex < gridData.Rows.Count; iINdex++)
            {
                FaceBook model = gridData.Rows[iINdex].DataBoundItem as FaceBook;
                FaceBookController fbController = new FaceBookController();
                fbController.RenameToUS(model);
                if (Global.DBContext.ChangeTracker.HasChanges())
                {
                    Global.DBContext.SaveChanges();
                }
            }
        }

        private void menuXoaTK_Click(object sender, EventArgs e)
        {

        }

        private void btnRegFBAccount_Click(object sender, EventArgs e)
        {
            btnRegFBAccount.Enabled = false;
            if (btnRegFBAccount.Text.Contains("Start"))
            {
                btnRegFBAccount.Text = "Stop Reg Auto";
            }
            else
            {
                btnRegFBAccount.Text = "Start Reg Auto";
            }
            //Task.Factory.StartNew(() =>
            {
                while (btnRegFBAccount.Text.Contains("Stop"))
                {
                    Debug.WriteLine("Begin");
                    MobileModermController.Disconnect();
                    MobileModermController.Connect();
                    Debug.WriteLine("Start Reg");
                    FaceBookController controller = new FaceBookController();
                    FaceBook fb = controller.RegNewAccount();
                    Debug.WriteLine("End Reg");
                    if (fb != null)
                    {
                        Debug.WriteLine("Begin get Package");
                        FBPackage package = Global.DBContext.FBPackage.Where(m => m.FaceBooks.Count < 3).FirstOrDefault();
                        Debug.WriteLine("End get Package");
                        if (package == null)
                        {
                            long iMax = Global.DBContext.FBPackage.Max(m => m.ID);
                            package = new FBPackage { Pack = iMax + 1 };
                        }
                        fb.FBPackage = package;
                        Debug.WriteLine("Begin save");
                        Global.DBContext.FaceBook.Add(fb);
                        Global.DBContext.SaveChanges();
                        Debug.WriteLine("End save");
                        //var t = Global.DBContext.FaceBook.ToList();
                    }
                    controller.Dispose();
                }
                btnRegFBAccount.Enabled = true;
            }
            //);
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
            //isBusy = true;
            btnUpdateInfo.Enabled = false;
            Task.Factory.StartNew(() => updateInfo(false));
        }



        private void FaceBookManager_Load(object sender, EventArgs e)
        {
            //string str = @"";
            //foreach(string strLogin in str.Split('\n'))
            //{
            //FaceBookController controller = new FaceBookController();
            //FaceBook model = new FaceBook();
            //model.Login = strLogin.Trim();
            //model.Pass = "random8&^";//"S12365hh*";

            //    if (controller.LoginMobile(model))
            //    {
            //        model.FBPackageID = 3;
            //        Global.DBContext.FaceBook.Add(model);
            //        Global.DBContext.SaveChanges();
            //    }

            //}
            //t.Enabled = false;
            //t.Interval = 2 * 60 * 1000;
            ////  t.Tick += new EventHandler(t_Tick);
            //t.Enabled = true;
            // t.Start();
            //var facebooks = Global.DBContext.FaceBook.Where(x => x.FBPackageID == 2).ToList();
            //FaceBookController cl = new FaceBookController();

            //for (int iIndex = 14; iIndex < 30; iIndex++)
            //{
            //    cl.Like(facebooks[iIndex], "1507272666189235");
            //    System.Threading.Thread.Sleep(2000);
            //}

            //var pages = getPages(2);
            //FaceBookController cl = new FaceBookController();
            //List<string> listPost = new List<string>();

            //listPost.Add("331482693689624");
            //listPost.Add("331841126987114");
            //listPost.Add("334201940084366");
            //listPost.Add("334503823387511");
            //listPost.Add("336957949808765");
            //listPost.Add("338848622953031");
            //listPost.Add("340395706131656");
            //listPost.Add("342703079234252");
            //listPost.Add("343920365779190");
            //listPost.Add("346062188898341");
            //listPost.Add("348187828685777");
            //listPost.Add("349921741845719");
            //listPost.Add("351593658345194");

            //int iStart = 3200;
            //while (listPost.Count > 0)
            //{
            //    string strDoWork = listPost[0];
            //    listPost.RemoveAt(0);
            //    int iEnd = iStart + (new Random().Next(80, 140));
            //    for (int iIndex = iStart; iIndex <= iEnd; iIndex++)
            //    {
            //        cl.Like(pages[iIndex].AccessToken, strDoWork);
            //        int iRD = new Random().Next(0, 2);
            //        if (iRD == 1)
            //        {
            //            System.Threading.Thread.Sleep(1000);
            //            cl.Share(pages[iIndex], strDoWork, "");
            //        }
            //        System.Threading.Thread.Sleep(1000);
            //    }
            //    iStart = iEnd + 1;
            //}


            //WebClientEx client = new WebClientEx();
            //List<string> name = new List<string>();
            //for (int iPage = 1; iPage <= 19; iPage++)
            //{
            //    client.DoGet("http://babynames.net/list/last-names-for-first-names-1?page=" + iPage);
            //    MatchCollection mc = Regex.Matches(client.ResponseText, "result-name[^>]+>(?<val>[^<]+)");
            //    foreach (Match m in mc)
            //    {
            //        name.Add(m.Groups["val"].Value.Trim());
            //    }
            //}
            //string strName = string.Join(",", name.ToArray());

            getDataOnload();
        }

        private List<Page> getPages(int iPackage)
        {
            var pages = Global.DBContext.Page.Where(x => x.FaceBook.FBPackageID == iPackage).ToList();
            return pages;
        }

        void t_Tick(object sender, EventArgs e)
        {
            createPageToolStripMenuItem_Click(null, null);
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
                gridData.DataSource = null;
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
                    if (gridData.Rows[iIndex].DataBoundItem is FaceBook)
                    {
                        FaceBook fb = gridData.Rows[iIndex].DataBoundItem as FaceBook;
                        gridData[GridMainFormConst.STT, iIndex].Value = iIndex + 1;
                    }
                }
            }
        }

        private bool checkFaceBook()
        {
            bool bOK = false;
            try
            {
                List<Task> tasks = new List<Task>();
                List<FaceBook> listDelete = new List<FaceBook>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    if (this.IsDisposed) return false;
                    FaceBook model = gridData.Rows[iIndex].DataBoundItem as FaceBook;
                    Task task = Task.Factory.StartNew(() =>
                    {
                        int iIndexPost = iIndex;
                        bool bPostOK = new FaceBookController().checkFaceBook(model);
                        if (!bPostOK)
                        {
                            listDelete.Add(model);
                        }
                        else bOK = true;
                        MethodInvoker action = delegate
                        {
                            if (bPostOK)
                            {
                                try
                                {
                                    gridData[GridMainFormConst.Status, iIndexPost].Value = "Live";
                                }
                                catch { }
                            }
                            else
                            {
                                try
                                {
                                    gridData[GridMainFormConst.Status, iIndexPost].Value = "Die";
                                }
                                catch { }

                            }
                        };
                        this.BeginInvoke(action);
                    });
                    task.Wait();
                }
                if (listDelete.Count > 0)
                {
                    listDelete.ForEach(m => m.FBPackageID = 1);
                    Global.DBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return bOK;
        }

        private void confirmEmail(bool bDoAll)
        {
            try
            {
                //MobileModermController.Disconnect();
                //MobileModermController.Connect();
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
                                try
                                {
                                    gridData[GridMainFormConst.Status, iIndexPost].Value = "Confirm OK";
                                }
                                catch { }
                                listConfirm.Add(model);
                            }
                            else
                            {
                                try
                                {
                                    gridData[GridMainFormConst.Status, iIndexPost].Value = "Confirm Fail";
                                }
                                catch { }
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
                while (true)
                {
                    //MobileModermController.Disconnect();
                    //MobileModermController.Connect();

                    MethodInvoker action = delegate
                    {
                        btnPostStatus_Click(null, null);
                    };
                    this.BeginInvoke(action);
                    System.Threading.Thread.Sleep(1000);
                    while (isBusy) System.Threading.Thread.Sleep(1000);

                    //action = delegate
                    //{
                    //    btnUploadProfilePhoto_Click(null, null);
                    //};
                    //this.BeginInvoke(action);
                    //System.Threading.Thread.Sleep(1000);
                    //while (isBusy) System.Threading.Thread.Sleep(1000);

                    //action = delegate
                    //{
                    //    btnConfirmEmail_Click(null, null);
                    //};
                    //this.BeginInvoke(action);
                    //System.Threading.Thread.Sleep(1000);
                    //while (isBusy) System.Threading.Thread.Sleep(1000);

                    bool bCanMoveNext = false;
                    action = delegate
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
                //List<Task> tasks = new List<Task>();
                //for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                //{
                //    if (this.IsDisposed) return;
                //    FaceBook model = gridData.Rows[iIndex].DataBoundItem as FaceBook;
                //    //new FaceBookController().PostStatus(model);
                //    Task task = Task.Factory.StartNew(() =>
                //    {
                //        int iIndexPost = iIndex;
                //        bool bPostOK = new FaceBookController().UpdateProfileInfo(model);
                //        MethodInvoker action = delegate
                //        {
                //            if (bPostOK)
                //            {
                //                gridData[GridMainFormConst.Status, iIndexPost].Value = "Update Info OK";
                //            }
                //            else
                //            {
                //                gridData[GridMainFormConst.Status, iIndexPost].Value = "Update Info Fail";
                //            }
                //        };
                //        this.BeginInvoke(action);

                //    });
                //    task.Wait();
                //}
                //while (tasks.Any(t => !t.IsCompleted))
                //{
                //    Application.DoEvents();
                //    System.Threading.Thread.Sleep(1000);
                //}
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
                List<FBPackage> listPackage = Global.DBContext.FBPackage.Where(x => x.ID != 10 && x.FaceBooks.Count > 0).ToList();
                BindingSource bindingPackage = new BindingSource { DataSource = listPackage };
                txtPackNo.DataSource = bindingPackage;
                txtPackNo.DisplayMember = TablePackageConst.Pack;
                txtPackNo.ValueMember = TablePackageConst.ID;
                //txtPackNo.SelectedValue = 2;
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

        private void gridData_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
