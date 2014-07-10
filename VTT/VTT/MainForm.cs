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
        #region - DECLARE -
        string profileJson = @"{'tin':{'access_token':'dm5pX3Rva2VuPTAuODE1NzM1MDAgMTQwMjM3NzkyMSsxNzk2NjIzMzgx','user_name':'vt89qn','user_id':'823493150','user_status':'1','avatar_img_link':'http://avatar.my.soha.vn/80/vt89qn.png'}
                              ,'thi':{'access_token':'dm5pX3Rva2VuPTAuNTMwOTg3MDAgMTQwMjMxMDk5MSs3NDc4MTU2NjA=','user_name':'Pole','user_id':'91031','user_status':'1','avatar_img_link':'http://avatar.my.soha.vn/80/Pole.png'}}";

        List<string> listManhTuongKhongGhep = new List<string> { "ChanPhi", "TieuKieu", "TonThuongHuong", "TruongOanhOanh", "TaoHoa" };

        List<string> listTuongUpLevelBangVang = new List<string> { "HoangCai", "TonLoDuc", "TienDung", "ChuDongTu", "BaTrieu" };

        dynamic dataLogin = null;
        Timer timerUpLevel = new Timer();

        int tower_achievement = 10;
        int tower_floor = 0;
        private string authentication_token { get { return dataLogin["authentication_token"]; } }
        private string id { get { return dataLogin["id"]; } }
        private int attack_turn_count { get { return dataLogin["attack_turn_count"]; } set { dataLogin["attack_turn_count"] = value; } }
        private int current_defense_turn_count { get { return dataLogin["current_defense_turn_count"]; } set { dataLogin["current_defense_turn_count"] = value; } }
        #endregion
        #region - CONTRUCTOR -
        public MainForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(MainForm_Load);

        }
        #endregion


        void MainForm_Load(object sender, EventArgs e)
        {
            buildDefaultDataOnLoad();
            this.btnLogin.Click += new EventHandler(btnLogin_Click);
            this.timerUpLevel.Enabled = false;
            this.timerUpLevel.Interval = 60000;
            this.timerUpLevel.Tick += (objs, obje) => { levelup(); };
            this.btnBatDauUpLevel.Click += new EventHandler(btnBatDauUpLevel_Click);
            this.btnGiamThoiGianCho.Click += (objs, obje) => { reduce_cooldown(); };
            this.btnLeoThap.Click += (objs, obje) => { attack_tower_floor(); };
            this.btnVuotAi.Click += (objs, obje) => { attack_city(); };
            this.btnSaThaiTuong.Click += (objs, obje) => { fire(); };
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
        #region - fire -
        private void fire()
        {
            try
            {
                this.btnSaThaiTuong.Enabled = false;
                txtStatus.Text = string.Empty;

                foreach (dynamic officer in dataLogin["owned_officers"])
                {
                    bool bFire = false;
                    int iLevel = 50;
                    foreach (dynamic component in officer["components"])
                    {
                        if (component.ContainsKey("level"))
                        {
                            iLevel = component["level"];
                            //if (iLevel > 1)
                            //{
                            //    bFire = false;
                            //    break;
                            //}
                        }
                        if (component.ContainsKey("rank") && component["rank"] <= 2)
                        {
                            bFire = true;
                        }
                    }
                    if (bFire)
                    {
                        for (int iIndex = 0; iIndex < 6; iIndex++)
                        {
                            new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(fire)).Start(officer["id"]);
                        }
                        System.Threading.Thread.Sleep(2000);
                    }
                }
            }
            catch { }
            finally
            {
                this.btnSaThaiTuong.Enabled = true;
            }
        }
        private void fire(object id)
        {
            WebClientEx client = new WebClientEx();
            client.DoGet("https://vtt-01.zoygame.com/owned_officers/" + id + "/fire?authentication_token=" + authentication_token);
        }
        #endregion
        #region - attack_city -
        private void attack_city()
        {
            try
            {
                btnVuotAi.Enabled = false;
                if (attack_turn_count < int.Parse(txtVuotAiDoKho.Text.Trim()))
                {
                    MessageBox.Show("Không đủ cờ chiến");
                    return;
                }

                string strtenai = txtVuotAiTenAi.SelectedValue.ToString();
                string strdokho = txtVuotAiDoKho.Text.Trim();



                WebClientEx client = new WebClientEx();
                NameValueCollection param = new NameValueCollection();
                param.Add("authentication_token", authentication_token);
                param.Add("city_id", txtVuotAiTenAi.SelectedValue.ToString());
                param.Add("star", txtVuotAiDoKho.Text.Trim());
                foreach (dynamic item in dataLogin["current_defense_battle"]["deployed_officer_coordinates"])
                {
                    param.Add("formation[]", item["owned_officer_id"]);
                }
                client.DoPost(param, "https://vtt-01.zoygame.com/players/" + id + "/attack_city");
                if (client.ResponseText != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    dynamic rs = serializer.Deserialize<dynamic>(client.ResponseText);

                    current_defense_turn_count = rs["current_defense_turn_count"];
                    attack_turn_count = rs["attack_turn_count"];

                    System.Threading.Thread.Sleep(500);
                    client = new WebClientEx();
                    param = new NameValueCollection();
                    param.Add("authentication_token", authentication_token);
                    client.DoPost(param, "https://vtt-01.zoygame.com/players/" + id + "/won_attack_city");
                    if (client.ResponseText != null)
                    {
                        string capture_card_id = string.Empty;
                        if (chkVuotAiChieuHangTuong.Checked)
                        {
                            //foreach (dynamic item in data["owned_items"])
                            //{
                            //    if (item["name"] == "capture_card_04")
                            //    {
                            //        capture_card_id = item["id"];
                            //        break;
                            //    }
                            //}
                            //if (string.IsNullOrEmpty(capture_card_id))
                            //{
                            //    foreach (dynamic item in data["owned_items"])
                            //    {
                            //        if (item["name"] == "capture_card_03")
                            //        {
                            //            capture_card_id = item["id"];
                            //            break;
                            //        }
                            //    }
                            //}
                            //if (string.IsNullOrEmpty(capture_card_id))
                            //{
                            //    foreach (dynamic item in data["owned_items"])
                            //    {
                            //        if (item["name"] == "capture_card_02")
                            //        {
                            //            capture_card_id = item["id"];
                            //            break;
                            //        }
                            //    }
                            //}
                            if (string.IsNullOrEmpty(capture_card_id))
                            {
                                foreach (dynamic item in dataLogin["owned_items"])
                                {
                                    if (item["name"] == "capture_card_01")
                                    {
                                        capture_card_id = item["id"];
                                        if (item["quantity"] <= 0)
                                        {
                                            txtStatus.Text = "Hết thẻ chiêu hàng";
                                            return;
                                        }
                                        item["quantity"] -= 1;
                                        break;
                                    }
                                }
                            }
                        }

                        param = new NameValueCollection();
                        param.Add("authentication_token", authentication_token);
                        param.Add("reward_type", chkVuotAiChieuHangTuong.Checked ? "officer" : "chest");
                        param.Add("game_mode", "attack");
                        param.Add("capture_card_id", capture_card_id);
                        if (chkVuotAiChieuHangTuong.Checked)
                        {
                            client = new WebClientEx();
                            client.DoPost(param, "https://vtt-01.zoygame.com/players/" + id + "/retrieve_reward_for_last_won_battle");
                            if (client.ResponseText != null)
                            {
                                txtStatus.Text = "ĐÃ bắt được tướng";
                            }
                            else
                            {
                                txtStatus.Text = "KHÔNG bắt được tướng";
                            }

                        }
                        else
                        {
                            for (int iIndex = 0; iIndex < 6; iIndex++)
                            {
                                new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(retrieve_reward_for_last_won_battle)).Start(param);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                btnVuotAi.Enabled = true;
            }
        }

        private void retrieve_reward_for_last_won_battle(object param)
        {
            WebClientEx clientfat = new WebClientEx();
            clientfat.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/players/" + id + "/retrieve_reward_for_last_won_battle");
        }
        #endregion
        #region - attack_tower_floor -
        private void attack_tower_floor()
        {
            bool bDone = false;

            while (true)
            {
                WebClientEx client = new WebClientEx();
                NameValueCollection param = new NameValueCollection();
                param.Add("authentication_token", authentication_token);
                client.DoPost(param, "https://vtt-01.zoygame.com/players/" + id + "/attack_next_tower_floor");
                if (client.ResponseText != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    dynamic rs = serializer.Deserialize<object>(client.ResponseText);

                    param = new NameValueCollection();
                    param.Add("authentication_token", authentication_token);
                    param.Add("won_floor", bDone ? "0" : "1");
                    param.Add("floor_npc_id", rs["floor_npc_id"]);
                    string strRD = new Random().Next(1, 99).ToString();
                    //param.Add("remaining_hp", bDone ? "0" : tower_floor <= 25 ? ("5." + strRD) : ("82." + strRD));
                    param.Add("remaining_hp", bDone ? "0" : tower_floor <= 53 ? ("82." + strRD) : ("5." + strRD));

                    for (int iIndex = 0; iIndex < 6; iIndex++)
                    {
                        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(finish_attack_tower)).Start(param);
                    }
                    System.Threading.Thread.Sleep(1000);
                    if (bDone) break;
                    if (tower_achievement >= 300)
                    {
                        bDone = true;
                    }
                }
                else
                {
                    MessageBox.Show("Leo chua xong,login leo tiep"); ;
                    break;
                }
            }
        }

        private void finish_attack_tower(object param)
        {
            WebClientEx clientfat = new WebClientEx();
            clientfat.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/players/" + id + "/finish_attack_tower");
            if (clientfat.ResponseText != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic dataRS = serializer.Deserialize<object>(clientfat.ResponseText);

                tower_achievement = dataRS["tower_data"]["tower_achievement"];
                tower_floor = dataRS["tower_data"]["tower_floor"];
            }
        }
        #endregion
        #region - reduce_cooldown -
        private void reduce_cooldown()
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
                System.Threading.Thread.Sleep(1500);
                levelup();
            }
        }

        private void reduce_cooldown(object officer)
        {
            dynamic of = officer;
            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
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
        #endregion
        #region - levelup -
        private void levelup()
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

        private void levelup(object officer)
        {
            dynamic of = officer;
            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
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
        #endregion
        #region - login -
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
        #region - Default data -
        private void buildDefaultDataOnLoad()
        {
            DataTable dtTenAi = new DataTable();
            dtTenAi.Columns.Add("id", typeof(string));
            dtTenAi.Columns.Add("name", typeof(string));
            dtTenAi.Rows.Add("dinh_dao", "Định Đào");
            dtTenAi.Rows.Add("hop_phi", "Hợp Phì");
            dtTenAi.Rows.Add("hoa_dung", "Hoa Dung");
            dtTenAi.Rows.Add("xich_bich", "Xích Bích");
            dtTenAi.Rows.Add("nguu_chu", "Ngưu Chữ");
            dtTenAi.Rows.Add("ho_lao_quan", "Hổ Lao Quan");
            dtTenAi.Rows.Add("doc_bac_vong", "Dốc Bắc Vọng");
            dtTenAi.Rows.Add("di_lang", "Di Lăng");
            txtVuotAiTenAi.DataSource = dtTenAi;
            txtVuotAiTenAi.DisplayMember = "name";
            txtVuotAiTenAi.ValueMember = "id";
            txtVuotAiTenAi.SelectedIndex = 0;
        }
        #endregion
        #endregion
    }
}
