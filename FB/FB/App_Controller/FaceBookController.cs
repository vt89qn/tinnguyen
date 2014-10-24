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
using System.Collections;

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
                string sig = Utilities.getSignFB(param, AppSettings.api_secret);
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

        public void checkFaceBook(FaceBook model)
        {
            FBExtraData exData = GetExtraData(model);
            WebClientEx client = new WebClientEx();
            client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
            Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
            ArrayList listPost = new ArrayList();
            client.Authorization = dicData["access_token"].ToString();
            NameValueCollection param = new NameValueCollection();
            string hash_id = Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString());
            hash_id = hash_id.Substring(0, 8) + "-" + hash_id.Substring(8, 4) + "-" + hash_id.Substring(12, 4) + "-" + hash_id.Substring(16, 4) + "-" + hash_id.Substring(20, 12);
            #region - Check live -
            {
                WebClientEx clientAPI = new WebClientEx();
                clientAPI.DoGet("https://graph.facebook.com/v2.1/me/?access_token=" + client.Authorization + "&fields=birthday&format=json&method=get&pretty=0&suppress_http_code=1");
                if (clientAPI.Error != null && client.ResponseText.Contains("birthday"))
                {
                    //Live
                }
                else if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("\"error\""))
                {
                    model.FBPackageID = 1;
                    Global.DBContext.SaveChanges();
                }
                else
                {
                }
            }
            #endregion
            #region - Get New Feed -
            {
                param = new NameValueCollection();
                param.Add("batch", "[{'method':'POST','body':'query_id=10153084471926729&method=get&query_params=%7B%22action_location%22%3A%22feed%22%7D&locale=en_US&client_country_code=VN&fb_api_req_friendly_name=NewsFeedQueryDepth2','name':'first-fetch','omit_response_on_success':false,'relative_url':'graphql'}]".Replace("'", "\""));
                param.Add("fb_api_caller_class", "com.facebook.feed.server.NewsFeedServiceImplementation");
                param.Add("flush", "1");
                client.DoPost(param, "https://graph.facebook.com/?include_headers=false&decode_body_json=false&streamable_json_response=true&locale=en_US&client_country_code=VN");
                if (!string.IsNullOrEmpty(client.ResponseText))
                {
                    dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                    if (dicData.ContainsKey("first-fetch") && dicData["first-fetch"] is ArrayList)
                    {
                        foreach (Dictionary<string, object> dicFeed in dicData["first-fetch"] as ArrayList)
                        {
                            if (dicFeed.ContainsKey("body"))
                            {
                                listPost = (((dicFeed["body"] as Dictionary<string, object>)["viewer"] as Dictionary<string, object>)["news_feed"] as Dictionary<string, object>)["edges"] as ArrayList;
                            }
                        }
                    }
                }
            }
            #endregion
            #region - Post Status -
            if (!exData.LastUpdateStatus.HasValue || exData.LastUpdateStatus.Value.Date.AddDays(new Random().Next(0, 4)) <= DateTime.Today)
            {
                if (PostStatus(model))
                {
                    exData.LastUpdateStatus = DateTime.Now;
                }
            }
            #endregion
            #region - Upload photo -
            if (!exData.LastUpLoadPhoto.HasValue || exData.LastUpLoadPhoto.Value.Date.AddDays(new Random().Next(0, 4)) <= DateTime.Today)
            {
                if (Global.LisCoverPhotoLink.Count > 0)
                {
                    UploadPhoto(model, Global.LisCoverPhotoLink[0]);
                    Global.LisCoverPhotoLink.RemoveAt(0);
                    exData.LastUpLoadPhoto = DateTime.Now;
                }
            }
            #endregion
            #region - Update Profile Photo -
            if (!exData.LastUpdateProfilePhoto.HasValue || exData.LastUpdateProfilePhoto.Value.Date.AddDays(new Random().Next(10, 20)) <= DateTime.Today)
            {
                if (Global.ListProfilePhotoLink.Count > 0)
                {
                    string id = UploadPhoto(model, Global.ListProfilePhotoLink[0]);
                    Global.ListProfilePhotoLink.RemoveAt(0);
                    if (!string.IsNullOrEmpty(id))
                    {
                        param.Add("batch", "[{\"method\":\"POST\",\"body\":\"qn=" + hash_id
                     + "&time_since_original_post=3&scaled_crop_rect=%7B%22y%22%3A0%2C%22height%22%3A1%2C%22width%22%3A1%2C%22x%22%3A0%7D&locale=en_US&client_country_code=VN&fb_api_req_friendly_name=publish-photo\",\"name\":\"publish\",\"omit_response_on_success\":false,"
                     + "\"relative_url\":\"" + model.FBID + "/picture/" + id + "\"}]");
                        param.Add("fb_api_caller_class", "com.facebook.photos.upload.protocol.PhotoPublisher");
                        client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=en_US&client_country_code=VN");
                        exData.LastUpdateProfilePhoto = DateTime.Now;
                    }
                }
            }
            #endregion
            #region - Update Cover Photo -
            if (!exData.LastUpdateCoverPhoto.HasValue || exData.LastUpdateCoverPhoto.Value.Date.AddDays(new Random().Next(10, 20)) <= DateTime.Today)
            {
                if (Global.LisCoverPhotoLink.Count > 0)
                {
                    string id = UploadPhoto(model, Global.LisCoverPhotoLink[Global.LisCoverPhotoLink.Count - 1]);
                    Global.LisCoverPhotoLink.RemoveAt(Global.LisCoverPhotoLink.Count - 1);
                    if (!string.IsNullOrEmpty(id))
                    {
                        param = new NameValueCollection();
                        param.Add("batch", "[{\"method\":\"POST\",\"body\":\"qn=" + hash_id
                         + "&time_since_original_post=12&photo=" + id + "&focus_y=0&locale=en_US&client_country_code=VN&fb_api_req_friendly_name=publish-photo\",\"name\":\"publish\",\"omit_response_on_success\":false,"
                         + "\"relative_url\":\"" + model.FBID + "/cover\"}]");
                        param.Add("fb_api_caller_class", "com.facebook.photos.upload.protocol.PhotoPublisher");
                        client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=en_US&client_country_code=VN");
                        exData.LastUpdateCoverPhoto = DateTime.Now;
                    }
                }
            }
            #endregion
            #region - Like -
            if (listPost.Count > 0)
            {
                int iRD = new Random().Next(0, 4);
                for (int iIndex = 0; iIndex < iRD; iIndex++)
                {
                    if (listPost.Count > iIndex)
                    {
                        string strPostID = (((listPost[iIndex] as Dictionary<string, object>)["node"] as Dictionary<string, object>)["feedback"] as Dictionary<string, object>)["legacy_api_post_id"].ToString();
                        Like(model, strPostID);
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            #endregion
            #region - Comment -
            if (listPost.Count > 0)
            {
                int iRD = new Random().Next(0, 4);
                for (int iIndex = 0; iIndex < iRD; iIndex++)
                {
                    if (listPost.Count > iIndex)
                    {
                        string strPostID = (((listPost[iIndex] as Dictionary<string, object>)["node"] as Dictionary<string, object>)["feedback"] as Dictionary<string, object>)["legacy_api_post_id"].ToString();
                        Comment(model, strPostID);
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            #endregion
            #region - Make Friend -
            {
                int iRD = new Random().Next(0, 3);
                //Select in Package
                List<FaceBook> listFB = (from fb in Global.DBContext.FaceBook
                                         where fb.ID != model.ID
                                         && fb.FBPackageID == model.FBPackageID
                                         && !(from o in Global.DBContext.FBFriend
                                              select o.FBID1)
                                                .Contains(fb.ID)
                                        && !(from o in Global.DBContext.FBFriend
                                             select o.FBID2)
                                        .Contains(fb.ID)
                                         select fb).ToList();
                while (listFB.Count > 0 && iRD > 0)
                {
                    if (SendFriendRequest(model, listFB[0]) &&
                     AcceptFriendRequest(listFB[0], model))
                    {
                        Global.DBContext.FBFriend.Add(new FBFriend { FBID1 = model.ID, FBID2 = listFB[0].ID });
                        Global.DBContext.SaveChanges();
                    }
                    listFB.RemoveAt(0);
                    iRD--;
                }
                //Select in Package
                listFB = (from fb in Global.DBContext.FaceBook
                          where fb.ID != model.ID
                          && !(from o in Global.DBContext.FBFriend
                               select o.FBID1)
                                 .Contains(fb.ID)
                         && !(from o in Global.DBContext.FBFriend
                              select o.FBID2)
                         .Contains(fb.ID)
                          select fb).Take(iRD).ToList();
                while (listFB.Count > 0 && iRD > 0)
                {
                    if (SendFriendRequest(model, listFB[0]) &&
                     AcceptFriendRequest(listFB[0], model))
                    {
                        Global.DBContext.FBFriend.Add(new FBFriend { FBID1 = model.ID, FBID2 = listFB[0].ID });
                        Global.DBContext.SaveChanges();
                    }
                    listFB.RemoveAt(0);
                    iRD--;
                }
            }
            #endregion
        }

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
            FBExtraData extraData = new FBExtraData();

            WebClientEx client = new WebClientEx();
            client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;

            NameValueCollection param = new NameValueCollection();

            string hash_id = Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString());
            hash_id = hash_id.Substring(0, 8) + "-" + hash_id.Substring(8, 4) + "-" + hash_id.Substring(12, 4) + "-" + hash_id.Substring(16, 4) + "-" + hash_id.Substring(20, 12);
            string sig = Utilities.getSignFB(param, AppSettings.api_secret);

            string strName = Utilities.ConvertToUsignNew(Utilities.GetMaleName());
            param = new NameValueCollection();
            param.Add("api_key", AppSettings.api_key);
            param.Add("attempt_login", "true");
            extraData.BirthDay = new DateTime(new Random().Next(1979, 1990), new Random().Next(1, 12), new Random(3).Next(1, 28));
            param.Add("birthday", extraData.BirthDay.Value.ToString("yyyy-MM-dd"));
            param.Add("client_country_code", "VN");

            param.Add("fb_api_caller_class", "com.facebook.registration.protocol.RegisterAccountMethod");
            param.Add("fb_api_req_friendly_name", "registerAccount");
            param.Add("firstname", strName.Trim().Split('_')[0]);
            System.Threading.Thread.Sleep(34);
            param.Add("format", "json");
            param.Add("gender", "M");
            param.Add("lastname", strName.Trim().Split('_')[1]);
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
            extraData.ComfirmedEmail = false;
            param.Add("email", Utilities.ConvertToUsignNew(param["firstname"].Replace(" ", "").Trim() + param["lastname"].Replace(" ", "").Trim() + new Random().Next(10, 9999)).ToLower() + "@" + listEmail[new Random().Next(0, listEmail.Count - 1)]);
            param.Add("locale", "en_US");
            param.Add("method", "user.register"); string strPass = param["firstname"].Split(' ')[0] + new List<string> { "!", "@", "#", "$", "%", "^", "~", "&", "*", "(" }[new Random().Next(0, 10)] + param["lastname"].Split(' ')[1] + new Random().Next(10, 1000);
            System.Threading.Thread.Sleep(19);
            strPass += new List<string> { "!", "@", "#", "$", "%", "^", "~", "&", "*", "(" }[new Random().Next(0, 10)];
            param.Add("password", strPass);
            param.Add("reg_instance", hash_id);
            param.Add("return_multiple_errors", "true");
            sig = Utilities.getSignFB(param, AppSettings.api_secret);
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

                    if (LoginMobile(model, client))
                    {
                        client.SetCookieV2 = false;
                        dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                        client.Authorization = dicData["access_token"].ToString();

                        #region - Upload profile picture -
                        if (Global.ListProfilePhotoLink.Count > 0)
                        {
                            string id = UploadPhoto(model, Global.ListProfilePhotoLink[0]);
                            Global.ListProfilePhotoLink.RemoveAt(0);
                            if (!string.IsNullOrEmpty(id))
                            {
                                param.Add("batch", "[{\"method\":\"POST\",\"body\":\"qn=" + hash_id
                             + "&time_since_original_post=3&scaled_crop_rect=%7B%22y%22%3A0%2C%22height%22%3A1%2C%22width%22%3A1%2C%22x%22%3A0%7D&locale=en_US&client_country_code=VN&fb_api_req_friendly_name=publish-photo\",\"name\":\"publish\",\"omit_response_on_success\":false,"
                             + "\"relative_url\":\"" + model.FBID + "/picture/" + id + "\"}]");
                                param.Add("fb_api_caller_class", "com.facebook.photos.upload.protocol.PhotoPublisher");
                                client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=en_US&client_country_code=VN");
                                extraData.LastUpdateProfilePhoto = DateTime.Now;
                            }
                        }
                        #endregion
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

                        #region - Update work and class info -
                        param = new NameValueCollection();
                        param.Add("work", "[{'id':'1479876348939597','privacy':'{\\'value\\':\\'EVERYONE\\'}','ref':'nux_android'}]".Replace("'", "\""));
                        param.Add("education", "[{'id':'823697264348577','privacy':'{\\'value\\':\\'EVERYONE\\'}','ref':'nux_android','type':'College'},{'id':'823697264348577','privacy':'{\\'value\\':\\'EVERYONE\\'}','ref':'nux_android','type':'High School'}]".Replace("'", "\""));
                        param.Add("location", "{'id':'108458769184495','privacy':'{\\'value\\':\\'EVERYONE\\'}','ref':'nux_android'}".Replace("'", "\""));
                        param.Add("hometown", "{'id':'108458769184495','privacy':'{\\'value\\':\\'EVERYONE\\'}','ref':'nux_android'}".Replace("'", "\""));
                        param.Add("locale", "en_US");
                        param.Add("client_country_code", "VN");
                        param.Add("fb_api_req_friendly_name", "save_core_profile_info");
                        client.DoPost(param, "https://graph.facebook.com/me");
                        extraData.UpdatedProfileInfo = true;
                        #endregion
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
                        if (client.ResponseText.ToLower().Trim() != "true") return null;
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

                        string strNewEmail = model.Login.Substring(0, model.Login.IndexOf("@")) + "@vuathuthanh.net";
                        param = new NameValueCollection();
                        param.Add("add_contactpoint", strNewEmail);
                        param.Add("add_contactpoint_type", "EMAIL");
                        param.Add("format", "json");
                        param.Add("locale", "en_US");
                        param.Add("client_country_code", "VN");
                        param.Add("fb_api_req_friendly_name", "editRegistrationContactpoint");
                        param.Add("method", "user.editregistrationcontactpoint");
                        param.Add("fb_api_caller_class", "com.facebook.confirmation.protocol.EditRegistrationContactpointMethod");

                        #region - confirm email -
                        client.DoPost(param, "https://api.facebook.com/method/user.editregistrationcontactpoint");
                        if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText == "true")
                        {
                            System.Threading.Thread.Sleep(5000);
                            int iTry = 0;
                            bool bFindEmail = false;
                            while (iTry < 3 && !bFindEmail)
                            {
                                Pop3Client pop3 = new Pop3Client();
                                pop3.Connect("vuathuthanh.net", 110, false);
                                pop3.Authenticate("mail@vuathuthanh.net", "uxBa2@05");
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
                                    try
                                    {
                                        pop3.DeleteAllMessages();
                                        pop3.Disconnect();
                                    }
                                    catch { }
                                    model.Login = strNewEmail;
                                    extraData.ComfirmedEmail = true;
                                }
                                else
                                {
                                    System.Threading.Thread.Sleep(3000);
                                    iTry++;
                                }
                                try
                                {
                                    pop3.Disconnect();
                                }
                                catch { }
                            }
                        }
                        #endregion

                        #region - update cover photo -
                        if (Global.LisCoverPhotoLink.Count > 0)
                        {
                            string id = UploadPhoto(model, Global.LisCoverPhotoLink[Global.LisCoverPhotoLink.Count - 1]);
                            Global.ListProfilePhotoLink.RemoveAt(Global.LisCoverPhotoLink.Count - 1);

                            if (!string.IsNullOrEmpty(id))
                            {
                                param = new NameValueCollection();
                                param.Add("batch", "[{\"method\":\"POST\",\"body\":\"qn=" + hash_id
                                 + "&time_since_original_post=12&photo=" + id + "&focus_y=0&locale=en_US&client_country_code=VN&fb_api_req_friendly_name=publish-photo\",\"name\":\"publish\",\"omit_response_on_success\":false,"
                                 + "\"relative_url\":\"" + model.FBID + "/cover\"}]");
                                param.Add("fb_api_caller_class", "com.facebook.photos.upload.protocol.PhotoPublisher");
                                client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=en_US&client_country_code=VN");
                                extraData.LastUpdateCoverPhoto = DateTime.Now;
                            }
                        }
                        #endregion

                        SetExtraData(model, extraData);
                        return model;
                    }
                }
            }
            return null;
        }

        public bool PostStatus(FaceBook model)
        {
            try
            {
                FBExtraData extraData = GetExtraData(model);
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                client.Authorization = dicData["access_token"].ToString();

                Random r = new Random();
                string strStatus = Global.DBContext.StatusData.ElementAt(r.Next(1, Global.DBContext.StatusData.Count())).Text;

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
                else if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText.Contains("\"error\""))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public string UploadPhoto(FaceBook model, string strLink)
        {
            try
            {
                FBExtraData extraData = GetExtraData(model);
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                if (Global.LisCoverPhotoLink.Count > 0)
                {
                    NameValueCollection param = new NameValueCollection();
                    param.Add("access_token", dicData["access_token"].ToString());
                    param.Add("format", "json");
                    param.Add("method", "post");
                    param.Add("pretty", "0");
                    param.Add("suppress_http_code", "1");
                    param.Add("url", Global.LisCoverPhotoLink[0]);
                    WebClientEx clientAPI = new WebClientEx();
                    clientAPI.DoPost(param, "https://graph.facebook.com/v2.1/me/photos");
                    if (!string.IsNullOrEmpty(clientAPI.ResponseText))
                    {
                        dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(clientAPI.ResponseText);
                        if (dicData.ContainsKey("id"))
                        {
                            string id = dicData["id"].ToString();
                            return id;
                        }
                    }
                }
            }
            catch { }
            return string.Empty;
        }

        public bool ConfirmEmail(FaceBook model)
        {
            try
            {
                if (model.Login.EndsWith("@vuathuthanh.net")) return false;
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                client.Authorization = dicData["access_token"].ToString();

                NameValueCollection param = new NameValueCollection();
                string strNewEmail = model.Login.Substring(0, model.Login.IndexOf("@")) + "@vuathuthanh.net";
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
                    System.Threading.Thread.Sleep(3000);
                    int iTry = 0;
                    bool bFindEmail = false;
                    while (iTry < 3 && !bFindEmail)
                    {
                        Pop3Client pop3 = new Pop3Client();
                        pop3.Connect("vuathuthanh.net", 110, false);
                        pop3.Authenticate("mail@vuathuthanh.net", "uxBa2@05");
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
                            System.Threading.Thread.Sleep(3000);
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

        public void Like(FaceBook model, string id)
        {
            try
            {
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                if (Global.LisCoverPhotoLink.Count > 0)
                {
                    NameValueCollection param = new NameValueCollection();
                    param.Add("access_token", dicData["access_token"].ToString());
                    param.Add("format", "json");
                    param.Add("method", "post");
                    param.Add("pretty", "0");
                    param.Add("suppress_http_code", "1");
                    WebClientEx clientAPI = new WebClientEx();
                    clientAPI.DoPost(param, "https://graph.facebook.com/v2.1/" + id + "/likes");
                }
            }
            catch { }
        }

        public void Comment(FaceBook model, string id)
        {
            try
            {
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                if (Global.LisCoverPhotoLink.Count > 0)
                {
                    Random r = new Random();
                    NameValueCollection param = new NameValueCollection();
                    param.Add("access_token", dicData["access_token"].ToString());
                    param.Add("format", "json");
                    param.Add("method", "post");
                    param.Add("pretty", "0");
                    param.Add("suppress_http_code", "1");
                    param.Add("message", Global.DBContext.StatusData.ElementAt(r.Next(1, Global.DBContext.StatusData.Count())).Text);
                    WebClientEx clientAPI = new WebClientEx();
                    clientAPI.DoPost(param, "https://graph.facebook.com/v2.1/" + id + "/comments");
                }
            }
            catch { }
        }

        private FBExtraData GetExtraData(FaceBook model)
        {
            FBExtraData fbExtraData = new FBExtraData();
            if (model != null && !string.IsNullOrEmpty(model.ExtraData))
            {
                fbExtraData = new JavaScriptSerializer().Deserialize<FBExtraData>(model.ExtraData);
            }
            return fbExtraData;
        }

        private void SetExtraData(FaceBook model, FBExtraData extraData)
        {
            if (!extraData.CreateDate.HasValue) extraData.CreateDate = DateTime.Now;
            model.ExtraData = new JavaScriptSerializer().Serialize(extraData);
        }
    }
}
