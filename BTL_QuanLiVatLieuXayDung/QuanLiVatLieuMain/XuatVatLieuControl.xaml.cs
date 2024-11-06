using BTL_QuanLyVatLieuXayDung.Data.Common;
using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BTL_QuanLiVatLieuXayDung.QuanLiVatLieuMain
{
    /// <summary>
    /// Interaction logic for XuatVatLieuControl.xaml
    /// </summary>
    public partial class XuatVatLieuControl : UserControl
    {
        private double total = 0;
        List<XuatVatLieuDto> _xuatVatLieuDtos;
        private readonly IContainerRepository _containerRepository;
        private readonly ITypeVatLieuRepository _typeVatLieuRepository;
        private readonly INhapRepostiory _nhapRepostiory;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly IHoaDonRepository _hoaDonRepository;
        private readonly IDetailHoaDonRepostiory _detailHoaDonRepostiory;
        public XuatVatLieuControl(
             List<XuatVatLieuDto> xuatVatLieuDtos,
             IVatLieuRepository vatLieuRepository,
             IHoaDonRepository hoaDonRepository,
             IDetailHoaDonRepostiory detailHoaDonRepostiory,
             IContainerRepository containerRepository,
             ITypeVatLieuRepository typeVatLieuRepository,
             INhapRepostiory nhapRepostiory)
        {
            InitializeComponent();
            _xuatVatLieuDtos = xuatVatLieuDtos;
            _hoaDonRepository = hoaDonRepository;
            _vatLieuRepository = vatLieuRepository;
            _detailHoaDonRepostiory = detailHoaDonRepostiory;
            _containerRepository = containerRepository;
            _typeVatLieuRepository = typeVatLieuRepository;
            _nhapRepostiory = nhapRepostiory;
        }

        private async void Create_Click(object sender, RoutedEventArgs e)
        {
            var checkedItems = GetCheckedItems();
            if (checkedItems.Count == 0)
            {
                MessageBox.Show("Không có vật liệu đc chọn để xuất.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var address = createAddress.Text;
            var sdt = createSoDienThoai.Text;
            var code = createCodeHoaDon.Text;
            var totalMoney = createTotalMoney.Text;

            // Validation
            var check = ValidateInputs(code, address, sdt);
            if (!check) return;
            await using (var transaction = await _hoaDonRepository.BeginTransactionAsync())
            {
                try
                {
                    var hoaDon = new HoaDon()
                    {
                        CodeHoaDon = code,
                        AddressNhan = address,
                        SoDienThoai = sdt,
                        DescriptionHoaDon = createMoTa.Text,
                        TotalMoney = double.Parse(totalMoney),
                        Status = nameof(EStatus.Active),
                        CreateBy = "Username",
                    };
                    _hoaDonRepository.Add(hoaDon);
                    await _hoaDonRepository.SaveDbSetAsync();

                    foreach (var item in checkedItems)
                    {
                        var soLuong = double.Parse(item.Value.Split(':').Last());
                        var vatLieu = await _vatLieuRepository.GetByIdAsync(item.Key);
                        var detail = new DetailHoaDon()
                        {
                            HoaDonId = hoaDon.Id,
                            Quanity = soLuong,
                            TotalMoney = soLuong * vatLieu!.Price,
                            VatLieuId = item.Key,
                            Status = nameof(EStatus.Active),
                        };
                        _detailHoaDonRepostiory.Add(detail);
                        await _detailHoaDonRepostiory.SaveDbSetAsync();
                        vatLieu.Quantity -= soLuong;
                        if (vatLieu.Quantity < 0)
                        {
                            await transaction.RollbackAsync();
                            MessageBox.Show("Xuất kho không thành công vì ko đủ số lượng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        _vatLieuRepository.Update(vatLieu);
                        await _vatLieuRepository.SaveDbSetAsync();
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    MessageBox.Show("Xuất và cập nhật kho thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
            }

            MessageBox.Show("Xuất và cập nhật kho thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadUserControl(new QuanLiVatLieuMainControl(
                _containerRepository,
               _typeVatLieuRepository,
               _vatLieuRepository,
               _nhapRepostiory,
               _hoaDonRepository,
               _detailHoaDonRepostiory));
        }
        private string GenerateRandomInvoice(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder invoiceNumber = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                invoiceNumber.Append(chars[random.Next(chars.Length)]);
            }

            return invoiceNumber.ToString();
        }
        private async void XuatVatLieu_Load(object sender, RoutedEventArgs e)
        {
            checkedVatLieu.Items.Clear();
            foreach (var x in _xuatVatLieuDtos)
            {
                var vatLieu = await _vatLieuRepository.GetByIdAsync(x.Id);
                checkedVatLieu.Items.Add(new KeyValueV2Item()
                {
                    Key = x.Id,
                    Value = string.Format("{0}_Số lượng:{1}", vatLieu!.NameVatLieu, x.Quanity.ToString()),
                    IsChecked = true
                });
                total += x.Quanity * vatLieu.Price;
                UpdateTotalMoney();
            }
            createCodeHoaDon.Text = GenerateRandomInvoice(12);
        }
        private List<KeyValueV2Item> GetCheckedItems()
        {
            var checkedItems = new List<KeyValueV2Item>();

            foreach (var item in checkedVatLieu.Items)
            {
                var keyValue = item as KeyValueV2Item;
                if (keyValue != null && keyValue.IsChecked)
                {
                    checkedItems.Add(keyValue);
                }
            }
            return checkedItems;
        }

        private bool ValidateInputs(string code, string address, string sdt)
        {
            var hoaDon = _hoaDonRepository.FindByCondition(x => x.CodeHoaDon.Equals(code) && x.Status != nameof(EStatus.Delete)).Any();
            if (hoaDon)
            {
                MessageBox.Show("Mã hóa đơn đã tồn tại. Đã đổi mã khác", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createCodeHoaDon.Text = GenerateRandomInvoice(12);
                return false;
            }

            if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ nhận.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createAddress.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                createSoDienThoai.Focus();
                return false;
            }

            return true;
        }
        private async void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null)
                return;

            var item = checkBox.DataContext as KeyValueV2Item;
            if (item == null)
                return;

            var vatLieu = await _vatLieuRepository.GetByIdAsync(item.Key);
            var soLuong = double.Parse(item.Value.Split(':').Last());

            // Calculate total money when the item is checked
            total += soLuong * vatLieu!.Price;
            UpdateTotalMoney();
        }

        private async void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox == null)
                return;

            var item = checkBox.DataContext as KeyValueV2Item;
            if (item == null)
                return;

            var vatLieu = await _vatLieuRepository.GetByIdAsync(item.Key);
            var soLuong = double.Parse(item.Value.Split(':').Last());

            // Subtract from total money when the item is unchecked
            total -= soLuong * vatLieu!.Price;
            UpdateTotalMoney();
        }

        private void UpdateTotalMoney()
        {
            // Update the total money label
            createTotalMoney.Text = total <= 0 ? "0" : total.ToString();
        }


        private async void Reset_Click(object sender, RoutedEventArgs e)
        {
            await ClearData();
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
        private async Task ClearData()
        {
            checkedVatLieu.Items.Clear();
            foreach (var x in _xuatVatLieuDtos)
            {
                var vatLieu = await _vatLieuRepository.GetByIdAsync(x.Id);
                checkedVatLieu.Items.Add(new KeyValueV2Item()
                {
                    Key = x.Id,
                    Value = string.Format("{0}_Số lượng:{1}", vatLieu!.NameVatLieu, x.Quanity.ToString()),
                    IsChecked = true
                });
                total += x.Quanity * vatLieu.Price;
                UpdateTotalMoney();
            }
            createCodeHoaDon.Text = GenerateRandomInvoice(12);
            createAddress.Text = string.Empty;
            createSoDienThoai.Text = string.Empty;
            createMoTa.Text = string.Empty;
            
        }
        private void LoadUserControl(UserControl userControl)
        {
         
            Content = userControl;
        }
    }
}
