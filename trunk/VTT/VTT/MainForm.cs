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
using System.Collections;
using System.IO;

namespace VTT
{
    public partial class MainForm : Form
    {
        #region - DECLARE -
        string profileJson = @"{'tin':{'access_token':'dm5pX3Rva2VuPTAuODE1NzM1MDAgMTQwMjM3NzkyMSsxNzk2NjIzMzgx','user_name':'vt89qn','user_id':'823493150','user_status':'1','avatar_img_link':'http://avatar.my.soha.vn/80/vt89qn.png'}
                              ,'thi':{'access_token':'dm5pX3Rva2VuPTAuNTMwOTg3MDAgMTQwMjMxMDk5MSs3NDc4MTU2NjA=','user_name':'Pole','user_id':'91031','user_status':'1','avatar_img_link':'http://avatar.my.soha.vn/80/Pole.png'}}";



        Dictionary<string, object> dataLogin = null;
        Timer timerUpLevel = new Timer();

        int tower_achievement = 10;
        int tower_floor = 0;
        private string authentication_token { get { return dataLogin["authentication_token"].ToString(); } }
        private string id { get { return dataLogin["id"].ToString(); } }
        private int attack_turn_count { get { return (int)dataLogin["attack_turn_count"]; } set { dataLogin["attack_turn_count"] = value; } }
        private int current_defense_turn_count { get { return (int)dataLogin["current_defense_turn_count"]; } set { dataLogin["current_defense_turn_count"] = value; } }

