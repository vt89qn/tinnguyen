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

    public class FBExtraData
    {
        public DateTime? CreateDate { get; set; }
        public DateTime? LastUpdateStatus { get; set; }
        public DateTime? LastUpLoadPhoto { get; set; }
        public DateTime? BirthDay { get; set; }
        public DateTime? LastUpdateProfilePhoto { get; set; }
        public DateTime? LastUpdateCoverPhoto { get; set; }
        public bool? ComfirmedEmail { get; set; }
        public bool? UpdatedProfileInfo { get; set; }

    }
}
