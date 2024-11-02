using BTL_QuanLyVatLieuXayDung.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_QuanLyVatLieuXayDung.Data.Models
{
    public class Container: BaseEntity
    {
        public Container()
        {
            Id = Ulid.NewUlid().ToString();
            VatLieus = new HashSet<VatLieu>();
        }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "nvarchar")]
        public string NameContainer { get; set; }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string CodeContainer { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string UrlImage{ get; set; }
        [StringLength(255)]
        [Column(TypeName = "nvarchar")]
        public string DescriptionContainer { get; set; }

        public virtual ICollection<VatLieu> VatLieus { get; set; }
    }
}
