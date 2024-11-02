namespace BTL_QuanLyVatLieuXayDung.Data.Dto
{
    public class NhapDto
    {
        public string Id { get; set; }
        public string NameVatLieu { get; set; }
        public double Quantity { get; set; } = 0;
        public double Price { get; set; }
        public double TotalMoney { get; set; }
        public string Status { get; set; }
    }
}
