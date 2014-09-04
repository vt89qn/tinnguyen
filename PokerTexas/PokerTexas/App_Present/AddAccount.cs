using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TableConstants;
using PokerTexas.App_Common;
using PokerTexas.App_Model;
using PokerTexas.App_Controller;

namespace PokerTexas.App_Present
{
    public partial class AddAccount : Form
    {
        #region - DECLARE -
        private FaceBook model;
        #endregion
        #region - PROPERTIES -
        public string Account
        {
            set { this.txtUserName.Text = value; }
        }
        public string Password
        {
            set { this.txtPassword.Text = value; }
        }
        public FaceBook Model
        {
            get { return this.model; }
        }
        #endregion
        #region - CONTRUCTOR -
        public AddAccount()
        {
            InitializeComponent();
            this.Load += new EventHandler(AddAccount_Load);
        }
        #endregion
        #region - EVENT -
        void AddAccount_Load(object sender, EventArgs e)
        {
            try
            {
                this.btnOK.Click += new EventHandler(btnOK_Click);
                this.btnCancel.Click += new EventHandler(btnCancel_Click);
                this.txtUserName.KeyUp += new KeyEventHandler(txtUserName_KeyUp);
                this.txtPassword.KeyUp += new KeyEventHandler(txtUserName_KeyUp);
                this.Password = AppSettings.DefaultPass;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void txtUserName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnOK_Click(null, null);
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                FaceBook fb = new FaceBook();
                fb.Login = this.txtUserName.Text.Trim();
                fb.Pass = this.txtPassword.Text.Trim();
                FaceBookController fbController = new FaceBookController();
                if (fbController.LoginMobile(fb))
                {
                    this.model = fb;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể đăng nhập !");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Enabled = true;
            }
        }
        #endregion

    }
}
