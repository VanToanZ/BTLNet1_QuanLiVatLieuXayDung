using BTL_QuanLyVatLieuXayDung.Data.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BTL_QuanLyVatLieuXayDung.Data.Models
{
    public class HoaDon : BaseEntity
    {
        public HoaDon()
        {
            Id = Ulid.NewUlid().ToString();
            DetailHoaDons = new HashSet<DetailHoaDon>();
        }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string CodeHoaDon { get; set; }
        [StringLength(255)]
        [Column(TypeName = "nvarchar")]
        public string AddressNhan { get; set; }
        [StringLength(255)]
        [Column(TypeName = "nvarchar")]
        public string DescriptionHoaDon { get; set; }
        [StringLength(11)]
        [Column(TypeName = "varchar")]
        public string SoDienThoai { get; set; }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string CreateBy { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        [Required]
        public double TotalMoney { get; set; }

        public virtual ICollection<DetailHoaDon> DetailHoaDons { get; set; }    
    }
}
