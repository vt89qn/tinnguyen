using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using FaceBookNuker.Models;
public class DataProvider
{
    public static SSCVNContext SSCVNDB = new SSCVNContext();
    public static DBContext DB = new DBContext();

    private static DataProvider provider;
    public static DataProvider Provider
    {
        get
        {
            if (provider == null)
                provider = new DataProvider();
            return provider;
        }
    }
}
