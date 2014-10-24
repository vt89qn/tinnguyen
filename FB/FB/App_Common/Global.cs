using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FB.App_Context;
using FB.App_UserControl;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Collections;

namespace FB.App_Common
{
    public class Global
    {
        public static PokerContext DBContext = new PokerContext();

        private static List<string> listCoverPhotoLink = new List<string>();
        public static List<string> LisCoverPhotoLink
        {
            get
            {
                if (listCoverPhotoLink.Count == 0)
                {
                    getListCoverAndProfileLink();
                }
                return listCoverPhotoLink;
            }
        }

        private static List<string> listProfilePhotoLink = new List<string>();
        public static List<string> ListProfilePhotoLink
        {
            get
            {
                if (listProfilePhotoLink.Count == 0)
                {
                    getListCoverAndProfileLink();
                }
                return listProfilePhotoLink;
            }
        }

        private static void getListCoverAndProfileLink()
        {
            if (listCoverPhotoLink.Count == 0 || listProfilePhotoLink.Count == 0)
            {
                WebClientEx client = new WebClientEx();
                client.DoGet("https://500px.com/popular");
                if (!string.IsNullOrEmpty(client.ResponseText))
                {
                    string strToken = Regex.Match(client.ResponseText, "content=\"(?<val>[^\"]+)\" name=\"csrf-token\"").Groups["val"].Value.Trim();
                    if (!string.IsNullOrEmpty(strToken))
                    {
                        client.DoGet("https://api.500px.com/v1/photos?rpp=38&feature=popular&image_size[]=3&image_size[]=5&page=" + new Random().Next(1, 400) + "&sort=&include_states=true&formats=jpeg%2Clytro&only=&authenticity_token=" + strToken);
                        if (!string.IsNullOrEmpty(client.ResponseText))
                        {
                            Dictionary<string, object> dicData = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(client.ResponseText);
                            if (dicData.ContainsKey("photos"))
                            {
                                foreach (Dictionary<string, object> dicPhoto in dicData["photo"] as ArrayList)
                                {
                                    foreach (Dictionary<string, object> dicImage in dicPhoto["images"] as ArrayList)
                                    {
                                        if (dicImage["size"].ToString() == "3")
                                        {
                                            listProfilePhotoLink.Add(dicImage["url"].ToString());
                                        }
                                        if (dicImage["size"].ToString() == "5")
                                        {
                                            listCoverPhotoLink.Add(dicImage["url"].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
