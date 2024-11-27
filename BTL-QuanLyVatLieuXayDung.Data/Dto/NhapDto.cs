namespace BTL_QuanLyVatLieuXayDung.Data.Dto
{
    public class NhapDto
    {
        public string Id { get; set; } = null!;
        public string NameVatLieu { get; set; } = null!;
        public double Quantity { get; set; } = 0;
        public double Price { get; set; }
        public double TotalMoney { get; set; }
        public string Status { get; set; } = null!;
    }
    public class NhapReportDto
    {
        public DateTime NgayNhap { get; set; }
        public string NameVatLieu { get; set; } = null!;
        public byte[] Picture { get; set; } = null!;
        public double TotalQuantity { get; set; } = 0;
        public double TotalPrice { get; set; }
    }
}
