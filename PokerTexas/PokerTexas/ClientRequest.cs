using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Drawing;

public class ClientRequest
{
    #region - DECLARE -
    Exception error = null;
    #endregion
    #region - PROPERTIES -
    public Exception Error
    {
        get { return this.error; }
    }
    #endregion
    public string DoPost(WebClientEx webclient, NameValueCollection parameters, string strURL,Dictionary<HttpRequestHeader,string> additionHeader)
    {
        try
        {
            error = null;
            webclient.Headers.Clear();
            webclient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            webclient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36");                                          
            webclient.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //webclient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            webclient.Headers.Add(HttpRequestHeader.AcceptLanguage, "vi-VN,vi;q=0.8,fr-FR;q=0.6,fr;q=0.4,en-US;q=0.2,en;q=0.2");
            webclient.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            if (additionHeader != null)
            {
                foreach (KeyValuePair<HttpRequestHeader, string> item in additionHeader)
                {
                    webclient.Headers.Add(item.Key, item.Value);
                }
            }
            return Encoding.Default.GetString(webclient.UploadValues(strURL, parameters));
        }
        catch(Exception ex)
        {
            this.error = ex;
        }
        return null;
    }
    public string DoPost(WebClientEx webclient, NameValueCollection parameters, string strURL)
    {
        return this.DoPost(webclient, parameters, strURL, null);
    }
    public string DoPost(WebClientEx webclient, string parameters, string strURL, Dictionary<HttpRequestHeader, string> additionHeader)
    {
        try
        {
            error = null;
            webclient.Headers.Clear();
            webclient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            webclient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36");
            webclient.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //webclient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            webclient.Headers.Add(HttpRequestHeader.AcceptLanguage, "vi-VN,vi;q=0.8,fr-FR;q=0.6,fr;q=0.4,en-US;q=0.2,en;q=0.2");
            webclient.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            if (additionHeader != null)
            {
                foreach (KeyValuePair<HttpRequestHeader, string> item in additionHeader)
                {
                    webclient.Headers.Add(item.Key, item.Value);
                }
            }
            return webclient.UploadString(strURL, parameters);
        }
        catch (Exception ex)
        {
            this.error = ex;
        }
        return null;
    }
    public string DoPost(WebClientEx webclient, string parameters, string strURL)
    {
        return this.DoPost(webclient, parameters, strURL, null);
    }
    public string DoGet(WebClientEx webclient, string strURL, Dictionary<HttpRequestHeader, string> additionHeader)
    {
        try
        {
            error = null;
            webclient.Headers.Clear();
            webclient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            webclient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36");
            webclient.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //webclient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            webclient.Headers.Add(HttpRequestHeader.AcceptLanguage, "vi-VN,vi;q=0.8,fr-FR;q=0.6,fr;q=0.4,en-US;q=0.2,en;q=0.2");
            webclient.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            if (additionHeader != null)
            {
                foreach (KeyValuePair<HttpRequestHeader, string> item in additionHeader)
                {
                    webclient.Headers.Add(item.Key, item.Value);
                }
            }
            return webclient.DownloadString(strURL);
        }
        catch(Exception ex)
        {
            this.error = ex;
        }
        return null;
    }
    public string DoGet(WebClientEx webclient, string strURL)
    {
        return this.DoGet(webclient, strURL, null);
    }
    public Image GetImage(WebClientEx webclient, string strURL)
    {
        try
        {
            error = null;
            webclient.Headers.Clear();
            webclient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            webclient.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36");
            webclient.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //webclient.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,sdch");
            webclient.Headers.Add(HttpRequestHeader.AcceptLanguage, "vi-VN,vi;q=0.8,fr-FR;q=0.6,fr;q=0.4,en-US;q=0.2,en;q=0.2");
            webclient.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            byte[] byteArrayIn = webclient.DownloadData(strURL);
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        catch (Exception ex)
        {
            this.error = ex;
        }
        return null;
    }
}
