using BTL_QuanLiVatLieuXayDung.QuanLiVatLieuMain;
using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BTL_QuanLiVatLieuXayDung.QuanLiLoaiVatLieuMain
{
    /// <summary>
    /// Interaction logic for QuanLiLoaiVatLieuControl.xaml
    /// </summary>
    public partial class QuanLiLoaiVatLieuControl : UserControl
    {
        private readonly ITypeVatLieuRepository _typeVatLieuRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly IContainerRepository _containerRepository;

        private int currentPage = 1;   // Trang hiện tại
        private int pageSize = 3;      // Số lượng item mỗi trang
        private int totalRecords = 0;   // Tổng số bản ghi
        private int totalPages = 0;     // Tổng số trang
        public QuanLiLoaiVatLieuControl(
             ITypeVatLieuRepository typeVatLieuRepository,
             IVatLieuRepository vatLieuRepository,
             IContainerRepository containerRepository)
        {
            InitializeComponent();
            _typeVatLieuRepository = typeVatLieuRepository;
            _vatLieuRepository = vatLieuRepository;
            _containerRepository = containerRepository;
            
        }

        private async void CreateMaterial_Click(object sender, RoutedEventArgs e)
        {
            if (dataTypeVatLieu.SelectedItem is TypeVatLieuDto selectedTypeVatLieu)
            {
                var containers = await _containerRepository
                    .FindByCondition(x => x.Status.Equals(nameof(EStatus.Active)))
                    .AnyAsync();
                if (!containers)
                {
                    MessageBox.Show("Chưa có khu vực để tạo vật liệu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (selectedTypeVatLieu.Status.Equals(nameof(EStatus.Inactive)))
                {
                    MessageBox.Show("Loại vật liệu đang không hoạt động.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                LoadUserControl(new CreateVatLieuControl(selectedTypeVatLieu.Id, _containerRepository, _vatLieuRepository, _typeVatLieuRepository));
            }
            else
            {
                MessageBox.Show("Vui lòng chọn loại vật liệu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new CreateTypeVatLieuControl(_typeVatLieuRepository, _vatLieuRepository, _containerRepository));
        }
        private async void QuanLiTypeVatLieu_Load(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            var data = await GetData();
            dataTypeVatLieu.ItemsSource = data;
            UpdatePaginationButtons();
        }
        private async Task<List<TypeVatLieuDto>> GetData()
        {
            // Tính toán dữ liệu cho trang hiện tại
            var skip = (currentPage - 1) * pageSize;
            var query = _typeVatLieuRepository
                .FindByCondition(x => x.Status != nameof(EStatus.Delete))
            .Skip(skip)
                .Take(pageSize);

            // Lấy dữ liệu từ cơ sở dữ liệu
            var typeVatLieus = await query.ToListAsync();

            // Tính tổng số bản ghi và số trang
            totalRecords = await _typeVatLieuRepository
                .FindByCondition(x => x.Status != nameof(EStatus.Delete))
            .CountAsync();

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var typeVatLieuDtos = typeVatLieus.Select(x => new TypeVatLieuDto()
            {
                Id = x.Id,
                NameTypeVatLieu = x.NameTypeVatLieu,
                CodeTypeVatLieu = x.CodeTypeVatLieu,
                Picture = File.ReadAllBytes(x.UrlImage),
                CreateBy = x.CreateBy,
                Status = x.Status,
                IsChecked = x.Status.Equals(nameof(EStatus.Active)) ? true : false,
            }).ToList();
            return typeVatLieuDtos;
        }
        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dataTypeVatLieu.SelectedItem is TypeVatLieuDto selectedUser)
            {
                var typeVatLieu = await _typeVatLieuRepository.GetByIdAsync(selectedUser.Id);
                if (typeVatLieu != null)
                {
                    LoadUserControl(new UpdateTypeVatLieuControl(typeVatLieu, typeVatLieu.Id, _typeVatLieuRepository, _vatLieuRepository, _containerRepository));
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn loại vật liệu để sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    var typeVatLieuDto = dataGridCell.DataContext as TypeVatLieuDto;
                    var vatLieu = await _vatLieuRepository.ExistVatLieuByTypeVatLieuId(typeVatLieuDto!.Id);
                    if (vatLieu)
                    {
                        MessageBox.Show("Không thay đổi được trạng thái loại vật liệu này vì đang được sử dụng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    var typeVatLieu = await _typeVatLieuRepository.GetByIdAsync(typeVatLieuDto!.Id);
                    // Làm gì đó khi checkbox được click
                    
                    bool isChecked = !typeVatLieuDto.IsChecked;
                    if (isChecked)
                    {
                        typeVatLieu!.Status = nameof(EStatus.Active);
                        _typeVatLieuRepository.Update(typeVatLieu);
                    }
                    else
                    {
                        typeVatLieu!.Status = nameof(EStatus.Inactive);
                        _typeVatLieuRepository.Update(typeVatLieu);
                    }
                    await _typeVatLieuRepository.SaveDbSetAsync();
                    await LoadData();
                    // Cập nhật trạng thái hoặc xử lý logic khác
                }
            }
        }
        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataTypeVatLieu.SelectedItem is TypeVatLieuDto selectedContainer)
            {
                var vatLieu = await _vatLieuRepository.ExistVatLieuByTypeVatLieuId(selectedContainer.Id);
                if (vatLieu)
                {
                    MessageBox.Show("Không được xóa loại vật liệu này vì đang được sử dụng.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var typeVatLieu = await _typeVatLieuRepository.GetByIdAsync(selectedContainer.Id);
                if (typeVatLieu != null)
                {
                    var result = MessageBox.Show("Bạn có thật sự muốn xóa loại vật liệu này?", "Thông báo", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        _typeVatLieuRepository.Delete(typeVatLieu);
                        await _typeVatLieuRepository.SaveDbSetAsync();
                        MessageBox.Show("Xóa loại vật liệu thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadData();
                    }
                }
                else
                {
                    MessageBox.Show("Xóa khu vực không thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn loại vật liệu để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            var sql = _typeVatLieuRepository
               .FindByCondition(x => !x.Status.Equals(nameof(EStatus.Delete)));
            string name = searchNameTypeVatLieu.Text;

            if (!string.IsNullOrEmpty(name))
            {
                sql = sql.Where(x => x.NameTypeVatLieu.Contains(name));
            }
            string code = searchMaLoaiVatLieu.Text;
            if (!string.IsNullOrEmpty(code))
            {
                sql = sql.Where(x => x.CodeTypeVatLieu.Contains(code));
            }
            var typeVatLieus = await sql.ToListAsync();
            var typeLoaiVatLieuDtos = typeVatLieus.Select(x => new TypeVatLieuDto()
            {
                Id = x.Id,
                NameTypeVatLieu = x.NameTypeVatLieu,
                CodeTypeVatLieu = x.CodeTypeVatLieu,
                CreateBy = x.CreateBy,
                Picture = File.ReadAllBytes(x.UrlImage),
                Status = x.Status,
                IsChecked = x.Status.Equals(nameof(EStatus.Active)) ? true : false,
            }).ToList();
            dataTypeVatLieu.ItemsSource = typeLoaiVatLieuDtos;
            currentPage = 1;
            UpdatePaginationButtons();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiLoaiVatLieuControl(_typeVatLieuRepository, _vatLieuRepository, _containerRepository));
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
