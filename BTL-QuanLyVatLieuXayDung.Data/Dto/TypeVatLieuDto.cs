using System.Reflection;

namespace BTL_QuanLyVatLieuXayDung.Data.Dto
{
    public class TypeVatLieuDto
    {
        public string Id { get; set; }
        public string NameTypeVatLieu { get; set; }
        public string CodeTypeVatLieu { get; set; }
        public ImageFileMachine Picture { get; set; }
        public string CreateBy { get; set; }
        public string Status { get; set; }
    }
}
