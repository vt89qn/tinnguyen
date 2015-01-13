using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinhLuongThoiVu.App_Context;

namespace TinhLuongThoiVu.App_Common
{
    public static class Global
    {
        public static DataContext DBContext = new DataContext();
    }
}
