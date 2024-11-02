using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_QuanLyVatLieuXayDung.Data.Common
{
    public class BaseStatus
    {
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string Status {  get; set; } 
    }
}
