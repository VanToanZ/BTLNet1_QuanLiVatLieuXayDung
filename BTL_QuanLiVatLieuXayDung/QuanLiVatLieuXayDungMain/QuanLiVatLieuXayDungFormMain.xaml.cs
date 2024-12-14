using BTL_QuanLiVatLieuXayDung.QuanLiHeThongMain;
using BTL_QuanLiVatLieuXayDung.QuanLiKhuVucHangMain;
using BTL_QuanLiVatLieuXayDung.QuanLiLoaiVatLieuMain;
using BTL_QuanLiVatLieuXayDung.QuanLiNhapVatLieuMain;
using BTL_QuanLiVatLieuXayDung.QuanLiTaiKhoanMain;
using BTL_QuanLiVatLieuXayDung.QuanLiVatLieuMain;
using BTL_QuanLiVatLieuXayDung.QuanLiXuatVatLieuMain;
using BTL_QuanLiVatLieuXayDung.ReportVatLieuMain;
using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Window = System.Windows.Window;

namespace BTL_QuanLiVatLieuXayDung.QuanLiVatLieuXayDungMain
{
    /// <summary>
    /// Interaction logic for QuanLiVatLieuXayDungFormMain.xaml
    /// </summary>
    public partial class QuanLiVatLieuXayDungFormMain : Window
    {
        List<MenuItem> allMenuItems = new List<MenuItem>();

        // Dictionary để lưu trạng thái ban đầu của mỗi MenuItem
        Dictionary<MenuItem, (SolidColorBrush originalColor, SolidColorBrush originalBorderColor, Thickness originalBorderThickness)> originalStates =
            new Dictionary<MenuItem, (SolidColorBrush, SolidColorBrush, Thickness)>();
        string _role;
        User _user;
        private readonly IUserRepository _userRepository;
        private readonly ITypeVatLieuRepository _typeVatLieuRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly IContainerRepository _containerRepository;
        private readonly INhapRepostiory _nhapRepostiory;
        private readonly IHoaDonRepository _hoaDonRepository;
        private readonly IDetailHoaDonRepostiory _detailHoaDonRepostiory;
        private readonly IConfigRepository _configRepository;
        public QuanLiVatLieuXayDungFormMain(
            User user,
            string role,
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
            _user = user;
            _role = role;
            _userRepository = userRepository;
            _typeVatLieuRepository = typeVatLieuRepository;
            _vatLieuRepository = vatLieuRepository;
            _containerRepository = containerRepository;
            _nhapRepostiory = nhapRepostiory;
            _hoaDonRepository = hoaDonRepository;
            _detailHoaDonRepostiory = detailHoaDonRepostiory;
            _configRepository = configRepository;
            allMenuItems.AddRange(new[]
            {
                txtQuanLiDon,
                txtQuanLiBaoCao,
                txtHeThong,
                txtQuanLiKhoHang,
                txtQuanLiTaiKhoan,
                txtThongTinTaiKhoan,
                txtQuanLiXuat,
                txtQuanLiVatLieu,
                txtQuanLiXuat,
                txtQuanLiLoaiVatLieu,
                txtQuanLiKhuVuc,
            });
            LoadUserControl(new ThongTinTaiKhoanForm(_user));
            SaveOriginalStates();
        }

