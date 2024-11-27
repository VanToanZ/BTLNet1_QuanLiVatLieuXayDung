
using BTL_QuanLyVatLieuXayDung.Data.Common;
using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace BTL_QuanLiVatLieuXayDung.QuanLiTaiKhoanMain
{
    /// <summary>
    /// Interaction logic for QuanLiTaiKhoanMainControl.xaml
    /// </summary>
    public partial class QuanLiTaiKhoanMainControl : UserControl
    {
        private readonly IUserRepository _userRepository;
        private int currentPage = 1;   // Trang hiện tại
        private int pageSize = 3;      // Số lượng item mỗi trang
        private int totalRecords = 0;   // Tổng số bản ghi
        private int totalPages = 0;
        public QuanLiTaiKhoanMainControl(IUserRepository userRepository)
        {
            InitializeComponent();
            _userRepository = userRepository;

            var items = new List<KeyValueItem>
            {
                new KeyValueItem { Key = "NhanVien", Value = "Nhân viên" },
                new KeyValueItem { Key = "Admin", Value = "Admin" },
            };

            // Set data for ComboBox
            searchRoles.ItemsSource = items;
        }
        private async Task LoadData()
        {
            var data = await GetData();
            dataTaiKhoan.ItemsSource = data;
            UpdatePaginationButtons();
        }

        private async Task<List<UserDto>> GetData()
        {
            // Tính toán dữ liệu cho trang hiện tại
            var skip = (currentPage - 1) * pageSize;
            var query = _userRepository
                .FindByCondition(x => x.Status != nameof(EStatus.Delete))
                .Skip(skip)
                .Take(pageSize);

            // Lấy dữ liệu từ cơ sở dữ liệu
            var users = await query.ToListAsync();

            // Tính tổng số bản ghi và số trang
            totalRecords = await _userRepository
                .FindByCondition(x => x.Status != nameof(EStatus.Delete))
            .CountAsync();

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return users.Select(x => new UserDto()
            {
                Id = x.Id,
                UserName = x.UserName,
                FullName = x.FullName,
                Address = x.Address,
                CCCD = x.CCCD,
                Email = x.Email,
                Role = x.Role,
                Status = x.Status
            }).ToList();
        }

        private async void search_Click(object sender, RoutedEventArgs e)
        {
            var users = _userRepository
                .FindByCondition(x => !x.Status.Equals(nameof(EStatus.Delete)));

            string searchUsername = searchTenTaiKhoan.Text;

            if (!string.IsNullOrEmpty(searchUsername))
            {
                users = users.Where(x => x.UserName.Contains(searchUsername));
            }

            var role = searchRoles.SelectedItem as KeyValueItem;
            if (role != null)
            {
                users = users.Where(x => x.Role.Contains(role.Key));
            }

            var userDtos = await users.Select(x => new UserDto()
            {
                Id = x.Id,
                UserName = x.UserName,
                FullName = x.FullName,
                Address = x.Address,
                CCCD = x.CCCD,
                Email = x.Email,
                Role = x.Role,
                Status = x.Status
            }).ToListAsync();

            dataTaiKhoan.ItemsSource = userDtos;
            currentPage = 1;
            UpdatePaginationButtons();
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
           LoadUserControl(new CreateTaiKhoanControl(_userRepository));
        }

        private async void sua_Click(object sender, RoutedEventArgs e)
        {
            if (dataTaiKhoan.SelectedItem is UserDto selectedUser)
            {
                var user = await _userRepository.GetByIdAsync(selectedUser.Id);
                if (user != null)
                {
                    LoadUserControl(new UpdateTaiKhoanControl(user, user.Id, _userRepository));
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn tài khoản để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void xoa_Click(object sender, RoutedEventArgs e)
        {
            if (dataTaiKhoan.SelectedItem is UserDto selectedUser)
            {
                var user = await _userRepository.GetByIdAsync(selectedUser.Id);
                if (user != null)
                {
                    var result = MessageBox.Show("Bạn có thật sự muốn xóa tài khoản này?", "Thông báo", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        _userRepository.Delete(user);
                        await _userRepository.SaveDbSetAsync();
                        MessageBox.Show("Xóa tài khoản thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn tài khoản để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void LoadUserControl<T>(T userControl)
        {
            // Clear existing controls
            this.Content = userControl; // Load the new user control
        }

        private async void quanLiTaiKhoan_Load(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private void reset_click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiTaiKhoanMainControl(_userRepository));
        }
        private void UpdatePaginationButtons()
        {
            // Xóa tất cả các nút số trang hiện có
            PaginationPanel.Children.Clear();

            // Thêm nút Previous
            btnPrevious.IsEnabled = currentPage > 1;

            // Thêm các số trang
            for (int i = 1; i <= totalPages; i++)
            {
                var pageButton = new Button
                {
                    Content = i,
                    Tag = i,
                    Width = 30,
                    Margin = new Thickness(5),
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                // Nếu nút này là trang hiện tại, thay đổi màu nền của nút
                if (i == currentPage)
                {
                    pageButton.Background = System.Windows.Media.Brushes.White; // Màu nền cho trang hiện tại
                }
                else
                {
                    pageButton.Background = System.Windows.Media.Brushes.LightGray; // Màu nền mặc định
                }

                pageButton.Click += PageButton_Click;
                PaginationPanel.Children.Add(pageButton);
            }

            // Thêm nút Next
            btnNext.IsEnabled = currentPage < totalPages;
        }
        private async void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                await LoadData();
            }
        }

        private async void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                await LoadData();
            }
        }

        private async void PageButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                currentPage = (int)button.Tag;
                await LoadData();
            }
        }
    }
}
