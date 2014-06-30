using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.Web.Services;

namespace VuaThuThanh
{
    public partial class Default : System.Web.UI.Page
    {
        string profileJson = @"{'tin':{'access_token':'dm5pX3Rva2VuPTAuODE1NzM1MDAgMTQwMjM3NzkyMSsxNzk2NjIzMzgx','user_name':'vt89qn','user_id':'823493150','user_status':'1','avatar_img_link':'http://avatar.my.soha.vn/80/vt89qn.png'}
                              ,'thi':{'access_token':'dm5pX3Rva2VuPTAuNTMwOTg3MDAgMTQwMjMxMDk5MSs3NDc4MTU2NjA=','user_name':'Pole','user_id':'91031','user_status':'1','avatar_img_link':'http://avatar.my.soha.vn/80/Pole.png'}}";
        WebClientEx client = new WebClientEx();
        string authentication_token = string.Empty;
        string id = string.Empty;
        int current_wave = 1;
        int attack_turn_count = 1;
        int current_defense_turn_count = 30;
        int star = 3;
        int total_wave_count = 60;
        dynamic data;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region - Đăng nhập -
        protected void btnLogin_Click(object sender, DirectEventArgs e)
        {
            string strProfile = txtProfile.Value.ToString();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic dataProfile = serializer.Deserialize<dynamic>(profileJson);
            if (dataProfile[strProfile] != null)
            {
                client = new WebClientEx();
                NameValueCollection param = new NameValueCollection();
                param.Add("access_token", dataProfile[strProfile]["access_token"]);
                param.Add("user_name", dataProfile[strProfile]["user_name"]);
                param.Add("user_id", dataProfile[strProfile]["user_id"]);
                param.Add("user_status", dataProfile[strProfile]["user_status"]);
                param.Add("avatar_img_link", dataProfile[strProfile]["avatar_img_link"]);
                client.DoPost(param, "https://vtt-01.zoygame.com/players/sign_in");

                if (client.ResponseText != null)
                {
                    serializer = new JavaScriptSerializer();
                    dynamic rs = serializer.Deserialize<dynamic>(client.ResponseText);
                    Session["data"] = rs;
                    writeCurrentInfo();
                    WindowLogin.Hide();
                    WindowWorking.Show();

                }
            }
        }
        #endregion

        #region - Thủ Thành -
        protected void btnChayQuanLenh_Click(object sender, DirectEventArgs e)
        {
            txtStatus.Text = string.Empty;
            setStatus("START : Chạy quân lệnh\n");
            readSession();
            defense_next_wave();
            writeCurrentInfo();
            setStatus("\nEND : Chạy quân lệnh");
        }

        private void defense_next_wave()
        {
            writeSession();
            if (current_wave >= total_wave_count) { setStatus("Đánh xong thành"); return; };
            if (current_defense_turn_count < star) { setStatus("Hết quân lệnh"); return; };

            client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            client.DoPost(param, "https://vtt-01.zoygame.com/players/" + id + "/defense_next_wave");
            if (client.ResponseText != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic rs = serializer.Deserialize<object>(client.ResponseText);

                current_defense_turn_count = (int)rs["current_defense_turn_count"];
                System.Threading.Thread.Sleep(500);

                finish_wave();
            }
        }

        private void finish_wave()
        {
            client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            param.Add("killed_city_officer", current_wave == total_wave_count ? "1" : "0");
            param.Add("escaped_enemies_count", "0");
            param.Add("killed_animal", current_wave == total_wave_count ? "0" : "1");
            client.DoPost(param, "https://vtt-01.zoygame.com/players/" + id + "/finish_wave");
            if (client.ResponseText != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic rs = serializer.Deserialize<object>(client.ResponseText);

                current_wave = rs["player"]["current_defense_battle"]["current_wave"] + 1;

                System.Threading.Thread.Sleep(500);
                defense_next_wave();
            }
        }
        #endregion

