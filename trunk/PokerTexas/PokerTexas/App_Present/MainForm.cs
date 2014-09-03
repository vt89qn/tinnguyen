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

namespace PokerTexas.App_Present
{
    public partial class MainForm : Form
    {
        #region - DECLARE -
        DataTable dtPack = new DataTable(M_PackageConst.TableName);
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
            PokerContext db = new PokerContext();
            var fbList = db.FaceBook.ToList();
            foreach (FaceBook fb in fbList)
            {
                PokerController con = new PokerController();
                Poker pk = new Poker();
                pk.FaceBook = fb;
                pk.Package = db.Package.ToList()[0];
                if (con.LoginMobile(pk))
                {
                    db.Poker.Add(pk);
                    db.SaveChanges();
                    var t = db.Poker.ToList();
                }
            }

            //FaceBookController con = new FaceBookController();

            //FaceBook fb = new FaceBook();
            //fb.Login = "eiua38475@popchick.com";
            //fb.Pass = "S12365hh*";
            //if (con.LoginMobile(fb))
            //{
            //    PokerContext db = new PokerContext();
            //    db.FaceBook.Add(fb);
            //    db.SaveChanges();

            //    var t = db.FaceBook.ToList();
            //}
        }

        private void txtPackNo_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnThemTaiKhoan_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region - METHOD -
        #endregion


    }
}
