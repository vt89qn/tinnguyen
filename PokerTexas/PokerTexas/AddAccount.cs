using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TableConstants;

namespace PokerTexas
{
    public partial class AddAccount : Form
    {
        #region - DECLARE -
        private Dictionary<string, object> dicParams = new Dictionary<string, object>();
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
        public Dictionary<string, object> DicParams
        {
            get { return this.dicParams; }
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
                this.Password = System.Configuration.ConfigurationSettings.AppSettings["DefaultPass"].Trim();
            }
            catch (Exception ex)
            {
                //Handle Error
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
                fb.LoginInfo = new LoginContain(this.txtUserName.Text.Trim(), this.txtPassword.Text.Trim());
                if (fb.Login())
                {
                    dicParams = new Dictionary<string, object>();
                    dicParams.Add(M_AccountConst.Account, this.txtUserName.Text.Trim());
                    dicParams.Add(M_AccountConst.Password, this.txtPassword.Text.Trim());
                    Serializer serial = new Serializer();
                    byte[] bytesCookie = serial.ConvertObjectToBlob(fb.StoreCookie);
                    dicParams.Add(M_AccountConst.CookieFB, bytesCookie);
                    dicParams.Add(M_AccountConst.FaceBookID, fb.FBAccountID);
                    this.Close();
                    //this.Dispose();
                }
                else
                {
                    MessageBox.Show("Can't login, Please check your account !");
                }
            }
            catch (Exception ex)
            {
                //Handle Error
            }
            finally
            {
                this.Enabled = true;
            }
        }
        #endregion

    }
}
