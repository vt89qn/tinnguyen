using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Instagram.App_Data;
using System.Collections.Specialized;
using System.Net;

namespace Instagram
{
    class IGController
    {
        public IGDBDataContext IGDB { get; set; }
        public Exception Error { get; set; }
        public WebClientEx Client { get; set; }
        public IG_Account Model { get; set; }

        public IGController()
        {
            Client = new WebClientEx();
            if (Model != null)
            {
                if (!string.IsNullOrEmpty(Model.PhoneInfo))
                {
                    Client.UserAgent = "Instagram 6.5.0 Android (" + Model.PhoneInfo + ")";
                }
            }
        }

        public bool SignUp()
        {
            try
            {
                IG_Account account = new IG_Account();
                List<string> version = new List<string> { "18/4.1.1", "17/4.0.1", "17/4.2.2", "17/4.3", "16/4.0", "15/2.3", "16/3.0", "17/4.1", "17/4.2", "18/4.3.2", "14/2.3.1" };
                List<string> manufacter = new List<string> { "samsung", "acer", "asus", "alcatel", "htc", "hp", "lenovo", "lg", "zte", "sky" };
                List<string> display = new List<string> { "480x800", "800x1200", "720x1080", "640x1136", "3200x1800" };
                List<string> listDPI = new List<string> { "480", "240", "320" };
                List<string> listName = new List<string> { "SHG", "SM", "SPH", "SCH", "LG", "GT" };

                account.PhoneInfo = string.Format("{0}; {1}dpi; {2}; {3}; {4}; {4}; aries; en_US", version[new Random().Next(version.Count)]
                    , listDPI[new Random().Next(listDPI.Count)], display[new Random().Next(display.Count)]
                    , manufacter[new Random().Next(manufacter.Count)]
                    , listName[new Random().Next(listName.Count)] + "-" + ((char)new Random().Next(65, 90)).ToString().ToUpper() + new Random().Next(10, 900));
                Client.UserAgent = "Instagram 6.5.0 Android (" + account.PhoneInfo + ")";
                //Client.UserAgent = "Instagram 6.0.8 (iPhone4,1; iPhone OS 7_1_2; vi_VN; vi) AppleWebKit/420+";
                WebClientEx client = new WebClientEx();
                client.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36";
                client.DoGet("http://www.fakenamegenerator.com/gen-random-en-uk.php");
                if (!string.IsNullOrEmpty(client.ResponseText))
                {
                    string strValue = Regex.Match(client.ResponseText, "<div class=\"address\">[\\r\\n\\s]+<h3>(?<1>.+)</h3>").Groups[1].Value;
                    if (string.IsNullOrEmpty(strValue))
                    {
                        throw new Exception("Random RealName empty !");
                    }
                    account.RealName = strValue;
                    strValue = Regex.Match(client.ResponseText, "<li class=\"tel\"><span class=\"value\">(?<1>.+)</span>").Groups[1].Value;
                    if (string.IsNullOrEmpty(strValue))
                    {
                        throw new Exception("Random PhoneNumber empty !");
                    }
                    account.PhoneNumber = strValue;
                    account.UserName = account.RealName.ToLower().Replace(" ", "__").Trim();
                    if (!checkUserName(account.UserName))
                    {
                        account.UserName = account.RealName.ToLower().Replace(" ", "_").Trim();
                        if (!checkUserName(account.UserName))
                        {
                            account.UserName = account.RealName.ToLower().Replace(" ", "").Trim() + new Random().Next(10, 99).ToString();
                        }
                    }
                    account.Pass = Utilities.GetMd5Hash(account.UserName).Substring(0, 12);
                    account.Email = account.UserName + "@yahoo.com";


                    string csrftoken = string.Empty;
                    if (Client.CookieContainer != null)
                    {
                        foreach (Cookie cookie in Client.CookieContainer.GetCookies(new Uri("http://i.instagram.com/")))
                        {
                            if (cookie.Name == "csrftoken")
                            {
                                csrftoken = cookie.Value;
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(csrftoken))
                    {
                        string _uuid = Utilities.GetMd5Hash(account.UserName).ToUpper();
                        _uuid = _uuid.Substring(0, 8) + "-" + _uuid.Substring(8, 4)
                           + "-" + _uuid.Substring(12, 4)
                           + "-" + _uuid.Substring(16, 4)
                           + "-" + _uuid.Substring(20, 12);
                        string strBody = "{\"_uuid\":\"" + _uuid + "\",\"password\":\"" + account.Pass + "\",\"username\":\"" + account.UserName + "\",\"device_id\":\"" + _uuid + "\",\"email\":\"" + account.Email + "\",\"_csrftoken\":\"" + csrftoken + "\"}";
                        string strSignedBody = getSignedString(strBody);
                        if (!string.IsNullOrEmpty(strSignedBody))
                        {

                            NameValueCollection param = new NameValueCollection();
                            param.Add("signed_body", strSignedBody + "." + strBody);
                            param.Add("ig_sig_key_version", "4");
                            Client.DoPost(param, "https://i.instagram.com/api/v1/accounts/create/");
                            if (!string.IsNullOrEmpty(Client.ResponseText) && Client.ResponseText.Contains("\"status\":\"ok\""))
                            {
                                string pk = Regex.Match(Client.ResponseText, "\"pk\":(?<val>[0-9]+)").Groups["val"].Value.Trim();
                                strBody = "{\"_uuid\":\"" + _uuid + "\",\"_csrftoken\":\"" + csrftoken + "\",\"_uid\":\"" + pk + "\",\"first_name\":\"" + account.RealName + "\",\"phone_number\":\"" + account.PhoneNumber.Replace(" ", "").Trim() + "\"}";
                                strSignedBody = getSignedString(strBody);

                                param = new NameValueCollection();
                                param.Add("signed_body", strSignedBody + "." + strBody);
                                param.Add("ig_sig_key_version", "4");
                                Client.DoPost(param, "https://i.instagram.com/api/v1/accounts/set_phone_and_name/");


                                account.Cookie = new System.Data.Linq.Binary(Utilities.ConvertObjectToBlob(Client.CookieContainer));
                                IGDB = new IGDBDataContext();
                                IGDB.IG_Accounts.InsertOnSubmit(account);
                                IGDB.SubmitChanges();
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error = ex;
            }
            return false;
        }

        private bool checkUserName(string userName)
        {
            Client.DoGet("http://i.instagram.com/api/v1/si/fetch_headers/?guid=" + Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString()) + "&challenge_type=signup");
            string csrftoken = string.Empty;
            if (Client.CookieContainer != null)
            {
                foreach (Cookie cookie in Client.CookieContainer.GetCookies(new Uri("http://i.instagram.com/")))
                {
                    if (cookie.Name == "csrftoken")
                    {
                        csrftoken = cookie.Value;
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(csrftoken))
            {
                string strBody = "{\"_csrftoken\":\"" + csrftoken + "\",\"username\":\"" + userName + "\"}";
                string strSignedBody = getSignedString(strBody);
                if (!string.IsNullOrEmpty(strSignedBody))
                {
                    NameValueCollection param = new NameValueCollection();
                    param.Add("signed_body", strSignedBody + "." + strBody);
                    param.Add("ig_sig_key_version", "4");
                    Client.DoPost(param, "http://i.instagram.com/api/v1/users/check_username/");
                    if (!string.IsNullOrEmpty(Client.ResponseText) && Client.ResponseText.Contains("\"available\":true"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private string getSignedString(string body)
        {
            string strReturn = string.Empty;
            IGDB = new IGDBDataContext();
            IGDB.IG_SignStrings.InsertOnSubmit(new IG_SignString { String = body });
            IGDB.SubmitChanges();
            while (true)
            {
                IGDB = new IGDBDataContext();
                IG_SignString signstring = (from _signstring in IGDB.IG_SignStrings where _signstring.String == body select _signstring).FirstOrDefault();
                if (signstring != null && !string.IsNullOrEmpty(signstring.SignedString))
                {
                    strReturn = signstring.SignedString;
                    IGDB.IG_SignStrings.DeleteOnSubmit(signstring);
                    IGDB.SubmitChanges();
                    break;
                }
                System.Threading.Thread.Sleep(1000);
            }
            return strReturn;
        }
    }
}
