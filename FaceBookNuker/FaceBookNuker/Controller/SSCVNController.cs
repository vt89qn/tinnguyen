using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FaceBookNuker.Models;
using System.Collections.Specialized;

namespace FaceBookNuker.Controller
{
    public class SSCVNController
    {
        public WebClientEx Web { get; set; }

        public void GenName()
        {
            try
            {
                List<string> listMail = new List<string>() { "@gmail.com", "@outlook.com", "@zing.vn", "@live.com" };
                int iIndexMail = 0;
                for (int iIndex = 0; iIndex < 1000; iIndex++)
                {
                    Web = new WebClientEx();
                    Web.DoGet("http://ssc.vn/member.php?u=" + (230000 + iIndex));
                    //string strName = Regex.Match(Web.ResponseText, "<span class=\"member_username\"><font color=\"#1e6bbf\"><b>tritravinhtcn</b></font></span>", RegexOptions.IgnoreCase).Value;
                    string strName = Regex.Match(Web.ResponseText, "<span class=\"member_username\">[^/]+/", RegexOptions.IgnoreCase).Value;
                    strName = Regex.Match(strName, "<b>(?<1>[^<]+)").Groups[1].Value;
                    string strTest = Regex.Replace(strName, "[a-z0-9]", string.Empty).Trim();
                    if (strTest != string.Empty) continue;
                    strName = Regex.Replace(strName, "[^a-zA-Z0-9]", string.Empty).Trim();
                    int i = new Random().Next(10, 200);

                    if (!string.IsNullOrEmpty(strName))
                    {
                        strName = strName.ToLower() + i.ToString();

                        string strPass = strName + strName;
                        iIndexMail++;
                        if (iIndexMail >= listMail.Count) iIndexMail = 0;
                        string strEmail = strName + listMail[iIndexMail];

                        M_Account m = new M_Account();
                        m.Email = strEmail;
                        m.Pass = strPass;
                        m.Name = strName;

                        DataProvider.SSCVNDB.M_Account.Add(m);
                        DataProvider.SSCVNDB.SaveChanges();
                    }
                }
            }
            catch
            {
            }
            finally
            {
                DataProvider.SSCVNDB.SaveChanges();
            }

        }

        public void RegAccount(object objaccount)
        {
            try
            {
                M_Account account = objaccount as M_Account;
                Web = new WebClientEx();
                Web.DoGet("http://ssc.vn/dang@kySSC.php");

                Match mCaptcha = Regex.Match(Web.ResponseText, "(?<no1>[0-9\\s]+)(?<math>(cộng|trừ|nhân|chia))(?<no2>[0-9 ]+)", RegexOptions.IgnoreCase);
                int iNo1 = int.Parse(mCaptcha.Groups["no1"].Value.Trim());
                string strMath = mCaptcha.Groups["math"].Value.Trim();
                int iNo2 = int.Parse(mCaptcha.Groups["no2"].Value.Trim());
                if (strMath == "cộng")
                {
                    iNo1 = iNo1 + iNo2;
                }
                else if (strMath == "trừ")
                {
                    iNo1 = iNo1 - iNo2;
                }
                else if (strMath == "chia")
                {
                    iNo1 = iNo1 / iNo2;
                }
                else
                {
                    iNo1 = iNo1 * iNo2;
                }
                string humanverifyhash = Regex.Match(Web.ResponseText, "name=\"humanverify\\[hash\\]\"[^\"]+value=\"(?<val>[^\"]+)").Groups["val"].Value.Trim();
                string securitytoken = Regex.Match(Web.ResponseText, "name=\"securitytoken\"[^\"]+value=\"(?<val>[^\"]+)").Groups["val"].Value.Trim();
                NameValueCollection param = new NameValueCollection();
                param.Add("username",account.Name);
                param.Add("password",string.Empty);
                param.Add("passwordconfirm",string.Empty);
                param.Add("email",account.Email);
                param.Add("emailconfirm",account.Email);
                param.Add("humanverify[input]",iNo1.ToString());
                param.Add("humanverify[hash]",humanverifyhash);
                param.Add("timezoneoffset","7");
                param.Add("dst","2");
                param.Add("agree","1");
                param.Add("s",string.Empty);
	            param.Add("securitytoken",securitytoken);
                param.Add("do","addmember");
                param.Add("url","forum.php");
                param.Add("password_md5",Utilities.GetMd5Hash(account.Pass));
                param.Add("passwordconfirm_md5",Utilities.GetMd5Hash(account.Pass));
                param.Add("day",string.Empty);
	            param.Add("month",string.Empty);
                param.Add("year", string.Empty);
                Web.DoPost(param, "http://ssc.vn/dang@kySSC.php?do=addmember");
            }
            catch { }
        }

        public void Comment(M_Account account)
        { 
            
        }

        public bool Login(M_Account account)
        {
            Web = new WebClientEx();
            Web.DoGet("http://ssc.vn/forum.php");

            string s = Regex.Match(Web.ResponseText, "name=\"s\"[^\"]+value=\"(?<val>[^\"]+)").Groups["val"].Value.Trim();
            string securitytoken = Regex.Match(Web.ResponseText, "name=\"securitytoken\"[^\"]+value=\"(?<val>[^\"]+)").Groups["val"].Value.Trim();
            NameValueCollection param = new NameValueCollection();
            param.Add("vb_login_username",account.Name);
            param.Add("vb_login_password",string.Empty);
            param.Add("vb_login_password_hint","Mật Khẩu");
            param.Add("cookieuser","1");
            param.Add("s", s);
	        param.Add("securitytoken",securitytoken);
            param.Add("do","login");
            param.Add("vb_login_md5password",Utilities.GetMd5Hash(account.Pass));
            param.Add("vb_login_md5password_utf", Utilities.GetMd5Hash(account.Pass));
            Web.DoPost(param, string.Format("http://ssc.vn/login.php?s={0}&do=login", s));
            if (Web.ResponseText.Contains(account.Name))
            {
                M_Name name = new M_Name { Name = account.Name, Pass = account.Pass, Cookie = Utilities.ConvertObjectToBlob(Web.CookieContainer) };
                DataProvider.SSCVNDB.M_Name.Add(name);
                DataProvider.SSCVNDB.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
