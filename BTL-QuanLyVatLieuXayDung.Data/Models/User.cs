using BTL_QuanLyVatLieuXayDung.Data.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_QuanLyVatLieuXayDung.Data
{
    public class User: BaseEntity
    {
        public User()
        {
            Id = Ulid.NewUlid().ToString();
        }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string UserName { get; set; } = null!;
        [Required]
        [StringLength(200)]
        [Column(TypeName = "nvarchar")]
        public string FullName { get; set; } = null!;
        [Required]
        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string Password { get; set; } = null!;
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string Role { get; set; } = null!;
        [Required]
        [StringLength(255)]
        [Column(TypeName = "varchar")]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(300)]
        [Column(TypeName = "nvarchar")]
        public string Address { get; set; } = null!;
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string CCCD { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? UrlImage {  get; set; }
    }
}
