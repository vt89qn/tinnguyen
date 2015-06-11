using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerTexas.App_Model
{
    public class FaceBook
    {
        public FaceBook()
        {
            Pokers = new List<Poker>();
        }
        public long ID { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public string FBID { get; set; }
        public string MBLoginText { get; set; }
        public string BirthDay { get; set; }
        public virtual ICollection<Poker> Pokers { get; set; }
    }

    public class Package
    {
        public Package()
        {
            Pokers = new List<Poker>();
        }
        public long ID { get; set; }
        public long Pack { get; set; }

        public virtual ICollection<Poker> Pokers { get; set; }
    }
    public class Poker
    {
        public long ID { get; set; }

        public long FaceBookID { get; set; }
        public virtual FaceBook FaceBook { get; set; }

        public string PKID { get; set; }

        public long PackageID { get; set; }
        public virtual Package Package { get; set; }

        public string MBLoginText { get; set; }

        public byte[] WebCookie { get; set; }
        public string X_TUNNEL_VERIFY { get; set; }
    }

    public class PokerExData
    {
        public string m_vkey { get; set; }
        public string m_mtkey { get; set; }
        public string access_token { get; set; }
        public string ip_address { get; set; }
        public string apik { get; set; }
        public string sid { get; set; }
        public string mtkey { get; set; }
        public string mnick { get; set; }
        public string expLevel { get; set; }
        public string loginkey { get; set; }
        public int count_login_fail { get; set; }
        public decimal? money { get; set; }
        public decimal? earnmoney { get; set; }
        public long? old_pack { get; set; }
        public string scoped_user_id { get; set; }
        public string bytkn { get; set; }
    }

    public class IPAddress
    {
        public long ID { get; set; }
        public string IP { get; set; }
        public string Date { get; set; }
    }
}
