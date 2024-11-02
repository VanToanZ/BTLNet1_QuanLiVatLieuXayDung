using System.ComponentModel.DataAnnotations;

namespace BTL_QuanLyVatLieuXayDung.Data.Common
{
    public class BaseEntity: BaseStatus
    {
        [Key]
        public string Id { get; set; }
    }
}
