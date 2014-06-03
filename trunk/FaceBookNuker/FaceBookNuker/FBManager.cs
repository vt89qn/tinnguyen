using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FaceBookNuker.Models;
using FaceBookNuker.Controller;

namespace FaceBookNuker
{
    public partial class FBManager : Form
    {
        #region - DECLARE -
        Session workingSession = null;
        #endregion
        public FBManager()
        {
            InitializeComponent();
            this.Load += new EventHandler(FBManager_Load);
            this.btnPostStatus.Click += (objs, obje) =>
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(postStatus)).Start();
                //postStatus();
            };
            this.btnCheckProblemAccount.Click += (objs, obje) =>
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(checkProblemAccount)).Start();
            };
            this.btnImportFacebook.Click += new EventHandler(btnImportFacebook_Click);
            this.btnRegNew.Click += new EventHandler(btnRegNew_Click);
            this.btnFeedMail.Click += new EventHandler(btnFeedMail_Click);
            this.btnNewSession.Click += new EventHandler(btnNewSession_Click);

        }

        void FBManager_Load(object sender, EventArgs e)
        {
            loadWorkingSession();
            loadAllData();
            //this.ShowInTaskbar = false;
        }

        void btnNewSession_Click(object sender, EventArgs e)
        {
            workingSession = new Session();
            workingSession.Date = DateTime.Now;
            DataProvider.DB.Session.Add(workingSession);
            DataProvider.DB.SaveChanges();

            this.txtSession.Text = workingSession.ID.ToString();
            this.txtDateStart.Text = workingSession.Date == null ? string.Empty : ((DateTime)workingSession.Date).ToString("yyyy-MM-dd");
        }

        void btnFeedMail_Click(object sender, EventArgs e)
        {
            FaceBookController controller = new FaceBookController();
            controller.FeedEmail();
        }
        private void btnImportFacebook_Click(object sender, EventArgs e)
        {
            string strAccount = Clipboard.GetText();
            if (string.IsNullOrEmpty(strAccount))
            {
                TMessage.ShowInfomation("Nothing in clipboad");
                return;
            }
            if (TMessage.ShowQuestion("Are you sure to import there account", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel) return;

            foreach (string strLogin in strAccount.Split('\n'))
            {
                if (string.IsNullOrEmpty(strLogin.Trim())) continue;
                FaceBookController fb = new FaceBookController();
                fb.Models = new FaceBook();
                fb.Models.Login = strLogin.Split('|')[0].Trim();
                fb.Models.Pass = "Cuongpro123";//"random8&^";
                Clipboard.SetText(fb.Models.Login);
                if (fb.CheckLogin())
                {
                    var existFB = DataProvider.DB.FaceBook.Where(m => m.UserID == fb.Models.UserID).ToList();
                    if (existFB != null && existFB.Count == 0)
                    {
                        fb.Models.Cookies = Serializer.ConvertObjectToBlob(fb.WebClient.CookieContainer);
                        fb.Models.Status = 1;
                        DataProvider.DB.FaceBook.Add(fb.Models);
                        DataProvider.DB.SaveChanges();
                    }
                }
                else
                {
                    if (fb.WebClient.Error != null)
                    { }
                    else if (fb.WebClient.ResponseText.Contains("Email báº¡n Ä‘Ă£ nháº­p khĂ´ng thuá»™c báº¥t ká»³ tĂ i khoáº£n nĂ o.")) { }
                    else if (fb.WebClient.ResponseText.Contains("If this account reflects your real name and personal information, please help us verify it"))
                    {

                    }
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {

        }

        private void btnRegNew_Click(object sender, EventArgs e)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(regFaceBookAccounts)).Start();

        }

        #region - METHOD -
        private void regFaceBookAccounts()
        {
            try
            {
                while (true)
                {
                    string strNewName = Utilities.GenerationNewName(false);
                    if (!string.IsNullOrEmpty(strNewName))
                    {
                        //dang ky Email
                        string strNewEmail = Utilities.ConvertToUnSign3(strNewName.Replace(" ", string.Empty)).ToLower().Trim();

                        //Dang Ky Tai Khoan
                        FaceBookController fb = new FaceBookController();
                        fb.RegNewAccount(strNewName, strNewEmail + "@tinphuong.me");
                    }
                    System.Threading.Thread.Sleep(300000);
                }
            }
            catch
            {
            }
            finally { }
        }

        private void loadAllData()
        {
            try
            {
                //var dtFB = DataProvider.Provider.GetData("SELECT * FROM M_FaceBook", "");
                //if (dtFB != null && dtFB.Rows.Count > 0)
                //{
                //    foreach (DataRow rowFB in dtFB.Rows)
                //    {
                //        FaceBook model = new FaceBook();
                //        model.Cookies = (byte[])rowFB["Cookie"];
                //        model.Login = rowFB["Login"].ToString();
                //        model.Pass = rowFB["Pass"].ToString();
                //        model.Status = 1;
                //        model.UserID = rowFB["UserID"].ToString();
                //        db.FaceBook.Add(model);
                //    }
                //}
                //db.SaveChanges();
                //var listInactive = (from fb in DataProvider.DB.FaceBook
                //                    join post in DataProvider.DB.PostStatus on fb.ID equals post.FaceBookID into fbpost
                //                    from fp in fbpost.DefaultIfEmpty()
                //                    where fp.FaceBookID == null
                //                    select fb).ToList();
                //if (listInactive.Count > 0)
                //{
                //    listInactive.ForEach(m => m.Status = 2);
                //    DataProvider.DB.SaveChanges();
                //}

                List<FaceBook> listFB = (from m in DataProvider.DB.FaceBook
                                         select m).ToList();

                if (gridData.Columns.Count == 0)
                {
                    gridData.AutoGenerateColumns = false;
                    DataGridViewTextBoxColumn colStt = new DataGridViewTextBoxColumn();
                    colStt.Name = GridConst.STT;
                    colStt.HeaderText = GridConst.STT;
                    colStt.Width = 50;
                    colStt.SortMode = DataGridViewColumnSortMode.NotSortable;
                    gridData.Columns.Add(colStt);

                    DataGridViewTextBoxColumn colAccount = new DataGridViewTextBoxColumn();
                    colAccount.Name = GridConst.Login;
                    colAccount.HeaderText = GridConst.Login;
                    colAccount.Width = 200;
                    colAccount.DataPropertyName = GridConst.Login;
                    colAccount.SortMode = DataGridViewColumnSortMode.NotSortable;
                    gridData.Columns.Add(colAccount);

                    DataGridViewTextBoxColumn colStatus = new DataGridViewTextBoxColumn();
                    colStatus.Name = GridConst.GridStatus;
                    colStatus.HeaderText = GridConst.GridStatus;
                    colStatus.Width = 500;
                    colStatus.SortMode = DataGridViewColumnSortMode.NotSortable;
                    gridData.Columns.Add(colStatus);

                    DataGridViewTextBoxColumn colController = new DataGridViewTextBoxColumn();
                    colController.Name = GridConst.Controller;
                    colController.Visible = false;
                    gridData.Columns.Add(colController);

                }
                BindingSource bs = new BindingSource();
                bs.DataSource = listFB;
                gridData.DataSource = bs;

                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    gridData[GridConst.STT, iIndex].Value = iIndex + 1;
                    FaceBookController fbController = new FaceBookController
                    {
                        Models = gridData.Rows[iIndex].DataBoundItem as FaceBook
                        ,
                        SessionID = workingSession.ID
                        ,
                        GridIndex = iIndex
                    };
                    gridData[GridConst.Controller, iIndex].Value = fbController;
                    gridData.Rows[iIndex].Tag = fbController;
                    fbController.StatusChanged += (objs, obje) =>
                    {
                        gridData.Invoke((Action)(() =>
                        {
                            gridData[GridConst.GridStatus, (objs as FaceBookController).GridIndex].Value = obje.Status;
                        }));
                    };
                }
            }
            catch (Exception ex)
            {
                TMessage.ShowException(ex);
            }
        }

        private void loadWorkingSession()
        {
            try
            {
                var ss = DataProvider.DB.Session.OrderByDescending(mbox => mbox.ID).Take(1).ToList();
                if (ss.Count > 0)
                {
                    workingSession = ss[0];
                }
                else
                {
                    workingSession = new Session();
                    workingSession.Date = DateTime.Now;
                    DataProvider.DB.Session.Add(workingSession);
                    DataProvider.DB.SaveChanges();
                }
                this.txtSession.Text = workingSession.ID.ToString();
                this.txtDateStart.Text = workingSession.Date == null ? string.Empty : ((DateTime)workingSession.Date).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                TMessage.ShowException(ex);
            }
        }

        private void postStatus()
        {
            try
            {
                //btnPostStatus.Enabled = false;
                int iCountDo = 0;
                while (iCountDo < gridData.Rows.Count)
                {
                    iCountDo = 0;
                    int iIndex = 0;
                    for (; iIndex < gridData.Rows.Count; iIndex++)
                    {
                        FaceBookController fbController = gridData.Rows[iIndex].Tag as FaceBookController; //gridData[GridConst.Controller, iIndex].Value as FaceBookController;
                        //Check account die
                        FaceBook fb = gridData.Rows[iIndex].DataBoundItem as FaceBook;
                        if (fb.Status != null && fb.Status != 1)
                        {
                        }
                        else
                        {
                            fb = DataProvider.DB.FaceBook.Find(fb.ID);
                        }
                        if (fb != null && fb.Status != null && fb.Status != 1)
                        {
                            fbController.SetStatusChanged("Having problems with this account");
                            iCountDo++;
                            continue;
                        }
                        //Check not update status
                        var posted = DataProvider.DB.PostStatus.Where(m => m.FaceBookID == fb.ID && m.SessionID == workingSession.ID).ToList();
                        if (posted.Count > 0)
                        {
                            fbController.SetStatusChanged("This account has Posted Status in this Session");
                            iCountDo++;
                            continue;
                        }

                        //Check Still working
                        if (fbController.IsWorking)
                        {
                            continue;
                        }
                        //Post status
                        fbController.PostStatus();
                        if (!fbController.WorkingFail)
                        {
                            //Sleep 1 minutes
                            System.Threading.Thread.Sleep(120000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TMessage.ShowException(ex);
            }
            finally
            {
            }
        }

        private void checkProblemAccount()
        {
            try
            {
                List<FaceBook> listFBToDelete = new List<FaceBook>();
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    //Check account die
                    FaceBook fb = gridData.Rows[iIndex].DataBoundItem as FaceBook;
                    if (fb != null && fb.Status != null && fb.Status != 1)
                    {
                        //FaceBookController fbController = gridData[GridConst.Controller, iIndex].Value as FaceBookController;
                        //if (fbController.CheckLogin())
                        //{
                        //    fb.Cookies = Serializer.ConvertObjectToBlob(fbController.WebClient.CookieContainer);
                        //    fb.Status = 1;
                        //    DataProvider.DB.Entry(fb).State = System.Data.Entity.EntityState.Modified;
                        //    DataProvider.DB.SaveChanges();
                        //}
                        //else
                        //{
                        //    if (fbController.WebClient.Error != null)
                        //    {
                        //    }
                        //    else if (fbController.WebClient.ResponseText.Contains("If this account reflects your real name and personal information, please help us verify it"))
                        //    {
                        listFBToDelete.Add(fb);
                        //}
                        //}
                    }
                }
                if (listFBToDelete.Count > 0 && TMessage.ShowQuestion("Are you sure want to delete problem account", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    DataProvider.DB.FaceBook.RemoveRange(listFBToDelete);
                    DataProvider.DB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                TMessage.ShowException(ex);
            }
            finally
            {
            }
        }
        #endregion
    }
}
