using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

class FaceBook
{
    #region - DECLARE -
    WebClientEx client = new WebClientEx();
    LoginContain login = new LoginContain();
    string strFaceBookAccountID = string.Empty;
    Exception error = null;
    #endregion
    #region - PROPERTIES -
    public Exception Error
    {
        get { return this.error; }
    }
    public string FBAccountID
    {
        get { return this.strFaceBookAccountID; }
    }
    public LoginContain LoginInfo
    {
        set { this.login = value; }
        get { return this.login; }
    }
    public CookieContainer StoreCookie
    {
        set { this.client.CookieContainer = value; }
        get { return this.client.CookieContainer; }
    }
    #endregion
    #region - CONTRUCTOR -
    public FaceBook()
    {//Do nothing !
    }
    public FaceBook(LoginContain login)
    {
        this.login = login;
    }
    #endregion
    #region - METHOD -
    public bool Login()
    {
        try
        {
            NameValueCollection param = new NameValueCollection();
            client.DoGet("https://www.facebook.com/login.php");
            if (!string.IsNullOrEmpty(client.ResponseText))
            {
                if (client.ResponseText.Contains("logout.php") && !client.ResponseText.Contains("login.php"))
                {
                    return true;
                }
                param.Add("email", login.UserName);
                param.Add("pass", login.PassWord);
                client.AllowAutoRedirect = false;
                client.DoPost(param, "https://www.facebook.com/login.php");
                client.AllowAutoRedirect = true;
                string strLocation = client.ResponseHeaders[HttpResponseHeader.Location];
                if (strLocation != null && strLocation.Contains("login.php"))
                {
                    return false;
                }
                client.AllowAutoRedirect = false;
                client.DoGet("https://www.facebook.com/me/about");
                client.AllowAutoRedirect = true;
                strLocation = client.ResponseHeaders[HttpResponseHeader.Location];
                if (!string.IsNullOrEmpty(strLocation) && !strLocation.Contains("login.php"))
                {
                    this.strFaceBookAccountID = string.Empty;
                    foreach (Cookie cookie in client.CookieContainer.GetCookies(new Uri("https://www.facebook.com/")))
                    {
                        if (cookie.Name == "c_user")
                        {
                            this.strFaceBookAccountID = cookie.Value;
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(strFaceBookAccountID))
                    {
                        return true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            this.error = ex;
        }
        return false;
    }
    #endregion
}
