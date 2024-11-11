
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BTL_QuanLiVatLieuXayDung.QuanLiKhuVucHangMain
{
    /// <summary>
    /// Interaction logic for CreateKhuVucHangControl.xaml
    /// </summary>
    public partial class CreateKhuVucHangControl : UserControl
    {
        private string imageDirectory = @"Images\";
        private readonly IContainerRepository _containerRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        public CreateKhuVucHangControl(
            IContainerRepository containerRepository,
            IVatLieuRepository vatLieuRepository)
        {
            InitializeComponent();
            _containerRepository = containerRepository;
            _vatLieuRepository = vatLieuRepository;
        }

        private async void createKhuVucHang_Click_1(object sender, RoutedEventArgs e)
        {
            string name = createNameContainer.Text;
            string code = createCodeContainer.Text;
            string mota = createMotaContainer.Text;
            string status = nameof(EStatus.Active);

            if (!await ValidateInputs(name, code))
            {
                return;
            }

            string filePath = "";
            var image = createImageContainer.Source as BitmapImage;
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
            var container = new Container
            {
                NameContainer = name,
                CodeContainer = code,
                DescriptionContainer = mota,
                UrlImage = filePath,
                Status = status
            };

            _containerRepository.Add(container);
            await _containerRepository.SaveDbSetAsync();
            MessageBox.Show("Save successful.", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearData();
        }
        private void ClearData()
        {
            createNameContainer.Text = string.Empty;
            createCodeContainer.Text = string.Empty;
            createMotaContainer.Text = string.Empty;
            createImageContainer.Source = null; // Reset image
        }

        private async Task<bool> ValidateInputs(string name, string code)
        {
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter the material type name.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                createNameContainer.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Please enter the material type code.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                createCodeContainer.Focus();
                return false;
            }

            if (await _containerRepository.ExistCodeContainer(code))
            {
                MessageBox.Show("The material type code already exists.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                createCodeContainer.Focus();
                return false;
            }

            return true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiKhuVucHangControl(_containerRepository, _vatLieuRepository));
        }

        private void LoadUserControl(UserControl userControl)
        {
            // Clear existing controls
            this.Content = null;
            userControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            userControl.VerticalAlignment = VerticalAlignment.Stretch;
            this.Content = userControl;
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

                // Show the dialog and check if the result is DialogResult.OK
                if (dialog.ShowDialog() == true) // Correct comparison
                {
                    imageLocation = dialog.FileName;
                    createImageContainer.Source = new BitmapImage(new Uri(imageLocation));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải ảnh: {ex.Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }
    }
}
