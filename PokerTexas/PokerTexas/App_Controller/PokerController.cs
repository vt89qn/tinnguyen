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

namespace PokerTexas.App_Controller
{
    public class PokerController : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string status = string.Empty;
        private decimal money = 0;
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
                }
            }
            get { return money; }
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public bool LoginMobile()
        {
            try
            {
                WebClientEx client = new WebClientEx();
                if (string.IsNullOrEmpty(Models.MBAccessToken))
                {
                    Dictionary<string, object> dicLoginFaceBook = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(Models.FaceBook.MBLoginText);
                    Models.MBAccessToken = dicLoginFaceBook["access_token"].ToString();
                }
                client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                client.DoGet("https://graph.facebook.com/me?access_token=" + Models.MBAccessToken + "&format=json");
                Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                if (dicInfo.ContainsKey("name"))
                {
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
                    dic_param.Add("mnick", dicInfo["name"]);
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
                    client.X_TUNNEL_VERIFY = Get_X_TUNNEL_VERIFY(Models.FaceBook.FBID);
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
                        dic = new SortedDictionary<string, object>();
                        dic.Add("api", "62");
                        dic.Add("langtype", "13");
                        dic.Add("method", "System.loadInit");
                        dic.Add("mid", Models.PKID);
                        dic.Add("mtkey", mtkey);
                        dic.Add("protocol", "1");
                        dic.Add("sid", "110");
                        dic.Add("time", ((int)(DateTime.Now.AddHours(-7).Subtract(new DateTime(1970, 1, 1)).TotalSeconds)).ToString());
                        dic.Add("unid", "193");
                        dic.Add("version", "5.3.1");
                        dic.Add("vkey", Utilities.GetMd5Hash(vkey + "M"));
                        dic.Add("vmid", Models.PKID);


                        dic_param = new SortedDictionary<string, object>();

                        dic.Add("param", dic_param);
                        dic.Add("sig", Utilities.GetMd5Hash(Utilities.getSigPoker(dic, mtkey)));

                        param = new NameValueCollection();
                        api = new JavaScriptSerializer().Serialize(dic);
                        param.Add("api", api);
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

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public void NhanThuongHangNgayMobile()
        {
            try
            {
                this.Status = "Bắt đầu nhận thưởng hàng ngày";
                Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(Models.MBLoginText);
                string mtkey = (dicInfo["ret"] as Dictionary<string, object>)["mtkey"].ToString();
                string vkey = (dicInfo["ret"] as Dictionary<string, object>)["vkey"].ToString();
                SortedDictionary<string, object> dic = new SortedDictionary<string, object>();

                #region - Members.phoneContinuous -
                dic = new SortedDictionary<string, object>();
                dic.Add("api", "62");
                dic.Add("langtype", "13");
                dic.Add("method", "Members.phoneContinuous");
                dic.Add("mid", Models.PKID);
                dic.Add("mtkey", mtkey);
                dic.Add("protocol", "1");
                dic.Add("sid", "110");
                dic.Add("time", ((int)(DateTime.Now.AddHours(-7).Subtract(new DateTime(1970, 1, 1)).TotalSeconds)).ToString());
                dic.Add("unid", "193");
                dic.Add("version", "5.3.1");
                dic.Add("vkey", Utilities.GetMd5Hash(vkey + "M"));
                dic.Add("vmid", Models.PKID);


                SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                dic_param.Add("test", "0");

                dic.Add("param", dic_param);
                dic.Add("sig", Utilities.GetMd5Hash(Utilities.getSigPoker(dic, mtkey)));

                NameValueCollection param = new NameValueCollection();
                string api = new JavaScriptSerializer().Serialize(dic);
                param.Add("api", api);
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                if (!string.IsNullOrEmpty(client.ResponseText))
                {

                }
                #endregion

                #region - Members.setMoney -
                dic = new SortedDictionary<string, object>();
                dic.Add("api", "62");
                dic.Add("langtype", "13");
                dic.Add("method", "Members.setMoney");
                dic.Add("mid", Models.PKID);
                dic.Add("mtkey", mtkey);
                dic.Add("protocol", "1");
                dic.Add("sid", "110");
                dic.Add("time", ((int)(DateTime.Now.AddHours(-7).Subtract(new DateTime(1970, 1, 1)).TotalSeconds)).ToString());
                dic.Add("unid", "193");
                dic.Add("version", "5.3.1");
                dic.Add("vkey", Utilities.GetMd5Hash(vkey + "M"));
                dic.Add("vmid", Models.PKID);


                dic_param = new SortedDictionary<string, object>();
                dic_param.Add("sext", "");
                dic_param.Add("sflag", "0");
                dic_param.Add("stype", "0");

                dic.Add("param", dic_param);
                dic.Add("sig", Utilities.GetMd5Hash(Utilities.getSigPoker(dic, mtkey)));

                param = new NameValueCollection();
                api = new JavaScriptSerializer().Serialize(dic);
                param.Add("api", api);
                client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("mmoney"))
                {
                    dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                    string money = (dicInfo["ret"] as Dictionary<string, object>)["mmoney"].ToString();
                    decimal dmoney = 0;
                    if (decimal.TryParse(money, out dmoney))
                    {
                        this.Money = dmoney;
                    }
                }
                #endregion

                this.Status = "Nhận thưởng hàng ngày thành công";
            }
            catch (Exception ex)
            {
                throw ex;
                this.Status = "Có lỗi trong quá trình nhận thưởng hàng ngày";
            }
        }

        public void TangQuaBiMat()
        {
            try
            {
                this.Status = "Bắt đầu tặng quà bí mật";
                Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(Models.MBLoginText);
                string mtkey = (dicInfo["ret"] as Dictionary<string, object>)["mtkey"].ToString();
                string vkey = (dicInfo["ret"] as Dictionary<string, object>)["vkey"].ToString();
                SortedDictionary<string, object> dic = new SortedDictionary<string, object>();

                foreach (Poker to in this.Models.Package.Pokers)
                {
                    if (to == this.Models) continue;
                    #region - Presents.post -
                    dic = new SortedDictionary<string, object>();
                    dic.Add("api", "62");
                    dic.Add("langtype", "13");
                    dic.Add("method", "Presents.post");
                    dic.Add("mid", Models.PKID);
                    dic.Add("mtkey", mtkey);
                    dic.Add("protocol", "1");
                    dic.Add("sid", "110");
                    dic.Add("time", ((int)(DateTime.Now.AddHours(-7).Subtract(new DateTime(1970, 1, 1)).TotalSeconds)).ToString());
                    dic.Add("unid", "193");
                    dic.Add("version", "5.3.1");
                    dic.Add("vkey", Utilities.GetMd5Hash(vkey + "M"));
                    dic.Add("vmid", Models.PKID);


                    SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                    dic_param.Add("to", to.PKID);

                    dic.Add("param", dic_param);
                    dic.Add("sig", Utilities.GetMd5Hash(Utilities.getSigPoker(dic, mtkey)));

                    NameValueCollection param = new NameValueCollection();
                    string api = new JavaScriptSerializer().Serialize(dic);
                    param.Add("api", api);
                    WebClientEx client = new WebClientEx();
                    client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                    client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                    if (!string.IsNullOrEmpty(client.ResponseText))
                    {

                    }
                    #endregion

                    this.Status = "Tặng quà bí mật thành công cho " + to.FaceBook.Login;
                    System.Threading.Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                this.Status = "Có lỗi trong quá trình Tặng quà bí mật";
            }
        }

        public void NhanQuaBiMat()
        {
            try
            {
                this.Status = "Bắt đầu nhận quà bí mật";
                Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(Models.MBLoginText);
                string mtkey = (dicInfo["ret"] as Dictionary<string, object>)["mtkey"].ToString();
                string vkey = (dicInfo["ret"] as Dictionary<string, object>)["vkey"].ToString();
                SortedDictionary<string, object> dic = new SortedDictionary<string, object>();

                foreach (Poker to in this.Models.Package.Pokers)
                {
                    if (to == this.Models) continue;
                    #region - Presents.get -
                    dic = new SortedDictionary<string, object>();
                    dic.Add("api", "62");
                    dic.Add("langtype", "13");
                    dic.Add("method", "Presents.get");
                    dic.Add("mid", Models.PKID);
                    dic.Add("mtkey", mtkey);
                    dic.Add("protocol", "1");
                    dic.Add("sid", "110");
                    dic.Add("time", ((int)(DateTime.Now.AddHours(-7).Subtract(new DateTime(1970, 1, 1)).TotalSeconds)).ToString());
                    dic.Add("unid", "193");
                    dic.Add("version", "5.3.1");
                    dic.Add("vkey", Utilities.GetMd5Hash(vkey + "M"));
                    dic.Add("vmid", Models.PKID);


                    SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                    dic_param.Add("id", DateTime.Today.ToString("yyyyMMdd") + "|" + to.PKID);

                    dic.Add("param", dic_param);
                    dic.Add("sig", Utilities.GetMd5Hash(Utilities.getSigPoker(dic, mtkey)));

                    NameValueCollection param = new NameValueCollection();
                    string api = new JavaScriptSerializer().Serialize(dic);
                    param.Add("api", api);
                    WebClientEx client = new WebClientEx();
                    client.RequestType = WebClientEx.RequestTypeEnum.Poker;
                    client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                    if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("money"))
                    {
                        dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
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
                throw ex;
                this.Status = "Có lỗi trong quá trình nhận quà bí mật";
            }
        }

        public string Get_X_TUNNEL_VERIFY(string sigKey)
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
            TPDataContext TPDB = new TPDataContext();
            TPDB.PK_SignStrings.InsertOnSubmit(new PK_SignString { String = jSon, Seed = iSeed });
            TPDB.SubmitChanges();
            System.Threading.Thread.Sleep(2000);
            while (true)
            {
                TPDB = new TPDataContext();
                PK_SignString signstring = (from _signstring in TPDB.PK_SignStrings where _signstring.String == jSon select _signstring).FirstOrDefault();
                if (signstring != null && !string.IsNullOrEmpty(signstring.SignedString1))
                {
                    strReturn += signstring.SignedString1 + "&" + signstring.SignedString2;
                    TPDB.PK_SignStrings.DeleteOnSubmit(signstring);
                    TPDB.SubmitChanges();
                    break;
                }
                System.Threading.Thread.Sleep(1000);
            }


            return strReturn;
        }
    }
}
