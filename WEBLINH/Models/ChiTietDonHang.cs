namespace WEBLINH.Models
{
    public class ChiTietDonHang
    {
        public int MaDH { get; set; }
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public string Anh { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get { return SoLuong * DonGia; } }
    }
}
