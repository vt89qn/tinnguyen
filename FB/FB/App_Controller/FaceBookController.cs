using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FB.App_UserControl;
using FB.App_Common;
using System.Collections.Specialized;
using System.Data.SQLite;
using FB.App_Model;
using System.Web.Script.Serialization;
using OpenPop.Pop3;
using System.Text.RegularExpressions;
using System.Web;
using System.Drawing;
using System.IO;
using System.Net;

namespace FB.App_Controller
{
    public class FaceBookController
    {
        public bool LoginMobile(FaceBook model, WebClientEx client)
        {
            try
            {
                if (client == null)
                {
                    client = new WebClientEx();
                    client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                    client.DoGet("https://developers.facebook.com/resources/dialog_descriptions_android.json");
                }
                NameValueCollection param = new NameValueCollection();
                param.Add("api_key", AppSettings.api_key);
                param.Add("client_country_code", "VN");
                param.Add("credentials_type", "password");
                param.Add("email", model.Login);
                param.Add("error_detail_type", "button_with_disabled");
                param.Add("fb_api_caller_class", "com.facebook.auth.protocol.AuthenticateMethod");
                param.Add("fb_api_req_friendly_name", "authenticate");
                param.Add("format", "json");
                param.Add("generate_machine_id", "1");
                param.Add("generate_session_cookies", "1");
                param.Add("locale", "en_US");
                param.Add("method", "auth.login");
                param.Add("password", model.Pass);
                string sig = Utilities.getSignFB(param, "62f8ce9f74b12f84c123cc23437a4a32");
                param.Add("sig", sig);

                client.DoPost(param, "https://b-api.facebook.com/method/auth.login");
                if (!string.IsNullOrEmpty(client.ResponseText)
                    && client.ResponseText.Contains("session_key")
                    && client.ResponseText.Contains("access_token"))
                {
                    model.MBLoginText = client.ResponseText;
                    model.MBCookie = Utilities.ConvertObjectToBlob(client.CookieContainer);

                    Dictionary<string, object> rs = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                    if (rs.ContainsKey("uid"))
                    {
                        model.FBID = rs["uid"].ToString();
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

        public bool LoginMobile(FaceBook model)
        {
            return this.LoginMobile(model, null);
        }

        /// <summary>
        /// Send request from Model 1 to Model 2
        /// </summary>
        /// <param name="model1"></param>
        /// <param name="model2"></param>
        /// <returns></returns>
        public bool SendFriendRequest(FaceBook model1, FaceBook model2)
        {
            try
            {
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model1.MBLoginText);
                client.Authorization = dicData["access_token"].ToString();

                NameValueCollection param = new NameValueCollection();
                param.Add("uid", model2.FBID);
                param.Add("hf", "profile_button");
                param.Add("ref", "pb_likes");
                param.Add("format", "json");
                param.Add("locale", "en_US");
                param.Add("client_country_code", "VN");
                param.Add("fb_api_req_friendly_name", "sendFriendRequest");
                param.Add("fb_api_caller_class", "com.facebook.friends.protocol.SendFriendRequestMethod");

                client.DoPost(param, "https://graph.facebook.com/me/friends");
                if (!string.IsNullOrEmpty(client.ResponseText) && (client.ResponseText == "true"
                    || client.ResponseText.Contains("There is already a pending friend request to this user")))
                {
                    return true;
                }
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
        public bool AcceptFriendRequest(FaceBook model1, FaceBook model2)
        {
            try
            {
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model1.MBLoginText);
                client.Authorization = dicData["access_token"].ToString();

                NameValueCollection param = new NameValueCollection();
                param.Add("uid", model2.FBID);
                param.Add("confirm", "1");
                param.Add("ref", "m_jewel");
                param.Add("format", "json");
                param.Add("locale", "en_US");
                param.Add("client_country_code", "VN");
                param.Add("fb_api_req_friendly_name", "respondToFriendRequest");
                param.Add("method", "facebook.friends.confirm");
                param.Add("fb_api_caller_class", "com.facebook.friends.protocol.RespondToFriendRequestMethod");

                client.DoPost(param, "https://api.facebook.com/method/facebook.friends.confirm");
                if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText == "true")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public string GetFaceBookLoginURL(FaceBook model, string strDestURl)
        {
            string strURl = string.Empty;
            try
            {
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                string secret = dicData["secret"].ToString();
                string session_key = dicData["session_key"].ToString();

                NameValueCollection param = new NameValueCollection();
                param.Add("api_key", AppSettings.api_key);
                param.Add("session_key", session_key);
                param.Add("t", Utilities.GetCurrentSecond());
                param.Add("uid", model.FBID);
                param.Add("url", strDestURl);

                string sig = Utilities.getSignFB(param, secret);
                //param.Add("sig", sig);

                strURl = "https://m.facebook.com/auth.php?api_key="
                    + AppSettings.api_key
                    + "&session_key=" + session_key
                    + "&sig=" + sig
                    + "&t=" + param["t"]
                    + "&uid=" + model.FBID
                    + "&url=" + param["url"];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strURl;
        }

        public FaceBook RegNewAccount()
        {
            FaceBook model = null;


            WebClientEx client = new WebClientEx();
            client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;

            NameValueCollection param = new NameValueCollection();
            //param.Add("api_key", AppSettings.api_key);
            //param.Add("client_country_code", "VN");
            //param.Add("configs", "{\"07d391956322d7afe00b6505f6a330f1596da06e\":\"\",\"3e73be94500a5d288fdc97ae65e7d7a8b7ca7f47\":\"\",\"9094751dc4e6c2523467057e66cb434d711180b6\":\"\"}");
            //param.Add("debug", "true");
            //param.Add("fb_api_caller_class", "com.facebook.xconfig.sync.XSyncApiMethod");
            //param.Add("fb_api_req_friendly_name", "syncXConfigs");
            //param.Add("format", "json");
            string hash_id = Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString());
            hash_id = hash_id.Substring(0, 8) + "-" + hash_id.Substring(8, 4) + "-" + hash_id.Substring(12, 4) + "-" + hash_id.Substring(16, 4) + "-" + hash_id.Substring(20, 12);
            ////param.Add("hash_id", "104f3416-8659-49fd-b722-4b3c6d2af0b5");
            ////b7b90f99-d3d6-6dac-aa77-a683880ae38e
            //param.Add("hash_id", hash_id);
            //param.Add("locale", "en_US");
            //param.Add("method", "xconfig.fetch");
            string sig = Utilities.getSignFB(param, "62f8ce9f74b12f84c123cc23437a4a32");
            //param.Add("sig", sig);

            //client.DoPost(param, "https://api.facebook.com/method/xconfig.fetch");
            //if (!string.IsNullOrEmpty(client.ResponseText))
            {

                //param = new NameValueCollection();
                //param.Add("api_key", AppSettings.api_key);
                //param.Add("client_country_code", "VN");
                //param.Add("fb_api_caller_class", "com.facebook.gk.FetchMobileGatekeepersMethod");
                //param.Add("fb_api_req_friendly_name", "fetchSessionlessGKInfo");
                //param.Add("format", "json");
                //param.Add("hash_id", hash_id);
                //param.Add("locale", "en_US");
                //param.Add("method", "mobile.gatekeepers");
                //param.Add("query", "android_bootstrap_tier_kill_switch,android_mobile_subno_fetch,android_xconfig_fetch");
                //param.Add("query_hash", "42C741F21A83DB81946F95509D99BFA038A79BC1");
                //sig = Utilities.getSignFB(param, "62f8ce9f74b12f84c123cc23437a4a32");
                //param.Add("sig", sig);
                //client.SetCookieV2 = true;
                //client.DoPost(param, "https://api.facebook.com/method/mobile.gatekeepers");
                //client.SetCookieV2 = false;
                //client.DoGet("https://developers.facebook.com/resources/dialog_descriptions_android.json");

                string strName = Utilities.ConvertToUsignNew(Utilities.GetMaleName());
                param = new NameValueCollection();
                param.Add("api_key", AppSettings.api_key);
                param.Add("attempt_login", "true");
                param.Add("birthday", new Random().Next(1979, 1990).ToString() + "-" + new Random().Next(1, 12) + "-" + new Random(3).Next(1, 28));
                param.Add("client_country_code", "VN");

                param.Add("fb_api_caller_class", "com.facebook.registration.protocol.RegisterAccountMethod");
                param.Add("fb_api_req_friendly_name", "registerAccount");
                param.Add("firstname", strName.Trim().Split(' ')[0]);
                System.Threading.Thread.Sleep(34);
                param.Add("format", "json");
                param.Add("gender", "M");
                param.Add("lastname", strName.Substring(strName.IndexOf(' ')).Trim());
                List<string> listEmail = new List<string>();
                listEmail.Add("gmail.com");
                listEmail.Add("hotmail.com");
                listEmail.Add("yahoo.com");
                listEmail.Add("live.com");
                listEmail.Add("gmail.com");
                listEmail.Add("rocket.com");
                listEmail.Add("yahoo.com");
                listEmail.Add("outlook.com");
                listEmail.Add("live.com");

                param.Add("email", Utilities.ConvertToUsignNew(param["firstname"].Replace(" ", "").Trim() + param["lastname"].Replace(" ", "").Trim() + new Random().Next(10, 99)).ToLower() + "@" + listEmail[new Random().Next(0, listEmail.Count - 1)]);
                param.Add("locale", "en_US");
                param.Add("method", "user.register");
                param.Add("password", Utilities.GetMd5Hash(param["email"]).Substring(0, new Random().Next(13, 20)));
                param.Add("reg_instance", hash_id);
                param.Add("return_multiple_errors", "true");
                sig = Utilities.getSignFB(param, "62f8ce9f74b12f84c123cc23437a4a32");
                param.Add("sig", sig);
                client.SetCookieV2 = true;
                //System.Threading.Thread.Sleep(5000);
                client.DoPost(param, "https://b-api.facebook.com/method/user.register");

                if (!string.IsNullOrEmpty(client.ResponseText) && client.Error == null)
                {
                    Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                    if (dicData.ContainsKey("session_info")
                        && dicData.ContainsKey("account_type"))
                    {
                        model = new FaceBook();
                        model.Login = param["email"];
                        model.Pass = param["password"];
                        model.MBLoginText = new JavaScriptSerializer().Serialize(dicData["session_info"]);
                        model.FBID = (dicData["session_info"] as Dictionary<string, object>)["uid"].ToString();




                        //client.Authorization = (dicData["session_info"] as Dictionary<string, object>)["access_token"].ToString();
                        //param = new NameValueCollection();

                        //param.Add("normalized_contactpoint", model.Login);
                        //param.Add("contactpoint_type", "EMAIL");
                        //param.Add("code", "17679");
                        //param.Add("source", "ANDROID_DIALOG_API");
                        //param.Add("format", "json");
                        //param.Add("locale", "en_US");
                        //param.Add("client_country_code", "VN");
                        //param.Add("method", "user.confirmcontactpoint");
                        //param.Add("fb_api_req_friendly_name", "confirmContactpoint");
                        //param.Add("fb_api_caller_class", "com.facebook.confirmation.protocol.ConfirmContactpointMethod");
                        //System.Threading.Thread.Sleep(3000);
                        //client.DoPost(param, "https://api.facebook.com/method/user.confirmcontactpoint");

                        //if (client.ResponseText != "true") return null;





                        if (LoginMobile(model, client))
                        {
                            client.SetCookieV2 = false;
                            dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                            client.Authorization = dicData["access_token"].ToString();
                            param = new NameValueCollection();
                            param.Add("format", "JSON");
                            param.Add("nux_id", "ANDROID_NEW_ACCOUNT_WIZARD");
                            param.Add("step", "upload_profile_pic");
                            param.Add("status", "COMPLETE");
                            param.Add("extra_data", "{}");
                            param.Add("locale", "en_US");
                            param.Add("client_country_code", "VN");
                            param.Add("method", "user.updateNuxStatus");
                            param.Add("fb_api_req_friendly_name", "updateNuxStatus");
                            param.Add("fb_api_caller_class", "com.facebook.nux.status.UpdateNuxStatusMethod");
                            //System.Threading.Thread.Sleep(1000);
                            client.DoPost(param, "https://api.facebook.com/method/user.updateNuxStatus");

                            param = new NameValueCollection();
                            param.Add("format", "JSON");
                            param.Add("nux_id", "ANDROID_NEW_ACCOUNT_WIZARD");
                            param.Add("step", "classmates_coworkers");
                            param.Add("status", "COMPLETE");
                            param.Add("extra_data", "{}");
                            param.Add("locale", "en_US");
                            param.Add("client_country_code", "VN");
                            param.Add("method", "user.updateNuxStatus");
                            param.Add("fb_api_req_friendly_name", "updateNuxStatus");
                            param.Add("fb_api_caller_class", "com.facebook.nux.status.UpdateNuxStatusMethod");
                            //System.Threading.Thread.Sleep(1000);
                            client.DoPost(param, "https://api.facebook.com/method/user.updateNuxStatus");

                            param = new NameValueCollection();
                            param.Add("format", "JSON");
                            param.Add("nux_id", "ANDROID_NEW_ACCOUNT_WIZARD");
                            param.Add("step", "contact_importer");
                            param.Add("status", "COMPLETE");
                            param.Add("extra_data", "{}");
                            param.Add("locale", "en_US");
                            param.Add("client_country_code", "VN");
                            param.Add("method", "user.updateNuxStatus");
                            param.Add("fb_api_req_friendly_name", "updateNuxStatus");
                            param.Add("fb_api_caller_class", "com.facebook.nux.status.UpdateNuxStatusMethod");
                            //System.Threading.Thread.Sleep(1000);
                            client.DoPost(param, "https://api.facebook.com/method/user.updateNuxStatus");


                            //wait email for 10s
                            //System.Threading.Thread.Sleep(5000);
                            //#region - Feed Email -
                            ////Pop3.Pop3MailClient gmailClient = new Pop3.Pop3MailClient("pop.gmail.com", 995, true, "vantin.work@gmail.com", "leostbuqbcepmvae");
                            ////gmailClient.IsAutoReconnect = true;
                            ////gmailClient.ReadTimeout = 60000; //give pop server 60 seconds to answer

                            //////establish connection
                            ////gmailClient.Connect();

                            //////get mailbox statistics
                            ////int NumberOfMails, MailboxSize;
                            ////gmailClient.GetMailboxStats(out NumberOfMails, out MailboxSize);

                            ////for (int iIndex = NumberOfMails; iIndex > NumberOfMails - 3; iIndex--)
                            ////{
                            ////    //get email
                            ////    string Email;
                            ////    bool bFindEmail = false;
                            ////    gmailClient.GetRawEmail(iIndex, out Email);
                            ////    Email = Email.Replace("=\r\n", "").Replace("=3D", "=");
                            ////    if (Email.Contains(model.Login))
                            ////    {
                            ////        string url = System.Text.RegularExpressions.Regex.Match(Email, @"https://www\.facebook\.com/n/\?confirmemail\.php[^\s]+").Value;
                            ////        if (!string.IsNullOrEmpty(url))
                            ////        {
                            ////            WebClientEx clientFB = new WebClientEx();
                            ////            clientFB.DoGet(url);
                            ////            if (!string.IsNullOrEmpty(clientFB.ResponseText))
                            ////            {
                            ////                url = System.Text.RegularExpressions.Regex.Match(clientFB.ResponseText, "form action=\"(?<val>/login/confirm/\\?auth_token=[^\"]+)").Groups["val"].Value.Trim();
                            ////                url = "https://www.facebook.com" + url;
                            ////                url = url.Replace("&amp;", "&");
                            ////                string lsd = System.Text.RegularExpressions.Regex.Match(clientFB.ResponseText, "name=\"lsd\" value=\"(?<val>[^\"]+)").Groups["val"].Value.Trim();
                            ////                param = new NameValueCollection();
                            ////                param.Add("lsd", lsd);
                            ////                param.Add("confirm", "1");
                            ////                clientFB.DoPost(param, url);
                            ////                if (!string.IsNullOrEmpty(clientFB.ResponseText))
                            ////                {
                            ////                    url = System.Text.RegularExpressions.Regex.Match(clientFB.ResponseText, "\\?confirmemail\\.php\\&e=[^\"]+").Value;
                            ////                    if (!string.IsNullOrEmpty(url))
                            ////                    {
                            ////                        url = "https://www.facebook.com/n/" + url;
                            ////                        clientFB.DoGet(url);

                            ////                    }
                            ////                }
                            ////            }
                            ////            bFindEmail = true;
                            ////        }
                            ////    }
                            ////    //if (Email.Contains("facebookmail.com"))
                            ////    //{
                            ////    //    //delete email
                            ////    //    gmailClient.DeleteEmail(iIndex);
                            ////    //}
                            ////    if (bFindEmail) break;
                            ////}

                            //////close connection
                            ////gmailClient.Disconnect();
                            //string code = string.Empty;
                            //Pop3Client pop3 = new Pop3Client();
                            //pop3.Connect("pop.gmail.com", 995, true);
                            //pop3.Authenticate("vantin.work@gmail.com", "leostbuqbcepmvae");
                            //int count = pop3.GetMessageCount();
                            //Dictionary<string, string> dicMail = new Dictionary<string, string>();
                            //for (int iIndex = count; iIndex >= count - 3; iIndex--)
                            //{
                            //    bool bFindEmail = false;
                            //    OpenPop.Mime.Message message = pop3.GetMessage(iIndex);
                            //    if (message.Headers.To[0].MailAddress.Address == model.Login)
                            //    {
                            //        var strBody = message.MessagePart.MessageParts[0].BodyEncoding.GetString(message.MessagePart.MessageParts[0].Body);
                            //        string url = Regex.Match(strBody, @"http[^\s]+confirmemail.php[^\s]+").Value;
                            //        if (!string.IsNullOrEmpty(url))
                            //        {
                            //            code = Regex.Match(url, "&c=(?<val>[0-9]+)").Groups["val"].Value.Trim();
                            //            if (!string.IsNullOrEmpty(code)) break;
                            //            WebClientEx clientFB = new WebClientEx();
                            //            model.FBID = (dicData["session_info"] as Dictionary<string, object>)["uid"].ToString();
                            //            string t = GetFaceBookLoginURL(model, url);
                            //            clientFB.DoGet(url);
                            //            if (!string.IsNullOrEmpty(clientFB.ResponseText))
                            //            {
                            //                url = System.Text.RegularExpressions.Regex.Match(clientFB.ResponseText, "form action=\"(?<val>/login/confirm/\\?auth_token=[^\"]+)").Groups["val"].Value.Trim();
                            //                url = "https://www.facebook.com" + url;
                            //                url = url.Replace("&amp;", "&");
                            //                string lsd = System.Text.RegularExpressions.Regex.Match(clientFB.ResponseText, "name=\"lsd\" value=\"(?<val>[^\"]+)").Groups["val"].Value.Trim();
                            //                param = new NameValueCollection();
                            //                param.Add("lsd", lsd);
                            //                param.Add("confirm", "1");
                            //                clientFB.DoPost(param, url);
                            //                if (!string.IsNullOrEmpty(clientFB.ResponseText))
                            //                {
                            //                    if (clientFB.Response.ResponseUri.AbsolutePath.Contains("checkpoint")) return null;
                            //                    url = System.Text.RegularExpressions.Regex.Match(clientFB.ResponseText, "\\?confirmemail\\.php\\&e=[^\"]+").Value;
                            //                    if (!string.IsNullOrEmpty(url))
                            //                    {
                            //                        url = "https://www.facebook.com/n/" + url;
                            //                        clientFB.DoGet(url);

                            //                    }
                            //                }
                            //            }
                            //            bFindEmail = true;
                            //        }
                            //        try
                            //        {
                            //            pop3.DeleteMessage(iIndex);
                            //        }
                            //        catch { }
                            //    }
                            //    if (bFindEmail) break;
                            //}
                            ////pop3.DeleteAllMessages();
                            //pop3.Disconnect();
                            //#endregion

                            //if (!string.IsNullOrEmpty(code))
                            //{
                            //    param = new NameValueCollection();

                            //    param.Add("normalized_contactpoint", model.Login);
                            //    param.Add("contactpoint_type", "EMAIL");
                            //    param.Add("code", code);
                            //    param.Add("source", "ANDROID_DIALOG_API");
                            //    param.Add("format", "json");
                            //    param.Add("locale", "en_US");
                            //    param.Add("client_country_code", "VN");
                            //    param.Add("method", "user.confirmcontactpoint");
                            //    param.Add("fb_api_req_friendly_name", "confirmContactpoint");
                            //    param.Add("fb_api_caller_class", "com.facebook.confirmation.protocol.ConfirmContactpointMethod");
                            //    //System.Threading.Thread.Sleep(3000);
                            //    client.DoPost(param, "https://api.facebook.com/method/user.confirmcontactpoint");
                            //}
                            return model;
                        }
                    }


                }
            }
            return null;
        }

        public bool PostStatus(FaceBook model)
        {
            try
            {
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                client.Authorization = dicData["access_token"].ToString();

                WebClientEx clientGetStatus = new WebClientEx();
                clientGetStatus.DoGet("http://www.postrandomonium.com/Random");
                string strStatus = "i post this !";
                if (clientGetStatus.ResponseText != null)
                {
                    string strStatus2 = Regex.Match(clientGetStatus.ResponseText, "class=\"small-message\">[\\s\n\r]+<span>(?<1>.+)</span>").Groups[1].Value;
                    if (!string.IsNullOrEmpty(strStatus2))
                    {
                        strStatus = strStatus2;
                    }
                }

                NameValueCollection param = new NameValueCollection();
                strStatus = HttpUtility.UrlEncode(strStatus);
                string hash_id = Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString());
                hash_id = hash_id.Substring(0, 8) + "-" + hash_id.Substring(8, 4) + "-" + hash_id.Substring(12, 4) + "-" + hash_id.Substring(16, 4) + "-" + hash_id.Substring(20, 12);
                param.Add("batch", "[{\"method\":\"POST\",\"body\":\"privacy=%7B%22value%22%3A%22EVERYONE%22%7D&message="
                    + strStatus + "&nectar_module=newsfeed_composer&composer_session_id=" + hash_id
                    + "&qn=" + hash_id + "&idempotence_token=" + hash_id
                    + "&audience_exp=true&time_since_original_post=1&attach_place_suggestion=true&format=json&locale=en_US&client_country_code=VN&fb_api_req_friendly_name=graphObjectPosts\",\"name\":\"graphObjectPosts\",\"omit_response_on_success\":false,\"relative_url\":\"" +
                    model.FBID + "/feed\"}]");
                param.Add("fb_api_caller_class", "com.facebook.composer.publish.ComposerPublishServiceHandler");

                client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=en_US&client_country_code=VN");
                if (client.Error == null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public bool ConfirmEmail(FaceBook model)
        {
            try
            {
                if (model.Login.EndsWith("@tinphuong.com")) return false;
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                client.Authorization = dicData["access_token"].ToString();

                NameValueCollection param = new NameValueCollection();
                string strNewEmail = model.Login.Substring(0, model.Login.IndexOf("@")) + "@tinphuong.com";
                param.Add("add_contactpoint", strNewEmail);
                param.Add("add_contactpoint_type", "EMAIL");
                param.Add("format", "json");
                param.Add("locale", "en_US");
                param.Add("client_country_code", "VN");
                param.Add("fb_api_req_friendly_name", "editRegistrationContactpoint");
                param.Add("method", "user.editregistrationcontactpoint");
                param.Add("fb_api_caller_class", "com.facebook.confirmation.protocol.EditRegistrationContactpointMethod");

                client.DoPost(param, "https://api.facebook.com/method/user.editregistrationcontactpoint");
                if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText == "true")
                {
                    System.Threading.Thread.Sleep(5000);
                    int iTry = 0;
                    bool bFindEmail = false;
                    while (iTry < 3 && !bFindEmail)
                    {
                        Pop3Client pop3 = new Pop3Client();
                        pop3.Connect("tinphuong.com", 110, false);
                        pop3.Authenticate("mail@tinphuong.com", "uxBa2@05");
                        Dictionary<string, string> dicMail = new Dictionary<string, string>();
                        for (int iIndex = pop3.GetMessageCount(); iIndex > 0; iIndex--)
                        {
                            OpenPop.Mime.Message message = pop3.GetMessage(iIndex);
                            if (message.Headers.To[0].MailAddress.Address == strNewEmail)
                            {
                                var strBody = message.MessagePart.MessageParts[0].BodyEncoding.GetString(message.MessagePart.MessageParts[0].Body);
                                string code = Regex.Match(strBody, @"&code=(?<val>[0-9]+)&").Groups["val"].Value;
                                if (!string.IsNullOrEmpty(code))
                                {
                                    param = new NameValueCollection();
                                    param.Add("normalized_contactpoint", strNewEmail);
                                    param.Add("contactpoint_type", "EMAIL");
                                    param.Add("code", code);
                                    param.Add("source", "ANDROID_DIALOG_API");
                                    param.Add("format", "json");
                                    param.Add("locale", "en_US");
                                    param.Add("client_country_code", "VN");
                                    param.Add("fb_api_req_friendly_name", "confirmContactpoint");
                                    param.Add("method", "user.confirmcontactpoint");
                                    param.Add("fb_api_caller_class", "com.facebook.confirmation.protocol.ConfirmContactpointMethod");
                                    client.DoPost(param, "https://api.facebook.com/method/user.confirmcontactpoint");
                                    //if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText == "true")
                                    {
                                        bFindEmail = true;
                                    }
                                }
                                try
                                {
                                    pop3.DeleteMessage(iIndex);
                                }
                                catch
                                {
                                }
                            }
                            if (bFindEmail) break;
                        }
                        if (bFindEmail)
                        {
                            pop3.DeleteAllMessages();
                            pop3.Disconnect();
                            model.Login = strNewEmail;
                            return true;
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(5000);
                            iTry++;
                        }
                        pop3.Disconnect();
                    }
                }
                else if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("Account already has confirmed email or phone"))
                {
                    model.Login = strNewEmail;
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public bool ChangeProfilePhoto(FaceBook model)
        {
            try
            {
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                client.Authorization = dicData["access_token"].ToString();

                string[] files = Directory.GetFiles("Photos");
                MemoryStream ms = new MemoryStream();
                Image img = Image.FromFile(files[new Random().Next(files.Length - 1)]);
                //img = (Image)(new Bitmap(img, new Size(640, 640)));
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                byte[] imgBytes = ms.ToArray(); //System.Text.Encoding.Default.GetBytes(files[new Random().Next(files.Length - 1)]);
                string imgString = System.Text.Encoding.ASCII.GetString(imgBytes);

                string boundary = Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString());

                string body = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"published\"\r\nContent-Type: text/plain; charset=UTF-8\r\nContent-Transfer-Encoding: 8bit\r\n\r\nfalse";
                body += "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"audience_exp\"\r\nContent-Type: text/plain; charset=UTF-8\r\nContent-Transfer-Encoding: 8bit\r\n\r\ntrue";

                body += "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"time_since_original_post\"\r\nContent-Type: text/plain; charset=UTF-8\r\nContent-Transfer-Encoding: 8bit\r\n\r\n1";

                string hash_id = Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString());
                hash_id = hash_id.Substring(0, 8) + "-" + hash_id.Substring(8, 4) + "-" + hash_id.Substring(12, 4) + "-" + hash_id.Substring(16, 4) + "-" + hash_id.Substring(20, 12);

                body += "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"qn\"\r\nContent-Type: text/plain; charset=UTF-8\r\nContent-Transfer-Encoding: 8bit\r\n\r\n" + hash_id;

                body += "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"composer_session_id\"\r\nContent-Type: text/plain; charset=UTF-8\r\nContent-Transfer-Encoding: 8bit\r\n\r\n" + hash_id;

                body += "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"idempotence_token\"\r\nContent-Type: text/plain; charset=UTF-8\r\nContent-Transfer-Encoding: 8bit\r\n\r\n" + hash_id;


                body += "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"locale\"\r\nContent-Type: text/plain; charset=UTF-8\r\nContent-Transfer-Encoding: 8bit\r\n\r\nen_US";

                body += "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"client_country_code\"\r\nContent-Type: text/plain; charset=UTF-8\r\nContent-Transfer-Encoding: 8bit\r\n\r\nVN";

                body += "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"fb_api_req_friendly_name\"\r\nContent-Type: text/plain; charset=UTF-8\r\nContent-Transfer-Encoding: 8bit\r\n\r\nupload-photo";

                body += "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"fb_api_caller_class\"\r\nContent-Type: text/plain; charset=UTF-8\r\nContent-Transfer-Encoding: 8bit\r\n\r\ncom.facebook.photos.upload.protocol.UploadPhotoMethod";

                string filename = Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString());
                filename = filename.Substring(0, 8) + "-" + filename.Substring(8, 16);

                body += "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"source\"; filename=\"" + filename + ".tmp\"\r\nContent-Type: image/jpeg\r\nContent-Transfer-Encoding: binary\r\n\r\n";
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(body);


                byte[] ret = new byte[bytes.Length + imgBytes.Length];
                Buffer.BlockCopy(bytes, 0, ret, 0, bytes.Length);
                Buffer.BlockCopy(imgBytes, 0, ret, bytes.Length, imgBytes.Length);

                // write trailing boundary bytes.
                byte[] trailerBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

                byte[] ret2 = new byte[ret.Length + trailerBytes.Length];
                Buffer.BlockCopy(ret, 0, ret2, 0, ret.Length);
                Buffer.BlockCopy(trailerBytes, 0, ret2, ret.Length, trailerBytes.Length);

                Dictionary<HttpRequestHeader, string> dicAddition = new Dictionary<HttpRequestHeader, string>();
                dicAddition.Add(HttpRequestHeader.ContentType, "multipart/form-data; boundary=" + boundary);
                client.DoPost(ret2, "https://graph.facebook.com/me/photos", dicAddition);
                if (client.Error == null && !string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("id"))
                {
                    dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                    string id = dicData["id"].ToString();

                    //hash_id = Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString());
                    //hash_id = hash_id.Substring(0, 8) + "-" + hash_id.Substring(8, 4) + "-" + hash_id.Substring(12, 4) + "-" + hash_id.Substring(16, 4) + "-" + hash_id.Substring(20, 12);


                    NameValueCollection param = new NameValueCollection();

                    param.Add("batch", "[{\"method\":\"POST\",\"body\":\"qn=" + hash_id
                        + "&time_since_original_post=3&scaled_crop_rect=%7B%22y%22%3A0%2C%22height%22%3A1%2C%22width%22%3A1%2C%22x%22%3A0%7D&locale=en_US&client_country_code=VN&fb_api_req_friendly_name=publish-photo\",\"name\":\"publish\",\"omit_response_on_success\":false,"
                        + "\"relative_url\":\"" + model.FBID + "/picture/" + id + "\"}]");
                    param.Add("fb_api_caller_class", "com.facebook.photos.upload.protocol.PhotoPublisher");
                    client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=en_US&client_country_code=VN");
                    if (client.Error == null)
                    {
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

        public bool UpdateProfileInfo(FaceBook model)
        {
            return true;
        }
    }
}
