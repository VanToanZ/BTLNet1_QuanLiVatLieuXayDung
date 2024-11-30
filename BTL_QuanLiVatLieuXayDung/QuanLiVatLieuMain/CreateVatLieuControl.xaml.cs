using BTL_QuanLiVatLieuXayDung.QuanLiLoaiVatLieuMain;
using BTL_QuanLyVatLieuXayDung.Data.Common;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Path = System.IO.Path;
using UserControl = System.Windows.Controls.UserControl;

namespace BTL_QuanLiVatLieuXayDung.QuanLiVatLieuMain
{
    /// <summary>
    /// Interaction logic for CreateVatLieuControl.xaml
    /// </summary>
    public partial class CreateVatLieuControl : UserControl
    {
        private string imageDirectory = @"Images\";
        string _idTypeVatLieu;
        private readonly IContainerRepository _containerRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly ITypeVatLieuRepository _typeVatLieuRepository;
        public CreateVatLieuControl(
            string idTypeVatLieu,
            IContainerRepository containerRepository,
            IVatLieuRepository vatLieuRepository,
            ITypeVatLieuRepository typeVatLieuRepository)
        {
            InitializeComponent();
            _idTypeVatLieu = idTypeVatLieu;
            _containerRepository = containerRepository;
            _vatLieuRepository = vatLieuRepository;
            _typeVatLieuRepository = typeVatLieuRepository;
        }

        private async void CreateVatLieu_Load(object sender, RoutedEventArgs e)
        {
             await LoadData();
        }
        private async Task LoadData()
        {
            var items = new List<KeyValueItem>
            {
                new KeyValueItem { Key = "Cái", Value = "Cái" },
                new KeyValueItem { Key = "Viên", Value = "Viên" },
                new KeyValueItem { Key = "Kg", Value = "Kilogram" },
            };
            createDonVi.ItemsSource = items;
            createDonVi.SelectedIndex = 0;
            var containers = await GetContainers();
            createContainerId.ItemsSource = containers;
            createContainerId.SelectedIndex = 0;
        }
        private async Task<List<KeyValueItem>> GetContainers()
        {
            var containers = await _containerRepository
               .FindByCondition(x => x.Status != nameof(EStatus.Delete))
               .ToListAsync();
            var keyValueItems = containers.Select(x => new KeyValueItem()
            {
                Key = x.Id,
                Value = x.NameContainer
            }).ToList();
            return keyValueItems;
        }
        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            var selectedContainer = createContainerId.SelectedItem as KeyValueItem;
            var selectedUnit = createDonVi.SelectedItem as KeyValueItem;
            string name = createNameVatLieu.Text;
            string code = createCodeVatLieu.Text;
            float.TryParse(createPrice.Text, out float price);
            if (!await ValidateInputs(name, code))
            {
                return;
            }
            string filePath = "";
            var image = createImageVatLieu.Source as BitmapImage;
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
            var vatLieu = new VatLieu()
            {
                NameVatLieu = name,
                CodeVatLieu = code,
                ContainerId = selectedContainer!.Key,
                TypeVatLieuId = _idTypeVatLieu,
                Price = price,
                Quantity = 0,
                Unit = selectedUnit!.Key,
                UrlImage = filePath,
                Status = nameof(EStatus.Active),
                CreateBy = Application.Current.Properties["username"]?.ToString() ?? "",
            };
            _vatLieuRepository.Add(vatLieu);
            await _vatLieuRepository.SaveDbSetAsync();
            MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            ClearData();
        }
        private async Task<bool> ValidateInputs(string name, string code)
        {
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên  vật liệu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createNameVatLieu.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Vui lòng nhập mã vật liệu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createCodeVatLieu.Focus();
                return false;
            }
            else
            {
                var vatLieu = await _vatLieuRepository.ExistCodeVatLieu(code);
                if (vatLieu)
                {
                    MessageBox.Show("nhập mã vật liệu đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    createCodeVatLieu.Focus();
                    return false;
                }
            }

            return true;
        }
        private void ClearData()
        {
            // Xóa các trường TextBox
            createNameVatLieu.Text = string.Empty;
            createCodeVatLieu.Text = string.Empty;
            createPrice.Text = string.Empty;
            createImageVatLieu.Source = null;
            createDonVi.SelectedIndex = 0;
            // Đặt lại ComboBox
            createContainerId.SelectedIndex = 0; // Hoặc chọn giá trị mặc định nếu có
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiLoaiVatLieuControl(
                _typeVatLieuRepository,
                _vatLieuRepository, 
                _containerRepository
               ));
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
                    createImageVatLieu.Source = new BitmapImage(new Uri(imageLocation));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải ảnh: {ex.Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadUserControl(UserControl userControl)
        {
            // Clear existing controls
            Content = userControl;
        }
    }
}
