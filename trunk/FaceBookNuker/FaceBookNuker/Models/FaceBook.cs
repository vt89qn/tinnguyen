//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace FaceBookNuker.Models
{
    public partial class FaceBook
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public byte[] Cookies { get; set; }
        public Nullable<int> Status { get; set; }
    }
    
}