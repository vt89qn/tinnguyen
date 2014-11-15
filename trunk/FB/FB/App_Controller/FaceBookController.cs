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
using System.Globalization;
using System.Diagnostics;

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
                param.Add("locale", "vi_VN");
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

        public bool checkFaceBook(FaceBook model)
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
                if (clientAPI.Error == null && clientAPI.ResponseText.Contains("birthday"))
                {
                    //Live
                    if (!exData.BirthDay.HasValue)
                    {
                        try
                        {

                            exData.BirthDay = DateTime.Parse(Regex.Match(clientAPI.ResponseText, "birthday\":\"(?<val>[^\"]+)").Groups["val"].Value.Replace("\\/", "-")
                                , new DateTimeFormatInfo { FullDateTimePattern = "dd-MM-yyyy" }, DateTimeStyles.None);
                        }
                        catch { }
                    }
                }
                else if (!string.IsNullOrEmpty(clientAPI.ResponseText) && clientAPI.ResponseText.Contains("\"error\""))
                {
                    //model.FBPackageID = 1;
                    //Global.DBContext.SaveChanges();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            //Like(model, "bichlien1234");
            //Like(model, "352321118281980");
            //Like(model, "150884418415392");
            return true;
            #endregion
            #region - Get New Feed -
            {
                param = new NameValueCollection();
                param.Add("batch", "[{'method':'POST','body':'query_id=10153084471926729&method=get&query_params=%7B%22action_location%22%3A%22feed%22%7D&locale=vi_VN&client_country_code=VN&fb_api_req_friendly_name=NewsFeedQueryDepth2','name':'first-fetch','omit_response_on_success':false,'relative_url':'graphql'}]".Replace("'", "\""));
                param.Add("fb_api_caller_class", "com.facebook.feed.server.NewsFeedServiceImplementation");
                param.Add("flush", "1");
                client.DoPost(param, "https://graph.facebook.com/?include_headers=false&decode_body_json=false&streamable_json_response=true&locale=vi_VN&client_country_code=VN");
                if (client.Error == null && !string.IsNullOrEmpty(client.ResponseText))
                {
                    try
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
                    catch { }
                }
            }
            #endregion
            #region - Post Status -
            System.Threading.Thread.Sleep(23);
            if (!exData.LastUpdateStatus.HasValue || exData.LastUpdateStatus.Value.Date.AddDays(new Random().Next(0, 2)) < DateTime.Today)
            {
                if (PostStatus(model))
                {
                    exData.LastUpdateStatus = DateTime.Now;
                }
            }
            #endregion
            #region - Upload photo -
            System.Threading.Thread.Sleep(23);
            if (!exData.LastUpLoadPhoto.HasValue || exData.LastUpLoadPhoto.Value.Date.AddDays(new Random().Next(0, 4)) < DateTime.Today)
            {
                if (Global.LisCoverPhotoLink.Count > 0)
                {
                    UploadPhoto(client.Authorization, Global.LisCoverPhotoLink[0]);
                    Global.LisCoverPhotoLink.RemoveAt(0);
                    exData.LastUpLoadPhoto = DateTime.Now;
                }
            }
            #endregion
            #region - Update Profile Photo -
            System.Threading.Thread.Sleep(23);
            if (!exData.LastUpdateProfilePhoto.HasValue || exData.LastUpdateProfilePhoto.Value.Date.AddDays(new Random().Next(5, 10)) < DateTime.Today)
            {
                if (Global.ListProfilePhotoLink.Count > 0)
                {
                    string id = UploadPhoto(client.Authorization, Global.ListProfilePhotoLink[0]);
                    Global.ListProfilePhotoLink.RemoveAt(0);
                    if (!string.IsNullOrEmpty(id))
                    {
                        param = new NameValueCollection();
                        param.Add("batch", "[{\"method\":\"POST\",\"body\":\"qn=" + hash_id
                     + "&time_since_original_post=3&scaled_crop_rect=%7B%22y%22%3A0%2C%22height%22%3A1%2C%22width%22%3A1%2C%22x%22%3A0%7D&locale=vi_VN&client_country_code=VN&fb_api_req_friendly_name=publish-photo\",\"name\":\"publish\",\"omit_response_on_success\":false,"
                     + "\"relative_url\":\"" + model.FBID + "/picture/" + id + "\"}]");
                        param.Add("fb_api_caller_class", "com.facebook.photos.upload.protocol.PhotoPublisher");
                        client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=vi_VN&client_country_code=VN");
                        exData.LastUpdateProfilePhoto = DateTime.Now;
                    }
                }
            }
            #endregion
            #region - Update Cover Photo -
            System.Threading.Thread.Sleep(23);
            if (!exData.LastUpdateCoverPhoto.HasValue || exData.LastUpdateCoverPhoto.Value.Date.AddDays(new Random().Next(5, 10)) < DateTime.Today)
            {
                if (Global.LisCoverPhotoLink.Count > 0)
                {
                    string id = UploadPhoto(client.Authorization, Global.LisCoverPhotoLink[Global.LisCoverPhotoLink.Count - 1]);
                    Global.LisCoverPhotoLink.RemoveAt(Global.LisCoverPhotoLink.Count - 1);
                    if (!string.IsNullOrEmpty(id))
                    {
                        param = new NameValueCollection();
                        param.Add("batch", "[{\"method\":\"POST\",\"body\":\"qn=" + hash_id
                         + "&time_since_original_post=12&photo=" + id + "&focus_y=0&locale=vi_VN&client_country_code=VN&fb_api_req_friendly_name=publish-photo\",\"name\":\"publish\",\"omit_response_on_success\":false,"
                         + "\"relative_url\":\"" + model.FBID + "/cover\"}]");
                        param.Add("fb_api_caller_class", "com.facebook.photos.upload.protocol.PhotoPublisher");
                        client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=vi_VN&client_country_code=VN");
                        exData.LastUpdateCoverPhoto = DateTime.Now;
                    }
                }
            }
            #endregion
            #region - Like -
            if (listPost.Count > 0)
            {
                System.Threading.Thread.Sleep(23);
                int iRD = new Random().Next(0, 4);
                for (int iIndex = 0; iIndex < iRD; iIndex++)
                {
                    if (listPost.Count > iIndex)
                    {
                        string strPostID = string.Empty;
                        try
                        {
                            strPostID = (((listPost[iIndex] as Dictionary<string, object>)["node"] as Dictionary<string, object>)["feedback"] as Dictionary<string, object>)["legacy_api_post_id"].ToString();
                        }
                        catch { }
                        if (!string.IsNullOrEmpty(strPostID))
                        {
                            Like(model, strPostID);
                            //System.Threading.Thread.Sleep(1000);
                        }
                    }
                }
            }
            #endregion
            #region - Comment -
            if (listPost.Count > 0)
            {
                System.Threading.Thread.Sleep(23);
                int iRD = new Random().Next(0, 4);
                for (int iIndex = 0; iIndex < iRD; iIndex++)
                {
                    if (listPost.Count > iIndex)
                    {
                        string strPostID = string.Empty;
                        try
                        {
                            strPostID = (((listPost[iIndex] as Dictionary<string, object>)["node"] as Dictionary<string, object>)["feedback"] as Dictionary<string, object>)["legacy_api_post_id"].ToString();
                        }
                        catch { }
                        if (!string.IsNullOrEmpty(strPostID))
                        {
                            Comment(model, strPostID);
                            //System.Threading.Thread.Sleep(1000);
                        }
                    }
                }
            }
            #endregion
            #region - Make Friend -
            {
                System.Threading.Thread.Sleep(23);
                int iRD = new Random().Next(0, 3);
                //Select in Package
                List<FaceBook> listFB = getFaceBookNotFriend(model, true, iRD);
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
                listFB = getFaceBookNotFriend(model, false, iRD);
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
            SetExtraData(model, exData);
            Global.DBContext.SaveChanges();
            return true;
        }

        private List<FaceBook> getFaceBookNotFriend(FaceBook model, bool bInpackage, int iNo)
        {
            if (bInpackage)
            {
                return (from fb in Global.DBContext.FaceBook
                        where fb.ID != model.ID
                        && fb.FBPackageID == model.FBPackageID
                        && !(from o in Global.DBContext.FBFriend
                             where o.FBID2 == model.ID
                             select o.FBID1)
                               .Contains(fb.ID)
                       && !(from o in Global.DBContext.FBFriend
                            where o.FBID1 == model.ID
                            select o.FBID2)
                       .Contains(fb.ID)
                        select fb).ToList();
            }
            else
            {
                return (from fb in Global.DBContext.FaceBook
                        where fb.ID != model.ID
                        && !(from o in Global.DBContext.FBFriend
                             where o.FBID2 == model.ID
                             select o.FBID1)
                               .Contains(fb.ID)
                       && !(from o in Global.DBContext.FBFriend
                            where o.FBID1 == model.ID
                            select o.FBID2)
                       .Contains(fb.ID)
                        select fb).Take(iNo).ToList();
            }
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
                param.Add("locale", "vi_VN");
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
                param.Add("locale", "vi_VN");
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

        public void FeedAccessToken(FaceBook model)
        {
            try
            {
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                client.Authorization = dicData["access_token"].ToString();
                NameValueCollection param = new NameValueCollection();
                param.Add("batch", "[{'method':'GET','name':'prefetchAccessToken','omit_response_on_success':false,'relative_url':'fql?format=json&q=%7B%22token_query%22%3A%22SELECT+page_id%2C+name%2C+access_token+FROM+page+WHERE+page_id+IN+%28SELECT+page_id+FROM+%23page_query%29%22%2C%22page_query%22%3A%22SELECT+page_id+FROM+page_admin+WHERE+uid+%3D+me%28%29+AND+type+%21%3D+%27APPLICATION%27+ORDER+BY+last_used_time+DESC%22%7D&locale=en_US&client_country_code=VN&fb_api_req_friendly_name=page_access_token'}]".Replace("'", "\""));
                param.Add("fb_api_caller_class", "com.facebook.feed.server.NewsFeedServiceImplementation");
                param.Add("flush", "1");
                client.DoPost(param, "https://graph.facebook.com/?include_headers=false&decode_body_json=false&streamable_json_response=true&locale=vi_VN&client_country_code=VN");
                if (client.Error == null && !string.IsNullOrEmpty(client.ResponseText))
                {
                    dicData = new JavaScriptSerializer { MaxJsonLength = int.MaxValue }.Deserialize<Dictionary<string, object>>(client.ResponseText);
                    foreach (Dictionary<string, object> pagefeed in ((((((dicData["prefetchAccessToken"] as ArrayList)[1] as Dictionary<string, object>)["body"] as Dictionary<string, object>)["data"] as ArrayList)[1] as Dictionary<string, object>)["fql_result_set"] as ArrayList))
                    {
                        Page page = getFBByID(pagefeed["page_id"].ToString());
                        if (page == null)
                        {
                            page = new Page();
                            Global.DBContext.Page.Add(page);
                        }
                        page.AccessToken = pagefeed["access_token"].ToString();
                        page.FaceBookID = model.ID;
                        page.PageID = pagefeed["page_id"].ToString();
                        page.PageName = pagefeed["name"].ToString();
                    }
                    Global.DBContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void RenameToUS(FaceBook model)
        {
            try
            {
                FBExtraData extraData = GetExtraData(model);
                if (extraData != null && extraData.ProfileUS.HasValue && extraData.ProfileUS.Value) return;
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                if (dicData != null && dicData.ContainsKey("session_cookies"))
                {
                    WebClientEx client = new WebClientEx();
                    client.CookieContainer = new CookieContainer();
                    foreach (Dictionary<string, object> dicCookie in dicData["session_cookies"] as ArrayList)
                    {
                        client.CookieContainer.Add(new Cookie(dicCookie["name"].ToString(), dicCookie["value"].ToString()
                            , dicCookie["path"].ToString(), dicCookie["domain"].ToString()));
                    }
                    foreach (Page page in model.Pages)
                    {
                        client.DoGet("https://m.facebook.com/pages/edit/info/" + page.PageID);
                        NameValueCollection param = new NameValueCollection();
                        string fb_dtsg = Regex.Match(client.ResponseText, "\"token\":\"(?<val>[^\"]+)").Groups["val"].Value;
                        if (!string.IsNullOrEmpty(fb_dtsg))
                        {
                            page.PageName = Utilities.GetMaleName(true);
                            param.Add("fb_dtsg", fb_dtsg);
                            param.Add("charset_test", "€,´,€,´,水,Д,Є");
                            param.Add(page.PageID + ":name", page.PageName);
                            param.Add("m_sess", "");
                            param.Add("__dyn", "");
                            param.Add("__req", "1");
                            param.Add("__ajax__", "true");
                            param.Add("__user", model.FBID);
                            client.DoPost(param, "https://m.facebook.com/a/pages/edit/info/" + page.PageID);
                        }
                    }
                    extraData.ProfileUS = true;
                    SetExtraData(model, extraData);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void UpdatePhotoAndCover(Page model)
        {
            PageData pageData = new JavaScriptSerializer { MaxJsonLength = int.MaxValue }.Deserialize<PageData>(model.PageData == null ? "" : model.PageData);
            if (pageData == null) pageData = new PageData();
            if (pageData.LUCP.HasValue) return;
            WebClientEx client = new WebClientEx();
            client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
            client.Authorization = model.AccessToken;
            NameValueCollection param = new NameValueCollection();
            #region - Upload profile picture -

            if (Global.ListProfilePhotoLink.Count > 0)
            {
                string id = UploadPhoto(model.AccessToken, Global.ListProfilePhotoLink[0]);
                Global.ListProfilePhotoLink.RemoveAt(0);
                if (!string.IsNullOrEmpty(id))
                {
                    param = new NameValueCollection();
                    string hash_id = Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString());
                    hash_id = hash_id.Substring(0, 8) + "-" + hash_id.Substring(8, 4) + "-" + hash_id.Substring(12, 4) + "-" + hash_id.Substring(16, 4) + "-" + hash_id.Substring(20, 12);

                    param.Add("batch", "[{\"method\":\"POST\",\"body\":\"qn=" + hash_id
                 + "&scaled_crop_rect=%7B%22y%22%3A0%2C%22height%22%3A1%2C%22width%22%3A1%2C%22x%22%3A0%7D&locale=vi_VN&client_country_code=VN&fb_api_req_friendly_name=publish-photo\",\"name\":\"publish\",\"omit_response_on_success\":false,"
                 + "\"relative_url\":\"" + model.PageID + "/picture/" + id + "\"}]");
                    param.Add("fb_api_caller_class", "com.facebook.photos.upload.protocol.PhotoPublisher");
                    client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=vi_VN&client_country_code=VN");
                }
            }
            #endregion

            #region - update cover photo -
            if (Global.LisCoverPhotoLink.Count > 0)
            {
                string id = UploadPhoto(client.Authorization, Global.LisCoverPhotoLink[Global.LisCoverPhotoLink.Count - 1]);
                Global.LisCoverPhotoLink.RemoveAt(Global.LisCoverPhotoLink.Count - 1);

                if (!string.IsNullOrEmpty(id))
                {
                    param = new NameValueCollection();
                    string hash_id = Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString());
                    hash_id = hash_id.Substring(0, 8) + "-" + hash_id.Substring(8, 4) + "-" + hash_id.Substring(12, 4) + "-" + hash_id.Substring(16, 4) + "-" + hash_id.Substring(20, 12);

                    param.Add("batch", "[{\"method\":\"POST\",\"body\":\"qn=" + hash_id
                     + "&photo=" + id + "&focus_y=0&locale=vi_VN&client_country_code=VN&fb_api_req_friendly_name=publish-photo\",\"name\":\"publish\",\"omit_response_on_success\":false,"
                     + "\"relative_url\":\"" + model.PageID + "/cover\"}]");
                    param.Add("fb_api_caller_class", "com.facebook.photos.upload.protocol.PhotoPublisher");
                    client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=vi_VN&client_country_code=VN");
                }
            }
            #endregion

            pageData.LUCP = DateTime.Now;
            pageData.LUPP = DateTime.Now;
            model.PageData = new JavaScriptSerializer { MaxJsonLength = int.MaxValue }.Serialize(pageData);
        }

        public void UpdatePhotoAndCover(FaceBook model)
        {
            foreach (Page page in model.Pages)
            {
                UpdatePhotoAndCover(page);
            }
            if (Global.DBContext.ChangeTracker.HasChanges())
            {
                Global.DBContext.SaveChanges();
            }
        }

        private Page getFBByID(string PageID)
        {
            return Global.DBContext.Page.Where(page => page.PageID == PageID).FirstOrDefault();
        }

        public void CreateNewPage(FaceBook model)
        {
            FBExtraData extraData = GetExtraData(model);
            if (extraData.BlockCreatePage.HasValue && extraData.BlockCreatePage.Value) return;
            if (model.Pages.Count >= 300) return;
            int iCount = 0;
            while (iCount < 500)
            {

                WebClientEx client = new WebClientEx();
                client.DoGet(GetFaceBookLoginURL(model, "https://m.facebook.com/pages/create"));
                NameValueCollection param = new NameValueCollection();
                List<string> listSub = new List<string>() { "1103", "1601", "1600", "1301", "1609", "1606", "1802", "1610", "1614", "1615", "1611", "1617", "1113", "1701", "1604", "1114", "1202", "1605", "2632", "1616", "1700", "1108", "1602", "1613", "1109" };

                string fb_dtsg = Regex.Match(client.ResponseText, "\"token\":\"(?<val>[^\"]+)").Groups["val"].Value;

                Page page = new Page();
                page.FaceBookID = model.ID;
                page.PageName = Utilities.GetMaleName(false);

                param.Add("fb_dtsg", fb_dtsg);
                param.Add("charset_test", "€,´,€,´,水,Д,Є");
                param.Add("page_name", page.PageName);
                param.Add("super_category", "1007");
                param.Add("category", listSub[new Random().Next(0, listSub.Count - 1)]);
                param.Add("m_sess", "");
                param.Add("__dyn", "");
                param.Add("__req", "2");
                param.Add("__ajax__", "true");
                param.Add("__user", model.FBID);
                client.DoPost(param, "https://m.facebook.com/pages/create/add/");
                if (client.Error == null && !string.IsNullOrEmpty(client.ResponseText))
                {
                    //if (client.ResponseText.Contains("Please slow down, or you could be blocked from using it")) return;
                    string strpageID = Regex.Match(client.ResponseText, @"getting_started\.php\?id=(?<val>[0-9]+)").Groups["val"].Value;
                    if (!string.IsNullOrEmpty(strpageID))
                    {
                        //return;
                        //page.PageID = strpageID;
                        //Global.DBContext.Page.Add(page);
                        //int iBeginWait = 0;
                        //while (Global.ContextBusy && iBeginWait < 10)
                        //{
                        //    System.Threading.Thread.Sleep(new Random().Next(200, 1000));
                        //    iBeginWait++;
                        //}
                        //Global.ContextBusy = true;
                        //Global.DBContext.SaveChanges();
                        //Global.ContextBusy = false;
                        //iCount++;
                    }
                    else if (client.ResponseText.Contains(@"khi s\u1eed d\u1ee5ng qu\u00e1 nhanh") || client.ResponseText.Contains(@"You\u2019ve been blocked from using it"))
                    {
                        extraData.BlockCreatePage = true;
                        SetExtraData(model, extraData);
                        return;
                    }
                    else
                    {
                        return;
                    }
                    //else return;
                }
                System.Threading.Thread.Sleep(5000);
            }
        }

        public FaceBook RegNewAccount()
        {
            Debug.WriteLine("-- Start INNER Reg --");
            FaceBook model = null;
            FBExtraData extraData = new FBExtraData();

            WebClientEx client = new WebClientEx();
            client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;

            NameValueCollection param = new NameValueCollection();

            string hash_id = Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString());
            hash_id = hash_id.Substring(0, 8) + "-" + hash_id.Substring(8, 4) + "-" + hash_id.Substring(12, 4) + "-" + hash_id.Substring(16, 4) + "-" + hash_id.Substring(20, 12);
            string sig = Utilities.getSignFB(param, AppSettings.api_secret);

            string strName = Utilities.GetMaleName(false);
            param = new NameValueCollection();
            param.Add("api_key", AppSettings.api_key);
            param.Add("attempt_login", "true");
            {
                System.Threading.Thread.Sleep(23);
                int iY = new Random().Next(1979, 1990);
                System.Threading.Thread.Sleep(23);
                int iM = new Random().Next(1, 12);
                System.Threading.Thread.Sleep(23);
                int iD = new Random(3).Next(1, 28);
                extraData.BirthDay = new DateTime(iY, iM, iD);
            }
            param.Add("birthday", extraData.BirthDay.Value.ToString("yyyy-MM-dd"));
            param.Add("client_country_code", "VN");

            param.Add("fb_api_caller_class", "com.facebook.registration.protocol.RegisterAccountMethod");
            param.Add("fb_api_req_friendly_name", "registerAccount");
            param.Add("firstname", strName.Trim().Split(' ')[0]);
            System.Threading.Thread.Sleep(34);
            param.Add("format", "json");
            param.Add("gender", "M");
            param.Add("lastname", strName.Trim().Split(' ')[1]);
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
            param.Add("email", Utilities.ConvertToUnSign3(param["firstname"].Replace(" ", "").Trim() + param["lastname"].Replace(" ", "").Trim() + new Random().Next(10, 9999)).ToLower() + "@" + listEmail[new Random().Next(0, listEmail.Count - 1)]);
            param.Add("locale", "vi_VN");
            param.Add("method", "user.register"); string strPass = Utilities.ConvertToUnSign3(param["firstname"].Split(' ')[0] + new List<string> { "!", "@", "#", "$", "%", "^", "~", "&", "*", "(" }[new Random().Next(0, 10)] + param["lastname"].Split(' ')[0] + new Random().Next(10, 1000));
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
                            string id = UploadPhoto(client.Authorization, Global.ListProfilePhotoLink[0]);
                            Global.ListProfilePhotoLink.RemoveAt(0);
                            if (!string.IsNullOrEmpty(id))
                            {
                                param = new NameValueCollection();

                                param.Add("batch", "[{\"method\":\"POST\",\"body\":\"qn=" + hash_id
                             + "&scaled_crop_rect=%7B%22y%22%3A0%2C%22height%22%3A1%2C%22width%22%3A1%2C%22x%22%3A0%7D&locale=vi_VN&client_country_code=VN&fb_api_req_friendly_name=publish-photo\",\"name\":\"publish\",\"omit_response_on_success\":false,"
                             + "\"relative_url\":\"" + model.FBID + "/picture/" + id + "\"}]");
                                param.Add("fb_api_caller_class", "com.facebook.photos.upload.protocol.PhotoPublisher");
                                client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=vi_VN&client_country_code=VN");
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
                        param.Add("locale", "vi_VN");
                        param.Add("client_country_code", "VN");
                        param.Add("method", "user.updateNuxStatus");
                        param.Add("fb_api_req_friendly_name", "updateNuxStatus");
                        param.Add("fb_api_caller_class", "com.facebook.nux.status.UpdateNuxStatusMethod");
                        //System.Threading.Thread.Sleep(1000);
                        client.DoPost(param, "https://api.facebook.com/method/user.updateNuxStatus");

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
                        extraData.UpdatedProfileInfo = true;
                        #endregion
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
                        //System.Threading.Thread.Sleep(1000);
                        client.DoPost(param, "https://api.facebook.com/method/user.updateNuxStatus");
                        if (client.ResponseText.ToLower().Trim() != "true") return null;
                        param = new NameValueCollection();
                        param.Add("format", "JSON");
                        param.Add("nux_id", "ANDROID_NEW_ACCOUNT_WIZARD");
                        param.Add("step", "contact_importer");
                        param.Add("status", "COMPLETE");
                        param.Add("extra_data", "{}");
                        param.Add("locale", "vi_VN");
                        param.Add("client_country_code", "VN");
                        param.Add("method", "user.updateNuxStatus");
                        param.Add("fb_api_req_friendly_name", "updateNuxStatus");
                        param.Add("fb_api_caller_class", "com.facebook.nux.status.UpdateNuxStatusMethod");
                        //System.Threading.Thread.Sleep(1000);
                        client.DoPost(param, "https://api.facebook.com/method/user.updateNuxStatus");
                        //string strNewEmail = new Random().Next(1, 999) % 2 == 0 ? "@vuathuthanh.net" : "@tinphuong.com";
                        string strNewEmail = "@tinphuong.com";
                        strNewEmail = model.Login.Substring(0, model.Login.IndexOf("@")) + strNewEmail;
                        param = new NameValueCollection();
                        param.Add("add_contactpoint", strNewEmail);
                        param.Add("add_contactpoint_type", "EMAIL");
                        param.Add("format", "json");
                        param.Add("locale", "vi_VN");
                        param.Add("client_country_code", "VN");
                        param.Add("fb_api_req_friendly_name", "editRegistrationContactpoint");
                        param.Add("method", "user.editregistrationcontactpoint");
                        param.Add("fb_api_caller_class", "com.facebook.confirmation.protocol.EditRegistrationContactpointMethod");

                        #region - confirm email -
                        client.DoPost(param, "https://api.facebook.com/method/user.editregistrationcontactpoint");
                        if (!string.IsNullOrEmpty(client.ResponseText) && client.ResponseText == "true")
                        {
                            string strCode = Utilities.GetConfirmCode(strNewEmail);
                            if (!string.IsNullOrEmpty(strCode))
                            {
                                param = new NameValueCollection();
                                param.Add("normalized_contactpoint", strNewEmail);
                                param.Add("contactpoint_type", "EMAIL");
                                param.Add("code", strCode);
                                param.Add("source", "ANDROID_DIALOG_API");
                                param.Add("format", "json");
                                param.Add("locale", "vi_VN");
                                param.Add("client_country_code", "VN");
                                param.Add("fb_api_req_friendly_name", "confirmContactpoint");
                                param.Add("method", "user.confirmcontactpoint");
                                param.Add("fb_api_caller_class", "com.facebook.confirmation.protocol.ConfirmContactpointMethod");
                                client.DoPost(param, "https://api.facebook.com/method/user.confirmcontactpoint");
                                model.Login = strNewEmail;
                                extraData.ComfirmedEmail = true;
                            }
                        }
                        #endregion

                        #region - update cover photo -
                        if (Global.LisCoverPhotoLink.Count > 0)
                        {
                            string id = UploadPhoto(client.Authorization, Global.LisCoverPhotoLink[Global.LisCoverPhotoLink.Count - 1]);
                            Global.LisCoverPhotoLink.RemoveAt(Global.LisCoverPhotoLink.Count - 1);

                            if (!string.IsNullOrEmpty(id))
                            {
                                param = new NameValueCollection();
                                param.Add("batch", "[{\"method\":\"POST\",\"body\":\"qn=" + hash_id
                                 + "&photo=" + id + "&focus_y=0&locale=vi_VN&client_country_code=VN&fb_api_req_friendly_name=publish-photo\",\"name\":\"publish\",\"omit_response_on_success\":false,"
                                 + "\"relative_url\":\"" + model.FBID + "/cover\"}]");
                                param.Add("fb_api_caller_class", "com.facebook.photos.upload.protocol.PhotoPublisher");
                                client.DoPost(param, "https://graph.facebook.com/?include_headers=false&locale=vi_VN&client_country_code=VN");
                                extraData.LastUpdateCoverPhoto = DateTime.Now;
                            }
                        }
                        #endregion
                        //Like(model, "639987956113732");
                        Debug.WriteLine("-- SetExtraData --");
                        SetExtraData(model, extraData);
                        Debug.WriteLine("-- End INNER Reg --");
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
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                int iRD = new Random().Next(1, 4500);
                string strStatus = Global.DBContext.StatusData.Where(m => m.ID == iRD).FirstOrDefault().Text;
                //strStatus = HttpUtility.UrlEncode(strStatus);
                NameValueCollection param = new NameValueCollection();
                param.Add("access_token", dicData["access_token"].ToString());
                param.Add("format", "json");
                param.Add("method", "post");
                param.Add("pretty", "0");
                param.Add("suppress_http_code", "1");
                param.Add("message", strStatus);
                WebClientEx clientAPI = new WebClientEx();
                clientAPI.DoPost(param, "https://graph.facebook.com/v2.1/me/feed");
                if (clientAPI.Error == null)
                {
                    return true;
                }
                else if (!string.IsNullOrEmpty(clientAPI.ResponseText) && clientAPI.ResponseText.Contains("\"error\""))
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

        public string UploadPhoto(string access_token, string strLink)
        {
            try
            {
                if (!string.IsNullOrEmpty(strLink))
                {
                    NameValueCollection param = new NameValueCollection();
                    param.Add("access_token", access_token);
                    param.Add("format", "json");
                    param.Add("method", "post");
                    param.Add("pretty", "0");
                    param.Add("suppress_http_code", "1");
                    param.Add("url", strLink);
                    WebClientEx clientAPI = new WebClientEx();
                    clientAPI.DoPost(param, "https://graph.facebook.com/v2.1/me/photos");
                    if (!string.IsNullOrEmpty(clientAPI.ResponseText))
                    {
                        Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(clientAPI.ResponseText);
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
                param.Add("locale", "vi_VN");
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
                                    param.Add("locale", "vi_VN");
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

        public void Like(string access_token, string id)
        {
            try
            {
                NameValueCollection param = new NameValueCollection();
                param.Add("access_token", access_token);
                param.Add("format", "json");
                param.Add("method", "post");
                param.Add("pretty", "0");
                param.Add("suppress_http_code", "1");
                WebClientEx clientAPI = new WebClientEx();
                clientAPI.DoPost(param, "https://graph.facebook.com/v2.1/" + id + "/likes");
            }
            catch { }
        }

        public void Share(Page page, string id, string strMessage)
        {
            try
            {
                string hash_id = Utilities.GetMd5Hash(DateTime.Now.Ticks.ToString());
                hash_id = hash_id.Substring(0, 8) + "-" + hash_id.Substring(8, 4) + "-" + hash_id.Substring(12, 4) + "-" + hash_id.Substring(16, 4) + "-" + hash_id.Substring(20, 12);
                WebClientEx clientEx = new WebClientEx();
                clientEx.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                clientEx.Authorization = page.AccessToken;
                NameValueCollection param = new NameValueCollection();
                
                param.Add("batch", ("[{'method':'POST','body':'message=" + strMessage + "&nectar_module=pages_identity_ufi&composer_session_id=" + hash_id + "&qn=" + hash_id + "&idempotence_token=" + hash_id + "&audience_exp=true&attach_place_suggestion=true&format=json&id=" + id + "&to=" + page.PageID + "&fb_api_req_friendly_name=graphObjectShare','name':'graphObjectShares','omit_response_on_success':false,'relative_url':'sharedposts'}]").Replace("'", "\""));
                param.Add("fb_api_caller_class", "com.facebook.composer.publish.ComposerPublishServiceHandler");
                clientEx.DoPost(param, "https://graph.facebook.com/?include_headers=false&decode_body_json=false&streamable_json_response=true&locale=vi_VN&client_country_code=VN");
                if (clientEx.Error == null && !string.IsNullOrEmpty(clientEx.ResponseText))
                {
                }
            }
            catch { }
        }

        public void Comment(FaceBook model, string id)
        {
            try
            {
                Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.MBLoginText);
                Random r = new Random();
                NameValueCollection param = new NameValueCollection();
                param.Add("access_token", dicData["access_token"].ToString());
                param.Add("format", "json");
                param.Add("method", "post");
                param.Add("pretty", "0");
                param.Add("suppress_http_code", "1");
                int iRD = new Random().Next(1, 4500);
                string strStatus = Global.DBContext.StatusData.Where(m => m.ID == iRD).FirstOrDefault().Text;
                param.Add("message", strStatus);
                WebClientEx clientAPI = new WebClientEx();
                clientAPI.DoPost(param, "https://graph.facebook.com/v2.1/" + id + "/comments");
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
