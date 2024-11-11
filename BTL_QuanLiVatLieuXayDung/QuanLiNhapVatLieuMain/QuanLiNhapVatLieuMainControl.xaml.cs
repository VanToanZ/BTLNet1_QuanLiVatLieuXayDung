using BTL_QuanLyVatLieuXayDung.Data.Common;
using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace BTL_QuanLiVatLieuXayDung.QuanLiNhapVatLieuMain
{
    /// <summary>
    /// Interaction logic for QuanLiNhapVatLieuMainControl.xaml
    /// </summary>
    public partial class QuanLiNhapVatLieuMainControl : UserControl
    {
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly INhapRepostiory _nhapRepostiory;
        private int currentPage = 1;   // Trang hiện tại
        private int pageSize = 3;      // Số lượng item mỗi trang
        private int totalRecords = 0;   // Tổng số bản ghi
        private int totalPages = 0;
        public QuanLiNhapVatLieuMainControl(
            IVatLieuRepository vatLieuRepository,
            INhapRepostiory nhapRepostiory)
        {
            InitializeComponent();
            _vatLieuRepository = vatLieuRepository;
            _nhapRepostiory = nhapRepostiory;
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            var nhapVatLieus = _nhapRepostiory
                .FindByCondition(x => !x.Status.Equals(nameof(EStatus.Delete)));
            var selectedVatLieu = searchVatLieu.SelectedItem as KeyValueItem;
            if (selectedVatLieu != null)
            {
                nhapVatLieus = nhapVatLieus.Where(x => x.VatLieuId == selectedVatLieu.Key);
            }

            nhapVatLieus = nhapVatLieus.Include(x => x.VatLieuForeignKey);
            var nhapVatLieuExcute = await nhapVatLieus.ToListAsync();
            var nhapVatLieuDtos = nhapVatLieuExcute.Select(x => new NhapDto()
            {
                Id = x.Id,
                NameVatLieu = x.VatLieuForeignKey.NameVatLieu,
                Quantity = x.Quantity,
                Price = x.Price,
                TotalMoney = x.TotalMoney,
                Status = x.Status
            }).ToList();
            dataNhap.ItemsSource = nhapVatLieuDtos;
            currentPage = 1;
            UpdatePaginationButtons();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiNhapVatLieuMainControl(_vatLieuRepository, _nhapRepostiory));
        }
        private async void NhapVatLieu_Load(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            searchVatLieu.ItemsSource = await GetVatLieus();
            dataNhap.ItemsSource = await GetNhapVatLieus();
            UpdatePaginationButtons();

        }
        private async Task<List<KeyValueItem>> GetVatLieus()
        {
            var containers = await _vatLieuRepository
               .FindByCondition(x => x.Status != nameof(EStatus.Delete))
               .ToListAsync();
            var keyValueItems = containers.Select(x => new KeyValueItem()
            {
                Key = x.Id,
                Value = x.NameVatLieu
            }).ToList();
            return keyValueItems;
        }
        private async Task<List<NhapDto>> GetNhapVatLieus()
        {
            // Tính toán dữ liệu cho trang hiện tại
            var skip = (currentPage - 1) * pageSize;
            var query = _nhapRepostiory
                .FindByCondition(x => x.Status != nameof(EStatus.Delete))
                .Include(x => x.VatLieuForeignKey)
                .Skip(skip)
                .Take(pageSize);

            // Lấy dữ liệu từ cơ sở dữ liệu
            var nhapVatLieus = await query.ToListAsync();

            // Tính tổng số bản ghi và số trang
            totalRecords = await _nhapRepostiory
                .FindByCondition(x => x.Status != nameof(EStatus.Delete))
            .CountAsync();

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);          
            var nhapVatLieuDtos = nhapVatLieus.Select(x => new NhapDto()
            {
                Id = x.Id,
                NameVatLieu = x.VatLieuForeignKey.NameVatLieu,
                Quantity = x.Quantity,
                Price = x.Price,
                TotalMoney = x.TotalMoney,
                Status = x.Status
            });
            return nhapVatLieuDtos.ToList();
        }
        private void LoadUserControl(UserControl userControl)
        {
            Content = userControl;
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
