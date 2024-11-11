using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Data.Models;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace BTL_QuanLiVatLieuXayDung.QuanLiHeThongMain
{
    public partial class QuanLiHeThongMainControl : UserControl
    {
        private readonly IConfigRepository _configRepository;

        private int currentPage = 1;   // Trang hiện tại
        private int pageSize = 3;      // Số lượng item mỗi trang
        private int totalRecords = 0;   // Tổng số bản ghi
        private int totalPages = 0;     // Tổng số trang

        public QuanLiHeThongMainControl(IConfigRepository configRepository)
        {
            InitializeComponent();
            _configRepository = configRepository;
        }

        private async void HeThong_Load(object sender, RoutedEventArgs e)
        {
            await LoadData();
            UpdatePaginationButtons();
        }

        private async Task LoadData()
        {
            var data = await GetData();
            dataHeThong.ItemsSource = data;
            UpdatePaginationButtons();
        }

        private async Task<List<Config>> GetData()
        {
            var skip = (currentPage - 1) * pageSize;
            var query = _configRepository
                .FindByCondition(x => x.Status != nameof(EStatus.Delete))
                .Skip(skip)
                .Take(pageSize);

            totalRecords = await _configRepository
                .FindByCondition(x => x.Status != nameof(EStatus.Delete))
                .CountAsync();  // Đếm tổng số bản ghi

            totalPages = (totalRecords + pageSize - 1) / pageSize; // Tính tổng số trang

            return await query.ToListAsync();
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

        private async void Sua_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in dataHeThong.SelectedItems)
            {
                if (item is Config row) // Assuming the row is of type Config
                {
                    _configRepository.Update(row);
                    await _configRepository.SaveDbSetAsync();
                }
            }
        }
    }
}
