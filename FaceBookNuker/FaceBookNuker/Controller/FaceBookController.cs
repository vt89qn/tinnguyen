using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using OpenPop.Pop3;
using FaceBookNuker.Models;
using System.IO;

namespace FaceBookNuker.Controller
{
    class FaceBookController
    {
        #region - DECLARE -
        public event EventHandler<StatusChangedEventArgs> StatusChanged;
        #endregion
        #region - PROPERTIES -
        //Global Var
        public FaceBook Models { get; set; }
        public Exception Error { get; set; }
        public WebClientEx WebClient { get; set; }
        //Grid var
        public string GridStatus { get; set; }
        public int GridIndex { get; set; }
        //working var
        public bool IsWorking { get; set; }
        public bool WorkingFail { get; set; }
        public int SessionID { get; set; }
        #endregion
        #region - CONTRUCTOR -
        public FaceBookController()
        {//Do nothing !
            WebClient = new WebClientEx();
        }
        #endregion
        #region - METHOD -
        public bool CheckLogin()
        {
            try
            {
                SetStatusChanged("Begin login");
                NameValueCollection param = new NameValueCollection();
                //WebClient = new WebClientEx();
                WebClient.DoGet("https://www.facebook.com/login.php");
                this.Error = WebClient.Error;
                if (!string.IsNullOrEmpty(WebClient.ResponseText))
                {
                    if (WebClient.ResponseText.Contains("logout.php") && !WebClient.ResponseText.Contains("login.php"))
                    {
                        SetStatusChanged("Login successful");
                        return true;
                    }
                    //param.Add("lsd", Regex.Match(strResponse,"name=\"lsd\" value=\"([^\"]+)").Groups[1].Value);
                    //param.Add("display", Regex.Match(strResponse,"name=\"display\" value=\"(?<1>()|[^\"]+)").Groups[1].Value);
                    //param.Add("enable_profile_selector", Regex.Match(strResponse,"name=\"enable_profile_selector\" value=\"(?<1>()|[^\"]+)").Groups[1].Value);
                    //param.Add("legacy_return", Regex.Match(strResponse,"name=\"legacy_return\" value=\"(?<1>()|[^\"]+)").Groups[1].Value);
                    //param.Add("profile_selector_ids", Regex.Match(strResponse,"name=\"profile_selector_ids\" value=\"(?<1>()|[^\"]+)").Groups[1].Value);
                    //param.Add("trynum", Regex.Match(strResponse,"name=\"trynum\" value=\"(?<1>()|[^\"]+)").Groups[1].Value);
                    //param.Add("timezone", Regex.Match(strResponse,"name=\"timezone\" value=\"(?<1>()|[^\"]+)").Groups[1].Value);
                    //param.Add("lgnrnd", Regex.Match(strResponse,"name=\"lgnrnd\" value=\"(?<1>()|[^\"]+)").Groups[1].Value);
                    //param.Add("lgnjs", Regex.Match(strResponse,"name=\"lgnjs\" value=\"(?<1>()|[^\"]+)").Groups[1].Value);
                    param.Add("email", this.Models.Login == string.Empty ? this.Models.UserID : this.Models.Login);
                    param.Add("pass", this.Models.Pass);
                    //param.Add("persistent", Regex.Match(strResponse,"name=\"persistent\" value=\"(?<1>()|[^\"]+)").Groups[1].Value);
                    //param.Add("default_persistent", Regex.Match(strResponse,"name=\"default_persistent\" value=\"(?<1>()|[^\"]+)").Groups[1].Value);
                    WebClient.AllowAutoRedirect = false;
                    WebClient.DoPost(param, "https://www.facebook.com/login.php");
                    WebClient.AllowAutoRedirect = true;
                    this.Error = WebClient.Error;
                    string strLocation = WebClient.ResponseHeaders[HttpResponseHeader.Location];
                    if (strLocation.Contains("login.php"))
                    {
                        return false;
                    }
                    else if (strLocation.Contains("checkpoint"))
                    { //Try Sloved Check point
                        WebClient.DoGet(strLocation);
                        if (WebClient.ResponseText.Contains("If this account reflects your real name and personal information, please help us verify it"))
                        {
                            return false;
                        }
                        else if (WebClient.ResponseText.Contains("We’re working hard to make sure that everyone on Facebook uses their real identity"))
                        {
                            return false;
                        }
                        if (WebClient.ResponseText.Contains("This block will be lifted in 30 days"))
                        {//Block 30 days
                            param = new NameValueCollection();
                            param.Add("fb_dtsg", Regex.Match(WebClient.ResponseText, "name=\"fb_dtsg\"\\s+value=\"([^\"]+)").Groups[1].Value);
                            param.Add("nh", Regex.Match(WebClient.ResponseText, "name=\"nh\"\\s+value=\"([^\"]+)").Groups[1].Value);
                            param.Add("submit[Next]", "Next");
                            WebClient.DoPost(param, "https://www.facebook.com/checkpoint/?next");

                            param = new NameValueCollection();
                            param.Add("fb_dtsg", Regex.Match(WebClient.ResponseText, "name=\"fb_dtsg\"\\s+value=\"([^\"]+)").Groups[1].Value);
                            param.Add("nh", Regex.Match(WebClient.ResponseText, "name=\"nh\"\\s+value=\"([^\"]+)").Groups[1].Value);
                            param.Add("submit[Okay]", "OK");
                            WebClient.DoPost(param, "https://www.facebook.com/checkpoint/?next");

                            param = new NameValueCollection();
                            param.Add("fb_dtsg", Regex.Match(WebClient.ResponseText, "name=\"fb_dtsg\"\\s+value=\"([^\"]+)").Groups[1].Value);
                            param.Add("nh", Regex.Match(WebClient.ResponseText, "name=\"nh\"\\s+value=\"([^\"]+)").Groups[1].Value);
                            param.Add("confirm", "on");
                            param.Add("submit[Okay]", "OK");
                            WebClient.DoPost(param, "https://www.facebook.com/checkpoint/?next");

                        }
                        else
                        {//Login strainger computer
                            param = new NameValueCollection();
                            param.Add("fb_dtsg", Regex.Match(WebClient.ResponseText, "name=\"fb_dtsg\"\\s+value=\"([^\"]+)").Groups[1].Value);
                            param.Add("nh", Regex.Match(WebClient.ResponseText, "name=\"nh\"\\s+value=\"([^\"]+)").Groups[1].Value);
                            param.Add("submit[Continue]", "Continue");
                            WebClient.DoPost(param, "https://www.facebook.com/checkpoint/?next");
                            string strCaptchaURL = "https://www.facebook.com/captcha/tfbimage.php?" + Regex.Match(WebClient.ResponseText, "captcha_challenge_code=.+captcha_challenge_hash=[^\"]+").Value;
                            WebClient.GetImage(strCaptchaURL);

                            string strcaptcha_persist_data = Regex.Match(WebClient.ResponseText, "name=\"captcha_persist_data\"\\s+value=\"([^\"]+)").Groups[1].Value;
                            param = new NameValueCollection();
                            param.Add("fb_dtsg", Regex.Match(WebClient.ResponseText, "name=\"fb_dtsg\"\\s+value=\"([^\"]+)").Groups[1].Value);
                            param.Add("nh", Regex.Match(WebClient.ResponseText, "name=\"nh\"\\s+value=\"([^\"]+)").Groups[1].Value);
                            param.Add("geo", "true");
                            param.Add("captcha_persist_data", strcaptcha_persist_data);
                            param.Add("captcha_response", "yfrthh");
                            param.Add("achal", "8");
                            param.Add("submit[Submit]", "Submit");
                            WebClient.DoPost(param, "https://www.facebook.com/checkpoint/?next");

                            strcaptcha_persist_data = Regex.Match(WebClient.ResponseText, "name=\"captcha_persist_data\"\\s+value=\"([^\"]+)").Groups[1].Value;
                            param = new NameValueCollection();
                            param.Add("fb_dtsg", Regex.Match(WebClient.ResponseText, "name=\"fb_dtsg\"\\s+value=\"([^\"]+)").Groups[1].Value);
                            param.Add("nh", Regex.Match(WebClient.ResponseText, "name=\"nh\"\\s+value=\"([^\"]+)").Groups[1].Value);
                            param.Add("geo", "true");
                            if (WebClient.ResponseText.Contains("please enter your birthday"))
                            {
                                param.Add("birthday_captcha_month", "1");
                                param.Add("birthday_captcha_day", "1");
                                param.Add("birthday_captcha_year", "1981");
                            }
                            else
                            {
                                param.Add("captcha_response", "tintin");
                            }
                            param.Add("captcha_persist_data", strcaptcha_persist_data);
                            param.Add("achal", "5");
                            param.Add("submit[Continue]", "Continue");
                            WebClient.AllowAutoRedirect = false;
                            WebClient.DoPost(param, "https://www.facebook.com/checkpoint/?next");
                            WebClient.AllowAutoRedirect = true;

                            if (WebClient.ResponseText.Contains("Because it happened from a location you") && WebClient.ResponseText.Contains("normally log in from"))
                            {
                                param = new NameValueCollection();
                                param.Add("fb_dtsg", Regex.Match(WebClient.ResponseText, "name=\"fb_dtsg\"\\s+value=\"([^\"]+)").Groups[1].Value);
                                param.Add("nh", Regex.Match(WebClient.ResponseText, "name=\"nh\"\\s+value=\"([^\"]+)").Groups[1].Value);
                                param.Add("submit[This was me]", "This was me");
                                WebClient.AllowAutoRedirect = false;
                                WebClient.DoPost(param, "https://www.facebook.com/checkpoint/?next");
                                WebClient.AllowAutoRedirect = true;
                            }
                        }
                    }
                    WebClient.AllowAutoRedirect = false;
                    WebClient.DoGet("https://www.facebook.com/me/about");
                    WebClient.AllowAutoRedirect = true;
                    this.Error = WebClient.Error;
                    strLocation = WebClient.ResponseHeaders[HttpResponseHeader.Location];
                    if (!string.IsNullOrEmpty(strLocation) && !strLocation.Contains("login.php"))
                    {
                        this.Models.UserID = string.Empty;
                        foreach (Cookie cookie in WebClient.CookieContainer.GetCookies(new Uri("https://www.facebook.com/")))
                        {
                            if (cookie.Name == "c_user")
                            {
                                this.Models.UserID = cookie.Value;
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(Models.UserID))
                        {
                            SetStatusChanged("Login successful");
                            this.Models.Login = Regex.Match(strLocation, "/([^/]+)/about").Groups[1].Value.Trim();
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Error = ex;
            }
            SetStatusChanged("Login fail");
            return false;
        }

        public bool RegNewAccount(string strName, string strMail)
        {
            NameValueCollection param = new NameValueCollection();
            //WebClient.Proxy = new WebProxy("202.43.173.20", 8080);
            WebClient.DoGet("http://www.facebook.com/r.php");
            this.Error = WebClient.Error;
            if (!string.IsNullOrEmpty(WebClient.ResponseText))
            {
                System.Threading.Thread.Sleep(15000);
                string reg_instance = Regex.Match(WebClient.ResponseText, "name=\"reg_instance\"\\s+value=\"([^\"]+)").Groups[1].Value;
                string captcha_persist_data = Regex.Match(WebClient.ResponseText, "name=\"captcha_persist_data\"\\s+value=\"([^\"]+)").Groups[1].Value;
                string captcha_session = Regex.Match(WebClient.ResponseText, "name=\"captcha_session\"\\s+value=\"([^\"]+)").Groups[1].Value;
                string extra_challenge_params = Regex.Match(WebClient.ResponseText, "name=\"extra_challenge_params\"\\s+value=\"([^\"]+)").Groups[1].Value; ;
                string revision = Regex.Match(WebClient.ResponseText, "\"revision\":([\\d]+)").Groups[1].Value;
                string token = Regex.Match(WebClient.ResponseText, "\"token\":\"([^\"]+)\"").Groups[1].Value; ;
                string ph = Regex.Match(WebClient.ResponseText, "\"push_phase\":\"([^\"]+)\"").Groups[1].Value;
                string locale = Regex.Match(WebClient.ResponseText, "name=\"locale\"\\s+value=\"([^\"]+)").Groups[1].Value; ;
                param = new NameValueCollection();
                param.Add("__a", "1");
                param.Add("__dyn", "7wiXwNAwsUKEkxqnFw");
                param.Add("__req", "1");
                param.Add("__rev", revision);
                param.Add("__user", "0");
                param.Add("lsd", token);
                param.Add("miny_encode_ms", "2");
                param.Add("ph", ph);
                string time = Convert.ToInt64(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 7, 0, 0)).TotalMilliseconds).ToString();
                param.Add("ts", time);//	1396110078242
                WebClient.DoPost(param, "https://www.facebook.com/ajax/bz");
                System.Threading.Thread.Sleep(30000);

                string strURL = "https://pixel.facebook.com/ajax/register/logging.php?__a=1&__dyn=7wiXwNAwsUKEkxqnFw&__req=2&__rev=" + revision + "&__user=0&action=postload_focus&asyncSignal=9541&lsd=" + token + "&reg_instance=" + reg_instance;
                WebClient.GetImage(strURL);
                System.Threading.Thread.Sleep(60000);


                param = new NameValueCollection();
                param.Add("lsd", token);
                param.Add("firstname", strName.Split(' ')[0].Trim());
                param.Add("lastname", strName.Split(' ')[1].Trim());
                param.Add("reg_email__", strMail);
                param.Add("reg_email_confirmation__", strMail);
                param.Add("reg_passwd__", "random8&^");
                param.Add("birthday_day", "8");
                param.Add("birthday_month", "5");
                param.Add("birthday_year", "1989");
                param.Add("sex", "2");
                param.Add("asked_to_login", "0");
                param.Add("terms", "on");
                //param.Add("ab_test_data","AAAAyyyZ/MZMlMAAMAAAMAMAAMMAAAAMAAAAAAAA/3QaHCCCAEQCAf");
                param.Add("locale", "vi_VN");
                param.Add("reg_instance", reg_instance);
                param.Add("contactpoint_label", "email_only");
                param.Add("abtest_registration_group", "1");
                param.Add("captcha_persist_data", captcha_persist_data);
                param.Add("captcha_session", captcha_session);
                param.Add("extra_challenge_params", extra_challenge_params);
                param.Add("recaptcha_type", "password");
                param.Add("ignore", "captcha|pc");
                param.Add("__user", "0");
                param.Add("__a", "1");
                param.Add("__dyn", token);
                param.Add("__req", "4");
                param.Add("__rev", revision);

                WebClient.DoPost(param, "https://www.facebook.com/ajax/register.php");

                string error = Regex.Match(WebClient.ResponseText, "\"error\":\"([^\"]+)\"").Groups[1].Value; ;
                if (WebClient.ResponseText.Contains("firstname"))
                {
                    List<string> listL = new List<string>() { "Nguyen", "Phan", "Le", "Vo", "Bung" };
                    List<string> listF = new List<string>() { "Trieu", "Tien", "Khac", "Viet", "Song", "Tan", "Khoc", "Chien", "Lanh", "Bung" };
                    param["firstname"] = listF[new Random(1).Next(0, listF.Count - 1)];
                    System.Threading.Thread.Sleep(123);
                    param["lastname"] = listL[new Random(1).Next(0, listL.Count - 1)];
                    param["__req"] = "5";
                    System.Threading.Thread.Sleep(5000);
                    WebClient.DoPost(param, "https://www.facebook.com/ajax/register.php");

                    error = Regex.Match(WebClient.ResponseText, "\"error\":\"([^\"]+)\"").Groups[1].Value; ;
                    if (!string.IsNullOrEmpty(error))
                    {
                        System.Threading.Thread.Sleep(5000);
                        param["__req"] = "6";
                        WebClient.DoPost(param, "https://www.facebook.com/ajax/register.php");

                        error = Regex.Match(WebClient.ResponseText, "\"error\":\"([^\"]+)\"").Groups[1].Value; ;
                        if (!string.IsNullOrEmpty(error))
                        {
                            System.Threading.Thread.Sleep(5000);
                            param["__req"] = "7";
                            WebClient.DoPost(param, "https://www.facebook.com/ajax/register.php");

                            error = Regex.Match(WebClient.ResponseText, "\"error\":\"([^\"]+)\"").Groups[1].Value; ;
                            if (!string.IsNullOrEmpty(error))
                            {
                                System.Threading.Thread.Sleep(5000);
                                param["__req"] = "8";
                                WebClient.DoPost(param, "https://www.facebook.com/ajax/register.php");
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(error))
                {
                    System.Threading.Thread.Sleep(5000);
                    param["__req"] = "5";
                    WebClient.DoPost(param, "https://www.facebook.com/ajax/register.php");

                    error = Regex.Match(WebClient.ResponseText, "\"error\":\"([^\"]+)\"").Groups[1].Value; ;
                    if (!string.IsNullOrEmpty(error))
                    {
                        System.Threading.Thread.Sleep(5000);
                        param["__req"] = "6";
                        WebClient.DoPost(param, "https://www.facebook.com/ajax/register.php");

                        error = Regex.Match(WebClient.ResponseText, "\"error\":\"([^\"]+)\"").Groups[1].Value; ;
                        if (!string.IsNullOrEmpty(error))
                        {
                            System.Threading.Thread.Sleep(5000);
                            param["__req"] = "7";
                            WebClient.DoPost(param, "https://www.facebook.com/ajax/register.php");

                            error = Regex.Match(WebClient.ResponseText, "\"error\":\"([^\"]+)\"").Groups[1].Value; ;
                            if (!string.IsNullOrEmpty(error))
                            {
                                System.Threading.Thread.Sleep(5000);
                                param["__req"] = "8";
                                WebClient.DoPost(param, "https://www.facebook.com/ajax/register.php");
                            }
                        }
                    }
                }
                //param.Add("action", "postload_focus");
                //param.Add("reg_instance", "undefined");
                //param.Add("m_sess", "");
                //string fb_dtsg = Regex.Match(WebClient.ResponseText, "\"token\"(()|s+):(()|s+)\"(?<1>[^\"]+)").Groups[1].Value;
                //string lsd = Regex.Match(WebClient.ResponseText, "name=\"lsd\"\\s+value=\"([^\"]+)").Groups[1].Value;
                //string reg_instance = Regex.Match(WebClient.ResponseText, "name=\"reg_instance\"\\s+value=\"([^\"]+)").Groups[1].Value;
                //param.Add("fb_dtsg", fb_dtsg);
                //param.Add("lsd", lsd);
                //param.Add("__dyn", "1Z3p40x846AeVk2m4omyo");
                //param.Add("__req", "1");
                //param.Add("__ajax__", "true");
                //param.Add("__user", "0");
                ////System.Threading.Thread.Sleep(10000);
                //WebClient.DoPost(param, "http://m.facebook.com/a/registration/logger/");


                //param = new NameValueCollection();
                //param.Add("data", "[{'user':'0','page_id':'x3a8h1','posts':[['ods:ms.time_spent.qa.m',{'time_spent.bits.js_initialized':[1]},1394941209859,0]],'trigger':'ods:ms.time_spent.qa.m'}]".Replace("'", "\""));
                //param.Add("ts", "1394941209860");
                //param.Add("ph", "V3");
                //param.Add("m_sess", "");
                //param.Add("fb_dtsg", fb_dtsg);
                //param.Add("lsd", lsd);
                //param.Add("__dyn", "1Z3p40x846AeVk2m4omyo");
                //param.Add("__req", "2");
                //param.Add("__ajax__", "true");
                //param.Add("__user", "0");
                ////System.Threading.Thread.Sleep(10000);
                //WebClient.DoPost(param, "http://m.facebook.com/a/bz");
                //param = new NameValueCollection();
                //param.Add("lsd", lsd);
                //param.Add("charset_test", "€,´,€,´,水,Д,Є");
                //param.Add("reg_instance", reg_instance);
                //param.Add("cred_label", "email_or_phone");
                //param.Add("submission_request", "true");
                //param.Add("firstname", strName.Split(' ')[0].Trim());
                //param.Add("lastname", strName.Split(' ')[1].Trim());
                //param.Add("email", strMail);
                //param.Add("gender", "2");
                //param.Add("month", "05");
                //param.Add("day", "08");
                //param.Add("year", "1989");
                //param.Add("pass", "random8&^");
                //param.Add("submit", "Sign Up");
                ////System.Threading.Thread.Sleep(10000);
                //WebClient.DoPost(param, "http://m.facebook.com/r.php");

                //System.Threading.Thread.Sleep(60000);
                //Get confirm code
                //Pop3Client pop3 = new Pop3Client();
                //pop3.Connect("mail.tinphuong.me", 110, false);
                //pop3.Authenticate("email@tinphuong.me", "ngvantin");
                //int count = pop3.GetMessageCount();
                //Dictionary<string, string> dicMail = new Dictionary<string, string>();
                //for (int iIndex = 1; iIndex <= count; iIndex++)
                //{
                //    OpenPop.Mime.Message message = pop3.GetMessage(iIndex);
                //    var strBody = message.MessagePart.MessageParts[0].BodyEncoding.GetString(message.MessagePart.MessageParts[0].Body);
                //    string strLink = Regex.Match(strBody, @"http[^\s]+confirmemail.php[^\s]+").Value;
                //    if (!string.IsNullOrEmpty(strLink) && !dicMail.ContainsKey(message.Headers.To[0].MailAddress.Address))
                //    {
                //        dicMail.Add(message.Headers.To[0].MailAddress.Address, strLink);
                //    }
                //}
                ////pop3.DeleteAllMessages();
                //string strEmails = string.Empty;
                //foreach (var item in dicMail)
                //{
                //    System.Threading.Thread.Sleep(1000);
                //    strEmails += (string.IsNullOrEmpty(strEmails) ? "" : "\r\n") + item.Key;
                //    WebClient = new WebClientEx();
                //    WebClient.DoGet(item.Value);
                //    //if (WebClient.Response.ResponseUri.AbsolutePath.Contains("login")) continue;
                //    //strResponse = client.DoGet("https://www.facebook.com/" + item.Value);
                //    string strFileName = "newacc.txt";
                //    string strAcc = item.Key;
                //    if (File.Exists(strFileName))
                //    {
                //        StreamReader rd = new StreamReader(strFileName);
                //        strAcc = rd.ReadToEnd();
                //        strAcc += "\r\n" + item.Key;
                //        rd.Close();
                //        rd.Dispose();
                //    }

                //    StreamWriter wr = new StreamWriter(strFileName);
                //    wr.Write(strAcc);
                //    wr.Close();
                //    wr.Dispose();
                //}
            }
            return false;
        }

        public void FeedEmail()
        {
            try
            {
                IsWorking = true;
                WorkingFail = false;
                Pop3Client pop3 = new Pop3Client();
                pop3.Connect("mail.tinphuong.me", 110, false);
                pop3.Authenticate("email@tinphuong.me", "ngvantin");
                int count = pop3.GetMessageCount();
                Dictionary<string, string> dicMail = new Dictionary<string, string>();
                for (int iIndex = 1; iIndex <= count; iIndex++)
                {
                    OpenPop.Mime.Message message = pop3.GetMessage(iIndex);
                    var strBody = message.MessagePart.MessageParts[0].BodyEncoding.GetString(message.MessagePart.MessageParts[0].Body);
                    string strLink = Regex.Match(strBody, @"http[^\s]+confirmemail.php[^\s]+").Value;
                    if (!string.IsNullOrEmpty(strLink) && !dicMail.ContainsKey(message.Headers.To[0].MailAddress.Address))
                    {
                        dicMail.Add(message.Headers.To[0].MailAddress.Address, strLink);
                    }
                }
                //pop3.DeleteAllMessages();
                string strEmails = string.Empty;
                foreach (var item in dicMail)
                {
                    System.Threading.Thread.Sleep(10000);
                    strEmails += (string.IsNullOrEmpty(strEmails) ? "" : "\r\n") + item.Key;
                    WebClient = new WebClientEx();
                    this.Models = new FaceBook();
                    Models.Login = item.Key;
                    Models.Pass = "random8&^";
                    CheckLogin();
                    WebClient.DoGet(item.Value);
                    if (WebClient.Response.ResponseUri.AbsolutePath.Contains("login")) continue;
                    //strResponse = client.DoGet("https://www.facebook.com/" + item.Value);
                    string strFileName = "newacc.txt";
                    string strAcc = item.Key;
                    if (File.Exists(strFileName))
                    {
                        StreamReader rd = new StreamReader(strFileName);
                        strAcc = rd.ReadToEnd();
                        strAcc += "\r\n" + item.Key;
                        rd.Close();
                        rd.Dispose();
                    }

                    StreamWriter wr = new StreamWriter(strFileName);
                    wr.Write(strAcc);
                    wr.Close();
                    wr.Dispose();
                }
            }
            catch (Exception ex)
            {
                this.Error = ex;
                WorkingFail = true;
            }
            finally
            {
                IsWorking = false;
            }
        }

        public bool ChangePass(string strNewPass)
        {
            try
            {
                SetStatusChanged("Begin change pass");
                SetStatusChanged("Login successful");
                return true;
            }
            catch (Exception ex)
            {
                this.Error = ex;
            }
            return false;
        }

        public void SetStatusChanged(string strStatus)
        {
            if (StatusChanged != null)
            {
                StatusChanged(this, new StatusChangedEventArgs(strStatus));
            }
        }

        public void PostStatus()
        {
            try
            {
                SetStatusChanged("Begin post status");
                IsWorking = true;
                WorkingFail = false;
                string strPostContent = "This is a post from me";
                //get status
                WebClient.DoGet("http://www.postrandomonium.com/Random");
                if (WebClient.ResponseText != null)
                {
                    strPostContent = Regex.Match(WebClient.ResponseText, "class=\"small-message\">[\\s\n\r]+<span>(?<1>.+)</span>").Groups[1].Value;
                }
                //Check facebook before post
                WebClient.CookieContainer = (Serializer.ConvertBlobToObject(Models.Cookies) as CookieContainer);
                WebClient.DoGet("https://www.facebook.com/me/about");
                this.Error = WebClient.Error;
                string strLocation = WebClient.ResponseHeaders[HttpResponseHeader.Location];
                if (WebClient.Response.ResponseUri.AbsolutePath.Contains("login.php"))
                {//Problem with account
                    SetStatusChanged("Having problems with this account");
                    this.Models.Status = 2;
                    DataProvider.DB.Entry(this.Models).State = System.Data.Entity.EntityState.Modified;
                    DataProvider.DB.SaveChanges();
                    WorkingFail = true;
                }
                else
                {
                    string fb_dtsg = Regex.Match(WebClient.ResponseText, "\"token\"(()|s+):(()|s+)\"(?<1>[^\"]+)").Groups[1].Value;
                    NameValueCollection param = new NameValueCollection();
                    param.Add("fb_dtsg", fb_dtsg);
                    param.Add("xhpc_targetid", Models.UserID);
                    param.Add("xhpc_message_text", strPostContent);
                    param.Add("xhpc_message", strPostContent);
                    param.Add("__user", Models.UserID);
                    param.Add("__a", "1");
                    param.Add("__req", "a");
                    WebClient.DoPost(param, "https://www.facebook.com/ajax/updatestatus.php");

                    var poststatus = new Models.PostStatus();
                    poststatus.FaceBookID = Models.ID;
                    poststatus.StatusContent = strPostContent;
                    poststatus.SessionID = this.SessionID;
                    DataProvider.DB.PostStatus.Add(poststatus);
                    SetStatusChanged("Post status Done !");
                    this.Models.Cookies = Serializer.ConvertObjectToBlob(this.WebClient.CookieContainer);
                    DataProvider.DB.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this.Error = ex;
                WorkingFail = true;
                SetStatusChanged("Something wrong !");
            }
            finally
            {
                IsWorking = false;
            }
        }
        #endregion
    }
    class StatusChangedEventArgs : EventArgs
    {
        public string Status { get; set; }
        public StatusChangedEventArgs(string status)
        {
            this.Status = status;
        }
    }
}
