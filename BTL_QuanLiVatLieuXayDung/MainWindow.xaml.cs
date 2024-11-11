using BTL_QuanLiVatLieuXayDung.QuanLiVatLieuXayDungMain;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using System.Windows;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace BTL_QuanLiVatLieuXayDung
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IUserRepository _userRepository;
        private readonly ITypeVatLieuRepository _typeVatLieuRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly IContainerRepository _containerRepository;
        private readonly INhapRepostiory _nhapRepostiory;
        private readonly IHoaDonRepository _hoaDonRepository;
        private readonly IDetailHoaDonRepostiory _detailHoaDonRepostiory;
        private readonly IConfigRepository _configRepository;
        public MainWindow(
            IUserRepository userRepository,
            ITypeVatLieuRepository typeVatLieuRepository,
            IVatLieuRepository vatLieuRepository,
            IContainerRepository containerRepository,
            INhapRepostiory nhapRepostiory,
            IHoaDonRepository hoaDonRepository,
            IDetailHoaDonRepostiory detailHoaDonRepostiory,
            IConfigRepository configRepository)
        {
            InitializeComponent();
            _userRepository = userRepository;
            _typeVatLieuRepository = typeVatLieuRepository;
            _vatLieuRepository = vatLieuRepository;
            _containerRepository = containerRepository;
            _nhapRepostiory = nhapRepostiory;
            _hoaDonRepository = hoaDonRepository;
            _detailHoaDonRepostiory = detailHoaDonRepostiory;
            _configRepository = configRepository;
        }
        private async void login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string passWord = txtPassword.Password;
            var acount = await _userRepository.GetAccount(username, passWord);
            if (acount != null)
            {
                Application.Current.Properties["username"] = username;
                Application.Current.Properties["role"] = acount.Role;
                var qlMain = new QuanLiVatLieuXayDungFormMain(
                     acount,
                    acount.Role,                   
                    _userRepository,
                    _typeVatLieuRepository,
                    _vatLieuRepository,
                    _containerRepository,
                    _nhapRepostiory,
                    _hoaDonRepository,
                    _detailHoaDonRepostiory,
                    _configRepository);
                this.Hide();
                qlMain.Show();
            }
            else
            {
                MessageBox.Show("Đăng nhập không thành công. Vui lòng kiểm tra lại tên đăng nhập và mật khẩu.",
                 "Lỗi đăng nhập",
                 MessageBoxButton.OK,
                 MessageBoxImage.Error);
            }
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có thật sự muốn xóa khu vực này?", "Thông báo", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                Application.Current.Shutdown(); // Đóng ứng dụng
            }
        }
    }
}