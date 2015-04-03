using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerTexas.App_Common
{
    public class AppSettings
    {
        public static readonly string UserAgentFaceBook = System.Configuration.ConfigurationManager.AppSettings["UserAgentFaceBook"];
        public static readonly string UserAgentPoker = System.Configuration.ConfigurationManager.AppSettings["UserAgentPoker"];
        public static readonly string UserAgentBrowser = System.Configuration.ConfigurationManager.AppSettings["UserAgentBrowser"];
        public static readonly string api_key = System.Configuration.ConfigurationManager.AppSettings["api_key"];
        public static readonly string DefaultPass = System.Configuration.ConfigurationManager.AppSettings["DefaultPass"];
        public static readonly string URLToCopy = System.Configuration.ConfigurationManager.AppSettings["URLToCopy"];
        public static readonly string GetMoneyOnLoad = System.Configuration.ConfigurationManager.AppSettings["GetMoneyOnLoad"];
        public static readonly string Name3G = System.Configuration.ConfigurationManager.AppSettings["Name3G"];
        public static readonly bool Seft = !string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["Seft"]);
        public static readonly string PackMakeFriend = System.Configuration.ConfigurationManager.AppSettings["PackMakeFriend"];
    }
}
