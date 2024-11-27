namespace BTL_QuanLyVatLieuXayDung.Data.Dto
{
    public class XuatDto
    {
    }
    public class XuatVatLieuDto
    {
        public string Id { get; set; }
        public double Quanity { get; set; }
    }
    public class XuatVatLieuReportDto
    {
        public string NameVatLieu { get; set; } = null!;
        public byte[] Picture { get; set; } = null!;
        public double TotalQuantity { get; set; } = 0;
        public double TotalMoney { get; set; }
    }
}
