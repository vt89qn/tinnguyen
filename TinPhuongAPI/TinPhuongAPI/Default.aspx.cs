using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using TinPhuongAPI.App_DB;
using System.Web.Script.Serialization;
namespace TinPhuongAPI
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["Page"]))
            {
                TPDBDataContext TPDB = new TPDBDataContext();
                if (Request["page"].ToLower() == "ig")
                {
                    if (Request["stage"].ToLower() == "get")
                    {
                        var strings = (from _string in TPDB.IG_SignStrings where _string.SignedString == null select _string).ToList();
                        List<Dictionary<string, string>> listUnsigned = new List<Dictionary<string, string>>();
                        foreach (IG_SignString _string in strings)
                        {
                            Dictionary<string,string> dicAdd = new Dictionary<string,string>();
                            dicAdd.Add("unsignedstring",_string.String);
                            listUnsigned.Add(dicAdd);
                        }
                        Response.Write(new JavaScriptSerializer().Serialize(listUnsigned));
                    }
                    else if (Request["stage"].ToLower() == "set")
                    {
                        string signedString = Request["signedstring"].Trim();
                        List<Dictionary<string, string>> listSigned = new JavaScriptSerializer().Deserialize<List<Dictionary<string, string>>>(signedString);
                        foreach (Dictionary<string, string> dicE in listSigned)
                        {
                            var dbstring = (from _string in TPDB.IG_SignStrings where _string.String == dicE["unsignedstring"] select _string).FirstOrDefault();
                            if (dbstring != null)
                            {
                                dbstring.SignedString = dicE["signedstring"];
                            }
                        }
                        TPDB.SubmitChanges();
                    }
                }
            }
        }
    }
}