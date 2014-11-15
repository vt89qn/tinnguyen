using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FB.App_Model
{
    public class FaceBook
    {
        public FaceBook()
        {
            Pages = new List<Page>();
        }
        public long ID { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public string FBID { get; set; }
        public string MBLoginText { get; set; }
        public byte[] MBCookie { get; set; }
        public byte[] WebCookie { get; set; }
        public string ExtraData { get; set; }

        public long FBPackageID { get; set; }
        public virtual FBPackage FBPackage { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
    }

    public class Page
    {
        public long ID { get; set; }
        public string PageID { get; set; }
        public string PageName { get; set; }
        public string AccessToken { get; set; }
        public string PageData { get; set; }

        public long FaceBookID { get; set; }
        public virtual FaceBook FaceBook { get; set; }
    }

    public class FBPackage
    {
        public FBPackage()
        {
            FaceBooks = new List<FaceBook>();
        }
        public long ID { get; set; }
        public long Pack { get; set; }

        public virtual ICollection<FaceBook> FaceBooks { get; set; }
    }

    public class FBFriend
    {
        public long ID { get; set; }
        public long FBID1 { get; set; }
        public long FBID2 { get; set; }
    }

    public class Like
    {
        public long ID { get; set; }
        public long FromID { get; set; }
        public long ToID { get; set; }
    }

    public class StatusData
    {
        public long ID { get; set; }
        public string Text { get; set; }
    }

    public class PageData
    {
        public DateTime? LUS { get; set; }
        public DateTime? LUP { get; set; }
        public DateTime? LUPP { get; set; }
        public DateTime? LUCP { get; set; }
    }

    public class FBExtraData
    {
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdateStatus { get; set; }
        public DateTime? LastUpLoadPhoto { get; set; }
        public DateTime? BirthDay { get; set; }
        public DateTime? LastUpdateProfilePhoto { get; set; }
        public DateTime? LastUpdateCoverPhoto { get; set; }
        public DateTime? LastMakeFriend { get; set; }
        public bool? ComfirmedEmail { get; set; }
        public bool? UpdatedProfileInfo { get; set; }
        public bool? BlockCreatePage { get; set; }
        public bool? ProfileUS { get; set; }

    }
}
