namespace BTL_QuanLyVatLieuXayDung.Data.Dto
{
    public class VatLieuDto
    {
        public string Id { get; set; } = null!;
        public string NameVatLieu { get; set; } = null!;
        public string CodeVatLieu { get; set; } = null!;
        public string TypeVatLieu { get; set; } = null!;
        public string KhuVuc { get; set; } = null!;
        public byte[] Picture { get; set; } = null!;
        public double Quantity { get; set; } = 0;
        public double Price { get; set; }
        public string Unit { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string SoLuongXuatVatLieu { get; set; } = "0";
        public bool IsChecked { get; set; }
    }
}
