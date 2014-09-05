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

namespace PokerTexas.App_Controller
{
    public class FaceBookController
    {
        public bool LoginMobile(FaceBook model)
        {
            try
            {
                WebClientEx client = new WebClientEx();
                client.RequestType = WebClientEx.RequestTypeEnum.FaceBook;
                client.DoGet("https://developers.facebook.com/resources/dialog_descriptions_android.json");

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

        public string GetFaceBookLoginURL(FaceBook model,string strDestURl)
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
    }
}
