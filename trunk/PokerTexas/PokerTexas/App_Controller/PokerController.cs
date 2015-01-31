using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PokerTexas.App_Model;
using PokerTexas.App_Common;
using PokerTexas.App_UserControl;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using PokerTexas.App_Context;
using TableConstants;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using FluorineFx.Net;
using FluorineFx;
using FluorineFx.Messaging.Api.Service;

namespace PokerTexas.App_Controller
{
    public class PokerController : INotifyPropertyChanged
    {
        #region - INTERFACE METHOD -
        public event PropertyChangedEventHandler PropertyChanged;
        public Control GridContainer;
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                try
                {
                    //If the Proxy object is living in a non-UI thread, use invoke
                    if (GridContainer != null && GridContainer.InvokeRequired)
                    {
                        GridContainer.BeginInvoke(new Action(() => PropertyChanged(this, new PropertyChangedEventArgs(name))));
                    }
                    ////Otherwise update directly
                    else
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs(name));
                    }
                }
                catch { }
            }
        }
        #endregion
        #region - DECLARE -
        private string status = string.Empty;
        private decimal money = 0;
        private decimal earnToday = 0;
        private decimal oldMoney = 0;
        public bool bWebLogedIn = false;
        private bool bTryLogin = false;
        private bool bMBLogedIn = false;
        private bool bTryMBLogin = false;
        private bool bTryLoginFB = false;
        #endregion
        #region - PROPERTY -
        public Poker Models { get; set; }
        public string Status
        {
            set
            {
                if (value != this.status)
                {
                    this.status = value;
                    this.NotifyPropertyChanged(GridMainFormConst.Status);
                }
            }
            get { return status; }
        }

        public decimal Money
        {
            set
            {
                if (value != this.money)
                {
                    this.money = value;
                    this.NotifyPropertyChanged(GridMainFormConst.Money);
                    if (oldMoney == 0) oldMoney = money;
                    this.EarnToday = money - oldMoney;
                }
            }
            get { return money; }
        }

        public decimal EarnToday
        {
            set
            {
                if (value != this.earnToday)
                {
                    this.earnToday = value;
                    this.NotifyPropertyChanged(GridMainFormConst.EarnToday);
                }
            }
            get { return earnToday; }
        }

        public Image ImageCaptcha { get; set; }
        #endregion
        #region - CONTRUCTOR -

        #endregion
        #region - METHOD -
        public bool LoginMobile()
        {
            try
            {
                bTryMBLogin = true;
                var exData = GetExData();

                if (string.IsNullOrEmpty(exData.ip_address)) exData.ip_address = Utilities.GenNewIpAddress();
                NameValueCollection param = new NameValueCollection();
                WebClientEx client = new WebClientEx();
                Dictionary<string, object> dicLoginFaceBook = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(Models.FaceBook.MBLoginText);
                if (!string.IsNullOrEmpty(exData.access_token))
                {
                    //Check valid accessToken
                    client.DoGet("https://graph.facebook.com/app?access_token=" + exData.access_token);
                    if (!client.ResponseText.Contains("179106755472856"))
                    {
                        exData.access_token = string.Empty;
                    }
                }
                if (string.IsNullOrEmpty(exData.access_token))
                {
                    client = new WebClientEx();
                    client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                    client.CookieContainer = new CookieContainer();
                    foreach (Dictionary<string, object> dicCookie in dicLoginFaceBook["session_cookies"] as ArrayList)
                    {
                        client.CookieContainer.Add(new Cookie(dicCookie["name"].ToString(), dicCookie["value"].ToString()
                            , dicCookie["path"].ToString(), dicCookie["domain"].ToString()));
                    }
                    client.DoGet("https://m.facebook.com/dialog/oauth?android_key=OxqiSpUjtzo3w7W4XydskZIzCFU%0A&calling_package_key=com.boyaa.vn&client_id=179106755472856&display=touch&redirect_uri=fbconnect%3A%2F%2Fsuccess&type=user_agent&refsrc=https%3A%2F%2Fm.facebook.com%2Fauth.php&_rdr");
                    exData.access_token = Regex.Match(client.ResponseText, "access_token=(?<val>[a-zA-Z0-9]+)", RegexOptions.IgnoreCase).Groups["val"].Value.Trim();
                    if (string.IsNullOrEmpty(exData.access_token) && client.ResponseText.Contains("dialog/oauth/read"))
                    {
                        param = new NameValueCollection();
                        param.Add("fb_dtsg", Utilities.GetRegexString(client.ResponseText, "fb_dtsg", 1));
                        param.Add("charset_test", "€,´,€,´,水,Д,Є");
                        param.Add("from_post", "1");
                        param.Add("app_id", "179106755472856");
                        param.Add("redirect_uri", "fbconnect://success");
                        param.Add("display", "touch");
                        param.Add("public_info_nux", "1");
                        param.Add("read", "public_profile,user_friends,baseline");
                        param.Add("gdp_version", "3");
                        param.Add("seen_scopes", "public_profile,user_friends,baseline");
                        param.Add("ref", "Default");
                        param.Add("return_format", "access_token");
                        param.Add("sso_device", "android");
                        param.Add("ref", "Default");
                        param.Add("__CONFIRM__", "OK");
                        client.DoPost(param, "https://m.facebook.com/v1.0/dialog/oauth/read");
                        exData.access_token = Regex.Match(client.ResponseText, "access_token=(?<val>[a-zA-Z0-9]+)", RegexOptions.IgnoreCase).Groups["val"].Value.Trim();
                    }
                }
                if (string.IsNullOrEmpty(exData.access_token))
                {
                    this.Status = "Không thể lấy token facebook,thử login lại";
                    bMBLogedIn = false;
                    return false;
                }
                client = new WebClientEx();
                client.IpHeader = exData.ip_address;


                #region - Members.Create -
                SortedDictionary<string, object> dic = new SortedDictionary<string, object>();
                dic.Add("api", "62");
                dic.Add("langtype", "13");
                dic.Add("method", "Members.Create");
                dic.Add("mid", "0");
                dic.Add("mtkey", "");
                dic.Add("protocol", "1");
                dic.Add("sid", "110");
                dic.Add("time", ((int)(DateTime.Now.AddHours(-7).Subtract(new DateTime(1970, 1, 1)).TotalSeconds)).ToString());
                dic.Add("unid", "193");
                dic.Add("version", "5.4.3");
                dic.Add("vkey", "");
                dic.Add("vmid", "");


                SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                dic_param.Add("ANPSetting", "4");
                //dic_param.Add("APNSToken", "APA91bHQCDLRqHBRC3_8L6osr_Qro7kX0HvXdt8h6kfZiqeyTDqZtia45wXGLZ6GjvNldsXhI4oXDxVWfBDm7ZdjjyDmwl2sYa3ObeKcrl1dqqtF6RZcZ4qT3yfVtLvunuChsPMFPBYxOzNHOeNbEvjcB7HFMVjr8g");
                dic_param.Add("appid", "1");
                dic_param.Add("appkey", "");

                dic_param.Add("is_overseas", "1");
                dic_param.Add("mbig", "");
                //string name = dicInfo.ContainsKey("name") ? dicInfo["name"].ToString() : "";
                dic_param.Add("mnick", Models.FaceBook.Login);
                dic_param.Add("protocol", "1");
                dic_param.Add("sitemid", Models.FaceBook.FBID);
                dic_param.Add("token", exData.access_token);
                dic.Add("param", dic_param);

                dic.Add("sig", Utilities.GetMd5Hash(Utilities.getSigPoker(dic, string.Empty, "V")));

                param = new NameValueCollection();
                string api = new JavaScriptSerializer().Serialize(dic);
                param.Add("api", api);
                client = new WebClientEx();
                client.IpHeader = exData.ip_address;
                client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                client.X_TUNNEL_VERIFY = string.IsNullOrEmpty(Models.X_TUNNEL_VERIFY) ? Get_X_TUNNEL_VERIFY(Models.FaceBook.FBID) : Models.X_TUNNEL_VERIFY;
                client.SetAPIV8 = true;
                client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                if (!string.IsNullOrEmpty(client.ResponseText)
                    && client.ResponseText.Contains("mtkey")
                    && client.ResponseText.Contains("vkey"))
                {
                    Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                    Dictionary<string, object> ret = dicInfo["ret"] as Dictionary<string, object>;
                    Models.PKID = ret["mid"].ToString();
                    Models.X_TUNNEL_VERIFY = client.X_TUNNEL_VERIFY;
                    string mtkey = ret["mtkey"].ToString();
                    string vkey = ret["vkey"].ToString();
                    exData.m_mtkey = mtkey;
                    exData.m_vkey = vkey;

                    if (ret.ContainsKey("mmoney"))
                    {
                        string money = ret["mmoney"].ToString();
                        decimal dmoney = 0;
                        if (decimal.TryParse(money, out dmoney))
                        {
                            this.Money = dmoney;
                        }
                    }

                    #region - System.loadInit -
                    dic_param = new SortedDictionary<string, object>();

                    param = new NameValueCollection();
                    param.Add("api", getAPIString("System.loadInit", dic_param));

                    client = new WebClientEx();
                    client.IpHeader = exData.ip_address;
                    client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                    client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                    if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("mmoney"))
                    {
                        dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                        string money = ((dicInfo["ret"] as Dictionary<string, object>)["aUser"] as Dictionary<string, object>)["mmoney"].ToString();
                        decimal dmoney = 0;
                        if (decimal.TryParse(money, out dmoney))
                        {
                            this.Money = dmoney;
                        }
                    }
                    #endregion
                    SetExData(exData);
                    bMBLogedIn = true;
                    return true;
                }
                #endregion
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return false;
        }

        public void NhanThuongHangNgayMobile()
        {
            try
            {
                this.Status = "Bắt đầu nhận thưởng hàng ngày";
                if (!bMBLogedIn && !bTryMBLogin)
                {
                    LoginMobile();
                }
                if (!bMBLogedIn) return;
                var exData = GetExData();
                #region - Members.phoneContinuous -
                SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                dic_param.Add("test", "0");

                NameValueCollection param = new NameValueCollection();
                param.Add("api", getAPIString("Members.phoneContinuous", dic_param));

                WebClientEx client = new WebClientEx();
                client.IpHeader = exData.ip_address;
                client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                #endregion



                this.Status = "Nhận thưởng hàng ngày thành công";
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình nhận thưởng hàng ngày";
                //throw ex;
            }
        }

        public void NhanThuong2MMobile()
        {
            if (!bMBLogedIn && !bTryMBLogin)
            {
                LoginMobile();
            }
            if (!bMBLogedIn) return;
            var exData = GetExData();
            #region - Members.setMoney -
            SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
            dic_param = new SortedDictionary<string, object>();
            dic_param.Add("sext", "");
            dic_param.Add("sflag", "0");
            dic_param.Add("stype", "0");

            NameValueCollection param = new NameValueCollection();
            param.Add("api", getAPIString("Members.setMoney", dic_param));
            WebClientEx client = new WebClientEx();
            client.IpHeader = exData.ip_address;
            client.RequestType = WebClientEx.RequestTypeEnum.Poker;
            bool bTrySetMoney = false;
            int iCount = 0;
        SetMoney: ;
            client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
            if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("mmoney"))
            {
                Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                string money = (dicInfo["ret"] as Dictionary<string, object>)["mmoney"].ToString();
                decimal dmoney = 0;
                if (decimal.TryParse(money, out dmoney))
                {
                    this.Money = dmoney;
                    bTrySetMoney = true;
                }
            }
            if (!bTrySetMoney && iCount <= 4)
            {
                System.Threading.Thread.Sleep(1000);
                iCount++;
                goto SetMoney;
            }
            #endregion
        }

        public void KyTenMobile()
        {
            return;
            this.Status = "Bắt đầu nhận ky ten mobile";
            if (!bMBLogedIn && !bTryMBLogin)
            {
                LoginMobile();
            }
            if (!bMBLogedIn) return;
            var exData = GetExData();
            #region - Act.getAward -
            SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
            dic_param.Add("actVer", "2.1");
            dic_param.Add("allowLottery", "1");
            dic_param.Add("allowType", "0|1|2|3");
            dic_param.Add("isNew", "1");
            NameValueCollection param = new NameValueCollection();
            param.Add("api", getAPIString("Act.getNowAct", dic_param));
            WebClientEx client = new WebClientEx();
            client.IpHeader = exData.ip_address;
            client.RequestType = WebClientEx.RequestTypeEnum.Poker;
            client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
            if (client.Error == null && !string.IsNullOrEmpty(client.ResponseText))
            {
                Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                string actID = string.Empty;
                string aVersion = string.Empty;
                if (dicInfo.ContainsKey("ret") && dicInfo["ret"] is ArrayList)
                {
                    foreach (Dictionary<string, object> dicRet in dicInfo["ret"] as ArrayList)
                    {
                        if (dicRet.ContainsKey("actType") && dicRet.ContainsKey("actID") && dicRet.ContainsKey("version"))
                        {
                            if (dicRet.ContainsKey("actType") && dicRet["actType"] != null && dicRet["actType"].ToString().Trim() == "0")
                            {
                                actID = dicRet["actID"].ToString().Trim();
                            }
                            aVersion += dicRet["version"].ToString().Trim() + "#";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(aVersion) && !string.IsNullOrEmpty(actID))
                {
                    Dictionary<string, object> dic_param2 = new Dictionary<string, object>();
                    dic_param2 = new Dictionary<string, object>();
                    dic_param2.Add("aVersion", aVersion);
                    dic_param2.Add("actID", actID);
                    dic_param2.Add("actVer", "2.1");
                    dic_param2.Add("isGetAward", "1");
                    param = new NameValueCollection();
                    param.Add("api", getAPIString("Act.getAward", dic_param2));

                    client = new WebClientEx();
                    client.IpHeader = exData.ip_address;
                    client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                    client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                }
            }
            #endregion
            this.Status = "ket thuc nhận ky ten mobile";
        }

        public void TangQuaBiMat_OLD()
        {
            try
            {
                this.Status = "Bắt đầu tặng quà bí mật";
                if (!bMBLogedIn && !bTryMBLogin)
                {
                    LoginMobile();
                }
                if (!bMBLogedIn) return;
                var exData = GetExData();
                int iFrom = 0;
                List<Poker> listPokers = Models.Package.Pokers.ToList();
                for (; iFrom < listPokers.Count; iFrom++)
                {
                    if (listPokers[iFrom].PKID == Models.PKID) break;
                }
                for (int iPoker = 1; iPoker <= 6; iPoker++)
                {
                    #region - Presents.post -
                    SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                    Poker p = new Poker();
                    if (iFrom + iPoker < listPokers.Count)
                    {
                        p = listPokers[iFrom + iPoker];
                    }
                    else if (iFrom + iPoker - listPokers.Count < listPokers.Count)
                    {
                        p = listPokers[iFrom + iPoker - listPokers.Count];
                    }
                    dic_param.Add("to", p.PKID);
                    NameValueCollection param = new NameValueCollection();
                    param.Add("api", getAPIString("Presents.post", dic_param));

                    WebClientEx client = new WebClientEx();
                    client.IpHeader = exData.ip_address;
                    client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                    client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                    #endregion

                    this.Status = "Tặng quà bí mật thành công cho " + p.FaceBook.Login;
                    System.Threading.Thread.Sleep(4000);
                }
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình Tặng quà bí mật";
                //throw ex;
            }
        }

        public void NhanQuaBiMat()
        {
            try
            {
                this.Status = "Bắt đầu nhận quà bí mật";
                if (!bMBLogedIn && !bTryMBLogin)
                {
                    LoginMobile();
                }
                if (!bMBLogedIn) return;
                var exData = GetExData();
                SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                NameValueCollection param = new NameValueCollection();
                WebClientEx client = new WebClientEx();
                List<Dictionary<string, object>> listPresents = new List<Dictionary<string, object>>();
                {
                    #region - Presents.lists -
                    dic_param = new SortedDictionary<string, object>();
                    dic_param.Add("sid", "110");
                    param = new NameValueCollection();
                    param.Add("api", getAPIString("Presents.lists", dic_param));

                    client = new WebClientEx();
                    client.IpHeader = exData.ip_address;
                    client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                    client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                    if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("ret"))
                    {
                        Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                        Dictionary<string, object> ret = (dicInfo["ret"] as Dictionary<string, object>);
                        if (ret.ContainsKey("lists") && ret["lists"] is ICollection)
                        {
                            foreach (Dictionary<string, object> diclist in ret["lists"] as ICollection)
                            {
                                listPresents.Add(diclist);
                            }
                        }
                    }
                    #endregion
                }

                int iFrom = 0;
                List<Poker> listPokers = Models.Package.Pokers.ToList();

                for (; iFrom < listPokers.Count; iFrom++)
                {
                    if (listPokers[iFrom].PKID == Models.PKID) break;
                }
                for (int iPoker = 1; iPoker <= 6; iPoker++)
                {
                    Poker p = new Poker();
                    if (iFrom + iPoker < listPokers.Count)
                    {
                        p = listPokers[iFrom + iPoker];
                    }
                    else if (iFrom + iPoker - listPokers.Count < listPokers.Count)
                    {
                        p = listPokers[iFrom + iPoker - listPokers.Count];
                    }
                    string id_presents = string.Empty;
                    foreach (Dictionary<string, object> diclist in listPresents)
                    {
                        if (diclist.ContainsKey("from") && diclist["from"].ToString() == p.PKID)
                        {
                            id_presents = diclist["id"].ToString();
                            listPresents.Remove(diclist);
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(id_presents))
                    {
                        System.Threading.Thread.Sleep(2000);
                        #region - Presents.get -
                        dic_param = new SortedDictionary<string, object>();
                        dic_param.Add("id", id_presents);

                        param = new NameValueCollection();
                        param.Add("api", getAPIString("Presents.get", dic_param));

                        client = new WebClientEx();
                        client.IpHeader = exData.ip_address;
                        client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                        client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                        if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("money"))
                        {
                            Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                            string money = (dicInfo["ret"] as Dictionary<string, object>)["money"].ToString();
                            decimal dmoney = 0;
                            if (decimal.TryParse(money, out dmoney))
                            {
                                this.Money += dmoney;
                            }
                        }
                        #endregion

                        this.Status = "Nhận quà bí mật thành công từ " + p.FaceBook.Login;

                    }

                    System.Threading.Thread.Sleep(2000);

                    #region - Presents.post -
                    dic_param = new SortedDictionary<string, object>();
                    dic_param.Add("to", p.PKID);
                    param = new NameValueCollection();
                    param.Add("api", getAPIString("Presents.post", dic_param));

                    client = new WebClientEx();
                    client.IpHeader = exData.ip_address;
                    client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                    client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                    #endregion

                    this.Status = "Tặng quà bí mật thành công cho " + p.FaceBook.Login;

                }

                foreach (Dictionary<string, object> diclist in listPresents)
                {
                    if (diclist.ContainsKey("from"))
                    {
                        System.Threading.Thread.Sleep(2000);
                        #region - Presents.get -
                        dic_param = new SortedDictionary<string, object>();
                        dic_param.Add("id", diclist["id"].ToString());

                        param = new NameValueCollection();
                        param.Add("api", getAPIString("Presents.get", dic_param));

                        client = new WebClientEx();
                        client.IpHeader = exData.ip_address;
                        client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                        client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                        if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("money"))
                        {
                            Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                            string money = (dicInfo["ret"] as Dictionary<string, object>)["money"].ToString();
                            decimal dmoney = 0;
                            if (decimal.TryParse(money, out dmoney))
                            {
                                this.Money += dmoney;
                            }
                        }
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình nhận quà bí mật";
                //throw ex;
            }
        }

        public void GetInitMoney(bool bShowStatus)
        {
            try
            {
                if (bShowStatus)
                {
                    this.Status = "Bắt đầu kiểm tra tiền";
                }
                var exData = GetExData();
                #region - Misc.getUserField -
                SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                dic_param.Add("fields", "-2147476476");
                dic_param.Add("uid", Models.PKID);

                NameValueCollection param = new NameValueCollection();
                param.Add("api", getAPIString("Misc.getUserField", dic_param));

                WebClientEx client = new WebClientEx();
                client.IpHeader = exData.ip_address;
                client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("mmoney"))
                {
                    Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                    string money = (dicInfo["ret"] as Dictionary<string, object>)["mmoney"].ToString();
                    decimal dmoney = 0;
                    if (decimal.TryParse(money, out dmoney))
                    {
                        this.Money = dmoney;
                    }
                }
                if (bShowStatus)
                {
                    this.Status = "Kiểm tra tiền thành công";
                }
                #endregion
            }
            catch
            {
                if (bShowStatus)
                {
                    this.Status = "Có lỗi xảy ra trong quá trình kiểm tra tiền";
                }
            }
        }

        private string getAPIString(string strMethod, IDictionary<string, object> param)
        {
            var exData = GetExData();

            SortedDictionary<string, object> dic = new SortedDictionary<string, object>();
            dic.Add("api", "62");
            dic.Add("langtype", "13");
            dic.Add("method", strMethod);
            dic.Add("mid", Models.PKID);
            dic.Add("mtkey", exData.m_mtkey);
            dic.Add("protocol", "1");
            dic.Add("sid", "110");
            dic.Add("time", Utilities.GetCurrentSecond());
            dic.Add("unid", "193");
            dic.Add("version", "5.4.3");
            dic.Add("vkey", Utilities.GetMd5Hash(exData.m_vkey + "M"));
            dic.Add("vmid", Models.PKID);
            dic.Add("param", param);
            dic.Add("sig", Utilities.GetMd5Hash(Utilities.getSigPoker(dic, exData.m_mtkey, "V")));
            return new JavaScriptSerializer().Serialize(dic);
        }

        private string Get_X_TUNNEL_VERIFY(string sigKey)
        {
            int iSeed = new Random().Next(10, 1000);

            string strReturn = "2.0.0&";
            strReturn += iSeed.ToString("X") + "&";
            SortedDictionary<string, string> treemap = new SortedDictionary<string, string>();
            treemap.Add("api", "62");
            treemap.Add("appID", "f61DAecVdKkJQ2l4nakA");
            treemap.Add("appVer", "5.4.3");
            treemap.Add("zSeed", iSeed.ToString());
            treemap.Add("zUid", string.Empty);
            treemap.Add("blistHash", string.Empty);
            treemap.Add("binHash", string.Empty);
            treemap.Add("iOSModel", "phone");
            treemap.Add("iOSType", "android");
            treemap.Add("iOSVer", "2.2");
            treemap.Add("macID", Utilities.GetMd5Hash(sigKey));
            treemap.Add("macAddr", string.Empty);
            treemap.Add("udid", "-1");
            treemap.Add("androidId", string.Empty);

            string jSon = new JavaScriptSerializer().Serialize(treemap);

            WebClientEx client = new WebClientEx();
            NameValueCollection param = new NameValueCollection();
            param.Add("stage", "upload");
            param.Add("page", "pk");
            param.Add("unsignedstring", jSon);
            param.Add("seed", iSeed.ToString());
            client.DoPost(param, "http://115.79.60.134:8082/api/Default.aspx");
            while (true)
            {
                param = new NameValueCollection();
                param.Add("stage", "download");
                param.Add("page", "pk");
                param.Add("unsignedstring", jSon);
                param.Add("seed", iSeed.ToString());
                client.DoPost(param, "http://115.79.60.134:8082/api/Default.aspx");
                if (!string.IsNullOrEmpty(client.ResponseText))
                {
                    List<Dictionary<string, object>> listSigned = new JavaScriptSerializer().Deserialize<List<Dictionary<string, object>>>(client.ResponseText);
                    foreach (Dictionary<string, object> dicSigned in listSigned)
                    {
                        if (dicSigned["page"].ToString() == "pk")
                        {
                            if (dicSigned["unsignedstring"].ToString() == jSon)
                            {
                                strReturn += dicSigned["signedstring1"].ToString() + "&" + dicSigned["signedstring2"].ToString();
                                return strReturn;
                                //break;
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
            //return strReturn;
        }

        public void LoginWebApp()
        {
            LoginWebApp("https://apps.facebook.com/vntexas/?by_ref=27&by_langtype=13&by_mid=22666800&by_time=1417402559&by_sig=07e2e8ec4c2af82de1895c2139567f9e", false);
        }

        public void LoginWebApp(string strLinkLogin, bool bForceLogin)
        {
            try
            {
                if (!bTryLogin) bTryLogin = true;
                this.Status = "Bắt đầu Authen Facebook";
                var exData = GetExData();
                if (string.IsNullOrEmpty(exData.ip_address)) exData.ip_address = Utilities.GenNewIpAddress();
                bool bTryStartOver = false;
            StartOver: ;
                WebClientEx client = new WebClientEx();
                client.IpHeader = exData.ip_address;
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                NameValueCollection param = new NameValueCollection();

                if (!bForceLogin)
                {
                    if (!string.IsNullOrEmpty(exData.mtkey))
                    { //Check Logged-in
                        object objCookie = Utilities.ConvertBlobToObject(Models.WebCookie);
                        if (objCookie is CookieContainer)
                        {
                            client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;
                            client.DoGet("https://pclpvdpk01.boyaagame.com/texas/ajax/message.php?sid=" + exData.sid
                                + "&mid=" + Models.PKID + "&mtkey=" + exData.mtkey + "&langtype=13");
                            if (client.ResponseText.Contains("\"ok\":1"))
                            {
                                bWebLogedIn = true;
                                this.Status = "Authen Facebook Thành Công";
                                return;
                            }
                        }
                    }
                }
                client.CookieContainer = new CookieContainer();
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(Models.FaceBook.MBLoginText);
                foreach (Dictionary<string, object> dicCookie in dicData["session_cookies"] as ArrayList)
                {
                    client.CookieContainer.Add(new Cookie(dicCookie["name"].ToString(), dicCookie["value"].ToString()
                        , dicCookie["path"].ToString(), dicCookie["domain"].ToString()));
                }
                bool bTryGo = false;
            FBeginAuthen: ;
                client.AllowAutoRedirect = false;
                client.DoGet("https://www.facebook.com/connect/ping?client_id=179106755472856&domain=pclpvdpk01.boyaagame.com&origin=1&redirect_uri=https%3A%2F%2Fs-static.ak.facebook.com%2Fconnect%2Fxd_arbiter%2F2_ZudbRXWRs.js%3Fversion%3D41%23cb%3Df1fcbf37e4%26domain%3Dpclpvdpk01.boyaagame.com%26origin%3Dhttps%253A%252F%252Fpclpvdpk01.boyaagame.com%252Ff2e72221b%26relation%3Dparent&response_type=token%2Csigned_request%2Ccode&sdk=joey");
                client.AllowAutoRedirect = true;
                if (client.Error == null && client.Response.ResponseUri.AbsoluteUri.Contains("error=not_authorized"))
                {
                    //Not Authen
                    client.DoGet("https://apps.facebook.com/dialog/oauth?display=async&domain=pclpvdpk01.boyaagame.com&scope=email%2Cpublish_stream%2Cpublish_actions&e2e=%7B%7D&app_id=179106755472856&sdk=joey&client_id=179106755472856&origin=5&response_type=token%2Csigned_request&redirect_uri=https%3A%2F%2Fwww.facebook.com%2Fdialog%2Freturn%2Farbiter%23origin%3Dhttps%253A%252F%252Fpclpvdpk01.boyaagame.com%252Ftexas%252Ffacebookvn%252Floadingpage.php&state=f3993495d&__asyncDialog=1&__user=" + Models.FaceBook.FBID + "&__a=1");
                    param = new NameValueCollection();
                    param.Add("fb_dtsg", Utilities.GetRegexString(client.ResponseText, "fb_dtsg", 1));
                    param.Add("app_id", "179106755472856");
                    param.Add("redirect_uri", "https://www.facebook.com/dialog/return/arbiter#origin=https%3A%2F%2Fpclpvdpk01.boyaagame.com%2Ftexas%2Ffacebookvn%2Floadingpage.php");
                    param.Add("display", "async");
                    param.Add("sdk", "joey");
                    param.Add("from_post", "1");
                    param.Add("audience[0][value]", "10");
                    param.Add("GdpEmailBucket_grantEmailType", "contact_email");
                    param.Add("readwrite", "email,public_profile,user_friends,publish_stream,create_note,photo_upload,publish_checkins,share_item,status_update,video_upload,publish_actions,baseline");
                    //param.Add("gdp_version", "2.5");
                    param.Add("seen_scopes", "email,public_profile,user_friends,publish_stream,create_note,photo_upload,publish_checkins,share_item,status_update,video_upload,publish_actions,baseline");
                    param.Add("ref", "Default");
                    param.Add("return_format", "signed_request,access_token,base_domain");
                    param.Add("domain", "pclpvdpk01.boyaagame.com");
                    param.Add("__CONFIRM__", "1");
                    param.Add("__user", Models.FaceBook.FBID.ToString());
                    param.Add("__a", "1");
                    client.DoPost(param, "https://www.facebook.com/dialog/oauth/readwrite");
                    client.AllowAutoRedirect = false;
                    client.DoGet("https://www.facebook.com/connect/ping?client_id=179106755472856&domain=pclpvdpk01.boyaagame.com&origin=1&redirect_uri=https%3A%2F%2Fs-static.ak.facebook.com%2Fconnect%2Fxd_arbiter%2F2_ZudbRXWRs.js%3Fversion%3D41%23cb%3Df1fcbf37e4%26domain%3Dpclpvdpk01.boyaagame.com%26origin%3Dhttps%253A%252F%252Fpclpvdpk01.boyaagame.com%252Ff2e72221b%26relation%3Dparent&response_type=token%2Csigned_request%2Ccode&sdk=joey");
                    client.AllowAutoRedirect = true;
                }
                if (client.Error == null && client.Response.Headers["Location"] != null
                    && client.Response.Headers["Location"].Contains("signed_request"))
                {
                    string signed_request = Regex.Match(client.Response.Headers["Location"], "signed_request=(?<val>[^&]+)").Groups["val"].Value;
                    //Models.WebAccessToken = Utilities.GetRegexString(client.ResponseText, "signed_request", 1);
                    param = new NameValueCollection();
                    param.Add("signed_request", signed_request);

                    Dictionary<HttpRequestHeader, string> dicHeader = new Dictionary<HttpRequestHeader, string>();
                    dicHeader.Add(HttpRequestHeader.Referer, strLinkLogin);
                    string strLinkPost = strLinkLogin.Substring(strLinkLogin.IndexOf("?"));
                    client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/facebookvn/" + strLinkPost, dicHeader);
                    if (client.Error == null && !string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("apik"))
                    {
                        Models.WebCookie = Utilities.ConvertObjectToBlob(client.CookieContainer);
                        string apik = Regex.Match(client.ResponseText, @"apik:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                        string mid = Regex.Match(client.ResponseText, @"mid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                        string sid = Regex.Match(client.ResponseText, @"sid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                        string mtkey = Regex.Match(client.ResponseText, @"mtkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                        string mnick = Regex.Match(client.ResponseText, @"mnick:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                        string expLevel = Regex.Match(client.ResponseText, @"expLevel:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                        string loginkey = Regex.Match(client.ResponseText, @"loginkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();

                        exData.apik = apik;
                        exData.mnick = mnick;
                        exData.mtkey = mtkey;
                        exData.sid = sid;
                        exData.expLevel = expLevel;
                        exData.loginkey = loginkey;
                        exData.count_login_fail = 0;
                        SetExData(exData);
                        bWebLogedIn = true;
                        this.Status = "Authen Facebook Thành Công";
                        return;
                    }
                }
                if (!bTryGo)
                {
                    bTryGo = true;
                    goto FBeginAuthen;
                }
                if (!bTryStartOver)
                {
                    bTryStartOver = true;
                    FaceBookController fbController = new FaceBookController();
                    if (fbController.LoginMobile(Models.FaceBook))
                    {
                        goto StartOver;
                    }
                    else Models.PackageID = 0;
                }
                exData.count_login_fail++;
                SetExData(exData);
                this.Status = "Kiểm tra lại tài khoản facebook này";
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình Authen Facebook";
                //throw ex;
            }
        }

        public string ChiaSeChipMayMan()
        {
            string href = string.Empty;
            try
            {
                this.Status = "Bắt đầu chia sẻ chip may mắn";
                if (!bWebLogedIn) return string.Empty;
                var exData = GetExData();
                NameValueCollection param = new NameValueCollection();
                param.Add("ref", "27");
                param.Add("mid", Models.PKID);
                param.Add("sid", exData.sid);
                param.Add("mtkey", exData.mtkey);
                param.Add("sitemid", Models.FaceBook.FBID);
                param.Add("langtype", "13");
                param.Add("mnick", exData.mnick);
                param.Add("expLevel", exData.expLevel);
                WebClientEx client = new WebClientEx();
                client.IpHeader = exData.ip_address;
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;
                Dictionary<HttpRequestHeader, string> dicHeader = new Dictionary<HttpRequestHeader, string>();
                dicHeader.Add(HttpRequestHeader.Referer, "https://pclpvdpk01.boyaagame.com/texas/facebookvn/");
                client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/api/facebook/ui.php", dicHeader);
                if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("properties") && client.ResponseText.Contains("href"))
                {
                    Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                    href = ((dicInfo["properties"] as ArrayList)[0] as Dictionary<string, object>)["href"].ToString();
                }
                System.Threading.Thread.Sleep(2000);
                param.Add("flag", "1");
                client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/api/facebook/uis.php", dicHeader);

                this.Status = "Chia sẻ chip may mắn thành công";
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình Chia sẻ chip may mắn";
                //throw ex;
            }
            return href;
        }

        public void RutFanChip(string strLink)
        {
            try
            {
                this.Status = "Bắt đầu nhận fan chip";
                var exData = GetExData();
                NameValueCollection param = new NameValueCollection();
                WebClientEx client = new WebClientEx();
                client.IpHeader = exData.ip_address;
                Dictionary<HttpRequestHeader, string> dicHeader = new Dictionary<HttpRequestHeader, string>();
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;

                param = new NameValueCollection();
                param.Add("by_ref", "77");
                string strRegex = Regex.Match(strLink, "sn=(?<val>[0-9a-zA-Z]+)").Groups["val"].Value.Trim();
                param.Add("sn", strRegex);
                param.Add("loginkey", exData.loginkey);
                param.Add("act", "1003");
                param.Add("mid", Models.PKID);
                param.Add("sid", exData.sid);
                param.Add("mtkey", exData.mtkey);
                param.Add("sitemid", Models.FaceBook.FBID);
                param.Add("langtype", "13");

                dicHeader = new Dictionary<HttpRequestHeader, string>();
                dicHeader.Add(HttpRequestHeader.Referer, strLink.Replace("apps.facebook.com/vntexas", "pclpvdpk01.boyaagame.com/texas/facebookvn"));
                client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/api/facebook/rest.php", dicHeader);
                this.Status = "nhận fan chip thành công";
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình rút fan chip";
                //throw ex;
            }
        }

        public void NhanChipMayMan(List<string> listLink)
        {
            try
            {
                if (!bWebLogedIn) return;
                this.Status = "Bắt đầu nhận chip may mắn";
                var exData = GetExData();
                NameValueCollection param = new NameValueCollection();
                WebClientEx client = new WebClientEx();
                client.IpHeader = exData.ip_address;
                Dictionary<HttpRequestHeader, string> dicHeader = new Dictionary<HttpRequestHeader, string>();
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;

                foreach (string strLink in listLink)
                {
                    param = new NameValueCollection();
                    param.Add("by_ref", "27");
                    param.Add("by_langtype", "13");
                    string strRegex = Regex.Match(strLink, "by_mid=(?<val>[0-9]+)").Groups["val"].Value.Trim();
                    param.Add("by_mid", strRegex);
                    strRegex = Regex.Match(strLink, "by_adfeed=(?<val>[0-9]+)").Groups["val"].Value.Trim();
                    if (!string.IsNullOrEmpty(strRegex))
                    {
                        param.Add("by_adfeed", strRegex);
                    }
                    strRegex = Regex.Match(strLink, "by_time=(?<val>[0-9]+)").Groups["val"].Value.Trim();
                    param.Add("by_time", strRegex);
                    strRegex = Regex.Match(strLink, "by_sig=(?<val>[0-9a-zA-Z]+)").Groups["val"].Value.Trim();
                    param.Add("by_sig", strRegex);
                    param.Add("loginkey", exData.loginkey);
                    param.Add("act", "1003");
                    param.Add("mid", Models.PKID);
                    param.Add("sid", exData.sid);
                    param.Add("mtkey", exData.mtkey);
                    param.Add("sitemid", Models.FaceBook.FBID);
                    param.Add("langtype", "13");

                    dicHeader = new Dictionary<HttpRequestHeader, string>();
                    dicHeader.Add(HttpRequestHeader.Referer, strLink.Replace("apps.facebook.com/vntexas", "pclpvdpk01.boyaagame.com/texas/facebookvn"));
                    client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/api/facebook/rest.php", dicHeader);

                    System.Threading.Thread.Sleep(5000);
                }
                this.Status = "Nhận chip may mắn thành công";
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình nhận chip may mắn";
                //throw ex;
            }
        }

        public void TangCo4La()
        {
            try
            {
                //return;
                if (!bWebLogedIn) return;
                //KyTen();
                this.Status = "Bắt đầu tặng cỏ 4 lá";
                var exData = GetExData();
                NameValueCollection param = new NameValueCollection();
                param.Add("ref", "30");
                param.Add("act", "1001");
                param.Add("tpl", "contain30");
                param.Add("mid", Models.PKID);
                param.Add("sid", exData.sid);
                param.Add("mtkey", exData.mtkey);
                param.Add("sitemid", Models.FaceBook.FBID);
                param.Add("langtype", "13");
                WebClientEx client = new WebClientEx();
                client.IpHeader = exData.ip_address;
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;
                Dictionary<HttpRequestHeader, string> dicHeader = new Dictionary<HttpRequestHeader, string>();
                dicHeader.Add(HttpRequestHeader.Referer, "https://pclpvdpk01.boyaagame.com/texas/facebookvn/?_hd=1&token=&sign=");
                client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/api/facebook/rest.php", dicHeader);
                if (!string.IsNullOrEmpty(client.ResponseText)
                    && client.ResponseText.Contains("by_time")
                    && client.ResponseText.Contains("by_sig"))
                {
                    System.Threading.Thread.Sleep(1000);
                    string strParams = string.Empty;
                    strParams += "by_ref=30&by_mid=" + Models.PKID;
                    strParams += "&by_langtype=13";

                    string strRegex = Regex.Match(client.ResponseText, "by_time\":(?<val>[\\s0-9]+)").Groups["val"].Value.Trim();
                    strParams += "&by_time=" + strRegex;

                    strRegex = Regex.Match(client.ResponseText, "by_sig\":(?<val>[\"\\s0-9a-zA-Z]+)").Groups["val"].Value.Replace("\"", "").Trim();
                    strParams += "&by_sig=" + strRegex;

                    strParams += "&act=1002";
                    if (AppSettings.Seft)
                    {
                        int iFrom = 0;
                        List<Poker> listPokers = Models.Package.Pokers.ToList();
                        for (; iFrom < listPokers.Count; iFrom++)
                        {
                            if (listPokers[iFrom].PKID == Models.PKID) break;
                        }
                        for (int iPoker = 1; iPoker <= 5; iPoker++)
                        {
                            if (iFrom + iPoker < listPokers.Count)
                            {
                                strParams += "&to[]=" + listPokers[iFrom + iPoker].FaceBook.FBID;
                            }
                            else if (iFrom + iPoker - listPokers.Count < listPokers.Count)
                            {
                                strParams += "&to[]=" + listPokers[iFrom + iPoker - listPokers.Count].FaceBook.FBID;
                            }
                        }
                    }
                    else
                    {
                        List<Poker> listPokers = Models.Package.Pokers.ToList();
                        for (int iFrom = 0; iFrom < listPokers.Count; iFrom++)
                        {
                            if (listPokers[iFrom].PKID == Models.PKID) continue;
                            strParams += "&to[]=" + listPokers[iFrom].FaceBook.FBID;
                        }
                    }
                    strParams += "&mid=" + Models.PKID;
                    strParams += "&sid=" + exData.sid;
                    strParams += "&mtkey=" + exData.mtkey;
                    strParams += "&sitemid=" + Models.FaceBook.FBID;
                    strParams += "&langtype=13";
                    client.DoPost(strParams, "https://pclpvdpk01.boyaagame.com/texas/api/facebook/rest.php", dicHeader, "POST");
                    this.Status = "Tặng Cỏ 4 Lá Thành Công";
                }
                else
                {
                    this.Status = "Tặng Cỏ 4 Lá Thất Bại";
                }
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình tặng cỏ 4 lá";
                //throw ex;
            }
        }

        public void NhanCo4La()
        {
            try
            {
                if (!bWebLogedIn) return;
                this.Status = "Bắt đầu nhận cỏ 4 lá";
                var exData = GetExData();
                WebClientEx client = new WebClientEx();
                client.IpHeader = exData.ip_address;
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;
                CookieCollection ckCollect = client.CookieContainer.GetCookies(new Uri("https://pclpvdpk01.boyaagame.com/"));
                foreach (Cookie ck in ckCollect)
                {
                    if (ck.Name.Contains("REQUEST|"))
                    {
                        ck.Expires = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                    }
                }
                client.DoGet("https://pclpvdpk01.boyaagame.com/texas/ajax/message.php?sid=" + exData.sid + "&mid=" + Models.PKID + "&mtkey=" + exData.mtkey + "&langtype=13");

                if (!string.IsNullOrEmpty(client.ResponseText))
                {
                    MatchCollection mc = Regex.Matches(client.ResponseText, "id\":[0-9]+,\"cid\":\"1\",\"type\":1");
                    if (mc.Count > 0)
                    {
                        for (int iIndex = 0; iIndex < 12 && iIndex < mc.Count; iIndex++)
                        {
                            string strId = Regex.Match(mc[iIndex].Value, "id\":[0-9]+").Value;
                            strId = strId.Replace("id\":", string.Empty).Trim();
                            NameValueCollection param = new NameValueCollection();
                            param.Add("op", "1");
                            param.Add("msgid[]", strId);
                            client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/ajax/message.php?sid=" + exData.sid + "&mid=" + Models.PKID + "&mtkey=" + exData.mtkey + "&langtype=13");
                            if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("chips\":-1"))
                            {
                                break;
                            }
                            System.Threading.Thread.Sleep(2000);
                        }
                    }
                }
                this.Status = "Nhận Cỏ 4 Lá Thành Công";
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình tặng cỏ 4 lá";
                //throw ex;
            }
        }

        public void KyTenWeb()
        {
            try
            {
                if (!bWebLogedIn) return;
                this.Status = "Bắt đầu ký tên";
                var exData = GetExData();
                NameValueCollection param = new NameValueCollection();
                WebClientEx client = new WebClientEx();
                client.IpHeader = exData.ip_address;
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;
                string textparam = "id=2104&cmd[info][]=A&cmd[info][]=day&apik=" + exData.apik;
                client.DoPost(textparam, "https://pclpvdpk01.boyaagame.com/texas/ac/api.php");
                if (!string.IsNullOrEmpty(client.ResponseText))
                {
                    Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                    if (dicData.ContainsKey("info"))
                    {
                        Dictionary<string, object> info = dicData["info"] as Dictionary<string, object>;

                        int t = 1;
                        if (!info.ContainsKey("A"))
                        {
                            t = 1;
                        }
                        else if (info["A"] is Dictionary<string, object>)
                        {
                            Dictionary<string, object> dicA = info["A"] as Dictionary<string, object>;
                            if (dicA.ContainsKey("tv1"))
                            {
                                int.TryParse(dicA["tv1"].ToString(), out t);
                                if (t >= 1)
                                {
                                    t++;
                                }
                            }
                        }
                        if (AppSettings.Seft)
                        {

                            if (t <= 30)
                            {
                                for (int iIndex = t; iIndex <= 30; iIndex++)
                                {
                                    param = new NameValueCollection();
                                    param.Add("id", "2104");
                                    param.Add("cmd[change][c." + iIndex + "]", "1");
                                    param.Add("apik", exData.apik);
                                    System.Threading.Thread.Sleep(2000);
                                    client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/ac/api.php");
                                }
                            }
                            else
                            {
                                if (Models.PackageID <= 30)
                                {
                                    foreach (int iIndex in new[] { 5, 7, 9, 10, 11, 13, 15, 17, 19, 20, 21, 23, 25, 27, 29 })
                                    {
                                        param = new NameValueCollection();
                                        param.Add("id", "2104");
                                        param.Add("cmd[change][c." + iIndex + "]", "1");
                                        param.Add("apik", exData.apik);
                                        System.Threading.Thread.Sleep(2000);
                                        client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/ac/api.php");
                                    }
                                }
                                else
                                {
                                    foreach (int iIndex in new[] { 4, 5, 7, 9, 10, 11, 13, 15, 17, 19, 20, 21, 23, 25, 27, 29 })
                                    {
                                        param = new NameValueCollection();
                                        param.Add("id", "2104");
                                        param.Add("cmd[change][c." + iIndex + "]", "1");
                                        param.Add("apik", exData.apik);
                                        System.Threading.Thread.Sleep(2000);
                                        client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/ac/api.php");
                                    }
                                }
                                         
                            }
                        }
                        else
                        {
                            if (t <= 30)
                            {
                                for (int iIndex = t; iIndex <= 30; iIndex++)
                                {
                                    param = new NameValueCollection();
                                    param.Add("id", "2104");
                                    param.Add("cmd[change][c." + iIndex + "]", "1");
                                    param.Add("apik", exData.apik);
                                    System.Threading.Thread.Sleep(2000);
                                    client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/ac/api.php");
                                }
                            }
                            else
                            {
                                t = 4;
                                param = new NameValueCollection();
                                param.Add("id", "2104");
                                param.Add("cmd[change][c." + t + "]", "1");
                                param.Add("apik", exData.apik);
                                System.Threading.Thread.Sleep(2000);
                                client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/ac/api.php");
                            }
                        }
                    }
                }
                if (AppSettings.Seft)
                {//Ki ten than bai
                    //param = new NameValueCollection();
                    //param.Add("cmd", "init");
                    //param.Add("apik", apik);
                    //client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/act/471/ajax.php");
                    //if (!string.IsNullOrEmpty(client.ResponseText))
                    //{
                    //    //if (!client.ResponseText.Contains("isSigned\":1"))
                    //    {
                    //        param = new NameValueCollection();
                    //        param.Add("cmd", "sign");
                    //        param.Add("apik", apik);
                    //        client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/act/471/ajax.php");
                    //    }
                    //}
                }
                //Chia se
                param = new NameValueCollection();
                param.Add("ref", "574");
                param.Add("mid", Models.PKID);
                param.Add("sid", exData.sid);
                param.Add("mtkey", exData.mtkey);
                param.Add("sitemid", Models.FaceBook.FBID);
                param.Add("langtype", "13");
                param.Add("mnick", exData.mnick);
                param.Add("flag", "1");
                client.DoPost(param, "http://pclpvdpk01.boyaagame.com/texas/api/facebook/uis.php");

                //Quay Vong
                client.DoGet("http://pclpvdpk01.boyaagame.com/texas/activite/wheel/ajax.php?sid=" + exData.sid + "&mid=" + Models.PKID + "&mtkey=" + exData.mtkey + "&langtype=13&cmd=goturn");
                System.Threading.Thread.Sleep(2000);
                client.DoGet("http://pclpvdpk01.boyaagame.com/texas/activite/wheel/ajax.php?sid=" + exData.sid + "&mid=" + Models.PKID + "&mtkey=" + exData.mtkey + "&langtype=13&cmd=goturn");

                this.Status = "Ký tên thành công";
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình Ký tên";
                //throw ex;
            }
        }

        private int getPoint(string strCards)
        {
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            List<string> listC = new List<string>();
            List<string> listD = new List<string>();
            Dictionary<string, int> countEachCard = new Dictionary<string, int>();
            foreach (string strCard in strCards.Split('-'))
            {
                if (strCard.StartsWith("a"))
                {
                    listA.Add(strCard);
                }
                if (strCard.StartsWith("b"))
                {
                    listB.Add(strCard);
                }
                if (strCard.StartsWith("c"))
                {
                    listC.Add(strCard);
                }
                if (strCard.StartsWith("d"))
                {
                    listD.Add(strCard);
                }
                string strCardNumber = strCard.Substring(1);
                string strCardType = strCard.Substring(0, 1);
                if (!countEachCard.ContainsKey(strCardNumber))
                {
                    countEachCard.Add(strCardNumber, 0);
                }
                countEachCard[strCardNumber]++;
            }

            if (listA.Count == 5)
            {
                foreach (KeyValuePair<string, int> card in countEachCard)
                {
                    bool bFind = true;
                    for (int iINdex = 0; iINdex < 5; iINdex++)
                    {
                        if (!("-" + strCards + "-").Contains("-a" + (int.Parse(card.Key) + iINdex) + "-"))
                        {
                            bFind = false;
                            break;
                        }
                    }
                    if (bFind) return 9;
                }
            }
            if (listB.Count == 5)
            {
                foreach (KeyValuePair<string, int> card in countEachCard)
                {
                    bool bFind = true;
                    for (int iINdex = 0; iINdex < 5; iINdex++)
                    {
                        if (!("-" + strCards + "-").Contains("-b" + (int.Parse(card.Key) + iINdex) + "-"))
                        {
                            bFind = false;
                            break;
                        }
                    }
                    if (bFind) return 9;
                }
            }
            if (listC.Count == 5)
            {
                foreach (KeyValuePair<string, int> card in countEachCard)
                {
                    bool bFind = true;
                    for (int iINdex = 0; iINdex < 5; iINdex++)
                    {
                        if (!("-" + strCards + "-").Contains("-c" + (int.Parse(card.Key) + iINdex) + "-"))
                        {
                            bFind = false;
                            break;
                        }
                    }
                    if (bFind) return 9;
                }
            }
            if (listD.Count == 5)
            {
                foreach (KeyValuePair<string, int> card in countEachCard)
                {
                    bool bFind = true;
                    for (int iINdex = 0; iINdex < 5; iINdex++)
                    {
                        if (!("-" + strCards + "-").Contains("-d" + (int.Parse(card.Key) + iINdex) + "-"))
                        {
                            bFind = false;
                            break;
                        }
                    }
                    if (bFind) return 9;
                }
            }
            bool bFind4 = false;
            bool bFind3 = false;
            bool bFind2 = false;
            bool bFind2_2 = false;
            foreach (KeyValuePair<string, int> card in countEachCard)
            {
                if (card.Value == 4) bFind4 = true;
                if (card.Value == 3) bFind3 = true;
                if (card.Value == 2 && bFind2) bFind2_2 = true;
                if (card.Value == 2) bFind2 = true;
            }
            if (bFind4) return 8;
            if (bFind3 && bFind2) return 7;
            if (listA.Count == 5 || listB.Count == 5 || listC.Count == 5 || listD.Count == 5) return 6;
            foreach (KeyValuePair<string, int> card in countEachCard)
            {
                bool bFind = true;
                for (int iINdex = 0; iINdex < 5; iINdex++)
                {
                    if (!("-" + strCards + "-").Contains("-a" + (int.Parse(card.Key) + iINdex) + "-")
                        && !("-" + strCards + "-").Contains("-b" + (int.Parse(card.Key) + iINdex) + "-")
                        && !("-" + strCards + "-").Contains("-c" + (int.Parse(card.Key) + iINdex) + "-")
                        && !("-" + strCards + "-").Contains("-d" + (int.Parse(card.Key) + iINdex) + "-")
                        )
                    {
                        bFind = false;
                        break;
                    }
                }
                if (bFind) return 5;
            }
            if (bFind3) return 4;
            if (bFind2 && bFind2_2) return 3;
            if (bFind2) return 2;
            return 1;
        }

        public void PlayMiniGame()
        {
            try
            {
                if (!bWebLogedIn) return;
                this.Status = "Bắt đầu chơi mini game";
                var exData = GetExData();
                //string apik = data.apik;
                //string mid = Regex.Match(Models.WebLoginText, @"mid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                //string sid = Regex.Match(Models.WebLoginText, @"sid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                //string mtkey = Regex.Match(Models.WebLoginText, @"mtkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                //string mnick = Regex.Match(Models.WebLoginText, @"mnick:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                WebClientEx client = new WebClientEx();
                client.IpHeader = exData.ip_address;
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;
                string strChoose = "1";
                string strserialCard = string.Empty;
                Dictionary<string, string> dicDB = Utilities.DeSerializeObject("noviceGame.obj") as Dictionary<string, string>;
                if (dicDB == null) dicDB = new Dictionary<string, string>();
                int iIndex = 1;
                string isShowMoney = "";
                for (; iIndex <= 10; iIndex++)
                {
                    NameValueCollection param = new NameValueCollection();
                    if (iIndex == 1)
                    {
                        param.Add("cmd", "info");
                        param.Add("apik", exData.apik);
                    }
                    else if (iIndex > 1)
                    {
                        param.Add("cmd", "choose");
                        param.Add("choose", strChoose);
                        param.Add("apik", exData.apik);

                    }
                    client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/ajax/noviceGame.php");
                    System.Threading.Thread.Sleep(2000);
                    if (!string.IsNullOrEmpty(client.ResponseText))
                    {
                        Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                        if (dicData.ContainsKey("ret"))
                        {
                            Dictionary<string, object> ret = dicData["ret"] as Dictionary<string, object>;
                            string strCard1 = string.Empty;
                            string strCard2 = string.Empty;
                            ArrayList cardType = null;
                            if (ret.ContainsKey("isShowMoney") && ret["isShowMoney"] != null)
                            {
                                isShowMoney = ret["isShowMoney"].ToString();
                            }
                            if (ret.ContainsKey("cardType"))
                            {
                                cardType = ret["cardType"] as ArrayList;
                            }
                            else if (ret.ContainsKey("nextArr"))
                            {
                                Dictionary<string, object> nextArr = ret["nextArr"] as Dictionary<string, object>;
                                if (nextArr != null && nextArr.ContainsKey("cardType"))
                                {
                                    cardType = nextArr["cardType"] as ArrayList;
                                }
                            }
                            if (cardType != null)
                            {
                                if (iIndex > 1 && !string.IsNullOrEmpty(strserialCard) && !dicDB.ContainsKey(strserialCard))
                                {
                                    dicDB.Add(strserialCard, strChoose);
                                }
                                strserialCard = new JavaScriptSerializer().Serialize(cardType);
                                if (dicDB != null && dicDB.ContainsKey(strserialCard))
                                {
                                    strChoose = dicDB[strserialCard];
                                }
                                else
                                {
                                    if (cardType.Count == 3)
                                    {
                                        strCard1 = cardType[0].ToString() + "-" + cardType[1];
                                        strCard2 = cardType[0].ToString() + "-" + cardType[2];
                                        int iPoint1 = getPoint(strCard1);
                                        int iPoint2 = getPoint(strCard2);
                                        if (iPoint1 < iPoint2) strChoose = "2";
                                        else if (iPoint1 > iPoint2) strChoose = "1";
                                        else if (iPoint1 == 8) strChoose = "2";

                                    }
                                    else if (cardType.Count == 4)
                                    {
                                        strCard1 = cardType[0].ToString() + "-" + cardType[2];
                                        strCard2 = cardType[0].ToString() + "-" + cardType[3];
                                        int iPoint1 = getPoint(strCard1);
                                        int iPoint2 = getPoint(strCard2);
                                        if (iPoint1 < iPoint2) strChoose = "1";
                                        else strChoose = "2";
                                    }
                                    else if (cardType.Count == 5)
                                    {
                                        strCard1 = cardType[0].ToString() + "-" + cardType[1] + "-" + cardType[3];
                                        strCard2 = cardType[0].ToString() + "-" + cardType[2];
                                        int iPoint1 = getPoint(strCard1);
                                        int iPoint2 = getPoint(strCard2);
                                        if (iPoint1 < iPoint2) strChoose = "2";
                                        else strChoose = "1";
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(strserialCard))
                                {
                                    if (!string.IsNullOrEmpty(ret["err"] as string))
                                    {
                                        if (!dicDB.ContainsKey(strserialCard))
                                        {
                                            dicDB.Add(strserialCard, strChoose == "1" ? "2" : "1");
                                        }
                                        else
                                        {
                                            strChoose = strChoose == "1" ? "2" : "1";
                                            if (dicDB[strserialCard] != strChoose)
                                            {
                                                dicDB[strserialCard] = strChoose;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!dicDB.ContainsKey(strserialCard))
                                        {
                                            dicDB.Add(strserialCard, strChoose);
                                        }
                                        else
                                        {
                                            if (dicDB[strserialCard] != strChoose)
                                            {
                                                dicDB[strserialCard] = strChoose;
                                            }
                                        }
                                    }
                                }
                                break;
                            };
                        }
                    }
                }
                Dictionary<string, string> dicDB2 = Utilities.DeSerializeObject("noviceGame.obj") as Dictionary<string, string>;
                if (dicDB2 == null) dicDB2 = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> item in dicDB)
                {
                    if (!dicDB2.ContainsKey(item.Key))
                    {
                        dicDB2.Add(item.Key, item.Value);
                    }
                    else if (dicDB2[item.Key] != item.Value)
                    {
                        string t = dicDB2[item.Key];
                        dicDB2[item.Key] = item.Value;
                    }
                }
                Utilities.SerializeObject("noviceGame.obj", dicDB2);
                this.Status = "mini game xong (round : " + iIndex + ", money : " + isShowMoney + " )";
                System.Threading.Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình chơi mini game";
                //throw ex;
            }
        }

        public void getBirthDayInfo()
        {
            //try
            //{
            //    this.Status = "Bắt đầu lấy ngày sinh";
            //    WebClientEx client = new WebClientEx();
            //    client.IpHeader = exData.ip_address;
            //    Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(Models.FaceBook.MBLoginText);
            //    if (dicData.ContainsKey("access_token"))
            //    {
            //        client.DoGet("https://graph.facebook.com/v2.1/me?access_token=" + dicData["access_token"] + "&format=json&method=get&pretty=0&suppress_http_code=1");
            //        if (client.Error == null && !string.IsNullOrEmpty(client.ResponseText))
            //        {
            //            dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
            //            if (dicData.ContainsKey("birthday"))
            //            {
            //                Models.FaceBook.BirthDay = dicData["birthday"].ToString();
            //                this.Status = "lấy ngày sinh thành công";
            //            }
            //            else if (dicData.ContainsKey("error"))
            //            {
            //                this.Status = "kiểm tra lại account fb này";
            //            }
            //        }
            //    }
            //    else
            //    {
            //        this.Status = "kiểm tra lại account fb này";
            //    }

            //}
            //catch (Exception ex)
            //{
            //    this.Status = "Có lỗi trong quá trình lấy ngày sinh";
            //    //throw ex;
            //}
        }

        public PokerExData GetExData()
        {
            var exData = new PokerExData();
            if (!string.IsNullOrEmpty(Models.MBLoginText))
            {
                exData = new JavaScriptSerializer().Deserialize<PokerExData>(Models.MBLoginText);
                if (exData == null) exData = new PokerExData();
            }
            return exData;
        }

        public void SetExData(PokerExData data)
        {
            Models.MBLoginText = new JavaScriptSerializer().Serialize(data);
        }

        public List<string> ListFriend = new List<string>();
        public void GetAllFriend()
        {
            try
            {
                NetConnection _connection = new NetConnection();
                _connection.ObjectEncoding = ObjectEncoding.AMF3;
                _connection.Connect("http://pclpvdpk01.boyaagame.com/texas/api/gateway.php");

                var exData = GetExData();


                SortedDictionary<string, object> dicA = new SortedDictionary<string, object>();
                dicA.Add("unid", 110);
                dicA.Add("mid", Models.PKID);
                dicA.Add("mtkey", exData.mtkey);
                dicA.Add("param", null);
                dicA.Add("time", Utilities.GetCurrentSecond());
                dicA.Add("langtype", 13);
                dicA.Add("count", 5);
                dicA.Add("sid", 110);
                string sig = Utilities.GetMd5Hash(Utilities.getSigPoker(dicA, exData.mtkey, "M"));
                dicA.Add("sig", sig);
                ServerHelloMsgHandler rp = new ServerHelloMsgHandler();
                _connection.Call("Members.getFriendAllIds", rp, dicA);
                int iCount = 0;
                while (iCount < 10)
                {
                    iCount++;
                    System.Threading.Thread.Sleep(1000);
                    if (rp.Result != null && rp.Result is Dictionary<string, object>)
                    {
                        Dictionary<string, object> dicRS = rp.Result as Dictionary<string, object>;
                        ListFriend = new List<string>();
                        if (dicRS.ContainsKey("ret") && dicRS["ret"] is object[])
                        {
                            foreach (object id in dicRS["ret"] as object[])
                            {
                                ListFriend.Add(id.ToString());
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void NhanTraiTim()
        {
            try
            {
                var exData = GetExData();

                WebClientEx client = new WebClientEx();
                client.IpHeader = exData.ip_address;
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.DoGet("https://pclpvdpk01.boyaagame.com/texas/includes/isfans.php?sid=110&mid=" + Models.PKID + "&mtkey=" + exData.mtkey + "&langtype=13&isfans=1&t=" + Utilities.GetCurrentSecond());


                NetConnection _connection = new NetConnection();
                _connection.ObjectEncoding = ObjectEncoding.AMF3;
                _connection.Connect("http://pclpvdpk01.boyaagame.com/texas/api/gateway.php");



                SortedDictionary<string, object> dicA = new SortedDictionary<string, object>();
                dicA.Add("unid", 110);
                dicA.Add("mid", Models.PKID);
                dicA.Add("mtkey", exData.mtkey);
                dicA.Add("langtype", 13);
                dicA.Add("count", 16);
                dicA.Add("time", Utilities.GetCurrentSecond());
                dicA.Add("param", null);
                dicA.Add("sid", 110);
                string sig = Utilities.GetMd5Hash(Utilities.getSigPoker(dicA, exData.mtkey, "M"));
                dicA.Add("sig", sig);
                _connection.Call("Gifts.getNewFansChips", new ServerHelloMsgHandler(), dicA);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Send request from Model 1 to Model 2
        /// </summary>
        /// <param name="model1"></param>
        /// <param name="model2"></param>
        /// <returns></returns>
        public bool SendFriendRequest(Poker to)
        {
            try
            {
                NetConnection _connection = new NetConnection();
                _connection.ObjectEncoding = ObjectEncoding.AMF3;
                _connection.Connect("http://pclpvdpk01.boyaagame.com/texas/api/gateway.php");

                var exData = GetExData();


                SortedDictionary<string, object> dicA = new SortedDictionary<string, object>();
                dicA.Add("mid", Models.PKID);
                dicA.Add("mtkey", exData.mtkey);
                dicA.Add("time", Utilities.GetCurrentSecond());
                SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                dic_param.Add("tmid", to.PKID);
                dic_param.Add("words", "");
                dic_param.Add("ntid", 3);
                dic_param.Add("fmid", Models.PKID);
                dic_param.Add("fsid", 3);
                dicA.Add("param", dic_param);
                dicA.Add("count", 69);

                dicA.Add("sid", 110);
                dicA.Add("unid", 110);
                dicA.Add("langtype", 13);
                string sig = Utilities.GetMd5Hash(Utilities.getSigPoker(dicA, exData.mtkey, "M"));
                dicA.Add("sig", sig);
                _connection.Call("Members.friendRequest", new ServerHelloMsgHandler(), dicA);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        /// <summary>
        /// Model 1 accept friend Request from Model2
        /// </summary>
        /// <param name="model1"></param>
        /// <param name="model2"></param>
        /// <returns></returns>
        public bool AcceptFriendRequest()
        {
            try
            {
                if (!bWebLogedIn)
                {
                    LoginWebApp();
                }
                if (!bWebLogedIn) return false;
                var exData = GetExData();
                WebClientEx client = new WebClientEx();
                client.IpHeader = exData.ip_address;
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;

                NameValueCollection param = new NameValueCollection();
                param.Add("cmd", "info");
                param.Add("p", "1");
                param.Add("size", "10");
                param.Add("apik", exData.apik);
                client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/ajax/addFriends.php");
                if (client.Error == null && !string.IsNullOrEmpty(client.ResponseText))
                {
                    Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                    if (dicData.ContainsKey("ret") && dicData["ret"] is ArrayList)
                    {
                        foreach (object item in dicData["ret"] as ArrayList)
                        {
                            if (item is ArrayList)
                            {
                                string id = (item as ArrayList)[1].ToString();
                                if ((item as ArrayList)[2].ToString() == "4")
                                {
                                    bool bTRy = false;
                                TryGo: ;
                                    param = new NameValueCollection();
                                    param.Add("cmd", "deal");
                                    param.Add("op", "1");
                                    param.Add("msgid", id);
                                    param.Add("apik", exData.apik);
                                    client.DoPost(param, "https://pclpvdpk01.boyaagame.com/texas/ajax/addFriends.php");
                                    if (string.IsNullOrEmpty(client.ResponseText) || client.ResponseText.Contains("ok\":0"))
                                    {
                                        if (!bTRy)
                                        {
                                            bTRy = true;
                                            goto TryGo;
                                        }
                                    }
                                    System.Threading.Thread.Sleep(2000);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public void Bet()
        {
            bool bWin = false;
            bool bRed = false;
            int iCountFail = 0;
            while (true)
            {
                if (bWin)
                {
                    bWin = Bet(bRed);
                }
                else
                {
                    bRed = !bRed;
                    bWin = Bet(bRed);
                }
                if (!bWin)
                {
                    iCountFail++;
                    if (iCountFail >= 2)
                    {
                        bWin = true;
                        iCountFail = 0;
                    }
                }
                else iCountFail = 0;
                System.Threading.Thread.Sleep(3000);
            }
        }

        private bool Bet(bool bRed)
        {
            try
            {
                NetConnection _connection = new NetConnection();
                _connection.ObjectEncoding = ObjectEncoding.AMF3;
                _connection.Connect("http://pclpvdpk01.boyaagame.com/texas/api/gateway.php");

                Dictionary<string, object> dicA = new Dictionary<string, object>();
                Dictionary<string, object> param = new Dictionary<string, object>();
                int betmoney = -100000000;
                param.Add("money",betmoney);
                param.Add("bet", new int[] { bRed ? betmoney : 0, bRed ? 0 : betmoney, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                dicA.Add("param", param);
                dicA.Add("sid", 110);
                dicA.Add("langtype", 13);
                dicA.Add("count", 110);
                dicA.Add("unid", 110);
                dicA.Add("mid", 19350592);
                dicA.Add("time", Utilities.GetCurrentSecond());
                dicA.Add("mtkey", "S9jFoes91bQiCc3WW9ZOG2qPlfnOrP");

                string sig = Utilities.GetMd5Hash(Utilities.getSigPoker(dicA, "S9jFoes91bQiCc3WW9ZOG2qPlfnOrP", "M"));
                dicA.Add("sig", sig);
                ServerHelloMsgHandler rp = new ServerHelloMsgHandler();
                _connection.Call("Funcs.playThreeCards", rp, dicA);
                int iCount = 0;
                while (iCount < 10)
                {
                    iCount++;
                    System.Threading.Thread.Sleep(1000);
                    if (rp.Result != null && rp.Result is Dictionary<string, object>)
                    {
                        Dictionary<string, object> dicRS = rp.Result as Dictionary<string, object>;
                        ListFriend = new List<string>();
                        if (dicRS.ContainsKey("ret") && dicRS["ret"] is Dictionary<string, object>)
                        {
                            var reward = (dicRS["ret"] as Dictionary<string, object>)["reward"];
                            if (reward != null && reward.ToString() != "0") return true;
                        }
                        break;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class ServerHelloMsgHandler : IPendingServiceCallback
        {
            public object Result { get; set; }
            public void ResultReceived(IPendingServiceCall call)
            {
                Result = call.Result;
            }
        }
        #endregion
    }
}