        #region - Leo Tháp -
        protected void btnLeoThap_Click(object sender, DirectEventArgs e)
        {
            txtStatus.Text = string.Empty;
            setStatus("START : Leo Tháp\n");
            readSession();
            bool bDone = false;
            while (true)
            {
                client = new WebClientEx();
                NameValueCollection param = new NameValueCollection();
                param.Add("authentication_token", authentication_token);
                client.DoPost(param, "https://vtt-01.zoygame.com/players/" + id + "/attack_next_tower_floor");
                if (client.ResponseText != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    dynamic rs = serializer.Deserialize<object>(client.ResponseText);

                    client = new WebClientEx();
                    param = new NameValueCollection();
                    param.Add("authentication_token", authentication_token);
                    param.Add("won_floor", bDone ? "0" : "1");
                    param.Add("floor_npc_id", rs["floor_npc_id"]);
                    param.Add("remaining_hp", bDone ? "0" : ("81." + (new Random().Next(1, 99))));
                    client.DoPost(param, "https://vtt-01.zoygame.com/players/" + id + "/finish_attack_tower");
                    if (client.ResponseText != null)
                    {
                        if (bDone) break;
                        serializer = new JavaScriptSerializer();
                        rs = serializer.Deserialize<object>(client.ResponseText);
                        if (rs["tower_data"]["tower_achievement"] >= 200)
                        {
                            bDone = true;
                        }
                    }
                }
            }

            writeCurrentInfo();
            setStatus("\nEND : Leo Tháp");
        }
        #endregion

        #region - Nuốt quân lệnh / Cờ chiến -
        protected void btnNuotQuanLenh_Click(object sender, DirectEventArgs e)
        {
            txtStatus.Text = string.Empty;
            setStatus("START : Nuốt quân lệnh / cờ chiến\n");
            readSession();
            string loaiquanlenh = txtLoaiQuanLenh.Value.ToString();
            string item_id = string.Empty;
            int isl = -1;
            int islrequest = int.Parse(txtSoluongQL.Text.Trim());
            dynamic itemselect = null;
            foreach (dynamic item in data["owned_items"])
            {
                if (item["name"] == loaiquanlenh)
                {
                    isl = item["quantity"];
                    item_id = item["id"];
                    itemselect = item;
                    break;
                }
            }
            if (string.IsNullOrEmpty(item_id))
            {
                X.Msg.Alert("Vua thủ thành", "không tìm thấy item").Show();
            }
            else if (isl < islrequest)
            {
                X.Msg.Alert("Vua thủ thành", "số lượng không đủ").Show();
            }
            else
            {
                client = new WebClientEx();
                NameValueCollection param = new NameValueCollection();
                param.Add("owned_item_id", item_id);
                param.Add("quantity", islrequest.ToString());
                param.Add("authentication_token", authentication_token);
                client.DoPost(param, "https://vtt-01.zoygame.com/owned_items/use_item");
                if (client.ResponseText != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    dynamic rs = serializer.Deserialize<dynamic>(client.ResponseText);
                    itemselect["quantity"] = itemselect["quantity"] - islrequest;

                    current_defense_turn_count = rs["player_data"]["current_defense_turn_count"];
                    attack_turn_count = rs["player_data"]["attack_turn_count"];
                    writeSession();
                    writeCurrentInfo();
                }
            }
            setStatus("\nEND : Nuốt quân lệnh / cờ chiến");
        }
        #endregion

