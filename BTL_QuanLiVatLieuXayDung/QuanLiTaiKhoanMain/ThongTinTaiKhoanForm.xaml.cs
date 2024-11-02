using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BTL_QuanLiVatLieuXayDung.QuanLiTaiKhoanMain
{
    /// <summary>
    /// Interaction logic for ThongTinTaiKhoanForm.xaml
    /// </summary>
    public partial class ThongTinTaiKhoanForm : UserControl
    {
        public ThongTinTaiKhoanForm(User user)
        {
            InitializeComponent();
            valueHoTen.Content = user.FullName;
            valueChucVu.Content = user.Role.Equals(nameof(ETypeUser.NhanVien)) ? "Nhân viên" : nameof(ETypeUser.Admin);
            valueCCCD.Content = user.CCCD;
            valueEmail.Content = user.Email;
            valueDiaChi.Content = user.Address;
            if (!string.IsNullOrEmpty(user.UrlImage))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(user.UrlImage, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                imageTaiKhoan.Source = bitmap;
            }
            else
            {
                // Optionally handle cases where the URL is empty or invalid
                imageTaiKhoan.Source = null; // or set a default image
            }
        }
    }
}