        private Dictionary<string, object> current_defense_battle { get { return (Dictionary<string, object>)dataLogin["current_defense_battle"]; } set { dataLogin["current_defense_battle"] = value; } }
        private int current_wave { get { return current_defense_battle != null ? (int)current_defense_battle["current_wave"] + 1 : -1; } set { current_defense_battle["current_wave"] = value - 1; } }
        private int total_wave_count { get { return current_defense_battle != null ? (int)current_defense_battle["total_wave_count"] : -1; } }
        private int star { get { return current_defense_battle != null ? (int)current_defense_battle["star"] : -1; } }
        private string city_id { get { return current_defense_battle != null ? current_defense_battle["city_id"].ToString() : "-1"; } }

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
            //dataLogin = Utilities.DeSerializeObject("login.data");
            this.btnLogin.Click += new EventHandler(btnLogin_Click);
            this.timerUpLevel.Enabled = false;
            this.timerUpLevel.Interval = 60000;
            this.timerUpLevel.Tick += (objs, obje) => { levelup(); };
            this.btnBatDauUpLevel.Click += new EventHandler(btnBatDauUpLevel_Click);
            this.btnGiamThoiGianCho.Click += (objs, obje) => { reduce_cooldown(); };
            this.btnLeoThap.Click += (objs, obje) => { attack_tower_floor(); };
            this.btnVuotAi.Click += (objs, obje) => { attack_city(); };
            this.btnSaThaiTuong.Click += (objs, obje) => { fire(); };
            this.btnThuThanh.Click += (objs, obje) => { defence_city(); };
            this.btnDungQLCC.Click += (objs, obje) => { use_item(); };
            this.btnGhepManhTuongTuDong.Click += (objs, obje) => { merge(); };
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
                setStatus("Login không thành công");
            }
            else
            {
                tabMain.SelectedTab = pageThanh;
                writeGlobalInfo();
            }
        }

        #region - METHOD -
        #region - merge -
        private void merge()
        {
            try
            {
                btnGhepManhTuongTuDong.Enabled = false;
                txtStatus.Text = string.Empty;
                setStatus("START : Ghép mảnh tướng\n");

                List<string> listManhTuongKhongGhep = new List<string> { "ChanPhi", "TieuKieu", "TonThuongHuong", "TruongOanhOanh", "TaoHoa",
                "TruongNam","KiLinh","LaPham","TruongChieu","TruongLuong","HaTe","QuachMa","LaHien","TruongBao","DiemTuong","DienPhong","CongTonToan","LuuHuan","TangBa"};

                for (int i = 2; i <= 4; i++)
                {
                    List<Dictionary<string, object>> dicManhTuong = layDanhSachManhTuong(i);
                    int iManhTuong = 5;
                    while (iManhTuong == 5)
                    {
                        iManhTuong = 0;
                        string strURL = "https://vtt-01.zoygame.com/owned_officer_souls/merge?authentication_token=" + authentication_token;

                        foreach (Dictionary<string, object> item in dicManhTuong)
                        {
                            if (listManhTuongKhongGhep.Contains(item["officer_id"].ToString())) continue;
                            if ((int)item["quantity"] > 0)
                            {
                                int iGetMT = 0;
                                iGetMT += ((int)item["quantity"] < 5 - iManhTuong ? (int)item["quantity"] : 5 - iManhTuong);
                                iManhTuong += iGetMT;
                                item["quantity"] = (int)item["quantity"] - iGetMT;
                                for (int iIndex = 0; iIndex < iGetMT; iIndex++)
                                {
                                    strURL += "&owned_officer_soul_ids%5B%5D=" + item["id"].ToString();
                                }
                            }
                            if (iManhTuong == 5)
                            {
                                for (int iIndex = 0; iIndex < 6; iIndex++)
                                {
                                    new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(merge)).Start(strURL);
                                }
                                System.Threading.Thread.Sleep(2000);
                                break;
                            }
                        }
                    }
                    login();
                }

            }
            catch { }
            finally
            {
                btnGhepManhTuongTuDong.Enabled = true;
                setStatus("\nEND : Ghép mảnh tướng");
                writeGlobalInfo();
            }

        }

        private void merge(object url)
        {
            WebClientEx client = new WebClientEx();
            client.DoGet(url.ToString());
        }

        private List<Dictionary<string, object>> layDanhSachManhTuong(int rank)
        {
            DataTable tblHeroes = GetDataTableFromCSV();
            List<Dictionary<string, object>> dicManhTuong = new List<Dictionary<string, object>>();
            if (tblHeroes.Rows.Count > 0)
            {
                foreach (Dictionary<string, object> owned_officer_soul in dataLogin["owned_officer_souls"] as ArrayList)
                {
                    if ((int)owned_officer_soul["quantity"] > 0 && tblHeroes.Select(string.Format("name = '{0}' and rank = '{1}'", owned_officer_soul["officer_id"], rank)).Length > 0)
                    {
                        dicManhTuong.Add(owned_officer_soul);
                        //dicManhTuong.Add(owned_officer_soul["id"].ToString(), (int)owned_officer_soul["quantity"]);
                    }
                }
            }
            return dicManhTuong;
        }
        public static DataTable GetDataTableFromCSV()
        {
            try
            {
                string strFileName = "heroes.txt";
                DataTable tbl = new DataTable();
                StreamReader sr = new StreamReader(strFileName);
                string strline = "";
                string[] values = null;
                int d = 0;
                bool creatColumns = true;
                while (!sr.EndOfStream)
                {
                    d++;
                    strline = sr.ReadLine();
                    values = strline.Split(',');
                    //insert rows for table
                    if (creatColumns == false)
                    {
                        //if (values[1].ToString().Equals("1") || values[1].ToString().Equals("5")) continue;
                        DataRow row = tbl.NewRow();
                        for (int i = 0; i < values.Length; i++)
                        {
                            row[i] = values[i].ToString().Replace("\"", "");
                        }
                        tbl.Rows.Add(row);
                    }
                    //creat columns for table
                    else if (creatColumns == true)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            tbl.Columns.Add(values[i].ToString().Replace("\"", ""));
                        }
                        creatColumns = false;
                    }
                }
                sr.Close();
                return tbl;
            }
            catch
            {
                System.Data.DataTable tbl = new System.Data.DataTable();
                return tbl;
            }
        }
        #endregion

        #region - use_item -
        private void use_item()
        {
            txtStatus.Text = string.Empty;
            setStatus("START : Nuốt quân lệnh / cờ chiến\n");

            string loaiquanlenh = txtQLCC.SelectedValue.ToString();
            string item_id = string.Empty;
            int isl = -1;
            int islrequest = int.Parse(txtDungQLCC_SoLuong.Text.Trim());
            Dictionary<string, object> itemselect = null;
            foreach (Dictionary<string, object> item in (dataLogin["owned_items"] as ArrayList))
            {
                if (item["name"].ToString() == loaiquanlenh)
                {
                    isl = (int)item["quantity"];
                    item_id = item["id"].ToString();
                    itemselect = item;
                    break;
                }
            }
            if (string.IsNullOrEmpty(item_id))
            {
                setStatus("không tìm thấy item");
            }
            else if (isl < islrequest)
            {
                setStatus("số lượng không đủ");
            }
            else
            {
                WebClientEx client = new WebClientEx();
                NameValueCollection param = new NameValueCollection();
                param.Add("owned_item_id", item_id);
                param.Add("quantity", islrequest.ToString());
                param.Add("authentication_token", authentication_token);
                client.DoPost(param, "https://vtt-01.zoygame.com/owned_items/use_item");
                if (client.ResponseText != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Dictionary<string, object> rs = serializer.Deserialize<Dictionary<string, object>>(client.ResponseText);
                    itemselect["quantity"] = (int)itemselect["quantity"] - islrequest;

                    current_defense_turn_count = (int)(rs["player_data"] as Dictionary<string, object>)["current_defense_turn_count"];
                    attack_turn_count = (int)(rs["player_data"] as Dictionary<string, object>)["attack_turn_count"];
                }
            }
            setStatus("\nEND : Nuốt quân lệnh / cờ chiến");
            writeGlobalInfo();
        }
        #endregion

        #region - defence_city -
        private void defence_city()
        {
            txtStatus.Text = string.Empty;
            setStatus("START : Thủ Thành \n");
            defense_next_wave();
            writeGlobalInfo();
            setStatus("END : Thủ Thành \n");
        }

        private void defense_next_wave()
        {
            if (current_wave == -1)
            { //Khong co o thanh nao
                Dictionary<string, object> lastcity = null;
                foreach (Dictionary<string, object> city in (dataLogin["city_conquest_statuses"] as ArrayList))
                {
                    if ((int)city["unlocked_star"] == 0)
                    {
                        break;
                    }
                    else
                    {
                        lastcity = city;
                    }
                }
                defense_city(lastcity["city_id"].ToString(), (int)lastcity["unlocked_star"]);
                login();
                defense_next_wave();
                return;
            }
            if (current_wave == total_wave_count)
            {
                if (chkChayHetQL.Checked)
                {
                    leave_defense_city();
                    defense_city(city_id, star);
                    defense_next_wave();
                    return;
                }
                else
                {
                    setStatus("Đánh xong thành");
                    return;
                }
            }

            if (current_defense_turn_count < star) { setStatus("Hết quân lệnh"); return; };

            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            client.DoPost(param, "https://vtt-01.zoygame.com/players/" + id + "/defense_next_wave");
            if (client.ResponseText != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> rs = serializer.Deserialize<Dictionary<string, object>>(client.ResponseText);

                current_defense_turn_count = (int)rs["current_defense_turn_count"];
                System.Threading.Thread.Sleep(500);

                finish_wave();
            }
        }

        private void finish_wave()
        {

            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            param.Add("killed_city_officer", current_wave == total_wave_count ? "1" : "0");
            param.Add("escaped_enemies_count", "0");
            param.Add("killed_animal", current_wave == total_wave_count ? "0" : "1");

            for (int iIndex = 0; iIndex < 5; iIndex++)
            {
                new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(finish_wave)).Start(param);
            }

            System.Threading.Thread.Sleep(1500);

            defense_next_wave();
        }

        private void finish_wave(object param)
        {
            WebClientEx client = new WebClientEx();
            client.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/players/" + id + "/finish_wave");
            if (client.ResponseText != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> rs = serializer.Deserialize<Dictionary<string, object>>(client.ResponseText);

                current_wave = (int)((rs["player"] as Dictionary<string, object>)["current_defense_battle"] as Dictionary<string, object>)["current_wave"] + 1;
            }
        }

        private void leave_defense_city()
        {
            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            client.DoPost(param, "https://vtt-01.zoygame.com/players/" + id + "/leave_defense_city");
        }

        private void defense_city(string city_id, int star)
        {
            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            param.Add("city_id", city_id);
            param.Add("star", star.ToString());
            param.Add("is_retrying", "0");
            client.DoPost(param, "https://vtt-01.zoygame.com/players/" + id + "/defense_city");
            if (client.ResponseText != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Dictionary<string, object> rs = serializer.Deserialize<Dictionary<string, object>>(client.ResponseText);
                current_defense_battle = rs["current_defense_battle"] as Dictionary<string, object>;

                ArrayList old_deployed_officer_coordinates = rs["old_deployed_officer_coordinates"] as ArrayList;
                foreach (Dictionary<string, object> officer_coordinate in old_deployed_officer_coordinates)
                {
                    client = new WebClientEx();
                    param = new NameValueCollection();
                    string strURL = "coordinate%5B%5D={0}&coordinate%5B%5D={1}&owned_officer_id={2}&authentication_token={3}";
                    strURL = string.Format(strURL, (officer_coordinate["coordinate"] as List<int>)[0].ToString(), (officer_coordinate["coordinate"] as List<int>)[1].ToString(), officer_coordinate["owned_officer_id"], authentication_token);
                    client.DoPost(strURL, "https://vtt-01.zoygame.com/players/" + id + "/update_owned_officer_coordinate", null, "PUT");
                }
            }
        }
        #endregion

        #region - fire -
        private void fire()
        {
            try
            {
                this.btnSaThaiTuong.Enabled = false;
                foreach (Dictionary<string, object> officer in (dataLogin["owned_officers"] as ArrayList))
                {
                    bool bFire = false;
                    foreach (Dictionary<string, object> component in (officer["components"] as ArrayList))
                    {
                        if (component.ContainsKey("rank") && (int)component["rank"] <= 2)
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
                string strURL = "city_id={0}&authentication_token={1}&star={2}";
                strURL = string.Format(strURL, txtVuotAiTenAi.SelectedValue.ToString(), authentication_token, txtVuotAiDoKho.Text.Trim());
                foreach (Dictionary<string, object> item in (dataLogin["current_defense_battle"] as Dictionary<string, object>)["deployed_officer_coordinates"] as ArrayList)
                {
                    strURL += "&formation%5B%5D=" + item["owned_officer_id"].ToString();
                }
                client.DoPost(strURL, "https://vtt-01.zoygame.com/players/" + id + "/attack_city");
                if (client.ResponseText != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Dictionary<string, object> rs = serializer.Deserialize<Dictionary<string, object>>(client.ResponseText);

                    current_defense_turn_count = (int)rs["current_defense_turn_count"];
                    attack_turn_count = (int)rs["attack_turn_count"];

                    System.Threading.Thread.Sleep(500);
                    client = new WebClientEx();
                    NameValueCollection param = new NameValueCollection();
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
                                foreach (Dictionary<string, object> item in (dataLogin["owned_items"] as ArrayList))
                                {
                                    if (item["name"].ToString() == "capture_card_01")
                                    {
                                        capture_card_id = item["id"].ToString();
                                        if ((int)item["quantity"] <= 0)
                                        {
                                            setStatus("Hết thẻ chiêu hàng");
                                            return;
                                        }
                                        item["quantity"] = (int)item["quantity"] - 1;
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
                                setStatus("ĐÃ bắt được tướng");
                            }
                            else
                            {
                                setStatus("KHÔNG bắt được tướng");
                            }

                        }
                        else
                        {
                            for (int iIndex = 0; iIndex < 6; iIndex++)
                            {
                                new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(retrieve_reward_for_last_won_battle)).Start(param);
                            }
                            System.Threading.Thread.Sleep(2000);
                            attack_city();
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
                    Dictionary<string, object> rs = serializer.Deserialize<Dictionary<string, object>>(client.ResponseText);

                    param = new NameValueCollection();
                    param.Add("authentication_token", authentication_token);
                    param.Add("won_floor", bDone ? "0" : "1");
                    param.Add("floor_npc_id", rs["floor_npc_id"].ToString());
                    string strRD = new Random().Next(1, 99).ToString();
                    param.Add("remaining_hp", bDone ? "0" : (tower_floor + 2) % 5 == 0 ? ("82." + strRD) : ("45." + strRD));

                    for (int iIndex = 0; iIndex < 6; iIndex++)
                    {
                        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(finish_attack_tower)).Start(param);
                    }
                    System.Threading.Thread.Sleep(1000);
                    if (bDone) break;
                    if (tower_achievement >= 360 || tower_floor >= 104)
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
                Dictionary<string, object> dataRS = serializer.Deserialize<Dictionary<string, object>>(clientfat.ResponseText);

                tower_achievement = (int)(dataRS["tower_data"] as Dictionary<string, object>)["tower_achievement"];
                tower_floor = (int)(dataRS["tower_data"] as Dictionary<string, object>)["tower_floor"];
            }
        }
        #endregion

        #region - reduce_cooldown -
        private void reduce_cooldown()
        {
            List<string> listTuongUpLevelBangVang = new List<string> { "HoangCai", "TonLoDuc", "TienDung", "ChuDongTu", "BaTrieu" };
            List<Dictionary<string, object>> listTuongUp = new List<Dictionary<string, object>>();
            foreach (Dictionary<string, object> officer in (dataLogin["owned_officers"] as ArrayList))
            {
                if (listTuongUpLevelBangVang.Contains(officer["officer_id"].ToString()))
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

            {
                //if (listTuongUp.Count == 5)
                //{
                //    listTuongUp.ForEach(x => { new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(unlock_skill_2)).Start(x); });
                //}
            }
        }

        private void reduce_cooldown(object officer)
        {
            Dictionary<string, object> of = officer as Dictionary<string, object>;
            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            param.Add("hours", "1");
            param.Add("cooldown_type", "LevelComp");
            param.Add("is_skill_2", "false");
            client.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/owned_officers/" + of["id"] + "/reduce_cooldown");
            if (!string.IsNullOrEmpty(client.ResponseText))
            {
                Dictionary<string, object> dataRS = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue }.Deserialize<Dictionary<string, object>>(client.ResponseText);
                foreach (Dictionary<string, object> component in (of["components"] as ArrayList))
                {
                    if (component.ContainsKey("leveled_up_at"))
                    {
                        component["leveled_up_at"] = (dataRS["component_data"] as Dictionary<string, object>)["leveled_up_at"];
                    }
                }
            }
        }

        private void unlock_stone_2(object officer)
        {
            Dictionary<string, object> of = officer as Dictionary<string, object>;
            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            client.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/owned_officers/" + of["id"] + "/unlock_stone_2");
        }

        private void unlock_skill_2(object officer)
        {
            Dictionary<string, object> of = officer as Dictionary<string, object>;
            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            client.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/owned_officers/" + of["id"] + "/unlock_skill_2");
        }

        private void equip_stone()
        {

        }

        #endregion

        #region - levelup -
        private void levelup()
        {
            List<Dictionary<string, object>> listTuongUp = new List<Dictionary<string, object>>();
            foreach (Dictionary<string, object> officer in (dataLogin["owned_officers"] as ArrayList))
            {
                DateTime timeLevelup = DateTime.MaxValue;
                int iLevel = 60;
                int iRank = 3;
                foreach (Dictionary<string, object> component in (officer["components"] as ArrayList))
                {
                    if (component.ContainsKey("leveled_up_at"))
                    {
                        timeLevelup = new DateTime(1970, 1, 1).AddSeconds(Convert.ToDouble(component["leveled_up_at"])).AddHours(7).AddSeconds(Convert.ToDouble(component["cooldown_time_to_next_level"]));
                    }
                    if (component.ContainsKey("level"))
                    {
                        iLevel = (int)component["level"];
                    }
                    if (component.ContainsKey("rank"))
                    {
                        iRank = (int)component["rank"];
                    }
                }
                if (DateTime.Now <= timeLevelup || (iLevel < 50 && iRank < 3))
                {
                }
                else
                {
                    listTuongUp.Add(officer);
                }

            }
            if (listTuongUp.Count > 0)
            {
                login();
                listTuongUp.ForEach(x => new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(levelup)).Start(x));
                System.Threading.Thread.Sleep(2000);
                //login();
            }
        }

        private void levelup(object officer)
        {
            Dictionary<string, object> of = officer as Dictionary<string, object>;
            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            client.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/owned_officers/" + of["id"] + "/level_up");
            if (!string.IsNullOrEmpty(client.ResponseText))
            {
                Dictionary<string, object> dataRS = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue }.Deserialize<Dictionary<string, object>>(client.ResponseText);
                foreach (Dictionary<string, object> component in (of["components"] as ArrayList))
                {
                    if (component.ContainsKey("leveled_up_at"))
                    {
                        component["leveled_up_at"] = (dataRS["level_comp"] as Dictionary<string, object>)["leveled_up_at"];
                        component["cooldown_time_to_next_level"] = (dataRS["level_comp"] as Dictionary<string, object>)["cooldown_time_to_next_level"];
                    }
                }
            }
        }
        #endregion

        #region - login -
        private bool login()
        {
            //if (dataLogin != null)
            //{
            //    WebClientEx client = new WebClientEx();
            //    client.DoGet("https://vtt-01.zoygame.com/bulletins?authentication_token=" + authentication_token);
            //    if (client.Error != null)
            //    {
            //        dataLogin = null;
            //    }
            //    else
            //    {
            //        return true;
            //    }
            //}
            //if (dataLogin == null)
            {
                string strProfile = System.Configuration.ConfigurationSettings.AppSettings["login"];
                Dictionary<string, object> dataProfile = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(profileJson);

                WebClientEx client = new WebClientEx();
                NameValueCollection param = new NameValueCollection();

                param.Add("access_token", (dataProfile[strProfile] as Dictionary<string, object>)["access_token"].ToString());
                param.Add("user_name", (dataProfile[strProfile] as Dictionary<string, object>)["user_name"].ToString());
                param.Add("user_id", (dataProfile[strProfile] as Dictionary<string, object>)["user_id"].ToString());
                param.Add("user_status", (dataProfile[strProfile] as Dictionary<string, object>)["user_status"].ToString());
                param.Add("avatar_img_link", (dataProfile[strProfile] as Dictionary<string, object>)["avatar_img_link"].ToString());

                client.DoPost(param, "https://vtt-01.zoygame.com/players/sign_in");

                if (client.ResponseText != null)
                {
                    dataLogin = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue }.Deserialize<Dictionary<string, object>>(client.ResponseText);
                    Utilities.SerializeObject("login.data", dataLogin);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region - Default data -
        private void buildDefaultDataOnLoad()
        {
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
            {
                DataTable dtQLCC = new DataTable();
                dtQLCC.Columns.Add("id", typeof(string));
                dtQLCC.Columns.Add("name", typeof(string));
                dtQLCC.Rows.Add("defense_turn_count_restore_00", "QL sơn trại");
                dtQLCC.Rows.Add("defense_turn_count_restore_01", "QL sơ cấp");
                dtQLCC.Rows.Add("defense_turn_count_restore_02", "QL trung cấp");
                dtQLCC.Rows.Add("defense_turn_count_restore_03", "QL cao cấp");
                dtQLCC.Rows.Add("attack_turn_count_restore_01", "CC sơ cấp");
                dtQLCC.Rows.Add("attack_turn_count_restore_02", "CC trung cấp");
                dtQLCC.Rows.Add("attack_turn_count_restore_03", "CC cao cấp");
                txtQLCC.DataSource = dtQLCC;
                txtQLCC.DisplayMember = "name";
                txtQLCC.ValueMember = "id";
                txtQLCC.SelectedIndex = -1;
                txtQLCC.SelectedValueChanged += (objs, pbje) =>
                {
                    string strValue = txtQLCC.SelectedValue.ToString();
                    if (dataLogin != null)
                    {
                        foreach (Dictionary<string, object> item in (dataLogin["owned_items"] as ArrayList))
                        {
                            if (item["name"].ToString() == strValue)
                            {
                                int soluong = Convert.ToInt32(item["quantity"]);
                                if (soluong > 1)
                                {
                                    soluong--;
                                }
                                txtDungQLCC_SoLuong.Text = soluong.ToString();
                                break;
                            }
                        }
                    }
                };
                txtQLCC.SelectedIndex = 0;
            }
        }

        private void writeGlobalInfo()
        {
            setStatus("Quân Lệnh : " + current_defense_turn_count);
            setStatus("Cờ Chiến : " + attack_turn_count);
            setStatus("Đợt : " + current_wave);
            setStatus("-----Quân lệnh-----");
            var t = dataLogin["owned_items"].GetType();
            foreach (Dictionary<string, object> item in (dataLogin["owned_items"] as ArrayList))
            {
                if (item["name"].ToString() == "defense_turn_count_restore_00")
                {
                    setStatus("QL sơn trại : " + item["quantity"]);
                }
                else if (item["name"].ToString() == "defense_turn_count_restore_01")
                {
                    setStatus("QL sơ cấp : " + item["quantity"]);
                }
                else if (item["name"].ToString() == "defense_turn_count_restore_02")
                {
                    setStatus("QL trung cấp : " + item["quantity"]);
                }
                else if (item["name"].ToString() == "defense_turn_count_restore_03")
                {
                    setStatus("QL cao cấp : " + item["quantity"]);
                }
            }
            setStatus("-----Cờ chiến-----");
            foreach (Dictionary<string, object> item in (dataLogin["owned_items"] as ArrayList))
            {
                if (item["name"].ToString() == "attack_turn_count_restore_01")
                {
                    setStatus("CC sơ cấp : " + item["quantity"]);
                }
                else if (item["name"].ToString() == "attack_turn_count_restore_02")
                {
                    setStatus("CC trung cấp : " + item["quantity"]);
                }
                else if (item["name"].ToString() == "attack_turn_count_restore_03")
                {
                    setStatus("CC cao cấp : " + item["quantity"]);
                }
            }
        }

        private void setStatus(string strStatus)
        {
            txtStatus.Text += "\r\n" + DateTime.Now.ToString("HH:mm:ss") + "    " + strStatus;
            //move the caret to the end of the text
            txtStatus.SelectionStart = txtStatus.TextLength;
            //scroll to the caret
            txtStatus.ScrollToCaret();
        }
        #endregion
        #endregion
    }
}
