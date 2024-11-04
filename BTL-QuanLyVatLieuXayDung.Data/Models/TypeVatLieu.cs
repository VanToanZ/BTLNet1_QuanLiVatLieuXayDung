using BTL_QuanLyVatLieuXayDung.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_QuanLyVatLieuXayDung.Data.Models
{
    public class TypeVatLieu: BaseEntity
    {
        public TypeVatLieu()
        {
            Id = Ulid.NewUlid().ToString();
            VatLieus = new HashSet<VatLieu>();
        }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string NameTypeVatLieu { get; set; } = null!;
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string CodeTypeVatLieu { get; set; } = null!;
        [Required]
        [Column(TypeName = "text")]
        public string UrlImage { get; set; } = null!;
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string CreateBy { get; set; } = null!;
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string? UpdateBy { get; set; }
        public virtual ICollection<VatLieu> VatLieus { get; set; }
    }
}
