using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinhLuongThoiVu.App_Model
{
    public class NhanVien
    {
        public NhanVien()
        {
            ThoiGianLamViecs = new List<ThoiGianLamViec>();
        }
        public long ID { get; set; }
        public string Ten { get; set; }
        public virtual ICollection<ThoiGianLamViec> ThoiGianLamViecs { get; set; }
    }

    public class ThoiGianLamViec
    {
        public long ID { get; set; }

        public long NhanVienID { get; set; }
        public virtual NhanVien NhanVien { get; set; }
        
        public string Ngay { get; set; }
        public long? GioBatDauCaSang { get; set; }
        public long? PhutBatDauCaSang { get; set; }
        public long? GioKetThucCaSang { get; set; }
        public long? PhutKetThucCaSang { get; set; }

        public long? GioBatDauCaChieu { get; set; }
        public long? PhutBatDauCaChieu { get; set; }
        public long? GioKetThucCaChieu { get; set; }
        public long? PhutKetThucCaChieu { get; set; }

        public long? GioBatDauTC1 { get; set; }
        public long? PhutBatDauTC1 { get; set; }
        public long? GioKetThucTC1 { get; set; }
        public long? PhutKetThucTC1 { get; set; }

        public long? GioBatDauTC2 { get; set; }
        public long? PhutBatDauTC2 { get; set; }
        public long? GioKetThucTC2 { get; set; }
        public long? PhutKetThucTC2 { get; set; }
    }
}
