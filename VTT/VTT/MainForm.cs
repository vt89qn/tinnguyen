using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Web.Script.Serialization;

namespace VTT
{
    public partial class MainForm : Form
    {
        string profileJson = @"{'tin':{'access_token':'dm5pX3Rva2VuPTAuODE1NzM1MDAgMTQwMjM3NzkyMSsxNzk2NjIzMzgx','user_name':'vt89qn','user_id':'823493150','user_status':'1','avatar_img_link':'http://avatar.my.soha.vn/80/vt89qn.png'}
                              ,'thi':{'access_token':'dm5pX3Rva2VuPTAuNTMwOTg3MDAgMTQwMjMxMDk5MSs3NDc4MTU2NjA=','user_name':'Pole','user_id':'91031','user_status':'1','avatar_img_link':'http://avatar.my.soha.vn/80/Pole.png'}}";

        List<string> listManhTuongKhongGhep = new List<string> { "ChanPhi","TieuKieu","TonThuongHuong","TruongOanhOanh","TaoHoa"};
        dynamic dataLogin = null;
        WebClientEx client = null;
        NameValueCollection param = null;
        public MainForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(MainForm_Load);
            this.btnLogin.Click += new EventHandler(btnLogin_Click);
        }

        void btnLogin_Click(object sender, EventArgs e)
        {
            if (!login())
            {
                MessageBox.Show("Login không thành công");
            }
            else
            {
                tabMain.SelectedTab = pageTool;
            }
        }

        void MainForm_Load(object sender, EventArgs e)
        {
        }

        #region - METHOD -
        private bool login()
        {
            string strProfile = "tin";
            dynamic dataProfile = new JavaScriptSerializer().Deserialize<dynamic>(profileJson);

            client = new WebClientEx();
            param = new NameValueCollection();

            param.Add("access_token", dataProfile[strProfile]["access_token"]);
            param.Add("user_name", dataProfile[strProfile]["user_name"]);
            param.Add("user_id", dataProfile[strProfile]["user_id"]);
            param.Add("user_status", dataProfile[strProfile]["user_status"]);
            param.Add("avatar_img_link", dataProfile[strProfile]["avatar_img_link"]);

            client.DoPost(param, "https://vtt-01.zoygame.com/players/sign_in");

            if (client.ResponseText != null)
            {
                dataLogin = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue }.Deserialize<dynamic>(client.ResponseText);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
