using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FaceBookNuker.Controller;
using FaceBookNuker.Models;
using System.IO;

namespace FaceBookNuker
{
    public partial class SSCVN : Form
    {
        Timer timecheckNumer;
        List<M_Account> account;
        int iStep = 50;
        int iCurrentNumber = 0;
        public int CurrentNumber
        {
            set
            {
                iCurrentNumber = value;
                if (iCurrentNumber > 499000)
                {
                    btnGenName_Click(null, null);
                }
            }
            get { return this.iCurrentNumber; }
        }

        public SSCVN()
        {
            InitializeComponent();
            this.Load += new EventHandler(SSCVN_Load);
        }

        void SSCVN_Load(object sender, EventArgs e)
        {
            timecheckNumer = new Timer();
            timecheckNumer.Enabled = false;
            timecheckNumer.Interval = 1000;
            //timecheckNumer.Tick += new EventHandler(timecheckNumer_Tick);
            timecheckNumer.Enabled = true;
            account = DataProvider.SSCVNDB.M_Account.ToList();
            //Utilities.SerializeObject("db.ssc", account);
            //object objaccount = Utilities.DeSerializeObject("db.ssc");
            //if (objaccount is List<M_Account>)
            //{
            //    account = objaccount as List<M_Account>;
            //}
            //else
            //{
            //    account = DataProvider.SSCVNDB.M_Account.ToList();
            //}
            //SSCVNController sscvn = new SSCVNController();
            //sscvn.GenName();
            btnGenName_Click(null, null);
        }

        void timecheckNumer_Tick(object sender, EventArgs e)
        {
            WebClientEx Web = new WebClientEx();
            try
            {
                timecheckNumer.Enabled = false;
                if (iCurrentNumber > 499000) return;
                int iNumber = 478680;
                StreamReader rd = new StreamReader("number.txt");
                string t = rd.ReadToEnd();
                rd.Close();
                rd.Dispose();
                if (!int.TryParse(t.Trim(), out iNumber))
                {
                    iNumber = 478680;
                }

                bool bFound = true;
                while (bFound)
                {
                    lblNo.Text = iNumber.ToString();
                    Web.DoGet("http://ssc.vn/member.php?u=" + iNumber);
                    if (string.IsNullOrEmpty(Web.ResponseText) || Web.ResponseText.Contains("Chủ đề đã bị khóa hoặc xóa"))
                    {
                        bFound = false;
                        //iStep = 1;
                    }
                    else
                    {
                        iNumber += iStep;
                    }
                }
                CurrentNumber = iNumber;
                lblNo.Text = iNumber.ToString();
                StreamWriter rw = new StreamWriter("number.txt");
                rw.Write(iNumber.ToString());
                rw.Close();
                rw.Dispose();
            }
            catch { }
            finally
            {
                Web.Dispose();
                if (iCurrentNumber < 499000)
                    timecheckNumer.Enabled = true;
            }
        }

        private void btnGenName_Click(object sender, EventArgs e)
        {

            int iThread = 5;
            for (int iIndex = 0; iIndex < account.Count; iIndex++)
            {
                int iCount = iIndex + iThread;
                for (; iIndex < account.Count && iIndex <= iCount; iIndex++)
                {
                    SSCVNController sscvn = new SSCVNController();
                    new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(sscvn.RegAccount)).Start(account[iIndex]);
                    //sscvn.RegAccount(account[iIndex]);
                }
                System.Threading.Thread.Sleep(1000);
            }

        }
    }
}
