using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using PokerTexas.App_Common;

namespace PokerTexas.App_UserControl
{
    public class WebClientEx : WebClient
    {
        public enum RequestTypeEnum
        {
            Poker,
            FaceBook,
            Nomal
        }
        #region - DECLARE -
        public Exception Error = null;
        public CookieContainer CookieContainer = new CookieContainer();
        public bool AllowAutoRedirect = true;
        public WebRequest Request { get; set; }
        public WebResponse Response { get; set; }
        public string ResponseText { get; set; }
        public Image ResponseImage { get; set; }
        public string UserAgent = "Instagram 6.5.0 Android (17/4.2.2; 240dpi; 480x800; samsung; SGH-T959; SGH-T959; aries; en_US)";
        public string X_TUNNEL_VERIFY = string.Empty;
        public string Authorization = string.Empty;
        public RequestTypeEnum RequestType = RequestTypeEnum.Nomal;
        public bool SetAPIV8 = false;
        #endregion

        #region - METHOD -
        public WebClientEx()
        {
            this.Encoding = Encoding.UTF8;
            //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
        }
        protected override WebRequest GetWebRequest(Uri address)
        {
            try
            {
                Request = base.GetWebRequest(address);
                if (Request is HttpWebRequest)
                {
                    (Request as HttpWebRequest).CookieContainer = CookieContainer;
                    (Request as HttpWebRequest).AllowAutoRedirect = AllowAutoRedirect;
                }
            }
            catch (Exception ex)
            {
                this.Error = ex;
            }
            return Request;
        }
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            try
            {
                Response = base.GetWebResponse(request);
            }
            catch (WebException ex)
            {
                this.Error = ex;
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    ResponseText = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                this.Error = ex;
            }
            return Response;
        }

        public string DoPost(NameValueCollection parameters, string strURL, Dictionary<HttpRequestHeader, string> additionHeader)
        {
            try
            {
                Error = null;
                ResponseText = null;
                addHeader(additionHeader);
                ResponseText = Encoding.Default.GetString(this.UploadValues(strURL, parameters));
            }
            catch (Exception ex)
            {
                this.Error = ex;
            }
            return ResponseText;
        }
        public string DoPost(NameValueCollection parameters, string strURL)
        {
            return this.DoPost(parameters, strURL, null);
        }

        public string DoPost(string parameters, string strURL, Dictionary<HttpRequestHeader, string> additionHeader, string Method)
        {
            try
            {
                Error = null;
                ResponseText = null;
                addHeader(additionHeader);
                ResponseText = this.UploadString(strURL, Method, parameters);
            }
            catch (Exception ex)
            {
                this.Error = ex;
            }
            return ResponseText;
        }
        public string DoPost(string parameters, string strURL)
        {
            return this.DoPost(parameters, strURL, null, "POST");
        }

        public string DoGet(string strURL, Dictionary<HttpRequestHeader, string> additionHeader)
        {
            try
            {
                Error = null;
                ResponseText = string.Empty;
                addHeader(additionHeader);
                ResponseText = this.DownloadString(strURL);
            }
            catch (Exception ex)
            {
                this.Error = ex;
            }
            return ResponseText;
        }
        public string DoGet(string strURL)
        {
            return this.DoGet(strURL, null);
        }

        private void addHeader(Dictionary<HttpRequestHeader, string> additionHeader)
        {
            this.Headers.Clear();

            if (RequestType == RequestTypeEnum.Poker)
            {
                if (!string.IsNullOrEmpty(X_TUNNEL_VERIFY))
                {
                    this.Headers.Add("X-TUNNEL-VERIFY", X_TUNNEL_VERIFY);
                }
                if (SetAPIV8)
                {
                    this.Headers.Add("api-v8", "1");
                }
            }
            else if (RequestType == RequestTypeEnum.FaceBook)
            {
                if (!string.IsNullOrEmpty(Authorization))
                {
                    this.Headers.Add("Authorization", "OAuth " + Authorization);
                }
                this.Headers.Add(HttpRequestHeader.UserAgent, AppSettings.UserAgentFaceBook);
            }
            else
            {
                this.Headers.Add(HttpRequestHeader.UserAgent, UserAgent);
            }
            if (additionHeader != null)
            {
                foreach (KeyValuePair<HttpRequestHeader, string> item in additionHeader)
                {
                    this.Headers.Add(item.Key, item.Value);
                }
            }
        }
        #endregion
    }
}