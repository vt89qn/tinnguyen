using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.Timers;
using System.IO;

namespace VuaThuThanh
{
    public partial class LevelUp : System.Web.UI.Page
    {
        string profileJson = @"{'tin':{'access_token':'dm5pX3Rva2VuPTAuODE1NzM1MDAgMTQwMjM3NzkyMSsxNzk2NjIzMzgx','user_name':'vt89qn','user_id':'823493150','user_status':'1','avatar_img_link':'http://avatar.my.soha.vn/80/vt89qn.png'}
                              ,'thi':{'access_token':'dm5pX3Rva2VuPTAuNTMwOTg3MDAgMTQwMjMxMDk5MSs3NDc4MTU2NjA=','user_name':'Pole','user_id':'91031','user_status':'1','avatar_img_link':'http://avatar.my.soha.vn/80/Pole.png'}}";
        WebClientEx client = new WebClientEx();
        NameValueCollection param = new NameValueCollection();
        private const string Run = "Run";
        private const string LoginData = "LoginData";
        private const string data = "data";
        private const string Profile = "Profile";
        //private const string ServerPath = @"C:\Inetpub\vhosts\tinphuong.me\vtt.tinphuong.com\";
        private const string ServerPath = @"D:\Tinnv Data\Project\VuaThuThanh\VuaThuThanh\";
        private string strProfile = "tin";
        private string authentication_token = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnCheckLevelUp_Click(object sender, DirectEventArgs e)
        {
            WindowLogin.Hide();
            WindowWorking.Show();
            btnTrangThai.Text = "Đang dừng, bấm để chạy";

            dynamic data = Utilities.DeSerializeObject(ServerPath + strProfile + ".data");
            if (data is Dictionary<string, object>)
            {
                if (data[Run])
                {
                    btnTrangThai.Text = "Đang chạy, bấm để dừng lại";
                    btnTrangThai.Icon = Icon.Stop;
                }
            }
            else
            {
                if (File.Exists(ServerPath + strProfile + ".data"))
                {
                    try
                    {
                        File.Delete(ServerPath + strProfile + ".data");
                    }
                    catch { }
                }
            }
        }
        protected void btnTrangThai_Click(object sender, DirectEventArgs e)
        {
            dynamic data = Utilities.DeSerializeObject(ServerPath + strProfile + ".data");
            bool bRun = false;
            if (data is Dictionary<string, object>)
            {
                if (data[Run])
                {
                    bRun = true;
                }
            }
            if (bRun)
            {//Stop
                data[Run] = false;
                Utilities.SerializeObject(ServerPath + strProfile + ".data", data);
                btnTrangThai.Text = "Đang dừng, bấm để chạy";
                btnTrangThai.Icon = Icon.ApplicationGo;
            }
            else
            { //Run
                btnTrangThai.Text = "Đang chạy, bấm để dừng lại";
                btnTrangThai.Icon = Icon.Stop;

                System.Timers.Timer timerTask = new System.Timers.Timer(100);
                timerTask.Enabled = true;
                timerTask.Elapsed += new ElapsedEventHandler(timerTask_Elapsed);
            }
        }

        protected void timerTask_Elapsed(object source, ElapsedEventArgs e)
        {
            System.Timers.Timer timerTask = (System.Timers.Timer)source;
            timerTask.Interval = 60000;
            dynamic Data = Utilities.DeSerializeObject(ServerPath + strProfile + ".data");
            if (Data is Dictionary<string, object>)
            {
                if (!Data[Run])
                {//Stop
                    timerTask.Enabled = false;
                    timerTask.Dispose();
                    return;
                }
            }

            if (!(Data is Dictionary<string, object>))
            {
                var t = login();
                Data = new Dictionary<string, object>();
                Data.Add(Run, true);
                Data.Add(LoginData, t);
                Utilities.SerializeObject(ServerPath + strProfile + ".data", Data);
            }
            //Tinh thoi gian up tuong
            List<string> listTuongUp = new List<string>();
            foreach (dynamic officer in Data[LoginData]["owned_officers"])
            {
                foreach (dynamic component in officer["components"])
                {
                    if (component.ContainsKey("level") && component.ContainsKey("leveled_up_at"))
                    {
                        DateTime timeLevelup = new DateTime(1970, 1, 1).AddSeconds(Convert.ToDouble(component["leveled_up_at"])).AddHours(7).AddSeconds(Convert.ToDouble(component["cooldown_time_to_next_level"]));
                        if (DateTime.Now > timeLevelup)
                        {
                            listTuongUp.Add(officer["id"]);
                        }
                    }
                }
            }
            if (listTuongUp.Count > 0)
            {
                dynamic t = login();
                authentication_token = t["authentication_token"];
                listTuongUp.ForEach(x => new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(levelup)).Start(x));
                System.Threading.Thread.Sleep(2000);
                t = login();
                Data[LoginData] = t;
                Utilities.SerializeObject(ServerPath + strProfile + ".data", Data);
            }
        }

        private void levelup(object id)
        {
            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("authentication_token", authentication_token);
            client.DoPost((NameValueCollection)param, "https://vtt-01.zoygame.com/owned_officers/" + id + "/level_up");
        }

        private dynamic login()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic dataProfile = serializer.Deserialize<dynamic>(profileJson);
            if (dataProfile[strProfile] != null)
            {
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
                    serializer = new JavaScriptSerializer();
                    dynamic rs = serializer.Deserialize<dynamic>(client.ResponseText);
                    return rs;
                }
            }
            return false;
        }

    }
}