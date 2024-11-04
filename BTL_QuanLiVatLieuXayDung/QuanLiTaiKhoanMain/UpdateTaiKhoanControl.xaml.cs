using BTL_QuanLyVatLieuXayDung.Data;
using BTL_QuanLyVatLieuXayDung.Data.Common;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Path = System.IO.Path;

namespace BTL_QuanLiVatLieuXayDung.QuanLiTaiKhoanMain
{
    /// <summary>
    /// Interaction logic for UpdateTaiKhoanControl.xaml
    /// </summary>
    /// 

    public partial class UpdateTaiKhoanControl : UserControl
    {
        private string _id;
        private User _user;
        private readonly IUserRepository _userRepository;
        private string imageDirectory = @"Images\";
        public UpdateTaiKhoanControl(User user, string id, IUserRepository userRepository)
        {
            InitializeComponent();
            _user = user;
            _id = id;
            _userRepository = userRepository;

            var items = new List<KeyValueItem>
            {
                new KeyValueItem { Key = "NhanVien", Value = "Nhân viên" },
                new KeyValueItem { Key = "Admin", Value = "Admin" },
            };

            // Set the ItemsSource for the ComboBox
            updateTkRole.Items.Clear();
            updateTkRole.ItemsSource = items;

            // Bind user properties to UI elements
            updateUsername.Text = user.UserName;
            updateMatKhau.Password = user.Password; // Secure input
            updateFullname.Text = user.FullName;
            updateEmail.Text = user.Email;
            updateAddress.Text = user.Address;
            updateCccd.Text = user.CCCD;

            // Set the selected role by finding the matching item
            var selectedItem = items.FirstOrDefault(x => x.Key.Equals(user.Role));
            if (selectedItem != null)
            {
                updateTkRole.SelectedItem = selectedItem;
            }

            // Load user image
            if (!string.IsNullOrEmpty(user.UrlImage))
            {
                updateImageTK.Source = new BitmapImage(new Uri(user.UrlImage, UriKind.RelativeOrAbsolute));
            }
        }

        private void updateTaiKhoan_Load(object sender, RoutedEventArgs e)
        {

        }

        private async void updateTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            string userName = updateUsername.Text;
            string fullName = updateFullname.Text;
            string diaChi = updateAddress.Text;
            string email = updateEmail.Text;
            string password = updateMatKhau.Password;
            string cccd = updateCccd.Text;
            var selectedRole = updateTkRole.SelectedItem as KeyValueItem;
            string role = selectedRole?.Key ?? nameof(ETypeUser.NhanVien);
            string status = nameof(EStatus.Active).ToString();

            if (!await ValidateInputs(userName, fullName, diaChi, email, password, cccd))
                return;

            var image = updateImageTK.Source as BitmapImage;
            string filePath = string.Empty;

            if (image != null)
            {
                try
                {
                    string fileName = $"Image_{DateTime.Now:yyyyMMddHHmmss}.png";
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), imageDirectory, fileName);

                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), imageDirectory)))
                    {
                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), imageDirectory));
                    }

                    // Save image (requires conversion)
                    SaveImage(image, filePath);
                    MessageBox.Show("Hình ảnh đã được lưu và URL đã được lưu vào cơ sở dữ liệu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi lưu hình ảnh: {ex.Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Không có hình ảnh nào để lưu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            // Update user details
            _user.UserName = userName;
            _user.FullName = fullName;
            _user.Email = email;
            _user.CCCD = cccd;
            _user.Password = password;
            _user.Address = diaChi;
            _user.Status = status;
            _user.Role = role;
            _user.UrlImage = filePath;
            _userRepository.Update(_user);
            await _userRepository.SaveDbSetAsync();

            MessageBox.Show("Update thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadUserControl(new QuanLiTaiKhoanMainControl(_userRepository));
        }
        private void SaveImage(BitmapImage image, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }

        private async Task<bool> ValidateInputs(string userName, string fullName, string diaChi, string email, string password, string cccd)
        {
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Vui lòng nhập tên người dùng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var existUserName = await _userRepository.ExistAccountByUserNameAndDiffrentId(userName, _id);
            if (existUserName)
            {
                MessageBox.Show("Tên tài khoản này đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(cccd))
            {
                MessageBox.Show("Vui lòng nhập căn cước công dân.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập họ và tên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(diaChi))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập email.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Email không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
        private void UpLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|All files (*.*)|*.*"
                };
                if (dialog.ShowDialog() == true)
                {
                    updateImageTK.Source = new BitmapImage(new Uri(dialog.FileName));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi tải ảnh", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadUserControl(UserControl userControl)
        {
            // Clear existing controls
            Content = userControl;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiTaiKhoanMainControl(_userRepository));
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            var items = new List<KeyValueItem>
            {
                new KeyValueItem { Key = "NhanVien", Value = "Nhân viên" },
                new KeyValueItem { Key = "Admin", Value = "Admin" },
            };

            // Set the ItemsSource for the ComboBox
            
            updateTkRole.ItemsSource = items;

            // Bind user properties to UI elements
            updateUsername.Text = _user.UserName;
            updateMatKhau.Password = _user.Password; // Secure input
            updateFullname.Text = _user.FullName;
            updateEmail.Text = _user.Email;
            updateAddress.Text = _user.Address;
            updateCccd.Text = _user.CCCD;

            // Set the selected role by finding the matching item
            var selectedItem = items.FirstOrDefault(x => x.Key.Equals(_user.Role));
            if (selectedItem != null)
            {
                updateTkRole.SelectedItem = selectedItem;
            }

            // Load user image
            if (!string.IsNullOrEmpty(_user.UrlImage))
            {
                updateImageTK.Source = new BitmapImage(new Uri(_user.UrlImage, UriKind.RelativeOrAbsolute));
            }
        }
    }
}
