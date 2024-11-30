using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Path = System.IO.Path;
using UserControl = System.Windows.Controls.UserControl;

namespace BTL_QuanLiVatLieuXayDung.QuanLiKhuVucHangMain
{
    /// <summary>
    /// Interaction logic for UpdateKhuVucHangControl.xaml
    /// </summary>
    public partial class UpdateKhuVucHangControl : UserControl
    {
        private string _id;
        private Container _container;
        private readonly IContainerRepository _containerRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        private string imageDirectory = @"Images\";
        public UpdateKhuVucHangControl(
             Container container,
             string id,
             IContainerRepository containerRepository,
             IVatLieuRepository vatLieuRepository)
        {
            InitializeComponent();
            _container = container;
            _id = id;
            _containerRepository = containerRepository;
            _vatLieuRepository = vatLieuRepository;
        }

        private async void UpdateKhuVucHang_Click(object sender, RoutedEventArgs e)
        {
            string name = updateNameContainer.Text;
            string code = updateCodeContainer.Text;
            string mota = updateMotaContainer.Text;

            var isValid = await ValidateInputs(name, code);
            if (!isValid) return;
            var image = updateImageContainer.Source as BitmapImage;
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
            // Update container properties
            _container.NameContainer = name;
            _container.CodeContainer = code;
            _container.UrlImage = filePath;
            _container.DescriptionContainer = mota;

            _containerRepository.Update(_container);
            await _containerRepository.SaveDbSetAsync();
            LoadUserControl(new QuanLiKhuVucHangControl(_containerRepository, _vatLieuRepository));
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
        private async Task<bool> ValidateInputs(string name, string code)
        {
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập khu vực.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                updateNameContainer.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Vui lòng nhập mã khu vực.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                updateCodeContainer.Focus();
                return false;
            }

            var typeVatLieuExists = await _containerRepository.ExistContainerByCodeAndDiffrentId(code, _id);
            if (typeVatLieuExists)
            {
                MessageBox.Show("Nhập mã khu vực đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                updateCodeContainer.Focus();
                return false;
            }

            return true;
        }
        private void UpdateKhuVucHang_Load(object sender, RoutedEventArgs e)
        {
            updateNameContainer.Text = _container.NameContainer;
            updateCodeContainer.Text = _container.CodeContainer;            
            if (!string.IsNullOrEmpty(_container.UrlImage))
            {
                updateImageContainer.Source = new BitmapImage(new Uri(_container.UrlImage, UriKind.RelativeOrAbsolute));
            }
            updateMotaContainer.Text = _container.DescriptionContainer;

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiKhuVucHangControl(_containerRepository, _vatLieuRepository));
        }
        private void LoadUserControl(UserControl userControl)
        {
            // Clear existing controls
            this.Content = null;
            Content = userControl;
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
                    updateImageContainer.Source = new BitmapImage(new Uri(dialog.FileName));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi tải ảnh", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            updateNameContainer.Text = _container.NameContainer;
            updateCodeContainer.Text = _container.CodeContainer;
            if (!string.IsNullOrEmpty(_container.UrlImage))
            {
                updateImageContainer.Source = new BitmapImage(new Uri(_container.UrlImage, UriKind.RelativeOrAbsolute));
            }
            updateMotaContainer.Text = _container.DescriptionContainer;
        }
    }
}
