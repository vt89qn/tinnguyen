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
using System.Timers;
using System.Data;
using System.IO;

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
        string city_id = string.Empty;
        int total_wave_count = 60;
        int tower_achievement = 10;
        int tower_floor = 0;
        dynamic data;
        static System.Timers.Timer aTimer;

        string strLoginData = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region - Đăng nhập -
        protected void btnLogin_Click(object sender, DirectEventArgs e)
        {
            login();
            writeCurrentInfo();
            WindowLogin.Hide();
            WindowWorking.Show();
        }
        protected void login()
        {
            string strProfile = txtProfile.Value.ToString();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic dataProfile = serializer.Deserialize<dynamic>(profileJson);
            if (dataProfile[strProfile] != null)
            {
                WebClientEx client = new WebClientEx();
                NameValueCollection param = new NameValueCollection();
                param.Clear();
                param.Add("access_token", dataProfile[strProfile]["access_token"]);
                param.Add("user_name", dataProfile[strProfile]["user_name"]);
                param.Add("user_id", dataProfile[strProfile]["user_id"]);
                param.Add("user_status", dataProfile[strProfile]["user_status"]);
                param.Add("avatar_img_link", dataProfile[strProfile]["avatar_img_link"]);
                client.DoPost(param, "https://vtt-01.zoygame.com/players/sign_in");

                if (client.ResponseText != null)
                {
                    serializer = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue };
                    dynamic rs = serializer.Deserialize<dynamic>(client.ResponseText);
                    Session["data"] = rs;
                }
            }
            writeFileTxt();
            //setTimeLevelUp();
        }

        private void writeFileTxt()
        {
            DataTable tblHeroes = GetDataTableFromCSV();
            if (tblHeroes.Rows.Count > 0)
            {
                readSession();
                string newHeroes = string.Empty;
                foreach (dynamic owned_officer_soul in data["owned_officer_souls"])
                {
                    DataRow[] rowCheck = tblHeroes.Select(string.Format("name = '{0}'", owned_officer_soul["officer_id"]));
                    if (rowCheck.Length == 0)
                    {
                        newHeroes = string.Concat(newHeroes, Environment.NewLine, string.Format("{0},{1},{2},{3}", owned_officer_soul["officer_id"], "0", "0", ""));
                    }
                }
                if (newHeroes != string.Empty)
                {
                    string path = HttpContext.Current.Server.MapPath("heroes.txt");
                    //new
                    if (!File.Exists(path))
                    {
                        // Create a file to write to. 
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine(newHeroes);
                            sw.Close();
                        }
                    }
                    //modify
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(newHeroes);
                        sw.Close();
                    }
                }
            }
        }
        #endregion

        #region - Thủ Thành -
        protected void btnChayQuanLenh_Click(object sender, DirectEventArgs e)
        {
            chayQuanLenh();
        }

        private void chayQuanLenh()
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
            if (current_wave == total_wave_count)
            {
                setStatus("Đánh xong thành");
                //leave_defense_city();
                //defense_city(city_id, star.ToString());
                return;
            }

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
            client = new WebClientEx();
            client.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/players/" + id + "/finish_wave");
            if (client.ResponseText != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic rs = serializer.Deserialize<object>(client.ResponseText);

                current_wave = rs["player"]["current_defense_battle"]["current_wave"] + 1;
            }
        }

        private void leave_defense_city()
        {
            client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            client.DoPost(param, "https://vtt-01.zoygame.com/players/" + id + "/leave_defense_city");
        }

        private void defense_city(string city_id, string star)
        {
            client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            param.Add("city_id", city_id);
            param.Add("star", star);
            param.Add("is_retrying", "0");
            client.DoPost(param, "https://vtt-01.zoygame.com/players/" + id + "/defense_city");
            if (client.ResponseText != null)
            {
                chayQuanLenh();
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

                    param = new NameValueCollection();
                    param.Add("authentication_token", authentication_token);
                    param.Add("won_floor", bDone ? "0" : "1");
                    param.Add("floor_npc_id", rs["floor_npc_id"]);
                    string strRD = new Random().Next(1, 99).ToString();
                    param.Add("remaining_hp", bDone ? "0" : tower_floor <= 25 ? ("5." + strRD) : ("82." + strRD));

                    for (int iIndex = 0; iIndex < 6; iIndex++)
                    {
                        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(finish_attack_tower)).Start(param);
                    }

                    System.Threading.Thread.Sleep(1000);
                    if (bDone) break;
                    if (tower_achievement >= 295)
                    {
                        bDone = true;
                    }
                }
                else { setStatus("Leo chua xong,login leo tiep"); break; }
            }

            writeCurrentInfo();
            setStatus("\nEND : Leo Tháp");
        }

        private void finish_attack_tower(object param)
        {
            WebClientEx clientfat = new WebClientEx();
            clientfat.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/players/" + id + "/finish_attack_tower");
            if (clientfat.ResponseText != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic rs = serializer.Deserialize<object>(clientfat.ResponseText);

                tower_achievement = rs["tower_data"]["tower_achievement"];
                tower_floor = rs["tower_data"]["tower_floor"];
            }
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
                    if (component.ContainsKey("rank") && component["rank"] <= 3)
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
        public static DataTable GetDataTableFromCSV()
        {
            try
            {
                string strFileName = HttpContext.Current.Server.MapPath("heroes.txt");
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
                /*
                System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + System.IO.Path.GetDirectoryName(strFileName) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\"");
                conn.Open();
                string strQuery = "SELECT * FROM [" + System.IO.Path.GetFileName(strFileName) + "]";
                System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(strQuery, conn);
                System.Data.DataSet ds = new System.Data.DataSet("CSV File");
                adapter.Fill(ds);
                 */
                return tbl;
            }
            catch
            {
                System.Data.DataTable tbl = new System.Data.DataTable();
                return tbl;
            }
        }

        private Dictionary<string, int> layDanhSachManhTuong(int rank)
        {
            DataTable tblHeroes = GetDataTableFromCSV();
            Dictionary<string, int> dicManhTuong = new Dictionary<string, int>();
            if (tblHeroes.Rows.Count > 0)
            {
                readSession();
                foreach (dynamic owned_officer_soul in data["owned_officer_souls"])
                {
                    if (owned_officer_soul["quantity"] > 0 && tblHeroes.Select(string.Format("name = '{0}' and rank = '{1}' and ISNULL(merge,'') <> '1'", owned_officer_soul["officer_id"], rank)).Length > 0)
                    {
                        dicManhTuong.Add(owned_officer_soul["id"], owned_officer_soul["quantity"]);
                    }
                }
            }
            return dicManhTuong;
        }

        private void ghepManhTuong()
        {
            for (int i = 2; i <= 3; i++)
            {
                Dictionary<string, int> dicManhTuong = layDanhSachManhTuong(i);

                int iManhTuong = 5;
                while (iManhTuong == 5)
                {
                    iManhTuong = 0;
                    string strURL = "https://vtt-01.zoygame.com/owned_officer_souls/merge?authentication_token=" + authentication_token;
                    Dictionary<string, int> dicMT = new Dictionary<string, int>();


                    foreach (KeyValuePair<string, int> item in dicManhTuong)
                    {
                        if (item.Value > 0)
                        {
                            int iGetMT = 0;
                            iGetMT += (item.Value < 5 - iManhTuong ? item.Value : 5 - iManhTuong);
                            iManhTuong += iGetMT;
                            dicMT.Add(item.Key, iGetMT);
                            for (int iIndex = 0; iIndex < iGetMT; iIndex++)
                            {
                                strURL += "&owned_officer_soul_ids%5B%5D=" + item.Key;
                            }
                        }
                        if (iManhTuong == 5)
                        {
                            for (int iIndex = 0; iIndex < 7; iIndex++)
                            {
                                new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(merge)).Start(strURL);
                            }
                            System.Threading.Thread.Sleep(2000);
                            foreach (KeyValuePair<string, int> idicMT in dicMT)
                            {
                                dicManhTuong[idicMT.Key] -= idicMT.Value;
                            }
                            break;
                        }
                    }
                }
                login();
            }
        }


        protected void btnGhepManhTuongTuDong_Click(object sender, DirectEventArgs e)
        {
            txtStatus.Text = string.Empty;
            setStatus("START : Ghép mảnh tướng\n");
            ghepManhTuong();
            setStatus("END : Ghép mảnh tướng\n");
        }
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
        protected void txtChonManhTuong_Change(object sender, DirectEventArgs e)
        {
            readSession();
            Dictionary<string, int> dicManhTuong = Session["mt"] as Dictionary<string, int>;
            int iTongSoManhTuong = 0;
            foreach (KeyValuePair<string, int> item in dicManhTuong)
            {
                iTongSoManhTuong += item.Value;
            }
            foreach (dynamic owned_officer_soul in data["owned_officer_souls"])
            {
                if (owned_officer_soul["id"] == txtChonManhTuong.Value.ToString())
                {
                    txtSoLuongManhTuong.Text = ((owned_officer_soul["quantity"] <= 5 - iTongSoManhTuong ? owned_officer_soul["quantity"] : (5 - iTongSoManhTuong))).ToString();
                    break;
                }
            }
        }
        protected void btnChonManhTuong_Click(object sender, DirectEventArgs e)
        {
            readSession();
            txtManhTuongDaChon.Text = string.Empty;
            Dictionary<string, int> dicManhTuong = Session["mt"] as Dictionary<string, int>;
            dicManhTuong.Add(txtChonManhTuong.Value.ToString(), int.Parse(txtSoLuongManhTuong.Text.Trim()));
            int iTotal = 0;
            foreach (KeyValuePair<string, int> item in dicManhTuong)
            {
                foreach (dynamic owned_officer_soul in data["owned_officer_souls"])
                {
                    if (owned_officer_soul["id"] == item.Key)
                    {
                        iTotal += item.Value;
                        txtManhTuongDaChon.Text += owned_officer_soul["officer_id"] + " : " + item.Value + "\r\n";
                        break;
                    }
                }
            }
            if (iTotal == 5)
            {
                btnGhepManhTuongOK_Click(sender, e);
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

        private void banDo()
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
            if (lstItems.Count >= 0)
            {
                string strData = "authentication_token=" + authentication_token;

                foreach (string id in lstItems)
                {
                    strData += "&owned_item_ids%5B%5D=" + id;
                }
                client = new WebClientEx();
                client.DoPost(strData, "https://vtt-01.zoygame.com/owned_items/sell");
            }
        }

        protected void btnGhepDo_Click(object sender, DirectEventArgs e)
        {
            //banDo();
            //return;
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
                for (int i = 0; i <= lstItems.Count - 3; i += 3)
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

                    for (int iIndex = 0; iIndex < 5; iIndex++)
                    {
                        new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(forge)).Start(param);
                    }
                    System.Threading.Thread.Sleep(1000);
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

        #region - Tự động nâng cấp tướng -
        List<string> lstHeroes = new List<string>();
        protected void btnAutoLevelUp_Click(object sender, DirectEventArgs e)
        {
            //lstHeroes.Add("539686ea7376724a6fbb0800");
            //lstHeroes.Add("5396875c7376724a2c720800");
            //lstHeroes.Add("539a7f697376726291355300");
            //lstHeroes.Add("539e9c417376727926c92d01");
            //lstHeroes.Add("53a277fe73767250e7554c00");
            //lstHeroes.Add("53a2781873767250490c4c00");
            //lstHeroes.Add("53ae7cde73767271bd930700");
            //lstHeroes.Add("53b13eea7376727461551a00");
            //lstHeroes.Add("53b1433673767274bf281b00");
            //lstHeroes.Add("53b1434473767274bf301b00");
            //lstHeroes.Add("53b19c1c737672742b014100");
            //lstHeroes.Add("53b1a18d73767274bf344300");
            //lstHeroes.Add("53b2baa97376726ecf642700");
            //lstHeroes.Add("53b28de67376726e37361b00");
            //lstHeroes.Add("53b35eb97376726e37b34c00");
            readSession();
            foreach (dynamic officer in data["owned_officers"])
            {
                bool bAllowUpLevel = false;
                int level = 50;
                foreach (dynamic component in officer["components"])
                {
                    if (component.ContainsKey("level"))
                    {
                        level = component["level"];
                        if (level == 1)
                        {
                            bAllowUpLevel = false;
                            break;
                        }
                    }
                    if (component.ContainsKey("rank"))
                    {
                        if (component["rank"] >= 4 || (component["rank"] < 4 && level > 1))
                            bAllowUpLevel = true;
                    }
                }
                if (bAllowUpLevel)
                {
                    lstHeroes.Add(officer["id"]);
                }
            }
            // Create a timer with a one second interval.
            aTimer = new System.Timers.Timer(120 * 1000);

            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // Set the Interval
            //aTimer.Interval = se * 1000;
            aTimer.Enabled = true;
        }
        // Specify what you want to happen when the Elapsed event is  
        // raised.         
        protected void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            foreach (string id in lstHeroes)
            {
                levelup(id);
            }
        }

        private void setTimeLevelUp()
        {
            readSession();
            //config
            int fromlevel = 2;
            int fromrank = 3;
            //read owned_officers
            Dictionary<object, object> dicOfficers = new Dictionary<object, object>();
            DateTime dtNearLevelUp = new DateTime(1989, 2, 8);
            foreach (dynamic officer in data["owned_officers"])
            {
                bool allowUp = true;
                DateTime dt = new DateTime();
                foreach (dynamic component in officer["components"])
                {
                    if (component.ContainsKey("level") && component.ContainsKey("leveled_up_at"))
                    {
                        if (component["level"] < fromlevel)
                        {
                            allowUp = false;
                            break;
                        }
                        else
                        {
                            if (component["level"] == 1)
                            {
                                dt = DateTime.Now;
                            }
                            else
                            {
                                double leveled_up_at = Convert.ToDouble(component["leveled_up_at"]);
                                if (dtNearLevelUp.Year == 1989)
                                {
                                    dtNearLevelUp = new DateTime(1970, 1, 1).AddSeconds(leveled_up_at).AddHours(7).AddSeconds(Convert.ToDouble(component["cooldown_time_to_next_level"]));
                                }
                                dt = new DateTime(1970, 1, 1).AddSeconds(leveled_up_at).AddHours(7).AddSeconds(Convert.ToDouble(component["cooldown_time_to_next_level"]));
                                if (dt.Subtract(dtNearLevelUp).TotalSeconds <= 0)
                                {
                                    dtNearLevelUp = dt;
                                }
                            }
                        }
                    }
                    if (component.ContainsKey("rank") && component["rank"] < fromrank)
                    {
                        allowUp = false;
                        break;
                    }
                }
                if (allowUp)
                {
                    dicOfficers.Add(officer["id"], dt);
                }
            }
            double se = dtNearLevelUp.Subtract(DateTime.Now).TotalSeconds;
            if (se <= 0)
            {
                //auto level up                
                foreach (KeyValuePair<object, object> item in dicOfficers)
                {
                    if (Convert.ToDateTime(item.Value).Subtract(DateTime.Now).TotalSeconds <= 0)
                    {
                        levelup(item.Key);
                    }
                }
                //re-login
                login();
            }
            else
            {
                // Create a timer with a one second interval.
                aTimer = new System.Timers.Timer(se * 1000);

                // Hook up the Elapsed event for the timer.
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

                // Set the Interval
                //aTimer.Interval = se * 1000;
                aTimer.Enabled = true;
            }
        }

        private void levelup(object id)
        {
            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", txtAuthenticationToken.Text.Trim());
            client.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/owned_officers/" + id + "/level_up");
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
            city_id = data["current_defense_battle"]["city_id"];
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
            data["current_defense_battle"]["city_id"] = city_id;
            data["current_defense_battle"]["total_wave_count"] = total_wave_count;
            data["attack_turn_count"] = attack_turn_count;
            Session["data"] = data;
        }

        private void writeCurrentInfo()
        {
            data = Session["data"];
            txtAuthenticationToken.Text = authentication_token;
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