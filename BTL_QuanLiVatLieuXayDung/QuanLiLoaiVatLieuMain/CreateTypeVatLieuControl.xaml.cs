using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace BTL_QuanLiVatLieuXayDung.QuanLiLoaiVatLieuMain
{
    /// <summary>
    /// Interaction logic for CreateTypeVatLieuControl.xaml
    /// </summary>
    public partial class CreateTypeVatLieuControl : UserControl
    {
        private string imageDirectory = @"Images\";
        private readonly ITypeVatLieuRepository _typeVatLieuRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly IContainerRepository _containerRepository;
        public CreateTypeVatLieuControl(
             ITypeVatLieuRepository typeVatLieuRepository,
             IVatLieuRepository vatLieuRepository,
             IContainerRepository containerRepository)
        {
            InitializeComponent();
            _typeVatLieuRepository = typeVatLieuRepository;
            _vatLieuRepository = vatLieuRepository;
            _containerRepository = containerRepository;
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
                    // Assuming you have an Image control named createImageTypeVl in your XAML
                    createImageTypeVl.Source = new BitmapImage(new Uri(imageLocation));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi tải ảnh", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiLoaiVatLieuControl(_typeVatLieuRepository, _vatLieuRepository, _containerRepository));
        }
        private void LoadUserControl(UserControl userControl)
        {
            Content = userControl;
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private async void CreateTypeVatLieu_Click(object sender, RoutedEventArgs e)
        {
            string createBy = Application.Current.Properties["username"]?.ToString()!; ;
            string name = createNameTypeVatLieu.Text;
            string code = createCodeTypeVatLieu.Text;
            string status = nameof(EStatus.Active).ToString();
            var check = await ValidateInputs(name, code);
            if (!check) return;
            string filePath = "";
            var image = createImageTypeVl.Source as BitmapImage;
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


            var typeVatLieu = new TypeVatLieu()
            {
                NameTypeVatLieu = name,
                CodeTypeVatLieu = code,
                UrlImage = filePath,
                CreateBy = createBy,
                Status = status,
            };
            _typeVatLieuRepository.Add(typeVatLieu);
            await _typeVatLieuRepository.SaveDbSetAsync();
            MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearData();
        }
        private async Task<bool> ValidateInputs(string name, string code)
        {
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên loại vật liệu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createNameTypeVatLieu.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Vui lòng nhập mã loại vật liệu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createCodeTypeVatLieu.Focus();
                return false;
            }
            else
            {
                var typeVatLieu = await _typeVatLieuRepository.ExistCodeTypeVatLieu(code);
                if (typeVatLieu)
                {
                    MessageBox.Show("Mã loại vật liệu đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    createCodeTypeVatLieu.Focus();
                    return false;
                }
            }
            return true;
        }
        private void ClearData()
        {
            createNameTypeVatLieu.Text = string.Empty;
            createCodeTypeVatLieu.Text = string.Empty;
            createImageTypeVl.Source = null; // Reset the image
        }
    }
}
