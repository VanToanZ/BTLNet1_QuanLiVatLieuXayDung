using BTL_QuanLiVatLieuXayDung.QuanLiVatLieuMain;
using BTL_QuanLyVatLieuXayDung.Data.Common;
using BTL_QuanLyVatLieuXayDung.Data.Dto;
using BTL_QuanLyVatLieuXayDung.Data.Enum;
using BTL_QuanLyVatLieuXayDung.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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

namespace BTL_QuanLiVatLieuXayDung.QuanLiXuatVatLieuMain
{
    /// <summary>
    /// Interaction logic for QuanLiXuatVatLieuMainControl.xaml
    /// </summary>
    public partial class QuanLiXuatVatLieuMainControl : UserControl
    {
        private readonly IHoaDonRepository _hoaDonRepository;
        private readonly IVatLieuRepository _vatLieuRepository;
        private readonly IDetailHoaDonRepostiory _detailHoaDonRepostiory;
        private int currentPage = 1;   // Trang hiện tại
        private int pageSize = 3;      // Số lượng item mỗi trang
        private int totalRecords = 0;   // Tổng số bản ghi
        private int totalPages = 0;
        public QuanLiXuatVatLieuMainControl(
            IHoaDonRepository hoaDonRepository,
             IVatLieuRepository vatLieuRepository,
             IDetailHoaDonRepostiory detailHoaDonRepostiory
            )
        {
            InitializeComponent();
            _hoaDonRepository = hoaDonRepository;
            _vatLieuRepository = vatLieuRepository;
            _detailHoaDonRepostiory = detailHoaDonRepostiory;
        }

        private async void search_Click(object sender, RoutedEventArgs e)
        {
            var sql = _hoaDonRepository
               .FindByCondition(x => !x.Status.Equals(nameof(EStatus.Delete)));
            var codeHoaDon = searchCodeHoaDon.Text;
            if (codeHoaDon != null)
            {
                sql = sql.Where(x => x.CodeHoaDon.Equals(codeHoaDon));
            }

            var hoaDonDtos = await sql.Select(x => new HoaDonDto()
            {
                Id = x.Id,
                CodeHoaDon = x.CodeHoaDon,
                AddressNhan = x.AddressNhan,
                DescriptionHoaDon = x.DescriptionHoaDon,
                SoDienThoai = x.SoDienThoai,
                TotalMoney = x.TotalMoney,
            }).ToListAsync();
            dataHoaDon.ItemsSource = hoaDonDtos;
        }

        private async void HoaDon_Load(object sender, RoutedEventArgs e)
        {
            await LoadData();
        }
        private async Task LoadData()
        {
            dataHoaDon.ItemsSource = await GetNhapVatLieus();

        }
        private async Task<List<HoaDonDto>> GetNhapVatLieus()
        {
            // Tính toán dữ liệu cho trang hiện tại
            var skip = (currentPage - 1) * pageSize;
            var query = _hoaDonRepository
                .FindByCondition(x => x.Status != nameof(EStatus.Delete))
            .Skip(skip)
                .Take(pageSize);

            // Lấy dữ liệu từ cơ sở dữ liệu
            var hoaDons = await query.ToListAsync();

            // Tính tổng số bản ghi và số trang
            totalRecords = await _hoaDonRepository
                .FindByCondition(x => x.Status != nameof(EStatus.Delete))
            .CountAsync();

            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            var hoaDonDtos = hoaDons.Select(x => new HoaDonDto()
            {
                Id = x.Id,
                CodeHoaDon = x.CodeHoaDon,
                AddressNhan = x.AddressNhan,
                DescriptionHoaDon = x.DescriptionHoaDon,
                SoDienThoai = x.SoDienThoai,
                TotalMoney = x.TotalMoney,
            });
            return hoaDonDtos.ToList();
        }
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadUserControl(new QuanLiXuatVatLieuMainControl(
                _hoaDonRepository
                ,_vatLieuRepository,
                _detailHoaDonRepostiory));
        }
        private void LoadUserControl(UserControl userControl)
        {
            Content = userControl;
        }
        private async void Deatail_Click(object sender, RoutedEventArgs e)
        {
            if (dataHoaDon.SelectedItem is HoaDonDto selectedUser)
            {
                var hoaDon = await _hoaDonRepository.GetByIdAsync(selectedUser.Id);
                if (hoaDon != null)
                {
                    LoadUserControl(new DetailHoaDonControl(
                        hoaDon.Id,              
                       _vatLieuRepository,
                       _detailHoaDonRepostiory));
                }
            }
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
