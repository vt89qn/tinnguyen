using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerTexas.App_UserControl;
using PokerTexas.App_Common;
using System.Collections.Specialized;
using System.Data.SQLite;
using PokerTexas.App_Model;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using System.Drawing;
using System.IO;
using System.Net;

namespace PokerTexas.App_Controller
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
            try
            {
                FaceBook model = null;
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;

                NameValueCollection param = new NameValueCollection();
                string hash_id = Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString());
                hash_id = hash_id.Substring(0, 8) + "-" + hash_id.Substring(8, 4) + "-" + hash_id.Substring(12, 4) + "-" + hash_id.Substring(16, 4) + "-" + hash_id.Substring(20, 12);
                string sig = Utilities.getSignFB(param, "62f8ce9f74b12f84c123cc23437a4a32");
                string strName = Utilities.ConvertToUsignNew(Utilities.GetMaleName());
                string phoneNumber = "+8412" + new List<string> { "1", "2", "3", "4", "5", "6" }[new Random().Next(0, 6)];
                System.Threading.Thread.Sleep(23);
                phoneNumber += new Random().Next(1111111, 9999999).ToString();
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
                string email = (strName.Replace(" ", "").Trim() + new Random().Next(10, 9999)).ToLower() + "@" + listEmail[new Random().Next(0, listEmail.Count - 1)];
                param = new NameValueCollection();
                param.Add("api_key", AppSettings.api_key);
                param.Add("attempt_login", "true");
                param.Add("birthday", new Random().Next(1980, 1995).ToString() + "-" + new Random().Next(1, 12).ToString("00") + "-" + new Random(3).Next(1, 28).ToString("00"));
                param.Add("client_country_code", "VN");

                param.Add("fb_api_caller_class", "com.facebook.registration.protocol.RegisterAccountMethod");
                param.Add("fb_api_req_friendly_name", "registerAccount");
                param.Add("firstname", strName.Trim().Split(' ')[1]);
                System.Threading.Thread.Sleep(34);
                param.Add("format", "json");
                param.Add("gender", "M");
                param.Add("lastname", strName.Split(' ')[0] +" "+ strName.Split(' ')[2]);
                param.Add("phone", phoneNumber);
                //param.Add("email", email);
                param.Add("locale", "en_US");
                param.Add("method", "user.register");
                param.Add("password", phoneNumber.Replace("+84", "0"));
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
                        //model.Login = param["email"];
                        model.Pass = param["password"];
                        model.MBLoginText = new JavaScriptSerializer().Serialize(dicData["session_info"]);
                        model.FBID = (dicData["session_info"] as Dictionary<string, object>)["uid"].ToString();
                        model.Login = model.FBID;
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
                                    param = new NameValueCollection();

                                    param.Add("batch", "[{\"method\":\"POST\",\"body\":\"qn=" + hash_id
                                 + "&scaled_crop_rect=%7B%22y%22%3A0%2C%22height%22%3A1%2C%22width%22%3A1%2C%22x%22%3A0%7D&locale=vi_VN&client_country_code=VN&fb_api_req_friendly_name=publish-photo\",\"name\":\"publish\",\"omit_response_on_success\":false,"
                                 + "\"relative_url\":\"" + model.FBID + "/picture/" + id + "\"}]");
                                    param.Add("fb_api_caller_class", "com.facebook.photos.upload.protocol.PhotoPublisher");
                                    client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=vi_VN&client_country_code=VN");

                                }
                            }

                            param = new NameValueCollection();
                            param.Add("format", "JSON");
                            param.Add("nux_id", "ANDROID_NEW_ACCOUNT_WIZARD");
                            param.Add("step", "upload_profile_pic");
                            param.Add("status", "COMPLETE");
                            param.Add("extra_data", "{}");
                            param.Add("locale", "vi_VN");
                            param.Add("client_country_code", "VN");
                            param.Add("method", "user.updateNuxStatus");
                            param.Add("fb_api_req_friendly_name", "updateNuxStatus");
                            param.Add("fb_api_caller_class", "com.facebook.nux.status.UpdateNuxStatusMethod");
                            //System.Threading.Thread.Sleep(1000);
                            client.DoPost(param, "https://api.facebook.com/method/user.updateNuxStatus");
                            #endregion

                            #region - Update work and class info -
                            param = new NameValueCollection();
                            param.Add("work", ("[{'id':'" + Utilities.GetProfileInfo(4) + "','privacy':'{\\'value\\':\\'EVERYONE\\'}','ref':'nux_android'}]").Replace("'", "\""));
                            param.Add("education", ("[{'id':'" + Utilities.GetProfileInfo(3) + "','privacy':'{\\'value\\':\\'EVERYONE\\'}','ref':'nux_android','type':'College'},{'id':'" + Utilities.GetProfileInfo(2) + "','privacy':'{\\'value\\':\\'EVERYONE\\'}','ref':'nux_android','type':'High School'}]").Replace("'", "\""));
                            //param.Add("location", "{'id':'108458769184495','privacy':'{\\'value\\':\\'EVERYONE\\'}','ref':'nux_android'}".Replace("'", "\""));
                            //param.Add("hometown", "{'id':'108458769184495','privacy':'{\\'value\\':\\'EVERYONE\\'}','ref':'nux_android'}".Replace("'", "\""));
                            param.Add("locale", "vi_VN");
                            param.Add("client_country_code", "VN");
                            param.Add("fb_api_req_friendly_name", "save_core_profile_info");
                            client.DoPost(param, "https://graph.facebook.com/me");


                            param = new NameValueCollection();
                            param.Add("format", "JSON");
                            param.Add("nux_id", "ANDROID_NEW_ACCOUNT_WIZARD");
                            param.Add("step", "classmates_coworkers");
                            param.Add("status", "COMPLETE");
                            param.Add("extra_data", "{}");
                            param.Add("locale", "vi_VN");
                            param.Add("client_country_code", "VN");
                            param.Add("method", "user.updateNuxStatus");
                            param.Add("fb_api_req_friendly_name", "updateNuxStatus");
                            param.Add("fb_api_caller_class", "com.facebook.nux.status.UpdateNuxStatusMethod");
                            client.DoPost(param, "https://api.facebook.com/method/user.updateNuxStatus");
                            #endregion

                            #region - update cover photo -
                            if (Global.LisCoverPhotoLink.Count > 0)
                            {
                                string id = UploadPhoto(model, Global.LisCoverPhotoLink[Global.LisCoverPhotoLink.Count - 1]);
                                Global.LisCoverPhotoLink.RemoveAt(Global.LisCoverPhotoLink.Count - 1);

                                if (!string.IsNullOrEmpty(id))
                                {
                                    param = new NameValueCollection();
                                    param.Add("batch", "[{\"method\":\"POST\",\"body\":\"qn=" + hash_id
                                     + "&photo=" + id + "&focus_y=0&locale=vi_VN&client_country_code=VN&fb_api_req_friendly_name=publish-photo\",\"name\":\"publish\",\"omit_response_on_success\":false,"
                                     + "\"relative_url\":\"" + model.FBID + "/cover\"}]");
                                    param.Add("fb_api_caller_class", "com.facebook.photos.upload.protocol.PhotoPublisher");
                                    client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=vi_VN&client_country_code=VN");
                                }
                            }
                            #endregion
                            return model;
                        }
                    }

                }
            }
            catch { }
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

        public string UploadPhoto(FaceBook model, string strLink)
        {
            try
            {
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                if (!string.IsNullOrEmpty(strLink))
                {
                    NameValueCollection param = new NameValueCollection();
                    param.Add("access_token", dicData["access_token"].ToString());
                    param.Add("format", "json");
                    param.Add("method", "post");
                    param.Add("pretty", "0");
                    param.Add("suppress_http_code", "1");
                    param.Add("url", strLink);
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

        public void Like(FaceBook model, string id)
        {
            try
            {
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                NameValueCollection param = new NameValueCollection();
                param.Add("access_token", dicData["access_token"].ToString());
                param.Add("format", "json");
                param.Add("method", "post");
                param.Add("pretty", "0");
                param.Add("suppress_http_code", "1");
                WebClientEx clientAPI = new WebClientEx();
                clientAPI.DoPost(param, "https://graph.facebook.com/v2.1/" + id + "/likes");

            }
            catch { }
        }
    }
}
