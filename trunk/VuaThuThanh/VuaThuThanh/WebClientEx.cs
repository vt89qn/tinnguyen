using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Configuration;

public class WebClientEx : WebClient
{
    #region - DECLARE -
    public Exception Error = null;
    public CookieContainer CookieContainer = new CookieContainer();
    public bool AllowAutoRedirect = true;
    public WebRequest Request { get; set; }
    public WebResponse Response { get; set; }
    public string ResponseText { get; set; }
    public Image ResponseImage { get; set; }
    private string strUserAgent = "ah3q-appstore/1.26 (iPhone; iOS 7.1.1; Scale/2.00)";
    #endregion
    #region - METHOD -
    public WebClientEx()
    {
        this.Encoding = Encoding.UTF8;
        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
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
        catch (Exception ex)
        {
            this.Error = ex;
        }
        return Response;
    }
    public WebClientEx Copy()
    {
        return (WebClientEx)this.MemberwiseClone();
    }
    public string DoPost(NameValueCollection parameters, string strURL, Dictionary<HttpRequestHeader, string> additionHeader)
    {
        try
        {
            Error = null;
            ResponseText = null;
            this.Headers.Clear();
            this.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            this.Headers.Add(HttpRequestHeader.UserAgent, strUserAgent);
            this.Headers.Add(HttpRequestHeader.Accept, "*/*");
            this.Headers.Add(HttpRequestHeader.AcceptLanguage, "vi, en, en-us;q=0.8");
            if (additionHeader != null)
            {
                foreach (KeyValuePair<HttpRequestHeader, string> item in additionHeader)
                {
                    this.Headers.Add(item.Key, item.Value);
                }
            }

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
    public string DoPost(string parameters, string strURL, Dictionary<HttpRequestHeader, string> additionHeader)
    {
        try
        {
            Error = null;
            ResponseText = null;
            this.Headers.Clear();
            this.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            this.Headers.Add(HttpRequestHeader.UserAgent, strUserAgent);
            this.Headers.Add(HttpRequestHeader.Accept, "*/*");
            this.Headers.Add(HttpRequestHeader.AcceptLanguage, "vi, en, en-us;q=0.8");
            if (additionHeader != null)
            {
                foreach (KeyValuePair<HttpRequestHeader, string> item in additionHeader)
                {
                    this.Headers.Add(item.Key, item.Value);
                }
            }
            ResponseText = this.UploadString(strURL, parameters);
        }
        catch (Exception ex)
        {
            this.Error = ex;
        }
        return ResponseText;
    }
    public string DoPost(string parameters, string strURL)
    {
        return this.DoPost(parameters, strURL, null);
    }
    public string DoGet(string strURL, Dictionary<HttpRequestHeader, string> additionHeader)
    {
        try
        {
            Error = null;
            ResponseText = string.Empty;
            this.Headers.Clear();
            this.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            this.Headers.Add(HttpRequestHeader.UserAgent, strUserAgent);
            this.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //this.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            this.Headers.Add(HttpRequestHeader.AcceptLanguage, "vi-VN,vi;q=0.8,fr-FR;q=0.6,fr;q=0.4,en-US;q=0.2,en;q=0.2");
            this.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            if (additionHeader != null)
            {
                foreach (KeyValuePair<HttpRequestHeader, string> item in additionHeader)
                {
                    this.Headers.Add(item.Key, item.Value);
                }
            }
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
    public Image GetImage(string strURL)
    {
        try
        {
            Error = null;
            ResponseImage = null;
            this.Headers.Clear();
            this.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            this.Headers.Add(HttpRequestHeader.UserAgent, strUserAgent);
            this.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //this.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            this.Headers.Add(HttpRequestHeader.AcceptLanguage, "vi-VN,vi;q=0.8,fr-FR;q=0.6,fr;q=0.4,en-US;q=0.2,en;q=0.2");
            this.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            byte[] byteArrayIn = this.DownloadData(strURL);
            MemoryStream ms = new MemoryStream(byteArrayIn);
            ResponseImage = Image.FromStream(ms);
        }
        catch (Exception ex)
        {
            this.Error = ex;
        }
        return ResponseImage;
    }
    #endregion
}