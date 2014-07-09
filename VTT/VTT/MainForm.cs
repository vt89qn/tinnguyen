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

        List<string> listManhTuongKhongGhep = new List<string> { "ChanPhi", "TieuKieu", "TonThuongHuong", "TruongOanhOanh", "TaoHoa" };

        List<string> listTuongUpLevelBangVang = new List<string> { "HoangCai", "TonLoDuc", "TienDung", "ChuDongTu", "BaTrieu" };

        dynamic dataLogin = null;
        Timer timerUpLevel = new Timer();
        public MainForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(MainForm_Load);

        }



        void MainForm_Load(object sender, EventArgs e)
        {
            this.btnLogin.Click += new EventHandler(btnLogin_Click);
            this.timerUpLevel.Enabled = false;
            this.timerUpLevel.Interval = 60000;
            this.timerUpLevel.Tick += new EventHandler(timerUpLevel_Tick);
            this.btnBatDauUpLevel.Click += new EventHandler(btnBatDauUpLevel_Click);
            this.btnGiamThoiGianCho.Click += new EventHandler(btnGiamThoiGianCho_Click);
        }

        void btnGiamThoiGianCho_Click(object sender, EventArgs e)
        {
            List<dynamic> listTuongUp = new List<dynamic>();
            foreach (dynamic officer in dataLogin["owned_officers"])
            {
                if (listTuongUpLevelBangVang.Contains(officer["officer_id"]))
                {
                    listTuongUp.Add(officer);
                }
            }

            if (listTuongUp.Count == 5)
            {
                listTuongUp.ForEach(x => { new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(reduce_cooldown)).Start(x); });
                System.Threading.Thread.Sleep(3000);
                timerUpLevel_Tick(null, null);
            }
        }

        void btnBatDauUpLevel_Click(object sender, EventArgs e)
        {
            this.timerUpLevel.Enabled = !this.timerUpLevel.Enabled;
            if (timerUpLevel.Enabled)
            {
                this.timerUpLevel.Start();
                btnBatDauUpLevel.Text = "Kết thúc";
            }
            else
            {
                this.timerUpLevel.Stop();
                btnBatDauUpLevel.Text = "Bắt đầu";
            }
        }

        void timerUpLevel_Tick(object sender, EventArgs e)
        {
            List<dynamic> listTuongUp = new List<dynamic>();
            foreach (dynamic officer in dataLogin["owned_officers"])
            {
                DateTime timeLevelup = DateTime.MaxValue;
                int iLevel = 60;
                int iRank = 3;
                foreach (dynamic component in officer["components"])
                {
                    if (component.ContainsKey("leveled_up_at"))
                    {
                        timeLevelup = new DateTime(1970, 1, 1).AddSeconds(Convert.ToDouble(component["leveled_up_at"])).AddHours(7).AddSeconds(Convert.ToDouble(component["cooldown_time_to_next_level"]));
                    }
                    if (component.ContainsKey("level"))
                    {
                        iLevel = component["level"];
                    }
                    if (component.ContainsKey("rank"))
                    {
                        iRank = component["rank"];
                    }
                }
                if (DateTime.Now < timeLevelup || (iLevel < 50 && iRank < 3))
                {
                }
                else
                {
                    listTuongUp.Add(officer);
                }

            }
            if (listTuongUp.Count > 0)
            {
                listTuongUp.ForEach(x => new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(levelup)).Start(x));
                //System.Threading.Thread.Sleep(2000);
                //login();
            }
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

        #region - METHOD -

        private void reduce_cooldown(object officer)
        {
            dynamic of = officer;
            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", dataLogin["authentication_token"]);
            param.Add("hours", "1");
            param.Add("cooldown_type", "LevelComp");
            param.Add("is_skill_2", "false");
            client.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/owned_officers/" + of["id"] + "/reduce_cooldown");
            if (!string.IsNullOrEmpty(client.ResponseText))
            {
                dynamic dataRS = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue }.Deserialize<dynamic>(client.ResponseText);
                foreach (dynamic component in of["components"])
                {
                    if (component.ContainsKey("leveled_up_at"))
                    {
                        component["leveled_up_at"] = dataRS["component_data"]["leveled_up_at"];
                    }
                }
            }

        }


        private void levelup(object officer)
        {
            dynamic of = officer;
            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", dataLogin["authentication_token"]);
            client.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/owned_officers/" + of["id"] + "/level_up");
            if (!string.IsNullOrEmpty(client.ResponseText))
            {
                dynamic dataRS = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue }.Deserialize<dynamic>(client.ResponseText);
                foreach (dynamic component in of["components"])
                {
                    if (component.ContainsKey("leveled_up_at"))
                    {
                        component["leveled_up_at"] = dataRS["level_comp"]["leveled_up_at"];
                        component["cooldown_time_to_next_level"] = dataRS["level_comp"]["cooldown_time_to_next_level"];
                    }
                }
            }
        }

        private bool login()
        {
            string strProfile = "tin";
            dynamic dataProfile = new JavaScriptSerializer().Deserialize<dynamic>(profileJson);

            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();

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