        #region - Vượt ải -
        protected void btnVuotAi_Click(object sender, DirectEventArgs e)
        {
            readSession();
            if (attack_turn_count < int.Parse(txtDoKhoAi.Value.ToString().Trim()))
            {
                X.Msg.Alert("Vua thủ thành", "hết cờ chiến").Show();
                return;
            }

            txtStatus.Text = string.Empty;
            setStatus("START : Vượt ải\n");


            string strtenai = txtTenAi.Value.ToString();
            string strdokho = txtDoKhoAi.Value.ToString();



            client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            param.Add("city_id", txtTenAi.Value.ToString());
            param.Add("star", txtDoKhoAi.Value.ToString());
            foreach (dynamic item in data["current_defense_battle"]["deployed_officer_coordinates"])
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
                        foreach (dynamic item in data["owned_items"])
                        {
                            if (item["name"] == "capture_card_01")
                            {
                                capture_card_id = item["id"];
                                break;
                            }
                        }
                    }
                    client = new WebClientEx();
                    param = new NameValueCollection();
                    param.Add("authentication_token", authentication_token);
                    param.Add("reward_type", "officer");
                    param.Add("game_mode", "attack");
                    param.Add("capture_card_id", capture_card_id);
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
                writeSession();
                writeCurrentInfo();
            }

            setStatus("\nEND : Vượt ải");
        }

        #endregion

        #region - Đuổi tướng -
        protected void btnDuoiTuong_Click(object sender, DirectEventArgs e)
        {
            txtStatus.Text = string.Empty;
            setStatus("START : Đuổi tướng\n");
            readSession();

            foreach (dynamic officer in data["owned_officers"])
            {
                bool bFire = false;
                int iLevel = 50;
                foreach (dynamic component in officer["components"])
                {
                    if (component.ContainsKey("level"))
                    {
                        iLevel = component["level"];
                        if (iLevel > 1)
                        {
                            bFire = false;
                            break;
                        }
                    }
                    if (component.ContainsKey("rank") && component["rank"] <= 2)
                    {
                        bFire = true;
                    }
                }
                if (bFire)
                {
                    for (int iIndex = 0; iIndex < 7; iIndex++)
                    {
                        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(fire)).Start(officer["id"]);
                    }
                    System.Threading.Thread.Sleep(5000);
                }
            }

            writeCurrentInfo();
            setStatus("\nEND : Đuổi tướng");
        }

        private void fire(object id)
        {
            WebClientEx we = new WebClientEx();
            we.DoGet("https://vtt-01.zoygame.com/owned_officers/" + id + "/fire?authentication_token=" + authentication_token);
        }
        #endregion

        #region - Ghép mảnh tướng -
        protected void btnGhepManhTuong_Click(object sender, DirectEventArgs e)
        {
            readSession();
            windowGhepManhTuong.Show();
            txtManhTuongDaChon.Text = string.Empty;
            Session["mt"] = new Dictionary<string, int>();

            Dictionary<string, string> dicManhTuong = new Dictionary<string, string>();

            foreach (dynamic owned_officer_soul in data["owned_officer_souls"])
            {
                if (owned_officer_soul["quantity"] > 0)
                {
                    dicManhTuong.Add(owned_officer_soul["id"], owned_officer_soul["officer_id"] + "(" + owned_officer_soul["quantity"] + ")");
                }
            }
            List<object> dataManhTuong = new List<object>();
            foreach (KeyValuePair<string, string> item in dicManhTuong.OrderBy(key => key.Value))
            {
                dataManhTuong.Add(new { manhtuong_id = item.Key, manhtuong_name = item.Value });
            }
            manhTuongStore.DataSource = dataManhTuong;
            manhTuongStore.DataBind();
        }
        protected void btnChonManhTuong_Click(object sender, DirectEventArgs e)
        {
            readSession();
            txtManhTuongDaChon.Text = string.Empty;
            Dictionary<string, int> dicManhTuong = Session["mt"] as Dictionary<string, int>;
            dicManhTuong.Add(txtChonManhTuong.Value.ToString(), int.Parse(txtSoLuongManhTuong.Text.Trim()));

            foreach (KeyValuePair<string, int> item in dicManhTuong)
            {
                foreach (dynamic owned_officer_soul in data["owned_officer_souls"])
                {
                    if (owned_officer_soul["id"] == item.Key)
                    {
                        txtManhTuongDaChon.Text += owned_officer_soul["officer_id"] + " : " + item.Value + "\r\n";
                        break;
                    }
                }
            }
        }
        protected void btnGhepManhTuongHuy_Click(object sender, DirectEventArgs e)
        {
            windowGhepManhTuong.Hide();
        }
        protected void btnGhepManhTuongOK_Click(object sender, DirectEventArgs e)
        {
            readSession();
            windowGhepManhTuong.Hide();
            //check
            int iTongManhTuong = 0;
            foreach (KeyValuePair<string, int> item in Session["mt"] as Dictionary<string, int>)
            {
                iTongManhTuong += item.Value;

                foreach (dynamic owned_officer_soul in data["owned_officer_souls"])
                {
                    if (owned_officer_soul["id"] == item.Key)
                    {
                        if (owned_officer_soul["quantity"] < item.Value)
                        {
                            X.Msg.Alert("Vua thủ thành", "Mảnh tướng " + owned_officer_soul["officer_id"] + " éo đủ").Show();
                            return;
                        }
                        else
                        {
                            owned_officer_soul["quantity"] = owned_officer_soul["quantity"] - item.Value;
                        }
                    }
                }
            }
            if (iTongManhTuong != 5)
            {
                X.Msg.Alert("Vua thủ thành", "số lượng không hợp lệ").Show();
                return;
            }
            string strURL = "https://vtt-01.zoygame.com/owned_officer_souls/merge?authentication_token=" + authentication_token;
            foreach (KeyValuePair<string, int> item in Session["mt"] as Dictionary<string, int>)
            {
                for (int iIndex = 0; iIndex < item.Value; iIndex++)
                {
                    strURL += "&owned_officer_soul_ids%5B%5D=" + item.Key;
                }
            }

            for (int iIndex = 0; iIndex < 7; iIndex++)
            {
                new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(merge)).Start(strURL);
            }

            writeSession();
        }

        private void merge(object url)
        {
            WebClientEx we = new WebClientEx();
            we.DoGet(url.ToString());
        }

        #endregion

        #region - Ghép vũ khí / linh thạch -
        protected void btnGhepDo_Click(object sender, DirectEventArgs e)
        {
            txtStatus.Text = string.Empty;
            setStatus("START : Ghép đồ\n");
            readSession();

            string item_id = string.Concat(cmbLoai.Value, "_", cmbCap.Value);
            List<string> lstItems = new List<string>();
            int soluong = Convert.ToInt16(cmbSoLuong.Value);
            foreach (dynamic item in data["owned_items"])
            {
                if (item["item_id"] == item_id)
                {
                    lstItems.Add(item["id"]);

                }
            }
            //send
            if (lstItems.Count >= 3)
            {
                for (int i = 0; i < lstItems.Count - 3; i += 3)
                {
                    if (soluong != 0 && i >= soluong * 3)
                    {
                        setStatus(string.Concat("\nEND : Đã ghép xong ", cmbLoai.Text, " ", cmbCap.Text));
                        setStatus("\nEND : Ghép đồ");
                        return;
                    }
                    NameValueCollection param = new NameValueCollection();
                    param.Add("authentication_token", authentication_token);
                    param.Add("owned_item_count", "3");
                    for (int t = 0; t < 3; t++)
                    {
                        param.Add(string.Concat("owned_item_id_", t), lstItems[i + t]);
                    }

                    for (int iIndex = 0; iIndex < 7; iIndex++)
                    {
                        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(forge)).Start(param);
                    }
                    System.Threading.Thread.Sleep(5000);
                }
                setStatus(string.Concat("\nEND : Đã ghép xong ", cmbLoai.Text, " ", cmbCap.Text));
            }
            else
            {
                setStatus(string.Concat("\nEND : Không đủ ", cmbLoai.Text, " ", cmbCap.Text));
            }

            setStatus("\nEND : Ghép đồ");
        }

        private void forge(object param)
        {
            WebClientEx client = new WebClientEx();
            client.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/owned_items/forge");
        }
        #endregion

        #region - Common -
        void readSession()
        {
            data = Session["data"];
            authentication_token = data["authentication_token"];
            current_defense_turn_count = data["current_defense_turn_count"];
            current_wave = data["current_defense_battle"]["current_wave"] + 1;
            star = data["current_defense_battle"]["star"];
            total_wave_count = data["current_defense_battle"]["total_wave_count"];
            attack_turn_count = data["attack_turn_count"];
            id = data["id"];
        }

        void writeSession()
        {
            data = Session["data"];
            data["authentication_token"] = authentication_token;
            data["current_defense_turn_count"] = current_defense_turn_count;
            data["current_defense_battle"]["current_wave"] = current_wave - 1;
            data["current_defense_battle"]["star"] = star;
            data["current_defense_battle"]["total_wave_count"] = total_wave_count;
            data["attack_turn_count"] = attack_turn_count;
            Session["data"] = data;
        }

        private void writeCurrentInfo()
        {
            data = Session["data"];
            setStatus("Quân Lệnh : " + data["current_defense_turn_count"]);
            setStatus("Cờ Chiến : " + data["attack_turn_count"]);
            setStatus("Đợt : " + (data["current_defense_battle"]["current_wave"] + 1));
            setStatus("-----Quân lệnh-----");
            foreach (dynamic item in data["owned_items"])
            {
                if (item["name"] == "defense_turn_count_restore_00")
                {
                    setStatus("QL sơn trại : " + item["quantity"]);
                }
                else if (item["name"] == "defense_turn_count_restore_01")
                {
                    setStatus("QL sơ cấp : " + item["quantity"]);
                }
                else if (item["name"] == "defense_turn_count_restore_02")
                {
                    setStatus("QL trung cấp : " + item["quantity"]);
                }
                else if (item["name"] == "defense_turn_count_restore_03")
                {
                    setStatus("QL cao cấp : " + item["quantity"]);
                }
            }
            setStatus("-----Cờ chiến-----");
            foreach (dynamic item in data["owned_items"])
            {
                if (item["name"] == "attack_turn_count_restore_01")
                {
                    setStatus("CC sơ cấp : " + item["quantity"]);
                }
                else if (item["name"] == "attack_turn_count_restore_02")
                {
                    setStatus("CC trung cấp : " + item["quantity"]);
                }
                else if (item["name"] == "attack_turn_count_restore_03")
                {
                    setStatus("CC cao cấp : " + item["quantity"]);
                }
            }
        }

        private void setStatus(string strStatus)
        {
            txtStatus.Text += "\n" + strStatus;
        }
        #endregion
    }
}