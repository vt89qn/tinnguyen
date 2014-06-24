using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Data;
using TableConstants;
using System.Linq;
using System.ComponentModel;

public class Poker : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    WebClientEx client = new WebClientEx();
    Serializer serial = new Serializer();
    string strResponse = string.Empty;
    string status = string.Empty;

    bool bHadAuthen = false;

    Image imgCaptcha = null;
    bool bGotCaptchaDayMoney = false;
    bool bWrongCaptcha = false;
    bool bEnterCaptcha = false;

    List<string> listLinkLuckyChip = new List<string>();
    bool bGotLinkChip = false;

    bool bSendChip = false;

    Dictionary<HttpRequestHeader, string> dicHeader = new Dictionary<HttpRequestHeader, string>();
    NameValueCollection param = new NameValueCollection();

    public delegate void UpdateInfoEvent(Poker sender, object objValue, string AccountXid, string strColName);
    public event UpdateInfoEvent UpdateInfo;

    public Image ImageCaptcha
    {
        get { return this.imgCaptcha; }
        set { this.imgCaptcha = value; }
    }
    public bool GotCaptchaDayMoney
    {
        get { return this.bGotCaptchaDayMoney; }
        set { this.bGotCaptchaDayMoney = value; }
    }
    public bool WrongCaptcha
    {
        get { return this.bWrongCaptcha; }
        set { this.bWrongCaptcha = value; }
    }
    public bool EnterCaptcha
    {
        set { this.bEnterCaptcha = value; }
        get { return this.bEnterCaptcha; }
    }

    public List<string> ListLinkLuckyChip
    {
        set { this.listLinkLuckyChip = value; }
        get { return this.listLinkLuckyChip; }
    }
    public bool GotLinkChip
    {
        set { this.bGotLinkChip = value; }
        get { return this.bGotLinkChip; }
    }

    public bool SendChip
    {
        set { this.bSendChip = value; }
        get { return this.bSendChip; }
    }
    public string Mid { set; get; }
    public string Sid = "110";
    public string Mtkey { set; get; }
    public string Sitemid { set; get; }
    public string Loginkey { set; get; }
    public string Apik { set; get; }
    public string Vkey { set; get; }
    public string Sig { set; get; }
    public string Ldsig { set; get; }
    public string Mnick { set; get; }
    public string Ldtime { set; get; }
    public string Ldmactivetime { set; get; }
    public bool GotdayMoney { set; get; }
    public DataRow RowData { set; get; }



    public int STT { set; get; }
    public string Status
    {
        set
        {
            this.status = value;
            this.NotifyPropertyChanged("Status");
        }
        get { return status; }
    }
    public string PackageXid { set; get; }
    public string Account { set; get; }

    public bool HadAuthen
    {
        get { return this.bHadAuthen; }
        set { this.bHadAuthen = value; }
    }
    public bool AccountDie { set; get; }

    private void NotifyPropertyChanged(string name)
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

    public bool AuthenPoker()
    {
        try
        {
            string strParam = string.Empty;
            this.Status = "Bắt đầu Authen.";
            string strSignedRequest = string.Empty;
            #region - Check Poker Cookie -
            if (RowData[M_AccountConst.CookiePoker] is byte[])
            { //
                object objCookie = serial.ConvertBlobToObject(RowData[M_AccountConst.CookiePoker] as byte[]);
                if (objCookie is CookieContainer)
                {
                    this.client.CookieContainer = objCookie as CookieContainer;
                    goto FBeginAuthen;
                }
            }
            #endregion
            #region - Check Facebook Cookie -
        FCheckCookieFacebook: ;
            if (RowData[M_AccountConst.CookieFB] is byte[])
            { //
                object objCookie = serial.ConvertBlobToObject(RowData[M_AccountConst.CookieFB] as byte[]);
                if (objCookie is CookieContainer)
                {
                    this.client.CookieContainer = objCookie as CookieContainer;
                    client.AllowAutoRedirect = false;
                    strResponse = client.DoGet("https://www.facebook.com/connect/ping?client_id=373853122704634&domain=vntexas0.boyaagame.com&origin=1&redirect_uri=http%3A%2F%2Fstatic.ak.facebook.com%2Fconnect%2Fxd_arbiter.php%3Fversion%3D40%23cb%3Df3ccdac778%26domain%3Dvntexas0.boyaagame.com%26origin%3Dhttp%253A%252F%252Fvntexas0.boyaagame.com%252Ff292069cbc%26relation%3Dparent&response_type=token%2Csigned_request%2Ccode&sdk=joey");
                    client.AllowAutoRedirect = true;
                    string strLocation = client.ResponseHeaders[HttpResponseHeader.Location];
                    if (strLocation.Contains("error=not_authorized"))
                    {
                        this.Status = "Yêu cầu authen ở website.";
                        this.HadAuthen = false;
                        return false;
                    }
                    strSignedRequest = Regex.Match(strLocation, "signed_request=[^&]+").Value.Replace("signed_request=", string.Empty).Trim();
                    if (string.IsNullOrEmpty(strSignedRequest))
                    {
                        this.Status = "Authen Facebook Thất bại";
                        FaceBook fb = new FaceBook();
                        fb.LoginInfo = new LoginContain(RowData[M_AccountConst.Account].ToString(), RowData[M_AccountConst.Password].ToString());
                        if (fb.Login())
                        {
                            byte[] bytesCookie = serial.ConvertObjectToBlob(fb.StoreCookie);
                            RowData[M_AccountConst.CookieFB] = bytesCookie;
                            if (UpdateInfo != null)
                            {
                                UpdateInfo(this, bytesCookie, RowData[M_AccountConst.Pid].ToString(), M_AccountConst.CookieFB);
                            }
                            this.client.CookieContainer = fb.StoreCookie;
                            strResponse = client.DoGet("http://vntexas0.boyaagame.com/");
                            client.AllowAutoRedirect = false;
                            strResponse = client.DoGet("https://www.facebook.com/connect/ping?client_id=373853122704634&domain=vntexas0.boyaagame.com&origin=1&redirect_uri=http%3A%2F%2Fstatic.ak.facebook.com%2Fconnect%2Fxd_arbiter.php%3Fversion%3D40%23cb%3Df3ccdac778%26domain%3Dvntexas0.boyaagame.com%26origin%3Dhttp%253A%252F%252Fvntexas0.boyaagame.com%252Ff292069cbc%26relation%3Dparent&response_type=token%2Csigned_request%2Ccode&sdk=joey");
                            client.AllowAutoRedirect = true;
                            strLocation = client.ResponseHeaders[HttpResponseHeader.Location];
                            strSignedRequest = Regex.Match(strLocation, "signed_request=[^&]+").Value.Replace("signed_request=", string.Empty).Trim();
                            if (string.IsNullOrEmpty(strSignedRequest))
                            {
                                this.Status = "Không Thể Authen Facebook";
                                this.HadAuthen = false;
                                return false;
                            }
                        }
                    }
                    client.CookieContainer.Add(new Cookie("fbm_373853122704634", "base_domain=.boyaagame.com", "/", "vntexas0.boyaagame.com"));
                    client.CookieContainer.Add(new Cookie("fbsr_373853122704634", strSignedRequest, "/", "vntexas0.boyaagame.com"));
                }
            }
            #endregion
        FBeginAuthen: ;
            strResponse = client.DoGet("http://vntexas0.boyaagame.com/");

            if (!string.IsNullOrEmpty(strResponse))
            {
                strSignedRequest = string.Empty;
                strSignedRequest = Regex.Match(strResponse, "name=\"signed_request\" value=\"[^\"]+").Value;
                strSignedRequest = Regex.Replace(strSignedRequest, "name=\"signed_request\" value=\"", string.Empty, RegexOptions.IgnoreCase);
                if (string.IsNullOrEmpty(strSignedRequest))
                {
                    if (RowData[M_AccountConst.CookiePoker] is byte[])
                    {
                        RowData[M_AccountConst.CookiePoker] = DBNull.Value;
                        goto FCheckCookieFacebook;
                    }
                    else
                    {
                        goto FAuthenFail;
                    }
                }
                param = new NameValueCollection();
                param.Add("signed_request", strSignedRequest);
                dicHeader = new Dictionary<HttpRequestHeader, string>();
                dicHeader.Add(HttpRequestHeader.Referer, "http://vntexas0.boyaagame.com/");
                strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/facebookvn/?_hd=1&token=&sign=", dicHeader);
                if (!string.IsNullOrEmpty(strResponse))
                {
                    if (strResponse.Contains("<script>window.open"))
                    {
                        if (RowData[M_AccountConst.CookiePoker] is byte[])
                        {
                            RowData[M_AccountConst.CookiePoker] = DBNull.Value;
                            goto FCheckCookieFacebook;
                        }
                        else
                        {
                            goto FAuthenFail;
                        }
                    }
                    else if (strResponse.Contains("close.php"))
                    {
                        this.Status = "Account Die .";
                        this.AccountDie = true;
                        this.HadAuthen = false;
                        return false;
                    }
                    string strMid = Regex.Match(strResponse, @"mid:[\s\d]+").Value;
                    Mid = strMid.Replace("mid:", string.Empty).Trim();

                    string strLdTime = Regex.Match(strResponse, @"time:[\s\d]+").Value;
                    Ldtime = strLdTime.Replace("time:", string.Empty).Trim();

                    string strLdmactivetime = Regex.Match(strResponse, @"ldmactivetim[^,]+").Value;
                    Ldmactivetime = strLdmactivetime.Replace("ldmactivetime\":", string.Empty).Replace("\"", string.Empty).Trim();

                    string strLsig = Regex.Match(strResponse, "ldsig\":\"[0-9a-zA-Z]+").Value;
                    Ldsig = strLsig.Replace("ldsig\":\"", string.Empty).Trim();

                    //string strSid = Regex.Match(strResponse, @"sid:[\s\d]+").Value;
                    //Sid = strSid.Replace("sid:", string.Empty).Trim();

                    string strMtkey = Regex.Match(strResponse, @"mtkey:[\s']+[^']+").Value;
                    Mtkey = Regex.Replace(strMtkey, @"mtkey:[\s']+", string.Empty, RegexOptions.IgnoreCase);

                    string strLoginkey = Regex.Match(strResponse, @"loginkey:[\s']+[^']+").Value;
                    Loginkey = Regex.Replace(strLoginkey, @"loginkey:[\s']+", string.Empty, RegexOptions.IgnoreCase);

                    string strApik = Regex.Match(strResponse, @"apik:[\s']+[^']+").Value;
                    Apik = Regex.Replace(strApik, @"apik:[\s']+", string.Empty, RegexOptions.IgnoreCase);

                    string strSitemid = Regex.Match(strResponse, @"sitemid:[\s']+[^']+").Value;
                    Sitemid = Regex.Replace(strSitemid, @"sitemid:[\s']+", string.Empty, RegexOptions.IgnoreCase);

                    string strSig = Regex.Match(strResponse, @"sig=[a-zA-Z0-9]+").Value;
                    Sig = Regex.Replace(strSig, @"sig=", string.Empty, RegexOptions.IgnoreCase);

                    string strMnick = Regex.Match(strResponse, @"mnick:[\s']+[^']+").Value;
                    Mnick = Regex.Replace(strMnick, @"mnick:[\s']+", string.Empty, RegexOptions.IgnoreCase);

                    string strDayMoney = Regex.Match(strResponse, @"dayMoney:[\s\d]+").Value;
                    GotdayMoney = false;
                    RowData[M_AccountConst.CookiePoker] = serial.ConvertObjectToBlob(this.client.CookieContainer);
                    if (UpdateInfo != null)
                    {
                        UpdateInfo(this, RowData[M_AccountConst.CookiePoker], RowData[M_AccountConst.Pid].ToString(), M_AccountConst.CookiePoker);
                    }
                    bHadAuthen = true;
                    this.Status = "Authen Thành Công";
                    return true;
                }
            }
        FAuthenFail: ;
            bHadAuthen = false;
            this.Status = "Authen Poker Thất Bại";
            return false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return false;
    }
    public bool AuthenPokerFBApp()
    {
        string strParam = string.Empty;
        this.Status = "Bắt đầu Authen.";
        if (RowData[M_AccountConst.CookieFB] is byte[])
        { //
            object objCookie = serial.ConvertBlobToObject(RowData[M_AccountConst.CookieFB] as byte[]);
            if (objCookie is CookieContainer)
            {
                #region - Using Store Cookie -
                this.client.CookieContainer = objCookie as CookieContainer;
                strResponse = client.DoGet("http://vntexas0.boyaagame.com/");
                strResponse = client.DoGet("https://apps.facebook.com/vntexas/");
                strParam = Regex.Match(strResponse, "name=\"signed_request\" value=\"[^\"]+").Value;
                strParam = Regex.Replace(strParam, "name=\"signed_request\" value=\"", string.Empty, RegexOptions.IgnoreCase);
                if (!string.IsNullOrEmpty(strParam))
                {
                    param = new NameValueCollection();
                    param.Add("signed_request", strParam);
                    dicHeader = new Dictionary<HttpRequestHeader, string>();
                    dicHeader.Add(HttpRequestHeader.Referer, "https://apps.facebook.com/vntexas/");
                    strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/facebookvn/", dicHeader);
                    if (!string.IsNullOrEmpty(strResponse))
                    {
                        string strMid = Regex.Match(strResponse, @"mid:[\s\d]+").Value;
                        Mid = strMid.Replace("mid:", string.Empty).Trim();

                        string strLdTime = Regex.Match(strResponse, @"time:[\s\d]+").Value;
                        Ldtime = strLdTime.Replace("time:", string.Empty).Trim();

                        string strLdmactivetime = Regex.Match(strResponse, @"ldmactivetim[^,]+").Value;
                        Ldmactivetime = strLdmactivetime.Replace("ldmactivetime\":", string.Empty).Replace("\"", string.Empty).Trim();

                        string strLsig = Regex.Match(strResponse, "ldsig\":\"[0-9a-zA-Z]+").Value;
                        Ldsig = strLsig.Replace("ldsig\":\"", string.Empty).Trim();

                        //string strSid = Regex.Match(strResponse, @"sid:[\s\d]+").Value;
                        //Sid = strSid.Replace("sid:", string.Empty).Trim();

                        string strMtkey = Regex.Match(strResponse, @"mtkey:[\s']+[^']+").Value;
                        Mtkey = Regex.Replace(strMtkey, @"mtkey:[\s']+", string.Empty, RegexOptions.IgnoreCase);

                        string strLoginkey = Regex.Match(strResponse, @"loginkey:[\s']+[^']+").Value;
                        Loginkey = Regex.Replace(strLoginkey, @"loginkey:[\s']+", string.Empty, RegexOptions.IgnoreCase);

                        string strApik = Regex.Match(strResponse, @"apik:[\s']+[^']+").Value;
                        Apik = Regex.Replace(strApik, @"apik:[\s']+", string.Empty, RegexOptions.IgnoreCase);

                        string strSitemid = Regex.Match(strResponse, @"sitemid:[\s']+[^']+").Value;
                        Sitemid = Regex.Replace(strSitemid, @"sitemid:[\s']+", string.Empty, RegexOptions.IgnoreCase);

                        string strSig = Regex.Match(strResponse, @"sig=[a-zA-Z0-9]+").Value;
                        Sig = Regex.Replace(strSig, @"sig=", string.Empty, RegexOptions.IgnoreCase);

                        string strMnick = Regex.Match(strResponse, @"mnick:[\s']+[^']+").Value;
                        Mnick = Regex.Replace(strMnick, @"mnick:[\s']+", string.Empty, RegexOptions.IgnoreCase);

                        string strDayMoney = Regex.Match(strResponse, @"dayMoney:[\s\d]+").Value;
                        GotdayMoney = false;

                        bHadAuthen = true;
                        this.Status = "Authen Thành Công";

                        CookieCollection ckCollect = client.CookieContainer.GetCookies(new Uri("http://www.vntexas0.boyaagame.com/"));
                        foreach (Cookie ck in ckCollect)
                        {
                            client.CookieContainer.Add(new Uri("http://vntexas0.boyaagame.com/"), new Cookie(ck.Name, ck.Value, ck.Path));
                        }
                        return true;
                    }
                }
                #endregion
            }
        }

        bHadAuthen = false;
        this.Status = "Authen Poker Thất Bại";
        return false;
    }
    public void NhanThuongHangNgay(object objStep)
    {
        if (objStep is string)
        {
            if (!bHadAuthen)
            {
                if (!AuthenPoker())
                {
                    return;
                }
            }
            if (objStep.ToString() == "S1")
            {
                this.Status = "Request Captcha ...";
                //get day bonus
                strResponse = client.DoGet("http://vntexas0.boyaagame.com/texas/ajax/verifycode/verifyCode.php?sid=" + Sid + "&mid=" + Mid + "&mtkey=" + Mtkey + "&langtype=13");
                if (!string.IsNullOrEmpty(strResponse) && strResponse.Trim() == "{\"status\":0}")
                {
                    strResponse = client.DoGet("http://vntexas0.boyaagame.com/texas/ajax/verifycode/verifycode.html?" + Convert.ToInt32(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 7, 0, 0)).TotalSeconds));
                    if (!string.IsNullOrEmpty(strResponse))
                    {
                        objStep = "S2";
                    }
                }
                else if (!string.IsNullOrEmpty(strResponse) && strResponse.Trim() == "{\"status\":\"vip3\"}")
                {
                    strResponse = client.DoGet("http://vntexas0.boyaagame.com/texas/ajax/verifycode/getDaymoney.php?sid=" + Sid + "&mid=" + Mid + "&mtkey=" + Mtkey + "&langtype=13");
                    GotdayMoney = true;
                    this.Status = "Nhận Thưởng Thành Công";
                }
                else
                {

                    GotdayMoney = true;
                    this.Status = "Hôm nay đã nhận thưởng rồi !";
                }
            }
            if (objStep.ToString() == "S2")
            {
                this.Status = "Request Captcha ...";
                Image img = client.GetImage("http://vntexas0.boyaagame.com/texas/valid2.php?mid=" + Mid + "&v=" + new Random().NextDouble());
                if (img != null)
                {
                    this.imgCaptcha = img;
                    this.Status = "Request Captcha Thành Công";
                }
                else
                {
                    this.Status = "Request Captcha Thất Bại";
                }
                bGotCaptchaDayMoney = true;
            }
            if (objStep.ToString().IndexOf("S3") == 0)
            {
                strResponse = client.DoGet("http://vntexas0.boyaagame.com/texas/ajax/verifycode/verifyCode.php?sid=" + Sid + "&mid=" + Mid + "&mtkey=" + Mtkey + "&langtype=13&vcc=d341c9&code=" + objStep.ToString().Substring(2));
                if (!string.IsNullOrEmpty(strResponse) && strResponse.Contains("suc"))
                {
                    strResponse = client.DoGet("http://vntexas0.boyaagame.com/texas/ajax/verifycode/verifyCode.php?sid=" + Sid + "&mid=" + Mid + "&mtkey=" + Mid + "&langtype=13");
                    strResponse = client.DoGet("http://vntexas0.boyaagame.com/texas/ajax/verifycode/getDaymoney.php?sid=" + Sid + "&mid=" + Mid + "&mtkey=" + Mtkey + "&langtype=13");
                    this.Status = "Nhận Thưởng Thành Công";
                    GotdayMoney = true;

                    //Chia se
                    param = new NameValueCollection();
                    param.Add("ref", "574");
                    param.Add("mid", this.Mid);
                    param.Add("sid", this.Sid);
                    param.Add("mtkey", this.Mtkey);
                    param.Add("sitemid", this.Sitemid);
                    param.Add("langtype", "13");
                    param.Add("mnick", this.Mnick);
                    param.Add("flag", "1");
                    strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/api/facebook/uis.php");
                    ////Moi ban
                    //param = new NameValueCollection();
                    //param.Add("ref","418");
                    //param.Add("filters[]","app_non_users");
                    //param.Add("act","1001");
                    //param.Add("tpl","contain418");
                    //param.Add("mid",this.Mid);
                    //param.Add("sid","110");
                    //param.Add("mtkey",this.Mtkey);
                    //param.Add("sitemid",this.Sitemid);
                    //param.Add("langtype", "13");
                    //strResponse = request.DoPost(client, param, "http://vntexas0.boyaagame.com/texas/api/facebook/rest.php");

                    //param = new NameValueCollection();
                    //param.Add("by_ref","418");
                    //param.Add("by_mid",this.Mid);
                    //param.Add("by_langtype","13");
                    //param.Add("by_time", this.Ldtime);
                    //param.Add("by_sig",this.Sig);
                    //param.Add("act", "1002");
                    //param.Add("to[]", "100002494" + new Random().Next(0, 999999));
                    //param.Add("mid", this.Mid);
                    //param.Add("sid", "110");
                    //param.Add("mtkey",this.Mtkey);
                    //param.Add("sitemid",this.Sitemid);
                    //param.Add("langtype", "13");
                    //strResponse = request.DoPost(client, param, "http://vntexas0.boyaagame.com/texas/api/facebook/rest.php");


                    //Quay Vong
                    strResponse = client.DoGet("http://vntexas0.boyaagame.com/texas/activite/wheel/ajax.php?sid=" + this.Sid + "&mid=" + this.Mid + "&mtkey=" + this.Mtkey + "&langtype=13&cmd=goturn");
                    System.Threading.Thread.Sleep(2000);
                    strResponse = client.DoGet("http://vntexas0.boyaagame.com/texas/activite/wheel/ajax.php?sid=" + this.Sid + "&mid=" + this.Mid + "&mtkey=" + this.Mtkey + "&langtype=13&cmd=goturn");
                    //System.Threading.Thread.Sleep(2000);
                    //strResponse = request.DoGet(client, "http://vntexas0.boyaagame.com/texas/activite/wheel/ajax.php?sid=" + this.Sid + "&mid=" + this.Mid + "&mtkey=" + this.Mtkey + "&langtype=13&cmd=goturn");
                }
                else
                {
                    this.Status = "Nhập Sai Captcha";
                    bWrongCaptcha = true;
                }
            }
        }
    }
    public void NhanChipMayMan(object objStep)
    {
        try
        {
            if (!bHadAuthen)
            {
                if (!AuthenPokerFBApp())
                {
                    return;
                }
            }
            if (objStep is List<string>)
            {//Request Chip ^^
                List<string> listLink = objStep as List<string>;
                this.Status = "Đang Nhận Chip May Mắn ...";
                foreach (string strLink in listLink)
                {
                    param = new NameValueCollection();
                    param.Add("by_ref", "27");
                    param.Add("by_langtype", "13");
                    string strRegex = Regex.Match(strLink, "by_mid=[0-9]+").Value;
                    param.Add("by_mid", strRegex.Replace("by_mid=", string.Empty).Trim());
                    strRegex = Regex.Match(strLink, "by_time=[0-9]+").Value;
                    param.Add("by_time", strRegex.Replace("by_time=", string.Empty).Trim());
                    strRegex = Regex.Match(strLink, "by_sig=[0-9a-zA-Z]+").Value;
                    param.Add("by_sig", strRegex.Replace("by_sig=", string.Empty).Trim());
                    param.Add("loginkey", this.Loginkey);
                    param.Add("act", "1003");
                    //param.Add("logindata[ldtime]", this.Ldtime);
                    //param.Add("logindata[ldmactivetime]", this.Ldmactivetime);
                    //param.Add("logindata[ldtype]", "0");
                    //param.Add("logindata[ldsig]", this.Ldsig);
                    param.Add("mid", this.Mid);
                    param.Add("sid", "110");
                    param.Add("mtkey", this.Mtkey);
                    param.Add("sitemid", this.Sitemid);
                    param.Add("langtype", "13");

                    dicHeader = new Dictionary<HttpRequestHeader, string>();
                    dicHeader.Add(HttpRequestHeader.Referer, strLink);
                    strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/api/facebook/rest.php", dicHeader);
                    if (!string.IsNullOrEmpty(strResponse))
                    {
                    }
                }
                this.Status = "Đã Nhập Toàn Bộ Chip May Mắn";
                //ki ten

                param = new NameValueCollection();

                param.Add("act", "checkin");
                if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                {
                    param.Add("num", "1");
                }
                if (DateTime.Today.DayOfWeek == DayOfWeek.Tuesday)
                {
                    param.Add("num", "2");
                }
                if (DateTime.Today.DayOfWeek == DayOfWeek.Wednesday)
                {
                    param.Add("num", "3");
                }
                if (DateTime.Today.DayOfWeek == DayOfWeek.Thursday)
                {
                    param.Add("num", "4");
                }
                if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
                {
                    param.Add("num", "5");
                }
                if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday)
                {
                    param.Add("num", "6");
                }
                if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                {
                    param.Add("num", "7");
                }
                strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/includes/newcheckin.php?sid=" + this.Sid + "&mid=" + this.Mid + "&mtkey=" + this.Mtkey + "&langtype=13&sid=" + this.Sid + "&langtype=13&t=" + new Random().NextDouble().ToString());

                if (!string.IsNullOrEmpty(strResponse))
                {
                }

                //Nhan thuong ki ten
                if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                {
                    this.Status = "Bắt Đầu Nhận Chip Thưởng Ký Tên";

                    param = new NameValueCollection();

                    param.Add("act", "reward");
                    param.Add("num", "1");
                    strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/includes/newcheckin.php?sid=" + this.Sid + "&mid=" + this.Mid + "&mtkey=" + this.Mtkey + "&langtype=13&sid=" + this.Sid + "&langtype=13&t=" + new Random().NextDouble().ToString());
                    if (!string.IsNullOrEmpty(strResponse))
                    {
                    }

                    param = new NameValueCollection();

                    param.Add("act", "reward");
                    param.Add("num", "2");
                    strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/includes/newcheckin.php?sid=" + this.Sid + "&mid=" + this.Mid + "&mtkey=" + this.Mtkey + "&langtype=13&sid=" + this.Sid + "&langtype=13&t=" + new Random().NextDouble().ToString());
                    if (!string.IsNullOrEmpty(strResponse))
                    {
                    }
                    this.Status = "Nhận Chip Thưởng Ký Tên Thành Công";
                }
                if (string.IsNullOrEmpty(Apik.Trim()))
                {
                    this.Status = "Account có vấn đề, cần check lại !";
                }
                //Ky ten Bingo
                {
                    #region - Bingo -
                    //this.Status = "Bắt Đầu Kí Tên Bingo";
                    //List<string> listDay = new List<string>();
                    //List<string> listSign = new List<string>();
                    //List<string> listWeek = new List<string>();
                    //List<string> listPerfect = new List<string>();

                    //param = new NameValueCollection();
                    //param.Add("apik", Apik);
                    //client.DoPost(param, "http://vntexas0.boyaagame.com/texas/act/233/ajax.php?cmd=info");
                    //listDay = Regex.Match(client.ResponseText, "\"daystr\":\"([^\"]+)").Groups[1].Value.Split(',').ToList();
                    //listSign = Regex.Match(client.ResponseText, "\"signstr\":\"([^\"]+)").Groups[1].Value.Split(',').ToList();
                    //listWeek = Regex.Match(client.ResponseText, "\"recstr\":\"(\\d,\\d,\\d,\\d,\\d)").Groups[1].Value.Split(',').ToList();
                    //listPerfect = Regex.Match(client.ResponseText, "\"recstr\":\"\\d,\\d,\\d,\\d,\\d,(\\d,\\d,\\d,\\d,\\d,\\d)").Groups[1].Value.Split(',').ToList();
                    //if (listDay.Count > 10 && listSign.Count > 10)
                    //{
                    //    //Check sign today
                    //    if (listDay.Contains(DateTime.Today.ToString("MMdd")) && listSign[listDay.IndexOf(DateTime.Today.ToString("MMdd"))] == "1")
                    //    {
                    //        param = new NameValueCollection();
                    //        param.Add("apik", Apik);
                    //        strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/act/233/ajax.php?cmd=sign&date=" + DateTime.Today.ToString("MMdd"));
                    //        listSign[listDay.IndexOf(DateTime.Today.ToString("MMdd"))] = "2";
                    //        System.Threading.Thread.Sleep(2000);
                    //    }
                    //    //Sign missed day
                    //    if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                    //    {
                    //        for (int iIndex = 0; iIndex < 6; iIndex++)
                    //        {
                    //            if (listDay.Contains(DateTime.Today.AddDays(iIndex - 6).ToString("MMdd")) && listSign[listDay.IndexOf(DateTime.Today.AddDays(iIndex - 6).ToString("MMdd"))] != "2")
                    //            {
                    //                param = new NameValueCollection();
                    //                param.Add("apik", Apik);
                    //                client.DoPost(param, "http://vntexas0.boyaagame.com/texas/act/233/ajax.php?cmd=resign&date=" + DateTime.Today.AddDays(iIndex - 6).ToString("MMdd"));
                    //                listSign[listDay.IndexOf(DateTime.Today.AddDays(iIndex - 6).ToString("MMdd"))] = "2";
                    //                System.Threading.Thread.Sleep(2000);
                    //                break;
                    //            }
                    //        }
                    //    }
                    //    //Check sign perfect Bingo
                    //    //if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                    //    {
                    //        //Check sign every day of week
                    //        for (int iIndex = 0; iIndex < 5; iIndex++)
                    //        {
                    //            bool bCheckAllWeek = true;
                    //            for (int iDay = iIndex * 5; iDay < (iIndex + 1) * 5; iDay++)
                    //            {
                    //                if (listSign[iDay] != "2")
                    //                {
                    //                    bCheckAllWeek = false;
                    //                }
                    //            }
                    //            if (bCheckAllWeek && listWeek[iIndex] != "2")
                    //            {
                    //                param = new NameValueCollection();
                    //                param.Add("apik", Apik);
                    //                strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/act/233/ajax.php?cmd=award&type=" + iIndex);
                    //                System.Threading.Thread.Sleep(2000);
                    //                listWeek[iIndex] = "2";
                    //            }
                    //        }
                    //        //Check sign day of month
                    //        for (int iIndex = 0; iIndex < 5; iIndex++)
                    //        {
                    //            bool bCheckAll = true;
                    //            for (int iDay = 0; iDay < 5; iDay++)
                    //            {
                    //                if (listSign[iDay * 5 + iIndex] != "2")
                    //                {
                    //                    bCheckAll = false;
                    //                }
                    //            }
                    //            if (bCheckAll && listPerfect[iIndex] != "2")
                    //            {
                    //                param = new NameValueCollection();
                    //                param.Add("apik", Apik);
                    //                strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/act/233/ajax.php?cmd=award&type=" + (iIndex + 5));
                    //                System.Threading.Thread.Sleep(2000);
                    //                listPerfect[iIndex] = "2";
                    //            }
                    //        }
                    //        //Check perfect total
                    //        bool bPerfectBingo = true;
                    //        for (int iIndex = 0; iIndex < listSign.Count; iIndex++)
                    //        {
                    //            if (listSign[iIndex] != "2")
                    //            {
                    //                bPerfectBingo = false;
                    //                break;
                    //            }
                    //        }
                    //        if (bPerfectBingo)
                    //        {
                    //            param = new NameValueCollection();
                    //            param.Add("apik", Apik);
                    //            strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/act/233/ajax.php?cmd=award&type=10");
                    //            System.Threading.Thread.Sleep(2000);
                    //        }
                    //    }
                    //}
                    //this.Status = "Kí Tên Bingo Hoàn Thành";
                    #endregion
                }
                //Ky ten WorldCup
                {
                    this.Status = "Bắt Đầu Kí Tên World Cup";
                    param = new NameValueCollection();
                    param.Add("cmd", "init");
                    param.Add("apik", Apik);
                    client.DoPost(param, "https://vntexas0.boyaagame.com/texas/act/648/ajax.php");
                    if (client.ResponseText.Contains("hasSignToday\":false"))
                    {
                        param = new NameValueCollection();
                        param.Add("cmd", "sign");
                        param.Add("apik", Apik);
                        client.DoPost(param, "https://vntexas0.boyaagame.com/texas/act/648/ajax.php");
                    }
                    this.Status = "Kí Tên World Cup Hoàn Thành";
                }
            }
            else
            {//Request Link Chip
                this.Status = "Request Link Chip May Mắn ";
                param = new NameValueCollection();
                param.Add("ref", "27");
                param.Add("mid", this.Mid);
                param.Add("sid", this.Sid);
                param.Add("mtkey", this.Mtkey);
                param.Add("sitemid", this.Sitemid);
                param.Add("langtype", "13");
                param.Add("mnick", this.Mnick);
                strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/api/facebook/ui.php");
                if (!string.IsNullOrEmpty(strResponse))
                {
                    string strLink = Regex.Match(strResponse, "\"link\":\"[^\"]+").Value;
                    strLink = Regex.Replace(strLink, "\"link\":\"", string.Empty).Trim();
                    strLink = strLink.Replace("\\/", "/");
                    listLinkLuckyChip.Add(strLink);
                }

                param = new NameValueCollection();
                param.Add("ref", "27");
                param.Add("mid", this.Mid);
                param.Add("sid", this.Sid);
                param.Add("mtkey", this.Mtkey);
                param.Add("sitemid", this.Sitemid);
                param.Add("langtype", "13");
                param.Add("mnick", this.Mnick);
                param.Add("flag", "1");
                strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/api/facebook/uis.php");

                System.Threading.Thread.Sleep(567);
                bGotLinkChip = true;
                this.Status = string.Format("Request Link Chip May Mắn Hoàn Thành");
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }
    }
    public void NhanThuongFanHangNgay(object objRef)
    {
        if (!bHadAuthen)
        {
            if (!AuthenPoker())
            {
                return;
            }
        }
        this.Status = "Bắt Đầu Nhận Thưởng Fan";
        param = new NameValueCollection();
        param.Add("ref", "135");
        param.Add("mid", this.Mid);
        param.Add("sid", this.Sid);
        param.Add("mtkey", this.Mtkey);
        param.Add("sitemid", this.Sitemid);
        param.Add("langtype", "13");
        param.Add("mnick", this.Mnick);
        strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/api/facebook/ui.php");
        System.Threading.Thread.Sleep(1000);

        param = new NameValueCollection();
        param.Add("ref", "135");
        param.Add("mid", this.Mid);
        param.Add("sid", this.Sid);
        param.Add("mtkey", this.Mtkey);
        param.Add("sitemid", this.Sitemid);
        param.Add("langtype", "13");
        param.Add("mnick", this.Mnick);
        param.Add("flag", "1");
        strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/api/facebook/uis.php");
        this.Status = "Nhận Thưởng Fan Thành Công";
    }
    public void NhanCo4La(object objStep)
    {
        if (!bHadAuthen)
        {
            if (!AuthenPoker())
            {
                return;
            }
        }
        if (objStep is List<string>)
        {//Send Co 4 La
            List<string> listFBID = objStep as List<string>;
            this.Status = "Bắt Đầu Tặng Cỏ 4 Lá ...";
            param = new NameValueCollection();
            param.Add("ref", "30");
            param.Add("act", "1001");
            param.Add("tpl", "contain30");
            param.Add("mid", this.Mid);
            param.Add("sid", this.Sid);
            param.Add("mtkey", this.Mtkey);
            param.Add("sitemid", this.Sitemid);
            param.Add("langtype", "13");
            strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/api/facebook/rest.php");
            if (!string.IsNullOrEmpty(strResponse))
            {
                System.Threading.Thread.Sleep(1234);
                string strParams = string.Empty;
                strParams += "by_ref=30&by_mid=" + this.Mid;
                strParams += "&by_langtype=13";

                string strRegex = Regex.Match(strResponse, "by_time\":[\\s0-9]+").Value;
                strRegex = strRegex.Replace("by_time\":", string.Empty).Trim();
                strParams += "&by_time=" + strRegex;

                strRegex = Regex.Match(strResponse, "by_sig\":[\"\\s0-9a-zA-Z]+").Value;
                strRegex = strRegex.Replace("by_sig\":", string.Empty).Trim();
                strRegex = strRegex.Replace("\"", string.Empty).Trim();
                strParams += "&by_sig=" + strRegex;

                strParams += "&act=1002";
                foreach (string strFBID in listFBID)
                {
                    strParams += "&to[]=" + strFBID;
                }
                strParams += "&mid=" + this.Mid;
                strParams += "&sid=" + this.Sid;
                strParams += "&mtkey=" + this.Mtkey;
                strParams += "&sitemid=" + this.Sitemid;
                strParams += "&langtype=13";
                strResponse = client.DoPost(strParams, "http://vntexas0.boyaagame.com/texas/api/facebook/rest.php");
                this.Status = "Tặng Cỏ 4 Lá Thành Công";
            }
            else
            {
                this.Status = "Tặng Cỏ 4 Lá Thất Bại";
            }
            bSendChip = true;
        }
        else
        { //Reciver Co 4 La
            this.Status = "Bắt Đầu Nhận Cỏ 4 Lá ...";
            CookieCollection ckCollect = client.CookieContainer.GetCookies(new Uri("http://vntexas0.boyaagame.com/"));
            foreach (Cookie ck in ckCollect)
            {
                if (ck.Name.Contains("REQUEST|"))
                {
                    ck.Expires = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                    //client.CookieContainer.SetCookies(new Uri("http://vntexas0.boyaagame.com/"), ck.Name + "=.");
                }
            }
            strResponse = client.DoGet("http://vntexas0.boyaagame.com/texas/ajax/message.php?sid=" + this.Sid + "&mid=" + this.Mid + "&mtkey=" + this.Mtkey + "&langtype=13");
            if (!string.IsNullOrEmpty(strResponse))
            {
                MatchCollection mc = Regex.Matches(strResponse, "id\":[0-9]+,\"cid\":1,\"type\":1");
                if (mc.Count > 0)
                {
                    for (int iIndex = 0; iIndex < 12 && iIndex < mc.Count; iIndex++)
                    {
                        string strId = Regex.Match(mc[iIndex].Value, "id\":[0-9]+").Value;
                        strId = strId.Replace("id\":", string.Empty).Trim();
                        param = new NameValueCollection();
                        param.Add("op", "1");
                        param.Add("msgid[]", strId);
                        int iCountfRequestCo4La = 0;
                    fRequestCo4La: ;
                        strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/ajax/message.php?sid=" + this.Sid + "&mid=" + this.Mid + "&mtkey=" + this.Mtkey + "&langtype=13");
                        if (string.IsNullOrEmpty(strResponse) || strResponse.Contains("Error request"))
                        {
                            if (iCountfRequestCo4La < 3)
                            {
                                iCountfRequestCo4La++;
                                goto fRequestCo4La;
                            }
                        }
                        System.Threading.Thread.Sleep(1500);
                    }
                }
            }

            this.Status = "Nhận Cỏ 4 Lá Thành Công";
        }
    }
    public void NhanThuongFanNhom(object objStep)
    {
        if (!bHadAuthen)
        {
            if (!AuthenPoker())
            {
                return;
            }
        }
        this.Status = "Bắt Đầu Nhận Chip Thưởng Fan Nhóm";
        string strLink = objStep.ToString();

        param = new NameValueCollection();
        param.Add("by_ref", "77");

        string strRegex = Regex.Match(strLink, "by_flid=[0-9]+").Value;
        param.Add("by_flid", strRegex.Replace("by_flid=", string.Empty).Trim());

        strRegex = Regex.Match(strLink, "by_time=[0-9]+").Value;
        param.Add("by_time", strRegex.Replace("by_time=", string.Empty).Trim());

        strRegex = Regex.Match(strLink, "by_mid=[0-9]+").Value;

        strRegex = Regex.Match(strLink, "by_sig=[0-9a-zA-Z]+").Value;
        param.Add("by_sig", strRegex.Replace("by_sig=", string.Empty).Trim());

        param.Add("act", "1003");
        param.Add("logindata[ldtime]", this.Ldtime);
        param.Add("logindata[ldmactivetime]", this.Ldmactivetime);
        param.Add("logindata[ldtype]", "0");
        param.Add("logindata[ldsig]", this.Ldsig);
        param.Add("mid", this.Mid);
        param.Add("sid", this.Sid);
        param.Add("mtkey", this.Mtkey);
        param.Add("sitemid", this.Sitemid);
        param.Add("langtype", "13");
        dicHeader = new Dictionary<HttpRequestHeader, string>();
        dicHeader.Add(HttpRequestHeader.Referer, strLink);
        strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/api/facebook/rest.php", dicHeader);
        if (!string.IsNullOrEmpty(strResponse))
        {
        }
        this.Status = "Nhận Chip Thưởng Fan Nhóm Thành Công";
    }
    public void NhanKyTen(object objStep)
    {
        try
        {
            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
            {
                if (!bHadAuthen)
                {
                    if (!AuthenPoker())
                    {
                        return;
                    }
                }
                this.Status = "Bắt Đầu Nhận Chip Thưởng Ký Tên";

                param = new NameValueCollection();

                param.Add("act", "reward");
                param.Add("num", "1");
                strResponse = client.DoPost(param, "http://vntexas0.boyaagame.com/texas/includes/newcheckin.php?sid=" + this.Sid + "&mid=" + this.Mid + "&mtkey=" + this.Mtkey + "&langtype=13&sid=" + this.Sid + "&langtype=13&t=" + new Random().NextDouble().ToString());
                if (!string.IsNullOrEmpty(strResponse))
                {
                }
                this.Status = "Nhận Chip Thưởng Ký Tên Thành Công";
            }
        }
        catch (Exception ex)
        {

        }
    }
}
