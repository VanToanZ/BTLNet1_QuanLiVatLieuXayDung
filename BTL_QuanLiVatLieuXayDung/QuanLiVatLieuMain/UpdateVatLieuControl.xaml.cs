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
    /// Interaction logic for UpdateVatLieuControl.xaml
    /// </summary>
    public partial class UpdateVatLieuControl : UserControl
    {
        private string imageDirectory = @"Images\";
        VatLieu _vatLieu;
        private readonly IContainerRepository _containerRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly ITypeVatLieuRepository _typeVatLieuRepository;
        private readonly INhapRepostiory _nhapRepostiory;
        private readonly IHoaDonRepository _hoaDonRepository;
        private readonly IDetailHoaDonRepostiory _detailHoaDonRepostiory;
        public UpdateVatLieuControl(
             VatLieu vatLieu,
             IContainerRepository containerRepository,
             IVatLieuRepository vatLieuRepository,
             ITypeVatLieuRepository typeVatLieuRepository,
             INhapRepostiory nhapRepostiory,
             IHoaDonRepository hoaDonRepository,
             IDetailHoaDonRepostiory detailHoaDonRepostiory)
        {
            InitializeComponent();
            _vatLieu = vatLieu;
            _containerRepository = containerRepository;
            _vatLieuRepository = vatLieuRepository;
            _typeVatLieuRepository = typeVatLieuRepository;
            _nhapRepostiory = nhapRepostiory;
            _hoaDonRepository = hoaDonRepository;
            _detailHoaDonRepostiory = detailHoaDonRepostiory;
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
                    updateImageVatLieu.Source = new BitmapImage(new Uri(imageLocation));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải ảnh: {ex.Message}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }      
        }

        private async Task<bool> ValidateInputs(string name, string code)
        {
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên  vật liệu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                updateNameVatLieu.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Vui lòng nhập mã vật liệu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                updateCodeVatLieu.Focus();
                return false;
            }
            else
            {
                var vatLieu = await _vatLieuRepository.ExistTypeVatLieuByCodeAndDiffrentId(code, _vatLieu.Id);
                if (vatLieu)
                {
                    MessageBox.Show("nhập mã vật liệu đã tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    updateCodeVatLieu.Focus();
                    return false;
                }
            }

            return true;
        }
        private async void UpdateVatLieu_Load(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var containers = await GetContainers();
            updateContainerId.ItemsSource = containers;
            string containerId = _vatLieu.ContainerId;
            var indexContainer = containers.FindIndex(x => x.Key.Equals(containerId));
            if (indexContainer >= 0)
            {
                updateContainerId.SelectedIndex = indexContainer; // Set the selected index to the found index
            }
            ///typeVatLieu
            var typeVatLieus = await GetTypeVatLieus();
            updateTypeVatLieuId.ItemsSource = typeVatLieus;
            string typeVatLieuId = _vatLieu.TypeVatLieuId;
            var indexTypeVatLieu = typeVatLieus.FindIndex(x => x.Key.Equals(typeVatLieuId));
            if (indexTypeVatLieu >= 0)
            {
                updateTypeVatLieuId.SelectedIndex = indexTypeVatLieu; // Set the selected index to the found index
            }

            var items = new List<KeyValueItem>
            {
                new KeyValueItem { Key = "Cái", Value = "Cái" },
                new KeyValueItem { Key = "Viên", Value = "Viên" },
                new KeyValueItem { Key = "Kg", Value = "Kilogram" },
            };
            // Gán dữ liệu cho ComboBox
            updateDonVi.ItemsSource = items;
            var indexUnit = items.FindIndex(x => x.Key.Equals(_vatLieu.Unit));
            if (indexUnit >= 0)
            {
                updateDonVi.SelectedIndex = indexUnit; // Set the selected index to the found index
            }

            updateNameVatLieu.Text = _vatLieu.NameVatLieu;
            updateCodeVatLieu.Text = _vatLieu.CodeVatLieu;
            if (!string.IsNullOrEmpty(_vatLieu.UrlImage))
            {
                updateImageVatLieu.Source = new BitmapImage(new Uri(_vatLieu.UrlImage, UriKind.RelativeOrAbsolute));
            }
            updatePrice.Text = Convert.ToString(_vatLieu.Price);
            updateQuanity.Text = Convert.ToString(_vatLieu.Quantity);
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
        private async Task<List<KeyValueItem>> GetTypeVatLieus()
        {
            var typeVatLieus = await _typeVatLieuRepository
               .FindByCondition(x => x.Status != nameof(EStatus.Delete))
               .ToListAsync();
            var keyValueItems = typeVatLieus.Select(x => new KeyValueItem()
            {
                Key = x.Id,
                Value = x.NameTypeVatLieu
            }).ToList();
            return keyValueItems;
        }
        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            string name = updateNameVatLieu.Text;
            string code = updateCodeVatLieu.Text;
            float.TryParse(updatePrice.Text, out float price);
            float.TryParse(updateQuanity.Text, out float quanity);
            var selectedContainerId = updateContainerId.SelectedItem as KeyValueItem;
            var selectedTypeVatLieuId = updateTypeVatLieuId.SelectedItem as KeyValueItem;
            var selectedUnit = updateDonVi.SelectedItem as KeyValueItem;
            var check = await ValidateInputs(name, code);
            if (!check)
            {
                return;
            }
            string filePath = "";
            var image = updateImageVatLieu.Source as BitmapImage;
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

                   // MessageBox.Show("Hình ảnh đã được lưu và URL đã được lưu vào cơ sở dữ liệu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
            _vatLieu.NameVatLieu = name;
            _vatLieu.CodeVatLieu = code;
            _vatLieu.ContainerId = selectedContainerId!.Key;
            _vatLieu.TypeVatLieuId = selectedTypeVatLieuId!.Key;
            _vatLieu.Unit = selectedUnit!.Key;
            _vatLieu.Price = price;
            _vatLieu.Quantity = quanity;
            _vatLieu.UrlImage = filePath;
            _vatLieu.UpdateBy = Properties.Settings.Default.UserName ?? "";
            _vatLieu.UpdateAt = DateTime.Now;
            _vatLieuRepository.Update(_vatLieu);
            await _vatLieuRepository.SaveDbSetAsync();
            LoadUserControl(new QuanLiVatLieuMainControl(
                _containerRepository,
                _typeVatLieuRepository,
                _vatLieuRepository,
                _nhapRepostiory,
                _hoaDonRepository,
                _detailHoaDonRepostiory));
        }

        private async void Reset_Click(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiVatLieuMainControl(                     
                       _containerRepository,
                       _typeVatLieuRepository,
                       _vatLieuRepository,                      
                       _nhapRepostiory,
                       _hoaDonRepository,
                       _detailHoaDonRepostiory));
        }
        private void LoadUserControl(UserControl userControl)
        {
            // Clear existing controls
            Content = userControl;
        }
    }
}
