using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace BTL_QuanLiVatLieuXayDung.QuanLiVatLieuMain
{
    /// <summary>
    /// Interaction logic for NhapVatLieuControl.xaml
    /// </summary>
    /// 
    public partial class NhapVatLieuControl : UserControl
    {
        private bool isLoading = false;
        string _idVatLieu;
        VatLieu _vatLieu;
        private readonly IContainerRepository _containerRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly ITypeVatLieuRepository _typeVatLieuRepository;
        private readonly INhapRepostiory _nhapRepostiory;
        private readonly IHoaDonRepository _hoaDonRepository;
        private readonly IDetailHoaDonRepostiory _detailHoaDonRepostiory;
        public NhapVatLieuControl(
            string idVatLieu,
            VatLieu vatLieu,
            IContainerRepository containerRepository,
            IVatLieuRepository vatLieuRepository,
            ITypeVatLieuRepository typeVatLieuRepository,
            INhapRepostiory nhapRepostiory,
            IHoaDonRepository hoaDonRepository,
            IDetailHoaDonRepostiory detailHoaDonRepostiory)
        {
            InitializeComponent();
            _idVatLieu = idVatLieu;
            _vatLieu = vatLieu;
            _containerRepository = containerRepository;
            _vatLieuRepository = vatLieuRepository;
            _typeVatLieuRepository = typeVatLieuRepository;
            _nhapRepostiory = nhapRepostiory;
            _hoaDonRepository = hoaDonRepository;
            _detailHoaDonRepostiory = detailHoaDonRepostiory;
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            float.TryParse(createQuantity.Text, out float quantity);
            float.TryParse(createPrice.Text, out float price);
            float.TryParse(createTotalMoney.Text, out float total);
            var nhap = new Nhap()
            {
                VatLieuId = _vatLieu.Id,
                Quantity = quantity,
                Price = price,
                TotalMoney = total,
                Status = nameof(EStatus.Active),
                CreateBy = Properties.Settings.Default.UserName ?? "",
            };
            _nhapRepostiory.Add(nhap);
            await _nhapRepostiory.SaveDbSetAsync();
            _vatLieu.Quantity = _vatLieu.Quantity + quantity;
            _vatLieuRepository.Update(_vatLieu);
            await _vatLieuRepository.SaveDbSetAsync();
            MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadUserControl(new QuanLiVatLieuMainControl(
                _containerRepository,
                _typeVatLieuRepository,
                _vatLieuRepository,
                _nhapRepostiory,
                _hoaDonRepository,
                _detailHoaDonRepostiory
                ));
        }
        private void CreateNhapVatLieu_Load(object sender, System.EventArgs e)
        {
            createNameMaterial.Text = _vatLieu.NameVatLieu;
        }
        private void CreatePrice_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Allow control characters and digits
            if (!char.IsControl(e.Text[0]) && !char.IsDigit(e.Text[0]))
            {
                e.Handled = true; // Prevent the character from being entered
            }
        }
        private void CreateQuantity_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Allow control characters and digits
            if (!char.IsControl(e.Text[0]) && !char.IsDigit(e.Text[0]) && e.Text[0] != ',')
            {
                e.Handled = true; // Prevent the character from being entered
            }
        }
        private void UpdateTotalMoney()
        {
            // Try to parse the quantity and price
            if (float.TryParse(createQuantity.Text, out float quantity) &&
                float.TryParse(createPrice.Text, out float price))
            {
                // Calculate total money
                float total = quantity * price;

                // Update the total money TextBox
                createTotalMoney.Text = total.ToString("N0"); // Format as needed (e.g., "N0" for no decimal)
            }
            else
            {
                // If either parsing fails, clear the total money TextBox
                createTotalMoney.Text = string.Empty;
            }
        }

        private void createPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotalMoney();
        }

        private void createQuantity_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            UpdateTotalMoney();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            createNameMaterial.Text = _vatLieu.NameVatLieu;
            createPrice.Text = null;
            createQuantity.Text = null;
            createTotalMoney.Text = null;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiVatLieuMainControl(
                _containerRepository,
                _typeVatLieuRepository,
                _vatLieuRepository,
                _nhapRepostiory,
                _hoaDonRepository,
                _detailHoaDonRepostiory
                ));
        }
        private void LoadUserControl(UserControl userControl)
        {
            // Clear existing controls
            Content = userControl;
        }
    }
}
