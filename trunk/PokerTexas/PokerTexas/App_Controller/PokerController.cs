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
        public bool LoginMobile()
        {
            try
            {
                bTryMBLogin = true;
                WebClientEx client = new WebClientEx();
                //if (string.IsNullOrEmpty(Models.MBAccessToken))
                {
                    Dictionary<string, object> dicLoginFaceBook = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(Models.FaceBook.MBLoginText);
                    Models.MBAccessToken = dicLoginFaceBook["access_token"].ToString();
                }
                client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                client.DoGet("https://graph.facebook.com/me?access_token=" + Models.MBAccessToken + "&format=json");
                Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                if (dicInfo.ContainsKey("id"))
                {
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
                    dic.Add("version", "5.3.1");
                    dic.Add("vkey", "");
                    dic.Add("vmid", "");


                    SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                    dic_param.Add("ANPSetting", "4");
                    dic_param.Add("appid", "1");
                    dic_param.Add("appkey", "");

                    dic_param.Add("is_overseas", "1");
                    dic_param.Add("mbig", "");
                    string name = dicInfo.ContainsKey("name") ? dicInfo["name"].ToString() : "";
                    dic_param.Add("mnick", name);
                    dic_param.Add("protocol", "1");
                    dic_param.Add("sitemid", Models.FaceBook.FBID);
                    dic_param.Add("token", Models.MBAccessToken);
                    dic.Add("param", dic_param);

                    dic.Add("sig", Utilities.GetMd5Hash(Utilities.getSigPoker(dic, string.Empty)));


                    NameValueCollection param = new NameValueCollection();
                    string api = new JavaScriptSerializer().Serialize(dic);
                    param.Add("api", api);
                    client = new WebClientEx();
                    client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                    client.X_TUNNEL_VERIFY = string.IsNullOrEmpty(Models.X_TUNNEL_VERIFY) ? Get_X_TUNNEL_VERIFY(Models.FaceBook.FBID) : Models.X_TUNNEL_VERIFY;
                    client.SetAPIV8 = true;
                    client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                    if (!string.IsNullOrEmpty(client.ResponseText)
                        && client.ResponseText.Contains("mtkey")
                        && client.ResponseText.Contains("vkey"))
                    {
                        dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                        Models.MBLoginText = client.ResponseText;
                        Models.PKID = (dicInfo["ret"] as Dictionary<string, object>)["mid"].ToString();
                        Models.X_TUNNEL_VERIFY = client.X_TUNNEL_VERIFY;
                        string mtkey = (dicInfo["ret"] as Dictionary<string, object>)["mtkey"].ToString();
                        string vkey = (dicInfo["ret"] as Dictionary<string, object>)["vkey"].ToString();

                        #region - System.loadInit -
                        dic_param = new SortedDictionary<string, object>();

                        param = new NameValueCollection();
                        param.Add("api", getAPIString("System.loadInit", dic_param));

                        client = new WebClientEx();
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
                        bMBLogedIn = true;
                        return true;
                    }
                    #endregion
                }
                else
                {
                    if (!bTryLoginFB)
                    {
                        bTryLoginFB = true;
                        FaceBookController fbController = new FaceBookController();
                        if (fbController.LoginMobile(Models.FaceBook))
                        {
                            Global.DBContext.SaveChanges();
                            return LoginMobile();
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
        #endregion
        #region - METHOD -
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
                #region - Members.phoneContinuous -
                SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                dic_param.Add("test", "0");

                NameValueCollection param = new NameValueCollection();
                param.Add("api", getAPIString("Members.phoneContinuous", dic_param));

                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                #endregion
                #region - Members.setMoney -
                dic_param = new SortedDictionary<string, object>();
                dic_param.Add("sext", "");
                dic_param.Add("sflag", "0");
                dic_param.Add("stype", "0");

                param = new NameValueCollection();
                param.Add("api", getAPIString("Members.setMoney", dic_param));
                client = new WebClientEx();
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

                this.Status = "Nhận thưởng hàng ngày thành công";
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình nhận thưởng hàng ngày";
                throw ex;
            }
        }

        public void TangQuaBiMat()
        {
            try
            {
                this.Status = "Bắt đầu tặng quà bí mật";
                if (!bMBLogedIn && !bTryMBLogin)
                {
                    LoginMobile();
                }
                if (!bMBLogedIn) return;
                foreach (Poker to in this.Models.Package.Pokers)
                {
                    if (to == this.Models) continue;
                    #region - Presents.post -
                    SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                    dic_param.Add("to", to.PKID);

                    NameValueCollection param = new NameValueCollection();
                    param.Add("api", getAPIString("Presents.post", dic_param));

                    WebClientEx client = new WebClientEx();
                    client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                    client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                    #endregion

                    this.Status = "Tặng quà bí mật thành công cho " + to.FaceBook.Login;
                    System.Threading.Thread.Sleep(4000);
                }
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình Tặng quà bí mật";
                throw ex;
            }
        }

        public void NhanQuaBiMat()
        {
            try
            {
                this.Status = "Bắt đầu nhận quà bí mật";
                foreach (Poker to in this.Models.Package.Pokers)
                {
                    if (to == this.Models) continue;
                    #region - Presents.get -
                    SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                    dic_param.Add("id", DateTime.Today.ToString("yyyyMMdd") + "|" + to.PKID);

                    NameValueCollection param = new NameValueCollection();
                    param.Add("api", getAPIString("Presents.get", dic_param));

                    WebClientEx client = new WebClientEx();
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

                    this.Status = "Nhận quà bí mật thành công từ " + to.FaceBook.Login;
                    System.Threading.Thread.Sleep(4000);
                }
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình nhận quà bí mật";
                throw ex;
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
                #region - Misc.getUserField -
                SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                dic_param.Add("fields", "-2147476476");
                dic_param.Add("uid", Models.PKID);

                NameValueCollection param = new NameValueCollection();
                param.Add("api", getAPIString("Misc.getUserField", dic_param));

                WebClientEx client = new WebClientEx();
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

        private string getAPIString(string strMethod, SortedDictionary<string, object> param)
        {
            Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(Models.MBLoginText);
            string mtkey = (dicInfo["ret"] as Dictionary<string, object>)["mtkey"].ToString();
            string vkey = (dicInfo["ret"] as Dictionary<string, object>)["vkey"].ToString();

            SortedDictionary<string, object> dic = new SortedDictionary<string, object>();
            dic.Add("api", "62");
            dic.Add("langtype", "13");
            dic.Add("method", strMethod);
            dic.Add("mid", Models.PKID);
            dic.Add("mtkey", mtkey);
            dic.Add("protocol", "1");
            dic.Add("sid", "110");
            dic.Add("time", Utilities.GetCurrentSecond());
            dic.Add("unid", "193");
            dic.Add("version", "5.4.2");
            dic.Add("vkey", Utilities.GetMd5Hash(vkey + "M"));
            dic.Add("vmid", Models.PKID);
            dic.Add("param", param);
            dic.Add("sig", Utilities.GetMd5Hash(Utilities.getSigPoker(dic, mtkey)));
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
            treemap.Add("appVer", "5.3.1");
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


            //TPDataContext TPDB = new TPDataContext();
            //TPDB.PK_SignStrings.InsertOnSubmit(new PK_SignString { String = jSon, Seed = iSeed });
            //TPDB.SubmitChanges();
            //System.Threading.Thread.Sleep(2000);

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
                                break;
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }
            return strReturn;
        }

        public void LoginWebAppCache()
        {
            try
            {
                this.Status = "Bắt đầu Authen Facebook";
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                NameValueCollection param = new NameValueCollection();
                if (!string.IsNullOrEmpty(Models.MBLoginText))
                {

                }
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình Authen Facebook";
                throw ex;
            }
        }

        public void LoginWebApp()
        {
            if (DateTime.Today.Date == new DateTime(2014, 12, 01).Date)
            {
                LoginWebApp("https://apps.facebook.com/vntexas/?by_ref=27&by_langtype=13&by_mid=22666800&by_time=1417402559&by_sig=07e2e8ec4c2af82de1895c2139567f9e", true);
            }
            else
            {
                LoginWebApp("https://apps.facebook.com/vntexas/?by_ref=27&by_langtype=13&by_mid=22666800&by_time=1417402559&by_sig=07e2e8ec4c2af82de1895c2139567f9e", false);
            }
        }

        public void LoginWebApp(string strLinkLogin, bool bForceLogin)
        {
            try
            {
                if (!bTryLogin) bTryLogin = true;
                this.Status = "Bắt đầu Authen Facebook";
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                NameValueCollection param = new NameValueCollection();
                if (!bForceLogin)
                {
                    if (!string.IsNullOrEmpty(Models.MBLoginText))
                    { //Check Logged-in
                        object objCookie = Utilities.ConvertBlobToObject(Models.WebCookie);
                        if (objCookie is CookieContainer)
                        {
                            client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;
                            string mid = Regex.Match(Models.WebLoginText, @"mid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                            string sid = Regex.Match(Models.WebLoginText, @"sid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                            string mtkey = Regex.Match(Models.WebLoginText, @"mtkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                            client.DoGet("https://httpvntexas01.boyaagame.com/texas/ajax/message.php?sid=" + sid
                                + "&mid=" + mid + "&mtkey=" + mtkey + "&langtype=13");
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
                client.DoGet(strLinkLogin);
                if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("signed_request"))
                {
                    Models.WebAccessToken = Utilities.GetRegexString(client.ResponseText, "signed_request", 1);
                    param = new NameValueCollection();
                    param.Add("signed_request", Models.WebAccessToken);

                    Dictionary<HttpRequestHeader, string> dicHeader = new Dictionary<HttpRequestHeader, string>();
                    dicHeader.Add(HttpRequestHeader.Referer, strLinkLogin);
                    string strLinkPost = strLinkLogin.Substring(strLinkLogin.IndexOf("?"));
                    client.DoPost(param, "https://httpvntexas01.boyaagame.com/texas/facebookvn/" + strLinkPost, dicHeader);
                    if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("apik"))
                    {
                        Models.WebCookie = Utilities.ConvertObjectToBlob(client.CookieContainer);
                        Models.WebLoginText = client.ResponseText;
                        bWebLogedIn = true;
                        this.Status = "Authen Facebook Thành Công";
                        return;
                    }
                }
                this.Status = "Kiểm tra lại tài khoản facebook này";
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình Authen Facebook";
                throw ex;
            }
        }

        public string ChiaSeChipMayMan()
        {
            string href = string.Empty;
            try
            {
                this.Status = "Bắt đầu chia sẻ chip may mắn";
                if (!bWebLogedIn) return string.Empty;

                string mid = Regex.Match(Models.WebLoginText, @"mid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                string sid = Regex.Match(Models.WebLoginText, @"sid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                string mtkey = Regex.Match(Models.WebLoginText, @"mtkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                string mnick = Regex.Match(Models.WebLoginText, @"mnick:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                string expLevel = Regex.Match(Models.WebLoginText, @"expLevel:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                NameValueCollection param = new NameValueCollection();
                param.Add("ref", "27");
                param.Add("mid", mid);
                param.Add("sid", sid);
                param.Add("mtkey", mtkey);
                param.Add("sitemid", Models.FaceBook.FBID);
                param.Add("langtype", "13");
                param.Add("mnick", mnick);
                param.Add("expLevel", expLevel);
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;
                Dictionary<HttpRequestHeader, string> dicHeader = new Dictionary<HttpRequestHeader, string>();
                dicHeader.Add(HttpRequestHeader.Referer, "https://httpvntexas01.boyaagame.com/texas/facebookvn/");
                client.DoPost(param, "https://httpvntexas01.boyaagame.com/texas/api/facebook/ui.php", dicHeader);
                if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("properties") && client.ResponseText.Contains("href"))
                {
                    Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                    href = ((dicInfo["properties"] as ArrayList)[0] as Dictionary<string, object>)["href"].ToString();
                }
                System.Threading.Thread.Sleep(2000);
                param.Add("flag", "1");
                client.DoPost(param, "https://httpvntexas01.boyaagame.com/texas/api/facebook/uis.php", dicHeader);

                this.Status = "Chia sẻ chip may mắn thành công";
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình Chia sẻ chip may mắn";
                throw ex;
            }
            return href;
        }

        public void RutFanChip(string strLink)
        {
            try
            {
                this.Status = "Bắt đầu nhận fan chip";
                NameValueCollection param = new NameValueCollection();
                WebClientEx client = new WebClientEx();
                Dictionary<HttpRequestHeader, string> dicHeader = new Dictionary<HttpRequestHeader, string>();
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;

                //client.DoGet(strLink);
                //Models.WebAccessToken = Regex.Match(client.ResponseText, "name=\"signed_request\" value=\"(?<val>[^\"]+)").Groups["val"].Value;

                //param = new NameValueCollection();
                //param.Add("signed_request", Models.WebAccessToken);

                //dicHeader = new Dictionary<HttpRequestHeader, string>();
                //dicHeader.Add(HttpRequestHeader.Referer, strLink);
                //string strLinkPost = strLink.Substring(strLink.IndexOf("?"));
                //client.DoPost(param, "https://httpvntexas01.boyaagame.com/texas/facebookvn/" + strLinkPost, dicHeader);

                //Models.WebCookie = Utilities.ConvertObjectToBlob(client.CookieContainer);
                //Models.WebLoginText = client.ResponseText;

                string mid = Regex.Match(Models.WebLoginText, @"mid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                string mtkey = Regex.Match(Models.WebLoginText, @"mtkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                string loginkey = Regex.Match(Models.WebLoginText, @"loginkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();

                param = new NameValueCollection();
                param.Add("by_ref", "77");
                string strRegex = Regex.Match(strLink, "sn=(?<val>[0-9a-zA-Z]+)").Groups["val"].Value.Trim();
                param.Add("sn", strRegex);
                param.Add("loginkey", loginkey);
                param.Add("act", "1003");
                param.Add("mid", mid);
                param.Add("sid", "110");
                param.Add("mtkey", mtkey);
                param.Add("sitemid", Models.FaceBook.FBID);
                param.Add("langtype", "13");

                dicHeader = new Dictionary<HttpRequestHeader, string>();
                dicHeader.Add(HttpRequestHeader.Referer, strLink.Replace("apps.facebook.com/vntexas", "httpvntexas01.boyaagame.com/texas/facebookvn"));
                client.DoPost(param, "https://httpvntexas01.boyaagame.com/texas/api/facebook/rest.php", dicHeader);
                this.Status = "nhận fan chip thành công";
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình rút fan chip";
                throw ex;
            }
        }

        public void NhanChipMayMan(List<string> listLink)
        {
            try
            {
                if (!bWebLogedIn) return;
                this.Status = "Bắt đầu nhận chip may mắn";
                NameValueCollection param = new NameValueCollection();
                WebClientEx client = new WebClientEx();
                Dictionary<HttpRequestHeader, string> dicHeader = new Dictionary<HttpRequestHeader, string>();
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;

                //client.DoGet(listLink[0]);
                //Models.WebAccessToken = Regex.Match(client.ResponseText, "name=\"signed_request\" value=\"(?<val>[^\"]+)").Groups["val"].Value;

                //param = new NameValueCollection();
                //param.Add("signed_request", Models.WebAccessToken);

                //dicHeader = new Dictionary<HttpRequestHeader, string>();
                //dicHeader.Add(HttpRequestHeader.Referer, listLink[0]);
                //string strLinkPost = listLink[0].Substring(listLink[0].IndexOf("?"));
                //client.DoPost(param, "https://httpvntexas01.boyaagame.com/texas/facebookvn/" + strLinkPost, dicHeader);

                //Models.WebCookie = Utilities.ConvertObjectToBlob(client.CookieContainer);
                //Models.WebLoginText = client.ResponseText;

                string mid = Regex.Match(Models.WebLoginText, @"mid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                string mtkey = Regex.Match(Models.WebLoginText, @"mtkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                string loginkey = Regex.Match(Models.WebLoginText, @"loginkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
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
                    param.Add("loginkey", loginkey);
                    param.Add("act", "1003");
                    param.Add("mid", mid);
                    param.Add("sid", "110");
                    param.Add("mtkey", mtkey);
                    param.Add("sitemid", Models.FaceBook.FBID);
                    param.Add("langtype", "13");

                    dicHeader = new Dictionary<HttpRequestHeader, string>();
                    dicHeader.Add(HttpRequestHeader.Referer, strLink.Replace("apps.facebook.com/vntexas", "httpvntexas01.boyaagame.com/texas/facebookvn"));
                    client.DoPost(param, "https://httpvntexas01.boyaagame.com/texas/api/facebook/rest.php", dicHeader);
                    System.Threading.Thread.Sleep(5000);
                }
                this.Status = "Nhận chip may mắn thành công";
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình nhận chip may mắn";
                throw ex;
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
                string mid = Regex.Match(Models.WebLoginText, @"mid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                string sid = Regex.Match(Models.WebLoginText, @"sid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                string mtkey = Regex.Match(Models.WebLoginText, @"mtkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();

                NameValueCollection param = new NameValueCollection();
                param.Add("ref", "30");
                param.Add("act", "1001");
                param.Add("tpl", "contain30");
                param.Add("mid", mid);
                param.Add("sid", sid);
                param.Add("mtkey", mtkey);
                param.Add("sitemid", Models.FaceBook.FBID);
                param.Add("langtype", "13");
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;
                Dictionary<HttpRequestHeader, string> dicHeader = new Dictionary<HttpRequestHeader, string>();
                dicHeader.Add(HttpRequestHeader.Referer, "https://httpvntexas01.boyaagame.com/texas/facebookvn/?_hd=1&token=&sign=");
                client.DoPost(param, "https://httpvntexas01.boyaagame.com/texas/api/facebook/rest.php", dicHeader);
                if (!string.IsNullOrEmpty(client.ResponseText)
                    && client.ResponseText.Contains("by_time")
                    && client.ResponseText.Contains("by_sig"))
                {
                    System.Threading.Thread.Sleep(1000);
                    string strParams = string.Empty;
                    strParams += "by_ref=30&by_mid=" + mid;
                    strParams += "&by_langtype=13";

                    string strRegex = Regex.Match(client.ResponseText, "by_time\":(?<val>[\\s0-9]+)").Groups["val"].Value.Trim();
                    strParams += "&by_time=" + strRegex;

                    strRegex = Regex.Match(client.ResponseText, "by_sig\":(?<val>[\"\\s0-9a-zA-Z]+)").Groups["val"].Value.Replace("\"", "").Trim();
                    strParams += "&by_sig=" + strRegex;

                    strParams += "&act=1002";
                    foreach (Poker pk in Models.Package.Pokers)
                    {
                        if (pk.PKID == Models.PKID) continue;
                        strParams += "&to[]=" + pk.FaceBook.FBID;
                    }
                    strParams += "&mid=" + mid;
                    strParams += "&sid=" + sid;
                    strParams += "&mtkey=" + mtkey;
                    strParams += "&sitemid=" + Models.FaceBook.FBID;
                    strParams += "&langtype=13";
                    client.DoPost(strParams, "https://httpvntexas01.boyaagame.com/texas/api/facebook/rest.php", dicHeader, "POST");
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
                throw ex;
            }
        }

        public void NhanCo4La()
        {
            try
            {
                if (!bWebLogedIn) return;
                this.Status = "Bắt đầu nhận cỏ 4 lá";
                string mid = Regex.Match(Models.WebLoginText, @"mid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                string sid = Regex.Match(Models.WebLoginText, @"sid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                string mtkey = Regex.Match(Models.WebLoginText, @"mtkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;
                CookieCollection ckCollect = client.CookieContainer.GetCookies(new Uri("https://httpvntexas01.boyaagame.com/"));
                foreach (Cookie ck in ckCollect)
                {
                    if (ck.Name.Contains("REQUEST|"))
                    {
                        ck.Expires = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                    }
                }
                client.DoGet("https://httpvntexas01.boyaagame.com/texas/ajax/message.php?sid=" + sid + "&mid=" + mid + "&mtkey=" + mtkey + "&langtype=13");

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
                            int iCountfRequestCo4La = 0;
                        fRequestCo4La: ;
                            client.DoPost(param, "https://httpvntexas01.boyaagame.com/texas/ajax/message.php?sid=" + sid + "&mid=" + mid + "&mtkey=" + mtkey + "&langtype=13");
                            if (string.IsNullOrEmpty(client.ResponseText) || client.ResponseText.Contains("Error request"))
                            {
                                if (iCountfRequestCo4La < 3)
                                {
                                    iCountfRequestCo4La++;
                                    goto fRequestCo4La;
                                }
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
                throw ex;
            }
        }

        public void KyTen()
        {
            try
            {
                if (!bWebLogedIn) return;
                this.Status = "Bắt đầu ký tên";
                string apik = Regex.Match(Models.WebLoginText, @"apik:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                string mid = Regex.Match(Models.WebLoginText, @"mid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                string sid = Regex.Match(Models.WebLoginText, @"sid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                string mtkey = Regex.Match(Models.WebLoginText, @"mtkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                string mnick = Regex.Match(Models.WebLoginText, @"mnick:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;
                NameValueCollection param = new NameValueCollection();
                param.Add("cmd", "init");
                param.Add("apik", apik);
                client.DoPost(param, "https://httpvntexas01.boyaagame.com/texas/act/762/ajax.php");
                if (!string.IsNullOrEmpty(client.ResponseText))
                {
                    if (!client.ResponseText.Contains("isSigned\":1"))
                    {
                        param = new NameValueCollection();
                        param.Add("cmd", "sign");
                        param.Add("apik", apik);
                        client.DoPost(param, "https://httpvntexas01.boyaagame.com/texas/act/762/ajax.php");
                    }
                }

                //Chia se
                param = new NameValueCollection();
                param.Add("ref", "574");
                param.Add("mid", mid);
                param.Add("sid", sid);
                param.Add("mtkey", mtkey);
                param.Add("sitemid", Models.FaceBook.FBID);
                param.Add("langtype", "13");
                param.Add("mnick", mnick);
                param.Add("flag", "1");
                client.DoPost(param, "http://httpvntexas01.boyaagame.com/texas/api/facebook/uis.php");

                //Quay Vong
                client.DoGet("http://httpvntexas01.boyaagame.com/texas/activite/wheel/ajax.php?sid=" + sid + "&mid=" + mid + "&mtkey=" + mtkey + "&langtype=13&cmd=goturn");
                System.Threading.Thread.Sleep(2000);
                client.DoGet("http://httpvntexas01.boyaagame.com/texas/activite/wheel/ajax.php?sid=" + sid + "&mid=" + mid + "&mtkey=" + mtkey + "&langtype=13&cmd=goturn");

                this.Status = "Ký tên thành công";
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình Ký tên";
                throw ex;
            }
        }

        public void NhanThuongHangNgayWeb(string strStep)
        {
            try
            {
                if (strStep == "1")
                {
                    this.Status = "Bắt đầu nhận thưởng hàng ngày";
                    this.ImageCaptcha = null;
                    if (!bWebLogedIn)
                    {
                        if (!bTryLogin) LoginWebApp();
                    }
                    if (!bWebLogedIn) return;
                    //KyTen();

                    string mid = Regex.Match(Models.WebLoginText, @"mid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                    string sid = Regex.Match(Models.WebLoginText, @"sid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                    string mtkey = Regex.Match(Models.WebLoginText, @"mtkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                    this.Status = "Request Captcha ...";
                    //get day bonus
                    WebClientEx client = new WebClientEx();
                    client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                    client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;
                    client.DoGet("https://httpvntexas01.boyaagame.com/texas/ajax/verifycode/verifyCode.php?sid=" + sid + "&mid=" + mid + "&mtkey=" + mtkey + "&langtype=13");

                    if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText == "{\"status\":0}")
                    {
                        client.DoGet("https://httpvntexas01.boyaagame.com/texas/ajax/verifycode/verifycode.html?" + Convert.ToInt32(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 7, 0, 0)).TotalSeconds));
                    }
                    else if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Trim() == "{\"status\":\"vip3\"}")
                    {
                        client.DoGet("https://httpvntexas01.boyaagame.com/texas/ajax/verifycode/getDaymoney.php?sid=" + sid + "&mid=" + mid + "&mtkey=" + mtkey + "&langtype=13");
                        this.Status = "Nhận Thưởng Thành Công";
                        return;
                    }
                    else
                    {
                        this.Status = "Hôm nay đã nhận thưởng rồi !";
                        return;
                    }

                    this.Status = "Request Captcha ...";
                    int iCount = 0;
                    while (this.ImageCaptcha == null && iCount < 3)
                    {
                        client.GetImage("http://httpvntexas01.boyaagame.com/texas/valid2.php?mid=" + mid + "&v=" + new Random().NextDouble());
                        if (client.ResponseImage != null)
                        {
                            this.ImageCaptcha = client.ResponseImage;
                            this.Status = "Request Captcha Thành Công";
                        }
                        iCount++;
                    }
                }
                else
                {
                    if (!bWebLogedIn) return;
                    string mid = Regex.Match(Models.WebLoginText, @"mid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                    string sid = Regex.Match(Models.WebLoginText, @"sid:(?<val>[\s\d]+)").Groups["val"].Value.Trim();
                    string mtkey = Regex.Match(Models.WebLoginText, @"mtkey:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                    string mnick = Regex.Match(Models.WebLoginText, @"mnick:[\s']+(?<val>[^']+)").Groups["val"].Value.Trim();
                    WebClientEx client = new WebClientEx();
                    client.RequestType = WebClientEx.RequestTypeEnum.PokerWeb;
                    client.CookieContainer = Utilities.ConvertBlobToObject(Models.WebCookie) as CookieContainer;
                    client.DoGet("http://httpvntexas01.boyaagame.com/texas/ajax/verifycode/verifyCode.php?sid=" + sid + "&mid=" + mid + "&mtkey=" + mtkey + "&langtype=13&vcc=d341c9&code=" + strStep);
                    if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("suc"))
                    {
                        this.ImageCaptcha = null;
                        client.DoGet("http://httpvntexas01.boyaagame.com/texas/ajax/verifycode/verifyCode.php?sid=" + sid + "&mid=" + mid + "&mtkey=" + mid + "&langtype=13");
                        client.DoGet("http://httpvntexas01.boyaagame.com/texas/ajax/verifycode/getDaymoney.php?sid=" + sid + "&mid=" + mid + "&mtkey=" + mtkey + "&langtype=13");
                        this.Status = "Nhận Thưởng Thành Công";

                        //Chia se
                        NameValueCollection param = new NameValueCollection();
                        param.Add("ref", "574");
                        param.Add("mid", mid);
                        param.Add("sid", sid);
                        param.Add("mtkey", mtkey);
                        param.Add("sitemid", Models.FaceBook.FBID);
                        param.Add("langtype", "13");
                        param.Add("mnick", mnick);
                        param.Add("flag", "1");
                        client.DoPost(param, "http://httpvntexas01.boyaagame.com/texas/api/facebook/uis.php");

                        //Quay Vong
                        client.DoGet("http://httpvntexas01.boyaagame.com/texas/activite/wheel/ajax.php?sid=" + sid + "&mid=" + mid + "&mtkey=" + mtkey + "&langtype=13&cmd=goturn");
                        System.Threading.Thread.Sleep(2000);
                        client.DoGet("http://httpvntexas01.boyaagame.com/texas/activite/wheel/ajax.php?sid=" + sid + "&mid=" + mid + "&mtkey=" + mtkey + "&langtype=13&cmd=goturn");
                    }
                    else
                    {
                        client.GetImage("http://httpvntexas01.boyaagame.com/texas/valid2.php?mid=" + mid + "&v=" + new Random().NextDouble());
                        this.ImageCaptcha = client.ResponseImage;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình nhận thưởng hàng ngày";
                throw ex;
            }
        }

        public void getBirthDayInfo()
        {
            try
            {
                this.Status = "Bắt đầu lấy ngày sinh";
                WebClientEx client = new WebClientEx();
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(Models.FaceBook.MBLoginText);
                if (dicData.ContainsKey("access_token"))
                {
                    client.DoGet("https://graph.facebook.com/v2.1/me?access_token=" + dicData["access_token"] + "&format=json&method=get&pretty=0&suppress_http_code=1");
                    if (client.Error == null && !string.IsNullOrEmpty(client.ResponseText))
                    {
                        dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                        if (dicData.ContainsKey("birthday"))
                        {
                            Models.FaceBook.BirthDay = dicData["birthday"].ToString();
                            this.Status = "lấy ngày sinh thành công";
                        }
                        else if (dicData.ContainsKey("error"))
                        {
                            this.Status = "kiểm tra lại account fb này";
                        }
                    }
                }
                else
                {
                    this.Status = "kiểm tra lại account fb này";
                }

            }
            catch (Exception ex)
            {
                this.Status = "Có lỗi trong quá trình lấy ngày sinh";
                throw ex;
            }
        }
        #endregion
    }
}
