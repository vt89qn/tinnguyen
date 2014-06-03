using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FaceBookNuker.Controller;

namespace FaceBookNuker
{
    public partial class ChangePass : Form
    {
        #region - DECLARE -
        BackgroundWorker bgwk;
        int iWorkingIndex = 0;
        #endregion
        #region - CONTRUCTOR -
        public ChangePass()
        {
            InitializeComponent();
            this.Load += new EventHandler(ChangePass_Load);
        }
        #endregion
        #region - EVENT -
        void ChangePass_Load(object sender, EventArgs e)
        {
            try
            {
                this.btnSelectFile.Click += new EventHandler(btnSelectFile_Click);
                this.btnApplyCurrentPass.Click += new EventHandler(btnApplyPass_Click);
                this.btnApplyNewPass.Click += new EventHandler(btnApplyPass_Click);
                this.txtCurrentPass.KeyUp += (objs, obje) => { if (obje.KeyCode == Keys.Enter) { btnApplyPass_Click(btnApplyCurrentPass, null); } };
                this.txtNewPass.KeyUp += (objs, obje) => { if (obje.KeyCode == Keys.Enter) { btnApplyPass_Click(btnApplyNewPass, null); } };
                this.btnStart.Click += new EventHandler(btnStart_Click);
                this.btnStop.Click += new EventHandler(btnStop_Click);
                this.bgwk = new BackgroundWorker();
                bgwk.WorkerSupportsCancellation = true;
                bgwk.DoWork += new DoWorkEventHandler(bgwk_DoWork);
                bgwk.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwk_RunWorkerCompleted);
                createGrid(null);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Please make sure you want to stop process !", this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    this.btnSelectFile.Enabled = true;
                    this.btnApplyCurrentPass.Enabled = true;
                    this.btnApplyNewPass.Enabled = true;
                    this.btnStart.Enabled = true;
                    this.btnStop.Enabled = false;
                    this.bgwk.CancelAsync();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        void bgwk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!this.bgwk.CancellationPending)
                {
                    iWorkingIndex += 1;
                    if (iWorkingIndex < gridData.Rows.Count)
                    {
                        this.bgwk.RunWorkerAsync(iWorkingIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        void bgwk_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (e.Argument is int)
                {
                    int iGridIndex = (int)e.Argument;
                    FaceBookController fb = new FaceBookController();//new LoginContain(gridData[ChangePassGridConst.Login, iGridIndex].Value.ToString(), gridData[ChangePassGridConst.CurrentPass, iGridIndex].Value.ToString()));
                    fb.StatusChanged += (objs, obje) =>
                    {
                        gridData.Invoke((Action)(() =>
                        {
                            gridData[ChangePassGridConst.Status, iGridIndex].Value = obje.Status;
                        }));
                    };
                    if (fb.CheckLogin())
                    {
                        if (!bgwk.CancellationPending)
                        {
                            fb.ChangePass(gridData[ChangePassGridConst.NewPass, iGridIndex].Value.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Please make sure you want to change pass !", this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    this.btnSelectFile.Enabled = false;
                    this.btnApplyCurrentPass.Enabled = false;
                    this.btnApplyNewPass.Enabled = false;
                    this.btnStart.Enabled = false;
                    this.btnStop.Enabled = true;
                    this.bgwk.RunWorkerAsync(iWorkingIndex);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        void btnApplyPass_Click(object sender, EventArgs e)
        {
            try
            {
                for (int iIndex = 0; iIndex < gridData.Rows.Count; iIndex++)
                {
                    gridData[sender == btnApplyCurrentPass ? ChangePassGridConst.CurrentPass : ChangePassGridConst.NewPass, iIndex].Value
                        = sender == btnApplyCurrentPass ? txtCurrentPass.Text : txtNewPass.Text;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        void btnSelectFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog of = new OpenFileDialog();
                of.Multiselect = false;
                if (of.ShowDialog() == DialogResult.OK)
                {
                    string strFileName = of.FileName;
                    if (Path.GetExtension(strFileName).ToUpper() == ".TXT")
                    {
                        txtFileLocation.Text = strFileName;
                        importFile(strFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion
        #region - METHOD -
        private void importFile(string strFileName)
        {
            try
            {
                StreamReader sr = new StreamReader(strFileName);
                string strContent = sr.ReadToEnd();
                DataTable dtSource = createTableSource();
                foreach (string strAccount in strContent.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(strAccount.Trim()))
                    {
                        dtSource.Rows.Add(new object[] { strAccount.Trim() });
                    }
                }
                createGrid(dtSource);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void createGrid(DataTable dtSource)
        {
            try
            {
                btnStop.Enabled = false;
                iWorkingIndex = 0;
                if (dtSource == null || dtSource.Rows.Count == 0)
                {
                    btnStart.Enabled = false;
                    dtSource = createTableSource();
                }
                else
                {
                    btnStart.Enabled = true;
                }
                gridData.DataSource = dtSource;
                for (int iIndex = 0; iIndex < gridData.Columns.Count; iIndex++)
                {
                    gridData.Columns[iIndex].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                gridData.Columns[ChangePassGridConst.Login].ReadOnly = true;
                gridData.Columns[ChangePassGridConst.Status].ReadOnly = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private DataTable createTableSource()
        {
            DataTable dtSource = new DataTable();
            dtSource.Columns.Add(ChangePassGridConst.Login, typeof(string));
            dtSource.Columns.Add(ChangePassGridConst.CurrentPass, typeof(string));
            dtSource.Columns.Add(ChangePassGridConst.NewPass, typeof(string));
            dtSource.Columns.Add(ChangePassGridConst.Status, typeof(string));
            return dtSource;
        }
        #endregion
    }
}
