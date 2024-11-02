using BTL_QuanLyVatLieuXayDung.Data.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_QuanLyVatLieuXayDung.Data.Models
{
    public class Config: BaseEntity
    {
        public Config()
        {
            Id = Ulid.NewUlid().ToString();
        }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string ParamName { get; set; }
        [Required]
        [StringLength(500)]
        [Column(TypeName = "nvarchar")]
        public string ParamValue { get; set; }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string Group { get; set; }
    }
}
