using BTL_QuanLyVatLieuXayDung.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_QuanLyVatLieuXayDung.Data.Models
{
    public class VatLieu: BaseEntity
    {
        public VatLieu()
        {
            Id = Ulid.NewUlid().ToString();
            Nhaps = new HashSet<Nhap>();
            DetailHoaDons = new HashSet<DetailHoaDon>();
        }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string NameVatLieu { get; set; } = null!;
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string CodeVatLieu { get; set; } = null!;
        [Required]
        [ForeignKey("TypeVatLieuForeignKey")]
        public string TypeVatLieuId { get; set; } = null!;
        public TypeVatLieu TypeVatLieuForeignKey { get; set; }
        [Required]
        [ForeignKey("ContainerForeignKey")]
        public string ContainerId { get; set; } = null!;
        public Container ContainerForeignKey { get; set; }
        [Required]
        public double Quantity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string Unit { get; set; }
        [Column(TypeName = "text")]
        public string? UrlImage { get; set; }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string CreateBy { get; set; } = null!;
        public DateTime CreateAt { get; set; } = DateTime.Now; 
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public virtual ICollection<Nhap> Nhaps { get; set; } = null!;
        public virtual ICollection<DetailHoaDon> DetailHoaDons { get; set; } = null!;
    }
}
