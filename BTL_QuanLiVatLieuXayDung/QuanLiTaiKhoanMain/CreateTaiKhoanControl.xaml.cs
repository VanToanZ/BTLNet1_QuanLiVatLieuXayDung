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
    /// Interaction logic for CreateTaiKhoanControl.xaml
    /// </summary>
    public partial class CreateTaiKhoanControl : UserControl
    {
        private string imageDirectory = @"Images\";
        private readonly IUserRepository _userRepository;
        public CreateTaiKhoanControl(IUserRepository userRepository)
        {
            InitializeComponent();
            _userRepository = userRepository;
            var items = new List<KeyValueItem>
            {
                new KeyValueItem { Key = "NhanVien", Value = "Nhân viên" },
                new KeyValueItem { Key = "Admin", Value = "Admin" },
            };

            // Gán dữ liệu cho ComboBox
            createTkRole.ItemsSource = items;
        }

        private async void CreateTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            string userName = createUsername.Text;
            string fullName = createFullname.Text;
            string diaChi = createAddress.Text;
            string email = createEmail.Text;
            string password = createMatKhau.Text;
            string cccd = createCccd.Text;

            var selectedRole = createTkRole.SelectedItem as KeyValueItem;
            string role = selectedRole?.Key ?? nameof(ETypeUser.NhanVien);
            string status = nameof(EStatus.Active).ToString();

            if (!await ValidateInputs(userName, fullName, diaChi, email, password, cccd))
            {
                return;
            }

            string filePath = "";
            var image = createImageTK.Source as BitmapImage;
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

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(image));
                        encoder.Save(stream);
                    }

                    //MessageBox.Show("Hình ảnh đã được lưu và URL đã được lưu vào cơ sở dữ liệu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi lưu hình ảnh: {ex.Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Không có hình ảnh nào để lưu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = new User
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                CCCD = cccd,
                Password = password,
                Address = diaChi,
                Status = status,
                Role = role,
                UrlImage = filePath,
            };

            _userRepository.Add(user);
            await _userRepository.SaveDbSetAsync();
            MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearData();
        }

        private void UpLoad_Click(object sender, RoutedEventArgs e)
        {
            string imageLocation = "";
            try
            {
                OpenFileDialog dialog = new OpenFileDialog
                {
                    Filter = "JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|All files (*.*)|*.*"
                };
                if (dialog.ShowDialog() == true)
                {
                    imageLocation = dialog.FileName;
                    createImageTK.Source = new BitmapImage(new Uri(imageLocation));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi tải ảnh", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<bool> ValidateInputs(string userName, string fullName, string diaChi, string email, string password, string cccd)
        {
            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Vui lòng nhập tên người dùng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createUsername.Focus();
                return false;
            }

            var account = await _userRepository.GetAccountByUserName(userName);
            if (account != null)
            {
                MessageBox.Show("Tên tài khoản đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createUsername.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createMatKhau.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(cccd))
            {
                MessageBox.Show("Vui lòng nhập căn cước công dân.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createCccd.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(fullName))
            {
                MessageBox.Show("Vui lòng nhập họ và tên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createFullname.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(diaChi))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createAddress.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập email.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createEmail.Focus();
                return false;
            }

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Email không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createEmail.Focus();
                return false;
            }

            return true;
        }

        private void ClearData()
        {
            createUsername.Text = string.Empty;
            createFullname.Text = string.Empty;
            createAddress.Text = string.Empty;
            createEmail.Text = string.Empty;
            createMatKhau.Text = string.Empty;
            createCccd.Text = string.Empty;
            createTkRole.SelectedIndex = -1;
            createImageTK.Source = null;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiTaiKhoanMainControl(_userRepository));
        }

        private void LoadUserControl(UserControl userControl)
        {
            // Clear existing controls
            this.Content = null;
            userControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            userControl.VerticalAlignment = VerticalAlignment.Stretch;
            this.Content = userControl;
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            createUsername.Text = string.Empty;
            createFullname.Text = string.Empty;
            createAddress.Text = string.Empty;
            createEmail.Text = string.Empty;
            createMatKhau.Text = string.Empty;
            createCccd.Text = string.Empty;
            createTkRole.SelectedIndex = -1;
            createImageTK.Source = null;
        }
    }
}
