using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FB.App_Context;

namespace FB.App_Common
{
    public static class Global
    {
        public static PokerContext DBContext = new PokerContext();
    }
}
