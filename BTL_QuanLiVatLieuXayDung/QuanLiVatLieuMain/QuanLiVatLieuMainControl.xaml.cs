using BTL_QuanLyVatLieuXayDung.Data.Common;
using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using CheckBox = System.Windows.Controls.CheckBox;
using MessageBox = System.Windows.Forms.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace BTL_QuanLiVatLieuXayDung.QuanLiVatLieuMain
{
    /// <summary>
    /// Interaction logic for QuanLiVatLieuMainControl.xaml
    /// </summary>
    public partial class QuanLiVatLieuMainControl : UserControl
    {
        private readonly IContainerRepository _containerRepository;
        private readonly ITypeVatLieuRepository _typeVatLieuRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly INhapRepostiory _nhapRepostiory;
        private readonly IHoaDonRepository _hoaDonRepository;
        private readonly IDetailHoaDonRepostiory _detailHoaDonRepostiory;
        public QuanLiVatLieuMainControl(
            IContainerRepository containerRepository,
            ITypeVatLieuRepository typeVatLieuRepository,
            IVatLieuRepository vatLieuRepository,
            INhapRepostiory nhapRepostiory,
            IHoaDonRepository hoaDonRepository,
            IDetailHoaDonRepostiory detailHoaDonRepostiory)
        {
            InitializeComponent();
            _containerRepository = containerRepository;
            _typeVatLieuRepository = typeVatLieuRepository;
            _vatLieuRepository = vatLieuRepository;
            _nhapRepostiory = nhapRepostiory;
            _hoaDonRepository = hoaDonRepository;
            _detailHoaDonRepostiory = detailHoaDonRepostiory;
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            var vatLieus = _vatLieuRepository
                .FindByCondition(x => !x.Status.Equals(nameof(EStatus.Delete)));
            string name = searchNameVatLieu.Text;

            if (!string.IsNullOrEmpty(name))
            {
                vatLieus = vatLieus.Where(x => x.NameVatLieu.Contains(name));
            }
            string code = searchMaVatLieu.Text;
            if (!string.IsNullOrEmpty(code))
            {
                vatLieus = vatLieus.Where(x => x.CodeVatLieu.Contains(code));
            }
            var selectedContainer = searchContainer.SelectedItem as KeyValueItem;
            if (selectedContainer != null)
            {
                vatLieus = vatLieus.Where(x => x.ContainerId.Equals(selectedContainer.Key));
            }
            var selectedTypeVatLieu = searchTypeVatLieu.SelectedItem as KeyValueItem;
            if (selectedTypeVatLieu != null)
            {
                vatLieus = vatLieus.Where(x => x.TypeVatLieuId.Equals(selectedTypeVatLieu.Key));
            }
            vatLieus = vatLieus.Include(x => x.ContainerForeignKey).Include(x => x.TypeVatLieuForeignKey);
            var vatLieuExcute = await vatLieus.ToListAsync();
            var vatLieuDtos = vatLieuExcute.Select(x => new VatLieuDto()
            {
                Id = x.Id,
                NameVatLieu = x.NameVatLieu,
                CodeVatLieu = x.CodeVatLieu,
                TypeVatLieu = x.TypeVatLieuForeignKey.NameTypeVatLieu,
                KhuVuc = x.ContainerForeignKey.NameContainer,
                Picture = File.ReadAllBytes(x.UrlImage!),
                Quantity = x.Quantity,
                Price = x.Price,
                Unit = x.Unit,
                Status = x.Status,
                IsChecked = x.Status.Equals(nameof(EStatus.Active)) ? true : false,
            }).ToList();
            dataVatLieu.ItemsSource = vatLieuDtos;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiVatLieuMainControl(
               _containerRepository,
               _typeVatLieuRepository,
               _vatLieuRepository,
               _nhapRepostiory,
               _hoaDonRepository,
               _detailHoaDonRepostiory));
        }

        private void CreateMaterial_Click(object sender, RoutedEventArgs e)
        {

        }
        private async void QuanLiVatLieuControl_Load(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }
        private async Task LoadData()
        {
            searchContainer.ItemsSource = await GetContainers();
            searchTypeVatLieu.ItemsSource = await GetTypeVatLieus();
            dataVatLieu.ItemsSource = await GetVatLieus();
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

        private async Task<List<VatLieuDto>> GetVatLieus()
        {
            var vatLieus = await _vatLieuRepository
               .FindByCondition(x => x.Status != nameof(EStatus.Delete))
               .Include(x => x.ContainerForeignKey)
               .Include(x => x.TypeVatLieuForeignKey)
               .ToListAsync();
            var vatLieuDtos = vatLieus.Select(x => new VatLieuDto()
            {
                Id = x.Id,
                NameVatLieu = x.NameVatLieu,
                CodeVatLieu = x.CodeVatLieu,
                TypeVatLieu = x.TypeVatLieuForeignKey.NameTypeVatLieu,
                KhuVuc = x.ContainerForeignKey.NameContainer,
                Picture = File.ReadAllBytes(x.UrlImage!),
                Quantity = x.Quantity,
                Price = x.Price,
                Unit = x.Unit,
                Status = x.Status,
                IsChecked = x.Status.Equals(nameof(EStatus.Active)) ? true : false,
            });
            return vatLieuDtos.ToList();
        }
        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dataVatLieu.SelectedItem is VatLieuDto selectedUser)
            {
                var vatLieu = await _vatLieuRepository.GetByIdAsync(selectedUser.Id);
                if (vatLieu != null)
                {
                    LoadUserControl(new UpdateVatLieuControl(
                        vatLieu.Id,
                        vatLieu,
                       _containerRepository,
                       _vatLieuRepository,
                       _typeVatLieuRepository,
                       _nhapRepostiory,
                       _hoaDonRepository,
                       _detailHoaDonRepostiory));
                }
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataVatLieu.SelectedItem is VatLieuDto selectedUser)
            {
                var vatLieu = await _vatLieuRepository.GetByIdAsync(selectedUser.Id);
                if (vatLieu != null)
                {
                    var result = MessageBox.Show("Bạn có thật sự muốn xóa vật liệu này?", "Thông báo", MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                    {
                        try
                        {
                            _vatLieuRepository.Delete(vatLieu);
                            await _vatLieuRepository.SaveDbSetAsync();
                        }
                        catch (Exception)
                        {
                            vatLieu.Status = nameof(EStatus.Delete);
                            throw;
                        }

                        MessageBox.Show("Xóa vật liệu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadData();
                    }
                }
                else
                {
                    MessageBox.Show("Xóa vật liệu không thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                LoadUserControl(new QuanLiVatLieuMainControl(
                    _containerRepository,
                    _typeVatLieuRepository,
                    _vatLieuRepository,
                    _nhapRepostiory,
                    _hoaDonRepository,
                    _detailHoaDonRepostiory));
            }
        }

        private async void Nhap_Click(object sender, RoutedEventArgs e)
        {
            if (dataVatLieu.SelectedItem is VatLieuDto selectedUser)
            {
                var vatLieu = await _vatLieuRepository.GetByIdAsync(selectedUser.Id);
                if (vatLieu != null && vatLieu.Status.Equals(nameof(EStatus.Active)))
                {
                    LoadUserControl(new NhapVatLieuControl(
                        vatLieu.Id,
                        vatLieu,
                       _containerRepository,
                       _vatLieuRepository,
                       _typeVatLieuRepository,
                       _nhapRepostiory,
                       _hoaDonRepository,
                       _detailHoaDonRepostiory));
                }
                else
                {
                    MessageBox.Show("Vật liệu đang không hoạt động hoặc không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
           
        }

        private async void Xuat_Click(object sender, RoutedEventArgs e)
        {
            var vatLieus = new List<XuatVatLieuDto>();

            // Loop through selected items in the DataGrid
            foreach (var item in dataVatLieu.SelectedItems)
            {
                if (item is VatLieuDto row) // Assuming the row is of type VatLieuDto
                {                  
                    try
                    {
                        var id = row.Id;
                        var soLuongXuat = row.SoLuongXuatVatLieu;
                        double soLuongParse;
                        double.TryParse(soLuongXuat.ToString(), out soLuongParse);
                        // Try to parse the quantity to a float
                        //double soLuongParse = double.Parse(soLuongXuat);

                        var vatLieu = await _vatLieuRepository.GetByIdAsync(id);
                        if (vatLieu!.Quantity <= 0)
                        {
                            MessageBox.Show($"Số lượng: {vatLieu.NameVatLieu} đã hết.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (vatLieu!.Quantity < soLuongParse)
                        {
                            // If the quantity in the repository is less than the requested, show error
                            MessageBox.Show("Số lượng không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (vatLieu.Status.Equals(nameof(EStatus.Inactive)))
                        {
                            MessageBox.Show("Vật liệu đang không hoạt động.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Add to vatLieus list if all conditions are met
                        vatLieus.Add(new XuatVatLieuDto()
                        {
                            Id = id,
                            Quanity = soLuongParse,
                        });
                    }
                    catch (Exception)
                    {
                        // Catch invalid parsing of quantity
                        MessageBox.Show("Số lượng không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            // Load the user control with the selected items
           LoadUserControl(new XuatVatLieuControl(
                vatLieus,
                _vatLieuRepository,
                _hoaDonRepository,
                _detailHoaDonRepostiory,
                _containerRepository,
                _typeVatLieuRepository,
                _nhapRepostiory));
        }
        private async void CheckBox_PreviewMouseUp(object sender, RoutedEventArgs e)
        {
            // Xử lý khi người dùng nhấn vào checkbox
            var dataGridCell = sender as DataGridCell;
            if (dataGridCell != null)
            {
                var checkBox = dataGridCell.Content as CheckBox;
                if (checkBox != null)
                {
                    var vatLieuDto = dataGridCell.DataContext as VatLieuDto;
                    var vatLieu = await _vatLieuRepository.GetByIdAsync(vatLieuDto!.Id);
                    // Làm gì đó khi checkbox được click

                    bool isChecked = !vatLieuDto.IsChecked;
                    if (isChecked)
                    {
                        vatLieu!.Status = nameof(EStatus.Active);
                        _vatLieuRepository.Update(vatLieu);
                    }
                    else
                    {
                        vatLieu!.Status = nameof(EStatus.Inactive);
                        _vatLieuRepository.Update(vatLieu);
                    }
                    await _containerRepository.SaveDbSetAsync();
                    await LoadData();
                    // Cập nhật trạng thái hoặc xử lý logic khác
                }
            }
        }
        private void LoadUserControl<T>(T userControl)
        {
            // Clear existing controls
            this.Content = userControl; // Load the new user control
        }
    }
}
