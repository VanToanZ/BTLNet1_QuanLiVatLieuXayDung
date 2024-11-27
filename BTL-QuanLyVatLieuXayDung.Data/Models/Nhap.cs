using BTL_QuanLyVatLieuXayDung.Data.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_QuanLyVatLieuXayDung.Data.Models
{
    public class Nhap: BaseEntity
    {
        public Nhap()
        {
            Id = Ulid.NewUlid().ToString();
        }
        [Required]
        public double Quantity { get; set; }
        [Required]
        [ForeignKey("VatLieuForeignKey")]
        public string VatLieuId { get; set; } = null!;
        public VatLieu VatLieuForeignKey { get; set; } = null!;
        [Required]
        public double Price { get; set; }
        [Required]
        public double TotalMoney { get; set; }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string CreateBy { get; set; } = null!;
        public DateTime CreateAt { get; set; } = DateTime.Now;
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
