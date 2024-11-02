namespace BTL_QuanLyVatLieuXayDung.Data.Dto
{
    public class VatLieuDto
    {
        public string Id { get; set; }
        public string NameVatLieu { get; set; }
        public string CodeVatLieu { get; set; }
        public string TypeVatLieu { get; set; }
        public string KhuVuc { get; set; }
        public double Quantity { get; set; } = 0;
        public double Price { get; set; }
        public string Unit { get; set; }
        public string Status { get; set; }
    }
}
