using BTL_QuanLyVatLieuXayDung.Data.Common;
using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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

        private int currentPage = 1;   // Trang hiện tại
        private int pageSize = 3;      // Số lượng item mỗi trang
        private int totalRecords = 0;   // Tổng số bản ghi
        private int totalPages = 0;     // Tổng số trang
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
            currentPage = 1;
            UpdatePaginationButtons();
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
            UpdatePaginationButtons();
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
            // Tính toán dữ liệu cho trang hiện tại
            var skip = (currentPage - 1) * pageSize;
            // Chuyển đổi dữ liệu thành dạng DTO
            var query = _vatLieuRepository
               .FindByCondition(x => x.Status != nameof(EStatus.Delete))
               .Include(x => x.ContainerForeignKey)
               .Include(x => x.TypeVatLieuForeignKey)
               .Skip(skip)
               .Take(pageSize);

            // Lấy dữ liệu từ cơ sở dữ liệu
            var vatLieus = await query.ToListAsync();

            // Tính tổng số bản ghi và số trang
            totalRecords = await _vatLieuRepository
               .FindByCondition(x => x.Status != nameof(EStatus.Delete))
               .CountAsync();

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

           
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
                        vatLieu,
                       _containerRepository,
                       _vatLieuRepository,
                       _typeVatLieuRepository,
                       _nhapRepostiory,
                       _hoaDonRepository,
                       _detailHoaDonRepostiory));
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn vật liệu để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataVatLieu.SelectedItem is VatLieuDto vatLieuDto)
            {
                var vatLieu = await _vatLieuRepository.GetByIdAsync(vatLieuDto.Id);
                if (vatLieu != null)
                {
                    var result = MessageBox.Show("Bạn có thật sự muốn xóa vật liệu này?", "Thông báo", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    { 
                        var nhapByVatLieuId = await _nhapRepostiory.FindByCondition(x => x.VatLieuId.Equals(vatLieuDto.Id)).FirstOrDefaultAsync();
                        var xuatByVatLieuId = await _detailHoaDonRepostiory.FindByCondition(x => x.VatLieuId.Equals(vatLieuDto.Id)).FirstOrDefaultAsync();
                        if (nhapByVatLieuId != null || xuatByVatLieuId != null)
                        {
                            vatLieu.Status = nameof(EStatus.Delete);
                            _vatLieuRepository.Update(vatLieu);
                            await _vatLieuRepository.SaveDbSetAsync();
                        }
                        else
                        {
                            _vatLieuRepository.Delete(vatLieu);
                            await _vatLieuRepository.SaveDbSetAsync();
                        }                       

                        MessageBox.Show("Xóa vật liệu thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadData();
                    }
                }
                else
                {
                    MessageBox.Show("Xóa vật liệu không thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                LoadUserControl(new QuanLiVatLieuMainControl(
                    _containerRepository,
                    _typeVatLieuRepository,
                    _vatLieuRepository,
                    _nhapRepostiory,
                    _hoaDonRepository,
                    _detailHoaDonRepostiory));
            }
            else
            {
                MessageBox.Show("Vui lòng chọn vật liệu để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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
                    MessageBox.Show("Vật liệu đang không hoạt động hoặc không tồn tại.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn vật liệu để nhập.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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
                            MessageBox.Show($"Số lượng: {vatLieu.NameVatLieu} đã hết.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (vatLieu!.Quantity < soLuongParse)
                        {
                            // If the quantity in the repository is less than the requested, show error
                            MessageBox.Show("Số lượng không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (vatLieu.Status.Equals(nameof(EStatus.Inactive)))
                        {
                            MessageBox.Show("Vật liệu đang không hoạt động.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        MessageBox.Show("Số lượng không hợp lệ.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            if (vatLieus.Any())
            {
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
            else
            {
                MessageBox.Show("Vui lòng chọn vật liệu để xuất.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
           
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

        private void UpdatePaginationButtons()
        {
            // Xóa tất cả các nút số trang hiện có
            PaginationPanel.Children.Clear();

            // Thêm nút Previous
            btnPrevious.IsEnabled = currentPage > 1;

            // Thêm các số trang
            for (int i = 1; i <= totalPages; i++)
            {
                var pageButton = new Button
                {
                    Content = i,
                    Tag = i,
                    Width = 30,
                    Margin = new Thickness(5),
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                // Nếu nút này là trang hiện tại, thay đổi màu nền của nút
                if (i == currentPage)
                {
                    pageButton.Background = System.Windows.Media.Brushes.White; // Màu nền cho trang hiện tại
                }
                else
                {
                    pageButton.Background = System.Windows.Media.Brushes.LightGray; // Màu nền mặc định
                }

                pageButton.Click += PageButton_Click;
                PaginationPanel.Children.Add(pageButton);
            }

            // Thêm nút Next
            btnNext.IsEnabled = currentPage < totalPages;
        }


        private async void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                await LoadData();
            }
        }

        private async void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                await LoadData();
            }
        }

        private async void PageButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                currentPage = (int)button.Tag;
                await LoadData();
            }
        }
    }
}
