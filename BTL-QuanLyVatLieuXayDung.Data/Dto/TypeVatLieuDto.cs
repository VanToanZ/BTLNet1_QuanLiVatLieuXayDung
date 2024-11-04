using System.Reflection;

namespace BTL_QuanLyVatLieuXayDung.Data.Dto
{
    public class TypeVatLieuDto
    {
        public string Id { get; set; } = null!;
        public string NameTypeVatLieu { get; set; } = null!;
        public string CodeTypeVatLieu { get; set; } = null!;
        public byte[] Picture { get; set; } = null!;
        public string CreateBy { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
