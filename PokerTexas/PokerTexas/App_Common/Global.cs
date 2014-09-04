using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerTexas.App_Context;

namespace PokerTexas.App_Common
{
    public static class Global
    {
        public static PokerContext DBContext = new PokerContext();
    }
}
