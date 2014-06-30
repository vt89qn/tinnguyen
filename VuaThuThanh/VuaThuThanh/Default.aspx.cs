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
                              ,'thi':{}}";
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

        protected void btnChayQuanLenh_Click(object sender, DirectEventArgs e)
        {
            txtStatus.Text = string.Empty;
            setStatus("START : Chạy quân lệnh\n");
            readSession();
            defense_next_wave();
            writeCurrentInfo();
            setStatus("\nEND : Chạy quân lệnh");
        }

        protected void btnLeoThaph_Click(object sender, DirectEventArgs e)
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

    }
}