using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using FaceBookNuker.Models;

namespace FaceBookNuker.Controller
{
    class DropboxController
    {
        public WebClientEx Web { get; set; }

        public void Reg()
        {
            var accounts = DataProvider.SSCVNDB.M_Account.ToList();
            if (accounts.Count > 0)
            {
                M_Account acc = accounts[12];
                Web = new WebClientEx();
                //Web.DoGet("https://db.tt/nI9xUqFW");
                //Web.DoGet("https://www.dropbox.com/referrals/NTY1NDczNTU5OQ");//Tin
                Web.DoGet("https://www.dropbox.com/referrals/NTMwMjI3NDczNDk");//Anh
                string TOKEN = Regex.Match(Web.ResponseText, "\"TOKEN\"[^\"]+\"(?<val>[^\"]+)").Groups["val"].Value.Trim();
                NameValueCollection param = new NameValueCollection();
                param.Add("cont", "/");
                param.Add("referral_code", "NTMwMjI3NDczNDk");
                param.Add("eh", "");
                param.Add("signup_tag", "referral");
                param.Add("fname", acc.Name);
                param.Add("lname", acc.Name);
                param.Add("email", acc.Email);
                param.Add("password", acc.Pass);
                param.Add("tos_agree", "True");
                param.Add("t", TOKEN);
                param.Add("is_xhr", "true");
                Web.DoPost(param, "https://www.dropbox.com/ajax_register");
            }
        }
    }
}
