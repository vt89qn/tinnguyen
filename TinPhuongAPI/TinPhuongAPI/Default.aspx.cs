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
            TPDBDataContext TPDB = new TPDBDataContext();
            if (Request["stage"].ToLower() == "get")
            {
                List<Dictionary<string, object>> listUnsigned = new List<Dictionary<string, object>>();
                if (string.IsNullOrEmpty(Request["page"]) || Request["page"] == "ig")
                {
                    var strings = (from _string in TPDB.IG_SignStrings where _string.SignedString == null select _string).ToList();
                    foreach (IG_SignString _string in strings)
                    {
                        Dictionary<string, object> dicAdd = new Dictionary<string, object>();
                        dicAdd.Add("page", "ig");
                        dicAdd.Add("unsignedstring", _string.String);
                        listUnsigned.Add(dicAdd);
                    }
                }
                if (string.IsNullOrEmpty(Request["page"]) || Request["page"] == "pk")
                {
                    var strings = (from _string in TPDB.PK_SignStrings where (_string.SignedString1 == null || _string.SignedString2 == null) select _string).ToList();
                    foreach (PK_SignString _string in strings)
                    {
                        Dictionary<string, object> dicAdd = new Dictionary<string, object>();
                        dicAdd.Add("page", "pk");
                        dicAdd.Add("seed", _string.Seed);
                        dicAdd.Add("unsignedstring", _string.String);
                        listUnsigned.Add(dicAdd);
                    }
                }
                Response.Write(new JavaScriptSerializer().Serialize(listUnsigned));
            }
            else if (Request["stage"].ToLower() == "set")
            {
                string signedString = Request["signedstring"].Trim();
                List<Dictionary<string, object>> listSigned = new JavaScriptSerializer().Deserialize<List<Dictionary<string, object>>>(signedString);
                foreach (Dictionary<string, object> dicE in listSigned)
                {
                    if (dicE["page"].ToString() == "ig")
                    {
                        var dbstring = (from _string in TPDB.IG_SignStrings where _string.String == dicE["unsignedstring"].ToString() select _string).FirstOrDefault();
                        if (dbstring != null)
                        {
                            dbstring.SignedString = dicE["signedstring"].ToString();
                        }
                    }
                    else if (dicE["page"].ToString() == "pk")
                    {
                        var dbstring = (from _string in TPDB.PK_SignStrings where (_string.String == dicE["unsignedstring"].ToString() && _string.Seed.ToString() == dicE["seed"].ToString()) select _string).FirstOrDefault();
                        if (dbstring != null)
                        {
                            dbstring.SignedString1 = dicE["signedstring1"].ToString();
                            dbstring.SignedString2 = dicE["signedstring2"].ToString();
                        }
                    }
                }
                TPDB.SubmitChanges();
            }
        }
    }
}