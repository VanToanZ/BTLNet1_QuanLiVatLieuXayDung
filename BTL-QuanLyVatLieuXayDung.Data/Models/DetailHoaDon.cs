using BTL_QuanLyVatLieuXayDung.Data.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_QuanLyVatLieuXayDung.Data.Models
{
    public class DetailHoaDon: BaseEntity
    {
        public DetailHoaDon()
        {
            Id = Ulid.NewUlid().ToString();
        }
        public float Quanity { get; set; }

        [ForeignKey("HoaDonForeignKey")]
        public string HoaDonId { get; set; }
        public HoaDon HoaDonForeignKey { get; set; }
        [ForeignKey("VatLieuForeignKey")]
        public string VatLieuId { get; set; }
        public VatLieu VatLieuForeignKey { get; set; }
        [Required]
        public double TotalMoney { get; set; }
    }
}
