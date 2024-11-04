namespace BTL_QuanLyVatLieuXayDung.Data.Dto
{
    public class ContainerDto
    {
        public string Id { get; set; } = null!;
        public string NameContainer { get; set; } = null!;
        public string CodeContainer { get; set; } = null!;
        public string? DescriptionContainer { get; set; }
        public byte[] Picture { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
