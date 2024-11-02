using System.Drawing;
using System.Reflection;

namespace BTL_QuanLyVatLieuXayDung.Data.Dto
{
    public class ContainerDto
    {
        public string Id { get; set; }
        public string NameContainer { get; set; }
        public string CodeContainer { get; set; }
        public string DescriptionContainer { get; set; }
        public ImageFileMachine Picture { get; set; }
        public string Status { get; set; }
    }
}