        private void quanLiVatLieuXayDung_Load(object sender, RoutedEventArgs e)
        {
            if (_role.Equals(nameof(ETypeUser.NhanVien)))
            {
                txtQuanLiBaoCao.Visibility = Visibility.Hidden;
                txtHeThong.Visibility = Visibility.Hidden;
                txtQuanLiTaiKhoan.Visibility = Visibility.Hidden;
            }
            LoadUserControl(new ThongTinTaiKhoanForm(_user));
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var login = new MainWindow(
                    _userRepository,
                    _typeVatLieuRepository,
                    _vatLieuRepository,
                    _containerRepository,
                    _nhapRepostiory,
                    _hoaDonRepository,
                    _detailHoaDonRepostiory,
                    _configRepository);
            this.Hide();
            login.Show();
        }
        private void thongTinTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                SelectMenuItem(clickedItem);  // Gọi hàm SelectMenuItem để thay đổi màu sắc, viền cho mục đã chọn
            }
            LoadUserControl(new ThongTinTaiKhoanForm(_user));
        }

        private void InfoUser_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                ResetAllMenuItems(); // Đặt lại trạng thái của tất cả các MenuItem
                SelectMenuItem(clickedItem);  // Chọn và thay đổi màu sắc cho mục đã chọn
            }
            LoadUserControl(new ThongTinTaiKhoanForm(_user));
        }
        private void quanLiTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                ResetAllMenuItems(); // Đặt lại trạng thái của tất cả các MenuItem
                SelectMenuItem(clickedItem);  // Chọn và thay đổi màu sắc cho mục đã chọn
            }
            LoadUserControl(new QuanLiTaiKhoanMainControl(_userRepository));
        }
        private void quanLiKhoHang_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                SelectMenuItem(clickedItem);  // Chọn và thay đổi màu sắc cho mục đã chọn
            }
            // Load user control nếu có
        }
        private void khuVucHang_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                ResetAllMenuItems(); // Đặt lại trạng thái của tất cả các MenuItem
                SelectMenuItem(clickedItem);  // Chọn và thay đổi màu sắc cho mục đã chọn
            }
            LoadUserControl(new QuanLiKhuVucHangControl(_containerRepository, _vatLieuRepository));
        }
        private void reportNhap_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                ResetAllMenuItems(); // Đặt lại trạng thái của tất cả các MenuItem
                SelectMenuItem(clickedItem);  // Chọn và thay đổi màu sắc cho mục đã chọn
            }
            LoadUserControl(new ReportNhapVatLieuMainControl(_nhapRepostiory));
        }
        private void reportXuat_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                ResetAllMenuItems(); // Đặt lại trạng thái của tất cả các MenuItem
                SelectMenuItem(clickedItem);  // Chọn và thay đổi màu sắc cho mục đã chọn
            }
            LoadUserControl(new ReportXuatVatLieuMainControl(_detailHoaDonRepostiory, _hoaDonRepository));
        }
        private void loaiVatLieu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                ResetAllMenuItems(); // Đặt lại trạng thái của tất cả các MenuItem
                SelectMenuItem(clickedItem);  // Chọn và thay đổi màu sắc cho mục đã chọn
            }
            LoadUserControl(new QuanLiLoaiVatLieuControl(_typeVatLieuRepository,_vatLieuRepository ,_containerRepository)); ;
        }
        private void vatLieu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                ResetAllMenuItems(); // Đặt lại trạng thái của tất cả các MenuItem
                SelectMenuItem(clickedItem);  // Chọn và thay đổi màu sắc cho mục đã chọn
            }
            LoadUserControl(new QuanLiVatLieuMainControl(
              _containerRepository,
              _typeVatLieuRepository,
              _vatLieuRepository,
              _nhapRepostiory,
              _hoaDonRepository,
              _detailHoaDonRepostiory));
        }
        private void quanLiDon_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                SelectMenuItem(clickedItem);  // Chọn và thay đổi màu sắc cho mục đã chọn
            }
        }
        private void donNhap_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                ResetAllMenuItems(); // Đặt lại trạng thái của tất cả các MenuItem
                SelectMenuItem(clickedItem);  // Chọn và thay đổi màu sắc cho mục đã chọn
            }
            LoadUserControl(new QuanLiNhapVatLieuMainControl(_vatLieuRepository, _nhapRepostiory));
        }
        private void hoaDon_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                ResetAllMenuItems(); // Đặt lại trạng thái của tất cả các MenuItem
                SelectMenuItem(clickedItem);  // Chọn và thay đổi màu sắc cho mục đã chọn
            }
            LoadUserControl(new QuanLiXuatVatLieuMainControl(_hoaDonRepository,_vatLieuRepository, _detailHoaDonRepostiory));
        }
        private void quanLiBaoCao_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                SelectMenuItem(clickedItem);  // Chọn và thay đổi màu sắc cho mục đã chọn
            }
        }
        private void heThong_Click(object sender, RoutedEventArgs e)
        {
            MenuItem clickedItem = sender as MenuItem;

            if (clickedItem != null)
            {
                ResetAllMenuItems(); // Đặt lại trạng thái của tất cả các MenuItem
                SelectMenuItem(clickedItem);  // Chọn và thay đổi màu sắc cho mục đã chọn
            }   
            LoadUserControl(new QuanLiHeThongMainControl(_configRepository));
        }

        private void LoadUserControl<T>(T userControl)
        {
            // Clear existing content
            MainContent.Content = null;

            // Set the new user control
            MainContent.Content = userControl;
        }

        // Danh sách chứa tất cả các MenuItem hoặc các đối tượng bạn muốn thao tác
       

        // Hàm lưu trạng thái ban đầu cho tất cả các MenuItems
        void SaveOriginalStates()
        {
            foreach (var menuItem in allMenuItems)
            {
                // Lưu lại các thuộc tính của mỗi MenuItem
                originalStates[menuItem] = (
                    new SolidColorBrush((menuItem.Foreground as SolidColorBrush)?.Color ?? Colors.Black),
                    new SolidColorBrush((menuItem.BorderBrush as SolidColorBrush)?.Color ?? Colors.Gray),
                    menuItem.BorderThickness
                );
            }
        }
        private void SelectMenuItem(MenuItem selectedMenuItem)
        {
            // Thay đổi trạng thái của mục được chọn (MenuItem chính)
            selectedMenuItem.Foreground = new SolidColorBrush(Colors.Red);  // Màu chữ đỏ
            selectedMenuItem.BorderBrush = new SolidColorBrush(Colors.Red);  // Viền đỏ
            selectedMenuItem.BorderThickness = new Thickness(2);  // Độ dày viền = 2
            selectedMenuItem.FontWeight = FontWeights.Bold;  // In đậm font chữ
        }
        private void MenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            MenuItem openedSubMenuItem = sender as MenuItem;

            if (openedSubMenuItem != null)
            {
                // Đặt lại màu cho tất cả các mục menu
                ResetAllMenuItems();

                // Thay đổi màu cho submenu đang mở
                openedSubMenuItem.Foreground = new SolidColorBrush(Colors.Red);  // Màu chữ đỏ
                openedSubMenuItem.BorderBrush = new SolidColorBrush(Colors.Red);  // Viền đỏ
                openedSubMenuItem.BorderThickness = new Thickness(2);  // Độ dày viền = 2
                openedSubMenuItem.FontWeight = FontWeights.Bold;  // In đậm font chữ
            }
        }


        private void ResetAllMenuItems()
        {
            foreach (var menuItem in allMenuItems)
            {
                if (originalStates.ContainsKey(menuItem))
                {
                    menuItem.Foreground = originalStates[menuItem].originalColor;
                    menuItem.BorderBrush = originalStates[menuItem].originalBorderColor;
                    menuItem.BorderThickness = originalStates[menuItem].originalBorderThickness;
                    menuItem.FontWeight = FontWeights.Normal;  // Đặt lại font chữ về bình thường
                }
                if (menuItem.HasItems)
                {
                    foreach (MenuItem subMenuItem in menuItem.Items)
                    {
                        subMenuItem.Foreground = originalStates[menuItem].originalColor;
                        subMenuItem.BorderBrush = originalStates[menuItem].originalBorderColor;
                        subMenuItem.BorderThickness = originalStates[menuItem].originalBorderThickness;
                        subMenuItem.FontWeight = FontWeights.Normal;
                    }
                }
            }
        }
    }
}
