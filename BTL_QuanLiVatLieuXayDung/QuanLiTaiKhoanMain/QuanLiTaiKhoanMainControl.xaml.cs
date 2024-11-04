using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Data.Common;
using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace BTL_QuanLiVatLieuXayDung.QuanLiTaiKhoanMain
{
    /// <summary>
    /// Interaction logic for QuanLiTaiKhoanMainControl.xaml
    /// </summary>
    public partial class QuanLiTaiKhoanMainControl : UserControl
    {
        private readonly IUserRepository _userRepository;
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
        }

        private async Task<List<UserDto>> GetData()
        {
            var users = await _userRepository
                .FindByCondition(x => x.Status != nameof(EStatus.Delete))
                .ToListAsync();

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
    }
}
