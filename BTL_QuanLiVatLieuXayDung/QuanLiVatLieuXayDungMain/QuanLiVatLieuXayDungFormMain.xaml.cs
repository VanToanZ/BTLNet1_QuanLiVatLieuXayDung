using BTL_QuanLiVatLieuXayDung.QuanLiTaiKhoanMain;
using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using System.Windows;
using System.Windows.Forms;

namespace BTL_QuanLiVatLieuXayDung.QuanLiVatLieuXayDungMain
{
    /// <summary>
    /// Interaction logic for QuanLiVatLieuXayDungFormMain.xaml
    /// </summary>
    public partial class QuanLiVatLieuXayDungFormMain : Window
    {
        string _role;
        User _user;
        private readonly IUserRepository _userRepository;
        private readonly ITypeVatLieuRepository _typeVatLieuRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly IContainerRepository _containerRepository;
        private readonly INhapRepostiory _nhapRepostiory;
        private readonly IHoaDonRepository _hoaDonRepository;
        private readonly IDetailHoaDonRepostiory _detailHoaDonRepostiory;
        public QuanLiVatLieuXayDungFormMain(
            User user,
            string role,
            IUserRepository userRepository,
            ITypeVatLieuRepository typeVatLieuRepository,
            IVatLieuRepository vatLieuRepository,
            IContainerRepository containerRepository,
            INhapRepostiory nhapRepostiory,
            IHoaDonRepository hoaDonRepository,
            IDetailHoaDonRepostiory detailHoaDonRepostiory)
        {
            InitializeComponent();
            _user = user;
            _role = role;
            _userRepository = userRepository;
            _typeVatLieuRepository = typeVatLieuRepository;
            _vatLieuRepository = vatLieuRepository;
            _containerRepository = containerRepository;
            _nhapRepostiory = nhapRepostiory;
            _hoaDonRepository = hoaDonRepository;
            _detailHoaDonRepostiory = detailHoaDonRepostiory;
            
        }

        private void quanLiVatLieuXayDung_Load(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new ThongTinTaiKhoanForm(_user));
        }
        private void thongTinTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new ThongTinTaiKhoanForm(_user));
        }
        private void quanLiTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiTaiKhoanMainControl(_userRepository));
        }
        private void quanLiKhoHang_Click(object sender, RoutedEventArgs e)
        {

        }
        private void khuVucHang_Click(object sender, RoutedEventArgs e)
        {

        }
        private void loaiVatLieu_Click(object sender, RoutedEventArgs e)
        {

        }
        private void vatLieu_Click(object sender, RoutedEventArgs e)
        {

        }
        private void quanLiDon_Click(object sender, RoutedEventArgs e)
        {

        }
        private void donNhap_Click(object sender, RoutedEventArgs e)
        {

        }
        private void hoaDon_Click(object sender, RoutedEventArgs e)
        {

        }
        private void quanLiBaoCao_Click(object sender, RoutedEventArgs e)
        {

        }
        private void heThong_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadUserControl<T>(T userControl)
        {
            // Clear existing content
            MainContent.Content = null;

            // Set the new user control
            MainContent.Content = userControl;
        }
    }
}
