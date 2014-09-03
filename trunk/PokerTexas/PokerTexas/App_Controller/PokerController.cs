using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PokerTexas.App_Model;
using PokerTexas.App_Common;
using PokerTexas.App_UserControl;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using PokerTexas.App_Context;

namespace PokerTexas.App_Controller
{
    public class PokerController
    {
        public bool LoginMobile(Poker model)
        {
            WebClientEx client = new WebClientEx();
            if (string.IsNullOrEmpty(model.MBAccessToken))
            {
                Dictionary<string, object> dicLoginFaceBook = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(model.FaceBook.MBLoginText);
                model.MBAccessToken = dicLoginFaceBook["access_token"].ToString();
            }
            client = new WebClientEx();
            client.UserAgent = AppSettings.UserAgentFaceBook;
            client.DoGet("https://graph.facebook.com/me?access_token=" + model.MBAccessToken + "&format=json");
            Dictionary<string, object> dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
            if (dicInfo.ContainsKey("name"))
            {
                SortedDictionary<string, object> dic = new SortedDictionary<string, object>();
                dic.Add("api", "62");
                dic.Add("langtype", "13");
                dic.Add("method", "Members.Create");
                dic.Add("mid", "0");
                dic.Add("mtkey", "");
                dic.Add("protocol", "1");
                dic.Add("sid", "110");
                dic.Add("time", ((int)(DateTime.Now.AddHours(-7).Subtract(new DateTime(1970, 1, 1)).TotalSeconds)).ToString());
                dic.Add("unid", "193");
                dic.Add("version", "5.3.1");
                dic.Add("vkey", "");
                dic.Add("vmid", "");


                SortedDictionary<string, object> dic_param = new SortedDictionary<string, object>();
                dic_param.Add("ANPSetting", "4");
                dic_param.Add("appid", "1");
                dic_param.Add("appkey", "");

                dic_param.Add("is_overseas", "1");
                dic_param.Add("mbig", "");
                dic_param.Add("mnick", dicInfo["name"]);
                dic_param.Add("protocol", "1");
                dic_param.Add("sitemid", model.FaceBook.FBID);
                dic_param.Add("token", model.MBAccessToken);
                dic.Add("param", dic_param);

                dic.Add("sig", Utilities.GetMd5Hash(Utilities.getSigPoker(dic, string.Empty)));


                NameValueCollection param = new NameValueCollection();
                string api = new JavaScriptSerializer().Serialize(dic);
                param.Add("api", api);
                client = new WebClientEx();
                client.UserAgent = AppSettings.UserAgentPoker;
                client.X_TUNNEL_VERIFY = Get_X_TUNNEL_VERIFY(model.FaceBook.FBID);
                client.DoPost(param, "http://poker2011001.boyaa.com/texas/api/api.php");
                if (!string.IsNullOrEmpty(client.ResponseText) 
                    && client.ResponseText.Contains("mtkey")
                    && client.ResponseText.Contains("vkey"))
                {
                    //mid=21830508
                    dicInfo = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                    model.MBLoginText = client.ResponseText;
                    model.PKID = ((dicInfo["ret"]as Dictionary<string,object>)["aUser"]as Dictionary<string,object>)["mid"].ToString();
                    return true;
                }
            }
            return false;
        }

        public string Get_X_TUNNEL_VERIFY(string sigKey)
        {
            int iSeed = new Random().Next(10, 1000);

            string strReturn = "2.0.0&";
            strReturn += iSeed.ToString("X") + "&";
            SortedDictionary<string, string> treemap = new SortedDictionary<string, string>();
            treemap.Add("api", "62");
            treemap.Add("appID", "f61DAecVdKkJQ2l4nakA");
            treemap.Add("appVer", "5.3.1");
            treemap.Add("zSeed", iSeed.ToString());
            treemap.Add("zUid", string.Empty);
            treemap.Add("blistHash", string.Empty);
            treemap.Add("binHash", string.Empty);
            treemap.Add("iOSModel", "phone");
            treemap.Add("iOSType", "android");
            treemap.Add("iOSVer", "2.2");
            treemap.Add("macID", Utilities.GetMd5Hash(sigKey));
            treemap.Add("macAddr", string.Empty);
            treemap.Add("udid", "-1");
            treemap.Add("androidId", string.Empty);

            string jSon = new JavaScriptSerializer().Serialize(treemap);
            TPDataContext TPDB = new TPDataContext();
            TPDB.PK_SignStrings.InsertOnSubmit(new PK_SignString { String = jSon, Seed = iSeed });
            TPDB.SubmitChanges();
            System.Threading.Thread.Sleep(2000);
            while (true)
            {
                TPDB = new TPDataContext();
                PK_SignString signstring = (from _signstring in TPDB.PK_SignStrings where _signstring.String == jSon select _signstring).FirstOrDefault();
                if (signstring != null && !string.IsNullOrEmpty(signstring.SignedString1))
                {
                    strReturn += signstring.SignedString1 + "&" + signstring.SignedString2;
                    TPDB.PK_SignStrings.DeleteOnSubmit(signstring);
                    TPDB.SubmitChanges();
                    break;
                }
                System.Threading.Thread.Sleep(1000);
            }


            return strReturn;
        }
    }
}
