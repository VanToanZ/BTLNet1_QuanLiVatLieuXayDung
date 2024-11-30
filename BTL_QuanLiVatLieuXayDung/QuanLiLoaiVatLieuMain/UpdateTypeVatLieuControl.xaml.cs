using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Path = System.IO.Path;
using UserControl = System.Windows.Controls.UserControl;

namespace BTL_QuanLiVatLieuXayDung.QuanLiLoaiVatLieuMain
{
    /// <summary>
    /// Interaction logic for UpdateTypeVatLieuControl.xaml
    /// </summary>
    public partial class UpdateTypeVatLieuControl : UserControl
    {
        string _id;
        TypeVatLieu _typeVatLieu;
        private readonly ITypeVatLieuRepository _typeVatLieuRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly IContainerRepository _containerRepository;
        private string imageDirectory = @"Images\";
        public UpdateTypeVatLieuControl(
             TypeVatLieu typeVatLieu,
             string id,
             ITypeVatLieuRepository typeVatLieuRepository,
             IVatLieuRepository vatLieuRepository,
             IContainerRepository containerRepository)
        {
            InitializeComponent();
            _id = id;
            _typeVatLieu = typeVatLieu;
            _vatLieuRepository = vatLieuRepository;
            _containerRepository = containerRepository;
            _typeVatLieuRepository = typeVatLieuRepository;
           
        }

        private async void UpdateTypeVatLieu_Click(object sender, RoutedEventArgs e)
        {
            string name = updateNameTypeVatLieu.Text;
            string code = updateCodeTypeVatLieu.Text;
            var isValid = await ValidateInputs(name, code);
            if (!isValid) return;
            var image = updateImageTypeVl.Source as BitmapImage;
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
            _typeVatLieu.NameTypeVatLieu = name;
            _typeVatLieu.CodeTypeVatLieu = code;
            _typeVatLieu.UrlImage = filePath;
            _typeVatLieu.UpdateBy = Application.Current.Properties["username"]?.ToString() ?? "";
            _typeVatLieuRepository.Update(_typeVatLieu);
            await _typeVatLieuRepository.SaveDbSetAsync();
            LoadUserControl(new QuanLiLoaiVatLieuControl(_typeVatLieuRepository, _vatLieuRepository, _containerRepository));
        }
        private async Task<bool> ValidateInputs(string name, string code)
        {
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên loại vật liệu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                updateNameTypeVatLieu.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Vui lòng nhập mã loại vật liệu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                updateCodeTypeVatLieu.Focus();
                return false;
            }
            else
            {
                var typeVatLieu = await _typeVatLieuRepository.ExistTypeVatLieuByCodeAndDiffrentId(code, _id);
                if (typeVatLieu)
                {
                    MessageBox.Show("nhập mã loại vật liệu đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    updateCodeTypeVatLieu.Focus();
                    return false;
                }
            }

            return true;
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
                    updateImageTypeVl.Source = new BitmapImage(new Uri(dialog.FileName));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi tải ảnh", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateTypeVatLieu_Load(object sender, RoutedEventArgs e)
        {
            updateNameTypeVatLieu.Text = _typeVatLieu.NameTypeVatLieu;
            updateCodeTypeVatLieu.Text = _typeVatLieu.CodeTypeVatLieu;
            if (!string.IsNullOrEmpty(_typeVatLieu.UrlImage))
            {
                updateImageTypeVl.Source = new BitmapImage(new Uri(_typeVatLieu.UrlImage, UriKind.RelativeOrAbsolute));
            }
        }
        private void LoadUserControl(UserControl userControl)
        {
            // Clear existing controls
            this.Content = null;
            Content = userControl;
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            updateNameTypeVatLieu.Text = _typeVatLieu.NameTypeVatLieu;
            updateCodeTypeVatLieu.Text = _typeVatLieu.CodeTypeVatLieu;
            if (!string.IsNullOrEmpty(_typeVatLieu.UrlImage))
            {
                updateImageTypeVl.Source = new BitmapImage(new Uri(_typeVatLieu.UrlImage, UriKind.RelativeOrAbsolute));
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiLoaiVatLieuControl(_typeVatLieuRepository, _vatLieuRepository, _containerRepository));
        }
    }
}
